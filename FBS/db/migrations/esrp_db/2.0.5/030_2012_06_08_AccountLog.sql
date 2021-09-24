-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (30, '030_2012_06_08_AccountLog')
-- =========================================================================
PRINT N'Altering [dbo].[AccountLog]...';


GO
ALTER TABLE [dbo].[AccountLog] DROP COLUMN [HasCrocEgeIntegration];


GO