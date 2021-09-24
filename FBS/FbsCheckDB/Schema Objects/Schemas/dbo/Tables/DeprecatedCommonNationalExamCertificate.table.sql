CREATE TABLE [dbo].[DeprecatedCommonNationalExamCertificate] (
    [Id]                       BIGINT           NOT NULL,
    [CreateDate]               DATETIME         NOT NULL,
    [UpdateDate]               DATETIME         NOT NULL,
    [UpdateId]                 UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId]          BIGINT           NOT NULL,
    [EditorIp]                 NVARCHAR (255)   NOT NULL,
    [Number]                   NVARCHAR (255)   NOT NULL,
    [EducationInstitutionCode] NVARCHAR (255)   NOT NULL,
    [Year]                     INT              NOT NULL,
    [LastName]                 NVARCHAR (255)   NOT NULL,
    [FirstName]                NVARCHAR (255)   NOT NULL,
    [PatronymicName]           NVARCHAR (255)   NOT NULL,
    [Sex]                      BIT              NOT NULL,
    [Class]                    NVARCHAR (255)   NOT NULL,
    [InternalPassportSeria]    NVARCHAR (255)   NOT NULL,
    [PassportSeria]            NVARCHAR (255)   NOT NULL,
    [PassportNumber]           NVARCHAR (255)   NOT NULL,
    [EntrantNumber]            NVARCHAR (255)   NULL,
    [RegionId]                 INT              NOT NULL
);

