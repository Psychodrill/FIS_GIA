--DECLARE @competitiveGroups Identifiers; 
--INSERT INTO @competitiveGroups (id) VALUES (245740);
--INSERT INTO @competitiveGroups (id) VALUES (245738);
--INSERT INTO @competitiveGroups (id) VALUES (245739);


--DECLARE @campaignID INT = 13564
--DECLARE @institutionID int = 6346;

--Кампания не принадлежит этому институту
IF (@campaignID NOT IN (SELECT CampaignId FROM Campaign WHERE InstitutionID = @institutionID))
RETURN


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
WHERE cg.CampaignID = @CampaignID AND IsAdditional = 0 AND (cg.EducationSourceID in (16,20) OR cg.EducationSourceID = 14)

--ОТЛАДКА
--SELECT * FROM @VolumeTransfer

--готовимся к апдейту 
DECLARE @VolumeUpdate TABLE (  
   DirectionID int NOT null ,
   CompetitiveGroupID int NOT null ,
   EducationFormId INT not null ,
   EducationSourceId INT not null ,
   freeTotal int null ,
   freeFed int null ,
   freeReg int null ,	
   freeMun int null ,	
   budget_CompetitiveGroupID INT not null 
)

INSERT INTO @VolumeUpdate
SELECT 
       benefit.DirectionID ,
       benefit.CompetitiveGroupID ,
       benefit.EducationFormId,
       benefit.EducationSourceId,
       benefit.CGTotal - benefit.OrderTotal AS freeTotal,
       benefit.CGFed - benefit.OrderFed AS freeFed,
       benefit.CGReg - benefit.OrderReg AS freeReg,
       benefit.CGMun - benefit.OrderMun AS freeMun,
       budget.CompetitiveGroupID AS budget_CompetitiveGroupID
FROM @VolumeTransfer benefit 
LEFT JOIN @VolumeTransfer budget 
ON benefit.DirectionID = budget.DirectionID
AND benefit.CampaignID = budget.CampaignID
AND benefit.EducationFormId = budget.EducationFormId
AND benefit.EducationLevelID = budget.EducationLevelID
AND benefit.IdLevelBudget = budget.IdLevelBudget
AND benefit.CGPSum = budget.CGPSum
AND (budget.EducationSourceID = 14 OR budget.CompetitiveGroupID IS NULL)
WHERE benefit.EducationSourceID in (16,20) AND budget.CompetitiveGroupID IS NOT NULL AND benefit.CompetitiveGroupID in (SELECT id FROM @competitiveGroups)
--AND benefit.CompetitiveGroupID = 245731

--отладка
--SELECT * FROM @VolumeUpdate

----для льготного объема
--SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId, EducationSourceId, 
--SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
--GROUP BY DirectionID, EducationFormId, EducationSourceId

----для бюджетного объема

	
--SELECT t.AdmissionVolumeID, MAX(NumberBudgetO), MAX(NumberBudgetOZ), MAX(NumberBudgetZ) FROM (
--SELECT 
--	av.AdmissionVolumeID,
--	vu.EducationFormId,
--	NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN av.NumberBudgetO + vu.freeTotal ELSE NumberBudgetO END)
--	,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN av.NumberBudgetOZ + vu.freeTotal ELSE NumberBudgetOZ END)
--	,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN av.NumberBudgetZ + vu.freeTotal ELSE NumberBudgetZ END)
--FROM 
--	AdmissionVolume av
--		INNER JOIN 
--	(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId,
--	SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
--	GROUP BY DirectionID, EducationFormId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
--) t		
--GROUP BY t.AdmissionVolumeID

--для бюджетных конкурсов
--SELECT @CampaignId AS CampaignId, budget_CompetitiveGroupID AS budget_CompetitiveGroupID, EducationFormId, DirectionID,
--SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
--GROUP BY budget_CompetitiveGroupID, EducationFormId, DirectionID

