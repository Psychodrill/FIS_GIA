insert into Migrations(MigrationVersion, MigrationName) values (90, '090_2013_06_14_fbs.sql')

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[SearchCommonNationalExamCertificate]
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
    from    
      (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
          b.TypographicNumber, a.ParticipantID, b.CreateDate
       FROM rbd.Participants a with (nolock, fastfirstrow) 
        left JOIN prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) [certificate] 
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

  if not @subjectMarks is null
  begin       
    delete search 
    from @Search search 
    where exists(select 1 
          from [prn].CertificatesMarks certificate_subject with(nolock) 
            inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                and search.ParticipantID = certificate_subject.ParticipantFK
                and inner_search.ParticipantID = search.ParticipantID 
            right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                and certificate_subject.Mark = subject_mark.Mark 
          where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)    
       --and CertificateId is not null    
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
      isnull(cast(S.certificateId as nvarchar(250)),'Нет свидетельства' ) certificateId,
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
        when isnull(CD.UseYear,0) <> 0 then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when isnull(CD.UseYear,0) <> 0 then 'Аннулировано' 
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
      left outer join prn.CancelledCertificates CD with (nolock) 
        on CD.[UseYear] between @yearFrom and @yearTo 
                and S.CertificateId = CD.CertificateFK 
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
