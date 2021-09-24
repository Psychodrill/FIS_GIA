CREATE TABLE [dbo].[CommonNationalExamCertificateDeny] (
    [Id]                   BIGINT           NOT NULL,
    [CreateDate]           DATETIME         NOT NULL,
    [UpdateDate]           DATETIME         NOT NULL,
    [UpdateId]             UNIQUEIDENTIFIER NULL,
    [Year]                 INT              NOT NULL,
    [CertificateNumber]    NVARCHAR (255)   NOT NULL,
    [Comment]              NTEXT            NOT NULL,
    [NewCertificateNumber] NVARCHAR (255)   NULL
);

