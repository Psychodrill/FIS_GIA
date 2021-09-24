-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (3, '003__2011_01_06__RecruitmentCampaigns')
-- =========================================================================


-- ������ ��������������, ������� ������� ��� ��� �����

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




-- ���������� ������� �������� ��������

CREATE TABLE [dbo].[RecruitmentCampaigns] (
  [Id] int NOT NULL,
  [ModelName] nvarchar(400) NOT NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
GO




-- ��������� ���������� ������� �������� ��������

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (1, '�� �� ����� �������� � ��������������� ����������� � ������������ �������� ������������ ��������������')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (2, '�� ����� ������� � ��������������� ���������� � ������������ �������� ������������ ���������������, � ��� ����� �� ���� 

���������� � �������')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (3, '�� ����� ������� � ������������ �������� ���������������, � ��� ����� �� ���� �������')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (4, '�� ����� ��������� ��������������� �����������, ������ �� ������� ������������ �������� ������������ ��������������')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (5, '�� �������� �������� � ������������ �������� ������������ ��������������')
GO

insert into [dbo].[RecruitmentCampaigns] (Id, ModelName)
values (999, '������ ������ ������')
GO




-- ��������� � ������� ����������� ������ �� ���������� �������� ��������

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