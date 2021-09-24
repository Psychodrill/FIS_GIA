CREATE function [dbo].[ReportOrgActivation_OTHER]()
RETURNS @OTHER TABLE 
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
	
DECLARE @RCOI INT
DECLARE @OUO INT
DECLARE @OtherOrg INT
SELECT @RCOI = COUNT(*) FROM Organization2010 WHERE TypeId=3 
SELECT @OUO = COUNT(*) FROM Organization2010 WHERE TypeId=4 
SELECT @OtherOrg = COUNT(*) FROM Organization2010 WHERE TypeId=5

INSERT INTO @OTHER
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
SELECT 'РЦОИ','',@RCOI,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=3

UNION ALL
SELECT
'Орган управления образованием','',@OUO,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=4 
UNION ALL
SELECT
'Другое','',@OtherOrg,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId<>1 AND OrgReq.TypeId<>2 AND OrgReq.TypeId<>3 AND OrgReq.TypeId<>4 AND OrgReq.TypeId<>5

return
END


