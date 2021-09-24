-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (60, '060_2012_07_01_ModifyReport.sql')
-- =========================================================================
GO


--�����: ����� ����� � �������� ������������
ALTER FUNCTION [dbo].[ReportCertificateLoadShortTVF]()
RETURNS @report TABLE 
(
	[��� �������] nvarchar(10) null,
	[������] nvarchar(100) null,
[����� ������������ �� 2008] int null,
[����� ������������ �� 2009] int null,
[����� ������������ �� 2010] int null,
[����� ������������ �� 2011] int null,
[����� ������������ �� 2012] int null,
[����� ������������] int null
)
as
begin
declare @PreResult table 
(
	[��� �������] nvarchar(10) null,
	[������] nvarchar(100) null,
[����� ������������ �� 2008] int null,
[����� ������������ �� 2009] int null,
[����� ������������ �� 2010] int null,
[����� ������������ �� 2011] int null,
[����� ������������ �� 2012] int null,
[����� ������������] int null
)
insert into @PreResult
select
	replace(r.code,1000,'-') [��� �������]
	, r.name [������]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2008) 
[����� ������������ �� 2008]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2009) 
[����� ������������ �� 2009]	
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2010) 
[����� ������������ �� 2010]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2011) 
[����� ������������ �� 2011]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2012) 
[����� ������������ �� 2012]
	, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id) 
[����� ������������]
	
from dbo.Region r with (nolock)
where r.InCertificate = 1


insert into @report
select * from @PreResult 
union
select '�����',
'-',
SUM([����� ������������ �� 2008]),
SUM([����� ������������ �� 2009]),
SUM([����� ������������ �� 2010]),
SUM([����� ������������ �� 2011]),
SUM([����� ������������ �� 2012]),
SUM([����� ������������])
from @PreResult
order by [��� �������]

return
end
