-- exec dbo.SearchAccountAuthenticationLog
-- =============================================
-- Поиск в логе аутентификации записей об аккаунте
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- v.1.1: Modified by Fomin Dmitriy 20.05.2008
-- Добавлено поле IsVpnIp.
-- v.1.2: Modified by Fomin Dmitriy 20.05.2008
-- Анонимное событие на регистрацию проводит 
-- неявную аутентификацию.
-- v.1.3: Modified by Sedov A.G. 22.05.2008
-- Переделана выборка данных, выборка теперь 
-- выполняется из dbo.AuthenticationEventLog 
-- =============================================
CREATE procedure dbo.SearchAccountAuthenticationLog
	@login nvarchar(255)
	, @startRowIndex int = null 
	, @maxRowCount int = null 
	, @showCount bit = null 
as
begin
	declare
		@declareCommandText nvarchar(4000)
		, @params nvarchar(4000)
		, @commandText nvarchar(4000) 
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortAsc bit
		, @verifyEventCode nvarchar(255)
		, @registrationEventCode nvarchar(255)

	set @verifyEventCode = 'USR_VERIFY'
	set @registrationEventCode = 'USR_REG'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = '' 

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Date datetime ' +
			'	, Ip nvarchar(255) ' +
			'	, IsPasswordValid bit ' +
			'	, IsIpValid bit ' +
			'	) ' 
	
	
	if isnull(@showCount, 0) = 0
		set @commandText = 
			'select <innerHeader> ' +
			'	auth_log.Date Date ' +
			'	, auth_log.Ip Ip ' + 
			'   , auth_log.IsPasswordValid ' + 
			'	, auth_log.IsIpValid ' + 
			'from ' + 
			'	dbo.AuthenticationEventLog auth_log with (nolock) ' + 
			'		inner join dbo.Account account with (nolock, fastfirstrow) ' + 
			'			on account.Id = auth_log.AccountId ' + 
			'where 1 = 1 ' 
	else
		set @commandText = 
			'select count(*) ' +
			'from ' + 
			'	dbo.AuthenticationEventLog auth_log with (nolock) ' +
			'		inner join dbo.Account account with (nolock, fastfirstrow) ' +
			'			on account.Id = auth_log.AccountId ' +
			'where 1 = 1 '

	set @commandText = @commandText +
		' and account.[Login] = @login '

	if isnull(@showCount, 0) = 0
	begin
		begin
			set @innerOrder = 'order by Date <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection> '
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
			'	search.Date ' +
			'	, search.Ip ' +
			'	, search.IsPasswordValid ' +
			'	, search.IsIpValid ' +
			'	, case ' +
			'		when exists(select 1 ' +
			'				from dbo.VpnIp vpn_ip ' +
			'				where vpn_ip.Ip = search.Ip) then 1 ' +
			'		else 0 ' +
			'	end IsVpnIp ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText

	set @params = 
			'@login nvarchar(255) ' +  
			', @verifyEventCode varchar(100) ' +
			', @registrationEventCode varchar(100) '

	exec sp_executesql @commandText, @params
			, @login 
			, @verifyEventCode
			, @registrationEventCode

	return 0
end
