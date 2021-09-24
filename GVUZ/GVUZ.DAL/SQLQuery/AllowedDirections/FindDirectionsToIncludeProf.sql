-- ����� ����������� ��� ���������� � ������ �� ��������� ����������� ����������� � ������ � ����������� ��

--if (OBJECT_ID('tempdb..#tmpId') IS NOT NULL)
--drop table #tmpId;

--GO

--CREATE TABLE #tmpId(directionId int not null PRIMARY KEY)

--declare @institutionId int
--declare @educationLevelId int = 2
--declare @ugsId int = 2
--declare @year int

;
with used_dirs as -- �����������, ������� ������ ���� ��������� �� ������� ��� ������
(
   select -- ����������� ������� ��� ���� � ������� �� ���������� � �.�. � ����������� �������
		DirectionId 
   from 
		InstitutionDirectionRequest req
   where 
		InstitutionId = @institutionId
		and req.RequestType = 2
   union
   select -- �����������, ������� ��� ��������� � ������ � ����������� ��
		DirectionId
   from 
		EntranceTestProfileDirection 
   union
   select -- �����������, ��������� �������������, �� ��� �� ����������� � ������
		DirectionId 
   from 
		#tmpId
),
avail_dirs as -- ��������� ����������� ��� ��������� � ������ �� ���������� � ������ � ����������� ��
(
   select distinct -- ��� ����������� � AllowedDirections
		DirectionId
   from 
		AllowedDirections
   where 
		InstitutionId = @institutionId
		and [Year] = @year
   except -- ����� ���, ��� ���������� ��� ��������� � ������ �� ���������� � ������ � ����������� ��
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
	and dir.IsVisible = 1
