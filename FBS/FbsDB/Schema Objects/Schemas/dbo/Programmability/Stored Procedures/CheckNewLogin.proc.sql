-- exec dbo.CheckNewLogin

-- =============================================
-- Проверка нового логина.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modofied by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- =============================================
CREATE procedure dbo.CheckNewLogin
	@login nvarchar(255)
as
begin
	declare @isExists bit

	if exists(select 1
			from 
				dbo.Account account with (nolock, fastfirstrow)
			where
				account.[Login] = @login)
		set @isExists = 1
	else
		set @isExists = 0

	select
		@login [Login]
		, @isExists IsExists

	return 0
end
