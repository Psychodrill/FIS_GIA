if not exists (select * from sysobjects where id = OBJECT_ID(N'GlobalMinEge'))
begin

	Create Table GlobalMinEge
	(
		EgeYear int not null,
		MinEgeScore int not null
	) On [Primary]

	Alter table GlobalMinEge
	Add constraint PK_GlobalMinEge Primary key clustered
	(
		EgeYear
	) On [Primary]

end

GO

if not exists (select * from GlobalMinEge where EgeYear = 2014)

insert into GlobalMinEge
Values (2014, 65) -- Если не 65 - проставить вместо него нужное