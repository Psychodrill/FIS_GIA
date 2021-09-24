-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (1, '001__2011_01_03__BatchGUID')
-- =========================================================================


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' AND B.TABLE_NAME = 'BatchGUID'))
  DROP TABLE [dbo].[BatchGUID]
GO

CREATE TABLE [dbo].[BatchGUID] (
  [BatchUniqueId] uniqueidentifier NOT NULL,
  [Id] bigint NOT NULL,
  PRIMARY KEY CLUSTERED ([BatchUniqueId])
)
GO

CREATE UNIQUE INDEX [BatchGUID_uq] ON [dbo].[BatchGUID] 
  ([Id])
GO

IF OBJECT_ID('GetBatchStatusById') IS NOT NULL
DROP PROC GetBatchStatusById
GO

CREATE PROCEDURE [dbo].[GetBatchStatusById]
    @batchUniqueId uniqueidentifier,
    @userLogin varchar(255),
    @isProcess bit output,
    @isCorrect bit output,
    @isFound bit output,
    @searchType int output
AS
BEGIN
    declare @externalId bigint
    declare @internalId bigint
    declare @accountId bigint

    set @isProcess = 0
    set @isCorrect = 0
    set @isFound = 0
    set @searchType = 0
    
    -- Выполняем перекодировку номера пакета из GUID во внешний код
    set @externalId = isnull((select B.Id from dbo.BatchGUID B where B.BatchUniqueId = @batchUniqueId), -1)
    if (@externalId = -1)
    begin
    	return
    end
    
    -- Выполняем поиск кода пользователя
    set @accountId = isnull((select A.Id from dbo.Account A where A.[Login] = @userLogin), -1)
    if (@accountId = -1)
    begin
    	return
    end

    -- Выполняем поиск сертификата по внешнему коду
    select 
    	@isProcess = isnull(C.IsProcess, 0),
        @isCorrect = isnull(C.IsCorrect, 1),
        @searchType = C.SearchType,
        @isFound = 1
    from 
    	(
        select CB.Id, CB.IsProcess, CB.IsCorrect, 1 SearchType
        from CommonNationalExamCertificateCheckBatch CB
        where CB.OwnerAccountId = @accountId
        union all
        select RB.Id, RB.IsProcess, RB.IsCorrect, (case RB.IsTypographicNumber when 1 then 3 else 2 end) SearchType
	    from CommonNationalExamCertificateRequestBatch RB
        where RB.OwnerAccountId = @accountId
        ) C
    where 
    	C.Id = dbo.GetInternalId(@externalId)
END
GO