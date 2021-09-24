ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_ch] DEFAULT ((0)) FOR [UniqueChecks];

