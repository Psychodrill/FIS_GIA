--------------------------------------------------
-- Можно ли редактировать пользователю свой
-- документ регистрации.
-- v1.0: Created by Fomin Dmitriy 04.04.2008
--------------------------------------------------
CREATE function dbo.CanEditUserAccountRegistrationDocument
	(
	@status nvarchar(255)
	)
returns bit
as
begin
	return case
			when not @status in ('activated', 'deactivated') then 1
			else 0
		end
end
