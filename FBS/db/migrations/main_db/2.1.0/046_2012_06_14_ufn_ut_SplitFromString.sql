-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (46, '046_2012_06_14_ufn_ut_SplitFromString')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_ut_SplitFromString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ufn_ut_SplitFromString]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[ufn_ut_SplitFromString] 
(
	@string nvarchar(max),
	@delimeter nvarchar(1) = ' '
)
returns @ret table (nam nvarchar(1000) )
as
begin
	if len(@string)=0 
		return 
	declare @s int, @e int
	set @s = 0
	while charindex(@delimeter,@string,@s) <> 0
	begin
		set @e = charindex(@delimeter,@string,@s)
		insert @ret values (rtrim(ltrim(substring(@string,@s,@e - @s))))
		set @s = @e + 1
	end
	insert @ret values (rtrim(ltrim(substring(@string,@s,300))))
	return
end
GO


