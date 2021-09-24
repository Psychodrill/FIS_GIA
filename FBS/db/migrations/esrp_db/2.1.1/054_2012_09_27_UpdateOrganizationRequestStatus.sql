-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (54, '054_2012_09_27_UpdateOrganizationRequestStatus.sql')
go

alter PROCEDURE [dbo].[UpdateOrganizationRequestStatus]	
	  @orgRequestID INT
	, @statusID INT
	, @needConsiderLinkedUsers BIT = 1 -- ���� 1, �� ������ ������� ������������� ������������� � ������
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
		
	if @statusID=5 and not exists(SELECT * 
				  FROM OrganizationRequest2010 or1          
						join Organization2010 org on org.id=or1.OrganizationId       
						join Organization2010 orgm on orgm.id=org.MainId
						join OrganizationRequest2010 orm on orgm.id=orm.OrganizationId
						join Account ac on ac.OrganizationId=orm.Id 
                  Where or1.Id = @orgRequestID and (ac.Status = 'activated' OR ac.Status='deactivated'))
				  and exists(SELECT * 
				  FROM OrganizationRequest2010 or1          
						join Organization2010 org on org.id=or1.OrganizationId       
				  where org.MainId is not null and or1.Id = @orgRequestID)
    begin
		RAISERROR('$���������� ������������ ������������, ��� ��� � ��������� ���������� ��� �������������� �������������', 18, 1)
		RETURN -1     
    end
                    		
	-- ���� ���������� ��������� �� ���������� �������
	IF (@statusID = 5 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (2,3,6)))
	BEGIN
		RAISERROR('$���������� ������������ ������ � ������� �������.', 18, 1)
		RETURN -1
	END		
			
	-- GVUZ-595 ��������� ������������� ������ ��� ����������� ����������.
	IF (@statusID = 5 AND EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$���������� ������������ ������, �.�. �� ��������� ����� ���������� ��� �������������� �������������.', 18, 1)
		RETURN -1
	END			

	-- ���� ������������ ��������� �� ���������� �������
	IF (@statusID = 6 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5)))
	BEGIN
		RAISERROR('$���������� �������������� ������ � ������� �������.', 18, 1)
		RETURN -1
	END		

	-- ���� ���������� �� ��������� ��������� �� ���������� �������
	IF (@statusID = 3 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5,6)))
	BEGIN
		RAISERROR('$���������� ��������� �� ��������� ������ � ������� �������.', 18, 1)
		RETURN -1
	END
	
	BEGIN TRAN	
		-- ����� ������� ���������
		-- GVUZ-810 ��� �������� �� ��������� ('revision') �� �������� Registration � Revision ������ �� ��������.
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
				
		-- ���� ��������� ������������� � ������, �� ���� �� ���� ������������� ��������� � ������ �� ������ �� ����� ������ ��� ���������
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

