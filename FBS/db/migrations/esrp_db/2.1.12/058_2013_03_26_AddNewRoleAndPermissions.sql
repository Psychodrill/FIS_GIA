-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (58, '058_2013_03_26_AddNewRoleAndPermissions')
go

/****** Object:  Table [dbo].[Role]    Script Date: 03/26/2013 17:05:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET IDENTITY_INSERT [dbo].[Role] ON
INSERT [dbo].[Role] ([Id], [Code], [Name]) VALUES (50, N'EditOURCModel', N'Выбор модели приемной комиссии')
INSERT [dbo].[Role] ([Id], [Code], [Name]) VALUES (51, N'Organizations', N'Организации')
SET IDENTITY_INSERT [dbo].[Role] OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Table [dbo].[GroupRole]    Script Date: 03/26/2013 17:05:23 ******/
SET IDENTITY_INSERT [dbo].[GroupRole] ON
INSERT [dbo].[GroupRole] ([Id], [RoleId], [GroupId], [IsActive], [IsActiveCondition]) VALUES (190, 50, 3, 1, NULL)
INSERT [dbo].[GroupRole] ([Id], [RoleId], [GroupId], [IsActive], [IsActiveCondition]) VALUES (191, 51, 2, 1, NULL)
INSERT [dbo].[GroupRole] ([Id], [RoleId], [GroupId], [IsActive], [IsActiveCondition]) VALUES (192, 51, 1, 1, NULL)
SET IDENTITY_INSERT [dbo].[GroupRole] OFF
