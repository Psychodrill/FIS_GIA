/****** Object:  Table [dbo].[blk_ApplicationIndividualAchievements]    Script Date: 05/30/2014 16:46:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

if exists (select * from sysobjects where id = OBJECT_ID(N'blk_ApplicationIndividualAchievements'))
	DROP TABLE [dbo].[blk_ApplicationIndividualAchievements]

if not exists (select * from sysobjects where id = OBJECT_ID(N'blk_ApplicationIndividualAchievements'))
begin
	CREATE TABLE [dbo].[blk_ApplicationIndividualAchievements](
		[ApplicationUID] [varchar](50) NOT NULL,
		[IAUID] [varchar](50) NULL,
		[IAName] [varchar](100) NOT NULL,
		[IAMark] decimal(7,4) NULL,
		[EntrantDocumentUID] [varchar](50) NOT NULL,
		[Id] [uniqueidentifier] NOT NULL,
		[ParentId] [uniqueidentifier] NULL,
		[UID] [varchar](200) NULL,
		[ImportPackageId] [int] NOT NULL,
		[InstitutionId] [int] NOT NULL)
end