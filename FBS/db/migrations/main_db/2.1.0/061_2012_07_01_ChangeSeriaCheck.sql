-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (61, '061_2012_07_01_ChangeSeriaCheck.sql')
-- =========================================================================
GO

-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

ALTER proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]	 
	 @subjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
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

	declare @CId bigint,@sourceEntityIds nvarchar(4000) 
	declare @Search table 
	( 
		LastName nvarchar(255) 
		, FirstName nvarchar(255) 
		, PatronymicName nvarchar(255) 
		, CertificateId bigint 
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
		from CommonNationalExamCertificate certificate with (nolock)
		where 
		' 
    if not @internalPassportSeria is null
    begin
        set @commandText = @commandText + ' certificate.InternalPassportSeria = @internalPassportSeria '
	end
	
	if not @passportNumber is null
    begin
        set @commandText = @commandText + ' and  certificate.PassportNumber = @passportNumber '
    end
	else
	begin
		goto nullresult
	end
	
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
		select distinct b.SubjectId,b.Mark  from 
		@Search search
			join CommonNationalExamCertificateSubject b with(nolock) 
			on search.CertificateId = b.CertificateId
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
	( pkid int identity(1,1) primary key, CertificateId bigint
	)     
	insert @Search1
    select distinct S.CertificateId 
		from @Search S   
	where CertificateId > 0	
	
	declare @CertificateId bigint,@pkid int
	while exists(select * from @Search1)
	begin
	  select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateId = @CertificateId 
                	  
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
			case when S.CertificateId>0 then 1 else 0 end IsExist, 
			case	when CD.Id >0 then 1 end IsDeny,  
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
			CC.UniqueOtherCheck UniqueOtherCheck 
		from 
			@Search S 
            inner join CommonNationalExamCertificate C with (nolock) 
            	on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
				on CC.Id  = C.Id 
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
	exec dbo.RegisterEvent 
			@accountId = @editorAccountId, 
			@ip = @ip, 
			@eventCode = @eventCode, 
			@sourceEntityIds = @sourceEntityIds, 
			@eventParams = @eventParams, 
			@updateId = null 
	
	return 0
end