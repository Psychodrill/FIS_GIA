
-- =============================================
-- Получение списка заданий на загрузку сертификатов.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateLoadingTask]
	@startRowIndex int = null
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
		, @server nvarchar(30)

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	
	select @server = (select top 1 ss.name + '.fbs_loader_db' from task_db..SystemServer ss
		join sys.servers s on s.name = ss.name
		where rolecode = 'loader')

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			N'declare @search table ' +
			N'	( ' +
			N'	Id bigint ' +
			N'	, UpdateDate datetime ' +
			N'	, EditorAccountId bigint ' +
			N'	, EditorIp nvarchar(255) ' +
			N'	, SourceBatchUrl nvarchar(255) ' +
			N'	, IsActive bit ' +
			N'	, IsProcess bit ' +
			N'	, IsCorrect bit ' +
			N'	, IsLoaded bit ' +
			N'	, ErrorCount int ' +
			N'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				N'select <innerHeader> ' +
				N'	cne_certificate_loading_task.Id Id ' +
				N'	, cne_certificate_loading_task.CreateDate UpdateDate ' +
				N'	, cne_certificate_loading_task.EditorAccountId EditorAccountId ' +
				N'	, cne_certificate_loading_task.EditorIp EditorIp ' +
				N'	, cne_certificate_loading_task.SourceBatchUrl SourceBatchUrl ' +
				N'	, cne_certificate_loading_task.IsActive IsActive ' +
				N'	, cne_certificate_loading_task.IsProcess IsProcess ' +
				N'	, cne_certificate_loading_task.IsCorrect IsCorrect ' +
				N'	, cne_certificate_loading_task.IsLoaded IsLoaded ' +
				N'	, 0 ErrorCount  ' +
				N'from ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task with (nolock) '
	else
		set @commandText = 
				N'select count(*) ' +
				N'from ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task with (nolock) '	
	if isnull(@showCount, 0) = 0
	begin
		begin
			set @innerOrder = N'order by Id desc '
			set @outerOrder = N'order by Id asc '
			set @resultOrder = N'order by Id desc '
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
		set @commandText = @commandText +
			N'update search '+
			N'set ' +
			N'	ErrorCount = ( '+
			N'		select '+ 
			N'			count(*)  '+
			N'		from  '+
			N'			' + @server + N'.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error '+
			N'		where '+
			N'			cne_certificate_loading_task_error.TaskId = search.Id '+
			N'		) '+
			N'from '+
			N'	@search search '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			N'select ' +
			N'	dbo.GetExternalId(search.Id) Id ' +
			N'	, search.UpdateDate UpdateDate ' +
			N'	, account.Login EditorLogin ' +
			N'	, account.LastName EditorLastName ' +
			N'	, account.FirstName EditorFirstName ' +
			N'	, account.PatronymicName EditorPatronymicName ' +
			N'	, search.EditorIp EditorIp ' +
			N'	, search.SourceBatchUrl SourceBatchUrl ' +
			N'	, search.IsActive IsActive ' +
			N'	, search.IsProcess IsProcess ' +
			N'	, search.IsCorrect IsCorrect ' +
			N'	, search.IsLoaded IsLoaded ' +
			N'	, search.ErrorCount ErrorCount ' +
			N'from ' +
			N'	@search search ' + 
			N'		left join dbo.Account account ' +
			N'			on search.EditorAccountId = account.Id ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText

	return 0
end

