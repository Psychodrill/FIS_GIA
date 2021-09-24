INSERT INTO PlanAdmissionVolume 
( 
	CampaignID
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
)
VALUES
(
	@CampaignID
	,@AdmissionItemTypeID
	,@DirectionID
	,@EducationSourceID
	,@EducationFormID
	,@Number
	,@UID
	,@CreatedDate
	,@ModifiedDate
	,@AdmissionVolumeGUID
	,@ParentDirectionID
)

SELECT @@IDENTITY