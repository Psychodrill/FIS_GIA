
--test data--
-------------
--DECLARE @competitiveGroups Identifiers; 
--INSERT INTO @competitiveGroups (id) VALUES (245311);
--INSERT INTO @competitiveGroups (id) VALUES (241699);
--INSERT INTO @competitiveGroups (id) VALUES (245338);

--DECLARE @InstitutionID int = 587;
--DECLARE @copy_year int = 2025;
--DECLARE @copy_сampaignType int = 1;
--DECLARE @copy_levelBudget int = 1;
-----------------
--end test data--

DECLARE @CompetitiveGroupsInserted TABLE (CompetitiveGroupID_NEW INT, CompetitiveGroupID_OLD INT)
DECLARE @EntranceTestItemCInserted TABLE (EntranceTestItemID_NEW INT, EntranceTestItemID_OLD INT)  --Для льгот

declare @CompetitiveGroupToCopy table (
	[CompetitiveGroupID] [int] NOT NULL,
	[InstitutionID] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Course] [smallint] NOT NULL,
	[UID] [varchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[CampaignID] [int] NULL,
	[CompetitiveGroupGUID] [uniqueidentifier] NULL,
	[IsFromKrym] [bit] NULL,
	[IsAdditional] [bit] NULL,
	[EducationFormId] [smallint] NULL,
	[EducationSourceId] [smallint] NULL,
	[EducationLevelID] [smallint] NULL,
	[DirectionID] [int] NULL,
	[IdLevelBudget] [int] NULL
) 

--таблица с результатом
declare @result table (
	[CompetitiveGroupID] [int] NOT NULL,
	[Message] varchar(250) NULL,
	[NewCompetitiveGroupID] [int] NULL
) 

--если иностранцы по квоте уровня бюджета быть не может!
IF @copy_сampaignType = 5 SET @copy_levelBudget = NULL

--заполняем первичными данными
insert into @result
select id, null, null from @competitiveGroups

--если уровень бюджета не задан, везде ошибка
--if (@copy_levelBudget is null OR @copy_levelBudget not in (1,2,3))
--Begin
--	update @result 
--	set [Message] = 'Необходимо выбрать уровень бюджета!'
--End;

--получаем кампанию
DECLARE @CampaignID int = (
select top (1) CampaignID 
from Campaign c (NOLOCK)
where c.YearStart = @copy_year and c.CampaignTypeID = @copy_сampaignType and c.InstitutionID = @InstitutionID
and c.StatusID = 1
)

--select @CampaignID
--если кампании не существует, везде ошибка
if (@CampaignID is null)
Begin
	update @result 
	set [Message] = 'Кампания с выбранными параметрами не существует, либо статус этой кампании не "Идет набор"!'

	--select * from @result
End;

