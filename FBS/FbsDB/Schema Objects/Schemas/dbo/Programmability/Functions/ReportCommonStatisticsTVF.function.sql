Create function [dbo].[ReportCommonStatisticsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Всего свидетельств] INT
,[Зарегистрировано пользователей] INT
,[Проверок всего] INT
,[Уникальных проверок] INT
,[Уникальных пакетных проверок] INT
,[Уникальных интерактивных проверок] INT
)
AS 
BEGIN

DECLARE @CNEsCount INT
SELECT @CNEsCount=COUNT(*) 
FROM CommonNationalExamCertificate

DECLARE @UsersCount INT
SELECT @UsersCount=COUNT(*) 
FROM Account Acc
INNER JOIN GroupAccount GA ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId AND OReq.OrganizationId IS NOT NULL

DECLARE @TotalChecks INT
DECLARE @TotalUniqueChecks INT

SELECT @TotalChecks=SUM([всего])
,@TotalUniqueChecks=SUM([всего уникальных проверок])
FROM ReportTotalChecksTVF(null,null)


DECLARE @UniqueChecks_Batch INT

SELECT @UniqueChecks_Batch=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]='Пакетная'


DECLARE @UniqueChecks_UI INT

SELECT @UniqueChecks_UI=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]='Интерактивная'


INSERT INTO @Report
SELECT @CNEsCount,@UsersCount,@TotalChecks,@TotalUniqueChecks,@UniqueChecks_Batch,@UniqueChecks_UI

RETURN
END

