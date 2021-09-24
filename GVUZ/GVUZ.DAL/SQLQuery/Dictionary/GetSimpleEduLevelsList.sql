select
	ItemTypeId Id,
	Name
from
	AdmissionItemType
where
	ItemLevel = 2
order by
	DisplayOrder