--для целевых организаций => зачисленные по каждой 
--используется ниже
--SELECT 
--	cgti.CompetitiveGroupID, 
--	cgti.CompetitiveGroupTargetId, 
--	(cgti.NumberTargetO+cgti.NumberTargetZ+cgti.NumberTargetOZ) AS volume, 
--	COUNT(a_acgi.ApplicationId) AS inOrder
-- FROM 
--	CompetitiveGroupTargetItem cgti (NOLOCK) 
--	INNER JOIN @VolumeUpdate VU ON cgti.CompetitiveGroupID = VU.CompetitiveGroupID
--	LEFT JOIN ApplicationCompetitiveGroupItem a_acgi (NOLOCK)  ON a_acgi.CompetitiveGroupTargetId = cgti.CompetitiveGroupTargetID
--	AND vu.CompetitiveGroupID = a_acgi.CompetitiveGroupId AND a_acgi.OrderOfExceptionId IS NULL	AND a_acgi.OrderOfAdmissionID IS NOT NULL	
--    LEFT JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId AND a.StatusID = 8 
-- GROUP BY cgti.CompetitiveGroupID, cgti.CompetitiveGroupTargetId, cgti.NumberTargetO, cgti.NumberTargetZ, cgti.NumberTargetOZ


--------------------------------------------------
----------УБЕРАЕМ МЕСТА С КВОТЫ/ЦЕЛЕВОГО----------
--------------------------------------------------
--BEGIN TRAN

--AdmissionVolume
UPDATE AdmissionVolume
	--бюджет
	SET NumberTargetO = i.NumberTargetO 
	,NumberTargetOZ =  i.NumberTargetOZ 
	,NumberTargetZ =  i.NumberTargetZ 
	,NumberQuotaO = i.NumberQuotaO 
	,NumberQuotaOZ =  i.NumberQuotaOZ 
	,NumberQuotaZ =  i.NumberQuotaZ 
FROM (
	SELECT t.AdmissionVolumeID, MIN(NumberTargetO) AS NumberTargetO, MIN(NumberTargetOZ) AS NumberTargetOZ, MIN(NumberTargetZ) AS NumberTargetZ
			, MIN(NumberQuotaO) AS NumberQuotaO, MIN(NumberQuotaOZ) AS NumberQuotaOZ, MIN(NumberQuotaZ) AS NumberQuotaZ FROM (
	SELECT 
		av.AdmissionVolumeID,
		vu.EducationFormId
		,NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN NumberTargetO - vu.freeTotal ELSE NumberTargetO END)
		,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN NumberTargetOZ - vu.freeTotal ELSE NumberTargetOZ END)
		,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN NumberTargetZ - vu.freeTotal ELSE NumberTargetZ END)
		--квота
		,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN NumberQuotaO - vu.freeTotal ELSE NumberQuotaO END)
		,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN NumberQuotaOZ - vu.freeTotal ELSE NumberQuotaOZ END)
		,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN NumberQuotaZ - vu.freeTotal ELSE NumberQuotaZ END)
	FROM 
		AdmissionVolume av
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId, EducationSourceId, 
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId, EducationSourceId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
	) t		
	GROUP BY t.AdmissionVolumeID
) i WHERE i.AdmissionVolumeID = AdmissionVolume.AdmissionVolumeID

--DistributedAdmissionVolume
UPDATE DistributedAdmissionVolume
	SET NumberTargetO = i.NumberTargetO 
	,NumberTargetOZ =  i.NumberTargetOZ 
	,NumberTargetZ =  i.NumberTargetZ 
	,NumberQuotaO = i.NumberQuotaO 
	,NumberQuotaOZ =  i.NumberQuotaOZ 
	,NumberQuotaZ =  i.NumberQuotaZ 
