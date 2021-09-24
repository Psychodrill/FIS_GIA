-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (68, '068_2012_07_11_OpenFbsLogs.sql')
-- =========================================================================
GO

-- =============================================
-- Author:		Yusupov K.I.
-- Create date: 04-06-2010
-- Description:	Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
Alter PROCEDURE [dbo].[AddCNEWebUICheckEvent]
@AccountLogin NVARCHAR(255),					-- логин проверяющего
	@LastName NVARCHAR(255)=null,				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255)=null,				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255)=null,			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20)=NULL,			-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20)=NULL,			-- номер документа сертифицируемого (паспорта)
	@CNENumber NVARCHAR(20)=NULL,				-- номер сертификата
	@TypographicNumber NVARCHAR(20)=NULL,		-- типографический номер сертификата 
	@RawMarks NVARCHAR(500)=null,				-- средние оценки по предметам (через запятую, в определенном порядке)
	@EventId INT output							-- id зарегистрированного события
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
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,CNENumber,Marks) 
        VALUES 
			(@AccountId,'CNENumber',@FirstName,@LastName,@PatronymicName,@CNENumber,@RawMarks)
	END
	ELSE IF (@PassportNumber IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,PassportSeria,PassportNumber,Marks) 
        VALUES 
			(@AccountId,'Passport',@FirstName,@LastName,@PatronymicName,@PassportSeria,@PassportNumber,@RawMarks)
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

GO

-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

alter proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
	  @number nvarchar(255) = null				-- номер сертификата	
	, @checkSubjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
	, @xml xml out
as
begin 
	
	if @number is null
	begin
		RAISERROR (N'Номер св-ва не указан',10,1);
		return
	end

	declare @eventId int

	if @shouldCheckMarks = 1
		exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @checkSubjectMarks, @CNENumber = @number, @eventId = @eventId output
	
  
	declare 
		@commandText nvarchar(max)
		, @declareCommandText nvarchar(max)
		, @selectCommandText nvarchar(max)
		, @baseName nvarchar(255)
		, @yearFrom int
		, @yearTo int
		, @accountId bigint
        , @organizationId bigint
    	, @CId bigint
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
	, certificateId bigint
	, IsDeny bit
	, DenyComment ntext
	, DenyNewcertificateNumber nvarchar(255)
	, [Year] int
	, PassportSeria nvarchar(255)
	, PassportNumber nvarchar(255)
	, RegionId int
	, RegionName nvarchar(255)
	, TypographicNumber nvarchar(255)
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
			when [certificate].Id>0 then 1
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
	from 
		(select null ''empty'') t left join 
		dbo.CommonNationalExamcertificate [certificate] with (nolock, fastfirstrow) on 
				[certificate].[Year] between @yearFrom and @yearTo '
	
    set @sql = @sql + '	and [certificate].Number = @number'
	set @sql = @sql + '			
		left outer join dbo.Region region
			on region.Id = [certificate].RegionId
		left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
			on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.certificateNumber = [certificate].Number'

	insert into @certificate_check 	
	exec sp_executesql @sql,N'@number nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int',@number = @number,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo
	
	--SELECT * FROM @certificate_check
	
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
            @certificateId = @CId
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
        CC.UniqueOtherCheck UniqueOtherCheck
        into #table
	from @certificate_check certificate_check
    	inner join CommonNationalExamcertificate C on C.Id = certificate_check.certificateId
        left outer join ExamcertificateUniqueChecks CC on CC.Id = C.Id
		left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]					
		left outer join (
			select			
				getcheck_subject.SubjectId id,[subject].Name,
				certificate_subject.[Year],certificate_subject.certificateId,	
				isnull(getcheck_subject.SubjectId, certificate_subject.SubjectId) SubjectId
				, getcheck_subject.[Mark] CheckSubjectMark
				, certificate_subject.[Mark] SubjectMark
				, case
					when getcheck_subject.Mark = certificate_subject.Mark then 1
					else 0
				end SubjectMarkIsCorrect
				, certificate_subject.HasAppeal
				,mm.[MinimalMark]
				,mm1.[MinimalMark] MinimalMark1
			from 
			CommonNationalExamcertificateSubject certificate_subject with (nolock)
			join dbo.[Subject] [subject]	on [subject].Id = certificate_subject.SubjectId		
			left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject	on getcheck_subject.SubjectId = certificate_subject.SubjectId			
			left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.[Year] = mm.[Year] 
			left join [MinimalMark] as mm1 on certificate_subject.SubjectId = mm1.[SubjectId] and certificate_subject.[Year] = mm1.[Year] 
			) check_subject
			on certificate_check.[Year] = check_subject.[Year] and certificate_check.certificateId = check_subject.certificateId
			
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

GO

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
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @subjectMarks, @passportSeria = @passportSeria, @passportNumber = @passportNumber,  @eventId = @eventId output

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
		--select * from @Search
		
		-- записать в лог интерактивных проверок
		IF EXISTS (SELECT * FROM @Search) AND @shouldWriteLog = 1
		BEGIN
			UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 certificateId FROM @Search)
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