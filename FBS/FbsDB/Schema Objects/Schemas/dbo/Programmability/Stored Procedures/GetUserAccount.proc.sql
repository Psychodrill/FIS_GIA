
-- =============================================
-- Получение информации о пользователе.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.1.2: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.3: Modified by Fomin Dmitriy 07.07.2008
-- Добавлены поля EducationInstitutionTypeId, 
-- EducationInstitutionTypeName.
-- v.2.0 Modified by A.Vinichenko 14.04.2011
-- Информация об организации пользоватетеля берется 
-- из таблицы Organization2010
-- =============================================
CREATE procedure [dbo].[GetUserAccount]
	@login nvarchar(255)
as
begin
	declare @currentYear int, @accountId bigint--, @userGroupId int

	set @currentYear = Year(GetDate())

--	select @userGroupId = [group].Id
--	from dbo.[Group] [group] with (nolock, fastfirstrow)
--	where [group].Code = 'User'

	select @accountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	select account.[Login]
		, account.LastName
		, account.FirstName
		, account.PatronymicName
		, region.[Id] OrganizationRegionId
		, region.[Name] OrganizationRegionName
		, OReq.Id OrganizationId
		, OReq.FullName OrganizationName
		, OReq.OwnerDepartment OrganizationFounderName
		, OReq.LawAddress OrganizationAddress
		, OReq.DirectorFullName OrganizationChiefName
		, OReq.Fax OrganizationFax
		, OReq.Phone OrganizationPhone
		, OReq.EMail OrganizationEmail
		, OReq.Site OrganizationSite
		, OReq.ShortName OrganizationShortName
		, OReq.FactAddress OrganizationFactAddress
		, OReq.DirectorPosition OrganizationDirectorPosition
		, OReq.IsPrivate OrganizationIsPrivate
		, OReq.IsFilial OrganizationIsFilial
		, OReq.PhoneCityCode OrganizationPhoneCode
		, OReq.AccreditationSertificate AccreditationSertificate
		, OReq.INN OrganizationINN
		, OReq.OGRN OrganizationOGRN
		, account.Phone
		, account.Email
		, account.IpAddresses IpAddresses 
		, account.Status 
		, case
			when account.CanViewUserAccountRegistrationDocument = 1 
				then account.RegistrationDocument 
			else null
		end RegistrationDocument 
		, case
			when account.CanViewUserAccountRegistrationDocument = 1 
				then account.RegistrationDocumentContentType
			else null
		end RegistrationDocumentContentType
		, account.AdminComment AdminComment
		, dbo.CanEditUserAccount(account.Status, account.ConfirmYear, @currentYear) CanEdit
		, dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument 
		, account.HasFixedIp HasFixedIp
		, account.HasCrocEgeIntegration HasCrocEgeIntegration
		, OrgType.Id OrgTypeId
		, OrgType.[Name] OrgTypeName
		, OrgKind.Id OrgKindId
		, OrgKind.[Name] OrgKindName
		, OReq.Id OReqId
	from (select
				account.[Login] [Login]
				, account.LastName LastName
				, account.FirstName FirstName
				, account.PatronymicName PatronymicName
				, account.OrganizationId OrganizationId
				, account.Phone Phone
				, account.Email Email
				, account.ConfirmYear ConfirmYear
				, account.RegistrationDocument RegistrationDocument
				, account.RegistrationDocumentContentType RegistrationDocumentContentType
				, account.AdminComment AdminComment
				, account.IpAddresses IpAddresses
				, account.HasFixedIp HasFixedIp
				, account.HasCrocEgeIntegration HasCrocEgeIntegration
				, dbo.GetUserStatus(account.ConfirmYear, account.Status
						, @currentYear, account.RegistrationDocument) Status 
				, dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument
			from dbo.Account account with (nolock, fastfirstrow)
			where account.[Id] = @accountId
--					and account.Id in (
--						select group_account.AccountId
--						from dbo.GroupAccount group_account
--						where group_account.GroupId = @userGroupId)
			) account
			left outer join dbo.Organization2010 OReq with (nolock, fastfirstrow) 
			left outer join dbo.Region region with (nolock, fastfirstrow) on region.[Id] = OReq.RegionId
			left outer join dbo.OrganizationType2010 OrgType on OReq.TypeId = OrgType.Id
			left outer join dbo.OrganizationKind OrgKind on OReq.KindId = OrgKind.Id
				on OReq.[Id] = account.OrganizationId
	return 0
end
