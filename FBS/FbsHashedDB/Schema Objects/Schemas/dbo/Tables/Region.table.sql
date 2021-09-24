CREATE TABLE [dbo].[Region] (
    [Id]                   INT            NOT NULL,
    [Code]                 NVARCHAR (255) NOT NULL,
    [Name]                 NVARCHAR (255) NOT NULL,
    [InOrganization]       BIT            NOT NULL,
    [InCertificate]        BIT            NOT NULL,
    [SortIndex]            TINYINT        NOT NULL,
    [InOrganizationEtalon] BIT            NOT NULL
);

