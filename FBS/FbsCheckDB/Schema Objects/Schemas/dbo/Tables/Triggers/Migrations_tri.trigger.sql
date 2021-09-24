/*
CREATE TRIGGER [dbo].[Migrations_tri] ON [dbo].[Migrations]
WITH EXECUTE AS CALLER
INSTEAD OF INSERT
AS
BEGIN

	-- 1. Объявляем переменные

	-- Текущая версия базы (последняя отработанная миграция)
	declare @current_version int
    set @current_version = ISNULL((select max(M.MigrationVersion) from Migrations M), -1)
    
	-- Версия миграции, которую пытаемся отработать
	declare @new_version int
    set @new_version = ISNULL((select max(M.MigrationVersion) from inserted M), 0)
    
    -- 2. Если версия новой миграции равна версию БД или больше на единицу, то 
    --    отрабатываем ее, иначе выдаем ошибку
    if (@new_version = @current_version or @new_version - 1 = @current_version or @current_version = -1)
    begin
		insert into Migrations (MigrationVersion, MigrationName)
		select 
			MigrationVersion, 
            MigrationName
		from 
			inserted
    end
    else begin
		raiserror
			(N'Текущая версия БД: %d, попытка отработать миграцию с версией: %d',
			10, -- Severity.
			1, -- State.
			@current_version, -- текущая версия БД.
			@new_version); -- версия миграции, которую пытаемся отработать
	end
END*/
