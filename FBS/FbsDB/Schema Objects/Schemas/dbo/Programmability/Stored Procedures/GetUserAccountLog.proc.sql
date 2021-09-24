-- exec dbo.GetUserAccountLog

-- =============================================
-- Получить лог учетной записи пользователя.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- v.1.3: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.4: Modified by Sedov Anton 25.06.2008
-- Удалено поле RegistrationDocument
-- вместо него возвращается null
-- v.1.5: Modified by Sedov Anton 10.07.2008
-- В результат добавлено поле
-- EducationInstitutionTypeName
-- =============================================
CREATE procedure [dbo].[GetUserAccountLog]
	@login nvarchar(255)
	, @versionId int
as
begin
	declare
		@accountId bigint

	select @accountId = account.Id
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	select
		account_log.[Login] [Login]
		, account_log.VersionId VersionId
		, account_log.UpdateDate UpdateDate
		, editor.[Login] EditorLogin
		, editor.LastName EditorLastName
		, editor.FirstName EditorFirstName
		, editor.PatronymicName EditorPatronymicName
		, account_log.EditorIp EditorIp
		, account_log.IsVpnEditorIp IsVpnEditorIp
		, account_log.LastName LastName
		, account_log.FirstName FirstName
		, account_log.PatronymicName PatronymicName
		, organization_log.RegionId OrganizationRegionId
		, region.[Name] OrganizationRegionName
		, organization_log.[Name] OrganizationName
		, organization_log.FounderName OrganizationFounderName
		, organization_log.Address OrganizationAddress
		, organization_log.ChiefName OrganizationChiefName
		, organization_log.Fax OrganizationFax
		, organization_log.Phone OrganizationPhone
		, account_log.Phone Phone
		, account_log.Email Email
		, account_log.IpAddresses IpAddresses
		, account_log.HasFixedIp HasFixedIp
		, null RegistrationDocument
		, account_log.AdminComment AdminComment
		, account_log.Status Status
		, account_log.HasCrocEgeIntegration HasCrocEgeIntegration
		, education_institution_type.[Name] EducationInstitutionTypeName
	from
		dbo.AccountLog account_log with (nolock, fastfirstrow)
			left outer join dbo.OrganizationLog organization_log with (nolock, fastfirstrow)
				left join dbo.OrganizationType2010 education_institution_type
					on education_institution_type.Id = organization_log.EducationInstitutionTypeId
				on account_log.OrganizationId = organization_log.OrganizationId
				and organization_log.UpdateId = (select top 1 last_linked_account_log.UpdateId
						from dbo.AccountLog last_linked_account_log with (nolock, fastfirstrow)
						where last_linked_account_log.AccountId = @accountId
							and last_linked_account_log.VersionId = (select max(inner_account_log.VersionId)
									from dbo.AccountLog inner_account_log with (nolock)
										inner join dbo.OrganizationLog inner_organization_log with (nolock)
											on inner_account_log.OrganizationId = inner_organization_log.OrganizationId
												and inner_account_log.UpdateId = inner_organization_log.UpdateId
									where inner_account_log.AccountId = @accountId
										and inner_account_log.VersionId <= @versionId))
			left outer join dbo.Region region with (nolock, fastfirstrow)
				on organization_log.RegionId = region.[Id]
			left outer join dbo.Account editor with (nolock, fastfirstrow)
				on editor.Id = account_log.EditorAccountId
	where
		account_log.AccountId = @accountId
		and account_log.VersionId = @versionId

	return 0
end

