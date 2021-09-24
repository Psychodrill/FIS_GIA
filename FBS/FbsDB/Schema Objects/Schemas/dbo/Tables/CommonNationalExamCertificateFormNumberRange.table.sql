CREATE TABLE [dbo].[CommonNationalExamCertificateFormNumberRange] (
    [Id]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME         NOT NULL,
    [UpdateDate] DATETIME         NOT NULL,
    [UpdateId]   UNIQUEIDENTIFIER NOT NULL,
    [Year]       INT              NOT NULL,
    [RegionId]   INT              NOT NULL,
    [NumberFrom] NVARCHAR (255)   NOT NULL,
    [NumberTo]   NVARCHAR (255)   NOT NULL
);

