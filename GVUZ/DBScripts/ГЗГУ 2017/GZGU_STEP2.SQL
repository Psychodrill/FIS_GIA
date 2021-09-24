USE [gvuz_start_2016]
GO
/****** Object:  StoredProcedure [dbo].[GZGU_STEP2]    Script Date: 08/11/2017 11:28:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GZGU_STEP2]
AS
BEGIN

declare @year int = 2017;
declare @regDate datetime = '2017-07-26 23:59:59.000' 


--DECLARE @aetd_tmp TABLE (
--  ApplicationID INT NOT NULL,
--  StatusId int NULL,
--  [1] BIT NOT NULL,
--  [2] BIT NOT NULL,
--  [3] BIT NOT NULL,
--  [4] BIT NOT NULL,
--  [Creat] BIT NOT NULL);
  
  
--Declare @t1_15 Table (
--ApplicationID int not null,
--DocumentTypeID int not null,
--CompetitiveGroupID int not null);

--insert into @t1_15
--SELECT app.ApplicationID, edj.DocumentTypeID, cg1.CompetitiveGroupID
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON cg1.CompetitiveGroupID=acgij.CompetitiveGroupId 
--INNER JOIN Campaign c (NOLOCK) ON c.CampaignID = cg1.CampaignID AND c.CampaignTypeID IN (1,2) AND c.YearStart = @year
--WHERE cg1.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL  
--AND aetdj.EntranceTestItemID IS NULL AND aetdj.BenefitID = 1;

-- select * from gzgu_temp_result

delete from gzgu_temp_result;

--insert into @result
insert into gzgu_temp_result (
	InstitutionID,
	CompetitiveGroupID,
	DirectionID,
	EducationFormId,
	EducationSourceId,
	IdLevelBudget,
	[1_4],
	[1_5],
	[1_6],
	[1_7],
	[1_8],
	[1_9],
	[1_10],
	[1_11],
	[1_12],
	[1_13],
	[1_14],
	[1_15],
	[1_16],
	[1_17],
	[1_18],

	[1_19],
	[1_20],
	--[1_21],
	--[1_22],
	--[1_23],
	[1_24],
	[1_25],
	[1_26],
	[1_27],
	[1_28],
	[1_29],
	[1_30],
	[1_31],
	[1_32],
	[1_33],
	[1_34],
	[1_35],
	[1_36],
	[1_37],
	[1_38],
	[1_39],
	[1_40],
	[1_41],
	[1_42],
	[1_43],
	[1_44],
	[1_45],
	[1_46],
	[1_47],
	[1_48],
	
	[2_1], 
	[2_1_cnt], 
	[2_2], 
	[2_2_cnt], 
	[2_3], 
	[2_3_cnt], 
	[2_4], 
	[2_4_cnt], 
	[2_5], 
	[2_5_cnt], 
	[2_6], 
	[2_6_cnt], 
	[2_7], 
	[2_7_cnt], 
	[2_8], 
	[2_8_cnt], 

	[2_9], 
	[2_10], 
	[2_11], 
	[2_12], 
	[2_13], 
	[2_14], 

	[2_15],
	[2_16],
	[2_17],
	[2_18],
	[2_19],
	[2_20],

	[2_21],
	[2_21_cnt],
	[2_22],
	[2_22_cnt],
	[2_23],
	[2_23_cnt],
	[2_24],
	[2_24_cnt],
	[2_25],
	[2_25_cnt],
	[2_26],
	[2_26_cnt],

	[3_1],
	[3_2],
	[3_3],
	[3_4],
	[3_5],

	[3_6],
	[3_12],
	[3_13],
	[3_14],
	[3_15],
	[3_16],
	[3_17],
	[3_18],
	[3_19],
	[3_20],

	[3_26],
	[3_27],

	[3_29],
	[3_29_cnt],
	[3_30],
	[3_30_cnt],
	[3_31],
	[3_31_cnt],
	[3_32],
	[3_32_cnt],
	[3_33],
	[3_33_cnt],
	[3_34],
	[3_34_cnt],

	[3_36],
	[3_36_cnt],
	[3_37],
	[3_37_cnt],
	[3_38],
	[3_38_cnt],
	[3_39],
	[3_39_cnt],
	[3_40],
	[3_40_cnt],
	[3_41],
	[3_41_cnt]
)
	SELECT DISTINCT 
	--i.EsrpOrgID
	--,d.NewCode AS [DirCode]
	--,d.Name as [1_1]
	--,CASE WHEN ait.Name = 'Очно-заочная (вечерняя)' THEN 'Очно-заочная' ELSE ait.Name END AS [1_2]
	--,CASE WHEN cg.EducationSourceId IN (14,16,20)  THEN lb.BudgetName ELSE 'Платная основа' END AS [1_3]
	i.InstitutionID
	,cg.CompetitiveGroupID
	,cg.DirectionID
	,cg.EducationFormId
	,cg.EducationSourceId
	,CASE WHEN cg.EducationSourceId = 15 THEN 4 ELSE isnull(IdLevelBudget, 0) END AS IdLevelBudget

	
	,ISNULL(cgij.NumberBudgetO,0)+ISNULL(cgijp.NumberPaidO,0)+ISNULL(cgij.NumberQuotaO,0)+ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberBudgetOZ,0)+ISNULL(cgijp.NumberPaidOZ,0)+ISNULL(cgij.NumberQuotaOZ,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberBudgetZ,0)+ISNULL(cgijp.NumberPaidZ,0)+ISNULL(cgij.NumberQuotaZ,0)+ISNULL(cgij.NumberTargetZ,0)
	--from gzgu_1_8  gz (NOLOCK)
	--INNER JOIN CompetitiveGroupItem cgij on cgij.CompetitiveGroupID = gz.CompetitiveGroupID
	--WHERE cg.CompetitiveGroupID = gz.CompetitiveGroupID) 
	as [1_4]
	
	,ISNULL(cgij.NumberQuotaO,0)+ISNULL(cgij.NumberQuotaOZ,0)+ISNULL(cgij.NumberQuotaZ,0)
	--from gzgu_1_8  gz (NOLOCK)
	--INNER JOIN CompetitiveGroupItem cgij on cgij.CompetitiveGroupID = gz.CompetitiveGroupID
	--WHERE cg.CompetitiveGroupID = gz.CompetitiveGroupID) 
	as [1_5]

	,ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberTargetZ,0)
	--from gzgu_1_8  gz (NOLOCK)
	--INNER JOIN CompetitiveGroupItem cgij on cgij.CompetitiveGroupID = gz.CompetitiveGroupID
	--WHERE cg.CompetitiveGroupID = gz.CompetitiveGroupID) 
	as [1_6]
	

	,ISNULL(cgij.NumberBudgetO,0)+ISNULL(cgij.NumberBudgetOZ,0)+ISNULL(cgij.NumberBudgetZ,0) + ISNULL(cgijp.NumberPaidO,0)+ISNULL(cgijp.NumberPaidOZ,0)+ISNULL(cgijp.NumberPaidZ,0)
	--from gzgu_1_8  gz (NOLOCK)
	--INNER JOIN CompetitiveGroupItem cgij on cgij.CompetitiveGroupID = gz.CompetitiveGroupID
	--WHERE cg.CompetitiveGroupID = gz.CompetitiveGroupID) 
	as [1_7]
	
	


,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_8
WHERE CompetitiveGroupID = cg.CompetitiveGroupID AND moreRegDate = 0
) AS [1_8] 

,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM Application app (NOLOCK) 
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID=acgij.CompetitiveGroupId AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
WHERE cgj.EducationLevelID IN (2,4,5) 
AND app.StatusID IN (2,4,3,8) AND app.RegistrationDate <= @regDate  AND aetdj.BenefitID = 1 
) AS [1_9] 

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_8
WHERE CompetitiveGroupID = cg.CompetitiveGroupID AND moreRegDate = 0 AND EducationSourceId=20
) AS [1_10] 

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_8
WHERE CompetitiveGroupID = cg.CompetitiveGroupID AND moreRegDate = 0 AND EducationSourceId=16
) AS [1_11]

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_8
WHERE CompetitiveGroupID = cg.CompetitiveGroupID AND moreRegDate = 0 AND EducationSourceId NOT IN (16,20)
) AS [1_12]

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_8
WHERE CompetitiveGroupID = cg.CompetitiveGroupID AND moreRegDate = 1
) AS [1_13]


------------------------------------------------------------------------
,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_App_8
Where CompetitiveGroupID = cg.CompetitiveGroupID
) AS [1_14]


,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_15
Where CompetitiveGroupID = cg.CompetitiveGroupID AND DocumentTypeID = 10
) AS [1_15]

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_15
Where CompetitiveGroupID = cg.CompetitiveGroupID AND DocumentTypeID = 27
) AS [1_16]

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_15
Where CompetitiveGroupID = cg.CompetitiveGroupID AND DocumentTypeID = 9
) AS [1_17]

,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_1_15
Where CompetitiveGroupID = cg.CompetitiveGroupID AND DocumentTypeID IN (20, 21, 22)
) AS [1_18]

--------------------------------------------------------------------
--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
-- AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL  AND cgj.EducationSourceId =20
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_19] 
,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_App_8
Where CompetitiveGroupID = cg.CompetitiveGroupID  AND EducationSourceId =20
) AS [1_19] 

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
-- AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL  AND cgj.EducationSourceId =16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_20] 
,(SELECT COUNT(DISTINCT ApplicationID) 
FROM gzgu_App_8
Where CompetitiveGroupID = cg.CompetitiveGroupID  AND EducationSourceId = 16
) AS [1_20]  

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
-- AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND aetdj.BenefitID IS NULL AND cgj.EducationSourceId NOT IN (16,20) 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_24]
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID 
AND aetdj.BenefitID != 3 AND EducationSourceId NOT IN (16,20) and edj.DocumentTypeID in (9,10)
) AS [1_24]  

,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID 
AND aetdj.BenefitID = 3 AND edj.DocumentTypeID = 10
) AS [1_25]  

,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
INNER JOIN Application app1 (NOLOCK) ON app1.ApplicationID = app.ApplicationID
INNER JOIN Entrant ej ON app1.EntrantID = ej.EntrantID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID 
AND aetdj.BenefitID = 3 AND edj.DocumentTypeID = 27 AND ej.RegionID IN (92,82)
) AS [1_26]  

,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID 
AND aetdj.BenefitID = 3 AND edj.DocumentTypeID = 9
) AS [1_27]  

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
--AND aetdj.SourceID =3 AND aetdj.BenefitID = 3 AND edj.DocumentTypeID IN (20, 21, 22)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_28] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID 
AND aetdj.SourceID =3 AND aetdj.BenefitID = 3 AND edj.DocumentTypeID IN (20, 21, 22)
) AS [1_28]  

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID=acgij.CompetitiveGroupId 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID  
-- ) AS [1_29]
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
) AS [1_29] 
 

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
--AND aetdj.EntranceTestItemID IS NULL AND aetdj.BenefitID = 1 AND edj.DocumentTypeID IN (9, 10,20,21,22, 27, 28)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_30]
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument edj (NOLOCK) ON aetdj.EntrantDocumentID = edj.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND aetdj.BenefitID = 1 AND edj.DocumentTypeID IN (9, 10,20,21,22, 27, 28)
) AS [1_30] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 20
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_31] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
) AS [1_31] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
----INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 1 and foo.[2] = 0 --AND foo.[3] = 0 AND foo.[4] = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 20
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_32] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND gas.[s1] = 1 and gas.[s2] = 0
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
) AS [1_32] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
----INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND ((foo.[1] = 1 AND foo.[2] = 0) OR (foo.[1] = 1 AND foo.[2] = 1))
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 20
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_33] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 1 and gas.s2 = 1) -- ((foo.[1] = 1 AND foo.[2] = 0) OR (foo.[1] = 1 AND foo.[2] = 1)) ???
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
) AS [1_33] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[2] = 1 and foo.[1] = 0 --AND foo.[2]] = 1 --AND foo.[4] = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 20
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_34] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[2] = 1 and foo.[1] = 0
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 0 and gas.s2 = 1) 
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
) AS [1_34] 

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
--AND cgj.EducationSourceId = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_35] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[2] = 1 and foo.[1] = 0
--INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 0 and gas.s2 = 1) 
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
) AS [1_35] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
----INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 1 and foo.[2] = 0--AND foo.[3] = 0 AND foo.[4] = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [1_36] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 1 and foo.[2] = 0
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 1 and gas.s2 = 0) 
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
) AS [1_36] 

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
----INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND ((foo.[1] = 1 AND foo.[2] = 0) OR (foo.[1] = 1 AND foo.[2] = 1))
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [1_37] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 1 and gas.s2 = 1)
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
) AS [1_37] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
----INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[2] = 1 AND FOO.[1] = 0--AND foo.[2] = 1 --AND foo.[4] = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_38] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 0 and foo.[2] = 1
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 0 and gas.s2 = 1) 
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
) AS [1_38] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntranceTestItemC eticj ON aetdj.EntranceTestItemID=eticj.EntranceTestItemID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 1  AND FOO.[2] = 0 and foo.[3] = 0 --AND foo.[2] IN (1,2) AND foo.[3] = 1 AND foo.[4] = 0 AND foo.Creat = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId NOT IN (16,20)
--AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_39] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[1] = 1  AND FOO.[2] = 0 and foo.[3] = 0
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s1 = 1 and gas.s2 = 0 and gas.s3 = 0) 
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
--AND foo.BenefitID != 3 
) AS [1_39] 
-----------------------------------------------------------

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntranceTestItemC eticj ON aetdj.EntranceTestItemID=eticj.EntranceTestItemID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID  AND foo.[3] = 1 AND foo.[4] = 0 AND foo.Creat = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND  acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId NOT IN (16,20)
--AND aetdj.BenefitID = 3  
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_40] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[3] = 1 AND foo.[4] = 0 AND foo.Creat = 0
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.s3 = 1 and gas.s4 = 0 and gas.Creat = 0) 
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
AND foo.BenefitID = 3 
) AS [1_40] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntranceTestItemC eticj ON aetdj.EntranceTestItemID=eticj.EntranceTestItemID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND ((foo.[1] = 1 AND foo.[2] = 0) OR (foo.[1] = 1 AND foo.[2] = 1)) AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId NOT IN (16,20) 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_41] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND ((foo.[1] = 1 AND foo.[2] = 0) OR (foo.[1] = 1 AND foo.[2] = 1)) AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 1) AND gas.[s3] = 0 AND gas.[s4] = 0 AND gas.Creat = 1
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
) AS [1_41] 


--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK) 
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationID
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntranceTestItemC eticj ON aetdj.EntranceTestItemID=eticj.EntranceTestItemID
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 0 AND foo.[2] = 1 AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0
--WHERE cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_42] 
,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM gzgu_App_8 app
--INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID AND foo.[1] = 0 AND foo.[2] = 1 AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 0 AND gas.[s2] = 1 AND gas.[s3] = 0 AND gas.[s4] = 0 AND gas.Creat = 1)
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
) AS [1_42], 
----------------------------------------------------------------------------------

	'0' as [1_43],
	'0' as  [1_44],
	'0' as [1_45],
	'0' as  [1_46],
	'0' as  [1_47],
	 '0' as [1_48],

--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] 
--FROM Application aj (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
--AND cgj.EducationSourceID = 20 
--AND cgj.CompetitiveGroupID = cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL 
--AND acgij.EducationFormId = cg.EducationFormId 
--) foo) AS [2_1] 
--,(SELECT ISNULL(count(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] 
--FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
--AND cgj.EducationSourceID = 20 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL 
--AND acgij.EducationFormId = cg.EducationFormId 
--) foo) AS [2_1_cnt] 
(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0 AND gas.Creat = 0)
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
) AS [2_1]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0 AND gas.Creat = 0)
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
) AS [2_1_cnt]



--,(SELECT ISNULL(AVG(foo.ResultValue),0)  FROM(
--SELECT aetdj.ResultValue AS [ResultValue] 
--FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8  AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
--AND aetdj.SourceID = 1
--AND cgj.EducationSourceID = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--AND acgij.EducationFormId = cg.EducationFormId
--) foo) AS [2_2]

--,(SELECT ISNULL(count(foo.ResultValue),0)  FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8  AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
--AND aetdj.SourceID = 1
--AND cgj.EducationSourceID = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--AND acgij.EducationFormId = cg.EducationFormId
--) foo) AS [2_2_cnt]

,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0 AND gas.Creat = 0)
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
) AS [2_2]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[1] = 1 AND foo.[2] = 0 AND foo.Creat = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0 AND gas.Creat = 0)
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
) AS [2_2_cnt]



--,(SELECT ISNULL(AVG(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0 
--AND cgj.EducationSourceId NOT IN (16,20) --AND aetdj.SourceID=1
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--AND acgij.EducationFormId = cg.EducationFormId
--) foo1) AS [2_3] 

--,(SELECT ISNULL(count(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0 
--AND cgj.EducationSourceId NOT IN (16,20) --AND aetdj.SourceID=1
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
-- AND acgij.EducationFormId = cg.EducationFormId
--) foo1) AS [2_3_cnt] 

,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0 AND gas.[s3] = 0 AND gas.[s4] = 0 AND gas.Creat = 0)
INNER JOIN ApplicationEntranceTestDocument AETDJ (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
AND aetdj.BenefitID IS NULL
) AS [2_3]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[3] = 0 AND foo.[4] = 0 AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0 AND gas.[s3] = 0 AND gas.[s4] = 0 AND gas.Creat = 0)
INNER JOIN ApplicationEntranceTestDocument AETDJ (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
AND aetdj.BenefitID IS NULL
) AS [2_3_cnt]

--,(SELECT ISNULL(AVG(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID AND foo.[3] = 1 AND foo.[4] = 0 --AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5)  --AND foo.[3] = 1 AND foo.[4] = 0 --AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0 
--AND aj.StatusID=8 AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID = cg.CompetitiveGroupID 
--and aetdj.EntranceTestTypeID = 1
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
-- AND acgij.EducationFormId = cg.EducationFormId
--) foo1) AS [2_4]


--,(SELECT ISNULL(count(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID AND foo.[3] = 1 AND foo.[4] = 0 --AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5)  --AND foo.[3] = 1 AND foo.[4] = 0 --AND foo.Creat = 0 and foo.[1] = 1 AND foo.[2] = 0 
--AND aj.StatusID=8 AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--and aetdj.EntranceTestTypeID = 1
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--AND acgij.EducationFormId = cg.EducationFormId
--) foo1) AS [2_4_cnt]

,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[3] = 1 AND foo.[4] = 0  
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s3] = 1 AND gas.[s4] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)

and aetdj.EntranceTestTypeID = 1
) AS [2_4]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --AND foo.[3] = 1 AND foo.[4] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s3] = 1 AND gas.[s4] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)

and aetdj.EntranceTestTypeID = 1
) AS [2_4_cnt]


--,(SELECT ISNULL(AVG(foo.ResultValue),0)  FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND cgj.EducationSourceID = 20 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3) --and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1))
--AND acgij.EducationFormId = cg.EducationFormId
--) foo) AS [2_5] 
--,(SELECT ISNULL(count(foo.ResultValue),0)  FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND cgj.EducationSourceID = 20 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3) --and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1))
--AND acgij.EducationFormId = cg.EducationFormId
--) foo) AS [2_5_cnt] 
,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
and aetdj.EntranceTestTypeID in (1,3)
) AS [2_5]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 20
and aetdj.EntranceTestTypeID in (1,3)
) AS [2_5_cnt]



--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND cgj.EducationSourceID = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3)  --and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1)) 
--AND acgij.EducationFormId = cg.EducationFormId
--) foo) AS [2_6]

--,(SELECT ISNULL(count(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID 
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND cgj.EducationSourceID = 16
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3)  --and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1)) 
-- AND acgij.EducationFormId = cg.EducationFormId
--) foo) AS [2_6_cnt]

,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
and aetdj.EntranceTestTypeID in (1,3)
) AS [2_6]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId = 16
and aetdj.EntranceTestTypeID in (1,3)
) AS [2_6_cnt]


--,(SELECT ISNULL(AVG(foo1.ResultValue),0)  FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID AND foo.[3] = 0 AND foo.[4] = 0 --AND foo.Creat = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3)  AND foo.[3] = 0 AND foo.[4] = 0  
--AND  acgij.EducationFormId = cg.EducationFormId
----and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1)) 
--) foo1) AS [2_7] 

--,(SELECT ISNULL(count(foo1.ResultValue),0)  FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID AND foo.[3] = 0 AND foo.[4] = 0 --AND foo.Creat = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3)   AND foo.[3] = 0 AND foo.[4] = 0  
--AND  acgij.EducationFormId = cg.EducationFormId
----and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1)) 
--) foo1) AS [2_7_cnt] 

,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s3] = 0 AND gas.[s4] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
AND aetdj.BenefitID IS NULL
and aetdj.EntranceTestTypeID in (1,3) --AND foo.[3] = 0 AND foo.[4] = 0 
) AS [2_7]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s3] = 0 AND gas.[s4] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
AND aetdj.BenefitID IS NULL
and aetdj.EntranceTestTypeID in (1,3) --AND foo.[3] = 0 AND foo.[4] = 0 
) AS [2_7_cnt]


--,(SELECT ISNULL(AVG(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID AND foo.[3] = 1 AND foo.[4] = 0 --AND foo.Creat = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3)   AND acgij.EducationFormId = cg.EducationFormId
----and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1))
--) foo1) AS [2_8]

--,(SELECT ISNULL(count(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID AND foo.[3] = 1 AND foo.[4] = 0 --AND foo.Creat = 0
--INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cgj.CampaignID AND cj.CampaignTypeID = 1
--WHERE cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 AND cgj.EducationSourceId NOT IN (16,20)
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID 
--AND acgij.OrderOfAdmissionID IS NOT NULL  AND acgij.OrderOfExceptionID IS NULL
--and aetdj.EntranceTestTypeID in (1,3)  AND acgij.EducationFormId = cg.EducationFormId
----and (foo.[1] = 1 and foo.[2] = 0 or (foo.[1] = 1 and foo.[2] = 1))
--) foo1) AS [2_8_cnt]

,(SELECT ISNULL(AVG(foo.ResultValue),0) 
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s3] = 1 AND gas.[s4] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)
and aetdj.EntranceTestTypeID in (1,3) --AND foo.[3] = 1 AND foo.[4] = 0 
) AS [2_8]
,(SELECT ISNULL(COUNT(foo.ResultValue),0)  
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s3] = 1 AND gas.[s4] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID=aetdj.ApplicationID AND aetdj.ID = foo.ID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND EducationSourceId NOT IN (16,20)

and aetdj.EntranceTestTypeID in (1,3) --AND foo.[3] = 1 AND foo.[4] = 0 
) AS [2_8_cnt]

-------------------------------------------------------------------------------

, (SELECT avg(ISNULL(ia.MaxValue,0))
FROM CompetitiveGroup cg1 (NOLOCK)
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cg1.CampaignID AND cj.CampaignTypeID = 1
inner JOIN InstitutionAchievements ia  (NOLOCK) ON ia.CampaignID = cj.CampaignID
where cg.CompetitiveGroupID=cg1.CompetitiveGroupID AND ia.Idcategory in (1,
2,
3,
4,
5,
6,
7,
8)
) AS [2_9]

 

, (SELECT avg(ISNULL(ia.MaxValue,0))
  FROM CompetitiveGroup cg1 (NOLOCK)
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cg1.CampaignID AND cj.CampaignTypeID = 1
left JOIN InstitutionAchievements ia (NOLOCK) ON ia.CampaignID = cj.CampaignID
where cg.CompetitiveGroupID=cg1.CompetitiveGroupID AND ia.Idcategory = 9
) AS [2_10]


, (SELECT avg(ISNULL(ia.MaxValue,0))
  FROM CompetitiveGroup cg1(NOLOCK) 
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cg1.CampaignID AND cj.CampaignTypeID = 1
left JOIN InstitutionAchievements ia (NOLOCK) ON ia.CampaignID = cj.CampaignID
where cg.CompetitiveGroupID=cg1.CompetitiveGroupID AND ia.Idcategory = 17
) AS [2_11]


, (SELECT avg(ISNULL(ia.MaxValue,0))
  FROM CompetitiveGroup cg1 (NOLOCK)
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cg1.CampaignID AND cj.CampaignTypeID = 1
left JOIN InstitutionAchievements ia (NOLOCK) ON ia.CampaignID = cj.CampaignID
where cg.CompetitiveGroupID=cg1.CompetitiveGroupID AND ia.Idcategory = 10
) AS [2_12]



, (SELECT avg(ISNULL(ia.MaxValue,0))
  FROM CompetitiveGroup cg1 (NOLOCK)
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cg1.CampaignID AND cj.CampaignTypeID = 1
inner JOIN InstitutionAchievements ia  (NOLOCK) ON ia.CampaignID = cj.CampaignID
where cg.CompetitiveGroupID=cg1.CompetitiveGroupID AND ia.Idcategory = 11
) AS [2_13]



, (SELECT avg(ISNULL(ia.MaxValue,0))
  FROM CompetitiveGroup cg1 (NOLOCK)
INNER JOIN Campaign cj (NOLOCK) ON cj.CampaignID = cg1.CampaignID AND cj.CampaignTypeID = 1
INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.CampaignID = cj.CampaignID
where cg.CompetitiveGroupID=cg1.CompetitiveGroupID AND ia.Idcategory = 12
) AS [2_14]

----------------------------------------------------------

--,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID = acgij.CompetitiveGroupId 
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND ia.Idcategory =1 ) AS [2_15]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory in (1,
2,
3,
4,
5,
6,
7,
8)
) AS [2_15] 


--,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID = acgij.CompetitiveGroupId 
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 
--AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND ia.Idcategory = 15  ) AS [2_16]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 9
) AS [2_16] 


--,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID = acgij.CompetitiveGroupId 
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND ia.Idcategory = 17  ) AS [2_17]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 17
) AS [2_17] 


--,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID = acgij.CompetitiveGroupId 
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND ia.Idcategory = 10  ) AS [2_18]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 10
) AS [2_18] 

--,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID = acgij.CompetitiveGroupId 
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND ia.Idcategory = 11  ) AS [2_19]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 11
) AS [2_19] 

--,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID = acgij.CompetitiveGroupId 
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND ia.Idcategory = 12  ) AS [2_20]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 12
) AS [2_20] 


--,(SELECT (ISNULL(AVG(foo.ResultValue),0) + ISNULL(AVG(foo.MAXVALUE),0))/2 FROM(
--SELECT aetdj.ResultValue AS [ResultValue],ia.MaxValue 
--FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = aj.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND ia.Idcategory = 1 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID,ia.MaxValue) foo) AS [2_21] 

,(SELECT (ISNULL(AVG(aetdj.ResultValue),0) + ISNULL(AVG(ia.MAXVALUE),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory in (1,
2,
3,
4,
5,
6,
7,
8)
) AS [2_21] 
,(SELECT (ISNULL(Count(aetdj.ResultValue),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory in (1,
2,
3,
4,
5,
6,
7,
8)
) AS [2_21_cnt] 


--,(SELECT (ISNULL(AVG(foo.ResultValue),0) + ISNULL(AVG(foo.MAXVALUE),0))/2  FROM(
--SELECT aetdj.ResultValue AS [ResultValue],ia.MaxValue FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = aj.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND ia.Idcategory = 15 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID,ia.MaxValue) foo) AS [2_22] 
,(SELECT (ISNULL(AVG(aetdj.ResultValue),0) + ISNULL(AVG(ia.MAXVALUE),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 9
) AS [2_22] 
,(SELECT (ISNULL(Count(aetdj.ResultValue),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 9
) AS [2_22_cnt] 

--,(SELECT (ISNULL(AVG(foo.ResultValue),0) + ISNULL(AVG(foo.MAXVALUE),0))/2  FROM(
--SELECT aetdj.ResultValue AS [ResultValue],ia.MaxValue FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = aj.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND ia.Idcategory = 17 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID,ia.MaxValue) foo) AS [2_23] 
,(SELECT (ISNULL(AVG(aetdj.ResultValue),0) + ISNULL(AVG(ia.MAXVALUE),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 17
) AS [2_23] 
,(SELECT (ISNULL(Count(aetdj.ResultValue),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 17
) AS [2_23_cnt] 

--,(SELECT (ISNULL(AVG(foo.ResultValue),0) + ISNULL(AVG(foo.MAXVALUE),0))/2 FROM(
--SELECT aetdj.ResultValue AS [ResultValue],ia.MaxValue FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN IndividualAchivement ina  (NOLOCK)ON ina.ApplicationID = aj.ApplicationID
--INNER JOIN InstitutionAchievements ia  (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND ia.Idcategory = 10 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID,ia.MaxValue) foo) AS [2_24] 
,(SELECT (ISNULL(AVG(aetdj.ResultValue),0) + ISNULL(AVG(ia.MAXVALUE),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 10
) AS [2_24] 
,(SELECT (ISNULL(Count(aetdj.ResultValue),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 10
) AS [2_24_cnt] 

--,(SELECT (ISNULL(AVG(foo.ResultValue),0) + ISNULL(AVG(foo.MAXVALUE),0))/2  FROM(
--SELECT aetdj.ResultValue AS [ResultValue],ia.MaxValue FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN IndividualAchivement ina  (NOLOCK) ON ina.ApplicationID = aj.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND ia.Idcategory = 11 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID,ia.MaxValue) foo) AS [2_25] 
,(SELECT (ISNULL(AVG(aetdj.ResultValue),0) + ISNULL(AVG(ia.MAXVALUE),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 11
) AS [2_25] 
,(SELECT (ISNULL(Count(aetdj.ResultValue),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 11
) AS [2_25_cnt] 

--,(SELECT (ISNULL(AVG(foo.ResultValue),0) + ISNULL(AVG(foo.MAXVALUE),0))/2  FROM(
--SELECT aetdj.ResultValue AS [ResultValue],ia.MaxValue FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID = aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId = cgj.CompetitiveGroupID
--INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = aj.ApplicationID
--INNER JOIN InstitutionAchievements ia (NOLOCK) ON ia.IdAchievement = ina.IdAchievement
--WHERE  c.CampaignTypeID = 1 AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID = 8  AND ia.Idcategory = 12 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID,ia.MaxValue) foo) AS [2_26] 
,(SELECT (ISNULL(AVG(aetdj.ResultValue),0) + ISNULL(AVG(ia.MAXVALUE),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 12
) AS [2_26] 
,(SELECT (ISNULL(Count(aetdj.ResultValue),0)) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN IndividualAchivement ina (NOLOCK) ON ina.ApplicationID = app.ApplicationID
INNER JOIN InstitutionAchievements ia (NOLOCK) ON  ia.IdAchievement = ina.IdAchievement
Where app.CompetitiveGroupID = cg.CompetitiveGroupID and CampaignTypeID = 1
AND ia.Idcategory = 12
) AS [2_26_cnt] 
---------------------------------------------------------------------------------------------------------

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) AND app.StatusID IN (2,4)
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_1] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_24 app
Where CompetitiveGroupID = cg.CompetitiveGroupID
) AS [3_1] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) AND app.StatusID IN (2,4)
-- and aetd1.BenefitID = 1 
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID   ) AS [3_2] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_24 app
INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
and aetd1.BenefitID = 1 
) AS [3_2] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) AND app.StatusID IN (2,4)
-- AND cg1.EducationSourceId = 20
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID   ) AS [3_3] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_24 app
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceId = 20
) AS [3_3] 


--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) AND app.StatusID IN (2,4)
-- AND cg1.EducationSourceId = 16
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_4]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_24 app
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceId = 16
) AS [3_4] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) AND app.StatusID IN (2,4) 
--and cg1.EducationSourceId NOT IN (16, 20) 
-- and aetd1.BenefitID is null
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_5] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_24 app
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceId NOT IN (16, 20) 
 --AND aetd1.BenefitID is null
) AS [3_5] 



,(SELECT COUNT(G.APPLICATIONID) From gzgu_3_64 g
  --inner join CompetitiveGroup cg (nolock) on cg.CompetitiveGroupID = g.CompetitiveGroupID 
  Where g.CompetitiveGroupID=cg.CompetitiveGroupID --and g.DocumentTypeID = 11
) AS [3_6] 

,(SELECT COUNT(G.APPLICATIONID) From gzgu_3_64 g
  --inner join CompetitiveGroup cg (nolock) on cg.CompetitiveGroupID = g.CompetitiveGroupID 
  Where g.CompetitiveGroupID=cg.CompetitiveGroupID and g.DocumentTypeID = 11
) AS [3_12] 
,(SELECT COUNT(G.APPLICATIONID) From gzgu_3_64 g
  --inner join CompetitiveGroup cg (nolock) on cg.CompetitiveGroupID = g.CompetitiveGroupID 
  Where g.CompetitiveGroupID=cg.CompetitiveGroupID and g.DocumentTypeID = 30) AS [3_13] 
,(SELECT COUNT(G.APPLICATIONID) From gzgu_3_64 g
 -- inner join CompetitiveGroup cg (nolock) on cg.CompetitiveGroupID = g.CompetitiveGroupID 
  Where g.CompetitiveGroupID=cg.CompetitiveGroupID and g.DocumentTypeID = 31
  ) AS [3_14] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL  
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_15] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
) AS [3_15] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL  
-- and aetd1.BenefitID = 1 
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_16] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 and aetd1.BenefitID = 1 
) AS [3_16] 


--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL 
-- AND cg1.EducationSourceId = 20
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_17] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceId = 20
) AS [3_17] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL 
-- AND cg1.EducationSourceId = 16
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_18]
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceId = 16
) AS [3_18] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL 
-- and aetd1.BenefitID is null
-- AND cg1.EducationSourceId NOT IN(16,20)
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_19] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetd1 (NOLOCK) ON app.ApplicationID = aetd1.ApplicationID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceId NOT IN (16,20)
 AND aetd1.BenefitID is null
) AS [3_19] 


--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL 
-- AND ed1.DocumentTypeID =11
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_20] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND ed1.DocumentTypeID = 11
) AS [3_20] 


--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL 
-- AND ed1.DocumentTypeID = 30
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_26] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND ed1.DocumentTypeID = 30
) AS [3_26] 

--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--WHERE c.CampaignTypeID IN (1,2) AND cg1.EducationLevelID IN (2,4,5) 
--AND app.StatusID=8 AND acgi1.OrderOfAdmissionID IS NOT NULL AND acgi1.OrderOfExceptionID IS NULL 
-- AND ed1.DocumentTypeID = 31
-- and cg1.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [3_27] 
,(SELECT ISNULL(COUNT(DISTINCT app.ApplicationID),0) 
FROM gzgu_App_8 app
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND ed1.DocumentTypeID = 31
) AS [3_27] 

------------------------------------------------------------------------------------------------

--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and  foo.[1] = 1 and foo.[2] = 0 
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
-- AND aetdj.SourceID=1 AND ed1.DocumentTypeID =11 and cgj.EducationSourceID=20 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_29] 

,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 11 and app.EducationSourceID=20 
) AS [3_29] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 11 and app.EducationSourceID=20 
) AS [3_29_cnt] 


--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and  foo.[1] = 1 and foo.[2] = 0 
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
--AND aetdj.SourceID=1 AND ed1.DocumentTypeID =11 and cgj.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_30] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16,20)
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 11  AND foo.BenefitID IS NULL 
) AS [3_30] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16,20)
 AND foo.SourceID=1 AND ed1.DocumentTypeID =11  AND foo.BenefitID IS NULL 
) AS [3_30_cnt] 


--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and  foo.[1] = 1 and foo.[2] = 0 
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
--AND aetdj.SourceID=1 AND ed1.DocumentTypeID =30 and cgj.EducationSourceID = 20 AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_31] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID = 20
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 30 AND foo.BenefitID IS NULL 
) AS [3_31] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID = 20
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 30 AND foo.BenefitID IS NULL 
) AS [3_31_cnt] 


--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and  foo.[1] = 1 and foo.[2] = 0 
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
--AND aetdj.SourceID=1 AND ed1.DocumentTypeID =30 and cgj.EducationSourceID not in(16,20) AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_32] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16, 20)
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 30 AND foo.BenefitID IS NULL 
) AS [3_32] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16, 20)
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 30 AND foo.BenefitID IS NULL 
) AS [3_32_cnt] 

--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and  foo.[1] = 1 and foo.[2] = 0 
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
--AND aetdj.SourceID=1 AND ed1.DocumentTypeID =31 and cgj.EducationSourceID=20 AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_33] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID = 20
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 31 AND foo.BenefitID IS NULL 
) AS [3_33] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID = 20
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 31 AND foo.BenefitID IS NULL 
) AS [3_33_cnt] 

--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and  foo.[1] = 1 and foo.[2] = 0 
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
-- AND aetdj.SourceID=1 AND ed1.DocumentTypeID =31 and cgj.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_34] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16, 20)
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 31 AND foo.BenefitID IS NULL 
) AS [3_34] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and  foo.[1] = 1 and foo.[2] = 0 
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1 AND gas.[s2] = 0)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16, 20)
 AND foo.SourceID=1 AND ed1.DocumentTypeID = 31 AND foo.BenefitID IS NULL 
) AS [3_34_cnt] 

------------------------------------------------------------

--,(SELECT ISNULL(AVG(foo1.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and (( foo.[1] = 1 and foo.[2] = 0 ) or (foo.[1] = 1 and foo.[2] = 1))
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
-- AND ed1.DocumentTypeID =11 and cgj.EducationSourceID=20 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo1) AS [3_36] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID = 20
 AND ed1.DocumentTypeID = 11
) AS [3_36] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID = 20
 AND ed1.DocumentTypeID = 11
) AS [3_36_cnt] 


--,(SELECT ISNULL(AVG(foo.ResultValue),0) FROM(
--SELECT aetdj.ResultValue AS [ResultValue] 
--FROM Application aj (NOLOCK)
--INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON aj.ApplicationID=aetdj.ApplicationID
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON aj.ApplicationID=acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON acgij.CompetitiveGroupId=cgj.CompetitiveGroupID
--INNER JOIN EntrantDocument ed1 (NOLOCK) ON aetdj.EntrantDocumentID = ed1.EntrantDocumentID
--INNER JOIN gzgu_aetd foo ON aj.ApplicationID = foo.ApplicationID and (( foo.[1] = 1 and foo.[2] = 0 ) or (foo.[1] = 1 and foo.[2] = 1))
--WHERE c.CampaignTypeID IN (1,2) AND cgj.EducationLevelID IN (2,4,5) AND aj.StatusID=8 
--AND ed1.DocumentTypeID =11 and cgj.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL 
--AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL
--GROUP BY aetdj.ResultValue, aetdj.SubjectID, aj.ApplicationID) foo) AS [3_37] 
,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16, 20)
 AND ed1.DocumentTypeID = 11 AND aetdj.BenefitID IS NULL 
) AS [3_37] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
 AND app.EducationSourceID not in (16, 20)
 AND ed1.DocumentTypeID = 11 AND aetdj.BenefitID IS NULL 
) AS [3_37_cnt] 


,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID =30 and app.EducationSourceID=20 AND aetdj.BenefitID IS NULL 
) AS [3_38] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID =30 and app.EducationSourceID=20 AND aetdj.BenefitID IS NULL  
) AS [3_38_cnt] 



,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID =30 and app.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL 
) AS [3_39] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID =30 and app.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL  
) AS [3_39_cnt] 



,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID = 31 and app.EducationSourceID = 20 AND aetdj.BenefitID IS NULL 
) AS [3_40] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID = 31 and app.EducationSourceID = 20 AND aetdj.BenefitID IS NULL  
) AS [3_40_cnt] 


,(SELECT ISNULL(AVG(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID =31 and app.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL 
) AS [3_41] 
,(SELECT ISNULL(Count(foo.ResultValue),0)
FROM gzgu_App_8 app
INNER JOIN gzgu_aetd foo ON app.ApplicationID = foo.ApplicationID --and ((foo.[1] = 1 and foo.[2] = 0) or (foo.[1] = 1 and foo.[2] = 1))
INNER JOIN gzgu_App_Sources gas ON app.ApplicationID = gas.ApplicationID AND (gas.[s1] = 1)
INNER JOIN ApplicationEntranceTestDocument aetdj (NOLOCK) ON app.ApplicationID = aetdj.ApplicationID and aetdj.ID = foo.ID
inner join ApplicationEntrantDocument aed (nolock) on aed.ApplicationID = app.ApplicationID
INNER JOIN EntrantDocument ed1 (NOLOCK) ON aed.EntrantDocumentID = ed1.EntrantDocumentID
Where app.CompetitiveGroupID = cg.CompetitiveGroupID
AND ed1.DocumentTypeID = 31 and app.EducationSourceID not in (16,20) AND aetdj.BenefitID IS NULL  
) AS [3_41_cnt] 


FROM Institution i (NOLOCK)
INNER JOIN dbo.Campaign c (NOLOCK) ON i.InstitutionID = c.InstitutionID AND c.YearStart = @year AND c.CampaignTypeID IN (1,2)  
INNER JOIN CompetitiveGroup cg (NOLOCK) ON c.CampaignID = cg.CampaignID
INNER JOIN GZGU_KCP_F.dbo.AdmissionVolume av (NOLOCK) ON cg.DirectionID=av.DirectionID 
and av.AdmissionItemTypeID = cg.EducationLevelID
AND av.InstitutionID=i.InstitutionID AND cg.CampaignID = av.CampaignID --and cg.EducationSourceId != 15
LEFT JOIN GZGU_KCP_F.dbo.DistributedAdmissionVolume adv (NOLOCK) ON av.AdmissionVolumeID = adv.AdmissionVolumeID 
AND (ISNULL(adv.NumberBudgetO,0)+ISNULL(adv.NumberBudgetOZ,0)+ISNULL(adv.NumberBudgetZ,0)+ISNULL(adv.NumberQuotaO,0)+ISNULL(adv.NumberQuotaOZ,0)+ISNULL(adv.NumberQuotaZ,0)+ISNULL(adv.NumberTargetO,0)+ISNULL(adv.NumberTargetOZ,0)+ISNULL(adv.NumberTargetZ,0))<>0 
LEFT JOIN GZGU_KCP_F.dbo.CompetitiveGroupItem cgij (NOLOCK) ON cg.CompetitiveGroupID = cgij.CompetitiveGroupID
LEFT join CompetitiveGroupItem cgijp (nolock) on cgijp.CompetitiveGroupID = cg.CompetitiveGroupID AND CG.EducationSourceId = 15


WHERE i.InstitutionID not in (6346,7272,6348)
--and cg.CreatedDate < @regDate AND CG.ModifiedDate < @regDate
and (ISNULL(cgij.NumberBudgetO,0)+ISNULL(cgij.NumberQuotaO,0)+ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberBudgetOZ,0)+ISNULL(cgij.NumberQuotaOZ,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberBudgetZ,0)+ISNULL(cgij.NumberQuotaZ,0)+ISNULL(cgij.NumberTargetZ,0)) != 0
	
GROUP BY 
cg.CompetitiveGroupID
,i.institutionID
,cg.DirectionID
,cg.EducationFormId
,cg.EducationSourceId 
,adv.IdLevelBudget
,cg.DirectionID
,cgij.NumberBudgetO,cgijp.NumberPaidO,cgij.NumberQuotaO,cgij.NumberTargetO,cgij.NumberBudgetOZ,cgijp.NumberPaidOZ,cgij.NumberQuotaOZ,cgij.NumberTargetOZ,cgij.NumberBudgetZ,cgijp.NumberPaidZ,cgij.NumberQuotaZ,cgij.NumberTargetZ


END
