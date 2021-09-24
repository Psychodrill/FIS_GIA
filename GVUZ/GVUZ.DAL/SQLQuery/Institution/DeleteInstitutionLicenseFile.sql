-- удаление файла лицензии ОО и ссылки на него
--declare @institutionId int

declare @attachmentId int
declare @licenseId int

select TOP 1 
	@attachmentId = lic.AttachmentId, 
	@licenseId = lic.LicenseId 
from 
	InstitutionLicense lic
	inner join Institution ins on lic.InstitutionId = ins.InstitutionId
where
	ins.InstitutionId = @institutionId
order by 
	lic.CreatedDate desc 

if (@licenseId is not null) 
	update InstitutionLicense set AttachmentId = null where LicenseId = @licenseId

if (@attachmentId is not null)
	delete from Attachment where AttachmentId = @attachmentId
