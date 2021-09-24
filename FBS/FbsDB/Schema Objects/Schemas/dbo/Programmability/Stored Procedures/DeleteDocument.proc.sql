-- exec dbo.DeleteDocument

-- =============================================
-- Удаление документов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc dbo.DeleteDocument
	@ids nvarchar(255)
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
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	set @eventCode = N'DOC_DEL'

	begin tran delete_document_tran

		delete document_context
		from 
			dbo.DocumentContext document_context
				inner join @idTable idTable
					on document_context.DocumentId = idTable.[id]

		if (@@error <> 0)
			goto undo

		delete [document]
		from 
			dbo.[Document] [document]
				inner join @idTable idTable
					on [document].[Id] = idTable.[id]

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran delete_document_tran

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0

	undo:

	rollback tran delete_document_tran

	return 1

end
