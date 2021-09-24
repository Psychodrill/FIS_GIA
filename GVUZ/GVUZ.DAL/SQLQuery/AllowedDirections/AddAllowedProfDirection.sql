--declare @institutionId int
--declare @requestId int

merge into EntranceTestProfileDirection d
using(
	select 
		req.InstitutionId, 
		req.DirectionId 
	from 
		InstitutionDirectionRequest req 
	where 
		req.RequestId = @requestId 
		and req.InstitutionId = @institutionId
) t
on 
	d.DirectionId = t.DirectionId
when not matched then 
	insert(DirectionID, CreatedDate) 
	values(t.DirectionId, GETDATE());