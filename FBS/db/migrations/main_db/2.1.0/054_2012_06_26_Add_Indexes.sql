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
-- Запись информации о текущей миграции в лог
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
