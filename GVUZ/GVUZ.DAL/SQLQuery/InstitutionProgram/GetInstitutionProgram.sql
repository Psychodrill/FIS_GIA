--DECLARE @InstitutionID INT = 587;

SELECT ip.*, 
case when pr1.ex is null then 1 else 0 end  as CanRemove 
FROM InstitutionProgram AS ip  (NOLOCK)
outer apply 
(select top 1 1 AS ex
	From CompetitiveGroupToProgram pr  (NOLOCK)
	Where pr.InstitutionProgramID = ip.InstitutionProgramID) as pr1
WHERE ip.InstitutionID=@InstitutionID
ORDER BY ModifiedDate desc;

SELECT 
	cg.CompetitiveGroupID,
    cg.Name AS CompetitiveGroupName,
	c.Name AS CampaignName,
    ip.InstitutionProgramID,
    ip.Name AS InstitutionProgramName FROM
CompetitiveGroup cg (NOLOCK)
JOIN Campaign C (NOLOCK) ON cg.CampaignID = c.CampaignID
JOIN CompetitiveGroupToProgram cgp (NOLOCK) ON cgp.CompetitiveGroupID = cg.CompetitiveGroupID
JOIN InstitutionProgram IP (NOLOCK) ON cgp.InstitutionProgramID = ip.InstitutionProgramID

WHERE CG.InstitutionID = @InstitutionID
