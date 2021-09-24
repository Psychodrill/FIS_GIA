ALTER TABLE [dbo].[CommonNationalExamCertificateRequestBatch]
    ADD CONSTRAINT [DF_CommonNationalExamCertificateRequestBatch_IsTypographic] DEFAULT ((0)) FOR [IsTypographicNumber];

