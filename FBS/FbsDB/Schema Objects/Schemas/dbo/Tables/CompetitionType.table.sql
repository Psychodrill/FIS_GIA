CREATE TABLE [dbo].[CompetitionType] (
    [Id]        INT            NOT NULL,
    [Code]      NVARCHAR (255) NOT NULL,
    [Name]      NVARCHAR (255) NOT NULL,
    [SubjectId] INT            NULL
);

