/*
   30 июня 2014 г.11:21:36
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
ALTER TABLE dbo.RecomendedLists SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecomendedLists', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecomendedLists', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecomendedLists', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.RecomendedListsHistory
	(
	ID int NOT NULL IDENTITY (1, 1),
	RecListID int NOT NULL,
	DateAdd datetime NOT NULL,
	DateDelete datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.RecomendedListsHistory ADD CONSTRAINT
	PK_RecomendedListsHistory PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_RecomendedListsHistory_RecListID ON dbo.RecomendedListsHistory
	(
	RecListID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.RecomendedListsHistory ADD CONSTRAINT
	FK_RecomendedListsHistory_RecomendedLists1 FOREIGN KEY
	(
	RecListID
	) REFERENCES dbo.RecomendedLists
	(
	RecListID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecomendedListsHistory SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RecomendedListsHistory', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RecomendedListsHistory', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RecomendedListsHistory', 'Object', 'CONTROL') as Contr_Per 