INSERT INTO DistributedPlanAdmissionVolume 
( 
	PlanAdmissionVolumeID
	,IdLevelBudget
	,Number
)
VALUES
(
	@PlanAdmissionVolumeID
	,@IdLevelBudget
	,@Number
)

SELECT @@IDENTITY