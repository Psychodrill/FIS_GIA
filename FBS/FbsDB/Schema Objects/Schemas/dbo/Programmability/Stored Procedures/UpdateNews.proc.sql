-- exec dbo.UpdateNews

-- =============================================
-- Сохранение изменений новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE proc dbo.UpdateNews
	@id bigint output
	, @date datetime
	, @name nvarchar(255)
	, @description ntext
	, @text ntext
	, @isActive bit
	, @editorLogin nvarchar(255) = null
	, @editorIp nvarchar(255) = null
as
begin
	declare 
		@newNews bit
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
	
	set @currentDate = getdate()

	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	if isnull(@id, 0) = 0 -- новая новость
	begin
		set @newNews = 1
		set @eventCode = N'NWS_CREATE'
	end
	else
	begin -- update существующего документа
		set @internalId = dbo.GetInternalId(@id)

		select 
			@oldIsActive = news.IsActive
		from 
			dbo.News news with (nolock, fastfirstrow)
		where
			news.[Id] = @internalId

		set @eventCode = N'NWS_EDIT'

		if @oldIsActive <> @isActive
		begin
			set @public = 1
			if @isActive = 1
				set @publicEventCode = N'DOC_PUBLIC'
			else
				set @publicEventCode = N'DOC_UNPUBLIC'
		end
	end

	begin tran insert_update_news_tran

		if @newNews = 1 -- новая новость
		begin
			insert dbo.News
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, Date
				, [Name]
				, Description
				, [Text]
				, IsActive
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @date
				, @name
				, @description
				, @text
				, @isActive

			if (@@error <> 0)
				goto undo

			set @internalId = scope_identity()
			set @id = dbo.GetExternalId(@internalId)

			if (@@error <> 0)
				goto undo
		end	
		else 
		begin -- update существующей новости

			update news
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, EditorIp = @editorIp
				, Date = @date
				, [Name] = @name
				, Description = @description
				, [Text] = @text
				, IsActive = @isActive
			from
				dbo.News news with (rowlock)
			where
				news.[Id] = @internalId

			if (@@error <> 0)
				goto undo
		end	

	if @@trancount > 0
		commit tran insert_update_news_tran

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

	rollback tran insert_update_news_tran

	return 1

end
