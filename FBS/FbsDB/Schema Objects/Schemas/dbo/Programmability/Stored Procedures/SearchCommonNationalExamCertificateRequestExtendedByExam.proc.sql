-- exec dbo.SearchCommonNationalExamCertificateRequestExtendedByExam

-- =============================================
-- Получить список проверок.
-- v.1.0: Created by Fomin Dmitriy 18.07.2008
-- Создана по SearchCommonNationalExamCertificateRequest.
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]
	@login nvarchar(255)
	, @batchId bigint
as
begin
	declare 
		@innerBatchId bigint
		, @accountId bigint
		, @commandText nvarchar(4000)
		, @declareCommandText nvarchar(4000)
		, @viewSelectCommandText nvarchar(4000)
		, @pivotSubjectColumns nvarchar(4000)
		, @viewSelectPivot1CommandText nvarchar(4000)
		, @viewSelectPivot2CommandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @sortColumn nvarchar(20) 
		, @sortAsc bit 

	set @commandText = ''
	set @pivotSubjectColumns = ''
	set @viewSelectPivot1CommandText = ''
	set @viewSelectPivot2CommandText = ''
	set @viewCommandText = ''
	set @declareCommandText = ''
	set @sortColumn = N'Id'
	set @sortAsc = 1
	
	if @batchId is not null
		set @innerBatchId = dbo.GetInternalId(@batchId)

	select
		@accountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	set @declareCommandText = 
		N'declare @search table 
			(
			BatchId bigint
			, CertificateNumber nvarchar(255)
			, LastName nvarchar(255)
			, FirstName nvarchar(255)
			, PatronymicName nvarchar(255)
			, PassportSeria nvarchar(255)
			, PassportNumber nvarchar(255)
			, IsExist bit
			, SourceCertificateId  bigint
			, SourceCertificateYear int
			)
		'

	set @declareCommandText = @declareCommandText +
		N'declare @request table 
			(
			LastName nvarchar(255)
			, FirstName nvarchar(255)
			, PatronymicName nvarchar(255)
			, PassportSeria nvarchar(255)
			, PassportNumber nvarchar(255)
			)
		'

	set @commandText = @commandText +
		'insert into @request 
		select distinct
			cne_certificate_request.LastName 
			, cne_certificate_request.FirstName 
			, cne_certificate_request.PatronymicName 
			, cne_certificate_request.PassportSeria 
			, cne_certificate_request.PassportNumber 
		from 
			dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
				inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
					on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
		where
			cne_certificate_request_batch.OwnerAccountId = <accountId> 
			and cne_certificate_request_batch.[Id] = <innerBatchId>
			and cne_certificate_request_batch.IsProcess = 0 
		'
	
	set @commandText = @commandText +
		N'insert into @search
		select 
			dbo.GetExternalId(<innerBatchId>) BatchId
			, cne_certificate_request.SourceCertificateNumber CertificateNumber
			, request.LastName LastName
			, request.FirstName FirstName
			, request.PatronymicName PatronymicName
			, request.PassportSeria PassportSeria
			, request.PassportNumber PassportNumber
			, case
				when not cne_certificate_request.SourceCertificateId is null then 1
				else 0
			end IsExist
			, cne_certificate_request.SourceCertificateId
			, cne_certificate_request.SourceCertificateYear
		from @request request
			left outer join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
				inner join dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
						and cne_certificate_request_batch.OwnerAccountId = <accountId> 
						and cne_certificate_request_batch.[Id] = <innerBatchId>
						and cne_certificate_request_batch.IsProcess = 0 
				on request.FirstName = cne_certificate_request.FirstName
					and request.LastName = cne_certificate_request.LastName
					and request.PatronymicName = cne_certificate_request.PatronymicName
					and request.PassportSeria = cne_certificate_request.PassportSeria
					and request.PassportNumber = cne_certificate_request.PassportNumber
					and cne_certificate_request.IsDeny = 0
		'

	set @declareCommandText = @declareCommandText +
		N' declare @subjects table  
			( 
			CertificateId bigint 
			, Mark numeric(5,1) 
			, HasAppeal bit  
			, SubjectCode nvarchar(255)  
			, HasExam bit
			) 
		'

	set @commandText = @commandText +
		N'insert into @subjects  
		select
			cne_certificate_subject.CertificateId 
			, cne_certificate_subject.Mark
			, cne_certificate_subject.HasAppeal
			, subject.Code
			, 1 
		from	
			dbo.CommonNationalExamCertificateSubject cne_certificate_subject
				left outer join dbo.Subject subject
					on subject.SubjectId = cne_certificate_subject.SubjectId
		where 
			exists(select 1 
					from @search search
					where cne_certificate_subject.CertificateId = search.SourceCertificateId
						and cne_certificate_subject.[Year] = search.SourceCertificateYear)
		' 
	
	set @viewSelectCommandText = 
		N'select
			search.BatchId
			, search.CertificateNumber
			, search.LastName
			, search.FirstName
			, search.PatronymicName
			, search.PassportSeria
			, search.PassportNumber
			, search.IsExist
		'

	set @viewCommandText = 
		N'from @search search '

	declare
		@subjectCode nvarchar(255)
		, @pivotSelect nvarchar(4000)

	set @pivotSelect = ''

	declare subject_cursor cursor forward_only for
	select 
		[subject].Code
	from 
		dbo.Subject [subject]

	open subject_cursor 
	fetch next from subject_cursor into @subjectCode
	while @@fetch_status = 0
		begin
		if len(@pivotSubjectColumns) > 0
			set @pivotSubjectColumns = @pivotSubjectColumns + ','
		set @pivotSubjectColumns = @pivotSubjectColumns + replace('[<code>]', '<code>', @subjectCode)
		
		set @pivotSelect = @pivotSelect + 
			N' , isnull(exam_pvt.[<code>], 0) [<code>HasExam] '
				
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

	set @viewCommandText = @viewCommandText + 
		N'left outer join (select 
			subjects.CertificateId
			, subjects.SubjectCode
			, cast(subjects.HasExam as int) HasExam 
			from @subjects subjects) subjects
				pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
			on search.SourceCertificateId = exam_pvt.CertificateId '
			
	set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)

	set @viewCommandText = @viewCommandText

	set @commandText = replace(
			replace(@commandText, '<innerBatchId>', @innerBatchId), '<accountId>', @accountid)

	exec (@declareCommandText + @commandText + @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText)
	return 0
end

