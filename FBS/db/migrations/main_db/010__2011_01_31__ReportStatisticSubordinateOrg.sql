-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (10, '010__2011_01_31__ReportStatisticSubordinateOrg')
-- =========================================================================




IF object_id (N'dbo.ReportStatisticSubordinateOrg', N'TF') is not null
	DROP FUNCTION dbo.ReportStatisticSubordinateOrg;
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
	UserUpdateDate datetime null,
	CountUniqueChecks int null
)
AS BEGIN

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

RETURN
END
GO


IF object_id (N'dbo.ReportXMLSubordinateOrg', N'TF') is not null
	DROP FUNCTION [dbo].[ReportXMLSubordinateOrg];
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
	[Дата активации пользователя] datetime null,
	[Количество уникальных проверок] int null
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
	UserUpdateDate [Дата активации],
	CountUniqueChecks [Уникальных проверок]
FROM
	dbo.ReportStatisticSubordinateOrg ( @periodBegin, @periodEnd, @departmentId)
RETURN
END
GO


--Добавлены поля кода организации, учредителя
ALTER function [dbo].[ReportOrgsInfoByRegionTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL
	, @arg NVARCHAR(50) = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
,[Дата создания] datetime null
,[Создана из справочника] NVARCHAR(20) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] NVARCHAR(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Код учредителя] int null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
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
,[Код ОУ] int null
,[Код головного ОУ] int null
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
AND IOrgReq.RegionId=@arg
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
AND IOrgReq.RegionId=@arg
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
AND IOrgReq.RegionId=@arg
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
AND IOrgReq.RegionId=@arg
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
AND IOrgReq.RegionId=@arg
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
AND IOrgReq.RegionId=@arg
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
	AND IOrgReq.RegionId=@arg
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
	AND IOrgReq.RegionId=@arg
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
AND IOrgReq.RegionId=@arg
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
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(ChLog.EventDate) AS FirstCheck,MAX(ChLog.EventDate) AS LastCheck
	FROM CNEWebUICheckLog ChLog
	INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
	WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
) AS RawCheckLimitDates
GROUP BY OrganizationId







INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,REPLACE(REPLACE(Org.WasImportedAtStart,1,'Да'),0,'Нет') AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN 'Есть'
	ELSE 'Нет'
	END AS [Аккредитация по факту] 	
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.DepartmentId AS [Код учредителя]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,Org.PhoneCityCode AS [Код города] 
,Org.Phone AS [Телефон] 
,Org.EMail AS [EMail]  

,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]

,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
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
AS [Работа с ФБС],
Org.Id [Код ОУ],
Org.MainId [Код головного ОУ]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
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
WHERE Org.RegionId=@arg

RETURN
END
GO