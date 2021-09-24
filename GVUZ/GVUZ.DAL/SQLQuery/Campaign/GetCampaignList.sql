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
	ct.Name AS CampaignTypeName,
	substring((
       SELECT t.Name AS [text()]
       FROM (
       SELECT ', ' + ait.Name Name,
       ROW_NUMBER() over(PARTITION BY ait.ItemTypeID ORDER BY cel.CampaignID) is_distinct
       FROM
       CampaignEducationLevel AS cel
	   INNER JOIN AdmissionItemType AS ait ON ait.ItemTypeID = cel.EducationLevelID
       WHERE
       cel.CampaignID = c.CampaignID
       ) t 
       WHERE t.is_distinct = 1 ORDER BY t.Name
       for xml path('')
       ), 3, 8000) LevelsEducation
FROM
	Campaign AS c
		INNER JOIN CampaignStatus AS cs ON cs.StatusID = c.StatusID
		INNER JOIN CampaignTypes AS ct ON ct.CampaignTypeID = c.CampaignTypeID
WHERE c.InstitutionID=@InstitutionID

                    