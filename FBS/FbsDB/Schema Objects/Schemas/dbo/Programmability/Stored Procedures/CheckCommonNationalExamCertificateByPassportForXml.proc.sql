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
						isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year, ParticipantID
		from (
			select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
				   COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
				   COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
				   a.ParticipantID AS ParticipantID
            from rbd.Participants a with (nolock)				
				join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID and cm.useyear=a.useyear
				left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and b.useyear=a.useyear
				left join prn.CancelledCertificates c with (nolock) on c.CertificateFK=b.CertificateID and c.useyear=b.useyear
			where 1=1		 
		' 
	if @ParticipantID is not null 
		set @commandText = @commandText + '	and a.ParticipantID = @ParticipantID'		
				
    if not @internalPassportSeria is null
    begin
        set @commandText = @commandText + ' and a.DocumentSeries = @internalPassportSeria '
	end
	
	if not @passportNumber is null
    begin
        set @commandText = @commandText + ' and  a.DocumentNumber = @passportNumber '
    end
	else
	begin
		goto nullresult
	end	
	
	set @commandText = @commandText + ' ) certificate '
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
            left join ExamCertificateUniqueChecks CC with (nolock) 
				on CC.IdGuid  = S.certificateId 
			left join prn.CancelledCertificates CD with (nolock) 
				on S.CertificateId = CD.CertificateFK and S.[Year]=CD.UseYear
			left join dbo.Region region with (nolock)
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