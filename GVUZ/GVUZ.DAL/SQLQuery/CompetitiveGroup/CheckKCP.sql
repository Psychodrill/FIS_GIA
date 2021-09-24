--declare @CampaignID int = 46;
--declare @ExcludedCompetitiveGroupID int = 88; -- Когда не нужно добалять значение текущего конкурса (он редактируется). Можно передать null или -1

--declare @EducationFormID smallint = 11;
----public const short PostalTuition = 10; Z
----public const short FullTimeTuition = 11; O
----public const short MixedTuition = 12; OZ
--declare @EducationSourceID INT = 16; 
----public const short BudgetPlaces = 14; 
----public const short PaidPlaces = 15;
----public const short TargetReception = 16;
----public const short Quota = 20;

--declare @EducationLevelID int = 2;
--declare @DirectionID int  = 6017;

DECLARE @ValueAV int;
DECLARE @ValueCG int;
DECLARE @ValueDAV int;
DECLARE @ValueCGDAV int;
IF (@EducationSourceID = 14) -- Budget
BEGIN
	IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberBudgetO], 0)
		From AdmissionVolume av  (NOLOCK)
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberBudgetO], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberBudgetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberBudgetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberBudgetOZ], 0)
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;
		
		select @ValueDAV = isnull(dav.[NumberBudgetOZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberBudgetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberBudgetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberBudgetZ], 0)
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberBudgetZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberBudgetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberBudgetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

IF (@EducationSourceID = 15) -- Paid (тут DAV не нужен!)
BEGIN
	IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberPaidO], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberPaidO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID 
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberPaidOZ], 0)
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberPaidOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberPaidZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberPaidZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

IF (@EducationSourceID = 20) -- Quota
BEGIN
	IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberQuotaO], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberQuotaO], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberQuotaO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberQuotaO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberQuotaOZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberQuotaOZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberQuotaOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberQuotaOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberQuotaZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberQuotaZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberQuotaZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberQuotaZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

IF (@EducationSourceID = 16) -- Target
BEGIN
 IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberTargetO], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberTargetO], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberTargetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberTargetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberTargetOZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberTargetOZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberTargetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberTargetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberTargetZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueDAV = isnull(dav.[NumberTargetZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.DirectionID = @DirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberTargetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberTargetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

select @ValueAV as ValueAV, @ValueDAV as ValueDAV, @ValueCG as ValueCG, @ValueCGDAV AS ValueCGDAV;