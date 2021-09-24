
-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch
-- ================================================================
-- Изменить поле Executing в пакете проверки для сертификатов
-- v.1.0: Created by Sedov A.G. 28.05.2008 
-- ================================================================
CREATE procedure dbo.UpdateCommonNationalExamCertificateCheckBatch
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
		update exam_certificate_check_batch
		set exam_certificate_check_batch.Executing = @executing
		from dbo.CommonNationalExamCertificateCheckBatch exam_certificate_check_batch with(rowlock)
		where exam_certificate_check_batch.Id = @id '
	
	set @commandText = @chooseDbText + @commandText


	exec sp_executesql @commandText
		, N'@id bigint, @executing bit'
		, @id 
		, @executing 
	
	return 0
end
