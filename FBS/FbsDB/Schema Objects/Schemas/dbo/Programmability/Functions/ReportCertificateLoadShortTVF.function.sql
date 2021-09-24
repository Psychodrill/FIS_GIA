--Отчет: Общий отчет о загрузке сертификатов
CREATE FUNCTION [dbo].[ReportCertificateLoadShortTVF]()
RETURNS @report TABLE 
(
	[Код региона] nvarchar(10) null,
	[Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств за 2012] int null,
[Всего свидетельств] int null
)
as
begin
declare @PreResult table 
(
	[Код региона] nvarchar(10) null,
	[Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств за 2012] int null,
[Всего свидетельств] int null
)
insert into @PreResult
select
	replace(r.code,1000,'-') [Код региона]
	, r.name [Регион]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2008) 
[Всего свидетельств за 2008]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2009) 
[Всего свидетельств за 2009]	
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2010) 
[Всего свидетельств за 2010]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2011) 
[Всего свидетельств за 2011]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2012) 
[Всего свидетельств за 2012]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id) 
[Всего свидетельств]
	
from dbo.Region r with (nolock)
where r.InCertificate = 1


insert into @report
select * from @PreResult 
union
select 'Всего',
'-',
SUM([Всего свидетельств за 2008]),
SUM([Всего свидетельств за 2009]),
SUM([Всего свидетельств за 2010]),
SUM([Всего свидетельств за 2011]),
SUM([Всего свидетельств за 2012]),
SUM([Всего свидетельств])
from @PreResult
order by [Код региона]

return
end
