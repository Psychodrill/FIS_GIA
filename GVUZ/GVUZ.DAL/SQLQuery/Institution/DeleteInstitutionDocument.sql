-- удаление файла документа, прикрепленного к ОО и ссылки на удаляемый файл
--declare @institutionId int
--declare @attachmentId int

if (@institutionId is not null and @attachmentId is not null)
delete from InstitutionDocuments where InstitutionId = @institutionId and AttachmentId = @attachmentId

if (@attachmentId is not null) 
delete from Attachment where AttachmentId = @attachmentId