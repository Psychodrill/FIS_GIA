-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (37, '037__2011_07_26__AddIndexes')
-- =========================================================================
GO


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ExamCertificateUniqueChecks]') AND name = N'IExamCertificateUniqueChecks_Id')
DROP INDEX [IExamCertificateUniqueChecks_Id] ON [dbo].[ExamCertificateUniqueChecks] WITH ( ONLINE = OFF )
GO


create index IExamCertificateUniqueChecks_Id on dbo.ExamCertificateUniqueChecks(Id)
go