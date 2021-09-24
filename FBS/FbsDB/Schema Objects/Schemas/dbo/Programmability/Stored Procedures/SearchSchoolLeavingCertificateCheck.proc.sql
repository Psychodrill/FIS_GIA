
-- exec dbo.SearchSchoolLeavingCertificateCheck
-- ==============================================
-- Получение списка проверенных аттестатов
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ==============================================
CREATE procedure dbo.SearchSchoolLeavingCertificateCheck
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
			from dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch with (nolock, fastfirstrow)
				inner join dbo.Account account with (nolock, fastfirstrow)
					on schoolleaving_certificate_check_batch.OwnerAccountId = account.[Id]
			where 
				schoolleaving_certificate_check_batch.Id = @internalBatchId
					and schoolleaving_certificate_check_batch.IsProcess = 0
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
			'	, IsDeny bit' +
			'	, DenyComment ntext ' +
			'	) '

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	schoolleaving_certificate_check.Id ' +
				'	, schoolleaving_certificate_check.CertificateNumber ' +
				'	, case when school_leaving_certificate_deny.Id is null ' +
				'			then 0 ' +
				'		else 1 ' +
				'	end IsDeny ' + 
				'	, school_leaving_certificate_deny.Comment ' + 
				'from dbo.SchoolLeavingCertificateCheck schoolleaving_certificate_check with (nolock) ' +
				'	left join dbo.SchoolLeavingCertificateDeny school_leaving_certificate_deny with(nolock) ' +
				'		on school_leaving_certificate_deny.Id = schoolleaving_certificate_check.SourceCertificateDenyId ' +   
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.SchoolLeavingCertificateCheck schoolleaving_certificate_check with (nolock) ' +
				'where 1 = 1 ' 

	
	set @commandText = @commandText + 
		'	and schoolleaving_certificate_check.BatchId = @internalBatchId ' 

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
			'	, search.IsDeny IsDeny ' + 
			'	, search.DenyComment DenyComment ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@internalBatchId bigint'
		, @internalBatchId
	
	return 0
end
