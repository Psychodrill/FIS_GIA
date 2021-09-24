-- ����� ����������� ��� ���������� � ������ �����������

--declare @institutionId int
--declare @educationLevelId int = 2
--declare @ugsId int = 2

;with avail_dirs as -- ��������� ����������� ��� ��������� � ������ �� ���������� � ������ �����������
(
   select -- ��� ����������� �� ������� Direction � ������ ������� �� ������ ����������� � ���
		dir.DirectionId
   from 
		Direction dir
		inner join ParentDirection ugs on dir.ParentId = ugs.ParentDirectionID
   where 
		dir.EducationLevelId = @educationLevelId
		and ugs.ParentDirectionID = @ugsId
   except -- ����� ���, ��� ���������� ��� ���������
   select distinct -- ����������� ������� ��� ���� � ������ �����������
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
