CREATE function [dbo].[ReportRegistredOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
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
)
AS 
BEGIN

 
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
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id IN 
	(SELECT OReq.OrganizationId 
	FROM OrganizationRequest2010 OReq
	WHERE OReq.OrganizationId  IS NOT NULL)

RETURN
END