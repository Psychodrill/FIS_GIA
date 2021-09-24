SELECT distinct d.*, pd.* , ait.*
FROM Direction AS d
	INNER JOIN ParentDirection AS pd ON pd.ParentDirectionID = d.ParentID
	INNER JOIN AllowedDirections AS ad ON ad.DirectionID = d.DirectionID
	INNER JOIN CampaignEducationLevel AS cel ON cel.EducationLevelID = ad.AdmissionItemTypeID
	LEFT JOIN AdmissionItemType AS ait ON ait.ItemTypeID = ad.AdmissionItemTypeID
WHERE ad.InstitutionID=@InstitutionID AND cel.CampaignID = @CampaignID