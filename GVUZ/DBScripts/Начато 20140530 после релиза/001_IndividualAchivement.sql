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
ALTER TABLE dbo.EntrantDocument SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Application SET (LOCK_ESCALATION = TABLE)
GO

if not exists (select * from sysobjects where id = OBJECT_ID(N'IndividualAchivement'))
begin
	CREATE TABLE dbo.IndividualAchivement
		(
		IAID int NOT NULL IDENTITY (1, 1),
		ApplicationID int NOT NULL,
		IAUID varchar(50) NULL,
		IAName varchar(100) NOT NULL,
		IAMark decimal(7, 4) NULL,
		EntrantDocumentID int NOT NULL,
		CreatedDate datetime NULL,
		ModifiedDate datetime NULL
		)  ON [PRIMARY]
	ALTER TABLE dbo.IndividualAchivement ADD CONSTRAINT
		DF_IndividualAchivement_CreatedDate DEFAULT getdate() FOR CreatedDate
	ALTER TABLE dbo.IndividualAchivement ADD CONSTRAINT
		PK_IndividualAchivement PRIMARY KEY CLUSTERED 
		(
		IAID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX IX_IndividualAchivement_AppID ON dbo.IndividualAchivement
		(
		ApplicationID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX IX_IndividualAchivement_EntrandDocumentID ON dbo.IndividualAchivement
		(
		EntrantDocumentID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	ALTER TABLE dbo.IndividualAchivement ADD CONSTRAINT
		FK_IndividualAchivement_Application1 FOREIGN KEY
		(
		ApplicationID
		) REFERENCES dbo.Application
		(
		ApplicationID
		) ON UPDATE  NO ACTION 
		 ON DELETE  NO ACTION 
	ALTER TABLE dbo.IndividualAchivement ADD CONSTRAINT
		FK_IndividualAchivement_EntrantDocument1 FOREIGN KEY
		(
		EntrantDocumentID
		) REFERENCES dbo.EntrantDocument
		(
		EntrantDocumentID
		) ON UPDATE  NO ACTION 
		 ON DELETE  NO ACTION 
	ALTER TABLE dbo.IndividualAchivement SET (LOCK_ESCALATION = TABLE)
end
COMMIT
