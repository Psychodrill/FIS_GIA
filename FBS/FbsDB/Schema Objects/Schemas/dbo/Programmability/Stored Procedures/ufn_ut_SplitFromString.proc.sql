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
