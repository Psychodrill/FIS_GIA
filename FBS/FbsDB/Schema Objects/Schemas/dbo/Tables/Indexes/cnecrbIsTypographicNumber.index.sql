CREATE NONCLUSTERED INDEX [cnecrbIsTypographicNumber]
    ON [dbo].[CommonNationalExamCertificateRequestBatch]([IsTypographicNumber] ASC)
    INCLUDE([Id], [CreateDate], [OwnerAccountId], [IsProcess], [IsCorrect], [year]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];

