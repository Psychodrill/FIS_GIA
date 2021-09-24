CREATE function [dbo].[ReportChecksByOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Имя региона] NVARCHAR(255) null
,[Полное наименование] NVARCHAR(4000) NULL
,[Тип] NVARCHAR(255) null
,[ОПФ] NVARCHAR(50) null
,[Количество проверок] INT null
,[Количество уникальных проверок] INT NULL
,[Работа с ФБС] NVARCHAR(20) NULL
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 

DECLARE @NumberChecksByOrg TABLE
(
	OrganizationId INT,
	TotalNumberChecks INT,
	UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
	OrganizationId INT,
	TotalTNChecks INT,
	UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
	OrganizationId INT,
	TotalPassportChecks INT,
	UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
GROUP BY IOrgReq.OrganizationId




DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	TotalUIChecks INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ChLog.FoundedCNEId) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd AND ChLog.FoundedCNEId is not NULL
GROUP BY IOrgReq.OrganizationId








INSERT INTO @Report
SELECT 
Reg.[Name] AS [Имя региона]
,Org.FullName AS [Полное наименование]
,OrgType.[Name] AS [Тип]
,REPLACE(REPLACE(Org.IsPrivate,1,'Негосударственный'),0,'Государственный') AS [ОПФ]

,ISNULL(NumberChecks.TotalNumberChecks,0)
+ISNULL(PassportChecks.TotalPassportChecks,0)
+ISNULL(TNChecks.TotalTNChecks,0)
+ISNULL(UIChecks.TotalUIChecks,0) AS [Количество проверок]  
,ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных проверок] 
,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС]


FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId


LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId

ORDER BY Reg.Id,Org.TypeId,Org.IsPrivate

RETURN
END