
-- =============================================
-- Получение списка ошибок задания на загрузку закрытий сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError] 
	@taskId bigint = null
	, @startRowIndex int = null
	, @maxRowCount int = null
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
		, @sortColumn nvarchar(20)
		, @sortAsc bit
		, @internalTaskId bigint
		, @server nvarchar(30)

	set @internalTaskId = dbo.GetInternalId(@taskId)

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @sortColumn = N'Date'
	set @sortAsc = 1

	select @server = (select top 1 ss.name + '.fbs_loader_db' from task_db..SystemServer ss
		join sys.servers s on s.name = ss.name
		where rolecode = 'loader')

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			N'declare @search table ' +
			N'	( ' +
			N'	TaskId bigint ' +
			N'	, Date datetime ' +
			N'	, RowIndex bigint ' +
			N'	, Error ntext ' +
			N'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				N'select <innerHeader> ' +
				N'	cne_certificate_deny_loading_task_error.TaskId TaskId ' +
				N'	, cne_certificate_deny_loading_task_error.Date Date ' +
				N'	, cne_certificate_deny_loading_task_error.RowIndex RowIndex ' +
				N'	, cne_certificate_deny_loading_task_error.Error Error ' +
				N'from ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error with (nolock) ' +
				N'where 1 = 1'
	else
		set @commandText = 
				N'select count(*) ' +
				N'from ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error with (nolock) ' +
				N'where 1 = 1'

	if not @taskId is null
		set @commandText = @commandText + N' and cne_certificate_deny_loading_task_error.TaskId = @internalTaskId '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = N'Date'
		begin
			set @innerOrder = 'order by Date <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection> '
		end
		else 
		begin
			set @innerOrder = 'order by Date <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection> '
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
			set @innerSelectHeader = replace(N'top <count>', N'<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
			set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = N'top 10000000'
			set @outerSelectHeader = N'top 10000000'
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
		N'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			N'select ' +
			N'	dbo.GetExternalId(search.TaskId) TaskId ' +
			N'	, search.Date Date ' +
			N'	, search.RowIndex RowIndex ' +
			N'	, search.Error Error ' +
			N'from ' +
			N'	@search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@internalTaskId bigint'
		, @internalTaskId

	return 0
end
