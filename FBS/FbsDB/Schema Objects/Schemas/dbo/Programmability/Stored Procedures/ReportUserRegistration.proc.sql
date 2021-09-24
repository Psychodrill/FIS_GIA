-- =============================================
-- Получение отчета о регистрации пользователей.
-- =============================================
CREATE procedure [dbo].[ReportUserRegistration]
as
begin

DECLARE @StartDate DATETIME
SET @StartDate= '2010-05-15' -- dateadd(month, -1, getdate())

SELECT 
DAY(UpdateDay) AS [Day]
--, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(@MonthAgo))+'/'+CONVERT(NVARCHAR(50),MONTH(GETDATE()))+'/'+CONVERT(NVARCHAR(50),UpdateDay)) AS [date]
, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(UpdateDay))+'-'+CONVERT(NVARCHAR(50),MONTH(UpdateDay))+'-'+CONVERT(NVARCHAR(50),DAY(UpdateDay))) AS [date]
, SUM([Активирован]) AS [activated]
, SUM([На регистрации])  AS [registration]
, SUM([На доработке]) AS [revision]
, SUM([На согласовании]) AS [consideration]
, SUM([Отключен])AS [deactivated]


FROM(
	SELECT 
		CONVERT(NVARCHAR(4),YEAR(F.UpdateDate))+'-'+CONVERT(NVARCHAR(2),MONTH(F.UpdateDate))+'-'+CONVERT(NVARCHAR(2),DAY(F.UpdateDate)) 
	AS UpdateDay,
		case when F.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when F.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when F.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when F.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when F.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID,UpdateDate,[Status]
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR ([Status]='registration' and VersionId=1)) AND UpdateDate >= @StartDate 
	) F ON A.ID=F.AccountID
) T  
GROUP BY UpdateDay
ORDER BY [date]
end





