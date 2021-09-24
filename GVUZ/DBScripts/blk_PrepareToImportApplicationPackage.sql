USE [gvuz_start]
GO
/****** Object:  StoredProcedure [dbo].[blk_PrepareToImportApplicationPackage]    Script Date: 07/24/2013 18:19:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[blk_PrepareToImportApplicationPackage] @packageId INT, @userLogin VARCHAR(100)
AS
	SET NOCOUNT ON	
	
	DECLARE @applicationIds TABLE (Id INT NOT NULL PRIMARY KEY)		
	DECLARE @entrantIds TABLE (Id INT NOT NULL)
	DECLARE @institutionId INT
		
	INSERT INTO @applicationIds
	SELECT a_db.ApplicationID
	FROM blk_Application a 
		JOIN dbo.[Application] a_db with (updlock, rowlock) ON 
			a_db.InstitutionID = a.InstitutionId AND
			a_db.ApplicationNumber = a.ApplicationNumber AND
			a_db.RegistrationDate = a.RegistrationDate			
	WHERE a.ImportPackageId = @packageId
			
	-- Выбираем все у которых нет связки с другими заявлениями
	DECLARE @entrantDocumentIds TABLE (Id INT NOT NULL)
	DECLARE @relatedDocumentIds TABLE (Id INT NOT NULL)
	-----------------------------------------------------------------------------
	-- все документы по заявлениям
	INSERT INTO @entrantDocumentIds	
	SELECT DISTINCT aed.EntrantDocumentID
	FROM @applicationIds a 
		JOIN ApplicationEntrantDocument aed with (updlock, rowlock) ON aed.ApplicationID = a.Id
		--JOIN EntrantDocument ed with (updlock, rowlock) on aed.EntrantDocumentID = ed.EntrantDocumentID
		--JOIN Entrant e with (updlock, rowlock) on e.IdentityDocumentID = ed.EntrantDocumentID
	UNION
	SELECT aed.EntrantDocumentID
	FROM  
		@applicationIds a 
		JOIN dbo.ApplicationEntranceTestDocument aed ON aed.ApplicationID = a.Id
	WHERE aed.EntrantDocumentID IS NOT NULL
			
	-- имеющие ссылки на другие заявления
	INSERT INTO @relatedDocumentIds	
	SELECT DISTINCT aed.EntrantDocumentID
	FROM a
		@entrantDocumentIds AS A1
		JOIN ApplicationEntrantDocument aed with (updlock, rowlock) ON aed.EntrantDocumentID = A1.Id
		--JOIN Entrant e with (updlock, rowlock) on e.IdentityDocumentID = aed.EntrantDocumentID
	WHERE aed.ApplicationID NOT IN (SELECT Id from @applicationIds)				
	UNION
	SELECT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntranceTestDocument aed ON aed.EntrantDocumentID = A1.Id
	WHERE 
		aed.EntrantDocumentID IS NOT NULL AND
		aed.ApplicationID NOT IN (SELECT Id from @applicationIds)	

	-- удаляем кросс документы
	DELETE ed FROM @entrantDocumentIds ed
		JOIN @relatedDocumentIds rd ON ed.Id = rd.Id
	
	-- лок
	declare @stub INT	
	SELECT @stub = COUNT(*) FROM
	EntrantDocument a with (updlock, rowlock) 
	join @entrantDocumentIds i on i.Id = a.EntrantDocumentID	
	
	SELECT @stub = COUNT(*) FROM dbo.Entrant WITH (UPDLOCK, ROWLOCK)	
	WHERE IdentityDocumentID IN (SELECT Id FROM @entrantDocumentIds)
		
	-----------------------------------------------------------------------------
	SELECT TOP 1 @institutionId = InstitutionId
	FROM blk_Application
	WHERE ImportPackageId = @packageId

	/*---------------------------------------------------------*
	 * ЛОГИРОВАНИЕ
	 *---------------------------------------------------------*/		
	--INSERT INTO [dbo].[PersonalDataAccessLog](
	--	[Method], 
	--	[OldData], 
	--	[NewData], 
	--	[ObjectType], 
	--	[AccessMethod], 
	--	[InstitutionID], 
	--	[UserLogin], 
	--	[ObjectID], 
	--	[AccessDate])
	--SELECT 
	--	'D',		
	--	'[{"ApplicationUID":"' + ISNULL(a_db.[UID], '') + 
	--	'","ApplicationNumber":"' + a_db.ApplicationNumber + 
	--	'","ApplicationDate":"\/Date(' + CAST(DATEDIFF(s, '01-01-1970', 
	--		DATEADD(HOUR, DATEDIFF(hour, GETDATE(), GETUTCDATE()), a_db.RegistrationDate)) AS VARCHAR) + 
	--	'000)\/","ApplicationID":' + CAST(a_db.ApplicationID AS VARCHAR) +
	--	',"EntrantUID":"' + ISNULL(e.[UID], '') + 
	--	'","EntrantDocumentID":' + CAST(e.IdentityDocumentID AS VARCHAR) + 
	--	',"EntrantID":' + CAST(e.EntrantID AS VARCHAR) + '}]',		
	--	NULL,
	--	'Application',
	--	'ImportDeleteApplication',		
	--	a_db.InstitutionID,
	--	@userLogin,
	--	NULL,
	--	GETDATE()	
	--FROM 		
	--	dbo.[Application] a_db
	--	JOIN @applicationIds a ON a_db.ApplicationID = a.Id
	--	JOIN dbo.Entrant e ON a_db.EntrantID = e.EntrantID	
	--WHERE a_db.InstitutionID = @institutionId				
	
	UPDATE dbo.Entrant with (updlock, rowlock) 
	SET IdentityDocumentID = NULL
	WHERE IdentityDocumentID IN (SELECT Id FROM @entrantDocumentIds)	
	
	DELETE a FROM [EntrantDocumentEgeAndOlympicSubject] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentOlympicTotal] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentOlympic] a WITH (UPDLOCK, ROWLOCK)	 
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentEge] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentEdu] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [EntrantDocumentDisability] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentCustom] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [EntrantDocumentIdentity] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID

	DELETE a FROM [OrderOfAdmissionHistory] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntranceTestDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntrantDocument] a WITH (UPDLOCK, ROWLOCK)	
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [EntrantDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [ApplicationSelectedCompetitiveGroupTarget] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroupItem] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroup] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationConsidered] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID

	--INSERT INTO @entrantIds
	--SELECT e.EntrantID
	--FROM [dbo].[Application] a  
	--	JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	--	JOIN Entrant e ON a.EntrantID = e.EntrantID
	--WHERE a.InstitutionID = @institutionId AND
	--	NOT EXISTS(SELECT * FROM [EntrantDocument] ed WHERE ed.EntrantID = e.EntrantID)	
		
	DELETE a FROM [dbo].[Application] a WITH (UPDLOCK, ROWLOCK)
		JOIN @applicationIds ai ON ai.Id = a.ApplicationID
	WHERE InstitutionID = @institutionId
		
	--/* Чистим энтрантов и персонов */
	--DELETE e FROM Entrant e WITH (UPDLOCK, ROWLOCK)
	--	JOIN @entrantIds ei ON ei.Id = e.EntrantID
	--WHERE 
	--	NOT EXISTS(SELECT * FROM [EntrantLanguage] el WHERE el.EntrantID = e.EntrantID) AND
	--	NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID)		

	SET NOCOUNT OFF
