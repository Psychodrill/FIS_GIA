/*
   30 июня 2014 г.11:17:43
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
ALTER TABLE dbo.Application SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Application', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Application', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Application', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Direction SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Direction', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Direction', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Direction', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompetitiveGroup SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.CompetitiveGroup', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.CompetitiveGroup', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.CompetitiveGroup', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Campaign SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Campaign', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Campaign', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Campaign', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Institution SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Institution', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Institution', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Institution', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.RecomendedLists
	(
	RecListID int NOT NULL IDENTITY (1, 1),
	InstitutionID int NOT NULL,
	CampaignID int NOT NULL,
	EduLevelID int NOT NULL,
	EduFormID int NOT NULL,
	CompetitiveGroupID int NOT NULL,
	DirectionID int NOT NULL,
	Stage tinyint NOT NULL,
	ApplicationID int NOT NULL,
	Rating decimal(7, 4) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.RecomendedLists ADD CONSTRAINT
	PK_RecomendedLists PRIMARY KEY CLUSTERED 
	(
	RecListID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_RecomendedLists_InstitutionID ON dbo.RecomendedLists
	(
	InstitutionID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RecomendedLists_CampaignID ON dbo.RecomendedLists
	(
	CampaignID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RecomendedLists_CompetitiveGroupID ON dbo.RecomendedLists
	(
	CompetitiveGroupID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_RecomendedLists_DirectionID ON dbo.RecomendedLists
	(
	DirectionID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.RecomendedLists ADD CONSTRAINT
	FK_RecomendedLists_Institution1 FOREIGN KEY
	(
	InstitutionID
	) REFERENCES dbo.Institution
	(
	InstitutionID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecomendedLists ADD CONSTRAINT
	FK_RecomendedLists_Campaign1 FOREIGN KEY
	(
	CampaignID
	) REFERENCES dbo.Campaign
	(
	CampaignID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecomendedLists ADD CONSTRAINT
	FK_RecomendedLists_CompetitiveGroup1 FOREIGN KEY
	(
	CompetitiveGroupID
	) REFERENCES dbo.CompetitiveGroup
	(
	CompetitiveGroupID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecomendedLists ADD CONSTRAINT
	FK_RecomendedLists_Direction1 FOREIGN KEY
	(
	DirectionID
	) REFERENCES dbo.Direction
	(
	DirectionID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecomendedLists ADD CONSTRAINT
	FK_RecomendedLists_Application1 FOREIGN KEY
	(
	ApplicationID
	) REFERENCES dbo.Application
	(
	ApplicationID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecomendedLists SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecomendedLists', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecomendedLists', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecomendedLists', 'Object', 'CONTROL') as Contr_Per 