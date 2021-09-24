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
alter proc [dbo].[SearchCommonNationalExamCertificateCheck]
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
				, CertificateId uniqueidentifier 
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
				'	, cne_check.SourceCertificateIdGuid CertificateId ' +
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
				, CertificateSubjectId uniqueidentifier 
				, SubjectCode nvarchar(255) 
				, CheckMark nvarchar(10) 
				, Mark nvarchar(10) 
				, HasAppeal int 
				, IsCorrect int 
				, primary key (CheckId, SubjectId)
				) '

		set @commandText = @commandText +
			' 					
			insert into @check_subject 
			select 
				  subject_check.CheckId 
				, subject_check.SubjectId
				, subject_check.SourceCertificateSubjectIdGuid 
				, subject.Code 
				, case when subject_check.[Mark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[Mark] as nvarchar(9)),''.'','','')
				, case when subject_check.[SourceMark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[SourceMark] as nvarchar(9)),''.'','','')
				, cast(subject_check.SourceHasAppeal as int) 
				, cast(subject_check.IsCorrect as int) 
			from dbo.CommonNationalExamCertificateSubjectCheck subject_check with (nolock) 
			inner join dbo.Subject subject on subject.SubjectId = subject_check.SubjectId 
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
					on unique_cheks.idGUID = search.CertificateId
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
						pivot (count(CertificateSubjectId) for SubjectCode in (<subject_columns>)) as 

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


/*
exec dbo.SearchCommonNationalExamCertificateCheck @login=N'rick_box@mail.ru',@batchId=301638626047,@startRowIndex=N'1',@maxRowCount=N'20'
*/