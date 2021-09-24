/****** Object:  StoredProcedure [dbo].[gvuz_ValidateOtherApplicationsCount]    Script Date: 07/23/2014 18:58:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


alter PROC [dbo].[gvuz_ValidateOtherApplicationsCount] 
 @appilationId INT, 
 @dateFrom DATETIME, 
 @dateTO DATETIME
AS
BEGIN
 
	;WITH f as (
			SELECT DISTINCT
				e.FirstName,
				e.LastName,
				ISNULL(e.MiddleName, '') AS MiddleName,		
				ed.DocumentTypeID,
				ISNULL(ed.DocumentSeries, '') AS DocumentSeries,
				ed.DocumentNumber		
			FROM 
				Application as a 
				JOIN ApplicationEntrantDocument ad ON a.ApplicationID = ad.ApplicationID
				JOIN EntrantDocument ed ON ad.EntrantDocumentID = ed.EntrantDocumentID
				JOIN Entrant e ON e.EntrantID = ed.EntrantID
			WHERE 
				a.ApplicationID = @appilationId AND 
				a.RegistrationDate >= @dateFrom AND 
				a.RegistrationDate <= @dateTO AND 
				ed.DocumentTypeID = 1)
	SELECT DISTINCT
		a.InstitutionID	
	FROM	
		Entrant e
		JOIN EntrantDocument ed ON e.EntrantID = ed.EntrantID		
		JOIN ApplicationEntrantDocument ad ON ed.EntrantDocumentID = ad.EntrantDocumentID
		JOIN Application as a ON ad.ApplicationID = a.ApplicationID
		JOIN ApplicationStatusType s ON s.StatusID = a.StatusID
		JOIN ApplicationSelectedCompetitiveGroup ag ON ag.ApplicationID = a.ApplicationID
		JOIN ApplicationSelectedCompetitiveGroupItem item ON item.ApplicationID = a.ApplicationID
		--JOIN CompetitiveGroupItem AS gitem ON gitem.CompetitiveGroupItemID = item.ItemID
		JOIN CompetitiveGroupItem AS gitem ON gitem.CompetitiveGroupItemID = item.CompetitiveGroupItemID
		JOIN CompetitiveGroup g ON g.CompetitiveGroupID = ag.CompetitiveGroupID
		JOIN f ON
			f.FirstName = e.FirstName AND
			f.LastName = e.LastName AND
			f.MiddleName = e.MiddleName AND
			f.DocumentTypeID = ed.DocumentTypeID AND				
			f.DocumentSeries = ed.DocumentSeries AND
			f.DocumentNumber = ed.DocumentNumber
	WHERE 
		ed.DocumentTypeID =1 AND
		a.RegistrationDate >= @dateFrom AND 
		a.RegistrationDate <= @dateTO AND 		
		s.IsActiveApp = 1 AND
		gitem.EducationLevelID IN (2, 5, 19) AND
		g.Course = 1
END