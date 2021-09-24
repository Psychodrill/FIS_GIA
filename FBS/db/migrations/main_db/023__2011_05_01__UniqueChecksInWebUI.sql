-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (23, '023__2011_05_01__UniqueChecksInWebUI')
-- =========================================================================
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
			from dbo.CommonNationalExamCertificateCheckBatch cne_certificate_check_batch with (nolock, fastfirstrow)
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
					  case when isnull(cne_check.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно'' 
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
				, case when subject_check.[Mark] < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(subject_check.[Mark] as nvarchar(9)),''.'','','')
				, case when subject_check.[SourceMark] < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(subject_check.[SourceMark] as nvarchar(9)),''.'','','')
				, cast(subject_check.SourceHasAppeal as int) 
				, cast(subject_check.IsCorrect as int) 
			from dbo.CommonNationalExamCertificateSubjectCheck subject_check with (nolock) 
			inner join dbo.Subject subject on subject.Id = subject_check.SubjectId 
			left join [MinimalMark] as mm on subject_check.[SubjectId] = mm.[SubjectId] and Year(GetDate()) = mm.[Year] 
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
					when search.CheckPatronymicName collate cyrillic_general_ci_ai = search.PatronymicName then 1 
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
						pivot (min(CheckMark) for SubjectCode in (<subject_columns>)) as chk_mrk_pvt 
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
						pivot (Sum(CertificateSubjectId) for SubjectCode in (<subject_columns>)) as sbj_pvt 
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
					case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно'' 
					else ''Истек срок'' end end as [Status]
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
						on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId
					left join [ExpireDate] as ed with (nolock) on cne_certificate_request.[SourceCertificateYear] = ed.[Year]	
					left join dbo.CommonNationalExamCertificate cnec with (nolock) on cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateId = cnec.id
			where 1 = 1 '
	else
		set @commandText = 
			N'
			select count(*)
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
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
					, case when cne_certificate_subject.[Mark] < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
					, cne_certificate_subject.HasAppeal
					, subject.Code
					, 1 
				from	
					dbo.CommonNationalExamCertificateSubject cne_certificate_subject
					left outer join dbo.Subject subject on subject.Id = cne_certificate_subject.SubjectId
					left join [MinimalMark] as mm on cne_certificate_subject.[SubjectId] = mm.[SubjectId] and Year(GetDate()) = mm.[Year]
				where 
					exists(select 1 
							from @search search
							where cne_certificate_subject.CertificateId = search.SourceCertificateId
								and cne_certificate_subject.[Year] = search.SourceCertificateYear)
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
				set @pivotSubjectColumns = @pivotSubjectColumns + replace('[<code>]', '<code>', @subjectCode)
				
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
							pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as apl_pvt
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

GO


ALTER proc [dbo].[SearchCommonNationalExamCertificateWildcard]
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

	if @showCount = 0
	set @commandText = @commandText + 
		' declare 
			@sourceEntityIds nvarchar(4000)  
		declare @Search table 
			( 
			  row int
			, LastName nvarchar(255) 
			, FirstName nvarchar(255) 
			, PatronymicName nvarchar(255) 
			, CertificateId bigint 
			, CertificateNumber nvarchar(255) 
			, RegionId int 
			, PassportSeria nvarchar(255) 
			, PassportNumber nvarchar(255) 
			, TypographicNumber nvarchar(255) 
			, Year int 
		   , primary key(CertificateNumber, row) 
			) 
		
		insert into @Search 
		select top (@startRowIndex+@maxRowCount-1)
			  row_number() over (order by c.year, c.id) as row
			, c.LastName 
			, c.FirstName 
			, c.PatronymicName 
			, c.Id 
			, c.Number 
			, c.RegionId 
			, isnull(c.PassportSeria, @internalPassportSeria) 
			, isnull(c.PassportNumber, @passportNumber) 
			, c.TypographicNumber 
			, c.Year 
		'
	if @showCount = 1
		set @commandText = ' select count(*) '
	
	set @commandText = @commandText + 
		'
		from dbo.CommonNationalExamCertificate c with (nolock) 
		where 
			c.[Year] between @yearFrom and @yearTo 
			and (@lastName is null 
				or c.LastName collate cyrillic_general_ci_ai like @lastName)
			and (@firstName is null 
				or c.FirstName collate cyrillic_general_ci_ai like @firstName)
			and (@patronymicName is null 
				or c.PatronymicName collate cyrillic_general_ci_ai like @patronymicName) 
			and (@internalPassportSeria is null 
				or c.InternalPassportSeria like @internalPassportSeria) 
			and (@passportNumber is null 
				or c.PassportNumber like @passportNumber) 
			and (@typographicNumber is null 
				or c.TypographicNumber like @typographicNumber)
			and (@Number is null 
				or c.Number like @Number) '	

	if @showCount = 0
		set @commandText = @commandText + 
			'select 
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
				, search.Year 
				, case when ed.[ExpireDate] is null then ''Не найдено'' else 
					case when cne_certificate_deny.Id is not null then ''Аннулировано'' else 
					  case when getdate() <= ed.[ExpireDate] then ''Действительно'' 
					  else ''Истек срок'' end end end as [Status]
				, unique_cheks.UniqueIHEaFCheck
			 from @Search search
				left outer join dbo.ExamCertificateUniqueChecks unique_cheks
					on unique_cheks.Id = search.CertificateId 
				left outer join dbo.CommonNationalExamCertificateDeny cne_certificate_deny with (nolock) 
					on cne_certificate_deny.[Year] between @yearFrom and @yearTo 
						and search.CertificateNumber = cne_certificate_deny.CertificateNumber 
				left outer join dbo.Region region with (nolock) 
					on region.[Id] = search.RegionId 
				left join ExpireDate ed on  ed.year = search.year 
			 where row between @startRowIndex and (@startRowIndex+@maxRowCount-1)
			 exec dbo.RegisterEvent 
				@accountId = @editorAccountId  
				, @ip = @ip 
				, @eventCode = @eventCode 
				, @sourceEntityIds = ''0''  
				, @eventParams = @eventParams 
				, @updateId = null ' 
		
	exec sp_executesql @commandText
		, N'@lastName nvarchar(255)
			, @firstName nvarchar(255)
			, @patronymicName nvarchar(255)
			, @internalPassportSeria nvarchar(255)
			, @passportNumber nvarchar(255)
			, @typographicNumber nvarchar(255) 
			, @Number nvarchar(255)
			, @yearFrom int, @yearTo int 
			, @editorAccountId bigint
			, @ip nvarchar(255) 
			, @eventCode nvarchar(255) 
			, @eventParams nvarchar(4000) 
			, @startRowIndex int
			, @maxRowCount int
			'
		, @lastName
		, @firstName
		, @patronymicName
		, @internalPassportSeria
		, @passportNumber
		, @typographicNumber
		, @Number
		, @yearFrom
		, @YearTo
		, @editorAccountId
		, @ip
		, @eventCode
		, @eventParams
		, @startRowIndex
		, @maxRowCount

	return 0
end
