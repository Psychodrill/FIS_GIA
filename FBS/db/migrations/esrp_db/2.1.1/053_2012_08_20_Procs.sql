-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (53, '053_2012_08_20_Procs.sql')
go

-- exec dbo.GetAccount

-- =============================================
-- Получение информации об учетной записи 
-- внутреннего пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- v.1.1: Modified by Makarev Andrey 10.04.2008
-- Добавлено поле Account.IpAddresses
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- =============================================
alter procedure dbo.GetAccount
	@login nvarchar(255)
as
begin
	select
		account.[Login] [Login]
		, account.LastName LastName 
		, account.FirstName FirstName
		, account.PatronymicName PatronymicName
		, account.Email Email
		, account.Phone Phone
		, account.IsActive IsActive
		, account.IpAddresses IpAddresses
		, account.HasFixedIp HasFixedIp
		, account.PasswordHash PasswordHash
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	return 0
end

go
-- exec dbo.GetAccountLog

-- =============================================
-- Получить лог учетной записи.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- =============================================
alter procedure dbo.GetAccountLog
	@login nvarchar(255)
	, @versionId int
as
begin

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
		, account_log.Phone Phone
		, account_log.Email Email
		, account_log.IpAddresses IpAddresses
		, account_log.HasFixedIp HasFixedIp
		, account_log.IsActive IsActive
		, account_log.PasswordHash PasswordHash
	from
		dbo.AccountLog account_log with (nolock, fastfirstrow)
			inner join dbo.Account account with (nolock, fastfirstrow)
				on account_log.AccountId = account.[Id]
			left outer join dbo.Account editor with (nolock, fastfirstrow)
				on editor.Id = account_log.EditorAccountId
	where
		account.[Login] = @login
		and account_log.VersionId = @versionId

	return 0
end
go
-- exec dbo.GetRemindAccount

-- =============================================
-- Получить забытую учетную запись.
-- v.1.0: Created by Makarev Andrey
-- =============================================
alter procedure dbo.GetRemindAccount
	@email nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	
	declare
		@currentYear int
		, @eventCode nvarchar(255)
		, @editorAccountId bigint
		, @login nvarchar(255) 
		, @accountId bigint
		, @accountIds nvarchar(255)
		, @PasswordHash nvarchar(510)

	set @currentYear = year(getdate())
	set @eventCode = N'USR_REMIND'

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin

	select top 1
		@login = account.[Login] 
		, @accountId = account.[Id]
		, @PasswordHash = account.PasswordHash
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.email = @email
	order by 
		dbo.GetUserStatusOrder(dbo.GetUserStatus(account.ConfirmYear , account.Status
				, @currentYear, account.RegistrationDocument)) desc
		, account.UpdateDate desc

	select 
		@login [Login]
		, @email email
		, @PasswordHash

	set @accountIds = isnull(convert(nvarchar(255), @accountId), '')

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = @email
		, @updateId = null

	return 0
end
go

-- exec dbo.GetUserAccount

-- =============================================
-- Получение информации о пользователе.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.1.3: Modified by Fomin Dmitriy 07.07.2008
-- Добавлены поля EducationInstitutionTypeId, 
-- EducationInstitutionTypeName.
-- =============================================
alter PROCEDURE [dbo].[GetUserAccount] @login NVARCHAR(255)
AS 
    BEGIN
        DECLARE @currentYear INT ,
            @accountId BIGINT--, @userGroupId int

        SET @currentYear = YEAR(GETDATE())

