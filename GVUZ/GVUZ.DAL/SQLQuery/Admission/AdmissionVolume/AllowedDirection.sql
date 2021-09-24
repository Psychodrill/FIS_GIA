--USE gvuz_develop_2016
--DECLARE @InstitutionID INT = 587, 
--		@CampaignID INT = 46

SELECT distinct
	ad.AdmissionItemTypeID,
	ait.Name AS AdmissionTypeName,
	ad.DirectionID,
	d.Code,
	d.NewCode,
	d.Name AS DirectionName,
	pd.ParentDirectionID,
	pd.Name AS ParentDirectionName,
	pd.Code AS ParentDirectionCode,
	d.QUALIFICATIONCODE AS QualificationCode
	,ait.DisplayOrder, pd.Name, d.Name
FROM
	AllowedDirections AS ad 
	INNER JOIN AdmissionItemType AS ait ON ait.ItemTypeID = ad.AdmissionItemTypeID
	INNER JOIN Direction AS d ON d.DirectionID = ad.DirectionID
	INNER JOIN ParentDirection AS pd ON pd.ParentDirectionID = d.ParentID
	INNER JOIN CampaignEducationLevel AS cel ON cel.EducationLevelID = ait.ItemTypeID
WHERE ad.InstitutionID=@InstitutionID AND cel.CampaignID=@CampaignID
ORDER BY ait.DisplayOrder, pd.Name, d.Name
