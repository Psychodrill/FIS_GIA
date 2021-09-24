--DECLARE @CompetitiveGroupTargetID INT @InstitutionID
DELETE FROM CompetitiveGroupTarget WITH (ROWLOCK)
WHERE CompetitiveGroupTargetID=@CompetitiveGroupTargetID
	AND InstitutionID = @InstitutionID