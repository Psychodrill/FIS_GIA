

-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[GetDeliveryRecipients]
	@id bigint
as
begin
	select
		recipients.RecipientCode RecipientCode
	from 
		dbo.DeliveryRecipients recipients with (nolock)
	where
		recipients.[DeliveryId] = @id

	return 0
end








