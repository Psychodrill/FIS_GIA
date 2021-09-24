CREATE TABLE [dbo].[Subject] (
    [Id]        INT            NOT NULL,
    [Code]      NVARCHAR (255) NOT NULL,
    [Name]      NVARCHAR (255) NOT NULL,
    [SortIndex] INT            NOT NULL,
    [IsActive]  BIT            NOT NULL
);

