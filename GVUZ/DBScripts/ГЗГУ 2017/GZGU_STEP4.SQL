USE [gvuz_start_2016]
GO
/****** Object:  StoredProcedure [dbo].[GZGU_STEP3]    Script Date: 08/11/2017 11:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GZGU_STEP3]
AS
BEGIN
--DELETE  FROM gzgu_temp_result
--SELECT * FROM gzgu_temp_result

INSERT INTO GZGUBackFull_2017
select  
org.INN AS [INN]
,org.KPP AS [KPP]
,org.FullName AS [InsName]  
,d.NewCode [DirCode]
,d.Name [1_1]
,CASE WHEN ait.Name = 'Очная форма' THEN 1 WHEN ait.Name = 'Очно-заочная (вечерняя)' THEN 2 
WHEN  ait.Name = 'Заочная форма' THEN 3 END AS [1_2]
,gtr.IdLevelBudget AS [1_3],
	ISNULL(SUM(gtr.[1_4]),0) AS [1_4],
	ISNULL(SUM(gtr.[1_5]),0) AS [1_5],
	ISNULL(SUM(gtr.[1_6]),0) AS [1_6],
	ISNULL(SUM(gtr.[1_7]),0) AS[1_7],
	ISNULL(SUM(gtr.[1_8]),0) AS [1_8],
	ISNULL(SUM(gtr.[1_9]),0) AS [1_9],
	ISNULL(SUM(gtr.[1_10]),0) AS [1_10],
	ISNULL(SUM(gtr.[1_11]),0) AS [1_11],
	ISNULL(SUM(gtr.[1_12]),0) AS [1_12],
	ISNULL(SUM(gtr.[1_13]),0) AS [1_13],
	ISNULL(SUM(gtr.[1_14]),0) AS [1_14],
	ISNULL(SUM(gtr.[1_15]),0) AS [1_15],
	ISNULL(SUM(gtr.[1_16]),0) AS [1_16],
	ISNULL(SUM(gtr.[1_17]),0) AS [1_17],
	ISNULL(SUM(gtr.[1_18]),0) AS[1_18],

	ISNULL(SUM(gtr.[1_19]),0) AS [1_19],
	ISNULL(SUM(gtr.[1_20]),0) AS [1_20],
	--[1_21],
	--[1_22],
	--[1_23],
	ISNULL(SUM(gtr.[1_24]),0) AS [1_24],
	ISNULL(SUM(gtr.[1_25]),0) AS [1_25],
	ISNULL(SUM(gtr.[1_26]),0) AS [1_26],
	ISNULL(SUM(gtr.[1_27]),0) AS [1_27],
	ISNULL(SUM(gtr.[1_28]),0) AS [1_28],
	ISNULL(SUM(gtr.[1_29]),0) AS [1_29],
	ISNULL(SUM(gtr.[1_30]),0) AS [1_30],
	ISNULL(SUM(gtr.[1_31]),0) AS [1_31],
	ISNULL(SUM(gtr.[1_32]),0) AS [1_32],
	ISNULL(SUM(gtr.[1_33]),0) AS [1_33],
	ISNULL(SUM(gtr.[1_34]),0) AS [1_34],
	ISNULL(SUM(gtr.[1_35]),0) AS [1_35],
	ISNULL(SUM(gtr.[1_36]),0) AS [1_36],
	ISNULL(SUM(gtr.[1_37]),0) AS [1_37],
	ISNULL(SUM(gtr.[1_38]),0) AS [1_38],
	ISNULL(SUM(gtr.[1_39]),0) AS [1_39],
	ISNULL(SUM(gtr.[1_40]),0) AS [1_40],
	ISNULL(SUM(gtr.[1_41]),0) AS [1_41],
	ISNULL(SUM(gtr.[1_42]),0) AS [1_42],
	ISNULL(SUM(gtr.[1_43]),0) AS [1_43],
	ISNULL(SUM(gtr.[1_44]),0) AS [1_44],
	ISNULL(SUM(gtr.[1_45]),0) AS [1_45],
	ISNULL(SUM(gtr.[1_46]),0) AS [1_46],
	ISNULL(SUM(gtr.[1_47]),0) AS [1_47],
	ISNULL(SUM(gtr.[1_48]),0) AS [1_48],

	(
SELECT ISNULL(Sum(t.[2_1] * t.[2_1_cnt])/Sum(t.[2_1_cnt]) , 0)
From gzgu_temp_result t
where t.[2_1_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_1] ,
	
	(
SELECT ISNULL(Sum(t.[2_2] * t.[2_2_cnt])/Sum(t.[2_2_cnt]) , 0)
From gzgu_temp_result t
where t.[2_2_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_2] 
	, 
(
SELECT ISNULL(Sum(t.[2_3] * t.[2_3_cnt])/Sum(t.[2_3_cnt]) , 0)
From gzgu_temp_result t
where t.[2_3_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_3] 
	, 
	(
SELECT ISNULL(Sum(t.[2_4] * t.[2_4_cnt])/Sum(t.[2_4_cnt]) , 0)
From gzgu_temp_result t
where t.[2_4_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_4] 
	, 
	(
SELECT ISNULL(Sum(t.[2_5] * t.[2_5_cnt])/Sum(t.[2_5_cnt]) , 0)
From gzgu_temp_result t
where t.[2_5_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_5] 
	, 
	(
SELECT ISNULL(Sum(t.[2_6] * t.[2_6_cnt])/Sum(t.[2_6_cnt]) , 0)
From gzgu_temp_result t
where t.[2_6_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_6] 
	, 
	(
SELECT ISNULL(Sum(t.[2_7] * t.[2_7_cnt])/Sum(t.[2_7_cnt]) , 0)
From gzgu_temp_result t
where t.[2_7_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_7] 
	, 
	(
SELECT ISNULL(Sum(t.[2_8] * t.[2_8_cnt])/Sum(t.[2_8_cnt]) , 0)
From gzgu_temp_result t
where t.[2_8_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_8] 
	,  

	ISNULL(AVG(gtr.[2_9]),0) AS [2_9], 
	ISNULL(AVG(gtr.[2_10]),0) AS [2_10], 
	ISNULL(AVG(gtr.[2_11]),0) AS [2_11], 
	ISNULL(AVG(gtr.[2_12]),0) AS [2_12], 
	ISNULL(AVG(gtr.[2_13]),0) AS [2_13], 
	ISNULL(AVG(gtr.[2_14]),0) AS [2_14], 

	ISNULL(SUM(gtr.[2_15]),0) AS [2_15],
	ISNULL(SUM(gtr.[2_16]),0) AS [2_16],
	ISNULL(SUM(gtr.[2_17]),0) AS [2_17],
	ISNULL(SUM(gtr.[2_18]),0) AS [2_18],
	ISNULL(SUM(gtr.[2_19]),0) AS [2_19],
	ISNULL(SUM(gtr.[2_20]),0) AS [2_20],

	(
SELECT ISNULL(Sum(t.[2_21] * t.[2_21_cnt])/Sum(t.[2_21_cnt]) , 0)
From gzgu_temp_result t
where t.[2_21_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_21] ,
	
		(
SELECT ISNULL(Sum(t.[2_22] * t.[2_22_cnt])/Sum(t.[2_22_cnt]) , 0)
From gzgu_temp_result t
where t.[2_22_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_22] ,
	(
SELECT ISNULL(Sum(t.[2_23] * t.[2_23_cnt])/Sum(t.[2_23_cnt]) , 0)
From gzgu_temp_result t
where t.[2_23_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_23] ,
	(
SELECT ISNULL(Sum(t.[2_24] * t.[2_24_cnt])/Sum(t.[2_24_cnt]) , 0)
From gzgu_temp_result t
where t.[2_24_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_24] ,
	(
SELECT ISNULL(Sum(t.[2_25] * t.[2_25_cnt])/Sum(t.[2_25_cnt]) , 0)
From gzgu_temp_result t
where t.[2_25_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_25] ,
	(
SELECT ISNULL(Sum(t.[2_21] * t.[2_21_cnt])/Sum(t.[2_21_cnt]) , 0)
From gzgu_temp_result t
where t.[2_21_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [2_21] ,

	ISNULL(SUM(gtr.[3_1]),0) AS [3_1],
	ISNULL(SUM(gtr.[3_2]),0) AS [3_2],
	ISNULL(SUM(gtr.[3_3]),0) AS [3_3],
	ISNULL(SUM(gtr.[3_4]),0) AS [3_4],
	ISNULL(SUM(gtr.[3_5]),0) AS [3_5],

	ISNULL(SUM(gtr.[3_6]),0) AS [3_6],
	ISNULL(SUM(gtr.[3_12]),0) AS [3_12],
	ISNULL(SUM(gtr.[3_13]),0) AS [3_13],
	ISNULL(SUM(gtr.[3_14]),0) AS [3_14],
	ISNULL(SUM(gtr.[3_15]),0) AS [3_15],
	ISNULL(SUM(gtr.[3_16]),0) AS [3_16],
	ISNULL(SUM(gtr.[3_17]),0) AS [3_17],
	ISNULL(SUM(gtr.[3_18]),0) AS [3_18],
	ISNULL(SUM(gtr.[3_19]),0) AS [3_19],
	ISNULL(SUM(gtr.[3_20]),0) AS [3_20],

	ISNULL(SUM(gtr.[3_26]),0) AS [3_26],
	ISNULL(SUM(gtr.[3_27]),0) AS [3_27],

	(SELECT ISNULL(Sum(t.[3_29] * t.[3_29_cnt])/Sum(t.[3_29_cnt]) , 0)
From gzgu_temp_result t
where t.[3_29_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_29] ,
	(SELECT ISNULL(Sum(t.[3_30] * t.[3_30_cnt])/Sum(t.[3_30_cnt]) , 0)
From gzgu_temp_result t
where t.[3_30_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_30] ,
	(SELECT ISNULL(Sum(t.[3_31] * t.[3_31_cnt])/Sum(t.[3_31_cnt]) , 0)
From gzgu_temp_result t
where t.[3_31_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_31] ,
	(SELECT ISNULL(Sum(t.[3_32] * t.[3_32_cnt])/Sum(t.[3_32_cnt]) , 0)
From gzgu_temp_result t
where t.[3_32_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_32] ,
	(SELECT ISNULL(Sum(t.[3_33] * t.[3_33_cnt])/Sum(t.[3_33_cnt]) , 0)
From gzgu_temp_result t
where t.[3_33_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_33] ,
	(SELECT ISNULL(Sum(t.[3_34] * t.[3_34_cnt])/Sum(t.[3_34_cnt]) , 0)
From gzgu_temp_result t
where t.[3_34_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_34] ,
	(SELECT ISNULL(Sum(t.[3_36] * t.[3_36_cnt])/Sum(t.[3_36_cnt]) , 0)
From gzgu_temp_result t
where t.[3_36_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_36] ,
	(SELECT ISNULL(Sum(t.[3_37] * t.[3_37_cnt])/Sum(t.[3_37_cnt]) , 0)
From gzgu_temp_result t
where t.[3_37_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_37] ,
	(SELECT ISNULL(Sum(t.[3_38] * t.[3_38_cnt])/Sum(t.[3_38_cnt]) , 0)
From gzgu_temp_result t
where t.[3_38_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_38] ,
	(SELECT ISNULL(Sum(t.[3_39] * t.[3_39_cnt])/Sum(t.[3_39_cnt]) , 0)
From gzgu_temp_result t
where t.[3_39_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_39] ,
	(SELECT ISNULL(Sum(t.[3_40] * t.[3_40_cnt])/Sum(t.[3_40_cnt]) , 0)
From gzgu_temp_result t
where t.[3_40_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_40] ,
	(SELECT ISNULL(Sum(t.[3_41] * t.[3_41_cnt])/Sum(t.[3_41_cnt]) , 0)
From gzgu_temp_result t
where t.[3_41_cnt] != 0 AND 
t.DirectionID = gtr.DirectionID AND t.EducationFormId = gtr.EducationFormId AND t.IdLevelBudget = gtr.IdLevelBudget
and t.InstitutionID = i.InstitutionID
) as [3_41] 
FROM Institution i (NOLOCK)
inner join gzgu_temp_result gtr (NOLOCK)  on i.InstitutionID = gtr.InstitutionID
inner join esrp_prod.dbo.Organization2010 org (NOLOCK) on org.Id = i.EsrpOrgID
inner join CompetitiveGroup cg (NOLOCK) on cg.CompetitiveGroupID = gtr.CompetitiveGroupID
inner join Campaign c (nolock) on c.CampaignID = cg.CampaignID
left join Direction d (NOLOCK)  on d.DirectionID = gtr.directionID
LEFT JOIN AdmissionItemType ait (NOLOCK) ON gtr.EducationFormId = ait.ItemTypeID
--LEFT JOIN LevelBudget lb (NOLOCK) ON gtr.IdLevelBudget=lb.IdLevelBudget
--where ((gtr.EducationSourceId !=15 and gtr.IdlevelBudget !=0) or (gtr.EducationSourceId =15 and gtr.IdlevelBudget =0))
where gtr.IdLevelBudget != 0
group by i.InstitutionID
,i.EsrpOrgID
,d.NewCode
,gtr.EducationFormId
,gtr.IdLevelBudget
,gtr.DirectionID 
,d.Name
,org.INN
,org.KPP
,org.FullName
,ait.Name
--,LB.BudgetName
--gtr.[1_4],
--	gtr.[1_5],
--	gtr.[1_6],
--	gtr.[1_7],
--	gtr.[1_8],
--gtr.[1_9],
--	gtr.[1_8],
--	gtr.[1_10],
--	gtr.[1_11],
--	gtr.[1_12],
--	gtr.[1_13],
--	gtr.[1_14],
--	gtr.[1_15],
--	gtr.[1_16],
--	gtr.[1_17],
--	gtr.[1_18],

--	gtr.[1_19],
--	gtr.[1_20],
--	--[1_21],
--	--[1_22],
--	--[1_23],
--	gtr.[1_24],
--	gtr.[1_25],
--	gtr.[1_26],
--	gtr.[1_27],
--	gtr.[1_28],
--	gtr.[1_29],
--	gtr.[1_30],
--	gtr.[1_31],
--	gtr.[1_32],
--	gtr.[1_33],
--	gtr.[1_34],
--	gtr.[1_35],
--	gtr.[1_36],
--	gtr.[1_37],
--	gtr.[1_38],
--	gtr.[1_39],
--	gtr.[1_40],
--	gtr.[1_41],
--	gtr.[1_42],

--	gtr.[2_1], 
--	gtr.[2_1_cnt], 
--	gtr.[2_2], 
--	gtr.[2_2_cnt], 
--	gtr.[2_3], 
--	gtr.[2_3_cnt], 
--	gtr.[2_4], 
--	gtr.[2_4_cnt], 
--	gtr.[2_5], 
--	gtr.[2_5_cnt], 
--	gtr.[2_6], 
--	gtr.[2_6_cnt], 
--	gtr.[2_7], 
--	gtr.[2_7_cnt], 
--	gtr.[2_8], 
--	gtr.[2_8_cnt], 

--	gtr.[2_9], 
--	gtr.[2_10], 
--	gtr.[2_11], 
--	gtr.[2_12], 
--	gtr.[2_13], 
--	gtr.[2_14], 

--	gtr.[2_15],
--	gtr.[2_16],
--	gtr.[2_17],
--	gtr.[2_18],
--	gtr.[2_19],
--	gtr.[2_20],

--	gtr.[2_21],
--	gtr.[2_22],
--	gtr.[2_23],
--	gtr.[2_24],
--	gtr.[2_25],
--	gtr.[2_26],
--	gtr.[3_6] ,
--	gtr.[3_12] ,
--	gtr.[3_13] ,
--c.CampaignID,c.CampaignTypeID,cg.EducationFormId,cg.EducationSourceId,cg.EducationLevelID
END
