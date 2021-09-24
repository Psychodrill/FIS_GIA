-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (48, '048_2012_07_25_AddOrgTimeAgreement')
-- =========================================================================

ALTER TABLE [dbo].[Organization2010] ADD
	[IsAgreedTimeConnection] [bit] NULL,
	[IsAgreedTimeEnterInformation] [bit] NULL
	
ALTER TABLE [dbo].[OrganizationUpdateHistory] ADD
	[IsAgreedTimeConnection] [bit] NULL,
	[IsAgreedTimeEnterInformation] [bit] NULL

GO


ALTER PROCEDURE [dbo].[SelectInformationOrg]
 @Id INT
AS
BEGIN
SELECT
                        O.Id,O.[Version],O.FullName,O.ShortName,O.INN,O.OGRN,O.KPP, O.OwnerDepartment,O.IsPrivate,O.IsFilial,
                        O.DirectorFullName,O.DirectorPosition,O.AccreditationSertificate,O.LawAddress,O.FactAddress,
                        O.PhoneCityCode,O.Phone,O.Fax,O.EMail,O.[Site],
                        Reg.Id as RegionId, [Type].Id as TypeId, Kind.Id as KindId,
						Reg.Code as RegionCode,
                        Reg.Name as RegionName, [Type].Name as TypeName, Kind.Name as KindName,
                        O.RCModel as RCModelId, RC.ModelName as RCModelName, O.RCDescription,
                        O.CNFederalBudget, O.CNTargeted, O.CNLocalBudget, O.CNPaying, O.CNFullTime, O.CNEvening, O.CNPostal,
                        O.MainId, MO.FullName as MainFullName, MO.ShortName as MainShortName,
						O.StatusId, [Status].Name as StatusName,
                        O.NewOrgId, [NO].FullName as NewOrgFullName, [NO].ShortName as NewOrgShortName,
                        O.DepartmentId, DO.FullName as DepartmentFullName, DO.ShortName as DepartmentShortName,
						O.CreateDate, O.UpdateDate, O.DateChangeStatus, O.Reason, O.ReceptionOnResultsCNE, O.TimeConnectionToSecureNetwork, O.TimeEnterInformationInFIS,
						sch.Id as ConnectionSchemeId, sch.Name as ConnectionSchemeName, conStatus.Id as ConnectionStatusId, conStatus.Name as ConnectionStatusName,
						O.LetterToReschedule, O.LetterToRescheduleContentType, O.LetterToRescheduleName,
						O.IsAgreedTimeConnection, O.IsAgreedTimeEnterInformation
                    FROM 
                        dbo.Organization2010 O
                    INNER JOIN 
                        dbo.Region Reg ON Reg.Id=O.RegionId 
                    INNER JOIN 
                        dbo.OrganizationType2010 [Type] ON [Type].Id=O.TypeId
                    INNER JOIN 
                        dbo.OrganizationKind Kind ON Kind.Id=O.KindId 
                    INNER JOIN 
                        dbo.ConnectionScheme sch ON sch.Id=O.ConnectionSchemeId
                    INNER JOIN 
                        dbo.ConnectionStatus conStatus ON conStatus.Id=O.ConnectionStatusId
                    INNER JOIN 
                        [dbo].[RecruitmentCampaigns] RC on O.RCModel = RC.Id
                    INNER JOIN 
						[dbo].[OrganizationOperatingStatus] [Status] on O.StatusId = [Status].Id
                    LEFT JOIN
                        [dbo].[Organization2010] MO on O.MainId = MO.Id
					LEFT JOIN 
						[dbo].[Organization2010] [NO] on O.NewOrgId = [NO].Id
                    LEFT JOIN
                        [dbo].[Organization2010] DO on O.DepartmentId = DO.Id
                    WHERE 
                        O.Id=@id
end

GO

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
				O.IsAgreedTimeEnterInformation				
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
                JOIN dbo.Organization2010 O ON OReq.OrganizationId = O.Id
                LEFT OUTER JOIN dbo.Region region WITH ( NOLOCK, FASTFIRSTROW ) ON region.[Id] = OReq.RegionId
                LEFT OUTER JOIN dbo.OrganizationType2010 OrgType ON OReq.TypeId = OrgType.Id
                LEFT OUTER JOIN dbo.OrganizationKind OrgKind ON OReq.KindId = OrgKind.Id ON OReq.[Id] = account.OrganizationId
                LEFT OUTER JOIN dbo.RecruitmentCampaigns RCModel ON OReq.RCModelID = RCModel.Id
        RETURN 0
    END

GO




