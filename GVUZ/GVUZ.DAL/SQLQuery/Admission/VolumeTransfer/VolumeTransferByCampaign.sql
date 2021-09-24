
--DECLARE @campaignID INT = 13564
--DECLARE @institutionID int = 6346;


DECLARE @VolumeTransfer TABLE (   
   InstitutionID int NOT null ,
   DirectionID int NOT null ,
   DirectionName varchar(500) null,
   DirectionCode varchar(50) null,
   CompetitiveGroupID int NOT null ,
   Name varchar(200) null,
   UID varchar(200) null,
   CampaignID int NOT null ,
   IsAdditional bit null,
   CGPSum int NOT null ,
   EducationFormId smallint NOT null ,
   EducationForm VARCHAR(100) ,
   EducationSourceId smallint NOT null ,
   EducationSource VARCHAR(100),
   EducationLevelID smallint NOT null ,
   EducationLevel VARCHAR(100),
   IdLevelBudget smallint null ,
   BudgetName VARCHAR(100),
   CGTotal int null ,
   CGFed int null ,
   CGReg int null ,	
   CGMun int null ,	
   OrderTotal int null ,
   OrderFed int null ,
   OrderReg int null ,
   OrderMun int null 
)
--собираем форму для переброса мест
--1. найдем все льготные конкурсы, бюджетные конкурсы, их кцп и зачисленных
INSERT INTO @VolumeTransfer
SELECT 
       cg.InstitutionID ,
       cg.DirectionID ,
       d.Name AS DirectionName ,
       d.NewCode AS DirectionCode ,
       cg.CompetitiveGroupID ,
       cg.Name ,
       cg.UID ,
       cg.CampaignID ,
       cg.IsAdditional ,
       (SELECT  ISNULL(SUM(cgp.InstitutionProgramID), 0) FROM CompetitiveGroupTOProgram cgp (NOLOCK) where cgp.CompetitiveGroupID = cg.CompetitiveGroupID) AS CGPSum,
       cg.EducationFormId ,
       form.Name,
       cg.EducationSourceId ,
       source.Name,
       cg.EducationLevelID ,
		level.Name,
       cg.IdLevelBudget,   
       lb.BudgetName  , 
       ISNULL(KCP.CGTotal, 0),
       ISNULL(KCP.CGFed, 0),
       ISNULL(KCP.CGReg, 0),
       ISNULL(KCP.CGMun, 0),
       ISNULL(IN_ORDER.OrderTotal, 0),
       ISNULL(IN_ORDER.OrderFed, 0),
       ISNULL(IN_ORDER.OrderReg, 0),	
       ISNULL(IN_ORDER.OrderMun, 0)
