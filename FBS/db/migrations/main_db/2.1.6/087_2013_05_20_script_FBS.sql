insert into Migrations(MigrationVersion, MigrationName) values (87, '087_2013_05_20_script_FBS.sql')

                     
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
go
alter proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]  
   @subjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @shouldWriteLog BIT = 1         -- нужно ли записывать в лог интерактивных проверок 
  ,@xml xml out
as
begin 
set nocount on
  
  if @passportSeria is null and @passportNumber is null
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
      , @passportNumber nvarchar(255) '
    ,@internalPassportSeria, @passportNumber    
    
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
      on search.CertificateId = b.CertificateFK
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
      S.CertificateId CertificateId,  
      S.CertificateNumber CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case when S.CertificateId is not null then 1 else 0 end IsExist, 
      case  when CD.Id >0 then 1 end IsDeny,  
      CD.Comment DenyComment, 
      CD.NewCertificateNumber NewCertificateNumber, 
      S.Year Year,
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
      S.ParticipantsID
    from 
        @Search S 
            left join dbo.vw_Examcertificate C with (nolock) 
              on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.IdGuid  = S.certificateId 
      left outer join CommonNationalExamCertificateDeny CD with (nolock) 
        on S.CertificateNumber = CD.CertificateNumber 
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
      UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 cast(certificateId as nvarchar(510)) FROM @Search)
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

alter PROCEDURE [dbo].[ExecuteChecksCount]
    @OrganizationId bigint,
    @CertificateId bigint = null,
    @CertificateIdGuid uniqueidentifier =null
AS
BEGIN
  if (@OrganizationId = 0 or @OrganizationId is null or @CertificateId is null)
    return -1

  -- Выполняла ли данная организация проверку данного сертификата
    declare @isExists bit

  -- Значения инкрементов
  declare @uniqueIHEaFCheck int
  declare @uniqueIHECheck int
  declare @uniqueIHEFCheck int 

  declare @uniqueTSSaFCheck int
  declare @uniqueTSSCheck int
  declare @uniqueTSSFCheck int
    
  declare @uniqueRCOICheck int
  declare @uniqueOUOCheck int
  declare @uniqueFounderCheck int
  declare @uniqueOtherCheck int 
    
    -- Тип организации
    declare @orgType int
    
    -- Является ли организация филлиалом
    declare @isFilial bit

  -- Инициализация переменных
    set @isExists = 0
  set @uniqueIHEaFCheck = 0
  set @uniqueIHECheck = 0
  set @uniqueIHEFCheck = 0
  set @uniqueTSSaFCheck = 0
  set @uniqueTSSCheck = 0
  set @uniqueTSSFCheck = 0
  set @uniqueRCOICheck = 0
  set @uniqueOUOCheck = 0
  set @uniqueFounderCheck = 0
  set @uniqueOtherCheck = 0
    set @orgType = 0
    set @isFilial = 0

  if @OrganizationId is null 
    set @isExists = 
        (
      select count(*) 
      from [dbo].[OrganizationCertificateChecks] OCC 
      where OCC.CertificateId = @CertificateId and OCC.OrganizationId = @OrganizationId
      )
  else  
    set @isExists = 
        (
      select count(*) 
      from [dbo].[OrganizationCertificateChecks] OCC 
      where OCC.CertificateId = @CertificateId and OCC.CertificateIdGuid = @CertificateIdGuid
      )

    if (@isExists = 0)
    begin
      insert into [dbo].[OrganizationCertificateChecks] (CertificateId, OrganizationId, CertificateIdGuid)
        values (@CertificateId, @OrganizationId, @CertificateIdGuid)
        
        select
          @orgType = isnull(O.TypeId, 0),
            @isFilial = (case when (O.MainId is null) then 0 else 1 end)
        from
          [dbo].[Organization2010] O
        where
          O.Id = @OrganizationId

        if (@orgType = 0)
          return -1

    if (@orgType = 1)
        begin
          set @uniqueIHEaFCheck = 1
            if (@isFilial = 1)
              set @uniqueIHEFCheck = 1
            else
              set @uniqueIHECheck = 1
        end           
        
    if (@orgType = 2)
        begin
          set @uniqueTSSaFCheck = 1
            if (@isFilial = 1)
              set @uniqueTSSFCheck = 1
            else
              set @uniqueTSSCheck = 1
        end           
        
    if (@orgType = 3)
        begin
          set @uniqueRCOICheck = 1
        end           

    if (@orgType = 4)
        begin
          set @uniqueOUOCheck = 1
        end           

    if (@orgType = 6)
        begin
          set @uniqueFounderCheck = 1
        end           

    if (@orgType = 5)
        begin
          set @uniqueOtherCheck = 1
        end
        
        declare @year int
        if @CertificateId is null
      set @year = 
        (select top 1 C.UseYear
        from [prn].[Certificates] C
        where C.CertificateID = @CertificateIdGuid)
        else
      set @year = 
        (select top 1 C.[Year]
        from CommonNationalExamCertificate C
        where C.Id = @CertificateId)
          
    if not exists 
      (select *
      from [dbo].[ExamCertificateUniqueChecks] EC
      where EC.Id = @CertificateId) 
      or
      not exists (select *
      from [dbo].[ExamCertificateUniqueChecks] EC
      where EC.idGUID = @CertificateIdGuid) 
    begin
      insert into [dbo].[ExamCertificateUniqueChecks] 
        (
        [Year], 
        Id, 
        UniqueChecks,
        UniqueIHEaFCheck,
        UniqueIHECheck,
        UniqueIHEFCheck,
        UniqueTSSaFCheck,
        UniqueTSSCheck,
        UniqueTSSFCheck,
        UniqueRCOICheck,
        UniqueOUOCheck,
        UniqueFounderCheck,
        UniqueOtherCheck,
        idGUID
        )
      values 
        (
        @year, 
        @CertificateId,
        1,
        @uniqueIHEaFCheck,
        @uniqueIHECheck,
        @uniqueIHEFCheck,
        @uniqueTSSaFCheck,
        @uniqueTSSCheck,
        @uniqueTSSFCheck,
        @uniqueRCOICheck,
        @uniqueOUOCheck,
        @uniqueFounderCheck,
        @uniqueOtherCheck,
        @CertificateIdGuid
        )
    end
    else begin
        if @CertificateId is not null  
        update 
              [dbo].[ExamCertificateUniqueChecks]
        set 
          UniqueChecks = UniqueChecks + 1,
          UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
          UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
          UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
          UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
          UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
          UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
          UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
          UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
          UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
          UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
        where
              Id = @CertificateId
              and [Year] = @year
          else
        update 
              [dbo].[ExamCertificateUniqueChecks]
        set 
          UniqueChecks = UniqueChecks + 1,
          UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
          UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
          UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
          UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
          UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
          UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
          UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
          UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
          UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
          UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
        where
              idGUID = @CertificateIdGuid
              and [Year] = @year          
        end

        return 1
    end
    else begin
      return 0
    end
