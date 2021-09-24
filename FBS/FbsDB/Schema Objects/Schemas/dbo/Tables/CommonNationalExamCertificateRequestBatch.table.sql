CREATE TABLE [dbo].[CommonNationalExamCertificateRequestBatch] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CreateDate]          DATETIME       NOT NULL,
    [UpdateDate]          DATETIME       NOT NULL,
    [OwnerAccountId]      BIGINT         NOT NULL,
    [IsProcess]           BIT            NOT NULL,
    [IsCorrect]           BIT            NULL,
    [Batch]               NTEXT          NOT NULL,
    [Executing]           BIT            NULL,
    [IsTypographicNumber] BIT            NOT NULL,
    [year]                INT            NULL,
    [Filter]              NVARCHAR (255) NULL
);

