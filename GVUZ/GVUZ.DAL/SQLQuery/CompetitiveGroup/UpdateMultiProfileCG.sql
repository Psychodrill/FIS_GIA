/* Для отладки */
--Declare @CompetitiveGroupID int = 11;
--Declare @InstitutionID int = 587;
--Declare @Name varchar(200) = 'name2';
--Declare @UID varchar(200) = 'name2';
--Declare @CampaignID int = 46;
--Declare @EducationFormID int = 11;
--Declare @EducationSourceID int = 16;
--Declare @EducationLevelID int = 2;
--Declare @DirectionID int = 55;

--Declare @IdLevelBudget int = 1;


DECLARE @MultiProfileCompetitions TABLE (
		MultiProfileCompetitionID INT NULL
	); -- успешно сохраненный конкурс

MERGE MultiProfileCompetition AS TARGET
USING (SELECT @CompetitiveGroupID as MultiProfileCompetitionID) AS SOURCE
ON TARGET.MultiProfileCompetitionID = SOURCE.MultiProfileCompetitionID
WHEN MATCHED THEN
	UPDATE 
	SET 
      TARGET.[Name] = @Name
      ,TARGET.[Course] = 1
	  ,TARGET.[UID] = @UID
      ,TARGET.[CampaignID] = @CampaignID
      ,TARGET.[EducationFormID] = @EducationFormID
	  ,TARGET.[EducationSourceID] = @EducationSourceID
	  ,TARGET.[EducationLevelID] = @EducationLevelID
	  ,TARGET.[DirectionID] = @DirectionID
	  ,TARGET.[IdLevelBudget] = @IdLevelBudget
      ,TARGET.[ModifiedDate] = GETDATE()
	  ,TARGET.[QuotaVolume] = @QuotaVolume
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([InstitutionID]
           ,[Name]
           ,[Course]
           ,[UID]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[CampaignID]
           ,[EducationFormId]
           ,[EducationSourceId]
           ,[EducationLevelID]
           ,[DirectionID]
		   ,[IdLevelBudget]
		   ,[QuotaVolume])
     VALUES
           (@InstitutionID
           ,@Name
		   ,1
		   ,@UID
		   ,GETDATE()
		   ,GETDATE()
		   ,@CampaignID
		   ,@EducationFormID
		   ,@EducationSourceID
		   ,@EducationLevelID
		   ,@DirectionID
		   ,@IdLevelBudget
		   ,@QuotaVolume
			)
OUTPUT INSERTED.MultiProfileCompetitionID INTO @MultiProfileCompetitions;


--DELETE FROM CompetitiveGroupProgram WHERE CompetitiveGroupID in (SELECT CompetitiveGroupID FROM @MultiProfileCompetitions);
--DELETE FROM CompetitiveGroupTargetItem WHERE CompetitiveGroupID in (SELECT CompetitiveGroupID FROM @MultiProfileCompetitions);

SELECT MultiProfileCompetitionID FROM @MultiProfileCompetitions;