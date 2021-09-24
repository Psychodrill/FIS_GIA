--------------------------------------------------
-- Можно ли редактировать пользователю свою
-- учетную запись.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.0: Modified by Fomin Dmitriy 19.05.2008
-- Модифицировать анкету можно до утверждения ее
-- документом.
--------------------------------------------------
CREATE function dbo.CanEditUserAccount
	(
	@status nvarchar(255)
	, @confirmYear int
	, @currentYear int
	)
returns bit
as
begin
	return case
			when not @status in ('activated', 'deactivated') then 1
			else 0
		end
end
