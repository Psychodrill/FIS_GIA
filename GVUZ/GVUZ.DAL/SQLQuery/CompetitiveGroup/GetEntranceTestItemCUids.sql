--declare @InstitutionID int = 587;

select distinct etic.[EntranceTestItemID] as ID, etic.UID
,isnull(s.Name, etic.SubjectName) as Name
From [EntranceTestItemC] etic with (nolock)
left join Subject s with (nolock) on s.SubjectID = etic.SubjectID
left join CompetitiveGroup cg with (nolock) on etic.CompetitiveGroupID = cg.CompetitiveGroupID
Where cg.InstitutionID = @InstitutionID and etic.UID is not null;