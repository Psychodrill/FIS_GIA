-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (13, '013__2011_02_20__UpdateGroupRole')
-- =========================================================================




update GroupRole
set IsActiveCondition = 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated'''
where RoleId in (19, 37, 38) and GroupId = 6
