

-- =============================================
-- Получение информации об учетной записи 
-- внутреннего пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- v.1.1: Modified by Makarev Andrey 10.04.2008
-- Добавлено поле Account.IpAddresses
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.2.0: Modified by A. Vinichenko 12.04.2011
-- =============================================
CREATE procedure [dbo].[GetAccount]
	@login nvarchar(255)
as
begin
	select
		account.[Login] [Login]
		, account.LastName LastName 
		, account.FirstName FirstName
		, account.PatronymicName PatronymicName
		, account.Email Email
		, account.Phone Phone
		, account.IsActive IsActive
		, account.IpAddresses IpAddresses
		, account.HasFixedIp HasFixedIp
		, account.UpdateDate UpdateDate
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	return 0
end
