-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (30, '030__2011_06_30__AlterProcReportCertificateLoadTVF')
-- =========================================================================
GO

--�����: ���������� ����� � �������� ������������
ALTER FUNCTION [dbo].[ReportCertificateLoadTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[��� �������] nvarchar(10) null,
	[������] nvarchar(100) null,
[����� ������������ �� 2008] int null,
[����� ������������ �� 2009] int null,
[����� ������������ �� 2010] int null,
[����� ������������ �� 2011] int null,
[����� ������������] int null,
[����� ������������] int null,
[��������� ������������] int null,
[����� �������������� ������������] int null,
[����� �������������� ������������] int null,
[��������� �������������� ������������] int null
)
as
begin

insert into @report
select
	r.code [��� �������]
	, r.name [������]
, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2008) 
[����� ������������ �� 2008]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2009) 
[����� ������������ �� 2009]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2010) 
[����� ������������ �� 2010]	
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2011) 
[����� ������������ �� 2011]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id) 
[����� ������������]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.createdate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[����� ������������]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.updatedate <> c.createdate and c.updatedate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[��������� ������������]
	, (select count(*) from CommonNationalExamCertificateDeny d with(nolock)
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where c.regionid = r.id) 
[����� �������������� ������������]
	, (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[����� �������������� ������������]
	, (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate <> d.updatedate AND d.updatedate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[��������� �������������� ������������]
from dbo.Region r with (nolock)
where r.InCertificate = 1
order by r.id asc


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '', '����� �� ' + case @days when 1 then '24 ����' else cast(@days as varchar(10)) + ' ����' end
,sum([����� ������������ �� 2008])
,sum([����� ������������ �� 2009])
,sum([����� ������������ �� 2010])
,sum([����� ������������ �� 2011])
,sum([����� ������������])
,sum([����� ������������])
,sum([��������� ������������])
,sum([����� �������������� ������������])
,sum([����� �������������� ������������])
,sum([��������� �������������� ������������])
from @report


return
end
