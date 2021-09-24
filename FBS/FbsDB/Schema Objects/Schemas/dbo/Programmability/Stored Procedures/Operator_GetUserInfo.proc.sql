--------------------------------------------
-- Получение информации о пользователе
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE PROCEDURE [dbo].[Operator_GetUserInfo]
(	@OperatorLogin nvarchar(255), 
	@UserLogin nvarchar(255), 
	@IsMainOperator bit out, 
	@MainOperatorName varchar(255) out, 
	@Comments varchar(1024) out)
AS 
	SET NOCOUNT ON
	DECLARE @UserID int, @OperatorID int
	SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin
	SELECT @UserID=ID FROM dbo.Account WHERE [Login]=@UserLogin

	-- вставляем, если нет связи и другим оператором
	INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
	SELECT A.ID CheckedUserID, @OperatorID OperatorID
	FROM dbo.Account A
	LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
	WHERE A.ID=@UserID AND OL.CheckedUserID IS NULL

	-- данные о текущем пользователе
	SELECT 
		@IsMainOperator=CASE WHEN A.ID=@OperatorID THEN 1 ELSE 0 END,
		@MainOperatorName=A.LastName+' '+A.FirstName +'('+A.Login+')',
		@Comments=OL.Comments
	FROM dbo.OperatorLog OL
	INNER JOIN dbo.Account A ON OL.OperatorID=A.ID
	WHERE CheckedUserID=@UserID

	PRINT @@ROWCOUNT
	
	-- данные об остальных 'моих' пользователях
	SELECT A.Login, A.LastName+' '+FirstName FIO
	FROM dbo.OperatorLog OL
	INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
	WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID 
		AND A.Status='consideration'
		AND (Comments IS NULL OR LEN(RTRIM(Comments))=0)
		

	-- данные об остальных 'моих' пользователях с комментариями
	SELECT A.Login, A.LastName+' '+FirstName FIO
	FROM dbo.OperatorLog OL
	INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
	WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID
		AND A.Status='consideration'
		AND LEN(RTRIM(Comments))>0
