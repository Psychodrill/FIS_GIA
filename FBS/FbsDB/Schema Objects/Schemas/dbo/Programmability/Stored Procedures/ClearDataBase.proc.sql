-- ================================================
-- Приводит БД в состояние на момент начала эксплуатации системы
-- путем очистки таблиц
-- Очищаются только таблицы, связанные с проверками свидетельств
-- Создана: Юсупов Кирилл, 10.06.2010
-- ================================================
CREATE procedure [dbo].[ClearDataBase]
as
begin
	delete from dbo.EventLog where EventCode like 'CNE_%'
	delete from dbo.CNEWebUICheckLog
	delete from dbo.CheckCommonNationalExamCertificateLog
	delete from dbo.CommonNationalExamCertificateCheckLog

	delete from dbo.CommonNationalExamCertificateDenyLoadingTask
	delete from dbo.CommonNationalExamCertificateDenyLoadingTaskError
	delete from dbo.CommonNationalExamCertificateLoadingTask
	delete from dbo.CommonNationalExamCertificateLoadingTaskError

	delete from dbo.CommonNationalExamCertificateSubject
	delete from dbo.CommonNationalExamCertificate
    delete from dbo.CommonNationalExamCertificateDeny
	
	delete from dbo.ImportingCommonNationalExamCertificateSubject
	delete from dbo.ImportingCommonNationalExamCertificate

	delete from dbo.CommonNationalExamCertificateCheck
	delete from dbo.CommonNationalExamCertificateCheckBatch
	delete from dbo.CommonNationalExamCertificateRequest
	delete from dbo.CommonNationalExamCertificateRequestBatch

	delete from dbo.CommonNationalExamCertificateForm
	delete from dbo.CommonNationalExamCertificateFormNumberRange
	
	delete from dbo.CommonNationalExamCertificateSubjectCheck
	delete from dbo.CommonNationalExamCertificateSubjectForm

	delete from dbo.DeprecatedCommonNationalExamCertificateSubject
	delete from dbo.DeprecatedCommonNationalExamCertificate
	return 0
end

