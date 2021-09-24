--------------------------------------------
-- Добавление комментария пользователю
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE PROCEDURE dbo.Operator_AddUserComment
(@UserLogin nvarchar(255), @Comment varchar(1024))
AS 
	SET NOCOUNT ON

	UPDATE dbo.OperatorLog
	SET Comments=@Comment, DTLastChange=GETDATE()
	WHERE CheckedUserID IN (SELECT ID FROM dbo.Account WHERE [Login]=@UserLogin)
	
