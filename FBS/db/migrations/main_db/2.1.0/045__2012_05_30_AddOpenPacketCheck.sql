-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (45, '045__2012_05_30_AddOpenPacketCheck')
-- =========================================================================
GO



GO
ALTER TABLE [dbo].[CommonNationalExamCertificateCheckBatch]
    ADD [Type]    INT    CONSTRAINT [DF__CommonNati__Type__4A7ED9BC] DEFAULT ((0)) NULL,
        [outerId] BIGINT NULL,
        [Year]    INT    NULL;


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificateCheckBatch].[IX_CommonNationalExamCertificateCheckBatch]...';


GO
CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificateCheckBatch]
    ON [dbo].[CommonNationalExamCertificateCheckBatch]([outerId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificateSubject].[FK_CommonNationalExamCertificateSubject_cert]...';


GO
CREATE NONCLUSTERED INDEX [FK_CommonNationalExamCertificateSubject_cert]
    ON [dbo].[CommonNationalExamCertificateSubject]([CertificateId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0);


GO
PRINT N'Altering [dbo].[AddCNEWebUICheckEvent]...';


GO
-- =============================================
-- Author:		Yusupov K.I.
-- Create date: 04-06-2010
-- Description:	Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
ALTER PROCEDURE [dbo].[AddCNEWebUICheckEvent]
@AccountLogin NVARCHAR(255),			-- логин проверяющего
	@LastName NVARCHAR(255)=null,				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255)=null,				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255)=null,			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20)=NULL,		-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20)=NULL,		-- номер документа сертифицируемого (паспорта)
	@CNENumber NVARCHAR(20)=NULL,			-- номер сертификата
	@TypographicNumber NVARCHAR(20)=NULL,   -- типографический номер сертификата 
	@RawMarks NVARCHAR(500)=null			-- средние оценки по предметам (через запятую, в определенном порядке)
AS
BEGIN
	IF 
	(
		@TypographicNumber IS NULL AND
		@CNENumber IS NULL AND
		(@PassportSeria IS NULL OR @PassportNumber IS NULL) AND
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
	  SELECT @@Identity
END
GO
PRINT N'Altering [dbo].[UpdateCommonNationalExamCertificateCheckBatch]...';


GO
-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch

-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
ALTER proc [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
	@id bigint output
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @batch ntext
	, @filter nvarchar(255)
	, @type int=0
	, @outerId bigint=null
	, @year nvarchar(10)
as
begin
	declare 
		@currentDate datetime
		, @accountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @internalId bigint

	set @updateId = newid()
	
	set @currentDate = getdate()

	select
		@accountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	set @eventCode = N'CNE_BCH_CHK'

	begin tran insert_check_batch_tran

		insert dbo.CommonNationalExamCertificateCheckBatch
			(
			CreateDate
			, UpdateDate
			, OwnerAccountId
			, IsProcess
			, IsCorrect
			, Batch
			, [Filter]
			,[Type]
			,outerId
			, [Year]
			)
		select
			@currentDate
			, @currentDate
			, @accountId
			, 1
			, null
			, @batch
			, @filter
			,@type
			,@outerId
			, @year
			
		if (@@error <> 0)
			goto undo

		set @internalId = scope_identity()
		set @id = dbo.GetExternalId(@internalId)

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran insert_check_batch_tran

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_check_batch_tran

	return 1

end
GO
PRINT N'Creating [dbo].[CheckCommonNationalExamCertificateByNumberForXml]...';


GO
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

CREATE proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
	  @number nvarchar(255) = null				-- номер сертификата	
	, @checkSubjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	,@xml xml out
as
begin 
declare  @checkLastName nvarchar(255) 	, @checkFirstName nvarchar(255)	, @checkPatronymicName nvarchar(255), @checkTypographicNumber nvarchar(20)
select @checkLastName = null			, @checkFirstName = null, @checkPatronymicName=  null ,@checkTypographicNumber=null
	
	if @checkTypographicNumber is null and @number is null
	begin
		RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
		return
	end
    
    if ( select COUNT(*) from dbo.GetSubjectMarks(@checkSubjectMarks))<2
    goto nullresult
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
	
	if @number is not null 
		set @sql = @sql + '	and [certificate].Number = @number'
	if @CheckTypographicNumber is not null 
		set @sql = @sql + '	and [certificate].TypographicNumber=@CheckTypographicNumber'
	
	set @sql = @sql + '			
		left outer join dbo.Region region
			on region.Id = [certificate].RegionId
		left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
			on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.certificateNumber = [certificate].Number'
	
	insert into @certificate_check 		
	exec sp_executesql @sql,N'@checkLastName nvarchar(255),@number nvarchar(255),@checkTypographicNumber nvarchar(20),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int',@checkLastName=@checkLastName,@number = @number,@checkTypographicNumber=@checkTypographicNumber,@checkFirstName=@checkFirstName,@checkPatronymicName=@checkPatronymicName,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo
	
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
            @certificateId = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
	-------------------------------------------------------------
	
	select
		certificate_check.certificateId
		,certificate_check.Number Number
		, certificate_check.CheckLastName CheckLastName
		, certificate_check.LastName LastName
		, certificate_check.LastNameIsCorrect LastNameIsCorrect
		, certificate_check.CheckFirstName CheckFirstName
		, certificate_check.FirstName FirstName
		, certificate_check.FirstNameIsCorrect FirstNameIsCorrect
		, certificate_check.CheckPatronymicName CheckPatronymicName
		, certificate_check.PatronymicName PatronymicName
		, certificate_check.PatronymicNameIsCorrect PatronymicNameIsCorrect
		, certificate_check.IsExist IsExist
		, check_subject.Id  SubjectId
		, check_subject.Name  SubjectName
		, case when check_subject.CheckSubjectMark < check_subject.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',')  CheckSubjectMark
		, case when check_subject.SubjectMark < check_subject.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',')  SubjectMark
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
			from 
			CommonNationalExamcertificateSubject certificate_subject with (nolock)
			join dbo.[Subject] [subject]	on [subject].Id = certificate_subject.SubjectId		
			left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject	on getcheck_subject.SubjectId = certificate_subject.SubjectId			
			left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.[Year] = mm.[Year] 
			) check_subject
			on certificate_check.[Year] = check_subject.[Year] and certificate_check.certificateId = check_subject.certificateId
			
			--select * from #table
			
