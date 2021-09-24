-- exec dbo.SearchAccount

-- =============================================
-- Поиск пользователей горячей линии.
-- v.1.0: Created by Makarev Andrey 09.04.2008
-- v.1.1: Modified by Makarev Andrey 11.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Sedov Anton 16.05.2008
-- добавлен параметр @email
-- =============================================
CREATE proc dbo.SearchAccount
	@groupCode nvarchar(255)
	, @login nvarchar(255) = null
	, @lastName nvarchar(255) = null
	, @isActive bit = null
	, @email nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = N'login'
	, @sortAsc bit = 1
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
		, @userGroupId int
		, @lastNameFormat nvarchar(255)

	if isnull(@lastName, N'') <> N''
		set @lastNameFormat = N'%' + replace(@lastName, N' ', '%') + N'%'

	select
		@userGroupId = [group].[Id]
	from
		dbo.[Group] [group] with (nolock, fastfirstrow)
	where
		[group].Code = @groupCode

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Login nvarchar(255) ' +
			'	, LastName nvarchar(255) ' +
			'	, FirstName nvarchar(255) ' +
			'	, PatronymicName nvarchar(255) ' +
			'	, IsActive bit ' +
			'   , Email nvarchar(255) ' + 
			'	, Id bigint not null ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	account.Login Login ' +
				'	, account.LastName LastName ' +
				'	, account.FirstName FirstName ' +
				'	, account.PatronymicName PatronymicName ' +
				'	, account.IsActive IsActive ' +
				'   , account.Email Email ' +
				'	, account.[Id] ' +
				'from dbo.Account account with (nolock) ' +
				'	inner join dbo.GroupAccount group_account with (nolock) ' +
				'		on account.[Id] = group_account.AccountId ' +
				'where ' +
				'	group_account.GroupId = @userGroupId '
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Account account with (nolock, fastfirstrow) ' +
				'	inner join dbo.GroupAccount group_account with (nolock) ' +
				'		on account.[Id] = group_account.AccountId ' +
				'where ' + 
				'	group_account.GroupId = @userGroupId ' 
	
	if not @login is null
		set @commandText = @commandText + ' and account.Login = @login '

	if not @isActive is null
		set @commandText = @commandText + ' and account.IsActive = @isActive '

	if not @lastName is null
		set @commandText = @commandText + ' and account.LastName like @lastNameFormat '
	
	if not @email is null
		set @commandText = @commandText + ' and account.Email = @email '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'login'
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection> '
		end
		else if @sortColumn = 'name'
		begin
			set @innerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @outerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @resultOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
		end
		else if @sortColumn = 'email'
		begin
			set @innerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Email <orderDirection>, Id <orderDirection> '
		end 
		else if @sortColumn = 'Id'
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> ' 
		end 
		else 
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
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
			'	search.Login ' +
			'	, search.LastName ' +
			'	, search.FirstName ' +
			'	, search.PatronymicName ' +
			'	, search.IsActive ' +
			'	, search.Email ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 
	
	set @params = 
			'@userGroupId int ' +
			', @login nvarchar(255) ' +
			', @IsActive bit ' + 
			', @lastNameFormat nvarchar(255) ' +
			', @email nvarchar(255) ' 
	
	exec sp_executesql @commandText, @params, 
			@userGroupId
			, @login
			, @IsActive
			, @lastNameFormat
			, @email 

	return 0
end
