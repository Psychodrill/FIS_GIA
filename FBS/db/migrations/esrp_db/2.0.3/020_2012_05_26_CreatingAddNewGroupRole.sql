-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (20, '020_2012_05_26_CreatingAddNewGroupRole')
-- =========================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddNewGroupRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddNewGroupRole]
GO
PRINT N'Creating [dbo].[AddNewGroupRole]...';
GO
create PROCEDURE [dbo].[AddNewGroupRole]
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
		if isnull(@password, '') <> '' 
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
		END
	
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