-- exec dbo.DeleteAskedQuestion

-- =============================================
-- Удаление вопросов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc dbo.DeleteAskedQuestion
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
	
	set @eventCode = N'FAQ_DEL'

	begin tran delete_faq_tran

		delete asked_question_context
		from 
			dbo.AskedQuestionContext asked_question_context
				inner join @idTable idTable
					on asked_question_context.AskedQuestionId = idTable.[id]

		if (@@error <> 0)
			goto undo

		delete asked_question
		from 
			dbo.AskedQuestion asked_question
				inner join @idTable idTable
					on asked_question.[Id] = idTable.[id]

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran delete_faq_tran

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0

	undo:

	rollback tran delete_faq_tran

	return 1

end
