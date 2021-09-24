CREATE proc [dbo].[SearchRegion]
as
begin
	select
		region.[Id] RegionId
		, region.[Name] [Name]
	from dbo.Region region
	where region.InOrganization = 1
	order by region.[Name] --region.SortIndex

	return 0
end