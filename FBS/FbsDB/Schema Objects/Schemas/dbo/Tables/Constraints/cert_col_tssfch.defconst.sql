ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_tssfch] DEFAULT ((0)) FOR [UniqueTSSFCheck];

