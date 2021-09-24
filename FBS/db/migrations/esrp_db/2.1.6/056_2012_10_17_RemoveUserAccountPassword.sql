insert into Migrations(MigrationVersion, MigrationName) values (56, '056_2012_10_17_RemoveUserAccountPassword')
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modified 29.01.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
alter PROCEDURE [dbo].[AddNewGroupRole]
@password NVARCHAR (255)=null, 
@organizationTypeId INT=null,
@orgRequestID INT=null,
@ListSystemId nvarchar(max),
@accountId int,
@error int=1 output
AS
set nocount on
begin try
	set @error=1
	begin tran	
		declare @tableSystemId table (SystemId int)
		insert @tableSystemId(SystemId)	
		select * from ufn_ut_SplitFromString(@ListSystemId,',') 	
	
		if exists(select * from @tableSystemId where SystemId = 0)
		begin
			delete OrganizationRequestAccount 
			where OrgRequestID = @orgRequestID AND AccountID = @accountId
			
			delete GroupAccount 
			where AccountId = @accountId
			
			delete @tableSystemId where SystemId = 0
		end
		-- установка группы пользователя.
		IF exists(select * from @tableSystemId where SystemId = 3)
		BEGIN								
		-- fbd_^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	15, @accountId 
			where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId =15)
			union all
			select	3, @accountId 
			where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId =3)			
	
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			select @orgRequestID, @accountId, 15 
			where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 15)
			union all
			select @orgRequestID, @accountId, 3 
			where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 3)			
		
			delete @tableSystemId where SystemId = 3								
		END	
		IF exists(select * from @tableSystemId where SystemId = 2)
		BEGIN		
			-- ВУЗ
			IF(@organizationTypeId = 1)
			BEGIN				
				-- fbs_^vuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	6, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 6)			
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 6 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 6)									
			END
			-- ССУЗ
			ELSE IF(@organizationTypeId = 2)
			BEGIN						
				-- fbs_^ssuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	7, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 7)

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 7 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 7)									

			END
			-- РЦОИ
			ELSE IF(@organizationTypeId = 3)
			BEGIN						
				-- fbs_^infoprocessing
				insert dbo.GroupAccount(GroupId, AccountID)
				select	8, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 8)		

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 8 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 8)									

			END
			-- Орган управления образованием
			ELSE IF(@organizationTypeId = 4)
			BEGIN
				-- fbs_^direction
				insert dbo.GroupAccount(GroupId, AccountID)
				select	9, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 9)	

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 9 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 9)									
			END
			-- Другое
			ELSE IF(@organizationTypeId = 5)
			BEGIN							
				-- fbs_^other
				insert dbo.GroupAccount(GroupId, AccountID)				
				select	11, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 11)

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 11 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 11)									
			END
			-- Учредитель
			ELSE IF(@organizationTypeId = 6)
			BEGIN		
				-- fbs_^founder
				insert dbo.GroupAccount(GroupId, AccountID)
				select	10, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 10)
				
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 10 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 10)									
			END	
		
			delete @tableSystemId where SystemId = 2			
		END	
	
		IF exists(select * from @tableSystemId)
		begin
			
			insert dbo.GroupAccount(GroupId, AccountID)
			select	b.id, @accountId 
			from @tableSystemId a 
				join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			where not exists(select * 
							 from GroupAccount 
							 WHERE AccountId = @accountId 
									AND GroupId =b.id)

			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			select @orgRequestID, @accountId,b.id 
			from @tableSystemId a
				join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			where not exists(select * 
							 from OrganizationRequestAccount 
							 WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId 
									AND GroupId =b.id)
		end
						
		-- временно
		/*if isnull(@password, '') <> '' 
		begin
			if exists(select * 
					from dbo.UserAccountPassword user_account_password
					where user_account_password.AccountId = @accountId)
			BEGIN
				update user_account_password
					set [Password] = @password
						from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId
			end	
			else		
				insert dbo.UserAccountPassword(AccountId, [Password])
				select @accountId, @password
		END*/
	
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
-- =============================================
-- Author:		Сулиманов А.М.
-- Create date: 2009-05-07
-- Description:	Удаление из БД всего, что касается AccountId (не анализируются связи)
-- =============================================
alter PROCEDURE [dbo].[DeleteAccount]
(@AccountID int)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM dbo.AccountIp WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountKey WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountLog WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountRoleActivity WHERE AccountId=@AccountID
	DELETE FROM dbo.GroupAccount WHERE AccountId=@AccountID
--	DELETE FROM dbo.Organization WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Id=@AccountID)
	/*DELETE FROM dbo.UserAccountPassword WHERE AccountId=@AccountID*/
	DELETE FROM dbo.Account WHERE Id=@AccountID
END
GO
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
alter proc [dbo].[UpdateAccountPassword]
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

/*
* -- GVUZ-785 закомментирован блок по причине ошибки. Ошибка происходит, в случае, если у пользователя несколько групп
* 
* 
* -- временно
	if isnull(@password, '') <> '' and N'User' = (select 
						[group].[code]
					from
						dbo.[Group] [group]
							inner join dbo.GroupAccount group_account
								on [group].[Id] = group_account.GroupId
					where
						group_account.AccountId = @accountId)
	begin*/
	/*	if exists(select 
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
		end*/
	/*end*/

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
GO
/*

*/
alter proc [dbo].[usp_repl_ApplyBatch]
@name nvarchar(250),@xml xml,@type int
as
set nocount on
if @name='Account'
begin
 exec [dbo].usp_repl_AccountApplyBatch @xml,@type
 return 
end
if @name='Organization2010'
begin
 exec [dbo].usp_repl_Organization2010ApplyBatch @xml,@type
 return 
end
if @name='OrganizationRequest2010'
begin
 exec [dbo].usp_repl_OrganizationRequest2010ApplyBatch @xml,@type
 return 
end
if @name='OrganizationRequestAccount'
begin
 exec [dbo].usp_repl_OrganizationRequestAccountApplyBatch @xml,@type
 return 
end
/*if @name='UserAccountPassword'
begin
 exec [dbo].usp_repl_UserAccountPasswordApplyBatch @xml,@type
 return 
end*/
if @name='Group'
begin
 exec [dbo].usp_repl_GroupApplyBatch @xml,@type
 return 
end
if @name='GroupAccount'
begin
 exec [dbo].usp_repl_GroupAccountApplyBatch @xml,@type
 return 
end
if @name='GroupRole'
begin
 exec [dbo].usp_repl_GroupRoleApplyBatch @xml,@type
 return 
end
GO
drop proc[dbo].[usp_repl_UserAccountPasswordApplyBatch]
GO
drop proc [dbo].[usp_repl_UserAccountPasswordGetBatch]
go
drop proc [dbo].[usp_repl_UserAccountPasswordReplDone]
go
drop TABLE [dbo].[UserAccountPassword_repl]
go
drop TABLE [dbo].[UserAccountPassword]
go
delete from repl_Tables where getbatch='dbo.usp_repl_UserAccountPasswordGetBatch'
