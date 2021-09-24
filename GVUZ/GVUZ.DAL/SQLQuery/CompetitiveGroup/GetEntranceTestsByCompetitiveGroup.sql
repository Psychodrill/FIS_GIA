--DECLARE @CompetitiveGroupID INT = 8; --136790; --137253; --4; -- 41442 тут есть записи, кот. можно и нельзя удалять!

select etic.EntranceTestItemID as ItemID
	,etic.EntranceTestTypeID as TestType
	--,isnull(etic.SubjectId, 0) as SubjectID 
	,isnull(s.Name, etic.SubjectName) as TestName
	,etic.MinScore as Value
	,etic.UID
	,etic.EntranceTestPriority
	,etic.IsForSPOandVO
	,etic.ReplacedEntranceTestItemID
	,isnull(repS.Name, repEtic.SubjectName) as ReplacedEntranceTestItemName 
	,cast ( case when q.ID is not null then 0 else 1 end as bit) as CanRemove
	,etic.IsFirst
	,etic.IsSecond
from EntranceTestItemC etic with (nolock)
inner join CompetitiveGroup cg with (nolock) on etic.CompetitiveGroupID = cg.CompetitiveGroupID
left join Subject s with (nolock) on s.SubjectID = etic.SubjectID
left join EntranceTestItemC repEtic with (nolock) on etic.ReplacedEntranceTestItemID = repEtic.EntranceTestItemID
left join Subject repS with (nolock) on repS.SubjectID = repEtic.SubjectID
OUTER APPLY
	(select top(1) ID, EntranceTestItemID 
		From ApplicationEntranceTestDocument aetd with (nolock) 
		inner join application a with (nolock) on aetd.ApplicationID = a.ApplicationID
		where aetd.EntranceTestItemID= etic.EntranceTestItemID
		and a.StatusID != 6 /* если заявление отозвано - то можно редактировать! */
	)as q 

Where cg.CompetitiveGroupID = @CompetitiveGroupID;
