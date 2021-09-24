-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (15, '015__2011_02_21__UpdateGroupRole')
-- =========================================================================



update GroupRole
set IsActiveCondition = 'dbo.GetUserStatus(account.ConfirmYear, account.Status, Year(GetDate()), account.RegistrationDocument) = '+'''activated'''
where RoleId in (39) and GroupId = 7
