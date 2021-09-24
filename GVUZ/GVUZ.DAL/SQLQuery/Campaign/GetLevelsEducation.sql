--DECLARE @CampaignID INT

SELECT
	ait.Name AS LevelsEducation
FROM
	CampaignEducationLevel AS cel
		INNER JOIN AdmissionItemType AS ait ON ait.ItemTypeID = cel.EducationLevelID
WHERE cel.CampaignID = @CampaignID