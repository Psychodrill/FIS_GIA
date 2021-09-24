-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (9, '009_2012_05_14_UpdateUserAccount.sql')
-- =========================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
@position NVARCHAR (255)=null,
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
@organizationRcModelId INT=null,
@orgRCDescription NVARCHAR(400)=NULL,
@ExistingOrgId INT=NULL,
@orgRequestID INT=null OUTPUT,
@accessToFbs BIT = 0,
@accessToFbd BIT = 0,
@ReceptionOnResultsCNE BIT = null
AS
BEGIN	
	-- при добавлении пользователя - проверка есть ли уже такой?
	if exists (SELECT * FROM Account a WHERE a.Email = @email AND @login = '')
	BEGIN
		RAISERROR('$Пользователь с указанным логином уже существует.', 18, 1)
		RETURN -1
	END
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit		
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)		
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam BIT
		, @userStatusBefore NVARCHAR(510)
		, @isRegistrationDocumentExistsForUser BIT

	set @updateId = newid()
	
	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id], 
	  @isRegistrationDocumentExistsForUser = case when account.RegistrationDocument IS NULL THEN 0 ELSE 1 END
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
		
		-- в качестве логина пользователя используем email
		SET @login = @email

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @userStatusBefore = account.[Status]
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
			-- берем последнюю поданную заявку			
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
		
	-- определяем идентификатор статуса
	DECLARE @statusID INT
	SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @status

	begin tran insert_update_account_tran
	
		IF(@orgRequestID IS NULL)
		BEGIN
			-- заявка подается не зависимо от того, новый аккаунт создается или обновляется старый
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
				OrganizationId,
				StatusID,
				RCModelID,
				RCDescription,
				ReceptionOnResultsCNE
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
				@ExistingOrgId,
				@statusID,
				@organizationRcModelId,
				@orgRCDescription,
				@ReceptionOnResultsCNE
				 
			if (@@error <> 0)
				goto undo

			select @orgRequestID = scope_identity()
			if (@@error <> 0)
				goto undo
		END
	
		if @newAccount = 1 -- внесение нового пользователя
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
				, Position
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
				, @orgRequestID
				, 1
				, @currentYear
				, @phone
				, @position
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
		end	
		else 
		begin -- update существующего пользователя			
			
			-- если пользователь получает доступ к ФБД и привязывается к организации, у которой уже есть активированный УС ОУ для ФБД, то выводим ошибку
			IF(@userStatusBefore = 'activated' AND @accessToFbd = 1 AND EXISTS(
				SELECT * FROM OrganizationRequest2010 or1 JOIN Account a ON or1.Id = a.OrganizationId
					JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id AND ora.AccountID = a.Id
					JOIN [GROUP] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @ExistingOrgId AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly') AND 
			      g.Code = 'fbd_^authorizedstaff'))
			BEGIN
				RAISERROR('$Сохранение не выполнено. У указанной организации уже есть активированный УС ОУ для ФБД.', 18, 1)
				goto undo
			END			
			
			if @isOrganizationOwner = 1
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
				OrganizationId=@ExistingOrgId,
				ReceptionOnResultsCNE=@ReceptionOnResultsCNE
				-- GVUZ-779 При изменении статуса пользователя статус заявления не меняется.
				--StatusID = @StatusID
			from 
				dbo.OrganizationRequest2010 OReq with (rowlock)
			where
				OReq.[Id] = @orgRequestID

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
				, Position = @position
				, Email = @email
				, ConfirmYear = @currentYear
				-- GVUZ-761 Статус УС ОУ, который имеет доступ только чтение, после оставления заявки на активацию - меняем на "Registration" 
				, [Status] = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
				, OrganizationId = @orgRequestID
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
			END
		end			
				
		-- установка группы пользователя.
		IF(@accessToFbd = 1)
		BEGIN
			-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
			DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId IN (15, 3)
			DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId IN (15, 3)
				
			-- fbd_^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	15, @accountId
			if (@@error <> 0) goto undo
			
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 15)
			if (@@error <> 0) goto undo
													
			-- esrp^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	3, @accountId
			if (@@error <> 0) goto undo
				
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 3)
			if (@@error <> 0) goto undo
											
		END	
		IF(@accessToFbs = 1)
		BEGIN
			-- ВУЗ
			IF(@organizationTypeId = 1)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 6
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 6
						
				-- fbs_^vuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	6, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 6)
				if (@@error <> 0) goto undo
			END
			-- ССУЗ
			ELSE IF(@organizationTypeId = 2)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 7
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 7
							
				-- fbs_^ssuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	7, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 7)
				if (@@error <> 0) goto undo						
			END
			-- РЦОИ
			ELSE IF(@organizationTypeId = 3)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 8
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 8
							
				-- fbs_^infoprocessing
				insert dbo.GroupAccount(GroupId, AccountID)
				select	8, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 8)
				if (@@error <> 0) goto undo						
			END
			-- Орган управления образованием
			ELSE IF(@organizationTypeId = 4)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 9
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 9
			
				-- fbs_^direction
				insert dbo.GroupAccount(GroupId, AccountID)
				select	9, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 9)
				if (@@error <> 0) goto undo						
			END
			-- Другое
			ELSE IF(@organizationTypeId = 5)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 11
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 11
							
				-- fbs_^other
				insert dbo.GroupAccount(GroupId, AccountID)				
				select	11, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 11)
				if (@@error <> 0) goto undo						
			END
			-- Учредитель
			ELSE IF(@organizationTypeId = 6)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 10
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 10
							
				-- fbs_^founder
				insert dbo.GroupAccount(GroupId, AccountID)
				select	10, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 10)
				if (@@error <> 0) goto undo						
			END				
		END	
								
	-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		BEGIN
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
	END
	
	-- GVUZ-761 оставляем в заявлении признак, если проводится активация пользователя
	-- флаг используется при выдаче шаблона документа для скана заявки	
	UPDATE OrganizationRequest2010
	SET IsForActivation = 1
	WHERE @accessToFbd = 1 AND @userStatusBefore = 'readonly' AND Id = @orgRequestID
	
	-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
	-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
	IF(@isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL)	
	BEGIN		
		IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
		BEGIN
			UPDATE OrganizationRequest2010 SET StatusID = 2 WHERE Id = @orgRequestID
		END
	END

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
go

