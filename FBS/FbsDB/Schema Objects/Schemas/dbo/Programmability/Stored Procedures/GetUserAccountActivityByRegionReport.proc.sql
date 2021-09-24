
-- exec dbo.GetUserAccountActivityByRegionReport
-- ========================================================
-- Отчет по активности пользователей по регионам.
-- v.1.0: Create by Fomin Dmitriy 03.09.2008
-- ========================================================
CREATE procedure dbo.GetUserAccountActivityByRegionReport
as 
begin
	declare
		@year int

	set @year = Year(GetDate())
	
	select
		region.Code RegionCode
		, region.Name RegionName
		, report.[Count]
		, case 
			when report.RegionId is null then 1
			else 0
		end IsTotal
	from (select 
			region.Id RegionId
			, count(*) [Count]
		from dbo.AuthenticationEventLog auth_log
			inner join dbo.Account account
				inner join dbo.Organization organization
					inner join dbo.Region region
						on region.Id = organization.RegionId
					on organization.Id = account.OrganizationId
				on auth_log.AccountId = account.Id
		where
			account.Id in (select 
						group_account.AccountId
					from dbo.GroupAccount group_account
						inner join dbo.[Group] [group]
							on [group].Id = group_account.GroupId
					where [group].Code = 'User')
			and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
					, account.RegistrationDocument) = 'activated'
			and auth_log.IsPasswordValid = 1
			and auth_log.IsIpValid = 1
		group by 
			region.Id
		with rollup) report
			left outer join dbo.Region region
				on region.Id = report.RegionId
	order by
		IsTotal
		, region.SortIndex

	return 0
end
