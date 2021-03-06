/*
   5 июня 2014 г.16:35:12
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
ALTER TABLE dbo.BenefitItemSubject
	DROP CONSTRAINT CK_BenefitItemSubject_EgeMinValue
GO
ALTER TABLE dbo.BenefitItemSubject ADD CONSTRAINT
	CK_BenefitItemSubject_EgeMinValue CHECK (([EgeMinValue]>=(0) AND [EgeMinValue]<=(100)))
GO
ALTER TABLE dbo.BenefitItemSubject SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
