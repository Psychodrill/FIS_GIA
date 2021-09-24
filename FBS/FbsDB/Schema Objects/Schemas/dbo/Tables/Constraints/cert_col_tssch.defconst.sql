ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_tssch] DEFAULT ((0)) FOR [UniqueTSSCheck];

