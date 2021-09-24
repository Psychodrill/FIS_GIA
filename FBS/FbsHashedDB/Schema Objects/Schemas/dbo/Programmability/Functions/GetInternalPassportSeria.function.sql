
------------------------------------------------
-- Функция получения серии паспорта
-- без пробелов
-- v.1.0: Created by Fomin Dmitriy 21.06.2008
-----------------------------------------------
create function [dbo].[GetInternalPassportSeria]
	(
	@passportSeria nvarchar(255)
	)
returns nvarchar(255)
as
begin
	return replace(@passportSeria, ' ', '')
end

