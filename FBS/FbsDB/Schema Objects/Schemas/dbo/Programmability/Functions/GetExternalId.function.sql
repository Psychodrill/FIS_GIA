
-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
CREATE function [dbo].[GetExternalId]
	(
	@internalId bigint
	)
returns bigint
as
begin
	if isnull(@internalId, -1) < 0
		return null
	if @internalId = 0
		return 0

	declare
		@base bigint
		, @shift bigint
		, @shiftedId bigint

	set @base = power(2, 20)
	set @shift = 11541954384
	
	set @shiftedId = @internalId + @shift
	return (@shiftedId / @base) + (@shiftedId % @base) * @base
end
