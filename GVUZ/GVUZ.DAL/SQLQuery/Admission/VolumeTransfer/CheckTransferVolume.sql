--DECLARE @campaignID INT = 13564
--DECLARE @institutionID int = 6346;

SELECT admis.DirectionID ,
	   D.NewCode AS DirectionCode,
	   D.Name AS DirectionName,
       admis.EducationLevelID ,
       [level].name AS EducationLevelName,
       admis.EducationSourceId ,
       [source].name AS EducationSourceName,
       admis.EducationFormId ,
       [form].name AS EducationFormName,
       ISNULL(admis.Volume, 0) AS AVVolume,
       ISNULL(compet.Volume, 0) AS CGVolume FROM
(
 SELECT	AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 14 AS EducationSourceId, 11 AS EducationFormId, ISNULL(av.[NumberBudgetO], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberBudgetO], 0) != 0
UNION
 SELECT AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 14 AS EducationSourceId, 12 AS EducationFormId, ISNULL(av.[NumberBudgetOZ], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberBudgetOZ], 0) != 0
UNION
 SELECT AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 14 AS EducationSourceId, 10 AS EducationFormId, ISNULL(av.[NumberBudgetZ], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberBudgetZ], 0) != 0
 --целевой (16)
UNION
 SELECT	AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 16 AS EducationSourceId, 11 AS EducationFormId, ISNULL(av.[NumberTargetO], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberTargetO], 0) != 0
UNION
 SELECT AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 16 AS EducationSourceId, 12 AS EducationFormId, ISNULL(av.[NumberTargetOZ], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberTargetOZ], 0) != 0
UNION
 SELECT AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 16 AS EducationSourceId, 10 AS EducationFormId, ISNULL(av.[NumberTargetZ], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberTargetZ], 0) != 0	
  --квота (20)
UNION
 SELECT	AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 20 AS EducationSourceId, 11 AS EducationFormId, ISNULL(av.[NumberQuotaO], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberTargetO], 0) != 0
UNION
 SELECT AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 20 AS EducationSourceId, 12 AS EducationFormId, ISNULL(av.[NumberQuotaOZ], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberTargetOZ], 0) != 0
UNION
 SELECT AV.DirectionID, AV.AdmissionItemTypeID AS EducationLevelID, 20 AS EducationSourceId, 10 AS EducationFormId, ISNULL(av.[NumberQuotaZ], 0) AS Volume
 FROM AdmissionVolume AV WHERE AV.CampaignID = @CampaignID AND isnull(av.[NumberTargetZ], 0) != 0	
) AS admis
LEFT JOIN (
--конкурсы
SELECT
cg.DirectionID
,cg.EducationLevelID
,cg.EducationSourceId
,cg.EducationFormId
,(
ISNULL(sum(isnull(cgi.[NumberBudgetO], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberBudgetOZ], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberBudgetZ], 0)) , 0) 

+ISNULL(sum(isnull(cgi.[NumberPaidO], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberPaidOZ], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberPaidZ], 0)) , 0) 

+ISNULL(sum(isnull(cgi.[NumberQuotaO], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberQuotaOZ], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberQuotaZ], 0)) , 0) 

+ISNULL(sum(isnull(cgi.[NumberTargetO], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberTargetOZ], 0)) , 0) 
+ISNULL(sum(isnull(cgi.[NumberTargetZ], 0)) , 0) 
) AS Volume
From CompetitiveGroup cg  (NOLOCK)
LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
Where cg.CampaignID = @CampaignID AND cg.EducationLevelID != 15
GROUP BY DirectionID, EducationLevelID, cg.EducationSourceId, EducationFormId ) AS compet 
ON compet.DirectionID = admis.DirectionID AND compet.EducationFormId = admis.EducationFormId AND compet.EducationLevelID = admis.EducationLevelID AND compet.EducationSourceId = admis.EducationSourceId
INNER JOIN Direction D ON D.DirectionID = admis.DirectionID
INNER JOIN AdmissionItemType [level] ON [level].ItemTypeID = admis.EducationLevelID
INNER JOIN AdmissionItemType form ON form.ItemTypeID = admis.EducationFormID
INNER JOIN AdmissionItemType [source] ON [source].ItemTypeID = admis.EducationSourceID
WHERE ISNULL(admis.Volume, 0) != ISNULL(compet.Volume, 0)
