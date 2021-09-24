CREATE TABLE [dbo].[CommonNationalExamCertificateRequest] (
    [Id]                       BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BatchId]                  BIGINT         NOT NULL,
    [LastName]                 NVARCHAR (255) NOT NULL,
    [FirstName]                NVARCHAR (255) NULL,
    [PatronymicName]           NVARCHAR (255) NULL,
    [PassportSeria]            NVARCHAR (255) NULL,
    [PassportNumber]           NVARCHAR (255) NULL,
    [IsCorrect]                BIT            NULL,
    [SourceCertificateId]      BIGINT         NULL,
    [SourceCertificateYear]    INT            NULL,
    [SourceCertificateNumber]  NVARCHAR (255) NULL,
    [SourceRegionId]           INT            NULL,
    [IsDeny]                   BIT            NULL,
    [DenyComment]              NTEXT          NULL,
    [DenyNewCertificateNumber] NVARCHAR (255) NULL,
    [TypographicNumber]        NVARCHAR (255) NULL
);

