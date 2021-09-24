CREATE TABLE [dbo].[CompetitionCertificate] (
    [Id]                BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]        DATETIME         NOT NULL,
    [UpdateDate]        DATETIME         NOT NULL,
    [UpdateId]          UNIQUEIDENTIFIER NOT NULL,
    [EditorAccountId]   BIGINT           NULL,
    [EditorIp]          NVARCHAR (255)   NOT NULL,
    [CompetitionTypeId] INT              NOT NULL,
    [Year]              INT              NOT NULL,
    [LastName]          NVARCHAR (255)   NOT NULL,
    [FirstName]         NVARCHAR (255)   NOT NULL,
    [PatronymicName]    NVARCHAR (255)   NOT NULL,
    [Degree]            NVARCHAR (255)   NOT NULL,
    [RegionId]          INT              NOT NULL,
    [City]              NVARCHAR (255)   NOT NULL,
    [School]            NVARCHAR (255)   NOT NULL,
    [Class]             NVARCHAR (255)   NOT NULL
);

