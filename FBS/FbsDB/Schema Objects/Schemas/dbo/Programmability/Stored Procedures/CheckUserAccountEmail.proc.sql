CREATE PROCEDURE [dbo].[CheckUserAccountEmail]
	@login nvarchar(255)
	,@email nvarchar(255)
	,@IsUniq bit out
AS
BEGIN
	-- если e-mail не меняется, то считаем его уникальным
	IF EXISTS(	SELECT 1 FROM dbo.Account WITH (NOLOCK) 
				WHERE Email = @email and [Login]=@login)
		SET @IsUniq = 1
	ELSE 
	IF EXISTS(	SELECT 1 FROM dbo.Account WITH (NOLOCK) 
				WHERE Email = @email and [Status]!='deactivated' and [Login]!=@login)
		SET @IsUniq = 0
	ELSE 
		SET @IsUniq = 1
END
