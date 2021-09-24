CREATE TABLE [dbo].[OrganizationLog] (
    [OrganizationId]             BIGINT           NOT NULL,
    [VersionId]                  INT              NOT NULL,
    [UpdateDate]                 DATETIME         NOT NULL,
    [UpdateId]                   UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId]            BIGINT           NULL,
    [EditorIp]                   NVARCHAR (255)   NOT NULL,
    [RegionId]                   BIGINT           NOT NULL,
    [DepartmentOwnershipCode]    NVARCHAR (255)   NULL,
    [Name]                       NVARCHAR (2000)  NOT NULL,
    [FounderName]                NVARCHAR (2000)  NOT NULL,
    [Address]                    NVARCHAR (500)   NOT NULL,
    [ChiefName]                  NVARCHAR (255)   NOT NULL,
    [Fax]                        NVARCHAR (255)   NULL,
    [Phone]                      NVARCHAR (255)   NULL,
    [EducationInstitutionTypeId] INT              NULL
);

