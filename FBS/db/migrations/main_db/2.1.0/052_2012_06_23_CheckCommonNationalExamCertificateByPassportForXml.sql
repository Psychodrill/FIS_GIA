-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (52, '052_2012_06_23_CheckCommonNationalExamCertificateByPassportForXml')
-- =========================================================================
GO

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

GO

-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

ALTER proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
	  @number nvarchar(255) = null				-- номер сертификата	
	, @checkSubjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
	, @xml xml out
as
begin 
declare  @checkLastName nvarchar(255) 	, @checkFirstName nvarchar(255)	, @checkPatronymicName nvarchar(255), @checkTypographicNumber nvarchar(20)
select @checkLastName = null			, @checkFirstName = null, @checkPatronymicName=  null ,@checkTypographicNumber=null
	
	if @checkTypographicNumber is null and @number is null
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
	
	--SELECT * FROM @certificate_check
	
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
Alter proc [dbo].[SearchCommonNationalExamCertificateCheck]
	@login nvarchar(255)
	, @batchId bigint			-- id пакета
	, @startRowIndex int = 1	-- пейджинг
	, @maxRowCount int = null	-- пейджинг
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
	set	@sortAsc = 0

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table 
				( 
				Id bigint 
				, BatchId bigint 
				, CertificateNumber nvarchar(255) 
				, CertificateId bigint 
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
				'	cne_check.Id ' +
				'	, cne_check.BatchId ' +
				'	, cne_check.CertificateNumber ' +
				'	, cne_check.SourceCertificateId CertificateId ' +
				'	, cne_check.IsOriginal ' +
				'	, cne_check.LastName CheckLastName ' +
				'	, cne_check.SourceLastName LastName ' +
				'	, cne_check.FirstName CheckFirstName ' +
				'	, cne_check.SourceFirstName FirstName ' +
				'	, cne_check.PatronymicName CheckPatronymicName ' +
				'	, cne_check.SourcePatronymicName PatronymicName ' +
				'	, cne_check.IsCorrect ' +
				'	, cne_check.IsDeny ' +
				'	, cne_check.Year ' +
				'	, cne_check.TypographicNumber ' +
				'	, case when ed.[ExpireDate] is null then ''Не найдено'' else 
					  case when isnull(cne_check.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 

''Действительно'' 
					  else ''Истек срок'' end end as [Status] ' +
				'	, r.name as RegionName ' +
				'	, r.code as RegionCode ' +
				'	, cne_check.PassportSeria ' +
				'	, cne_check.PassportNumber ' +
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
		'	and cne_check.BatchId = @internalBatchId '
	if @certNumber is not null
		set @commandText = @commandText + 
		'	and cne_check.CertificateNumber = ''' + @certNumber + ' '''

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
					'	, chk_mrk_pvt.[<code>] [<code>CheckMark]  
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
					on unique_cheks.Id = search.CertificateId
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

GO