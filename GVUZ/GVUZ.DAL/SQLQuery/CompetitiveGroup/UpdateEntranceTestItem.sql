-- Отладочный пример
--DECLARE @CompetitiveGroupID INT = 8; 
--DECLARE @EntranceTests EntranceTests; 
--DECLARE @BenefitItems BenefitItems;

---- BenefitItemProfile
--DECLARE @BenefitItemProfiles BenefitItemProfiles;

---- BenefitItemSubject
--DECLARE @BenefitItemSubjects BenefitItemSubjects;

---- BenefitItemOlympic 
--DECLARE @BenefitItemOlympics BenefitItemOlympics;

---- BenefitItemOlympicProfile
--DECLARE @BenefitItemOlympicProfiles BenefitItemProfiles;

--insert into @EntranceTests (ItemID
--	,TestName
--	,TestType
--	,UID
--	,Value
--	,EntranceTestPriority
--	,IsForSPOandVO
--	,ReplacedEntranceTestItemID
--	,GUID)
--Select 32, 'Русский язык', 1, null, 50, 1, 0, 0, NewID() union
--Select -2, 'Местный язык', 2, 'лангедок', 75, 2, 1, 32, NewID() 


--insert into @BenefitItems ([ItemID],
--	[EntranceTestItemID],
--	[OlympicDiplomTypeID],
--	[OlympicLevelFlags],
--	[BenefitID],
--	[IsForAllOlympic],
--	[CompetitiveGroupID],
--	[UID],
--	[OlympicYear],
--	[EgeMinValue],
--	[BenefitItemGUID],
--	[ClassFlags],
--  [IsCreative],
--  [IsAthletic])
--Select -1, -2, 2, 7, 3, 1, @CompetitiveGroupID, 'UID1', 2016, 55, NewID(), 30 union
--Select -2, -2, 3, 3, 3, 0, @CompetitiveGroupID, 'UID2', 2016, 65, NewID(), 7 union
--Select -3, 0, 1, 255, 1, 0, @CompetitiveGroupID, 'UID3', 2016, 75, NewID(), 255 union
--select  1, 0, 3, 255, 1, 0, @CompetitiveGroupID, 'вфыаыв', 2015, 50, newID(), 255 union
--select  2, 0, 3, 7, 1, 1, @CompetitiveGroupID, null, 2015, 99, newID(), 255 

--insert into @BenefitItemProfiles ([BenefitItemTempID], OlympicProfileID)
--select 1, 255 union
--select 2, 2 union
--select -1, 3

--insert into @BenefitItemSubjects([BenefitItemTempID], SubjectId, EgeMinValue)
--select 1, 1, 70 union
--select 1, 2, 99 union
--select -3, 3, 33

--insert into @BenefitItemOlympics([ID], [BenefitItemTempID], [OlympicTypeID], [OlympicLevel], [ClassFlags], [OlympicLevelFlags], [GUID])
--select 1, 1, 443, 5, 7, 5, newID() union
--select -1, 1, 444, 255, 255, 255, newID() union
--select -2, -3, 445, 5, 7, 5, newID()

--insert into @BenefitItemOlympicProfiles ([BenefitItemTempID], OlympicProfileID) 
--select 1, 255 union
--select -2, 2 union
--select -2, 3


-- ===
declare @ids Table(
	EntranceTestItemID int not null,
	guid uniqueidentifier null,
	ItemID int null
);

declare @benefitItemIds Table(
	BenefitItemID int not null,
	guid uniqueidentifier null,
	ItemID int null
);

declare @BenefitItemOlympicsIds Table(
	ID int not null,
	guid uniqueidentifier null,
	ItemID int null
);


-- 1. Удалить все записи, которых уже нет
delete bip
from BenefitItemCProfile bip
inner join BenefitItemC bic on bic.BenefitItemID = bip.BenefitItemID
Where bic.CompetitiveGroupID = @CompetitiveGroupID;

delete bis
from BenefitItemSubject bis
inner join BenefitItemC bic on bic.BenefitItemID = bis.BenefitItemID
Where bic.CompetitiveGroupID = @CompetitiveGroupID;

