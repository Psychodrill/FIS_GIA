-- exec dbo.GetDocument

-- =============================================
-- Получение документа.
-- v.1.0: Created by Makarev Andrey 17.04.2008
-- v.1.1: Modified by Makarev Andrey 19.04.2008
-- Получение данных по внутреннему id.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
CREATE proc dbo.GetDocument
	@id bigint
as
begin

	declare @internalId bigint

	set @internalId = dbo.GetInternalId(@id)

	select
		@id [Id]
		, [document].[Name] [Name]
		, [document].Description Description
		, [document].[Content] [Content]
		, [document].ContentSize ContentSize
		, [document].ContentType ContentType
		, [document].IsActive IsActive
		, [document].ActivateDate ActivateDate
		, [document].ContextCodes ContextCodes
		, [document].RelativeUrl RelativeUrl
	from 
		dbo.[Document] [document] with (nolock, fastfirstrow)
	where
		[document].[Id] = @internalId

	return 0
end
