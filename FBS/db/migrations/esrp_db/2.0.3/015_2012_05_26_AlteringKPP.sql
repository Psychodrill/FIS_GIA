-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (15, '015_2012_05_26_AlteringKPP')
-- =========================================================================
PRINT N'Altering [dbo].[OrganizationRequest2010]...';
GO
ALTER TABLE [dbo].[OrganizationRequest2010]
    ADD [KPP] NVARCHAR (9) NULL;
GO
ALTER TABLE [dbo].[Organization2010]
    ADD [KPP] NVARCHAR (9) NULL;
GO
ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD [KPP] NVARCHAR (9) NULL;
GO