FROM (
	SELECT t.DistributedAdmissionVolumeID, MIN(NumberTargetO) AS NumberTargetO, MIN(NumberTargetOZ) AS NumberTargetOZ, MIN(NumberTargetZ) AS NumberTargetZ
			, MIN(NumberQuotaO) AS NumberQuotaO, MIN(NumberQuotaOZ) AS NumberQuotaOZ, MIN(NumberQuotaZ) AS NumberQuotaZ FROM (
	SELECT 
		dav.DistributedAdmissionVolumeID,
		vu.EducationFormId,
		NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN dav.NumberTargetO - vu.freeFed ELSE dav.NumberTargetO END)
		,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN dav.NumberTargetOZ - vu.freeFed ELSE dav.NumberTargetOZ END)
		,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN dav.NumberTargetZ - vu.freeFed ELSE dav.NumberTargetZ END)
		--квота
		,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN dav.NumberQuotaO - vu.freeFed ELSE dav.NumberQuotaO END)
		,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN dav.NumberQuotaOZ - vu.freeFed ELSE dav.NumberQuotaOZ END)
		,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN dav.NumberQuotaZ - vu.freeTotal ELSE dav.NumberQuotaZ END)
	FROM 
		DistributedAdmissionVolume dav
		INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId, EducationSourceId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId, EducationSourceId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
		WHERE dav.IdLevelBudget = 1
	) t		
	GROUP BY t.DistributedAdmissionVolumeID
) i WHERE i.DistributedAdmissionVolumeID = DistributedAdmissionVolume.DistributedAdmissionVolumeID AND DistributedAdmissionVolume.IdLevelBudget = 1


UPDATE DistributedAdmissionVolume
	SET NumberTargetO = i.NumberTargetO 
	,NumberTargetOZ =  i.NumberTargetOZ 
	,NumberTargetZ =  i.NumberTargetZ 
	,NumberQuotaO = i.NumberQuotaO 
	,NumberQuotaOZ =  i.NumberQuotaOZ 
	,NumberQuotaZ =  i.NumberQuotaZ 
FROM (
	SELECT t.DistributedAdmissionVolumeID, MIN(NumberTargetO) AS NumberTargetO, MIN(NumberTargetOZ) AS NumberTargetOZ, MIN(NumberTargetZ) AS NumberTargetZ
			, MIN(NumberQuotaO) AS NumberQuotaO, MIN(NumberQuotaOZ) AS NumberQuotaOZ, MIN(NumberQuotaZ) AS NumberQuotaZ FROM (
	SELECT 
		dav.DistributedAdmissionVolumeID,
		vu.EducationFormId,
		NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN dav.NumberTargetO - vu.freeReg ELSE dav.NumberTargetO END)
		,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN dav.NumberTargetOZ - vu.freeReg ELSE dav.NumberTargetOZ END)
		,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN dav.NumberTargetZ - vu.freeReg ELSE dav.NumberTargetZ END)
		--квота
		,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN dav.NumberQuotaO - vu.freeReg ELSE dav.NumberQuotaO END)
		,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN dav.NumberQuotaOZ - vu.freeReg ELSE dav.NumberQuotaOZ END)
		,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN dav.NumberQuotaZ - vu.freeReg ELSE dav.NumberQuotaZ END)
	FROM 
		DistributedAdmissionVolume dav
		INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId, EducationSourceId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId, EducationSourceId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
		WHERE dav.IdLevelBudget = 2
	) t		
	GROUP BY t.DistributedAdmissionVolumeID
) i WHERE i.DistributedAdmissionVolumeID = DistributedAdmissionVolume.DistributedAdmissionVolumeID AND DistributedAdmissionVolume.IdLevelBudget = 2



UPDATE DistributedAdmissionVolume
	SET NumberTargetO = i.NumberTargetO 
	,NumberTargetOZ =  i.NumberTargetOZ 
	,NumberTargetZ =  i.NumberTargetZ 
	,NumberQuotaO = i.NumberQuotaO 
	,NumberQuotaOZ =  i.NumberQuotaOZ 
	,NumberQuotaZ =  i.NumberQuotaZ 
