insert into Migrations(MigrationVersion, MigrationName) values (26, '026_2013_06_13_fbs_check_db.sql')
go

/****** Object:  StoredProcedure [dbo].[RefreshCommonNationalExamCertificateFormPartition]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.RefreshCommonNationalExamCertificateFormPartition
-- ================================================
-- Обновить секционирование бланков сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 08.06.2008
-- v.1.1: Modified by Sedov Anton 20.06.2008
-- v.1.2: Modified by Sedov Anton 12.08.2008
-- Добавлено сохранение записей бланков в архивной таблице
-- Добавлено обновление секций предметов сертификатов
-- ================================================
alter procedure [dbo].[RefreshCommonNationalExamCertificateFormPartition]
as
begin
  declare
    @declareCommandText nvarchar(max)
    , @commandText nvarchar(max)
    , @clearCommandText nvarchar(max)
    , @deletePartition nvarchar(max)
    , @deleteSubjectPartition nvarchar(max)
    , @addSubjectPartition nvarchar(max)
  
  set @declareCommandText =
    N'
    declare
      @year int
      , @yearFrom int
      , @yearTo int
      , @partition bigint
      , @regionId int
    '

  set @deletePartition = 
    N'
    select
      @yearFrom = actuality.YearFrom
      , @yearTo = actuality.YearTo
    from dbo.GetCommonNationalExamCertificateActuality() actuality

    declare @actualYear table
      (
      [Year] int
      )

    set @year = @yearFrom
    while @year <= @yearTo
    begin
      insert into @actualYear
      values (@year)

      set @year = @year + 1
    end

    set @year = Year(GetDate())
  
    if not exists(select 1
        from dbo.CommonNationalExamCertificateFormActivePartition active_partition
          full outer join @actualYear actual_year 
            on active_partition.[Year] = actual_year.[Year]
        where 
          active_partition.[Year] is null
          or actual_year.[Year] is null)
      goto quitproc

    declare partition_cursor cursor forward_only for  
    select
      deprecated_partition.Partition [Partition]
    from 
      dbo.CommonNationalExamCertificateFormActivePartition deprecated_partition
    where 
      deprecated_partition.[Year] < @yearFrom
    order by 
      [Partition] asc

    open partition_cursor
    fetch next from partition_cursor into @partition
    while @@fetch_status <> -1
    begin
      if not object_id(''dbo.DeprecatedCommonNationalExamCertificateForm'') is null
        drop table dbo.DeprecatedCommonNationalExamCertificateForm
    
      create table dbo.DeprecatedCommonNationalExamCertificateForm 
        (
          Id bigint identity(1,1) not null
          , CreateDate datetime not null
          , UpdateDate datetime not null
          , UpdateId uniqueidentifier not null
          , [Year] int not null
          , RegionId int not null
          , [Partition] bigint not null
          , Number nvarchar(255) not null
          , CertificateNumber nvarchar(255)
          , LastName nvarchar(255)
          , FirstName nvarchar(255)
          , PatronymicName nvarchar(255)
          , PassportSeria nvarchar(255)
          , PassportNumber nvarchar(255)
          , IsBlank bit not null
          , IsDeny bit not null
          , IsDuplicate bit not null
          , IsValid bit
          , SourceLastName nvarchar(255)
          , SourceFirstName nvarchar(255)
          , SourcePatronymicName nvarchar(255)
          , SourcePassportSeria nvarchar(255)
          , SourcePassportNumber nvarchar(255)
          , IsCertificateExist bit
          , IsCertificateDeny bit
        ) 

      alter table dbo.CommonNationalExamCertificateForm
      switch partition $partition.CommonNationalExamCertificateFormPartitionFunction(@partition)  
      to dbo.DeprecatedCommonNationalExamCertificateForm
        
      alter partition function CommonNationalExamCertificateFormPartitionFunction()
      merge range(@partition)

      delete from dbo.CommonNationalExamCertificateFormActivePartition
      where [Partition] = @partition

      insert into dbo.CommonNationalExamCertificateFormArchive
      select
        cne_certificate_form.CreateDate
        , cne_certificate_form.UpdateDate
        , cne_certificate_form.UpdateId
        , cne_certificate_form.[Year]
        , cne_certificate_form.RegionId
        , cne_certificate_form.[Partition]
        , cne_certificate_form.Number
        , cne_certificate_form.CertificateNumber
        , cne_certificate_form.LastName
        , cne_certificate_form.FirstName
        , cne_certificate_form.PatronymicName
        , cne_certificate_form.PassportSeria
        , cne_certificate_form.PassportNumber
        , cne_certificate_form.IsBlank
        , cne_certificate_form.IsDeny
        , cne_certificate_form.IsDuplicate
        , cne_certificate_form.IsValid
        , cne_certificate_form.SourceLastName
        , cne_certificate_form.SourceFirstName
        , cne_certificate_form.SourcePatronymicName
        , cne_certificate_form.SourcePassportSeria
        , cne_certificate_form.SourcePassportNumber
        , cne_certificate_form.IsCertificateExist
        , cne_certificate_form.IsCertificateDeny
      from  
        dbo.DeprecatedCommonNationalExamCertificateForm cne_certificate_form

      drop table dbo.DeprecatedCommonNationalExamCertificateForm

      fetch next from partition_cursor into @partition
    end
    close partition_cursor
    deallocate partition_cursor

    delete from dbo.CommonNationalExamCertificateFormActivePartition
    where [Year] < @yearFrom
    '
  
  set @commandText = 
    N'
    if exists(select 1
      from dbo.CommonNationalExamCertificateFormActivePartition [partition]
      where [partition].[Year] = @year) goto quitproc 

    declare partition_cursor cursor forward_only for  
    select
      region.Id RegionId
      , dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
    from dbo.Region region
    where 
      region.InCertificate = 1
    order by 
      [Partition] asc

    open partition_cursor
    fetch next from partition_cursor into @regionId, @partition
    while @@fetch_status <> -1
    begin
      alter partition scheme CommonNationalExamCertificateFormPartitionScheme 
      next used [primary] 
      
      alter partition function CommonNationalExamCertificateFormPartitionFunction()
      split range (@partition)

      fetch next from partition_cursor into @regionId, @partition
    end
    close partition_cursor
    deallocate partition_cursor

    insert into dbo.CommonNationalExamCertificateFormActivePartition
      (
      [Year]
      , [RegionId]
      , [Partition]
      )
    select 
      @year
      , region.Id RegionId
      , dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
    from 
      dbo.Region region
    where 
      region.InCertificate = 1
    order by 
      [Partition] asc
    '

  set @deleteSubjectPartition =
    N'
    if not exists(select 1
        from dbo.CommonNationalExamCertificateSubjectFormActivePartition active_subject_partition
          full outer join @actualYear actual_year 
            on active_subject_partition.[Year] = actual_year.[Year]
        where 
          active_subject_partition.[Year] is null
          or actual_year.[Year] is null) goto quitproc

    declare partition_subject_cursor cursor forward_only for  
    select
      deprecated_subject_partition.Partition [Partition]
    from 
      dbo.CommonNationalExamCertificateSubjectFormActivePartition deprecated_subject_partition
    where 
      deprecated_subject_partition.[Year] < @yearFrom
    order by 
      [Partition] asc

    open partition_subject_cursor
    fetch next from partition_subject_cursor into @partition
    while @@fetch_status <> -1
    begin
      if not object_id(''dbo.DeprecatedCommonNationalExamCertificateSubjectForm'') is null
        drop table dbo.DeprecatedCommonNationalExamCertificateSubjectForm
    
      create table dbo.DeprecatedCommonNationalExamCertificateSubjectForm 
        (
          Id bigint identity(1,1) not null
          , [Year] int not null
          , RegionId int not null
          , [Partition] bigint not null
          , FormId bigint
          , SubjectId int
          , Mark numeric(5,1)
        ) 

      alter table dbo.CommonNationalExamCertificateSubjectForm
      switch partition $partition.CommonNationalExamCertificateSubjectFormPartitionFunction(@partition)  
      to dbo.DeprecatedCommonNationalExamCertificateSubjectForm
        
      alter partition function CommonNationalExamCertificateSubjectFormPartitionFunction()
      merge range(@partition)

      delete from dbo.CommonNationalExamCertificateSubjectFormActivePartition
      where [Partition] = @partition

      insert into dbo.CommonNationalExamCertificateSubjectFrom 
      select 
        deprecate_cne_certificate_subject_form.[Year]
        , deprecate_cne_certificate_subject_form.RegionId
        , deprecate_cne_certificate_subject_form.[Partition]
        , deprecate_cne_certificate_subject_form.FormId
        , deprecate_cne_certificate_subject_form.SubjectId
        , deprecate_cne_certificate_subject_form.Mark
      from 
        dbo.DeprecateCommonNationalExamCertificateSubjectForm deprecate_cne_certificate_subject_form

      drop table dbo.DeprecatedCommonNationalExamCertificateSubjectForm

      fetch next from partition_subject_cursor into @partition
    end
    close partition_subject_cursor
    deallocate partition_subject_cursor

    delete from dbo.CommonNationalExamCertificateSubjectFormActivePartition
    where [Year] < @yearFrom
    ' 
    
  set @addSubjectPartition =
    N'
    if exists(select 1
      from dbo.CommonNationalExamCertificateSubjectFormActivePartition [partition]
      where [partition].[Year] = @year) goto quitproc
    
    declare partition_subject_cursor cursor forward_only for  
    select
      region.Id RegionId
      , dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
    from dbo.Region region
    where 
      region.InCertificate = 1
    order by 
      [Partition] asc

    open partition_subject_cursor
    fetch next from partition_subject_cursor into @regionId, @partition
    while @@fetch_status <> -1
    begin
      alter partition scheme CommonNationalExamCertificateSubjectFormPartitionScheme 
      next used [primary] 
      
      alter partition function CommonNationalExamCertificateSubjectFormPartitionFunction()
      split range (@partition)

      fetch next from partition_subject_cursor into @regionId, @partition
    end
    close partition_subject_cursor
    deallocate partition_subject_cursor

    insert into dbo.CommonNationalExamCertificateSubjectFormActivePartition
      (
      [Year]
      , [RegionId]
      , [Partition]
      )
    select 
      @year
      , region.Id RegionId
      , dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
    from 
      dbo.Region region
    where 
      region.InCertificate = 1
    order by 
      [Partition] asc

    quitproc:
    '

  exec (@declareCommandText + @deletePartition + @commandText + @deleteSubjectPartition + 
    @addSubjectPartition)

  return 0  
end
GO
/****** Object:  StoredProcedure [dbo].[SearchProcessingCommonNationalExamCertificateRequestBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec SearchProcessingCommonNationalExamCertificateRequestBatch
-- ============================================================
-- Получение списока пакетов для запроса сертификатов ЕГЭ,
-- находящихся в обработке 
-- v.1.0: Created by Sedov Anton 28.05.2008
-- v.1.1: Modified by Fomin Dmitriy 28.05.2008
-- Добавлено ограничение по IsProcess.
-- v.1.2: Modified by Fomin Dmitriy 28.05.2008
-- Убрано поле Batch - парсинг будет на уровне задачи.
-- v.1.3: Modified By Sedov Anton 05.06.2008
-- Добавлен выбор базы данных, на  которой 
-- будет выполняться процедура
-- ============================================================
alter procedure [dbo].[SearchProcessingCommonNationalExamCertificateRequestBatch]
as
begin
  declare 
    @chooseDbText nvarchar(4000)
    , @commandText nvarchar(4000)
    
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
  set @commandText = 
    N'
    select 
      exam_certificate_request_batch.Id Id
      , exam_certificate_request_batch.Executing Executing 
    from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(nolock)
    where 
      exam_certificate_request_batch.IsProcess = 1
      and exam_certificate_request_batch.Executing is null '

  set @commandText = @chooseDbText + @commandText

  exec sp_executesql @commandText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchProcessingCommonNationalExamCertificateCheckBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec SearchProcessingCommonNationalExamCertificateCheckBatch
-- ============================================================
-- Получение списока пакетов для проверки сертификатов ЕГЭ
-- , находящихся в обработке
-- v.1.0: Created by Sedov Anton 28.05.2008
-- v.1.1: Modified by Fomin Dmitriy 28.05.2008
-- Добавлено ограничение по IsProcess.
-- v.1.2: Modified by Fomin Dmitriy 28.05.2008
-- Убрано поле Batch - парсинг будет на уровне задачи.
-- ============================================================
alter procedure [dbo].[SearchProcessingCommonNationalExamCertificateCheckBatch]
as
begin
  declare 
    @chooseDbText nvarchar(4000)
    , @commandText nvarchar(4000)

  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
  set @commandText = 
    N'
    select 
      exam_certificate_check_batch.Id Id
      , exam_certificate_check_batch.Executing Executing
    from dbo.CommonNationalExamCertificateCheckBatch exam_certificate_check_batch with(nolock)
    where 
      exam_certificate_check_batch.IsProcess = 1
      and exam_certificate_check_batch.Executing is null '

  set @commandText = @chooseDbText + @commandText
  exec sp_executesql @commandText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[ImportCommonNationalExamCertificateDeny]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.ImportCommonNationalExamCertificateDeny
-- ================================================
-- Загрузка отмен сертификатов ЕГЭ одной секции
-- v.1.0: Create by Sedov Anton 11.06.2008
-- ================================================
alter procedure [dbo].[ImportCommonNationalExamCertificateDeny]
  @importFilePath nvarchar(255) 
as
begin
  declare 
    @year int
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(4000)
    , @partition int 
  
  set @year = Year(GetDate())

  select
    @yearFrom = actuality.YearFrom
    , @yearTo = actuality.YearTo
  from dbo.GetCommonNationalExamCertificateActuality() actuality

  -- Удаление устаревших секций запрещений сертификатов.
  if exists(select 1
      from dbo.CommonNationalExamCertificateDenyActivePartition active_partition
      where active_partition.[Year] < @yearFrom)
  begin
    declare partition_cursor cursor forward_only for 
    select
      deprecated_partition.[Year] [Partition]
    from 
      dbo.CommonNationalExamCertificateDenyActivePartition deprecated_partition
    where 
      deprecated_partition.[Year] < @yearFrom
    order by 
      [Partition] asc
    
    open partition_cursor
    fetch next from partition_cursor into @partition
    while @@fetch_status <> -1
    begin 
      if not object_id('dbo.DeprecatedCommonNationalExamCertificateDeny') is null
        drop table dbo.DeprecatedCommonNationalExamCertificateDeny

      create table dbo.DeprecatedCommonNationalExamCertificateDeny
        (
        Id bigint not null
        , CreateDate datetime not null
        , UpdateDate datetime not null
        , UpdateId  uniqueidentifier
        , [Year] int not null
        , CertificateNumber nvarchar(255) not null
        , Comment ntext not null
        , NewCertificateNumber nvarchar(255)
        )

      alter table dbo.DeprecatedCommonNationalExamCertificateDeny
      add constraint DeprecatedCertificateDenyPK
        primary key clustered ([Year], CertificateNumber)

      alter table dbo.CommonNationalExamCertificateDeny
      switch partition $partition.CommonNationalExamCertificateDenyLeftPartition(@partition) 
        to dbo.DeprecatedCommonNationalExamCertificateDeny

      insert into dbo.CommonNationalExamCertificateDenyArchive
      select
        cne_certificate_deny.CreateDate
        , cne_certificate_deny.UpdateDate
        , cne_certificate_deny.UpdateId
        , cne_certificate_deny.[Year]
        , cne_certificate_deny.CertificateNumber
        , cne_certificate_deny.Comment
        , cne_certificate_deny.NewCertificateNumber
      from dbo.DeprecatedCommonNationalExamCertificateDeny cne_certificate_deny
      
      drop table dbo.DeprecatedCommonNationalExamCertificateDeny

      alter partition scheme CommonNationalExamCertificateDenyPartitionScheme
      next used [primary]
      
      alter partition function CommonNationalExamCertificateDenyLeftPartition()
      merge range(@partition)

      fetch next from partition_cursor into @partition
    end
    close partition_cursor
    deallocate partition_cursor

    delete from dbo.CommonNationalExamCertificateDenyActivePartition
    where [Year] < @yearFrom
  end

  -- Добавление новых секций запрещений сертификатов.
  if not exists(select 1
      from dbo.CommonNationalExamCertificateDenyActivePartition [partition]
      where [partition].[Year] = @year)
  begin
    alter partition scheme CommonNationalExamCertificateDenyPartitionScheme
    next used [primary]
      
    alter partition function CommonNationalExamCertificateDenyLeftPartition()
    split range (@year)

    insert into dbo.CommonNationalExamCertificateDenyActivePartition
      (
      [Year]
      )
    values 
      (
      @year
      )
  end  
    
  -- Импорт данных запрещений сертификатов.
  if not object_id('dbo.ImportingCommonNationalExamCertificateDeny') is null
    drop table dbo.ImportingCommonNationalExamCertificateDeny

  create table dbo.ImportingCommonNationalExamCertificateDeny
    (
    Id bigint not null
    , CreateDate datetime not null
    , UpdateDate datetime not null
    , UpdateId  uniqueidentifier
    , [Year] int not null
    , CertificateNumber nvarchar(255) not null
    , Comment ntext not null
    , NewCertificateNumber nvarchar(255)
    )

  set @commandText = 
    N'bulk insert dbo.ImportingCommonNationalExamCertificateDeny
    from ''<importFile>'' with (codepage = 1251) ' 

  set @commandText = replace(@commandText, '<importFile>', 
    replace(@importFilePath, '''', '''''')) 

  exec(@commandText) 
  
  set @commandText = replace(
    'alter table dbo.ImportingCommonNationalExamCertificateDeny
    with check
    add constraint ImportingCertificateDenyCK
      check ([Year] = <year>) ', '<year>', @year)

  exec(@commandText) 

  alter table dbo.ImportingCommonNationalExamCertificateDeny
  add constraint ImportingCertificateDenyPK
    primary key clustered ([Year], CertificateNumber)

  if not object_id('dbo.DeprecatedCommonNationalExamCertificateDeny') is null
    drop table dbo.DeprecatedCommonNationalExamCertificateDeny

  create table dbo.DeprecatedCommonNationalExamCertificateDeny
    (
    Id bigint not null
    , CreateDate datetime not null
    , UpdateDate datetime not null
    , UpdateId  uniqueidentifier
    , [Year] int not null
    , CertificateNumber nvarchar(255) not null
    , Comment ntext not null
    , NewCertificateNumber nvarchar(255)
    )

  alter table dbo.DeprecatedCommonNationalExamCertificateDeny
  add constraint DeprecatedCertificatePK
    primary key clustered ([Year], CertificateNumber)

  -- Перенос данных в таблицы.
  alter table dbo.CommonNationalExamCertificateDeny
  switch partition $partition.CommonNationalExamCertificateDenyLeftPartition(@year) 
    to dbo.DeprecatedCommonNationalExamCertificateDeny

  alter table dbo.ImportingCommonNationalExamCertificateDeny
  switch to dbo.CommonNationalExamCertificateDeny
    partition $partition.CommonNationalExamCertificateDenyLeftPartition(@year) 

  drop table dbo.DeprecatedCommonNationalExamCertificateDeny
  drop table dbo.ImportingCommonNationalExamCertificateDeny

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[ImportCommonNationalExamCertificate]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.ImportCommonNationalExamCertificate
-- =============================================
-- Загрузка сертификатов ЕГЭ одной секции
-- v.1.0: Created by Sedov Anton 11.06.2008
-- v.1.1: Modified by Fomin Dmitriy 12.06.2008
-- Исправление ошибок. Объединение ХПшек в одну.
-- ============================================= 
alter procedure [dbo].[ImportCommonNationalExamCertificate]
  @importCertificateFilePath nvarchar(255) 
  , @importCertificateSubjectFilePath nvarchar(255) 
as
begin
  -- Подготовка секций.
  declare 
    @year int
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(4000)
    , @partition int

  set @year = Year(GetDate())

  select
    @yearFrom = actuality.YearFrom
    , @yearTo = actuality.YearTo
  from dbo.GetCommonNationalExamCertificateActuality() actuality 

  -- Удаление устаревших секций сертификатов.
  if exists(select 1
      from dbo.CommonNationalExamCertificateActivePartition active_partition
      where active_partition.[Year] < @yearFrom)
  begin
    declare partition_cursor cursor forward_only for 
    select
      deprecated_partition.[Year] [Partition]
    from 
      dbo.CommonNationalExamCertificateActivePartition deprecated_partition
    where 
      deprecated_partition.[Year] < @yearFrom
    order by 
      [Partition] asc
    
    open partition_cursor
    fetch next from partition_cursor into @partition
    while @@fetch_status <> -1
    begin 
      if not object_id('dbo.DeprecatedCommonNationalExamCertificate') is null
        drop table dbo.DeprecatedCommonNationalExamCertificate

      create table dbo.DeprecatedCommonNationalExamCertificate
        (
        Id bigint not null
        , CreateDate datetime not null
        , UpdateDate datetime not null
        , UpdateId uniqueidentifier not null
        , EditorAccountId bigint not null
        , EditorIp nvarchar(255) not null
        , Number nvarchar(255) not null
        , EducationInstitutionCode nvarchar(255) not null
        , [Year] int not null 
        , LastName nvarchar(255) not null
        , FirstName nvarchar(255)  not null
        , PatronymicName nvarchar(255) not null
        , Sex bit not null
        , Class nvarchar(255) not null
        , InternalPassportSeria nvarchar(255) not null
        , PassportSeria nvarchar(255) not null
        , PassportNumber nvarchar(255) not null
        , EntrantNumber nvarchar(255) null
        , RegionId int not null 
        )

      alter table dbo.DeprecatedCommonNationalExamCertificate
      add constraint DeprecatedCertificatePK
        primary key clustered ([Year], Id)

      alter table dbo.CommonNationalExamCertificate
      switch partition $partition.CommonNationalExamCertificateLeftPartition(@partition) 
        to dbo.DeprecatedCommonNationalExamCertificate

      insert into dbo.CommonNationalExamCertificateArchive
      select
        deprecate_cne_certificate.CreateDate
        , deprecate_cne_certificate.UpdateDate
        , deprecate_cne_certificate.UpdateId 
        , deprecate_cne_certificate.EditorAccountId
        , deprecate_cne_certificate.EditorIp
        , deprecate_cne_certificate.Number
        , deprecate_cne_certificate.EducationInstitutionCode
        , deprecate_cne_certificate.[Year]
        , deprecate_cne_certificate.LastName
        , deprecate_cne_certificate.FirstName
        , deprecate_cne_certificate.PatronymicName
        , deprecate_cne_certificate.Sex
        , deprecate_cne_certificate.Class
        , deprecate_cne_certificate.InternalPassportSeria
        , deprecate_cne_certificate.PassportSeria
        , deprecate_cne_certificate.PassportNumber
        , deprecate_cne_certificate.EntrantNumber
        , deprecate_cne_certificate.RegionId
      from 
        dbo.DeprecatedCommonNationalExamCertificate deprecate_cne_certificate
      drop table dbo.DeprecatedCommonNationalExamCertificate

      alter partition scheme CommonNationalExamCertificatePartitionScheme
      next used [primary]
      
      alter partition function CommonNationalExamCertificateLeftPartition()
      merge range(@partition)

      fetch next from partition_cursor into @partition
    end
    close partition_cursor
    deallocate partition_cursor

    delete from dbo.CommonNationalExamCertificateActivePartition
    where [Year] < @yearFrom
  end

  -- Удаление устаревших секций предметов сертификатов.
  if exists(select 1
      from dbo.CommonNationalExamCertificateSubjectActivePartition active_partition
      where active_partition.[Year] < @yearFrom)
  begin
    declare partition_cursor cursor forward_only for 
    select
      deprecated_partition.[Year] [Partition]
    from 
      dbo.CommonNationalExamCertificateSubjectActivePartition deprecated_partition
    where 
      deprecated_partition.[Year] < @yearFrom
    order by 
      [Partition] asc
    
    open partition_cursor
    fetch next from partition_cursor into @partition
    while @@fetch_status <> -1
    begin 
      if not object_id('dbo.DeprecatedCommonNationalExamCertificateSubject') is null
        drop table dbo.DeprecatedCommonNationalExamCertificateSubject

      create table dbo.DeprecatedCommonNationalExamCertificateSubject
        (
        Id bigint not null
        , CertificateId bigint not null
        , SubjectId  bigint not null
        , Mark numeric(5,1) not null
        , HasAppeal bit not null
        , [Year] int not null
        , RegionId int not null
        )

      alter table dbo.DeprecatedCommonNationalExamCertificateSubject
      add constraint DeprecatedCertificateSubjectPK
        primary key clustered ([Year], CertificateId, SubjectId)

      alter table dbo.CommonNationalExamCertificateSubject
      switch partition $partition.CommonNationalExamCertificateSubjectLeftPartition(@partition) 
        to dbo.DeprecatedCommonNationalExamCertificateSubject


      insert into dbo.CommonNationalExamCertificateSubjectArchive
      select 
        deprecate_cne_certificate_subject.CertificateId
        , deprecate_cne_certificate_subject.SubjectId
        , deprecate_cne_certificate_subject.Mark
        , deprecate_cne_certificate_subject.HasAppeal
        , deprecate_cne_certificate_subject.[Year]
        , deprecate_cne_certificate_subject.RegionId
      from 
        dbo.DeprecatedCommonNationalExamCertificateSubject deprecate_cne_certificate_subject

      drop table dbo.DeprecatedCommonNationalExamCertificateSubject

      alter partition scheme CommonNationalExamCertificateSubjectPartitionScheme
      next used [primary]
      
      alter partition function CommonNationalExamCertificateSubjectLeftPartition()
      merge range(@partition)

      fetch next from partition_cursor into @partition
    end
    close partition_cursor
    deallocate partition_cursor

    delete from dbo.CommonNationalExamCertificateSubjectActivePartition
    where [Year] < @yearFrom
  end

  -- Добавление новых секций сертификатов.
  if not exists(select 1
      from dbo.CommonNationalExamCertificateActivePartition [partition]
      where [partition].[Year] = @year)
  begin
    alter partition scheme CommonNationalExamCertificatePartitionScheme
    next used [primary]
      
    alter partition function CommonNationalExamCertificateLeftPartition()
    split range (@year)

    insert into dbo.CommonNationalExamCertificateActivePartition
      (
      [Year]
      )
    values 
      (
      @year
      )
  end  
  
  -- Добавление новых секций предметов сертификатов.
  if not exists(select 1
      from dbo.CommonNationalExamCertificateSubjectActivePartition [partition]
      where [partition].[Year] = @year)
  begin
    alter partition scheme CommonNationalExamCertificateSubjectPartitionScheme
    next used [primary]
      
    alter partition function CommonNationalExamCertificateSubjectLeftPartition()
    split range (@year)

    insert into dbo.CommonNationalExamCertificateSubjectActivePartition
      (
      [Year]
      )
    values 
      (
      @year
      )
  end  
    
  -- Импорт данных сертификатов.
  if not object_id('dbo.ImportingCommonNationalExamCertificate') is null
    drop table dbo.ImportingCommonNationalExamCertificate

  create table dbo.ImportingCommonNationalExamCertificate
    (
    Id bigint not null
    , CreateDate datetime not null
    , UpdateDate datetime not null
    , UpdateId uniqueidentifier not null
    , EditorAccountId bigint not null
    , EditorIp nvarchar(255) not null
    , Number nvarchar(255) not null
    , EducationInstitutionCode nvarchar(255) not null
    , [Year] int not null 
    , LastName nvarchar(255) not null
    , FirstName nvarchar(255)  not null
    , PatronymicName nvarchar(255) not null
    , Sex bit not null
    , Class nvarchar(255) not null
    , InternalPassportSeria nvarchar(255) not null
    , PassportSeria nvarchar(255) not null
    , PassportNumber nvarchar(255) not null
    , EntrantNumber nvarchar(255) null
    , RegionId int not null 
    )

  set @commandText = 
    N'bulk insert dbo.ImportingCommonNationalExamCertificate
    from ''<importFile>'' with (codepage = 1251) ' 

  set @commandText = replace(@commandText, '<importFile>', 
    replace(@importCertificateFilePath, '''', '''''')) 

  exec(@commandText) 
  
  set @commandText = replace(
    'alter table dbo.ImportingCommonNationalExamCertificate
    with check
    add constraint ImportingCertificateCK
      check ([Year] = <year>) ', '<year>', @year)

  exec(@commandText) 

  alter table dbo.ImportingCommonNationalExamCertificate
  add constraint ImportingCertificatePK
    primary key clustered ([Year], Id)

  create nonclustered index IdxImportingCertificateNumber
  on dbo.ImportingCommonNationalExamCertificate ([Year], Number)

  create nonclustered index IdxImportingCertificateOwner
  on dbo.ImportingCommonNationalExamCertificate ([Year], LastName, FirstName, PatronymicName)

  if not object_id('dbo.DeprecatedCommonNationalExamCertificate') is null
    drop table dbo.DeprecatedCommonNationalExamCertificate

  create table dbo.DeprecatedCommonNationalExamCertificate
    (
    Id bigint not null
    , CreateDate datetime not null
    , UpdateDate datetime not null
    , UpdateId uniqueidentifier not null
    , EditorAccountId bigint not null
    , EditorIp nvarchar(255) not null
    , Number nvarchar(255) not null
    , EducationInstitutionCode nvarchar(255) not null
    , [Year] int not null 
    , LastName nvarchar(255) not null
    , FirstName nvarchar(255)  not null
    , PatronymicName nvarchar(255) not null
    , Sex bit not null
    , Class nvarchar(255) not null
    , InternalPassportSeria nvarchar(255) not null
    , PassportSeria nvarchar(255) not null
    , PassportNumber nvarchar(255) not null
    , EntrantNumber nvarchar(255) null
    , RegionId int not null 
    )

  alter table dbo.DeprecatedCommonNationalExamCertificate
  add constraint DeprecatedCertificatePK
    primary key clustered ([Year], Id)

  -- Импорт данных предметов сертификатов.
  if not object_id('dbo.ImportingCommonNationalExamCertificateSubject') is null
    drop table dbo.ImportingCommonNationalExamCertificateSubject

  create table dbo.ImportingCommonNationalExamCertificateSubject
    (
    Id bigint not null
    , CertificateId bigint not null
    , SubjectId  bigint not null
    , Mark numeric(5,1) not null
    , HasAppeal bit not null
    , [Year] int not null
    , RegionId int not null
    )

  set @commandText = 
    N'bulk insert dbo.ImportingCommonNationalExamCertificateSubject
    from ''<importFile>'' with (codepage = 1251) ' 

  set @commandText = replace(@commandText, '<importFile>', 
    replace(@importCertificateSubjectFilePath, '''', '''''')) 

  exec(@commandText) 

  set @commandText = replace(
    'alter table dbo.ImportingCommonNationalExamCertificateSubject
    with check
    add constraint ImportingCertificateSubjectCK
      check ([Year] = <year>) ', '<year>', @year)

  exec(@commandText) 

  alter table dbo.ImportingCommonNationalExamCertificateSubject
  add constraint ImportingCertificateSubjectPK
    primary key clustered ([Year], CertificateId, SubjectId)

  if not object_id('dbo.DeprecatedCommonNationalExamCertificateSubject') is null
    drop table dbo.DeprecatedCommonNationalExamCertificateSubject

  create table dbo.DeprecatedCommonNationalExamCertificateSubject
    (
    Id bigint not null
    , CertificateId bigint not null
    , SubjectId  bigint not null
    , Mark numeric(5,1) not null
    , HasAppeal bit not null
    , [Year] int not null
    , RegionId int not null
    )

  alter table dbo.DeprecatedCommonNationalExamCertificateSubject
  add constraint DeprecatedCertificateSubjectPK
    primary key clustered ([Year], CertificateId, SubjectId)

  -- Перенос данных в таблицы.
  alter table dbo.CommonNationalExamCertificate
  switch partition $partition.CommonNationalExamCertificateLeftPartition(@year) 
    to dbo.DeprecatedCommonNationalExamCertificate

  alter table dbo.CommonNationalExamCertificateSubject
  switch partition $partition.CommonNationalExamCertificateSubjectLeftPartition(@year) 
    to dbo.DeprecatedCommonNationalExamCertificateSubject

  alter table dbo.ImportingCommonNationalExamCertificate
  switch to dbo.CommonNationalExamCertificate
    partition $partition.CommonNationalExamCertificateLeftPartition(@year) 

  alter table dbo.ImportingCommonNationalExamCertificateSubject
  switch to dbo.CommonNationalExamCertificateSubject
    partition $partition.CommonNationalExamCertificateSubjectLeftPartition(@year) 

  -- Очистка.
  drop table dbo.DeprecatedCommonNationalExamCertificate
  drop table dbo.ImportingCommonNationalExamCertificate
  drop table dbo.DeprecatedCommonNationalExamCertificateSubject
  drop table dbo.ImportingCommonNationalExamCertificateSubject

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateCheckBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch
-- ================================================================
-- Изменить поле Executing в пакете проверки для сертификатов
-- v.1.0: Created by Sedov A.G. 28.05.2008 
-- ================================================================
alter procedure [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
  @id bigint
  , @executing bit
as
begin
  declare 
    @chooseDbText nvarchar(4000)
    , @commandText nvarchar(4000)
  
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
  set @commandText = 
    N'
    update exam_certificate_check_batch
    set exam_certificate_check_batch.Executing = @executing
    from dbo.CommonNationalExamCertificateCheckBatch exam_certificate_check_batch with(rowlock)
    where exam_certificate_check_batch.Id = @id '
  
  set @commandText = @chooseDbText + @commandText


  exec sp_executesql @commandText
    , N'@id bigint, @executing bit'
    , @id 
    , @executing 
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateRequestBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateCommonNationalExamCertificateRequestBatch
-- ================================================================
-- Изменить поле Executing в пакете проверки для запросов
-- v.1.0: Created by Sedov A.G. 28.05.2008 
-- v.1.1: Modified by Sedov Anton 05.06.2008 
-- Добавлен динамический выбор БД,
-- на которой будет выполнена процедура
-- ================================================================
alter procedure [dbo].[UpdateCommonNationalExamCertificateRequestBatch]
  @id bigint
  , @executing bit
as
begin
  declare 
    @chooseDbText nvarchar(4000)
    , @commandText nvarchar(4000)

  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
  set @commandText = 
    N'
    update exam_certificate_request_batch
    set exam_certificate_request_batch.Executing = @executing
    from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(rowlock)
    where exam_certificate_request_batch.Id = @id '
  
  set @commandText = @chooseDbText + @commandText

  exec sp_executesql @commandText
    , N'@id bigint, @executing bit'
    , @id
    , @executing

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetCommonNationalExamCertificateRequestBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- dbo.GetCommonNationalExamCertificateRequestBatch
-- ==============================================
-- Вывести запись из таблицы  пакетов запросов 
-- по ИД пакета
-- v.1.0: Created by Sedov Anton 28.05.2008
-- ===============================================
alter procedure [dbo].[GetCommonNationalExamCertificateRequestBatch]
  @id bigint 
as
begin
  declare 
    @chooseDbText nvarchar(4000)
    , @commandText nvarchar(4000)
  
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
  set @commandText = 
    N'
    select 
      exam_certificate_request_batch.Id Id
      , exam_certificate_request_batch.Executing Executing
      , exam_certificate_request_batch.Batch Batch
    from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(nolock)
    where exam_certificate_request_batch.Id = @id
    '

  set @commandText = @chooseDbText + @commandText 

  exec sp_executesql @commandText
    , N'@id bigint'
    , @id
  
  return 0 
end
GO
/****** Object:  StoredProcedure [dbo].[GetCommonNationalExamCertificateCheckBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- dbo.GetCommonNationalExamCertificateCheckBatch
-- ==============================================
-- Вывести запись из таблицы  пакетов сертификатов 
-- по ИД пакета
-- v.1.0: Created by Sedov Anton 28.05.2008
-- v.1.1: Modified by Sedov Anton 05.06.2008
-- ===============================================
alter procedure [dbo].[GetCommonNationalExamCertificateCheckBatch]
  @id bigint
as
begin
  declare 
    @chooseDbText nvarchar(4000)
    , @commandText nvarchar(4000)

  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName());
  
  set @commandText = 
    N' 
    select 
      exam_certificate_check_batch.Id Id
      , exam_certificate_check_batch.Executing Executing
      , exam_certificate_check_batch.Batch Batch
    from dbo.CommonNationalExamCertificateCheckBatch exam_certificate_check_batch with(nolock) 
    where exam_certificate_check_batch.Id = @id
    '

  set @commandText = @chooseDbText + @commandText 

  exec sp_executesql @commandText
    , N'@id bigint'
    , @id

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateForm]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckCommonNationalExamCertificateForm
-- ================================================
-- Проверить бланки свидетельств ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 08.06.2008
-- v.1.1: Modified by Fomin Dmitriy 16.06.2008
-- Проверки проводятся в момент сохранения формы.
-- ================================================
alter procedure [dbo].[CheckCommonNationalExamCertificateForm]
  @regionCode nvarchar(50)
as
begin
  declare
    @commandText nvarchar(4000)
    , @declareCommandText nvarchar(4000)
    , @chooseDbText nvarchar(4000)

  set @chooseDbText = ''
  set @declareCommandText = ''

  set @chooseDbText = 'use <databaseName> '
  set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())  

  set @declareCommandText = 
    'declare
      @regionId int
      , @year int
      , @partition bigint '

  set @commandText = replace(
    '
    select @regionId = region.Id 
    from dbo.Region region with (nolock)
    where region.Code = ''<region_code>''

    set @year = Year(GetDate())
    set @partition = dbo.GetCommonNationalExamCertificateFormPartition(@regionId, @year)

    select
      form.Number
      , form.CertificateNumber
      , form.IsValid
      , form.IsCertificateExist
      , form.IsCertificateDeny
    from dbo.CommonNationalExamCertificateForm form
    where form.Partition = @partition
      and form.IsDeny = 0 ', '<region_code>', replace(@regionCode, '''', ''''''))

  exec (@chooseDbText + @declareCommandText + @commandText)

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateRequest]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec ExecuteCommonNationalExamCertificateRequest
-- ==================================================
-- Запрос сертификатов ЕГЭ
-- v.1.0: Created by Sedov A.G. 26.05.2008
-- v.1.1: Modified by Fomin Dmitriy 31.05.2008
-- Исправление ошибок обработки. 
-- Добавлены поля IsDeny, DenyComment.
-- v.1.2: Modified by Sedov Anton 02.06.2008
-- В результате учитываются аннулированые сертификаты.
-- v.1.3: Modified by Fomin Dmitriy 02.06.2008
-- Работа над ошибками.
-- v.1.4: Modified by Sedov Anton 05.06.2008
-- Выполняется выбор базы данных, на которой
-- выполнится  процедура.
-- ==================================================
alter procedure [dbo].[ExecuteCommonNationalExamCertificateRequest]
  @batchId bigint
as
begin
  declare
    @commandText nvarchar(4000)
    , @chooseDbText nvarchar (4000)

  set @chooseDbText = ''

  set @chooseDbText = 'use <databaseName> '
  set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())  
  
  set @commandText = 
    N'
    update exam_certificate_request_batch
    set
      UpdateDate = GetDate()
      , IsProcess = 0
      , IsCorrect = case 
        when not exists(select 1
            from dbo.CommonNationalExamCertificateRequest certificate_request with(nolock)
            where certificate_request.BatchId = exam_certificate_request_batch.Id 
              and certificate_request.IsCorrect = 0) then 1
        else 0
      end
    from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(rowlock)
    where exam_certificate_request_batch.Id = <BatchIdentifier> '

  set @commandText = replace(@commandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))

  exec (@chooseDbText + @commandText) 

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateCheck]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.ExecuteCommonNationalExamCertificateCheck
-- =======================================================
-- Проверка сертификатов  ЕГЭ.
-- v.1.0: Created by Sedov Anton 23.05.2008
-- v.1.1: Modified by Sedov Anton 27.05.2008
-- Добавлено ограничение сертификатов по годам
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Убрана колонка HasAppeal. 
-- Исправлена логика работы.
-- Добавлены колонки IsDeny, DenyComment.
-- v.1.3: Modified by Sedov Anton 02.06.2008
-- Оптимизирована работа процедуры
-- v.1.4: Modified by Sedov Anton 05.06.2008
-- Выполняется выбор базы данных, на которой
-- выполнится  процедура. 
-- =======================================================
alter procedure [dbo].[ExecuteCommonNationalExamCertificateCheck]
  @batchId bigint
as
BEGIN

  declare
    @commandText nvarchar(4000)
    , @chooseDbText nvarchar (4000)

  set @chooseDbText = ''

  set @chooseDbText = 'use <databaseName> '
  set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())  

  set @commandText =  
    N'    
    update certificate_check_batch
    set
      IsProcess = 0
      , IsCorrect = case 
        when not exists(select 1 
          from dbo.CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
          where exam_certificate_check.BatchId = certificate_check_batch.Id
            and exam_certificate_check.IsCorrect = 0) then 1
        else 0
      end
      , UpdateDate = GetDate()
    from dbo.CommonNationalExamCertificateCheckBatch certificate_check_batch with(rowlock)
    where certificate_check_batch.Id = <BatchIdentifier> '
  
  set @commandText = replace(@commandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))

  exec (@chooseDbText + @commandText)
  
  return 0

end
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateRequest]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
   Для работы  процедуры требуются следующая 
   временная таблица:
   create table #CommonNationalExamCertificateRequest
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , Index bigint
    )
*/

