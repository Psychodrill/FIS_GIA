/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists(select 1 where USER_ID('fbs')>0)
begin
CREATE USER [fbs]
WITH DEFAULT_SCHEMA=[dbo]
end

EXEC sp_addrolemember N'db_owner', N'fbs'
GO
