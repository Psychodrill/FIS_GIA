-- =============================================
-- Получить роли учетной записи.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение офомления.
-- =============================================
CREATE procedure dbo.GetAccountRole
	@login nvarchar(255) -- логин учетной записи
as
begin

	select
		@login [Login]
		, account_role.RoleCode RoleCode
	from
		dbo.AccountRole account_role with (nolock, fastfirstrow)
			inner join dbo.Account account with (nolock, fastfirstrow)
				on account_role.AccountId = account.[Id]
	where
		account.[Login] = @login
		and (account_role.IsActiveCondition is null
			or exists(select 1
				from dbo.AccountRoleActivity activity
				where activity.AccountId = account_role.AccountId
					and activity.RoleId = account_role.RoleId
					and activity.UpdateDate >= account.UpdateDate))

	return 0
end
