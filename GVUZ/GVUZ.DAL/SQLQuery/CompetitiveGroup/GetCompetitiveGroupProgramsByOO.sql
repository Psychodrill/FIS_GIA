
SELECT ip.InstitutionProgramID AS [value]
      --,isnull([UID], '') as UID
      --,@CompetitiveGroupID AS [CompetitiveGroupID] 
      --,isnull([Name], '') as Name
      --,isnull([Code], '') as Code
	  ,isnull([Code], '') + ' ' + isnull([Name], '') + 
	  (case when UID is not null THEN '(UID: ' + UID + ')' ELSE '' END) as [label]
FROM  InstitutionProgram ip (NOLOCK) 
WHERE ip.InstitutionID = @InstitutionID;