
-- exec dbo.GetUserAccountByRegionReport
-- ========================================================
-- Отчет пользователей по регионам.
-- ========================================================
CREATE procedure [dbo].[GetUserAccountByRegionReport]
as 
begin

	;with RegionUserCountCTE as
	(select 
		isnull(r.Code, '') RegionCode
		, isnull(r.Name, 'Не указано') RegionName
		, count(*) [Count]
	from dbo.Account a with(nolock)
		left join dbo.OrganizationRequest2010 OrgReq with(nolock) on OrgReq.Id = a.OrganizationId
		left join dbo.Region r with(nolock) on r.Id = OrgReq.RegionId
		inner join dbo.GroupAccount ga on ga.AccountId=a.id
	where ga.groupid=1
	group by
		r.Id, r.Code, r.Name
	)
	select *, 0 [IsTotal]
	from RegionUserCountCTE
	union all
	select NULL, NULL, sum(count), 1 from RegionUserCountCTE
	order by [IsTotal], RegionCode

	return 0
end