FROM (
	SELECT t.DistributedAdmissionVolumeID, MIN(NumberTargetO) AS NumberTargetO, MIN(NumberTargetOZ) AS NumberTargetOZ, MIN(NumberTargetZ) AS NumberTargetZ
			, MIN(NumberQuotaO) AS NumberQuotaO, MIN(NumberQuotaOZ) AS NumberQuotaOZ, MIN(NumberQuotaZ) AS NumberQuotaZ FROM (
	SELECT 
		dav.DistributedAdmissionVolumeID,
		vu.EducationFormId,
		NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN dav.NumberTargetO - vu.freeMun ELSE dav.NumberTargetO END)
		,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN dav.NumberTargetOZ - vu.freeMun ELSE dav.NumberTargetOZ END)
		,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN dav.NumberTargetZ - vu.freeMun ELSE dav.NumberTargetZ END)
		--квота
		,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN dav.NumberQuotaO - vu.freeMun ELSE dav.NumberQuotaO END)
		,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN dav.NumberQuotaOZ - vu.freeMun ELSE dav.NumberQuotaOZ END)
		,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN dav.NumberQuotaZ - vu.freeMun ELSE dav.NumberQuotaZ END)
	FROM 
		DistributedAdmissionVolume dav
		INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId, EducationSourceId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId, EducationSourceId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
		WHERE dav.IdLevelBudget = 3
	) t		
	GROUP BY t.DistributedAdmissionVolumeID
) i WHERE i.DistributedAdmissionVolumeID = DistributedAdmissionVolume.DistributedAdmissionVolumeID AND DistributedAdmissionVolume.IdLevelBudget = 3


UPDATE dav 
	SET NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN dav.NumberTargetO - vu.freeMun ELSE dav.NumberTargetO END)
	,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN dav.NumberTargetOZ - vu.freeMun ELSE dav.NumberTargetOZ END)
	,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN dav.NumberTargetZ - vu.freeMun ELSE dav.NumberTargetZ END)
	--квота
	,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN dav.NumberQuotaO - vu.freeMun ELSE dav.NumberQuotaO END)
	,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN dav.NumberQuotaOZ - vu.freeMun ELSE dav.NumberQuotaOZ END)
	,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN dav.NumberQuotaZ - vu.freeMun ELSE dav.NumberQuotaZ END)
FROM DistributedAdmissionVolume dav
INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
	INNER JOIN 
	(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId, EducationSourceId, 
	SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
	GROUP BY DirectionID, EducationFormId, EducationSourceId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
WHERE dav.IdLevelBudget = 3


--CompetitiveGroupItem
UPDATE  cgi
	SET NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN cgi.NumberTargetO - vu.freeFed ELSE cgi.NumberTargetO END)
	,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN cgi.NumberTargetOZ - vu.freeFed ELSE cgi.NumberTargetOZ END)
	,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN cgi.NumberTargetZ - vu.freeFed ELSE cgi.NumberTargetZ END)
	--квота
	,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN cgi.NumberQuotaO - vu.freeFed ELSE cgi.NumberQuotaO END)
	,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN cgi.NumberQuotaOZ - vu.freeFed ELSE cgi.NumberQuotaOZ END)
	,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN cgi.NumberQuotaZ - vu.freeFed ELSE cgi.NumberQuotaZ END)
FROM CompetitiveGroupItem cgi
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
INNER JOIN @VolumeUpdate vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.CompetitiveGroupID
WHERE cg.IdLevelBudget = 1

UPDATE  cgi
	SET NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN cgi.NumberTargetO - vu.freeReg ELSE cgi.NumberTargetO END)
	,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN cgi.NumberTargetOZ - vu.freeReg ELSE cgi.NumberTargetOZ END)
	,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN cgi.NumberTargetZ - vu.freeReg ELSE cgi.NumberTargetZ END)
	--квота
	,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN cgi.NumberQuotaO - vu.freeReg ELSE cgi.NumberQuotaO END)
	,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN cgi.NumberQuotaOZ - vu.freeReg ELSE cgi.NumberQuotaOZ END)
	,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN cgi.NumberQuotaZ - vu.freeReg ELSE cgi.NumberQuotaZ END)
FROM CompetitiveGroupItem cgi
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
INNER JOIN @VolumeUpdate vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.CompetitiveGroupID
WHERE cg.IdLevelBudget = 2

