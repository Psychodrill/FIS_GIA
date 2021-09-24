create function [dbo].[ReportCheckedCNEsDetailedTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Номер свидетельства] NVARCHAR(255)
,[Регион свидетельства] NVARCHAR(500)
,[Количество проверок] INT
,[Проверяющее ОУ] NVARCHAR(4000)
,[Тип ОУ/ОПФ ОУ] NVARCHAR(255)
,[Регион ОУ] NVARCHAR(500)
--,[Дата первой проверки] INT
)
AS 
BEGIN

DECLARE @BaseReport TABLE 
(
	CNENumber NVARCHAR(255),
	CNEId BIGINT,
	OrgId INT
)
INSERT INTO @BaseReport(CNENumber,CNEId,OrgId)
SELECT CNENumber,CNEId,OrgId FROM [ReportCheckedCNEsBASE]()

DECLARE @ChecksCount TABLE
(
	CNEId BIGINT,
	Checks INT
)
INSERT INTO @ChecksCount
SELECT 
	CNEId AS CNEId,
	COUNT(OrgId) AS OrgCount
FROM @BaseReport AS IChecks
GROUP BY IChecks.CNEId HAVING COUNT(IChecks.OrgId)>=6

DECLARE @ReportWithoutOrder TABLE
(
[Номер свидетельства] NVARCHAR(255)
,[Регион свидетельства] NVARCHAR(500)
,[Количество проверок] INT
,[Проверяющее ОУ] NVARCHAR(4000)
,[Тип ОУ/ОПФ ОУ] NVARCHAR(255)
,[Регион ОУ] NVARCHAR(500)
)

INSERT INTO @ReportWithoutOrder
SELECT 
IChecks.CNENumber AS [Номер свидетельства]
,IChecks.CNERegName AS [Регион свидетельства]
,IChecks.OrgCount AS [Количество проверок]
,IChecks.OrgName AS [Проверяющее ОУ]
,IChecks.OrgType+'/'+CASE WHEN IChecks.OPF=1 THEN 'Негосударственный' ELSE 'Государственный' END AS [Тип ОУ/ОПФ ОУ]
,IChecks.OrgRegName AS [Регион ОУ]
FROM 
(
	SELECT 
	CNENumber,
	Org.FullName AS OrgName,
	OrgType.Name AS OrgType,
	Org.IsPrivate AS OPF,
	OrgReg.Name AS OrgRegName,
	CNEReg.Name AS CNERegName,	
	CntRpt.Checks AS OrgCount
	FROM @ChecksCount CntRpt
	INNER JOIN @BaseReport AS Rpt ON CntRpt.CNEId=Rpt.CNEId
	INNER JOIN Organization2010 Org ON Org.Id=Rpt.OrgId
	INNER JOIN dbo.CommonNationalExamCertificate CNE ON CNE.Id=Rpt.CNEId
	INNER JOIN dbo.Region CNEReg ON CNEReg.Id=CNE.RegionId
	INNER JOIN dbo.Region OrgReg ON OrgReg.Id=Org.RegionId
	INNER JOIN dbo.OrganizationType2010 OrgType ON Org.TypeId=OrgType.Id
) AS IChecks

INSERT INTO @Report 
SELECT * FROM @ReportWithoutOrder
ORDER BY [Количество проверок] DESC,[Номер свидетельства]

RETURN
END