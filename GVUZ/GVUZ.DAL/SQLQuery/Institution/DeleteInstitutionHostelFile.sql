-- удаление файла общежития ОО и ссылки на него
--declare @institutionId int

declare @attachmentId int

select TOP 1 
	@attachmentId = ins.HostelAttachmentId
from 
	Institution ins
where
	ins.InstitutionId = @institutionId

if (@attachmentId is not null)
begin
	update Institution set HostelAttachmentId = null where InstitutionId = @institutionId

	delete from Attachment where AttachmentId = @attachmentId
end