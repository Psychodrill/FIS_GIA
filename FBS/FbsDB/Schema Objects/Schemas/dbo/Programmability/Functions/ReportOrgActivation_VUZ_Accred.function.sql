CREATE function [dbo].[ReportOrgActivation_VUZ_Accred]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
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
	
DECLARE @VUZState INT
DECLARE @VUZPriv INT

SELECT @VUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

SELECT @VUZPriv = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT
'ВУЗ','Всего',@VUZState+@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

return
END


