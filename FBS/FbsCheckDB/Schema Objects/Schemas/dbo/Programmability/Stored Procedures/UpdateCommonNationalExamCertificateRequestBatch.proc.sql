
-- exec dbo.UpdateCommonNationalExamCertificateRequestBatch
-- ================================================================
-- Изменить поле Executing в пакете проверки для запросов
-- v.1.0: Created by Sedov A.G. 28.05.2008 
-- v.1.1: Modified by Sedov Anton 05.06.2008 
-- Добавлен динамический выбор БД,
-- на которой будет выполнена процедура
-- ================================================================
CREATE procedure dbo.UpdateCommonNationalExamCertificateRequestBatch
	@id bigint
	, @executing bit
as
begin
	declare 
		@chooseDbText nvarchar(4000)
		, @commandText nvarchar(4000)

	set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
	
	set @commandText = 
		N'
		update exam_certificate_request_batch
		set exam_certificate_request_batch.Executing = @executing
		from dbo.CommonNationalExamCertificateRequestBatch exam_certificate_request_batch with(rowlock)
		where exam_certificate_request_batch.Id = @id '
	
	set @commandText = @chooseDbText + @commandText

	exec sp_executesql @commandText
		, N'@id bigint, @executing bit'
		, @id
		, @executing

	return 0
end
