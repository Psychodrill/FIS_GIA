--DECLARE @CampaignID int  = 13572
SELECT  DISTINCT
	d.DirectionID
    ,d.Code
    ,d.Name
    ,d.ParentID
    ,d.SYS_GUID
    ,d.EDULEVEL
    ,d.EDUPROGRAMTYPE
    ,d.UGSCODE
    ,d.UGSNAME
    ,d.QUALIFICATIONCODE
    ,d.QUALIFICATIONNAME
    ,d.PERIOD
    ,d.EDU_DIRECTORY
    ,d.EDUPR_ADDITIONAL
    ,d.CreatedDate
    ,d.ModifiedDate
    ,d.NewCode
    --,d.EducationLevelId
    ,ad.AdmissionItemTypeID AS EducationLevelId
FROM 
	Direction d
	INNER JOIN AllowedDirections ad (NOLOCK) ON d.DirectionID = ad.DirectionID
	INNER JOIN AdmissionItemType lvl (NOLOCK) ON lvl.ItemTypeID = ad.AdmissionItemTypeID
	INNER JOIN CampaignEducationLevel (NOLOCK) cel ON cel.EducationLevelID = lvl.ItemTypeID
WHERE 
	cel.CampaignID=@CampaignID
	and ad.InstitutionID = (SELECT TOP 1 InstitutionID FROM Campaign (NOLOCK) WHERE CampaignID = @campaignId)

