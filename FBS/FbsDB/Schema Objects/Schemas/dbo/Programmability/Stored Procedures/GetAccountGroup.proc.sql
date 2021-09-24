-- exec dbo.GetAccountGroup

-- =============================================
-- Получение группы пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- =============================================
CREATE procedure dbo.GetAccountGroup
	@login nvarchar(50)
as
begin
	select top 1
		account.[Login] [Login]
		, [group].Code GroupCode
	from dbo.GroupAccount group_account with (nolock, fastfirstrow)
		inner join dbo.Account account with (nolock, fastfirstrow)
			on group_account.AccountId = account.Id
		inner join dbo.[Group] [group] with (nolock, fastfirstrow)
			on [group].Id = group_account.GroupId
	where account.[Login] = @login

	return 0
end