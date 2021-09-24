CREATE TABLE [dbo].[Document] (
    [Id]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]      DATETIME         NOT NULL,
    [UpdateDate]      DATETIME         NOT NULL,
    [UpdateId]        UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId] BIGINT           NOT NULL,
    [EditorIp]        NVARCHAR (255)   NOT NULL,
    [Name]            NVARCHAR (255)   NOT NULL,
    [Description]     NTEXT            NOT NULL,
    [Content]         IMAGE            NOT NULL,
    [ContentSize]     INT              NOT NULL,
    [ContentType]     NVARCHAR (255)   NULL,
    [IsActive]        BIT              NOT NULL,
    [ActivateDate]    DATETIME         NULL,
    [ContextCodes]    NVARCHAR (4000)  NULL,
    [RelativeUrl]     NVARCHAR (255)   NULL
);

