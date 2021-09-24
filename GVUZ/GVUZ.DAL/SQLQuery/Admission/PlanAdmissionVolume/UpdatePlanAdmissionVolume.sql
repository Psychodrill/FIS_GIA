UPDATE 
	PlanAdmissionVolume 
SET
	CampaignID = @CampaignID
	,AdmissionItemTypeID = @AdmissionItemTypeID
	,DirectionID = @DirectionID
	,EducationSourceID = @EducationSourceID
	,EducationFormID = @EducationFormID
	,Number = @Number
	,UID = @UID
	,CreatedDate = @CreatedDate
	,ModifiedDate = @ModifiedDate
	,AdmissionVolumeGUID = @AdmissionVolumeGUID
	,ParentDirectionID = @ParentDirectionID
WHERE 
	PlanAdmissionVolumeID = @PlanAdmissionVolumeID 