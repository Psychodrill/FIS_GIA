-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (57, '057_2013_03_26_DirectorPosition')
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectorPosition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PositionName] [nvarchar](255) NOT NULL,
	[PositionNameInGenetive] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DirectorPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[DirectorPosition] ON
INSERT [dbo].[DirectorPosition] ([Id], [PositionName], [PositionNameInGenetive]) VALUES (1, N'Ректор', N'Ректору')
INSERT [dbo].[DirectorPosition] ([Id], [PositionName], [PositionNameInGenetive]) VALUES (2, N'Директор', N'Директору')
SET IDENTITY_INSERT [dbo].[DirectorPosition] OFF
