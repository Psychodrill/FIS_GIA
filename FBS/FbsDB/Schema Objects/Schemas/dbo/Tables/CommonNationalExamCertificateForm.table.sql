CREATE TABLE [dbo].[CommonNationalExamCertificateForm] (
    [Id]                 BIGINT           NOT NULL,
    [Number]             NVARCHAR (255)   NOT NULL,
    [CreateDate]         DATETIME         NOT NULL,
    [UpdateDate]         DATETIME         NOT NULL,
    [UpdateId]           UNIQUEIDENTIFIER NOT NULL,
    [Year]               INT              NOT NULL,
    [RegionId]           INT              NOT NULL,
    [Partition]          BIGINT           NOT NULL,
    [CertificateNumber]  NVARCHAR (255)   NULL,
    [LastName]           NVARCHAR (255)   NULL,
    [FirstName]          NVARCHAR (255)   NULL,
    [PatronymicName]     NVARCHAR (255)   NULL,
    [PassportSeria]      NVARCHAR (255)   NULL,
    [PassportNumber]     NVARCHAR (255)   NULL,
    [IsBlank]            BIT              NOT NULL,
    [IsDeny]             BIT              NOT NULL,
    [IsDuplicate]        BIT              NOT NULL,
    [IsValid]            BIT              NULL,
    [IsCertificateExist] BIT              NULL,
    [IsCertificateDeny]  BIT              NULL
);

