select
	distinct 
	pd.ParentDirectionId Id,
	case when pd.code is not null then rtrim(pd.Code) + ' - ' + pd.Name else pd.Name end as Name
from
	ParentDirection pd
	inner join Direction d ON pd.ParentDirectionID = d.ParentID AND d.IsVisible = 1
order by
	Name