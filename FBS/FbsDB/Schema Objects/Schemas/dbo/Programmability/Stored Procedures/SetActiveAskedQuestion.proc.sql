-- exec dbo.SetActiveAskedQuestion

-- =============================================
-- Установка активности вопроса.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc dbo.SetActiveAskedQuestion
	@ids nvarchar(255)
	, @isActive bit
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @currentDate datetime
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()
	set @currentDate = getdate()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	if @isActive = 1
		set @eventCode = N'FAQ_PUBLIC'
	else
		set	@eventCode = N'FAQ_UNPUBLIC'

	update asked_question
	set
		UpdateDate = @currentDate
		, UpdateId = @updateId
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, IsActive = @isActive
	from 
		dbo.AskedQuestion asked_question with (rowlock)
			inner join @idTable idTable
				on asked_question.[id] = idTable.[id]

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0
end
