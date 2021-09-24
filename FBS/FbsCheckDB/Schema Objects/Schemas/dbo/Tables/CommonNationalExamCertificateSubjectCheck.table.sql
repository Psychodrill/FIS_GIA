CREATE TABLE [dbo].[CommonNationalExamCertificateSubjectCheck] (
    [Id]                         BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CertificateCheckingId]      UNIQUEIDENTIFIER NULL,
    [BatchId]                    BIGINT           NOT NULL,
    [CheckId]                    BIGINT           NOT NULL,
    [SubjectId]                  BIGINT           NOT NULL,
    [Mark]                       SMALLINT         NULL,
    [IsCorrect]                  BIT              NULL,
    [SourceCertificateSubjectId] BIGINT           NULL,
    [SourceMark]                 SMALLINT         NULL,
    [SourceHasAppeal]            BIT              NULL,
    [msrepl_tran_version]        UNIQUEIDENTIFIER NOT NULL
);

