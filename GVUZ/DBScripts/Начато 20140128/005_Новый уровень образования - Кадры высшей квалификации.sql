if not exists (select * from AdmissionItemType where ItemTypeID = 18 and Name = '����� ������ ������������')
	insert into AdmissionItemType
	(ItemTypeID, Name, ItemLevel, CanBeSkipped, AutoCopyName, DisplayOrder)
	Values(18, '����� ������ ������������', 2, 0, 1, 26)