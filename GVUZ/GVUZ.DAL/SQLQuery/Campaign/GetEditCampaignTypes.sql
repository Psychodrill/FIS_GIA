SELECT
	ct.CampaignTypeID,
	ct.Name
FROM CampaignTypes AS ct LEFT JOIN Campaign AS c ON c.CampaignTypeID = ct.CampaignTypeID
AND c.InstitutionID=@InstitutionID AND c.YearStart = @YearStart
WHERE c.CampaignTypeID IS NULL