-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (6, '006__2011_01_25__OrganizationType')
-- =========================================================================





IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND  A.TABLE_NAME = 'Organization2010' AND A.COLUMN_NAME = 'TypeId'))
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [FK__Organizat__TypeI__28C8E036]
END
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND  A.TABLE_NAME = 'OrganizationRequest2010' AND A.COLUMN_NAME = 'TypeId'))
BEGIN
	ALTER TABLE [dbo].[OrganizationRequest2010] DROP [FK__Organizat__TypeI__2F75DDC5]
END
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'OrganizationType2010'))
  DROP TABLE [dbo].[OrganizationType2010]
GO
	
CREATE TABLE [dbo].[OrganizationType2010] (
  [Id] int IDENTITY(1, 1) NOT NULL,
  [Name] nvarchar(30) NOT NULL,
  [SortOrder] int NOT NULL, --Поле добавлено для того, что бы выводить отстортированный список в интерфейсе и не менять ключи полей
  PRIMARY KEY CLUSTERED ([Id])
)
GO

INSERT INTO [dbo].[OrganizationType2010] (Name, SortOrder)
VALUES ('ВУЗ', 1)
GO

INSERT INTO [dbo].[OrganizationType2010] (Name, SortOrder)
VALUES ('ССУЗ', 2)
GO

INSERT INTO [dbo].[OrganizationType2010] (Name, SortOrder)
VALUES ('РЦОИ', 3)
GO

INSERT INTO [dbo].[OrganizationType2010] (Name, SortOrder)
VALUES ('Орган управления образованием', 4)
GO

INSERT INTO [dbo].[OrganizationType2010] (Name, SortOrder)
VALUES ('Другое', 6)
GO

INSERT INTO [dbo].[OrganizationType2010] (Name, SortOrder)
VALUES ('Учредитель', 5)
GO

ALTER TABLE [dbo].[Organization2010]
ADD CONSTRAINT [FK__Organizat__TypeI__28C8E036] FOREIGN KEY ([TypeId]) 
  REFERENCES [dbo].[OrganizationType2010] ([Id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO

ALTER TABLE [dbo].[OrganizationRequest2010]
ADD CONSTRAINT [FK__Organizat__TypeI__2F75DDC5] FOREIGN KEY ([TypeId]) 
  REFERENCES [dbo].[OrganizationType2010] ([Id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO