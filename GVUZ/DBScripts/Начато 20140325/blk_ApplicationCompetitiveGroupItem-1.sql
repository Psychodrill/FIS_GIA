USE [gvuz_tags]
GO

/****** Object:  Table [dbo].[blk_ApplicationCompetitiveGroupItem]    Script Date: 04/29/2014 13:21:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_ApplicationCompetitiveGroupItem]') AND type in (N'U'))
DROP TABLE [dbo].[blk_ApplicationCompetitiveGroupItem]
GO

USE [gvuz_tags]
GO

/****** Object:  Table [dbo].[blk_ApplicationCompetitiveGroupItem]    Script Date: 04/29/2014 13:21:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[blk_ApplicationCompetitiveGroupItem](
	[ApplicationUID] [varchar](200) NOT NULL,
	[CompetitivegroupUID] [varchar](200) NULL,
	[CompetitiveGroupItemUID] [varchar](200) NULL,
	[EducationForm] [int] NOT NULL,
	[EducationSource] [int] NOT NULL,
	[Priority] [int] NULL,
	[CompetitiveGroupTargetUID] [nvarchar](200) NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[UID] [varchar](200) NULL,
	[ImportPackageId] [int] NOT NULL,
	[InstitutionId] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


