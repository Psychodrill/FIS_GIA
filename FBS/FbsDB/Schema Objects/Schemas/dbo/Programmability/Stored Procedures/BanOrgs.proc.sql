-- =============================================
-- Забанить организации, которые сделали слишком много неверных проверок по паспорту без оценок.
-- =============================================
CREATE procedure dbo.BanOrgs
as
begin
	
DECLARE @wrongChecksLimit INT, @wrongChecksPercent FLOAT
SET @wrongChecksLimit = 3000
set @wrongChecksPercent = 0.3

DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @singleWrongCheck (accountId, count)
SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData FROM dbo.EventLog WHERE 
-- это значит что ничего не найдено
SourceEntityId IS NULL
-- '|%|%|' - это значит что поиск по паспорту и оценок нет
AND EventParams LIKE '|%|%|' 
AND AccountId IS NOT null
GROUP BY AccountId

DECLARE @batchWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @batchWrongCheck( accountId, count )
SELECT  batch.OwnerAccountId,
        COUNT(DISTINCT batchCheck.PassportNumber
              + ISNULL(batchCheck.PassportSeria, ''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   batchCheck.SourceCertificateId IS NULL
        AND batch.OwnerAccountId IS NOT NULL
        AND batchCheck.PassportNumber IS NOT null
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

-- выбрать все успешние проверки (без баллов)

DECLARE @singleSuccessCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @singleSuccessCheck (accountId, count)
SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData FROM dbo.EventLog WHERE 
-- это значит что ничего не найдено
SourceEntityId IS NOT NULL
-- '|%|%|' - это значит что поиск по паспорту и оценок нет
AND EventParams LIKE '|%|%|' 
AND AccountId IN (SELECT accountId FROM @allWrongCheck)
GROUP BY AccountId

DECLARE @batchSuccessCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @batchSuccessCheck( accountId, count )
SELECT  batch.OwnerAccountId,
        COUNT(DISTINCT batchCheck.PassportNumber
              + ISNULL(batchCheck.PassportSeria, ''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   batchCheck.SourceCertificateId IS NOT NULL
        AND batch.OwnerAccountId IS NOT NULL 
        AND batchCheck.PassportNumber IS NOT null
        AND batch.Type = 2
        AND NOT EXISTS ( SELECT *
                         FROM   dbo.CommonNationalExamCertificateSubjectCheck
                         WHERE  Mark IS NOT NULL
                                AND CheckId = batchCheck.id )
GROUP BY batch.OwnerAccountId


DECLARE @allSuccessCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @allSuccessCheck
        ( accountId, count )
SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleSuccessCheck s LEFT JOIN @batchSuccessCheck b ON s.accountId = b.accountId

INSERT INTO @allSuccessCheck
SELECT accountId, count FROM @batchSuccessCheck WHERE accountId NOT IN (SELECT accountId FROM @allSuccessCheck)

INSERT INTO dbo.BanData
        ( AccountId ,
          WrongCheckCount ,
          SuccessCheckCount
        )
SELECT w.accountId, w.[count], ISNULL(s.[count],0) FROM @allSuccessCheck s RIGHT JOIN @allWrongCheck w ON s.accountId = w.accountId

-- всех разрешаем
UPDATE dbo.Account SET IsBanned = 0

-- когото запрещаем
UPDATE  Account
SET     IsBanned = 1
WHERE   OrganizationId IN (
        SELECT  organizationId
        FROM    ( SELECT    SUM(s.[count]) s,
                            SUM(w.[count]) w,
                            OrganizationId
                  FROM      @allWrongCheck w
							INNER JOIN dbo.Account acc ON w.accountId = acc.Id
                            left JOIN @allSuccessCheck s ON s.accountId = w.accountId
                  GROUP BY  acc.OrganizationId
                ) checks
        WHERE   w >= @wrongChecksLimit
                AND w > ISNULL(s,0) * @wrongChecksPercent )
		AND organizationId IS NOT null

end
