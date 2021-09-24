SET CONCAT_NULL_YIELDS_NULL, --��������� �������������� ����������� ����������� � ���� �������� NULL ��� ������ ��������� ��������.
ANSI_NULLS, --������ ����������� �� ���������� ISO ��������� ���������� ��������� ������ (=) � ��� ����� (<>) ��� ������������� �� ���������� NULL.
ANSI_PADDING, --������������ ������ �������� � ������� �������� ������, ��� ������������ ������ �������, � ������ �������� � ������� ��������, ������� ���������� �������, � ������ char, varchar, binary � varbinary.
QUOTED_IDENTIFIER, --���������� SQL Server ��������� �������� ISO ������������ ���������� ��������� ��������������� � �����-���������. ��������������, ����������� � ������� �������, ����� ���� ���� ������������������ ��������� ������� Transact-SQL, ���� ����� ��������� �������, ������� ������ ��������� ��������� ���������� ��� ��������������� Transact-SQL.
ANSI_WARNINGS, --������ ��������� � ������������ �� ���������� ISO ��� ��������� ������� ������.
ARITHABORT, --��������� ������, ���� �� ����� ��� ���������� �������� ������ ������������ ��� ������� �� ����.
XACT_ABORT ON --���������, ��������� �� SQL Server �������������� ����� ������� ����������, ���� ���������� ����� Transact-SQL �������� ������ ����������.
SET NUMERIC_ROUNDABORT, --������ ������� ������� �� �������, ����������� � ��� �������, ����� ���������� � ��������� �������� � ������ ��������.
IMPLICIT_TRANSACTIONS OFF --������������� ��� ���������� ����� ������� ����������.
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
/*
SERIALIZABLE
��������� ���������.
���������� �� ����� ��������� ������, ������� ���� �������� ������� ������������, �� ��� �� ���� �������������.
������ ���������� �� ����� �������� ������, ����������� ������� �����������, �� �� ����������.
������ ���������� �� ����� ��������� ����� ������ �� ���������� �����, ������� ������ � �������� ������, ����������� ������������ ������� ����������, �� �� ����������.

���������� ��������� ��������������� � ��������� �������� �����, ��������������� �������� ������ ����� ����������, ����������� �� ����� ����������. 
���������� � ������� �����, ��������������� ����������� ������� ����������, ����������� ��� ������ ����������. ��� �����������, ��� ���� �����-���� ���������� ���������� ����������� ��������, 
��� ����� ��������� ��� �� ����� ����� �����. ���������� ��������� ����������� �� ���������� ����������. ��� ����� ������� ������� ��������, ��������� �� ��������� ����� ��������� ������ � 
��������� ���������� �� ���������� ����������. ��-�� ������� ������������ ���� �������� ������������� ������������ ������ ��� �������������. ���� �������� ��������� ��� ��, 
��� � ��������� HOLDLOCK ���� ������ �� ���� ����������� SELECT � ����������.
*/
GO
BEGIN TRANSACTION
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'IX_CommonNationalExamCertificate_InternalPassportSeria')
DROP INDEX [IX_CommonNationalExamCertificate_InternalPassportSeria] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificate_InternalPassportSeria] ON [dbo].[CommonNationalExamCertificate] 
(
	[InternalPassportSeria] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'IX_CommonNationalExamCertificate_PassportNumber')
DROP INDEX [IX_CommonNationalExamCertificate_PassportNumber] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificate_PassportNumber] ON [dbo].[CommonNationalExamCertificate] 
(
	[PassportNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificate]') AND name = N'IX_CommonNationalExamCertificate_Seria_Number')
DROP INDEX [IX_CommonNationalExamCertificate_Seria_Number] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO
CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificate_Seria_Number] ON [dbo].[CommonNationalExamCertificate] 
(
	[InternalPassportSeria] ASC
)
INCLUDE ( [PassportNumber]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (54, '054_2012_06_26_Add_Indexes.sql')
-- =========================================================================
GO

IF @@TRANCOUNT>0 BEGIN
	PRINT 'The database update succeeded.'
	--rollback TRANSACTION
	COMMIT TRANSACTION
	end
ELSE PRINT 'The database update failed.'
GO
SET NOEXEC OFF
GO
