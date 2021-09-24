CREATE TABLE [dbo].[CommonNationalExamCertificateDenyLoadingTaskError] (
    [Id]       BIGINT   IDENTITY (1, 1) NOT NULL,
    [Date]     DATETIME NOT NULL,
    [TaskId]   BIGINT   NOT NULL,
    [RowIndex] INT      NULL,
    [Error]    NTEXT    NOT NULL
);

