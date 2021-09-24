-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (59, '059_2013_03_26_AddFieldsForDirectorPosition')
go

ALTER TABLE [dbo].[Organization2010] ADD
	[DirectorPositionInGenetive] [nvarchar](255) NULL,
	[DirectorFullNameInGenetive] [nvarchar](255) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL
	
ALTER TABLE [dbo].[OrganizationUpdateHistory] ADD
	[DirectorPositionInGenetive] [nvarchar](255) NULL,
	[DirectorFullNameInGenetive] [nvarchar](255) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL
	
ALTER TABLE [dbo].[OrganizationRequest2010] ADD
	[DirectorPositionInGenetive] [nvarchar](255) NULL,
	[DirectorFullNameInGenetive] [nvarchar](255) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL

GO

/****** Object:  Default [DF_Organization2010_OUConfirmation]    Script Date: 03/26/2013 17:16:02 ******/
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [DF_Organization2010_OUConfirmation]  DEFAULT ((0)) FOR [OUConfirmation]
GO

/****** Object:  Default [DF_OrganizationUpdateHistory_OUConfirmation]    Script Date: 03/26/2013 17:16:02 ******/
ALTER TABLE [dbo].[OrganizationUpdateHistory] ADD  CONSTRAINT [DF_OrganizationUpdateHistory_OUConfirmation]  DEFAULT ((0)) FOR [OUConfirmation]
GO

/****** Object:  Default [DF_OrganizationRequest2010_OUConfirmation]    Script Date: 03/26/2013 17:16:02 ******/
ALTER TABLE [dbo].[OrganizationRequest2010] ADD  CONSTRAINT [DF_OrganizationRequest2010_OUConfirmation]  DEFAULT ((0)) FOR [OUConfirmation]
GO
