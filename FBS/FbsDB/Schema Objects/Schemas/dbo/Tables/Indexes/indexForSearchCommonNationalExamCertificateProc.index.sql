CREATE NONCLUSTERED INDEX [indexForSearchCommonNationalExamCertificateProc]
    ON [dbo].[ImportingCommonNationalExamCertificate]([LastName] ASC, [Year] ASC)
    INCLUDE([FirstName], [InternalPassportSeria], [Number], [PassportNumber], [PassportSeria], [PatronymicName], [RegionId]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];

