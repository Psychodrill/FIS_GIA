 
-- exec dbo.GetAccountKey
-- ====================================================
-- Получение ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
CREATE procedure dbo.GetAccountKey
	@login nvarchar(255)
	, @key nvarchar(255)
as
begin
	select
		account.Login [Login]
		, account_key.[Key]
		, account_key.DateFrom
		, account_key.DateTo
		, account_key.IsActive
	from dbo.Account account
		inner join dbo.AccountKey account_key
			on account.Id = account_key.AccountId
	where
		account.Login = @login
		and account_key.[Key] = @key

	return 0
end
