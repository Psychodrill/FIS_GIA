SET CONCAT_NULL_YIELDS_NULL, --Управляет представлением результатов объединения в виде значений NULL или пустых строковых значений.
ANSI_NULLS, --Задает совместимое со стандартом ISO поведение операторов сравнения «равно» (=) и «не равно» (<>) при использовании со значениями NULL.
ANSI_PADDING, --Контролирует способ хранения в столбце значений короче, чем определенный размер столбца, и способ хранения в столбце значений, имеющих замыкающие пробелы, в данных char, varchar, binary и varbinary.
QUOTED_IDENTIFIER, --Заставляет SQL Server следовать правилам ISO относительно разделения кавычками идентификаторов и строк-литералов. Идентификаторы, заключенные в двойные кавычки, могут быть либо зарезервированными ключевыми словами Transact-SQL, либо могут содержать символы, которые обычно запрещены правилами синтаксиса для идентификаторов Transact-SQL.
ANSI_WARNINGS, --Задает поведение в соответствии со стандартом ISO для некоторых условий ошибок.
ARITHABORT, --Завершает запрос, если во время его выполнения возникла ошибка переполнения или деления на ноль.
XACT_ABORT ON --Указывает, выполняет ли SQL Server автоматический откат текущей транзакции, если инструкция языка Transact-SQL вызывает ошибку выполнения.
SET NUMERIC_ROUNDABORT, --Задает уровень отчетов об ошибках, создаваемых в тех случаях, когда округление в выражении приводит к потере точности.
IMPLICIT_TRANSACTIONS OFF --Устанавливает для соединения режим неявных транзакций.
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
/*
SERIALIZABLE
Указывает следующее.
Инструкции не могут считывать данные, которые были изменены другими транзакциями, но еще не были зафиксированы.
Другие транзакции не могут изменять данные, считываемые текущей транзакцией, до ее завершения.
Другие транзакции не могут вставлять новые строки со значениями ключа, которые входят в диапазон ключей, считываемых инструкциями текущей транзакции, до ее завершения.

Блокировка диапазона устанавливается в диапазоне значений ключа, соответствующих условиям поиска любой инструкции, выполненной во время транзакции. 
Обновление и вставка строк, удовлетворяющих инструкциям текущей транзакции, блокируется для других транзакций. Это гарантирует, что если какая-либо инструкция транзакции выполняется повторно, 
она будет считывать тот же самый набор строк. Блокировки диапазона сохраняются до завершения транзакции. Это самый строгий уровень изоляции, поскольку он блокирует целые диапазоны ключей и 
сохраняет блокировку до завершения транзакции. Из-за низкого параллелизма этот параметр рекомендуется использовать только при необходимости. Этот параметр действует так же, 
как и настройка HOLDLOCK всех таблиц во всех инструкциях SELECT в транзакции.
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
-- Тригер на добавление записи в 
-- dbo.CommonNationalExamCertificateCheck
-- v.1.0: Created by Sedov Anton 22.07.2008
-- v.1.1: Modified by Fomin Dmitriy 11.09.2008
-- Ускорение.
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
-- Тригер на добавление события CNE_CHK в EventLog
-- v.1.0: Created by Sedov Anton 22.07.2008
-- v.1.1: Modified by Fomin Dmitriy 11.09.2008
-- Ускорение.
-- v.1.2: Modified by Valeev Denis 20.05.2009
-- Оптимизация
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
-- Запись информации о текущей миграции в лог
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
