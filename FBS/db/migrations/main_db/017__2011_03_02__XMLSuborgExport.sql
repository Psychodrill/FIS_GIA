-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (17, '017__2011_03_02__XMLSuborgExport')
-- =========================================================================
GO



--Функция по подведомственным учреждениям для экспорта в XML
ALTER FUNCTION  [dbo].[ReportXMLSubordinateOrg](
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
	[Дата активации пользователя] datetime null,
	[Количество уникальных проверок] int null
)
AS BEGIN
INSERT INTO @Report
SELECT
	A.Id [Код ОУ],
	A.FullName [Полное наименование],
	A.RegionId [Код региона] ,
	A.RegionName [Наименование региона],
	A.AccreditationSertificate [Свидетельство об аккредитации],
	A.DirectorFullName [ФИО руководителя],
	A.CountUser [Количество пользователей],
	A.UserUpdateDate [Дата активации],
	A.CountUniqueChecks [Уникальных проверок]
FROM
	dbo.ReportStatisticSubordinateOrg ( null, null, 7367) A
	inner join dbo.Organization2010 O on O.Id = A.Id
ORDER BY
	case when O.MainId is null then O.Id else O.MainId end, O.MainId, A.FullName
	
RETURN
END
GO