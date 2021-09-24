-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (34, '034__2011_07_20__CorrectMarks')
-- =========================================================================
GO




-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
ALTER proc [dbo].[CheckCommonNationalExamCertificateByNumber]
	  @number nvarchar(255) = null
	, @checkLastName nvarchar(255) = null
	, @checkFirstName nvarchar(255) = null
	, @checkPatronymicName nvarchar(255) = null
	, @checkSubjectMarks nvarchar(max) = null
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @checkTypographicNumber nvarchar(20) = null
as
begin 

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
	, CertificateId bigint
	, IsDeny bit
	, DenyComment ntext
	, DenyNewCertificateNumber nvarchar(255)
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

	insert into @certificate_check 
	select
		certificate.Number 
		, @CheckLastName 
		, [certificate].LastName 
		, case 
			when [certificate].LastName collate cyrillic_general_ci_ai = isnull(@CheckLastName, [certificate].LastName) then 1
			else 0
		end LastNameIsCorrect
		, @CheckFirstName 
		, [certificate].FirstName 
		, case 
			when [certificate].FirstName collate cyrillic_general_ci_ai = isnull(@CheckFirstName, [certificate].FirstName) then 1
			else 0
		end FirstNameIsCorrect
		, @CheckPatronymicName 
		, [certificate].PatronymicName 
		, case 
			when [certificate].PatronymicName collate cyrillic_general_ci_ai = isnull(@CheckPatronymicName,[certificate].PatronymicName) then 1
			else 0
		end PatronymicNameIsCorrect
		, case
			when not [certificate].Id is null then 1
			else 0
		end IsExist
		, [certificate].Id
		, case
			when not certificate_deny.Id is null then 1
			else 0
		end
		, certificate_deny.Comment
		, certificate_deny.NewCertificateNumber
		, certificate.[Year]
		, certificate.PassportSeria
		, certificate.PassportNumber
		, [certificate].RegionId
		, region.Name
		, certificate.TypographicNumber

	from 
		(select null 'empty') t left join 
		dbo.CommonNationalExamCertificate [certificate] with (nolock, fastfirstrow) on 
				certificate.[Year] between @yearFrom and @yearTo
				and certificate.Number = isnull(@number, certificate.Number)
				and isnull(certificate.TypographicNumber,'') = coalesce(@CheckTypographicNumber,certificate.TypographicNumber,'')
				--and certificate.Lastname collate cyrillic_general_ci_ai = @CheckLastName 
		left outer join dbo.Region region
			on region.Id = [certificate].RegionId
		left outer join CommonNationalExamCertificateDeny certificate_deny with (nolock, fastfirstrow)
			on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.CertificateNumber = certificate.Number
				--and certificate.Lastname collate cyrillic_general_ci_ai = @CheckLastName 

	set @eventParams = 
		isnull(@number, '') + '|' +
		isnull(@checkLastName, '') + '|' +
		isnull(@checkFirstName, '') + '|' +
		isnull(@checkPatronymicName, '') + '|' +
		isnull(@checkSubjectMarks, '') + '|' +
		isnull(@checkTypographicNumber, '')

	set @sourceEntityIds = '' 
	select 
		@sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), certificate_check.CertificateId) 
	from 
		@certificate_check certificate_check 
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	if @sourceEntityIds = '' 
		set @sourceEntityIds = null 


	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.CertificateId
    from 
        @certificate_check S
    where
    	S.CertificateId is not null
		
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @CertificateId = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
	-------------------------------------------------------------

	select
		certificate_check.CertificateId
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
		, subject.Id SubjectId
		, subject.Name SubjectName
		--, check_subject.CheckSubjectMark
		--, check_subject.SubjectMark
		, case when check_subject.CheckSubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',') CheckSubjectMark
		, case when check_subject.SubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',') SubjectMark
		, isnull(check_subject.SubjectMarkIsCorrect, 0) SubjectMarkIsCorrect
		, check_subject.HasAppeal
		, certificate_check.IsDeny
		, certificate_check.DenyComment
		, certificate_check.DenyNewCertificateNumber
		, certificate_check.PassportSeria
		, certificate_check.PassportNumber
		, certificate_check.RegionId
		, certificate_check.RegionName
		, certificate_check.Year
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
        isnull(CC.UniqueOtherCheck, 0) UniqueOtherCheck
	from @certificate_check certificate_check
    	inner join CommonNationalExamCertificate C
        	on C.Id = certificate_check.CertificateId
        left outer join ExamCertificateUniqueChecks CC
			on CC.Id = C.Id
		left join [ExpireDate] as ed on certificate_check.Year = ed.[Year]					
		left outer join (select
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
				, isnull(check_subject.SubjectId, certificate_subject.SubjectId) SubjectId
				, check_subject.[Mark] CheckSubjectMark
				, certificate_subject.[Mark] SubjectMark
				, case
					when check_subject.Mark = certificate_subject.Mark then 1
					else 0
				end SubjectMarkIsCorrect
				, certificate_subject.HasAppeal
			from CommonNationalExamCertificateSubject certificate_subject with (nolock)
				inner join @certificate_check certificate_check
					on certificate_check.[Year] = certificate_subject.[Year]
						and certificate_check.CertificateId = certificate_subject.CertificateId
				left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
					on check_subject.SubjectId = certificate_subject.SubjectId) check_subject
			left outer join dbo.Subject [subject]
				on [subject].Id = check_subject.SubjectId
			on 1 = 1
			left join [MinimalMark] as mm on check_subject.SubjectId = mm.[SubjectId] and /*year(getdate())*/ certificate_check.Year = mm.[Year] 
	
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
ALTER proc [dbo].[SearchCommonNationalExamCertificateCheck]
	@login nvarchar(255)
	, @batchId bigint
	, @startRowIndex int = 1
	, @maxRowCount int = null
	, @showCount bit = null
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
ALTER proc [dbo].[SearchCommonNationalExamCertificateRequest]
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
			, SourceCertificateId  bigint
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
					when not cne_certificate_request.SourceCertificateId is null then 1
					else 0
				end IsExist
				, region.Name RegionName
				, region.Code RegionCode
				, isnull(cne_certificate_request.IsDeny, 0) IsDeny 
				, cne_certificate_request.DenyComment DenyComment
				, cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
				, cne_certificate_request.SourceCertificateId
				, cne_certificate_request.SourceCertificateYear
				, cne_certificate_request.TypographicNumber
				, case when cne_certificate_request.SourceCertificateId is null then ''Не найдено'' else 
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
					left join dbo.CommonNationalExamCertificate cnec with (nolock) on 
cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateId = cnec.id
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
					CertificateId bigint 
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
					cne_certificate_subject.CertificateId 
					, case when cne_certificate_subject.[Mark] < mm.[MinimalMark] then ''!'' else '''' 
end + replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
					, cne_certificate_subject.HasAppeal
					, subject.Code
					, 1 
				from	
					dbo.CommonNationalExamCertificateSubject cne_certificate_subject
					left outer join dbo.Subject subject on subject.Id = 
cne_certificate_subject.SubjectId
					left join [MinimalMark] as mm on cne_certificate_subject.[SubjectId] = 
mm.[SubjectId] and cne_certificate_subject.Year = mm.[Year]
				where 
					exists(select 1 
							from @search search
							where cne_certificate_subject.CertificateId = 
search.SourceCertificateId
								and cne_certificate_subject.[Year] = 
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
					on unique_cheks.Id = search.SourceCertificateId '

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
						N'	, mrk_pvt.[<code>] [<code>Mark]  
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