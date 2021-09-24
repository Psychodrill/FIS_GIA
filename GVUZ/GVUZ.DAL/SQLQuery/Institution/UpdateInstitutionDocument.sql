-- добавление ссылки на файл документа ОО
--declare @institutionId int
--declare @attachmentId int

insert into InstitutionDocuments(InstitutionId, AttachmentId, [Year])
values(@institutionId, @attachmentId, @year)