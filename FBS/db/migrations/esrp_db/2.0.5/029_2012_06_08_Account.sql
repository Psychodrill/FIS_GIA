-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (29, '029_2012_06_08_Account')
-- =========================================================================

PRINT N'Altering [dbo].[Account]...';


GO
ALTER TABLE [dbo].[Account] DROP COLUMN [HasCrocEgeIntegration];


GO