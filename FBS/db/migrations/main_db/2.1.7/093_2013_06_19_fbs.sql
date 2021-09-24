insert into Migrations(MigrationVersion, MigrationName) values (93, '093_2013_06_19_fbs.sql')
go

alter PROC [dbo].[GetNEWebUICheckLog]
    @login NVARCHAR(255),
    @startRowIndex INT = 1,
    @maxRowCount INT = NULL,
    @showCount BIT = NULL,   -- если > 0, то выбирается общее кол-во
    @TypeCode NVARCHAR(255) -- Тип проверки
AS 
    BEGIN
        DECLARE @accountId BIGINT,
            @endRowIndex INTEGER

        IF ISNULL(@maxRowCount, -1) = -1 
            SET @endRowIndex = 10000000
        ELSE 
            SET @endRowIndex = @startRowIndex + @maxRowCount

        IF EXISTS ( SELECT  1
                    FROM    [Account] AS a2
                            JOIN [GroupAccount] ga ON ga.[AccountId] = a2.[Id]
                            JOIN [Group] AS g ON ga.[GroupId] = g.[Id]
                                                 AND g.[Code] = 'Administrator'
                    WHERE   a2.[Login] = @login ) 
            SET @accountId = NULL
        ELSE 
            SET @accountId = ISNULL(( SELECT    account.[Id]
                                      FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                                      WHERE     account.[Login] = @login
                                    ), 0)

        IF ISNULL(@showCount, 0) = 0 
            BEGIN 
                IF @accountId IS NULL 
                    SELECT  *
                    FROM    ( SELECT    b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,   
                                        case when isnumeric(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2))=1 then
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) end YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )
                ELSE 
                    SELECT  *
                    FROM    ( SELECT TOP ( @endRowIndex )
                                        b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,
                                        case when isnumeric(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2))=1 then
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) end YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     b.AccountId = @accountId
                                                    AND @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )   
            END
        ELSE 
            IF @accountId IS NULL 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE )
            ELSE 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   b.AccountId = @accountId
                        AND @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE ) 
        RETURN 0
    END
GO
-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
alter PROC [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]
    @batchId BIGINT,
    @xml XML OUT
AS 
    SET nocount ON
    DECLARE @id BIGINT,
        @isCorrect BIT
    SELECT  @id = id,
            @isCorrect = IsCorrect
    FROM    dbo.CommonNationalExamCertificateCheckBatch WITH ( NOLOCK, FASTFIRSTROW )
    WHERE   outerId = @batchId
            AND IsProcess = 0
            AND Executing = 0
    SET @xml = ( SELECT ( SELECT    cnCheck.Id AS '@Id',
                                    @isCorrect AS '@IsBatchCorrect',
                                    cnCheck.BatchId AS '@BatchId',
                                    cnCheck.CertificateCheckingId AS '@CertificateCheckingId',
                                    cnCheck.CertificateNumber AS '@CertificateNumber',
                                    cnCheck.IsOriginal AS '@IsOriginal',
                                    cnCheck.IsCorrect AS '@IsCorrect',
                                    cnCheck.SourceCertificateIdGuid AS '@SourceCertificateIdGuid',
                                    cnCheck.IsDeny AS '@IsDeny',
                                    cnCheck.DenyComment AS '@DenyComment',
                                    cnCheck.DenyNewCertificateNumber AS '@DenyNewCertificateNumber',
                                    cnCheck.Year AS '@Year',
                                    cnCheck.TypographicNumber AS '@TypographicNumber',
                                    cnCheck.RegionId AS '@RegionId',
                                    cnCheck.PassportSeria AS '@PassportSeria',
                                    cnCheck.PassportNumber AS '@PassportNumber',
                                    cnCheck.ParticipantFK AS '@ParticipantFK',
                                    ISNULL(unique_cheks.IdGuid,null) AS '@UniqueCheckIdGuid',
                  ISNULL(unique_cheks.UniqueIHEaFCheck,0) AS '@UniqueIHEaFCheck',
                  ISNULL(unique_cheks.[year],0) AS '@UniqueCheckYear',
                                    ( SELECT    b.Id AS 'subject/@Id',
                                                b.CheckId AS 'subject/@CheckId',
                                                b.SubjectId AS 'subject/@SubjectId',
                                                b.Mark AS 'subject/@Mark',
                                                b.IsCorrect AS 'subject/@IsCorrect',
                                                b.SourceCertificateSubjectIdGuid AS 'subject/@SourceCertificateSubjectIdGuid',
                                                b.SourceMark AS 'subject/@SourceMark',
                                                b.SourceHasAppeal AS 'subject/@SourceHasAppeal',
                                                b.Year AS 'subject/@Year'
                                      FROM      CommonNationalExamCertificateSubjectCheck b
                                      WHERE     --BatchId=@id and 
                                                b.CheckId = cnCheck.id
                                    FOR
                                      XML PATH(''),
                                          ROOT('subjects'),
                                          TYPE
                                    )
                          FROM      CommonNationalExamCertificateCheck cnCheck WITH ( NOLOCK)
              left outer join dbo.ExamCertificateUniqueChecks unique_cheks WITH ( NOLOCK)   on unique_cheks.IdGuid = cnCheck.SourceCertificateIdGuid 
                          WHERE     --cnCheck.id in(3201511,3201512)
                                    BatchId = @id
                        FOR
                          XML PATH('check'),
                              TYPE
                        )
               FOR
                 XML PATH('root'),
                     TYPE
               )
