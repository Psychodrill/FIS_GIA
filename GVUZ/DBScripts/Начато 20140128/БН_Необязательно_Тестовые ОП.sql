insert into Direction
(
	Code,
	Name,
	ParentID,
	SYS_GUID,
	EDULEVEL,
	EDUPROGRAMTYPE,
	UGSCODE,
	UGSNAME,
	QUALIFICATIONCODE,
	QUALIFICATIONNAME,
	PERIOD,
	EDU_DIRECTORY,
	EDUPR_ADDITIONAL,
	CreatedDate,
	ModifiedDate,
	NewCode
)

select 
	null, 
	'���������� � ��������', 
	1, 
	null, 
	'����� ������ ������������', 
	'����� ������ ������������', 
	'010000', 
	'������-�������������� �����', 
	70, 
	'���� ������ ������������', 
	null, 
	'����', 
	null, 
	null, 
	null, 
	'01.06.01'
union
select 
	null, 
	'������������ � �������������� �����', 
	1, null, '����� ������ ������������', '����� ������ ������������', '010000', '������-�������������� �����', 70, '���� ������ ������������', null, '����', null, null, null, '02.06.01'
union
select null, '������������ � �������������� �����', 1, null, '����� ������ ������������', '����� ������ ������������', '010000', '������-�������������� �����', 70, '���� ������ ������������', null, '����', null, null, null, '02.07.01'
union
select null, '������ � ����������', 1, null, '����� ������ ������������', '����� ������ ������������', '010000', '������-�������������� �����', 70, '���� ������ ������������', null, '����', null, null, null, '03.06.01'
union
select null, '���������� �����', 1, null, '����� ������ ������������', '����� ������ ������������', '010000', '������-�������������� �����', 70, '���� ������ ������������', null, '����', null, null, null, '04.07.01'

