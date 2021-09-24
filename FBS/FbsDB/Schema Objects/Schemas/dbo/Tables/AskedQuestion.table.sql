CREATE TABLE [dbo].[AskedQuestion] (
    [Id]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]      DATETIME         NOT NULL,
    [UpdateDate]      DATETIME         NOT NULL,
    [UpdateId]        UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId] BIGINT           NOT NULL,
    [EditorIp]        NVARCHAR (10)    NOT NULL,
    [Name]            NVARCHAR (255)   NOT NULL,
    [Question]        NTEXT            NOT NULL,
    [Answer]          NTEXT            NOT NULL,
    [IsActive]        BIT              NOT NULL,
    [ViewCount]       INT              NOT NULL,
    [Popularity]      DECIMAL (18, 4)  NOT NULL,
    [ContextCodes]    NVARCHAR (4000)  NULL
);

