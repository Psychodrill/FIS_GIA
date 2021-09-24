insert into Migrations(MigrationVersion, MigrationName) values (95, '095_2013_06_26_fbs.sql')
go
CREATE view [dbo].[vw_Examcertificate1]
as
select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
				   COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
				   COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
				   a.ParticipantID AS ParticipantID
            from rbd.Participants a with (nolock)				
				left join prn.CertificatesMarks cm on cm.ParticipantFK=a.ParticipantID	
				left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK
				left join prn.CancelledCertificates c on c.CertificateFK=b.CertificateID
GO


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
		ParticipantFK uniqueidentifier,
		primary key(row) 
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
			, c.ParticipantID
		'
	if @showCount = 1
		set @commandText = ' select count(*) '
	
	set @commandText = @commandText + 
		'
		from vw_Examcertificate1 c  
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
			and c.PassportSeria = @internalPassportSeria'
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
				isnull(cast(search.CertificateNumber as nvarchar(250)),'Нет свидетельства' ) CertificateNumber
				, search.LastName LastName 
				, search.FirstName FirstName 
				, search.PatronymicName PatronymicName 
				, search.PassportSeria PassportSeria 
				, search.PassportNumber PassportNumber 
				, search.TypographicNumber TypographicNumber 
				, region.Name RegionName 
				, case
					when search.CertificateId is not null or search.ParticipantFK is not null then 1
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
				, unique_cheks.UniqueIHEaFCheck,
				search.ParticipantFK ParticipantID
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

alter proc [dbo].[SearchCommonNationalExamCertificatePassport]
	@lastName nvarchar(255) = null			  -- фамилия сертифицируемого
	, @firstName nvarchar(255) = null		  -- имя сертифицируемого
	, @patronymicName nvarchar(255) = null	  -- отчетсво сертифицируемого
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
	, @login nvarchar(255)					  -- логин проверяющего
	, @ip nvarchar(255)						  -- ip проверяющего
	, @year int = null					      -- год выдачи сертификата
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
			vw_Examcertificate1 certificate				
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
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
alter proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]	 	 
	 @subjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
	, @shouldWriteLog BIT = 1				  -- нужно ли записывать в лог интерактивных проверок	
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
		from vw_Examcertificate1 certificate
		where 
		' 
	if @ParticipantID is not null 
		set @commandText = @commandText + '	and certificate.ParticipantID = @ParticipantID'		
				
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
			case	when CD.UseYear is not null then 1 end IsDeny,  
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
alter proc [dbo].[CheckCommonNationalExamCertificateByNumber]
	 @number nvarchar(255) = null				-- номер сертификата
	, @checkLastName nvarchar(255) = null		-- фамилия сертифицируемого
	, @checkFirstName nvarchar(255) = null		-- имя сертифицируемого
	, @checkPatronymicName nvarchar(255) = null -- отчетсво сертифицируемого
	, @checkSubjectMarks nvarchar(max) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
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
		(select null ''empty'') t 
		left join 
			vw_Examcertificate1 [certificate] on 
				[certificate].[Year] between @yearFrom and @yearTo '
	if @ParticipantID is not null 
		set @sql = @sql + '	and [certificate].ParticipantID = @ParticipantID'		
	if @number is not null 
		set @sql = @sql + '	and case when @number='''' and [certificate].Number is null then 1 when [certificate].Number=@number then 1 else 0 end = 1 '
	if @CheckTypographicNumber is not null 
		set @sql = @sql + '	and [certificate].TypographicNumber=@CheckTypographicNumber'
	
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
    	join vw_Examcertificate1 C
        	on 1= case when @ParticipantID is null and C.Id = certificate_check.certificateId then 1
        	        when @ParticipantID is not null and C.ParticipantID=certificate_check.ParticipantID then 1 
        	   else 0 end
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
				, isnull(check_subject.SubjectId, certificate_subject.SubjectCode) SubjectId
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
				inner join @certificate_check certificate_check
					on certificate_check.[Year] = certificate_subject.UseYear
						and 1= case when @ParticipantID is null and certificate_check.certificateId = certificate_subject.CertificateFK then 1
						            when @ParticipantID is not null and certificate_subject.ParticipantFK = certificate_check.ParticipantID then 1
						        else 0 end
				left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
					on check_subject.SubjectId = certificate_subject.SubjectCode
			) check_subject			
			on 1 = case when @ParticipantID is null 
			                and certificate_check.certificateId = check_subject.certificateId then 1
			            when @ParticipantID is not null 
			                and C.ParticipantID=check_subject.ParticipantID and @number <> '' and check_subject.certificateId=C.id then 1
			            when @ParticipantID is not null 
			                and C.ParticipantID=check_subject.ParticipantID and @number = '' and check_subject.certificateId<>C.id then 1
			       else 0 end
			left outer join dbo.[Subject] [subject]	on [subject].Id = check_subject.SubjectId			       
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