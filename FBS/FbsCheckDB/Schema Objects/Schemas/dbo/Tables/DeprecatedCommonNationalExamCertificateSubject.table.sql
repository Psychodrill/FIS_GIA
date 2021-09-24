CREATE TABLE [dbo].[DeprecatedCommonNationalExamCertificateSubject] (
    [Id]            BIGINT         NOT NULL,
    [CertificateId] BIGINT         NOT NULL,
    [SubjectId]     BIGINT         NOT NULL,
    [Mark]          NUMERIC (5, 1) NOT NULL,
    [HasAppeal]     BIT            NOT NULL,
    [Year]          INT            NOT NULL,
    [RegionId]      INT            NOT NULL
);