IF exists(select * from #table where  SubjectMarkIsCorrect=0 and SubjectId IS NOT null) or (select COUNT(*) from #table where SubjectId IS NOT null)<>(select COUNT(*) from dbo.GetSubjectMarks(@checkSubjectMarks) )
	delete from #table


	select @xml=(
	select 
	(
	select * from #table
	for xml path('check'), ELEMENTS XSINIL,type
	) 
	for xml path('root'),type
	)
	drop table #table
goto result	
nullresult:
	select @xml=(
	select null 
	for xml path('root'),type
	)
result:

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
GO
PRINT N'Creating [dbo].[CheckCommonNationalExamCertificateByPassportForXml]...';


GO
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

CREATE proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]	 
	 @subjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
	,@xml xml out
	,@withMarks bit=0
as
begin 
set nocount on
declare  @checkLastName nvarchar(255) 	, @checkFirstName nvarchar(255)	, @checkPatronymicName nvarchar(255), @checkTypographicNumber nvarchar(20)
select @checkLastName = null			, @checkFirstName = null, @checkPatronymicName=  null ,@checkTypographicNumber=null
	
	if @checkTypographicNumber is null and @passportSeria is null and @passportNumber is null
	begin
		RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
		return
	end
    
 if ( select COUNT(*) from dbo.GetSubjectMarks(@subjectMarks))<2
    goto nullresult

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
    	if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                '  certificate.InternalPassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                '  certificate.InternalPassportSeria = @internalPassportSeria '
        end
	end

	if not @passportNumber is null
    begin
    	if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
        	set @passportNumber = REPLACE(@passportNumber, '*', '%')
        	set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and  certificate.PassportNumber like @passportNumber '
        end
    	else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber '
        end
    end
	
	
	if @passportNumber is null
		goto nullresult

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
			
	delete search from 
	@Search  search
	where exists(
			select * from 
			dbo.GetSubjectMarks(@subjectMarks) subject_mark 
			left join  CommonNationalExamCertificateSubject certificate_subject with(nolock) 
			on search.CertificateId = certificate_subject.CertificateId and certificate_subject.SubjectId = subject_mark.SubjectId and certificate_subject.Mark = subject_mark.Mark 
			where  certificate_subject.CertificateId is null)		
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
			CC.UniqueOtherCheck UniqueOtherCheck ,
			case when @withMarks=1 then
			(select b.Name as 'Mark/SubjectName', a.Mark as 'Mark/SubjectMark',a.HasAppeal as 'Mark/HasAppeal' from CommonNationalExamcertificateSubject a 
			 join dbo.[Subject] b	on b.Id = a.SubjectId	where a.certificateId=s.CertificateId	 for XML path(''), ELEMENTS XSINIL,type) 
			else '' end Marks
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

	
	
GO
PRINT N'Creating [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]...';


GO
-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]
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
	set	@sortAsc = 0

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
					, (select count(SourceCertificateId) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
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
GO
PRINT N'Creating [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]...';


GO
-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]	
	 @batchId bigint,@xml xml out	
