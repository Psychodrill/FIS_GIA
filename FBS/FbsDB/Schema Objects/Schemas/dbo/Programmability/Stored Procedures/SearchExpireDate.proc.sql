CREATE proc [dbo].[SearchExpireDate]
as
BEGIN
--добавляем новый срок действия в новом году
insert into [ExpireDate] (
	[Year],
	[ExpireDate]
)
select year(getdate()), cast(cast((year(getdate())+1) as varchar(4)) + '1231' as datetime)
where not exists (select top 1 1 from [ExpireDate] ed where ed.year = year(getdate()))

select ed.year, convert(varchar(max), ed.[ExpireDate], 104) ExpireDate
from [ExpireDate] as ed with(nolock)

END

