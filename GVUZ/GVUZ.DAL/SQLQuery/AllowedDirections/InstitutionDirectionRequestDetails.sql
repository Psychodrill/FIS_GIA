--declare @institutionId int = 587

select
	req.RequestId,
	req.RequestType,
	req.RequestDate,
	req.RequestComment,
	dir.DirectionId,
	dir.Name DirectionName,
	dir.Code,
	dir.NewCode,
	dir.Name DirectionName,
	ugs.Code QualificationCode,
	ugs.Name QualificationName,
	edu_level.ItemTypeID EducationLevelId,
	edu_level.Name EducationLevelName

from
	InstitutionDirectionRequest req
	inner join Institution ins on req.InstitutionId = ins.InstitutionID
	inner join Direction dir on req.DirectionId = dir.DirectionId
	inner join ParentDirection ugs on dir.ParentID = ugs.ParentDirectionID
	left join AdmissionItemType edu_level on dir.EducationLevelId = edu_level.ItemTypeID and edu_level.ItemLevel = 2
where
	ins.InstitutionID = @institutionId
	and req.IsDenied = 0