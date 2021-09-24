-- удаление файла аккредитации ОО и ссылки на него
--declare @institutionId int

declare @attachmentId int
declare @accreditationId int

select TOP 1 
	@attachmentId = acc.AttachmentId, 
	@accreditationId = acc.AccreditationId 
from 
	InstitutionAccreditation acc
	inner join Institution ins on acc.InstitutionId = ins.InstitutionId
where
	ins.InstitutionId = @institutionId
order by 
	acc.CreatedDate desc 

if (@accreditationId is not null) 
	update InstitutionAccreditation set AttachmentId = null where AccreditationId = @accreditationId

if (@attachmentId is not null)
	delete from Attachment where AttachmentId = @attachmentId
