INSERT INTO [gvuz_develop_2016].[dbo].[AdmissionVolume]
           ([InstitutionID]
           ,[AdmissionItemTypeID]
           ,[DirectionID]
           ,[NumberBudgetO]
           ,[NumberBudgetOZ]
           ,[NumberBudgetZ]
           ,[NumberPaidO]
           ,[NumberPaidOZ]
           ,[NumberPaidZ]
           ,[UID]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[Course]
           ,[CampaignID]
           ,[NumberTargetO]
           ,[NumberTargetOZ]
           ,[NumberTargetZ]
           ,[NumberQuotaO]
           ,[NumberQuotaOZ]
           ,[NumberQuotaZ]
           ,[AdmissionVolumeGUID])
     VALUES
           (<InstitutionID, int,>
           ,<AdmissionItemTypeID, smallint,>
           ,<DirectionID, int,>
           ,<NumberBudgetO, int,>
           ,<NumberBudgetOZ, int,>
           ,<NumberBudgetZ, int,>
           ,<NumberPaidO, int,>
           ,<NumberPaidOZ, int,>
           ,<NumberPaidZ, int,>
           ,<UID, varchar(200),>
           ,<CreatedDate, datetime,>
           ,<ModifiedDate, datetime,>
           ,<Course, int,>
           ,<CampaignID, int,>
           ,<NumberTargetO, int,>
           ,<NumberTargetOZ, int,>
           ,<NumberTargetZ, int,>
           ,<NumberQuotaO, int,>
           ,<NumberQuotaOZ, int,>
           ,<NumberQuotaZ, int,>
           ,<AdmissionVolumeGUID, uniqueidentifier,>)
GO


