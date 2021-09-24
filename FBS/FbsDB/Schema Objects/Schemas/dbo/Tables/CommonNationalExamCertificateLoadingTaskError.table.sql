CREATE TABLE [dbo].[CommonNationalExamCertificateLoadingTaskError] (
    [Id]       BIGINT   IDENTITY (1, 1) NOT NULL,
    [Date]     DATETIME NOT NULL,
    [TaskId]   BIGINT   NOT NULL,
    [RowIndex] BIGINT   NULL,
    [Error]    NTEXT    NULL
);

