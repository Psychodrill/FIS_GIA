-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (51, '051_2012_06_21_NoLogginInClosedFbs.sql')
-- =========================================================================
GO

ALTER TABLE [dbo].[CheckCommonNationalExamCertificateLog] 
	ALTER COLUMN [AccountId] BIGINT NULL
	
GO

alter TABLE [dbo].[CommonNationalExamCertificateCheckBatch] 
	alter column [OwnerAccountId] BIGINT NULL
	
GO	