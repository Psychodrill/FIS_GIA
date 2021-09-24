CREATE function [dbo].[ReportCheckedCNEsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Номер свидетельства] NVARCHAR(255)
,[Проверок в различных регионах] INT
,[Проверок в различных ОУ] INT
,[Проверок в различных ВУЗах] INT
,[В негосударственных ВУЗах] INT
,[В государственных ВУЗах] INT
,[Проверок в различных ССУЗах] INT
,[В негосударственных ССУЗах] INT
,[В государственных ССУЗах] INT
)
AS 
BEGIN

INSERT INTO @Report
SELECT 
IChecks.CNENumber AS [Номер свидетельства]
,COUNT(DISTINCT IChecks.RegId) AS [Проверок в различных регионах]
,COUNT(*) AS [Проверок в различных ОУ] 
,COUNT(CASE WHEN IChecks.OrgType=1 THEN 1 ELSE NULL END) AS [Проверок в различных ВУЗах]
,COUNT(CASE WHEN IChecks.OrgType=1 AND IChecks.OPF=1 THEN 1 ELSE NULL END) AS [В негосударственных ВУЗах]
,COUNT(CASE  WHEN IChecks.OrgType=1 AND IChecks.OPF=0 THEN 1 ELSE NULL END) AS [В государственных ВУЗах]
,COUNT(CASE WHEN IChecks.OrgType=2 THEN 2 ELSE NULL END) AS [Проверок в различных ССУЗах]
,COUNT(CASE WHEN IChecks.OrgType=2 AND IChecks.OPF=1 THEN 1 ELSE NULL END) AS [В негосударственных ССУЗах]
,COUNT(CASE  WHEN IChecks.OrgType=2 AND IChecks.OPF=0 THEN 1 ELSE NULL END) AS [В государственных ССУЗах]
FROM 
(
	SELECT CNENumber ,OrgId,Org.TypeId AS OrgType,Org.IsPrivate AS OPF,Org.RegionId AS RegId  
	FROM [ReportCheckedCNEsBASE]() AS Rpt
	INNER JOIN Organization2010 Org ON Org.Id=Rpt.OrgId
) AS IChecks
GROUP BY IChecks.CNENumber HAVING COUNT(*)>=6


RETURN
END