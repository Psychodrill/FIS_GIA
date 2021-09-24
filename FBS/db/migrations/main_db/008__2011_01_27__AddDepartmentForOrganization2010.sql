-- =========================================================================
-- Запись информации о текущей миграции в лог
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
		+''', '+''''+T1.OwnerDepartment+''', '+'77, 6, 9, 0, 0, '+'''Москва'')')
from
	(select distinct O.OwnerDepartment
	from Organization2010 O
	where O.OwnerDepartment not in ('', 'Банк', 'Сам себе', 'Самому себе принадлежит') 
	) T1
order by
	T1.OwnerDepartment*/
	
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Администрация Президента Российской Федерации', 'Администрация Президента Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Верховный Суд Российской Федерации', 'Верховный Суд Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Генеральная прокуратура Российской Федерации', 'Генеральная прокуратура Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Исследовательский центр частного права при Президенте Российской Федерации', 'Исследовательский центр частного права при Президенте Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство внутренних дел Российской Федерации', 'Министерство внутренних дел Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство здравоохранения и социального развития Российской Федерации', 'Министерство здравоохранения и социального развития Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство иностранных дел Российской Федерации', 'Министерство иностранных дел Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство культуры Российской Федерации', 'Министерство культуры Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство обороны Российской Федерации', 'Министерство обороны Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство Российской Федерации по делам гражданской обороны, чрезвычайным ситуациям и ликвидации последствий стихийных бедствий', 'Министерство Российской Федерации по чрезвычайным ситуациям', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство сельского хозяйства Российской Федерации', 'Министерство сельского хозяйства Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство спорта, туризма и молодежной политики Российской Федерации', 'Министерство спорта, туризма и молодежной политики Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство финансов Российской Федерации', 'Министерство финансов Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство экономического развития Российской Федерации', 'Министерство экономического развития Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Министерство юстиции Российской Федерации', 'Министерство юстиции Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Правительство Российской Федерации', 'Правительство Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Российская академия наук', 'Российская академия наук', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Российская Академия художеств', 'Российская Академия художеств', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Управление делами Президента Российской Федерации', 'Управление делами Президента Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральная служба безопасности Российской Федерации', 'Федеральная служба безопасности Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральная служба исполнения наказаний ', 'Федеральная служба исполнения наказаний ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральная служба охраны Российской Федерации', 'Федеральная служба охраны Российской Федерации', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральная служба по гидрометеорологии и мониторингу окружающей среды', 'Федеральная служба по гидрометеорологии и мониторингу окружающей среды', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральная таможенная служба ', 'Федеральная таможенная служба ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство воздушного транспорта ', 'Федеральное агентство воздушного транспорта ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство железнодорожного транспорта ', 'Федеральное агентство железнодорожного транспорта ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство лесного хозяйства', 'Федеральное агентство лесного хозяйства', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство морского и речного транспорта ', 'Федеральное агентство морского и речного транспорта ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по государственным резервам ', 'Федеральное агентство по государственным резервам ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по здравоохранению и социальному развитию', 'Федеральное агентство по здравоохранению и социальному развитию', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по культуре и кинематографии', 'Федеральное агентство по культуре и кинематографии', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по образованию', 'Федеральное агентство по образованию', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по печати и массовым коммуникациям', 'Федеральное агентство по печати и массовым коммуникациям', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по рыболовству ', 'Федеральное агентство по рыболовству ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по сельскому хозяйству ', 'Федеральное агентство по сельскому хозяйству ', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство по строительству и жилищно-коммунальному хозяйству', 'Федеральное агентство по строительству и жилищно-коммунальному хозяйству', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство связи', 'Федеральное агентство связи', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное агентство специального строительства', 'Федеральное агентство специального строительства', 77, 6, 9, 0, 0, 'Москва')
insert into Organization2010 (CreateDate, UpdateDate, FullName, ShortName, RegionId, TypeId, KindId, IsPrivate, IsFilial, FactAddress) values (GETDATE(), GETDATE(), 'Федеральное медико-биологическое агентство', 'Федеральное медико-биологическое агентство', 77, 6, 9, 0, 0, 'Москва')
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