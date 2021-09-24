-- обновление ссылки на файл общежития ОО
--declare @institutionId int
--declare @attachmentId int

update
	Institution
set
	HostelAttachmentId = @attachmentId
where
	InstitutionId = @institutionId