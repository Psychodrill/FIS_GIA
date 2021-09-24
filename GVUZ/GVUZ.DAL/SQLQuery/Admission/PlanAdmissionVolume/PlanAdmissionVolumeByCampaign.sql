SELECT 
	PlanAdmissionVolumeID
	,CampaignID
	,AdmissionItemTypeID
	,DirectionID
	,EducationSourceID
	,EducationFormID
	,Number
	,UID
	,CreatedDate
	,ModifiedDate
	,AdmissionVolumeGUID
	,ParentDirectionID
FROM 
	PlanAdmissionVolume (NOLOCK)
WHERE
	CampaignID = @campaignId
