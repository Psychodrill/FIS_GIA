-- =============================================
-- Для отладки

--DECLARE @CompetitiveGroupID INT = 28; 
--declare @CampaignID int = 46;
--declare @IsFromKrym bit = 1;
--declare @IsAdditional bit = 0; 
--declare @EducationFormID smallint = 11;
--declare @EducationLevelID smallint = 2; 
--declare @DirectionID int = 6017;
--declare @EducationSourceID INT = 16; -- 14 - Бюджет, 16 - Целевой прием, 
--declare @Programs NamedUidedItems;
--declare @Targets Identifiers; 

--set @CompetitiveGroupID = 28;
--select @CampaignID = cg.CampaignID, @IsFromKrym = cg.IsFromKrym, @IsAdditional = cg.IsAdditional, @DirectionID = cg.DirectionID,
--@EducationFormID = cg.EducationFormId, @EducationLevelID = cg.EducationLevelID, @EducationSourceID = cg.EducationSourceId
--From CompetitiveGroup cg
--Where cg.CompetitiveGroupID = @CompetitiveGroupID;

--insert into @Programs (ID, Name) 
--select ProgramID, Name
--From CompetitiveGroupProgram Where CompetitiveGroupID = @CompetitiveGroupID;

--insert into @Targets (id)
--select CompetitiveGroupTargetID From CompetitiveGroupTargetItem Where CompetitiveGroupID = @CompetitiveGroupID;
-- =============================================

declare @tmpName varchar(250);
declare @tmpName2 varchar(250);

-- проверка, что у других конкурсов этого ОО нет программ с таким же UID
--select  @tmpName = cgp.Name, @tmpName2 = cgp.UID
--From CompetitiveGroupProgram cgp (NOLOCK)
--inner join CompetitiveGroup cg (NOLOCK) on cgp.CompetitiveGroupID = cg.CompetitiveGroupID
--inner join CompetitiveGroup cgID (NOLOCK) on cg.InstitutionID = cgID.InstitutionID AND cgID.CompetitiveGroupID = @CompetitiveGroupID and cg.CompetitiveGroupID != @CompetitiveGroupID
--Where cgp.UID in (select UID From @Programs);

--if (@tmpName is not null AND @tmpName != '')
--BEGIN
--	SELECT 11 as Code, 'В системе уже имеется программа обучения: "' + @tmpName + '" с такими же UID: ' + @tmpName2 as Message;
--	return;
--END

-- проверка дублей по другим конкурсам
Select @tmpName = cg.Name
From CompetitiveGroup cg  with (nolock)
Left join CompetitiveGroupProgram cgp  with (nolock) on cgp.CompetitiveGroupID = cg.CompetitiveGroupID
left join CompetitiveGroupProgram cgp0 with (nolock) on cgp0.CompetitiveGroupID = @CompetitiveGroupID

Left join CompetitiveGroupTargetItem cgti  with (nolock) on cgti.CompetitiveGroupID = cg.CompetitiveGroupID
left join CompetitiveGroupTargetItem cgti0 with (nolock) on cgti0.CompetitiveGroupID = @CompetitiveGroupID

Where cg.CompetitiveGroupID != @CompetitiveGroupID
AND cg.CampaignID = @CampaignID
AND cg.DirectionID = @DirectionID
AND cg.EducationFormId = @EducationFormID
AND cg.EducationLevelID = @EducationLevelID
AND cg.EducationSourceId = @EducationSourceID
AND cg.IsFromKrym = @IsFromKrym
AND cg.IsAdditional = @IsAdditional
AND cg.IdLevelBudget = @IdLevelBudget
AND (
		--cgp0.ProgramID is null OR 
		cg.CompetitiveGroupID in 
		(
			Select cg2.CompetitiveGroupID --, count(qTo.Name), count(qFrom.Name)
			From CompetitiveGroup cg2 (NOLOCK)
			outer apply 
			(
			select id from @Programs 
			except 
			select InstitutionProgramID as id from CompetitiveGroupToProgram cgp2 (NOLOCK) Where cgp2.CompetitiveGroupID = cg2.CompetitiveGroupID
			) as qTo
			outer apply 
			(
			select InstitutionProgramID as id from CompetitiveGroupToProgram cgp2 (NOLOCK) Where cgp2.CompetitiveGroupID = cg2.CompetitiveGroupID
			except
			select id from @Programs 
			) as qFrom
			Where cg2.CompetitiveGroupID != @CompetitiveGroupID
			group by cg2.CompetitiveGroupID
			having count(qTo.id)=0 and count(qFrom.id)=0
		) 
	)
AND (
		--(cgti0.CompetitiveGroupTargetID is null OR @EducationSourceID != 16) OR 
		cg.CompetitiveGroupID in 
		(
			Select cg2.CompetitiveGroupID --, count(qTo.Name), count(qFrom.Name)
			From CompetitiveGroup cg2 (NOLOCK)
			outer apply 
			(
			select id from @Targets 
			except 
			select CompetitiveGroupTargetID as id from CompetitiveGroupTargetItem cgti2 (NOLOCK) Where cgti2.CompetitiveGroupID = cg2.CompetitiveGroupID
			) as qTo
			outer apply 
			(
			select CompetitiveGroupTargetID as id from CompetitiveGroupTargetItem cgti2 (NOLOCK) Where cgti2.CompetitiveGroupID = cg2.CompetitiveGroupID
			except
			select id from @Targets 
			) as qFrom
			Where cg2.CompetitiveGroupID != @CompetitiveGroupID
			group by cg2.CompetitiveGroupID
			having count(qTo.id)=0 and count(qFrom.id)=0
		) 
	)

if (@tmpName is not null AND @tmpName != '')
begin
	declare @krimeaMessage varchar(255) = ''
	declare @campaignYear int
	select @campaignYear = YearStart from Campaign where CampaignID = @CampaignID 
	if(@campaignYear=2016)
	begin
		set @krimeaMessage = ', Прием жителей Крыма и Севастополя'
	end

	SELECT 1 as Code, 'В системе уже имеется конкурс "' + @tmpName + '" с такими же параметрами: Тип ПК, год ПК, Направление, Форма обучения, Источник финансирования, Уровень образования'+@krimeaMessage+', Уровень бюджета,  Дополнительный набор' as Message;
end
else 
	SELECT 0 as Code, '' as Message;
