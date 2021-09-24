CREATE TABLE [dbo].[CommonNationalExamCertificateRequest] (
    [Id]                       BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BatchId]                  BIGINT           NOT NULL,
    [LastName]                 NVARCHAR (255)   NOT NULL,
    [FirstName]                NVARCHAR (255)   NOT NULL,
    [PatronymicName]           NVARCHAR (255)   NOT NULL,
    [PassportSeria]            NVARCHAR (255)   NOT NULL,
    [PassportNumber]           NVARCHAR (255)   NOT NULL,
    [IsCorrect]                BIT              NULL,
    [SourceCertificateId]      BIGINT           NULL,
    [SourceCertificateYear]    INT              NULL,
    [SourceCertificateNumber]  NVARCHAR (255)   NULL,
    [SourceRegionId]           INT              NULL,
    [IsDeny]                   BIT              NULL,
    [DenyComment]              NTEXT            NULL,
    [DenyNewCertificateNumber] NVARCHAR (255)   NULL,
    [msrepl_tran_version]      UNIQUEIDENTIFIER NOT NULL
);

