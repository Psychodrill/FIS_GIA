-- exec dbo.GetNewUserLogin
-- =============================================
-- Генерируется новый логин пользователя. 
-- Логин генерируется в формате user<1..100000>'
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.03.2008
-- Приведение к стандарту
-- =============================================
CREATE proc dbo.GetNewUserLogin
	@login nvarchar(255) output
as
begin
	declare 
		@newLogin nvarchar(255)
		, @needNewLogin int
	set @needNewLogin = 1
	while @needNewLogin = 1 begin
		set @newLogin = N'user' + convert(nvarchar, Floor(Rand(CheckSum(NewId())) * 100000))
		if not exists(select 1 
						from
							dbo.Account account with (nolock, fastfirstrow)
						where
							account.[Login] = @newLogin)
			set @needNewLogin = 0
	end
	
	set @login = @newLogin

	return 0
end
