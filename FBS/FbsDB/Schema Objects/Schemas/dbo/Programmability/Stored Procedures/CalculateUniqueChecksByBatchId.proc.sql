
alter procedure [dbo].[CalculateUniqueChecksByBatchId]
	@batchId bigint,
    @checkType varchar(200)
as
begin
	-- =========================================================================
	-- Передается код пакета и по этому коду определяется какие проверки 
	-- были выполнены в рамках этого пакета, какие свидетельства были найдены
	-- По успешно найденным свидетельствам выполняется перерасчет количества
	-- уникальных проверок
	-- =========================================================================

	-- Описание входных параметров
    --
	-- @batchId - код пакета. По хорошему надо сделать его GUID и тогда проверять можно 
    --            без указатия  указания типа проверки. А пока он bigint то код пакетов
    --            по разным типам проверок могут пересекаться
    -- @checkType - тип проверки: 'passport_or_typo' - по паспорту или типографскому номеру,
    --              'certificate' - по номеру сертификата
    
    -- 1. Объявляем 'константы'
    declare @passport_or_typo varchar(100)
    set @passport_or_typo = 'passport_or_typo'
    	
    declare @certificate varchar(100)
    set @certificate = 'certificate'

	-- 2. Объявляем переменные
    declare @organizationId bigint
    set @organizationId = 0
    
    -- Используется в курсоре. В эту переменную пишется код свидетельства
    declare @CIdGuid uniqueidentifier 
    set @CIdGuid = null

	-- 3. По коду пакета определяем организацию, от которой выполнялась пакетная проверка        
    -- 
	-- Для начала надо понять, от имени какой организации выполнялась пакетная проверка, т.к.
    -- количество уникальных проверок - это сколько организаций проверило данное свидетельство.
    -- Существует три вида проверок. Информация о двух из них (по паспорту и типографскому 
    -- номеру) хранится в таблице CommonNationalExamCertificateRequestBatch, информация по 
    -- оставшейся (по номеру свидетельства) хранится в таблице 
    -- CommonNationalExamCertificateCheckBatch.
    --
    -- 3.1. По паспорту или типографскому номеру
    if (@checkType = @passport_or_typo)
    begin
	      set @organizationId = 
	          ISNULL(
	              (select 
	                  top 1 
	                  A.OrganizationId 
	              from 
	                  Account A with(nolock)
	              where A.Id = 
	                  (select 
	                      top 1 ERB.OwnerAccountId 
	                  from 
	                      CommonNationalExamCertificateRequestBatch ERB with(nolock) 
	                  where 
	                      ERB.Id = @batchId))
	              , 0
	        )
	end
        
    -- 3.2. По номеру свидетельства
    if (@checkType = @certificate)
    begin
	      set @organizationId = 
	          ISNULL(
	              (select 
	                  top 1 
	                  A.OrganizationId 
	              from 
	                  Account A with(nolock)
	              where A.Id = 
	                  (select 
	                      top 1 ECB.OwnerAccountId 
	                  from 
	                      CommonNationalExamCertificateCheckBatch ECB with(nolock)
	                  where 
	                      ECB.Id = @batchId))
	              , 0
	        )
	end
    
    -- 3.3. Организацию не нашли (или тип поиска был задан неверно) то выходим с кодом 0
    if (@organizationId = 0)
    	return 0
        
    
    -- 4. Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    
    -- 4.1. Выполняем действия для проверок по паспорту или типографскому номеру
    if (@checkType = @passport_or_typo)
    begin              
    	-- Отбираем все найденные свидетельства
	    declare db_cursor cursor for
	    select
	        distinct S.SourceCertificateIdGuid
	    from 
	        CommonNationalExamCertificateRequest S with(nolock)
	    where
	        S.SourceCertificateIdGuid is not null
            and S.BatchId = @batchId
		
        -- Для каждого найденного свидетельства вызываем хранимую процедуру подсчета проверок
	    open db_cursor   
	    fetch next from db_cursor INTO @CIdGuid   
	    while @@FETCH_STATUS = 0   
	    begin
	        exec dbo.ExecuteChecksCount
	            @OrganizationId = @organizationId,
	            @CertificateIdGuid = @CIdGuid
	        fetch next from db_cursor into @CIdGuid
	    end
        
	    close db_cursor   
	    deallocate db_cursor
    end
    
    -- 4.2. Выполняем действия для проверок по номеру свидетельства
    if (@checkType = @certificate)
    begin
    	-- Отбираем все найденные свидетельства
	    declare db_cursor cursor for
	    select
	        distinct S.SourceCertificateIdGuid
	    from 
	        CommonNationalExamCertificateCheck S with(nolock)
	    where
	        S.SourceCertificateIdGuid is not null
            and S.BatchId = @batchId
		
        -- Для каждого найденного свидетельства вызываем хранимую процедуру подсчета проверок
	    open db_cursor   
	    fetch next from db_cursor INTO @CIdGuid   
	    while @@FETCH_STATUS = 0   
	    begin
	        exec dbo.ExecuteChecksCount
	            @OrganizationId = @organizationId,
	            @CertificateIdGuid = @CIdGuid
	        fetch next from db_cursor into @CIdGuid
	    end
        
	    close db_cursor   
	    deallocate db_cursor
    end

	return 1
end
