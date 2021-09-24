-- выборка документов прикрепленных к сведениям об ОО
-- declare @institutionId int

select 
    -- документы ОО
	ins.InstitutionID InstitutionId,
	ins_doc.Year,
	doc.AttachmentId AttachmentId,
	doc.Name FileName,
	doc.DisplayName DisplayName,
	doc.MimeType MimeType,
	doc.FileID FileId
from
	Institution ins
	inner join InstitutionDocuments ins_doc on ins.InstitutionID = ins_doc.InstitutionId
	inner join Attachment doc on ins_doc.AttachmentId = doc.AttachmentID
where
	ins.InstitutionID = @institutionId
