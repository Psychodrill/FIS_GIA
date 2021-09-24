-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (18, '018_2012_05_26_AlteringSelectInformationOrg')
-- =========================================================================
PRINT N'Altering [dbo].[SelectInformationOrg]...';
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
      O.CreateDate, O.UpdateDate, O.DateChangeStatus, O.Reason, O.ReceptionOnResultsCNE
                    FROM 
                        dbo.Organization2010 O
                    INNER JOIN 
                        dbo.Region Reg ON Reg.Id=O.RegionId 
                    INNER JOIN 
                        dbo.OrganizationType2010 [Type] ON [Type].Id=O.TypeId
                    INNER JOIN 
                        dbo.OrganizationKind Kind ON Kind.Id=O.KindId 
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