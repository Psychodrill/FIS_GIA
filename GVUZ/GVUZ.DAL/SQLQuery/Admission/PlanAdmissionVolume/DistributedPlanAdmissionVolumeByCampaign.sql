SELECT 
	DistributedPlanAdmissionVolumeID
	,PlanAdmissionVolumeID
	,IdLevelBudget
	,Number
FROM 
	DistributedPlanAdmissionVolume (NOLOCK)
WHERE
	PlanAdmissionVolumeID IN 
	(SELECT PlanAdmissionVolumeID FROM PlanAdmissionVolume (NOLOCK) WHERE CampaignID = @campaignId)
