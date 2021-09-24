-- declare @CampaignID int = 46;
---

declare @cgForms int = 0;
declare @avForms int = 0;

select @cgForms = sum(efID) From (
select distinct  
case 
	when cg.EducationFormId = 10 then 1 
	when cg.EducationFormId = 11 then 2
	when cg.EducationFormId = 12 then 4
end  as efID
		From CompetitiveGroup cg  (NOLOCK)
		Where cg.CampaignID = @CampaignID) as q

-- 

select @avForms = max(O) +  max(OZ)*2 + max(Z)*4 From (
select distinct
case when 
	av.[NumberBudgetO] > 0 OR [NumberPaidO] > 0 OR [NumberTargetO] > 0 OR isnull([NumberQuotaO], 0) > 0
then 1 else 0 end as O
,case when 
	av.[NumberBudgetOZ] > 0 OR [NumberPaidOZ] > 0 OR [NumberTargetOZ] > 0 OR isnull([NumberQuotaOZ], 0) > 0
then 1 else 0 end as OZ
,case when 
	av.[NumberBudgetZ] > 0 OR [NumberPaidZ] > 0 OR [NumberTargetZ] > 0 OR isnull([NumberQuotaZ], 0) > 0
then 1 else 0 end as Z
From AdmissionVolume av (NOLOCK)
Where av.CampaignID = @CampaignID
	And not (
	av.[NumberBudgetO] = 0 AND av.[NumberBudgetOZ] = 0 AND av.[NumberBudgetZ] = 0 AND 
	av.[NumberPaidO] = 0 AND av.[NumberPaidOZ] = 0 AND av.[NumberPaidZ] = 0 AND 
	av.[NumberTargetO] = 0 AND av.[NumberTargetOZ] = 0 AND av.[NumberTargetZ] = 0 AND 
	isnull(av.[NumberQuotaO], 0) = 0 AND isnull(av.[NumberQuotaOZ], 0) = 0 AND isnull(av.[NumberQuotaZ], 0) = 0
	)
) as q

select isnull(@cgForms,0) | isnull(@avForms,0);