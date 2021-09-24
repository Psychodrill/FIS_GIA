/* Для отладки */
--Declare @CompetitiveGroupID int = 11;
--Declare @InstitutionID int = 587;
--Declare @Name varchar(200) = 'name2';
--Declare @UID varchar(200) = 'name2';
--Declare @CampaignID int = 46;
--Declare @IsFromKrym bit = 0;
--Declare @IsAdditional bit = 0;
--Declare @EducationFormID int = 11;
--Declare @EducationSourceID int = 16;
--Declare @EducationLevelID int = 2;
--Declare @DirectionID int = 55;

--Declare @NumberBudgetO int = 0;
--Declare @NumberBudgetOZ int = 0;
--Declare @NumberBudgetZ int = 0;

--Declare @NumberPaidO int = 0;
--Declare @NumberPaidOZ int = 0;
--Declare @NumberPaidZ int = 0;

--Declare @NumberQuotaO int = 0;
--Declare @NumberQuotaOZ int = 0;
--Declare @NumberQuotaZ int = 0;

--Declare @NumberTargetO int = 0;
--Declare @NumberTargetOZ int = 0;
--Declare @NumberTargetZ int = 0;

--Declare @IdLevelBudget int = 1;


DECLARE @CompetitiveGroups TABLE (
		CompetitiveGroupID INT NULL
	); -- успешно сохраненный конкурс

MERGE CompetitiveGroup AS TARGET
USING (SELECT @CompetitiveGroupID as CompetitiveGroupID) AS SOURCE
ON TARGET.CompetitiveGroupID = SOURCE.CompetitiveGroupID
WHEN MATCHED THEN
	UPDATE 
	SET 
      TARGET.[Name] = @Name
      ,TARGET.[Course] = 1
	  ,TARGET.[UID] = @UID
      ,TARGET.[CampaignID] = @CampaignID
	  ,TARGET.[IsFromKrym] = @IsFromKrym
	  ,TARGET.[IsAdditional] = @IsAdditional
      ,TARGET.[EducationFormID] = @EducationFormID
	  ,TARGET.[EducationSourceID] = @EducationSourceID
	  ,TARGET.[EducationLevelID] = @EducationLevelID
	  ,TARGET.[DirectionID] = @DirectionID
      ,TARGET.[ParentDirectionID] = @ParentDirectionID
	  ,TARGET.[IdLevelBudget] = @IdLevelBudget
      ,TARGET.[ModifiedDate] = GETDATE()
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([InstitutionID]
           ,[Name]
           ,[Course]
           ,[UID]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[CampaignID]
           ,[CompetitiveGroupGUID]
           ,[IsFromKrym]
           ,[IsAdditional]
           ,[EducationFormId]
           ,[EducationSourceId]
           ,[EducationLevelID]
           ,[DirectionID]
		   ,[IdLevelBudget]
           ,[ParentDirectionID])
     VALUES
           (@InstitutionID
           ,@Name
		   ,1
		   ,@UID
		   ,GETDATE()
		   ,GETDATE()
		   ,@CampaignID
		   ,null
           ,@IsFromKrym
		   ,@IsAdditional
		   ,@EducationFormID
		   ,@EducationSourceID
		   ,@EducationLevelID
		   ,@DirectionID
		   ,@IdLevelBudget
           ,@ParentDirectionID
			)
OUTPUT INSERTED.CompetitiveGroupID INTO @CompetitiveGroups;

Update acgi
set [CompetitiveGroupItemId] = null
From [ApplicationCompetitiveGroupItem] acgi
inner join CompetitiveGroupItem cgi ON cgi.CompetitiveGroupItemID = acgi.CompetitiveGroupItemId
inner join @CompetitiveGroups cg on cg.CompetitiveGroupID = cgi.CompetitiveGroupID;


DELETE FROM CompetitiveGroupItem WHERE CompetitiveGroupID in (SELECT CompetitiveGroupID FROM @CompetitiveGroups);
INSERT INTO [dbo].[CompetitiveGroupItem]
           ([CompetitiveGroupID]
           ,[NumberBudgetO]
           ,[NumberBudgetOZ]
           ,[NumberBudgetZ]
           ,[NumberPaidO]
           ,[NumberPaidOZ]
           ,[NumberPaidZ]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[NumberQuotaO]
           ,[NumberQuotaOZ]
           ,[NumberQuotaZ]
		   ,[NumberTargetO]
           ,[NumberTargetOZ]
           ,[NumberTargetZ])
SELECT 
	CompetitiveGroupID 
    ,@NumberBudgetO
    ,@NumberBudgetOZ
    ,@NumberBudgetZ
    ,@NumberPaidO
    ,@NumberPaidOZ
    ,@NumberPaidZ
    ,GETDATE()
    ,GETDATE()
    ,@NumberQuotaO
    ,@NumberQuotaOZ
    ,@NumberQuotaZ
	,@NumberTargetO
    ,@NumberTargetOZ
    ,@NumberTargetZ
FROM @CompetitiveGroups;

DELETE FROM CompetitiveGroupProgram WHERE CompetitiveGroupID in (SELECT CompetitiveGroupID FROM @CompetitiveGroups);
DELETE FROM CompetitiveGroupTargetItem WHERE CompetitiveGroupID in (SELECT CompetitiveGroupID FROM @CompetitiveGroups);

SELECT CompetitiveGroupID FROM @CompetitiveGroups;