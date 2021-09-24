-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (25, '025_2012_05_26_DeletingTable')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccountNew]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserAccountNew]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserAccount]
GO