FROM	
CompetitiveGroup cg (NOLOCK) 
INNER JOIN Direction d (NOLOCK) ON cg.DirectionID = d.DirectionID
INNER JOIN AdmissionItemType form (NOLOCK) ON form.ItemTypeID = cg.EducationFormId
INNER JOIN AdmissionItemType source (NOLOCK) ON source.ItemTypeID = cg.EducationSourceId
INNER JOIN AdmissionItemType level (NOLOCK) ON level.ItemTypeID = cg.EducationLevelID
LEFT JOIN LevelBudget lb ON lb.IdLevelBudget = cg.IdLevelBudget
--считаем кцп
INNER JOIN
(
	SELECT 
	cg.CompetitiveGroupID AS CompetitiveGroupID,
	CASE
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=11) THEN cgi.NumberBudgetO
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=12) THEN cgi.NumberBudgetOZ
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=10) THEN cgi.NumberBudgetZ
		     
		WHEN (cg.EducationSourceId=15 AND cg.EducationFormId=11) THEN cgi.NumberPaidO
		WHEN (cg.EducationSourceId=15 AND cg.EducationFormId=12) THEN cgi.NumberPaidOZ
		WHEN (cg.EducationSourceId=15 AND cg.EducationFormId=10) THEN cgi.NumberPaidZ
		     
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=11) THEN 
			case when isnull(cgti.NumberTargetO, 0) > 0 then cgti.NumberTargetO  else cgi.NumberTargetO end
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=12) THEN 
			case when isnull(cgti.NumberTargetOZ, 0) > 0 then cgti.NumberTargetOZ  else cgi.NumberTargetOZ end
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=10) THEN 
			case when isnull(cgti.NumberTargetZ, 0) > 0 then cgti.NumberTargetZ  else cgi.NumberTargetZ end
		     
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=11) THEN cgi.NumberQuotaO
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=12) THEN cgi.NumberQuotaOZ
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=10) THEN cgi.NumberQuotaZ
	END AS CGTotal --Объем приема в рамках КГ
	                     			
	
	,CASE
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=11 and cg.IdLevelBudget=1) THEN cgi.NumberBudgetO
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=12 and cg.IdLevelBudget=1) THEN cgi.NumberBudgetOZ
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=10 and cg.IdLevelBudget=1) THEN cgi.NumberBudgetZ
		     
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=11 and cg.IdLevelBudget=1) THEN cgi.NumberTargetO
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=12 and cg.IdLevelBudget=1) THEN cgi.NumberTargetOZ
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=10 and cg.IdLevelBudget=1) THEN cgi.NumberTargetZ
		     
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=11 and cg.IdLevelBudget=1) THEN cgi.NumberQuotaO
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=12 and cg.IdLevelBudget=1) THEN cgi.NumberQuotaOZ
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=10 and cg.IdLevelBudget=1) THEN cgi.NumberQuotaZ 
	END 
	AS CGFed --Распределенный объем приема в рамках КГ по федеральному уровню бюджета
		
	,CASE
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=11 and cg.IdLevelBudget=2) THEN cgi.NumberBudgetO
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=12 and cg.IdLevelBudget=2) THEN cgi.NumberBudgetOZ
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=10 and cg.IdLevelBudget=2) THEN cgi.NumberBudgetZ
		     
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=11 and cg.IdLevelBudget=2) THEN cgi.NumberTargetO
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=12 and cg.IdLevelBudget=2) THEN cgi.NumberTargetOZ
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=10 and cg.IdLevelBudget=2) THEN cgi.NumberTargetZ
		     
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=11 and cg.IdLevelBudget=2) THEN cgi.NumberQuotaO
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=12 and cg.IdLevelBudget=2) THEN cgi.NumberQuotaOZ
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=10 and cg.IdLevelBudget=2) THEN cgi.NumberQuotaZ 
	END 
	 AS CGReg --Распределенный объем приема в рамках кг по региональному уровню бюджета
	
	,CASE
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=11 and cg.IdLevelBudget=3) THEN cgi.NumberBudgetO
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=12 and cg.IdLevelBudget=3) THEN cgi.NumberBudgetOZ
		WHEN (cg.EducationSourceId=14 AND cg.EducationFormId=10 and cg.IdLevelBudget=3) THEN cgi.NumberBudgetZ
		     
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=11 and cg.IdLevelBudget=3) THEN cgi.NumberTargetO
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=12 and cg.IdLevelBudget=3) THEN cgi.NumberTargetOZ
		WHEN (cg.EducationSourceId=16 AND cg.EducationFormId=10 and cg.IdLevelBudget=3) THEN cgi.NumberTargetZ
		     
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=11 and cg.IdLevelBudget=3) THEN cgi.NumberQuotaO
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=12 and cg.IdLevelBudget=3) THEN cgi.NumberQuotaOZ
		WHEN (cg.EducationSourceId=20 AND cg.EducationFormId=10 and cg.IdLevelBudget=3) THEN cgi.NumberQuotaZ 
	 END 
	 AS CGMun--Распределенный объем приема по направлению подготовки / специальности  по муниципальному уровню бюджета
	

	FROM CompetitiveGroup AS cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem AS cgi (NOLOCK)ON cgi.CompetitiveGroupID = cg.CompetitiveGroupId
		LEFT JOIN AdmissionVolume AS av (NOLOCK)ON av.CampaignID = cg.CampaignID 	
			AND cg.EducationLevelID = av.AdmissionItemTypeID		
			AND cg.DirectionID = av.DirectionID  
		LEFT JOIN CompetitiveGroupTargetItem AS cgti (NOLOCK)ON cgti.CompetitiveGroupID = cg.CompetitiveGroupId 
			AND cgti.CompetitiveGroupTargetID = cg.CompetitiveGroupID 
) AS KCP ON KCP.CompetitiveGroupID = cg.CompetitiveGroupID


