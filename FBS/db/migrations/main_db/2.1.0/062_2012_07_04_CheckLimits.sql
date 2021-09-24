-- =========================================================================
-- «апись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (62, '062_2012_07_04_CheckLimits.sql')
-- =========================================================================
GO


/****** Object:  Table [dbo].[BanData]    Script Date: 07/04/2012 20:09:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BanData](
	[AccountId] [bigint] NOT NULL,
	[WrongCheckCount] [int] NOT NULL,
	[SuccessCheckCount] [int] NOT NULL,
	[CheckDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BanData] PRIMARY KEY CLUSTERED 
(
	[CheckDate] ASC,
	[AccountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BanData] ADD  DEFAULT ((0)) FOR [WrongCheckCount]
GO

ALTER TABLE [dbo].[BanData] ADD  DEFAULT ((0)) FOR [SuccessCheckCount]
GO

ALTER TABLE [dbo].[BanData] ADD  DEFAULT (getdate()) FOR [CheckDate]
GO

/****** Object:  UserDefinedFunction [dbo].[IsUserBanned]    Script Date: 07/04/2012 20:10:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------
-- »меет ли учетна€ запись пользовател€ комментарий администратора.
-- v1.0: Created by Makarev Andrey 04.04.2008
--------------------------------------------------
CREATE function [dbo].[IsUserBanned]
	(
	@login nvarchar(255)
	)
returns bit 
as  
begin
declare @result bit
set @result = 1
select top 1 @result = IsBanned from Account where [Login] = @login
return @result
	
end
GO


/****** Object:  StoredProcedure [dbo].[BanOrgs]    Script Date: 07/04/2012 20:11:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- «абанить организации, которые сделали слишком много неверных проверок по паспорту без оценок.
-- =============================================
CREATE procedure [dbo].[BanOrgs]
as
begin
	
DECLARE @wrongChecksLimit INT, @wrongChecksPercent FLOAT
SET @wrongChecksLimit = 4
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
SELECT  batch.OwnerAccountId, COUNT( DISTINCT batchCheck.PassportNumber + ISNULL(batchCheck.PassportSeria,''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   BatchId IN (
        SELECT  id
        from    ( SELECT    b.Id,
                            b.Type,
                            REPLACE(a.batchEntry, CHAR(13), '') batchEntry
                  FROM      dbo.CommonNationalExamCertificateCheckBatch b WITH ( NOLOCK )
                            CROSS APPLY ( SELECT    RTRIM(LTRIM(REPLACE(replace(val, ' ', ''), CHAR(13), ''))) batchEntry
                                          FROM      ufn_ut_SplitFromStringWithId(REPLACE(CAST(b.batch AS NVARCHAR(MAX)), CHAR(10), CHAR(13)), CHAR(13))
                                        ) a WHERE b.Type = 2
                ) a
        WHERE   a.batchEntry LIKE '%!%%!%!%!%!%!%!%!%!%!%!%!%!%!%!%' ESCAPE '!')
        AND SourceCertificateId IS NULL AND batch.OwnerAccountId IS NOT NULL
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
SELECT  batch.OwnerAccountId, COUNT( DISTINCT batchCheck.PassportNumber + ISNULL(batchCheck.PassportSeria,''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   BatchId IN (
        SELECT  id
        from    ( SELECT    b.Id,
                            b.Type,
                            REPLACE(a.batchEntry, CHAR(13), '') batchEntry
                  FROM      dbo.CommonNationalExamCertificateCheckBatch b WITH ( NOLOCK )
							INNER JOIN @allWrongCheck wr ON b.OwnerAccountId = wr.accountId
                            CROSS APPLY ( SELECT    RTRIM(LTRIM(REPLACE(replace(val, ' ', ''), CHAR(13), ''))) batchEntry
                                          FROM      ufn_ut_SplitFromStringWithId(REPLACE(CAST(b.batch AS NVARCHAR(MAX)), CHAR(10), CHAR(13)), CHAR(13))
                                        ) a WHERE b.Type = 2
                ) a
                
        WHERE   a.batchEntry LIKE '%!%%!%!%!%!%!%!%!%!%!%!%!%!%!%!%' ESCAPE '!')
        AND SourceCertificateId IS NOT NULL 
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
SELECT s.accountId, w.[count], s.[count] FROM @allSuccessCheck s INNER JOIN @allWrongCheck w ON s.accountId = w.accountId

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
                  FROM      @allSuccessCheck s
                            INNER JOIN @allWrongCheck w ON s.accountId = w.accountId
                            INNER JOIN dbo.Account acc ON s.accountId = acc.Id
                  GROUP BY  acc.OrganizationId
                ) checks
        WHERE   w >= @wrongChecksLimit
                AND w > s * @wrongChecksPercent )
		AND organizationId IS NOT null

end
GO

ALTER TABLE dbo.Account
ADD [IsBanned] BIT not null default(0)

GO

