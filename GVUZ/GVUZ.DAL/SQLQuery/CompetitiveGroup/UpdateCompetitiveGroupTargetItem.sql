INSERT INTO [dbo].[CompetitiveGroupTargetItem]
           ([CompetitiveGroupTargetID]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[NumberTargetO]
           ,[NumberTargetOZ]
           ,[NumberTargetZ]
           ,[CompetitiveGroupID])
     VALUES
           (@CompetitiveGroupTargetID
           ,GETDATE()
           ,GETDATE()
           ,@NumberTargetO
           ,@NumberTargetOZ
           ,@NumberTargetZ
           ,@CompetitiveGroupID);