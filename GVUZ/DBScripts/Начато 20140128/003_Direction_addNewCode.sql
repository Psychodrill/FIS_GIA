/*
   11 февраля 2014 г.15:41:46
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

if not exists (select * from syscolumns where name = N'NewCode' and id = object_id(N'Direction'))
	ALTER TABLE dbo.Direction ADD
		NewCode char(8) NULL
GO
ALTER Table dbo.Direction
Alter column Code char(6) Null
GO
COMMIT