--	select @userGroupId = [group].Id
--	from dbo.[Group] [group] with (nolock, fastfirstrow)
--	where [group].Code = 'User'

        SELECT  @accountId = account.[Id]
        FROM    dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
        WHERE   account.[Login] = @login

        SELECT  account.[Login] ,
                account.LastName ,
                account.FirstName ,
                account.PatronymicName ,
                region.[Id] OrganizationRegionId ,
                region.[Name] OrganizationRegionName ,
                OReq.Id OrganizationId ,
                OReq.FullName OrganizationName ,
                OReq.OwnerDepartment OrganizationFounderName ,
                OReq.LawAddress OrganizationAddress ,
                OReq.DirectorFullName OrganizationChiefName ,
                OReq.Fax OrganizationFax ,
                OReq.Phone OrganizationPhone ,
                OReq.EMail OrganizationEmail,
				OReq.RCModelID RCModelID,
				OReq.RCDescription,
				OReq.KPP,
				OReq.ReceptionOnResultsCNE ReceptionOnResultsCNE,
                OReq.Site OrganizationSite ,
                OReq.ShortName OrganizationShortName ,
                OReq.FactAddress OrganizationFactAddress ,
                OReq.DirectorPosition OrganizationDirectorPosition ,
                OReq.IsPrivate OrganizationIsPrivate ,
                OReq.IsFilial OrganizationIsFilial ,
                OReq.PhoneCityCode OrganizationPhoneCode ,
                OReq.AccreditationSertificate AccreditationSertificate ,
                OReq.INN OrganizationINN ,
                OReq.OGRN OrganizationOGRN ,
                account.Phone ,
                account.Position ,
                account.Email ,
                account.IpAddresses IpAddresses ,
                account.Status ,
                CASE WHEN account.CanViewUserAccountRegistrationDocument = 1
                     THEN account.RegistrationDocument
                     ELSE NULL
                END RegistrationDocument ,
                CASE WHEN account.CanViewUserAccountRegistrationDocument = 1
                     THEN account.RegistrationDocumentContentType
                     ELSE NULL
                END RegistrationDocumentContentType ,
                account.AdminComment AdminComment ,
                dbo.CanEditUserAccount(account.Status, account.ConfirmYear,
                                       @currentYear) CanEdit ,
                dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument ,
                account.HasFixedIp HasFixedIp,
                OrgType.Id OrgTypeId ,
                OrgType.[Name] OrgTypeName ,
                OrgKind.Id OrgKindId ,
                OrgKind.[Name] OrgKindName ,
                OReq.OrganizationId OReqId ,
                RCModel.ModelName ,
                OReq.RCDescription,
				O.TimeConnectionToSecureNetwork,
				O.TimeEnterInformationInFIS,
				O.IsAgreedTimeConnection,
				O.IsAgreedTimeEnterInformation,
				account.PasswordHash PasswordHash

        FROM    ( SELECT    account.[Login] [Login] ,
                            account.LastName LastName ,
                            account.FirstName FirstName ,
                            account.PatronymicName PatronymicName ,
                            account.OrganizationId OrganizationId ,
                            account.Phone Phone ,
                            account.Position Position ,
                            account.Email Email ,
                            account.ConfirmYear ConfirmYear ,
                            account.RegistrationDocument RegistrationDocument ,
                            account.RegistrationDocumentContentType RegistrationDocumentContentType ,
                            account.AdminComment AdminComment ,
                            account.IpAddresses IpAddresses ,
                            account.HasFixedIp HasFixedIp ,
                            dbo.GetUserStatus(account.ConfirmYear,
                                              account.Status, @currentYear,
                                              account.RegistrationDocument) Status ,
                            dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument,
                            account.PasswordHash PasswordHash
                  FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                  WHERE     account.[Id] = @accountId                  
                ) account 
                LEFT OUTER JOIN dbo.OrganizationRequest2010 OReq WITH ( NOLOCK, FASTFIRSTROW )
				JOIN dbo.Organization2010 O ON OReq.OrganizationId = O.Id
                LEFT OUTER JOIN dbo.Region region WITH ( NOLOCK, FASTFIRSTROW ) ON region.[Id] = OReq.RegionId
                LEFT OUTER JOIN dbo.OrganizationType2010 OrgType ON OReq.TypeId = OrgType.Id
                LEFT OUTER JOIN dbo.OrganizationKind OrgKind ON OReq.KindId = OrgKind.Id ON OReq.[Id] = account.OrganizationId
                LEFT OUTER JOIN dbo.RecruitmentCampaigns RCModel ON OReq.RCModelID = RCModel.Id
        RETURN 0
    END

go



-- exec dbo.GetUserAccountLog

-- =============================================
-- Получить лог учетной записи пользователя.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- v.1.4: Modified by Sedov Anton 25.06.2008
-- Удалено поле RegistrationDocument
-- вместо него возвращается null
-- v.1.5: Modified by Sedov Anton 10.07.2008
-- В результат добавлено поле
-- EducationInstitutionTypeName
-- =============================================
alter procedure [dbo].[GetUserAccountLog]
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
		, education_institution_type.[Name] EducationInstitutionTypeName
		, account_log.PasswordHash PasswordHash
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
go
