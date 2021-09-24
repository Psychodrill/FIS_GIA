-- выборка документа лицензии для ОО
--declare @institutionId int
--declare @licenseId int

select TOP 1
    -- документ лицензии InstitutionDocumentInfoDto
	ins.InstitutionID InstitutionId,
	lic_doc.AttachmentId AttachmentId,
	lic_doc.DisplayName,
	lic_doc.Name FileName,
	lic_doc.MimeType MimeType,
	lic_doc.FileID FileId
from
	Institution ins
	inner join InstitutionLicense lic on lic.InstitutionID = ins.InstitutionID
	inner join Attachment lic_doc on lic.AttachmentID = lic_doc.AttachmentID
where
	ins.InstitutionID = @institutionId
	and lic.LicenseId = @licenseId