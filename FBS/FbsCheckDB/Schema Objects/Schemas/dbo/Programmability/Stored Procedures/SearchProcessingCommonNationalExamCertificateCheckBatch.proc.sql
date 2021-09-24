-- exec SearchProcessingCommonNationalExamCertificateCheckBatch
-- ============================================================
-- Получение списока пакетов для проверки сертификатов ЕГЭ
-- , находящихся в обработке
-- v.1.0: Created by Sedov Anton 28.05.2008
-- v.1.1: Modified by Fomin Dmitriy 28.05.2008
-- Добавлено ограничение по IsProcess.
-- v.1.2: Modified by Fomin Dmitriy 28.05.2008
-- Убрано поле Batch - парсинг будет на уровне задачи.
-- ============================================================
create procedure [dbo].[SearchProcessingCommonNationalExamCertificateCheckBatch]
as
begin
	declare 
		@chooseDbText nvarchar(4000)
		, @commandText nvarchar(4000)

	set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
	
	set @commandText = 
		N'
		select 
			exam_certificate_check_batch.Id Id
			, exam_certificate_check_batch.Executing Executing
		from dbo.CommonNationalExamCertificateCheckBatch exam_certificate_check_batch with(nolock)
		where 
			exam_certificate_check_batch.IsProcess = 1
			and exam_certificate_check_batch.Executing is null '

	set @commandText = @chooseDbText + @commandText
	
	exec sp_executesql @commandText

	return 0
end