-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (7, '007__2011_01_25__OrganizationKind')
-- =========================================================================




IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND  A.TABLE_NAME = 'Organization2010' AND A.COLUMN_NAME = 'KindId'))
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [FK__Organizat__KindI__29BD046F]
END
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND  A.TABLE_NAME = 'OrganizationRequest2010' AND A.COLUMN_NAME = 'KindId'))
BEGIN
	ALTER TABLE [dbo].[OrganizationRequest2010] DROP [FK__Organizat__KindI__306A01FE]
END
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'OrganizationType2010'))
  DROP TABLE [dbo].[OrganizationKind]
GO
	
CREATE TABLE [dbo].[OrganizationKind] (
  [Id] int IDENTITY(1, 1) NOT NULL,
  [Name] nvarchar(30) NOT NULL,
  [SortOrder] int NOT NULL, --Поле добавлено для того, что бы выводить отстортированный список в интерфейсе и не менять ключи полей
  PRIMARY KEY CLUSTERED ([Id])
)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Институт', 1)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Классический университет', 2)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Технический университет', 3)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Профильный университет', 4)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Академия', 5)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Колледж', 6)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Техникум', 7)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Другое', 9)
GO

INSERT INTO [dbo].[OrganizationKind] (Name, SortOrder)
VALUES ('Министерство и ведомство', 8)
GO

ALTER TABLE [dbo].[Organization2010]
ADD CONSTRAINT [FK__Organizat__KindI__29BD046F] FOREIGN KEY ([KindId]) 
  REFERENCES [dbo].[OrganizationKind] ([Id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[OrganizationRequest2010]
ADD CONSTRAINT [FK__Organizat__KindI__306A01FE] FOREIGN KEY ([KindId]) 
  REFERENCES [dbo].[OrganizationKind] ([Id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO