-- exec dbo.UpdateDocument

-- =============================================
-- Сохранение изменений документа.
-- v.1.0: Created by Makarev Andrey 18.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Передаются не ИД контекстов, а коды.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
CREATE proc dbo.UpdateDocument
	@id bigint output
	, @name nvarchar(255)
	, @description ntext
	, @content image
	, @contentSize int
	, @contentType nvarchar(255)
	, @isActive bit
	, @contextCodes nvarchar(4000)
	, @relativeUrl nvarchar(255)
	, @editorLogin nvarchar(255) = null
	, @editorIp nvarchar(255) = null
as
begin
	declare 
		@newDocument bit
		, @currentDate datetime
		, @editorAccountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @activateDate datetime
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

	if isnull(@id, 0) = 0 -- новый документ
	begin
		set @newDocument = 1
		set @eventCode = N'DOC_CREATE'
		if @isActive = 1
			set @activateDate = @currentDate
	end
	else
	begin -- update существующего документа
		set @internalId = dbo.GetInternalId(@id)

		select 
			@oldIsActive = [document].IsActive
		from 
			dbo.[Document] [document] with (nolock, fastfirstrow)
		where
			[document].[Id] = @internalId

		insert @oldContextId
			(
			ContextId
			)
		select
			document_context.ContextId
		from
			dbo.DocumentContext document_context with (nolock)
		where
			document_context.DocumentId = @internalId

		set @eventCode = N'DOC_EDIT'

		if @oldIsActive <> @isActive
		begin
			set @public = 1
			if @isActive = 1
				select 
					@publicEventCode = N'DOC_PUBLIC'
					, @activateDate = @currentDate
			else
				select 
					@publicEventCode = N'DOC_UNPUBLIC'
					, @activateDate = null
		end
	end

	begin tran insert_update_document_tran

		if @newDocument = 1 -- новый документ
		begin
			insert dbo.[Document]
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Name]
				, Description
				, [Content]
				, ContentSize
				, ContentType
				, IsActive
				, ActivateDate
				, ContextCodes
				, RelativeUrl
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @name
				, @description
				, @content
				, @contentSize
				, @contentType
				, @isActive
				, @activateDate
				, @contextCodes
				, @relativeUrl

			if (@@error <> 0)
				goto undo

			set @internalId = scope_identity()
			set @id = dbo.GetExternalId(@internalId)

			if (@@error <> 0)
				goto undo

			insert dbo.DocumentContext
				(
				DocumentId
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
		begin -- update существующего документа

			update [document]
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, EditorIp = @editorIp
				, [Name] = @name
				, Description = @description
				, [Content] = @content
				, ContentSize = @contentSize
				, ContentType = @contentType
				, IsActive = @isActive
				, ActivateDate = case
						when @public = 1 then @activateDate
						else [document].ActivateDate
				end
				, ContextCodes = @contextCodes
				, RelativeUrl = @relativeUrl
			from
				dbo.[Document] [document] with (rowlock)
			where
				[document].[Id] = @internalId

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
				delete document_context
				from 
					dbo.DocumentContext document_context
				where
					document_context.DocumentId = @internalId

				if (@@error <> 0)
					goto undo

				insert dbo.DocumentContext
					(
					DocumentId
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
		commit tran insert_update_document_tran

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

	rollback tran insert_update_document_tran

	return 1

end
