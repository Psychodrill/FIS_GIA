-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (30, '030__2011_06_30__AlterProcReportCertificateLoadTVF')
-- =========================================================================
GO

--Отчет: Ежедневный отчет о загрузке сертификатов
ALTER FUNCTION [dbo].[ReportCertificateLoadTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[Код региона] nvarchar(10) null,
	[Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств] int null,
[Новых свидетельств] int null,
[Обновлено свидетельств] int null,
[Всего аннулированных свидетельств] int null,
[Новых аннулированных свидетельств] int null,
[Обновлено аннулированных свидетельств] int null
)
as
begin

insert into @report
select
	r.code [Код региона]
	, r.name [Регион]
, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2008) 
[Всего свидетельств за 2008]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2009) 
[Всего свидетельств за 2009]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2010) 
[Всего свидетельств за 2010]	
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2011) 
[Всего свидетельств за 2011]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id) 
[Всего свидетельств]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.createdate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[Новых свидетельств]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.updatedate <> c.createdate and c.updatedate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[Обновлено свидетельств]
	, (select count(*) from CommonNationalExamCertificateDeny d with(nolock)
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where c.regionid = r.id) 
[Всего аннулированных свидетельств]
	, (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[Новых аннулированных свидетельств]
	, (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate <> d.updatedate AND d.updatedate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[Обновлено аннулированных свидетельств]
from dbo.Region r with (nolock)
where r.InCertificate = 1
order by r.id asc


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '', 'Итого за ' + case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
,sum([Всего свидетельств за 2008])
,sum([Всего свидетельств за 2009])
,sum([Всего свидетельств за 2010])
,sum([Всего свидетельств за 2011])
,sum([Всего свидетельств])
,sum([Новых свидетельств])
,sum([Обновлено свидетельств])
,sum([Всего аннулированных свидетельств])
,sum([Новых аннулированных свидетельств])
,sum([Обновлено аннулированных свидетельств])
from @report


return
end
