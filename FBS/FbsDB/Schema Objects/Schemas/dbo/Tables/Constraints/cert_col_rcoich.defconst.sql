ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
    ADD CONSTRAINT [cert_col_rcoich] DEFAULT ((0)) FOR [UniqueRCOICheck];

