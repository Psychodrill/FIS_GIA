CREATE TABLE [dbo].[CommonNationalExamCertificateCheck] (
    [Id]                       BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CertificateCheckingId]    UNIQUEIDENTIFIER NULL,
    [BatchId]                  BIGINT           NOT NULL,
    [CertificateNumber]        NVARCHAR (255)   NOT NULL,
    [IsOriginal]               BIT              NOT NULL,
    [LastName]                 NVARCHAR (255)   NOT NULL,
    [FirstName]                NVARCHAR (255)   NOT NULL,
    [PatronymicName]           NVARCHAR (255)   NOT NULL,
    [IsCorrect]                BIT              NULL,
    [SourceCertificateId]      BIGINT           NULL,
    [SourceLastName]           NVARCHAR (255)   NULL,
    [SourceFirstName]          NVARCHAR (255)   NULL,
    [SourcePatronymicName]     NVARCHAR (255)   NULL,
    [IsDeny]                   BIT              NULL,
    [DenyComment]              NTEXT            NULL,
    [DenyNewCertificateNumber] NVARCHAR (255)   NULL,
    [msrepl_tran_version]      UNIQUEIDENTIFIER NOT NULL
);

