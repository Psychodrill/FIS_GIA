BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT

ALTER TABLE dbo.Subject SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.BenefitItemC SET (LOCK_ESCALATION = TABLE)
GO

if not exists (select * from sysobjects where id = OBJECT_ID(N'BenefitItemSubject'))
begin

	CREATE TABLE dbo.BenefitItemSubject
		(
		Id int NOT NULL Identity(1,1),
		BenefitItemId int NOT NULL,
		SubjectId int NOT NULL,
		EgeMinValue int NOT NULL
		)  ON [PRIMARY]

	ALTER TABLE dbo.BenefitItemSubject ADD CONSTRAINT
		CK_BenefitItemSubject_EgeMinValue CHECK (EgeMinValue > 0 and EgeMinValue <= 100)

	ALTER TABLE dbo.BenefitItemSubject ADD CONSTRAINT
		PK_BenefitItemSubject PRIMARY KEY CLUSTERED 
		(
		Id
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	ALTER TABLE dbo.BenefitItemSubject ADD CONSTRAINT
		FK_BenefitItemSubject_BenefitItemC1 FOREIGN KEY
		(
		BenefitItemId
		) REFERENCES dbo.BenefitItemC
		(
		BenefitItemID
		) ON UPDATE  NO ACTION 
		 ON DELETE  NO ACTION 
		
	ALTER TABLE dbo.BenefitItemSubject ADD CONSTRAINT
		FK_BenefitItemSubject_Subject1 FOREIGN KEY
		(
		SubjectId
		) REFERENCES dbo.Subject
		(
		SubjectID
		) ON UPDATE  NO ACTION 
		 ON DELETE  NO ACTION 
		
	ALTER TABLE dbo.BenefitItemSubject SET (LOCK_ESCALATION = TABLE)
end
GO