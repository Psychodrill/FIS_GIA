--declare @pageSize int = 10
--declare @page int = 1

declare @__minRow int = ((@page-1)*@pageSize) + 1
declare @__maxRow int = @__minRow + @pageSize - 1

;with query as
(
select
    ins.InstitutionId,
	ISNULL(ins.FullName, ins.BriefName) InstitutionName,
	count(req.RequestId) NumRequests,
	max(req.RequestDate) LastRequestDate
from
	InstitutionDirectionRequest req
	inner join Institution ins on req.InstitutionId = ins.InstitutionID
where
	req.IsDenied = 0
group by
	ins.InstitutionID, ISNULL(ins.FullName, ins.BriefName)
),
ordered as(
	select q.*, ROW_NUMBER() OVER(ORDER BY InstitutionName) __rn from query q
)
select * from ordered
where __rn between @__minRow and @__maxRow
order by __rn