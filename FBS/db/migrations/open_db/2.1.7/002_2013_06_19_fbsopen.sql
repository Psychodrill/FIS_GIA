insert into Migrations(MigrationVersion, MigrationName) values (02, '002_2013_06_19_fbsopen.sql')
go

if not exists(select * from sys.columns where name='ParticipantFK' and object_name(object_id)='CommonNationalExamCertificateCheck')
alter table CommonNationalExamCertificateCheck add ParticipantFK uniqueidentifier
go

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null   -- если > 0, то выбирается общее кол-во
  ,@type int
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  if exists ( select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login )
    set @accountId = null
  else 
    set @accountId = isnull(
      (select account.[Id] 
      from dbo.Account account with (nolock, fastfirstrow) 
      where account.[Login] = @login), 0)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table 
        ( 
        Id bigint 
        , CreateDate datetime 
        , IsProcess bit 
        , IsCorrect bit 
        , Login varchar(255) 
        , Total int
        , Found int
        ,outerId bigint
        ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> 
          b.Id Id 
          , b.CreateDate CreateDate 
          , b.IsProcess IsProcess 
          , b.IsCorrect IsCorrect 
          , a.login Login 
          , (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
          , (select count(CertificateNumber) from CommonNationalExamCertificateCheck c with(nolock) where CertificateNumber<>'''' and c.batchid = b.id) Found
          ,outerId
        from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
        left join account a on a.id = b.OwnerAccountId 
        where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 
        and type=<@type>' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) ' +
        'where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 and type=<@type>' 
set @commandText=REPLACE(@commandText,'<@type>',@type)
  if isnull(@showCount, 0) = 0
  begin
    set @innerOrder = 'order by CreateDate <orderDirection> '
    set @outerOrder = 'order by CreateDate <orderDirection> '
    set @resultOrder = 'order by CreateDate <orderDirection> '

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
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select 
        dbo.GetExternalId(s.Id) Id 
        , s.Login 
        , s.CreateDate 
        , s.IsProcess 
        , s.IsCorrect 
        , s.Total
        , s.Found
        ,outerId
      from @search s ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId

  return 0
end
go
-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Предметы отсортированы по порядку и актуальности.
-- Разделение на части текста по мере заполнения предыдущей части.
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Убрано сравнение HasAppeal. Добавлены IsDeny, DenyComment.
-- v.1.3: Modified by Sedov Anton 04.07.2008
-- В результат запроса  добавлено поле 
-- DenyNewCertificateNumber
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificateCheck]
  @login nvarchar(255)
  , @batchId bigint     -- id пакета
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null    -- если > 0, то выбирается общее кол-во
  , @certNumber nvarchar(50) = null -- опциональный номер сертификата
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.CommonNationalExamCertificateCheckBatch cne_certificate_check_batch with (nolock, 

fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on cne_certificate_check_batch.OwnerAccountId = account.[Id]
      where 
        cne_certificate_check_batch.Id = @internalBatchId
        and cne_certificate_check_batch.IsProcess = 0
        and (account.[Login] = @login or exists (select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login)))
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(max)
    , @commandText nvarchar(max)
    , @viewCommandText nvarchar(max)
    , @viewSelectCommandText nvarchar(max)
    , @viewSelectPivotCommandText nvarchar(max)
    , @pivotSubjectColumns nvarchar(max)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @viewSelectCommandText = ''
  set @viewSelectPivotCommandText = ''
  set @pivotSubjectColumns = ''
  set @sortColumn = N'Id'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table 
        ( 
        Id bigint 
        , BatchId bigint 
        , CertificateNumber nvarchar(255) 
        , CertificateId uniqueidentifier 
        , IsOriginal bit 
        , CheckLastName nvarchar(255) 
        , LastName nvarchar(255) 
        , CheckFirstName nvarchar(255) 
        , FirstName nvarchar(255) 
        , CheckPatronymicName nvarchar(255) 
        , PatronymicName nvarchar(255) 
        , IsCorrect bit 
        , IsDeny bit 
        , Year int
        , TypographicNumber nvarchar(255) 
        , [Status] nvarchar(255) 
          , RegionName nvarchar(255) 
          , RegionCode nvarchar(10) 
          , PassportSeria nvarchar(255) 
          , PassportNumber nvarchar(255) 
          , ParticipantID uniqueidentifier
        , primary key(id)
        ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' cne_check.Id ' +
        ' , cne_check.BatchId ' +
        ' , cne_check.CertificateNumber ' +
        ' , cne_check.SourceCertificateIdGuid CertificateId ' +
        ' , cne_check.IsOriginal ' +
        ' , cne_check.LastName CheckLastName ' +
        ' , cne_check.SourceLastName LastName ' +
        ' , cne_check.FirstName CheckFirstName ' +
        ' , cne_check.SourceFirstName FirstName ' +
        ' , cne_check.PatronymicName CheckPatronymicName ' +
        ' , cne_check.SourcePatronymicName PatronymicName ' +
        ' , cne_check.IsCorrect ' +
        ' , cne_check.IsDeny ' +
        ' , cne_check.Year ' +
        ' , cne_check.TypographicNumber ' +
        ' , case when ed.[ExpireDate] is null then ''Не найдено'' else 
            case when isnull(cne_check.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 

''Действительно'' 
            else ''Истек срок'' end end as [Status] ' +
        ' , r.name as RegionName ' +
        ' , r.code as RegionCode ' +
        ' , cne_check.PassportSeria ' +
        ' , cne_check.PassportNumber ' +
        '   , cne_check.ParticipantFK ' + 
        'from dbo.CommonNationalExamCertificateCheck cne_check with (nolock) ' +
        'left join [ExpireDate] as ed on cne_check.[Year] = ed.[Year] '+
        'left join [Region] as r on cne_check.regionid = r.[Id] '+
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateCheck cne_check with (nolock) ' +
        'where 1 = 1 ' 

  set @commandText = @commandText + 
    ' and cne_check.BatchId = @internalBatchId '
  if @certNumber is not null
    set @commandText = @commandText + 
    ' and cne_check.CertificateNumber = ''' + @certNumber + ' '''

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = 'order by Id <orderDirection> '
      set @outerOrder = 'order by Id <orderDirection> '
      set @resultOrder = 'order by Id <orderDirection> '
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
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
  begin
    set @declareCommandText = @declareCommandText +
      'declare @check_subject table
        ( 
          CheckId bigint 
        , SubjectId smallint
        , CertificateSubjectId bigint 
        , SubjectCode nvarchar(255) 
        , CheckMark nvarchar(10) 
        , Mark nvarchar(10) 
        , HasAppeal int 
        , IsCorrect int 
        , primary key (CheckId, SubjectId)
        ) '

    set @commandText = @commandText +
      ' insert into @check_subject 
      select 
          subject_check.CheckId 
        , subject_check.SubjectId
        , subject_check.SourceCertificateSubjectId 
        , subject.Code 
        , case when subject_check.[Mark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[Mark] as nvarchar(9)),''.'','','')
        , case when subject_check.[SourceMark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[SourceMark] as nvarchar(9)),''.'','','')
        , cast(subject_check.SourceHasAppeal as int) 
        , cast(subject_check.IsCorrect as int) 
      from dbo.CommonNationalExamCertificateSubjectCheck subject_check with (nolock) 
      inner join dbo.Subject subject on subject.Id = subject_check.SubjectId 
      left join [MinimalMark] as mm on subject_check.[SubjectId] = mm.[SubjectId] and subject_check.Year = 

mm.[Year] 
      where subject_check.BatchId = @internalBatchId 
      order by subject.IsActive desc, subject.SortIndex asc ' 

    set @viewSelectCommandText = 
      ' select 
          dbo.GetExternalId(search.Id) Id 
        , dbo.GetExternalId(search.BatchId) BatchId 
        , search.CertificateNumber 
        , search.IsOriginal 
        , search.CheckLastName 
        , search.LastName 
        , case 
          when search.CheckLastName collate cyrillic_general_ci_ai = search.LastName then 1 
          else 0 
          end LastNameIsCorrect 
        , search.CheckFirstName 
        , search.FirstName 
        , case 
          when search.CheckFirstName collate cyrillic_general_ci_ai = search.FirstName then 1 
          else 0 
          end FirstNameIsCorrect 
        , search.CheckPatronymicName 
        , search.PatronymicName 
        , case 
          when search.CheckPatronymicName collate cyrillic_general_ci_ai = 

search.PatronymicName then 1 
          else 0 
          end PatronymicNameIsCorrect 
        , case when search.CertificateNumber is not null then 1 else 0 end IsExist 
        , search.IsCorrect 
        , cast(search.IsDeny as bit) IsDeny 
        , cne_check.DenyComment   
        , cne_check.DenyNewCertificateNumber 
        , search.Year SourceCertificateYear
        , search.TypographicNumber 
        , search.[Status] 
        , search.RegionName 
        , search.RegionCode
        , search.PassportSeria 
        , search.PassportNumber
        , search.ParticipantID ' 
      

    declare
        @subjectCode nvarchar(255)
      , @pivotSelect nvarchar(max)

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
      set @pivotSubjectColumns = @pivotSubjectColumns + '[' + @subjectCode + ']'

      set @viewSelectPivotCommandText = @viewSelectPivotCommandText 
          + replace(
          ' , chk_mrk_pvt.[<code>] [<code>CheckMark]  
            , mrk_pvt.[<code>] [<code>Mark]  
            , case 
              when chk_mrk_pvt.[<code>] = mrk_pvt.[<code>] then 1 
              else 0 
            end [<code>MarkIsCorrect] 
            , apl_pvt.[<code>] [<code>HasAppeal] 
            , case 
              when not sbj_pvt.[<code>] is null then 1 
              else 0 
            end [<code>IsExist] 
            , crt_pvt.[<code>] [<code>IsCorrect] ' 
          , '<code>', @subjectCode)

      fetch next from subject_cursor into @subjectCode
    end
    close subject_cursor
    deallocate subject_cursor

    set @viewCommandText = replace(
       ',unique_cheks.UniqueIHEaFCheck
        from @search search 
        left outer join dbo.CommonNationalExamCertificateCheck cne_check with (nolock) 
          on cne_check.Id = search.Id
        left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on unique_cheks.IdGuid = search.CertificateId
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.CheckMark 
            from @check_subject check_subject) check_subject 
            pivot (min(CheckMark) for SubjectCode in (<subject_columns>)) as 

chk_mrk_pvt 
          on search.Id = chk_mrk_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.Mark 
            from @check_subject check_subject) check_subject 
            pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt 
          on search.Id = mrk_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.HasAppeal 
            from @check_subject check_subject) check_subject 
            pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as apl_pvt 
          on search.Id = apl_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.IsCorrect 
            from @check_subject check_subject) check_subject 
            pivot (Sum(IsCorrect) for SubjectCode in (<subject_columns>)) as crt_pvt 
          on search.Id = crt_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.CertificateSubjectId 
            from @check_subject check_subject) check_subject 
            pivot (Sum(CertificateSubjectId) for SubjectCode in (<subject_columns>)) as 

