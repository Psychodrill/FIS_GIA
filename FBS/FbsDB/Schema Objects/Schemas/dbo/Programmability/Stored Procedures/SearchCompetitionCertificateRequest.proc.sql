
-- exec dbo.SearchCompetitionCertificateRequest
-- ========================================================
-- Получить список проверенных сертификатов  
-- олимпиадников пакета
-- v.1.0: Created by Sedov Anton 15.08.2008
-- v.1.1: Modified by Sedov Anton 18.08.2008
-- Добавлено поле IsExist
-- v.1.2: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- ========================================================
CREATE procedure dbo.SearchCompetitionCertificateRequest
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
			from dbo.CompetitionCertificateRequestBatch batch with (nolock, fastfirstrow)
				inner join dbo.Account account with (nolock, fastfirstrow)
					on batch.OwnerAccountId = account.[Id]
			where 
				batch.Id = @internalBatchId
				and batch.IsProcess = 0
				and account.[Login] = @login)
		set @internalBatchId = 0

	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @viewSelectCommandText nvarchar(4000)
		, @viewSelectPivot1CommandText nvarchar(4000)
		, @viewSelectPivot2CommandText nvarchar(4000)
		, @pivotSubjectColumns nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortColumn nvarchar(20)
		, @sortAsc bit

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @viewSelectCommandText = ''
	set @viewSelectPivot1CommandText = ''
	set @viewSelectPivot2CommandText = ''
	set @pivotSubjectColumns = ''
	set @sortColumn = N'LastName'
	set	@sortAsc = 0

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	CompetitionTypeId int ' +
			'	, Name nvarchar(255) ' +
			'	, LastName nvarchar(255) ' +
			'	, FirstName nvarchar(255) ' +
			'	, PatronymicName nvarchar(255) ' +
			'	, Degree nvarchar(255) ' +
			'	, RegionName nvarchar(255) ' +
			'	, City nvarchar(255) ' + 
			'	, School nvarchar(255) ' +
			'	, Class nvarchar(255) ' +
			'	, IsExist bit ' + 
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	competition_certificate.CompetitionTypeId' +
				'	, competition_type.[Name] ' +
				'	, competition_certificate_request.LastName ' +
				'	, competition_certificate_request.FirstName ' +
				'	, competition_certificate_request.PatronymicName ' +
				'	, competition_certificate.Degree ' +
				'	, region.[Name] RegionName ' +
				'	, competition_certificate.City ' +
				'	, competition_certificate.School ' +
				'	, competition_certificate.Class ' +
				'	, case ' +
				'		when competition_certificate.Id is null then 0 ' +
				'		else 1 ' + 
				'	end IsExist ' +  
				'from	' + 
				'	dbo.CompetitionCertificateRequest competition_certificate_request ' + 
				'		left join dbo.CompetitionCertificate competition_certificate ' +
				'			left join dbo.CompetitionType competition_type ' + 
				'				on competition_certificate.CompetitionTypeId = competition_type.Id ' +
				'			left join dbo.Region region ' + 
				'				on competition_certificate.RegionId = region.Id ' + 
				'			on competition_certificate_request.SourceCertificateId = competition_certificate.Id ' + 
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.CompetitionCertificateRequest competition_certificate_request with (nolock) ' +
				'where 1 = 1 ' 

	set @commandText = @commandText + 
		'	and competition_certificate_request.BatchId = @internalBatchId '

	if isnull(@showCount, 0) = 0
	begin
		begin
			set @innerOrder = 'order by LastName <orderDirection> '
			set @outerOrder = 'order by LastName <orderDirection> '
			set @resultOrder = 'order by LastName <orderDirection> '
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
			'	search.CompetitionTypeId CompetitionTypeId ' +
			'	, search.[Name] CompetitiontypeName ' + 
			'	, search.LastName LastName ' +
			'	, search.FirstName FirstName ' + 
			'	, search.PatronymicName PatronymicName ' + 
			'	, search.Degree Degree ' + 
			'	, search.RegionName RegionName ' + 
			'	, search.City City ' + 
			'	, search.School School ' +
			'	, search.Class Class ' +  
			'	, search.IsExist ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@internalBatchId bigint'
		, @internalBatchId

	return 0
end
