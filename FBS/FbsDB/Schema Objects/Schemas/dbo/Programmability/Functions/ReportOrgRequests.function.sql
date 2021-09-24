/*
select * from [ReportOrgRequests](null,'20130101')
*/

create FUNCTION [dbo].[ReportOrgRequests]
(	
	@periodBegin DATETIME,
    @periodEnd DATETIME
)
RETURNS @report TABLE 
([Наименование организации] nvarchar(4000),
 [ТИП ОУ] nvarchar(4000), 
 [ОПФ] nvarchar(4000),
 [Учредитель] nvarchar(4000),
 [Головное ОУ] nvarchar(4000),
 [Филиал] nvarchar(4000),
 [Статус регистрации] nvarchar(4000) null,
 [Обязательность регистрации] nvarchar(10) null,
 [Код субъекта] nvarchar(1000) null,
 [Регион учредителя(код)] int null,
 [Регион Филиала(код)] int null,
 [Количество ошибочных запросов] int,
 [Количество интерактивных проверок] int,
 [Количество пакетных проверок] int,
 [Количество уникальных проверок] int  
 )

AS begin

	IF @periodBegin IS NULL 
		SET @periodBegin = '19000101'

	DECLARE @Temp_CNEWebUICheckLog TABLE(pk int PRIMARY KEY CLUSTERED identity(1,1),accountId BIGINT, count INT, FoundedCNEId nvarchar(510))
	
	insert @Temp_CNEWebUICheckLog
	select cb.AccountId, count(cb.id),cb.FoundedCNEId 
	FROM CNEWebUICheckLog cb with(index(IX_CNEWebUICheckLog_FoundedCNEId_EventDate_AccountId))
	where cb.eventdate between @periodBegin and @periodEnd
	group by cb.AccountId,FoundedCNEId
	
	--DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	--INSERT INTO @singleWrongCheck (accountId, count)
	--SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData 
	--FROM dbo.EventLog 
	--WHERE 
	---- это значит что ничего не найдено
	--	SourceEntityId IS NULL
	---- '|%|%|' - это значит что поиск по паспорту и оценок нет
	--	AND EventParams LIKE '|%|%|' 
	--	AND AccountId IS NOT null
	--	and [date] between @periodBegin and @periodEnd
	--GROUP BY AccountId

	DECLARE @batchWrongCheck TABLE(pk int PRIMARY KEY CLUSTERED identity(1,1),accountId BIGINT, count INT)

	INSERT INTO @batchWrongCheck(accountId, count)
	select t.AccountId, sum(cnt)
	from 
		(	
		select cb.OwnerAccountId AccountId, count(*) cnt
		FROM CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
		where c.[Year] is null and cb.UpdateDate between @periodBegin and @periodEnd and OwnerAccountId is not null
		group by cb.OwnerAccountId
		union all
		select cb.OwnerAccountId, count(*) cnt
		FROM CommonNationalExamCertificateRequest c 
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
		where cb.UpdateDate between @periodBegin and @periodEnd and c.SourceCertificateId IS NULL
		group by cb.OwnerAccountId
		union all
		select cb.AccountId, sum([count])
		FROM @Temp_CNEWebUICheckLog cb
		where cb.FoundedCNEId is null
		group by cb.AccountId
		) t
	group by t.AccountId
	
	--DECLARE @allWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	--INSERT INTO @allWrongCheck( accountId, count )
	--SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleWrongCheck s LEFT JOIN @batchWrongCheck b ON s.accountId = b.accountId

	--INSERT INTO @allWrongCheck
	--SELECT accountId, count FROM @batchWrongCheck WHERE accountId NOT IN (SELECT accountId FROM @allWrongCheck)

	declare @table_cnts table(AccountId bigint,cnt1 int, cnt2 int)
	
	insert @table_cnts
	select t.AccountId, sum(cnt1),sum(cnt2)
	from 
		(
		select cb.OwnerAccountId AccountId, count(*) cnt1, 0 cnt2
		from CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
		where cb.UpdateDate between @periodBegin and @periodEnd
		group by cb.OwnerAccountId
		union all
		select cb.OwnerAccountId, count(*) cnt, 0
		FROM CommonNationalExamCertificateRequest c 
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
		where cb.UpdateDate between @periodBegin and @periodEnd
		group by cb.OwnerAccountId
		union all
		select cb.OwnerAccountId, count(*) cnt, 0
		FROM CommonNationalExamCertificateSumCheck c 
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id  
		where cb.UpdateDate between @periodBegin and @periodEnd
		group by cb.OwnerAccountId
		union all
		select cb.AccountId, 0, sum([count]) 
		FROM @Temp_CNEWebUICheckLog cb
		group by cb.AccountId							
	) t
	group by t.AccountId
	
	declare @tbl_unic table (pk int primary key identity(1,1),id int, OrganizationId bigint,cnt int)
	
	insert @tbl_unic(OrganizationId,cnt)
	select OrganizationId, count(distinct CertificateNumber) from
	(
		select distinct Acc.OrganizationId,c.CertificateNumber 
		from CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 					
		union all
		select distinct Acc.OrganizationId,r.Number
		from CommonNationalExamCertificateRequest c 
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
			left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id 
  			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
		union all
		select distinct Acc.OrganizationId,cb.CNENumber
			from CNEWebUICheckLog cb
				left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 
				JOIN Account Acc ON Acc.Id=cb.AccountId 
			where FoundedCNEId is not null
		union all
		select distinct Acc.OrganizationId,c.Number
			from CNEWebUICheckLog cb
				join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 
				JOIN Account Acc ON Acc.Id=cb.AccountId 											
	) t
	group by OrganizationId	
	
	insert into @report
	select org.FullName, 
		   OrgType.[Name], 
		   REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') AS [ОПФ],
		    isnull(Dep.FullName,'Не установлено') AS [Учредитель],
		   case when isnull(OrgMain.FullName,'') ='' then org.FullName else OrgMain.FullName end [Головное ОУ],
		   REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') AS [Филиал],
	       ISNULL(ORS.Status, 'Регистрацию не начинали') AS [Статус регистрации],
		   ISNULL(MDL.ModelType, 'Да') [Обязательность регистрации],
		   Reg.Code AS [Код субъекта],
		   CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE Reg.Code END as [Регион учредителя(код)],
		   CASE WHEN OrgMain.FullName IS NOT NULL THEN OrgMain.RegionId ELSE Org.RegionId END as [Регион филиала(код)],
	       isnull(SUM(wc.[count]),0), isnull(sum(cnt2),0), isnull(sum(cnt1),0),
	       isnull(SUM(uc.cnt),0)	       
	from @table_cnts tc 
		left join @batchWrongCheck wc on tc.accountId=wc.accountId
		join dbo.Account acc on acc.Id = tc.accountId 
		join dbo.Organization2010 org on org.Id = acc.OrganizationId
		left join @tbl_unic uc on uc.OrganizationId=org.id		
		JOIN Region Reg ON Reg.Id = Org.RegionId
		JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
		LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
		left JOIN Organization2010 OrgMain ON OrgMain.Id = Org.MainId	
		LEFT JOIN ( select distinct orq.OrganizationId, cast('Отключенно' as nvarchar(4000)) as Status
                    from    Account A 
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                    where   Status = 'deactivated' 
						and orq.OrganizationId not in (
										                select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in ('activated','consideration','revision','registration') )
					union all
					select distinct orq.OrganizationId,cast('На регистрации'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'registration'
						and orq.OrganizationId not in (
														select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in( 'activated','consideration','revision' ))
					union all
					select distinct orq.OrganizationId,cast('На доработке'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'revision'
						and orq.OrganizationId not in (
														select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in('activated', 'consideration' ))
					union all
					select distinct orq.OrganizationId, cast('На согласовании'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'consideration'
						and orq.OrganizationId not in (
														select  orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status = 'activated' )
					union all
					select distinct	orq.OrganizationId,	cast('Активировано'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'activated'
					) ORS ON ORS.OrganizationId = Org.Id	
					LEFT JOIN 
						(select distinct a.Id, a.ModelType   
						 from 
							(
							select o.Id, 'Нет' as ModelType 
							from Organization2010 O
							where o.IsFilial=1 and o.MainId in (
								select O.id 
								from Organization2010 O
									inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id 
								where modeltype=3
							)
							union all
							select O.id, 'Нет' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id 
							where o.IsFilial=1 and modeltype=2
							) A
						) MDL ON MDL.Id = Org.Id                              
group by org.Id, org.FullName,OrgType.[Name],Org.IsPrivate,Dep.FullName,OrgMain.FullName,Org.IsFilial,ORS.Status,
		 MDL.ModelType,Reg.Code,Dep.RegionId,OrgMain.RegionId,Org.RegionId,OrgType.SortOrder
order by OrgType.SortOrder

return
end
GO


