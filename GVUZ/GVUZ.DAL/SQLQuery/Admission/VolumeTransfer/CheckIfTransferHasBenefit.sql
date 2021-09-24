--DECLARE @campaignID INT = 13564

SELECT 
	TOP 1 1 
 FROM ApplicationCompetitiveGroupItem acgi (NOLOCK) 
    INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = acgi.ApplicationId
    INNER JOIN OrderOfAdmission ooa (NOLOCK) on ooa.OrderID = acgi.OrderOfAdmissionID 
	INNER JOIN CompetitiveGroup cg (NOLOCK) on cg.CompetitiveGroupId = acgi.CompetitiveGroupId
 WHERE 
    a.StatusID = 8 
    AND acgi.OrderOfExceptionId IS NULL
    AND cg.CampaignID = @campaignID
    AND (cg.EducationSourceId IN (16,20) OR (cg.EducationSourceId = 14 AND ooa.IsForBeneficiary = 1))
    	