--считаем зачисленных
INNER JOIN
(	
	SELECT cg1.CompetitiveGroupId
	,(SELECT 
		COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
        INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL	
		AND a_cg.CompetitiveGroupID = cg1.CompetitiveGroupID
	) AS OrderTotal,

	(SELECT 
		COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
        INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL	
		AND a_cg.CompetitiveGroupID = cg1.CompetitiveGroupID
		AND a_acgi.OrderIdLevelBudget = 1	
	) AS OrderFed,	

	(SELECT 
		COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL	
		AND a_cg.CompetitiveGroupID = cg1.CompetitiveGroupID
		AND a_acgi.OrderIdLevelBudget = 2		
	) AS OrderReg,

	(SELECT 
		COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL	
		AND a_cg.CompetitiveGroupID = cg1.CompetitiveGroupID
		AND a_acgi.OrderIdLevelBudget = 3	
	) AS OrderMun
	FROM 
	CompetitiveGroup cg1 (NOLOCK) 
) AS IN_ORDER ON IN_ORDER.CompetitiveGroupID = CG.CompetitiveGroupID

WHERE cg.CampaignID = @campaignID AND IsAdditional = 0 AND (cg.EducationSourceID in (16,20) OR cg.EducationSourceID = 14)


--SELECT * FROM @VolumeTransfer


--пытаемся найти соответствие по конкурсам и сразу группируем по направлению
SELECT t2.*,  
		CASE WHEN t2.EducationFormId = 10 THEN ISNULL(av.NumberBudgetZ,0) WHEN t2.EducationFormId = 11 THEN ISNULL(av.NumberBudgetO,0) WHEN t2.EducationFormId = 12 THEN ISNULL(av.NumberBudgetOZ,0) END AS  BudgetTotal,
		CASE WHEN t2.EducationFormId = 10 THEN ISNULL(fed.NumberBudgetZ,0) WHEN t2.EducationFormId = 11 THEN ISNULL(fed.NumberBudgetO,0) WHEN t2.EducationFormId = 12 THEN ISNULL(fed.NumberBudgetOZ,0) END AS  BudgetFed,
	    CASE WHEN t2.EducationFormId = 10 THEN ISNULL(reg.NumberBudgetZ,0) WHEN t2.EducationFormId = 11 THEN ISNULL(reg.NumberBudgetO,0) WHEN t2.EducationFormId = 12 THEN ISNULL(reg.NumberBudgetOZ,0) END AS  BudgetReg,
		CASE WHEN t2.EducationFormId = 10 THEN ISNULL(mun.NumberBudgetZ,0) WHEN t2.EducationFormId = 11 THEN ISNULL(mun.NumberBudgetO,0) WHEN t2.EducationFormId = 12 THEN ISNULL(mun.NumberBudgetOZ,0) END AS  BudgetMun


 FROM
