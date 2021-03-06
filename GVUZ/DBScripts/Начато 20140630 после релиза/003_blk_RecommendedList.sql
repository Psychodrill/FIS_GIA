/*
   4 июля 2014 г.16:08:53
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
CREATE TABLE dbo.blk_RecommendedList
	(
	Stage int NOT NULL,
	ApplicationNumber nvarchar(50) NOT NULL,
	RegistrationDate datetime NOT NULL,
	EduLevelId int NOT NULL,
	EduFormId int NOT NULL,
	DirectionId int NOT NULL,
	CompetitiveGroupUID nvarchar(50) NOT NULL,
	Id uniqueidentifier NOT NULL,
	ParentId uniqueidentifier NULL,
	UID varchar(200) NULL,
	ImportPackageId int NOT NULL,
	InstitutionId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.blk_RecommendedList SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.blk_RecommendedList', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.blk_RecommendedList', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.blk_RecommendedList', 'Object', 'CONTROL') as Contr_Per 