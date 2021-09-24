-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (27, '027_2012_05_28_UpdateUserAccountStatus')
-- =========================================================================
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountStatus]    Script Date: 05/28/2012 09:31:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccountStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserAccountStatus]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountStatus]    Script Date: 05/28/2012 09:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateUserAccountStatus

-- =============================================
-- �������� ������ ������������.
-- v.1.0: Created by Makarev Andrey 08.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- �����������: �������� �������, ������� ������ 
-- ����������� �����.
-- v.1.2: Modified by Fomin Dmitriy 11.04.2008
-- ��������� �������� �����������, ���� �� �������.
-- v.1.3: Modified by Makarev Andrey 14.04.2008
-- �������� ������������� ���������� UpdateId.
-- v.1.4: Modified by Makarev Andrey 14.04.2008
-- ���������� � ���������.
-- v.1.5: Modified by Makarev Andrey 18.04.2008
-- ��������� ���������� �� dbo.RegisterEvent.
-- =============================================
CREATE proc [dbo].[UpdateUserAccountStatus]
	@login nvarchar(255)
	, @status nvarchar(255)
	, @adminComment ntext 
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @changeStatusByOrganizationRequest BIT = 0 -- 1 ���� ������ ������������ �������� ����� ������ �� �����������
												 -- � ���� ������ ���������� ����� ��������
