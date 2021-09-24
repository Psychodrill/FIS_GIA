--------------------------------------------
-- Получение 1-го "не обработанного пользователя"
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE PROCEDURE [dbo].[Operator_GetNewUser]
(	@OperatorLogin nvarchar(255), 
	@UserID int out, 
	@UserLogin nvarchar(255) out
)
AS 
	SET NOCOUNT ON
	DECLARE @OperatorID int
	DECLARE  @T TABLE(CheckedUserID int) 

	SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin

	INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
		OUTPUT INSERTED.CheckedUserID INTO @T(CheckedUserID)
	SELECT TOP 1 
		A.ID CheckedUserID, 
		@OperatorID OperatorID
	FROM dbo.Account A
	INNER JOIN dbo.Organization O ON A.OrganizationID=O.Id AND O.EtalonOrgID IS NOT NULL
	INNER JOIN dbo.GroupAccount GA ON A.ID=GA.AccountId AND GA.GroupId=1
	LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
	WHERE A.Status='consideration' AND OL.CheckedUserID IS NULL
	ORDER BY A.CreateDate 

	SELECT TOP 1 @UserID=A.ID, @UserLogin=A.[Login]
	FROM dbo.Account A
	WHERE A.ID IN (SELECT CheckedUserID FROM @T)
