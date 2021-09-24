-- exec dbo.UpdateAskedQuestion

-- =============================================
-- Сохранение изменений вопроса.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc dbo.UpdateAskedQuestion
	@id bigint output
	, @name nvarchar(255)
	, @question ntext
	, @answer ntext
	, @isActive bit
	, @contextCodes nvarchar(4000)
	, @editorLogin nvarchar(255) = null
	, @editorIp nvarchar(255) = null
as
begin
	declare 
		@newAskedQuestion bit
		, @currentDate datetime
		, @editorAccountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @oldIsActive bit
		, @public bit
		, @publicEventCode nvarchar(100)
		, @internalId bigint

	set @updateId = newid()

	declare @oldContextId table 
		(
		ContextId int
		)

	declare @newContextId table 
		(
		ContextId int
		)

	insert @newContextId
	select 
		context.Id
	from 
		dbo.GetDelimitedValues(@contextCodes) codes
			inner join dbo.Context context
				on context.Code = codes.[value]

	set @currentDate = getdate()

	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	if isnull(@id, 0) = 0 -- новая новость
	begin
		set @newAskedQuestion = 1
		set @eventCode = N'FAQ_CREATE'
	end
	else
	begin -- update существующей новости
		set @internalId = dbo.GetInternalId(@id)

		select 
			@oldIsActive = asked_question.IsActive
		from 
			dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
		where
			asked_question.[Id] = @internalId

		insert @oldContextId
			(
			ContextId
			)
		select
			asked_question_context.ContextId
		from
			dbo.AskedQuestionContext asked_question_context with (nolock)
		where
			asked_question_context.AskedQuestionId = @internalId

		set @eventCode = N'FAQ_EDIT'

		if @oldIsActive <> @isActive
		begin
			set @public = 1
			if @isActive = 1
				set @publicEventCode = N'FAQ_PUBLIC'
			else
				set @publicEventCode = N'FAQ_UNPUBLIC'
		end
	end

	begin tran insert_update_faq_tran

		if @newAskedQuestion = 1 -- новый документ
		begin
			insert dbo.AskedQuestion
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Name]
				, Question
				, Answer
				, IsActive
				, ViewCount
				, Popularity
				, ContextCodes
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @name
				, @question
				, @answer
				, @isActive
				, 0
				, 0
				, @contextCodes

			if (@@error <> 0)
				goto undo

			set @internalId = scope_identity()
			set @id = dbo.GetExternalId(@internalId)

			if (@@error <> 0)
				goto undo

			insert dbo.AskedQuestionContext
				(
				AskedQuestionId
				, ContextId
				)
			select
				@internalId
				, new_context_id.ContextId
			from 
				@newContextId new_context_id

			if (@@error <> 0)
				goto undo

		end	
		else 
		begin -- update существующего вопроса

			update asked_question
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, EditorIp = @editorIp
				, [Name] = @name
				, Question = @question
				, Answer = @answer
				, IsActive = @isActive
				, ContextCodes = @contextCodes
			from
				dbo.AskedQuestion asked_question with (rowlock)
			where
				asked_question.[Id] = @internalId

			if (@@error <> 0)
				goto undo

			if exists(select 
						1
					from
						@oldContextId old_context_id
							full outer join @newContextId new_context_id
								on old_context_id.ContextId = new_context_id.ContextId
					where
						old_context_id.ContextId is null
						or new_context_id.ContextId is null) 
			begin
				delete asked_question_context
				from 
					dbo.AskedQuestionContext asked_question_context
				where
					asked_question_context.AskedQuestionId = @internalId

				if (@@error <> 0)
					goto undo

				insert dbo.AskedQuestionContext
					(
					AskedQuestionId
					, ContextId
					)
				select
					@internalId
					, new_context_id.ContextId
				from 
					@newContextId new_context_id

				if (@@error <> 0)
					goto undo
			end
		end	

	if @@trancount > 0
		commit tran insert_update_faq_tran

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	if @public = 1
		exec dbo.RegisterEvent 
			@accountId = @editorAccountId
			, @ip = @editorIp
			, @eventCode = @publicEventCode
			, @sourceEntityIds = @ids
			, @eventParams = null
			, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_update_faq_tran

	return 1

end
