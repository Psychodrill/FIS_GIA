create proc [dbo].[UpdateMinimalMarks]
( @MinimalMarksString varchar(4000))
as
begin
update mm
set minimalmark = nmm.mark
from [MinimalMark] as mm
join GetSubjectMarks(@MinimalMarksString) nmm on nmm.[SubjectId] = mm.[Id] --не очень красиво читается
end