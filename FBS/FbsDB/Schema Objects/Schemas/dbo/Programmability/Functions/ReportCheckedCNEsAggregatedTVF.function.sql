CREATE function [dbo].[ReportCheckedCNEsAggregatedTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Количество свидетельств] INT
,[Проверок в различных ОУ] INT
)
AS 
BEGIN


INSERT INTO @Report ([Количество свидетельств],[Проверок в различных ОУ])
SELECT COUNT(IAggrChecks.CNEId) AS CNECount,IAggrChecks.OrgCount AS OrgCount FROM 
(
	SELECT 
	CNEId AS CNEId
	,COUNT(OrgId) AS OrgCount
	FROM [ReportCheckedCNEsBASE]() AS IChecks
	GROUP BY IChecks.CNEId HAVING COUNT(IChecks.OrgId)>=1
) AS IAggrChecks
GROUP BY IAggrChecks.OrgCount 
ORDER BY IAggrChecks.OrgCount 

RETURN
END

