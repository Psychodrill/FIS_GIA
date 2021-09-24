CREATE TABLE [dbo].[AccountIp] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [AccountId] BIGINT         NOT NULL,
    [Ip]        NVARCHAR (255) NULL
);

