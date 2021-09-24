ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_founderch] DEFAULT ((0)) FOR [UniqueFounderCheck];

