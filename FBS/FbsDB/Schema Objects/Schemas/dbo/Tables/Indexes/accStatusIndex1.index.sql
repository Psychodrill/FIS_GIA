CREATE NONCLUSTERED INDEX [accStatusIndex1]
    ON [dbo].[Account]([Status] ASC)
    INCLUDE([Id], [CreateDate], [Login], [LastName], [FirstName], [PatronymicName], [OrganizationId], [Email]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];
go
CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificate_PassportNumber] ON [dbo].[CommonNationalExamCertificate] 
(
	[PassportNumber] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
go
