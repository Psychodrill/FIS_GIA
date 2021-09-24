CREATE TABLE [dbo].[AccountKey] (
    [Id]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]      DATETIME         NOT NULL,
    [UpdateDate]      DATETIME         NOT NULL,
    [UpdateId]        UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId] BIGINT           NOT NULL,
    [EditorIp]        NVARCHAR (255)   NOT NULL,
    [AccountId]       BIGINT           NOT NULL,
    [Key]             NVARCHAR (255)   NOT NULL,
    [DateFrom]        DATETIME         NULL,
    [DateTo]          DATETIME         NULL,
    [IsActive]        BIT              NOT NULL
);

