-- Форма обучения: очная. Источник финансирования - бюджет
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	11,
	14,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresBudgetO = 1

-- Форма обучения: очно-заочная. Источник финансирования - бюджет
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	12,
	14,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresBudgetOZ = 1

-- Форма обучения: заочная. Источник финансирования - бюджет
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	10,
	14,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresBudgetZ = 1

-- Форма обучения: очная. Источник финансирования - платные
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	11,
	15,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresPaidO = 1

-- Форма обучения: очно-заочная. Источник финансирования - платные
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	12,
	15,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresPaidOZ = 1

-- Форма обучения: заочная. Источник финансирования - платные
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	10,
	15,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresPaidZ = 1

-- Форма обучения: очная. Источник финансирования - целевой приём
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	11,
	16,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresTargetO = 1 and kgt.IsForO = 1

-- Форма обучения: очно-заочная. Источник финансирования - целевой приём
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	12,
	16,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresTargetOZ = 1 and kgt.IsForOZ = 1

-- Форма обучения: заочная. Источник финансирования - целевой приём
insert into ApplicationCompetitiveGroupItem
(ApplicationId, CompetitiveGroupId, CompetitiveGroupItemId, CompetitiveGroupTargetId, EducationFormId, EducationSourceId, Priority)
select 
	app.ApplicationID,
	kg.CompetitiveGroupID,
	kgi.CompetitiveGroupItemID,
	kgt.CompetitiveGroupTargetID,
	10,
	16,
	0
from Application app
inner join ApplicationSelectedCompetitiveGroup kg on app.ApplicationID = kg.ApplicationID
inner join ApplicationSelectedCompetitiveGroupItem kgi on app.ApplicationID = kgi.ApplicationID
left outer join ApplicationSelectedCompetitiveGroupTarget kgt on app.ApplicationID = kgt.ApplicationID
where app.IsRequiresTargetZ = 1 and kgt.IsForZ = 1
