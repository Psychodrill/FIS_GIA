

;with query as
(
select
    distinct ins.InstitutionId,
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
)
select count(*) from query;