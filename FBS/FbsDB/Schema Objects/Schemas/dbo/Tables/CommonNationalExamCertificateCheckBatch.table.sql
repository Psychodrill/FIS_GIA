CREATE TABLE [dbo].[CommonNationalExamCertificateCheckBatch] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CreateDate]     DATETIME       NOT NULL,
    [UpdateDate]     DATETIME       NOT NULL,
    [OwnerAccountId] BIGINT         NULL,
    [IsProcess]      BIT            NOT NULL,
    [IsCorrect]      BIT            NULL,
    [Batch]          NTEXT          NOT NULL,
    [Executing]      BIT            NULL,
    [Filter]         NVARCHAR (255) NULL,
	[Type] int null /*default(0)*/,
	[outerId] bigint null,
	[Year] int null
);

