--DECLARE @CampaignID INT = 46;

SELECT
	cel.CampaignEducationLevelID,
	cel.CampaignID,
	cel.Course,
	cel.EducationLevelID,
	cel.PresentInLicense
--,_cg.ID
--,_av.ID
,case When _cg.ID is not null OR _av.ID is not null then 0 else 1 end as CanRemove
FROM
	CampaignEducationLevel AS cel (NOLOCK)
	outer apply
	(select top(1) cg.CompetitiveGroupID  as ID 
		From CompetitiveGroup cg  (NOLOCK)
		Where cg.CampaignID = cel.CampaignID AND cg.EducationLevelID = cel.EducationLevelID) as _cg
	outer apply
	(
		select top(1) av.AdmissionVolumeID as ID
		From AdmissionVolume av (NOLOCK)
		Where av.CampaignID = cel.CampaignID AND av.AdmissionItemTypeID = cel.EducationLevelID
		And not (
			av.[NumberBudgetO] = 0 AND av.[NumberBudgetOZ] = 0 AND av.[NumberBudgetZ] = 0 AND 
			av.[NumberPaidO] = 0 AND av.[NumberPaidOZ] = 0 AND av.[NumberPaidZ] = 0 AND 
			av.[NumberTargetO] = 0 AND av.[NumberTargetOZ] = 0 AND av.[NumberTargetZ] = 0 AND 
			isnull(av.[NumberQuotaO], 0) = 0 AND isnull(av.[NumberQuotaOZ], 0) = 0 AND isnull(av.[NumberQuotaZ], 0) = 0
			)
	) as _av
WHERE cel.CampaignID=@CampaignID