-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (14, '014__2011_02_21__EditSelfOrganization')
-- =========================================================================




delete from dbo.GroupRole where RoleId = 40
delete from dbo.Role where Id = 40

SET IDENTITY_INSERT dbo.Role ON
GO

insert into dbo.Role (Id, Code, Name) values (40, 'EditSelfOrganization', 'Редактирование собственной организации')

SET IDENTITY_INSERT dbo.Role OFF
GO


insert into dbo.GroupRole (RoleId, GroupId, IsActive, IsActiveCondition)
values (40, 1, 1, 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated''')