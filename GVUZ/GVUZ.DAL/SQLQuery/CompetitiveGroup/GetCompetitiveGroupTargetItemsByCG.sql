--DECLARE @CompetitiveGroupID INT = 8;
SELECT cgti.CompetitiveGroupTargetItemID
      ,cgti.CompetitiveGroupTargetID
	  ,cgt.UID
	  ,cgt.Name
	  ,cgt.ContractOrganizationName
      ,NumberTargetO
	  ,NumberTargetOZ
	  ,NumberTargetZ
FROM [CompetitiveGroupTargetItem] cgti WITH (NOLOCK)
INNER JOIN CompetitiveGroupTarget cgt WITH (NOLOCK) on cgt.CompetitiveGroupTargetID = cgti.CompetitiveGroupTargetID
Where [CompetitiveGroupID] = @CompetitiveGroupID;