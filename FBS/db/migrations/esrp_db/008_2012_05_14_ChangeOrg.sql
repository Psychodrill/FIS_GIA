-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (8, '008_2012_05_14_ChangeOrg.sql')
-- =========================================================================
GO

PRINT N'Altering [dbo].[Organization2010]...';


GO
ALTER TABLE [dbo].[Organization2010]
    ADD [ReceptionOnResultsCNE] BIT NULL;


GO
PRINT N'Altering [dbo].[OrganizationRequest2010]...';


GO
ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD [ReceptionOnResultsCNE] BIT NULL;


GO
PRINT N'Altering [dbo].[OrganizationUpdateHistory]...';


GO
ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD [ReceptionOnResultsCNE] BIT NULL;


GO
PRINT N'Altering Organization2010_fk...';


GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [Organization2010_fk];

GO
ALTER proc [dbo].[SelectInformationSystems]
AS
BEGIN
SELECT s.Name as ShortName, s.SystemID as SystemID, COUNT(g.SystemID) as NumberGroups, s.FullName, s.AvailableRegistration
	FROM dbo.System s
	LEFT JOIN [Group] g ON s.SystemID=g.SystemID
	GROUP BY s.Name, s.SystemID, s.FullName, s.AvailableRegistration
END
GO
