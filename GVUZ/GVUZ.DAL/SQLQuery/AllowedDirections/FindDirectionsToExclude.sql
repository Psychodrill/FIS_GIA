-- поиск направлений для добавления в заявку на исключение из списка разрешенных

--if (OBJECT_ID('tempdb..#tmpId') IS NOT NULL)
--drop table #tmpId;

--GO

--CREATE TABLE #tmpId(directionId int not null PRIMARY KEY)

--declare @institutionId int
--declare @educationLevelId int = 2
--declare @ugsId int = 2

;
with used_dirs as -- направления, которые должны быть исключены из выборки при поиске
(
   select -- направления которые уже есть в заявках на добавление, исключение в т.ч. в отклоненных заявках
		DirectionId 
   from 
		InstitutionDirectionRequest req
   where 
		InstitutionId = @institutionId
		and req.RequestType in (0, 1)
   union
   select -- направления, выбранные пользователем, но еще не сохраненные в заявке
		DirectionId 
   from 
		#tmpId
),
avail_dirs as -- доступные направления для включения в заявку на удаление из списка разрешенных
(
   select distinct -- все направления в AllowedDirections
		ad.DirectionId
   from 
		AllowedDirections ad
   where 
		ad.InstitutionId = @institutionId
   except -- кроме тех, что недоступны для включения в заявку на удаление
   select 
		DirectionId 
   from 
		used_dirs
)
select
	dir.DirectionId,
	dir.Code,
	dir.NewCode,
	dir.QUALIFICATIONCODE,
	dir.Name DirectionName,
	dir.QUALIFICATIONNAME,
	lvl.ItemTypeId EducationLevelId,
	lvl.Name EducationLevelName
from
	Direction dir
	inner join avail_dirs on dir.DirectionID = avail_dirs.DirectionID
	inner join AdmissionItemType lvl on dir.EducationLevelId = lvl.ItemTypeID and lvl.ItemLevel = 2
	inner join ParentDirection ugs on dir.ParentId = ugs.ParentDirectionId
where
	dir.EducationLevelId = @educationLevelId
	and ugs.ParentDirectionId = @ugsId
