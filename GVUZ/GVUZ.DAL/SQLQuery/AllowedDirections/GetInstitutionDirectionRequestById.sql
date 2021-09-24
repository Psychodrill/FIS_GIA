-- получение заявки по id
-- declare @institutionId int = 587
-- declare @requestId int
-- declare @loadComments bit

select
    req.RequestId,
	req.InstitutionId,
	req.RequestType,
	req.RequestDate,
	case @loadComments when 1 then req.RequestComment end Comment,
	case when (req.RequestComment is not null) then 1 else 0 end HasComment,
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
	and req.RequestId = @requestId