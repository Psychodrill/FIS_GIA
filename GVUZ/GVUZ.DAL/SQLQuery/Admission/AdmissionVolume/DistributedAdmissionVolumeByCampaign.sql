SELECT 
	DistributedAdmissionVolumeID
    ,AdmissionVolumeID
    ,IdLevelBudget
    ,NumberBudgetO
    ,NumberBudgetOZ
    ,NumberBudgetZ
    ,NumberQuotaO
    ,NumberQuotaOZ
    ,NumberQuotaZ
    ,NumberTargetO
    ,NumberTargetOZ
    ,NumberTargetZ
FROM 
	DistributedAdmissionVolume
WHERE
	AdmissionVolumeID IN 
	(SELECT AdmissionVolumeID FROM AdmissionVolume WHERE CampaignID = @campaignId)
