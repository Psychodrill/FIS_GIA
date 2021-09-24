CREATE TABLE [dbo].[CommonNationalExamCertificateSubjectForm] (
    [Id]        BIGINT         NOT NULL,
    [Year]      INT            NOT NULL,
    [RegionId]  INT            NOT NULL,
    [Partition] BIGINT         NOT NULL,
    [FormId]    BIGINT         NOT NULL,
    [SubjectId] INT            NOT NULL,
    [Mark]      NUMERIC (5, 1) NULL
);