(
SELECT 
	t.DirectionID,
	t.DirectionName,
	t.DirectionCode,
	t.DirectionName + ' ' + t.DirectionCode AS Direction,
	t.EducationFormId,
	t.EducationForm,
	SUM (CASE WHEN EducationSourceId = 16 THEN CGTotal ELSE 0 END) AS CGTargetTotal,
	SUM (CASE WHEN EducationSourceId = 16 THEN CGFed ELSE 0 END) AS CGTargetFed,
	SUM (CASE WHEN EducationSourceId = 16 THEN CGReg ELSE 0 END) AS CGTargetReg,
	SUM (CASE WHEN EducationSourceId = 16 THEN CGMun ELSE 0 END) AS CGTargetMun,
	SUM (CASE WHEN EducationSourceId = 16 THEN OrderTotal ELSE 0 END) AS OrderTargetTotal,
	SUM (CASE WHEN EducationSourceId = 16 THEN OrderFed ELSE 0 END) AS OrderTargetFed,
	SUM (CASE WHEN EducationSourceId = 16 THEN OrderReg ELSE 0 END) AS OrderTargetReg,
	SUM (CASE WHEN EducationSourceId = 16 THEN OrderMun ELSE 0 END) AS OrderTargetMun,
				
	SUM (CASE WHEN EducationSourceId = 20 THEN CGTotal ELSE 0 END) AS CGQuotaTotal,
	SUM (CASE WHEN EducationSourceId = 20 THEN CGFed ELSE 0 END) AS CGQuotaFed,
	SUM (CASE WHEN EducationSourceId = 20 THEN CGReg ELSE 0 END) AS CGQuotaReg,
	SUM (CASE WHEN EducationSourceId = 20 THEN CGMun ELSE 0 END) AS CGQuotaMun,
	SUM (CASE WHEN EducationSourceId = 20 THEN OrderTotal ELSE 0 END) AS OrderQuotaTotal,
	SUM (CASE WHEN EducationSourceId = 20 THEN OrderFed ELSE 0 END) AS OrderQuotaFed,
	SUM (CASE WHEN EducationSourceId = 20 THEN OrderReg ELSE 0 END) AS OrderQuotaReg,
	SUM (CASE WHEN EducationSourceId = 20 THEN OrderMun ELSE 0 END) AS OrderQuotaMun
	
	--ISNULL (MAX (t.budget_CGFed), 0) AS BudgetFed,
	--ISNULL (MAX (t.budget_CGReg), 0) AS BudgetReg,
	--ISNULL (MAX (t.budget_CGMun), 0) AS BudgetMun,
 FROM
(
SELECT 
	   benefit.InstitutionID ,
       benefit.DirectionID ,
       benefit.DirectionName ,
       benefit.DirectionCode ,
       benefit.CompetitiveGroupID ,
       benefit.Name ,
       benefit.UID ,
       benefit.CampaignID ,
       benefit.IsAdditional ,
       benefit.CGPSum ,
       benefit.EducationFormId ,
       benefit.EducationForm,
       benefit.EducationSourceId ,
       benefit.EducationSource,
       benefit.EducationLevelID ,
       benefit.EducationLevel,
       benefit.IdLevelBudget ,
       benefit.CGTotal ,
       benefit.CGFed ,
       benefit.CGReg ,
       benefit.CGMun ,
       benefit.OrderTotal ,
       benefit.OrderFed ,
       benefit.OrderReg ,
       benefit.OrderMun 
       
       --budget.CGTotal AS  budget_CGTotal,
       --budget.CGFed AS  budget_CGFed,
       --budget.CGReg AS  budget_CGReg,
       --budget.CGMun AS  budget_CGMun,
       --budget.OrderTotal AS  budget_OrderTotal,
       --budget.OrderFed AS  budget_OrderFed,
       --budget.OrderReg AS  budget_OrderReg,
       --budget.OrderMun AS  budget_OrderMun
FROM @VolumeTransfer benefit 
LEFT JOIN @VolumeTransfer budget 
ON benefit.DirectionID = budget.DirectionID
AND benefit.CampaignID = budget.CampaignID
AND benefit.EducationFormId = budget.EducationFormId
AND benefit.EducationLevelID = budget.EducationLevelID
AND benefit.IdLevelBudget = budget.IdLevelBudget
AND benefit.CGPSum = budget.CGPSum
AND (budget.EducationSourceID = 14 OR budget.CompetitiveGroupID IS NULL)
WHERE benefit.EducationSourceID in (16,20) AND budget.CompetitiveGroupID IS NOT NULL
)  t

GROUP BY t.DirectionID, t.DirectionName, t.DirectionCode, t.EducationFormId, t.EducationForm
) t2
INNER JOIN AdmissionVolume av ON av.DirectionID = t2.DirectionID AND av.CampaignID  = @campaignID
LEFT JOIN DistributedAdmissionVolume fed ON fed.AdmissionVolumeID = av.AdmissionVolumeID AND fed.IdLevelBudget = 1
LEFT JOIN DistributedAdmissionVolume reg ON reg.AdmissionVolumeID = av.AdmissionVolumeID AND reg.IdLevelBudget = 2
LEFT JOIN DistributedAdmissionVolume mun ON mun.AdmissionVolumeID = av.AdmissionVolumeID AND mun.IdLevelBudget = 3
--LEFT JOIN DistributedAdmissionVolume dav ON dav.AdmissionVolumeID = av.AdmissionVolumeID

