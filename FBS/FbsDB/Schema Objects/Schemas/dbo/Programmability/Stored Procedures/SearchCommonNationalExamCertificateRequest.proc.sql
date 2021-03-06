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
		, @innerOrder nvarchar(max)
		, @outerOrder nvarchar(max)
		, @resultOrder nvarchar(max)
		, @innerSelectHeader nvarchar(max)
		, @outerSelectHeader nvarchar(max)
		, @declareCommandText nvarchar(max)
		, @viewSelectCommandText nvarchar(max)
		, @pivotSubjectColumns nvarchar(max)
		, @viewSelectPivot1CommandText nvarchar(max)
		, @viewSelectPivot2CommandText nvarchar(max)
		, @viewCommandText nvarchar(max)
		, @sortColumn nvarchar(max) 
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
	set @isExtendedByExam = 1

	declare @nam nvarchar(max)
	select @nam=cast(newid() as nvarchar(max))	
	if @startRowIndex = 0 
		set @startRowIndex = 1
		
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
			pk int identity(1,1) 
			, Id bigint
			, BatchId bigint
			, CertificateNumber nvarchar(510)
			, LastName nvarchar(510)
			, FirstName nvarchar(510)
			, PatronymicName nvarchar(510)
			, PassportSeria nvarchar(510)
			, PassportNumber nvarchar(510)
			, IsExist int
			, RegionName nvarchar(510)
			, RegionCode nvarchar(510)
			, IsDeny int
			, DenyComment ntext
			, DenyNewCertificateNumber nvarchar(510)
			, SourceCertificateId uniqueidentifier
			, SourceCertificateYear int
			, TypographicNumber nvarchar(510)
			, [Status] nvarchar(510)
			, ParticipantID uniqueidentifier
			, IsTypographicNumber bit
			, primary key (pk,id)
			) '

	if isnull(@showCount, 0) = 0
	begin
		set @commandText = 
			N'					
			select 
				dbo.GetExternalId(cne_certificate_request.Id) [Id]
				, dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
				, cne_certificate_request.SourceCertificateNumber CertificateNumber
				, cne_certificate_request.LastName 
				, cne_certificate_request.FirstName 
				, cne_certificate_request.PatronymicName
				, cne_certificate_request.PassportSeria
				, cne_certificate_request.PassportNumber
				, case					
					when cne_certificate_request.ParticipantID is not null then 1					
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
					   when certificate_deny.CertificateFK is not null then ''Аннулировано''
					   when getdate() <= ed.[ExpireDate] then ''Действительно'' 
					   else ''Истек срок'' 
				  end as [Status]
				, cne_certificate_request.ParticipantID
				, IsTypographicNumber '
		set @commandText = 	@commandText + '		
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)											
					join 
					(
						SELECT distinct cne_certificate_request.Id,cne_certificate_request.BatchId,
							   cne_certificate_request.SourceCertificateYear,cne_certificate_request.SourceRegionId, 
							   cne_certificate_request.SourceCertificateNumber,cne_certificate_request.ParticipantID,
							   isnull(cne_certificate_request.LastName, a.Surname) LastName,
							   isnull(cne_certificate_request.FirstName, a.Name) FirstName,
							   isnull(cne_certificate_request.PatronymicName, a.SecondName) PatronymicName,
							   isnull(cne_certificate_request.PassportSeria, a.DocumentSeries) PassportSeria,
							   isnull(cne_certificate_request.PassportNumber, a.DocumentNumber) PassportNumber,
							   cne_certificate_request.IsDeny, cne_certificate_request.DenyNewCertificateNumber, 
							   cne_certificate_request.SourceCertificateIdGuid, isnull(cne_certificate_request.TypographicNumber,b.TypographicNumber) TypographicNumber,
							   b.LicenseNumber AS Number, b.UseYear AS Year,b.CertificateID 
						FROM dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock) 
						'
		if isnull(@maxRowCount,0) > 1
			set @commandText = @commandText + 
					'
					join @table cc on cc.ParticipantID=isnull(cne_certificate_request.ParticipantID,''BA2EAC45-000C-492D-81DE-4993EFB69A0E'')  
					'						
					
		set @commandText = @commandText + 
					'						
							left join prn.Certificates AS b with(nolock) on cne_certificate_request.[SourceCertificateYear] = b.useyear 
																				and cne_certificate_request.SourceCertificateIdGuid = b.CertificateID
							left JOIN rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and cne_certificate_request.[SourceCertificateYear] = a.useyear 				
					) cne_certificate_request
					on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
					join dbo.CommonNationalExamCertificateRequest cne_certificate_request1 with (nolock) on cne_certificate_request1.Id = cne_certificate_request.[Id]	

					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId		
					left join [ExpireDate] as ed with (nolock) 
						on cne_certificate_request.[SourceCertificateYear] = ed.[Year]										 
					left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow) on certificate_deny.[UseYear] = cne_certificate_request.[Year] and certificate_deny.CertificateFK = cne_certificate_request.CertificateID
			
			where 1 = 1 '			
	end
	else
	begin
		set @commandText = 
			N'
			select count(cne_certificate_request.Id)
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
						on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
			where 1 = 1 and cne_certificate_request_batch.[Id] = @innerBatchId 
				and cne_certificate_request_batch.IsProcess = 0 ' 
		exec sp_executesql @commandText, N'@innerBatchId bigint', @innerBatchId
		return
	end				
	
	set @commandText = @commandText +
			'   and cne_certificate_request_batch.[Id] = @innerBatchId 
				and cne_certificate_request_batch.IsProcess = 0 '

	set @commandText = ' insert into @search '+ @commandText	

	set @commandText = '
		declare @table table (pk int identity(1,1), ParticipantID uniqueidentifier, primary key(pk))
		
		insert @table
		select ParticipantID from
		(
			select ParticipantID, row_number() over (order by cc.ParticipantID '+case when @sortAsc = 1 then 'asc' else 'desc' end+') rn
			from 
			(
				select distinct isnull(ParticipantID,''BA2EAC45-000C-492D-81DE-4993EFB69A0E'') ParticipantID
				from CommonNationalExamCertificateRequest with (nolock) 
				where @innerBatchId = BatchId and '+cast(isnull(@maxRowCount,0) as nvarchar(50))+'='+cast(isnull(@maxRowCount,1) as nvarchar(50))+'
			) cc
		) cc
		where rn between 1 and '+cast(isnull(@maxRowCount,-1)+@startRowIndex-1 as nvarchar(50)) + 
		' '
		+@commandText
									
		if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
		begin
			set @declareCommandText = @declareCommandText +
				N' 
				declare @subjects table  
					( 
					id int identity(1,1)
					, ParticipantID uniqueidentifier
					, CertificateId uniqueidentifier 
					, Mark nvarchar(500)
					, HasAppeal bit  
					, SubjectCode nvarchar(500)  
					, HasExam bit
					, isCorrect int
					, SourceCertificateYear int
					, primary key(id,ParticipantID, CertificateId)
					) 					
				'

			set @commandText = @commandText +
				N'			
					
				insert into @subjects  
				select distinct 
					cne_certificate_subject.ParticipantFK
					, cne_certificate_subject.CertificateFK 
					, case when cne_certificate_subject.[Mark] < mm.[MinimalMark] 
						   then ''!'' 
						   else '''' 
					  end + 
						replace(cast(cne_certificate_subject.[Mark] as nvarchar(500)),''.'','','')
					, cne_certificate_subject.HasAppeal
					, subject.Code
					, 1 
					, case when s.SourceCertificateId is not null then 1 else 0 end
					, s.SourceCertificateYear
				from	
					[prn].CertificatesMarks cne_certificate_subject with(nolock)
					left join @search s on cne_certificate_subject.CertificateFK = s.SourceCertificateId
								and cne_certificate_subject.[UseYear] = s.SourceCertificateYear
					left join dbo.Subject subject with(nolock) on subject.SubjectId = cne_certificate_subject.SubjectCode
					left join [MinimalMark] as mm with(nolock) on cne_certificate_subject.[SubjectCode] = mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
				where 
					exists(select 1 
							from @search search
							where cne_certificate_subject.ParticipantFK = search.ParticipantID
								and cne_certificate_subject.[UseYear] = search.SourceCertificateYear)					
				' 
		end

		set @viewSelectCommandText = 
			N'
			select * into #mrk_pvt from (
				SELECT subjects.CertificateId
				  , subjects.Mark
				  , subjects.SubjectCode
				  , subjects.ParticipantID ParticipantId1
				  , isCorrect
				  , SourceCertificateYear
				FROM @subjects subjects) subjects
			PIVOT (min(Mark) FOR SubjectCode IN (<subject_columns>)) t 
			
			create index [IX_mrk_pvt_'+@nam+']  on #mrk_pvt (CertificateId, isCorrect, ParticipantId1, SourceCertificateYear)
			
			select * into #apl_pvt from (       
				SELECT subjects.CertificateId
					, cast(subjects.HasAppeal AS INT) HasAppeal
					, subjects.SubjectCode
					, subjects.ParticipantID ParticipantId1
					, isCorrect
					, SourceCertificateYear
				FROM @subjects subjects) subjects
			PIVOT (sum(HasAppeal) FOR SubjectCode IN (<subject_columns>)) t
			
			create index [IX_apl_pvt_'+@nam+'] on #apl_pvt (CertificateId, isCorrect, ParticipantId1, SourceCertificateYear)						
						
			select * into #exam_pvt from 
			(       
				select subjects.CertificateId
					, cast(subjects.HasExam as int) HasExam
					, subjects.SubjectCode
					, subjects.ParticipantID ParticipantId1
					, isCorrect
					, SourceCertificateYear
				from @subjects subjects 
			) subjects
			pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) t			
			
			create index [IX_exam_pvt_'+@nam+'] on #exam_pvt (CertificateId,isCorrect,ParticipantId1, SourceCertificateYear)	
			
			'
			
			if isnull(@maxRowCount,0)>0			
				set @viewSelectCommandText = @viewSelectCommandText+
			'				
			select * from 
			(
				select t.*,row_number() over (order by t.id '+case when @sortAsc = 1 then 'asc' else 'desc' end+') rn from 
				(	'
			set @viewSelectCommandText = @viewSelectCommandText+ '													   								
					select  
						search.Id 
						, search.BatchId
						, case when search.SourceCertificateId is null and ParticipantID is not null then ''Нет свидетельства'' else search.CertificateNumber end CertificateNumber
						, search.LastName
						, search.FirstName
						, search.PatronymicName 
						, search.PassportSeria
						, search.PassportNumber
						, case when IsTypographicNumber = 1 and SourceCertificateId is null then 0 
							   when ParticipantID is not null then 1
							   else 0 end  IsExist
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
						N' 
						, unique_cheks.UniqueIHEaFCheck
						from @search search 
						left join dbo.ExamCertificateUniqueChecks unique_cheks with(nolock)
							on unique_cheks.idGUID = search.SourceCertificateId				
						'

		if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
		begin 
			declare
				@subjectCode nvarchar(max)
				, @pivotSelect nvarchar(max)

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
								N'
								 LEFT JOIN #mrk_pvt mrk_pvt 
									ON 1 = CASE WHEN search.SourceCertificateId = mrk_pvt.CertificateId AND mrk_pvt.isCorrect = 1 THEN 1
												WHEN mrk_pvt.ParticipantId1 = search.ParticipantId AND search.SourceCertificateId IS NULL AND mrk_pvt.isCorrect = 0 THEN 1
												ELSE 0
										   END 
										   and mrk_pvt.ParticipantId1 = search.ParticipantId
								LEFT JOIN #apl_pvt apl_pvt 
									ON 1 = CASE WHEN search.SourceCertificateId = mrk_pvt.CertificateId AND apl_pvt.isCorrect = 1 THEN 1
												WHEN mrk_pvt.ParticipantId1 = search.ParticipantId AND search.SourceCertificateId IS NULL AND apl_pvt.isCorrect = 0 THEN 1
												ELSE 0
										   END 
										AND mrk_pvt.CertificateId = apl_pvt.CertificateId AND mrk_pvt.ParticipantId1 = apl_pvt.ParticipantId1 
										and apl_pvt.ParticipantId1 = search.ParticipantId
						   '
			set @viewSelectCommandText = replace(@viewSelectCommandText, '<subject_columns>', @pivotSubjectColumns)						   				
		end
					
		if isnull(@isExtendedByExam, 0) = 1
		begin
						set @viewCommandText = @viewCommandText + 
							N'left join #exam_pvt exam_pvt 
								ON 1 = CASE WHEN search.SourceCertificateId = exam_pvt.CertificateId AND exam_pvt.isCorrect = 1 THEN 1
												WHEN exam_pvt.ParticipantId1 = search.ParticipantId AND search.SourceCertificateId IS NULL AND exam_pvt.isCorrect = 0 THEN 1
												ELSE 0
										   END 
									AND mrk_pvt.CertificateId = exam_pvt.CertificateId AND mrk_pvt.ParticipantId1 = exam_pvt.ParticipantId1 
									and exam_pvt.ParticipantId1 = search.ParticipantId
								 	'
								
			if isnull(@maxRowCount,0)>0
				set @viewCommandText = @viewCommandText + '
					) t 
			 ) t
			where t.rn between '+cast(@startRowIndex as nvarchar(max))+' and '+cast(@maxRowCount+@startRowIndex-1 as nvarchar(max)) + '														
			
			drop table #mrk_pvt
			drop table #apl_pvt	
			drop table #exam_pvt			
					'
					
			set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
		else
		set @viewCommandText = @viewCommandText + '
					
			drop table #mrk_pvt
			drop table #apl_pvt	
			drop table #exam_pvt	
			'

	set @viewCommandText = @viewCommandText + @resultOrder 

	set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

	exec sp_executesql @commandText
		, N'@innerBatchId bigint'
		, @innerBatchId
		
	return 0
end
