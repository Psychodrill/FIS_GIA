CREATE TABLE [dbo].[EntrantRenunciation] (
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
    [PassportNumber]      NVARCHAR (255)   NOT NULL,
    [PassportSeria]       NVARCHAR (10)    NOT NULL
);

