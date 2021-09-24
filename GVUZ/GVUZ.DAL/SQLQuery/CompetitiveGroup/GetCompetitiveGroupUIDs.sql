--declare @InstitutionID int = 587;

select distinct cg.CompetitiveGroupID AS ID, cg.UID
From CompetitiveGroup cg  with (nolock)
Where cg.InstitutionID = @InstitutionID and cg.UID is not null;