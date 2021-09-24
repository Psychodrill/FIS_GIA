-- выборка документа аккредитации для ОО
--declare @institutionId int
--declare @accreditationId int

select TOP 1
    -- документ аккредитации InstitutionDocumentInfoDto
	ins.InstitutionID InstitutionId,
	acc_doc.AttachmentId AttachmentId,
	acc_doc.DisplayName,
	acc_doc.Name FileName,
	acc_doc.MimeType MimeType,
	acc_doc.FileID FileId
from
	Institution ins
	inner join InstitutionAccreditation acc on acc.InstitutionID = ins.InstitutionID
	inner join Attachment acc_doc on acc.AttachmentID = acc_doc.AttachmentID
where
	ins.InstitutionID = @institutionId
	and acc.AccreditationID = @accreditationId