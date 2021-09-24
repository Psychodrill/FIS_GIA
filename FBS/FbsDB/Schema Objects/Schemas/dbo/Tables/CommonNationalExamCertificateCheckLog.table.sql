CREATE TABLE [dbo].[CommonNationalExamCertificateCheckLog] (
    [Date]       DATETIME       NOT NULL,
    [AccountId]  BIGINT         NULL,
    [Ip]         NVARCHAR (255) NULL,
    [PeriodName] NVARCHAR (20)  NULL,
    [Count]      BIGINT         NOT NULL,
    [IsBatch]    BIT            NOT NULL
);

