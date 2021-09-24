CREATE procedure [dbo].[GetOrganizationTypeReport]
as 
begin
	SELECT 
	OrgType.Id,
	OrgType.[Name] AS TypeName,
	REPLACE(REPLACE(ISNULL(IsPrivate,''),'1','Негосударственный'),'0','Государственный') AS OPF,
	ISNULL(UsersCount ,0) AS UsersCount
	FROM
		(SELECT OrgReq.TypeId AS TypeId,CONVERT(NVARCHAR(5),OrgReq.IsPrivate) AS IsPrivate, COUNT(Acc.Id) AS UsersCount
		FROM dbo.Account Acc
		INNER JOIN dbo.OrganizationRequest2010 OrgReq
		ON Acc.OrganizationId=OrgReq.Id
		GROUP BY OrgReq.TypeId,OrgReq.IsPrivate
		) Rt
	RIGHT JOIN dbo.OrganizationType2010 OrgType
	ON OrgType.Id=TypeId
	UNION
	SELECT 6,'Итого','',COUNT(*) 
	FROM dbo.Account Acc 
	INNER JOIN dbo.OrganizationRequest2010 OrgReq
	ON Acc.OrganizationId=OrgReq.Id
	ORDER BY OrgType.Id
	--	declare
--		@year int
--
--	set @year = Year(GetDate())
--
--	select
--		[type].Name AS TypeName
--		, report.[Count]
--		, 0 IsSummary
--		, 0	IsTotal
--	from (select 
--			[type].Id OrganizationTypeId
--			, count(*) [Count]
--		from dbo.Account account
--			inner join dbo.OrganizationRequest2010 OrgReq
--				inner join dbo.OrganizationType2010 [type]
--					on [type].Id = OrgReq.TypeId
--				on OrgReq.Id = account.OrganizationId
--		where
--			account.Id in (select 
--						group_account.AccountId
--					from dbo.GroupAccount group_account
--						inner join dbo.[Group] [group]
--							on [group].Id = group_account.GroupId
--					where [group].Code = 'User')
--			and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
--					, account.RegistrationDocument) = 'activated'
--		group by
--			[type].Id
--		with cube) report
--			left outer join dbo.OrganizationType2010 [type]
--				on [type].Id = report.OrganizationTypeId
--	return 0
end