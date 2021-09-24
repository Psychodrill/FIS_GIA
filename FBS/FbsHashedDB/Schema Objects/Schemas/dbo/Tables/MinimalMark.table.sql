CREATE TABLE [dbo].[MinimalMark] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SubjectId]   INT            NOT NULL,
    [Year]        INT            NOT NULL,
    [MinimalMark] NUMERIC (5, 1) NOT NULL,
    [AccountId]   INT            NULL,
    [Updated]     DATETIME       NULL
);

