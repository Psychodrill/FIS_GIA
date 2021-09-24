-- =============================================
-- Получение списка рассылок.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[SearchDeliveries]
	@title nvarchar(255) = null
	, @createDateFrom datetime = null
	, @createDateTo datetime = null
	, @deliveryDateFrom datetime = null
	, @deliveryDateTo datetime = null
	, @status int = null
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
		, @titleFormat nvarchar(255)

	if isnull(@title, '') <> ''
		set @titleFormat = '%' + replace(@title, ' ' , '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, CreateDate datetime ' +
			'	, DeliveryDate datetime ' +
			'	, TypeCode nvarchar(20) ' +
			'	, Title nvarchar(255) ' +
			'	, Status int ' +
			'	, StatusName nvarchar(255) ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	delivery.Id Id ' +
				'	, delivery.CreateDate CreateDate ' +
				'	, delivery.DeliveryDate DeliveryDate ' +
				'	, delivery.TypeCode TypeCode ' +
				'	, delivery.Title Title ' +
				'	, delivery.Status Status ' +
				'	, status.Name StatusName ' +
				'from dbo.Delivery delivery with (nolock) ' +
				'inner join DeliveryStatus status on delivery.Status=status.Id '+
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Delivery delivery with (nolock) ' +
				'where 1 = 1 ' 
	
	if not @status is null
		set @commandText = @commandText + ' and delivery.Status = @status '

	if not @createDateFrom is null
		set @commandText = @commandText + ' and delivery.CreateDate >= @createDateFrom '

	if not @createDateTo is null
		set @commandText = @commandText + ' and delivery.CreateDate <= @createDateTo '

	if not @deliveryDateFrom is null
		set @commandText = @commandText + ' and delivery.DeliveryDate >= @deliveryDateFrom '

	if not @deliveryDateTo is null
		set @commandText = @commandText + ' and delivery.DeliveryDate <= @deliveryDateTo '

	if not @title is null
		set @commandText = @commandText + ' and delivery.Title like @titleFormat '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = N'Title'
		begin
			set @innerOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = N'StatusName'
		begin
			set @innerOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = N'CreateDate'
		begin
			set @innerOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
		end
		else
		begin 
			set @innerOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '

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
			'	search.Id Id ' +
			'	, search.CreateDate ' +
			'	, search.DeliveryDate ' +
			'	, search.TypeCode ' +
			'	, search.Title ' +
			'	, search.Status ' +
			'	, search.StatusName ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@status int, @createDateFrom datetime, @createDateTo datetime, @deliveryDateFrom datetime, @deliveryDateTo datetime, @titleFormat nvarchar(255)'
		, @status
		, @createDateFrom
		, @createDateTo
		, @deliveryDateFrom
		, @deliveryDateTo
		, @titleFormat

	return 0
end







