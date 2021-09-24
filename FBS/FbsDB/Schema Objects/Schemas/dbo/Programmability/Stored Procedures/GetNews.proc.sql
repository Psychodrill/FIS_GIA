-- exec dbo.GetNews

-- =============================================
-- Получить детальную информацию о новости.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим наименование новости.
-- =============================================
CREATE proc dbo.GetNews
	@id bigint
as
begin
	declare @internalId bigint

	set @internalId = dbo.GetInternalId(@id)

	select
		@id [Id]
		, news.Date Date
		, news.Description Description
		, news.[Text] [Text]
		, news.IsActive IsActive
		, news.[Name] [Name]
	from 
		dbo.News news with (nolock, fastfirstrow)
	where
		news.[Id] = @internalId

	return 0
end
