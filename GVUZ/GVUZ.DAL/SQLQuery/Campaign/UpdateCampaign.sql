DECLARE @Campaigns TABLE (
		CampaignID INT NULL
	); -- успешно сохраненная кампания

MERGE Campaign AS TARGET
USING (SELECT @CampaignID as CampaignID) AS SOURCE
ON TARGET.CampaignID = SOURCE.CampaignID
WHEN MATCHED THEN
	UPDATE 
	SET 
      TARGET.[Name] = @Name
      ,TARGET.[YearStart] = @YearStart
      ,TARGET.[YearEnd] = @YearEnd
      ,TARGET.[EducationFormFlag] = @EducationFormFlag
      ,TARGET.[StatusID] = @StatusID
      ,TARGET.[UID] = @UID
      ,TARGET.[ModifiedDate] = GETDATE()
      ,TARGET.[CampaignTypeID] = @CampaignTypeID
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([InstitutionID]
           ,[Name]
           ,[YearStart]
           ,[YearEnd]
           ,[EducationFormFlag]
           ,[StatusID]
           ,[UID]
           ,[CreatedDate]
		   ,[ModifiedDate]
           ,[CampaignTypeID])
     VALUES
           (@InstitutionID
           ,@Name
           ,@YearStart
           ,@YearEnd
           ,@EducationFormFlag
           ,@StatusID
           ,@UID
           ,GETDATE()
		   ,GETDATE()
           ,@CampaignTypeID)
OUTPUT INSERTED.CampaignID INTO @Campaigns;

DELETE FROM CampaignEducationLevel WHERE CampaignID in (SELECT CampaignID FROM @Campaigns);
SELECT CampaignID FROM @Campaigns;