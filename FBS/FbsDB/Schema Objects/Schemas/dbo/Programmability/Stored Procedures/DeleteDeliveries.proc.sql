-- exec dbo.DeleteNews

-- =============================================
-- Удаление рассылок.
-- v.1.0: Created by Yusupov Kirill 19.04.2010
-- =============================================

CREATE proc [dbo].[DeleteDeliveries]
	@ids nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select [value] from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @innerIds nvarchar(4000)

	

	set @updateId = newid()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	set @eventCode = N'DLV_DEL'

	--Удалим получателей рассылки
	delete recipient
		from 
			dbo.DeliveryRecipients recipient
				inner join @idTable idTable
					on recipient.DeliveryId = idTable.[id]

	--Удалим лог рассылки
	delete [log]
		from 
			dbo.DeliveryLog [log]
				inner join @idTable idTable
					on [log].DeliveryId = idTable.[id]

	--Удалим саму рассылку
	delete delivery
	from 
		dbo.Delivery delivery
			inner join @idTable idTable
				on delivery.Id = idTable.[id]


	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId
		
	return 0
end




