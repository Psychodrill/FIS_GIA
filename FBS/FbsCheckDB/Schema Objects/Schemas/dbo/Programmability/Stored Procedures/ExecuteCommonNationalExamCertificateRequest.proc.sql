
-- exec ExecuteCommonNationalExamCertificateRequest
-- ==================================================
-- Запрос сертификатов ЕГЭ
-- v.1.0: Created by Sedov A.G. 26.05.2008
-- v.1.1: Modified by Fomin Dmitriy 31.05.2008
-- Исправление ошибок обработки. 
-- Добавлены поля IsDeny, DenyComment.
-- v.1.2: Modified by Sedov Anton 02.06.2008
-- В результате учитываются аннулированые сертификаты.
-- v.1.3: Modified by Fomin Dmitriy 02.06.2008
-- Работа над ошибками.
-- v.1.4: Modified by Sedov Anton 05.06.2008
-- Выполняется выбор базы данных, на которой
-- выполнится  процедура.
-- ==================================================
CREATE procedure dbo.ExecuteCommonNationalExamCertificateRequest
	@batchId bigint
as
begin
	declare
		@commandText nvarchar(4000)
		, @chooseDbText nvarchar (4000)

	set @chooseDbText = ''

	set @chooseDbText = 'use <databaseName> '
	set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())	
	
	set @commandText = 
		N'
		update exam_certificate_request_batch
		set
			UpdateDate = GetDate()
			, IsProcess = 0
			, IsCorrect = case 
				when not exists(select 1
						from dbo.CommonNationalExamCertificateRequest certificate_request with(nolock)
						where certificate_request.BatchId = exam_certificate_request_batch.Id 
							and certificate_request.IsCorrect = 0) then 1
				else 0
			end
		from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(rowlock)
		where exam_certificate_request_batch.Id = <BatchIdentifier> '

	set @commandText = replace(@commandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))

	exec (@chooseDbText + @commandText)	

	return 0
end
