/*
   16 апреля 2014 г.16:39:27
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
ALTER TABLE dbo.CompetitiveGroupItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.CompetitiveGroupItem', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.CompetitiveGroupItem', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.CompetitiveGroupItem', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompetitiveGroup SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.CompetitiveGroup', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.CompetitiveGroup', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.CompetitiveGroup', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Application SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Application', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Application', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Application', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem ADD CONSTRAINT
	FK_ApplicationCompetitiveGroupItem_Application FOREIGN KEY
	(
	ApplicationId
	) REFERENCES dbo.Application
	(
	ApplicationID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem ADD CONSTRAINT
	FK_ApplicationCompetitiveGroupItem_CompetitiveGroup FOREIGN KEY
	(
	CompetitiveGroupId
	) REFERENCES dbo.CompetitiveGroup
	(
	CompetitiveGroupID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem ADD CONSTRAINT
	FK_ApplicationCompetitiveGroupItem_CompetitiveGroupItem FOREIGN KEY
	(
	CompetitiveGroupItemId
	) REFERENCES dbo.CompetitiveGroupItem
	(
	CompetitiveGroupItemID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationCompetitiveGroupItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ApplicationCompetitiveGroupItem', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ApplicationCompetitiveGroupItem', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ApplicationCompetitiveGroupItem', 'Object', 'CONTROL') as Contr_Per 