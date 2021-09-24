-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (66, '066_2012_07_06_isbanned.sql')
-- =========================================================================

GO
alter function dbo.IsUserBanned
	(
	@login nvarchar(255)
	)
returns bit 
as  
begin
declare @result bit
set @result = 0
select top 1 @result = IsBanned from Account where [Login] = @login
return @result
	
end

