 
-- exec dbo.CheckAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
CREATE procedure dbo.CheckAccountKey
	@key nvarchar(255)
	, @ip nvarchar(255)
as
begin
	declare
		@now datetime
		, @entityParams nvarchar(1000)
		, @sourceEntityIds nvarchar(255)
		, @accountId bigint
		, @isValid bit
		, @year int
		, @login nvarchar(255)

	set @now = convert(nvarchar(8), GetDate(), 112)
	set @year = Year(GetDate())

	select top 1
		@accountId = account.Id
		, @login = account.Login
	from dbo.Account account
		inner join dbo.AccountKey account_key
			on account.Id = account_key.AccountId
	where
		account_key.[Key] = @key
		and account_key.IsActive = 1
		and @now between isnull(account_key.DateFrom, @now) and isnull(account_key.DateTo, @now)
		and ((account.Id in (select group_account.AccountId
				from dbo.GroupAccount group_account
					inner join dbo.[Group] [group]
						on [group].Id = group_account.GroupId
				where [group].Code = 'User')
				and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
						, account.RegistrationDocument) = 'activated')
			or (account.Id in (select group_account.AccountId
				from dbo.GroupAccount group_account
					inner join dbo.[Group] [group]
						on [group].Id = group_account.GroupId
				where [group].Code = 'Administrator')
				and account.IsActive = 1))

	if not @login is null
		set @isValid = 1
	else
		set @isValid = 0
		
	select
		@key [Key]
		, @login [Login]
		, @isValid IsValid

	set @entityParams = @key + N'|' +
			convert(nvarchar, @isValid)

	set @sourceEntityIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = N'USR_KEY_VERIFY'
		, @sourceEntityIds = @sourceEntityIds
		, @eventParams = @entityParams
		, @updateId = null

	return 0
end