delete biotp
From BenefitItemCOlympicTypeProfile biotp
inner join BenefitItemCOlympicType biot on biot.ID = biotp.BenefitItemCOlympicTypeID
inner join BenefitItemC bic on bic.BenefitItemID = biot.BenefitItemID
Where bic.CompetitiveGroupID = @CompetitiveGroupID;

delete biot
From BenefitItemCOlympicType biot
inner join BenefitItemC bic on bic.BenefitItemID = biot.BenefitItemID
Where bic.CompetitiveGroupID = @CompetitiveGroupID;

delete from BenefitItemC
Where [CompetitiveGroupID]=@CompetitiveGroupID 
AND [BenefitItemID] not in (Select ItemID From @BenefitItems);

delete aetd
from [dbo].[ApplicationEntranceTestDocument] aetd
inner join [dbo].[EntranceTestItemC] etic on etic.EntranceTestItemID = aetd.EntranceTestItemID
inner join [dbo].[Application] a on aetd.ApplicationID = a.ApplicationID
Where (
	aetd.[EntranceTestItemID] not in (Select ItemID From @EntranceTests) AND
	etic.[CompetitiveGroupID] = @CompetitiveGroupID
)
AND a.StatusID != 6;

delete from [dbo].[EntranceTestItemC]
where [CompetitiveGroupID]=@CompetitiveGroupID 
AND [EntranceTestItemID] not in (Select ItemID From @EntranceTests);

-- 2. Записываем НЕ заменяющие ВИ
MERGE EntranceTestItemC AS TARGET
USING (
	SELECT et.*
	,s.SubjectID
	,case when s.SubjectID is null then et.TestName else null end as 'SubjectName'
	FROM @EntranceTests et
	left join Subject s on et.TestName = s.Name
	WHERE ReplacedEntranceTestItemID = 0
) AS SOURCE
ON TARGET.[EntranceTestItemID] = SOURCE.ItemID
WHEN MATCHED THEN
	UPDATE 
	SET 
		TARGET.[EntranceTestTypeID] = SOURCE.TestType
		,TARGET.[MinScore] = SOURCE.Value
		,TARGET.SubjectID = SOURCE.SubjectID
		,TARGET.SubjectName = SOURCE.SubjectName
		,TARGET.[UID] = SOURCE.UID
		,TARGET.[EntranceTestPriority] = SOURCE.EntranceTestPriority
		,TARGET.[IsForSPOandVO] = SOURCE.IsForSPOandVO
		,TARGET.[ReplacedEntranceTestItemID] = NULL
		,TARGET.[EntranceTestItemGUID] = SOURCE.GUID
		,TARGET.[IsFirst] = SOURCE.IsFirst
		,TARGET.[IsSecond] = SOURCE.IsSecond
WHEN NOT MATCHED BY TARGET THEN 
	INSERT 
		([CompetitiveGroupID]
        ,[EntranceTestTypeID]
        ,[MinScore]
        ,[SubjectID]
        ,[SubjectName]
        ,[UID]
        ,[CreatedDate]
        ,[ModifiedDate]
        ,[EntranceTestPriority]
        ,[EntranceTestItemGUID]
        ,[IsForSPOandVO]
        ,[ReplacedEntranceTestItemID]
		,[IsFirst]
		,[IsSecond])
	VALUES
		(@CompetitiveGroupID
		,SOURCE.TestType
		,SOURCE.Value
		,SOURCE.SubjectID
		,SOURCE.SubjectName
		,SOURCE.UID
		,GETDATE()
		,GETDATE()
		,SOURCE.EntranceTestPriority
		,SOURCE.GUID
		,SOURCE.IsForSPOandVO
		,NULL --SOURCE.ReplacedEntranceTestItemID
		,SOURCE.IsFirst
		,SOURCE.IsSecond)
	OUTPUT INSERTED.EntranceTestItemID, INSERTED.[EntranceTestItemGUID], NULL INTO @ids;



UPDATE IDS
SET 
	ItemID = et.ItemID
FROM @ids IDS
left join @EntranceTests et on et.GUID = ids.guid;

