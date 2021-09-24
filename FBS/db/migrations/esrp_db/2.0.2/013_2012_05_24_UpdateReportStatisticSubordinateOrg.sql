-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (13, '013_2012_05_24_UpdateReportStatisticSubordinateOrg')
-- =========================================================================

/****** Object:  UserDefinedFunction [dbo].[ReportStatisticSubordinateOrg]    Script Date: 05/23/2012 18:19:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportStatisticSubordinateOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportStatisticSubordinateOrg]
GO



CREATE FUNCTION  [dbo].[ReportStatisticSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	Id int null,
	FullName nvarchar(Max) null,
	RegionId int null,
	RegionName nvarchar(255) null,
	AccreditationSertificate nvarchar(255) null,
	DirectorFullName nvarchar(255) null,
	CountUser int null,
	UserUpdateDate datetime null
)
AS BEGIN

SET @periodBegin = DATEADD(YEAR, -1, GETDATE())
SET @periodEnd = GETDATE()



INSERT INTO @Report
SELECT
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name as RegionName,
	O.AccreditationSertificate,
	O.DirectorFullName,
	COUNT(A.Id) CountUser,
	MIN(A.UpdateDate) UserUpdateDate
from
	Organization2010 O
	INNER JOIN Region R on R.Id = O.RegionId
	LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
	LEFT JOIN Account A on A.OrganizationId = OrR.Id
where
	O.DepartmentId = @departmentId OR o.MainId = @departmentId
	
	
group by
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name,
	O.AccreditationSertificate,
	O.DirectorFullName

RETURN
END
GO

/****** Object:  UserDefinedFunction [dbo].[ReportXMLSubordinateOrg]    Script Date: 05/23/2012 18:19:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportXMLSubordinateOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportXMLSubordinateOrg]
GO


--Функция по подведомственным учреждениям для экспорта в XML
CREATE FUNCTION  [dbo].[ReportXMLSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	[Код ОУ] int null,
	[Полное наименование] nvarchar(Max) null,
	[Код региона] int null,
	[Наименование региона] nvarchar(255) null,
	[Свидетельство об аккредитации] nvarchar(255) null,
	[ФИО руководителя] nvarchar(255) null,
	[Количество пользователей] int null,
	[Дата активации пользователя] datetime null
)
AS BEGIN
INSERT INTO @Report
SELECT
	Id [Код ОУ],
	FullName [Полное наименование],
	RegionId [Код региона] ,
	RegionName [Наименование региона],
	AccreditationSertificate [Свидетельство об аккредитации],
	DirectorFullName [ФИО руководителя],
	CountUser [Количество пользователей],
	UserUpdateDate [Дата активации]
FROM
	dbo.ReportStatisticSubordinateOrg ( @periodBegin, @periodEnd, @departmentId)
RETURN
END

