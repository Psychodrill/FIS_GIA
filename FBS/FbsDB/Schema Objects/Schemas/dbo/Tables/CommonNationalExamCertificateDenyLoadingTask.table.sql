CREATE TABLE [dbo].[CommonNationalExamCertificateDenyLoadingTask] (
    [Id]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]      DATETIME         NOT NULL,
    [UpdateDate]      DATETIME         NOT NULL,
    [UpdateId]        UNIQUEIDENTIFIER NULL,
    [EditorAccountId] BIGINT           NULL,
    [EditorIp]        NVARCHAR (255)   NULL,
    [SourceBatchUrl]  NVARCHAR (255)   NOT NULL,
    [IsActive]        BIT              NOT NULL,
    [IsProcess]       BIT              NULL,
    [IsCorrect]       BIT              NULL,
    [IsLoaded]        BIT              NOT NULL
);

