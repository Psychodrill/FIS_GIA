
--------------------------------------------------
-- Получить годы актуальности сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
--------------------------------------------------
CREATE function dbo.GetCommonNationalExamCertificateActuality
	(
	)
returns @Actuality table (YearFrom int, YearTo int)
as 
begin
	insert into @Actuality
	select
		Year(GetDate()) - 5
		, Year(GetDate())

	return
end