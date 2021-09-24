-- список направлений в заявках 
-- declare @institutionId int = 587

select
    req.RequestId,
	req.RequestType,
	req.IsDenied,
	dir.DirectionId,
	dir.Code,
	dir.QUALIFICATIONCODE,
	dir.NewCode,
	dir.Name DirectionName,
	dir.QUALIFICATIONNAME,
	lvl.ItemTypeID EducationLevelId,
	lvl.Name EducationLevelName
from
	InstitutionDirectionRequest req
	inner join Direction dir on req.DirectionId = dir.DirectionID
	inner join AdmissionItemType lvl on dir.EducationLevelId = lvl.ItemTypeID and lvl.ItemLevel = 2
where
	req.InstitutionId = @institutionId
	and req.RequestType IN(@@REQUESTTYPES@@)
