CREATE TABLE [dbo].[CommonNationalExamCertificateCheck] (
    [Id]                       BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BatchId]                  BIGINT           NOT NULL,
    [CertificateCheckingId]    UNIQUEIDENTIFIER NULL,
    [CertificateNumber]        NVARCHAR (255)   NOT NULL,
    [IsOriginal]               BIT              NULL,
    [LastName]                 NVARCHAR (255)   NULL,
    [FirstName]                NVARCHAR (255)   NULL,
    [PatronymicName]           NVARCHAR (255)   NULL,
    [IsCorrect]                BIT              NULL,
    [SourceCertificateId]      BIGINT           NULL,
    [SourceLastName]           NVARCHAR (255)   NULL,
    [SourceFirstName]          NVARCHAR (255)   NULL,
    [SourcePatronymicName]     NVARCHAR (255)   NULL,
    [IsDeny]                   BIT              NULL,
    [DenyComment]              NTEXT            NULL,
    [DenyNewCertificateNumber] NVARCHAR (255)   NULL,
    [Year]                     INT              NULL,
    [TypographicNumber]        NVARCHAR (255)   NULL,
    [RegionId]                 INT              NULL,
    [PassportSeria]            NVARCHAR (255)   NULL,
    [PassportNumber]           NVARCHAR (255)   NULL,
	[idtemp]				   BIGINT NULL -- id для загрузки проверок из закрытой фбс	
);

GO

create index idx_CommonNationalExamCertificateCheck_idtemp on CommonNationalExamCertificateCheck(idtemp)
GO
