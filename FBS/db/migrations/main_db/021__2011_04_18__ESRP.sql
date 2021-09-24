-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (21, '021__2011_04_18__ESRP')
-- =========================================================================
GO



ALTER PROC [dbo].[VerifyAccount]
	@login nvarchar(255)
	, @ip nvarchar(255)
AS
BEGIN

	DECLARE @isLoginValid bit
		, @isIpValid bit
		, @accountId bigint
		, @entityParams nvarchar(1000)
		, @sourceEntityIds nvarchar(255)
	
	SELECT @isLoginValid = 0, @isIpValid = 0

	SELECT @accountId = [Id], 
			@isLoginValid = 
				CASE 
					WHEN [Status] <> 'deactivated' 
					THEN 1 
					ELSE 0 
				END 
	FROM dbo.Account with (nolock)
	WHERE [Login] = @login

	-- IP не проверяем - он валидный при валидном пароле
	SET @isIpValid=@isLoginValid

	SET @entityParams = @login + N'|' +	@ip + N'|' +
			CONVERT(nvarchar, @isLoginValid)  + '|' +
			CONVERT(nvarchar, @isIpValid)

	SET @sourceEntityIds = CONVERT(nvarchar(255), ISNULL(@accountId,0))

	SELECT @login [Login], @ip Ip, @isLoginValid IsLoginValid, @isIpValid IsIpValid

	EXEC dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = N'USR_VERIFY'
		, @sourceEntityIds = @sourceEntityIds
		, @eventParams = @entityParams
		, @updateId = null

	RETURN 0
END
GO

--Столбец-идентификатор модели приемной кампании
--может принимать значения null
ALTER TABLE [dbo].[Organization2010]
ALTER COLUMN RCModel int null
GO

--Внесение изменнеий в таблицу Group
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS B 
	WHERE B.TABLE_SCHEMA = 'dbo' 
		AND  B.TABLE_NAME = 'Group'
		AND B.COLUMN_NAME = 'GroupIdEsrp'))
  ALTER TABLE [dbo].[Group]
  DROP COLUMN GroupIdEsrp
GO

ALTER TABLE [dbo].[Group]
	ADD GroupIdEsrp int null
GO
--Обновление пользователей ВУЗ/ССУЗ.
UPDATE [dbo].[Group]
SET GroupIdEsrp = 6
WHERE Id = 1
GO
--Обновление администраторов системы.
UPDATE [dbo].[Group]
SET GroupIdEsrp = 4
WHERE Id = 2
GO
--Проверяющий
UPDATE [dbo].[Group]
SET GroupIdEsrp = 5
WHERE Id = 4
GO
--Учредитель
UPDATE [dbo].[Group]
SET GroupIdEsrp = 10
WHERE Id = 6
GO
--РЦОИ
UPDATE [dbo].[Group]
SET GroupIdEsrp = 8
WHERE Id = 7
GO


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
ALTER procedure [dbo].[GetAccount]
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
GO


IF object_id('dbo.UpdateAccountEsrp') is not null
	DROP PROCEDURE [dbo].[UpdateAccountEsrp]
GO

CREATE PROCEDURE [dbo].[UpdateAccountEsrp]
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

	exec RefreshRoleActivity @accountId
	
	return 0

	undo:

	rollback tran insert_update_account_tran

	return 1
end
GO


IF object_id('dbo.UpdateGroupUserEsrp') is not null
	DROP PROCEDURE [dbo].[UpdateGroupUserEsrp]
GO

CREATE PROCEDURE [dbo].[UpdateGroupUserEsrp]
	@login nvarchar(255),
	@groupIdEsrp int,
	@groupsEsrp nvarchar(255)
AS
BEGIN
	declare @accountId int
	select @accountId = A.Id
	from Account A
	where A.[Login] = @login
	
	declare @groupId int
	select @groupId = G.Id
	from [Group] G
	where G.GroupIdEsrp = @groupIdEsrp
	
	if (@groupsEsrp!=null)
	begin
		declare @sql nvarchar(1000)
		set @sql =
		'delete from GroupAccount
		where 
			GroupAccount.GroupId in (select G.Id
									 from [Group] G
									 where
										G.GroupIdEsrp not in (' + @groupsEsrp + ') 
									 ) '+
			'and GroupAccount.AccountId = ' + cast(@accountId as nvarchar(255))
		exec sp_executesql @sql
	end
	
	if exists (select GA.GroupId
			   from GroupAccount GA
			   where GA.AccountId = @accountId
			   and GA.GroupId = @groupId
			   )
	begin
		
		update GroupAccount
		set GroupId = @groupId
		where AccountId = @accountId
	
	end
	else begin
	
		insert into GroupAccount (GroupId, AccountId)
		values (@groupId, @accountId)
	
	end
