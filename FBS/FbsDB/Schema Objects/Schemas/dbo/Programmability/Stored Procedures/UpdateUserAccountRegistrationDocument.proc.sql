-- exec dbo.UpdateUserAccountRegistrationDocument

-- =============================================
-- Изменить регистрационный документ пользователя.
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.5: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены Status output-параметр.
-- =============================================
CREATE proc [dbo].[UpdateUserAccountRegistrationDocument]
	@login nvarchar(255)
	, @registrationDocument image
	, @registrationDocumentContentType nvarchar(255)
	, @status nvarchar(255) output
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare
		@accountId bigint
		, @editorAccountId bigint
		, @currentYear int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()

	set @currentYear = Year(GetDate())

	select
		@accountId = a.[Id]
		, @status = dbo.GetUserStatus(a.ConfirmYear, isnull(@status, a.Status), @currentYear, @registrationDocument)
	from 
		dbo.Account a with (nolock, fastfirstrow)
	where 
		a.[Login] = @login

	select
		@editorAccountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin

	update account
	set
		UpdateDate = GetDate()
		, UpdateId = @updateId
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, RegistrationDocument = @registrationDocument
		, RegistrationDocumentContentType = @registrationDocumentContentType
		, [Status] = @status
	from 
		dbo.Account account with (rowlock)
	where 
		account.[Id] = @accountId

	exec dbo.RefreshRoleActivity @accountId = @accountId

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = N'USR_EDIT'
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	if @registrationDocument is not null
	begin
		RAISERROR (N'
		Загружена новая заявка на регистрацию:
		Пользователь: %s (https://www.fbsege.ru/Administration/Accounts/Users/View.aspx?login=%s)
		

		----------------------------------------
		Данное письмо не является сообщением об ошибке, а служит для оповещения операторов.
		', 7, 2, @login, @login) with log
	end

	return 0
end
