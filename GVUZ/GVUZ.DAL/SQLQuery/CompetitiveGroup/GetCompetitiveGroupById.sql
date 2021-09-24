--DECLARE @CompetitiveGroupID INT = 4;
--DECLARE @InstitutionID INT = 587;

SELECT  
	cg.CompetitiveGroupID
	,cg.Name as CompetitiveGroupName 
	,ct.Name as CampaignTypeName
	,c.YearStart as CampaignYearStart 
	,c.CampaignID as CampaignID
	,c.StatusID as CampaignStatusID
	,cgEducationLevel.Name as EducationLevelName
	,cgDirection.Name as DirectionName
	,cgEducationSource.Name as EducationSourceName
	,cgEducationForm.Name as EducationFormName
	,cg.EducationLevelID
	,cg.EducationFormId
	,cg.EducationSourceId
	,cg.DirectionID
	,cg.ParentDirectionID
	,cg.UID
	,cg.Course
	,cg.IsFromKrym
	,cg.IsAdditional
	,etcd.DirectionID as CreativeDirection
	,etpd.DirectionID as ProfileDirection
	,pDirection.Name as UGSNAME

	,isnull(cgi.[NumberBudgetO], 0) as NumberBudgetO
    ,isnull(cgi.[NumberBudgetOZ], 0) as NumberBudgetOZ
    ,isnull(cgi.[NumberBudgetZ], 0) as NumberBudgetZ
    ,isnull(cgi.[NumberPaidO], 0) as NumberPaidO
    ,isnull(cgi.[NumberPaidOZ], 0) as NumberPaidOZ
    ,isnull(cgi.[NumberPaidZ], 0) as NumberPaidZ
    ,isnull(cgi.[NumberQuotaO], 0) as NumberQuotaO
    ,isnull(cgi.[NumberQuotaOZ], 0) as NumberQuotaOZ
    ,isnull(cgi.[NumberQuotaZ], 0) as NumberQuotaZ
	,isnull(cgi.[NumberTargetO], 0) as NumberTargetO
	,isnull(cgi.[NumberTargetOZ], 0) as NumberTargetOZ
	,isnull(cgi.[NumberTargetZ], 0) as NumberTargetZ

	,cg.IdLevelBudget
FROM CompetitiveGroup cg with (nolock)
Left join CompetitiveGroupItem cgi with (nolock) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
left join Campaign c with (nolock) on cg.CampaignID = c.CampaignID
left join CampaignTypes ct with (nolock) on c.CampaignTypeID = ct.CampaignTypeID
left join AdmissionItemType cgEducationLevel with (nolock) on cgEducationLevel.ItemTypeID = cg.EducationLevelID
left join Direction cgDirection with (nolock) on cgDirection.DirectionID = cg.DirectionID
left join ParentDirection pDirection with (nolock) on pDirection.ParentDirectionID = cg.ParentDirectionID
left join AdmissionItemType cgEducationSource with (nolock) on cgEducationSource.ItemTypeID = cg.EducationSourceId
left join AdmissionItemType cgEducationForm with (nolock) on cgEducationForm.ItemTypeID = cg.EducationFormId
left join EntranceTestCreativeDirection etcd with (nolock) on etcd.DirectionID = cg.DirectionID
left join EntranceTestProfileDirection etpd with (nolock) on etpd.DirectionID = cg.DirectionID
WHERE (@InstitutionID IS NULL OR c.InstitutionID = @InstitutionID)
AND cg.CompetitiveGroupID = @CompetitiveGroupID;