as
set nocount on
	declare @id bigint,@isCorrect bit
	select @id=id,@isCorrect=IsCorrect  
	from dbo.CommonNationalExamCertificateCheckBatch  with (nolock, fastfirstrow)				
	where outerId = @batchId and IsProcess=0 and Executing=0
	set @xml=(
	select 
	(
	select 	
	cnCheck.Id as '@Id',
	@isCorrect as '@IsBatchCorrect',
	cnCheck.BatchId as '@BatchId',
	cnCheck.CertificateCheckingId as '@CertificateCheckingId',
	cnCheck.CertificateNumber as '@CertificateNumber',
	cnCheck.IsOriginal as '@IsOriginal',
	cnCheck.LastName as '@LastName',
	cnCheck.FirstName as '@FirstName',
	cnCheck.PatronymicName as '@PatronymicName',
	cnCheck.IsCorrect as '@IsCorrect',
	cnCheck.SourceCertificateId as '@SourceCertificateId',
	cnCheck.SourceLastName as '@SourceLastName',
	cnCheck.SourceFirstName as '@SourceFirstName',
	cnCheck.SourcePatronymicName as '@SourcePatronymicName',
	cnCheck.IsDeny as '@IsDeny',
	cnCheck.DenyComment as '@DenyComment',
	cnCheck.DenyNewCertificateNumber as '@DenyNewCertificateNumber',
	cnCheck.Year as '@Year',
	cnCheck.TypographicNumber as '@TypographicNumber',
	cnCheck.RegionId as '@RegionId',
	cnCheck.PassportSeria as '@PassportSeria',
	cnCheck.PassportNumber as '@PassportNumber',
	(
		select 
		b.Id as 'subject/@Id',
		b.CheckId as 'subject/@CheckId',
		b.SubjectId as 'subject/@SubjectId',
		b.Mark as 'subject/@Mark',
		b.IsCorrect as 'subject/@IsCorrect',
		b.SourceCertificateSubjectId as 'subject/@SourceCertificateSubjectId',
		b.SourceMark as 'subject/@SourceMark',
		b.SourceHasAppeal as 'subject/@SourceHasAppeal',
		b.Year as 'subject/@Year'
		from 
		CommonNationalExamCertificateSubjectCheck b where 
		--BatchId=@id and 
		b.CheckId=cnCheck.id
		for XML path(''), root('subjects'),type
	) 
	from CommonNationalExamCertificateCheck  cnCheck
	where  
	--cnCheck.id in(3201511,3201512)
	BatchId=@id 
 for xml path('check'),type
 )
 for xml path('root'),type	
)
--select @xml result


/*
declare @xml xml
exec [SearchCommonNationalExamCertificateCheckByOuterId] 300462123775,@xml out
select @xml
*/
GO
PRINT N'Creating [dbo].[usp_cne_AddCheckBatchResult]...';


GO
-- =============================================
-- Добавление результатов проверки из xml
-- =============================================
CREATE proc [dbo].[usp_cne_AddCheckBatchResult]	
	 @xml xml=null,@batchid bigint=0
as
set nocount on	
if @xml is null
begin
 select null IsProcess1,null Total,null Found where 1=0
 return
end
select 	
	item.ref.value('@Id', 'bigint') as Id,
	item.ref.value('@IsBatchCorrect', 'bit') as IsBatchCorrect,
	item.ref.value('@BatchId', 'bigint') as BatchId,
	item.ref.value('@CertificateCheckingId', 'uniqueidentifier') as CertificateCheckingId,
	item.ref.value('@CertificateNumber', 'nvarchar(255)') as CertificateNumber,
	item.ref.value('@IsOriginal', 'bit') as IsOriginal,
	item.ref.value('@LastName', 'nvarchar(255)') as LastName,
	item.ref.value('@FirstName', 'nvarchar(255)') as FirstName,
	item.ref.value('@PatronymicName', 'nvarchar(255)') as PatronymicName,
	item.ref.value('@IsCorrect', 'bit') as IsCorrect,
	item.ref.value('@SourceCertificateId', 'bigint') as SourceCertificateId,
	item.ref.value('@SourceLastName', 'nvarchar(255)') as SourceLastName,
	item.ref.value('@SourceFirstName', 'nvarchar(255)') as SourceFirstName,
	item.ref.value('@SourcePatronymicName', 'nvarchar(255)') as SourcePatronymicName,
	item.ref.value('@IsDeny', 'bit') as IsDeny,
	item.ref.value('@DenyComment', 'nvarchar(max)') as DenyComment,
	item.ref.value('@DenyNewCertificateNumber', 'nvarchar(255)') as DenyNewCertificateNumber,
	item.ref.value('@Year', 'int') as Year,
	item.ref.value('@TypographicNumber', 'nvarchar(255)') as TypographicNumber,
	item.ref.value('@RegionId', 'int') as RegionId,
	item.ref.value('@PassportSeria', 'nvarchar(255)') as PassportSeria,
	item.ref.value('@PassportNumber', 'nvarchar(255)') as PassportNumber	
	into #check
