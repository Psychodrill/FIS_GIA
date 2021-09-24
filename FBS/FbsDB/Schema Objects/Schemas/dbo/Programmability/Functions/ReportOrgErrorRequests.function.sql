CREATE FUNCTION [dbo].[ReportOrgErrorRequests]
(	
	@periodBegin DATETIME,
    @periodEnd DATETIME
)
RETURNS @report TABLE 
([Наименование организации] nvarchar(4000),
 [Количество запросов] int)

AS begin

IF @periodBegin IS NULL 
 SET @periodBegin = '19000101'

DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @singleWrongCheck (accountId, count)
SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData FROM dbo.EventLog WHERE 
-- это значит что ничего не найдено
SourceEntityId IS NULL
-- '|%|%|' - это значит что поиск по паспорту и оценок нет
AND EventParams LIKE '|%|%|' 
AND AccountId IS NOT null
and [date] between @periodBegin and @periodEnd
GROUP BY AccountId

DECLARE @batchWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @batchWrongCheck(accountId, count)
SELECT  batch.OwnerAccountId,
        COUNT(DISTINCT batchCheck.PassportNumber
              + ISNULL(batchCheck.PassportSeria, ''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   batchCheck.SourceCertificateId IS NULL
        AND batch.OwnerAccountId IS NOT NULL
        AND batchCheck.PassportNumber IS NOT null
		AND batch.UpdateDate between @periodBegin and @periodEnd
        AND batch.Type = 2
        AND NOT EXISTS ( SELECT *
                         FROM   dbo.CommonNationalExamCertificateSubjectCheck
                         WHERE  Mark IS NOT NULL
                                AND CheckId = batchCheck.id )
GROUP BY batch.OwnerAccountId


DECLARE @allWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @allWrongCheck
        ( accountId, count )
SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleWrongCheck s LEFT JOIN @batchWrongCheck b ON s.accountId = b.accountId

INSERT INTO @allWrongCheck
SELECT accountId, count FROM @batchWrongCheck WHERE accountId NOT IN (SELECT accountId FROM @allWrongCheck)

insert into @report
select org.FullName, SUM(wc.[count]) from @allWrongCheck wc inner join dbo.Account acc on acc.Id = wc.accountId 
inner join dbo.Organization2010 org on org.Id = acc.OrganizationId
where wc.[count] > 0
group by org.Id, org.FullName 
order by org.FullName

return
end

GO