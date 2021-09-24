-- exec dbo.GetDocumentByUrl

-- =============================================
-- Получение документа по относительному Url.
-- v.1.0: Created by Fomin Dmitriy 24.04.2008
-- =============================================
CREATE proc dbo.GetDocumentByUrl
	@relativeUrl nvarchar(255)
as
begin
	select top 1
		dbo.GetExternalId([document].Id) Id
	from 
		dbo.Document [document] with (nolock, fastfirstrow)
	where 
		[document].RelativeUrl = @relativeUrl
		and [document].IsActive = 1

end