END
GO

--Изменение полей в таблице Account
ALTER TABLE [dbo].[Account]
ALTER COLUMN [EditorIp] [nvarchar](255) NULL
GO
ALTER TABLE [dbo].[Account]
ALTER COLUMN [HasFixedIp] [bit] NULL
GO

--Отключение триггера в таблице Account
DISABLE TRIGGER [dbo].[tgAccountLog]
ON [dbo].[Account]
GO

-- =============================================
-- Получение информации о пользователе.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.1.2: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.3: Modified by Fomin Dmitriy 07.07.2008
-- Добавлены поля EducationInstitutionTypeId, 
-- EducationInstitutionTypeName.
-- v.2.0 Modified by A.Vinichenko 14.04.2011
-- Информация об организации пользоватетеля берется 
-- из таблицы Organization2010
-- =============================================
ALTER procedure [dbo].[GetUserAccount]
	@login nvarchar(255)
as
begin
	declare @currentYear int, @accountId bigint--, @userGroupId int

	set @currentYear = Year(GetDate())

--	select @userGroupId = [group].Id
--	from dbo.[Group] [group] with (nolock, fastfirstrow)
--	where [group].Code = 'User'

	select @accountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	select account.[Login]
		, account.LastName
		, account.FirstName
		, account.PatronymicName
		, region.[Id] OrganizationRegionId
		, region.[Name] OrganizationRegionName
		, OReq.Id OrganizationId
		, OReq.FullName OrganizationName
		, OReq.OwnerDepartment OrganizationFounderName
		, OReq.LawAddress OrganizationAddress
		, OReq.DirectorFullName OrganizationChiefName
		, OReq.Fax OrganizationFax
		, OReq.Phone OrganizationPhone
		, OReq.EMail OrganizationEmail
		, OReq.Site OrganizationSite
		, OReq.ShortName OrganizationShortName
		, OReq.FactAddress OrganizationFactAddress
		, OReq.DirectorPosition OrganizationDirectorPosition
		, OReq.IsPrivate OrganizationIsPrivate
		, OReq.IsFilial OrganizationIsFilial
		, OReq.PhoneCityCode OrganizationPhoneCode
		, OReq.AccreditationSertificate AccreditationSertificate
		, OReq.INN OrganizationINN
		, OReq.OGRN OrganizationOGRN
		, account.Phone
		, account.Email
		, account.IpAddresses IpAddresses 
		, account.Status 
		, case
			when account.CanViewUserAccountRegistrationDocument = 1 
				then account.RegistrationDocument 
			else null
		end RegistrationDocument 
		, case
			when account.CanViewUserAccountRegistrationDocument = 1 
				then account.RegistrationDocumentContentType
			else null
		end RegistrationDocumentContentType
		, account.AdminComment AdminComment
		, dbo.CanEditUserAccount(account.Status, account.ConfirmYear, @currentYear) CanEdit
		, dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument 
		, account.HasFixedIp HasFixedIp
		, account.HasCrocEgeIntegration HasCrocEgeIntegration
		, OrgType.Id OrgTypeId
		, OrgType.[Name] OrgTypeName
		, OrgKind.Id OrgKindId
		, OrgKind.[Name] OrgKindName
		, OReq.Id OReqId
	from (select
				account.[Login] [Login]
				, account.LastName LastName
				, account.FirstName FirstName
				, account.PatronymicName PatronymicName
				, account.OrganizationId OrganizationId
				, account.Phone Phone
				, account.Email Email
				, account.ConfirmYear ConfirmYear
				, account.RegistrationDocument RegistrationDocument
				, account.RegistrationDocumentContentType RegistrationDocumentContentType
				, account.AdminComment AdminComment
				, account.IpAddresses IpAddresses
				, account.HasFixedIp HasFixedIp
				, account.HasCrocEgeIntegration HasCrocEgeIntegration
				, dbo.GetUserStatus(account.ConfirmYear, account.Status
						, @currentYear, account.RegistrationDocument) Status 
				, dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument
			from dbo.Account account with (nolock, fastfirstrow)
			where account.[Id] = @accountId
--					and account.Id in (
--						select group_account.AccountId
--						from dbo.GroupAccount group_account
--						where group_account.GroupId = @userGroupId)
			) account
			left outer join dbo.Organization2010 OReq with (nolock, fastfirstrow) 
			left outer join dbo.Region region with (nolock, fastfirstrow) on region.[Id] = OReq.RegionId
			left outer join dbo.OrganizationType2010 OrgType on OReq.TypeId = OrgType.Id
			left outer join dbo.OrganizationKind OrgKind on OReq.KindId = OrgKind.Id
				on OReq.[Id] = account.OrganizationId
	return 0
