 
-- exec dbo.SearchAccountKey
-- ====================================================
-- Поиск ключей.
-- v.1.0: Created by Fomin Dmitriy 28.08.2008
-- ====================================================
CREATE procedure dbo.SearchAccountKey
	@login nvarchar(255)
as
begin
	declare
		@now datetime

	set @now = convert(nvarchar(8), GetDate(), 112)

	select
		account_key.[Key]
		, account_key.DateFrom
		, account_key.DateTo
		, account_key.IsActive
	from dbo.Account account
		inner join dbo.AccountKey account_key
			on account.Id = account_key.AccountId
	where
		account.Login = @login
	order by
		case
			when @now < account_key.DateFrom then 2
			when @now > account_key.DateTo then 1
			else 0
		end asc

	return 0
end
