--DECLARE @CompetitiveGroupID INT = 8; 

select *
FROM BenefitItemC b  with (nolock)
--left join EntranceTestItemC etic ON etic.EntranceTestItemID = b.EntranceTestItemID
left join BenefitItemCOlympicType bicot with (nolock) on bicot.BenefitItemID = b.BenefitItemID
Where b.CompetitiveGroupID = @CompetitiveGroupID 
--OR etic.CompetitiveGroupID = @CompetitiveGroupID;