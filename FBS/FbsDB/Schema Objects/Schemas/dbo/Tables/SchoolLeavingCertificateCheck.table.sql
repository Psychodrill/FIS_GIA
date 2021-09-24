CREATE TABLE [dbo].[SchoolLeavingCertificateCheck] (
    [Id]                      BIGINT         IDENTITY (1, 1) NOT NULL,
    [BatchId]                 BIGINT         NOT NULL,
    [CertificateNumber]       NVARCHAR (255) NOT NULL,
    [IsCorrect]               BIT            NOT NULL,
    [SourceCertificateDenyId] BIGINT         NULL
);

