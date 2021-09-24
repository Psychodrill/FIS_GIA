
-- =============================================
-- Получить внутренний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
CREATE function dbo.GetInternalId
	(
	@externalId bigint
	)
returns bigint
as
begin
	declare
		@result bigint

	if isnull(@externalId, -1) < 0
		return null

	if @externalId = 0
		return 0

	declare
		@base bigint
		, @shift bigint

	set @base = power(2, 20)
	set @shift = 11541954384
	
	set @result = (@externalId / @base) + (@externalId % @base) * @base - @shift
	if dbo.GetExternalId(@result) <> @externalId
		return -1
	return @result
end
