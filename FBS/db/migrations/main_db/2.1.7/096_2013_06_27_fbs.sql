insert into Migrations(MigrationVersion, MigrationName) values (96, '096_2013_06_27_fbs.sql')
go

/****** Object:  View [dbo].[Subject]    Script Date: 06/27/2013 17:46:02 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Subject]'))
DROP VIEW [dbo].[Subject]
GO

USE [fbs]
GO

/****** Object:  View [dbo].[Subject]    Script Date: 06/27/2013 17:46:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Subject]
AS
SELECT     a.id, a.code, c.SubjectName AS Name, a.id AS SortIndex, 1 AS IsActive, c.SubjectCode AS SubjectId
FROM         dbo.SubjectMapping AS a INNER JOIN
                      dat.Subjects AS c ON c.SubjectCode = a.id_subject_new

GO


alter proc [dbo].[SearchCommonNationalExamCertificate]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @subjectMarks nvarchar(4000) = null   -- средние оценки по предметам (через запятую, в определенном порядке)
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @typographicNumber nvarchar(255) = null -- типографический номер сертификата 
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
  , @ParticipantID uniqueidentifier = null  
as
begin
  declare 
    @eventCode nvarchar(255)
        , @organizationId bigint
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(4000)
    , @internalPassportSeria nvarchar(255)

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  set @eventParams = 
    isnull(@lastName,'') + '|' 
    + isnull(@firstName,'') + '|' 
    + isnull(@patronymicName,'') + '|' 
    + isnull(@subjectMarks, '') + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') + '|' 
    + isnull(@typographicNumber, '')

  select
    @editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  if @year is not null
    select @yearFrom = @year, @yearTo = @year
  else
    select @yearFrom = 2008
      select @yearTo = Year(GetDate())

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  set @commandText = ''
  if isnull(@typographicNumber, '') <> ''
    set @eventCode = N'CNE_FND_TN'
  else 
    set @eventCode = N'CNE_FND_P'

  declare @CId bigint,@sourceEntityIds nvarchar(4000) 
  declare @Search table 
  ( 
    LastName nvarchar(255) 
    , FirstName nvarchar(255) 
    , PatronymicName nvarchar(255) 
    , CertificateId uniqueidentifier
    , CertificateNumber nvarchar(255) 
    , RegionId int 
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255) 
    , TypographicNumber nvarchar(255) 
    , Year int 
    , ParticipantID uniqueidentifier
  ) 
    
  set @commandText = @commandText +       
    'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year,ParticipantID
    from vw_Examcertificate1 [certificate] 
    where 
    certificate.[Year] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and certificate.LastName collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and certificate.FirstName collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and certificate.PatronymicName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportSeria like @internalPassportSeria and [certificate].id is not null '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.PassportSeria = @internalPassportSeria and [certificate].id is not null '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber and [certificate].id is not null '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber and [certificate].id is not null '
        end
    end
  
  if not @typographicNumber is null
    set @commandText = @commandText +
      ' and certificate.TypographicNumber = @typographicNumber and [certificate].id is not null '
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo                                 
                          
  if @subjectMarks is not null
  begin 
      
/*
  Для потомков и наверное для меня самого на будущее.
  Описание задания
  
  Нужно проверять если оценки полностью совпадают с одним из свидетельств (с номером или без номера), то выдавать это свидетельство. 
  Если есть оценки и с того и другого свидетельства то возвращать пусто
  
  Кроме того тут заложена логика проверки корректности введенных баллов. Если один из баллов введен не правильно, то вовращать пусто.
  Эта проверка должна осуществляться после первого правила (см выше) 
  
  Проблема 
  Так как для некоторых проверок нет номера свидетельства то приходится связываться по участнику. 
  Но если связываться по участнику, то выдаются все свидетества для этого учатника. Проблема была решена как....
  
  Описание логики работы
  Делаем два запроса для свидетельства с номером и без него.
  Запоминаем ИД сертификата и код предмета
  Смотрим в сформированную таблицу если есть повторяющиеся баллы то удаляем те записи где есть это повторение и нет свидетельства
  
  Смотрим есть после этого в таблице есть два разных свидетельства (без номера тоже считается), то вовращаем пустую проверку
    Иначе смотрим есть свидетельство с номером, то удаляем из @Search свидетельство без номера и осуществляем проверку кооректности введеных баллов
                          иначе удаляем из @Search свидетельство с номером и осуществляем проверку кооректности введеных баллов
*/                       
    create table #tt(id int primary key identity(1,1),code int ,CertificateId uniqueidentifier) 
                                    
    insert #tt
    select distinct certificate_subject.SubjectCode,inner_search.CertificateId 
    from [prn].CertificatesMarks certificate_subject with(nolock) 
      join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
          and 1=case when inner_search.CertificateId = certificate_subject.CertificateFK then 1             
                else 0  end 
      join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark                         
    where inner_search.CertificateId is not null 
    union all
    select distinct certificate_subject.SubjectCode,inner_search.CertificateId 
    from [prn].CertificatesMarks certificate_subject with(nolock) 
      join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                 and 1= case when inner_search.ParticipantID = certificate_subject.ParticipantFK then 1 
                else 0 end 
      join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark                         
    where inner_search.CertificateId is null        
            
    if exists(select * from #tt group by code having count(code)>1)
      delete a from #tt a 
        join (select code from #tt group by code having count(code)>1) b on a.code=b.code 
      where CertificateId is null 
                
    if exists(select * from #tt having count(distinct isnull(CertificateId,'22655BE0-C368-4EB8-8835-5E0F7BA807B5'))>1)
    begin     
      delete search 
      from @Search search 
      
      delete #tt
    end     
    
    if exists(select * from #tt where CertificateId is not null)  
    begin               
      delete search 
      from @Search search 
      where CertificateId is null
      
      delete search 
      from @Search search 
      where exists(select 1 
            from [prn].CertificatesMarks certificate_subject with(nolock) 
              inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                  and search.CertificateId = inner_search.CertificateId
                  and inner_search.CertificateId = certificate_subject.CertificateFK
              right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark 
            where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)      
            
    end       
            
    if exists(select * from #tt where CertificateId is null)  
    begin               
      delete search 
      from @Search search 
      where CertificateId is not null
                    
      delete search 
      from @Search search 
      where exists(select 1 
            from [prn].CertificatesMarks certificate_subject with(nolock) 
              inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                  and search.ParticipantID = inner_search.ParticipantID
                  and inner_search.ParticipantID = certificate_subject.ParticipantFK
              right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark 
            where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)            
    end
    
    drop table #tt    
  end

  set @sourceEntityIds = ''
  
  select @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), search.CertificateId) 
  from @Search search 
  
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
    
  if @sourceEntityIds = ''
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

  declare @Search1 table 
  ( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
  )     
  insert @Search1
    select distinct S.CertificateId 
    from @Search S   
  where CertificateId is not null

  declare @CertificateId uniqueidentifier,@pkid int
  while exists(select * from @Search1)
  begin
    select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 
      
  select 
      isnull(cast(S.certificateId as nvarchar(255)),'Нет свидетельства') CertificateId,
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.LastName is not null then 1 
        else 0 
      end IsExist, 
      case 
        when CD.Id >0 then 1 
      end IsDeny,  
      CD.Comment DenyComment, 
      CD.NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when CD.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно' 
                else 'Истек срок' 
      end as [Status], 
            CC.UniqueChecks UniqueChecks, 
      CC.UniqueIHEaFCheck UniqueIHEaFCheck,
      CC.UniqueIHECheck UniqueIHECheck, 
      CC.UniqueIHEFCheck UniqueIHEFCheck, 
      CC.UniqueTSSaFCheck UniqueTSSaFCheck,
      CC.UniqueTSSCheck UniqueTSSCheck, 
      CC.UniqueTSSFCheck UniqueTSSFCheck,
      CC.UniqueRCOICheck UniqueRCOICheck,
      CC.UniqueOUOCheck UniqueOUOCheck, 
      CC.UniqueFounderCheck UniqueFounderCheck,
      CC.UniqueOtherCheck UniqueOtherCheck,
      isnull(C.ParticipantID, S.ParticipantID) ParticipantID
    from 
      @Search S 
            left join dbo.vw_Examcertificate C with (nolock) 
              on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.idGUID  = C.Id 
      left outer join CommonNationalExamCertificateDeny CD with (nolock) 
        on CD.[Year] between @yearFrom and @yearTo 
                and S.CertificateNumber = CD.CertificateNumber 
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed
              on ed.[year] = S.[year]
            
  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null 
      
  return 0
end
go
-- =============================================
-- Получить список проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- v.1.1: Modified by Makarev Andrey 06.05.2008
-- Добавлен параметр @AccountId в sp_executesql
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Добавлены поля IsDeny, DenyComment.
-- v.1.3: Modified by Fomin Dmitriy 02.06.2008
-- Испралена логика: Check -> Request.
-- v.1.4: Modified by Sedov Anton 03.06.2008
-- Добавлен пейджинг
-- Добавлены параметры:
-- @startRowIndex, @maxRowCount, @showCount
-- v.1.5: Modified by Sedov Anton 18.06.2008
-- В результат добавлена выборка данных
-- серии и номера паспорта
-- v.1.6 Modified by Sedov Anton 18.06.2008
-- добавлен параметр расширения запроса
-- @isExtended, при значении 1 возвращаются
-- оценки по экзаменам
-- v.1.7 Modified by Sedov Anton 20.06.2008
-- добавлен параметр расширения запроса
-- @isExtendedbyExam, при 1 получаем
-- список экзаменов в которых участвовал
-- выпускник
-- v.1.8 : Modified by Makarev Andrey 23.06.2008
-- Исправлен пейджинг.
-- v.1.9:  Modified by Sedov Anton 04.07.2008
-- в результат запроса добавлено поле 
-- DenyNewCertificateNumber
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateRequest]
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
  , @isExtended bit = null
  , @isExtendedByExam bit = null
as
begin
  declare 
    @innerBatchId bigint
    , @accountId bigint
    , @commandText nvarchar(max)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @declareCommandText nvarchar(max)
    , @viewSelectCommandText nvarchar(max)
    , @pivotSubjectColumns nvarchar(max)
    , @viewSelectPivot1CommandText nvarchar(max)
    , @viewSelectPivot2CommandText nvarchar(max)
    , @viewCommandText nvarchar(max)
    , @sortColumn nvarchar(20) 
    , @sortAsc bit 

  set @commandText = ''
  set @pivotSubjectColumns = ''
  set @viewSelectPivot1CommandText = ''
  set @viewSelectPivot2CommandText = ''
  set @viewCommandText = ''
  set @viewSelectCommandText = ''
  set @declareCommandText = ''
  set @resultOrder = ''
  set @sortColumn = N'Id'
  set @sortAsc = 1
  
  if @batchId is not null
    set @innerBatchId = dbo.GetInternalId(@batchId)

  --если батч НЕ принадлежит пользователю, который пытается его посмотреть
  --или если смотрит НЕ админ, то не даем посмотреть
  if not exists(select top 1 1
      from dbo.CommonNationalExamCertificateRequestBatch cnecrb with (nolock, fastfirstrow)
        inner join dbo.Account a with (nolock, fastfirstrow)
          on cnecrb.OwnerAccountId = a.[Id]
      where 
        cnecrb.Id = @innerBatchId
        and cnecrb.IsProcess = 0
        and (a.[Login] = @login 
          or exists (select top 1 1 from [Account] as a2
          join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
          join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
          where a2.[Login] = @login)))
    set @innerBatchId = 0

  set @declareCommandText = 
    N'declare @search table 
      (
      Id bigint
      , BatchId bigint
      , CertificateNumber nvarchar(255)
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      , IsExist bit
      , RegionName nvarchar(255)
      , RegionCode nvarchar(10)
      , IsDeny bit
      , DenyComment ntext
      , DenyNewCertificateNumber nvarchar(255)
      , SourceCertificateId nvarchar(255)
      , SourceCertificateYear int
      , TypographicNumber nvarchar(255)
      , [Status] nvarchar(255)
      , ParticipantID uniqueidentifier
      , primary key(id)
      )
    '

  if isnull(@showCount, 0) = 0
    set @commandText = 
      N'select <innerHeader>
        dbo.GetExternalId(cne_certificate_request.Id) [Id]
        , dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
        , cne_certificate_request.SourceCertificateNumber CertificateNumber
        , isnull(cnec.LastName, cne_certificate_request.LastName) LastName
        , isnull(cnec.FirstName, cne_certificate_request.FirstName) FirstName
        , isnull(cnec.PatronymicName, cne_certificate_request.PatronymicName) PatronymicName
        , isnull(cnec.PassportSeria, cne_certificate_request.PassportSeria) PassportSeria
        , isnull(cnec.PassportNumber, cne_certificate_request.PassportNumber) PassportNumber
        , case
          when not cne_certificate_request.ParticipantID is null then 1
          else 0
        end IsExist
        , region.Name RegionName
        , region.Code RegionCode
        , isnull(cne_certificate_request.IsDeny, 0) IsDeny 
        , cne_certificate_request.DenyComment DenyComment
        , cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
        , cne_certificate_request.SourceCertificateIdGuid
        , cne_certificate_request.SourceCertificateYear
        , cne_certificate_request.TypographicNumber
        , case when cne_certificate_request.ParticipantID is null then ''Не найдено'' else 
          case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= 

ed.[ExpireDate] then ''Действительно'' 
          else ''Истек срок'' end end as [Status]
        , cne_certificate_request.ParticipantID
      from 
        dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
            on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
          left outer join dbo.Region region with (nolock)
            on region.[Id] = cne_certificate_request.SourceRegionId
          left join [ExpireDate] as ed with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = ed.[Year] 
          left join vw_Examcertificate cnec with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateIdGuid = cast(cnec.id as nvarchar(255)) 
      where 1 = 1 '
  else
    set @commandText = 
      N'
      select count(*)
      from 
        dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
            on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
          left outer join dbo.Region region with (nolock)
            on region.[Id] = cne_certificate_request.SourceRegionId
      where 1 = 1 ' 

  set @commandText = @commandText +
      '   and cne_certificate_request_batch.[Id] = @innerBatchId 
        and cne_certificate_request_batch.IsProcess = 0 '

  if isnull(@showCount, 0) = 0
  begin

    if @sortColumn = 'Id'
    begin
      set @innerOrder = ' order by Id <orderDirection> '
      set @outerOrder = ' order by Id <orderDirection> '
      set @resultOrder = ' order by Id <orderDirection> '
    end
    else 
    begin
      set @innerOrder = ' order by Id <orderDirection> '
      set @outerOrder = ' order by Id <orderDirection> '
      set @resultOrder = ' order by Id <orderDirection> '
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
      set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
      set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
      set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
      set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
      set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = 'top 10000000'
      set @outerSelectHeader = 'top 10000000'
    end

    set @commandText = replace(replace(replace(
        N'insert into @search ' + 
        N' select <outerHeader> * ' + 
        N' from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
  begin
    if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
    begin
      set @declareCommandText = @declareCommandText +
        N' declare @subjects table  
          ( 
          CertificateId uniqueidentifier 
          , Mark nvarchar(10)
          , HasAppeal bit  
          , SubjectCode nvarchar(255)  
          , HasExam bit
          , primary key(CertificateId, SubjectCode)
          ) 
        '

      set @commandText = @commandText +
        N'insert into @subjects  
        select
          cne_certificate_subject.CertificateFK 
          , case when cne_certificate_subject.[Mark] < mm.[MinimalMark] then ''!'' else '''' 

end + replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
          , cne_certificate_subject.HasAppeal
          , subject.Code
          , 1 
        from  
          [prn].CertificatesMarks cne_certificate_subject
          left join dbo.Subject subject on subject.SubjectId = 

cne_certificate_subject.SubjectCode
          left join [MinimalMark] as mm on cne_certificate_subject.[SubjectCode] = 

mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
        where 
          exists(select 1 
              from @search search
              where cast(cne_certificate_subject.CertificateFK as nvarchar(255)) = 

search.SourceCertificateId
                and cne_certificate_subject.[UseYear] = 

search.SourceCertificateYear)
        ' 
    end
    
    set @viewSelectCommandText = 
      N'select
        search.Id 
        , search.BatchId
        , case when search.SourceCertificateId =''Нет свидетельства'' then ''Нет свидетества'' else search.CertificateNumber end CertificateNumber
        , search.LastName
        , search.FirstName
        , search.PatronymicName
        , search.PassportSeria
        , search.PassportNumber
        , search.IsExist
        , search.RegionName
        , search.RegionCode
        , search.IsDeny 
        , search.DenyComment
        , search.DenyNewCertificateNumber
        , search.TypographicNumber
        , search.SourceCertificateYear
        , search.Status
        , search.ParticipantID
      '

    set @viewCommandText = 
      N' ,unique_cheks.UniqueIHEaFCheck
      from @search search 
      left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on cast(unique_cheks.idGUID as nvarchar(255)) = search.SourceCertificateId '

    if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
    begin 
      declare
        @subjectCode nvarchar(255)
        , @pivotSelect nvarchar(4000)

      set @pivotSelect = ''

      declare subject_cursor cursor fast_forward for
      select s.Code
      from dbo.Subject s with(nolock)
      order by s.id asc 

      open subject_cursor 
      fetch next from subject_cursor into @subjectCode
      while @@fetch_status = 0
        begin
        if len(@pivotSubjectColumns) > 0
          set @pivotSubjectColumns = @pivotSubjectColumns + ','
        set @pivotSubjectColumns = @pivotSubjectColumns + replace('[<code>]', '<code>', 

@subjectCode)
        
        if isnull(@isExtended, 0) = 1
          set @pivotSelect =  
            N'  , mrk_pvt.[<code>] [<code>Mark]  
              , apl_pvt.[<code>] [<code>HasAppeal] '
        if isnull(@isExtendedByExam, 0) = 1
          set @pivotSelect = @pivotSelect + 
            N' 
              , isnull(exam_pvt.[<code>], 0) [<code>HasExam] '
            
        set @pivotSelect = replace(@pivotSelect, '<code>', @subjectCode)

        if len(@viewSelectPivot1CommandText) + len(@pivotSelect) <= 4000
          and @viewSelectPivot2CommandText = ''
          set @viewSelectPivot1CommandText = @viewSelectPivot1CommandText + @pivotSelect
        else
          set @viewSelectPivot2CommandText = @viewSelectPivot2CommandText + @pivotSelect

        fetch next from subject_cursor into @subjectCode
      end
      close subject_cursor
      deallocate subject_cursor
    end

    if isnull(@isExtended, 0) = 1
    begin
      set @viewCommandText = @viewCommandText + 
        N'left outer join (select 
          subjects.CertificateId
          , subjects.Mark 
          , subjects.SubjectCode
          from @subjects subjects) subjects
            pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt
          on search.SourceCertificateId = cast(mrk_pvt.CertificateId as nvarchar(255))
          left outer join (select 
            subjects.CertificateId
            , cast(subjects.HasAppeal as int) HasAppeal 
            , subjects.SubjectCode
            from @subjects subjects) subjects
              pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as 

apl_pvt
            on search.SourceCertificateId = cast(apl_pvt.CertificateId  as nvarchar(255)) '
        set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
    end
          
    if isnull(@isExtendedByExam, 0) = 1
    begin
      set @viewCommandText = @viewCommandText + 
        N'left outer join (select 
          subjects.CertificateId
          , subjects.SubjectCode
          , cast(subjects.HasExam as int) HasExam 
          from @subjects subjects) subjects
            pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
          on search.SourceCertificateId = cast(exam_pvt.CertificateId as nvarchar(255)) '
          
      set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
    end
  end

  set @viewCommandText = @viewCommandText + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
      @viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

  print @commandText

  exec sp_executesql @commandText
    , N'@innerBatchId bigint'
    , @innerBatchId
    
  return 0
end
go

ALTER proc [dbo].[CheckCommonNationalExamCertificateByNumber]
   @number nvarchar(255) = null       -- номер сертификата
  , @checkLastName nvarchar(255) = null   -- фамилия сертифицируемого
  , @checkFirstName nvarchar(255) = null    -- имя сертифицируемого
  , @checkPatronymicName nvarchar(255) = null -- отчетсво сертифицируемого
  , @checkSubjectMarks nvarchar(max) = null -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @checkTypographicNumber nvarchar(20) = null -- типографический номер сертификата
  , @ParticipantID uniqueidentifier = null
as
begin 

  if @checkTypographicNumber is null and @number is null and @ParticipantID is null
  begin
    RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
    return
  end
  
    
  declare 
    @commandText nvarchar(max)
    , @declareCommandText nvarchar(max)
    , @selectCommandText nvarchar(max)
    , @baseName nvarchar(255)
    , @yearFrom int
    , @yearTo int
    , @accountId bigint
        , @organizationId bigint
      , @CId uniqueidentifier
    , @eventCode nvarchar(255)
    , @eventParams nvarchar(4000)
    , @sourceEntityIds nvarchar(4000) 
  
  declare @check_subject table
  (
  SubjectId int
  , Mark nvarchar(10)
  )
  
  create table #certificate_check
  (
    id int primary key identity(1,1)
  , Number nvarchar(255)
  , CheckLastName nvarchar(255)
  , LastName nvarchar(255)
  , LastNameIsCorrect bit
  , CheckFirstName nvarchar(255)
  , FirstName nvarchar(255)
  , FirstNameIsCorrect bit
  , CheckPatronymicName nvarchar(255)
  , PatronymicName nvarchar(255)
  , PatronymicNameIsCorrect bit
  , IsExist bit
  , certificateId uniqueidentifier
  , IsDeny bit
  , DenyComment ntext
  , DenyNewcertificateNumber nvarchar(255)
  , [Year] int
  , PassportSeria nvarchar(255)
  , PassportNumber nvarchar(255)
  , RegionId int
  , RegionName nvarchar(255)
  , TypographicNumber nvarchar(255)
  , ParticipantID uniqueidentifier
  )

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  if isnull(@checkTypographicNumber,'') <> ''
    select @yearFrom = 2009, @yearTo = Year(GetDate()) --2009-год появления типографского номера в БД
  else
    select @yearFrom = 2008, @yearTo = Year(GetDate())

  select
    @accountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  declare @sql nvarchar(max)
  
  set @sql = '
  select  
    [certificate].Number 
    , @CheckLastName CheckLastName
    , [certificate].LastName 
    , case 
      when @CheckLastName is null then 1 
      when [certificate].LastName collate cyrillic_general_ci_ai = @CheckLastName then 1
      else 0
    end LastNameIsCorrect
    , @CheckFirstName CheckFirstName
    , [certificate].FirstName 
    , case 
      when @CheckFirstName is null then 1 
      when [certificate].FirstName collate cyrillic_general_ci_ai = @CheckFirstName then 1
      else 0
    end FirstNameIsCorrect
    , @CheckPatronymicName CheckPatronymicName 
    , [certificate].PatronymicName 
    , case 
      when @CheckPatronymicName is null then 1 
      when [certificate].PatronymicName collate cyrillic_general_ci_ai = @CheckPatronymicName then 1
      else 0
    end PatronymicNameIsCorrect
    , case'
    if @ParticipantID is not null 
      set @sql = @sql + ' when certificate.ParticipantID is not null then 1'
    else
      set @sql = @sql + ' when [certificate].Id is not null then 1 '    
    set @sql = @sql + ' else 0 
    end IsExist
    , [certificate].Id
    , case
      when certificate_deny.UseYear is not null then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Reason
    , null NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , certificate.ParticipantID
  from 
    (select null ''empty'') t 
    left join 
      vw_Examcertificate1 [certificate] on 
        [certificate].[Year] between @yearFrom and @yearTo '
  if @ParticipantID is not null 
    set @sql = @sql + ' and [certificate].ParticipantID = @ParticipantID'   
  if @number is not null 
    set @sql = @sql + ' and case when @number='''' and [certificate].Number is null then 1 when [certificate].Number=@number then 1 else 0 end = 1 '
  if @CheckTypographicNumber is not null 
    set @sql = @sql + ' and [certificate].TypographicNumber=@CheckTypographicNumber'
  
  set @sql = @sql + '     
    left outer join dbo.Region region
      on region.Id = [certificate].RegionId
    left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] between @yearFrom and @yearTo
        and certificate_deny.CertificateFK = [certificate].id'

  insert into #certificate_check    
  exec sp_executesql @sql,N'@checkLastName nvarchar(255),@number nvarchar(255),@checkTypographicNumber nvarchar(20),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier',@checkLastName=@checkLastName,@number = @number,@checkTypographicNumber=@checkTypographicNumber,@checkFirstName=@checkFirstName,@checkPatronymicName=@checkPatronymicName,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID

    set @eventParams = 
    isnull(@number, '') + '|' +
    isnull(@checkLastName, '') + '|' +
    isnull(@checkFirstName, '') + '|' +
    isnull(@checkPatronymicName, '') + '|' +
    isnull(@checkSubjectMarks, '') + '|' +
    isnull(@checkTypographicNumber, '')

  set @sourceEntityIds = '' 
  select 
    @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(100), certificate_check.certificateId) 
  from 
    #certificate_check certificate_check 

  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
  if @sourceEntityIds = '' 
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        #certificate_check S
    where
      S.certificateId is not null
    
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @certificateIdGuid = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
  -------------------------------------------------------------
  
  set @sql = '
  select 
    isnull(cast(certificate_check.certificateId as nvarchar(250)),''Нет свидетельства'' ) certificateId
    ,certificate_check.Number
    , certificate_check.CheckLastName
    , certificate_check.LastName
    , certificate_check.LastNameIsCorrect
    , certificate_check.CheckFirstName
    , certificate_check.FirstName
    , certificate_check.FirstNameIsCorrect
    , certificate_check.CheckPatronymicName
    , certificate_check.PatronymicName
    , certificate_check.PatronymicNameIsCorrect
    , certificate_check.IsExist
    , [subject].Id SubjectId
    , [subject].Name SubjectName
    , case when check_subject.CheckSubjectMark < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),''.'','','') CheckSubjectMark
    , case when check_subject.SubjectMark < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),''.'','','') SubjectMark
    , isnull(check_subject.SubjectMarkIsCorrect, 0) SubjectMarkIsCorrect
    , check_subject.HasAppeal
    , certificate_check.IsDeny
    , certificate_check.DenyComment
    , certificate_check.DenyNewcertificateNumber
    , certificate_check.PassportSeria
    , certificate_check.PassportNumber
    , certificate_check.RegionId
    , certificate_check.RegionName
    , certificate_check.[Year]
    , certificate_check.TypographicNumber
    , case when ed.[ExpireDate] is null then ''Не найдено'' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then ''Аннулировано'' else
      case when getdate() <= ed.[ExpireDate] then ''Действительно''
      else ''Истек срок'' end end end as [Status],
        isnull(CC.UniqueChecks, 0) UniqueChecks,
        isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
        isnull(CC.UniqueIHECheck, 0) UniqueIHECheck,
        isnull(CC.UniqueIHEFCheck, 0) UniqueIHEFCheck,
        isnull(CC.UniqueTSSaFCheck, 0) UniqueTSSaFCheck,
        isnull(CC.UniqueTSSCheck, 0) UniqueTSSCheck,
        isnull(CC.UniqueTSSFCheck, 0) UniqueTSSFCheck,
        isnull(CC.UniqueRCOICheck, 0) UniqueRCOICheck,
        isnull(CC.UniqueOUOCheck, 0) UniqueOUOCheck,
        isnull(CC.UniqueFounderCheck, 0) UniqueFounderCheck,
        isnull(CC.UniqueOtherCheck, 0) UniqueOtherCheck,
        C.ParticipantID
  from #certificate_check certificate_check
      join vw_Examcertificate1 C on '
        
  if @ParticipantID is null  
    set @sql=@sql + ' C.Id = certificate_check.certificateId '
  else
        set @sql=@sql + ' C.ParticipantID=certificate_check.ParticipantID '
        
    set @sql=@sql + '  
        left outer join ExamcertificateUniqueChecks CC on CC.IdGuid = C.Id
    left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
    join (
      select
        certificate_check.Number 
        , certificate_check.CheckLastName
        , certificate_check.LastName 
        , certificate_check.LastNameIsCorrect
        , certificate_check.CheckFirstName
        , certificate_check.FirstName 
        , certificate_check.FirstNameIsCorrect
        , certificate_check.CheckPatronymicName
        , certificate_check.PatronymicName 
        , certificate_check.PatronymicNameIsCorrect
        , certificate_check.IsExist
        , isnull(check_subject.SubjectId, [subject].Id) SubjectId
        , check_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when check_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal,
        certificate_subject.certificatefk certificateId,
        certificate_subject.ParticipantFK ParticipantID
      from [prn].CertificatesMarks certificate_subject with (nolock)         
        inner join dbo.[Subject] [subject] on certificate_subject.SubjectCode = [subject].SubjectId
        inner join #certificate_check certificate_check
          on certificate_check.[Year] = certificate_subject.UseYear '
  if @ParticipantID is null   
    set @sql=@sql + ' and certificate_check.certificateId = certificate_subject.CertificateFK '
  else
    set @sql=@sql + ' and certificate_subject.ParticipantFK = certificate_check.ParticipantID '
    
  set @sql=@sql + ' 
        left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
          on check_subject.SubjectId = [subject].Id
      ) check_subject on '  
  if @ParticipantID is null   
    set @sql=@sql + ' certificate_check.certificateId = check_subject.certificateId '
  else
    if @number <> ''  
      set @sql=@sql + ' C.ParticipantID=check_subject.ParticipantID and check_subject.certificateId=C.id '
    else
      set @sql=@sql + ' C.ParticipantID=check_subject.ParticipantID and check_subject.certificateId<>C.id '
             
  set @sql=@sql + '
      left outer join dbo.[Subject] [subject] on check_subject.SubjectId = [subject].Id
      left join [MinimalMark] as mm on [subject].SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year] '      
  
  exec sp_executesql @sql,N'@checkSubjectMarks nvarchar(max)',@checkSubjectMarks=@checkSubjectMarks
  
  drop table #certificate_check 
    if @checkTypographicNumber is not null
      set @eventCode = 'CNE_CHK_TN'
    else
      set @eventCode = 'CNE_CHK'

    exec dbo.RegisterEvent 
      @accountId
      , @ip
      , @eventCode
      , @sourceEntityIds
      , @eventParams
      , @updateId = null
  return 0
end
go
