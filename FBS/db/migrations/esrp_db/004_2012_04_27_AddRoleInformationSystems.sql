-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (4, '004_2012_04_27_AddRoleInformationSystems')
-- =========================================================================

INSERT INTO [dbo].[Role]
           ([Code]
           ,[Name])
     VALUES
           ('InformationSystems'
           ,'Информационные системы')
GO

INSERT INTO [dbo].[GroupRole]
           ([RoleId]
           ,[GroupId]
           ,[IsActive]
           ,[IsActiveCondition])
     VALUES
           ((SELECT Id FROM [Role] WHERE [Code]='InformationSystems')
           ,1
           ,1
           ,NULL)
GO