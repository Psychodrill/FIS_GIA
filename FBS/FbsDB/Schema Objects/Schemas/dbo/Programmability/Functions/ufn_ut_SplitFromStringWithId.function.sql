CREATE FUNCTION [dbo].[ufn_ut_SplitFromStringWithId]
(
	@string nvarchar(max),
	@delimeter nvarchar(1) = ' ')
RETURNS @ret TABLE (id int identity(1,1) ,val nvarchar(4000) )
AS
BEGIN
	DECLARE @s int, @e int

	SET @s = 0
	WHILE CHARINDEX(@delimeter,@string,@s) <> 0
	BEGIN
		SET @e = CHARINDEX(@delimeter,@string,@s)
		INSERT @ret VALUES (SUBSTRING(@string,@s,@e - @s))
		SET @s = @e + 1
	END
	INSERT @ret VALUES (SUBSTRING(@string,@s,4000))
	RETURN
END