sbj_pvt 
          on search.Id = sbj_pvt.CheckId ' +
      @resultOrder, '<subject_columns>', @pivotSubjectColumns)
  end

  set @commandText = @declareCommandText + @commandText + @viewSelectCommandText + 
      @viewSelectPivotCommandText + @viewCommandText
  
  declare @params nvarchar(200)

  set @params = 
      '@internalBatchId int '
  print cast(@commandText as ntext)
  exec sp_executesql 
      @commandText
      ,@params
      ,@internalBatchId 
  
  PRINT @commandText
  PRINT @params
  print @internalBatchId
  
  return 0
end
go

ALTER PROC [dbo].[usp_cne_AddCheckBatchResult]
    @xml XML = NULL,
    @batchid BIGINT = 0
AS 
    SET nocount ON  
    IF @xml IS NULL 
        BEGIN
            SELECT  NULL IsProcess1,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END

    SELECT  item.ref.value('@Id', 'bigint') AS Id,      
            item.ref.value('@IsBatchCorrect', 'bit') AS IsBatchCorrect,
            item.ref.value('@BatchId', 'bigint') AS BatchId,
            item.ref.value('@CertificateCheckingId', 'uniqueidentifier') AS CertificateCheckingId,
            item.ref.value('@CertificateNumber', 'nvarchar(255)') AS CertificateNumber,
            item.ref.value('@IsOriginal', 'bit') AS IsOriginal,
            item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
            item.ref.value('@SourceCertificateIdGuid', 'uniqueidentifier') AS SourceCertificateId,
            item.ref.value('@IsDeny', 'bit') AS IsDeny,
            item.ref.value('@DenyComment', 'nvarchar(max)') AS DenyComment,
            item.ref.value('@DenyNewCertificateNumber', 'nvarchar(255)') AS DenyNewCertificateNumber,
            item.ref.value('@Year', 'int') AS Year,
            item.ref.value('@TypographicNumber', 'nvarchar(255)') AS TypographicNumber,
            item.ref.value('@RegionId', 'int') AS RegionId,
            item.ref.value('@PassportSeria', 'nvarchar(255)') AS PassportSeria,
            item.ref.value('@PassportNumber', 'nvarchar(255)') AS PassportNumber,
            item.ref.value('@ParticipantFK', 'uniqueidentifier') AS ParticipantFK,
            item.ref.value('@UniqueCheckIdGuid', 'uniqueidentifier') AS UniqueCheckIdGuid,
      item.ref.value('@UniqueIHEaFCheck', 'int') AS UniqueIHEaFCheck,
      item.ref.value('@UniqueCheckYear', 'int') AS UniqueCheckYear
    INTO    #check
    FROM    ( SELECT    @xml
            ) feeds ( feedXml )
            CROSS APPLY feedXml.nodes('/root/check') AS item ( ref )
