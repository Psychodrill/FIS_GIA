-- =========================================================================
-- ?????? ?????????? ? ??????? ???????? ? ???
insert into Migrations(MigrationVersion, MigrationName) values (31, '031__2011_06_30__AletrProcReportXMLSubordinateOrg')
-- =========================================================================
GO

--??????? ?? ???????????????? ??????????? ??? ???????? ? XML
ALTER FUNCTION  [dbo].[ReportXMLSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	[??? ??] int null,
	[?????? ????????????] nvarchar(Max) null,
	[??? ???????] int null,
	[???????????? ???????] nvarchar(255) null,
	[????????????? ?? ????????????] nvarchar(255) null,
	[??? ????????????] nvarchar(255) null,
	[?????????? ?????????????] int null,
	[???? ????????? ????????????] datetime null,
	[?????????? ?????????? ????????] int null
)
AS BEGIN
INSERT INTO @Report
SELECT
	A.Id [??? ??],
	A.FullName [?????? ????????????],
	A.RegionId [??? ???????] ,
	A.RegionName [???????????? ???????],
	A.AccreditationSertificate [????????????? ?? ????????????],
	A.DirectorFullName [??? ????????????],
	A.CountUser [?????????? ?????????????],
	A.UserUpdateDate [???? ?????????],
	A.CountUniqueChecks [?????????? ????????]
FROM
	dbo.ReportStatisticSubordinateOrg ( null, null, @departmentId) A
	inner join dbo.Organization2010 O on O.Id = A.Id
ORDER BY
	case when O.MainId is null then O.Id else O.MainId end, O.MainId, A.FullName
	
RETURN
END