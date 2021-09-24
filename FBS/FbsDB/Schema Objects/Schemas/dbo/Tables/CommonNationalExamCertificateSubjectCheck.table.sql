CREATE TABLE [dbo].[CommonNationalExamCertificateSubjectCheck] (
    [Id]                         BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BatchId]                    BIGINT         NOT NULL,
    [CheckId]                    BIGINT         NOT NULL,
    [SubjectId]                  BIGINT         NOT NULL,
    [Mark]                       NUMERIC (5, 1) NULL,
    [IsCorrect]                  BIT            NULL,
    [SourceCertificateSubjectId] BIGINT         NULL,
    [SourceMark]                 NUMERIC (5, 1) NULL,
    [SourceHasAppeal]            BIT            NULL,
    [Year]                       INT            NULL
);

GO

CREATE NONCLUSTERED INDEX [idx_CommonNationalExamCertificateSubjectCheck_CheckId] ON [dbo].[CommonNationalExamCertificateSubjectCheck] 
(
 [CheckId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO