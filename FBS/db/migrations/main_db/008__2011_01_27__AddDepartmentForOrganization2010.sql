-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (8, '008__2011_01_27__AddDepartmentForOrganization2010')
-- =========================================================================





IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND  A.TABLE_NAME = 'Organization2010' AND A.COLUMN_NAME = 'DepartmentId'))
BEGIN
	ALTER TABLE [dbo].[Organization2010] DROP [fk_OrzanizationDepartmentId]
	ALTER TABLE [dbo].[Organization2010] DROP COLUMN [DepartmentId]
END
GO

ALTER TABLE [dbo].[Organization2010]
	ADD [DepartmentId] int null
GO

/*select ('insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, '
		+'RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values ('
		+'GETDATE(), GETDATE(), '+''''+T1.OwnerDepartment
		+''', '+''''+T1.OwnerDepartment+''', '+'77, 6, 9, 0, 0, '+'''������'')')
from
	(select distinct O.OwnerDepartment
	from Organization2010 O
	where O.OwnerDepartment not in ('', '����', '��� ����', '������ ���� �����������') 
	) T1
order by
	T1.OwnerDepartment*/
	
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������� ���������� ���������� ���������', '������������� ���������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '��������� ��� ���������� ���������', '��������� ��� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ����������� ���������� ���������', '����������� ����������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������������� ����� �������� ����� ��� ���������� ���������� ���������', '����������������� ����� �������� ����� ��� ���������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ���������� ��� ���������� ���������', '������������ ���������� ��� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ��������������� � ����������� �������� ���������� ���������', '������������ ��������������� � ����������� �������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ����������� ��� ���������� ���������', '������������ ����������� ��� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ �������� ���������� ���������', '������������ �������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ������� ���������� ���������', '������������ ������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ���������� ��������� �� ����� ����������� �������, ������������ ��������� � ���������� ����������� ��������� ��������', '������������ ���������� ��������� �� ������������ ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ��������� ��������� ���������� ���������', '������������ ��������� ��������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ������, ������� � ���������� �������� ���������� ���������', '������������ ������, ������� � ���������� �������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ �������� ���������� ���������', '������������ �������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ �������������� �������� ���������� ���������', '������������ �������������� �������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������ ������� ���������� ���������', '������������ ������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '������������� ���������� ���������', '������������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '���������� �������� ����', '���������� �������� ����', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '���������� �������� ���������', '���������� �������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '���������� ������ ���������� ���������� ���������', '���������� ������ ���������� ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ������ ������������ ���������� ���������', '����������� ������ ������������ ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ������ ���������� ��������� ', '����������� ������ ���������� ��������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ������ ������ ���������� ���������', '����������� ������ ������ ���������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ������ �� ����������������� � ����������� ���������� �����', '����������� ������ �� ����������������� � ����������� ���������� �����', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ���������� ������ ', '����������� ���������� ������ ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� ���������� ���������� ', '����������� ��������� ���������� ���������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� ���������������� ���������� ', '����������� ��������� ���������������� ���������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� ������� ���������', '����������� ��������� ������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �������� � ������� ���������� ', '����������� ��������� �������� � ������� ���������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� ��������������� �������� ', '����������� ��������� �� ��������������� �������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� ��������������� � ����������� ��������', '����������� ��������� �� ��������������� � ����������� ��������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� �������� � ��������������', '����������� ��������� �� �������� � ��������������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� �����������', '����������� ��������� �� �����������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� ������ � �������� �������������', '����������� ��������� �� ������ � �������� �������������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� ����������� ', '����������� ��������� �� ����������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� ��������� ��������� ', '����������� ��������� �� ��������� ��������� ', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �� ������������� � �������-������������� ���������', '����������� ��������� �� ������������� � �������-������������� ���������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� �����', '����������� ��������� �����', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ��������� ������������ �������������', '����������� ��������� ������������ �������������', 77, 6, 9, 0, 0, '������')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), '����������� ������-������������� ���������', '����������� ������-������������� ���������', 77, 6, 9, 0, 0, '������')
GO

UPDATE Organization2010
SET DepartmentId = T1.DepartmentId
FROM (SELECT 
		O.Id,
		O1.Id as DepartmentId
	  FROM Organization2010 O
		inner join Organization2010 O1 on O.OwnerDepartment = O1.FullName
	  WHERE O.TypeId <> 6) T1
WHERE Organization2010.Id=T1.Id
GO

ALTER TABLE [dbo].[Organization2010]
ADD CONSTRAINT [fk_OrzanizationDepartmentId] FOREIGN KEY ([DepartmentId]) 
  REFERENCES [dbo].[Organization2010] ([Id]) 
  ON UPDATE NO ACTION
  ON DELETE NO ACTION
GO


					
					
update Organization2010
set DepartmentId = T1.DepartmentId
from (select O.Id , O1.Id as DepartmentId
					from Organization2010 O
						inner join Organization2010 O1 on O.OwnerDepartment = O1.FullName
					where O.TypeId <> 6) T1
where Organization2010.Id=T1.Id
GO

update Organization2010
set 
UpdateDate = (select MAX(O.UpdateDate)
		from Organization2010 O
		where O.TypeId <> 6)
where TypeId = 6
GO