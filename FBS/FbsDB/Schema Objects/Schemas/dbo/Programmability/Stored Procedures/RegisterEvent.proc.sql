-- =============================================
-- Добавление записи в таблицу EventLog.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлено поле UpdateId.
-- v.1.2: Modified by Makarev Andrey 18.04.2008
-- Регистрация событий по нескольким SourceEntityId.
-- v.1.3: Modified by Makarev Andrey 30.04.2008
-- Правильная работа с пустым @sourceEntityIds.
-- =============================================
create proc [dbo].[RegisterEvent]
	@accountId bigint
	, @ip nvarchar(255)
	, @eventCode nvarchar(100)
	, @sourceEntityIds nvarchar(4000)
	, @eventParams ntext
	, @updateId uniqueidentifier = null
as
BEGIN
IF (ISNULL(@sourceEntityIds,'') = '' )
insert dbo.EventLog
		(
		date
		, accountId
		, ip
		, eventCode
		, sourceEntityId
		, eventParams
		, UpdateId
		)
		VALUES (GETDATE(), @accountId , @ip , @eventCode , null, @eventParams , @updateId) 
ELSE				
	insert dbo.EventLog
		(
		date
		, accountId
		, ip
		, eventCode
		, sourceEntityId
		, eventParams
		, UpdateId
		)
	select
		GetDate()
		, @accountId
		, @ip
		, @eventCode
		, ids.value
		, @eventParams
		, @updateId
	from
		dbo.GetDelimitedValues(@sourceEntityIds) ids

	return 0
end

