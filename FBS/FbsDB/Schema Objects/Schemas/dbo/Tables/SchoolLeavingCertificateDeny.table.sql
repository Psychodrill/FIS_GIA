CREATE TABLE [dbo].[SchoolLeavingCertificateDeny] (
    [Id]                    BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]            DATETIME         NOT NULL,
    [UpdateDate]            DATETIME         NOT NULL,
    [UpdateId]              UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId]       BIGINT           NOT NULL,
    [EditorIp]              NVARCHAR (255)   NOT NULL,
    [CertificateNumberFrom] NVARCHAR (255)   NOT NULL,
    [CertificateNumberTo]   NVARCHAR (255)   NOT NULL,
    [OrganizationName]      NVARCHAR (255)   NULL,
    [Comment]               NTEXT            NULL
);

