/*Меняем длину поля DocumentNumber таблицы EntrantDocument*/
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
ALTER TABLE dbo.EntrantDocument
	DROP CONSTRAINT FK_EntrantDocument_DocumentType
GO
ALTER TABLE dbo.DocumentType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocument
	DROP CONSTRAINT FK_EntrantDocument_Attachment
GO
ALTER TABLE dbo.Attachment SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO

ALTER TABLE dbo.ApplicationEntrantDocument
	DROP CONSTRAINT FK_ApplicationEntrantDocument_EntrantDocument
GO
ALTER TABLE dbo.ApplicationEntranceTestDocument
	DROP CONSTRAINT FK_ApplicationEntranceTestDocument_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentCustom
	DROP CONSTRAINT FK_EntrantDocumentCustom_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentEdu
	DROP CONSTRAINT FK_EntrantDocumentEdu_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentIdentity
	DROP CONSTRAINT FK_EntrantDocumentIdentity_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentDisability
	DROP CONSTRAINT FK_EntrantDocumentDisability_EntrantDocument
GO
ALTER TABLE dbo.Entrant
	DROP CONSTRAINT FK_Entrant_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentEge
	DROP CONSTRAINT FK_EntrantDocumentEge_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentEgeAndOlympicSubject
	DROP CONSTRAINT FK_EntrantDocumentEgeAndOlympicSubject_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentOlympic
	DROP CONSTRAINT FK_EntrantDocumentOlympic_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocumentOlympicTotal
	DROP CONSTRAINT FK_EntrantDocumentOlympicTotal_EntrantDocument
GO
ALTER TABLE dbo.EntrantDocument
	DROP CONSTRAINT FK_EntrantDocument_Entrant
GO

ALTER TABLE [dbo].[EntrantDocument]
ALTER COLUMN [DocumentNumber] VARCHAR(100)
GO

ALTER TABLE dbo.EntrantDocument ADD CONSTRAINT
	FK_EntrantDocument_Attachment FOREIGN KEY
	(
	AttachmentID
	) REFERENCES dbo.Attachment
	(
	AttachmentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.EntrantDocument ADD CONSTRAINT
	FK_EntrantDocument_DocumentType FOREIGN KEY
	(
	DocumentTypeID
	) REFERENCES dbo.DocumentType
	(
	DocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentOlympicTotal ADD CONSTRAINT
	FK_EntrantDocumentOlympicTotal_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentOlympicTotal SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentOlympic ADD CONSTRAINT
	FK_EntrantDocumentOlympic_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentOlympic SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentEgeAndOlympicSubject ADD CONSTRAINT
	FK_EntrantDocumentEgeAndOlympicSubject_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentEgeAndOlympicSubject SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentEge ADD CONSTRAINT
	FK_EntrantDocumentEge_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentEge SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Entrant WITH NOCHECK ADD CONSTRAINT
	FK_Entrant_EntrantDocument FOREIGN KEY
	(
	IdentityDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.EntrantDocument ADD CONSTRAINT
	FK_EntrantDocument_Entrant FOREIGN KEY
	(
	EntrantID
	) REFERENCES dbo.Entrant
	(
	EntrantID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Entrant SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentDisability ADD CONSTRAINT
	FK_EntrantDocumentDisability_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentDisability SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentIdentity ADD CONSTRAINT
	FK_EntrantDocumentIdentity_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentIdentity SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentEdu ADD CONSTRAINT
	FK_EntrantDocumentEdu_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentEdu SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.EntrantDocumentCustom ADD CONSTRAINT
	FK_EntrantDocumentCustom_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.EntrantDocumentCustom SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationEntranceTestDocument ADD CONSTRAINT
	FK_ApplicationEntranceTestDocument_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationEntranceTestDocument SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ApplicationEntrantDocument ADD CONSTRAINT
	FK_ApplicationEntrantDocument_EntrantDocument FOREIGN KEY
	(
	EntrantDocumentID
	) REFERENCES dbo.EntrantDocument
	(
	EntrantDocumentID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ApplicationEntrantDocument SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
