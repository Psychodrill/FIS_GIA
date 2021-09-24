-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (25, '025__2011_05_21__CalculateUniqueChecksByBatchId')
-- =========================================================================


IF OBJECT_ID('CalculateUniqueChecksByBatchId') IS NOT NULL
DROP PROC CalculateUniqueChecksByBatchId
GO

create procedure [dbo].[CalculateUniqueChecksByBatchId]
	@batchId bigint,
    @checkType varchar(200)
as
begin
	-- =========================================================================
	-- ���������� ��� ������ � �� ����� ���� ������������ ����� �������� 
	-- ���� ��������� � ������ ����� ������, ����� ������������� ���� �������
	-- �� ������� ��������� �������������� ����������� ���������� ����������
	-- ���������� ��������
	-- =========================================================================

	-- �������� ������� ����������
    --
	-- @batchId - ��� ������. �� �������� ���� ������� ��� GUID � ����� ��������� ����� 
    --            ��� ��������  �������� ���� ��������. � ���� �� bigint �� ��� �������
    --            �� ������ ����� �������� ����� ������������
    -- @checkType - ��� ��������: 'passport_or_typo' - �� �������� ��� ������������� ������,
    --              'certificate' - �� ������ �����������
    
    -- 1. ��������� '���������'
    declare @passport_or_typo varchar(100)
    set @passport_or_typo = 'passport_or_typo'
    	
    declare @certificate varchar(100)
    set @certificate = 'certificate'

	-- 2. ��������� ����������
    declare @organizationId bigint
    set @organizationId = 0
    
    -- ������������ � �������. � ��� ���������� ������� ��� �������������
    declare @CId bigint 
    set @CId = 0

	-- 3. �� ���� ������ ���������� �����������, �� ������� ����������� �������� ��������        
    -- 
	-- ��� ������ ���� ������, �� ����� ����� ����������� ����������� �������� ��������, �.�.
    -- ���������� ���������� �������� - ��� ������� ����������� ��������� ������ �������������.
    -- ���������� ��� ���� ��������. ���������� � ���� �� ��� (�� �������� � ������������� 
    -- ������) �������� � ������� CommonNationalExamCertificateRequestBatch, ���������� �� 
    -- ���������� (�� ������ �������������) �������� � ������� 
    -- CommonNationalExamCertificateCheckBatch.
    --
    -- 3.1. �� �������� ��� ������������� ������
    if (@checkType = @passport_or_typo)
    begin
	      set @organizationId = 
	          ISNULL(
	              (select 
	                  top 1 
	                  A.OrganizationId 
	              from 
	                  Account A 
	              where A.Id = 
	                  (select 
	                      top 1 ERB.OwnerAccountId 
	                  from 
	                      CommonNationalExamCertificateRequestBatch ERB 
	                  where 
	                      ERB.Id = @batchId))
	              , 0
	        )
	end
        
    -- 3.2. �� ������ �������������
    if (@checkType = @certificate)
    begin
	      set @organizationId = 
	          ISNULL(
	              (select 
	                  top 1 
	                  A.OrganizationId 
	              from 
	                  Account A 
	              where A.Id = 
	                  (select 
	                      top 1 ECB.OwnerAccountId 
	                  from 
	                      CommonNationalExamCertificateCheckBatch ECB 
	                  where 
	                      ECB.Id = @batchId))
	              , 0
	        )
	end
    
    -- 3.3. ����������� �� ����� (��� ��� ������ ��� ����� �������) �� ������� � ����� 0
    if (@organizationId = 0)
    	return 0
        
    
    -- 4. ��� ������� ���������� ����������� �������� �������� ��������� �������� ��������
    
    -- 4.1. ��������� �������� ��� �������� �� �������� ��� ������������� ������
    if (@checkType = @passport_or_typo)
    begin
    	-- �������� ��� ��������� �������������
	    declare db_cursor cursor for
	    select
	        distinct S.SourceCertificateId
	    from 
	        CommonNationalExamCertificateRequest S
	    where
	        S.SourceCertificateId is not null
            and S.BatchId = @batchId
		
        -- ��� ������� ���������� ������������� �������� �������� ��������� �������� ��������
	    open db_cursor   
	    fetch next from db_cursor INTO @CId   
	    while @@FETCH_STATUS = 0   
	    begin
	        exec dbo.ExecuteChecksCount
	            @OrganizationId = @organizationId,
	            @CertificateId = @CId
	        fetch next from db_cursor into @CId
	    end
        
	    close db_cursor   
	    deallocate db_cursor
    end
    
    -- 4.2. ��������� �������� ��� �������� �� ������ �������������
    if (@checkType = @certificate)
    begin
    	-- �������� ��� ��������� �������������
	    declare db_cursor cursor for
	    select
	        distinct S.SourceCertificateId
	    from 
	        CommonNationalExamCertificateCheck S
	    where
	        S.SourceCertificateId is not null
            and S.BatchId = @batchId
		
        -- ��� ������� ���������� ������������� �������� �������� ��������� �������� ��������
	    open db_cursor   
	    fetch next from db_cursor INTO @CId   
	    while @@FETCH_STATUS = 0   
	    begin
	        exec dbo.ExecuteChecksCount
	            @OrganizationId = @organizationId,
	            @CertificateId = @CId
	        fetch next from db_cursor into @CId
	    end
        
	    close db_cursor   
	    deallocate db_cursor
    end

	return 1
end
GO