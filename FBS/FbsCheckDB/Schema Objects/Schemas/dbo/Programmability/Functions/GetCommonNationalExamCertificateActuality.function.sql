
--------------------------------------------------
-- Получить годы актуальности сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
-- v.1.1: Modified by Fomin Dmitriy 02.06.2008
-- С 2008 года свидетельства ЕГЭ выдаются на 1,5 года.
--------------------------------------------------
CREATE function dbo.GetCommonNationalExamCertificateActuality
	(
	)
returns @Actuality table (YearFrom int, YearTo int)
as 
begin
	-- С 2008 года сертификаты выдаются на 1,5 года с момента сдачи экзамена.
	if Year(GetDate()) = 2008
		insert into @Actuality
		select
			Year(GetDate())
			, Year(GetDate())
	else
		insert into @Actuality
		select
			Year(GetDate()) - 5
			, Year(GetDate())

	return
end
