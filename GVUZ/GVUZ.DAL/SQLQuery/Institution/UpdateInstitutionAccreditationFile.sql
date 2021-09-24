-- обновление ссылки на файл лицензии ОО
--declare @institutionId int
--declare @attachmentId int

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

if (@accreditationId is not null and @attachmentId is not null)
update
	InstitutionAccreditation 
set
	AttachmentId = @attachmentId
where
	AccreditationId = @accreditationId
	and InstitutionId = @institutionId