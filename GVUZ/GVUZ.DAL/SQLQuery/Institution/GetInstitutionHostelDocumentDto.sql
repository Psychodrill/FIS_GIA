-- ������� ��������� ��������� ��� ��
-- declare @institutionId int

select TOP 1
    -- �������� �������� InstitutionDocumentInfoDto
	ins.InstitutionID InstitutionId,
	doc.AttachmentId AttachmentId,
	doc.DisplayName,
	doc.Name FileName,
	doc.MimeType MimeType,
	doc.FileID FileId
from
	Institution ins
	inner join Attachment doc on ins.HostelAttachmentID = doc.AttachmentID
where
	ins.InstitutionID = @institutionId
