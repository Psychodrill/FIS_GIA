-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (35, '035__2011_07_20__AddIndexesSubj')
-- =========================================================================
GO




IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateSubject]') AND name = N'CNE_Subj_SubjId_Year')
DROP INDEX [CNE_Subj_SubjId_Year] ON [dbo].[CommonNationalExamCertificateSubject] WITH ( ONLINE = OFF )
GO

CREATE NONCLUSTERED INDEX [CNE_Subj_SubjId_Year] ON [dbo].[CommonNationalExamCertificateSubject] 
(
	[SubjectId] ASC,
	[Year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]







IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateSubjectCheck]') AND name = N'CNE_SubjCh_SubjId_Year')
DROP INDEX [CNE_SubjCh_SubjId_Year] ON [dbo].[CommonNationalExamCertificateSubjectCheck] WITH ( ONLINE = OFF )
GO

CREATE NONCLUSTERED INDEX [CNE_SubjCh_SubjId_Year] ON [dbo].[CommonNationalExamCertificateSubjectCheck] 
(
	[SubjectId] ASC,
	[Year] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]