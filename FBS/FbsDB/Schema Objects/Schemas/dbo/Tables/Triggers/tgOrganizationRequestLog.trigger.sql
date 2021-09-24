Create trigger [dbo].[tgOrganizationRequestLog]
on dbo.OrganizationRequest2010
for insert, update
as
	if update(UpdateDate) 
		insert into dbo.OrganizationLog
			(
			OrganizationId
			, VersionId
			, UpdateDate
			, UpdateId
			, EditorAccountId
			, EditorIp
			, RegionId
			, DepartmentOwnershipCode
			, [Name]
			, FounderName
			, Address
			, ChiefName
			, Fax
			, Phone
			, EducationInstitutionTypeId
			)
		select
			OrgReq.[Id]
			, (select isnull(max(organization_log.VersionId), 0) + 1
				from dbo.OrganizationLog organization_log
				where organization_log.OrganizationId = OrgReq.[Id])
			, OrgReq.UpdateDate
			, NEWID()
			, ''
			, ''
			, OrgReq.RegionId
			, ''
			, OrgReq.Fullname
			, OrgReq.OwnerDepartment
			, OrgReq.LawAddress
			, OrgReq.DirectorFullName
			, OrgReq.Fax
			, OrgReq.Phone
			, OrgReq.TypeId
		from Inserted OrgReq