-- 3. Заменяющие ВИ (позже, потому что у них ссылки на тех, кого заменяют)
MERGE EntranceTestItemC AS TARGET
USING (
	SELECT et.*
	,s.SubjectID
	,case when s.SubjectID is null then et.TestName else null end as 'SubjectName'
	,ids.EntranceTestItemID as 'ReplacedItemID'
	FROM @EntranceTests et
	left join Subject s on et.TestName = s.Name
	left join @ids ids on ids.ItemID = et.ReplacedEntranceTestItemID
	WHERE ReplacedEntranceTestItemID != 0
) AS SOURCE
ON TARGET.[EntranceTestItemID] = SOURCE.ItemID
WHEN MATCHED THEN
	UPDATE 
	SET 
		TARGET.[EntranceTestTypeID] = SOURCE.TestType
		,TARGET.[MinScore] = SOURCE.Value
		,TARGET.SubjectID = SOURCE.SubjectID
		,TARGET.SubjectName = SOURCE.SubjectName
		,TARGET.[UID] = SOURCE.UID
		,TARGET.[EntranceTestPriority] = SOURCE.EntranceTestPriority
		,TARGET.[IsForSPOandVO] = SOURCE.IsForSPOandVO
		,TARGET.[ReplacedEntranceTestItemID] = SOURCE.ReplacedItemID
		,TARGET.[EntranceTestItemGUID] = SOURCE.GUID
		,TARGET.[IsFirst] = SOURCE.IsFirst
		,TARGET.[IsSecond] = SOURCE.IsSecond
WHEN NOT MATCHED BY TARGET THEN 
	INSERT 
		([CompetitiveGroupID]
        ,[EntranceTestTypeID]
        ,[MinScore]
        ,[SubjectID]
        ,[SubjectName]
        ,[UID]
        ,[CreatedDate]
        ,[ModifiedDate]
        ,[EntranceTestPriority]
        ,[EntranceTestItemGUID]
        ,[IsForSPOandVO]
        ,[ReplacedEntranceTestItemID]
		,[IsFirst]
		,[IsSecond])
	VALUES
		(@CompetitiveGroupID
		,SOURCE.TestType
		,SOURCE.Value
		,SOURCE.SubjectID
		,SOURCE.SubjectName
		,SOURCE.UID
		,GETDATE()
		,GETDATE()
		,SOURCE.EntranceTestPriority
		,SOURCE.GUID
		,SOURCE.IsForSPOandVO
		,SOURCE.ReplacedItemID
		,SOURCE.IsFirst
		,SOURCE.IsSecond
		)
	OUTPUT INSERTED.EntranceTestItemID, INSERTED.[EntranceTestItemGUID], NULL INTO @ids;

UPDATE IDS
SET 
	ItemID = et.ItemID
FROM @ids IDS
left join @EntranceTests et on et.GUID = ids.guid
Where IDS.ItemID is null;


UPDATE ETIC
Set EntranceTestPriority = null
From EntranceTestItemC ETIC
inner join @ids ids on ids.EntranceTestItemID = ETIC.EntranceTestItemID
Where ETIC.EntranceTestPriority = -1;

-- ====================
--select * from @ids;
-- ====================

-- ====================
--SELECT bi.*, 
--	etids.EntranceTestItemID as eticID
--	FROM @BenefitItems bi
--	left join @ids etids on bi.EntranceTestItemID = etids.ItemId;
-- ====================


-- 4. BenefitItems
Merge BenefitItemC as TARGET
USING (
	SELECT bi.*, 
	etids.EntranceTestItemID as eticID
	FROM @BenefitItems bi
	left join @ids etids on bi.EntranceTestItemID = etids.ItemId
) AS SOURCE
ON TARGET.[BenefitItemID] = SOURCE.ItemID
WHEN MATCHED THEN
	UPDATE 
	SET 
		TARGET.[EntranceTestItemID] = SOURCE.eticID
      ,TARGET.[OlympicDiplomTypeID] = SOURCE.OlympicDiplomTypeID
      ,TARGET.[OlympicLevelFlags] = SOURCE.OlympicLevelFlags
      ,TARGET.[BenefitID] = SOURCE.BenefitID
      ,TARGET.[IsForAllOlympic] = SOURCE.IsForAllOlympic
      ,TARGET.[CompetitiveGroupID] = SOURCE.CompetitiveGroupID
      ,TARGET.[UID] = SOURCE.UID
      ,TARGET.[ModifiedDate] = GETDATE()
      ,TARGET.[OlympicYear] = SOURCE.OlympicYear
      ,TARGET.[EgeMinValue] = SOURCE.EgeMinValue
      ,TARGET.[BenefitItemGUID] = SOURCE.BenefitItemGUID
      ,TARGET.[ClassFlags] = SOURCE.ClassFlags
	  ,TARGET.[IsCreative] = SOURCE.IsCreative
	  ,TARGET.[IsAthletic] = SOURCE.IsAthletic
