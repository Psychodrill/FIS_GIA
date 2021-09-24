-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (37, '037_2012_06_14_SelectInformationOrg')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectInformationOrg]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SelectInformationOrg]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SelectInformationOrg]
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
						O.LetterToReschedule, O.LetterToRescheduleContentType, O.LetterToRescheduleName
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


