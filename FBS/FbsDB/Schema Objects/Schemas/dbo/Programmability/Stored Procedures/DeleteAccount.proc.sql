-- =============================================
-- Author:		Сулиманов А.М.
-- Create date: 2009-05-07
-- Description:	Удаление из БД всего, что касается AccountId (не анализируются связи)
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAccount]
(@AccountID int)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM dbo.AccountIp WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountKey WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountLog WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountRoleActivity WHERE AccountId=@AccountID
	DELETE FROM dbo.GroupAccount WHERE AccountId=@AccountID
	DELETE FROM dbo.Organization WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Id=@AccountID)
	DELETE FROM dbo.UserAccountPassword WHERE AccountId=@AccountID
	DELETE FROM dbo.Account WHERE Id=@AccountID
END
