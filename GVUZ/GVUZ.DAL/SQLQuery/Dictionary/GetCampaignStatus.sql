--DECLARE @CampaignStatusID INT
SELECT
	cs.StatusID,
	cs.Name,
	cs.CreatedDate,
	cs.ModifiedDate
FROM
	CampaignStatus AS cs