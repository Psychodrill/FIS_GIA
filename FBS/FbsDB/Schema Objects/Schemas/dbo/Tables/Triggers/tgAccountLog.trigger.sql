
-- =============================================
-- Триггер изменения аккаунта
-- v.1.0: Created by Kazakov Fedor 04.04.2008
-- v.1.1: Modified by Makarev Andrey 10.04.2008
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Добавлено поле UpdateId
-- v.1.3: Modified by Makarev Andrey 06.05.2008
-- Приведение к стандарту оформления
-- v.1.4: Modified by Fomin Dmitriy 08.05.2008
-- Приведено к стандарту.
-- v.1.5: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля HasFixedIp, IsVpnEditorIp.
-- v.1.6: Modified by Sedov Anton 25.06.2008
-- Удалено использование поля 
-- RegistrationDocument таблицы dbo.AccountLog
-- =============================================
CREATE trigger dbo.tgAccountLog
on dbo.Account
for insert, update
as
	if update(UpdateDate) or update(EditorAccountId) or update(UpdateId)
		insert into dbo.AccountLog
			(
			AccountId
			, VersionId
			, UpdateDate
			, UpdateId
			, EditorAccountId
			, EditorIp
			, [Login]
			, PasswordHash
			, OrganizationId
			, IsOrganizationOwner
			, ConfirmYear
			, LastName
			, FirstName
			, PatronymicName
			, Phone
			, Email
			, AdminComment
			, IsActive
			, Status
			, IpAddresses
			, IsActiveChange
			, IsStatusChange
			, IsEdit
			, IsPasswordChange
			, HasFixedIp
			, IsVpnEditorIp
			, HasCrocEgeIntegration
			)
		select
			inserted_account.[Id]
			, (select isnull(max(account_log.VersionId), 0) + 1
				from dbo.AccountLog account_log
				where account_log.AccountId = inserted_account.[Id])
			, inserted_account.UpdateDate
			, inserted_account.UpdateId
			, inserted_account.EditorAccountId
			, inserted_account.EditorIp
			, inserted_account.[Login]
			, inserted_account.PasswordHash
			, inserted_account.OrganizationId
			, inserted_account.IsOrganizationOwner
			, inserted_account.ConfirmYear
			, inserted_account.LastName
			, inserted_account.FirstName
			, inserted_account.PatronymicName
			, inserted_account.Phone
			, inserted_account.Email
			, account.AdminComment
			, inserted_account.IsActive
			, inserted_account.Status
			, inserted_account.IpAddresses
			, case 
				when update(IsActive) and deleted_account.IsActive <> inserted_account.IsActive
					then 1
				else 0
			end
			, case 
				when update([Status]) and deleted_account.Status <> inserted_account.Status
					then 1
				else 0
			end
			, case 
				when update([Login])
						or update(LastName)
						or update(FirstName)
						or update(PatronymicName)
						or update(OrganizationId)
						or update(IsOrganizationOwner)
						or update(ConfirmYear)
						or update(Phone)
						or update(Email)
						or update(IpAddresses)
					then 1
				else 0
			end
			, case 
				when update(PasswordHash)
					then 1
				else 0
			end
			, inserted_account.HasFixedIp
			, case
				when exists(select 1
						from dbo.VpnIp vpn_ip with (nolock)
						where vpn_ip.Ip = inserted_account.EditorIp
							and vpn_ip.IsActive = 1) then 1
				else 0
			end
			, inserted_account.HasCrocEgeIntegration
		from inserted inserted_account
			inner join dbo.Account account
				on account.[id] = inserted_account.[Id]
			left outer join deleted deleted_account
				on deleted_account.[id] = inserted_account.[Id]

GO
DISABLE TRIGGER [dbo].[tgAccountLog]
    ON [dbo].[Account];

