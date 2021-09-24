--declare @institutionId int
--declare @requestId int

delete 
	dir 
from 
	AllowedDirections dir
	inner join InstitutionDirectionRequest req on dir.InstitutionID = req.InstitutionId and dir.DirectionID = req.DirectionId