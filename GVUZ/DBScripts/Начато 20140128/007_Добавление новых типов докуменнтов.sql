if not exists (select * from DocumentType where Name = '������ �� ��������� ����������� (�����������)')
	insert into DocumentType
	(DocumentID, Name)
	Values(25, '������ �� ��������� ����������� (�����������)')


if not exists (select * from DocumentType where Name = '������ ��������� ����')
	insert into DocumentType
	(DocumentID, Name)
	Values(26, '������ ��������� ����')