SELECT @xml               
go
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
alter proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
    @number nvarchar(255) = null        -- номер сертификата  
  , @checkSubjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
  , @ParticipantID uniqueidentifier = null
  , @xml xml out
as
begin 
  
  if @number is null and @ParticipantID is null
  begin
    RAISERROR (N'Номер св-ва не указан',10,1);
    return
  end

  declare @eventId int

  if @shouldCheckMarks = 1
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @checkSubjectMarks, @CNENumber = @number, @IsOpenFbs = 1, @eventId = @eventId output
  
  
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
  
  declare @certificate_check table
  (
  Number nvarchar(255)
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
    , case
      when [certificate].ParticipantID is not null or [certificate].Id is not null then 1
      else 0
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
    , [certificate].ParticipantID
  from 
    (select null ''empty'') t left join 
    (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) [certificate] on 
        [certificate].[Year] between @yearFrom and @yearTo '
  if @ParticipantID is not null 
    set @sql = @sql + ' and [certificate].ParticipantID = @ParticipantID'   
  if @number is not null  
    set @sql = @sql + ' and [certificate].Number = @number'
  set @sql = @sql + '     
    left outer join dbo.Region region
      on region.Id = [certificate].RegionId
    left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] between @yearFrom and @yearTo
        and certificate_deny.CertificateFK = [certificate].id'

  insert into @certificate_check  
  exec sp_executesql @sql,N'@number nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier',@number = @number,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID
  select * from @certificate_check
  set @eventParams = 
    isnull(@number, '') + '||||' +
    isnull(@checkSubjectMarks, '') + '|' 

  set @sourceEntityIds = '' 
  select 
    @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(100), certificate_check.certificateId) 
  from 
    @certificate_check certificate_check 
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
  if @sourceEntityIds = '' 
    set @sourceEntityIds = null 


  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        @certificate_check S
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
  
      
  select
    certificate_check.certificateId
    ,certificate_check.Number Number
    , certificate_check.IsExist IsExist
    , check_subject.Id  SubjectId
    , check_subject.Name  SubjectName
    , case when check_subject.CheckSubjectMark < check_subject.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',')  CheckSubjectMark
    , case when check_subject.SubjectMark < check_subject.MinimalMark1 then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',')  SubjectMark
    ,check_subject.SubjectMarkIsCorrect SubjectMarkIsCorrect
    , check_subject.HasAppeal HasAppeal
    , certificate_check.IsDeny IsDeny
    , certificate_check.DenyComment DenyComment
    , certificate_check.DenyNewcertificateNumber DenyNewcertificateNumber
    , certificate_check.PassportSeria PassportSeria
    , certificate_check.PassportNumber PassportNumber
    , certificate_check.RegionId RegionId
    , certificate_check.RegionName RegionName
    , certificate_check.[Year] [Year]
    , certificate_check.TypographicNumber TypographicNumber
    , case when ed.[ExpireDate] is null then 'Не найдено' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
      case when getdate() <= ed.[ExpireDate] then 'Действительно'
      else 'Истек срок' end end end  as [Status],
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
        C.ParticipantID
        into #table
  from @certificate_check certificate_check
      join ((SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID)) C
          on 1= case when @ParticipantID is null and C.Id = certificate_check.certificateId then 1
                  when @ParticipantID is not null and C.ParticipantID=certificate_check.ParticipantID then 1 
             else 0 end
        left outer join ExamcertificateUniqueChecks CC on CC.IdGuid = certificate_check.certificateId
    left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
    left outer join (
      select      
        getcheck_subject.SubjectId id,[subject].Name,
        certificate_subject.UseYear [Year],certificate_subject.certificateFK certificateId, 
        isnull(getcheck_subject.SubjectId, certificate_subject.SubjectCode) SubjectId
        , getcheck_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when getcheck_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal
        ,mm.[MinimalMark]
        ,mm1.[MinimalMark] MinimalMark1
        ,certificate_subject.ParticipantFK ParticipantID
      from 
      [prn].CertificatesMarks certificate_subject with (nolock)
      join dbo.[Subject] [subject]  on [subject].Id = certificate_subject.SubjectCode   
      left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject  on getcheck_subject.SubjectId = certificate_subject.SubjectCode     
      left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.UseYear = mm.[Year] 
      left join [MinimalMark] as mm1 on certificate_subject.SubjectCode = mm1.[SubjectId] and certificate_subject.UseYear = mm1.[Year] 
      ) check_subject
      on certificate_check.[Year] = check_subject.[Year] and 
      1 = case when @ParticipantID is null 
                      and isnull(certificate_check.certificateId, '76A02C33-9A91-443A-996C-7640F7481B55') = isnull(check_subject.certificateId,'76A02C33-9A91-443A-996C-7640F7481B55') then 1
                  when @ParticipantID is not null 
                      and C.ParticipantID=check_subject.ParticipantID then 1
             else 0 end       
      
      --select * from #table
      
