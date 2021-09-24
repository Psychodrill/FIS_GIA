
-- exec dbo.SearchEntrantCheck
-- ==============================================
-- Получение списка проверенных абитуриентов
-- v.1.0: Created by Sedov Anton 09.07.2008
-- ==============================================
CREATE procedure dbo.SearchEntrantCheck 
	@login nvarchar(255)
	, @batchId bigint
	, @startRowIndex int = 1
	, @maxRowCount int = null
	, @showCount bit = null
as
begin
	declare
		@accountId bigint
		, @internalBatchId bigint

	set @internalBatchId = dbo.GetInternalId(@batchId)

	if not exists(select 1
			from dbo.EntrantCheckBatch entrant_check_batch with (nolock, fastfirstrow)
				inner join dbo.Account account with (nolock, fastfirstrow)
					on entrant_check_batch.OwnerAccountId = account.[Id]
			where 
				entrant_check_batch.Id = @internalBatchId
					and entrant_check_batch.IsProcess = 0
					and account.[Login] = @login)
		set @internalBatchId = 0

	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @sortColumn nvarchar(20)
		, @sortAsc bit

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @sortColumn = N'Id'
	set @sortAsc = 0
	
	
	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, CertificateNumber nvarchar(255) ' +
			'	, LastName nvarchar(255) ' +
			'	, FirstName nvarchar(255)' +
			'	, PatronymicName nvarchar(255) ' +
			'	, OrganizationName nvarchar(255)' +
			'	, EntrantCreateDate datetime ' +
			'	, IsExist bit ' +
			'	) '

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	entrant_check.Id ' +
				'	, entrant_check.CertificateNumber ' +
				'	, entrant_check.SourceLastName ' +
				'	, entrant_check.SourceFirstName ' +
				'	, entrant_check.SourcePatronymicName ' +
				'	, entrant_check.SourceOrganizationName ' +
				'	, entrant_check.SourceEntrantCreateDate ' +
				'	, case ' + 
				'		when not entrant_check.SourceEntrantId is null then 1 ' +
				'		else 0 ' +
				'	end IsExist ' + 
				'from dbo.EntrantCheck entrant_check with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.EntrantCheck entrant_check with (nolock) ' +
				'where 1 = 1 ' 

	
	set @commandText = @commandText + 
		'	and entrant_check.BatchCheckId = @internalBatchId ' 

	if isnull(@showCount, 0) = 0
	begin
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

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.CertificateNumber CertificateNumber' +
			'	, search.LastName LastName ' +
			'	, search.FirstName FirstName ' +
			'	, search.PatronymicName PatronymicName ' +
			'	, search.OrganizationName OrganizationName ' + 
			'	, search.EntrantCreateDate EntrantCreateDate ' + 
			'	, search.IsExist IsExist ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@internalBatchId bigint'
		, @internalBatchId
	
	return 0
end