from 
(
select @xml
) feeds(feedXml)
cross apply feedXml.nodes('/root/check') as item(ref)
if not exists(select * from #check)
begin
 select null IsProcess2,null Total,null Found where 1=0
 return
end
select 	
	item.ref.value('@Id', 'bigint') as Id,	
	item.ref.value('@CheckId', 'bigint') as CheckId,
	item.ref.value('@SubjectId', 'bigint') as SubjectId,
	item.ref.value('@Mark', 'numeric(5,1)') as Mark,
	item.ref.value('@IsCorrect', 'bit') as IsCorrect,
	item.ref.value('@SourceCertificateSubjectId', 'bigint') as SourceCertificateSubjectId,
	item.ref.value('@SourceMark', 'numeric(5,1)') as SourceMark,
	item.ref.value('@SourceHasAppeal', 'bigint') as SourceHasAppeal,
	item.ref.value('@Year', 'int') as [Year]
	into #mark
from 
(
select @xml
) feeds(feedXml)
cross apply feedXml.nodes('/root/check/subjects/subject') as item(ref)
if not exists(select * from #mark)
begin
 select null IsProcess3,null Total,null Found where 1=0
 return
end

--select * from #check
--select * from #mark
declare @id  bigint,@checkid bigint,@isCorrect bit
select @batchid=dbo.GetInternalId(@batchid)
if exists(select * from CommonNationalExamCertificateCheck where BatchId=@batchid)
begin
 select null IsProcess4,null Total,null Found where 1=0
 return
end
--select @batchid
begin try
begin tran
declare cur cursor local fast_forward for select id from #check
open cur
fetch next from cur into @id
while @@FETCH_STATUS=0
begin
 insert CommonNationalExamCertificateCheck(BatchId,CertificateCheckingId,CertificateNumber,IsOriginal,LastName,FirstName,PatronymicName,IsCorrect,SourceCertificateId,SourceLastName,SourceFirstName,SourcePatronymicName,IsDeny,DenyComment,DenyNewCertificateNumber,Year,TypographicNumber,RegionId,PassportSeria,PassportNumber)
 select @batchid,CertificateCheckingId,CertificateNumber,IsOriginal,LastName,FirstName,PatronymicName,IsCorrect,SourceCertificateId,SourceLastName,SourceFirstName,SourcePatronymicName,IsDeny,DenyComment,DenyNewCertificateNumber,Year,TypographicNumber,RegionId,PassportSeria,PassportNumber
 from #check where id=@id
 set @checkid=SCOPE_IDENTITY()
 insert CommonNationalExamCertificateSubjectCheck(BatchId,CheckId,SubjectId,Mark,IsCorrect,SourceCertificateSubjectId,SourceMark,SourceHasAppeal,Year)
 select @batchid,@checkid,SubjectId,Mark,IsCorrect,SourceCertificateSubjectId,SourceMark,SourceHasAppeal,Year
 from #mark where CheckId=@id 
 fetch next from cur into @id
end
close cur
deallocate cur
select top 1 @isCorrect=IsBatchCorrect from #check
update CommonNationalExamCertificateCheckBatch set IsProcess=0,Executing=0,IsCorrect=@isCorrect where id=@batchid
--select top 10 * from CommonNationalExamCertificateCheckBatch where id=@batchid
--select top 10 * from CommonNationalExamCertificateCheck where BatchId=@batchid  order by id desc
--select top 10 * from CommonNationalExamCertificateSubjectCheck where BatchId=@batchid order by id desc
select 
IsProcess
,(select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
, (select count(SourceCertificateId) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
from CommonNationalExamCertificateCheckBatch b where IsProcess=0 and id=@batchid
if @@trancount>0
	commit tran

end try
begin catch
 if @@trancount>0
	rollback
 	declare @msg nvarchar(4000)
 	set @msg=ERROR_MESSAGE()
 	raiserror(@msg,16,1)
 	return -1
end catch

--[SearchCommonNationalExamCertificateCheckByOuterId]
GO
PRINT N'Creating [dbo].[ufn_ut_SplitFromString]...';


GO
CREATE FUNCTION [dbo].[ufn_ut_SplitFromStringWithId]
(
	@string nvarchar(max),
	@delimeter nvarchar(1) = ' ')
RETURNS @ret TABLE (id int identity(1,1) ,val nvarchar(4000) )
AS
BEGIN
	DECLARE @s int, @e int

	SET @s = 0
	WHILE CHARINDEX(@delimeter,@string,@s) <> 0
	BEGIN
		SET @e = CHARINDEX(@delimeter,@string,@s)
		INSERT @ret VALUES (SUBSTRING(@string,@s,@e - @s))
		SET @s = @e + 1
	END
	INSERT @ret VALUES (SUBSTRING(@string,@s,4000))
	RETURN
END
GO
