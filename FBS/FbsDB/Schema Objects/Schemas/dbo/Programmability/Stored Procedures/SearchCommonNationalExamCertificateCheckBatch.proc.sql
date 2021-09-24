-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
create proc [dbo].[SearchCommonNationalExamCertificateCheckBatch]
	@login nvarchar(255)
	, @startRowIndex int = 1  -- пейджинг
	, @maxRowCount int = null -- пейджинг
	, @showCount bit = null   -- если > 0, то выбирается общее кол-во
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

	if exists ( select top 1 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
				where a2.[Login] = @login )
		set @accountId = null
	else 
		set @accountId = isnull(
			(select account.[Id] 
			from dbo.Account account with (nolock, fastfirstrow) 
			where account.[Login] = @login), 0)

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @sortColumn = N'CreateDate'
	set	@sortAsc = 0

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table 
				( 
				Id bigint 
				, CreateDate datetime 
				, IsProcess bit 
				, IsCorrect bit 
				, Login varchar(255) 
				, Total int
				, Found int
				) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> 
					b.Id Id 
					, b.CreateDate CreateDate 
					, b.IsProcess IsProcess 
					, b.IsCorrect IsCorrect 
					, a.login Login 
					, (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
					, (select count(SourceCertificateIdGuid) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
				from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
				left join account a on a.id = b.OwnerAccountId 
				where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) ' +
				'where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 

	if isnull(@showCount, 0) = 0
	begin
		set @innerOrder = 'order by CreateDate <orderDirection> '
		set @outerOrder = 'order by CreateDate <orderDirection> '
		set @resultOrder = 'order by CreateDate <orderDirection> '

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
			'select 
				dbo.GetExternalId(s.Id) Id 
				, s.Login 
				, s.CreateDate 
				, s.IsProcess 
				, s.IsCorrect 
				, s.Total
				, s.Found
			from @search s ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@accountId bigint'
		, @accountId

	return 0
end
