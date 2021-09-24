
-- =============================================
-- Получить короткое наименование организации.
-- v.1.0: Created by Fomin Dmitriy 06.05.2008
-- v.1.1: Modified by Fomin Dmitriy 08.05.2008
-- Поле наименования организации увеличено.
-- =============================================
CREATE function dbo.GetShortOrganizationName
	(
	@organizationName nvarchar(2000)
	)
returns nvarchar(2000)
as
begin
	-- Список сокращений.
	declare @word_abbreviation table
		(
		Word nvarchar(255)
		, Abbreviation nvarchar(255)
		)

	insert into @word_abbreviation values ('федеральный', 'Ф')
	insert into @word_abbreviation values ('республиканский', 'Р')
	insert into @word_abbreviation values ('областной', 'О')

	insert into @word_abbreviation values ('государственный', 'Г')
	insert into @word_abbreviation values ('негосударственный', 'НГ')
	insert into @word_abbreviation values ('образовательное', 'О')
	insert into @word_abbreviation values ('учреждение', 'У')
	insert into @word_abbreviation values ('высшего', 'В')
	insert into @word_abbreviation values ('среднего', 'С')
	insert into @word_abbreviation values ('профессионального', 'П')
	insert into @word_abbreviation values ('образования', 'О')

	insert into @word_abbreviation values ('университет', 'УНВ')
	insert into @word_abbreviation values ('академия', 'АКД')
	insert into @word_abbreviation values ('институт', 'ИНС')
	insert into @word_abbreviation values ('училище', 'УЧЛ')
	insert into @word_abbreviation values ('техникум', 'ТХК')
	insert into @word_abbreviation values ('колледж', 'КЛЖ')

	insert into @word_abbreviation values ('медицинский', 'МЕДИЦ')
	insert into @word_abbreviation values ('педагогический', 'ПЕДАГ')
	insert into @word_abbreviation values ('правосудия', 'ПРАВО')
	insert into @word_abbreviation values ('технический', 'ТЕХНЧ')
	insert into @word_abbreviation values ('технологический', 'ТЕХЛГ')
	insert into @word_abbreviation values ('политехнический', 'ПОЛТХ')
	insert into @word_abbreviation values ('юридический', 'ЮРИДЧ')
	insert into @word_abbreviation values ('текстильный', 'ТЕКСТ')
	insert into @word_abbreviation values ('сельскохозяйственная', 'СЕЛХЗ')
	insert into @word_abbreviation values ('машиностроительный', 'МАШСТ')
	insert into @word_abbreviation values ('приборостроительный', 'ПРБСТ')
	insert into @word_abbreviation values ('строительный', 'СТРОЙ')

	insert into @word_abbreviation values ('министерства', 'М')
	insert into @word_abbreviation values ('внутренних дел', 'ВД')
	insert into @word_abbreviation values ('юстиции', 'Ю')
	insert into @word_abbreviation values ('Российской Федерации', 'РФ')
	insert into @word_abbreviation values ('имени', 'им')

	-- Разбиение названия организации на слова.
	declare @organization_word table
		(
		Word nvarchar(255)
		)

	declare
		@startIndex int
		, @delimiterIndex int
		, @value nvarchar(4000)

	set @startIndex = 1
	set @delimiterIndex = charindex(' ', isnull(@organizationName, ''))
	while @delimiterIndex > 0
	begin
		set @value = ltrim(rtrim(substring(@organizationName, @startIndex, @delimiterIndex  - @startIndex)))
		if @value <> ''
			insert into @organization_word
			select @value
	
		set @startIndex = @delimiterIndex + 1
		set @delimiterIndex = charindex(' ', @organizationName, @startIndex)
	end

	if len(@organizationName) >= @startIndex 
	begin
		set @value = ltrim(rtrim(substring(@organizationName, @startIndex, len(@organizationName) - @startIndex + 1)))
		if @value <> ''
				insert into @organization_word
				select @value
	end

	-- Вывод результата.
	declare
		@word nvarchar(255)
		, @result nvarchar(2000)
		, @sameLevel decimal(18, 4)
		, @matchCount int

	set @sameLevel = 0.7
	set @matchCount = 3

	set @result = ''

	declare abbreviation_cursor cursor for
	select
		-- Вывести наилучшее сокращение, если такое есть.
		isnull((select top 1 word_abbreviation.Abbreviation 
				from (select word_abbreviation.Abbreviation 
							, dbo.CompareStrings(organization_word.Word, word_abbreviation.Word, @matchCount) SameLevel
						from @word_abbreviation word_abbreviation) word_abbreviation
				where word_abbreviation.SameLevel >= @sameLevel
				order by word_abbreviation.SameLevel desc), organization_word.Word)
	from @organization_word organization_word

	open abbreviation_cursor 
	fetch next from abbreviation_cursor into @word
	while @@fetch_status = 0
	begin
		if len(@result) > 0
			set @result = @result + ' '
		set @result = @result + @word

		fetch next from abbreviation_cursor into @word
	end
	close abbreviation_cursor
	deallocate abbreviation_cursor

	return @result
end
