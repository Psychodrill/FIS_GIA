USE [gvuz_tags]
GO
/****** Object:  StoredProcedure [dbo].[DeleteApplications_fromXml]    Script Date: 09/24/2013 14:04:54 ******/
/*Добавил необязательный параметр для возможности отключения логирования, по умолчанию оно включено (типа бдительный режим)*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[DeleteApplications_fromXml] @xml NTEXT, @institutionId INT, @userLogin VARCHAR(100), @logEnabled bit = true
AS
	SET NOCOUNT ON	
		
    DECLARE @idoc INT
    EXEC sp_xml_preparedocument @idoc output, @xml, '<ArrayOfApplicationShortRef xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"/>' ;

	-- заявления со статусами = 8
	CREATE TABLE #InOrderApplications (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL)
	CREATE UNIQUE CLUSTERED INDEX [idx_NumberDate] ON #InOrderApplications (ApplicationNumber, RegistrationDate)

	-- не найденные заявления
	CREATE TABLE #NotFoundApplications (		
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL)
	CREATE UNIQUE CLUSTERED INDEX [idx_NumberDate] ON #NotFoundApplications (ApplicationNumber, RegistrationDate)
   
    CREATE TABLE #Applications (
		[ApplicationNumber] [varchar](50) NULL,
		[RegistrationDate] [datetime] NOT NULL)		
	CREATE UNIQUE CLUSTERED INDEX [idx_NumberDate] ON #Applications (ApplicationNumber, RegistrationDate)
	
	INSERT INTO #Applications  (
		[ApplicationNumber],
		[RegistrationDate])
	SELECT 	
		[ApplicationNumber],
		[RegistrationDate]
	FROM openXml(@idoc, '//ApplicationShortRef', 1) 
	WITH 
	( 		
		[ApplicationNumber] [varchar](50) 'ApplicationNumber[@xsi:nil!="true" or not(@xsi:nil)]',
		[RegistrationDate] [datetime] 'RegistrationDate'	
	)		
	EXEC sp_xml_removedocument @idoc;

	CREATE TABLE #processedEntrantsIds ([Id] INT NOT NULL) 
	CREATE TABLE #applicationIds (
		[Id] INT NOT NULL PRIMARY KEY, 
		[ApplicationNumber] [varchar](50) NULL, 
		[RegistrationDate] [datetime] NOT NULL)	
	CREATE NONCLUSTERED INDEX [idx_NumberDate] ON #applicationIds (ApplicationNumber, RegistrationDate)		
	
	-- заявления которые точно есть в БД	
	INSERT INTO #applicationIds
	SELECT a_db.ApplicationID, a_db.ApplicationNumber, a_db.RegistrationDate
	FROM #Applications a 
		JOIN dbo.[Application] a_db ON 
			LTRIM (RTRIM (a_db.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a_db.RegistrationDate = a.RegistrationDate			
	WHERE a_db.InstitutionID = @institutionId AND a_db.StatusID != 8 -- InOrder

	-- Энтраты к которым привязаны удаляемые заявления
	INSERT INTO #processedEntrantsIds
	SELECT a.EntrantID
	FROM
		dbo.[Application] a 
		JOIN #applicationIds a1 ON a.ApplicationID = a1.Id
	WHERE a.InstitutionID = @institutionId

	-- которые со статусами 8
	INSERT INTO #InOrderApplications
	SELECT a_db.ApplicationNumber, a_db.RegistrationDate
	FROM #Applications a 
		JOIN dbo.[Application] a_db ON 			
			LTRIM (RTRIM (a_db.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a_db.RegistrationDate = a.RegistrationDate			
	WHERE a_db.InstitutionID = @institutionId AND a_db.StatusID = 8 -- InOrder

	-- которые не найдены
	INSERT INTO #NotFoundApplications
	SELECT a.ApplicationNumber, a.RegistrationDate
	FROM #Applications a 
		LEFT JOIN #applicationIds a1 ON
			LTRIM (RTRIM (a1.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a1.RegistrationDate = a.RegistrationDate			
		LEFT JOIN #InOrderApplications a2 ON
			LTRIM (RTRIM (a2.ApplicationNumber)) = LTRIM (RTRIM (a.ApplicationNumber)) AND
			a2.RegistrationDate = a.RegistrationDate			
	WHERE a1.Id IS NULL AND a2.ApplicationNumber IS NULL
	
	-- Выбираем все у которых нет связки с другими заявлениями
	DECLARE @entrantDocumentIds TABLE (Id INT NOT NULL)
	DECLARE @relatedDocumentIds TABLE (Id INT NOT NULL)
	-----------------------------------------------------------------------------
	-- все документы по заявлениям
	INSERT INTO @entrantDocumentIds	
	SELECT aed.EntrantDocumentID
	FROM #applicationIds a 
		JOIN ApplicationEntrantDocument aed ON aed.ApplicationID = a.Id
	UNION
	SELECT aed.EntrantDocumentID
	FROM  
		#applicationIds a 
		JOIN dbo.ApplicationEntranceTestDocument aed ON aed.ApplicationID = a.Id
	WHERE aed.EntrantDocumentID IS NOT NULL
			
	-- имеющие ссылки на другие заявления
	INSERT INTO @relatedDocumentIds	
	SELECT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntrantDocument aed ON	aed.EntrantDocumentID = A1.Id
	WHERE aed.ApplicationID NOT IN (SELECT Id from #applicationIds)				
	UNION
	SELECT aed.EntrantDocumentID
	FROM 
		@entrantDocumentIds AS A1
		JOIN ApplicationEntranceTestDocument aed ON aed.EntrantDocumentID = A1.Id
	WHERE 
		aed.EntrantDocumentID IS NOT NULL AND
		aed.ApplicationID NOT IN (SELECT Id from #applicationIds)	

	-- удаляем кросс документы
	DELETE ed FROM @entrantDocumentIds ed
		JOIN @relatedDocumentIds rd ON ed.Id = rd.Id
	-----------------------------------------------------------------------------

	/*---------------------------------------------------------*
	 * ЛОГИРОВАНИЕ
	 *---------------------------------------------------------*/
	if @logEnabled = 1
	begin
		INSERT INTO [dbo].[PersonalDataAccessLog](
			[Method], 
			[OldData], 
			[NewData], 
			[ObjectType], 
			[AccessMethod], 
			[InstitutionID], 
			[UserLogin], 
			[ObjectID], 
			[AccessDate])
		SELECT 
			'D',		
			'[{"ApplicationUID":"' + ISNULL(a_db.[UID], '') + 
			'","ApplicationNumber":"' + a_db.ApplicationNumber + 
			'","ApplicationDate":"\/Date(' + CAST(DATEDIFF(s, '01-01-1970', 
				DATEADD(HOUR, DATEDIFF(hour, GETDATE(), GETUTCDATE()), a_db.RegistrationDate)) AS VARCHAR) + 
			'000)\/","ApplicationID":' + CAST(a_db.ApplicationID AS VARCHAR) +
			',"EntrantUID":"' + ISNULL(e.[UID], '') + 
			'","EntrantDocumentID":' + CAST(ISNULL(e.IdentityDocumentID, 0) AS VARCHAR) + 
			',"EntrantID":' + CAST(a_db.EntrantID AS VARCHAR) + '}]',		
			NULL,
			'Application',
			'ImportDeleteApplication',		
			a_db.InstitutionID,
			@userLogin,
			NULL,
			GETDATE()	
		FROM 		
			dbo.[Application] a_db
			JOIN #applicationIds a ON a_db.ApplicationID = a.Id
			JOIN dbo.Entrant e ON a_db.EntrantID = e.EntrantID
		WHERE a_db.InstitutionID = @institutionId
	end

	UPDATE dbo.Entrant WITH (UPDLOCK, ROWLOCK)
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
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntranceTestDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [ApplicationEntrantDocument] a WITH (UPDLOCK, ROWLOCK)	
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [EntrantDocument] a WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantDocumentIds ai ON ai.Id = a.EntrantDocumentID
	
	DELETE a FROM [ApplicationSelectedCompetitiveGroupTarget] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroupItem] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	
	DELETE a FROM [dbo].[ApplicationSelectedCompetitiveGroup] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID

	DELETE a FROM [dbo].[ApplicationConsidered] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID 		
	
	DECLARE @entrantIds TABLE (Id INT NOT NULL)
	INSERT INTO @entrantIds
	SELECT e.EntrantID
	FROM [dbo].[Application] a 
		JOIN Entrant e ON a.EntrantID = e.EntrantID
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	WHERE a.InstitutionID = @institutionId AND
		NOT EXISTS(SELECT * FROM [EntrantDocument] ed WHERE ed.EntrantID = e.EntrantID)

	DELETE a FROM [dbo].[Application] a WITH (UPDLOCK, ROWLOCK)
		JOIN #applicationIds ai ON ai.Id = a.ApplicationID
	WHERE InstitutionID = @institutionId
	
	/* Чистим энтрантов и персонов */
	DELETE e FROM EntrantLanguage e WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantIds ei ON ei.Id = e.EntrantID
	WHERE 
		NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID)	
	
	DELETE e FROM Entrant e WITH (UPDLOCK, ROWLOCK)
		JOIN @entrantIds ei ON ei.Id = e.EntrantID
	WHERE 
		NOT EXISTS(SELECT * FROM [Application] a WHERE a.EntrantID = e.EntrantID) AND 		
		NOT EXISTS(SELECT * FROM [EntrantLanguage] el WHERE el.EntrantID = e.EntrantID)

	/*---------------------------------------------------------*
	 * Возврат результата в код
	 *---------------------------------------------------------*/		
	SELECT
		(SELECT 
			[ApplicationNumber] AS '@ApplicationNumber',
			[RegistrationDate] AS '@RegistrationDate'
			FROM #InOrderApplications
		 FOR
		 XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationIsInOrder',
		(SELECT 	
			[ApplicationNumber] AS '@ApplicationNumber',
			[RegistrationDate] AS '@RegistrationDate'		
			FROM #NotFoundApplications
		 FOR
		 XML PATH('ApplicationShortRefResult'), TYPE) AS 'ApplicationIsNotFound'
	FOR XML PATH(''),
	ROOT('DeleteApplicationsResult')		

	SET NOCOUNT OFF
