/*
   18 февраля 2014 г.13:54:18
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
ALTER TABLE dbo.AllowedDirections
	DROP CONSTRAINT CK_AllowedDirections
GO
ALTER TABLE dbo.AllowedDirections WITH NOCHECK ADD CONSTRAINT
	CK_AllowedDirections CHECK (([AdmissionItemTypeID]=(5) OR [AdmissionItemTypeID]=(4) OR [AdmissionItemTypeID]=(3) OR [AdmissionItemTypeID]=(2) OR [AdmissionItemTypeID]=(17) OR [AdmissionItemTypeID]=(18)))
GO
ALTER TABLE dbo.AllowedDirections SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AllowedDirections', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AllowedDirections', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AllowedDirections', 'Object', 'CONTROL') as Contr_Per 