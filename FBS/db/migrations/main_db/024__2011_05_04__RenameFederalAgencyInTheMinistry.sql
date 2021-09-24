-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (24, '024__2011_05_04__RenameFederalAgencyInTheMinistry')
-- =========================================================================



-- Переименование "Федерального агентства по образованию" в "Министерство образования и науки РФ"
update Organization2010
set FullName = 'Министерство образования и науки РФ',
ShortName = 'Министерство образования и науки РФ'
where FullName like '%агентство по образованию%'
GO


insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (20, 6, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (25, 6, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
insert into [dbo].[GroupRole] (RoleId, GroupId, IsActive, IsActiveCondition) values (32, 6, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')
GO

-- =============================================
-- Modified 04.05.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
ALTER PROCEDURE [dbo].[UpdateUserAccount]
@login NVARCHAR (255)=null OUTPUT, 
@passwordHash NVARCHAR (255)=null, 
@lastName NVARCHAR (255)=null, 
@firstName NVARCHAR (255)=null, 
@patronymicName NVARCHAR (255)=null, 
@organizationRegionId INT=null, 
@organizationFullName NVARCHAR (2000)=null,
@organizationShortName NVARCHAR (2000)=null, 
@organizationINN NVARCHAR (10)=null, 
@organizationOGRN NVARCHAR (13)=null, 
@organizationFounderName NVARCHAR (2000)=null, 
@organizationFactAddress NVARCHAR (500)=null,
@organizationLawAddress NVARCHAR (500)=null, 
@organizationDirName NVARCHAR (255)=null,
@organizationDirPosition NVARCHAR (500)=null, 
@organizationPhoneCode NVARCHAR (255)=null,
@organizationFax NVARCHAR (255)=null,
@organizationIsAccred bit=0,
@organizationIsPrivate bit=0,
@organizationIsFilial bit=0,
@organizationAccredSert nvarchar(255)=null,
@organizationEMail nvarchar(255)=null,
@organizationSite nvarchar(255)=null, 
@organizationPhone NVARCHAR (255)=null, 
@phone NVARCHAR (255)=null, 
@email NVARCHAR (255)=null, 
@ipAddresses NVARCHAR (4000)=null, 
@status NVARCHAR (255)=null OUTPUT, 
@registrationDocument IMAGE=null, 
@registrationDocumentContentType NVARCHAR (225)=null, 
@editorLogin NVARCHAR (255)=null, 
@editorIp NVARCHAR (255)=null, 
@password NVARCHAR (255)=null, 
@hasFixedIp BIT=null, 
@hasCrocEgeIntegration BIT=null, 
@organizationTypeId INT=null,
@organizationKindId INT=null, 
@ExistingOrgId INT=null
AS
begin
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit
		, @organizationId bigint
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)
		, @userGroupId int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam bit

	set @updateId = newid()
	
	declare @groupCode nvarchar(255)
	set @groupCode = 
		case @organizationTypeId 
			 when 6 then N'UserDepartment'
			 when 4 then N'Auditor'
			 when 3 then N'UserRCOI'
			 else N'User'
		end
	
	select	top 1 @userGroupId = [group].[Id]
	from dbo.[Group] [group] with (nolock, fastfirstrow)
	where [group].[code] = @groupCode
	
	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @editorLogin

	
	if isnull(@login, '') = ''
	begin 
		set @useOnlyDocumentParam = 1
		set @eventCode = N'USR_REG'
	end
	else
	begin
		set @useOnlyDocumentParam = 0
		set @eventCode = N'USR_EDIT'
	end

	if isnull(@login, '') = ''
		select top 1 @login = account.login
		from dbo.Account account with (nolock)
		where account.email = @email
			and dbo.GetUserStatus(@currentYear, 
				account.Status, account.ConfirmYear, account.RegistrationDocument) = 'registration'
		order by account.UpdateDate desc

	if isnull(@login, '') = '' -- внесение нового пользователя
	begin
		set @newAccount = 1

		exec dbo.GetNewUserLogin @login = @login output

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @status = dbo.GetUserStatus(@currentYear, isnull(@status, account.Status), @currentYear
				, @registrationDocument)
			, @registrationDocument = isnull(@registrationDocument, case
				-- Если документ нельзя просмотривать, то считаем, что его нет.
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
					or @useOnlyDocumentParam = 1 
					or isnull(datalength(account.RegistrationDocument),0)=0 
					then null
				else account.RegistrationDocument
			end)
			, @registrationDocumentContentType = case
				when not @registrationDocument is null then @registrationDocumentContentType
				-- Если документ нельзя просмотривать, то считаем, что его нет.
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
					or @useOnlyDocumentParam = 1 				
					then null
				else account.RegistrationDocumentContentType
			end
			, @isOrganizationOwner = account.IsOrganizationOwner
			, @organizationId = account.OrganizationID
			, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
			, @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, account.HasCrocEgeIntegration)
		from dbo.Account account with (nolock, fastfirstrow)
		where account.[Login] = @login


		if @accountId is null
			return 0

		
		insert @oldIpAddress(ip)
		select account_ip.ip
		from dbo.AccountIp account_ip with (nolock, fastfirstrow)
		where account_ip.AccountId = @accountId
	end

	if @hasFixedIp = 1
		insert @newIpAddress(ip)
		select ip_addresses.[value]
		from dbo.GetDelimitedValues(@ipAddresses) ip_addresses

	begin tran insert_update_account_tran

		
		if @newAccount = 1 -- внесение нового пользователя
		begin
--			if isnull(@organizationTypeId, 0) = 0
--				set @educationInstitutionTypeId = dbo.GetOrganizationEducationInstitutionTypeIdByName(@organizationFullName) 

--			insert dbo.Organization
--				(
--				CreateDate
--				, UpdateDate
--				, UpdateId
--				, EditorAccountId
--				, EditorIp
--				, RegionId
--				, DepartmentOwnershipCode
--				, [Name]
--				, FounderName
--				, Address
--				, ChiefName
--				, Fax
--				, Phone
--				, ShortName
--				, EducationInstitutionTypeId
--				, EtalonOrgId
--				)
--			select
--				getdate()
--				, getdate()
--				, @updateId
--				, @editorAccountId
--				, @editorIp
--				, @organizationRegionId
--				, @departmentOwnershipCode
--				, @organizationFullName
--				, @organizationFounderName
--				, @organizationLawAddress
--				, @organizationDirName
--				, @organizationFax
--				, @organizationPhone
--				, dbo.GetShortOrganizationName(@organizationFullName)
--				, @organizationTypeId
--				, @ExistingOrgId


			insert dbo.OrganizationRequest2010
				(
				FullName,
				ShortName,
				RegionId,
				TypeId,
				KindId,
				INN,
				OGRN,
				OwnerDepartment,
				IsPrivate,
				IsFilial,
				DirectorPosition,
				DirectorFullName,
				IsAccredited,
				AccreditationSertificate,
				LawAddress,
				FactAddress,
				PhoneCityCode,
				Phone,
				Fax,
				EMail,
				Site,
				OrganizationId
				)
			select
				@organizationFullName,
				@organizationShortName,
				@organizationRegionId,
				@organizationTypeId,
				@organizationKindId,
				@organizationINN,
				@organizationOGRN,		
				@organizationFounderName,
				@organizationIsPrivate,
				@organizationIsFilial,
				@organizationDirPosition,
				@organizationDirName,
				@organizationIsAccred,
				@organizationAccredSert,
				@organizationLawAddress,
				@organizationFactAddress,
				@organizationPhoneCode,
				@organizationPhone,
				@organizationFax,
				@organizationEMail,
				@organizationSite,	
				@ExistingOrgId
				 
			if (@@error <> 0)
				goto undo

			select @organizationId = scope_identity()

			if (@@error <> 0)
				goto undo

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
				, RegistrationDocumentContentType
				, AdminComment
				, IsActive
				, Status
				, IpAddresses
				, HasFixedIp
				, HasCrocEgeIntegration
				)
			select
				GetDate()
				, GetDate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @login
				, @passwordHash
				, @lastName
				, @firstName
				, @patronymicName
				, @organizationId
				, 1
				, @currentYear
				, @phone
				, @email
				, @registrationDocument
				, @registrationDocumentContentType
				, null
				, 1
				, @status
				, @ipAddresses
				, @hasFixedIp
				, @hasCrocEgeIntegration

			if (@@error <> 0)
				goto undo

			select @accountId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.AccountIp(AccountId, Ip)
			select	@accountId, new_ip_address.ip
			from @newIpAddress new_ip_address

			if (@@error <> 0)
				goto undo

			insert dbo.GroupAccount(GroupId, AccountID)
			select	@UserGroupId, @accountId

			if (@@error <> 0)
				goto undo
		end	
		else 
		begin -- update существующего пользователя
			if @isOrganizationOwner = 1
