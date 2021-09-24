
-- exec dbo.SearchEntrantCheckBatch
-- ===============================================
-- Процедура поиска пакетов  проверки абитриентов
-- v.1.0: Created by Sedov Anton 09.07.2008
-- ===============================================
CREATE procedure dbo.SearchEntrantCheckBatch
	@login nvarchar(255)
	, @startRowIndex int = 1
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
		, @accountId bigint

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @sortColumn = N'CreateDate'
	set	@sortAsc = 0

	select
		@accountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			N'
			declare @search table
				(
				Id bigint
				, CreateDate datetime
				, IsProcess bit 
				, IsCorrect bit
				)
				'

	if isnull(@showCount, 0) = 0 
		set @commandText = 
			N'
			select <innerHeader>
				entrant_check_batch.Id
				, entrant_check_batch.CreateDate
				, entrant_check_batch.IsProcess
				, entrant_check_batch.IsCorrect
			from 
				dbo.EntrantCheckBatch entrant_check_batch
			where 
				entrant_check_batch.OwnerAccountId = @accountId	
			'
	else 
		set @commandText = 
			N'
			select count(*)
			from dbo.EntrantCheckBatch entrant_check_batch
			where entrant_check_batch.OwnerAccountId = @accountid
			'

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'CreateDate'
		begin
			set @innerOrder = 'order by CreateDate <orderDirection> '
			set @outerOrder = 'order by CreateDate <orderDirection> '
			set @resultOrder = 'order by CreateDate <orderDirection> '
		end
		else
		begin
			set @innerOrder = 'order by CreateDate <orderDirection> '
			set @outerOrder = 'order by CreateDate <orderDirection> '
			set @resultOrder = 'order by CreateDate <orderDirection> '
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
			'	, search.CreateDate ' +
			'	, search.IsProcess ' +
			'	, search.IsCorrect ' +
			'from @search search ' + @resultOrder
	
	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@accountId bigint'
		, @accountId
	
	return 0
end
