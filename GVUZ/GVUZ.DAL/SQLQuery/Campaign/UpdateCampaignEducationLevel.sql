INSERT INTO [dbo].[CampaignEducationLevel]
           ([CampaignID]
           ,[Course]
           ,[EducationLevelID]
           ,[PresentInLicense])
     VALUES
           (@CampaignID
           ,@Course
           ,@EducationLevelID
           ,null --@PresentInLicense
		   );

