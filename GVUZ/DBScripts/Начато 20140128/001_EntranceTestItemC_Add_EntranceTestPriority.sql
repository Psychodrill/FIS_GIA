/*
   28 января 2014 г.17:07:36
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
ALTER TABLE dbo.EntranceTestItemC ADD
	EntranceTestPriority int NULL
GO
ALTER TABLE dbo.EntranceTestItemC SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.EntranceTestItemC', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.EntranceTestItemC', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.EntranceTestItemC', 'Object', 'CONTROL') as Contr_Per 