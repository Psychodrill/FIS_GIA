ALTER TABLE [dbo].[CommonNationalExamCertificateCheck]
    ADD CONSTRAINT [DF_CommonNationalExamCertificateCheck_IsOriginal] DEFAULT ((1)) FOR [IsOriginal];

