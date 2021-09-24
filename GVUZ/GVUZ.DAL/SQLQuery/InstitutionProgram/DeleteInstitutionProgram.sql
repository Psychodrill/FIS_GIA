--DECLARE @CompetitiveGroupTargetID INT @InstitutionID
DELETE FROM InstitutionProgram WITH (ROWLOCK)
WHERE InstitutionProgramID=@InstitutionProgramID
	AND InstitutionID = @InstitutionID