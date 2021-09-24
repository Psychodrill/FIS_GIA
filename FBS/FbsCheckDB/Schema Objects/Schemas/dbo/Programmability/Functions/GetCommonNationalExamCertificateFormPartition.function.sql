
-- ================================================
-- Получить секцию бланков сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 08.06.2008
-- ================================================
CREATE function dbo.GetCommonNationalExamCertificateFormPartition
	(
	@regionId int
	, @year int
	)
returns bigint
as
begin
	return @year * 1000 + @regionId
end
