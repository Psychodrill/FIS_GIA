/*
   11 апреля 2014 г.14:17:11
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
ALTER TABLE dbo.AdmissionVolume ADD
	NumberQuotaO int NULL,
	NumberQuotaOZ int NULL,
	NumberQuotaZ int NULL
GO
ALTER TABLE dbo.AdmissionVolume SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AdmissionVolume', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AdmissionVolume', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AdmissionVolume', 'Object', 'CONTROL') as Contr_Per 