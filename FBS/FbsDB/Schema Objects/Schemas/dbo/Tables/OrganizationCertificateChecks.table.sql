CREATE TABLE [dbo].[OrganizationCertificateChecks] (
    [CertificateId]  BIGINT NOT NULL,
    [OrganizationId] INT    NOT NULL,
    PRIMARY KEY CLUSTERED ([CertificateId] ASC, [OrganizationId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

