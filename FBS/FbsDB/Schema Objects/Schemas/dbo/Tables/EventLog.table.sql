CREATE TABLE [dbo].[EventLog] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [Date]           DATETIME         NOT NULL,
    [AccountId]      BIGINT           NULL,
    [Ip]             NVARCHAR (255)   NOT NULL,
    [EventCode]      NVARCHAR (100)   NOT NULL,
    [SourceEntityId] BIGINT           NULL,
    [EventParams]    NVARCHAR (4000)  NULL,
    [UpdateId]       UNIQUEIDENTIFIER NULL
);