UPDATE  cgi
	SET NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN cgi.NumberTargetO - vu.freeMun ELSE cgi.NumberTargetO END)
	,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN cgi.NumberTargetOZ - vu.freeMun ELSE cgi.NumberTargetOZ END)
	,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN cgi.NumberTargetZ - vu.freeMun ELSE cgi.NumberTargetZ END)
	--квота
	,NumberQuotaO = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 11 THEN cgi.NumberQuotaO - vu.freeMun ELSE cgi.NumberQuotaO END)
	,NumberQuotaOZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 12 THEN cgi.NumberQuotaOZ - vu.freeMun ELSE cgi.NumberQuotaOZ END)
	,NumberQuotaZ = (CASE WHEN vu.EducationSourceId = 20 and vu.EducationFormId = 10 THEN cgi.NumberQuotaZ - vu.freeMun ELSE cgi.NumberQuotaZ END)
FROM CompetitiveGroupItem cgi
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
INNER JOIN @VolumeUpdate vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.CompetitiveGroupID
WHERE cg.IdLevelBudget = 3


--кусочек для целового
UPDATE  cgti
	SET NumberTargetO = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 11 THEN cgti.NumberTargetO - (tr.volume - tr.inOrder) ELSE cgti.NumberTargetO END)
	,NumberTargetOZ = (CASE WHEN vu.EducationSourceId = 16 and vu.EducationFormId = 12 THEN cgti.NumberTargetOZ - (tr.volume - tr.inOrder) ELSE cgti.NumberTargetOZ END)
	,NumberTargetZ = (CASE WHEN vu.EducationSourceId = 16 AND vu.EducationFormId = 10 THEN cgti.NumberTargetZ - (tr.volume - tr.inOrder) ELSE cgti.NumberTargetZ END)
FROM CompetitiveGroupTargetItem cgti
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgti.CompetitiveGroupID
INNER JOIN @VolumeUpdate vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.CompetitiveGroupID
INNER JOIN (SELECT 
	cgti.CompetitiveGroupID, 
	cgti.CompetitiveGroupTargetId, 
	(cgti.NumberTargetO+cgti.NumberTargetZ+cgti.NumberTargetOZ) AS volume, 
	COUNT(a_acgi.ApplicationId) AS inOrder
 FROM 
	CompetitiveGroupTargetItem cgti (NOLOCK) 
	INNER JOIN @VolumeUpdate VU ON cgti.CompetitiveGroupID = VU.CompetitiveGroupID
	LEFT JOIN ApplicationCompetitiveGroupItem a_acgi (NOLOCK)  ON a_acgi.CompetitiveGroupTargetId = cgti.CompetitiveGroupTargetID
	AND vu.CompetitiveGroupID = a_acgi.CompetitiveGroupId AND a_acgi.OrderOfExceptionId IS NULL	AND a_acgi.OrderOfAdmissionID IS NOT NULL	
    LEFT JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId AND a.StatusID = 8 
 GROUP BY cgti.CompetitiveGroupID, cgti.CompetitiveGroupTargetId, cgti.NumberTargetO, cgti.NumberTargetZ, cgti.NumberTargetOZ) AS tr
 ON tr.CompetitiveGroupID = cg.CompetitiveGroupID AND tr.CompetitiveGroupTargetID = cgti.CompetitiveGroupTargetID


--------------------------------------------------
------------ДОБАВЛЯЕМ МЕСТА НА БЮДЖЕТ-------------
--------------------------------------------------
--AdmissionVolume
UPDATE AdmissionVolume
	--бюджет
	SET NumberBudgetO = i.NumberBudgetO 
	,NumberBudgetOZ =  i.NumberBudgetOZ 
	,NumberBudgetZ =  i.NumberBudgetZ 
