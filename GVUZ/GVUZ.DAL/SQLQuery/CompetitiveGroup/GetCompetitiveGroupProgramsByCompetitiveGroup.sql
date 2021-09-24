--DECLARE @CompetitiveGroupID INT = 8;

--SELECT [ProgramID]
--      ,isnull([UID], '') as UID
--      ,[CompetitiveGroupID] 
--      ,isnull([Name], '') as Name
--      ,isnull([Code], '') as Code
--FROM [CompetitiveGroupProgram] with (nolock)
--WHERE CompetitiveGroupID = @CompetitiveGroupID;

--Новая логика (2017)
SELECT p.InstitutionProgramID AS ProgramID
      ,isnull([UID], '') as UID
      ,[CompetitiveGroupID] 
      ,isnull([Name], '') as Name
      ,isnull([Code], '') as Code
	  ,isnull([Code], '') + ' ' + isnull([Name], '') + 
	  (case when UID is not null THEN '(UID: ' + UID + ')' ELSE '' END) as Program
FROM CompetitiveGroupToProgram p (NOLOCK)
INNER JOIN InstitutionProgram ip (NOLOCK) ON P.InstitutionProgramID = ip.InstitutionProgramID
WHERE CompetitiveGroupID = @CompetitiveGroupID;