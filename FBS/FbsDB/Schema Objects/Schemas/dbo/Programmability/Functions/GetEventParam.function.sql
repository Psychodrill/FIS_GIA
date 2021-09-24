
-- =============================================
-- Возвращает значение параметра по его номеру, 
-- из строки с разделителем |.
-- v.1.0: Created by Sedov Anton 15.05.2008
-- Измение размера выходного массива.
-- =============================================
create function dbo.GetEventParam
	(
	@eventParams nvarchar(4000)
	, @index int
	)
returns nvarchar(4000) with schemabinding
as
begin
	declare 
		@delimiterIndex int
		, @startIndex int
		, @sourceParams nvarchar(4000)
		, @result nvarchar(4000)
	
	set @delimiterIndex = 1
	set @startIndex = 1
	set @sourceParams = Convert(nvarchar(4000), @eventParams) 
	set @delimiterIndex = charindex('|', isnull(@sourceParams, ''))
	
	if @delimiterIndex = 0
		return @sourceParams

	set @delimiterIndex = 1

	while @index <> 0
	begin
		set @startIndex = 1
		set @delimiterIndex = charindex('|', isnull(@sourceParams, ''))

		if @delimiterIndex = 0
			set @result = @sourceParams
		else
		begin
			set @result = substring(@sourceParams, @startIndex, @delimiterIndex - @startIndex)
			set @startIndex = @delimiterIndex
			set @sourceParams = substring(@sourceParams
					, @startIndex + 1, len(@sourceParams) - @startIndex) 
		end

		set @index = @index - 1
	end	
	
	return @result
end