FROM (
	SELECT t.AdmissionVolumeID, MAX(NumberBudgetO) AS NumberBudgetO, MAX(NumberBudgetOZ) AS NumberBudgetOZ, MAX(NumberBudgetZ) AS NumberBudgetZ FROM (
	SELECT 
		av.AdmissionVolumeID,
		vu.EducationFormId,
		NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN av.NumberBudgetO + vu.freeTotal ELSE NumberBudgetO END)
		,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN av.NumberBudgetOZ + vu.freeTotal ELSE NumberBudgetOZ END)
		,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN av.NumberBudgetZ + vu.freeTotal ELSE NumberBudgetZ END)
	FROM 
		AdmissionVolume av
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
	) t		
	GROUP BY t.AdmissionVolumeID
) i WHERE i.AdmissionVolumeID = AdmissionVolume.AdmissionVolumeID


--DistributedAdmissionVolume
UPDATE DistributedAdmissionVolume
	SET NumberBudgetO = i.NumberBudgetO 
	,NumberBudgetOZ =  i.NumberBudgetOZ 
	,NumberBudgetZ =  i.NumberBudgetZ 
FROM (
	SELECT t.DistributedAdmissionVolumeID, MAX(NumberBudgetO) AS NumberBudgetO, MAX(NumberBudgetOZ) AS NumberBudgetOZ, MAX(NumberBudgetZ) AS NumberBudgetZ FROM (
	SELECT 
		dav.DistributedAdmissionVolumeID,
		vu.EducationFormId,
		NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN dav.NumberBudgetO + vu.freeFed ELSE dav.NumberBudgetO END)
		,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN dav.NumberBudgetOZ + vu.freeFed ELSE dav.NumberBudgetOZ END)
		,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN dav.NumberBudgetZ + vu.freeFed ELSE dav.NumberBudgetZ END)
	FROM 
		DistributedAdmissionVolume dav
		INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
		WHERE dav.IdLevelBudget = 1
	) t		
	GROUP BY t.DistributedAdmissionVolumeID
) i WHERE i.DistributedAdmissionVolumeID = DistributedAdmissionVolume.DistributedAdmissionVolumeID AND DistributedAdmissionVolume.IdLevelBudget = 1


UPDATE DistributedAdmissionVolume
	SET NumberBudgetO = i.NumberBudgetO 
	,NumberBudgetOZ =  i.NumberBudgetOZ 
	,NumberBudgetZ =  i.NumberBudgetZ 
FROM (
	SELECT t.DistributedAdmissionVolumeID, MAX(NumberBudgetO) AS NumberBudgetO, MAX(NumberBudgetOZ) AS NumberBudgetOZ, MAX(NumberBudgetZ) AS NumberBudgetZ FROM (
	SELECT 
		dav.DistributedAdmissionVolumeID,
		vu.EducationFormId,
		NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN dav.NumberBudgetO + vu.freeReg ELSE dav.NumberBudgetO END)
		,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN dav.NumberBudgetOZ + vu.freeReg ELSE dav.NumberBudgetOZ END)
		,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN dav.NumberBudgetZ + vu.freeReg ELSE dav.NumberBudgetZ END)
	FROM 
		DistributedAdmissionVolume dav
		INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
		WHERE dav.IdLevelBudget = 2
	) t		
	GROUP BY t.DistributedAdmissionVolumeID
) i WHERE i.DistributedAdmissionVolumeID = DistributedAdmissionVolume.DistributedAdmissionVolumeID AND DistributedAdmissionVolume.IdLevelBudget = 2



UPDATE DistributedAdmissionVolume
	SET NumberBudgetO = i.NumberBudgetO 
	,NumberBudgetOZ =  i.NumberBudgetOZ 
	,NumberBudgetZ =  i.NumberBudgetZ 
