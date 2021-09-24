-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (16, '016__2011_03_02__UniqueChecks')
-- =========================================================================
GO



ALTER FUNCTION  [dbo].[ReportStatisticSubordinateOrg](
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
	UserUpdateDate datetime null,
	CountUniqueChecks int null
)
AS BEGIN

/*

SET @periodBegin = DATEADD(YEAR, -1, GETDATE())
SET @periodEnd = GETDATE()

--Проверки по номеру
DECLARE @NumberChecksByOrg TABLE
(
	OrganizationId INT,
	UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM 
	CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
	O.DepartmentId = @departmentId
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY 
	IOrgReq.OrganizationId

--Проверки по типографскому номеру
DECLARE @TNChecksByOrg TABLE
(
	OrganizationId INT,
	UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM 
	CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE 
	O.DepartmentId = @departmentId
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND cb.IsTypographicNumber = 1
GROUP BY 
	IOrgReq.OrganizationId


--Провекри по паспорту
DECLARE @PassportChecksByOrg TABLE
(
	OrganizationId INT,
	UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM 
	CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
	O.DepartmentId = @departmentId
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND cb.IsTypographicNumber = 0
GROUP BY
	IOrgReq.OrganizationId

--Проверки интерактивные
DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
		ISNULL(ChLog.PassportSeria,'')+
		ISNULL(ChLog.PassportNumber,'')+
		ISNULL(ChLog.CNENumber,'')+
		ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM
	CNEWebUICheckLog ChLog
	INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
	O.DepartmentId = @departmentId
	AND ChLog.EventDate BETWEEN @periodBegin and @periodEnd
GROUP BY
	IOrgReq.OrganizationId

INSERT INTO @Report
SELECT
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name as RegionName,
	O.AccreditationSertificate,
	O.DirectorFullName,
	COUNT(A.Id) CountUser,
	MIN(A.UpdateDate) UserUpdateDate,
	isnull(sum(NCByOrg.UniqueNumberChecks) + 
		sum(TNByOrg.UniqueTNChecks) +
		sum(PByOrg.UniquePassportChecks) +
		sum(UIByOrg.UniqueUIChecks), 0) as CountUniqueChecks
from
	Organization2010 O
	INNER JOIN Region R on R.Id = O.RegionId
	LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
	LEFT JOIN Account A on A.OrganizationId = OrR.Id
	LEFT JOIN @NumberChecksByOrg NCByOrg ON NCByOrg.OrganizationId = O.Id
	LEFT JOIN @TNChecksByOrg TNByOrg ON TNByOrg.OrganizationId = O.Id
	LEFT JOIN @PassportChecksByOrg PByOrg ON PByOrg.OrganizationId = O.Id
	LEFT JOIN @UIChecksByOrg UIByOrg ON UIByOrg.OrganizationId = O.Id
where
	O.DepartmentId = @departmentId
group by
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name,
	O.AccreditationSertificate,
	O.DirectorFullName

*/

-- Количество уникальных проверок
DECLARE @UniqueChecks TABLE
(
	OrganizationId INT,
	UniqueNumberChecks INT
)

INSERT INTO @UniqueChecks
SELECT 
	O.Id,
	COUNT(*) AS UniqueNumberChecks
FROM 
	Organization2010 O 
	inner join OrganizationRequest2010 ORR on ORR.OrganizationId = O.Id
	inner join OrganizationCertificateChecks OCC on OCC.OrganizationId = ORR.Id
WHERE
	O.DepartmentId = @departmentId
GROUP BY 
	O.Id



INSERT INTO @Report
select
	A.Id,
	A.FullName,
	A.RegionId,
	A.RegionName,
	A.AccreditationSertificate,
	A.DirectorFullName,
	A.CountUser,
	A.UserUpdateDate,
	isnull(UC.UniqueNumberChecks, 0) as CountUniqueChecks
from
	(
	select
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
		O.DepartmentId = @departmentId
	group by
		O.Id,
		O.FullName,
		O.RegionId,
		R.Name,
		O.AccreditationSertificate,
		O.DirectorFullName
	) A 
	left join @UniqueChecks UC on UC.OrganizationId = A.Id


RETURN
END
GO