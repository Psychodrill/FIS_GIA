CREATE function [dbo].[ReportOrgsInfoTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование ФО] nvarchar(255) null
,[Код ФО] nvarchar(255) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Подана заявка на регистрацию] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
,[Количество проверок по номеру] int null
,[Количество уникальных проверок по номеру] INT NULL
,[Количество проверок по паспортным данным] INT NULL
,[Количество уникальных проверок по паспортным данным] INT NULL
,[Количество проверок по типографскому номеру] INT NULL
,[Количество уникальных проверок по типографскому номеру] INT NULL
,[Количество интерактивных проверок] INT NULL
,[Количество уникальных интерактивных проверок] INT NULL
,[Количество неправильных проверок] INT NULL
,[Первая проверка] datetime null
,[Последняя проверка] datetime null
,[Работа с ФБС] NVARCHAR(20)
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
	OrganizationId int,
	[Status] nvarchar(50),
	UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
	OrganizationId int,
	[activated] nvarchar(20),
	[deactivated] nvarchar(20),
	[consideration] nvarchar(20),
	[registration] nvarchar(20),
	[revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


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


DECLARE @WrongChecksByOrg TABLE
(
	OrganizationId INT,
	WrongChecks INT
)

INSERT INTO @WrongChecksByOrg
SELECT IWrong.OrganizationId,SUM(IWrong.WrongChecks) FROM 
(
	SELECT IOrgReq.OrganizationId AS OrganizationId,COUNT(*) AS WrongChecks
	FROM CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND c.SourceCertificateId IS NOT NULL
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,COUNT(*) AS WrongChecks
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND c.SourceCertificateId IS NOT NULL
	GROUP BY IOrgReq.OrganizationId
) AS IWrong
GROUP BY IWrong.OrganizationId





DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	TotalUIChecks INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
ISNULL(ChLog.PassportSeria,'')+
ISNULL(ChLog.PassportNumber,'')+
ISNULL(ChLog.CNENumber,'')+
ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId



DECLARE @CheckLimitDatesByOrg TABLE
(
	OrganizationId INT,
	FirstCheck DATETIME,
	LastCheck DATETIME
)
INSERT INTO @CheckLimitDatesByOrg
SELECT OrganizationId,MIN(FirstCheck),MAX(LastCheck)
FROM 
(
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(ChLog.EventDate) AS FirstCheck,MAX(ChLog.EventDate) AS LastCheck
	FROM CNEWebUICheckLog ChLog
	INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
	WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
	GROUP BY IOrgReq.OrganizationId
) AS RawCheckLimitDates
GROUP BY OrganizationId






INSERT INTO @Report
SELECT 
Org.[Полное наименование] AS [Полное наименование]
,ISNULL(Org.[Краткое наименование],'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Org.[Имя ФО] AS [Имя ФО]
,Org.[Код ФО] AS [Код ФО]
,Org.[Имя региона] AS [Имя региона]
,Org.[Код региона] AS [Код региона]
,Org.[Тип] AS [Тип]
,Org.[Вид] AS [Вид]
,Org.[ОПФ] AS [ОПФ]
,Org.[Филиал] AS [Филиал]
,Org.[Аккредитация по справочнику] AS [Аккредитация по справочнику]
,Org.[Свидетельство об аккредитации] AS [Свидетельство об аккредитации]
,Org.[Аккредитация по факту] AS [Аккредитация по факту] 	
,Org.[ФИО руководителя] AS [ФИО руководителя]
,Org.[Должность руководителя] AS [Должность руководителя]
,Org.[Ведомственная принадлежность] AS [Ведомственная принадлежность]
,Org.[Фактический адрес] AS [Фактический адрес]
,Org.[Юридический адрес] AS [Юридический адрес]
,Org.[Код города] AS[Код города]
,Org.[Телефон] AS [Телефон]
,Org.[EMail] AS [EMail]
,Org.[ИНН] AS [ИНН]
,Org.[ОГРН] AS [ОГРН]

,CASE 
	WHEN (ISNULL(UsersCnt.UsersCount,0)=0)
	THEN 'Нет'
	ELSE 'Да'
	END AS [Подана заявка на регистрацию]
,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]

,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]

,LimitDates.FirstCheck AS [Первая проверка]  
,LimitDates.LastCheck AS [Последняя проверка] 

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
ReportOrgsBASE() Org 
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId
LEFT JOIN @WrongChecksByOrg WrongChecks
ON Org.Id=WrongChecks.OrganizationId
LEFT JOIN @CheckLimitDatesByOrg LimitDates
ON Org.Id=LimitDates.OrganizationId

RETURN
END
