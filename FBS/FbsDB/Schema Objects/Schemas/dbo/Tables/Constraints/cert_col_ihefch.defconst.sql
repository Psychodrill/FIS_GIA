ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_ihefch] DEFAULT ((0)) FOR [UniqueIHEFCheck];

