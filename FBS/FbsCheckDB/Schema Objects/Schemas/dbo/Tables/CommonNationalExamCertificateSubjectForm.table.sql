CREATE TABLE [dbo].[CommonNationalExamCertificateSubjectForm] (
    [Id]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [Year]      INT      NOT NULL,
    [RegionId]  INT      NOT NULL,
    [Partition] BIGINT   NOT NULL,
    [FormId]    BIGINT   NOT NULL,
    [SubjectId] INT      NULL,
    [Mark]      SMALLINT NULL
);

