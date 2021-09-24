if not exists (select * from DocumentType where Name = 'Диплом об окончании аспирантуры (адъюнкатуры)')
	insert into DocumentType
	(DocumentID, Name)
	Values(25, 'Диплом об окончании аспирантуры (адъюнкатуры)')


if not exists (select * from DocumentType where Name = 'Диплом кандидата наук')
	insert into DocumentType
	(DocumentID, Name)
	Values(26, 'Диплом кандидата наук')