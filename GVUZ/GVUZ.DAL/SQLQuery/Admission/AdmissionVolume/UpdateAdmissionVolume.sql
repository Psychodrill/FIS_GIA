UPDATE AdmissionVolume
SET
	-- AdmissionVolumeID = ? -- this column value is auto-generated
	InstitutionID = @InstitutionID,
	AdmissionItemTypeID = @AdmissionItemTypeID,
	DirectionID = @DirectionID,
	NumberBudgetO = @NumberBudgetO,
	NumberBudgetOZ = @NumberBudgetOZ,
	NumberBudgetZ = @NumberBudgetZ,
	NumberPaidO = @NumberPaidO,
	NumberPaidOZ = @NumberPaidOZ,
	NumberPaidZ = @NumberPaidZ,
	UID = @UID,
	CreatedDate = @CreatedDate,
	ModifiedDate = @ModifiedDate,
	Course = @Course,
	CampaignID = @CampaignID,
	NumberTargetO = @NumberTargetO,
	NumberTargetOZ = @NumberTargetOZ,
	NumberTargetZ = @NumberTargetZ,
	NumberQuotaO = @NumberQuotaO,
	NumberQuotaOZ = @NumberQuotaOZ,
	NumberQuotaZ = @NumberQuotaZ,
	AdmissionVolumeGUID = @AdmissionVolumeGUID
	