END
go
-- Получение свидетельст за все года по ФИО и паспортным данным
-- Возвращаемы значения
-- Id - идентификатор свидетельства
-- CreateDate - Дата добавления сертификата
-- Number - номер свидетельства
-- Year - год
alter PROCEDURE [dbo].[GetCertificateByFioAndPassport]
  @LastName NVARCHAR(255) = null,       -- фамилия сертифицируемого
  @FirstName NVARCHAR(255) = null,        -- имя сертифицируемого
  @PatronymicName NVARCHAR(255) = null,     -- отчетсво сертифицируемого
  @PassportSeria NVARCHAR(20) = null,   -- серия документа сертифицируемого (паспорта)
  @PassportNumber NVARCHAR(20) = null,    -- номер документа сертифицируемого (паспорта)  
  @CurrentCertificateNumber NVARCHAR(255)   -- номер свидетельства, его нужно исключить при выборке
AS
BEGIN 
  declare @yearFrom int, @yearTo int
  select @yearFrom = 2008, @yearTo = Year(GetDate())
  
  SELECT  c.Id,
        c.CreateDate,
        c.Number,
        c.Year,
        marks,
        case when ed.[ExpireDate] is null then 'Не найдено'
             else case when isnull(certificate_deny.[Id], 0) <> 0
                       then 'Аннулировано'
                       else case when getdate() <= ed.[ExpireDate]
                                 then 'Действительно'
                                 else 'Истек срок'
                            end
                  end
        end as [Status]
