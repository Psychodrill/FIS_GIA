create proc [dbo].[UpdateExpireDates]
(@ExpireDatesString varchar(max))
as
begin

update ed
set expiredate = convert(datetime, substring(t.value, charindex('=',t.value)+1, len(t.value)), 104)
from [ExpireDate] as ed 
join getdelimitedvalues(@ExpireDatesString) t on ed.[Year] = substring(t.value, 0, charindex('=',t.value))
end