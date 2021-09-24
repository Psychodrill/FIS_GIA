SELECT
	c.CampaignID,
	c.InstitutionID,
	c.Name,
	c.YearStart,
	c.YearEnd,
	c.EducationFormFlag,
	c.StatusID,
	c.UID,
	c.CreatedDate,
	c.ModifiedDate,
	c.CampaignGUID,
	c.CampaignTypeID
FROM
	Campaign AS c
WHERE c.InstitutionID = @InstitutionID