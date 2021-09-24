
-- dbo.GetCommonNationalExamCertificateRequestBatch
-- ==============================================
-- Вывести запись из таблицы  пакетов запросов 
-- по ИД пакета
-- v.1.0: Created by Sedov Anton 28.05.2008
-- ===============================================
CREATE procedure dbo.GetCommonNationalExamCertificateRequestBatch
	@id bigint 
as
begin
	declare 
		@chooseDbText nvarchar(4000)
		, @commandText nvarchar(4000)
	
	set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
	
	set @commandText = 
		N'
		select 
			exam_certificate_request_batch.Id Id
			, exam_certificate_request_batch.Executing Executing
			, exam_certificate_request_batch.Batch Batch
		from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(nolock)
		where exam_certificate_request_batch.Id = @id
		'

	set @commandText = @chooseDbText + @commandText	

	exec sp_executesql @commandText
		, N'@id bigint'
		, @id
	
	return 0 
end
