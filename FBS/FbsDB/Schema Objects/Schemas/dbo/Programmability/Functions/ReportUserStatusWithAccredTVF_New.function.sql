CREATE function [dbo].[ReportUserStatusWithAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)



INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_OTHER()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего' OR [ ]='РЦОИ' OR [ ]='Орган управления образованием' OR [ ]='Другое'


INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
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
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON Org.Id=OReq.OrganizationId 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF_New (@periodBegin ,@periodEnd)

return
END


