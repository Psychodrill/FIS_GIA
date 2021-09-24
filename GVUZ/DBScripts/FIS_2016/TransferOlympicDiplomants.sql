RAISERROR ('ПРОВЕРЬ НАЗВАНИЯ БД ФБС И ФИС!', 16, 1)
RETURN

DECLARE @message varchar(max)
SET @message = '===== Обработка начата'
RAISERROR (@message, 10, 1) WITH NOWAIT

DECLARE @counter int
SET @counter = 0

DECLARE @fbs_id bigint
DECLARE @fbs_olympics_id bigint
DECLARE @fbs_last_name varchar(64)
DECLARE @fbs_first_name varchar(64)
DECLARE @fbs_middle_name varchar(64)
DECLARE @fbs_birth_date date
DECLARE @fbs_school_region varchar(32)
DECLARE @fbs_school_ege_code bigint
DECLARE @fbs_school_ege_name varchar(512)
DECLARE @fbs_form_number int
DECLARE @fbs_reg_code varchar(32)
DECLARE @fbs_result_level int
DECLARE @fbs_egeid varchar(1024)

DECLARE @fbs_olympiad_name varchar(255) 
DECLARE @fbs_olympiad_subject_name varchar(4000) 
DECLARE @fbs_olympiad_subject_profile_name varchar(4000) 
DECLARE @fbs_olympiad_year int

DECLARE @fbs_PersonId int
DECLARE @fbs_UseYear int
DECLARE @fbs_DocumentTypeCode int
DECLARE @fbs_DocumentSeries varchar(9)
DECLARE @fbs_DocumentNumber varchar(10)

DECLARE @gvuz_OlympicTypeProfileID int
DECLARE @gvuz_ResultLevelID smallint
DECLARE @gvuz_IdentityDocumentTypeID int
DECLARE @gvuz_OlympicDiplomantID bigint
DECLARE @gvuz_OlympicDiplomantDocumentID bigint

DECLARE @fbsOlympicDiplomantsCursor CURSOR
SET @fbsOlympicDiplomantsCursor = CURSOR FOR  
SELECT
	fbs_od.[id]
	,fbs_od.[olympics_id]
	,ISNULL(fbs_od.[last_name],fbs_p.[Surname]) as [last_name]
	,ISNULL(fbs_od.[first_name],fbs_p.[Name]) as [first_name]
	,fbs_od.[middle_name]
	,fbs_od.[birth_date]
	,ISNULL(fbs_od.[school_region],1000) as [school_region]
	,fbs_od.[school_ege_code]
	,fbs_od.[school_ege_name]
	,fbs_od.[form_number]
	,fbs_od.[reg_code]
	,fbs_od.[result_level]
	,fbs_od.[egeid]
	
	,fbs_o.[olympiad_name]
	,fbs_o.[olympiad_subject_name]
	,fbs_o.[olympiad_subject_profile_name]
	,fbs_o.[olympiad_year]
	
	,fbs_p.[PersonId]
	,fbs_p.[UseYear]
	,fbs_p.[DocumentTypeCode]
	,fbs_p.[DocumentSeries]
	,fbs_p.[DocumentNumber]
FROM
	[FBS].[dbo].[OlympicDiplomants]	fbs_od
	INNER JOIN [FBS].[dbo].[Olympics] fbs_o
		ON fbs_od.olympics_id = fbs_o.id
	LEFT JOIN [FBS].[dbo].[OlympicParticipants]	fbs_op 
		ON fbs_od.id = fbs_op.diplomant_id
	LEFT JOIN [FBS].[rbd].[Participants] fbs_p 
		ON fbs_op.participant_id = fbs_p.ParticipantID
WHERE
	(fbs_p.UseYear IN (2014,2015) OR fbs_p.UseYear IS NULL)	
	AND ISNULL(ISNULL(fbs_od.[first_name],fbs_p.[Name]),'') <> ''
	AND ISNULL(ISNULL(fbs_od.[last_name],fbs_p.[Surname]),'') <> ''
