
-- =============================================
-- Триггер изменения ключа доступа
-- v.1.0: Created by Fomin Dmitriy 01.09.2008
-- =============================================
CREATE trigger dbo.tgAccountKeyLog
on dbo.AccountKey
for insert, update
as
	if update(UpdateDate) or update(EditorAccountId) or update(UpdateId)
		insert into dbo.AccountKeyLog
			(
			AccountKeyId
			, VersionId
			, UpdateDate
			, UpdateId
			, EditorAccountId
			, EditorIp
			, [Key]
			, DateFrom
			, DateTo
			, IsActive
			)
		select
			inserted_account_key.Id
			, (select isnull(max(account_log.VersionId), 0) + 1
				from dbo.AccountKeyLog account_log
				where account_log.AccountKeyId = inserted_account_key.[Id])
			, inserted_account_key.UpdateDate
			, inserted_account_key.UpdateId
			, inserted_account_key.EditorAccountId
			, inserted_account_key.EditorIp
			, inserted_account_key.[Key]
			, inserted_account_key.DateFrom
			, inserted_account_key.DateTo
			, inserted_account_key.IsActive
		from inserted inserted_account_key
