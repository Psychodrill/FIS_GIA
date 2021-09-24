-- exec dbo.SearchAskedQuestion

-- =============================================
-- Получение списка вопросов.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc dbo.SearchAskedQuestion
	@name nvarchar(255) = null
	, @isActive bit = null
	, @contextCodes nvarchar(4000) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20)
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@nameFormat nvarchar(255)
		, @declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)

	if isnull(@name, '') <> ''
		set @nameFormat = '%' + replace(@name, ' ', '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, Name nvarchar(255) ' +
			'	, Question ntext ' +
			'	, IsActive bit ' +
			'	, Popularity decimal(18,4) ' +
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
				'	asked_question.Id Id ' +
				'	, asked_question.Name Name ' +
				'	, asked_question.Question Question ' +
				'	, asked_question.IsActive IsActive ' +
				'	, asked_question.Popularity Popularity ' +
				'from dbo.AskedQuestion asked_question with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.AskedQuestion asked_question with (nolock) ' +
				'where 1 = 1 ' 

	if not @nameFormat is null	
		set @commandText = @commandText + ' and asked_question.Name like @nameFormat '

	if not @isActive is null
		set @commandText = @commandText + ' and asked_question.IsActive = @isActive '

	if not @contextCodes is null
		set @commandText = @commandText + ' and not exists(select 1 ' +
				'		from @codes context_codes ' +
				'			inner join dbo.Context context ' +
				'				on context.Code = context_codes.Code ' +
				'			left outer join dbo.AskedQuestionContext asked_question_context with(nolock) ' +
				'				on asked_question_context.ContextId = context.Id ' +
				'					and asked_question_context.AskedQuestionId = asked_question.Id ' +
				'		where asked_question_context.Id is null) '

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
		else
		begin
			set @innerOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
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
			'	, search.Question ' +
			'	, search.IsActive ' +
			'	, search.Popularity ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@nameFormat nvarchar(255), @isActive bit, @contextCodes nvarchar(4000)'
		, @nameFormat
		, @IsActive
		, @contextCodes

	return 0
end
