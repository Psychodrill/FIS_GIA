ALTER procedure  [dbo].[SearchMinimalMark]
( @year int = null
, @getAvailableYears bit = 0)
as
begin 

--если настал новый учетный год, то нужно наплодить недостающие минимальные баллы за текущий год
insert into [MinimalMark] (
  [SubjectId],
  [Year],
  [MinimalMark]
) 
select s.[SubjectId], year(getdate()), 0
from [Subject] as s with(nolock)
left join [MinimalMark] as mm with(nolock) on 
mm.[SubjectId] = s.SubjectId 
and mm.year = year(getdate())
where mm.[Id] is null

if @getAvailableYears = 1
  select distinct year from [MinimalMark] as mm with(nolock)
else 
  select mm.id, mm.year, s.[Name], mm.[MinimalMark] 
  from minimalmark mm with(nolock)
  join subject s with(nolock) on s.SubjectId = mm.[SubjectId] and s.[IsActive] = 1
  where mm.year = isnull(@year, mm.year)
  order by mm.year, s.[SortIndex]

end