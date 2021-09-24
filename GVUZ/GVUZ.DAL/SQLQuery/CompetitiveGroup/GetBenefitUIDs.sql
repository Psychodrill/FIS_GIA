-- declare @CampaignID int = 46;

select distinct bi.BenefitItemID as ID
,bi.UID
,isnull(s.Name, etic.SubjectName) as Name
from BenefitItemC bi  with (nolock)
left join [EntranceTestItemC] etic with (nolock) ON bi.EntranceTestItemID = etic.EntranceTestItemID
left join Subject s with (nolock) on s.SubjectID = etic.SubjectID
left join CompetitiveGroup cg with (nolock) ON cg.CompetitiveGroupID = bi.CompetitiveGroupID
Where cg.CampaignID = @CampaignID and bi.UID is not null;