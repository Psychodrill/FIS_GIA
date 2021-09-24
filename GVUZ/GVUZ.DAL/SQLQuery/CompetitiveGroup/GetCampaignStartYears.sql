--DECLARE @InstitutionID INT = 587;

SELECT DISTINCT YearStart
FROM Campaign with (nolock)
WHERE InstitutionID = @InstitutionID