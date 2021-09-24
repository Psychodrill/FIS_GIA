-- exec dbo.SearchContext

-- =============================================
-- Поиск контекстов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Приведение к стандарту.
-- =============================================
CREATE proc dbo.SearchContext
as
begin

	select 
		context.Code Code
		, context.[Name] [Name]
	from 
		dbo.Context context with (nolock)

	return 0
end
