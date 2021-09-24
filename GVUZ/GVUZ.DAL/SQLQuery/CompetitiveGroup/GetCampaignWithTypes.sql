--declare @InstitutionID int = 587;
--SELECT c.[CampaignID]
--	  ,c.YearStart
--      ,ct.[Name]
--FROM [Campaign] c
--LEFT JOIN CampaignTypes ct on c.CampaignTypeID = ct.CampaignTypeID
SELECT c.[CampaignID]
	  ,c.YearStart
      ,ct.[Name]
	  ,ct.CampaignTypeID
	  ,cel.EducationLevelID as EducationLevelID
	  ,el.Name as EducationLevelName
	  ,c.EducationFormFlag
FROM [Campaign] c with (nolock)
LEFT JOIN CampaignTypes ct with (nolock) on c.CampaignTypeID = ct.CampaignTypeID
LEFT JOIN CampaignEducationLevel cel with (nolock) on c.CampaignID = cel.CampaignID
LEFT JOIN AdmissionItemType el with (nolock) on el.ItemTypeID = cel.EducationLevelID
WHERE C.InstitutionID = @InstitutionID
ORDER BY c.YearStart, cel.EducationLevelID;