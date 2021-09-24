CREATE TABLE [dbo].[CompetitionCertificateRequest] (
    [Id]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [BatchId]              BIGINT         NOT NULL,
    [LastName]             NVARCHAR (255) NOT NULL,
    [FirstName]            NVARCHAR (255) NOT NULL,
    [PatronymicName]       NVARCHAR (255) NOT NULL,
    [IsCorrect]            BIT            NULL,
    [SourceCertificateId]  BIGINT         NULL,
    [SourceLastName]       NVARCHAR (255) NULL,
    [SourceFirstName]      NVARCHAR (255) NULL,
    [SourcePatronymicName] NVARCHAR (255) NULL
);

