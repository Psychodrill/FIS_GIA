-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (7, '007_2012_04_28_UpdateOrganizationOperatingStatus.sql')
-- =========================================================================
delete from [dbo].[OrganizationOperatingStatus] where [id]=1
delete from [dbo].[OrganizationOperatingStatus] where [id]=2
delete from [dbo].[OrganizationOperatingStatus] where [id]=3

SET IDENTITY_INSERT [dbo].[OrganizationOperatingStatus] ON
INSERT INTO [dbo].[OrganizationOperatingStatus] ([id],[Name]) values (1,'�����������')
INSERT INTO [dbo].[OrganizationOperatingStatus] ([id],[Name]) values (2,'����������������')
INSERT INTO [dbo].[OrganizationOperatingStatus] ([id],[Name]) values (3,'���������������')
SET IDENTITY_INSERT [dbo].[OrganizationOperatingStatus] OFF