-- поиск дублирующихся заявлений


 CREATE PROC gvuz_ValidateOtherApplicationsCount 
	@appilationId INT, 
	@dateFrom DATETIME, 
	@dateTO DATETIME
 as
 
 SELECT 
	a.InstitutionID
FROM ApplicationEntrantDocument ad
JOIN EntrantDocument ed ON ad.EntrantDocumentID = ed.EntrantDocumentID
JOIN Application AS a ON a.ApplicationID = ad.ApplicationID
JOIN ApplicationStatusType s ON s.StatusID = a.StatusID
JOIN ApplicationSelectedCompetitiveGroup ag ON ag.ApplicationID = a.ApplicationID
JOIN ApplicationSelectedCompetitiveGroupItem item ON item.ApplicationID = a.ApplicationID
JOIN CompetitiveGroupItem AS gitem ON gitem.CompetitiveGroupItemID = item.ItemID AND gitem.EducationLevelID IN (2, 5)
JOIN CompetitiveGroup g ON g.CompetitiveGroupID = ag.CompetitiveGroupID
JOIN Entrant AS e ON e.EntrantID = a.EntrantID
JOIN
(
	SELECT 
	ed.DocumentTypeID,
	ISNULL(ed.DocumentSeries, '') AS DocumentSeries,
	ed.DocumentNumber,
	e.FirstName,
    e.LastName,
    ISNULL(e.MiddleName, '') AS MiddleName 
	 FROM Entrant e
	 JOIN EntrantDocument ed ON e.EntrantID = ed.EntrantID
) Doc ON Doc.FirstName = e.FirstName AND Doc.LastName = e.LastName
			AND Doc.DocumentTypeID = ed.DocumentTypeID
			AND Doc.DocumentNumber = ed.DocumentNumber
WHERE ed.DocumentTypeID IN (1, 2) 
	AND a.ApplicationID = @appilationId
	AND Doc.MiddleName = CASE WHEN LEN(ISNULL(e.MiddleName, '')) > 0 THEN e.MiddleName ELSE Doc.MiddleName END
	AND Doc.DocumentSeries = CASE WHEN LEN(ISNULL(ed.DocumentSeries, '')) > 0 AND ed.DocumentTypeID =1  
									THEN ed.DocumentSeries 
									ELSE Doc.DocumentSeries 
								END
	AND s.IsActiveApp = 1
	AND g.Course = 1
	AND  a.RegistrationDate >= @dateFrom AND a.RegistrationDate <= @dateTO
 GROUP BY a.ApplicationID,
	a.RegistrationDate,
	a.InstitutionID
 