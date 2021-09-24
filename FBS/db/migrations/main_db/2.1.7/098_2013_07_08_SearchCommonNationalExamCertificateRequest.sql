insert into Migrations(MigrationVersion, MigrationName) values (98, '098_2013_07_08_SearchCommonNationalExamCertificateRequest.sql')
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
			pk int identity(1,1) primary key
			, Id bigint
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
			, SourceCertificateId uniqueidentifier
			, SourceCertificateYear int
			, TypographicNumber nvarchar(255)
			, [Status] nvarchar(255)
			, ParticipantID uniqueidentifier
			)
		'

	if isnull(@showCount, 0) = 0
		set @commandText = 
			N'select <innerHeader> 
				dbo.GetExternalId(cne_certificate_request.Id) [Id]
				, dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
				, cne_certificate_request.SourceCertificateNumber CertificateNumber
				, cne_certificate_request.LastName 
				, cne_certificate_request.FirstName 
				, cne_certificate_request.PatronymicName
				, cne_certificate_request.PassportSeria
				, cne_certificate_request.PassportNumber
				, case
					when not cne_certificate_request.ParticipantID is null then 1
					else 0
				  end IsExist
				, region.Name RegionName
				, region.Code RegionCode
				, isnull(cne_certificate_request.IsDeny, 0) IsDeny 
				, cne_certificate_request1.DenyComment DenyComment
				, cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
				, cne_certificate_request.SourceCertificateIdGuid
				, cne_certificate_request.SourceCertificateYear
				, cne_certificate_request.TypographicNumber
				, case when cne_certificate_request.ParticipantID is null then ''Не найдено'' 
					   else 
							case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно'' 
								 else ''Истек срок'' end end as [Status]
				, cne_certificate_request.ParticipantID
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)											
					join 
					(
						SELECT distinct cne_certificate_request.Id,cne_certificate_request.BatchId,cne_certificate_request.SourceCertificateYear,cne_certificate_request.SourceRegionId, 
							   cne_certificate_request.SourceCertificateNumber,cne_certificate_request.ParticipantID,
							   isnull(cne_certificate_request.LastName, a.Surname) LastName,
							   isnull(cne_certificate_request.FirstName, a.Name) FirstName,
							   isnull(cne_certificate_request.PatronymicName, a.SecondName) PatronymicName,
							   isnull(cne_certificate_request.PassportSeria, a.DocumentSeries) PassportSeria,
							   isnull(cne_certificate_request.PassportNumber, a.DocumentNumber) PassportNumber,
							   cne_certificate_request.IsDeny, cne_certificate_request.DenyNewCertificateNumber, 
							   cne_certificate_request.SourceCertificateIdGuid, cne_certificate_request.TypographicNumber,
							   b.LicenseNumber AS Number, b.UseYear AS Year
						FROM dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock) 
							left join prn.Certificates AS b with(nolock) on cne_certificate_request.[SourceCertificateYear] = b.useyear 
																				and cne_certificate_request.SourceCertificateIdGuid = b.CertificateID
							left JOIN rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID					
					) cne_certificate_request
					on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
					join dbo.CommonNationalExamCertificateRequest cne_certificate_request1 with (nolock) on cne_certificate_request1.Id = cne_certificate_request.[Id]
					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId		
					left join [ExpireDate] as ed with (nolock) 
						on cne_certificate_request.[SourceCertificateYear] = ed.[Year]										 
			where 1 = 1 '
	else
		set @commandText = 
			N'
			select count(distinct cne_certificate_request.Id)
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

		set @commandText = replace(
									replace(
											replace(
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
					id int identity(1,1)
					, ParticipantID uniqueidentifier
					, CertificateId uniqueidentifier 
					, Mark nvarchar(10)
					, HasAppeal bit  
					, SubjectCode nvarchar(255)  
					, HasExam bit
					, isCorrect int
					, primary key(id,ParticipantID, SubjectCode)
					) 
				'

			set @commandText = @commandText +
				N'insert into @subjects  
				select
					cne_certificate_subject.ParticipantFK
					, cne_certificate_subject.CertificateFK 
					, case when cne_certificate_subject.[Mark] < mm.[MinimalMark] 
						   then ''!'' 
						   else '''' 
					  end + 
						replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
					, cne_certificate_subject.HasAppeal
					, subject.Code
					, 1 
					, case when search.SourceCertificateId is not null then 1 else 0 end
				from	
					[prn].CertificatesMarks cne_certificate_subject with(nolock)
					left join @search search on cne_certificate_subject.CertificateFK = search.SourceCertificateId
								and cne_certificate_subject.[UseYear] = search.SourceCertificateYear
					left join dbo.Subject subject on subject.SubjectId = cne_certificate_subject.SubjectCode
					left join [MinimalMark] as mm on cne_certificate_subject.[SubjectCode] = mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
				where 
					exists(select 1 
							from @search search
							where cne_certificate_subject.ParticipantFK = search.ParticipantID
								and cne_certificate_subject.[UseYear] = search.SourceCertificateYear)								
				' 
		end
		
		set @viewSelectCommandText = 
			N'select
				search.Id 
				, search.BatchId
				, case when search.SourceCertificateId is null and ParticipantID is not null then ''Нет свидетества'' else search.CertificateNumber end CertificateNumber
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
				, search.ParticipantID
			'

		set @viewCommandText = 
			N' ,unique_cheks.UniqueIHEaFCheck
			from @search search 
			left outer join dbo.ExamCertificateUniqueChecks unique_cheks
					on unique_cheks.idGUID = search.SourceCertificateId '

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
				N'
				left join (select 
					subjects.CertificateId
					, subjects.Mark 
					, subjects.SubjectCode
					, subjects.ParticipantID ParticipantId1
					, isCorrect
					from @subjects subjects) subjects
						pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt
							on 1= case when search.SourceCertificateId = mrk_pvt.CertificateId then 1 
									   when mrk_pvt.ParticipantId1=search.ParticipantId and search.SourceCertificateId is null and mrk_pvt.isCorrect=0 then 1
									   else 0 
								  end
				left join (select 
					subjects.CertificateId
					, cast(subjects.HasAppeal as int) HasAppeal 
					, subjects.SubjectCode
					, subjects.ParticipantID ParticipantId1
					, isCorrect
					from @subjects subjects) subjects
						pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as apl_pvt
							on 1= case when search.SourceCertificateId = mrk_pvt.CertificateId then 1 
									   when mrk_pvt.ParticipantId1=search.ParticipantId and search.SourceCertificateId is null and apl_pvt.isCorrect=0  then 1
									   else 0 
								  end 
								and mrk_pvt.CertificateId=apl_pvt.CertificateId '
				set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
					
		if isnull(@isExtendedByExam, 0) = 1
		begin
			set @viewCommandText = @viewCommandText + 
				N'left outer join (select 
					subjects.CertificateId
					, subjects.SubjectCode
					, cast(subjects.HasExam as int) HasExam 
					, subjects.ParticipantID ParticipantId1
					from @subjects subjects) subjects
						pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
					on search.SourceCertificateId = exam_pvt.CertificateId '
					
			set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
	end

	set @viewCommandText = @viewCommandText + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

	print @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText
	
	exec sp_executesql @commandText
		, N'@innerBatchId bigint'
		, @innerBatchId
		
	return 0
end
go

