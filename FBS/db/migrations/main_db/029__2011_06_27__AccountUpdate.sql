-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (29, '029__2011_06_27__AccountUpdate')
-- =========================================================================
GO




ALTER PROCEDURE [dbo].[UpdateAccountEsrp]
	@login nvarchar(255),
	@lastName nvarchar(255),
	@firstName nvarchar(255),
	@patronymicName nvarchar(255),
	@organizationId int,
	@phone nvarchar(255),
	@email nvarchar(255),
	@status nvarchar(255),
	@isActive bit
	--@ipAddresses nvarchar(400) = null
as
begin
	--существованние пользователя
	declare @exists table([login] nvarchar(255), isExists bit)
	insert @exists exec dbo.CheckNewLogin @login = @login
	
	declare 
		@isExists bit,
		@eventCode nvarchar(255),
		@accountId bigint,
		@innerStatus nvarchar(255),
		@confirmYear int,
		@currentYear int,
		@userGroupId int,
		@updateId	uniqueidentifier,
		@accountIds nvarchar(255)

	set @updateId = newid()
	
	select @isExists = user_exists.isExists
	from  @exists user_exists

	select @accountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	set @currentYear = year(getdate())
	set @confirmYear = @currentYear

	--declare @oldIpAddress table (ip nvarchar(255))

	--declare @newIpAddress table (ip nvarchar(255))
	
	--если логина нет - добавляем запись
	--если логин есть - меняем данные
	if @isExists = 0  -- внесение нового пользователя
	begin
		select 
			@eventCode = N'USR_REG'
			
		select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
	end
	else
	begin -- update существующего пользователя
		select 
			@accountId = account.[Id]
		from 
			dbo.Account account with (nolock, fastfirstrow)
		where
			account.[Login] = @login

		/*insert @oldIpAddress
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
		
		insert @newIpAddress
			(
			ip
			)
		select 
			ip_addresses.[value]
		from 
			dbo.GetDelimitedValues(@ipAddresses) ip_addresses*/
			
	end

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
				, null
				, null
				, @login
				, null
				, @lastName
				, @firstName
				, @patronymicName
				, @organizationId
				, 0
				, @confirmYear
				, @phone
				, @email
				, null
				, null
				, @isActive
				, @status
				, null
				, null

			if (@@error <> 0)
				goto undo

			select @accountId = scope_identity()

			if (@@error <> 0)
				goto undo

			/*insert dbo.AccountIp
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
				goto undo*/
		end
		else
		begin -- update существующего пользователя
			update account
			set
				UpdateDate = getdate()
				, UpdateId = @updateId
				, EditorAccountID = null
				, EditorIp = null
				, LastName = @lastName
				, FirstName = @firstName
				, PatronymicName = @patronymicName 
				, phone = @phone
				, email = @email
				, [Status] = @status
				, IsActive = @isActive
				, IpAddresses = null--@ipAddresses
				, HasFixedIp = null
			from
				dbo.Account account with (rowlock)
			where
				account.[Id] = @accountId

			if (@@error <> 0)
				goto undo

			/*if exists(select 
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
			end*/
		end

	if @@trancount > 0
		commit tran insert_update_account_tran

	/*set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId*/
	
	return 0

	undo:

	rollback tran insert_update_account_tran

	return 1
end
go