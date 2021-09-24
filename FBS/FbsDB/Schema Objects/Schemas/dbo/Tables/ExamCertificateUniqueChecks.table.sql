CREATE TABLE [dbo].[ExamCertificateUniqueChecks] (
    [Id]                 BIGINT NOT NULL,
    [Year]               INT    NOT NULL,
    [UniqueChecks]       INT    NOT NULL,
    [UniqueIHEaFCheck]   INT    NOT NULL,
    [UniqueIHECheck]     INT    NOT NULL,
    [UniqueIHEFCheck]    INT    NOT NULL,
    [UniqueTSSaFCheck]   INT    NOT NULL,
    [UniqueTSSCheck]     INT    NOT NULL,
    [UniqueTSSFCheck]    INT    NOT NULL,
    [UniqueRCOICheck]    INT    NOT NULL,
    [UniqueOUOCheck]     INT    NOT NULL,
    [UniqueFounderCheck] INT    NOT NULL,
    [UniqueOtherCheck]   INT    NOT NULL
);

