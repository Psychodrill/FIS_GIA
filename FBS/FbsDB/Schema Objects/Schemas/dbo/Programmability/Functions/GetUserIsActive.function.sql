--------------------------------------------------
-- Возвращает состояние действующей записи пользователя.
-- v1.0: Created by Fomin Dmitriy 10.04.2008
--------------------------------------------------
CREATE function dbo.GetUserIsActive
	(
	@status nvarchar(255)
	)
returns nvarchar(255) 
as  
begin
	return case
			when @status = N'deactivated' then 0
			else 1
		end
end

	
	