WHEN NOT MATCHED BY TARGET THEN 
	INSERT 
			([EntranceTestItemID]
           ,[OlympicDiplomTypeID]
           ,[OlympicLevelFlags]
           ,[BenefitID]
           ,[IsForAllOlympic]
           ,[CompetitiveGroupID]
           ,[UID]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[OlympicYear]
           ,[EgeMinValue]
           ,[BenefitItemGUID]
           ,[ClassFlags]
		   ,[IsCreative]
		   ,[IsAthletic]
		   )
     VALUES
           (SOURCE.eticID
           ,SOURCE.OlympicDiplomTypeID
           ,SOURCE.OlympicLevelFlags
           ,SOURCE.BenefitID
           ,SOURCE.IsForAllOlympic
           ,SOURCE.CompetitiveGroupID
           ,SOURCE.UID
           ,GETDATE()
           ,GETDATE()
           ,SOURCE.OlympicYear
           ,SOURCE.EgeMinValue
           ,SOURCE.BenefitItemGUID
           ,SOURCE.ClassFlags
		   ,SOURCE.IsCreative
		   ,SOURCE.IsAthletic)
	OUTPUT INSERTED.BenefitItemID, INSERTED.BenefitItemGUID, NULL INTO @benefitItemIds;

UPDATE IDS
SET 
	ItemID = bi.ItemID
FROM @benefitItemIds IDS
left join @BenefitItems bi on bi.BenefitItemGUID = ids.guid;
--Where IDS.ItemID is null;

insert into BenefitItemCProfile ([BenefitItemID], [OlympicProfileID])
select ids.BenefitItemID, bip.OlympicProfileID
from @BenefitItemProfiles bip
inner join @benefitItemIds ids on bip.BenefitItemTempID = ids.ItemID;

insert into BenefitItemSubject ([BenefitItemId], [SubjectId], [EgeMinValue])
select ids.BenefitItemID, bis.SubjectId, bis.EgeMinValue
from @BenefitItemSubjects bis
inner join @benefitItemIds ids on bis.BenefitItemTempID = ids.ItemID;

INSERT INTO [dbo].[BenefitItemCOlympicType]
           ([BenefitItemID]
           ,[OlympicTypeID]
           ,[CreatedDate]
           ,[ModifiedDate]
--           ,[OlympicLevel]
           ,[ClassFlags]
           ,[OlympicLevelFlags]
           ,[GUID])
output inserted.ID, inserted.GUID, null into @BenefitItemOlympicsIds
Select ids.BenefitItemID
,bio.OlympicTypeID
,GETDATE()
,GETDATE()
--,bio.OlympicLevel
,bio.ClassFlags
,bio.OlympicLevelFlags
,bio.GUID
from @BenefitItemOlympics bio
inner join @benefitItemIds ids on bio.BenefitItemTempID = ids.ItemID;

UPDATE IDS
SET 
	ItemID = bio.ID
FROM @BenefitItemOlympicsIds IDS
left join @BenefitItemOlympics bio on bio.GUID = ids.guid;

-- ====================
--select * from @BenefitItemOlympicsIds;
-- ====================

INSERT INTO [dbo].[BenefitItemCOlympicTypeProfile]
           ([BenefitItemCOlympicTypeID]
           ,[OlympicProfileID])
Select ids.ID, biop.OlympicProfileID
From @BenefitItemOlympicProfiles biop
inner join @BenefitItemOlympicsIds ids on ids.ItemID = biop.BenefitItemTempID;

