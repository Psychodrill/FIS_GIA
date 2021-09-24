/****** Object:  Table [dbo].[Migrations]    Script Date: 07/23/2012 12:03:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Migrations](
	[MigrationVersion] [int] NOT NULL,
	[MigrationName] [varchar](200) NOT NULL,
	[CreateDate] [datetime] NOT NULL DEFAULT CURRENT_TIMESTAMP
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[Migrations] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO

