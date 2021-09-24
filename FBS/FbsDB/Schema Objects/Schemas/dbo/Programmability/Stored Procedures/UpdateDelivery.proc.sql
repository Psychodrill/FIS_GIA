-- =============================================
-- Создание или редактирование рассылки.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[UpdateDelivery]
	@id bigint output
	, @title nvarchar(255)
	, @message nvarchar(4000)
	, @deliveryDate datetime
	, @deliveryType nvarchar(20)
	, @recipientIds nvarchar(max)
	, @editorLogin nvarchar(255) 
	, @editorIp nvarchar(255)
as
begin
	declare @eventCode nvarchar(100)
	
	if isnull(@id, 0) = 0 -- новая рассылка
	begin
		insert dbo.Delivery
			(
			Title
			, [Message]
			, DeliveryDate
			, TypeCode
			)
		select
			@title
			, @message
			, @deliveryDate
			, @deliveryType

		set @id = scope_identity()
		set @eventCode= N'DLV_CREATE'
	end	
	else 
	begin -- update существующей рассылки
		update delivery
		set
			Title = @title
			, [Message] = @message
			, DeliveryDate = @deliveryDate
			, TypeCode = @deliveryType
		from
			dbo.Delivery delivery with (rowlock)
		where
			delivery.[Id] = @id
		
		set @eventCode= N'DLV_EDIT'
	end	

	--Удалим старых получателей рассылки
	delete from dbo.DeliveryRecipients where DeliveryId = @id
	
	if (@recipientIds is not null)
	begin
		--[value] - recipientCode, @internalId - Id рассылки
		insert into dbo.DeliveryRecipients select [value],@id from dbo.GetDelimitedValues(@recipientIds)
	end

	declare @editorAccountId bigint
	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	declare @updateId uniqueidentifier
	set @updateId = newid()

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @id
		, @eventParams = null
		, @updateId = @updateId

	return 0
end




