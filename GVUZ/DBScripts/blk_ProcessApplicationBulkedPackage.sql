USE [gvuz_start]
GO
/****** Object:  StoredProcedure [dbo].[blk_ProcessApplicationBulkedPackage]    Script Date: 07/24/2013 18:22:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[blk_ProcessApplicationBulkedPackage] @packageId INT, @userLogin VARCHAR(100)
AS
	SET NOCOUNT ON	
		
	declare @stub INT
	DECLARE @InsertedApplicationsIds TABLE (Id INT NOT NULL PRIMARY KEY) 
	DECLARE @InsertedEntrantsIds TABLE (Id INT NOT NULL PRIMARY KEY) 
	DECLARE @InsertedEntrantDocumentsIds TABLE (Id INT NOT NULL PRIMARY KEY) 
	DECLARE @FailedApplicationsUIDs TABLE ([UID] VARCHAR(200) NOT NULL) 	

	/*---------------------------------------------------------*
	 * Удаление старой информации по поступившим заявлениям		 
	 *---------------------------------------------------------*/
	EXEC dbo.[blk_PrepareToImportApplicationPackage] @packageId, @userLogin;
	 
	SELECT @stub = COUNT(*)
	FROM dbo.blk_Entrant e
	join Entrant e_db with (updlock, rowlock) on 
		e_db.InstitutionId = e.InstitutionId AND
		e_db.[UID] = e.[UID]
		JOIN dbo.GenderType gt with(nolock) ON e.GenderId = gt.GenderID
	WHERE e.ImportPackageId = @packageId 		
	 
	/*---------------------------------------------------------*
	 * Entrant
	 *---------------------------------------------------------*/		 
	----DISABLE TRIGGER trig_Entrant_CreatedDate ON Entrant; 
	----DISABLE TRIGGER trig_Entrant_ModifiedDate ON Entrant;	
	UPDATE dbo.Entrant
	SET 
		CustomInformation = e.CustomInformation,
		SNILS = e.Snils,
		LastName = e.LastName,
		FirstName = e.FirstName,
		MiddleName = e.MiddleName,
		GenderID = gt.GenderID,
		ModifiedDate = GETDATE()
	FROM dbo.blk_Entrant e
		JOIN dbo.GenderType gt ON e.GenderId = gt.GenderID
	WHERE 
		e.ImportPackageId = @packageId AND
		e.InstitutionId = dbo.Entrant.InstitutionID AND
		e.[UID] = dbo.Entrant.[UID]
	
	INSERT INTO dbo.Entrant (
		CustomInformation,
		SNILS,
		InstitutionID,
		[UID],
		LastName,
		FirstName,
		MiddleName,
		GenderID,
		CreatedDate,
		ModifiedDate)
	OUTPUT INSERTED.EntrantID INTO @InsertedEntrantsIds
	SELECT DISTINCT
		e.CustomInformation,
		e.Snils,
		e.InstitutionId,
		e.[UID],
		e.LastName,
		e.FirstName,
		e.MiddleName,
		gt.GenderID,
		GETDATE(),
		GETDATE()		
	FROM dbo.blk_Entrant e		
		JOIN dbo.GenderType gt ON e.GenderId = gt.GenderID		
		LEFT JOIN dbo.Entrant e_db ON 			
			e.InstitutionId = e_db.InstitutionID AND
			e.[UID] = e_db.[UID]
	WHERE 	
		e.ImportPackageId = @packageId AND
		e_db.EntrantID IS NULL;
	--ENABLE TRIGGER trig_Entrant_CreatedDate ON Entrant; 
	--ENABLE TRIGGER trig_Entrant_ModifiedDate ON Entrant;	

	---- лок
	--SELECT @stub = 1 FROM
	--Entrant a with (updlock, rowlock) 
	--join @InsertedEntrantsIds i on i.Id = a.EntrantID

	/*---------------------------------------------------------*
	 * Application
	 *---------------------------------------------------------*/	
	--DISABLE TRIGGER trig_Application_CreatedDate ON [Application]; 
	--DISABLE TRIGGER trig_Application_ModifiedDate ON [Application];	 
	INSERT INTO dbo.[Application] (
		EntrantID ,
		RegistrationDate ,
		InstitutionID ,
		NeedHostel ,
		StatusID ,
		StatusDecision ,		          		          
		SourceID ,		          
		ApplicationNumber ,
		OriginalDocumentsReceived ,
		OrderCompetitiveGroupID ,		          
		OrderOfAdmissionID ,		          
		OrderCompetitiveGroupItemID ,
		OrderCalculatedRating ,
		OrderCalculatedBenefitID ,
		OrderEducationFormID ,
		OrderEducationSourceID ,
		LastDenyDate ,
		[UID] ,
		IsRequiresBudgetO ,
		IsRequiresBudgetOZ ,
		IsRequiresBudgetZ ,
		IsRequiresPaidO ,
		IsRequiresPaidOZ ,
		IsRequiresPaidZ ,
		OriginalDocumentsReceivedDate ,
		LastEgeDocumentsCheckDate ,
		OrderCompetitiveGroupTargetID ,
		IsRequiresTargetO ,
		IsRequiresTargetOZ ,
		IsRequiresTargetZ ,
		ApplicationGUID,
		ApproveInstitutionCount,
		FirstHigherEducation,
		ApprovePersonalData,
		FamiliarWithLicenseAndRules,
		FamiliarWithAdmissionType,
		FamiliarWithOriginalDocumentDeliveryDate,
		WizardStepID,
		CreatedDate,
		ModifiedDate)
	OUTPUT INSERTED.ApplicationID INTO @InsertedApplicationsIds
	SELECT 
		e.EntrantID,
		a.RegistrationDate,
		a.InstitutionId,
		a.NeedHostel,
		a.StatusId,
		a.StatusDecision,
		2, -- SourceID 
		a.ApplicationNumber,
		ISNULL(a.OriginalDocumentsReceived, 0), -- проставляется позже
		NULL ,    -- OrderCompetitiveGroupID - int ??? 
		a.OrderOfAdmissionId,
		NULL , -- OrderCompetitiveGroupItemID - int ???
		NULL , -- OrderCalculatedRating - decimal ???
        NULL , -- OrderCalculatedBenefitID - smallint ???
        NULL , -- OrderEducationFormID - smallint ???
        NULL , -- OrderEducationSourceID - smallint ???
		a.LastDenyDate,
		a.[UID],
		a.IsRequiresBudgetO,
		a.IsRequiresBudgetOZ,
		a.IsRequiresBudgetZ,
		a.IsRequiresPaidO,
		a.IsRequiresPaidOZ,
		a.IsRequiresPaidZ,
		a.OriginalDocumentsReceivedDate, -- проставляется позже
		NULL, -- LastEgeDocumentsCheckDate - datetime ???
		NULL, -- OrderCompetitiveGroupTargetID - int ???
		a.IsRequiresTargetO,
		a.IsRequiresTargetOZ,
		a.IsRequiresTargetZ,
		a.Id,
		1,1,1,1,1,1,
		2, -- WizardStepID
		GETDATE(),
		GETDATE()
	FROM 
		dbo.blk_Application a
		JOIN dbo.Entrant e ON
			e.[UID] = a.EntrantUID AND
			e.InstitutionId = a.InstitutionID
	WHERE a.ImportPackageId = @packageId;

	-- лок
	SELECT @stub = COUNT(*) FROM
	Application a with (updlock, rowlock) 
	join @InsertedApplicationsIds i on i.Id = a.ApplicationID	
	SELECT @stub = COUNT(*) FROM	
	Application a with (updlock, rowlock)
	JOIN ApplicationEntrantDocument aed with (updlock, rowlock) on a.ApplicationID = aed.ApplicationID
	join Entrant e with (updlock, rowlock) on e.IdentityDocumentID = aed.EntrantDocumentID  
	join @InsertedApplicationsIds i on i.Id = aed.ApplicationID

	/* Обновляем дату получения оригиналов */	
	UPDATE [dbo].[Application] 
	SET 
		[OriginalDocumentsReceived] = 1, 
		[OriginalDocumentsReceivedDate] = ed.OriginalReceivedDate,
		ModifiedDate = GETDATE()
	FROM dbo.blk_EntrantDocument ed
	WHERE 
		ed.ImportPackageId = @packageId AND
		ed.OriginalReceivedDate IS NOT NULL AND 
		[dbo].[Application].[ApplicationGUID] = ed.ParentId;
	--ENABLE TRIGGER trig_Application_CreatedDate ON [Application]; 
	--ENABLE TRIGGER trig_Application_ModifiedDate ON [Application];	 

	/*---------------------------------------------------------*
	 * ApplicationSelectedCompetitiveGroup
	 *---------------------------------------------------------*/
	INSERT INTO dbo.ApplicationSelectedCompetitiveGroup (
		ApplicationID ,
		CompetitiveGroupID ,
		CalculatedBenefitID ,
		CalculatedRating)
	SELECT 
		a.ApplicationID,
		cg.CompetitiveGroupID,
	    ascg.CalculatedBenefitId,
	    ascg.CalculatedRating
	FROM
		dbo.blk_ApplicationSelectedCompetitiveGroup ascg
		JOIN dbo.[Application] a ON ascg.ParentId = a.ApplicationGUID
		JOIN dbo.CompetitiveGroup cg ON ascg.[UID] = cg.[UID] 
	WHERE ascg.ImportPackageId = @packageId
		AND cg.InstitutionID = ascg.InstitutionId		
	
	/*---------------------------------------------------------*
	 * ApplicationSelectedCompetitiveGroupItem
	 *---------------------------------------------------------*/
	INSERT INTO dbo.ApplicationSelectedCompetitiveGroupItem  (
		ApplicationID ,
		CompetitiveGroupItemID)
	SELECT 
		a.ApplicationID,
		cgi.CompetitiveGroupItemID
	FROM
		dbo.blk_ApplicationSelectedCompetitiveGroupItem ascgi
		JOIN dbo.[Application] a ON ascgi.ParentId = a.ApplicationGUID
		JOIN dbo.CompetitiveGroupItem cgi ON ascgi.[UID] = cgi.[UID] 
		JOIN dbo.CompetitiveGroup cg ON cgi.CompetitiveGroupID = cg.CompetitiveGroupID				
	WHERE ascgi.ImportPackageId = @packageId		
		AND cg.InstitutionID = ascgi.InstitutionId	

	/*---------------------------------------------------------*
	 * ApplicationSelectedCompetitiveGroupTarget
	 *---------------------------------------------------------*/
	INSERT INTO dbo.ApplicationSelectedCompetitiveGroupTarget (
		ApplicationID ,
		CompetitiveGroupTargetID ,
		IsForO ,
		IsForOZ ,
		IsForZ)
	SELECT 
		a.ApplicationID,
		cgt.CompetitiveGroupTargetID,
		ascgt.IsForO,
		ascgt.IsForOZ,
		ascgt.IsForZ
	FROM
		dbo.blk_ApplicationSelectedCompetitiveGroupTarget ascgt
		JOIN dbo.[Application] a ON ascgt.ParentId = a.ApplicationGUID
		JOIN dbo.CompetitiveGroupTarget cgt ON cgt.[UID] = ascgt.[TargetOrganizationUID]
	WHERE ascgt.ImportPackageId = @packageId		
		AND cgt.InstitutionID = ascgt.InstitutionId;

	/*---------------------------------------------------------*
	 * EntrantDocument
	 *---------------------------------------------------------*/
	--DISABLE TRIGGER trig_EntrantDocument_CreatedDate ON EntrantDocument; 
	--DISABLE TRIGGER trig_EntrantDocument_ModifiedDate ON EntrantDocument;	 
	
	-- Не размножаем паспорта в БД
	UPDATE dbo.EntrantDocument with (updlock, rowlock)
	SET
		EntrantDocumentGUID = A1.Id,
		EntrantID = A1.EntrantID,
		ModifiedDate = GETDATE()		
	FROM 
		(SELECT DISTINCT
			ed1.EntrantDocumentID,
			ed.Id,
			e.EntrantID
		 FROM 
			blk_EntrantDocument ed  
			JOIN dbo.EntrantDocument ed1 ON 				
				ed1.DocumentTypeID = ed.DocumentTypeId AND
				ed1.DocumentSeries = ed.DocumentSeries AND
				ed1.DocumentNumber = ed.DocumentNumber AND
				ed1.DocumentDate = ed.DocumentDate AND				
				ed1.DocumentOrganization = ed.DocumentOrganization AND
				ed1.[UID] = ed.[UID]			
			JOIN dbo.Entrant e1 ON e1.EntrantID = ed1.EntrantID				
			JOIN dbo.blk_Application a ON a.Id = ed.ParentId
			JOIN dbo.[Application] a_db ON a_db.ApplicationGUID = a.Id		
			JOIN dbo.Entrant e ON e.EntrantID = a_db.EntrantID
		 WHERE ed.DocumentTypeId = 1 AND e1.InstitutionID = ed.InstitutionId
			AND ed.ImportPackageId = @packageId) AS A1			
	WHERE A1.EntrantDocumentID = EntrantDocument.EntrantDocumentID	
		 
	INSERT INTO dbo.EntrantDocument ( 
		EntrantID ,
		DocumentTypeID ,
		DocumentSeries ,
		DocumentNumber ,
		DocumentDate ,
		DocumentOrganization ,
		DocumentSpecificData ,
		AttachmentID ,
		[UID] ,
		EntrantDocumentGUID,
		CreatedDate,
		ModifiedDate)
	OUTPUT INSERTED.EntrantDocumentID INTO @InsertedEntrantDocumentsIds
	SELECT DISTINCT 
		e.EntrantID,
		ed.DocumentTypeId,
		ed.DocumentSeries,
		ed.DocumentNumber,
		ed.DocumentDate,
		ed.DocumentOrganization,
		ed.DocumentSpecificData,
		NULL , -- AttachmentID - int
		ed.[UID],
		ed.Id,
		GETDATE(),
		GETDATE()
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.blk_Application a ON a.Id = ed.ParentId
		JOIN dbo.[Application] a_db ON a_db.ApplicationGUID = a.Id		
		JOIN dbo.Entrant e ON e.EntrantID = a_db.EntrantID
		LEFT JOIN EntrantDocument ed1 ON ed1.EntrantDocumentGUID = ed.Id
	WHERE ed.ImportPackageId = @packageId AND ed1.EntrantDocumentID IS NULL;
	--ENABLE TRIGGER trig_EntrantDocument_CreatedDate ON EntrantDocument; 
	--ENABLE TRIGGER trig_EntrantDocument_ModifiedDate ON EntrantDocument;	 
		
	-- лок
	SELECT @stub = COUNT(*) FROM
	EntrantDocument a with (updlock, rowlock) 
	join @InsertedEntrantDocumentsIds i on i.Id = a.EntrantDocumentID
	SELECT @stub = COUNT(*) FROM	
	EntrantDocument a with (updlock, rowlock)
	--join Entrant e with (updlock, rowlock) on e.IdentityDocumentID = a.EntrantDocumentID 
	join ApplicationEntrantDocument aed with (updlock, rowlock) on a.EntrantDocumentID = aed.EntrantDocumentID
	join @InsertedEntrantDocumentsIds i on i.Id = aed.EntrantDocumentID

	/*---------------------------------------------------------*
	 * ApplicationEntrantDocument
	 *---------------------------------------------------------*/		
	--DISABLE TRIGGER trig_ApplicationEntrantDocument_CreatedDate ON ApplicationEntrantDocument; 
	--DISABLE TRIGGER trig_ApplicationEntrantDocument_ModifiedDate ON ApplicationEntrantDocument;	  
	INSERT INTO dbo.ApplicationEntrantDocument (
		ApplicationID ,
		EntrantDocumentID ,
		AttachedDocumentType ,
		[UID] ,
		OriginalReceivedDate,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		ed_db.EntrantDocumentID,
		NULL, -- AttachedDocumentType - int ???
		NULL, -- UID - varchar(200) ??? почему то всегда NULL
		ed.OriginalReceivedDate,
		GETDATE(),
		GETDATE()
	FROM 
		dbo.EntrantDocument ed_db	
		JOIN dbo.blk_EntrantDocument ed ON ed.Id = ed_db.EntrantDocumentGUID	
		JOIN dbo.[Application] a ON ed.ParentId = a.ApplicationGUID		
	WHERE ed.ImportPackageId = @packageId;
	--ENABLE TRIGGER trig_ApplicationEntrantDocument_CreatedDate ON ApplicationEntrantDocument; 
	--ENABLE TRIGGER trig_ApplicationEntrantDocument_ModifiedDate ON ApplicationEntrantDocument;	  
	
	/*---------------------------------------------------------*
	 * ApplicationEntranceTestDocument
	 *---------------------------------------------------------*/		
	--DISABLE TRIGGER trig_ApplicationEntranceTestDocument_CreatedDate ON ApplicationEntranceTestDocument; 
	--DISABLE TRIGGER trig_ApplicationEntranceTestDocument_ModifiedDate ON ApplicationEntranceTestDocument;		 
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd
		JOIN dbo.blk_EntrantDocument ed ON ed.EntranceTestResultId = aetd.Id
		JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id
		JOIN dbo.[Application] a ON ed.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND
			aetd.SourceId = 3 -- (OlympicDocument)	
		
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		NULL, -- InstitutionEntranceTest нет документа
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd
		JOIN dbo.[Application] a ON aetd.ParentId = a.ApplicationGUID								
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId							
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND
			aetd.SourceId = 2 -- (InstitutionEntranceTest)	
		
	-- для ЕГЭ документа может не быть
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd			
		LEFT JOIN dbo.blk_EntrantDocument ed ON 
			ed.ParentId = aetd.ParentId AND	
			ed.[UID] = aetd.EgeDocumentId AND
			ed.ImportPackageId = @packageId					
		LEFT JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id			
		JOIN dbo.[Application] a ON aetd.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND 
		aetd.SourceId = 1 -- (EgeDocument)
		
	-- для ГИА документ обязателен
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		aetd.EntranceTestTypeId,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		etic.EntranceTestItemID,
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		NULL, -- группу не проставялем
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd			
		JOIN dbo.blk_EntrantDocument ed ON 
			ed.ParentId = aetd.ParentId AND	
			ed.[UID] = aetd.EgeDocumentId AND
			ed.ImportPackageId = @packageId					
		JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id			
		JOIN dbo.[Application] a ON ed.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
		JOIN dbo.EntranceTestItemC etic ON
			etic.CompetitiveGroupID = cg.CompetitiveGroupID
			AND etic.EntranceTestTypeID = aetd.EntranceTestTypeId
			AND (etic.SubjectID = aetd.SubjectID OR 
				 LOWER(etic.SubjectName) = LOWER(aetd.SubjectName))
			AND aetd.SourceId IS NOT NULL		
	WHERE aetd.ImportPackageId = @packageId AND 
		aetd.SourceId = 4 -- (GiaDocument)		
		
	INSERT INTO dbo.ApplicationEntranceTestDocument ( 
		ApplicationID ,
		SubjectID ,
		EntrantDocumentID ,
		EntranceTestTypeID ,
		SourceID ,
		ResultValue ,
		BenefitID ,
		EntranceTestItemID ,
		[UID] ,
		InstitutionDocumentNumber ,
		InstitutionDocumentDate ,
		InstitutionDocumentTypeID ,
		CompetitiveGroupID ,
		HasEge,
		CreatedDate,
		ModifiedDate)
	SELECT DISTINCT 
		a.ApplicationID,
		aetd.SubjectId,
		ed_db.EntrantDocumentID,
		NULL,
		aetd.SourceId,
		aetd.ResultValue,
		aetd.BenefitId,
		NULL, -- для документов Benefit
		aetd.[UID],
		aetd.InstitutionDocumentNumber,
		aetd.InstitutionDocumentDate,
		aetd.InstitutionDocumentTypeId,
		cg.CompetitiveGroupID,
		0, -- HasEge - bit ??? хз что тут ставить			
		GETDATE(),
		GETDATE()
	FROM dbo.blk_ApplicationEntranceTestDocument aetd			
		LEFT JOIN dbo.blk_EntrantDocument ed ON ed.Id = aetd.BenefitEntrantDocumentId 
			AND ed.ImportPackageId = @packageId				
		LEFT JOIN dbo.EntrantDocument ed_db ON ed_db.EntrantDocumentGUID = ed.Id				
		JOIN dbo.[Application] a ON aetd.ParentId = a.ApplicationGUID				
		JOIN dbo.CompetitiveGroup cg ON 
			cg.[UID] = aetd.CompetitiveGroupUID AND
			cg.InstitutionID = aetd.InstitutionId				
	WHERE aetd.ImportPackageId = @packageId AND aetd.EntranceTestTypeId IS NULL;
	--ENABLE TRIGGER trig_ApplicationEntranceTestDocument_CreatedDate ON ApplicationEntranceTestDocument; 
	--ENABLE TRIGGER trig_ApplicationEntranceTestDocument_ModifiedDate ON ApplicationEntranceTestDocument;	
		
	/*---------------------------------------------------------*
	 * EntrantDocumentIdentity
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentIdentity (
		EntrantDocumentID ,
		IdentityDocumentTypeID ,
		GenderTypeID ,
		NationalityTypeID ,
		BirthDate ,
		BirthPlace ,
		SubdivisionCode)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.IdentityDocumentTypeId,
		gt.GenderID,
		ed.NationalityTypeId,
		ed.BirthDate,
		ed.BirthPlace,
		ed.SubdivisionCode
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
		JOIN dbo.Entrant e ON e.EntrantID = ed_db.EntrantID
		JOIN dbo.GenderType gt ON gt.GenderID = gt.GenderID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 1 -- IdentityDocument
	
	-- блокировка --
	--SELECT @stub = COUNT(e.EntrantID)
	--FROM
	--	Entrant e with (updlock, rowlock)
	--	JOIN dbo.EntrantDocument ed_db on e.EntrantID = ed_db.EntrantID		 
	--	JOIN dbo.blk_EntrantDocument ed ON ed.Id = ed_db.EntrantDocumentGUID
	--WHERE 
	--	ed.ImportPackageId = @packageId	
	--	AND ed_db.EntrantDocumentID != e.IdentityDocumentID 
	--	AND ed.DocumentTypeID = 1 -- IdentityDocument	
		
	UPDATE [dbo].[Entrant] with (updlock, rowlock)
	SET [IdentityDocumentID] = ed_db.EntrantDocumentID
	FROM		
		dbo.EntrantDocument ed_db
		JOIN dbo.blk_EntrantDocument ed	ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE
		ed.ImportPackageId = @packageId AND		
		ed_db.EntrantID = [dbo].[Entrant].EntrantID AND
		ed_db.EntrantDocumentID != [dbo].[Entrant].IdentityDocumentID AND
		ed.DocumentTypeID = 1 -- IdentityDocument
		
	/*---------------------------------------------------------*
	 * EntrantDocumentCustom		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentCustom ( 
		EntrantDocumentID ,
		DocumentTypeNameText ,
		AdditionalInfo)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.DocumentTypeNameText,
		ed.AdditionalInfo
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 15 -- CustomDocument = 15

	/*---------------------------------------------------------*
	 * EntrantDocumentDisability		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentDisability (
		EntrantDocumentID ,
		DisabilityTypeID)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.DisabilityTypeId
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 11 -- DisabilityDocument = 11
	
	/*---------------------------------------------------------*
	 * EntrantDocumentEdu		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentEdu (
		EntrantDocumentID ,
		RegistrationNumber ,
		InstitutionName,
		GPA)			
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.RegistrationNumber,
		i.FullName,
		ed.GPA
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
		LEFT JOIN dbo.Institution i ON i.InstitutionID = ed.InstitutionId
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId IN (3, 4, 5, 6, 7, 8, 16)
		
	/*---------------------------------------------------------*
	 * EntrantDocumentEge		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentEge ( 
		EntrantDocumentID ,
		DecisionNumber ,
		Decision ,
		DecisionDate ,
		TypographicNumber)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		NULL,
		NULL,
		NULL,
		NULL -- TypographicNumber ???
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 2 -- EgeDocument = 2

	/*---------------------------------------------------------*
	 * EntrantDocumentOlympic		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentOlympic (
		EntrantDocumentID ,
		DiplomaTypeID ,
		OlympicID)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		ed.DiplomaTypeId,
		ed.OlympicId
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 9 -- OlympicDocument = 9
		
	/*---------------------------------------------------------*
	 * EntrantDocumentOlympicTotal		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentOlympicTotal (
		EntrantDocumentID ,
		DiplomaTypeID ,
		OlympicPlace ,
		OlympicDate)
	SELECT DISTINCT
		ed_db.EntrantDocumentID,
		ed.DiplomaTypeId,
		ed.OlympicPlace,
		ed.OlympicDate
	FROM dbo.blk_EntrantDocument ed
		JOIN dbo.EntrantDocument ed_db ON ed.Id = ed_db.EntrantDocumentGUID
	WHERE 				
		ed.ImportPackageId = @packageId AND
		ed.DocumentTypeId = 10 -- OlympicTotalDocument = 10
		
	/*---------------------------------------------------------*
	 * EntrantDocumentEgeAndOlympicSubject		 
	 *---------------------------------------------------------*/				
	INSERT INTO dbo.EntrantDocumentEgeAndOlympicSubject ( 
		EntrantDocumentID ,
		SubjectID ,
		Value)
	SELECT DISTINCT 
		ed_db.EntrantDocumentID,
		edos.SubjectId,
		edos.Value
	FROM dbo.blk_EntrantDocumentEgeAndOlympicSubject edos
		JOIN dbo.EntrantDocument ed_db ON edos.ParentId = ed_db.EntrantDocumentGUID
	WHERE 				
		edos.ImportPackageId = @packageId

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
	--	'C',		
	--	'[{"ApplicationUID":"' + ISNULL(a.[UID], '') + 
	--	'","ApplicationNumber":"' + a.ApplicationNumber + 
	--	'","ApplicationDate":"\/Date(' + CAST(DATEDIFF(s, '01-01-1970', 
	--		DATEADD(HOUR, DATEDIFF(hour, GETDATE(), GETUTCDATE()), a.RegistrationDate)) AS VARCHAR) + 
	--	'000)\/","ApplicationID":' + CAST(a_db.ApplicationID AS VARCHAR) +
	--	',"EntrantUID":"' + ISNULL(e.[UID], '') + 
	--	'","EntrantDocumentID":' + CAST(e.IdentityDocumentID AS VARCHAR) + 
	--	',"EntrantID":' + CAST(e.EntrantID AS VARCHAR) + '}]',		
	--	NULL,
	--	'Application',
	--	'ImportApplication',		
	--	a.InstitutionID,
	--	@userLogin,
	--	NULL,
	--	GETDATE()	
	--FROM 
	--	dbo.blk_Application a
	--	JOIN dbo.[Application] a_db ON a.Id = a_db.ApplicationGUID
	--	JOIN dbo.Entrant e ON a_db.EntrantID = e.EntrantID
	--WHERE a.ImportPackageId = @packageId
	
	/*---------------------------------------------------------*
	 * Очистка BULK таблиц
	 *---------------------------------------------------------*/		
	EXEC dbo.[blk_DeleteApplicationPackage] @packageId

	/*---------------------------------------------------------*
	 * Возврат результата в код
	 *---------------------------------------------------------*/		
	SELECT
		(SELECT 
			Id AS '@Id' FROM @InsertedApplicationsIds
		 FOR
		 XML PATH('ImportResultItem'), TYPE) AS 'Successful',
		(SELECT 	
			[UID] AS '@UID'			
			FROM @FailedApplicationsUIDs
		 FOR
		 XML PATH('ImportResultItem'), TYPE) AS 'Failed'
	FOR XML PATH(''),
	ROOT('ImportResult')	
	
	SET NOCOUNT OFF
