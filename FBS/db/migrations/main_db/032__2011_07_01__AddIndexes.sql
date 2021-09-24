-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (32, '032__2011_07_01__AddIndexes')
-- =========================================================================
GO


-- Индексы по сертификатам

/****** Object:  Index [CNE_Id]    Script Date: 07/01/2011 11:43:38 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'CNE_Id')
DROP INDEX [CNE_Id] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [CNE_Id]    Script Date: 07/01/2011 11:42:56 ******/
CREATE UNIQUE NONCLUSTERED INDEX [CNE_Id] ON [dbo].[CommonNationalExamCertificate] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  Index [CNE_IdNumber]    Script Date: 07/01/2011 11:45:23 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'CNE_IdNumber')
DROP INDEX [CNE_IdNumber] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [CNE_IdNumber]    Script Date: 07/01/2011 11:45:53 ******/
CREATE NONCLUSTERED INDEX [CNE_IdNumber] ON [dbo].[CommonNationalExamCertificate] 
(
	[Id] ASC,
	[Number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  Index [CNE_YearNumTNLastName]    Script Date: 07/01/2011 11:46:22 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'CNE_YearNumTNLastName')
DROP INDEX [CNE_YearNumTNLastName] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [CNE_YearNumTNLastName]    Script Date: 07/01/2011 11:46:44 ******/
CREATE NONCLUSTERED INDEX [CNE_YearNumTNLastName] ON [dbo].[CommonNationalExamCertificate] 
(
	[Year] ASC,
	[Number] ASC,
	[TypographicNumber] ASC
)
INCLUDE ( [LastName]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO




/****** Object:  Index [CNE_YearPassportLastName]    Script Date: 07/01/2011 11:47:35 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'CNE_YearPassportLastName')
DROP INDEX [CNE_YearPassportLastName] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [CNE_YearPassportLastName]    Script Date: 07/01/2011 11:47:53 ******/
CREATE NONCLUSTERED INDEX [CNE_YearPassportLastName] ON [dbo].[CommonNationalExamCertificate] 
(
	[Year] ASC,
	[InternalPassportSeria] ASC,
	[PassportNumber] ASC
)
INCLUDE ( [LastName]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO







-- Индексы по предметам

/****** Object:  Index [CNE_Subj_Mark]    Script Date: 07/01/2011 11:50:48 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateSubject]') AND name = N'CNE_Subj_Mark')
DROP INDEX [CNE_Subj_Mark] ON [dbo].[CommonNationalExamCertificateSubject] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [CNE_Subj_Mark]    Script Date: 07/01/2011 11:51:04 ******/
CREATE NONCLUSTERED INDEX [CNE_Subj_Mark] ON [dbo].[CommonNationalExamCertificateSubject] 
(
	[Mark] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



/****** Object:  Index [CNE_Subj_SubjId]    Script Date: 07/01/2011 11:51:20 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateSubject]') AND name = N'CNE_Subj_SubjId')
DROP INDEX [CNE_Subj_SubjId] ON [dbo].[CommonNationalExamCertificateSubject] WITH ( ONLINE = OFF )
GO

/****** Object:  Index [CNE_Subj_SubjId]    Script Date: 07/01/2011 11:51:40 ******/
CREATE NONCLUSTERED INDEX [CNE_Subj_SubjId] ON [dbo].[CommonNationalExamCertificateSubject] 
(
	[SubjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO