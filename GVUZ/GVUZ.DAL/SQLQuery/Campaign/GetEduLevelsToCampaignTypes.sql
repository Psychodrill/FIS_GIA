
	SELECT
		ait.ItemTypeID,
		ait.Name,
		eltct.CampaignTypeID
	FROM
		AdmissionItemType AS ait
			INNER JOIN EduLevelsToCampaignTypes AS eltct ON eltct.AdmissionItemTypeID = ait.ItemTypeID
	WHERE ait.ItemLevel = 2
	