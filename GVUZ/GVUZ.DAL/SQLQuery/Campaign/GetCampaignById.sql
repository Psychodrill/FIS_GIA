--DECLARE @InstitutionID INT

SELECT
	c.CampaignID,
	c.InstitutionID,
	c.Name AS CampaignName,
	c.YearStart,
	c.YearEnd,
	c.EducationFormFlag,
	c.StatusID,
	c.UID,
	c.CreatedDate AS CampaignCreatedDate,
	c.ModifiedDate AS CampaignModifiedDate,
	c.CampaignGUID,
	c.CampaignTypeID,
	cs.Name AS CampaignStatusName,
	cs.CreatedDate AS CampaignStatusCreatedDate,
	cs.ModifiedDate AS CampaignStatusModifiedDate,
	ct.Name AS CampaignTypeName
FROM
	Campaign AS c
		INNER JOIN CampaignStatus AS cs ON cs.StatusID = c.StatusID
		INNER JOIN CampaignTypes AS ct ON ct.CampaignTypeID = c.CampaignTypeID
WHERE (@InstitutionID IS NULL OR c.InstitutionID = @InstitutionID)
	AND c.CampaignID = @CampaignID