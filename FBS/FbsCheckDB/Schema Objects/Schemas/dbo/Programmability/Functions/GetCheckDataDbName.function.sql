



--------------------------------------------------
-- Получение имени БД 
-- v.1.0: Created by Sedov Anton 03.06.2008
-- v.1.1: Modified by Fomin Dmitriy 08.06.2008
-- Приведение к стандарту.
--------------------------------------------------
CREATE function [dbo].[GetCheckDataDbName]
	(
	)
returns nvarchar(255)
as
begin
	declare 
		@dbName nvarchar(255)
		, @dataDbName nvarchar(255)
		, @stateRestoring int
	
	set @dataDbName = 'fbs'
	set @dbName = DB_Name()
	set @stateRestoring = 1

	select 
		@dbName = [database].[name]
	from sys.databases [database]
	where [database].[name] collate cyrillic_general_ci_ai = @dataDbName collate cyrillic_general_ci_ai
		and not [database].state = @stateRestoring  
	
	return @dbName
end



