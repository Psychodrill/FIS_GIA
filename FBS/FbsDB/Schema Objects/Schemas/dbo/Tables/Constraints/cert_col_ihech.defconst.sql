ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_ihech] DEFAULT ((0)) FOR [UniqueIHECheck];

