ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_otherch] DEFAULT ((0)) FOR [UniqueOtherCheck];

