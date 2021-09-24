--DECLARE @institutionID INT = 587;

select IsNull(FounderEsrpOrgId, 0) as FounderEsrpOrgId
--case when FounderEsrpOrgId = 9549 then 1 else 0 end as IsMVD
From Institution 
WHERE InstitutionID = @institutionID