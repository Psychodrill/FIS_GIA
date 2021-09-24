


CREATE PROC [dbo].[VerifyAccount]
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
