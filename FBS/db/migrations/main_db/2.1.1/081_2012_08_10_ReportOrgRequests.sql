-- =========================================================================
-- ?????? ?????????? ? ??????? ???????? ? ???
insert into Migrations(MigrationVersion, MigrationName) values (81, '081_2012_08_10_ReportOrgRequests')
-- =========================================================================
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgRequests]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgRequests]
GO

create FUNCTION [dbo].[ReportOrgRequests]
(	
	@periodBegin DATETIME,
    @periodEnd DATETIME
)
RETURNS @report TABLE 
([???????????? ???????????] nvarchar(4000),
 [??? ??] nvarchar(4000), 
 [???] nvarchar(4000),
 [??????????] nvarchar(4000),
 [???????? ??] nvarchar(4000),
 [??????] nvarchar(4000),
 [?????? ???????????] nvarchar(4000) null,
 [?????????????? ???????????] nvarchar(10) null,
 [??? ????????] nvarchar(1000) null,
 [?????? ??????????(???)] int null,
 [?????? ???????(???)] int null,
 [?????????? ????????? ????????] int,
 [?????????? ????????????? ????????] int,
 [?????????? ???????? ????????] int,
 [?????????? ?????????? ????????] int  
 )

AS begin

	IF @periodBegin IS NULL 
		SET @periodBegin = '19000101'

	DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	INSERT INTO @singleWrongCheck (accountId, count)
	SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData 
	FROM dbo.EventLog 
	WHERE 
	-- ??? ?????? ??? ?????? ?? ???????
		SourceEntityId IS NULL
	-- '|%|%|' - ??? ?????? ??? ????? ?? ???????? ? ?????? ???
		AND EventParams LIKE '|%|%|' 
		AND AccountId IS NOT null
		and [date] between @periodBegin and @periodEnd
	GROUP BY AccountId

	DECLARE @batchWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	INSERT INTO @batchWrongCheck(accountId, count)
	SELECT  batch.OwnerAccountId,
		    COUNT(DISTINCT batchCheck.PassportNumber
				     + ISNULL(batchCheck.PassportSeria, ''))
	FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
		INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
	WHERE   batchCheck.SourceCertificateId IS NULL
		    AND batch.OwnerAccountId IS NOT NULL
			AND batchCheck.PassportNumber IS NOT null
			AND batch.UpdateDate between @periodBegin and @periodEnd
			AND batch.Type = 2
			AND NOT EXISTS ( SELECT *
							FROM   dbo.CommonNationalExamCertificateSubjectCheck
							WHERE  Mark IS NOT NULL AND CheckId = batchCheck.id )
	GROUP BY batch.OwnerAccountId


	DECLARE @allWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	INSERT INTO @allWrongCheck( accountId, count )
	SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleWrongCheck s LEFT JOIN @batchWrongCheck b ON s.accountId = b.accountId

	INSERT INTO @allWrongCheck
	SELECT accountId, count FROM @batchWrongCheck WHERE accountId NOT IN (SELECT accountId FROM @allWrongCheck)

	declare @table_cnts table(AccountId bigint,cnt1 int, cnt2 int, cnt3 int)

	insert @table_cnts
	select cb.OwnerAccountId, count(*) cnt, null,null
	from CommonNationalExamCertificateCheck c
		JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
	where cb.UpdateDate between @periodBegin and @periodEnd
	group by cb.OwnerAccountId
	union all
	select cb.OwnerAccountId, count(*) cnt, null,null
	FROM CommonNationalExamCertificateRequest c 
		JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
	where cb.UpdateDate between @periodBegin and @periodEnd
	group by cb.OwnerAccountId
	union all
	select cb.OwnerAccountId, count(*) cnt, null,null
	FROM CommonNationalExamCertificateSumCheck c 
		JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id  
	where cb.UpdateDate between @periodBegin and @periodEnd
	group by cb.OwnerAccountId
	union all
	select cb.AccountId, null, count(*) ,null
	from CNEWebUICheckLog cb 
	where cb.eventdate between @periodBegin and @periodEnd
	group by cb.AccountId							

	--declare @tbl_unic table (pk int primary key identity(1,1),id int,CertificateNumber nvarchar(4000), Account int)
	
	--insert @tbl_unic
	--select * 
	--from 
	--	(						
	--		select  c.Id,c.CertificateNumber,Account
	--		FROM 
	--			(
	--			select min(c.id) id,c.CertificateNumber,Acc.Id Account
	--			 from CommonNationalExamCertificateCheck c
	--				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
	--				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--			where cb.UpdateDate between @periodBegin and @periodEnd
	--			group by CertificateNumber,Acc.Id
	--			) cb1							
	--			join CommonNationalExamCertificateCheck c on cb1.id=c.id 															
	--			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 								
	--		union all
	--		select c.Id,cb1.Number CertificateNumber,Account
	--		FROM 
	--			(select min(c.id) id, r.Number,Acc.Id Account
	--			from CommonNationalExamCertificateRequest c 
	--				JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
	--				left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id 
	--	  			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--			where cb.UpdateDate between @periodBegin and @periodEnd		  			
	--			group by r.Number,Acc.Id
	--			) cb1				
	--			join CommonNationalExamCertificateRequest c on cb1.id=c.id   
	--			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id
	--		union all
	--		select t.*
	--		from 
	--		(
	--			select cb.id,ISNULL(c.Number, cb.CNENumber) CertificateNumber,Account
	--			from 
	--			(
	--				select max(cb.id) id,c.Number,Acc.Id Account from
	--				(
	--					select cb.id,cb.FoundedCNEId
	--					from CNEWebUICheckLog cb -- with(index(I_CNEWebUICheckLog_AccId))
	--						join Account Acc /*with(index(accOrgIdIndex))*/ on acc.id=cb.AccountId	
	--						join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
	--					where cb.EventDate between @periodBegin and @periodEnd
	--				) cb
	--				left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 						
	--				group by c.Number,Acc.Id	
	--			) cb1
	--				join CNEWebUICheckLog cb on cb1.id=cb.id
	--				left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 
	--				left join ExamcertificateUniqueChecks CC on c.id=cc.id 
	--		) t
	--	) t
		
	--insert @table_cnts	
	--select t.Account, null,null,count(*) from
	--	(
	--	select a.* from @tbl_unic a
	--		join (select min(id)id, CertificateNumber, Account from @tbl_unic group by CertificateNumber, Account) b on a.id=b.id
	--	) t
	--group by Account

	insert into @report
	select org.FullName, 
		   OrgType.[Name], 
		   REPLACE(REPLACE(Org.IsPrivate, 1, '???????'), 0, '???-???') AS [???],
		   Dep.FullName AS [??????????],
		   case when isnull(OrgMain.FullName,'') ='' then org.FullName else OrgMain.FullName end [???????? ??],
		   REPLACE(REPLACE(Org.IsFilial, 1, '??'), 0, '???') AS [??????],
	       ISNULL(ORS.Status, '??????????? ?? ????????') AS [?????? ???????????],
		   ISNULL(MDL.ModelType, '??') [?????????????? ???????????],
		   Reg.Code AS [??? ????????],
		   CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE Null END as [?????? ??????????(???)],
		   CASE WHEN OrgMain.FullName IS NOT NULL THEN OrgMain.RegionId ELSE Org.RegionId END as [?????? ???????(???)],
	       isnull(SUM(wc.[count]),0), isnull(sum(cnt2),0), isnull(sum(cnt1),0),
	       isnull(SUM(cnt3),0)	       
	from @table_cnts tc 
		left join @allWrongCheck wc on tc.accountId=wc.accountId
		join dbo.Account acc on acc.Id = tc.accountId 
		join dbo.Organization2010 org on org.Id = acc.OrganizationId
		JOIN Region Reg ON Reg.Id = Org.RegionId
		JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
		LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
		left JOIN Organization2010 OrgMain ON OrgMain.Id = Org.MainId	
		LEFT JOIN ( select distinct orq.OrganizationId, cast('??????????' as nvarchar(4000)) as Status
                    from    Account A 
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                    where   Status = 'deactivated' 
						and orq.OrganizationId not in (
										                select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in ('activated','consideration','revision','registration') )
					union all
					select distinct orq.OrganizationId,cast('?? ???????????'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'registration'
						and orq.OrganizationId not in (
														select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in( 'activated','consideration','revision' ))
					union all
					select distinct orq.OrganizationId,cast('?? ?????????'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'revision'
						and orq.OrganizationId not in (
														select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in('activated', 'consideration' ))
					union all
					select distinct orq.OrganizationId, cast('?? ????????????'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'consideration'
						and orq.OrganizationId not in (
														select  orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status = 'activated' )
					union all
					select distinct	orq.OrganizationId,	cast('????????????'as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = 'activated'
					) ORS ON ORS.OrganizationId = Org.Id	
					LEFT JOIN ( select distinct
                                        a.Id,
                                        a.ModelType
                                from    ( select    o.Id,
                                                    '???' as ModelType
                                          from      Organization2010 O
                                          where     o.IsFilial = 1
                                                    and o.MainId in (
                                                    select  O.id
                                                    from    Organization2010 O
                                                            inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                                    )
                                          union all
                                          select    O.id,
                                                    '???'
                                          from      Organization2010 O
                                                    inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                          where     o.IsFilial = 1
                                                   
                                        ) A
                              ) MDL ON MDL.Id = Org.Id                              
group by org.Id, org.FullName,OrgType.[Name],Org.IsPrivate,Dep.FullName,OrgMain.FullName,Org.IsFilial,ORS.Status,
		 MDL.ModelType,Reg.Code,Dep.RegionId,OrgMain.RegionId,Org.RegionId
order by org.FullName

return
end
GO




