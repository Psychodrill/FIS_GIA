create function [dbo].[ReportEditedOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
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
,[Импортирована из справочника] nvarchar(13) null
)
AS 
BEGIN


INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
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
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,CASE WHEN (Org.WasImportedAtStart=1)
	THEN 'Да'
	ELSE 'Нет'
	END AS [Импортирована из справочника]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
WHERE (Org.CreateDate != Org.UpdateDate AND Org.WasImportedAtStart =1) OR (Org.WasImportedAtStart=0)
ORDER BY Org.WasImportedAtStart


RETURN
END