as
BEGIN
	declare
		@isActive bit
		, @eventCode nvarchar(255)
		, @accountId bigint
		, @editorAccountId bigint
		, @currentYear int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @isValidEmail BIT
		, @userEmail NVARCHAR(510)
		, @orgRequestID int
			

	set @updateId = newid()
	set @eventCode = N'USR_STATE'
	set @currentYear = Year(GetDate())
	
	select
		@editorAccountId = account.[Id],
		@userEmail = account.Email
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	SELECT
		@accountId = account.[Id],
		@orgRequestID = account.OrganizationId		
		/*
		* ������ ����������. � ��������� ���������� ������, ������� ��� ���������� ���������� ������������, �������������� �������� �� ���������.
		* , @status = case when @changeStatusByOrganizationRequest = 1 then @status else dbo.GetUserStatus(@currentYear, @status, @currentYear, 
				account.RegistrationDocument) END*/
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login	

	if(@changeStatusByOrganizationRequest = 0)
	BEGIN	
		-- ���� ������������ ��������� �� ���������� �������
		IF (@status = 'deactivated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('activated', 'readonly')))
		BEGIN
			RAISERROR('$���������� �������������� ������������ � ������� �������.', 18, 1)
			RETURN -1
		END	
		
		IF (@status = 'activated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('deactivated', 'readonly')))
		BEGIN	
			RAISERROR('$���������� ������������ ������������ � ������� �������.', 18, 1)
			RETURN -1
		END		
	END
	
	-- �������� �� ������������ ������������ � ��������� email
	EXEC @isValidEmail = CheckNewUserAccountEmail @email = @userEmail
	IF(@isValidEmail = 0)
	BEGIN
		RAISERROR('$���������� ������������ � ����� �� e-mail.', 18, 1)
		RETURN -1
	END	
	
	-- ��� ��������� ������� "������������" �������� �� ������� ����� ������
	IF(@status = 'activated' AND EXISTS(SELECT * FROM Account WHERE [Login] = @login AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$������������ �� �������� ���� ������.', 18, 1)
		RETURN -1
	END	
	
	-- GVUZ-595. ��� ������ � ��������������� ������� ������� ��������������� ���������� (������ �Deactivated�) ��������� ����������� �� ���������, 
    -- ���� ��� ����� �� �� ���� ������� ������ ��������������� ���������� �� ��������, �������� �� �������� �Deactivated�. (��������������)		            
    -- ������� �������� ������ ��� �� �� ��� GVUZ-780.
    IF (@status = 'activated' AND 
		-- ��������������� ������������ �������� �� �� ��� ��� �����������
		EXISTS(
			SELECT * 
			FROM Account a JOIN GroupAccount ga ON ga.AccountId = a.Id
				JOIN [Group] g ON g.Id = ga.GroupID
			WHERE a.[Login] = @login AND a.[Status] = 'deactivated' AND g.Code = 'fbd_^authorizedstaff'
		) AND
		-- ���� ����������������� �� �� ��� ��� ����������� ������� ������������
		EXISTS(
    	SELECT or1.OrganizationId, orgUser.[Login]
		FROM OrganizationRequest2010 or1 
			JOIN OrganizationRequest2010 orReqUsr ON orReqUsr.OrganizationId = or1.OrganizationId
			JOIN account orgUser ON orgUser.OrganizationId = orReqUsr.Id
			JOIN GroupAccount ga ON ga.AccountId = orgUser.Id
			JOIN [Group] g ON ga.GroupID = g.Id
    	WHERE or1.Id = @orgRequestID AND orgUser.[Status] <> 'deactivated' AND g.Code = 'fbd_^authorizedstaff' AND orReqUsr.Id <> @orgRequestID))
    BEGIN
		RAISERROR('$���������� ������������ ���������������� ��������������� ���������� ��� ������� � ���, �.�. ��� ������� �� ��� ���� ������� ������ ������������������ ��������������� ����������.', 18, 1)
		RETURN -1    	
    END
    
    -- GVUZ-624. ����������� ����������� ��������� ������� ������ ������������, ���� ���������, � ������� ��� �������, �� ������������.
    IF (@status = 'activated' AND NOT exists(
    		SELECT *
			FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.Id
			WHERE a.Id = @accountId	AND or1.StatusID = 5 -- ������������
			))
    BEGIN
		RAISERROR('$���������� ������������ ������������, �.�. ��������� ��� ������� ������������ �� ������������.', 18, 1)
		RETURN -1
    END

	BEGIN TRAN
	
		-- GVUZ-761 ��� ��������� ������������ �� �� � ��� - ������� ���������, ������ ����������.
		IF(@status = 'activated')
		BEGIN
			DECLARE @existOrgUserLogin nvarchar(255), @organizationID INT
			SELECT @organizationID = or1.OrganizationId 
			FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.id
			WHERE a.[Login] = @login
			
			-- ������������ ������������ ������ ������� � ������ �� �� ��� ���
			IF EXISTS(
				SELECT *
				FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
					JOIN Account a ON a.Id = ora.AccountID
					JOIN [Group] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @organizationID AND a.Id = @accountId AND g.code IN ('fbd_^authorizedstaff'))
			BEGIN
				-- ������� ������������� ��������� �� �� ��� ���.
				SELECT @existOrgUserLogin = a.[login]
				FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
					JOIN Account a ON a.Id = ora.AccountID
					JOIN [Group] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @organizationID AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly')
					AND g.code IN ('fbd_^authorizedstaff')
				
				-- ���� ����� ��������� �� �� ��� ���, �� ��������� ���.
				IF(@existOrgUserLogin IS NOT NULL)
				BEGIN
					exec UpdateUserAccountStatus @login = @existOrgUserLogin, @status = 'deactivated', 
					@adminComment = '������������ �� ������� ��������� ������ �� �� ��� ���', 
					@editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1			
					if (@@error <> 0)
						goto undo
				END				
			END
		END			
	
		update account
		set 
			-- GVUZ-810 ��� �������� �� ��������� �� �������� Registration � Revision ������ �� ��������.
			Status = CASE WHEN @status = 'revision' AND [Status] IN ('registration', 'revision') THEN [Status] ELSE @status END
			, AdminComment = @adminComment
			, IsActive = dbo.GetUserIsActive(@status)
			, UpdateDate = GetDate()
			, UpdateId = @updateId
			, ConfirmYear = @currentYear
			-- ������� �������� �����������, ���� �� �������.
			, RegistrationDocument = case
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 then null
				else account.RegistrationDocument
			end
			, EditorAccountId = @editorAccountId
			, EditorIp = @editorIp
		from 
			dbo.Account account with (rowlock)
		where
			account.[Id] = @accountId
		if (@@error <> 0)
			goto undo
								
	if @@trancount > 0 COMMIT TRAN

	exec dbo.RefreshRoleActivity @accountId = @accountId

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
		if @@trancount > 0 rollback tran
		return 1	
end
GO


