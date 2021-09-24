-- exec dbo.UpdateUserAccountStatus

-- =============================================
-- Изменить статус пользователя.
-- v.1.0: Created by Makarev Andrey 08.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Рефакторинг: выделена функция, изменеа логика 
-- означивания полей.
-- v.1.2: Modified by Fomin Dmitriy 11.04.2008
-- Удаляется документ регистрации, если он устарел.
-- v.1.3: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.4: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.5: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- =============================================
CREATE proc dbo.UpdateUserAccountStatus
	@login nvarchar(255)
	, @status nvarchar(255)
	, @adminComment ntext 
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare
		@isActive bit
		, @eventCode nvarchar(255)
		, @accountId bigint
		, @editorAccountId bigint
		, @currentYear int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()
	set @eventCode = N'USR_STATE'
	set @currentYear = Year(GetDate())
	
	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	select
		@accountId = account.[Id]
		, @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, 
				account.RegistrationDocument)
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	update account
	set 
		Status = @status
		, AdminComment = case
			when dbo.HasUserAccountAdminComment(@status) = 0 then null
			else @adminComment
		end
		, IsActive = dbo.GetUserIsActive(@status)
		, UpdateDate = GetDate()
		, UpdateId = @updateId
		, ConfirmYear = @currentYear
		-- Удаляем документ регистрации, если он устарел.
		, RegistrationDocument = case
			when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 then null
			else account.RegistrationDocument
		end
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
	from 
		dbo.Account account with (rowlock)
	where
		account.[Id] = @accountId

	exec dbo.RefreshRoleActivity @accountId = @accountId

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0
end
