CREATE TABLE [dbo].[Entrant] (
    [Id]                  BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]          DATETIME         NOT NULL,
    [UpdateDate]          DATETIME         NOT NULL,
    [UpdateId]            UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId]     BIGINT           NOT NULL,
    [EditorIp]            NVARCHAR (255)   NOT NULL,
    [OwnerOrganizationId] BIGINT           NOT NULL,
    [Year]                INT              NOT NULL,
    [LastName]            NVARCHAR (255)   NOT NULL,
    [FirstName]           NVARCHAR (255)   NOT NULL,
    [PatronymicName]      NVARCHAR (255)   NOT NULL,
    [CertificateNumber]   NVARCHAR (255)   NULL,
    [PassportNumber]      NVARCHAR (255)   NOT NULL,
    [PassportSeria]       NVARCHAR (255)   NOT NULL,
    [GIFOCategoryName]    NVARCHAR (255)   NULL,
    [DirectionCode]       NVARCHAR (255)   NOT NULL,
    [SpecialtyCode]       NVARCHAR (255)   NOT NULL
);

