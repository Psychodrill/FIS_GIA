CREATE TABLE [dbo].[CommonNationalExamCertificateCheckBatch] (
    [Id]                  BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CreateDate]          DATETIME         NOT NULL,
    [UpdateDate]          DATETIME         NOT NULL,
    [OwnerAccountId]      BIGINT           NOT NULL,
    [IsProcess]           BIT              NOT NULL,
    [IsCorrect]           BIT              NULL,
    [Batch]               NTEXT            NOT NULL,
    [Executing]           BIT              NULL,
    [msrepl_tran_version] UNIQUEIDENTIFIER NOT NULL
);

