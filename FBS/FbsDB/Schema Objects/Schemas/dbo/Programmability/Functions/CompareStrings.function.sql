
-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Длина сравниваемых строк увеличена.
-- =============================================
CREATE function dbo.CompareStrings
	(
	@string1 nvarchar(4000)
	, @string2 nvarchar(4000)
	-- Чувствительность: кол-во совпадающих символов подстроки.
	, @matchCount int 
	)
returns decimal(18, 4)
as
begin
	declare
		@compareStr1 nvarchar(4000)
		, @compareStr2 nvarchar(4000)
		, @i int
		, @j int
		, @count1 int
		, @count int

	set @matchCount = isnull(@matchCount, 3)
	set @compareStr1 = replace(isnull(@string1, ''), ' ', '')
	set @compareStr2 = replace(isnull(@string2, ''), ' ', '')
	set @count = 0

	if @compareStr1 = @compareStr2
		return 1

	if len(@compareStr1) = 0 or len(@compareStr2) = 0
		return 0

	set @i = 1
	while @i < len(@compareStr1)
	begin
		set @j = 1
		while @j < len(@compareStr2)	
		begin
			if substring(@compareStr1, @i, 1) = substring(@compareStr2, @j, 1)
			begin
				set @count1 = 1
				while (@i + @count1 <= len(@compareStr1)) and (@j + @count1 <= len(@compareStr2))
						and (substring(@compareStr1, @i + @count1, 1) = substring(@compareStr2, @j + @count1, 1))
					set @count1 = @count1 + 1
				set @i = @i + @count1 - 1
				set @j = @j + @count1 - 1
				if @count1 >= @matchCount
					set @count = @count + @count1
			end
			set @j = @j + 1
		end
		set @i = @i + 1
	end
	
	if len(@compareStr1) > len(@compareStr2)
		return cast(@count as decimal(18, 4)) / cast(len(@compareStr1) as decimal(18, 4))
	else
		return cast(@count as decimal(18, 4)) / cast(len(@compareStr2) as decimal(18, 4))

	return 0
end
