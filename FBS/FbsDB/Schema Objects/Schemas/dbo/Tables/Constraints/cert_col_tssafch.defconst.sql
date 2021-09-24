ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_tssafch] DEFAULT ((0)) FOR [UniqueTSSaFCheck];

