ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_ouoch] DEFAULT ((0)) FOR [UniqueOUOCheck];

