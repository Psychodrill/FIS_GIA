CREATE proc [dbo].[ReportCnecLoading] 
( @type varchar(10) = null)
as
begin

if(@type is null or @type not in ('month', 'week'))
	set @type = 'month'

select  
  day(n.value('date[1]', 'datetime')) day
, convert(varchar(10), n.value('date[1]', 'datetime'), 104) date
, n.value('cnecNew[1]', 'int') cnecNew
, n.value('cnecUpdated[1]', 'int') cnecUpdated
, n.value('cnecdNew[1]', 'int') cnecdNew
, n.value('cnecdUpdated[1]', 'int') cnecdUpdated
from report rp
cross apply rp.xml.nodes('unit') r(n)
where name = 'CnecLoading' + @type 
and rp.created = (select top 1 created from report where name = 'CnecLoading' + @type order by created desc)	

end