FROM (
	SELECT t.DistributedAdmissionVolumeID, MAX(NumberBudgetO) AS NumberBudgetO, MAX(NumberBudgetOZ) AS NumberBudgetOZ, MAX(NumberBudgetZ) AS NumberBudgetZ FROM (
	SELECT 
		dav.DistributedAdmissionVolumeID,
		vu.EducationFormId,
		NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN dav.NumberBudgetO + vu.freeMun ELSE dav.NumberBudgetO END)
		,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN dav.NumberBudgetOZ + vu.freeMun ELSE dav.NumberBudgetOZ END)
		,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN dav.NumberBudgetZ + vu.freeMun ELSE dav.NumberBudgetZ END)
	FROM 
		DistributedAdmissionVolume dav
		INNER JOIN AdmissionVolume av ON av.AdmissionVolumeID = dav.AdmissionVolumeID
			INNER JOIN 
		(SELECT @CampaignId AS CampaignId, DirectionID, EducationFormId,
		SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
		GROUP BY DirectionID, EducationFormId) vu ON vu.DirectionID = av.DirectionID AND av.CampaignID = @CampaignID
		WHERE dav.IdLevelBudget = 3
	) t		
	GROUP BY t.DistributedAdmissionVolumeID
) i WHERE i.DistributedAdmissionVolumeID = DistributedAdmissionVolume.DistributedAdmissionVolumeID AND DistributedAdmissionVolume.IdLevelBudget = 3


--CompetitiveGroupItem
UPDATE  cgi
	SET NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN cgi.NumberBudgetO + vu.freeFed ELSE cgi.NumberBudgetO END)
	,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN cgi.NumberBudgetOZ + vu.freeFed ELSE cgi.NumberBudgetOZ END)
	,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN cgi.NumberBudgetZ + vu.freeFed ELSE cgi.NumberBudgetZ END)
FROM CompetitiveGroupItem cgi
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
INNER JOIN (
SELECT @CampaignId AS CampaignId, budget_CompetitiveGroupID AS budget_CompetitiveGroupID, EducationFormId, DirectionID AS DirectionID,
SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
GROUP BY budget_CompetitiveGroupID, EducationFormId, DirectionID
) vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.budget_CompetitiveGroupID
WHERE cg.IdLevelBudget = 1

UPDATE  cgi
	SET NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN cgi.NumberBudgetO + vu.freeReg ELSE cgi.NumberBudgetO END)
	,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN cgi.NumberBudgetOZ + vu.freeReg ELSE cgi.NumberBudgetOZ END)
	,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN cgi.NumberBudgetZ + vu.freeReg ELSE cgi.NumberBudgetZ END)
FROM CompetitiveGroupItem cgi
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
INNER JOIN (
SELECT @CampaignId AS CampaignId, budget_CompetitiveGroupID AS budget_CompetitiveGroupID, EducationFormId, DirectionID AS DirectionID,
SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
GROUP BY budget_CompetitiveGroupID, EducationFormId, DirectionID
) vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.budget_CompetitiveGroupID
WHERE cg.IdLevelBudget = 2

UPDATE  cgi
	SET NumberBudgetO = (CASE WHEN vu.EducationFormId = 11 THEN cgi.NumberBudgetO + vu.freeMun ELSE cgi.NumberBudgetO END)
	,NumberBudgetOZ = (CASE WHEN vu.EducationFormId = 12 THEN cgi.NumberBudgetOZ + vu.freeMun ELSE cgi.NumberBudgetOZ END)
	,NumberBudgetZ = (CASE WHEN vu.EducationFormId = 10 THEN cgi.NumberBudgetZ + vu.freeMun ELSE cgi.NumberBudgetZ END)
FROM CompetitiveGroupItem cgi
INNER JOIN CompetitiveGroup cg ON cg.CompetitiveGroupID = cgi.CompetitiveGroupID
INNER JOIN (
SELECT @CampaignId AS CampaignId, budget_CompetitiveGroupID AS budget_CompetitiveGroupID, EducationFormId, DirectionID AS DirectionID,
SUM(freeTotal) AS freeTotal, SUM(freeFed) AS freeFed, SUM(freeReg) AS freeReg, SUM(freeMun) AS freeMun FROM @VolumeUpdate
GROUP BY budget_CompetitiveGroupID, EducationFormId, DirectionID
) vu ON vu.DirectionID = cg.DirectionID AND cg.CampaignID = @CampaignID AND cg.CompetitiveGroupID = vu.budget_CompetitiveGroupID
WHERE cg.IdLevelBudget = 3

--ROLLBACK TRAN
