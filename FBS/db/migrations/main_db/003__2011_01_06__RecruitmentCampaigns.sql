-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (3, '003__2011_01_06__RecruitmentCampaigns')
-- =========================================================================


-- Скрипт многопроходный, поэтому удаляем все что можно

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'RCModel')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [org2010_col_model]
	ALTER TABLE [dbo].[Organization2010] DROP [Organization2010_fk]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [RCModel]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'Organization2010' AND 

COLUMN_NAME = 'RCDescription')
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [RCDescription]
END
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND  B.TABLE_NAME = 'RecruitmentCampaigns'))
  DROP TABLE [dbo].[RecruitmentCampaigns]
GO




-- Справочник моделей приемных кампаний

CREATE TABLE [dbo].[RecruitmentCampaigns] (
  [Id] int NOT NULL,
  [ModelName] nvarchar(400) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO




-- Заполняем справочник моделей приемных кампаний

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (1, 'ОУ не имеет филиалов и самостоятельных факультетов и осуществляет проверки свидетельств самостоятельно')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (2, 'ОУ имеет филиалы и самостоятельные факультеты и осуществляет проверки свидетельств централизованно, в том числе за свои 

факультеты и филиалы')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (3, 'ОУ имеет филиалы и осуществляет проверки централизованно, в том числе за свои филиалы')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (4, 'ОУ имеет несколько самостоятельных факультетов, каждый из которых осуществляет проверку свидетельств самостоятельно')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (5, 'ОУ является филиалом и осуществляет проверку свидетельств самостоятельно')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (999, 'другая модель приема')
GO




-- Добавляем в таблицу организаций ссылки на справочник приемных кампаний

ALTER TABLE [dbo].[Organization2010]
  ADD [RCModel] int NOT NULL CONSTRAINT [org2010_col_model] DEFAULT 1,
  [RCDescription] nvarchar(400) NULL
GO

ALTER TABLE [dbo].[Organization2010]
ADD CONSTRAINT [Organization2010_fk] FOREIGN KEY ([RCModel]) 
  REFERENCES [dbo].[RecruitmentCampaigns] ([Id]) 
  ON UPDATE CASCADE
  ON DELETE NO ACTION
GO