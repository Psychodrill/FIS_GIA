create function [dbo].[ReportTotalChecksTVF_New](
	@from datetime = null
	, @to datetime = null)
RETURNS @report TABLE 
(	 					
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
	select @from = dateadd(year, -1, getdate()), @to = getdate()


INSERT INTO @report
SELECT [Тип проверки] 
,[всего] 
,[уникальных проверок по РН] 
,[уникальных проверок по ТН] 
,[уникальных проверок по документу] 
,[уникальных проверок по ФИО и баллам] 
,[всего уникальных проверок] 
FROM(
SELECT * FROM dbo.ReportChecksByPeriodTVF(@from,@to)
UNION ALL
SELECT NULL,NULL,NULL,NULL,NULL,NULL,NULL,5
UNION ALL
SELECT * FROM dbo.ReportChecksAllTVF()
) INN ORDER BY [order]

RETURN
end