--просто соответствие по конкурсам для детализации
SELECT 
       benefit.DirectionID ,
       benefit.DirectionName ,
       benefit.DirectionCode ,
       benefit.DirectionName + ' ' + benefit.DirectionCode AS Direction,
       benefit.CompetitiveGroupID ,
       benefit.Name ,

       benefit.CGPSum ,
       benefit.EducationFormId ,
       benefit.EducationForm,
       benefit.EducationSourceId ,
       benefit.EducationSource,
       benefit.EducationLevelID ,
       benefit.EducationLevel,
       benefit.IdLevelBudget ,
       benefit.BudgetName,
       benefit.CGTotal ,
       benefit.CGFed ,
       benefit.CGReg ,
       benefit.CGMun ,
       benefit.OrderTotal ,
       benefit.OrderFed ,
       benefit.OrderReg ,
       benefit.OrderMun ,
       
       budget.CompetitiveGroupID AS budget_CompetitiveGroupID,
       budget.Name AS budget_Name,
       ISNULL(budget.CGTotal, 0) AS  budget_CGTotal,
       ISNULL(budget.CGFed, 0) AS  budget_CGFed,
       ISNULL(budget.CGReg, 0) AS  budget_CGReg,
       ISNULL(budget.CGMun, 0) AS  budget_CGMun,
       ISNULL(budget.OrderTotal, 0) AS  budget_OrderTotal,
       ISNULL(budget.OrderFed, 0) AS  budget_OrderFed,
       ISNULL(budget.OrderReg, 0) AS  budget_OrderReg,
       ISNULL(budget.OrderMun, 0) AS  budget_OrderMun
FROM @VolumeTransfer benefit 
LEFT JOIN @VolumeTransfer budget 
ON benefit.DirectionID = budget.DirectionID
AND benefit.CampaignID = budget.CampaignID
AND benefit.EducationFormId = budget.EducationFormId
AND benefit.EducationLevelID = budget.EducationLevelID
AND benefit.IdLevelBudget = budget.IdLevelBudget
AND benefit.CGPSum = budget.CGPSum
AND (budget.EducationSourceID = 14 OR budget.CompetitiveGroupID IS NULL)
WHERE benefit.EducationSourceID in (16,20) AND budget.CompetitiveGroupID IS NOT NULL

--ОПшки для наших конкурсов
SELECT 
	cp.CompetitiveGroupID
	,cp.InstitutionProgramID
	,ip.Name
	,ip.Code
 FROM
CompetitiveGroupToProgram cp (NOLOCK)
INNER JOIN @VolumeTransfer t ON t.CompetitiveGroupID = cp.CompetitiveGroupID
INNER JOIN InstitutionProgram ip ON cp.InstitutionProgramID = ip.InstitutionProgramID
