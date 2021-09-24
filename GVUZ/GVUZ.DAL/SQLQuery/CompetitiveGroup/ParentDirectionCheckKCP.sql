--declare @CampaignID int = 39257;
--declare @ExcludedCompetitiveGroupID int = -1; --  огда не нужно добал€ть значение текущего конкурса (он редактируетс€). ћожно передать null или -1
--declare @IdLevelBudget int = 1;
--declare @EducationFormID smallint = 11;
--public const short PostalTuition = 10; Z
--public const short FullTimeTuition = 11; O
--public const short MixedTuition = 12; OZ
--declare @EducationSourceID INT = 14; 
--public const short BudgetPlaces = 14; 
--public const short PaidPlaces = 15;
--public const short TargetReception = 16;
--public const short Quota = 20;

--declare @EducationLevelID int = 2;
--declare @ParentDirectionID int  = 44;
--declare @InstitutionID int  = 1;
--declare @DirectionID   = 7270, 14040;

DECLARE @ValueAV int;
DECLARE @ValueCG int;
DECLARE @ValueDAV int;
DECLARE @ValueCGDAV int;


DECLARE @DirectionList TABLE (dir int);

INSERT @DirectionList  SELECT  d.DirectionID 
FROM Direction d 
join AllowedDirections ad ON d.DirectionID = ad.DirectionID
join AdmissionVolume av ON av.ParentDirectionID = d.ParentID
WHERE  d.ParentID = @ParentDirectionID AND ad.InstitutionID = @InstitutionID AND av.CampaignID = @CampaignID


IF (@EducationSourceID = 14) -- Budget
BEGIN
	IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberBudgetO], 0)
		From AdmissionVolume av  (NOLOCK)
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberBudgetO], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberBudgetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID 
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberBudgetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID 
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberBudgetOZ], 0)
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;
		
		select @ValueDAV = isnull(dav.[NumberBudgetOZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberBudgetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberBudgetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID 
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberBudgetZ], 0)
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberBudgetZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberBudgetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberBudgetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

IF (@EducationSourceID = 15) -- Paid (тут DAV не нужен!)
BEGIN
	IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberPaidO], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberPaidO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberPaidOZ], 0)
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberPaidOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberPaidZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberPaidZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

IF (@EducationSourceID = 20) -- Quota
BEGIN
	IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberQuotaO], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberQuotaO], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID= @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberQuotaO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberQuotaO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberQuotaOZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberQuotaOZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberQuotaOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberQuotaOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID 
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberQuotaZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberQuotaZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberQuotaZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi  (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberQuotaZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

IF (@EducationSourceID = 16) -- Target
BEGIN
 IF (@EducationFormID = 11) -- O
	BEGIN
		select @ValueAV = isnull(av.[NumberTargetO], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberTargetO], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberTargetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberTargetO], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 12) -- OZ
	BEGIN
		select @ValueAV = isnull(av.[NumberTargetOZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberTargetOZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberTargetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberTargetOZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END

	IF (@EducationFormID = 10) -- Z
	BEGIN
		select @ValueAV = isnull(av.[NumberTargetZ], 0) 
		From AdmissionVolume av 
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueDAV = isnull(dav.[NumberTargetZ], 0)
		From AdmissionVolume av  (NOLOCK)
		inner join DistributedAdmissionVolume dav (NOLOCK) on av.AdmissionVolumeID = dav.AdmissionVolumeID and dav.IdLevelBudget = @IdLevelBudget
		Where av.CampaignID = @CampaignID AND av.[AdmissionItemTypeID] = @EducationLevelID AND av.ParentDirectionID = @ParentDirectionID;

		select @ValueCG = isnull(sum(isnull(cgi.[NumberTargetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK)  
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
		
		select @ValueCGDAV = isnull(sum(isnull(cgi.[NumberTargetZ], 0)) , 0)
		From CompetitiveGroup cg (NOLOCK) 
		LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID and cg.IdLevelBudget = @IdLevelBudget
		Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID
        AND (cg.ParentDirectionID = @ParentDirectionID OR  cg.DirectionID in (select dir from @DirectionList))
		AND (@ExcludedCompetitiveGroupID is null OR cg.CompetitiveGroupID != @ExcludedCompetitiveGroupID);
	END
END

select @ValueAV as ValueAV, @ValueDAV as ValueDAV, @ValueCG as ValueCG, @ValueCGDAV AS ValueCGDAV;