--------------------------------------------------
-- Порядковый номер статуса.
-- v.1.0: Created by Fomin Dmitriy 11.04.2008
--------------------------------------------------
CREATE function dbo.GetUserStatusOrder
	(
	@status nvarchar(255)
	)
returns int
as
begin
	return case
			when @status = 'consideration' then 1
			when @status = 'revision' then 2
			when @status = 'activated' then 3
			when @status = 'registration' then 4
			when @status = 'deactivated' then 5
			else 5
		end
end
