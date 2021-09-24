--DECLARE @CampaignID int  = 13572
SELECT  
	pd.ParentDirectionID
	,pd.Code
	,pd.Name
	,pd.CreatedDate
	,pd.ModifiedDate 
FROM 
	ParentDirection pd
WHERE
	pd.ParentDirectionID IN 
	(SELECT DISTINCT d.ParentID FROM 
		Direction d
		INNER JOIN AllowedDirections ad (NOLOCK) ON d.DirectionID = ad.DirectionID
		INNER JOIN AdmissionItemType lvl (NOLOCK) ON lvl.ItemTypeID = ad.AdmissionItemTypeID
		INNER JOIN CampaignEducationLevel cel (NOLOCK) ON cel.EducationLevelID = lvl.ItemTypeID
	WHERE 
		cel.CampaignID=@CampaignID
		and ad.InstitutionID = (SELECT TOP 1 InstitutionID FROM Campaign (NOLOCK) WHERE CampaignID = @campaignId))
