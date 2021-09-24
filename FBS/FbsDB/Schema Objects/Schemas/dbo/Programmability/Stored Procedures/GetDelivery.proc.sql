

-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[GetDelivery]
	@id bigint
as
begin
	select
		@id [Id]
		, delivery.Title Title
		, delivery.[Message] [Message]
		, delivery.[CreateDate] [CreateDate]
		, delivery.DeliveryDate DeliveryDate
		, delivery.TypeCode TypeCode
	from 
		dbo.Delivery delivery with (nolock, fastfirstrow)
	where
		delivery.[Id] = @id

	return 0
end