--SELECT * FROM #check

    IF NOT EXISTS ( SELECT  *
                    FROM    #check ) 
        BEGIN
            SELECT  NULL IsProcess2,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END


--select * from #check
    DECLARE @id BIGINT, @checkid BIGINT, @isCorrect BIT
    SELECT  @batchid = dbo.GetInternalId(@batchid)
    IF EXISTS ( SELECT  *
                FROM    CommonNationalExamCertificateCheck
                WHERE   BatchId = @batchid ) 
        BEGIN
            SELECT  NULL IsProcess4,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END
--select @batchid
    BEGIN TRY
    BEGIN TRAN
    -- обновление статистики для уже проверенных св-в
      update    ExamCertificateUniqueChecks
      set       UniqueIHEaFCheck = #check.UniqueIHEaFCheck
      from      ExamCertificateUniqueChecks ex
          inner join #check on #check.UniqueCheckIdGuid = ex.IdGuid
                     AND #check.[UniqueCheckYear] = ex.[year]
          WHERE #check.UniqueCheckIdGuid is not null
      -- добавление статистики для еще не проверенных св-в
      insert    ExamCertificateUniqueChecks
          (
            IdGuid,
            [year],
            UniqueIHEaFCheck
          )

          select  DISTINCT #check.UniqueCheckIdGuid,
                    #check.UniqueCheckYear,
                        #check.UniqueIHEaFCheck
          from    #check
              left join ExamCertificateUniqueChecks ex on ex.IdGuid = #check.UniqueCheckIdGuid
                                    AND #check.[UniqueCheckYear] = ex.[year]
          where   ex.IdGuid is NULL AND #check.UniqueCheckIdGuid is not null

INSERT  CommonNationalExamCertificateCheck
                        (
                          BatchId,
                          CertificateCheckingId,
                          CertificateNumber,
                          IsOriginal,
                          IsCorrect,
                          SourceCertificateIdGuid,
                          IsDeny,
                          DenyComment,
                          DenyNewCertificateNumber,
                          Year,
                          TypographicNumber,
                          RegionId,
                          PassportSeria,
                          PassportNumber,
                          idtemp,
                          ParticipantFK
                        )                        
                        SELECT  @batchid,
                                CertificateCheckingId,
                                CertificateNumber,
                                IsOriginal,
                                IsCorrect,
                                SourceCertificateId,
                                IsDeny,
                                DenyComment,
                                DenyNewCertificateNumber,
                                Year,
                                TypographicNumber,
                                RegionId,
                                PassportSeria,
                                PassportNumber,
                                a.id,
                                ParticipantFK
                        FROM    #check a
                    
INSERT  CommonNationalExamCertificateSubjectCheck
                        (
                          BatchId,
                          CheckId,
                          SubjectId,
                          Mark,
                          IsCorrect,
                          SourceCertificateSubjectIdGuid,
                          SourceMark,
                          SourceHasAppeal,
                          Year
                        )
                        SELECT  @batchid,
                                b.id,
                                a.SubjectId,
                                a.Mark,
                                a.IsCorrect,
                                a.SourceCertificateSubjectIdGuid,
                                a.SourceMark,
                                a.SourceHasAppeal,
                                a.[Year]
                        FROM    
                        
                        (
                            SELECT  item.ref.value('@Id', 'bigint') AS Id,
                  item.ref.value('@CheckId', 'bigint') AS CheckId,
                  item.ref.value('@SubjectId', 'bigint') AS SubjectId,
                  item.ref.value('@Mark', 'numeric(5,1)') AS Mark,
                  item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
                  item.ref.value('@SourceCertificateSubjectIdGuid', 'uniqueidentifier') AS SourceCertificateSubjectIdGuid,
                  item.ref.value('@SourceMark', 'numeric(5,1)') AS SourceMark,
                  item.ref.value('@SourceHasAppeal', 'bigint') AS SourceHasAppeal,
                  item.ref.value('@Year', 'int') AS [Year]
                FROM    ( SELECT    @xml)      
                feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/root/check/subjects/subject') AS item ( ref )
                        ) a join CommonNationalExamCertificateCheck b on a.CheckId=b.idtemp and b.BatchId=@batchid
                       
 
        SELECT TOP 1
                @isCorrect = IsBatchCorrect
        FROM    #check
        UPDATE  CommonNationalExamCertificateCheckBatch
        SET     IsProcess = 0,
                Executing = 0,
                IsCorrect = @isCorrect
        WHERE   id = @batchid
        
--select top 10 * from CommonNationalExamCertificateCheckBatch where id=@batchid
--select top 10 * from CommonNationalExamCertificateCheck where BatchId=@batchid  order by id desc
--select top 10 * from CommonNationalExamCertificateSubjectCheck where BatchId=@batchid order by id desc
        SELECT  IsProcess,
                ( SELECT    COUNT(*)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Total,
                ( SELECT    COUNT(SourceCertificateId)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Found
        FROM    CommonNationalExamCertificateCheckBatch b
        WHERE   IsProcess = 0
                AND id = @batchid
                
        IF @@trancount > 0 
            COMMIT TRAN

    END TRY
    BEGIN CATCH
        IF @@trancount > 0 
            ROLLBACK
        DECLARE @msg NVARCHAR(4000)
        SET @msg = ERROR_MESSAGE()
        RAISERROR ( @msg, 16, 1 )
        RETURN -1
    END CATCH
