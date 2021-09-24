-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (47, '047_2012_07_17_Reports.sql')
-- =========================================================================
GO 


GO
CREATE function [dbo].[ReportOrgsRegistrationBASE] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [Полное наименование] NVARCHAR(4000) NULL,
      [Краткое наименование] NVARCHAR(2000) null,
      [ФО] nvarchar(1000) null,
      [Код ФО] int null,
      [Субъект РФ] nvarchar(1000) null,
      [Код субъекта] nvarchar(1000) null,
      [Тип] nvarchar(1000) null,
      [Вид] nvarchar(1000) null,
      [ОПФ] nvarchar(50) null,
      [Филиал] nvarchar(50) null,
      [ФИО руководителя] nvarchar(1000) null,
      [Должность руководителя] nvarchar(1000) null,
      [Учредитель] nvarchar(1000) null,
      [Фактический адрес] nvarchar(1000) null,
      [Юридический адрес] nvarchar(1000) null,
      [Код города] nvarchar(1000) null,
      [Телефон] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [ИНН] nvarchar(10) null,
      [КПП] nvarchar(9) null,
      [ОГРН] nvarchar(13) null,
      [Срок подключения к ЗКСПД] datetime null,
      [Срок предоставления информации в ФИС] datetime null,
      [Головная организация] nvarchar(MAX) null,
      [Регион Филиала(код)] int null,
      [Регион учредителя(код)] int null,
      [Статус регистрации] nvarchar(80) null,
      [Обязательность регистрации] nvarchar(10) null
    )
AS BEGIN
 
    INSERT  INTO @Report
            SELECT  Org.Id as [Id],
                    Org.FullName AS [Полное наименование],
                    ISNULL(Org.ShortName, '') AS [Краткое наименование],
                    FD.[Name] AS [ФО],
                    FD.Code AS [Код ФО],
                    Reg.[Name] AS [Субъект РФ],
                    Reg.Code AS [Код субъекта],
                    OrgType.[Name] AS [Тип],
                    OrgKind.[Name] AS [Вид],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель'  THEN '-' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') END AS [ОПФ],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель' THEN '-' ELSE REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') END AS [Филиал],
                    Org.DirectorFullName AS [ФИО руководителя],
                    Org.DirectorPosition AS [Должность руководителя],
                    Dep.FullName AS [Ведомственная принадлежность],
                    Org.FactAddress AS [Фактический адрес],
                    Org.LawAddress AS [Юридический адрес],
                    Org.PhoneCityCode AS [Код города],
                    Org.Phone AS [Телефон],
                    Org.EMail AS [EMail],
                    Org.INN AS [ИНН],
                    Org.KPP as [КПП],
                    Org.OGRN AS [ОГРН],
                    org.TimeConnectionToSecureNetwork as [Срок подключения к ЗКСПД],
                    org.TimeEnterInformationInFIS as [Срок предоставления информации в ФИС],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE Org.FullName END as [Головная организация],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as [Регион филиала(код)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE Null END as [Регион учредителя(код)],
                    ISNULL(ORS.Status, 'Регистрацию не начинали') [Статус регистрации],
                    ISNULL(MDL.ModelType, 'Да') [Обязательность регистрации]
            FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        'Отключенно' as Status
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
                                        'На регистрации'
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
                                        'На доработке'
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
                                        'На согласовании'
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
                                        'Активировано'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'activated'
                              ) ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN ( select distinct
                                        a.Id,
                                        a.ModelType
                                from    ( select    o.Id,
                                                    'Нет' as ModelType
                                          from      Organization2010 O
                                          where     o.IsFilial = 1
                                                    and o.MainId in (
                                                    select  O.id
                                                    from    Organization2010 O
                                                            inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                                    )
                                          union all
                                          select    O.id,
                                                    'Нет'
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

