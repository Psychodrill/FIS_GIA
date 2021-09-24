-- получение наименования ОО по его идентификатору
-- declare @institutionId int

select TOP 1 
	InstitutionId Id,
	ISNULL(FullName, BriefName) Name
from 
	Institution 
where
	InstitutionId = @institutionId