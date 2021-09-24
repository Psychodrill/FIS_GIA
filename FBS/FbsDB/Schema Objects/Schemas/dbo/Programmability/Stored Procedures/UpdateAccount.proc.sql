-- exec dbo.UpdateAccount
-- =============================================
-- Сохранить учетную запись.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.3: Modified by Makarev Andrey 16.04.2008
-- Измение процедуры GetDelimitedValues().
-- v.1.4: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.5: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- =============================================
CREATE procedure [dbo].[UpdateAccount]
	@login nvarchar(255)
	, @passwordHash nvarchar(255) = null
	, @lastName nvarchar(255)
	, @firstName nvarchar(255)
	, @patronymicName nvarchar(255)
	, @phone nvarchar(255)
	, @email nvarchar(255)
	, @isActive bit
	, @ipAddresses nvarchar(4000) = null
	, @groupCode nvarchar(255) = null
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @hasFixedIp bit = null
as
begin
	declare @exists table([login] nvarchar(255), isExists bit)

	insert @exists exec dbo.CheckNewLogin @login = @login
	
	declare 
		@isExists bit
		, @eventCode nvarchar(255)
		, @editorAccountId bigint
		, @accountId bigint
		, @status nvarchar(255)
		, @innerStatus nvarchar(255)
		, @confirmYear int
		, @currentYear int
		, @userGroupId int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()

	select @userGroupId = [group].[Id]
	from dbo.[Group] [group] with (nolock, fastfirstrow)
	where [group].[Code] = @groupCode

	select @isExists = user_exists.isExists
	from  @exists user_exists

	select @editorAccountId = account.[Id]
	from  dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @editorLogin

	select @accountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	set @currentYear = year(getdate())

	set @confirmYear = @currentYear

	declare @oldIpAddress table (ip nvarchar(255))

	declare @newIpAddress table (ip nvarchar(255))

--если логина нет - добавляем запись и добавляем пользователя в группу
--если логин есть - меняем данные
	if @isExists = 0  -- внесение нового пользователя
	begin
		select 
			@status = case when @groupCode='User' then  null else 'activated' end,
			@hasFixedIp = isnull(@hasFixedIp, 1), @eventCode = N'USR_REG'

		select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
	end
	else
	begin -- update существующего пользователя
		select 
			@accountId = account.[Id]
			, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
		from 
			dbo.Account account with (nolock, fastfirstrow)
		where
			account.[Login] = @login

		insert @oldIpAddress
			(
			ip
			)
		select
			account_ip.Ip
		from
			dbo.AccountIp account_ip with (nolock, fastfirstrow)
		where
			account_ip.AccountId = @accountId

		set @eventCode = N'USR_EDIT'
	end

	if @hasFixedIp = 1
		insert @newIpAddress
			(
			ip
			)
		select 
			ip_addresses.[value]
		from 
			dbo.GetDelimitedValues(@ipAddresses) ip_addresses

	begin tran insert_update_account_tran

		if @isExists = 0  -- внесение нового пользователя
		begin
			insert dbo.Account
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Login]
				, PasswordHash
				, LastName
				, FirstName
				, PatronymicName
				, OrganizationId
				, IsOrganizationOwner
				, ConfirmYear
				, Phone
				, Email
				, RegistrationDocument
				, AdminComment
				, IsActive
				, Status
				, IpAddresses
				, HasFixedIp
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @login
				, @passwordHash
				, @lastName
				, @firstName
				, @patronymicName
				, null
				, 0
				, @confirmYear
				, @phone
				, @email
				, null
				, null
				, @isActive
				, @status
				, @ipAddresses
				, @hasFixedIp

			if (@@error <> 0)
				goto undo

			select @accountId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.AccountIp
				(
				AccountId
				, Ip
				)
			select
				@accountId
				, new_ip_address.ip
			from 
				@newIpAddress new_ip_address

			if (@@error <> 0)
				goto undo

			insert dbo.GroupAccount
				(
				GroupId
				, AccountID
				)
			select
				@userGroupId
				, @accountId

			if (@@error <> 0)
				goto undo

		end
		else
		begin -- update существующего пользователя
			update account
			set
				UpdateDate = getdate()
				, UpdateId = @updateId
				, EditorAccountID = @editorAccountId
				, EditorIp = @editorIp
				, LastName = @lastName
				, FirstName = @firstName
				, PatronymicName = @patronymicName 
				, phone = @phone
				, email = @email
				, IsActive = @isActive
				, IpAddresses = @ipAddresses
				, HasFixedIp = @hasFixedIp
			from
				dbo.Account account with (rowlock)
			where
				account.[Id] = @accountId

			if (@@error <> 0)
				goto undo

			if exists(select 
						1
					from
						@oldIpAddress old_ip_address
							full outer join @newIpAddress new_ip_address
								on old_ip_address.ip = new_ip_address.ip
					where
						old_ip_address.ip is null
						or new_ip_address.ip is null) 
			begin
				delete account_ip
				from 
					dbo.AccountIp account_ip
				where
					account_ip.AccountId = @accountId

				if (@@error <> 0)
					goto undo

				insert dbo.AccountIp
					(
					AccountId
					, Ip
					)
				select
					@accountId
					, new_ip_address.ip
				from 
					@newIpAddress new_ip_address

				if (@@error <> 0)
					goto undo
			end
		end

	if @@trancount > 0
		commit tran insert_update_account_tran

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_update_account_tran

	return 1
end

