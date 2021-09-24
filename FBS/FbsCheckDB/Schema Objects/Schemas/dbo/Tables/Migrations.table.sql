CREATE TABLE [dbo].[Migrations] (
    [MigrationVersion] INT           NOT NULL,
    [MigrationName]    VARCHAR (200) NOT NULL,
    [CreateDate]       DATETIME      DEFAULT (getdate()) NOT NULL
);

