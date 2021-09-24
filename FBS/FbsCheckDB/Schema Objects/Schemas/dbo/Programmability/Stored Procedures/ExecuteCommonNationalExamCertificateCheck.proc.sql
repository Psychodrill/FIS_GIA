
-- exec dbo.ExecuteCommonNationalExamCertificateCheck
-- =======================================================
-- Проверка сертификатов  ЕГЭ.
-- v.1.0: Created by Sedov Anton 23.05.2008
-- v.1.1: Modified by Sedov Anton 27.05.2008
-- Добавлено ограничение сертификатов по годам
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Убрана колонка HasAppeal. 
-- Исправлена логика работы.
-- Добавлены колонки IsDeny, DenyComment.
-- v.1.3: Modified by Sedov Anton 02.06.2008
-- Оптимизирована работа процедуры
-- v.1.4: Modified by Sedov Anton 05.06.2008
-- Выполняется выбор базы данных, на которой
-- выполнится  процедура. 
-- =======================================================
CREATE procedure dbo.ExecuteCommonNationalExamCertificateCheck
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
		update certificate_check_batch
		set
			IsProcess = 0
			, IsCorrect = case 
				when not exists(select 1 
					from dbo.CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
					where exam_certificate_check.BatchId = certificate_check_batch.Id
						and exam_certificate_check.IsCorrect = 0) then 1
				else 0
			end
			, UpdateDate = GetDate()
		from dbo.CommonNationalExamCertificateCheckBatch certificate_check_batch with(rowlock)
		where certificate_check_batch.Id = <BatchIdentifier> '
	
	set @commandText = replace(@commandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))

	exec (@chooseDbText + @commandText)
	
	return 0

end
