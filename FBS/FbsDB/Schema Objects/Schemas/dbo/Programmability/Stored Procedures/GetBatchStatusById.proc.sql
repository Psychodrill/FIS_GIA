
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