--				update organization
--				set 
--					UpdateDate = GetDate()
--					, UpdateId = @updateId
--					, EditorAccountId = @editorAccountId
--					, EditorIp = @editorIp
--					, RegionId = @organizationRegionId
--					, DepartmentOwnershipCode = @departmentOwnershipCode
--					, [Name] = @organizationFullName
--					, FounderName = @organizationFounderName
--					, Address = @organizationLawAddress
--					, ChiefName = @organizationDirName
--					, Fax = @organizationFax
--					, Phone = @organizationPhone
--					, ShortName = dbo.GetShortOrganizationName(@organizationFullName)
--					, EducationInstitutionTypeId = @organizationTypeId
--					, EtalonOrgId=@ExistingOrgId
--				from 
--					dbo.Organization organization with (rowlock)
--				where
--					organization.[Id] = @organizationId

				update OReq
				set 
					UpdateDate = GetDate(),
					FullName=@organizationFullName,
					ShortName=@organizationShortName,
					RegionId=@organizationRegionId,
					TypeId=@organizationTypeId,
					KindId=@organizationKindId,
					INN=@organizationINN,
					OGRN=@organizationOGRN,		
					OwnerDepartment=@organizationFounderName,
					IsPrivate=@organizationIsPrivate,
					IsFilial=@organizationIsFilial,
					DirectorPosition=@organizationDirPosition,
					DirectorFullName=@organizationDirName,
					IsAccredited=@organizationIsAccred,
					AccreditationSertificate=@organizationAccredSert,
					LawAddress=@organizationLawAddress,
					FactAddress=@organizationFactAddress,
					PhoneCityCode=@organizationPhoneCode,
					Phone=@organizationPhone,
					Fax=@organizationFax,
					EMail=@organizationEMail,
					Site=@organizationSite,	
					OrganizationId=@ExistingOrgId
				from 
					dbo.OrganizationRequest2010 OReq with (rowlock)
				where
					OReq.[Id] = @organizationId

			if (@@error <> 0)
				goto undo

			update account
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, PasswordHash=isnull(@passwordHash,PasswordHash)
				, EditorIp = @editorIp
				, LastName = @lastName
				, FirstName = @firstName
				, PatronymicName = @patronymicName
				, Phone = @phone
				, Email = @email
				, ConfirmYear = @currentYear
				, Status = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
			from dbo.Account account with (rowlock)
			where account.[Id] = @accountId

			if (@@error <> 0)
				goto undo

			if exists(	select 1 
						from @oldIpAddress old_ip_address
						full outer join @newIpAddress new_ip_address
						on old_ip_address.ip = new_ip_address.ip
						where old_ip_address.ip is null
							or new_ip_address.ip is null) 
			begin
				delete account_ip
				from dbo.AccountIp account_ip
				where account_ip.AccountId = @accountId

				if (@@error <> 0)
					goto undo

				insert dbo.AccountIp(AccountId, Ip)
				select @accountId, new_ip_address.ip
				from @newIpAddress new_ip_address

				if (@@error <> 0)
					goto undo
			end
		end	

-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		begin
			update user_account_password
			set [Password] = @password
			from dbo.UserAccountPassword user_account_password
			where user_account_password.AccountId = @accountId

			if (@@error <> 0)
				goto undo
		end
		else
		begin
			insert dbo.UserAccountPassword(AccountId, [Password])
			select @accountId, @password

			if (@@error <> 0)
				goto undo
		end
	end

	if @@trancount > 0
		commit tran insert_update_account_tran

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RefreshRoleActivity @accountId = @accountId

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