FROM    dbo.vw_Examcertificate c
        CROSS APPLY ( SELECT    ( SELECT    CAST(s.SubjectCode AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      [prn].[CertificatesMarks] s
                                  WHERE     s.CertificateFK = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                    ) as marks
        LEFT JOIN dbo.ExpireDate ed ON ed.Year = c.Year
        LEFT OUTER JOIN CommonNationalExamcertificateDeny certificate_deny
        with ( nolock, fastfirstrow ) on certificate_deny.[Year] between @yearFrom and @yearTo
                                         and certificate_deny.certificateNumber = c.[Number]
WHERE    ( LastName = @LastName
            or @LastName is null
          )
          AND ( FirstName = @FirstName
                or @FirstName is null
              )
          AND ( PatronymicName = null
                or @PatronymicName is null
              )
          AND PassportNumber = @PassportNumber
          AND PassportSeria = @PassportSeria
          AND c.Number <> @CurrentCertificateNumber
END
go
-- =============================================
-- Добавление записи в таблицу EventLog.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлено поле UpdateId.
-- v.1.2: Modified by Makarev Andrey 18.04.2008
-- Регистрация событий по нескольким SourceEntityId.
-- v.1.3: Modified by Makarev Andrey 30.04.2008
-- Правильная работа с пустым @sourceEntityIds.
-- =============================================
alter proc [dbo].[RegisterEvent]
  @accountId bigint
  , @ip nvarchar(255)
  , @eventCode nvarchar(100)
  , @sourceEntityIds nvarchar(4000)
  , @eventParams ntext
  , @updateId uniqueidentifier = null
as
BEGIN
IF (ISNULL(@sourceEntityIds,'') = '' )
insert dbo.EventLog
    (
    date
    , accountId
    , ip
    , eventCode
    , sourceEntityId
    , eventParams
    , UpdateId
    )
    VALUES (GETDATE(), @accountId , @ip , @eventCode , null, @eventParams , @updateId) 
ELSE        
  insert dbo.EventLog
    (
    date
    , accountId
    , ip
    , eventCode
    , sourceEntityId
    , eventParams
    , UpdateId
    )
  select
    GetDate()
    , @accountId
    , @ip
    , @eventCode
    , ids.value
    , @eventParams
    , @updateId
  from
    dbo.GetDelimitedValues(@sourceEntityIds) ids

  return 0
end
go

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
  ) 
    
  set @commandText = @commandText +       
    'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year 
    from vw_Examcertificate certificate with (nolock)
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
                ' and certificate.PassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.PassportSeria = @internalPassportSeria '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber '
        end
    end
  
  if not @typographicNumber is null
    set @commandText = @commandText +
      ' and certificate.TypographicNumber = @typographicNumber '
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo

  if not @subjectMarks is null
  begin       
    delete search 
    from @Search search 
    where exists(select 1 
          from [prn].CertificatesMarks certificate_subject with(nolock) 
            inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                and search.CertificateId = certificate_subject.CertificateFK
                and inner_search.CertificateId = search.CertificateId 
            right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                and certificate_subject.Mark = subject_mark.Mark 
          where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)    
    
    
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
      S.CertificateId, 
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.CertificateId is not null then 1 
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
      C.ParticipantID
    from 
      @Search S 
            inner join dbo.vw_Examcertificate C with (nolock) 
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
alter proc [dbo].[SearchCommonNationalExamCertificateCheck]
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
        , CertificateSubjectId uniqueidentifier 
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
        , subject_check.SourceCertificateSubjectIdGuid 
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
        , case when search.CertificateId is not null then 1 else 0 end IsExist 
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
        , search.PassportNumber ' 
      

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
          on unique_cheks.idGUID = search.CertificateId
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
            pivot (count(CertificateSubjectId) for SubjectCode in (<subject_columns>)) as 

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


/*
exec dbo.SearchCommonNationalExamCertificateCheck @login=N'rick_box@mail.ru',@batchId=301638626047,@startRowIndex=N'1',@maxRowCount=N'20'
*/
go
-- получить все уникальные проверки сертификата вузами и их филиалами
alter proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
  @certificateId uniqueidentifier,  -- id сертификата
  @startRow INT = NULL, -- пейджинг, если null - то выбирается кол-во записей для этого сертификата всего
  @maxRow INT = NULL    -- пейджинг
AS
BEGIN
-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
  SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg 
        INNER JOIN vw_Examcertificate c ON c.Number = lg.CertificateNumber
        INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
        INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
        WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0
  RETURN
END
SELECT   @certificateId AS CertificateId, * FROM (
      select org.Id AS OrganizationId,
      org.FullName AS OrganizationFullName,
      lg.[Date] AS [Date],
      lg.IsBatch AS CheckType,
      DENSE_RANK() OVER(ORDER BY org.FullName) AS org
      FROM dbo.CheckCommonNationalExamCertificateLog lg 
          INNER JOIN vw_Examcertificate c ON c.Number = lg.CertificateNumber
        INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
        INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
        WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0) rowTable 
      WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow 
      ORDER BY org, rowTable.[Date] 
END
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
      , SourceCertificateId uniqueidentifier
      , SourceCertificateYear int
      , TypographicNumber nvarchar(255)
      , [Status] nvarchar(255)
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
          when not cne_certificate_request.SourceCertificateIdGuid is null then 1
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
        , case when cne_certificate_request.SourceCertificateIdGuid is null then ''Не найдено'' else 
          case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= 

ed.[ExpireDate] then ''Действительно'' 
          else ''Истек срок'' end end as [Status]
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

cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateIdGuid = cnec.id
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
          left outer join dbo.Subject subject on subject.Id = 

cne_certificate_subject.SubjectCode
          left join [MinimalMark] as mm on cne_certificate_subject.[SubjectCode] = 

mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
        where 
          exists(select 1 
              from @search search
              where cne_certificate_subject.CertificateFK = 

search.SourceCertificateId
                and cne_certificate_subject.[UseYear] = 

search.SourceCertificateYear)
        ' 
    end
    
    set @viewSelectCommandText = 
      N'select
        search.Id 
        , search.BatchId
        , search.CertificateNumber
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
      '

    set @viewCommandText = 
      N' ,unique_cheks.UniqueIHEaFCheck
      from @search search 
      left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on unique_cheks.idGUID = search.SourceCertificateId '

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
          on search.SourceCertificateId = mrk_pvt.CertificateId
          left outer join (select 
            subjects.CertificateId
            , cast(subjects.HasAppeal as int) HasAppeal 
            , subjects.SubjectCode
            from @subjects subjects) subjects
              pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as 

apl_pvt
            on search.SourceCertificateId = apl_pvt.CertificateId '
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
          on search.SourceCertificateId = exam_pvt.CertificateId '
          
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


alter proc [dbo].[SearchCommonNationalExamCertificateWildcard]
  @lastName nvarchar(255) = null
  , @firstName nvarchar(255) = null
  , @patronymicName nvarchar(255) = null
  , @passportSeria nvarchar(255) = null
  , @passportNumber nvarchar(255) = null
  , @typographicNumber nvarchar(255) = null
  , @Number nvarchar(255) = null
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @year int = null
  , @startRowIndex int = 1
  , @maxRowCount int = 20
  , @showCount bit = 0
as
begin
  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(max)
    , @internalPassportSeria nvarchar(255)

  set @eventParams = 
    @lastName + '|' 
    + @firstName + '|' 
    + @patronymicName + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') + '|' 
    + isnull(@Number, '') + '|' 
    + isnull(@typographicNumber, '') + '|' 
    + isnull(cast(@year as varchar(max)), '')

  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  if @year is not null
    select @yearFrom = @year, @yearTo = @year
  else
    select @yearFrom = 2008 --Первые св-ва датируются 2008 годом
    select @yearTo = Year(GetDate())

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  select 
    @commandText = ''
    ,@eventCode = N'CNE_FND_WLDCRD'

  declare @sourceEntityIds nvarchar(4000)  
  declare @Search table 
  ( 
    row int,
    LastName nvarchar(255), 
    FirstName nvarchar(255), 
    PatronymicName nvarchar(255), 
    CertificateId uniqueidentifier, 
    CertificateNumber nvarchar(255),
    RegionId int, 
    PassportSeria nvarchar(255), 
    PassportNumber nvarchar(255), 
    TypographicNumber nvarchar(255),
    Year int,
    primary key(CertificateNumber, row) 
      ) 
      
  if @showCount = 0
  set @commandText = @commandText + 
    ' 
    select top (@startRowIndex+@maxRowCount-1)
        row_number() over (order by c.year, c.id) as row
      , c.LastName 
      , c.FirstName 
      , c.PatronymicName 
      , c.Id 
      , c.Number 
      , c.RegionId 
      , isnull(c.PassportSeria, @internalPassportSeria) PassportSeria 
      , isnull(c.PassportNumber, @passportNumber) PassportNumber
      , c.TypographicNumber 
      , c.Year 
    '
  if @showCount = 1
    set @commandText = ' select count(*) '
  
  set @commandText = @commandText + 
    '
    from dbo.vw_Examcertificate c with (nolock) 
    where 
      c.[Year] between @yearFrom and @yearTo '
  
  if @lastName is not null 
    set @commandText = @commandText + '
      and c.LastName collate cyrillic_general_ci_ai = @lastName'
  if @firstName is not null 
    set @commandText = @commandText + '     
      and c.FirstName collate cyrillic_general_ci_ai = @firstName'
  if @patronymicName is not null 
    set @commandText = @commandText + '           
      and c.PatronymicName collate cyrillic_general_ci_ai = @patronymicName'
  if @internalPassportSeria is not null 
    set @commandText = @commandText + '                 
      and c.InternalPassportSeria = @internalPassportSeria'
  if @passportNumber is not null 
    set @commandText = @commandText + '                       
      and c.PassportNumber = @passportNumber'
  if @typographicNumber is not null 
    set @commandText = @commandText + '                             
      and c.TypographicNumber = @typographicNumber'
  if @Number is not null 
    set @commandText = @commandText + '                                   
      and c.Number = @Number '  
      
  if @showCount = 1     
    exec sp_executesql @commandText
      , N'@lastName nvarchar(255)
        , @firstName nvarchar(255)
        , @patronymicName nvarchar(255)
        , @internalPassportSeria nvarchar(255)
        , @passportNumber nvarchar(255)
        , @typographicNumber nvarchar(255) 
        , @Number nvarchar(255)
        , @yearFrom int, @yearTo int 
        , @startRowIndex int
        , @maxRowCount int'
      , @lastName
      , @firstName
      , @patronymicName
      , @internalPassportSeria
      , @passportNumber
      , @typographicNumber
      , @Number
      , @yearFrom
      , @YearTo
      , @startRowIndex
      , @maxRowCount    
  else
  begin   
    insert into @Search 
    exec sp_executesql @commandText
      , N'@lastName nvarchar(255)
        , @firstName nvarchar(255)
        , @patronymicName nvarchar(255)
        , @internalPassportSeria nvarchar(255)
        , @passportNumber nvarchar(255)
        , @typographicNumber nvarchar(255) 
        , @Number nvarchar(255)
        , @yearFrom int, @yearTo int 
        , @startRowIndex int
        , @maxRowCount int'
      , @lastName
      , @firstName
      , @patronymicName
      , @internalPassportSeria
      , @passportNumber
      , @typographicNumber
      , @Number
      , @yearFrom
      , @YearTo
      , @startRowIndex
      , @maxRowCount    
    
    select 
        search.CertificateNumber 
        , search.LastName LastName 
        , search.FirstName FirstName 
        , search.PatronymicName PatronymicName 
        , search.PassportSeria PassportSeria 
        , search.PassportNumber PassportNumber 
        , search.TypographicNumber TypographicNumber 
        , region.Name RegionName 
        , case 
          when not search.CertificateId is null then 1 
          else 0 
        end IsExist 
        , case 
          when not cne_certificate_deny.Id is null then 1 
          else 0 
        end IsDeny 
        , cne_certificate_deny.Comment DenyComment 
        , cne_certificate_deny.NewCertificateNumber 
        , search.[Year] 
        , case when ed.[ExpireDate] is null then 'Не найдено'  
             when cne_certificate_deny.Id is not null then 'Аннулировано' 
             when getdate() <= ed.[ExpireDate] then 'Действительно'
             else 'Истек срок' 
          end as [Status]
        , unique_cheks.UniqueIHEaFCheck
       from @Search search
        left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on unique_cheks.idGUID = search.CertificateId 
        left outer join dbo.CommonNationalExamCertificateDeny cne_certificate_deny with (nolock) 
          on cne_certificate_deny.[Year] between @yearFrom and @yearTo 
            and search.CertificateNumber = cne_certificate_deny.CertificateNumber 
        left outer join dbo.Region region with (nolock) 
          on region.[Id] = search.RegionId 
        left join [ExpireDate] ed on  ed.[year] = search.[year] 
       where row between @startRowIndex and (@startRowIndex+@maxRowCount-1)
       
       
       exec dbo.RegisterEvent 
        @accountId = @editorAccountId  
        , @ip = @ip 
        , @eventCode = @eventCode 
        , @sourceEntityIds = '0'
        , @eventParams = @eventParams 
        , @updateId = null 
  end
        
  return 0
end
go
alter proc [dbo].[SelectCNECCheckHystoryByOrgId]
  @idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
  @LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
  declare @str nvarchar(max),@ss nvarchar(10),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max),@yearFrom int, @yearTo int
  set @pf=@pf-1

  create table #tt(id [int] NOT NULL primary key)
  create table #t1(id [int] NOT NULL primary key)
    
  select @yearFrom = 2008, @yearTo = Year(GetDate())
        
  if @isUnique =0 
  begin
    if @fld='id'  
    begin
      if @so = 0 
      begin
        insert #tt
        select c.id 
        from Account c
          join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
        where d.id=@idorg and d.DisableLog=0
        
        insert #t1
        select top (@pf) cb.id 
        from CNEWebUICheckLog cb
          join #tt Acc on acc.id=cb.AccountId     
        order by cb.Id asc    
        
        select * from
        (
          select *,row_number() over (order by id asc) rn from
            (       
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
               ISNULL(c.PassportSeria,'')+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
                ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                ) as Marks, 
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGuid        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = c.CertificateNumber 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end
            order by c.Id asc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = r.Number 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id asc       
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks      
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=c.id
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID  
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                              
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join CommonNationalExamcertificateDeny certificate_deny 
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = t.CertificateNumber        
            ) t
        ) t
        where rn between @ps and @pf    
      end
      else  
      begin 
        insert #tt
        select c.id 
        from Account c
          join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
        where d.id=@idorg and d.DisableLog=0
      
        insert #t1
        select top (@pf) cb.id
        from CNEWebUICheckLog cb
          join #tt Acc on acc.id=cb.AccountId     
        order by cb.Id desc   
        
        select * from
        (
          select *,row_number() over (order by id desc) rn from
            (       
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
               ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
                ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                ) as Marks,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = c.CertificateNumber 
            where @idorg=Acc.OrganizationId     
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end           
            order by c.Id desc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed ON c.SourceCertificateYear = ed.[Year] 
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = r.Number 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id desc        
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks 
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id    
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join CommonNationalExamcertificateDeny certificate_deny 
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = t.CertificateNumber        
            ) t
        ) t
        where rn between @ps and @pf      
      end 
    end
    if @fld='TypeCheck'
    begin
      if @so = 0 
      begin
        insert #tt
        select c.id 
        from Account c
          join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
        where d.id=@idorg and d.DisableLog=0
        
        insert #t1
        select top (@pf) cb.id
        from CNEWebUICheckLog cb
          join #tt Acc on acc.id=cb.AccountId     
        order by cb.Id asc    
        
        select * from
        (
          select *,row_number() over (order by TypeCheck asc) rn from
            (       
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
               ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
                ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                ) as Marks,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = c.CertificateNumber 
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.Id asc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
              null as Marks, 
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = r.Number 
            where @idorg=Acc.OrganizationId     
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end           
            order by c.Id asc       
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks    
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id    
                left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID                
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join CommonNationalExamcertificateDeny certificate_deny 
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = t.CertificateNumber      
            ) t
        ) t
        where rn between @ps and @pf    
      end
      else  
      begin 
        insert #tt
        select c.id 
        from Account c
          join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
        where d.id=@idorg and d.DisableLog=0
      
        insert #t1
        select top (@pf) cb.id 
        from CNEWebUICheckLog cb
          join #tt Acc on acc.id=cb.AccountId     
        order by cb.Id desc   
        
        select * from
        (
          select *,row_number() over (order by TypeCheck desc) rn from
            (       
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
               ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
                ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                ) as Marks,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = c.CertificateNumber 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id desc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = r.Number 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id desc        
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks     
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id    
                left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID                
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join CommonNationalExamcertificateDeny certificate_deny 
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = t.CertificateNumber        
            ) t
        ) t
        where rn between @ps and @pf      
      end   
    end     
    if @fld='LastName'  
    begin
      if @so = 0 
      begin
        insert #tt
        select c.id 
        from Account c
          join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
        where d.id=@idorg and d.DisableLog=0
        
        insert #t1
        select top (@pf) cb.id
        from CNEWebUICheckLog cb
          join #tt Acc on acc.id=cb.AccountId     
        order by cb.LastName asc    
        
        select * from
        (
          select *,row_number() over (order by LastName asc) rn from
            (       
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
               ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
                ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                ) as Marks,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = c.CertificateNumber 
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.LastName asc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = r.Number 
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.LastName asc       
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks           
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id    
                left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID              
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join CommonNationalExamcertificateDeny certificate_deny 
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = t.CertificateNumber        
            ) t
        ) t
        where rn between @ps and @pf    
      end   
      else
      begin
        insert #tt
        select c.id 
        from Account c
          join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
        where d.id=@idorg and d.DisableLog=0
        
        insert #t1
        select top (@pf) cb.id 
        from CNEWebUICheckLog cb
          join #tt Acc on acc.id=cb.AccountId     
        order by cb.LastName desc   
        
        select * from
        (
          select *,row_number() over (order by LastName desc) rn from
            (       
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
               ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
                ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                ) as Marks,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = c.CertificateNumber 
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.LastName desc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = r.Number 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.LastName desc        
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.Id>0 then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+ ' '+c.PassportNumber, cb.PassportSeria + ' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks         
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id    
                left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID                
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join CommonNationalExamcertificateDeny certificate_deny 
                on certificate_deny.[Year] between @yearFrom and @yearTo
                  and certificate_deny.certificateNumber = t.CertificateNumber        
            ) t
        ) t
        where rn between @ps and @pf    
      end 
    end         
  end         
  else
  begin
    if @fld='id'  
    begin
      set @fldso1='c.Id'
      set @fldso2='c.Id'    
      set @fldso3='cb.Id'       
    end 
    if @fld='TypeCheck'
    begin
      set @fldso1='c.Id'
      set @fldso2='c.Id'
      set @fldso3='cb.Id'       
    end     
  
    if @fld='LastName'  
    begin
      set @fldso1='c.LastName'
      set @fldso2='c.LastName'
      set @fldso3='c.LastName'        
    end     
      
    if @so=1 
      set @ss=' desc'
    else
      set @ss=' asc'  
  
    set @str='
        declare @yearFrom int, @yearTo int    
        select @yearFrom = 2008, @yearTo = Year(GetDate())
        
        select * into #ttt
        from 
        (           
          select c.Id,cb.CreateDate,''Пакетная'' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
             ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + ''=''
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      '','', ''.'') + '','' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''''),
                                      TYPE
                                ) marks
                ) as Marks,
             case when ed.[ExpireDate] is null then ''Не найдено''  
              when certificate_deny.Id>0 then ''Аннулировано'' 
              when getdate() <= ed.[ExpireDate] then ''Действительно''
            else ''Истек срок'' end Status
          FROM 
            (select min(c.id) id,c.CertificateNumber 
             from CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id '
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '                           
    if @idorg<>-1 
      set @str=@str+'           
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'' '     
    set @str=@str+'               
            group by CertificateNumber) cb1 
                              
            join CommonNationalExamCertificateCheck c on cb1.id=c.id                              
            JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id                
            left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CNE.Id      
            left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID
            left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
            left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
              on certificate_deny.[Year] between @yearFrom and @yearTo
                and certificate_deny.certificateNumber = c.CertificateNumber            
          union all
          select c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
            ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
            null as Marks,
            case WHEN c.SourceCertificateIdGuid IS NULL THEN ''Не найдено'' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
                else ''Истек срок'' end end  STATUS
          FROM 
            (select min(c.id) id, r.Number 
            from CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
              left JOIN CommonNationalExamCertificate r ON c.SourceCertificateIdGuid=r.Id '
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '               
    if @idorg<>-1 
      set @str=@str+'               
                JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'''                    
    set @str=@str+'               
            group by r.Number
            ) cb1
            
            join CommonNationalExamCertificateRequest c on cb1.id=c.id   
            JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id             
            left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
            left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID        
            left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
            left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
              on certificate_deny.[Year] between @yearFrom and @yearTo
                and certificate_deny.certificateNumber = r.Number '
    set @str=@str+'   
                  
          union all
          select t.*,
            case when ed.[ExpireDate] is null then ''Не найдено''  
              when certificate_deny.Id>0 then ''Аннулировано'' 
              when getdate() <= ed.[ExpireDate] then ''Действительно''
            else ''Истек срок'' end Status
          from 
          (
            select cb.id,cb.EventDate,''Интерактивная'' TypeCheck, ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+'' ''+c.PassportNumber, cb.PassportSeria +'' ''+ cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks      
            from 
            (
              select max(cb.id) id,c.Number from
              (
                select cb.id,cb.FoundedCNEId
                from CNEWebUICheckLog cb  with(index(I_CNEWebUICheckLog_AccId))
                  join Account Acc with(index(accOrgIdIndex)) on acc.id=cb.AccountId  
                  join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 ' 
    if @TypeCheck= 'Интерактивная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '                   
    if @idorg<>-1 
      set @str=@str+'
                where @idorg=Acc.OrganizationId '               
    set @str=@str+'   
              ) cb
              left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id            
              group by c.Number 
            ) cb1
              join CNEWebUICheckLog cb on cb1.id=cb.id
              left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id '                
    set @str=@str+'               
              left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'''
    set @str=@str+'             
                  
          ) t
            left join [ExpireDate] as ed on ed.[Year] = t.[Year]
            left join CommonNationalExamcertificateDeny certificate_deny 
              on certificate_deny.[Year] between @yearFrom and @yearTo
                and certificate_deny.certificateNumber = t.CertificateNumber        
          ) t
          
    select * from
    (
      select *,row_number() over (order by '+@fld+@ss+') rn from
        (
        select a.* from #ttt a
          join (select min(id)id,CertificateNumber from #ttt group by CertificateNumber) b on a.id=b.id
        ) t
    ) t
    where rn between @ps and @pf
    order by rn
    
    drop table #ttt '
    
  end
  
  --print  @str
  
  exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255)',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName
  
  drop table #tt
  drop table #t1  
end

GO
alter proc [dbo].[SelectCNECCheckHystoryByOrgIdCount]
  @idorg int, @isUnique bit=0,@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
  declare @str nvarchar(max),@s nvarchar(100)
  
  set @s=cast(newid() as nvarchar(100))
  if @isUnique =0 
  begin
    set @str='
      create table #tt(id [int] NOT NULL primary key)
      create table #t1(id [int] NOT NULL primary key)
      
      insert #tt
      select c.id 
      from Account c
        join Organization2010 d on d.id=c.OrganizationId '
    if @idorg<>-1 
      set @str=@str+'
        where d.id=@idorg and d.DisableLog=0 '
        
    set @str=@str+'   
      insert #t1
      select cb.id 
      from CNEWebUICheckLog cb
        join #tt Acc on acc.id=cb.AccountId     
      order by cb.Id asc      
    
    select count(*) from
        (
        select 1 t
        FROM CommonNationalExamCertificateCheck c
          JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
          JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
    if @LastName is not null
      set @str=@str+'
              and c.LastName like ''%''+@LastName+''%'' '           
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 ' 
    if @idorg<>-1 
      set @str=@str+'
        where @idorg=Acc.OrganizationId '                         
    set @str=@str+' 
        union all
        select 1 t 
        FROM CommonNationalExamCertificateRequest c 
          JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
          left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id
          JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
    if @LastName is not null
      set @str=@str+'
              and c.LastName like ''%''+@LastName+''%'' '           
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '             
    if @idorg<>-1 
      set @str=@str+'
        where @idorg=Acc.OrganizationId '
    set @str=@str+'
        union all
        select 1 t
        from #t1 cb1
          join CNEWebUICheckLog cb on cb1.id=cb.id '
          
    if @TypeCheck= 'Интерактивная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '
    set @str=@str+'                         
          left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id  where 1 = 1 ' 
    if @LastName is not null
      set @str=@str+'
        and c.LastName like ''%''+@LastName+''%'' '                       
    set @str=@str+'       
        ) t
    drop table #tt
    drop table #t1        
        '
  end
  else
  begin
    set @str='
        declare @yearFrom int, @yearTo int    
        select @yearFrom = 2008, @yearTo = Year(GetDate())
        
        select * into #ttt
        from 
        (           
          select  c.Id,c.CertificateNumber
          FROM 
            (select min(c.id) id,c.CertificateNumber 
             from CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id '
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '                           
    if @idorg<>-1 
      set @str=@str+'           
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'' '     
    set @str=@str+'               
            group by CertificateNumber) cb1 
                              
            join CommonNationalExamCertificateCheck c on cb1.id=c.id                              
            JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id                
          union all
          select c.Id,cb1.Number CertificateNumber
          FROM 
            (select min(c.id) id, r.Number 
            from CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=r.Id '
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '               
    if @idorg<>-1 
      set @str=@str+'               
                JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'''                    
    set @str=@str+'               
            group by r.Number
            ) cb1
            
            join CommonNationalExamCertificateRequest c on cb1.id=c.id   
            JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id'
    set @str=@str+'   
          union all
          select t.*
          from 
          (
            select cb.id,ISNULL(c.Number, cb.CNENumber) CertificateNumber
            from 
            (
              select max(cb.id) id,c.Number from
              (
                select cb.id,cb.FoundedCNEId
                from CNEWebUICheckLog cb  with(index(I_CNEWebUICheckLog_AccId))
                  join Account Acc with(index(accOrgIdIndex)) on acc.id=cb.AccountId  
                  join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 ' 
    if @TypeCheck= 'Интерактивная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '                   
    if @idorg<>-1 
      set @str=@str+'
                where @idorg=Acc.OrganizationId '               
    set @str=@str+'   
              ) cb
              left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id            
              group by c.Number 
            ) cb1
              join CNEWebUICheckLog cb on cb1.id=cb.id
              left join vw_Examcertificate c ON cb.FoundedCNEId=c.Id '                
    set @str=@str+'               
              left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'''
    set @str=@str+'             
          ) t
          ) t
          
    select count(distinct CertificateNumber) from
    (
      select * from
        (
        select a.* from #ttt a
          join (select min(id)id,CertificateNumber from #ttt group by CertificateNumber) b on a.id=b.id
        ) t
    ) t
    
    drop table #ttt '
    
  end
  print @str
  exec sp_executesql @str,N'@idorg int,@LastName nvarchar(255)=null',@idorg=@idorg,@LastName=@LastName
    
end

go
alter PROC [dbo].[usp_cne_AddCheckBatchResult]
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
            item.ref.value('@SourceCertificateId', 'uniqueidentifier') AS SourceCertificateId,
            item.ref.value('@IsDeny', 'bit') AS IsDeny,
            item.ref.value('@DenyComment', 'nvarchar(max)') AS DenyComment,
            item.ref.value('@DenyNewCertificateNumber', 'nvarchar(255)') AS DenyNewCertificateNumber,
            item.ref.value('@Year', 'int') AS Year,
            item.ref.value('@TypographicNumber', 'nvarchar(255)') AS TypographicNumber,
            item.ref.value('@RegionId', 'int') AS RegionId,
            item.ref.value('@PassportSeria', 'nvarchar(255)') AS PassportSeria,
            item.ref.value('@PassportNumber', 'nvarchar(255)') AS PassportNumber,
            item.ref.value('@UniqueCheckId', 'bigint') AS UniqueCheckId,
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
          inner join #check on #check.UniqueCheckId = ex.Id
                     AND #check.[UniqueCheckYear] = ex.[year]
          WHERE #check.UniqueCheckId > 0
      -- добавление статистики для еще не проверенных св-в
      insert    ExamCertificateUniqueChecks
          (
            Id,
            [year],
            UniqueIHEaFCheck
          )

          select  DISTINCT #check.UniqueCheckId,
                    #check.UniqueCheckYear,
                        #check.UniqueIHEaFCheck
          from    #check
              left join ExamCertificateUniqueChecks ex on ex.Id = #check.UniqueCheckId
                                    AND #check.[UniqueCheckYear] = ex.[year]
          where   ex.Id is NULL AND #check.UniqueCheckId is not null

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
                          idtemp
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
                                a.id
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
                ( SELECT    COUNT(SourceCertificateIdGuid)
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
go

-- =============================================
-- Author:    Yusupov K.I.
-- Create date: 04-06-2010
-- Description: Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
alter PROCEDURE [dbo].[AddCNEWebUICheckEvent]
@AccountLogin NVARCHAR(255),          -- логин проверяющего
  @LastName NVARCHAR(255)=null,       -- фамилия сертифицируемого
  @FirstName NVARCHAR(255)=null,        -- имя сертифицируемого
  @PatronymicName NVARCHAR(255)=null,     -- отчетсво сертифицируемого
  @PassportSeria NVARCHAR(20)=NULL,     -- серия документа сертифицируемого (паспорта)
  @PassportNumber NVARCHAR(20)=NULL,      -- номер документа сертифицируемого (паспорта)
  @CNENumber NVARCHAR(20)=NULL,       -- номер сертификата
  @TypographicNumber NVARCHAR(20)=NULL,   -- типографический номер сертификата 
  @RawMarks NVARCHAR(500)=null,       -- средние оценки по предметам (через запятую, в определенном порядке)
  @IsOpenFbs bit=null,
  @EventId INT output             -- id зарегистрированного события
AS
BEGIN
  IF (SELECT disablelog 
    FROM dbo.Organization2010 
     WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Login = @AccountLogin)) = 1
  BEGIN
  SELECT @EventId = NULL
  RETURN
  END
  
  IF 
  (
    @TypographicNumber IS NULL AND
    @CNENumber IS NULL AND
    @PassportNumber IS NULL AND
    @RawMarks IS NULL
  )
  BEGIN
    RAISERROR (N'Не указаны паспортные данные, типографский номер, номер свидетельства и баллы по предметам одновременно',10,1)
    RETURN
  END

  DECLARE @AccountId BIGINT
  SELECT
    @AccountId = Acc.[Id]
  FROM
    dbo.Account Acc WITH (nolock, fastfirstrow)
  WHERE
    Acc.[Login] = @AccountLogin 

  IF (@TypographicNumber IS NOT NULL)
  BEGIN
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,TypographicNumber) 
        VALUES 
      (@AccountId,'Typographic',@FirstName,@LastName,@PatronymicName,@TypographicNumber)
  END
  ELSE IF (@CNENumber IS NOT NULL)
  BEGIN
    declare @logTypeNumber nvarchar(50)
    set @logTypeNumber = 'CNENumber' + case when isnull(@IsOpenFbs,0) = 1 then 'Open' else '' end
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,CNENumber,Marks) 
        VALUES 
      (@AccountId,@logTypeNumber,@FirstName,@LastName,@PatronymicName,@CNENumber,@RawMarks)
  END
  ELSE IF (@PassportNumber IS NOT NULL)
  BEGIN
    declare @logTypePassport nvarchar(50)
    set @logTypePassport = 'Passport' + case when isnull(@IsOpenFbs,0) = 1 then 'Open' else '' end

    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,PassportSeria,PassportNumber,Marks) 
        VALUES 
      (@AccountId,@logTypePassport,@FirstName,@LastName,@PatronymicName,@PassportSeria,@PassportNumber,@RawMarks)
  END
  ELSE IF (@RawMarks IS NOT NULL)
  BEGIN
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,Marks) 
        VALUES 
      (@AccountId,'Marks',@FirstName,@LastName,@PatronymicName,@RawMarks)
  END
    SELECT @EventId = @@Identity
END
go
alter proc [dbo].[CheckCommonNationalExamCertificateByNumber]
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
  
  declare @certificate_check table
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
    , case
      when @ParticipantID is null and [certificate].Id is not null then 1
      when @ParticipantID is not null and certificate.ParticipantID is not null then 1      
      else 0
    end IsExist
    , [certificate].Id
    , case
      when certificate_deny.Id>0 then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Comment
    , certificate_deny.NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , certificate.ParticipantID
  from 
    (select null ''empty'') t left join 
    (SELECT b.LicenseNumber AS Number, a.Surname AS FirstName, a.Name AS LastName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) [certificate] on 
        [certificate].[Year] between @yearFrom and @yearTo '
  if @ParticipantID is not null 
    set @sql = @sql + ' and [certificate].ParticipantID = @ParticipantID'   
  if @number is not null 
    set @sql = @sql + ' and [certificate].Number = @number'
  if @CheckTypographicNumber is not null 
    set @sql = @sql + ' and [certificate].TypographicNumber=@CheckTypographicNumber'
  
  set @sql = @sql + '     
    left outer join dbo.Region region
      on region.Id = [certificate].RegionId
    left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[Year] between @yearFrom and @yearTo
        and certificate_deny.certificateNumber = [certificate].Number'

  insert into @certificate_check    
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
    isnull(cast(certificate_check.certificateId as nvarchar(250)),'Нет свидетельства' ) certificateId
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
    , case when check_subject.CheckSubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',') CheckSubjectMark
    , case when check_subject.SubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',') SubjectMark
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
    , case when ed.[ExpireDate] is null then 'Не найдено' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
      case when getdate() <= ed.[ExpireDate] then 'Действительно'
      else 'Истек срок' end end end as [Status],
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
  from @certificate_check certificate_check
      join ((SELECT b.LicenseNumber AS Number, a.Surname AS FirstName, a.Name AS LastName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID)) C
          on 1= case when @ParticipantID is null and C.Id = certificate_check.certificateId then 1
                  when @ParticipantID is not null and C.ParticipantID=certificate_check.ParticipantID then 1 
             else 0 end
        left outer join ExamcertificateUniqueChecks CC
      on CC.IdGuid = C.Id
    left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
    left outer join (
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
        , isnull(check_subject.SubjectId, certificate_subject.SubjectCode) SubjectId
        , check_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when check_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal,
        certificate_check.certificateId,
        certificate_subject.ParticipantFK ParticipantID
      from [prn].CertificatesMarks certificate_subject with (nolock)         
        inner join @certificate_check certificate_check
          on certificate_check.[Year] = certificate_subject.UseYear
            and 1= case when @ParticipantID is null and certificate_check.certificateId = certificate_subject.CertificateFK then 1
                        when @ParticipantID is not null and certificate_subject.ParticipantFK = certificate_check.ParticipantID then 1
                    else 0 end
        left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
          on check_subject.SubjectId = certificate_subject.SubjectCode
            ) check_subject
      left outer join dbo.[Subject] [subject]
        on [subject].Id = check_subject.SubjectId
        
      on 1 = case when @ParticipantID is null 
                      and isnull(certificate_check.certificateId, '76A02C33-9A91-443A-996C-7640F7481B55') = isnull(check_subject.certificateId,'76A02C33-9A91-443A-996C-7640F7481B55') then 1
                  when @ParticipantID is not null 
                      and C.ParticipantID=check_subject.ParticipantID then 1
             else 0 end                 
                  
      left join [MinimalMark] as mm on check_subject.SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year]    
  
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
alter proc [dbo].[CheckCommonNationalExamCertificateByNumber]
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
  
  declare @certificate_check table
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
    , case
      when @ParticipantID is null and [certificate].Id is not null then 1
      when @ParticipantID is not null and certificate.ParticipantID is not null then 1      
      else 0
    end IsExist
    , [certificate].Id
    , case
      when certificate_deny.Id>0 then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Comment
    , certificate_deny.NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , certificate.ParticipantID
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
  if @CheckTypographicNumber is not null 
    set @sql = @sql + ' and [certificate].TypographicNumber=@CheckTypographicNumber'
  
  set @sql = @sql + '     
    left outer join dbo.Region region
      on region.Id = [certificate].RegionId
    left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[Year] between @yearFrom and @yearTo
        and certificate_deny.certificateNumber = [certificate].Number'

  insert into @certificate_check    
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
    isnull(cast(certificate_check.certificateId as nvarchar(250)),'Нет свидетельства' ) certificateId
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
    , case when check_subject.CheckSubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',') CheckSubjectMark
    , case when check_subject.SubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',') SubjectMark
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
    , case when ed.[ExpireDate] is null then 'Не найдено' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
      case when getdate() <= ed.[ExpireDate] then 'Действительно'
      else 'Истек срок' end end end as [Status],
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
  from @certificate_check certificate_check
      join ((SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID)) C
          on 1= case when @ParticipantID is null and C.Id = certificate_check.certificateId then 1
                  when @ParticipantID is not null and C.ParticipantID=certificate_check.ParticipantID then 1 
             else 0 end
        left outer join ExamcertificateUniqueChecks CC
      on CC.IdGuid = C.Id
    left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
    left outer join (
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
        , isnull(check_subject.SubjectId, certificate_subject.SubjectCode) SubjectId
        , check_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when check_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal,
        certificate_check.certificateId,
        certificate_subject.ParticipantFK ParticipantID
      from [prn].CertificatesMarks certificate_subject with (nolock)         
        inner join @certificate_check certificate_check
          on certificate_check.[Year] = certificate_subject.UseYear
            and 1= case when @ParticipantID is null and certificate_check.certificateId = certificate_subject.CertificateFK then 1
                        when @ParticipantID is not null and certificate_subject.ParticipantFK = certificate_check.ParticipantID then 1
                    else 0 end
        left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
          on check_subject.SubjectId = certificate_subject.SubjectCode
            ) check_subject
      left outer join dbo.[Subject] [subject]
        on [subject].Id = check_subject.SubjectId
        
      on 1 = case when @ParticipantID is null 
                      and isnull(certificate_check.certificateId, '76A02C33-9A91-443A-996C-7640F7481B55') = isnull(check_subject.certificateId,'76A02C33-9A91-443A-996C-7640F7481B55') then 1
                  when @ParticipantID is not null 
                      and C.ParticipantID=check_subject.ParticipantID then 1
             else 0 end                 
                  
      left join [MinimalMark] as mm on check_subject.SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year]    
  
  
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

create proc [dbo].[SearchCommonNationalExamCertificatePassport]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
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
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') 

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
  set @eventCode = N'CNE_FND_TN'

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
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year, ParticipantID 
    from 
    (
      select b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
            from rbd.Participants a with (nolock)
        left join prn.Certificates b with (nolock) on b.ParticipantFK = a.ParticipantID
        left join prn.CancelledCertificates c on c.CertificateFK=b.CertificateID
    ) certificate
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
                ' and certificate.PassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.PassportSeria = @internalPassportSeria '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber '
        end
    end
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @yearFrom, @YearTo

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
      isnull(cast(S.CertificateId as nvarchar(500)),'Нет свидетельства')CertificateId, 
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
-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null   -- если > 0, то выбирается общее кол-во
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
          , (select count(SourceCertificateIdGuid) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
        from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
        left join account a on a.id = b.OwnerAccountId 
        where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) ' +
        'where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 

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
      from @search s ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId

  return 0
end
go
-- exec dbo.SearchCommonNationalExamCertificateRequestBatch

-- =============================================
-- Получить список пакетных запросов сертификатов.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateRequestBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
  , @isTypographicNumber bit = 0
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(max)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  set @accountId = isnull(
    (select account.[Id] 
     from dbo.Account account with (nolock, fastfirstrow) 
     where account.[Login] = @login), 0)

  if exists ( select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login )
    set @accountId = null

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
        , Year int
        , Total int
        , Found int
      ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> 
          rb.Id 
          , rb.CreateDate 
          , rb.IsProcess  
          , rb.IsCorrect  
          , a.login 
          , rb.year
          , (select count(*) from CommonNationalExamCertificateRequest r with(nolock) where r.batchid = rb.id) Total
          , (select count(SourceCertificateIdGuid) from CommonNationalExamCertificateRequest r with(nolock) where r.batchid = rb.id) Found
        from dbo.CommonNationalExamCertificateRequestBatch rb with (nolock) 
        left join account a on a.id = rb.OwnerAccountId 
        where rb.OwnerAccountId = isnull(@accountId, rb.OwnerAccountId) 
        and rb.IsTypographicNumber = @isTypographicNumber '
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateRequestBatch rb with (nolock) ' +
        'where rb.OwnerAccountId = isnull(@accountId, rb.OwnerAccountId) ' +
        'and rb.IsTypographicNumber = @isTypographicNumber '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'CreateDate'
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
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
        , s.Year
        , s.Total
        , s.Found
      from @search s ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint, @isTypographicNumber bit'
    , @accountId
    , @isTypographicNumber

  return 0
end