IF @shouldCheckMarks = 1 AND  (exists(select * from #table where  SubjectMarkIsCorrect=0 and SubjectId IS NOT null) or (select COUNT(*) from #table where SubjectId IS NOT null)<>(select COUNT(*) from dbo.GetSubjectMarks(@checkSubjectMarks)))
  delete from #table
  --SELECT * FROM #table
  select @xml=(
  select 
  (
  select * from #table
  for xml path('check'), ELEMENTS XSINIL,type
  ) 
  for xml path('root'),type
  )
  
goto result 
nullresult:
  select @xml=(
  select null 
  for xml path('root'),type
  )
result:
    --SELECT * from @certificate_check
    -- записать в лог интерактивных проверок
    if @shouldCheckMarks = 1 and exists (select * from #table)
    UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 certificateId FROM @certificate_check)
      WHERE Id=@eventId
    drop table #table
    
    -- записать в лог всех проверок
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
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
alter proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]    
   @subjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @shouldWriteLog BIT = 1         -- нужно ли записывать в лог интерактивных проверок 
  , @ParticipantID uniqueidentifier = null
  ,@xml xml out
as
begin 
set nocount on
  
  if @passportSeria is null and @passportNumber is null and @ParticipantID is null
  begin
    RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
    return
  end
    
  IF (@passportSeria is null)
    SET @passportSeria = ''

  declare @eventId INT
  IF @shouldWriteLog = 1
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @subjectMarks, @passportSeria = @passportSeria, @passportNumber = @passportNumber, @IsOpenFbs = 1, @eventId = @eventId output

declare 
    @eventCode nvarchar(255)
        , @organizationId bigint
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)   
    , @commandText nvarchar(4000)
    , @internalPassportSeria nvarchar(255)

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  set @eventParams = 
    isnull(@subjectMarks, '') + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') + '|' 

  select
    @editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login    

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  set @commandText = ''
  set @eventCode = N'CNE_FND_P'

  declare @sourceEntityIds nvarchar(4000) 
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
    , ParticipantsID uniqueidentifier
  ) 
    
  set @commandText = @commandText +       
    'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year, ParticipantsID
    from 
    (
      select b.LicenseNumber AS Number, a.Surname AS FirstName, a.Name AS LastName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantsID
            from rbd.Participants a with (nolock)
        left join prn.Certificates b with (nolock) on b.ParticipantFK = a.ParticipantID
        left join prn.CancelledCertificates c on c.CertificateFK=b.CertificateID
    ) certificate
    where 
    ' 
  if @ParticipantID is not null 
    set @commandText = @commandText + ' and certificate.ParticipantID = @ParticipantID'   
        
    if not @internalPassportSeria is null
    begin
        set @commandText = @commandText + ' certificate.PassportSeria = @internalPassportSeria '
  end
  
  if not @passportNumber is null
    begin
        set @commandText = @commandText + ' and  certificate.PassportNumber = @passportNumber '
    end
  else
  begin
    goto nullresult
  end
  print @commandText
  insert into @Search
  exec sp_executesql @commandText
    , N' @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @ParticipantID uniqueidentifier '
    ,@internalPassportSeria, @passportNumber, @ParticipantID

    
