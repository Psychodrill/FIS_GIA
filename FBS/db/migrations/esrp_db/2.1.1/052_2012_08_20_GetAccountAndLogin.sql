-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (52, '052_2012_08_20_GetAccountAndLogin.sql')
go

GO
/****** Object:  StoredProcedure [dbo].[GetAccountAndLogin]    Script Date: 08/20/2012 10:44:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modified 29.01.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
ALTER PROCEDURE [dbo].[GetAccountAndLogin]
@login NVARCHAR (255)=null, 
@passwordHash NVARCHAR (255)=null, 
@lastName NVARCHAR (255)=null, 
@firstName NVARCHAR (255)=null, 
@patronymicName NVARCHAR (255)=null, 
@phone NVARCHAR (255)=null, 
@email NVARCHAR (255)=null, 
@position NVARCHAR (255)=null,
@ipAddresses NVARCHAR (4000)=null, 
@status NVARCHAR (255)=null, 
@registrationDocument IMAGE=null, 
@registrationDocumentContentType NVARCHAR (225)=null, 
@editorLogin NVARCHAR (255)=null, 
@editorIp NVARCHAR (255)=null, 
@password NVARCHAR (255)=null, 
@hasFixedIp BIT=null, 
@orgRequestID INT=null,
@error int=1 output
as
set nocount on
begin try
	set @error=1
	begin tran	
		if not exists(select  * from OrganizationRequest2010 where id=@orgRequestID)
			raiserror('Такая заявка не существует',16,1)
		
		if @status=''
			set @status=null
		if @login=''
			set @login=null
			
		declare @accountId bigint, @currentYear int, @editorAccountId bigint, @eventCode nvarchar(100), @updateId	uniqueidentifier, @useOnlyDocumentParam BIT
			, @userStatusBefore NVARCHAR(510), @isRegistrationDocumentExistsForUser BIT, @loginNew nvarchar(255)			
				
		declare @newIpAddress table (ip nvarchar(255))			
		declare @oldIpAddress table (ip nvarchar(255))				
					
		select @updateId = newid(), @currentYear = year(getdate())		
		
		select @editorAccountId = account.[Id], 
			   @isRegistrationDocumentExistsForUser = case when account.RegistrationDocument IS NULL then 0 else 1 end
		from dbo.Account account with (nolock, fastfirstrow)
		where account.[Login] = @editorLogin

		if @login is null
		begin 
			select @useOnlyDocumentParam = 1, @eventCode = N'USR_REG'

			select top 1 @loginNew = account.login	 
			from dbo.Account account with (nolock)
			where account.email = @email
			order by account.UpdateDate desc		
				
		end
		else
			select @useOnlyDocumentParam = 0, @eventCode = N'USR_EDIT', @loginNew = @login

		if @loginNew is null -- внесение нового пользователя
		begin			
			-- в качестве логина пользователя используем email
			select @loginNew = @email, @hasFixedIp = isnull(@hasFixedIp, 1)
			
			insert dbo.Account
				(
					CreateDate, UpdateDate, UpdateId, EditorAccountId, EditorIp, [Login], PasswordHash, LastName, FirstName, PatronymicName, OrganizationId,
					IsOrganizationOwner, ConfirmYear, Phone, Position, Email, RegistrationDocument, RegistrationDocumentContentType, AdminComment, IsActive,
					Status, IpAddresses, HasFixedIp
				)
			select GetDate(), GetDate(), @updateId, @editorAccountId, @editorIp, @loginNew, @passwordHash, @lastName, @firstName, @patronymicName, @orgRequestID,
				   1, @currentYear, @phone, @position, @email, @registrationDocument, @registrationDocumentContentType, null, 1, @status, @ipAddresses, 
				   @hasFixedIp

			select @accountId = scope_identity()
			
			if @hasFixedIp = 1
				insert @newIpAddress(ip)
				select ip_addresses.[value]
				from dbo.GetDelimitedValues(@ipAddresses) ip_addresses


			insert dbo.AccountIp(AccountId, Ip)
			select	@accountId, new_ip_address.ip
			from @newIpAddress new_ip_address			
		end
		else
		begin -- update существующего пользователя						
			declare @OrganizationId nvarchar(255)
			select @accountId = account.[Id],
				   @userStatusBefore = account.[Status],
				   @registrationDocument = isnull(@registrationDocument, 
					-- Если документ нельзя просмотривать, то считаем, что его нет.			   
				   case when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
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
				-- берем последнюю поданную заявку	
				, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
				, @OrganizationId=OrganizationId
			from dbo.Account account with (nolock, fastfirstrow)		
			where account.[Login] = @loginNew			
						
			declare @st nvarchar(255)		
			set @st = (select dbo.GetUserStatus(@currentYear, isnull(@status,account.[Status]), @currentYear, @registrationDocument) from dbo.Account account with (nolock, fastfirstrow) where account.[Login] = @loginNew)						

			if exists(select * from Account where [status] in ('registration','consideration','revision') and [login]=@loginNew and OrganizationId<>@orgRequestID)
			begin
				declare @dat nvarchar(255),@namest nvarchar(255)
				select @dat=convert(nvarchar(255),UpdateDate,104)+' '+convert(nvarchar(255),UpdateDate,108)  from Account where Id = @accountId	
				
				SELECT @namest=name FROM AccountStatus WHERE Code = @st
				
				raiserror(N'001Пользователь %s находится в статусе ''%s'' в заявке от %s.001', 16, 1, @email,@namest,@dat)						
			end
			
			update account set UpdateDate = GetDate(), UpdateId = @updateId, EditorAccountId = @editorAccountId, PasswordHash=isnull(@passwordHash,PasswordHash), 
							   EditorIp = @editorIp, LastName = @lastName, FirstName = @firstName, PatronymicName = @patronymicName, Phone = @phone,
							   Email = @email, ConfirmYear = @currentYear, [Status] = @st, IpAddresses = @ipAddresses, RegistrationDocument = @registrationDocument,
							   RegistrationDocumentContentType = @registrationDocumentContentType, HasFixedIp = @hasFixedIp, OrganizationId=@orgRequestID,position=@position
			from dbo.Account account with (rowlock)
			where account.[Id] = @accountId			
				
			if @hasFixedIp = 1
				insert @newIpAddress(ip)
				select ip_addresses.[value]
				from dbo.GetDelimitedValues(@ipAddresses) ip_addresses
			
			insert @oldIpAddress(ip)
			select account_ip.ip
			from dbo.AccountIp account_ip with (nolock, fastfirstrow)
			where account_ip.AccountId = @accountId
									
			if exists(select * from @oldIpAddress old_ip_address full join @newIpAddress new_ip_address on old_ip_address.ip = new_ip_address.ip
							where old_ip_address.ip is null	or new_ip_address.ip is null) 
			begin
				delete account_ip
				from dbo.AccountIp account_ip
				where AccountId = @accountId
			
				insert dbo.AccountIp(AccountId, Ip)
				select @accountId, ip
				from @newIpAddress
			end								
		end
		
		-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
		-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
		IF @isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL	
			and NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
				UPDATE OrganizationRequest2010 SET StatusID = case when StatusID<2 then 2 else StatusID end WHERE Id = @orgRequestID
	
		-- если все пользователи имеют сканы документов и статус на согласовании то меняет статус заявки на согласование
		IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
			AND NOT exists(select * from dbo.Account account with (nolock, fastfirstrow) where OrganizationId = @orgRequestID and account.[Status] ='registration')
			UPDATE OrganizationRequest2010 SET StatusID = case when StatusID<2 then 2 else StatusID end WHERE Id = @orgRequestID
			
		exec dbo.RefreshRoleActivity @accountId = @accountId

		exec dbo.RegisterEvent 
			@accountId = @editorAccountId
			, @ip = @editorIp
			, @eventCode = @eventCode
			, @sourceEntityIds = @accountId
			, @eventParams = null
			, @updateId = @updateId

		select @loginNew [login], @accountId [accountId]
	if @@trancount > 0 
		commit tran 

end try
begin catch
	set @error=-1
	if @@trancount > 0
		rollback tran 
	declare @er nvarchar(4000)
	set @er=error_message()
	raiserror(@er,16,1) 
	return -1
end catch
GO