/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromString]    Script Date: 05/24/2012 20:53:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_ut_SplitFromString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ufn_ut_SplitFromString]
GO

create function [dbo].[ufn_ut_SplitFromString] 
(
	@string nvarchar(max),
	@delimeter nvarchar(1) = ' '
)
returns @ret table (nam nvarchar(1000) )
as
begin
	if len(@string)=0 
		return 
	declare @s int, @e int
	set @s = 0
	while charindex(@delimeter,@string,@s) <> 0
	begin
		set @e = charindex(@delimeter,@string,@s)
		insert @ret values (rtrim(ltrim(substring(@string,@s,@e - @s))))
		set @s = @e + 1
	end
	insert @ret values (rtrim(ltrim(substring(@string,@s,300))))
	return
end
go

ALTER PROCEDURE [dbo].[UpdateUserAccountNew]
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
@position NVARCHAR (255)=null,
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
@organizationRcModelId INT=null,
@orgRCDescription NVARCHAR(400)=NULL,
@ExistingOrgId INT=NULL,
@orgRequestID INT=null OUTPUT,
@ListSystemId nvarchar(max),
@accessToFbd BIT = 0,
@ReceptionOnResultsCNE BIT = null
AS
BEGIN	
	-- при добавлении пользователя - проверка есть ли уже такой?
	if exists (SELECT * FROM Account a WHERE a.Email = @email AND @login = '')
	BEGIN
		RAISERROR('$Пользователь с указанным логином уже существует.', 18, 1)
		RETURN -1
	END
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit		
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)		
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam BIT
		, @userStatusBefore NVARCHAR(510)
		, @isRegistrationDocumentExistsForUser BIT

	set @updateId = newid()
	
	declare @tableSystemId table (SystemId int)
	insert @tableSystemId(SystemId)	
	select * from ufn_ut_SplitFromString(@ListSystemId,',') 	

	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id], 
	  @isRegistrationDocumentExistsForUser = case when account.RegistrationDocument IS NULL THEN 0 ELSE 1 END
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
		
		-- в качестве логина пользователя используем email
		SET @login = @email

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @userStatusBefore = account.[Status]
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
			-- берем последнюю поданную заявку			
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
		
	-- определяем идентификатор статуса
	DECLARE @statusID INT
	SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @status

	begin tran insert_update_account_tran
	
		IF(@orgRequestID IS NULL)
		BEGIN
			-- заявка подается не зависимо от того, новый аккаунт создается или обновляется старый
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
				OrganizationId,
				StatusID,
				RCModelID,
				RCDescription,
				ReceptionOnResultsCNE
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
				@ExistingOrgId,
				@statusID,
				@organizationRcModelId,
				@orgRCDescription,
				@ReceptionOnResultsCNE
				 
			if (@@error <> 0)
				goto undo

			select @orgRequestID = scope_identity()
			if (@@error <> 0)
				goto undo
		END
	
		if @newAccount = 1 -- внесение нового пользователя
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
				, Position
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
				, @orgRequestID
				, 1
				, @currentYear
				, @phone
				, @position
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
		end	
		else 
		begin -- update существующего пользователя			
			
			-- если пользователь получает доступ к ФБД и привязывается к организации, у которой уже есть активированный УС ОУ для ФБД, то выводим ошибку
			IF(@userStatusBefore = 'activated' AND @accessToFbd = 1 AND EXISTS(
				SELECT * FROM OrganizationRequest2010 or1 JOIN Account a ON or1.Id = a.OrganizationId
					JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id AND ora.AccountID = a.Id
					JOIN [GROUP] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @ExistingOrgId AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly') AND 
			      g.Code = 'fbd_^authorizedstaff'))
			BEGIN
				RAISERROR('$Сохранение не выполнено. У указанной организации уже есть активированный УС ОУ для ФБД.', 18, 1)
				goto undo
			END			
			
			if @isOrganizationOwner = 1
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
				OrganizationId=@ExistingOrgId,
				ReceptionOnResultsCNE=@ReceptionOnResultsCNE
				-- GVUZ-779 При изменении статуса пользователя статус заявления не меняется.
				--StatusID = @StatusID
			from 
				dbo.OrganizationRequest2010 OReq with (rowlock)
			where
				OReq.[Id] = @orgRequestID

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
				, Position = @position
				, Email = @email
				, ConfirmYear = @currentYear
				-- GVUZ-761 Статус УС ОУ, который имеет доступ только чтение, после оставления заявки на активацию - меняем на "Registration" 
				, [Status] = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
				, OrganizationId = @orgRequestID
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
			END
		end	
			
		-- установка группы пользователя.
		IF exists(select * from @tableSystemId where SystemId = 3)
		BEGIN
			-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
			DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId in (15,3)
			DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId in (15,3)
						
			-- fbd_^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	15, @accountId
			if (@@error <> 0) goto undo
			
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 15)
			if (@@error <> 0) goto undo
													
			-- esrp^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	3, @accountId
			if (@@error <> 0) goto undo
				
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 3)
			if (@@error <> 0) goto undo
			
			delete @tableSystemId where SystemId = 3								
		END	
		IF exists(select * from @tableSystemId where SystemId = 2)
		BEGIN		
			-- ВУЗ
			IF(@organizationTypeId = 1)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId =6		
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId =6	
					
				-- fbs_^vuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	6, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 6)
				if (@@error <> 0) goto undo
			END
			-- ССУЗ
			ELSE IF(@organizationTypeId = 2)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 7		
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 7	
							
				-- fbs_^ssuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	7, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 7)
				if (@@error <> 0) goto undo						
			END
			-- РЦОИ
			ELSE IF(@organizationTypeId = 3)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 8		
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 8	
							
				-- fbs_^infoprocessing
				insert dbo.GroupAccount(GroupId, AccountID)
				select	8, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 8)
				if (@@error <> 0) goto undo						
			END
			-- Орган управления образованием
			ELSE IF(@organizationTypeId = 4)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 9		
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 9	
							
				-- fbs_^direction
				insert dbo.GroupAccount(GroupId, AccountID)
				select	9, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 9)
				if (@@error <> 0) goto undo						
			END
			-- Другое
			ELSE IF(@organizationTypeId = 5)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 11		
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 11	
							
				-- fbs_^other
				insert dbo.GroupAccount(GroupId, AccountID)				
				select	11, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 11)
				if (@@error <> 0) goto undo						
			END
			-- Учредитель
			ELSE IF(@organizationTypeId = 6)
			BEGIN
				-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
				DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 10		
				DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId = 10	
							
				-- fbs_^founder
				insert dbo.GroupAccount(GroupId, AccountID)
				select	10, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 10)
				if (@@error <> 0) goto undo						
			END	
			
			delete @tableSystemId where SystemId = 2			
		END	
		
		IF exists(select * from @tableSystemId)
		begin
			-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
			DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId in (select b.id from @tableSystemId a join [Group] b on a.SystemId=b.SystemID and b.[Default]=1)
			DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId in (select b.id from @tableSystemId a join [Group] b on a.SystemId=b.SystemID and b.[Default]=1)
			
			insert dbo.GroupAccount(GroupId, AccountID)
			select	b.id, @accountId from @tableSystemId a join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			if (@@error <> 0) goto undo
					
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			select @orgRequestID, @accountId,b.id from @tableSystemId a join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			if (@@error <> 0) goto undo			  
		end
								
	-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		BEGIN
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
	END
	
	-- GVUZ-761 оставляем в заявлении признак, если проводится активация пользователя
	-- флаг используется при выдаче шаблона документа для скана заявки	
	UPDATE OrganizationRequest2010
	SET IsForActivation = 1
	WHERE @accessToFbd = 1 AND @userStatusBefore = 'readonly' AND Id = @orgRequestID
	
	-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
	-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
	IF(@isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL)	
	BEGIN		
		IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
		BEGIN
			UPDATE OrganizationRequest2010 SET StatusID = 2 WHERE Id = @orgRequestID
		END
	END

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
go