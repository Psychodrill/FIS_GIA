-- exec dbo.GetSubject

-- =============================================
-- Получение списка предметов.
-- v.1.0: Created by Makarev Andrey 04.05.2008
-- v.1.1: Modified by Makarev Andrey 05.05.2008
-- Добавлены хинты.
-- v.1.2: Modified by Fomin Dmitriy 30.05.2008
-- Отдавать ИД без шифрования.
-- =============================================
alter proc dbo.GetSubject
as
begin
	select
		[subject].SubjectId [Id], [subject].[Code] Code
		, [subject].[Name] [Name]
	from
		dbo.Subject [subject] with (nolock)
	where
		[subject].IsActive = 1
	order by 
		[subject].SortIndex

	return 0
end
