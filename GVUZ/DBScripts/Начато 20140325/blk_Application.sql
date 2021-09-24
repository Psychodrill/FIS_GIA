/*
   28 апреля 2014 г.15:16:33
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
ALTER TABLE dbo.blk_Application ADD
	IsRequiresQuotaO bit NOT NULL,
	IsRequiresQuotaOZ bit NOT NULL,
	IsRequiresQuotaZ bit NOT NULL
GO
ALTER TABLE dbo.blk_Application SET (LOCK_ESCALATION = TABLE)
GO
