-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (14, '014_2012_05_24_AddUserMainOrgCheck')
-- =========================================================================

/****** Object:  UserDefinedFunction [dbo].[IsUserFromMainOrg]    Script Date: 05/24/2012 09:45:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsUserFromMainOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IsUserFromMainOrg]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[IsUserFromMainOrg](@login nvarchar(255))
returns bit
as
BEGIN
DECLARE @ret bit
SELECT @ret = CASE WHEN EXISTS ( SELECT   *
                       FROM     dbo.Organization2010
                       WHERE    MainId IN (
                                SELECT  org.Id
                                FROM    dbo.Account acc
                                        INNER JOIN dbo.OrganizationRequest2010 req ON acc.OrganizationId = req.Id
                                        INNER JOIN dbo.Organization2010 org ON req.OrganizationId = org.Id
                                WHERE   acc.[Login] = LTRIM(RTRIM(@login)) ) )
         THEN 1
         ELSE 0
    END 
	return @ret
END

