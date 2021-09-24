-- походу не используется в FBS
CREATE TABLE [dbo].[OrganizationRequest2010] (
    [Id]                       INT             IDENTITY (1, 1) NOT NULL,
    [CreateDate]               DATETIME        NOT NULL,
    [UpdateDate]               DATETIME        NOT NULL,
    [FullName]                 NVARCHAR (1000) NOT NULL,
    [ShortName]                NVARCHAR (500)  NULL,
    [RegionId]                 INT             NOT NULL,
    [TypeId]                   INT             NOT NULL,
    [KindId]                   INT             NULL,
    [INN]                      NVARCHAR (10)   NULL,
    [OGRN]                     NVARCHAR (13)   NULL,
    [OwnerDepartment]          NVARCHAR (500)  NULL,
    [IsPrivate]                BIT             NULL,
    [IsFilial]                 BIT             NULL,
    [DirectorPosition]         NVARCHAR (255)  NULL,
    [DirectorFullName]         NVARCHAR (255)  NULL,
    [IsAccredited]             BIT             NULL,
    [AccreditationSertificate] NVARCHAR (255)  NULL,
    [LawAddress]               NVARCHAR (255)  NULL,
    [FactAddress]              NVARCHAR (255)  NULL,
    [PhoneCityCode]            NVARCHAR (10)   NULL,
    [Phone]                    NVARCHAR (100)  NULL,
    [Fax]                      NVARCHAR (100)  NULL,
    [EMail]                    NVARCHAR (100)  NULL,
    [Site]                     NVARCHAR (40)   NULL,
    [OrganizationId]           INT             NULL
);

