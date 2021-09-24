﻿CREATE TABLE [dbo].[OrganizationUpdateHistory] (
    [Id]                       INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [OriginalOrgId]            INT             NULL,
    [UpdateDescription]        NVARCHAR (MAX)  NULL,
    [Version]                  INT             NULL,
    [UpdateDate]               DATETIME        NULL,
    [FullName]                 NVARCHAR (1000) NULL,
    [ShortName]                NVARCHAR (500)  NULL,
    [RegionId]                 INT             NULL,
    [TypeId]                   INT             NULL,
    [KindId]                   INT             NULL,
    [INN]                      NVARCHAR (10)   NULL,
    [OGRN]                     NVARCHAR (13)   NULL,
    [OwnerDepartment]          NVARCHAR (500)  NULL,
    [IsPrivate]                BIT             NOT NULL,
    [IsFilial]                 BIT             NOT NULL,
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
    [RCModel]                  INT             NULL,
    [RCDescription]            NVARCHAR (400)  NULL,
    [MainId]                   INT             NULL,
    [DepartmentId]             INT             NULL,
    [CNFBFullTime]             INT             NULL,
    [CNFBEvening]              INT             NULL,
    [CNFBPostal]               INT             NULL,
    [CNPayFullTime]            INT             NULL,
    [CNPayEvening]             INT             NULL,
    [CNPayPostal]              INT             NULL,
    [CNFederalBudget]          INT             NULL,
    [CNTargeted]               INT             NULL,
    [CNLocalBudget]            INT             NULL,
    [CNPaying]                 INT             NULL,
    [CNFullTime]               INT             NULL,
    [CNEvening]                INT             NULL,
    [CNPostal]                 INT             NULL,
    [NewOrgId]                 INT             NULL,
    [StatusId]                 INT             NULL,
    [EditorUserName]           NVARCHAR (50)   NULL,
    [DisableLog]			   BIT DEFAULT ((0)) NOT NULL -- отключить журналирование, по умолчанию 0, т.е. включено
);
