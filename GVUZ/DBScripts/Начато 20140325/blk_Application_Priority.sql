USE [gvuz_tags]
GO

/****** Object:  Table [dbo].[blk_Application]    Script Date: 04/30/2014 11:13:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[blk_Application]') AND type in (N'U'))
DROP TABLE [dbo].[blk_Application]
GO

USE [gvuz_tags]
GO

/****** Object:  Table [dbo].[blk_Application]    Script Date: 04/30/2014 11:13:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[blk_Application](
	[EntrantUID] [varchar](200) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[ApplicationNumber] [varchar](50) NULL,
	[NeedHostel] [bit] NULL,
	[StatusId] [int] NOT NULL,
	[LastDenyDate] [datetime] NULL,
	[StatusDecision] [varchar](max) NULL,
	[IsRequiresBudgetO] [bit] NOT NULL,
	[IsRequiresBudgetOZ] [bit] NOT NULL,
	[IsRequiresBudgetZ] [bit] NOT NULL,
	[IsRequiresPaidO] [bit] NOT NULL,
	[IsRequiresPaidOZ] [bit] NOT NULL,
	[IsRequiresPaidZ] [bit] NOT NULL,
	[IsRequiresTargetO] [bit] NOT NULL,
	[IsRequiresTargetOZ] [bit] NOT NULL,
	[IsRequiresTargetZ] [bit] NOT NULL,
	[OriginalDocumentsReceived] [bit] NULL,
	[OriginalDocumentsReceivedDate] [datetime] NULL,
	[OrderOfAdmissionId] [int] NULL,
	[Priority] [int] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[UID] [varchar](200) NULL,
	[ImportPackageId] [int] NOT NULL,
	[InstitutionId] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


