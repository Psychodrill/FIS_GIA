-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (2, '002__2011_01_05__OrganizationFields')
-- =========================================================================


IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNFederalBudget')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_fb]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNFederalBudget]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNTargeted')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_targ]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNTargeted]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNLocalBudget')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_lb]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNLocalBudget]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNPaying')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_pay]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNPaying]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNFullTime')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_ft]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNFullTime]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNEvening')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_even]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNEvening]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'CNPostal')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_post]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [CNPostal]
END
GO


ALTER TABLE [dbo].[Organization2010]
	ADD [CNFederalBudget] int NOT NULL CONSTRAINT [org2010_col_fb] DEFAULT 0,
	[CNTargeted] int NOT NULL CONSTRAINT [org2010_col_targ] DEFAULT 0,
	[CNLocalBudget] int NOT NULL CONSTRAINT [org2010_col_lb] DEFAULT 0,
	[CNPaying] int  NOT NULL CONSTRAINT [org2010_col_pay] DEFAULT 0,
	[CNFullTime] int NOT NULL CONSTRAINT [org2010_col_ft] DEFAULT 0,
	[CNEvening] int NOT NULL CONSTRAINT [org2010_col_even] DEFAULT 0,
	[CNPostal] int NOT NULL CONSTRAINT [org2010_col_post] DEFAULT 0
GO


