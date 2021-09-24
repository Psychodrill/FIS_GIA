USE [gvuz_start_2016]
GO
/****** Object:  StoredProcedure [dbo].[GZGU_STEP4]    Script Date: 08/11/2017 11:28:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GZGU_STEP4]
AS
BEGIN

DECLARE @year INT = 2017;


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
	i.InstitutionID
	,cg.CompetitiveGroupID
	,cg.DirectionID
	,cg.EducationFormId
	,cg.EducationSourceId
	,CASE WHEN cg.EducationSourceId = 15 THEN 4 ELSE isnull(IdLevelBudget, 0) END AS IdLevelBudget,
	
	'0' AS [1_4],
	'0'  AS[1_5],
	'0'  AS[1_6],
	'0'  AS[1_7],
	'0'  AS[1_8],
	'0'  AS[1_9],
	'0' AS [1_10],
	'0'  AS[1_11],
	'0'  AS[1_12],
	'0'  AS[1_13],
	'0'  AS[1_14],
	'0'  AS[1_15],
	'0'  AS[1_16],
	'0'  AS[1_17],
	'0'  AS[1_18],

	'0'  AS[1_19],
	'0'  AS[1_20],
	--[1_21],
	--[1_22],
	--[1_23],
	'0'  AS[1_24],
	'0'  AS[1_25],
	'0'  AS[1_26],
	'0'  AS[1_27],
	'0'  AS[1_28],
	'0'  AS[1_29],
	'0'  AS[1_30],
	'0'  AS[1_31],
	'0' AS [1_32],
	'0'  AS[1_33],
	'0'  AS[1_34],
	'0'  AS[1_35],
	'0'  AS[1_36],
	'0'  AS[1_37],
	'0'  AS[1_38],
	'0'  AS[1_39],
	'0'  AS[1_40],
	'0'  AS[1_41],
	'0'  AS[1_42]



,ISNULL(cgij.NumberBudgetO,0)+ISNULL(cgijp.NumberPaidO,0)+ISNULL(cgij.NumberQuotaO,0)+ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberBudgetOZ,0)+ISNULL(cgijp.NumberPaidOZ,0)+ISNULL(cgij.NumberQuotaOZ,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberBudgetZ,0)+ISNULL(cgijp.NumberPaidZ,0)+ISNULL(cgij.NumberQuotaZ,0)+ISNULL(cgij.NumberTargetZ,0) AS [1_43]
,ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberTargetZ,0) AS [1_44]



,(SELECT COUNT(DISTINCT app.ApplicationID)
FROM Application app (NOLOCK)
INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
WHERE  cg1.EducationLevelID IN (18)  AND app.StatusID IN (2,4,3,8)  AND cg1.CompetitiveGroupID=cg.CompetitiveGroupID
) AS [1_45] 



,(SELECT COUNT(DISTINCT app.ApplicationID)
FROM Application app (NOLOCK)
INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
WHERE cg1.EducationLevelID IN (18)   AND app.StatusID IN (2,4,3,8)  AND cg1.CompetitiveGroupID=cg.CompetitiveGroupID AND cg1.EducationSourceId = 16 
) AS [1_46] 

,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM Application app (NOLOCK)
INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID=acgij.CompetitiveGroupId 
WHERE cgj.EducationLevelID IN (18)   AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
 AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID ) AS [1_47]



,(SELECT COUNT(DISTINCT app.ApplicationID) 
FROM Application app (NOLOCK)
INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID=acgij.CompetitiveGroupId 
WHERE cgj.EducationLevelID IN (18)   AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
 AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND cgj.EducationSourceId = 16 ) AS [1_48],


	'0' AS [2_1], 
	'0' AS [2_1_cnt], 
	'0' AS [2_2], 
	'0' AS [2_2_cnt], 
	'0' AS [2_3], 
	'0' AS [2_3_cnt], 
	'0' AS [2_4], 
	'0' AS [2_4_cnt], 
	'0' AS [2_5], 
	'0' AS [2_5_cnt], 
	'0' AS [2_6], 
	'0' AS [2_6_cnt], 
	'0' AS [2_7], 
	'0' AS[2_7_cnt], 
	'0' AS[2_8], 
	'0' AS[2_8_cnt], 

	'0' AS[2_9], 
	'0' AS[2_10], 
	'0' AS[2_11], 
	'0' AS[2_12], 
	'0' AS[2_13], 
	'0' AS[2_14], 

	'0' AS[2_15],
	'0' AS[2_16],
	'0' AS[2_17],
	'0' AS[2_18],
	'0' AS[2_19],
	'0' AS[2_20],

	'0' AS[2_21],
	'0' AS[2_21_cnt],
	'0' AS[2_22],
	'0' AS[2_22_cnt],
	'0' AS[2_23],
	'0' AS[2_23_cnt],
	'0' AS[2_24],
	'0' AS[2_24_cnt],
	'0' AS[2_25],
	'0' AS[2_25_cnt],
	'0' AS[2_26],
	'0' AS[2_26_cnt],

	'0' AS[3_1],
	'0' AS[3_2],
	'0' AS[3_3],
	'0' AS[3_4],
	'0' AS[3_5],

	'0' AS[3_6],
	'0' AS[3_12],
	'0' AS[3_13],
	'0' AS[3_14],
	'0' AS[3_15],
	'0' AS[3_16],
	'0' AS[3_17],
	'0' AS[3_18],
	'0' AS[3_19],
	'0' AS[3_20],

	'0' AS[3_26],
	'0' AS[3_27],

	'0' AS[3_29],
	'0' AS[3_29_cnt],
	'0' AS[3_30],
	'0' AS[3_30_cnt],
	'0' AS[3_31],
	'0' AS[3_31_cnt],
	'0' AS[3_32],
	'0' AS[3_32_cnt],
	'0' AS[3_33],
	'0' AS[3_33_cnt],
	'0' AS [3_34],
	'0' AS [3_34_cnt],

	'0' AS[3_36],
	'0' AS[3_36_cnt],
	'0' AS[3_37],
	'0' AS[3_37_cnt],
	'0' AS[3_38],
	'0' AS[3_38_cnt],
	'0' AS[3_39],
	'0' AS[3_39_cnt],
	'0' AS[3_40],
	'0' AS[3_40_cnt],
	'0' AS[3_41],
	'0' AS[3_41_cnt]
	
	

FROM Institution i (NOLOCK)
INNER JOIN dbo.Campaign c (NOLOCK) ON i.InstitutionID = c.InstitutionID AND c.YearStart = @year AND c.CampaignTypeID = 4 
INNER JOIN CompetitiveGroup cg (NOLOCK) ON c.CampaignID = cg.CampaignID

INNER JOIN GZGU_KCP_F.dbo.AdmissionVolume av (NOLOCK) ON cg.DirectionID=av.DirectionID 
and av.AdmissionItemTypeID = cg.EducationLevelID
AND av.InstitutionID=i.InstitutionID AND cg.CampaignID = av.CampaignID --and cg.EducationSourceId != 15
LEFT JOIN GZGU_KCP_F.dbo.DistributedAdmissionVolume adv (NOLOCK) ON av.AdmissionVolumeID = adv.AdmissionVolumeID 
AND (ISNULL(adv.NumberBudgetO,0)+ISNULL(adv.NumberBudgetOZ,0)+ISNULL(adv.NumberBudgetZ,0)+ISNULL(adv.NumberQuotaO,0)+ISNULL(adv.NumberQuotaOZ,0)+ISNULL(adv.NumberQuotaZ,0)+ISNULL(adv.NumberTargetO,0)+ISNULL(adv.NumberTargetOZ,0)+ISNULL(adv.NumberTargetZ,0))<>0 
LEFT JOIN GZGU_KCP_F.dbo.CompetitiveGroupItem cgij (NOLOCK) ON cg.CompetitiveGroupID = cgij.CompetitiveGroupID
LEFT join CompetitiveGroupItem cgijp (nolock) on cgijp.CompetitiveGroupID = cg.CompetitiveGroupID AND CG.EducationSourceId = 15



WHERE i.InstitutionID not in (6346,7272,6348)
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











--SELECT
--org.INN AS [INN]
--,org.KPP AS [KPP]
--,org.FullName AS [InsName] 
--,num11.[DirCode]
--,num11.[1_1]
--,CASE WHEN ait.Name = 'Очная форма' THEN 1 WHEN ait.Name = 'Очно-заочная (вечерняя)' THEN 2 
--WHEN  ait.Name = 'Заочная форма' THEN 3 END AS [1_2]
--,gtr.IdLevelBudget AS [1_3]

--,'0' AS [1_4]
--,'0' AS [1_5]
--,'0' AS [1_6]
--,'0' AS [1_7]
--,'0' AS [1_8]
--,'0' AS [1_9]
--,'0' AS [1_10]
--,'0' AS [1_11]
--,'0' AS [1_12]
--,'0' AS [1_13]
--,'0' AS [1_14]
--,'0' AS [1_15]
--,'0' AS [1_16]
--,'0' AS [1_17]
--,'0' AS [1_18]
--,'0' AS [1_19]
--,'0' AS [1_20]
--,'0' AS [1_24]
--,'0' AS [1_25]
--,'0' AS [1_26]
--,'0' AS [1_27]
--,'0' AS [1_28]
--,'0' AS [1_29]
--,'0' AS [1_30]
--,'0' AS [1_31]
--,'0' AS [1_32]
--,'0' AS [1_33]
--,'0' AS [1_34]
--,'0' AS [1_35]
--,'0' AS [1_36]
--,'0' AS [1_37]
--,'0' AS [1_38]
--,'0' AS [1_39]
--,'0' AS [1_40]
--,'0' AS [1_41]
--,'0' AS [1_42]
--,SUM(num11.[1_43]) AS [1_43]
--,SUM(num11.[1_44]) AS [1_44]
--,SUM(num11.[1_45]) AS [1_45]
--,SUM(num11.[1_46]) AS [1_46]
--,SUM(num11.[1_47]) AS [1_47]
--,SUM(num11.[1_48]) AS [1_48]
----,SUM(num11.[1_49]) AS [1_49]
--,'0' AS [2_1]
--,'0' AS [2_2]
--,'0' AS [2_3]
--,'0' AS [2_4]
--,'0' AS [2_5]
--,'0' AS [2_6]
--,'0' AS [2_7]
--,'0' AS [2_8]
--,'0' AS [2_9]
--,'0' AS [2_10]
--,'0' AS [2_11]
--,'0' AS [2_12]
--,'0' AS [2_13]
--,'0' AS [2_14]
--,'0' AS [2_15]
--,'0' AS [2_16]
--,'0' AS [2_17]
--,'0' AS [2_18]
--,'0' AS [2_19]
--,'0' AS [2_20]
--,'0' AS [2_21]
--,'0' AS [2_22]
--,'0' AS [2_23]
--,'0' AS [2_24]
--,'0' AS [2_25]
--,'0' AS [2_26]
--,'0' AS [3_1]
--,'0' AS [3_2]
--,'0' AS [3_3]
--,'0' AS [3_4]
--,'0' AS [3_5]
--,'0' AS [3_6]
--,'0' AS [3_12]
--,'0' AS [3_13]
--,'0' AS [3_15]
--,'0' AS [3_16]
--,'0' AS [3_17]
--,'0' AS [3_18]
--,'0' AS [3_19]
--,'0' AS [3_20]
--,'0' AS [3_26]
--,'0' AS [3_27]
--,'0' AS [3_29]
--,'0' AS [3_30]
--,'0' AS [3_31]
--,'0' AS [3_32]
--,'0' AS [3_33]
--,'0' AS [3_34]
--,'0' AS [3_36]
--,'0' AS [3_37]
--,'0' AS [3_38]
--,'0' AS [3_39]
--,'0' AS [3_40]
--,'0' AS [3_41]
--FROM(

--SELECT DISTINCT
--	i.EsrpOrgID 
--,d.NewCode AS [DirCode]
--,d.Name as [1_1]
--,CASE WHEN ait.Name = 'Очно-заочная (вечерняя)' THEN 'Очно-заочная' ELSE ait.Name END AS [1_2]
--,CASE WHEN cg.EducationSourceId = 15 THEN 4 ELSE isnull(adv.IdLevelBudget, 0) END AS IdLevelBudget
--,cg.CompetitiveGroupID





--,ISNULL(cgij.NumberBudgetO,0)+ISNULL(cgij.NumberPaidO,0)+ISNULL(cgij.NumberQuotaO,0)+ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberBudgetOZ,0)+ISNULL(cgij.NumberPaidOZ,0)+ISNULL(cgij.NumberQuotaOZ,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberBudgetZ,0)+ISNULL(cgij.NumberPaidZ,0)+ISNULL(cgij.NumberQuotaZ,0)+ISNULL(cgij.NumberTargetZ,0) AS [1_43]
--,ISNULL(cgij.NumberTargetO,0)+ISNULL(cgij.NumberTargetOZ,0)+ISNULL(cgij.NumberTargetZ,0) AS [1_44]



--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--WHERE c.CampaignTypeID IN (4) AND cg1.EducationLevelID IN (6) AND app.StatusID IN (2,4,3,8)  AND cg1.CompetitiveGroupID=cg.CompetitiveGroupID
--) AS [1_45] 



--,(SELECT COUNT(DISTINCT app.ApplicationID)
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgi1 (NOLOCK) ON app.ApplicationID = acgi1.ApplicationId
--INNER JOIN CompetitiveGroup cg1 (NOLOCK) ON acgi1.CompetitiveGroupId = cg1.CompetitiveGroupID
--WHERE c.CampaignTypeID IN (4) AND cg1.EducationLevelID IN (6)  AND app.StatusID IN (2,4,3,8)  AND cg1.CompetitiveGroupID=cg.CompetitiveGroupID AND cg1.EducationSourceId = 16 
-- ) AS [1_46] 

--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID=acgij.CompetitiveGroupId 
--WHERE c.CampaignTypeID IN (4) AND cgj.EducationLevelID IN (6)  AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID  ) AS [1_47]



--,(SELECT COUNT(DISTINCT app.ApplicationID) 
--FROM Application app (NOLOCK)
--INNER JOIN ApplicationCompetitiveGroupItem acgij (NOLOCK) ON app.ApplicationID = acgij.ApplicationId
--INNER JOIN CompetitiveGroup cgj (NOLOCK) ON cgj.CompetitiveGroupID=acgij.CompetitiveGroupId 
--WHERE c.CampaignTypeID IN (4) AND cgj.EducationLevelID IN (6)  AND app.StatusID=8 AND acgij.OrderOfAdmissionID IS NOT NULL AND acgij.OrderOfExceptionID IS NULL 
-- AND cgj.CompetitiveGroupID=cg.CompetitiveGroupID AND cgj.EducationSourceId = 16 ) AS [1_48]



--FROM Institution i (NOLOCK)
--INNER JOIN Campaign c (NOLOCK) ON i.InstitutionID = c.InstitutionID AND c.YearStart = @year AND c.CampaignTypeID = 4  
--INNER JOIN CompetitiveGroup cg (NOLOCK) ON c.CampaignID = cg.CampaignID
--INNER JOIN AdmissionVolume av (NOLOCK) ON cg.DirectionID=av.DirectionID 
--and av.AdmissionItemTypeID = cg.EducationLevelID
--AND av.InstitutionID=i.InstitutionID AND cg.CampaignID = av.CampaignID
----INNER JOIN gzgu_1_8 (NOLOCK) gz on cg.CompetitiveGroupID = gz.CompetitiveGroupID
--LEFT JOIN DistributedAdmissionVolume adv (NOLOCK) ON av.AdmissionVolumeID = adv.AdmissionVolumeID 
--AND (ISNULL(adv.NumberBudgetO,0)+ISNULL(adv.NumberBudgetOZ,0)+ISNULL(adv.NumberBudgetZ,0)+ISNULL(adv.NumberQuotaO,0)+ISNULL(adv.NumberQuotaOZ,0)+ISNULL(adv.NumberQuotaZ,0)+ISNULL(adv.NumberTargetO,0)+ISNULL(adv.NumberTargetOZ,0)+ISNULL(adv.NumberTargetZ,0))<>0 

----LEFT JOIN LevelBudget lb (NOLOCK) ON adv.IdLevelBudget=lb.IdLevelBudget


--LEFT JOIN CompetitiveGroupItem cgij (NOLOCK) ON cg.CompetitiveGroupID = cgij.CompetitiveGroupID
--WHERE i.InstitutionID not in (6346,7272,6348)

--GROUP BY 

--cg.CompetitiveGroupID
--,i.institutionID
--,cg.DirectionID
--,cg.EducationFormId
--,cg.EducationSourceId 
--,adv.IdLevelBudget
--,cg.DirectionID
--,cgij.NumberBudgetO,cgij.NumberPaidO,cgij.NumberQuotaO,cgij.NumberTargetO,cgij.NumberBudgetOZ,cgij.NumberPaidOZ,cgij.NumberQuotaOZ,cgij.NumberTargetOZ,cgij.NumberBudgetZ,cgij.NumberPaidZ,cgij.NumberQuotaZ,cgij.NumberTargetZ



--) num11
--INNER JOIN esrp_prod.dbo.Organization2010 org (NOLOCK) ON num11.EsrpOrgID = org.Id AND org.StatusId IN (1,2) 
--GROUP BY 

--num11.InstitutionID
--,num11.EsrpOrgID
--,num11.NewCode
--,num11.EducationFormId
--,num11.IdLevelBudget
--,num11.DirectionID 
--,num11.Name
--,org.INN
--,org.KPP
--,org.FullName
--,num11.Name
----ORDER BY num11.[DirCode],num11.[1_1],num11.[1_2],num11.[1_3]

--END
