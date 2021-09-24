-- поиск направлений дл€ добавлени€ в список разрешенных

--declare @institutionId int
--declare @educationLevelId int = 2
--declare @ugsId int = 2

;with avail_dirs as -- доступные направлени€ дл€ включени€ в за€вку на добавление в список разрешенных
(
   select -- все направлени€ из таблицы Direction с учетом фильтра по уровню образовани€ и ”√—
		dir.DirectionId
   from 
		Direction dir
		inner join ParentDirection ugs on dir.ParentId = ugs.ParentDirectionID
   where 
		dir.EducationLevelId = @educationLevelId
		and ugs.ParentDirectionID = @ugsId
   except -- кроме тех, что недоступны дл€ включени€
   select distinct -- направлени€ которые уже есть в списке разрешенных
		DirectionId 
   from 
		AllowedDirections
   where
		InstitutionId = @institutionId
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
Where dir.IsVisible = 1
