-- exec dbo.SearchDocument

-- =============================================
-- Получение списка документов.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- v.1.1: Modified by Fomin Dmitriy 18.04.2008
-- Добавлена фильтрация по наименованию.
-- v.1.2: Modified by Makarev Andrey 19.04.2008
-- Правильный вывод ИД.
-- v.1.3: Modified by Fomin Dmitriy 21.04.2008
-- Убраны лишние поля.
-- v.1.4: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле RelativeUrl.
-- =============================================
CREATE proc [dbo].[SearchDocument]
	@isActive bit = null
	, @contextCodes nvarchar(4000) = null
	, @name nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = N'Id'
	, @sortAsc bit = 0
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
			'	, Name nvarchar(255) ' +
			'	, Description ntext ' +
			'	, IsActive bit ' +
			'	, ActivateDate datetime ' +
			'	, ContextCodes nvarchar(4000) ' +
			'	, RelativeUrl nvarchar(255) ' +
			'   , Date datetime ' +
			'	) ' 

	if isnull(@contextCodes, '') <> ''
		set @declareCommandText = @declareCommandText + 
			'declare @codes table '+
			'	( ' +
			'	Code nvarchar(255) ' +
			'	) ' +
			'insert @codes select value from dbo.GetDelimitedValues(@contextCodes) '

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	document.Id Id ' +
				'	, document.Name Name ' +
				'	, document.Description Description ' +
				'	, document.IsActive IsActive ' +
				'	, document.ActivateDate ActivateDate ' +
				'	, document.ContextCodes ContextCodes ' +
				'	, document.RelativeUrl RelativeUrl ' +
				'	, document.UpdateDate Date ' +
				'from dbo.Document document with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Document document with (nolock) ' +
				'where 1 = 1 ' 
	
	if not @isActive is null
		set @commandText = @commandText + ' and document.IsActive = @isActive '

	if not @contextCodes is null
		set @commandText = @commandText + ' and not exists(select 1 ' +
				'		from @codes context_codes ' +
				'			inner join dbo.Context context ' +
				'				on context.Code = context_codes.Code ' +
				'			left outer join dbo.DocumentContext document_context with(nolock) ' +
				'				on document_context.ContextId = context.Id ' +
				'					and document_context.DocumentId = document.Id ' +
				'		where document_context.Id is null) '

	if not @nameFormat is null
		set @commandText = @commandText + ' and document.Name like @nameFormat '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'Name'
		begin
			set @innerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Name <orderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = 'Date'
		begin
			set @innerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection>, Id <orderDirection> '
		end
		else
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
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
			'	, search.Name ' +
			'	, search.Description ' +
			'	, search.IsActive ' +
			'	, search.ActivateDate ' +
			'	, search.ContextCodes ' +
			'	, search.RelativeUrl ' +
			'	, search.Date ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@isActive bit, @contextCodes nvarchar(4000), @nameFormat nvarchar(255)'
		, @IsActive
		, @contextCodes
		, @nameFormat

	return 0
end
