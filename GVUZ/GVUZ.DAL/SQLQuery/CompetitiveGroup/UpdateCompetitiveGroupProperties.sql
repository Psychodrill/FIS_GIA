
--DECLARE @properties TABLE (
--	[CompetitiveGroupID] [int] NOT NULL,
--	[PropertyTypeCode] [int] NOT NULL,
--	[PropertyValue] [varchar](500) NOT NULL)


MERGE [CompetitiveGroupProperties] AS TARGET
USING (	SELECT p.CompetitiveGroupID as CompetitiveGroupID
		,p.PropertyTypeCode as PropertyTypeCode
		,p.PropertyValue as PropertyValue
		from @properties p) AS SOURCE
	ON TARGET.CompetitiveGroupID = SOURCE.CompetitiveGroupID
	AND TARGET.PropertyTypeCode=SOURCE.PropertyTypeCode

WHEN MATCHED THEN
	UPDATE 
	SET 
		TARGET.PropertyValue = SOURCE.PropertyValue,
		TARGET.UpdatedDate = GETDATE()
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (
		CompetitiveGroupID ,
		PropertyTypeCode ,
		PropertyValue,
		CreatedDate,
		UpdatedDate
	)
	VALUES(
		CompetitiveGroupID,
		PropertyTypeCode ,
		PropertyValue,
		GetDate(),
		GetDate()
	);
--WHEN NOT MATCHED BY SOURCE 
--	AND TARGET.CompetitiveGroupID = @CompetitiveGroupID
--	AND TARGET.InstitutionProgramID not in (select id from @programs)
--THEN DELETE;