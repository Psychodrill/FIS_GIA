
--------------------------------------------------
-- Разбивает исходную строку на части, разделенные 
-- запятыми и знаками =.
-- v.1.0: Created by Makarev Andrey 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Приведение к стандарту.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал через xml для оптимизации
--------------------------------------------------
CREATE function [dbo].[GetSubjectMarks_OLD]
	(
	@subjectMarks nvarchar(4000)
	)
returns @SubjectMark table (SubjectId int, Mark numeric(5,1))
as
begin
	if len(ltrim(rtrim(@subjectMarks))) > 0
	begin
		DECLARE @x xml
		set @x = '<root><m k="' + replace(replace(@subjectMarks, '=', '" v="'), ',', '"/><m k="') + '"/></root>'
		insert into @SubjectMark
		SELECT  T.c.value('@k', 'numeric(5,1)') AS id,
				T.c.value('@v', 'numeric(5,1)') AS mark
		FROM    @x.nodes('/root/m') T ( c )
	end
	return
end
