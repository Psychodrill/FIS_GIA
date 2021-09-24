-- exec dbo.GetAccountLog

-- =============================================
-- Получить лог учетной записи.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- =============================================
CREATE procedure dbo.GetAccountLog
	@login nvarchar(255)
	, @versionId int
as
begin

	select
		account_log.[Login] [Login]
		, account_log.VersionId VersionId
		, account_log.UpdateDate UpdateDate
		, editor.[Login] EditorLogin
		, editor.LastName EditorLastName
		, editor.FirstName EditorFirstName
		, editor.PatronymicName EditorPatronymicName
		, account_log.EditorIp EditorIp
		, account_log.IsVpnEditorIp IsVpnEditorIp
		, account_log.LastName LastName
		, account_log.FirstName FirstName
		, account_log.PatronymicName PatronymicName
		, account_log.Phone Phone
		, account_log.Email Email
		, account_log.IpAddresses IpAddresses
		, account_log.HasFixedIp HasFixedIp
		, account_log.IsActive IsActive
	from
		dbo.AccountLog account_log with (nolock, fastfirstrow)
			inner join dbo.Account account with (nolock, fastfirstrow)
				on account_log.AccountId = account.[Id]
			left outer join dbo.Account editor with (nolock, fastfirstrow)
				on editor.Id = account_log.EditorAccountId
	where
		account.[Login] = @login
		and account_log.VersionId = @versionId

	return 0
end
