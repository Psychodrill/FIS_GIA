--USE [D:\CODE\DB\GVUZ.MDF]
--GO

/****** Object: SqlProcedure [dbo].[ChectBenefitOlympic] Script Date: 21.04.2016 13:34:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[ChectBenefitOlympic]
	@entranceTestItemID int, @groupID int, @olympicTypeProfileID int, 
	@diplomaTypeID int, @olympicID int, @formNumberID int, @docID int,
	@errorMessage nvarchar(4000) = null output, 
	@violationMessage nvarchar(4000) = null output, 
	@violationId int = 0 output
as begin
	set nocount on;

	-- возвращаемые переменные
	set @violationMessage = ''
	set @errorMessage = ''
	set @violationId = 0

	-- перекодировка класса (7 = 1; 8 = 2; 9 = 4; 10 = 8; 11 = 16)
	declare @class int = power(2, @formNumberID - 7)

	-- определение уровня олимпиады и перекодировка (1 (все) = 255, 2 (1) = 1, 3 (2) = 2, 4 (3) = 4, NULL = 0 ) 
	-- достаем профиль олимпиады
	declare @level int = 0
	declare @olympicLevelId int
	declare @olympicProfileId int

	select @olympicLevelId = OlympicLevelID, @olympicProfileId = OlympicProfileID
	from OlympicTypeProfile where OlympicTypeProfileID = @olympicTypeProfileID

	if(@olympicLevelId is not null) begin
		if(@olympicLevelId = 1) set @level = 255 else
		if(@olympicLevelId = 2) set @level = 1 else
		if(@olympicLevelId = 3) set @level = 2 else
		if(@olympicLevelId = 4) set @level = 4
	end

	-- наберем в эту таблицу только подходящие нам льготы
	declare @view table
	(	
		id int, forall bit, olympicyear int, classflag int, groupid int, test int, diplom int, 
		levelflag int, profileid int, _classflag  int, _levelflag int, olympic  int, _profileid int
	)
	insert into @view
	select 
		c.BenefitItemID id, c.IsForAllOlympic forall, c.OlympicYear olympicyear, c.ClassFlags classflag, 
		c.CompetitiveGroupID groupid, c.EntranceTestItemID test, c.OlympicDiplomTypeID diplom, 
		c.OlympicLevelFlags levelflag,	p.OlympicProfileID profileid, t.ClassFlags _classflag, 
		t.OlympicLevelFlags _levelflag, t.OlympicTypeID olympic, r.OlympicProfileID _profileid
	from BenefitItemC as c 
		left join BenefitItemCProfile as p on c.BenefitItemID = p.BenefitItemID
		left join BenefitItemCOlympicType as t on c.BenefitItemID = t.BenefitItemID
		left join BenefitItemCOlympicTypeProfile as r on t.ID = r.BenefitItemCOlympicTypeID
	where 
	-- только нужная нам группа
	c.CompetitiveGroupID = @groupID and 

	-- только нужный нам тип диплома
	(c.OlympicDiplomTypeID = 3 or c.OlympicDiplomTypeID = @diplomaTypeID) and

	-- если пришли с конкретным тестом, то оставим льготы только по нему и те, у которых null
	((@entranceTestItemID is null and c.EntranceTestItemID is null) 
		or (@entranceTestItemID is not null and (c.EntranceTestItemID = @entranceTestItemID or c.EntranceTestItemID is null))) and

	-- @class, представленный в виде битов соответствует BenefitItemCOlympicType.Class
	((t.ClassFlags = 255) or ((t.ClassFlags & @class) > 0)) and

	-- (BenefitItemC.OlympicLevelFlags = 255) ИЛИ (OlympicTypeProfile.OlympicLevelID = хотя бы одному из уровней олимпиады, закодированных в BenefitItemC.OlympicLevelFlags)
	((c.OlympicLevelFlags = 255) or ((c.OlympicLevelFlags & @level) > 0)) and

	--Если BenefitItemC.IsForAllOlympics = 0, то условие следующее: 
	--(OlympicTypeProfile.OlympicTypeID = BenefitItemCOlympicType.OlympicTypeID)
	--	И
	--((BenefitItemCOlympicTypeProfile отсутствует) 
	--  ИЛИ 
	--(OlympicTypeProfile.OlympicProfileID = BenefitItemCOlympicTypeProfile.OlympicProfileID))
		((c.IsForAllOlympic = 0 and t.OlympicTypeID = @olympicID and 
			((ISNULL(r.OlympicProfileID, ISNULL(p.OlympicProfileID,255)) = @olympicProfileId)
			or(ISNULL(r.OlympicProfileID, ISNULL(p.OlympicProfileID,255)) = 255)))
		or
	-- По профилям произвольных олимпиад условие следующее: OlympicTypeProfile.OlympicProfileID = BenefitItemCProfile.OlympicProfileID
		(c.IsForAllOlympic = 1 and (p.OlympicProfileID = @olympicProfileId or p.OlympicProfileID = 255)))
	 																									 
	--select * from @view

	declare @cnt int 
	select @cnt = count(distinct id) from @view
 
	-- найдена хотя бы одна запись по BenefitItemC, BenefitItemCOlympicType, BenefitItemCOlympicTypeProfile
	if (@cnt > 0)
	begin  
		-- запись о льготах есть, уходим, все нормально
		update EntrantDocument set OlympMatched = 1 where EntrantDocumentID = @docID
		return 0
	end

	-- записи о льготах отсутствуют, уходим, код 16 или 17

	-- вытаскиваем имя конкурсной группы
	declare @competitiveGroupName varchar(250) = ''
	select @competitiveGroupName = Name from CompetitiveGroup (NOLOCK) where CompetitiveGroupID = @groupID

	if(@entranceTestItemID is null) begin
		-- код и текст ошибки для случая без ссылки на дисциплину
		set @violationId = 17
		set @violationMessage = N'Для конкурсной группы ' + isnull(@competitiveGroupName,'') + ' не найдено ни одной общей льготы, позволяющей использовать указанный диплом для зачисления без вступительных испытаний'
	end
	else begin
		-- вытаскиваем имя предмета по ссылке, либо просто наименование
		declare @subjectID int
		declare @subjectName varchar(2000) = ''
		select @subjectID = SubjectID, @subjectName = SubjectName from EntranceTestItemC where EntranceTestItemID = @entranceTestItemID
		if(@subjectID is not null)
			select @subjectName = Name from Subject where SubjectID = @subjectID 

		-- код и текст ошибки для случая со ссылкой на дисциплину
		set @violationId = 16
		set @violationMessage = N'Для вступительного испытания '+ isnull(@subjectName,'') + ' в конкурсной группе ' + isnull(@competitiveGroupName,'') + ' не найдено ни одной льготы, позволяющей использовать указанный диплом для присвоения 100 баллов по вступительному испытанию'
	end

	update EntrantDocument 
		set OlympApproved = 0 where EntrantDocumentID = @docID

	update Application 
		set ViolationID = @violationId, ViolationErrors = @violationMessage 
		where ApplicationID = (select top 1 ApplicationID from ApplicationEntrantDocument (NOLOCK) where EntrantDocumentID = @docID)

	return 0 	
end