Else if (@CampaignID is not null)
	Begin
	--сначала заполняем таблицу которая нам нужны для проверки
	insert into @CompetitiveGroupToCopy 
	select 
		CompetitiveGroupID,
		InstitutionID,
		Name,
		Course,
		UID,
		CreatedDate,
		ModifiedDate,
		CampaignID,
		CompetitiveGroupGUID,
		IsFromKrym,
		IsAdditional,
		EducationFormId,
		EducationSourceId,
		EducationLevelID,
		DirectionID,
		CASE WHEN EducationSourceId != 15 THEN @copy_levelBudget ELSE null END AS IdLevelBudget
	 from CompetitiveGroup cg (NOLOCK)
	inner join @competitiveGroups ids on cg.CompetitiveGroupID = ids.id;

	--insert into @CompetitiveGroupProgramToCopy 
	--select 
	--	pr.ProgramID,
	--	pr.UID,
	--	pr.CompetitiveGroupID,
	--	pr.Name,
	--	pr.Code,
	--	GETDATE(),
	--	GETDATE() 
	--from CompetitiveGroupProgram pr
	--inner join @competitiveGroups ids on pr.CompetitiveGroupID = ids.id;

	--insert into @CompetitiveGroupTargetItemToCopy 
	--select 	
	--	tar.CompetitiveGroupTargetItemID,
	--	tar.CompetitiveGroupTargetID,
	--	GETDATE(),
	--	GETDATE(), 
	--	tar.NumberTargetO,
	--	tar.NumberTargetOZ,
	--	tar.NumberTargetZ,
	--	tar.CompetitiveGroupID
	--from CompetitiveGroupTargetItem tar
	--inner join @competitiveGroups ids on tar.CompetitiveGroupID = ids.id;


	--select * from @CompetitiveGroupToCopy
	--select * from @CompetitiveGroupProgramToCopy
	--select * from @CompetitiveGroupTargetItemToCopy

	--выполняем проверку
	--проверка на то, что кампания с таким же типом
	update @result
	set [Message] = 'Можно скопировать конкурс только в кампанию с таким же типом!' 
	From @result r
	INNER JOIN @CompetitiveGroupToCopy cg_toCopy ON r.CompetitiveGroupID = cg_toCopy.CompetitiveGroupID
	INNER JOIN CompetitiveGroup cg (NOLOCK)  ON cg.CompetitiveGroupID = cg_toCopy.CompetitiveGroupID 
	INNER JOIN Campaign c (NOLOCK) ON cg.CampaignID = c.CampaignID
	INNER JOIN Campaign c_toCopy (NOLOCK) ON c_toCopy.CampaignID = @CampaignID
	where c.CampaignTypeID != c_toCopy.CampaignTypeID

	--проверка на дублирующееся наименование
	update @result
	set [Message] = 'В кампании уже имеется конкурс "' + cg.Name + '" с таким же наименованием!' 
	From @result r
	INNER JOIN @CompetitiveGroupToCopy cg_toCopy ON r.CompetitiveGroupID = cg_toCopy.CompetitiveGroupID
	INNER JOIN CompetitiveGroup cg (NOLOCK) --on cg.CompetitiveGroupID != cg_toCopy.CompetitiveGroupID
	on cg.CampaignID = @CampaignID AND cg.Name = cg_toCopy.Name

	-- проверка дублей по другим конкурсам
	UPDATE @result
	set [Message] = 'В системе уже имеется конкурс "' + t.Name + '" с такими же параметрами: Тип ПК, год ПК, Направление, Форма обучения, Источник финансирования, Уровень образования, Уровень бюджета, Дополнительный набор!' 
	From 
	(SELECT r.CompetitiveGroupID, cg.Name FROM @result r
	INNER JOIN @CompetitiveGroupToCopy cg_toCopy ON r.CompetitiveGroupID = cg_toCopy.CompetitiveGroupID
	INNER JOIN CompetitiveGroup cg (NOLOCK) on --cg.CompetitiveGroupID != cg_toCopy.CompetitiveGroupID AND
	 cg.CampaignID = @CampaignID
	AND cg.DirectionID = cg_toCopy.DirectionID
	AND cg.EducationFormId = cg_toCopy.EducationFormID
	AND cg.EducationLevelID = cg_toCopy.EducationLevelID
	AND cg.EducationSourceId = cg_toCopy.EducationSourceID
	AND cg.IsFromKrym = cg_toCopy.IsFromKrym
	AND cg.IsAdditional = cg_toCopy.IsAdditional
	AND cg.IdLevelBudget = cg_toCopy.IdLevelBudget

	Left join CompetitiveGroupToProgram cgp  (NOLOCK) on cgp.CompetitiveGroupID = cg.CompetitiveGroupID
	left join CompetitiveGroupToProgram cgp_toCopy on cgp_toCopy.CompetitiveGroupID = cg_toCopy.CompetitiveGroupID

	Left join CompetitiveGroupTargetItem cgt  with (nolock) on cgt.CompetitiveGroupID = cg.CompetitiveGroupID
	left join CompetitiveGroupTargetItem cgt_toCopy with (nolock) on cgt_toCopy.CompetitiveGroupID = cg_toCopy.CompetitiveGroupID
	
	WHERE 
		CG.EducationSourceId <> 15
	GROUP BY r.CompetitiveGroupID, cg.Name	
	HAVING 
	SUM(ISNULL(cgp.InstitutionProgramID, 0)) = SUM(ISNULL(cgp_toCopy.InstitutionProgramID, 0))
	AND SUM(ISNULL(cgt.CompetitiveGroupTargetID, 0)) = SUM(ISNULL(cgt_toCopy.CompetitiveGroupTargetID, 0))
	
	) AS t
	WHERE  [@result].CompetitiveGroupID=t.CompetitiveGroupID

	--Начинаем копирование тех конкрусов, которые не попали в @result с ошибкой--
	-----------------------------------------------------------------------------

	--Для начала сам конкурс
	--INSERT INTO CompetitiveGroup
	--OUTPUT INSERTED.CompetitiveGroupID INTO @CompetitiveGroupsInserted
	--SELECT 
	--	--CompetitiveGroupID,
	--	InstitutionID,
	--	Name,
	--	Course,
	--	null, --UID,
	--	CreatedDate,
	--	ModifiedDate,
	--	@CampaignID,
	--	null, --CompetitiveGroupGUID,
	--	IsFromKrym,
	--	IsAdditional,
	--	EducationFormId,
	--	EducationSourceId,
	--	EducationLevelID,
	--	DirectionID 
	--from CompetitiveGroup cg (NOLOCK)
	--inner join @competitiveGroups ids on cg.CompetitiveGroupID = ids.id
	--where cg.CompetitiveGroupID in (select CompetitiveGroupID from @result where Message is null);

	--MERGE чтоб создать связку old - new id
	MERGE CompetitiveGroup AS TARGET
	USING (
		SELECT 
			CompetitiveGroupID
			,InstitutionID,
			Name,
			Course,
			null as UID,
			getdate() as CreatedDate,
			getdate() as ModifiedDate,
			@CampaignID as CampaignID,
			null as CompetitiveGroupGUID,
			IsFromKrym,
			IsAdditional,
			EducationFormId,
			EducationSourceId,
			EducationLevelID,
			DirectionID,
			CASE WHEN EducationSourceId != 15 THEN @copy_levelBudget ELSE null END AS IdLevelBudget
		from CompetitiveGroup cg (NOLOCK)
		inner join @competitiveGroups ids on cg.CompetitiveGroupID = ids.id
		where cg.CompetitiveGroupID in (select CompetitiveGroupID from @result where Message is null)
	) AS SOURCE
	ON 0 = 1
	WHEN NOT MATCHED BY TARGET THEN
	  INSERT 
		([InstitutionID]
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
		   ,[IdLevelBudget])
	  VALUES 
			(SOURCE.InstitutionID,
			SOURCE.Name,
			SOURCE.Course,
			SOURCE.UID,
			SOURCE.CreatedDate,
			SOURCE.ModifiedDate,
			SOURCE.CampaignID,
			SOURCE.CompetitiveGroupGUID,
			SOURCE.IsFromKrym,
			SOURCE.IsAdditional,
			SOURCE.EducationFormId,
			SOURCE.EducationSourceId,
			SOURCE.EducationLevelID,
			SOURCE.DirectionID,
			SOURCE.IdLevelBudget)
	OUTPUT INSERTED.CompetitiveGroupID, SOURCE.CompetitiveGroupID INTO @CompetitiveGroupsInserted;

	--select * from @CompetitiveGroupsInserted

	--Теперь места
	INSERT INTO CompetitiveGroupItem
	SELECT 
		--[CompetitiveGroupItemID]
      ids.CompetitiveGroupID_NEW
      ,0 --[NumberBudgetO]
      ,0 --[NumberBudgetOZ]
      ,0 --[NumberBudgetZ]
      ,0 --[NumberPaidO]
      ,0 --[NumberPaidOZ]
      ,0 --[NumberPaidZ]
      ,0 --[CreatedDate]
      ,0 --[ModifiedDate]
      ,0 --[NumberQuotaO]
      ,0 --[NumberQuotaOZ]
      ,0 --[NumberQuotaZ]
      ,0 --[NumberTargetO]
      ,0 --[NumberTargetOZ]
      ,0 --[NumberTargetZ]
	from CompetitiveGroupItem cg (NOLOCK)
	inner join @CompetitiveGroupsInserted ids on cg.CompetitiveGroupID = ids.CompetitiveGroupID_OLD;

	--программы
	INSERT INTO CompetitiveGroupToProgram
	SELECT 
		--[ProgramID]
      InstitutionProgramID
	  ,ids.CompetitiveGroupID_NEW
      ,GETDATE()
      ,GETDATE()
	from CompetitiveGroupToProgram cgp (NOLOCK)
	inner join @CompetitiveGroupsInserted ids on cgp.CompetitiveGroupID = ids.CompetitiveGroupID_OLD;

	--целевые организации
	INSERT INTO CompetitiveGroupTargetItem
	SELECT 
	   [CompetitiveGroupTargetID]
      ,GETDATE()
      ,GETDATE()
      ,[NumberTargetO]
      ,[NumberTargetOZ]
      ,[NumberTargetZ]
      ,ids.CompetitiveGroupID_NEW
	from CompetitiveGroupTargetItem targ (NOLOCK)
	inner join @CompetitiveGroupsInserted ids on targ.CompetitiveGroupID = ids.CompetitiveGroupID_OLD;

	--ВИ (незаменяемые)
	MERGE EntranceTestItemC AS TARGET
	USING (
		SELECT 
			[EntranceTestItemID]
		  ,ids.CompetitiveGroupID_NEW AS [CompetitiveGroupID]
		  ,[EntranceTestTypeID]
		  ,[MinScore]
		  ,[SubjectID]
		  ,[SubjectName]
		  ,null as [UID]
		  ,GETDATE() as [CreatedDate]
		  ,GETDATE() as [ModifiedDate]
		  ,[EntranceTestPriority]
		  ,null as [EntranceTestItemGUID]
		  ,[IsForSPOandVO]
		  ,[ReplacedEntranceTestItemID]
		from EntranceTestItemC ent (NOLOCK)
		inner join @CompetitiveGroupsInserted ids on ent.CompetitiveGroupID = ids.CompetitiveGroupID_OLD
		where ReplacedEntranceTestItemID is null
	) AS SOURCE
	ON 0 = 1
	WHEN NOT MATCHED BY TARGET THEN
	  INSERT 
		( --[EntranceTestItemID]
		  [CompetitiveGroupID]
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
		  ,[ReplacedEntranceTestItemID])
	  VALUES 
			(SOURCE.CompetitiveGroupID
		  ,SOURCE.EntranceTestTypeID
		  ,SOURCE.MinScore
		  ,SOURCE.SubjectID
		  ,SOURCE.SubjectName
		  ,SOURCE.UID
		  ,SOURCE.CreatedDate
		  ,SOURCE.ModifiedDate
		  ,SOURCE.EntranceTestPriority
		  ,SOURCE.EntranceTestItemGUID
		  ,SOURCE.IsForSPOandVO
		  ,SOURCE.ReplacedEntranceTestItemID)
	OUTPUT INSERTED.EntranceTestItemID, SOURCE.EntranceTestItemID INTO @EntranceTestItemCInserted;


	--ВИ (заменяемые)
	MERGE EntranceTestItemC AS TARGET
	USING (
		SELECT 
			[EntranceTestItemID]
		  ,ids.CompetitiveGroupID_NEW AS [CompetitiveGroupID]
		  ,[EntranceTestTypeID]
		  ,[MinScore]
		  ,[SubjectID]
		  ,[SubjectName]
		  ,null as [UID]
		  ,GETDATE() as [CreatedDate]
		  ,GETDATE() as [ModifiedDate]
		  ,[EntranceTestPriority]
		  ,null as [EntranceTestItemGUID]
		  ,[IsForSPOandVO]
		  ,[ReplacedEntranceTestItemID]
		from EntranceTestItemC ent (NOLOCK)
		inner join @CompetitiveGroupsInserted ids on ent.CompetitiveGroupID = ids.CompetitiveGroupID_OLD
		where ReplacedEntranceTestItemID is not null
	) AS SOURCE
	ON 0 = 1
	WHEN NOT MATCHED BY TARGET THEN
	  INSERT 
		( --[EntranceTestItemID]
		  [CompetitiveGroupID]
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
		  ,[ReplacedEntranceTestItemID])
	  VALUES 
			(SOURCE.CompetitiveGroupID
		  ,SOURCE.EntranceTestTypeID
		  ,SOURCE.MinScore
		  ,SOURCE.SubjectID
		  ,SOURCE.SubjectName
		  ,SOURCE.UID
		  ,SOURCE.CreatedDate
		  ,SOURCE.ModifiedDate
		  ,SOURCE.EntranceTestPriority
		  ,SOURCE.EntranceTestItemGUID
		  ,SOURCE.IsForSPOandVO
		  ,SOURCE.ReplacedEntranceTestItemID)
	OUTPUT INSERTED.EntranceTestItemID, SOURCE.EntranceTestItemID INTO @EntranceTestItemCInserted;

	--Льготы (для ВИ)
	INSERT INTO BenefitItemC
	SELECT 
	  etIds.EntranceTestItemID_NEW
      ,[OlympicDiplomTypeID]
      ,[OlympicLevelFlags]
      ,[BenefitID]
      ,[IsForAllOlympic]
      ,ids.CompetitiveGroupID_NEW
      ,null
      ,GETDATE()
      ,GETDATE()
      ,[OlympicYear]
      ,[EgeMinValue]
      ,null
      ,[ClassFlags]
      ,[IsCreative]
      ,[IsAthletic]
	from BenefitItemC benefit (NOLOCK)
	inner join @CompetitiveGroupsInserted ids on benefit.CompetitiveGroupID = ids.CompetitiveGroupID_OLD
	inner join @EntranceTestItemCInserted etIds on benefit.EntranceTestItemID = etIds.EntranceTestItemID_OLD;

	--Льготы (общие)
	INSERT INTO BenefitItemC
	SELECT 
	  null
      ,[OlympicDiplomTypeID]
      ,[OlympicLevelFlags]
      ,[BenefitID]
      ,[IsForAllOlympic]
      ,ids.CompetitiveGroupID_NEW
      ,null
      ,GETDATE()
      ,GETDATE()
      ,[OlympicYear]
      ,[EgeMinValue]
      ,null
      ,[ClassFlags]
      ,[IsCreative]
      ,[IsAthletic]
	from BenefitItemC benefit (NOLOCK)
	inner join @CompetitiveGroupsInserted ids on benefit.CompetitiveGroupID = ids.CompetitiveGroupID_OLD
	and benefit.EntranceTestItemID is null

	-----------------------------------------------------------------------------
	--Конец копирования
	-----------------------------------------------------------------------------

	--в конце закинем в result то что заинсертили
	update @result
		set Message = 'Конкурс скопирован успешно.',
		NewCompetitiveGroupID = CompetitiveGroupID_NEW
	from @CompetitiveGroupsInserted	
	where Message is null --пусто, значит не было ошибки
	AND CompetitiveGroupID_OLD = CompetitiveGroupID
End;

--забираем результат
select * from @result