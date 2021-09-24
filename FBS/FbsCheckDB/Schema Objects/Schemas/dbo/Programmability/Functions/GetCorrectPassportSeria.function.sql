
-- =============================================
-- Получить корректную серию паспорта.
-- v.1.0: Created by Fomin Dmitriy 21.06.2008
-- =============================================
CREATE function dbo.GetCorrectPassportSeria
	(
	@passportSeria nvarchar(255)
	)
returns nvarchar(255)
as
begin
	return replace(@passportSeria, ' ', '')
end
