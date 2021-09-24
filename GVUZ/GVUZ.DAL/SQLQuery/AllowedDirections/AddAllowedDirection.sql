
--declare @institutionId int
--declare @requestId int
--declare @year int

merge into AllowedDirections d
using (
	select TOP 1
		req.InstitutionId, 
		req.DirectionId, 
		dir.EducationLevelId,
		@year [Year] 
	from 
		InstitutionDirectionRequest req 
		inner join Direction dir on req.DirectionId = dir.DirectionID
	where 
		@year is not null 
		and req.InstitutionId = @institutionId 
		and req.RequestId = @requestId
) t
on 
	d.InstitutionId = t.InstitutionId 
	and d.DirectionId = t.DirectionId 
	and d.[Year] = t.[Year]
WHEN NOT MATCHED THEN
	insert(InstitutionId, DirectionId, CreatedDate, [Year], [AdmissionItemTypeId], [AllowedDirectionStatusID]) 
	values(t.InstitutionId, t.DirectionId, GETDATE(), @year, t.EducationLevelId, 3);