-- exec dbo.PrepareCommonNationalExamCertificateRequest
-- ====================================================
-- Подготовка пакетов для проверки сертификатов ЕГЭ
-- v.1.0: Created by Sedov A.G. 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 31.05.2008
-- Добавлено удаление старых данных.
-- v.1.2: Modified by Sedov Anton 18.06.2008
-- Добавлен выбор запросов  для проверки
-- v.1.3: Modified by Sedov Anton 18.06.2008
-- Добавлено поле IsExtended
-- v.1.4: Modified by Sedov Anton 19.06.2008
-- Оптимизирована работа процедуры
-- v.1.5: Modified by Fomin Dmitriy 19.06.2008
-- Исправление дефектов.
-- v.1.6: Modified by Fomin Dmitriy 21.06.2008
-- Серия паспорта сравнивается с приведением к корректному виду.
-- v.1.7: Modified by Sedov Anton 04.07.2008
-- Добавлено поле NewCertificateNumber для аннулированных
-- сертификатов
-- v.1.8: Modified by Sedov Anton 09.07.2008
-- Исправлена  логика  динамического выбора БД
-- таблицы которой используются  для получения
-- данных о сертификатах
-- v.1.9: Modified by Sedov Anton 28.07.2008
-- Добавлен параметр Index во временную таблицу 
-- проверки запросов
-- v.1.10: Modified by Fomin Dmitriy 29.07.2008
-- Изменен порядок сортировки: сначала аннулированные,
-- затем актуальные. Добавлена сортировка по номеру.
-- v.1.11: Modified by Valeev Denis 03.06.2009
-- Добавлена проверка по типографскому номеру
-- ====================================================
alter procedure [dbo].[PrepareCommonNationalExamCertificateRequest]
  @batchId bigint
