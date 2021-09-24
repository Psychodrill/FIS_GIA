-- exec dbo.GetRemindAccount

-- =============================================
-- Получить забытую учетную запись.
-- v.1.0: Created by Makarev Andrey
-- =============================================
CREATE procedure dbo.GetRemindAccount
	@email nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	
	declare
		@currentYear int
		, @eventCode nvarchar(255)
		, @editorAccountId bigint
		, @login nvarchar(255) 
		, @accountId bigint
		, @accountIds nvarchar(255)

	set @currentYear = year(getdate())
	set @eventCode = N'USR_REMIND'

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin

	select top 1
		@login = account.[Login] 
		, @accountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.email = @email
	order by 
		dbo.GetUserStatusOrder(dbo.GetUserStatus(account.ConfirmYear , account.Status
				, @currentYear, account.RegistrationDocument)) desc
		, account.UpdateDate desc

	select 
		@login [Login]
		, @email email

	set @accountIds = isnull(convert(nvarchar(255), @accountId), '')

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = @email
		, @updateId = null

	return 0
end
