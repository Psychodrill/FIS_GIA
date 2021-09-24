-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (23, '023_2012_05_26_AlteringUpdateOrganizationRequestStatus')
-- =========================================================================
PRINT N'Altering [dbo].[UpdateOrganizationRequestStatus]...';
GO
ALTER PROCEDURE [dbo].[UpdateOrganizationRequestStatus]	
	  @orgRequestID INT
	, @statusID INT
	, @needConsiderLinkedUsers BIT = 1 -- если 1, то меняем статусы прикрепленных пользователей к заявке
	, @comment VARCHAR(MAX)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
AS
BEGIN	
	DECLARE
		@curRequestStatus INT,
		@statusCode NVARCHAR(1020),
		@editorID INT
	
	SELECT @editorID = id FROM Account WHERE [login] = @editorLogin 
	SELECT @statusCode = Code FROM AccountStatus WHERE StatusID = @statusID		
	SELECT @curRequestStatus = or1.StatusID
	FROM OrganizationRequest2010 or1
	WHERE or1.Id = @statusID	
		
	-- если активируют проверить на допустимые статусы
	IF (@statusID = 5 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (2,3,6)))
	BEGIN
		RAISERROR('$Невозможно активировать заявку в текущем статусе.', 18, 1)
		RETURN -1
	END		
			
	-- GVUZ-595 Исключить подтверждение заявки без приложенных документов.
	IF (@statusID = 5 AND EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$Невозможно активировать заявку, т.к. не приложены сканы документов для регистрируемых пользователей.', 18, 1)
		RETURN -1
	END			

	-- если деактивируют проверить на допустимые статусы
	IF (@statusID = 6 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5)))
	BEGIN
		RAISERROR('$Невозможно деактивировать заявку в текущем статусе.', 18, 1)
		RETURN -1
	END		

	-- если отправляют на доработку проверить на допустимые статусы
	IF (@statusID = 3 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5,6)))
	BEGIN
		RAISERROR('$Невозможно отправить на доработку заявку в текущем статусе.', 18, 1)
		RETURN -1
	END
	
	BEGIN TRAN	
		-- смена статуса заявления
		-- GVUZ-810 При отправке на доработку ('revision') из статусов Registration и Revision статус не меняется.
		UPDATE OrganizationRequest2010 
		SET StatusID = CASE WHEN @statusCode = 'revision' AND StatusID IN (1, 3) THEN StatusID ELSE @statusID END		
		WHERE id = @orgRequestID
		
		DECLARE 
			@suggestedRCModelID INT,
			@suggestedRCDescription NVARCHAR(400),
			@orgID INT
		
		IF (@statusID = 5)
		BEGIN
			SELECT @orgID = OrganizationId, @suggestedRCModelID = RCModelID, @suggestedRCDescription = RCDescription			
			FROM dbo.OrganizationRequest2010
			WHERE Id = @orgRequestID
			
			UPDATE dbo.Organization2010
			SET
				RCModel = CASE WHEN @suggestedRCModelID IS NULL THEN 999 ELSE @suggestedRCModelID END,
				RCDescription = @suggestedRCDescription
			WHERE Id = @orgID
		END
		
		if (@@error <> 0) goto undo
				
		-- если учитываем пользователей в заявке, то идем по всем пользователям заявления и меняем им статус на новый статус для заявления
		if(@needConsiderLinkedUsers = 1)
		BEGIN
			DECLARE @curUserLogin NVARCHAR(510)			
			DECLARE linkedUsers CURSOR FOR
			  SELECT a.[login] FROM Account a WHERE a.OrganizationId = @orgRequestID
			
			OPEN linkedUsers
			FETCH NEXT FROM linkedUsers INTO @curUserLogin
			
			WHILE(@@FETCH_STATUS = 0)
			BEGIN				
				EXEC [UpdateUserAccountStatus] @login = @curUserLogin, @status = @statusCode, @adminComment = @comment,
				  @editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1
				if (@@error <> 0) goto undo
				FETCH NEXT FROM linkedUsers INTO @curUserLogin
			END			
			
			CLOSE linkedUsers
			DEALLOCATE linkedUsers
		END
		
	if @@trancount > 0 commit tran

	DECLARE @requestIds nvarchar(1024), @eventCode nvarchar(255), @updateId	UNIQUEIDENTIFIER
	set @updateId = newid()
	set @eventCode = N'REQ_STATUS'	
	set @requestIds = convert(nvarchar(1024), @orgRequestID)	
	exec dbo.RegisterEvent 
		  @accountId = @editorId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @requestIds
		, @eventParams = null
		, @updateId = @updateId
	
	RETURN 0
		
	undo:
		CLOSE linkedUsers
		DEALLOCATE linkedUsers	
		if @@trancount > 0 rollback
		return 1
END
GO