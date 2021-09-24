-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (47, '047_2012_07_17_Reports.sql')
-- =========================================================================
GO 


GO
CREATE function [dbo].[ReportOrgsRegistrationBASE] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [������ ������������] NVARCHAR(4000) NULL,
      [������� ������������] NVARCHAR(2000) null,
      [��] nvarchar(1000) null,
      [��� ��] int null,
      [������� ��] nvarchar(1000) null,
      [��� ��������] nvarchar(1000) null,
      [���] nvarchar(1000) null,
      [���] nvarchar(1000) null,
      [���] nvarchar(50) null,
      [������] nvarchar(50) null,
      [��� ������������] nvarchar(1000) null,
      [��������� ������������] nvarchar(1000) null,
      [����������] nvarchar(1000) null,
      [����������� �����] nvarchar(1000) null,
      [����������� �����] nvarchar(1000) null,
      [��� ������] nvarchar(1000) null,
      [�������] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [���] nvarchar(10) null,
      [���] nvarchar(9) null,
      [����] nvarchar(13) null,
      [���� ����������� � �����] datetime null,
      [���� �������������� ���������� � ���] datetime null,
      [�������� �����������] nvarchar(MAX) null,
      [������ �������(���)] int null,
      [������ ����������(���)] int null,
      [������ �����������] nvarchar(80) null,
      [�������������� �����������] nvarchar(10) null
    )
AS BEGIN
 
    INSERT  INTO @Report
            SELECT  Org.Id as [Id],
                    Org.FullName AS [������ ������������],
                    ISNULL(Org.ShortName, '') AS [������� ������������],
                    FD.[Name] AS [��],
                    FD.Code AS [��� ��],
                    Reg.[Name] AS [������� ��],
                    Reg.Code AS [��� ��������],
                    OrgType.[Name] AS [���],
                    OrgKind.[Name] AS [���],
                    Case when OrgType.[Name] = '����' OR OrgType.[Name] = '����������'  THEN '-' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, '�������'), 0, '���-���') END AS [���],
                    Case when OrgType.[Name] = '����' OR OrgType.[Name] = '����������' THEN '-' ELSE REPLACE(REPLACE(Org.IsFilial, 1, '��'), 0, '���') END AS [������],
                    Org.DirectorFullName AS [��� ������������],
                    Org.DirectorPosition AS [��������� ������������],
                    Dep.FullName AS [������������� ��������������],
                    Org.FactAddress AS [����������� �����],
                    Org.LawAddress AS [����������� �����],
                    Org.PhoneCityCode AS [��� ������],
                    Org.Phone AS [�������],
                    Org.EMail AS [EMail],
                    Org.INN AS [���],
                    Org.KPP as [���],
                    Org.OGRN AS [����],
                    org.TimeConnectionToSecureNetwork as [���� ����������� � �����],
                    org.TimeEnterInformationInFIS as [���� �������������� ���������� � ���],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE Org.FullName END as [�������� �����������],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as [������ �������(���)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE Null END as [������ ����������(���)],
                    ISNULL(ORS.Status, '����������� �� ��������') [������ �����������],
                    ISNULL(MDL.ModelType, '��') [�������������� �����������]
            FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        '����������' as Status
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'deactivated'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision'
                                                or Status = 'registration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        '�� �����������'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'registration'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        '�� ���������'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'revision'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        '�� ������������'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'consideration'
                                        and orq.OrganizationId not in (
                                        select  orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        '������������'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'activated'
                              ) ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN ( select distinct
                                        a.Id,
                                        a.ModelType
                                from    ( select    o.Id,
                                                    '���' as ModelType
                                          from      Organization2010 O
                                          where     o.IsFilial = 1
                                                    and o.MainId in (
                                                    select  O.id
                                                    from    Organization2010 O
                                                            inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                                    )
                                          union all
                                          select    O.id,
                                                    '���'
                                          from      Organization2010 O
                                                    inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                          where     o.IsFilial = 1
                                                   
                                        ) A
                              ) MDL ON MDL.Id = Org.Id
            where   org.StatusId = 1
		 order by OrgType.SortOrder

    RETURN
   END

GO

