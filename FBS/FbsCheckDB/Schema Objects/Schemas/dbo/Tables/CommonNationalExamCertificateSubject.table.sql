CREATE TABLE [dbo].[CommonNationalExamCertificateSubject] (
    [Id]            BIGINT         NOT NULL,
    [CertificateId] BIGINT         NOT NULL,
    [SubjectId]     BIGINT         NOT NULL,
    [Mark]          NUMERIC (5, 1) NULL,
    [HasAppeal]     BIT            NOT NULL,
    [Year]          INT            NOT NULL,
    [RegionId]      INT            NOT NULL
);

