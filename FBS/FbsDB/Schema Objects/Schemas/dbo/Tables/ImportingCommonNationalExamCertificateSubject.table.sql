CREATE TABLE [dbo].[ImportingCommonNationalExamCertificateSubject] (
    [Id]            BIGINT   NOT NULL,
    [CertificateId] BIGINT   NOT NULL,
    [SubjectId]     BIGINT   NOT NULL,
    [Mark]          SMALLINT NOT NULL,
    [HasAppeal]     BIT      NOT NULL,
    [Year]          INT      NOT NULL,
    [RegionId]      INT      NOT NULL
);

