-- exec dbo.SearchNews

-- =============================================
-- Получение списка новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим название новости.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Название новости добавлено в фильтр.
-- =============================================
CREATE proc dbo.SearchNews
	@isActive bit = null
	, @dateFrom datetime = null
	, @dateTo datetime = null
	, @name nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = null
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @nameFormat nvarchar(255)

	if isnull(@name, '') <> ''
		set @nameFormat = '%' + replace(@name, ' ' , '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, Date datetime ' +
			'	, Description ntext ' +
			'	, Name nvarchar(255) ' +
			'	, IsActive bit ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	news.Id Id ' +
				'	, news.Date Date ' +
				'	, news.Description Description ' +
				'	, news.Name Name ' +
				'	, news.IsActive IsActive ' +
				'from dbo.News news with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.News news with (nolock) ' +
				'where 1 = 1 ' 
	
	if not @isActive is null
		set @commandText = @commandText + ' and news.IsActive = @isActive '

	if not @dateFrom is null
		set @commandText = @commandText + ' and news.Date >= @dateFrom '

	if not @dateTo is null
		set @commandText = @commandText + ' and news.Date <= @dateTo '

	if not @name is null
		set @commandText = @commandText + ' and news.Name like @nameFormat '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = N'Name'
		begin
			set @innerOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = N'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
		end
		else 
		begin
			set @innerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection>, Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
			set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
			set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
		end
		else
		begin
			set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
			set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
			set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	dbo.GetExternalId(search.Id) Id ' +
			'	, search.Date ' +
			'	, search.Description ' +
			'	, search.Name ' +
			'	, search.IsActive ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@isActive bit, @dateFrom datetime, @dateTo datetime, @nameFormat nvarchar(255)'
		, @IsActive
		, @dateFrom
		, @dateTo
		, @nameFormat

	return 0
end
