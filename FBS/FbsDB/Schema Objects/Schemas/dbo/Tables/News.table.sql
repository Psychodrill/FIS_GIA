CREATE TABLE [dbo].[News] (
    [Id]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]      DATETIME         NOT NULL,
    [UpdateDate]      DATETIME         NOT NULL,
    [UpdateId]        UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId] BIGINT           NOT NULL,
    [EditorIp]        NVARCHAR (255)   NOT NULL,
    [Date]            DATETIME         NOT NULL,
    [Name]            NVARCHAR (255)   NOT NULL,
    [Description]     NTEXT            NOT NULL,
    [Text]            NTEXT            NOT NULL,
    [IsActive]        BIT              NOT NULL
);

