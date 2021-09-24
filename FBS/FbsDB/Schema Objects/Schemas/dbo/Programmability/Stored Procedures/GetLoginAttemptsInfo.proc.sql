-------------------------------------------------
--Автор: Сулиманов А.М.
--Дата: 2009-06-02
--Проверка количества попыток залогинится
-------------------------------------------------
CREATE PROCEDURE dbo.GetLoginAttemptsInfo
(	@IP varchar(32), 
	@TimeInterval int
)
AS
	SET NOCOUNT ON

	DECLARE @startDate datetime, @endDate datetime, @eventCode varchar(20)
	SET @endDate=GETDATE()
	SET @startDate=DATEADD(ss,-@TimeInterval,@endDate)
	SET @eventCode='USR_VERIFY'

	SELECT 
			ISNULL(MAX(Date),CAST('1900-01-01' as datetime)) LastLoginDate, 
			@endDate as CheckedDate, 
			--COUNT(*) Attempts, 
			ISNULL(SUM([LoginFailResult]),0) AttemptsFail
	FROM (
		SELECT 	
			--LEFT(EventParams,CHARINDEX('|',EventParams)-1) AS [Login],
			Date,
			CASE SUBSTRING(EventParams,LEN(EventParams)-2,1)
				WHEN '1' THEN 0
				ELSE 1
			END AS [LoginFailResult]
		FROM dbo.EventLog
		WHERE	(Date between @startDate and @endDate) 
				AND EventCode=@eventCode AND IP=@IP
	) T

