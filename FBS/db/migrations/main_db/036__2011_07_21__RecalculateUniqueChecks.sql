-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (36, '036__2011_07_21__RecalculateUniqueChecks')
-- =========================================================================
GO


ALTER PROCEDURE [dbo].[ExecuteChecksCount]
    @OrganizationId bigint,
    @CertificateId bigint
AS
BEGIN
	if (@OrganizationId = 0 or @OrganizationId is null or @CertificateId is null)
		return -1

	-- Выполняла ли данная организация проверку данного сертификата
    declare @isExists bit

	-- Значения инкрементов
	declare @uniqueIHEaFCheck int
	declare @uniqueIHECheck int
	declare @uniqueIHEFCheck int 

	declare @uniqueTSSaFCheck int
	declare @uniqueTSSCheck int
	declare @uniqueTSSFCheck int
    
	declare @uniqueRCOICheck int
	declare @uniqueOUOCheck int
	declare @uniqueFounderCheck int
	declare @uniqueOtherCheck int 
    
    -- Тип организации
    declare @orgType int
    
    -- Является ли организация филлиалом
    declare @isFilial bit

	-- Инициализация переменных
    set @isExists = 0
	set @uniqueIHEaFCheck = 0
	set @uniqueIHECheck = 0
	set @uniqueIHEFCheck = 0
	set @uniqueTSSaFCheck = 0
	set @uniqueTSSCheck = 0
	set @uniqueTSSFCheck = 0
	set @uniqueRCOICheck = 0
	set @uniqueOUOCheck = 0
	set @uniqueFounderCheck = 0
	set @uniqueOtherCheck = 0
    set @orgType = 0
    set @isFilial = 0


    set @isExists = 
    	(
        select count(*) 
        from [dbo].[OrganizationCertificateChecks] OCC 
        where OCC.CertificateId = @CertificateId and OCC.OrganizationId = @OrganizationId
        )

    if (@isExists = 0)
    begin
    	insert into [dbo].[OrganizationCertificateChecks] (CertificateId, OrganizationId)
        values (@CertificateId, @OrganizationId)
        
        select
        	@orgType = isnull(O.TypeId, 0),
            @isFilial = (case when (O.MainId is null) then 0 else 1 end)
        from
        	[dbo].[Organization2010] O
        where
        	O.Id = @OrganizationId

        if (@orgType = 0)
        	return -1

		if (@orgType = 1)
        begin
        	set @uniqueIHEaFCheck = 1
            if (@isFilial = 1)
            	set @uniqueIHEFCheck = 1
            else
            	set @uniqueIHECheck = 1
        end           
        
		if (@orgType = 2)
        begin
        	set @uniqueTSSaFCheck = 1
            if (@isFilial = 1)
            	set @uniqueTSSFCheck = 1
            else
            	set @uniqueTSSCheck = 1
        end           
        
		if (@orgType = 3)
        begin
        	set @uniqueRCOICheck = 1
        end           

		if (@orgType = 4)
        begin
        	set @uniqueOUOCheck = 1
        end           

		if (@orgType = 6)
        begin
        	set @uniqueFounderCheck = 1
        end           

		if (@orgType = 5)
        begin
        	set @uniqueOtherCheck = 1
        end
        
        declare @year int
		set @year = 
			(select top 1 C.[Year]
			from CommonNationalExamCertificate C
			where C.Id = @CertificateId)
					
		if not exists 
			(select top 1 EC.Id
			from [dbo].[ExamCertificateUniqueChecks] EC
			where EC.Id = @CertificateId)
		begin
			insert into [dbo].[ExamCertificateUniqueChecks] 
				(
				[Year], 
				Id, 
				UniqueChecks,
				UniqueIHEaFCheck,
				UniqueIHECheck,
				UniqueIHEFCheck,
				UniqueTSSaFCheck,
				UniqueTSSCheck,
				UniqueTSSFCheck,
				UniqueRCOICheck,
				UniqueOUOCheck,
				UniqueFounderCheck,
				UniqueOtherCheck
				)
			values 
				(
				@year, 
				@CertificateId,
				1,
				@uniqueIHEaFCheck,
				@uniqueIHECheck,
				@uniqueIHEFCheck,
				@uniqueTSSaFCheck,
				@uniqueTSSCheck,
				@uniqueTSSFCheck,
				@uniqueRCOICheck,
				@uniqueOUOCheck,
				@uniqueFounderCheck,
				@uniqueOtherCheck
				)
		end
		else begin
			update 
        		[dbo].[ExamCertificateUniqueChecks]
			set 
				UniqueChecks = UniqueChecks + 1,
				UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
				UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
				UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
				UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
				UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
				UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
				UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
				UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
				UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
				UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
			where
        		Id = @CertificateId
        		and [Year] = @year
        end

        return 1
    end
    else begin
    	return 0
    end
END
go








ALTER function [dbo].[ReportCheckedCNEsBASE](
	)
