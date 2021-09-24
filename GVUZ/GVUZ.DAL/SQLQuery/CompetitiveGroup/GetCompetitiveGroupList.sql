--DECLARE @InstitutionID INT = 587;

SELECT 
	cg.CompetitiveGroupID
	,cg.Name as CompetitiveGroupName 
	--,(ct.Name + '(' + CAST(c.YearStart as varchar(4)) + ')') as CampaignName
	,ct.Name as CampaignTypeName
	,c.YearStart as CampaignYearStart 
	,cgEducationLevel.Name as EducationLevelName
	,cgDirection.Name as DirectionName
	,CASE WHEN cgEducationSource.ItemTypeID = 20 THEN N'Особая квота' ELSE cgEducationSource.Name END as EducationSourceName
	,cgEducationForm.Name as EducationFormName
	,cg.EducationLevelID
	,cg.EducationFormId
	,cg.EducationSourceId
	,cg.DirectionID
	,cg.UID
	,cg.Course
	,cg.IsFromKrym
	,cg.IsAdditional
	,cg.IdLevelBudget
	,parDir.Name as UGSNAME
FROM CompetitiveGroup cg with (nolock)
Left join CompetitiveGroupItem cgi with (nolock) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
left join Campaign c with (nolock) on cg.CampaignID = c.CampaignID
left join CampaignTypes ct with (nolock) on c.CampaignTypeID = ct.CampaignTypeID
left join AdmissionItemType cgEducationLevel with (nolock) on cgEducationLevel.ItemTypeID = cg.EducationLevelID
left join Direction cgDirection with (nolock) on cgDirection.DirectionID = cg.DirectionID
left join AdmissionItemType cgEducationSource with (nolock) on cgEducationSource.ItemTypeID = cg.EducationSourceId
left join AdmissionItemType cgEducationForm with (nolock) on cgEducationForm.ItemTypeID = cg.EducationFormId
left join ParentDirection parDir with (nolock) on parDir.ParentDirectionID = cg.ParentDirectionID
WHERE cg.InstitutionID = @InstitutionID