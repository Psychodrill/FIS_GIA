

MERGE [CompetitiveGroupToProgram] AS TARGET
USING (
	SELECT p.id as InstitutionProgramID, @CompetitiveGroupID as CompetitiveGroupID from @programs p) AS SOURCE
ON TARGET.InstitutionProgramID = SOURCE.InstitutionProgramID
	AND TARGET.CompetitiveGroupID = SOURCE.CompetitiveGroupID
--WHEN MATCHED THEN
--	UPDATE 
--	SET 
--		InstitutionProgramID = @InstitutionProgramID,
--		ModifiedDate = GETDATE()
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (
		InstitutionProgramID ,
		CompetitiveGroupID ,
		CreatedDate ,
		ModifiedDate
	)
	VALUES(
		InstitutionProgramID,
		@CompetitiveGroupID,
        GETDATE(),
        GETDATE()
	)
WHEN NOT MATCHED BY SOURCE 
	AND TARGET.CompetitiveGroupID = @CompetitiveGroupID
	AND TARGET.InstitutionProgramID not in (select id from @programs)
THEN DELETE;