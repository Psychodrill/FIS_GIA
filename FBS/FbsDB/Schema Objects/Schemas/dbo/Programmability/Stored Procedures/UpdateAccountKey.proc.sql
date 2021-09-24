 
-- exec dbo.UpdateAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 01.09.2008
-- ====================================================
CREATE procedure dbo.UpdateAccountKey
	@login nvarchar(255)
	, @key nvarchar(255)
	, @dateFrom datetime
	, @dateTo datetime
	, @isActive bit
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare
		@accountId bigint
		, @editorAccountId bigint
		, @eventCode nvarchar(255)
		, @keyId bigint
		, @updateId	uniqueidentifier
		, @keyIds nvarchar(255)

	set @updateId = newid()
	
	select @accountId = account.Id
	from dbo.Account account
	where account.[Login] = @login

	select @editorAccountId = account.Id
	from dbo.Account account
	where account.[Login] = @editorLogin

	select @keyId = account_key.Id
	from dbo.AccountKey account_key
	where account_key.[Key] = @key
		and account_key.AccountId = @accountId

	if @keyId is null
	begin
		insert into dbo.AccountKey
			(
			CreateDate
			, UpdateDate
			, UpdateId
			, EditorAccountId
			, EditorIp
			, AccountId
			, [Key]
			, DateFrom
			, DateTo
			, IsActive
			)
		select
			GetDate()
			, GetDate()
			, @updateId
			, @editorAccountId
			, @editorip
			, @accountId
			, @key
			, @dateFrom
			, @dateTo
			, @isActive

		set @keyId = scope_identity()

		set @eventCode = 'USR_KEY_CREATE'
	end
	else
	begin
		update account_key
		set
			UpdateDate = GetDate()
			, UpdateId = @updateId
			, EditorAccountId = @editorAccountId
			, EditorIp = @editorIp
			, DateFrom = @dateFrom
			, DateTo = @dateTo
			, IsActive = @isActive
		from dbo.AccountKey account_key
		where account_key.Id = @keyId

		set @eventCode = 'USR_KEY_EDIT'
	end

	set @keyIds = @keyId

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @keyIds
		, @eventParams = null
		, @updateId = @updateId

	return 0
end
