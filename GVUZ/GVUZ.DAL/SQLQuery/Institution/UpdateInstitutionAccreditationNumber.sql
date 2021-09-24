-- обновление номера аккредитации ОО
--declare @institutionId int
--declare @accreditationNumber varchar

declare @accreditationId int

select TOP 1 
	@accreditationId = acc.AccreditationId 
from 
	InstitutionAccreditation acc
	inner join Institution ins on acc.InstitutionId = ins.InstitutionId
where
	ins.InstitutionID = @institutionId
order by 
	acc.CreatedDate desc

if (@accreditationId is not null)
update
	InstitutionAccreditation 
set
	Accreditation = @accreditationNumber
where
	AccreditationId = @accreditationId
	and InstitutionId = @institutionId