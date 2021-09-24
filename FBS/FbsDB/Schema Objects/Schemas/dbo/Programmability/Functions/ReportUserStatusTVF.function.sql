CREATE function [dbo].[ReportUserStatusTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @Statuses TABLE
(
	[Name] NVARCHAR (50),
	Code NVARCHAR (50),
	[Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
	[Name] NVARCHAR (50),
	Code BIT,
	[Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode INT,
	IsPrivateName NVARCHAR (50),
	IsPrivateCode INT,
	IsPrivateOrder INT,
	StatusName NVARCHAR(50),
	StatusCode NVARCHAR(50),
	StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	IsPrivateOrder NVARCHAR (50),
	StatusName NVARCHAR(50),
	UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
ON Acc.OrganizationId=OReq.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
	AND (
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	))
OR (
	(Acc.Status=Comb.StatusCode)
	AND (
		Comb.OrgTypeCode=10
	)
	AND (
		OReq.TypeId IS NOT NULL
	)
)
OR (
	Comb.StatusCode='total'
	AND ((
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	)
	OR
	((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
(	
	MainOrder INT,
	OrgTypeName NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	[Всего] INT,
	[Активирован] INT,
	[На регистрации] INT,
	[На доработке] INT,
	[На согласовании] INT,
	[Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult


return
END