ORDER BY 
	fbs_p.[PersonId]
	
OPEN @fbsOlympicDiplomantsCursor
FETCH NEXT FROM @fbsOlympicDiplomantsCursor 
INTO 
	@fbs_id 
	,@fbs_olympics_id 
	,@fbs_last_name 
	,@fbs_first_name 
	,@fbs_middle_name 
	,@fbs_birth_date
	,@fbs_school_region 
	,@fbs_school_ege_code 
	,@fbs_school_ege_name 
	,@fbs_form_number 
	,@fbs_reg_code 
	,@fbs_result_level 
	,@fbs_egeid 

	,@fbs_olympiad_name
	,@fbs_olympiad_subject_name  
	,@fbs_olympiad_subject_profile_name  
	,@fbs_olympiad_year 

	,@fbs_PersonId 
	,@fbs_UseYear 	
	,@fbs_DocumentTypeCode
	,@fbs_DocumentSeries
	,@fbs_DocumentNumber	
	
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @counter = @counter + 1
	IF (@counter % 1000 = 0)
	BEGIN
		SET @message = '===== Обработано '+CONVERT(varchar(20),@counter)+' записей'		
		RAISERROR (@message, 10, 1) WITH NOWAIT
	END
	
	SET @gvuz_OlympicTypeProfileID = NULL
	
	SELECT 
		@gvuz_OlympicTypeProfileID = gvuz_otp.[OlympicTypeProfileID]
	FROM
		[GVUZ_Develop].[dbo].[OlympicTypeProfile] gvuz_otp
	WHERE
		(gvuz_otp.[OlympicProfileID] IN
			(SELECT 
				[OlympicProfileID] 
			FROM 
				[GVUZ_Develop].[dbo].[OlympicProfile] gvuz_op 
			WHERE
				gvuz_op.ProfileName = @fbs_olympiad_subject_name)
		)
		AND (gvuz_otp.[OlympicTypeID] IN
			(SELECT 
				[OlympicID]
			FROM 
				[GVUZ_Develop].[dbo].[OlympicType] gvuz_ot
			WHERE
				gvuz_ot.[OlympicYear] = @fbs_olympiad_year
				AND gvuz_ot.[Name] = @fbs_olympiad_name)
		)		
	IF (@gvuz_OlympicTypeProfileID IS NULL)
	BEGIN
		SET @message = '== Не удалось найти олимпиаду '+
		'(@fbs_olympiad_subject_name='+ISNULL(@fbs_olympiad_subject_name,'')+
		',@fbs_olympiad_year='+ISNULL(CONVERT(varchar(10),@fbs_olympiad_year),'')+
		',@@fbs_olympiad_name='+ISNULL(@fbs_olympiad_name,'')+')'
		RAISERROR (@message, 10, 1) WITH NOWAIT 
	END
	ELSE
	BEGIN
		SET @gvuz_IdentityDocumentTypeID = NULL
		SET @gvuz_OlympicDiplomantID = NULL
		SET @gvuz_OlympicDiplomantDocumentID = NULL
		
		SELECT 
			@gvuz_IdentityDocumentTypeID = trans.IdentityDocumentTypeID
		FROM	
			[gvuz_develop_2016].[dbo].[Translation_RVIDT_IdentityDT] trans
		WHERE
			trans.DocumentTypeCode = @fbs_DocumentTypeCode
		
		IF(@gvuz_IdentityDocumentTypeID IS NULL)	
		BEGIN
			SET @gvuz_IdentityDocumentTypeID = 9
		END
			
		
		SELECT 
			@gvuz_OlympicDiplomantID = [OlympicDiplomantID]
		FROM
			[GVUZ_Develop].[dbo].[OlympicDiplomant] gvuz_od
		WHERE
			gvuz_od.[OlympicTypeProfileID] = @gvuz_OlympicTypeProfileID
			AND gvuz_od.[PersonId] = @fbs_PersonId
			AND gvuz_od.[DiplomaNumber] = @fbs_reg_code
			
		IF(@gvuz_OlympicDiplomantID IS NULL)
		BEGIN
			SET @message = '== Будет создана новая запись OlympicDiplomant '+
			'(OlympicTypeProfileID='+ISNULL(CONVERT(nvarchar(20),@gvuz_OlympicTypeProfileID),'')+
			',PersonId='+ISNULL(CONVERT(nvarchar(20),@fbs_PersonId),'')+
			',DiplomaNumber='+ISNULL(@fbs_reg_code,'')+')'
			RAISERROR (@message, 10, 1) WITH NOWAIT 		
		 
			INSERT INTO [GVUZ_Develop].[dbo].[OlympicDiplomant]
					   ([OlympicTypeProfileID]
					   ,[OlympicDiplomantIdentityDocumentID]
					   ,[SchoolRegionID]
					   ,[SchoolEgeCode]
					   ,[SchoolEgeName]
					   ,[FormNumber]
					   ,[DiplomaSeries]
					   ,[DiplomaNumber]
					   ,[DiplomaDateIssue]
					   ,[ResultLevelID]
					   ,[CreateDate]
					   ,[ModifiedDate]
					   ,[DeleteDate]
					   ,[Comment]
					   ,[StatusID]
					   ,[AdoptionUnfoundedComment]
					   ,[PersonId]
					   ,[PersonLinkDate]
					   ,[EndingDate])
				 VALUES
					   (@gvuz_OlympicTypeProfileID
					   ,NULL
					   ,@fbs_school_region
					   ,@fbs_school_ege_code
					   ,@fbs_school_ege_name
					   ,@fbs_form_number
					   ,NULL
					   ,@fbs_reg_code
					   ,NULL
					   ,CASE WHEN @fbs_result_level = 1 THEN 1 ELSE 2 END
					   ,GETDATE()
					   ,GETDATE()
					   ,NULL
					   ,'Запись создана из БД ФБС'
					   ,CASE WHEN @fbs_PersonId IS NULL THEN 4 ELSE 1 END
					   ,NULL
					   ,@fbs_PersonId
					   ,CASE WHEN @fbs_PersonId IS NULL THEN NULL ELSE GETDATE() END
					   ,@fbs_UseYear)	
					   
			SELECT @gvuz_OlympicDiplomantID = @@IDENTITY		   
			
			INSERT INTO [GVUZ_Develop].[dbo].[OlympicDiplomantDocument]
					   ([OlympicDiplomantID]
					   ,[BirthDate]
					   ,[IdentityDocumentTypeID]
					   ,[DocumentSeries]
					   ,[DocumentNumber]
					   ,[OrganizationIssue]
					   ,[DateIssue]
					   ,[LastName]
					   ,[FirstName]
					   ,[MiddleName])
				 VALUES
					   (@gvuz_OlympicDiplomantID
					   ,@fbs_birth_date
					   ,@gvuz_IdentityDocumentTypeID
					   ,@fbs_DocumentSeries
					   ,@fbs_DocumentNumber
					   ,NULL
					   ,NULL
					   ,@fbs_last_name
					   ,@fbs_first_name
					   ,@fbs_middle_name)
					   
			SELECT @gvuz_OlympicDiplomantDocumentID = @@IDENTITY
			
			UPDATE 
				[GVUZ_Develop].[dbo].[OlympicDiplomant] 
			SET 
				[OlympicDiplomantIdentityDocumentID] = @gvuz_OlympicDiplomantDocumentID
			WHERE
				[OlympicDiplomantID] = @gvuz_OlympicDiplomantID			
		END
		ELSE -- NOT (@gvuz_OlympicDiplomantID IS NULL)
		BEGIN
			SET @message = '== Будет обновлена существующая запись OlympicDiplomant '+
			'(OlympicDiplomantID='+CONVERT(varchar(20),@gvuz_OlympicDiplomantID)+
			',OlympicTypeProfileID='+ISNULL(CONVERT(nvarchar(20),@gvuz_OlympicTypeProfileID),'')+
			',PersonId='+ISNULL(CONVERT(nvarchar(20),@fbs_PersonId),'')+
			',DiplomaNumber='+ISNULL(@fbs_reg_code,'')+')'
			RAISERROR (@message, 10, 1) WITH NOWAIT  
			
			SELECT 
				@gvuz_OlympicDiplomantDocumentID = gvuz_odd.[OlympicDiplomantDocumentID]
			FROM
				[GVUZ_Develop].[dbo].[OlympicDiplomantDocument] gvuz_odd
			WHERE
				gvuz_odd.[OlympicDiplomantID] = @gvuz_OlympicDiplomantID
				AND gvuz_odd.[DocumentNumber] = @fbs_DocumentNumber
				AND gvuz_odd.[DocumentSeries] = @fbs_DocumentSeries
				
			IF(@gvuz_OlympicDiplomantDocumentID IS NULL)
			BEGIN
				SET @message = 'Будет создана запись OlympicDiplomantDocument '+
				'(DocumentSeries='+ISNULL(@fbs_DocumentSeries,'')+
				',DocumentNumber='+ISNULL(@fbs_DocumentNumber,'')+')'	
				RAISERROR (@message, 10, 1) WITH NOWAIT  			
				 			
				INSERT INTO [GVUZ_Develop].[dbo].[OlympicDiplomantDocument]
						   ([OlympicDiplomantID]
						   ,[BirthDate]
						   ,[IdentityDocumentTypeID]
						   ,[DocumentSeries]
						   ,[DocumentNumber]
						   ,[OrganizationIssue]
						   ,[DateIssue]
						   ,[LastName]
						   ,[FirstName]
						   ,[MiddleName])
					 VALUES
						   (@gvuz_OlympicDiplomantID
						   ,@fbs_birth_date
						   ,@gvuz_IdentityDocumentTypeID
						   ,@fbs_DocumentSeries
						   ,@fbs_DocumentNumber
						   ,NULL
						   ,NULL
						   ,@fbs_last_name
						   ,@fbs_first_name
						   ,@fbs_middle_name)
						   
				SELECT @gvuz_OlympicDiplomantDocumentID = @@IDENTITY
			
				UPDATE
					[GVUZ_Develop].[dbo].[OlympicDiplomant]
				SET 
					[Comment] = ISNULL([Comment],'Запись обновлена из БД ФБС')				
				WHERE
					[OlympicDiplomantID] = @gvuz_OlympicDiplomantID
			END
			ELSE
			BEGIN
				SET @message = 'Обновление не требуется'	
				RAISERROR (@message, 10, 1) WITH NOWAIT  
			END 
		END
	END

	FETCH NEXT FROM @fbsOlympicDiplomantsCursor 
	INTO 
		@fbs_id 
		,@fbs_olympics_id 
		,@fbs_last_name 
		,@fbs_first_name 
		,@fbs_middle_name 
		,@fbs_birth_date
		,@fbs_school_region 
		,@fbs_school_ege_code 
		,@fbs_school_ege_name 
		,@fbs_form_number 
		,@fbs_reg_code 
		,@fbs_result_level 
		,@fbs_egeid 

		,@fbs_olympiad_name
		,@fbs_olympiad_subject_name  
		,@fbs_olympiad_subject_profile_name  
		,@fbs_olympiad_year 

		,@fbs_PersonId 
		,@fbs_UseYear	
		,@fbs_DocumentTypeCode
		,@fbs_DocumentSeries
		,@fbs_DocumentNumber
END	--WHILE

SET @message = '===== Обработано '+CONVERT(varchar(20),@counter)+' записей'
RAISERROR (@message, 10, 1) WITH NOWAIT 
SET @message = '===== Обработка завершена'
RAISERROR (@message, 10, 1) WITH NOWAIT