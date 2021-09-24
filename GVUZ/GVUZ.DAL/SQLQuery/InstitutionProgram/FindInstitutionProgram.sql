SELECT TOP 1 1 
--SELECT ip.InstitutionProgramID ,
--       ip.InstitutionID ,
--       ip.UID ,
--       ip.Name ,
--       ip.Code ,
--       ip.CreatedDate ,
--       ip.ModifiedDate
FROM InstitutionProgram ip  (NOLOCK)
WHERE ip.InstitutionProgramID = @institutionProgramID AND ip.InstitutionID = @institutionID