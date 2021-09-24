CREATE TABLE [dbo].[EntrantCheck] (
    [Id]                      BIGINT         IDENTITY (1, 1) NOT NULL,
    [BatchCheckId]            BIGINT         NOT NULL,
    [CertificateNumber]       NVARCHAR (255) NOT NULL,
    [IsCorrect]               BIT            NULL,
    [SourceEntrantId]         BIGINT         NULL,
    [SourceLastName]          NVARCHAR (255) NULL,
    [SourceFirstName]         NVARCHAR (255) NULL,
    [SourcePatronymicName]    NVARCHAR (255) NULL,
    [SourceOrganizationName]  NVARCHAR (255) NULL,
    [SourceEntrantCreateDate] DATETIME       NULL
);