/*
declare @xml xml
exec  [CheckCommonNationalExamCertificateByPassportForXml]  @login='SuperAdmin@sibmail.com',@ip='SuperAdmin@sibmail.com',@passportSeria='9205',@passportNumber ='527439',@subjectMarks='',@xml=@xml out
select @xml 
*/
  

  if @subjectMarks is not null
  begin
    
  if exists(
    select * from
    dbo.GetSubjectMarks(@subjectMarks)  t
    left join (
    select distinct b.SubjectCode SubjectId,b.Mark  from 
    @Search search
      join [prn].CertificatesMarks b with(nolock) 
      on case when search.CertificateId is null and search.ParticipantsID = b.ParticipantFK then 1
              when search.CertificateId is not null and search.CertificateId = b.CertificateFK then 1
              else 0 end  = 1 
      ) tt
      on t.SubjectId=tt.SubjectId and t.Mark=tt.Mark
      where tt.SubjectId is null)
  delete from @search
      
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
           @certificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 
  
select @xml=(
  select 
  (
  select 
      isnull(cast(S.certificateId as nvarchar(250)),'Нет свидетельства' ) certificateId,
      S.CertificateNumber CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case when S.CertificateId is not null or S.ParticipantsID is not null then 1 else 0 end IsExist, 
      case  when CD.UseYear is not null then 1 end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when CD.UseYear is not null then 'Аннулировано' 
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
      S.ParticipantsID
    from 
        @Search S 
            left join dbo.vw_Examcertificate C with (nolock) 
              on case when C.Id = S.CertificateId then 1
                      when C.ParticipantID=S.ParticipantsID then 1
                      else 0 end=1
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.IdGuid  = S.certificateId 
      left outer join prn.CancelledCertificates CD with (nolock) 
        on S.CertificateId = CD.CertificateFK 
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed
              on ed.[year] = S.[year]
            for xml path('check'), ELEMENTS XSINIL,type
  ) 
  for xml path('root'),type
  )

/*
declare @xml xml
exec  [CheckCommonNationalExamCertificateByPassportForXml]  @login='SuperAdmin@sibmail.com',@ip='SuperAdmin@sibmail.com',@passportSeria='9205',@passportNumber ='527439',@subjectMarks='',@xml=@xml out
select @xml
*/
    
goto result 
nullresult:
  select @xml=(
  select null 
  for xml path('root'),type
  )
result:

    --select * from @Search
    
    -- записать в лог интерактивных проверок
    IF EXISTS (SELECT * FROM @Search) AND @shouldWriteLog = 1
    BEGIN
      UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 isnull(cast(certificateId as nvarchar(510)),'Нет свидетельства' ) FROM @Search where ParticipantsID is not null)
       WHERE Id=@eventId
    END
    

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
