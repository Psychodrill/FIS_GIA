/*
   16 апреля 2014 г.16:35:57
   Пользователь: scholar
   Сервер: 10.32.200.164
   База данных: gvuz_tags
   Приложение: 
*/

/* Чтобы предотвратить возможность потери данных, необходимо внимательно просмотреть этот сценарий, прежде чем запускать его вне контекста конструктора баз данных.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.ApplicationCompetitiveGroupItem
	(
	id int NOT NULL IDENTITY (1, 1),
	ApplicationId int NOT NULL,
	CompetitiveGroupId int NOT NULL,
	CompetitiveGroupItemId int NOT NULL,
	EducationFormId int NOT NULL,
	EducationSourceId int NOT NULL,
	Priority int NULL,
	CompetitiveGroupTargetId int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem ADD CONSTRAINT
	CK_AppItem_EduForm CHECK (EducationFormId = 10 
OR EducationFormId = 11
OR EducationFormId = 12)
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem ADD CONSTRAINT
	CK_AppItem_EduSource CHECK (EducationSourceId = 14
OR EducationSourceId = 15
OR EducationSourceId = 16
OR EducationSourceId = 20)
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem ADD CONSTRAINT
	PK_ApplicationCompetitiveGroupItem PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationCompetitiveGroupItem', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationCompetitiveGroupItem', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationCompetitiveGroupItem', 'Object', 'CONTROL') as Contr_Per 