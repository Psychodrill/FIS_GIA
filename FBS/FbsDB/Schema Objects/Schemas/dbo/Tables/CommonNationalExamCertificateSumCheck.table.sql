CREATE TABLE [dbo].[CommonNationalExamCertificateSumCheck] (
    [Id]       BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BatchId]  BIGINT          NOT NULL,
    [Name]     NVARCHAR (300)  NOT NULL,
    [Sum]      NUMERIC (10, 2) NULL,
    [Status]   INT             NOT NULL,
    [NameSake] BIT             NOT NULL
);

