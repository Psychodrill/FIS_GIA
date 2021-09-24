--------------------------------------------------
-- Возвращает статус пользователя.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Убран параметр isActive, теперь он вычисляется 
-- на основании статуса, а не наоборот.
-- v.1.2: Modified by Fomin Dmitriy 19.04.2008
-- Статус корректируется автоматически.
--------------------------------------------------
CREATE function dbo.GetUserStatus
	(
	@confirmYear int
	, @status nvarchar(255)
	, @currentYear int
	, @registrationDocument image
	)
returns nvarchar(255) 
as  
begin
	set @status = isnull(@status, N'registration')
	if @confirmYear < Year(GetDate()) 
		set @status = N'registration'

	return case
			when not @registrationDocument is null and @status = N'registration'
				then N'consideration'
			when @registrationDocument is null and not @status in (N'activated', N'deactivated')
				then N'registration'
			else @status
		end
end

	
	