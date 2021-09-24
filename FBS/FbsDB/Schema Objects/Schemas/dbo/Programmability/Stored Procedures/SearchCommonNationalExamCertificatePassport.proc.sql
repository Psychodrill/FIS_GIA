ALTER proc [dbo].[SearchCommonNationalExamCertificatePassport]
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
    'select * from 
     (
		select distinct a.Surname, a.Name, a.SecondName, b.CertificateID, b.LicenseNumber,
            b.Region, isnull(a.DocumentSeries, @internalPassportSeria) DocumentSeries, 
            isnull(a.DocumentNumber, @passportNumber) DocumentNumber, b.TypographicNumber, isnull(b.UseYear,a.UseYear) UseYear, a.ParticipantID 
		from 
			rbd.Participants AS a WITH (nolock) 
			JOIN prn.CertificatesMarks AS cm WITH (nolock) ON cm.ParticipantFK = a.ParticipantID AND cm.UseYear = a.UseYear 
			LEFT JOIN prn.Certificates AS b WITH (nolock) ON b.CertificateID = cm.CertificateFK AND b.UseYear = cm.UseYear 
			LEFT JOIN prn.CancelledCertificates AS c WITH (nolock) ON c.CertificateFK = b.CertificateID AND c.UseYear = b.UseYear   
		where a.[UseYear] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and a.Surname collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and a.Name collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and a.SecondName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentSeries like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and a.DocumentSeries = @internalPassportSeria '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentNumber like @passportNumber '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and a.DocumentNumber = @passportNumber '
        end
    end
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  set @commandText = @commandText + 
  ' ) t'
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
        when CD.UseYear is not null then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
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
      S.ParticipantID ParticipantID
    from 
      @Search S 
		left outer join ExamCertificateUniqueChecks CC with (nolock) on CC.idGUID  = S.CertificateId 
		left outer join prn.CancelledCertificates CD with (nolock) on CD.[UseYear] = S.[year] and S.CertificateId = CD.CertificateFK
		left join dbo.Region region with (nolock) on region.[Id] = S.RegionId 
		left join [ExpireDate] ed on ed.[year] = S.[year]
            
  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null 
      
  return 0
end