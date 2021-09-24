-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (6, '006_2012_04_28__RecreateAndFillRecruitmentCampaigns.sql')
-- =========================================================================
ALTER TABLE [dbo].[Organization2010] NOCHECK CONSTRAINT Organization2010_fk;
ALTER TABLE [dbo].[OrganizationRequest2010] NOCHECK CONSTRAINT FK_OrganizationRequest2010_RecruitmentCampaigns;
ALTER TABLE [dbo].[OrganizationUpdateHistory] NOCHECK CONSTRAINT FK_OrganizationUpdateHistory_RecruitmentCampaigns;
DELETE FROM [dbo].[RecruitmentCampaigns]
GO
INSERT INTO [dbo].[RecruitmentCampaigns]
           ([Id]
           ,[ModelName])
     VALUES
           (1
           ,'�� �� ����� �������� � ��������������� ����������� � ������������ �������� ������������ ��������������')
GO
INSERT INTO [dbo].[RecruitmentCampaigns]
           ([Id]
           ,[ModelName])
     VALUES
           (2
           ,'�� ����� ������� � ��������������� ���������� � ������������ �������� ������������ ���������������, � ��� ����� �� ���� ���������� � �������')
GO
INSERT INTO [dbo].[RecruitmentCampaigns]
           ([Id]
           ,[ModelName])
     VALUES
           (3
           ,'�� ����� ������� � ������������ �������� ���������������, � ��� ����� �� ���� �������')
GO
INSERT INTO [dbo].[RecruitmentCampaigns]
           ([Id]
           ,[ModelName])
     VALUES
           (4
           ,'�� ����� ��������� ��������������� �����������, ������ �� ������� ������������ �������� ������������ ��������������')
GO
INSERT INTO [dbo].[RecruitmentCampaigns]
           ([Id]
           ,[ModelName])
     VALUES
           (5
           ,'�� �������� �������� � ������������ �������� ������������ ��������������')
GO
INSERT INTO [dbo].[RecruitmentCampaigns]
           ([Id]
           ,[ModelName])
     VALUES
           (999
           ,'������ ������ ������')
GO
UPDATE [dbo].[Organization2010]
   SET [RCModel] = 1
   WHERE [RCModel] <> 999
GO
UPDATE [dbo].[OrganizationRequest2010]
   SET [RCModelID] = 1
   WHERE [RCModelID] <> 999
GO
ALTER TABLE [fbs].[dbo].[Organization2010] CHECK CONSTRAINT Organization2010_fk;
ALTER TABLE [dbo].[OrganizationUpdateHistory] CHECK CONSTRAINT FK_OrganizationUpdateHistory_RecruitmentCampaigns;
ALTER TABLE [dbo].[OrganizationRequest2010] CHECK CONSTRAINT FK_OrganizationRequest2010_RecruitmentCampaigns;

