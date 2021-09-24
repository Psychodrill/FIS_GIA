-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (28, '028_2012_06_07_GetYearsInRequests')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetYearsInRequests]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetYearsInRequests]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[GetYearsInRequests]
AS
BEGIN
SELECT DISTINCT year(CreateDate) as [Year] from [dbo].[OrganizationRequest2010]
ORDER BY year(CreateDate) DESC
END
GO