end
GO

--Внесение в базу группы пользователей ССУЗов
update [dbo].[Group]
set Name = 'Пользователи ВУЗ'
where Id = 1
GO

if not exists (select *
		   from [dbo].[Group] G
		   where G.Name = 'Пользователи ССУЗ')
begin
	set identity_insert [Group] on
	
	declare @idGroup  int 
	set @idGroup = 8
	
	insert into [dbo].[Group] (Id, Code, Name, GroupIdEsrp)
	values (@idGroup, 'User', 'Пользователи ССУЗ', 7)
	
	/*select
		'insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition) 
		 values (' + cast(RoleId as nvarchar(3)) + ', ' + cast(@idGroup as nvarchar(3)) 
		 + ', ' +
		 cast(IsActive as nvarchar(3)) + ', ' + 
		 case isnull(IsActiveCondition, '123')
			when '123'  then 'null'
			else IsActiveCondition
		end + ')'
	from GroupRole
	where GroupId = 1*/
	
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (19, 8, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (20, 8, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (21, 8, 1, 'isnull(account.HasCrocEgeIntegration, 0) = 1')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (23, 8, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (26, 8, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (28, 8, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (29, 8, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (40, 8, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
end
else begin
	update [dbo].[Group]
	set GroupIdEsrp = 7
	where Name = 'Пользователи ССУЗ'
end
GO

--Внесение в базу группы пользователей ОУО
if not exists (select *
		   from [dbo].[Group] G
		   where G.Name = 'Пользователи ОУО')
begin
	set identity_insert [Group] on
	
	declare @idGroup  int 
	set @idGroup = 9
	
	insert into [dbo].[Group] (Id, Code, Name, GroupIdEsrp)
	values (@idGroup, 'Auditor', 'Пользователи ОУО', 9)
	
	/*select
		'insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition) 
		 values (' + cast(RoleId as nvarchar(3)) + ', ' + cast(@idGroup as nvarchar(3)) 
		 + ', ' +
		 cast(IsActive as nvarchar(3)) + ', ' + 
		 case isnull(IsActiveCondition, '123')
			when '123'  then 'null'
			else IsActiveCondition
		end + ')'
	from GroupRole
	where GroupId = 4*/
	
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (18, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (19, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (2, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (20, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (21, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (22, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (23, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (24, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (25, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (26, 9, 0, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (27, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (28, 9, 0, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (32, 9, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (5, 9, 1, null)
end
else begin
	update [dbo].[Group]
	set GroupIdEsrp = 9
	where Name = 'Пользователи ОУО'
end
GO

--Внесение в базу группы пользователей Другое
if not exists (select *
		   from [dbo].[Group] G
		   where G.Name = 'Другое')
begin
	set identity_insert [Group] on
	
	declare @idGroup  int 
	set @idGroup = 10
	
	insert into [dbo].[Group] (Id, Code, Name, GroupIdEsrp)
	values (@idGroup, 'User', 'Другое', 11)
	
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (19, 10, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (20, 10, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (21, 10, 1, 'isnull(account.HasCrocEgeIntegration, 0) = 1')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (23, 10, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (26, 10, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (28, 10, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (29, 10, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (40, 10, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
end
else begin
	update [dbo].[Group]
	set GroupIdEsrp = 11
	where Name = 'Другое'
end
GO

--Внесение в базу группы пользователей Системы
if not exists (select *
		   from [dbo].[Group] G
		   where G.Name = 'Системы')
begin
	set identity_insert [Group] on
	
	declare @idGroup  int 
	set @idGroup = 11
	
	insert into [dbo].[Group] (Id, Code, Name, GroupIdEsrp)
	values (@idGroup, 'User', 'Системы', 12)
	
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (19, @idGroup, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (20, @idGroup, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (21, @idGroup, 1, 'isnull(account.HasCrocEgeIntegration, 0) = 1')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (23, @idGroup, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (26, @idGroup, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (28, @idGroup, 0, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (29, @idGroup, 1, null)
	insert into GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)      values (40, @idGroup, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '''+'activated'+'''')
end
else begin
	update [dbo].[Group]
	set GroupIdEsrp = 12
	where Name = 'Системы'
end
GO