as
begin
  
  declare 
    @chooseDbText nvarchar(max)
    , @declareCommandText nvarchar(max)
    , @executeCommandText nvarchar(max)
    , @baseName nvarchar(255)
    , @IndexText nvarchar(max)
    , @CUID nvarchar(1000)
    
  update #CommonNationalExamCertificateRequest set PassportSeria=replace(PassportSeria, ' ', '')
    
  set @CUID = cast(NEWID() as nvarchar(1000))
  set @IndexText = '      
    CREATE NONCLUSTERED INDEX [IX_CNECR_LastName'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [LastName] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
      
    CREATE NONCLUSTERED INDEX [IX_CNECR_FirstName'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [FirstName] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
          
    CREATE NONCLUSTERED INDEX [IX_CNECR_PatronymicName'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [PatronymicName] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
        
    CREATE NONCLUSTERED INDEX [IX_CNECR_PassportSeria'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [PassportSeria] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    
    CREATE NONCLUSTERED INDEX [IX_CNECR_PassportNumber'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [PassportNumber] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    
    CREATE NONCLUSTERED INDEX [IX_CNECR_Index'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [Index] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    
    '


  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1) 

  set @declareCommandText =
    N'
    declare
      @yearFrom int
      , @yearTo int
      , @IsTypographicNumber bit
      , @year int

    select @IsTypographicNumber = cnecrb.IsTypographicNumber, @year = cnecrb.year
    from [CommonNationalExamCertificateRequestBatch] as cnecrb
    where cnecrb.id = <BatchIdentifier>

    if @year is not null
      select @yearFrom = @year, @yearTo = @year
    else
      select @yearFrom = Year(GetDate()) - 5, @yearTo = Year(GetDate())
        
    '

  set @executeCommandText = 
    N'
    delete exam_certificate_request
    from dbo.CommonNationalExamCertificateRequest exam_certificate_request
    where exam_certificate_request.BatchId = <BatchIdentifier>
    
    declare @ncount int
    
    
    insert dbo.CommonNationalExamCertificateRequest
    (
    BatchId
    , LastName
    , FirstName
    , PatronymicName
    , PassportSeria
    , PassportNumber
    , IsCorrect
    , SourceCertificateIdGuid
    , SourceCertificateYear
    , SourceCertificateNumber
    , SourceRegionId
    , IsDeny
    , DenyComment
    , DenyNewCertificateNumber
    , TypographicNumber
    , ParticipantID
    )
    select 
      <BatchIdentifier>
      , exam_certificate_request.LastName
      , exam_certificate_request.FirstName
      , exam_certificate_request.PatronymicName
      , null/*isnull(exam_certificate_request.PassportSeria, exam_certificate.PassportSeria)*/
      , isnull(exam_certificate_request.PassportNumber, exam_certificate.PassportNumber)
      , case 
        when (exam_certificate.Id is null and exam_certificate.ParticipantID is not null) 
              and exam_certificate_deny.Year is null then 1
        else 0
      end
      , isnull(cast(exam_certificate.Id as nvarchar(500)),''Нет свидетельства'') Id
      , isnull(exam_certificate.[Year], @year)
      , exam_certificate.Number
      , exam_certificate.RegionId
      , isnull(exam_certificate_deny.IsDeny, 0)
      , exam_certificate_deny.Reason
      , exam_certificate_deny.NewCertificateNumber
      , isnull(exam_certificate_request.TypographicNumber, exam_certificate.TypographicNumber)
      , exam_certificate.ParticipantID
    from (select 
        exam_certificate_request.[Index]
        , exam_certificate_request.LastName 
        , exam_certificate_request.FirstName 
        , exam_certificate_request.PatronymicName 
        , exam_certificate_request.PassportSeria
        , dbo.GetInternalPassportSeria(exam_certificate_request.PassportSeria) InternalPassportSeria
        , exam_certificate_request.PassportNumber
        , exam_certificate_request.TypographicNumber 
      from #CommonNationalExamCertificateRequest exam_certificate_request) exam_certificate_request
      left join 
        (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
         FROM <dataDbName>.rbd.Participants a with (nolock, fastfirstrow) 
          left JOIN <dataDbName>.prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) exam_certificate
        on  exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_request.LastName
          and 1= case when exam_certificate_request.FirstName is null then 1
            when exam_certificate.FirstName collate cyrillic_general_ci_ai = exam_certificate_request.FirstName then 1
            else 0
          end
          and 1 = case when exam_certificate_request.PatronymicName is null then 1
             when exam_certificate.PatronymicName collate cyrillic_general_ci_ai = exam_certificate_request.PatronymicName  then 1
             else 0
          end
          and 1 = case when exam_certificate_request.PassportSeria is null then 1 
             when exam_certificate.PassportSeria collate cyrillic_general_ci_ai = exam_certificate_request.PassportSeria then 1
             else 0
          end         
          and 1 = case when exam_certificate_request.PassportNumber is null then 1
             when exam_certificate.PassportNumber collate cyrillic_general_ci_ai = exam_certificate_request.PassportNumber then 1
             else 0
          end
          and 1 = case when @IsTypographicNumber = 0 then 1 
                 when exam_certificate_request.TypographicNumber is null then 1 
                 when @IsTypographicNumber = 1   
                  and exam_certificate.TypographicNumber collate cyrillic_general_ci_ai = exam_certificate_request.TypographicNumber then 1 
                else 0
          end
          and exam_certificate.[Year] between @yearFrom and @yearTo
          
        left join (
          select 
            exam_certificate_deny.Reason
            , null NewCertificateNumber
            , exam_certificate_deny.CertificateFK
            , 1 IsDeny
            , exam_certificate_deny.[UseYear] [Year]
          from <dataDbName>.prn.CancelledCertificates exam_certificate_deny with(nolock)) as exam_certificate_deny
          on exam_certificate_deny.CertificateFK = exam_certificate.id
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo                  

    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''passport_or_typo'' 
    '
  
  set @declareCommandText = replace(@declareCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))
  set @executeCommandText = replace(replace(@executeCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
  
--  print @chooseDbText
--  print @declareCommandText
--  print @executeCommandText

  declare @CommonText nvarchar(max)
  set @CommonText=@chooseDbText + @IndexText + @declareCommandText + @executeCommandText
  print  @CommonText
  exec sp_executesql @CommonText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateCheckByPassport]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheckByPassport]
    @batchId BIGINT,
    @type INT = 1
AS 
    BEGIN

        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX),
            @yearCommandText NVARCHAR(MAX),
            @executeCommandText NVARCHAR(MAX),
            @searchCommandText NVARCHAR(MAX),
            @searchCommandText2 NVARCHAR(MAX),
            @addCondition NVARCHAR(MAX),
            @selectPass NVARCHAR(500)
        IF @type = 1 
            BEGIN
  
                SET @yearCommandText = N'
   declare
      @yearFrom int
      , @yearTo int     
      , @year int
    select  
      @yearFrom = actuality_years.YearFrom,
      @yearTo = actuality_years.YearTo
    from
      dbo.GetCommonNationalExamCertificateActuality() actuality_years
    '   
                SET @addCondition = 'exam_certificate.Number collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai'
                SET @selectPass = ', exam_certificate.PassportSeria
      , exam_certificate.PassportNumber'
            END
        IF @type = 2 
            BEGIN
                UPDATE  #CommonNationalExamCertificateCheck
                SET PassportSeria = REPLACE(PassportSeria, ' ', '')
                
                SET @yearCommandText = N'
          declare
              @yearFrom int
              , @yearTo int     
              , @year int

            select @year = cnecrb.year
            from CommonNationalExamCertificateCheckBatch as cnecrb
            where cnecrb.id = <BatchIdentifier>

            if @year is not null
              select @yearFrom = @year, @yearTo = @year
            else
              select @yearFrom = Year(GetDate()) - 5, @yearTo = Year(GetDate())
            '   
                SET @addCondition = '(exam_certificate.PassportSeria collate cyrillic_general_ci_ai = exam_certificate_check.PassportSeria collate cyrillic_general_ci_ai)
           and exam_certificate.PassportNumber collate cyrillic_general_ci_ai = exam_certificate_check.PassportNumber collate cyrillic_general_ci_ai
          '
                SET @selectPass = ', exam_certificate_check.PassportSeria
                   , exam_certificate_check.PassportNumber'
            END
        SET @chooseDbText = REPLACE('use <database>', '<database>',
                                    dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)

        SET @declareCommandText = N'
    if (select count(*) from #CommonNationalExamCertificateCheck)=0
    begin
     raiserror(''CommonNationalExamCertificateCheck is empty'',16,1)
     return
    end
    
    declare @certificateCheckIds table
      (
      Id bigint
      , CertificateNumber nvarchar(255)
      )

    declare @certificate_subject_correctness table
      (
      
      CertificateIdGuid uniqueidentifier
      , CertificateSubjectId bigint
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectIdGuid uniqueidentifier
      , BatchId bigint
      , CertificateCheckingId uniqueidentifier
      , ShareCertificateCheckingId uniqueidentifier
      , Year int
      , SubjectCode nvarchar(255)
      )
      declare @certificate_subject_correctness_result table
      (
      
      CertificateIdGuid uniqueidentifier
      , CertificateSubjectId bigint
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectIdGuid uniqueidentifier
      , BatchId bigint
      , CertificateCheckingId uniqueidentifier
      , ShareCertificateCheckingId uniqueidentifier
      , Year int
      , SubjectCode nvarchar(255)
      )
    
    declare @certificate_correctness table
      (
        id int not null identity(1,1) primary key,
      CertificateNumber nvarchar(255)
      , CertificateIdGuid uniqueidentifier
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , SourceLastName nvarchar(255)
      , SourceFirstName nvarchar(255)
      , SourcePatronymicName nvarchar(255)
      , IsCertificateCorrect bit
      , BatchId bigint
      , IsDeny bit
      , DenyComment ntext 
      , DenyNewCertificateNumber nvarchar(255)
      , CertificateYear int
      , CertificateCheckingId uniqueidentifier
      , ShareCertificateCheckingId uniqueidentifier
      , [Index] bigint
      , TypographicNumber nvarchar(255)
      , RegionId int
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
      '
        SET @searchCommandText = N' 
      
    select  exam_certificate.id as ID1,exam_certificate_check.id as ID2 into #tt
    from #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)      
      left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock) 
      on  <addCondition>
      and exam_certificate.[Year] between @yearFrom and @yearTo 
    where
      exam_certificate_check.BatchId = <BatchIdentifier>            
    
    insert @certificate_correctness
      (
      CertificateNumber
      , CertificateIdGuid
      , LastName
      , FirstName
      , PatronymicName
      , SourceLastName
      , SourceFirstName
      , SourcePatronymicName
      , IsCertificateCorrect
      , BatchId
      , IsDeny
      , DenyComment
      , DenyNewCertificateNumber
      , CertificateYear
      , CertificateCheckingId
      ,ShareCertificateCheckingId
      , [Index]
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    select 
      isnull(exam_certificate_check.CertificateNumber,exam_certificate.Number)
      , exam_certificate.Id
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName 
      , case when exam_certificate.Number is not null and exam_certificate_deny.Id is null then 1 else 0  end 
      , <BatchIdentifier>
      , case
        when not exam_certificate_deny.Id is null then 1
        else 0
      end
      , exam_certificate_deny.Comment
      ,  exam_certificate_deny.NewCertificateNumber
      , exam_certificate.[Year]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.ShareCertificateCheckingId
      , exam_certificate_check.[Index]
      , exam_certificate.TypographicNumber
      , exam_certificate.RegionId
      <@selectPass>
    from 
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
      left join #tt a on a.ID2=exam_certificate_check.id
      left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)  on a.ID1=exam_certificate.id     
        left join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate.Number collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
        
      
    drop table #tt
    /*from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
        left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
          on  
          <addCondition>
            and exam_certificate.[Year] between @yearFrom and @yearTo           
        left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate.Number collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
    where
      exam_certificate_check.BatchId = <BatchIdentifier>*/    
      
    '
        SET @searchCommandText = REPLACE(@searchCommandText, '<@selectPass>',
                                         @selectPass)   
        SET @searchCommandText = @yearCommandText + REPLACE(@searchCommandText,
                                                            '<addCondition>',
                                                            @addCondition)
        SET @searchCommandText2 = ' 
    insert @certificate_subject_correctness_result
      (
      CertificateIdGuid
      , CertificateSubjectId
      , Mark 
      , HasAppeal
      , IsSubjectCorrect
      , SourceMark 
      , SourceCertificateSubjectIdGuid  
      , BatchId
      , CertificateCheckingId
      ,ShareCertificateCheckingId
      , Year
      ,SubjectCode
      )
    select 
      isnull(certificate_correctness.[CertificateIdGuid], exam_certificate_subject.[CertificateFK])
      , isnull(exam_certificate_subject.SubjectCode, subject.Id)
      , exam_certificate_subject_check.Mark
      , exam_certificate_subject.HasAppeal
      , case
        when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
          then 1
        else 0
      end 
      , exam_certificate_subject.Mark
      , exam_certificate_subject.CertificateMarkID
      , <BatchIdentifier>
      , coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
      , coalesce(certificate_correctness.[ShareCertificateCheckingId], exam_certificate_subject_check.[ShareCertificateCheckingId])
      , coalesce(exam_certificate_subject.Useyear, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
      ,subject.Code
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateIdGuid = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[UseYear]
        full outer join 
        ( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject with(nolock)
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai            
            )   
            on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
          
    declare @CertificateCheckingId uniqueidentifier,@id int,@NewCertificateCheckingId uniqueidentifier
    declare certificate_correctness_cursor cursor local fast_forward for select id,CertificateCheckingId from @certificate_correctness      
    open certificate_correctness_cursor
    fetch next from certificate_correctness_cursor into @id,@CertificateCheckingId
    while @@FETCH_STATUS=0
    begin
      if exists(select * from @certificate_correctness where CertificateCheckingId=@CertificateCheckingId and id<>@id)
      begin
        set @NewCertificateCheckingId=NEWID()
        update @certificate_correctness set CertificateCheckingId=@NewCertificateCheckingId where id=@id
        insert #CommonNationalExamCertificateSubjectCheck(ShareCertificateCheckingId,CertificateCheckingId,BatchId,SubjectCode,Mark) 
        select @CertificateCheckingId,@NewCertificateCheckingId,BatchId,SubjectCode,Mark from #CommonNationalExamCertificateSubjectCheck where CertificateCheckingId=@CertificateCheckingId
      end
      fetch next from certificate_correctness_cursor into @id,@CertificateCheckingId
    end 
close certificate_correctness_cursor
deallocate certificate_correctness_cursor
--select * from @certificate_subject_correctness_result
--select * from #CommonNationalExamCertificateCheck--SubjectCheck 

    insert @certificate_subject_correctness
      (
      CertificateIdGuid
      , CertificateSubjectId
      , Mark 
      , HasAppeal
      , IsSubjectCorrect
      , SourceMark 
      , SourceCertificateSubjectIdGuid  
      , BatchId
      , CertificateCheckingId
      ,ShareCertificateCheckingId
      , Year
      ,SubjectCode
      )
    select 
      coalesce(certificate_correctness.[CertificateIdGuid], exam_certificate_subject.[CertificateFK])
      , isnull(exam_certificate_subject.SubjectCode, subject.Id)
      , exam_certificate_subject_check.Mark
      , exam_certificate_subject.HasAppeal
      , case
        when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
          then 1
        else 0
      end 
      , exam_certificate_subject.Mark
      , exam_certificate_subject.CertificateMarkID
      , <BatchIdentifier>
      , coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
      , coalesce(certificate_correctness.[ShareCertificateCheckingId], exam_certificate_subject_check.[ShareCertificateCheckingId])
      , coalesce(exam_certificate_subject.useyear, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
      ,subject.Code
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateIdGuid = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[useyear]
        full outer join 
        ( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject with(nolock)
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai            
            )   
            on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
              
--select * from @certificate_subject_correctness order by 1
--select * from @certificate_subject_correctness_result order by 1
declare @idcert uniqueidentifier
declare cur cursor for select ShareCertificateCheckingId from @certificate_correctness
open cur
fetch next from cur into @idcert
while @@fetch_status=0
begin
--select @idcert
 if exists(select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert  and mark is not null and SourceCertificateSubjectIdGuid is null)
 begin 
 --print -999 
  if <@type>=2
  update @certificate_correctness 
  set CertificateNumber=NULL,CertificateIdGuid=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
  select * from @certificate_correctness 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectIdGuid=null,year=null,SubjectCode=null,HasAppeal=null
  where ShareCertificateCheckingId=@idcert 
 end
--select * from @certificate_correctness where ShareCertificateCheckingId=@idcert 
--select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert 
--Сделал так чтобы если есть нет не одного совпадения то занулять проверку. Разделил на два блока так как не знаб логику работы первого блока
 if exists(select * from 
        (
          select max(cast(IsSubjectCorrect as int)) IsSubjectCorrect 
          from @certificate_subject_correctness_result 
          where ShareCertificateCheckingId=@idcert and mark is not null
          group by SubjectCode
        ) t 
        where IsSubjectCorrect=0)
  and exists(select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert and mark is not null)
 begin 
 --print -999 
  if <@type>=2
  update @certificate_correctness 
  set CertificateNumber=NULL,CertificateIdGuid=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
  select 1
  --select * from @certificate_correctness 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectIdGuid=null,year=null,SubjectCode=null,HasAppeal=null
  where ShareCertificateCheckingId=@idcert 
 end
 
 fetch next from cur into @idcert
end
close cur deallocate cur

 if <@type>=2
 begin
  delete b from  
  @certificate_correctness a 
  join @certificate_correctness b on a.PassportNumber=b.PassportNumber and a.PassportSeria=b.PassportSeria
  where a.CertificateIdGuid is null and b.CertificateIdGuid is null
  and  b.id>a.id
 end
--select * from @certificate_correctness
--select * from @certificate_subject_correctness order by CertificateIdGuid,CertificateSubjectId


    '

  
        SET @executeCommandText = N'
    delete exam_certificate_check
    from dbo.CommonNationalExamCertificateCheck exam_certificate_check
    where exam_certificate_check.BatchId = <BatchIdentifier>

    declare @certificateCheckId table
      (
      CertificateCheckId bigint
      , CheckingCertificateId uniqueidentifier
      )
    
    insert dbo.CommonNationalExamCertificateCheck
      (
      CertificateCheckingId
      , BatchId
      , CertificateNumber
      , LastName
      , FirstName
      , PatronymicName
      , IsCorrect
      , SourceCertificateIdGuid
      , SourceLastName
      , SourceFirstName
      , SourcePatronymicName
      , IsDeny
      , DenyComment
      , DenyNewCertificateNumber
      , Year
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    output inserted.Id, inserted.CertificateCheckingId 
      into @certificateCheckId 
    select 
      certificate_correctness.CertificateCheckingId
      , <BatchIdentifier>
      , isnull(certificate_correctness.CertificateNumber,'''')
      , isnull(certificate_correctness.LastName,'''')
      , certificate_correctness.FirstName
      , certificate_correctness.PatronymicName
      , case 
        when (certificate_correctness.IsCertificateCorrect = 1 
          and not exists(select 1 
            from @certificate_subject_correctness subject_correctness
            where subject_correctness.IsSubjectCorrect = 0 
              and certificate_correctness.CertificateIdGuid = subject_correctness.CertificateIdGuid
              and certificate_correctness.CertificateCheckingId = subject_correctness.CertificateCheckingId)) 
          then 1
        else 0
      end 
      , certificate_correctness.CertificateIdGuid
      , certificate_correctness.SourceLastName
      , certificate_correctness.SourceFirstName
      , certificate_correctness.SourcePatronymicName
      , certificate_correctness.IsDeny
      , certificate_correctness.DenyComment
      , certificate_correctness.DenyNewCertificateNumber
      , certificate_correctness.CertificateYear
      , certificate_correctness.TypographicNumber
      , certificate_correctness.RegionId
      , certificate_correctness.PassportSeria
      , certificate_correctness.PassportNumber
    from @certificate_correctness certificate_correctness 
        
    delete exam_certificate_subject_check
    from dbo.CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check
    where exam_certificate_subject_check.BatchId = <BatchIdentifier>

    insert dbo.CommonNationalExamCertificateSubjectCheck
      (
      BatchId
      , CheckId
      , SubjectId
      , Mark
      , IsCorrect
      , SourceCertificateSubjectIdGuid
      , SourceMark
      , SourceHasAppeal
      , Year
      ) 
    select 
    distinct
      <BatchIdentifier>
      , certificate_check_id.CertificateCheckId
      , subject_correctness.CertificateSubjectId 
      , subject_correctness.Mark
      , subject_correctness.IsSubjectCorrect 
      , subject_correctness.SourceCertificateSubjectIdGuid
      , subject_correctness.SourceMark
      , subject_correctness.HasAppeal
      , subject_correctness.Year
    from 
      @certificate_subject_correctness subject_correctness
        inner join @certificateCheckId certificate_check_id
          on certificate_check_id.CheckingCertificateId = subject_correctness.CertificateCheckingId 
      
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
   
       --select * from CommonNationalExamCertificateSubjectCheck where BatchId=<BatchIdentifier>    order by checkid,subjectid 
       --drop table rrr
       --   select * into rrr from #CommonNationalExamCertificateCheck
       --   drop table ttt
       --   select * into ttt from #CommonNationalExamCertificateSubjectCheck
    '

        SET @searchCommandText = REPLACE(REPLACE(@searchCommandText,
                                                 '<BatchIdentifier>',
                                                 CONVERT(NVARCHAR(255), @batchId)),
                                         '<dataDbName>', @baseName)
        SET @searchCommandText2 = REPLACE(REPLACE(REPLACE(@searchCommandText2,
                                                          '<BatchIdentifier>',
                                                          CONVERT(NVARCHAR(255), @batchId)),
                                                  '<dataDbName>', @baseName),
                                          '<@type>',
                                          CAST(@type AS VARCHAR(10)))
        SET @executeCommandText = REPLACE(REPLACE(@executeCommandText,
                                                  '<BatchIdentifier>',
                                                  CONVERT(NVARCHAR(255), @batchId)),
                                          '<dataDbName>', @baseName)

--print '---begin: PCNECC---\n'
--print @chooseDbText
--print @declareCommandText
--print @searchCommandText
--print @searchCommandText2
--print @executeCommandText
--print '\n---end: PCNECC---'

        DECLARE @commandText NVARCHAR(MAX)
        SET @commandText = @chooseDbText + @declareCommandText
            + @searchCommandText + @searchCommandText2 + @executeCommandText
  
        EXEC sp_executesql @commandText

        RETURN 0
    END
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateCheckByNumber]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
   Для работы  процедуры  требуются  следующие 
   временные таблицы:

create table #CommonNationalExamCertificateCheck
    ( 
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , CertificateNumber nvarchar(255)
    , LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , [Index] bigint
    )
  create table #CommonNationalExamCertificateSubjectCheck
    (
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , SubjectCode nvarchar(255)
    , Mark numeric(5,1)
    )
*/

-- exec dbo.PrepareCommonNationalExamCertificateCheck 
-- ================================================
-- Подготовка пакетов для проверки сертификатов ЕГЭ.
-- ================================================
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheckByNumber]
@batchId bigint,@type int=1
as
begin
  declare
    @chooseDbText nvarchar(max)
    , @baseName nvarchar(255
    )
    , @declareCommandText nvarchar(max)
    , @yearCommandText nvarchar(max)
    , @executeCommandText nvarchar(max)
    , @searchCommandText nvarchar(max)
    , @searchCommandText2 nvarchar(max)
    , @addCondition nvarchar(max)
    , @selectPass nvarchar(500)
  if @type=1
  begin
  
   set @yearCommandText=N'
   declare
      @yearFrom int
      , @yearTo int     
      , @year int
    select  
      @yearFrom = actuality_years.YearFrom,
      @yearTo = actuality_years.YearTo
    from
      dbo.GetCommonNationalExamCertificateActuality() actuality_years
    '   
   set @addCondition='exam_certificate.Number collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai'
   set @selectPass=', exam_certificate.PassportSeria
      , exam_certificate.PassportNumber'
  end
  if @type=2
  begin
  update #CommonNationalExamCertificateCheck set PassportSeria=replace(PassportSeria, ' ', '')
   set @yearCommandText=N'
  declare
      @yearFrom int
      , @yearTo int     
      , @year int

    select @year = cnecrb.year
    from CommonNationalExamCertificateCheckBatch as cnecrb
    where cnecrb.id = <BatchIdentifier>

    if @year is not null
      select @yearFrom = @year, @yearTo = @year
    else
      select @yearFrom = Year(GetDate()) - 5, @yearTo = Year(GetDate())
      
          '   
   set @addCondition='exam_certificate.InternalPassportSeria collate cyrillic_general_ci_ai = exam_certificate_check.PassportSeria collate cyrillic_general_ci_ai
   and exam_certificate.PassportNumber collate cyrillic_general_ci_ai = exam_certificate_check.PassportNumber collate cyrillic_general_ci_ai
   '
   set @selectPass=', exam_certificate_check.PassportSeria
      , exam_certificate_check.PassportNumber'
  end
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1)

  set @declareCommandText = 
    N'
    if (select count(*) from #CommonNationalExamCertificateCheck)=0
    begin
     raiserror(''CommonNationalExamCertificateCheck is empty'',16,1)
     return
    end
    
    declare @certificateCheckIds table
      (
      Id bigint
      , CertificateNumber nvarchar(255)
      )

    declare @certificate_subject_correctness table
      (
      
      CertificateId uniqueidentifier
      , CertificateSubjectId int
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectIdGuid uniqueidentifier
      , BatchId bigint
      , CertificateCheckingId uniqueidentifier
      , Year int
      , SubjectCode nvarchar(255)
      )
    
    declare @certificate_correctness table
      (
        id int not null identity(1,1) primary key,
      CertificateNumber nvarchar(255)
      , CertificateId uniqueidentifier
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , SourceLastName nvarchar(255)
      , SourceFirstName nvarchar(255)
      , SourcePatronymicName nvarchar(255)
      , IsCertificateCorrect bit
      , BatchId bigint
      , IsDeny bit
      , DenyComment ntext 
      , DenyNewCertificateNumber nvarchar(255)
      , CertificateYear int
      , CertificateCheckingId uniqueidentifier
      , [Index] bigint
      , TypographicNumber nvarchar(255)
      , RegionId int
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
      '
  set @searchCommandText = 
    N'  
    select @yearFrom,@yearTo      
    
    insert @certificate_correctness
      (
      CertificateNumber
      , CertificateId
      , LastName
      , FirstName
      , PatronymicName
      , SourceLastName
      , SourceFirstName
      , SourcePatronymicName
      , IsCertificateCorrect
      , BatchId
      , IsDeny
      , DenyComment
      , DenyNewCertificateNumber
      , CertificateYear
      , CertificateCheckingId
      , [Index]
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    select 
      isnull(exam_certificate_check.CertificateNumber,exam_certificate.Number)
      , exam_certificate.Id
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName 
      , case when exam_certificate.Number is not null and exam_certificate_deny.Id is null then 1 else 0  end 
      , <BatchIdentifier>
      , case
        when not exam_certificate_deny.Id is null then 1
        else 0
      end
      , exam_certificate_deny.Comment
      , exam_certificate_deny.NewCertificateNumber
      , exam_certificate.[Year]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.[Index]
      , exam_certificate.TypographicNumber
      , exam_certificate.RegionId
      <@selectPass>
    from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
        left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
          on  
          <addCondition>
            and exam_certificate.[Year] between @yearFrom and @yearTo           
        left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
    where
      exam_certificate_check.BatchId = <BatchIdentifier>
      
    select ''sdfsdfdsfddd'',*
    from @certificate_correctness
            
    '
set @searchCommandText=replace(@searchCommandText,'<@selectPass>',@selectPass)    
set @searchCommandText=
@yearCommandText+
REPLACE(@searchCommandText,'<addCondition>',@addCondition)
    set @searchCommandText2 = '
    

    insert @certificate_subject_correctness
      (
      CertificateId
      , CertificateSubjectId
      , Mark 
      , HasAppeal
      , IsSubjectCorrect
      , SourceMark 
      , SourceCertificateSubjectIdGuid
      , BatchId
      , CertificateCheckingId
      , Year
      ,SubjectCode
      )
    select 
      coalesce(certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateFK])
      , isnull(exam_certificate_subject.SubjectCode, subject.Id)
      , exam_certificate_subject_check.Mark
      , exam_certificate_subject.HasAppeal
      , case
        when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
          then 1
        else 0
      end 
      , exam_certificate_subject.Mark
      , exam_certificate_subject.CertificateMarkID
      , <BatchIdentifier>
      , coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
      , coalesce(exam_certificate_subject.useyear, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
      ,subject.Code
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[useyear]
        full outer join 
        ( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject with(nolock)
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai            
            )   on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
            and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId  
--select * from @certificate_subject_correctness order by 1
--select * from @certificate_correctness
declare @idcert uniqueidentifier
declare cur cursor for select CertificateCheckingId from @certificate_correctness
open cur
fetch next from cur into @idcert
while @@fetch_status=0
begin
--select @idcert
--select * from @certificate_subject_correctness where CertificateCheckingId=@idcert  and mark is not null and (SourceCertificateSubjectIdGuid is null or IsSubjectCorrect=0)
 if exists(select * from @certificate_subject_correctness where CertificateCheckingId=@idcert  and mark is not null and (SourceCertificateSubjectIdGuid is null or IsSubjectCorrect=0))
 begin
 --print -999
 if <@type>=1
  update @certificate_correctness 
  set CertificateId=null,SourceLastName=null,PassportNumber=null,PassportSeria=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where CertificateCheckingId=@idcert 
 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectIdGuid=null,year=null,SubjectCode=null,HasAppeal=null
  where CertificateCheckingId=@idcert 
 end
 fetch next from cur into @idcert
end
close cur deallocate cur

select * from @certificate_correctness
select * from @certificate_subject_correctness order by CertificateId,CertificateSubjectId
--select * from #CommonNationalExamCertificateSubjectCheck 

    '

  
  set @executeCommandText = 
    N'
    delete exam_certificate_check
    from dbo.CommonNationalExamCertificateCheck exam_certificate_check
    where exam_certificate_check.BatchId = <BatchIdentifier>

    declare @certificateCheckId table
      (
      CertificateCheckId bigint
      , CheckingCertificateId uniqueidentifier
      )
    
    insert dbo.CommonNationalExamCertificateCheck
      (
      CertificateCheckingId
      , BatchId
      , CertificateNumber
      , LastName
      , FirstName
      , PatronymicName
      , IsCorrect
      , SourceCertificateIdGuid
      , SourceLastName
      , SourceFirstName
      , SourcePatronymicName
      , IsDeny
      , DenyComment
      , DenyNewCertificateNumber
      , Year
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    output inserted.Id, inserted.CertificateCheckingId 
      into @certificateCheckId 
    select 
      certificate_correctness.CertificateCheckingId
      , <BatchIdentifier>
      , isnull(certificate_correctness.CertificateNumber,'''')
      , isnull(certificate_correctness.LastName,'''')
      , certificate_correctness.FirstName
      , certificate_correctness.PatronymicName
      , case 
        when (certificate_correctness.IsCertificateCorrect = 1 
          and not exists(select 1 
            from @certificate_subject_correctness subject_correctness
            where subject_correctness.IsSubjectCorrect = 0 
              and certificate_correctness.CertificateId = subject_correctness.CertificateId
              and certificate_correctness.CertificateCheckingId = subject_correctness.CertificateCheckingId)) 
          then 1
        else 0
      end 
      , certificate_correctness.CertificateId
      , certificate_correctness.SourceLastName
      , certificate_correctness.SourceFirstName
      , certificate_correctness.SourcePatronymicName
      , certificate_correctness.IsDeny
      , certificate_correctness.DenyComment
      , certificate_correctness.DenyNewCertificateNumber
      , certificate_correctness.CertificateYear
      , certificate_correctness.TypographicNumber
      , certificate_correctness.RegionId
      , certificate_correctness.PassportSeria
      , certificate_correctness.PassportNumber
    from @certificate_correctness certificate_correctness       
            
select ''dfsdfsdf'',*
    from @certificate_correctness
                
    delete exam_certificate_subject_check
    from dbo.CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check
    where exam_certificate_subject_check.BatchId = <BatchIdentifier>


    insert dbo.CommonNationalExamCertificateSubjectCheck
      (
      BatchId
      , CheckId
      , SubjectId
      , Mark
      , IsCorrect
      , SourceCertificateSubjectIdGuid
      , SourceMark
      , SourceHasAppeal
      , Year
      ) 
    select 
    distinct
      <BatchIdentifier>
      , certificate_check_id.CertificateCheckId
      , subject_correctness.CertificateSubjectId 
      , subject_correctness.Mark
      , subject_correctness.IsSubjectCorrect 
      , subject_correctness.SourceCertificateSubjectIdGuid
      , subject_correctness.SourceMark
      , subject_correctness.HasAppeal
      , subject_correctness.Year
    from 
      @certificate_subject_correctness subject_correctness
        inner join @certificateCheckId certificate_check_id
          on certificate_check_id.CheckingCertificateId = subject_correctness.CertificateCheckingId 
          
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
        
       select * from CommonNationalExamCertificateSubjectCheck where BatchId=<BatchIdentifier>    order by checkid,subjectid 
       --drop table rrr
       --   select * into rrr from #CommonNationalExamCertificateCheck
       --   drop table ttt
       --   select * into ttt from #CommonNationalExamCertificateSubjectCheck
    '

  set @searchCommandText = replace(replace(@searchCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
  set @searchCommandText2 = replace(replace(replace(@searchCommandText2, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName),'<@type>',CAST(@type as varchar(10)))
  set @executeCommandText = replace(replace(@executeCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)

--print '---begin: PCNECC---\n'
--print @chooseDbText
--print @declareCommandText
--print @searchCommandText
--print @searchCommandText2
--print @executeCommandText
--print '\n---end: PCNECC---'

  declare @commandText nvarchar(max)
  set @commandText=@chooseDbText +@declareCommandText + @searchCommandText + @searchCommandText2 + @executeCommandText
  
  exec sp_executesql @commandText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum]
    @batchId BIGINT=8774
AS 
    BEGIN
set nocount on
        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX)           
     
  
  --goto ttt   
   set @declareCommandText='
   
   declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
  -- update CommonNationalExamCertificateCheckBatch set Executing=1 where id=<@BatchId>
         --[PrepareCommonNationalExamCertificateCheckByFIOBySum    
update a set SubjectId=b.Id
from 
#CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code

declare @CertificateCheckingId  uniqueidentifier,@PassportNumber  nvarchar(255) ,@sum numeric(12,2)     
 declare cc cursor local fast_forward for 
       select 
       distinct
       --top 1
        a.CertificateCheckingId,b.PassportNumber,cast(a.PassportSeria as numeric(12,2))
       from 
       #CommonNationalExamCertificateCheck a join 
       vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber    
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
       where b.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
  open cc
  create table #t (SubjectId int,mark numeric(5,1))   
  declare @maxsum numeric(12,2)
  create index sdcjhsdklcj_sadfvsd_asdvd on #t(SubjectId)
  create table #tt (c varchar(4000) not null, s numeric(12,2) not null, level int not null, id int identity, primary key clustered(level, id))
  fetch next from cc into @CertificateCheckingId,@PassportNumber,@sum
  while @@FETCH_STATUS=0
  begin   
    -- print ''    10.''+right(convert(varchar,SYSDATETIME()),12)
    if(select LastName from #CommonNationalExamCertificateCheck where CertificateCheckingId=@CertificateCheckingId)=''1''
      goto endcur
    if exists(select * from #CommonNationalExamCertificateSubjectCheck where SubjectCode is null and CertificateCheckingId=@CertificateCheckingId)  
      goto endcur
    truncate table #t
    insert #t
    select
    c.SubjectCode,c.mark  
    from 
    dbo.vw_Examcertificate b with(nolock)
    join prn.CertificatesMarks c with(nolock) on b.Id=c.CertificateFK   
    join #CommonNationalExamCertificateSubjectCheck d on  c.SubjectCode=d.SubjectId and d.CertificateCheckingId=@CertificateCheckingId
    where b.[YEAR] between  @yearFrom and @yearTo and b.PassportNumber=@PassportNumber 
    --print ''    20.''+right(convert(varchar,SYSDATETIME()),12)
  if not exists(select * from 
    #CommonNationalExamCertificateSubjectCheck b      
    left join #t c on c.SubjectId=b.SubjectId
    where b.CertificateCheckingId=@CertificateCheckingId and c.SubjectId is null )
  begin
    
    truncate table #tt
    declare @i int, @l int
    select @i = min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where CertificateCheckingId=@CertificateCheckingId
    set @l = 0
    insert #tt(c, s, level) values('''', 0, 0)
    --print ''    30.''+right(convert(varchar,SYSDATETIME()),12)    
    while @i is not null 
    begin
      insert #tt(c, s, level) 
      select
      --'''',
       t.c+case t.c when '''' then '''' else ''+'' end+cast(x.mark as varchar(16)),
       t.s+x.mark, @l+1
      from (select * from #tt where level = @l) t
        cross join
        (select * from #t where SubjectId = @i) x
  
      set @i = (select min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where SubjectId>@i and CertificateCheckingId=@CertificateCheckingId)
      set @l = @l+1
    end

    
    
    if exists(select * from #tt where level = @l and s=@sum)
    begin    
     update #CommonNationalExamCertificateCheck set lastname=''1'',FirstName=cast(@sum as varchar(255)) where CertificateCheckingId=@CertificateCheckingId    
    end
    else
    begin
  --  select c, s from #tt where level = @l
     select @maxsum=max(s) from #tt where level = @l  
     update #CommonNationalExamCertificateCheck set FirstName=cast(@maxsum as varchar(255)) 
     where CertificateCheckingId=@CertificateCheckingId and cast(isnull(FirstName,0) as numeric(12,2))<@maxsum
     end
  end
    
    
 endcur:
 --print SYSDATETIME()
         fetch next from cc into @CertificateCheckingId,@PassportNumber,@sum
   end   
    drop table #t
    drop table #tt
   --select * from #CommonNationalExamCertificateCheck  
  delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
  print SYSDATETIME()
  insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)  
  select <@BatchId>,CertificateNumber,isnull(cast(FirstName as numeric(12,1)),-1),
  case when not exists(
        select * from 
        vw_Examcertificate ta with(nolock) 
        left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
        where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
      when lastname is null then 0 else 1 end [Status],
      case when (select COUNT(distinct PassportNumber) from vw_Examcertificate with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake
      from #CommonNationalExamCertificateCheck a
      print SYSDATETIME()
      --update CommonNationalExamCertificateCheckBatch set Executing=0,IsProcess=1 where id=<@BatchId>
        select * from CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>     
        -- select * from CommonNationalExamCertificateCheckBatch where id=<@BatchId>  
  '
 -- print @declareCommandText
  
  SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)
        set @declareCommandText=@chooseDbText+replace(@declareCommandText,'<@BatchId>',@BatchId)
       -- print @declareCommandText
EXEC sp_executesql @declareCommandText
  return 

       
     --[PrepareCommonNationalExamCertificateCheckByFIOBySum]    

--return
--ttt:
--        SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
--        SET @baseName = dbo.GetDataDbName(1, 1)
--set @chooseDbText=@chooseDbText+
--'
--drop table #CommonNationalExamCertificateCheck
--drop table #CommonNationalExamCertificateSubjectCheck
--select * into #CommonNationalExamCertificateCheck   from #CommonNationalExamCertificateCheck
--select * into #CommonNationalExamCertificateSubjectCheck   from #CommonNationalExamCertificateSubjectCheck
--'
----set @declareCommandText=@chooseDbText+replace(@declareCommandText,'@batchId',@BatchId)
--EXEC sp_executesql @chooseDbText
--return 

end
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateCheckByFioAndSubjects]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheckByFioAndSubjects]
    @batchId BIGINT=8774
AS 
    BEGIN
set nocount on
        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX)           
     declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
  
     
   set @declareCommandText='
   drop table      tmp_CommonNationalExamCertificateCheck
   select * into tmp_CommonNationalExamCertificateCheck from #CommonNationalExamCertificateCheck
   
   drop table      tmp_CommonNationalExamCertificateSubjectCheck
   select * into tmp_CommonNationalExamCertificateSubjectCheck from #CommonNationalExamCertificateSubjectCheck
   return 
   update a set SubjectId=b.Id
  from 
  #CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code
     --select * from #CommonNationalExamCertificateSubjectCheck
   declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
       --PrepareCommonNationalExamCertificateCheckByFIO    
       delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)
       select
       <@BatchId> BatchId,a.CertificateNumber Name,a.PassportNumber [Sum],      
        case when not exists(
        select * from 
        dbo.vw_Examcertificate ta with(nolock) 
        left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
        where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
      when tt.CertificateCheckingId is null then 0 else 1 end [Status],
      case when (select COUNT(distinct PassportNumber) from dbo.vw_Examcertificate with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake    
       from
       #CommonNationalExamCertificateCheck a        
       left join        
       (
       select * 
       from     
       (select distinct a1.CertificateCheckingId,a1.CertificateNumber fio,a1.PassportNumber sum, b1.PassportNumber from 
       #CommonNationalExamCertificateCheck a1 join 
       dbo.vw_Examcertificate b1 with(nolock) on  b1.fio=a1.CertificateNumber
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b1.Number
       where b1.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
       ) tbl
       where  
       
       not exists(
       select * from
       (
       select SubjectCode,Mark from #CommonNationalExamCertificateSubjectCheck a 
       where a.CertificateCheckingId=tbl.CertificateCheckingId
       ) t1
       left join 
       (
       select
        distinct a.CertificateCheckingId,a.PassportNumber,a.Code,a.Mark       
        from
       (
       select a.CertificateCheckingId,b.PassportNumber,d.Code,c.mark
       from 
       #CommonNationalExamCertificateCheck a join 
       dbo.vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
       join [prn].[CertificatesMarks] c with(nolock) on b.Id=c.CertificateFK
       join Subject d on d.id=c.SubjectCode
       where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join(
        select a.CertificateCheckingId,a.PassportNumber,c.Code,b.mark 
       from
       (select distinct a.CertificateCheckingId,a.CertificateNumber,b.PassportNumber from 
       #CommonNationalExamCertificateCheck a join 
       dbo.vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
              where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join #CommonNationalExamCertificateSubjectCheck b on b.CertificateCheckingId=a.CertificateCheckingId
       join Subject c on c.Code=b.SubjectCode

       ) b on a.PassportNumber=b.PassportNumber and a.Mark=b.mark and a.Code=b.Code
       where  a.PassportNumber=tbl.PassportNumber
       ) t2       
       on t1.SubjectCode=t2.code and t1.Mark=t2.Mark
       where t2.code is null
       )
       )       tt  on a.CertificateCheckingId=tt.CertificateCheckingId and a.CertificateNumber=tt.fio
    
       select * from CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       '
       
     --PrepareCommonNationalExamCertificateCheckByFIO    



        SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)
--set @chooseDbText=@chooseDbText+

--'
--drop table #CommonNationalExamCertificateCheck
--drop table #CommonNationalExamCertificateSubjectCheck
--select * into #CommonNationalExamCertificateCheck   from #CommonNationalExamCertificateCheck
--select * into #CommonNationalExamCertificateSubjectCheck   from #CommonNationalExamCertificateSubjectCheck
--'
set @declareCommandText=@chooseDbText+replace(@declareCommandText,'<@BatchId>',@BatchId)
EXEC sp_executesql @declareCommandText
--print @declareCommandText
return 

end
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateCheckByFIO]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheckByFIO]
    @batchId BIGINT=8774
AS 
    BEGIN
set nocount on
        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX)           
     declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
  
     
   set @declareCommandText='
   update a set SubjectId=b.Id
  from 
  #CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code
     --select * from #CommonNationalExamCertificateSubjectCheck
   declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
       --PrepareCommonNationalExamCertificateCheckByFIO    
       delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)
       select
       <@BatchId> BatchId,a.CertificateNumber Name,a.PassportNumber [Sum],      
        case when not exists(
        select * from 
        vw_Examcertificate ta with(nolock) 
        left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
        where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
      when tt.CertificateCheckingId is null then 0 else 1 end [Status],
      case when (select COUNT(distinct PassportNumber) from vw_Examcertificate  with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake   
       from
       #CommonNationalExamCertificateCheck a        
       left join        
       (
       select * 
       from     
       (select distinct a1.CertificateCheckingId,a1.CertificateNumber fio,a1.PassportNumber sum, b1.PassportNumber from 
       #CommonNationalExamCertificateCheck a1 join 
       vw_Examcertificate b1 with(nolock) on  b1.fio=a1.CertificateNumber
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b1.Number
       where b1.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
       ) tbl
       where  
       
       not exists(
       select * from
       (
       select SubjectCode,Mark from #CommonNationalExamCertificateSubjectCheck a 
       where a.CertificateCheckingId=tbl.CertificateCheckingId
       ) t1
       left join 
       (
       select
        distinct a.CertificateCheckingId,a.PassportNumber,a.Code,a.Mark       
        from
       (
       select a.CertificateCheckingId,b.PassportNumber,d.Code,c.mark
       from 
       #CommonNationalExamCertificateCheck a join 
       vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
       join prn.CertificatesMarks c with(nolock) on b.Id=c.CertificateFK
       join Subject d on d.id=c.SubjectCode
       where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join(
        select a.CertificateCheckingId,a.PassportNumber,c.Code,b.mark 
       from
       (select distinct a.CertificateCheckingId,a.CertificateNumber,b.PassportNumber from 
       #CommonNationalExamCertificateCheck a join 
       vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
              where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join #CommonNationalExamCertificateSubjectCheck b on b.CertificateCheckingId=a.CertificateCheckingId
       join Subject c on c.Code=b.SubjectCode

       ) b on a.PassportNumber=b.PassportNumber and a.Mark=b.mark and a.Code=b.Code
       where  a.PassportNumber=tbl.PassportNumber
       ) t2       
       on t1.SubjectCode=t2.code and t1.Mark=t2.Mark
       where t2.code is null
       )
       )       tt  on a.CertificateCheckingId=tt.CertificateCheckingId and a.CertificateNumber=tt.fio
    declare @id bigint,@PassportNumber  nvarchar(255) ,@sum numeric(12,2),@CertificateCheckingId  uniqueidentifier
    --select * from #CommonNationalExamCertificateCheck 
    
    select * into #CommonNationalExamCertificateSumCheck from  CommonNationalExamCertificateSumCheck a where a.BatchId=<@BatchId> and status=0
    
  --  select distinct a.id ,b.PassportNumber,a.sum,cc.CertificateCheckingId
    --from 
    --#CommonNationalExamCertificateSumCheck a 
    --join vw_Examcertificate b with(nolock) on  b.fio=a.name   
    --left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
    --join #CommonNationalExamCertificateCheck cc on cc.CertificateNumber=a.name and cast(cc.PassportSeria as numeric(12,2))=a.sum
    --where  de.CertificateNumber is null
    
  declare cc cursor local fast_forward for 
    select distinct a.id ,b.PassportNumber,a.sum,cc.CertificateCheckingId
    from 
    #CommonNationalExamCertificateSumCheck a 
    join vw_Examcertificate b with(nolock) on  b.fio=a.name   
    left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
    join #CommonNationalExamCertificateCheck cc on cc.CertificateNumber=a.name and cast(cc.PassportSeria as numeric(12,2))=a.sum
    where de.CertificateNumber is null
  open cc
  create table #t (SubjectId int,mark numeric(5,1))   
  declare @maxsum numeric(12,2)
  declare @t table(id bigint primary key)
  create index sdcjhsdklcj_sadfvsd_asdvd on #t(SubjectId)
  create table #tt (c varchar(4000) not null, s numeric(12,2) not null, level int not null, id int identity, primary key clustered(level, id))
  fetch next from cc into @id,@PassportNumber,@sum,@CertificateCheckingId
  while @@FETCH_STATUS=0
  begin 
    --select @id,@PassportNumber,@sum,@CertificateCheckingId
    -- print ''    10.''+right(convert(varchar,SYSDATETIME()),12)       
    
    
    truncate table #t
    insert #t
    select
    c.SubjectCode,c.mark  
    from 
    dbo.vw_Examcertificate b with(nolock)
    join prn.CertificatesMarks c with(nolock) on b.Id=c.CertificateFK   
    join #CommonNationalExamCertificateSubjectCheck d on  c.SubjectCode=d.SubjectId and d.CertificateCheckingId=@CertificateCheckingId
    where b.[YEAR] between  @yearFrom and @yearTo and b.PassportNumber=@PassportNumber 
    --print ''    20.''+right(convert(varchar,SYSDATETIME()),12)
    
    --select * from #t
  if not exists(select * from 
    #CommonNationalExamCertificateSubjectCheck b      
    left join #t c on c.SubjectId=b.SubjectId
    where b.CertificateCheckingId=@CertificateCheckingId and c.SubjectId is null )
  begin
    --select @id,@PassportNumber,@sum,@CertificateCheckingId
    truncate table #tt
    declare @i int, @l int
    select @i = min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where CertificateCheckingId=@CertificateCheckingId
    set @l = 0
    insert #tt(c, s, level) values('''', 0, 0)      
    while @i is not null 
    begin
      insert #tt(c, s, level) 
      select
      --'''',
       t.c+case t.c when '''' then '''' else ''+'' end+cast(x.mark as varchar(16)),
       t.s+x.mark, @l+1
      from (select * from #tt where level = @l) t
        cross join
        (select * from #t where SubjectId = @i) x
  
      set @i = (select min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where SubjectId>@i and CertificateCheckingId=@CertificateCheckingId)
      set @l = @l+1
    end
    --print ''    30.''+right(convert(varchar,SYSDATETIME()),12)  
    --select * from #tt where level = @l
    
     select @maxsum=max(s) from #tt where level = @l  
     if not exists(select * from @t where id=@id)
     begin
      insert @t values(@id)         
      update CommonNationalExamCertificateSumCheck set sum=@maxsum 
      where id=@id 
     end  
     else
     begin
     update CommonNationalExamCertificateSumCheck set sum=@maxsum 
      where id=@id and sum<@maxsum  
     end
  end
  else
  begin 
    if not exists(select * from @t where id=@id)
      update CommonNationalExamCertificateSumCheck set sum=-1 where id=@id
  end
       
    fetch next from cc into @id,@PassportNumber,@sum,@CertificateCheckingId
  end
close cc
deallocate cc 
drop table #CommonNationalExamCertificateSumCheck 
drop table #t
drop table #tt
       select * from CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       '
       
     --PrepareCommonNationalExamCertificateCheckByFIO    



        SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)
--set @chooseDbText=@chooseDbText+

--'
--drop table #CommonNationalExamCertificateCheck
--drop table #CommonNationalExamCertificateSubjectCheck
--select * into #CommonNationalExamCertificateCheck   from #CommonNationalExamCertificateCheck
--select * into #CommonNationalExamCertificateSubjectCheck   from #CommonNationalExamCertificateSubjectCheck
--'
set @declareCommandText=@chooseDbText+replace(@declareCommandText,'<@BatchId>',@BatchId)
EXEC sp_executesql @declareCommandText
return 

end
GO
/****** Object:  StoredProcedure [dbo].[PrepareCommonNationalExamCertificateCheck]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
   Для работы  процедуры  требуются  следующие 
   временные таблицы:
   create table #CommonNationalExamCertificateCheck
    ( 
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , CertificateNumber nvarchar(255)
    , LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , Index bigint
    )
  create table #CommonNationalExamCertificateSubjectCheck
    (
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , SubjectCode nvarchar(255)
    , Mark numeric(5,1)
    )
*/

-- exec dbo.PrepareCommonNationalExamCertificateCheck 
-- ================================================
-- Подготовка пакетов для проверки сертификатов ЕГЭ.
-- v.1.0: Created by Sedov A.G. 22.05.2008
-- v.1.1: Modified by Fomin Dmitriy 31.05.2008
-- Удаление старых данных по пакету, убрано поле HasAppeal.
-- v.1.2: Modified by Sedov Anton 03.06.2008
-- Удалёно использование параметра CertificateCheckingId
-- v.1.3: Modified by Sedov Anton 17.06.2008
-- Добавлена проверка пакетов
-- v.1.4: Modified by Sedov Anton 18.06.2008
-- Выполнена оптимизация  процедуры
-- v.1.5: Modified by Sedov Anton 04.07.2008
-- Добавлено поле DenyNewCertificateNumber
-- и NewСertificateNumber в таблицы проверок
-- сертификатов и аннулированных сертифкатов 
-- соответствено. 
-- v.1.6: Modified by Sedov Anton 09.07.2008
-- Исправлена логика динамического выбора БД
-- таблицы котрой  используются для получения
-- данных о сертификатах
-- v.1.7: Modified by Sedov Anton 28.07.2008
-- Добавлен параметр Index во временную таблицу
-- сертификатов.
-- ================================================
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheck] 
  @batchId bigint
as
begin

  declare
    @chooseDbText nvarchar(max)
    , @baseName nvarchar(255
    )
    , @declareCommandText nvarchar(max)
    , @executeCommandText nvarchar(max)
    , @searchCommandText nvarchar(max)
    , @searchCommandText2 nvarchar(max)
  
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1)

  set @declareCommandText = 
    N' 
    if (select count(*) from #CommonNationalExamCertificateCheck)=0
    begin
     raiserror(''CommonNationalExamCertificateCheck is empty'',16,1)
     return
    end
 
    declare 
      @yearFrom int
      , @yearTo int  

    declare @certificate_subject_correctness table
      (
      CertificateId uniqueidentifier
      , CertificateSubjectId int
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectId uniqueidentifier
      , BatchId bigint
      , CertificateCheckingId uniqueidentifier
      , Year int
      )
    
    declare @certificate_correctness table
      (
      CertificateNumber nvarchar(255)
      , CertificateId uniqueidentifier
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , SourceLastName nvarchar(255)
      , SourceFirstName nvarchar(255)
      , SourcePatronymicName nvarchar(255)
      , IsCertificateCorrect bit
      , BatchId bigint
      , IsDeny bit
      , DenyComment ntext 
      , DenyNewCertificateNumber nvarchar(255)
      , CertificateYear int
      , CertificateCheckingId uniqueidentifier
      , [Index] bigint
      , TypographicNumber nvarchar(255)
      , RegionId int
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
      '
    
  set @searchCommandText = 
    N'
    select  
      @yearFrom = actuality_years.YearFrom
      , @yearTo = actuality_years.YearTo
    from
      dbo.GetCommonNationalExamCertificateActuality() actuality_years
    select @yearFrom,@yearTo
    insert @certificate_correctness
      (
      CertificateNumber
      , CertificateId
      , LastName
      , FirstName
      , PatronymicName
      , SourceLastName
      , SourceFirstName
      , SourcePatronymicName
      , IsCertificateCorrect
      , BatchId
      , IsDeny
      , DenyComment
      , DenyNewCertificateNumber
      , CertificateYear
      , CertificateCheckingId
      , [Index]
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    select 
      exam_certificate_check.CertificateNumber
      , exam_certificate.Id
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName 
      , case 
        when exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_check.LastName collate cyrillic_general_ci_ai
          and exam_certificate.FirstName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.FirstName, exam_certificate.FirstName) collate cyrillic_general_ci_ai
          and exam_certificate.PatronymicName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.PatronymicName, exam_certificate.PatronymicName) collate cyrillic_general_ci_ai
          and exam_certificate_deny.UseYear is null
          then 1
        else 0
      end 
      , <BatchIdentifier>
      , case
        when isnull(exam_certificate_deny.UseYear,0) <> 0 then 1
        else 0
      end
      , exam_certificate_deny.Reason
      , null NewCertificateNumber
      , exam_certificate.[Year]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.[Index]
      , exam_certificate.TypographicNumber
      , exam_certificate.RegionId
      , exam_certificate.PassportSeria
      , exam_certificate.PassportNumber
    from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
        left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
          on  exam_certificate.Number collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
            and exam_certificate.[Year] between @yearFrom and @yearTo
            and exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_check.LastName
            and exam_certificate.FirstName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.FirstName, exam_certificate.FirstName)
            and exam_certificate.PatronymicName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.PatronymicName, exam_certificate.PatronymicName)
        left outer join <dataDbName>.prn.CancelledCertificates exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateFK = exam_certificate.id
            and exam_certificate_deny.[UseYear] between @yearFrom and @yearTo
    where
      exam_certificate_check.BatchId = <BatchIdentifier>          
    '

    set @searchCommandText2 = '
    insert @certificate_subject_correctness
      (
      CertificateId
      , CertificateSubjectId
      , Mark 
      , HasAppeal
      , IsSubjectCorrect
      , SourceMark 
      , SourceCertificateSubjectId  
      , BatchId
      , CertificateCheckingId
      , Year
      )
    select 
      coalesce(certificate_correctness.[CertificateId], check_certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateFK])
      , isnull(exam_certificate_subject.SubjectCode, subject.Id)
      , exam_certificate_subject_check.Mark
      , exam_certificate_subject.HasAppeal
      , case
        when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
          then 1
        else 0
      end 
      , exam_certificate_subject.Mark
      , exam_certificate_subject.CertificateMarkID
      , <BatchIdentifier>
      , coalesce(certificate_correctness.[CertificateCheckingId], check_certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
      , coalesce(exam_certificate_subject.useyear, check_certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[UseYear]
        full outer join #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode 
            inner join @certificate_correctness check_certificate_correctness
              on check_certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
          on exam_certificate_subject.SubjectCode = subject.Id
            and certificate_correctness.CertificateCheckingId = check_certificate_correctness.CertificateCheckingId

    '

  
  set @executeCommandText = 
    N'
    delete exam_certificate_check
    from dbo.CommonNationalExamCertificateCheck exam_certificate_check
    where exam_certificate_check.BatchId = <BatchIdentifier>

    declare @certificateCheckId table
      (
      CertificateCheckId bigint
      , CheckingCertificateId uniqueidentifier
      )
    
    insert dbo.CommonNationalExamCertificateCheck
      (
      CertificateCheckingId
      , BatchId
      , CertificateNumber
      , LastName
      , FirstName
      , PatronymicName
      , IsCorrect
      , SourceCertificateIdGuid
      , SourceLastName
      , SourceFirstName
      , SourcePatronymicName
      , IsDeny
      , DenyComment
      , DenyNewCertificateNumber
      , Year
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    output inserted.Id, inserted.CertificateCheckingId 
      into @certificateCheckId 
    select
      certificate_correctness.CertificateCheckingId
      , <BatchIdentifier>
      , isnull(certificate_correctness.CertificateNumber,'''')
      , isnull(certificate_correctness.LastName,'''')
      , certificate_correctness.FirstName
      , certificate_correctness.PatronymicName
      , case 
        when (certificate_correctness.IsCertificateCorrect = 1 
          and not exists(select 1 
            from @certificate_subject_correctness subject_correctness
            where subject_correctness.IsSubjectCorrect = 0 
              and certificate_correctness.CertificateId = subject_correctness.CertificateId
              and certificate_correctness.CertificateCheckingId = subject_correctness.CertificateCheckingId)) 
          then 1
        else 0
      end 
      , certificate_correctness.CertificateId
      , certificate_correctness.SourceLastName
      , certificate_correctness.SourceFirstName
      , certificate_correctness.SourcePatronymicName
      , certificate_correctness.IsDeny
      , certificate_correctness.DenyComment
      , certificate_correctness.DenyNewCertificateNumber
      , certificate_correctness.CertificateYear
      , certificate_correctness.TypographicNumber
      , certificate_correctness.RegionId
      , certificate_correctness.PassportSeria
      , certificate_correctness.PassportNumber
    from @certificate_correctness certificate_correctness 
        
    delete exam_certificate_subject_check
    from dbo.CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check
    where exam_certificate_subject_check.BatchId = <BatchIdentifier>

    select 
      <BatchIdentifier>
      , certificate_check_id.CertificateCheckId
      , subject_correctness.CertificateSubjectId 
      , subject_correctness.Mark
      , subject_correctness.IsSubjectCorrect 
      , subject_correctness.SourceCertificateSubjectId
      , subject_correctness.SourceMark
      , subject_correctness.HasAppeal
      , subject_correctness.Year
    from 
      @certificate_subject_correctness subject_correctness
        inner join @certificateCheckId certificate_check_id
          on certificate_check_id.CheckingCertificateId = subject_correctness.CertificateCheckingId 

    insert dbo.CommonNationalExamCertificateSubjectCheck
      (
      BatchId
      , CheckId
      , SubjectId
      , Mark
      , IsCorrect
      , SourceCertificateSubjectIdGuid
      , SourceMark
      , SourceHasAppeal
      , Year
      ) 
    select 
      <BatchIdentifier>
      , certificate_check_id.CertificateCheckId
      , subject_correctness.CertificateSubjectId 
      , subject_correctness.Mark
      , subject_correctness.IsSubjectCorrect 
      , subject_correctness.SourceCertificateSubjectId
      , subject_correctness.SourceMark
      , subject_correctness.HasAppeal
      , subject_correctness.Year
    from 
      @certificate_subject_correctness subject_correctness
        inner join @certificateCheckId certificate_check_id
          on certificate_check_id.CheckingCertificateId = subject_correctness.CertificateCheckingId 
                    
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
        
    '

  set @searchCommandText = replace(replace(@searchCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
  set @searchCommandText2 = replace(replace(@searchCommandText2, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)     
  set @executeCommandText = replace(replace(@executeCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)

--print '---begin: PCNECC---\n'
--print @chooseDbText
--print @declareCommandText
--print @searchCommandText
--print @searchCommandText2
--print @executeCommandText
--print '\n---end: PCNECC---'

  declare @commandText nvarchar(max)

  set @commandText=@chooseDbText +@declareCommandText + @searchCommandText + @searchCommandText2 + @executeCommandText
  
  exec sp_executesql @commandText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateForm]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
   Для работы процедуры требуются следующие 
   временные таблицы:
   create table #CommonNationalExamCertificateFormNumberRange
    ( 
    NumberFrom nvarchar(255)
    , NumberTo nvarchar(255)
    )
  create table #CommonNationalExamCertificateForm
    (
    CheckingFormId uniqueidentifier
    , Number nvarchar(255)
    , CertificateNumber nvarchar(255)
    , LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , IsBlank bit
    , IsDuplicate bit
    , IsDeny bit
    , IsValid bit
    )
  create table #CommonNationalExamCertificateSubjectForm
    (
    CheckingFormId uniqueidentifier
    , SubjectCode nvarchar(255)
    , Mark numeric(5,1)
    )
*/

-- exec dbo.UpdateCommonNationalExamCertificateForm
-- ================================================
-- Сохранить изменения бланков свидетельств ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 07.06.2008
-- v.1.1: Modified by Fomin Dmitriy 16.06.2008
-- Добавлено вычисление результатов.
-- v.1.2: Modified by Sedov Anton 16.06.2008
-- Добавлен выбор базы  на кторой  будет 
-- выполняться ХП.
-- ================================================
alter procedure [dbo].[UpdateCommonNationalExamCertificateForm]
  @regionCode nvarchar(50)
as
begin
  declare
    @commandText nvarchar(4000)
    , @declareCommandText nvarchar(4000)
    , @chooseDbText nvarchar(4000)
    , @insertCommandText nvarchar(4000)
    , @insertSubjectCommandText nvarchar(4000)
    , @alterCommandText nvarchar(4000)
    , @deleteOldCertificateForm nvarchar(4000)
    , @deleteOldSubjectForm nvarchar(4000)
    , @baseName nvarchar(255)

  set @chooseDbText = ''
  set @declareCommandText = ''
  set @alterCommandText = ''

  set @chooseDbText = 'use <databaseName> '
  set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1)
    

  set @declareCommandText = 
    N'
    declare
      @regionId int
      , @year int
      , @partition bigint
      , @updateId uniqueidentifier
      , @date datetime
      , @innerCommandText nvarchar(4000)

    set @innerCommandText = ''''
  
    declare @formId table
      (
      FormId bigint
      , Number nvarchar(255)
      )

    declare @certificateForm table
      (
      CheckingFormId uniqueidentifier
      , Number nvarchar(255)
      , CertificateId bigint
      , CertificateYear int
      , CertificateNumber nvarchar(255)
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      , IsBlank bit
      , IsDeny bit
      , IsDuplicate bit
      , IsValid bit
      , IsCertificateExist bit
      , IsCertificateDeny bit
      )
    '
  
  set @commandText =
    N'
    exec dbo.RefreshCommonNationalExamCertificateFormPartition

    select @regionId = region.Id
    from dbo.Region region with (nolock)
    where region.Code = ''<regionCode>''

    set @year = Year(GetDate())
    set @partition = dbo.GetCommonNationalExamCertificateFormPartition(@regionId, @year)
    set @updateId = NewId()
    set @date = GetDate();

    insert into @certificateForm
    select
      form.CheckingFormId
      , form.Number
      , [certificate].Id
      , [certificate].[Year]
      , form.CertificateNumber
      , form.LastName
      , form.FirstName
      , form.PatronymicName
      , form.PassportSeria
      , form.PassportNumber
      , form.IsBlank
      , form.IsDeny
      , form.IsDuplicate
      , cast(case
        when not [certificate].Id is null
          and form.LastName = certificate.LastName
          and form.FirstName = certificate.FirstName
          and form.PatronymicName = certificate.PatronymicName
          and certificate.InternalPassportSeria = dbo.GetInternalPassportSeria(form.PassportSeria)
          and form.PassportNumber = certificate.PassportNumber
          and certificate_deny.Id is null then 1
        else 0
      end as bit) IsValid
      , cast(case
        when not [certificate].Id is null then 1
        else 0
        end as bit) IsCertificateExist
      , cast(case
        when not certificate_deny.Id is null then 1
        else 0
      end as bit) IsCertificateDeny
    from #CommonNationalExamCertificateForm form
      left outer join <dataDbName>.dbo.CommonNationalExamCertificate [certificate] with (nolock)
        on [certificate].Number = form.CertificateNumber
      left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny certificate_deny with (nolock)
        on certificate_deny.CertificateNumber = form.CertificateNumber

    if exists(select 1
        from #CommonNationalExamCertificateFormNumberRange new_range
          full outer join dbo.CommonNationalExamCertificateFormNumberRange old_range with (nolock)
            on old_range.[Year] = @year
              and old_range.RegionId = @regionId
              and new_range.NumberFrom = old_range.NumberFrom
              and new_range.NumberTo = old_range.NumberTo
          where 
            new_range.NumberFrom is null
            or old_range.NumberFrom is null
            or new_range.NumberTo is null
            or old_range.NumberTo is null)
      begin
        delete from dbo.CommonNationalExamCertificateFormNumberRange
        where [Year] = @year
          and RegionId = @regionId

        insert into dbo.CommonNationalExamCertificateFormNumberRange
          (
          CreateDate
          , UpdateDate
          , UpdateId
          , [Year]
          , RegionId
          , NumberFrom
          , NumberTo
          )
        select
          @date
          , @date
          , @updateId
          , @year
          , @regionId
          , [range].NumberFrom
          , [range].NumberTo
        from #CommonNationalExamCertificateFormNumberRange [range]
      end
      '

    set @insertCommandText =
      N'
      if not object_id(''dbo.UpdatingCommonNationalExamCertificateForm<regionCode>'') is null
        drop table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>

      create table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
        (
        Id bigint identity(1,1) not null
        , Number nvarchar(255) not null 
        , CreateDate datetime not null
        , UpdateDate datetime not null
        , UpdateId uniqueidentifier not null
        , [Year] int not null
        , RegionId int not null
        , [Partition] bigint not null
        , CertificateNumber nvarchar(255) not null
        , LastName nvarchar(255) 
        , FirstName nvarchar(255)
        , PatronymicName nvarchar(255)
        , PassportSeria nvarchar(255)
        , PassportNumber nvarchar(255)
        , IsBlank bit not null
        , IsDeny bit not null
        , IsDuplicate bit not null
        , IsValid bit
        , IsCertificateExist bit
        , IsCertificateDeny bit 
        )

      insert dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
        (
        Number
        , CreateDate
        , UpdateDate
        , UpdateId
        , [Year]
        , RegionId
        , [Partition]
        , CertificateNumber
        , LastName
        , FirstName
        , PatronymicName
        , PassportSeria
        , PassportNumber
        , IsBlank
        , IsDeny
        , IsDuplicate
        , IsValid
        , IsCertificateExist
        , IsCertificateDeny
        )
      output inserted.Id, inserted.Number into @formId
      select
        form.Number
        , @date
        , @date
        , @updateId
        , @year
        , @regionId
        , @partition
        , form.CertificateNumber
        , form.LastName
        , form.FirstName
        , form.PatronymicName
        , form.PassportSeria
        , form.PassportNumber
        , form.IsBlank
        , form.IsDeny
        , form.IsDuplicate
        , form.IsValid
        , form.IsCertificateExist
        , form.IsCertificateDeny
      from @certificateForm form
      where form.IsValid = 0

      insert dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
        (
        Number
        , CreateDate
        , UpdateDate
        , UpdateId
        , [Year]
        , RegionId
        , [Partition]
        , CertificateNumber
        , LastName
        , FirstName
        , PatronymicName
        , PassportSeria
        , PassportNumber
        , IsBlank
        , IsDeny
        , IsDuplicate
        , IsValid
        , IsCertificateExist
        , IsCertificateDeny
        )
      output inserted.Id, inserted.Number into @formId
      select
        form.Number
        , @date
        , @date
        , @updateId
        , @year
        , @regionId
        , @partition
        , form.CertificateNumber
        , form.LastName
        , form.FirstName
        , form.PatronymicName
        , form.PassportSeria
        , form.PassportNumber
        , form.IsBlank
        , form.IsDeny
        , form.IsDuplicate
        , cast(case
          when not exists(select 1
              from <dataDbName>.dbo.CommonNationalExamCertificateSubject certificate_subject with (nolock)
                inner join @certificateForm inner_form
                  on inner_form.CheckingFormId = form.CheckingFormId
                    and certificate_subject.CertificateId = form.CertificateId
                    and certificate_subject.[Year] = form.CertificateYear
                inner join dbo.Subject [subject] with (nolock)
                  on subject.Id = certificate_subject.SubjectId
                full outer join #CommonNationalExamCertificateSubjectForm form_subject with (nolock)
                  on form_subject.SubjectCode = [subject].Code
                    and form_subject.Mark = certificate_subject.Mark
              where 
                (form_subject.CheckingFormId is null
                  or form_subject.CheckingFormId = form.CheckingFormId)
                and (form_subject.CheckingFormId is null
                  or certificate_subject.Id is null)) then 1
          else 0
        end as bit) IsValid
        , form.IsCertificateExist
        , form.IsCertificateDeny
      from @certificateForm form
      where form.IsValid = 1 '

    set @deleteOldCertificateForm = 
      N'
      if not object_id(''dbo.OldCommonNationalExamCertificateForm<regionCode>'') is null
        drop table dbo.OldCommonNationalExamCertificateForm<regionCode>

      create table dbo.OldCommonNationalExamCertificateForm<regionCode>
        (
        Id bigint identity(1,1) not null
        , Number nvarchar(255) not null 
        , CreateDate datetime not null
        , UpdateDate datetime not null
        , UpdateId uniqueidentifier not null
        , [Year] int not null
        , RegionId int not null
        , [Partition] bigint not null
        , CertificateNumber nvarchar(255) not null
        , LastName nvarchar(255) 
        , FirstName nvarchar(255)
        , PatronymicName nvarchar(255)
        , PassportSeria nvarchar(255)
        , PassportNumber nvarchar(255)
        , IsBlank bit not null
        , IsDeny bit not null
        , IsDuplicate bit not null
        , IsValid bit
        , IsCertificateExist bit
        , IsCertificateDeny bit 
        )
      
      set @innerCommandText = 
        N'' 
        alter table dbo.OldCommonNationalExamCertificateForm<regionCode> 
        with check
        add constraint OldCertificateFromCK<regionCode>
          check ([Partition] = <inner_partition>)
        ''
      set @innerCommandText = replace(@innerCommandText, ''<inner_partition>'', @partition)

      exec (@innerCommandText)

      alter table dbo.OldCommonNationalExamCertificateForm<regionCode>
      add constraint OldCertificateFormPK<regionCode>
        primary key clustered ([Partition], CertificateNumber)

      insert into dbo.OldCommonNationalExamCertificateForm<regionCode>
      select
        Number 
        , CreateDate 
        , UpdateDate 
        , UpdateId 
        , [Year]
        , RegionId
        , [Partition] 
        , CertificateNumber 
        , LastName 
        , FirstName 
        , PatronymicName 
        , PassportSeria
        , PassportNumber 
        , IsBlank 
        , IsDeny 
        , IsDuplicate 
        , IsValid 
        , IsCertificateExist 
        , IsCertificateDeny 
      from
        dbo.CommonNationalExamCertificateForm  
      where 
        [Partition] = @partition

      delete cne_certificate_form 
      from dbo.CommonNationalExamCertificateForm cne_certificate_form
      where cne_certificate_form.[Partition] = @partition
      '

    set @deleteOldSubjectForm = 
      N'
      if not object_id(''dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>'') is null
        drop table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>

      create table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
        (
        Id bigint identity(1,1) not null
        , [Year] int not null
        , RegionId int not null
        , [Partition] bigint not null
        , FormId bigint
        , SubjectId int not null
        , Mark numeric(5,1)
        )
      
      set @innerCommandText = 
        N''
        alter table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
        with check
        add constraint OldCertificateSubjectFormCK<regionCode>
          check ([Partition] = <inner_partition>)
        ''
      set @innerCommandText = replace(@innerCommandText, ''<inner_partition>'', @partition)

      exec (@innerCommandText)

      alter table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
      with check
      add constraint OldCertificateSubjectFormPK<regionCode>
        primary key clustered ([Partition], SubjectId)  

      insert dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
      select
        [Year]
        , RegionId
        , [Partition] 
        , FormId 
        , SubjectId 
        , Mark 
      from  dbo.CommonNationalExamCertificateSubjectForm
      where [Partition] = @partition
      
      delete cne_subject_form 
      from dbo.CommonNationalExamCertificateSubjectForm cne_subject_form
      where cne_subject_form.[Partition] = @partition
      '

    
    set @alterCommandText = 
      N'  
      set @innerCommandText = 
        N'' 
        alter table dbo.UpdatingCommonNationalExamCertificateForm<regionCode> 
        with check
        add constraint UpdatingCertificateFromCK<regionCode>
          check ([Partition] = <inner_partition>)
        ''
      set @innerCommandText = replace(@innerCommandText, ''<inner_partition>'', @partition)

      exec (@innerCommandText)

      alter table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
      add constraint UpdatingCertificateFormPK<regionCode>
        primary key clustered ([Partition], CertificateNumber)
      '

    set @insertSubjectCommandText = 
      N'
      if not object_id(''dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>'') is null
        drop table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>

      create table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
        (
        Id bigint identity(1,1) not null
        , Year int not null
        , RegionId int not null
        , [Partition] bigint not null
        , FormId bigint
        , SubjectId int not null
        , Mark numeric(5,1)
        )

      insert dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
        (
        [Year]
        , RegionId
        , [Partition]
        , FormId
        , SubjectId
        , Mark
        )
      select
        @year
        , @regionId
        , @partition
        , form_id.FormId
        , subject.Id
        , form_subject.Mark
      from #CommonNationalExamCertificateSubjectForm form_subject
        inner join #CommonNationalExamCertificateForm form
          inner join @formId form_id
            on form_id.Number = form.Number
          on form.CheckingFormId = form_subject.CheckingFormId
        left outer join dbo.Subject [subject] with (nolock)
          on subject.Code = form_subject.SubjectCode

      set @innerCommandText = 
        N''
        alter table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
        with check
        add constraint UpdatingCertificateSubjectFormCK<regionCode>
          check ([Partition] = <inner_partition>)
        ''
      set @innerCommandText = replace(replace(@innerCommandText, ''<inner_partition>'', @partition),
          ''<regionCode>'', @regionId)

      exec (@innerCommandText)

      alter table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
      with check
      add constraint UpdatingCertificateSubjectFormPK<regionCode>
        primary key clustered ([Partition], SubjectId)

      -- перенос  данных
      alter table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
      switch to dbo.CommonNationalExamCertificateForm
        partition $partition.CommonNationalExamCertificateFormPartitionFunction(@partition)

      alter table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
      switch to dbo.CommonNationalExamCertificateSubjectForm
        partition $partition.CommonNationalExamCertificateSubjectFormPartitionFunction(@partition)
      
      -- очистка
      drop table dbo.OldCommonNationalExamCertificateForm<regionCode>
      drop table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>

      drop table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
      drop table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>   
      '

  set @commandText = replace(replace(@commandText, '<regionCode>', 
      replace(@regionCode, '''', '''''')), '<dataDbName>', @baseName)
  set @insertCommandText = replace(replace(@insertCommandText, '<regionCode>', 
      replace(@regionCode, '''', '''''')), '<dataDbName>', @baseName)
  set @deleteOldCertificateForm = replace(@deleteOldCertificateForm, '<regionCode>', replace(@regionCode, '''', ''''''))  
  set @deleteOldSubjectForm = replace(@deleteOldSubjectForm, '<regionCode>', replace(@regionCode, '''', ''''''))
  set @alterCommandText = replace(@alterCommandText, '<regionCode>', replace(@regionCode, '''', ''''''))
  set @insertSubjectCommandText = replace(@insertSubjectCommandText, '<regionCode>', replace(@regionCode, '''', ''''''))

  exec (@chooseDbText + @declareCommandText + @commandText + @insertCommandText +
      @deleteOldCertificateForm + @deleteOldSubjectForm + @alterCommandText + @insertSubjectCommandText)
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[usp_cne_StartCheckBatch]    Script Date: 06/13/2013 18:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[usp_cne_StartCheckBatch]
@batchId bigint
as
set nocount on
  declare
    @chooseDbText nvarchar(max), @command nvarchar(max)
  
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
create table #CommonNationalExamCertificateCheck
    ( 
    id int identity(1,1) not null primary key,
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , CertificateNumber nvarchar(255)
    , LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , [Index] bigint
    , ShareCertificateCheckingId uniqueidentifier
    )
CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateCheck_1] ON [dbo].[#CommonNationalExamCertificateCheck] 
(
  [CertificateNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateCheck_2] ON [dbo].[#CommonNationalExamCertificateCheck] 
(
  [BatchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateCheck_3] ON [dbo].[#CommonNationalExamCertificateCheck] 
(
  [PassportSeria] ASC 
)
include ([PassportNumber])
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
      
  create table #CommonNationalExamCertificateSubjectCheck
    (
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , SubjectCode nvarchar(255)
    , Mark numeric(5,1)
    , ShareCertificateCheckingId uniqueidentifier
    , SubjectId int
    )
CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateSubjectCheck_1SubjectId] ON [dbo].[#CommonNationalExamCertificateSubjectCheck]
(
  [SubjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]    
declare @typeStr varchar(200),@subjectStartStr varchar(200),@setStr varchar(200)



set @command='
declare cur cursor local fast_forward for 
  select id,Batch from dbo.CommonNationalExamCertificateCheckBatch 
  where <typeStr> and id=@batchId
open cur
declare @id bigint,@batch nvarchar(max),@certificateCheckingId uniqueidentifier,@index int,@subjectNum int,@row nvarchar(4000),@subjectStart int
declare @batches table (val nvarchar(max))
declare @tab table (id int,val nvarchar(300))
select @subjectNum=<@subjectNum>,@subjectStart=<subjectStartStr>
fetch next from cur into @id,@batch
while @@FETCH_STATUS=0
begin     
  declare cc cursor local fast_forward for select val from [dbo].[ufn_ut_SplitFromStringWithId](REPLACE(REPLACE(@batch, CHAR(13) + CHAR(10), CHAR(13)), CHAR(10), CHAR(13)),CHAR(13)) order by id  
  open cc
  select @index=1
  fetch next from cc into @row
  while @@FETCH_STATUS=0
  begin     
    if len(isnull(@row,''''))=0
      goto endcur1    
    delete from @tab
    select @certificateCheckingId=NEWID()
    /*insert #CommonNationalExamCertificateCheck(ShareCertificateCheckingId,CertificateCheckingId,BatchId, [Index]) values(@certificateCheckingId,@certificateCheckingId,@id,@index)
    insert @tab( id,val)  select id,val from  [dbo].[ufn_ut_SplitFromStringWithId](@row,''%'') order by id
    --select * from @tab
  
  
    update #CommonNationalExamCertificateCheck 
    set 
    <setStr>
    from @tab
    where CertificateCheckingId=@certificateCheckingId
  */
      
   insert @tab( id,val)  select id,val from  [dbo].[ufn_ut_SplitFromStringWithId](@row,''%'') order by id
    
    declare @PassportSeria varchar(100)
    declare @PassportNumber varchar(100)
    
    declare @CertificateNumber varchar(100)
    declare @LastName varchar(100)
    declare @FirstName varchar(100)
    declare @PatronymicName varchar(100)
    
    select @PassportSeria = val from @tab where id=1
    set @CertificateNumber = @PassportSeria
    
    select @PassportNumber = val from @tab where id=2
    select @FirstName = val from @tab where id=3
    select @PatronymicName = val from @tab where id=4
  
    insert #CommonNationalExamCertificateCheck
      (ShareCertificateCheckingId,CertificateCheckingId,BatchId, [Index],
      CertificateNumber,
      LastName,
      FirstName,
      PatronymicName,
      PassportSeria,
      PassportNumber
      ) 
    
    values(@certificateCheckingId,@certificateCheckingId,@id,@index,
     @CertificateNumber,
       @PassportNumber,
       @FirstName,
       @PatronymicName,
       @PassportSeria,  
       @PassportNumber
    )
  
  
    insert #CommonNationalExamCertificateSubjectCheck(ShareCertificateCheckingId,CertificateCheckingId ,BatchId,SubjectCode, Mark)
    select @certificateCheckingId,@certificateCheckingId,@id,b.Code,cast(replace(val,'','',''.'') as numeric(5,1))
    from  @tab a 
    --left  
    join dbo.Subject b on a.id-@subjectStart+1=b.Id
    where a.id between @subjectStart and @subjectNum+@subjectStart-1 and val<>''''
    
    select @index=@index+1
    endcur1:
    fetch next from cc into @row
  end
  close cc
  deallocate cc
  ttt:
  fetch next from cur into @id,@batch
end
close cur
deallocate cur  
--select * from #CommonNationalExamCertificateCheck 
--select * from #CommonNationalExamCertificateSubjectCheck
--delete from #CommonNationalExamCertificateCheck 


'

----------------------------------------------------------------------------------------------    
select  @typeStr= 'ISNULL(type,0)=0'
    ,@subjectStartStr='5'
    ,@setStr='CertificateNumber=(select val from @tab where id=1),
    LastName=(select val from @tab where id=2),
    FirstName=(select val from @tab where id=3),
    PatronymicName=(select val from @tab where id=4)'
declare @commandStr nvarchar(max)       
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
      print '001000'
      
      update #CommonNationalExamCertificateCheck
      set 
        PassportSeria = null,
        PassportNumber = null
      
      exec [dbo].PrepareCommonNationalExamCertificateCheck @batchId
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck
    end
    
----------------------------------------------------------------------------------------------    
select  @typeStr= 'type=1'
    ,@subjectStartStr='2'
    ,@setStr='CertificateNumber=(select val from @tab where id=1)'    
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 11111
     
     update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria = null,
        PassportNumber = null
     
--     raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
      exec [dbo].PrepareCommonNationalExamCertificateCheckByNumber @batchId,1
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=2'
    ,@subjectStartStr='3'
    ,@setStr='PassportSeria=(select val from @tab where id=1),  
    PassportNumber=(select val from @tab where id=2)'   
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 22222
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        CertificateNumber = null
     
--     raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
      exec [dbo].PrepareCommonNationalExamCertificateCheckByPassport @batchId,2
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=3'
    ,@subjectStartStr='3'
    ,@setStr='PassportSeria=(select val from @tab where id=1),  
    PassportNumber=(select val from @tab where id=2)'   
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','15')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 3333
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria=PassportNumber
     
--select * from #CommonNationalExamCertificateCheck
--select  * from #CommonNationalExamCertificateSubjectCheck
      exec [dbo].[PrepareCommonNationalExamCertificateCheckByFIO] @batchId
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
--[usp_cne_StartCheckBatch] 8774
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=4'
    ,@subjectStartStr='3'
    ,@setStr='PassportSeria=(select val from @tab where id=1),  
    PassportNumber=(select val from @tab where id=2)'   
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','15')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 4444
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria=PassportNumber
     
--select * from #CommonNationalExamCertificateCheck
--select  * from #CommonNationalExamCertificateSubjectCheck
      exec [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum] @batchId
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
    
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=5'
    ,@subjectStartStr='2'
    ,@setStr='PassportSeria=(select val from @tab where id=1)'    
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 4444
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria=null,PassportNumber=null

--select * from #CommonNationalExamCertificateCheck
--select  * from #CommonNationalExamCertificateSubjectCheck
      exec [dbo].[PrepareCommonNationalExamCertificateCheckByFioAndSubjects] @batchId
      
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end   
drop table #CommonNationalExamCertificateCheck
drop table #CommonNationalExamCertificateSubjectCheck
--[usp_cne_StartCheckBatch] 8774
 --select getdate() exec dbo.usp_cne_StartCheckBatch @batchId=8351
GO
