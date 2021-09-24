--DECLARE @ItemTypeID INT

SELECT
	ait.ItemTypeID,
	ait.Name,
	ait.ItemLevel,
	ait.CanBeSkipped,
	ait.AutoCopyName,
	ait.DisplayOrder,
	ait.CreatedDate,
	ait.ModifiedDate
FROM
	AdmissionItemType AS ait