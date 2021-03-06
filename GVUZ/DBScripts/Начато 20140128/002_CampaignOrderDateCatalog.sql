/*
   3 февраля 2014 г.11:27:34
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
CREATE TABLE dbo.CampaignOrderDateCatalog
	(
	YearStart int NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL,
	TargetOrderDate date NOT NULL,
	Stage1OrderDate date NOT NULL,
	Stage2OrderDate date NOT NULL,
	PaidOrderDate date NOT NULL,
	PreviousUseDepth int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.CampaignOrderDateCatalog ADD CONSTRAINT
	PK_CampaignOrderDateCatalog PRIMARY KEY CLUSTERED 
	(
	YearStart
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CampaignOrderDateCatalog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.CampaignOrderDateCatalog', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.CampaignOrderDateCatalog', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.CampaignOrderDateCatalog', 'Object', 'CONTROL') as Contr_Per 