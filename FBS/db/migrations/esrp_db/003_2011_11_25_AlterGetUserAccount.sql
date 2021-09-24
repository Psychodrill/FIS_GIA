-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (3, '003__2011_11_25__AlterGetUserAccount')
-- =========================================================================

GO
PRINT N'Altering [dbo].[GetUserAccount]...';

GO

-- exec dbo.GetUserAccount

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
-- =============================================
ALTER PROCEDURE [dbo].[GetUserAccount] @login NVARCHAR(255)
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
                OReq.EMail OrganizationEmail ,
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
                account.HasFixedIp HasFixedIp ,
                account.HasCrocEgeIntegration HasCrocEgeIntegration ,
                OrgType.Id OrgTypeId ,
                OrgType.[Name] OrgTypeName ,
                OrgKind.Id OrgKindId ,
                OrgKind.[Name] OrgKindName ,
                OReq.OrganizationId OReqId ,
                RCModel.ModelName ,
                OReq.RCDescription
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
                            account.HasCrocEgeIntegration HasCrocEgeIntegration ,
                            dbo.GetUserStatus(account.ConfirmYear,
                                              account.Status, @currentYear,
                                              account.RegistrationDocument) Status ,
                            dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument
                  FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                  WHERE     account.[Id] = @accountId
--					and account.Id in (
--						select group_account.AccountId
--						from dbo.GroupAccount group_account
--						where group_account.GroupId = @userGroupId)
                  
                ) account 
                LEFT OUTER JOIN dbo.OrganizationRequest2010 OReq WITH ( NOLOCK,
                                                              FASTFIRSTROW )
                LEFT OUTER JOIN dbo.Region region WITH ( NOLOCK, FASTFIRSTROW ) ON region.[Id] = OReq.RegionId
                LEFT OUTER JOIN dbo.OrganizationType2010 OrgType ON OReq.TypeId = OrgType.Id
                LEFT OUTER JOIN dbo.OrganizationKind OrgKind ON OReq.KindId = OrgKind.Id ON OReq.[Id] = account.OrganizationId
                LEFT OUTER JOIN dbo.RecruitmentCampaigns RCModel ON OReq.RCModelID = RCModel.Id
        RETURN 0
    END
GO
