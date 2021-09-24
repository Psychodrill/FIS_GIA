DELETE FROM
	DistributedPlanAdmissionVolume
WHERE 	 
	PlanAdmissionVolumeID = @PlanAdmissionVolumeID  

DELETE FROM
	PlanAdmissionVolume
WHERE 	 
	PlanAdmissionVolumeID = @PlanAdmissionVolumeID  