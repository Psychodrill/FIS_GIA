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

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgOnInsertCommonNationalExamCertificateCheck]'))
DROP TRIGGER [dbo].[tgOnInsertCommonNationalExamCertificateCheck]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- =============================================
-- ������ �� ���������� ������ � 
-- dbo.CommonNationalExamCertificateCheck
-- v.1.0: Created by Sedov Anton 22.07.2008
-- v.1.1: Modified by Fomin Dmitriy 11.09.2008
-- ���������.
-- =============================================
CREATE trigger dbo.tgOnInsertCommonNationalExamCertificateCheck
on dbo.CommonNationalExamCertificateCheck
for insert 
as
begin
	insert into dbo.CheckCommonNationalExamCertificateLog
		(
		Date
		, AccountId
		, CertificateNumber
		, IsOriginal
		, IsBatch
		, IsExist
		)
	select		
		cne_check_batch.CreateDate
		, cne_check_batch.OwnerAccountId AccountId
		, inserted_cne_check.CertificateNumber CertificateNumber
		, inserted_cne_check.IsOriginal
        , 1 IsBatch
		, case
			when not inserted_cne_check.CertificateNumber is null then 1
			else 0
		end
	from 
		Inserted inserted_cne_check
			inner join dbo.CommonNationalExamCertificateCheckBatch cne_check_batch
				on inserted_cne_check.BatchId = cne_check_batch.Id
	where exists(select * from Account a join Organization2010 b on a.OrganizationId=b.id and b.DisableLog=0 and a.id=cne_check_batch.OwnerAccountId)
end
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tgCheckCommonNationalExamCertificateLog]'))
DROP TRIGGER [dbo].[tgCheckCommonNationalExamCertificateLog]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO


-- ================================================
-- ������ �� ���������� ������� CNE_CHK � EventLog
-- v.1.0: Created by Sedov Anton 22.07.2008
-- v.1.1: Modified by Fomin Dmitriy 11.09.2008
-- ���������.
-- v.1.2: Modified by Valeev Denis 20.05.2009
-- �����������
-- ================================================
create trigger [dbo].[tgCheckCommonNationalExamCertificateLog]
on [dbo].[EventLog] 
for insert
as 
begin
	insert into dbo.CheckCommonNationalExamCertificateLog
		(
		Date
		, AccountId
		, CertificateNumber
		, IsBatch
		, IsExist
		) 
	select 
		inserted_event.Date Date
		, inserted_event.AccountId AccountId
		, dbo.GetEventParam(inserted_event.EventParams, 1) CertificateNumber
		, 0 IsBatch
		, case
			when not inserted_event.SourceEntityId is null then 1
			else 0
		end IsExist 
	from 
		Inserted inserted_event
	where 
		inserted_event.EventCode = 'CNE_CHK'
		and exists(select * from Account a join Organization2010 b on a.OrganizationId=b.id and b.DisableLog=0 and a.id=inserted_event.AccountId)
end
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (53, '053_2012_06_25_Modify_Triggers.sql')
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
