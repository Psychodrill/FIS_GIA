--DECLARE @InstitutionID INT = 587;
--SELECT * FROM CompetitiveGroupTarget AS cgt WHERE cgt.InstitutionID=@InstitutionID

SELECT cgt.*, 
case when cgti.ID is null then 1 else 0 end  as CanRemove 
FROM CompetitiveGroupTarget AS cgt  (NOLOCK)
outer apply 
(select top(1) cgti.CompetitiveGroupTargetItemID as ID
	From CompetitiveGroupTargetItem cgti  (NOLOCK)
	Where cgti.CompetitiveGroupTargetID = cgt.CompetitiveGroupTargetID) as cgti
WHERE cgt.InstitutionID=@InstitutionID;