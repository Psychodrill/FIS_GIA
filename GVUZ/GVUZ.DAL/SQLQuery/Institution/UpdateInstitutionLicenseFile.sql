-- обновление ссылки на файл лицензии ОО
--declare @institutionId int
--declare @attachmentId int

declare @licenseId int

select TOP 1 
	@licenseId = lic.LicenseId 
from 
	InstitutionLicense lic 
	inner join Institution ins on lic.InstitutionId = ins.InstitutionId
where
	ins.InstitutionID = @institutionId
order by 
	lic.CreatedDate desc

if (@licenseId is not null and @attachmentId is not null)
update
	InstitutionLicense 
set
	AttachmentId = @attachmentId
where
	LicenseId = @licenseId
	and InstitutionId = @institutionId