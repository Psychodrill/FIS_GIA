-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (40, '040__2011_11_18__AlterOrganizationCreateHistory')
-- =========================================================================
GO

-- справочник статусов организации
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrganizationOperatingStatus](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](30) NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO dbo.[OrganizationOperatingStatus]
        ( [Name] )
VALUES  ( 'Действующая'  -- Name - nvarchar(30)
          )
GO

INSERT INTO dbo.[OrganizationOperatingStatus]
        ( [Name] )
VALUES  ( 'Реорганизованная'  -- Name - nvarchar(30)
          )
go

INSERT INTO dbo.[OrganizationOperatingStatus]
        ( [Name] )
VALUES  ( 'Ликвидированная'  -- Name - nvarchar(30)
          )
GO

ALTER TABLE dbo.Organization2010 ADD [StatusId] INT NOT NULL DEFAULT 1  
GO

ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK_Organization2010_OrganizationOperatingStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrganizationOperatingStatus] ([Id])
GO


ALTER TABLE dbo.Organization2010 ADD [NewOrgId] INT NULL  
GO

ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK_Organization2010_Organization2010] FOREIGN KEY([NewOrgId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK_Organization2010_Organization2010]
GO

ALTER TABLE [dbo].[Organization2010] ADD [Version] INT NOT NULL DEFAULT 1
GO

-- создание таблицы инстории изменений

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_KindI_29BD046F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_KindI_29BD046F]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_mainid]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_mainid]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_Organization2010]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_Organization2010]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_OrganizationOperatingStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_OrganizationOperatingStatus]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_OrzanizationDepartmentId]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_OrzanizationDepartmentId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_RecruitmentCampaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_Regio_27D4BBFD]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_Regio_27D4BBFD]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationUpdateHistory_TypeI_28C8E036]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]'))
ALTER TABLE [dbo].[OrganizationUpdateHistory] DROP CONSTRAINT [FK_OrganizationUpdateHistory_TypeI_28C8E036]
GO

/****** Object:  Table [dbo].[OrganizationUpdateHistory]    Script Date: 11/23/2011 16:42:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationUpdateHistory]') AND type in (N'U'))
DROP TABLE [dbo].[OrganizationUpdateHistory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrganizationUpdateHistory](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OriginalOrgId] [int] NULL,
	[UpdateDescription] NVARCHAR(MAX) NULL,
	[Version] int NULL,
	[UpdateDate] [datetime] NULL,
	[FullName] [nvarchar](1000) NULL,
	[ShortName] [nvarchar](500) NULL,
	[RegionId] [int] NULL,
	[TypeId] [int] NULL,
	[KindId] [int] NULL,
	[INN] [nvarchar](10) NULL,
	[OGRN] [nvarchar](13) NULL,
	[OwnerDepartment] [nvarchar](500) NULL,
	[IsPrivate] [bit] NOT NULL,
	[IsFilial] [bit] NOT NULL,
	[DirectorPosition] [nvarchar](255) NULL,
	[DirectorFullName] [nvarchar](255) NULL,
	[IsAccredited] [bit] NULL,
	[AccreditationSertificate] [nvarchar](255) NULL,
	[LawAddress] [nvarchar](255) NULL,
	[FactAddress] [nvarchar](255) NULL,
	[PhoneCityCode] [nvarchar](10) NULL,
	[Phone] [nvarchar](100) NULL,
	[Fax] [nvarchar](100) NULL,
	[EMail] [nvarchar](100) NULL,
	[Site] [nvarchar](40) NULL,
	[RCModel] [int] NULL,
	[RCDescription] [nvarchar](400) NULL,
	[MainId] [int] NULL,
	[DepartmentId] [int] NULL,
	[CNFBFullTime] [int] NULL,
	[CNFBEvening] [int] NULL,
	[CNFBPostal] [int] NULL,
	[CNPayFullTime] [int] NULL,
	[CNPayEvening] [int] NULL,
	[CNPayPostal] [int] NULL,
	[CNFederalBudget] [int] NULL,
	[CNTargeted] [int] NULL,
	[CNLocalBudget] [int] NULL,
	[CNPaying] [int] NULL,
	[CNFullTime] [int] NULL,
	[CNEvening] [int] NULL,
	[CNPostal] [int] NULL,
	[NewOrgId] [int] NULL,
	[StatusId] [int] NULL,
 CONSTRAINT [PK__OrganizationUpdateHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_KindI_29BD046F] FOREIGN KEY([KindId])
REFERENCES [dbo].[OrganizationKind] ([Id])
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_KindI_29BD046F]
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD CONSTRAINT [FK_OrganizationUpdateHistory_Regio_27D4BBFD] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_Regio_27D4BBFD]
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_TypeI_28C8E036] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrganizationType2010] ([Id])
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_TypeI_28C8E036]
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_OrzanizationDepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_OrzanizationDepartmentId]
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns] FOREIGN KEY([RCModel])
REFERENCES [dbo].[RecruitmentCampaigns] ([Id])
ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns]
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_mainid] FOREIGN KEY([MainId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_mainid]
GO

GO
ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_Organization2010] FOREIGN KEY([NewOrgId])
REFERENCES [dbo].[Organization2010] ([Id])
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT [FK_OrganizationUpdateHistory_Organization2010]
GO

ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_OrganizationOperatingStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrganizationOperatingStatus] ([Id])
GO
