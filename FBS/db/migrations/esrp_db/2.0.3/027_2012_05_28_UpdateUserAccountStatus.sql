-- =========================================================================
-- Запись информации о текущей миграции в лог
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
-- Изменить статус пользователя.
-- v.1.0: Created by Makarev Andrey 08.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Рефакторинг: выделена функция, изменеа логика 
-- означивания полей.
-- v.1.2: Modified by Fomin Dmitriy 11.04.2008
-- Удаляется документ регистрации, если он устарел.
-- v.1.3: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.4: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.5: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- =============================================
CREATE proc [dbo].[UpdateUserAccountStatus]
	@login nvarchar(255)
	, @status nvarchar(255)
	, @adminComment ntext 
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @changeStatusByOrganizationRequest BIT = 0 -- 1 если статус пользователя меняется через заявку на регистрацию
												 -- в этом случае игнорируем часть проверок
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
		* старый функционал. в процедуру передается статус, который уже необходимо установить пользьвателю, дополнительных проверок не требуется.
		* , @status = case when @changeStatusByOrganizationRequest = 1 then @status else dbo.GetUserStatus(@currentYear, @status, @currentYear, 
				account.RegistrationDocument) END*/
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login	

	if(@changeStatusByOrganizationRequest = 0)
	BEGIN	
		-- если деактивируют проверить на допустимые статусы
		IF (@status = 'deactivated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('activated', 'readonly')))
		BEGIN
			RAISERROR('$Невозможно деактивировать пользователя в текущем статусе.', 18, 1)
			RETURN -1
		END	
		
		IF (@status = 'activated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('deactivated', 'readonly')))
		BEGIN	
			RAISERROR('$Невозможно активировать пользователя в текущем статусе.', 18, 1)
			RETURN -1
		END		
	END
	
	-- проверка на сущестование пользователя с указанным email
	EXEC @isValidEmail = CheckNewUserAccountEmail @email = @userEmail
	IF(@isValidEmail = 0)
	BEGIN
		RAISERROR('$Существуют пользователи с таким же e-mail.', 18, 1)
		RETURN -1
	END	
	
	-- при установке статуса "Активировать" проверка на наличие скана заявки
	IF(@status = 'activated' AND EXISTS(SELECT * FROM Account WHERE [Login] = @login AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$Пользователь не приложил скан заявки.', 18, 1)
		RETURN -1
	END	
	
	-- GVUZ-595. При работе с заблокированной учетной записью уполномоченного сотрудника (статус «Deactivated») исключить возможность ее активации, 
    -- если для этого же ОУ есть учетная запись уполномоченного сотрудника со статусом, отличным от значения «Deactivated». (редактирование)		            
    -- Правило работает только для УС ОУ ФБД GVUZ-780.
    IF (@status = 'activated' AND 
		-- заблокированный пользователь является УС ОУ ФБД для организации
		EXISTS(
			SELECT * 
			FROM Account a JOIN GroupAccount ga ON ga.AccountId = a.Id
				JOIN [Group] g ON g.Id = ga.GroupID
			WHERE a.[Login] = @login AND a.[Status] = 'deactivated' AND g.Code = 'fbd_^authorizedstaff'
		) AND
		-- есть незаблокированный УС ОУ ФБД для организации данного пользователя
		EXISTS(
    	SELECT or1.OrganizationId, orgUser.[Login]
		FROM OrganizationRequest2010 or1 
			JOIN OrganizationRequest2010 orReqUsr ON orReqUsr.OrganizationId = or1.OrganizationId
			JOIN account orgUser ON orgUser.OrganizationId = orReqUsr.Id
			JOIN GroupAccount ga ON ga.AccountId = orgUser.Id
			JOIN [Group] g ON ga.GroupID = g.Id
    	WHERE or1.Id = @orgRequestID AND orgUser.[Status] <> 'deactivated' AND g.Code = 'fbd_^authorizedstaff' AND orReqUsr.Id <> @orgRequestID))
    BEGIN
		RAISERROR('$Невозможно активировать заблокированного уполномоченного сотрудника для доступа к ФБД, т.к. для данного ОУ уже есть учетная запись незаблокированного уполномоченного сотрудника.', 18, 1)
		RETURN -1    	
    END
    
    -- GVUZ-624. Исключается возможность активации учетной записи пользователя, если заявление, с которым она связана, не активировано.
    IF (@status = 'activated' AND NOT exists(
    		SELECT *
			FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.Id
			WHERE a.Id = @accountId	AND or1.StatusID = 5 -- активировано
			))
    BEGIN
		RAISERROR('$Невозможно активировать пользователя, т.к. заявление для данного пользователя не активировано.', 18, 1)
		RETURN -1
    END

	BEGIN TRAN
	
		-- GVUZ-761 при активации пользователя УС ОУ в ФБД - старого блокируем, нового активируем.
		IF(@status = 'activated')
		BEGIN
			DECLARE @existOrgUserLogin nvarchar(255), @organizationID INT
			SELECT @organizationID = or1.OrganizationId 
			FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.id
			WHERE a.[Login] = @login
			
			-- активируемый пользователь должен входить в группу УС ОУ для ФБД
			IF EXISTS(
				SELECT *
				FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
					JOIN Account a ON a.Id = ora.AccountID
					JOIN [Group] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @organizationID AND a.Id = @accountId AND g.code IN ('fbd_^authorizedstaff'))
			BEGIN
				-- находим существующего активного УС ОУ для ФБД.
				SELECT @existOrgUserLogin = a.[login]
				FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
					JOIN Account a ON a.Id = ora.AccountID
					JOIN [Group] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @organizationID AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly')
					AND g.code IN ('fbd_^authorizedstaff')
				
				-- если нашли активного УС ОУ для ФБД, то блокируем его.
				IF(@existOrgUserLogin IS NOT NULL)
				BEGIN
					exec UpdateUserAccountStatus @login = @existOrgUserLogin, @status = 'deactivated', 
					@adminComment = 'заблокирован по причине активации нового УС ОУ для ФБД', 
					@editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1			
					if (@@error <> 0)
						goto undo
				END				
			END
		END			
	
		update account
		set 
			-- GVUZ-810 При отправке на доработку из статусов Registration и Revision статус не меняется.
			Status = CASE WHEN @status = 'revision' AND [Status] IN ('registration', 'revision') THEN [Status] ELSE @status END
			, AdminComment = @adminComment
			, IsActive = dbo.GetUserIsActive(@status)
			, UpdateDate = GetDate()
			, UpdateId = @updateId
			, ConfirmYear = @currentYear
			-- Удаляем документ регистрации, если он устарел.
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