RETURNS @report TABLE 
(
CNEId BIGINT
,CNENumber NVARCHAR(255)
,OrgId INT
)
AS 
BEGIN
DECLARE @PreReport TABLE
(
	CNEId BIGINT
	,CNENumber NVARCHAR(255)
	,OrgId INT
)
INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateCheck c  ON c.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
INNER JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=0
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=1
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT CNE.Id AS CNEId,CNE.Number AS CNENumber,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CNEWebUICheckLog ChLog ON ChLog.FoundedCNEId=CNE.Id 
INNER JOIN Account Acc ON ChLog.AccountId=Acc.Id 
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @Report
SELECT DISTINCT * FROM @PreReport

RETURN
END
go















delete from OrganizationCertificateChecks
delete from ExamCertificateUniqueChecks


insert into OrganizationCertificateChecks (CertificateId, OrganizationId)
select
	distinct
	A.CertId,
	A.OrgId
from
(
-- Интерактивные проверки (через web-интерфейс)
select 
	WL.EventDate UpdateDate,
	WL.EventDate LogDate,
	O.Id OrgId,
	O.TypeId OrgType,
	O.MainId MainId,
	case
		when WL.CNENumber is not null then 'number'
		when WL.TypographicNumber is not null then 'typo'
		when WL.PassportNumber is not null then 'passport'
		else 'unknown'
	end CheckType,
	WL.FoundedCNEId CertId,
	'interactive' ClientType
from 
	CNEWebUICheckLog WL
	inner join Account A on A.Id = WL.AccountId
	inner join Organization2010 O on O.Id = A.OrganizationId
where
	year(WL.EventDate) = 2011
    
union all

-- Пакетные проверки и проверки через web-сервис
-- По номеру и ФИО
select
	CCB.UpdateDate,
	CCB.UpdateDate LogDate,
	O.Id OrgId,
	O.TypeId OrgType,
	O.MainId MainId,
	'number' CheckType,
	CC.SourceCertificateId CertId,
	'csv' ClientType
from
	CommonNationalExamCertificateCheck CC
	inner join CommonNationalExamCertificateCheckBatch CCB on CCB.Id = CC.BatchId
	inner join Account A on A.Id = CCB.OwnerAccountId
	inner join Organization2010 O on O.Id = A.OrganizationId
where
	year(CCB.UpdateDate) = 2011

union all

-- По паспорту и ФИО + по типографскому номеру и ФИО
select
	CRB.UpdateDate,
	CRB.UpdateDate LogDate,
	O.Id OrgId,
	O.TypeId OrgType,
	O.MainId MainId,
	case
		when CR.TypographicNumber is not null then 'typo'
		else 'passport'
	end CheckType,
	CR.SourceCertificateId CertId,
	'csv' ClientType
from
	CommonNationalExamCertificateRequest CR
	inner join CommonNationalExamCertificateRequestBatch CRB on CRB.Id = CR.BatchId
	inner join Account A on A.Id = CRB.OwnerAccountId
	inner join Organization2010 O on O.Id = A.OrganizationId
where
	year(CRB.UpdateDate) = 2011
) A
where A.CertId is not null and A.OrgId is not null

	

insert into ExamCertificateUniqueChecks
	(
	Id,
	[Year],
    UniqueChecks,
    UniqueIHEaFCheck,
    UniqueIHECheck,
    UniqueIHEFCheck,
    UniqueTSSaFCheck,
    UniqueTSSCheck,
    UniqueTSSFCheck,
    UniqueRCOICheck,
    UniqueOUOCheck,
    UniqueFounderCheck,
    UniqueOtherCheck
    )
select
    OCC.CertificateId,
    CNE.Year,
    count(distinct OCC.OrganizationId) UniqueChecks,
    count(distinct (case when O.TypeId = 1 then O.Id else null end)) UniqueIHEaFChecks,
    count(distinct (case when O.TypeId = 1 and O.MainId is null then O.Id else null end)) UniqueIHEChecks,
    count(distinct (case when O.TypeId = 1 and O.MainId is not null then O.Id else null end)) UniqueIHEFChecks,
    count(distinct (case when O.TypeId = 2 then O.Id else null end)) UniqueTSSaFCheck,
    count(distinct (case when O.TypeId = 2 and O.MainId is null then O.Id else null end)) UniqueTSSChecks,
    count(distinct (case when O.TypeId = 2 and O.MainId is not null then O.Id else null end)) UniqueTSSFChecks,
    count(distinct (case when O.TypeId = 3 then O.Id else null end)) UniqueRCOIChecks,
    count(distinct (case when O.TypeId = 4 then O.Id else null end)) UniqueOUOChecks,
    count(distinct (case when O.TypeId = 6 then O.Id else null end)) UniqueFounderChecks,
    count(distinct (case when O.TypeId = 5 then O.Id else null end)) UniqueOtherChecks
from
    OrganizationCertificateChecks OCC
    inner join CommonNationalExamCertificate CNE on CNE.Id = OCC.CertificateId
    inner join Organization2010 O on O.Id = OCC.OrganizationId
group by
    OCC.CertificateId,
    CNE.Year
order by 
	OCC.CertificateId


go