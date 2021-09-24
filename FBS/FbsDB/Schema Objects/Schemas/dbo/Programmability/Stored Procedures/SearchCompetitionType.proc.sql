
-- exec dbo.SearchCompetitionType
-- =============================================
-- Процедура поиска типов олимпиад
-- v.1.0. Created by Fomin Dmitriy 23.07.2008
-- v.1.1. Modified by Fomin Dmitriy 25.07.2008
-- Не нужно возвращать пустое значение.
-- =============================================
CREATE procedure dbo.SearchCompetitionType
as
begin
	select
		competition_type.Id
		, competition_type.Name
	from 
		dbo.CompetitionType competition_type
	order by
		[Name]

	return 0
end
