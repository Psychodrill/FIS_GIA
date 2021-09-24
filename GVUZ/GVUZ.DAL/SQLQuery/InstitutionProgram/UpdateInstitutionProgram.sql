--declare @InstitutionProgramID int = 0;
--declare @UID varchar(200) = '777';
--declare @Name varchar(200) = '777';
--declare @Code varchar(200) = '777';
--declare @InstitutionID int = 587;

IF EXISTS (SELECT TOP 1 1 FROM InstitutionProgram (NOLOCK)
 WHERE Name = @Name 
 AND (Code = @Code OR (Code is null and @Code is null))
 AND InstitutionID = @InstitutionID 
 AND InstitutionProgramID != @InstitutionProgramID
 ) AND EXISTS (
SELECT TOP 1 1 FROM InstitutionProgram (NOLOCK)
 WHERE UID = @UID 
 AND InstitutionID = @InstitutionID 
 AND InstitutionProgramID != @InstitutionProgramID
 )

BEGIN
	SELECT 1 as error, 'Уже существует программа с таким же наименованием и кодом!' as text
	UNION 
	SELECT 2 as error, 'Уже существует программа с таким же UID!'  as text
END;

ELSE IF EXISTS (SELECT TOP 1 1 FROM InstitutionProgram (NOLOCK)
 WHERE Name = @Name 
 AND (Code = @Code OR (Code is null and @Code is null))
 AND InstitutionID = @InstitutionID 
 AND InstitutionProgramID != @InstitutionProgramID
 )

BEGIN
	SELECT 1 as error, 'Уже существует программа с таким же наименованием и кодом!' as text
END;

ELSE IF EXISTS (
SELECT TOP 1 1 FROM InstitutionProgram (NOLOCK)
 WHERE UID = @UID 
 AND InstitutionID = @InstitutionID 
 AND InstitutionProgramID != @InstitutionProgramID
 )
BEGIN
	SELECT 2 as error, 'Уже существует программа с таким же UID!'  as text
END;

ELSE BEGIN
MERGE InstitutionProgram AS TARGET
USING (
	SELECT @InstitutionProgramID as InstitutionProgramID) AS SOURCE
ON TARGET.InstitutionProgramID = SOURCE.InstitutionProgramID
WHEN MATCHED THEN
	UPDATE 
	SET 
		Name = @Name,
		UID = @UID,
		Code = @Code,
		ModifiedDate = GETDATE()
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (
		InstitutionID ,
		[UID] ,
		[Name] ,
		[Code] 
	)
	VALUES(
		@InstitutionID,
		@UID,
		@Name,
		@Code
	);
	
	SELECT 0 as error, 'Сохранено успешно!'	as text
END;
