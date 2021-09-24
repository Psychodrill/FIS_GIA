ALTER TABLE [dbo].[AccountIp]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountKey]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountLog]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountRoleActivity]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CheckCommonNationalExamCertificateLog]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateCheckLog]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[EventLog]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[GroupAccount]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[UserAccountPassword]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AskedQuestionContext]  WITH CHECK ADD FOREIGN KEY([AskedQuestionId])
REFERENCES [dbo].[AskedQuestion] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateCheck]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[CommonNationalExamCertificateCheckBatch] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateRequest]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[CommonNationalExamCertificateCheckBatch] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubjectCheck]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[CommonNationalExamCertificateCheckBatch] ([Id])
GO

ALTER TABLE [dbo].[CompetitionCertificateRequest]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[CommonNationalExamCertificateCheckBatch] ([Id])
GO

ALTER TABLE [dbo].[SchoolLeavingCertificateCheck]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[CommonNationalExamCertificateCheckBatch] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([CertificateId])
REFERENCES [dbo].CommonNationalExamcertificate ([Id])
GO

ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([CertificateId])
REFERENCES [dbo].CommonNationalExamcertificate ([Id])
GO

ALTER TABLE [dbo].[ImportingCommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([CertificateId])
REFERENCES [dbo].CommonNationalExamcertificate ([Id])
GO

ALTER TABLE [dbo].[OrganizationCertificateChecks]  WITH CHECK ADD FOREIGN KEY([CertificateId])
REFERENCES [dbo].CommonNationalExamcertificate ([Id])
GO

ALTER TABLE [dbo].[OperatorLog]  WITH CHECK ADD FOREIGN KEY([CheckedUserID])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CompetitionCertificate]  WITH CHECK ADD FOREIGN KEY([CompetitionTypeId])
REFERENCES [dbo].[CompetitionType] ([Id])
GO

ALTER TABLE [dbo].[AskedQuestionContext]  WITH CHECK ADD FOREIGN KEY([ContextId])
REFERENCES [dbo].[Context] ([Id])
GO


ALTER TABLE [dbo].[DocumentContext]  WITH CHECK ADD FOREIGN KEY([ContextId])
REFERENCES [dbo].[Context] ([Id])
GO

ALTER TABLE [dbo].[DocumentContext]  WITH CHECK ADD FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([Id])
GO

ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountKey]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountLog]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AskedQuestion]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificate]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateDenyLoadingTask]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateLoadingTask]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CompetitionCertificate]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificate]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[Document]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[Entrant]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[EntrantRenunciation]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[ImportingCommonNationalExamCertificate]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[News]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[OrganizationLog]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[SchoolLeavingCertificateDeny]  WITH CHECK ADD FOREIGN KEY([EditorAccountId])
REFERENCES [dbo].[Account] ([Id])
GO
/*
ALTER TABLE [dbo].[OrganizationLog]  WITH CHECK ADD FOREIGN KEY([EducationInstitutionTypeId])
REFERENCES [dbo].[EducationInstitutionType] ([Id])
GO*/

ALTER TABLE [dbo].[GroupAccount]  WITH CHECK ADD FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO

ALTER TABLE [dbo].[GroupRole]  WITH CHECK ADD FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO

ALTER TABLE [dbo].[OperatorLog]  WITH CHECK ADD FOREIGN KEY([OperatorID])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[AccountLog]  WITH CHECK ADD FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[OrganizationCertificateChecks]  WITH CHECK ADD FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[OrganizationLog]  WITH CHECK ADD FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateCheckBatch]  WITH CHECK ADD FOREIGN KEY([OwnerAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateRequestBatch]  WITH CHECK ADD FOREIGN KEY([OwnerAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[CompetitionCertificateRequestBatch]  WITH CHECK ADD FOREIGN KEY([OwnerAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[EntrantCheckBatch]  WITH CHECK ADD FOREIGN KEY([OwnerAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[SchoolLeavingCertificateCheckBatch]  WITH CHECK ADD FOREIGN KEY([OwnerAccountId])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[Entrant]  WITH CHECK ADD FOREIGN KEY([OwnerOrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO

ALTER TABLE [dbo].[EntrantRenunciation]  WITH CHECK ADD FOREIGN KEY([OwnerOrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificate]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateCheck]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateForm]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateFormNumberRange]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubjectForm]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CompetitionCertificate]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificate]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[ImportingCommonNationalExamCertificate]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[ImportingCommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[OrganizationLog]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[AccountRoleActivity]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO

ALTER TABLE [dbo].[GroupRole]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO

ALTER TABLE [dbo].[SchoolLeavingCertificateCheck]  WITH CHECK ADD FOREIGN KEY([SourceCertificateDenyId])
REFERENCES [dbo].[SchoolLeavingCertificateDeny] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateCheck]  WITH CHECK ADD FOREIGN KEY([SourceCertificateId])
REFERENCES [dbo].[CommonNationalExamCertificate] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateRequest]  WITH CHECK ADD FOREIGN KEY([SourceCertificateId])
REFERENCES [dbo].[CommonNationalExamCertificate] ([Id])
GO

ALTER TABLE [dbo].[CompetitionCertificateRequest]  WITH CHECK ADD FOREIGN KEY([SourceCertificateId])
REFERENCES [dbo].[CommonNationalExamCertificate] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateRequest]  WITH CHECK ADD FOREIGN KEY([SourceRegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubjectCheck]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

ALTER TABLE [dbo].[CommonNationalExamCertificateSubjectForm]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

ALTER TABLE [dbo].[CompetitionType]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

ALTER TABLE [dbo].[ImportingCommonNationalExamCertificateSubject]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

ALTER TABLE [dbo].[MinimalMark]  WITH CHECK ADD FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO

DROP INDEX [idxGroupAccount] ON [dbo].[GroupAccount] WITH ( ONLINE = OFF )

DROP INDEX [SubjectIndex] ON [dbo].[Subject] WITH ( ONLINE = OFF )

ALTER TABLE [dbo].[CompetitionType] ADD  CONSTRAINT [PK_CompetitionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CommonNationalExamCertificateDenyLoadingTaskError] ADD  CONSTRAINT [PK_CommonNationalExamCertificateDenyLoadingTaskError] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[AccountKey] ADD  CONSTRAINT [PK_AccountKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CheckCommonNationalExamCertificateLog] ADD  CONSTRAINT [PK_CheckCommonNationalExamCertificateLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[VpnIp] ADD  CONSTRAINT [PK_VpnIp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[DeprecatedCommonNationalExamCertificate] ADD  CONSTRAINT [PK_DeprecatedCommonNationalExamCertificate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[DeliveryRecipients] ADD  CONSTRAINT [PK_DeliveryRecipients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[DeliveryLog] ADD  CONSTRAINT [PK_DeliveryLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[GroupAccount] ADD  CONSTRAINT [PK_GroupAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[AccountRoleActivity] ADD  CONSTRAINT [PK_AccountRoleActivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CNEWebUICheckLog] ADD  CONSTRAINT [PK_CNEWebUICheckLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CommonNationalExamCertificateCheckLog] ADD  CONSTRAINT [PK_CommonNationalExamCertificateCheckLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
GO