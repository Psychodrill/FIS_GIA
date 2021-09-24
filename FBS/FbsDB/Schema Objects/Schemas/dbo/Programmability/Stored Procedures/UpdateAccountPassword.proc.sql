-- exec dbo.UpdateAccountPassword

-- =============================================
-- Сохранить пароль пользователя.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 04.05.2008
-- Добавлен параметр password для обратной совместимости систем.
-- =============================================
CREATE proc dbo.UpdateAccountPassword
	@login nvarchar(255)
	, @passwordHash nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @password nvarchar(255) = null -- !временно
as
begin

	declare
		@editorAccountId bigint
		, @accountId bigint
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()

	select 
		@accountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	update account
	set
		PasswordHash = @passwordHash
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, UpdateDate = GetDate()
		, UpdateId = @updateId
	from
		dbo.Account account with (rowlock)
	where
		account.[Id] = @accountId

-- временно
	if isnull(@password, '') <> '' and N'User' = (select 
						[group].[code]
					from
						dbo.[Group] [group]
							inner join dbo.GroupAccount group_account
								on [group].[Id] = group_account.GroupId
					where
						group_account.AccountId = @accountId)
	begin
		if exists(select 
					1
				from
					dbo.UserAccountPassword user_account_password
				where
					user_account_password.AccountId = @accountId)
		begin
			update user_account_password
			set
				[Password] = @password
			from
				dbo.UserAccountPassword user_account_password
			where
				user_account_password.AccountId = @accountId
		end
		else
		begin
			insert dbo.UserAccountPassword
				(
				AccountId
				, [Password]
				)
			select 
				@accountId
				, @password
		end
	end

	exec dbo.RefreshRoleActivity @accountId = @accountId

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = N'USR_PASSW'
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0
end
