USE [gvuz_start_2016]
GO
/****** Object:  StoredProcedure [dbo].[GZGU_STEP1]    Script Date: 08/11/2017 11:27:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GZGU_STEP1]
AS
BEGIN
DELETE FROM	dbo.gzgu_aetd
DELETE FROM	dbo.gzgu_1_15
DELETE FROM	dbo.gzgu_1_8
DELETE FROM	dbo.gzgu_3_64
DELETE FROM	dbo.gzgu_App_24
DELETE FROM	dbo.gzgu_App_8
DELETE FROM	dbo.gzgu_App_Sources


declare @year int = 2017;
declare @regDate datetime = '2017-07-26 23:59:59.000';

insert into gzgu_1_8 (ApplicationID, CompetitiveGroupID, EducationSourceId, moreRegDate)
SELECT DISTINCT app.ApplicationID, cg1.CompetitiveGroupID, cg1.EducationSourceId, 
case when app.RegistrationDate <= @regDate then 0 else 1 end as moreRegDate
FROM Application app (NOLOCK) 
INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID 
INNER JOIN Campaign c (NOLOCK) ON c.CampaignID = cg1.CampaignID
WHERE c.CampaignTypeID IN (1,2) AND c.YearStart = @year
AND cg1.EducationLevelID IN (2,4,5) 
AND app.StatusID IN (2,4,3,8); 


insert into gzgu_3_64
SELECT DISTINCT A.ApplicationID, DocumentTypeID, cg.CompetitiveGroupID
FROM Application A (NOLOCK)
inner join Entrant e on a.EntrantID = e.EntrantID
inner join EntrantDocument ed on ed.EntrantID = e.EntrantID
inner join ApplicationCompetitiveGroupItem acgi on acgi.ApplicationId = A.ApplicationId
inner join CompetitiveGroup cg on cg.CompetitiveGroupID = acgi.CompetitiveGroupId
INNER JOIN Campaign C (NOLOCK) ON C.CampaignID = CG.CampaignID
WHERE c.CampaignTypeID IN (1,2) AND c.YearStart = 2017 AND YEAR(A.RegistrationDate) = 2017
AND cg.EducationLevelID IN (2,4,5) AND A.StatusID IN (2,4,3,8)
AND ed.DocumentTypeID IN (11, 30, 31)  AND C.CampaignTypeID IN (1,2)
GROUP BY  A.ApplicationID,DocumentTypeID,cg.CompetitiveGroupID;





insert into gzgu_1_15 (ApplicationID, DocumentTypeID, CompetitiveGroupID)
SELECT DISTINCT app.ApplicationID, edj.DocumentTypeID, cg1.CompetitiveGroupID
FROM Application app (NOLOCK) 
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON cg1.CompetitiveGroupID=acgij.CompetitiveGroupId 
INNER JOIN Campaign c (NOLOCK) ON c.CampaignID = cg1.CampaignID AND c.CampaignTypeID IN (1,2) AND c.YearStart = @year
WHERE cg1.EducationLevelID IN (2,4,5) AND app.StatusID=8 
AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL  
AND aetdj.EntranceTestItemID IS NULL AND aetdj.BenefitID = 1;



insert into gzgu_App_8 (ApplicationID, CompetitiveGroupID, EducationSourceId, CampaignTypeID)
SELECT DISTINCT aj.ApplicationID, cgj.CompetitiveGroupID, cgj.EducationSourceId, cj.CampaignTypeID 
FROM Application aj (NOLOCK)
INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.YearStart = @year AND cj.CampaignTypeID in (1, 2)
WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 and YEAR(aj.RegistrationDate) = 2017
AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL;



insert into gzgu_App_24 (ApplicationID, CompetitiveGroupID, EducationSourceId, CampaignTypeID)
SELECT DISTINCT aj.ApplicationID, cgj.CompetitiveGroupID, cgj.EducationSourceId, cj.CampaignTypeID 
FROM Application aj (NOLOCK)
INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.YearStart = @year AND cj.CampaignTypeID in (1, 2)
WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID IN (2,4,3,8)
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL;


INSERT INTO gzgu_aetd (ApplicationID, StatusId, ID, ResultValue, BenefitID, SourceID) --, [1], [2], [3], [4], [Creat])
SELECT distinct 
a.ApplicationID
,a.StatusID
,aetd.ID
,aetd.ResultValue
,aetd.BenefitID
,aetd.SourceID
FROM Application a (NOLOCK)
INNER JOIN ApplicationEntranceTestDocument aetd (NOLOCK) on a.ApplicationID=aetd.ApplicationID --AND aetd.BenefitID is NULL
--INNER JOIN EntranceTestItemC eticj (NOLOCK) ON aetd.EntranceTestItemID=eticj.EntranceTestItemID
where YEAR(RegistrationDate) = 2017
group by 
a.ApplicationID
,a.StatusID
,aetd.ID
,aetd.ResultValue
,aetd.BenefitID
,aetd.SourceID

	
	
INSERT INTO gzgu_App_Sources (ApplicationID, s1, s2, s3, s4, Creat)
Select ApplicationID, Max([1]), Max([2]), Max([3]), Max([4]), Max([Creat])
FROM (
SELECT 
a.ApplicationID
,CASE WHEN aetd.SourceID = 1 THEN 1 ELSE 0 END AS [1]
,CASE WHEN aetd.SourceID = 2 THEN 1 ELSE 0 END AS [2]
,CASE WHEN aetd.BenefitID = 3 THEN 1 ELSE 0 END AS [3]
,CASE WHEN aetd.SourceID = 4 THEN 1 ELSE 0 END AS [4]
,CASE WHEN eticj.EntranceTestTypeID IN (2,3) THEN 1 ELSE 0 END AS [Creat]
FROM Application a (NOLOCK)
INNER JOIN ApplicationEntranceTestDocument aetd (NOLOCK) on a.ApplicationID=aetd.ApplicationID 

INNER JOIN EntranceTestItemC eticj (NOLOCK) ON aetd.EntranceTestItemID=eticj.EntranceTestItemID
where YEAR(RegistrationDate) = 2017
GROUP BY a.ApplicationID, aetd.SourceID,aetD.BenefitID, aetd.EntranceTestTypeID,eticj.EntranceTestTypeID
) as q
group by ApplicationID;

	
END
