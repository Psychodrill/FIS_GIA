CREATE FUNCTION [dbo].[ReportCheckStatisticsTVFClosed](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE
    (
      [Код региона] NVARCHAR(10) NULL,
      [Регион] NVARCHAR(100) NULL,
      [Пакетов (по паспорту)] INT NULL,
      [Уникальных проверок (по паспорту)] INT NULL,
      [Всего проверок (по паспорту)] INT NULL,
      [Пакетов (по ТН)] INT NULL,
      [Уникальных проверок (по ТН)] INT NULL,
      [Всего проверок (по ТН)] INT NULL,
      [Пакетов (по номеру)] INT NULL,
      [Уникальных проверок (по номеру)] INT NULL,
      [Всего проверок (по номеру)] INT NULL,
      [Интерактивных проверок по паспорту] INT NULL,
      [Интерактивных проверок по номеру] INT NULL,
      [Интерактивных проверок по ТН] INT NULL,
      [Интерактивных проверок по баллам] INT NULL
    )
AS BEGIN

    INSERT  INTO @report
            SELECT  ISNULL(r.code, '') [Код региона],
                    ISNULL(r.name, 'Не указан') [Регион],
                    SUM(p.PassportBatchCount) [Пакетов (по паспорту)],
                    SUM(p.UniquePassportCount) [Уникальных проверок (по паспорту)],
                    SUM(p.TotalPassportCount) [Всего проверок (по паспорту)],
                    SUM(t.TypographicBatchCount) [Пакетов (по ТН)],
                    SUM(t.UniqueTypographicCount) [Уникальных проверок (по ТН)],
                    SUM(t.TotalTypographicCount) [Всего проверок (по ТН)],
                    SUM(n.NumberBatchCount) [Пакетов (по номеру)],
                    SUM(n.UniqueNumberCount) [Уникальных проверок (по номеру)],
                    SUM(n.TotalNumberCount) [Всего проверок (по номеру)],
                    SUM(iPassport.Cnt) [Интерактивных проверок по паспорту],
                    SUM(iCNENumber.Cnt) [Интерактивных проверок по номеру],
                    SUM(iTyp.Cnt) [Интерактивных проверок по ТН],
                    SUM(iMarks.Cnt) [Интерактивных проверок по баллым]
            FROM    region r WITH ( NOLOCK )
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT cnecrb.id) PassportBatchCount,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecr.[SourceCertificateId])) UniquePassportCount,
                                        COUNT(*) TotalPassportCount
                                FROM    [CommonNationalExamCertificateRequestBatch]
                                        AS cnecrb WITH ( NOLOCK )
                                        INNER JOIN [CommonNationalExamCertificateRequest]
                                        AS cnecr WITH ( NOLOCK ) ON cnecr.batchid = cnecrb.id
                                                                    AND cnecrb.[IsTypographicNumber] = 0
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cnecrb.OwnerAccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                WHERE   cnecrb.updatedate BETWEEN @periodBegin
                                                          AND     @periodEnd
                                GROUP BY ORg.regionid
                              ) p ON r.id = p.regionid
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT cnecrb.id) TypographicBatchCount,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecr.[SourceCertificateId])) UniqueTypographicCount,
                                        COUNT(*) TotalTypographicCount
                                FROM    [CommonNationalExamCertificateRequestBatch]
                                        AS cnecrb WITH ( NOLOCK )
                                        JOIN [CommonNationalExamCertificateRequest]
                                        AS cnecr WITH ( NOLOCK ) ON cnecr.batchid = cnecrb.id
                                                                    AND cnecrb.[IsTypographicNumber] = 1
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cnecrb.OwnerAccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                WHERE   cnecrb.updatedate BETWEEN @periodBegin
                                                          AND     @periodEnd
                                GROUP BY ORg.regionid
                              ) t ON r.id = t.regionid
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT cneccb.id) NumberBatchCount,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecc.SourceCertificateId)) UniqueNumberCount,
                                        COUNT(*) TotalNumberCount
                                FROM    [CommonNationalExamCertificateCheckBatch]
                                        AS cneccb WITH ( NOLOCK )
                                        JOIN [CommonNationalExamCertificateCheck]
                                        AS cnecc WITH ( NOLOCK ) ON cnecc.batchid = cneccb.id
																 AND ISNULL(cneccb.[Type],0) = 0
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cneccb.OwnerAccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                WHERE   cneccb.updatedate BETWEEN @periodBegin
                                                          AND     @periodEnd
                                GROUP BY ORg.regionid
                              ) n ON r.id = n.regionid
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Cnt
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = 'CNENumber'
                                GROUP BY ORg.regionid
                              ) iCNENumber ON iCNENumber.regionid = r.id
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Cnt
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = 'Passport'
                                GROUP BY ORg.regionid
                              ) iPassport ON iPassport.regionid = r.id
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Cnt
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = 'Typographic'
                                GROUP BY ORg.regionid
                              ) iTyp ON iTyp.regionid = r.id
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Cnt
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = 'Marks'
                                GROUP BY ORg.regionid
                              ) iMarks ON iMarks.regionid = r.id
            GROUP BY r.code,r.name
            ORDER BY MAX(r.id)


    DECLARE @days INT
    SET @days = DATEDIFF(DAY, @periodBegin, @periodEnd)
    INSERT  INTO @report
            SELECT  '',
                    'Итого за ' + CASE @days
                                    WHEN 1 THEN '24 часа'
                                    ELSE CAST(@days AS VARCHAR(10)) + ' дней'
                                  END,
                    SUM([Пакетов (по паспорту)]),
                    SUM([Уникальных проверок (по паспорту)]),
                    SUM([Всего проверок (по паспорту)]),
                    SUM([Пакетов (по ТН)]),
                    SUM([Уникальных проверок (по ТН)]),
                    SUM([Всего проверок (по ТН)]),
                    SUM([Пакетов (по номеру)]),
                    SUM([Уникальных проверок (по номеру)]),
                    SUM([Всего проверок (по номеру)]),
                    SUM([Интерактивных проверок по паспорту]),
                    SUM([Интерактивных проверок по номеру]),
                    SUM([Интерактивных проверок по ТН]),
                    SUM([Интерактивных проверок по баллам])
            FROM    @report

--Статистика за все время
    DECLARE @NumberUnique_UI INT
    SELECT  @NumberUnique_UI = COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                     + CONVERT(NVARCHAR, ChLog.FoundedCNEId))
    FROM    dbo.CNEWebUICheckLog ChLog WITH ( NOLOCK )
            INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
            INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                          AND GA.GroupId = 1
            INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                       AND ORg.Id IS NOT NULL
    WHERE   ChLog.FoundedCNEId IS NOT NULL
            AND ChLog.TypeCode = 'CNENumber'
	
    DECLARE @PassportUnique_UI INT
    SELECT  @PassportUnique_UI = COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                       + CONVERT(NVARCHAR, ChLog.FoundedCNEId))
    FROM    dbo.CNEWebUICheckLog ChLog WITH ( NOLOCK )
            INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
            INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                          AND GA.GroupId = 1
            INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                       AND ORg.Id IS NOT NULL
    WHERE   ChLog.FoundedCNEId IS NOT NULL
            AND ChLog.TypeCode = 'Passport'
	
    DECLARE @TypNumberUnique_UI INT
    SELECT  @TypNumberUnique_UI = COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                        + CONVERT(NVARCHAR, ChLog.FoundedCNEId))
    FROM    dbo.CNEWebUICheckLog ChLog WITH ( NOLOCK )
            INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
            INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                          AND GA.GroupId = 1
            INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                       AND ORg.Id IS NOT NULL
    WHERE   ChLog.FoundedCNEId IS NOT NULL
            AND ChLog.TypeCode = 'Typographic'
	
    DECLARE @MarksUnique_UI INT
    SELECT  @MarksUnique_UI = COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                    + CONVERT(NVARCHAR, ChLog.FoundedCNEId))
    FROM    dbo.CNEWebUICheckLog ChLog WITH ( NOLOCK )
            INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
            INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                          AND GA.GroupId = 1
            INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                       AND ORg.Id IS NOT NULL
    WHERE   ChLog.FoundedCNEId IS NOT NULL
            AND ChLog.TypeCode = 'Marks' ;
            
        WITH    PassportChecks ( [Пакетов (по паспорту)], [Уникальных проверок (по паспорту)], [Всего проверок (по паспорту)] )
                  AS ( SELECT   COUNT(DISTINCT cnecrb.id) [Пакетов (по паспорту)],
                                COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                      + CONVERT(NVARCHAR, cnecr.[SourceCertificateId])) [Уникальных проверок (по паспорту)],
                                COUNT(*) [Всего проверок (по паспорту)]
                       FROM     [CommonNationalExamCertificateRequestBatch] AS cnecrb
                                WITH ( NOLOCK )
                                JOIN [CommonNationalExamCertificateRequest] AS cnecr
                                WITH ( NOLOCK ) ON cnecr.batchid = cnecrb.id
                                                   AND cnecrb.[IsTypographicNumber] = 0
                                INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cnecrb.OwnerAccountId
                                INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                              AND GA.GroupId = 1
                                INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                           AND ORg.Id IS NOT NULL
                     ) ,
                TypographicChecks ( [Пакетов (по ТН)], [Уникальных проверок (по ТН)], [Всего проверок (по ТН)] )
                  AS ( SELECT   COUNT(DISTINCT cnecrb.id) [Пакетов (по ТН)],
                                COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                      + CONVERT(NVARCHAR, cnecr.[SourceCertificateId])) [Уникальных проверок (по ТН)],
                                COUNT(*) [Всего проверок (по ТН)]
                       FROM     [CommonNationalExamCertificateRequestBatch] AS cnecrb
                                WITH ( NOLOCK )
                                JOIN [CommonNationalExamCertificateRequest] AS cnecr
                                WITH ( NOLOCK ) ON cnecr.batchid = cnecrb.id
                                                   AND cnecrb.[IsTypographicNumber] = 1
                                INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cnecrb.OwnerAccountId
                                INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                              AND GA.GroupId = 1
                                INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                           AND ORg.Id IS NOT NULL
                     ) ,
                NumberChecks ( [Пакетов (по номеру)], [Уникальных проверок (по номеру)], [Всего проверок (по номеру)] )
                  AS ( SELECT   COUNT(DISTINCT cneccb.id) [Пакетов (по номеру)],
                                COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                      + CONVERT(NVARCHAR, cnecc.SourceCertificateId)) [Уникальных проверок (по номеру)],
                                COUNT(*) [Всего проверок (по номеру)]
                       FROM     [CommonNationalExamCertificateCheckBatch] AS cneccb
                                WITH ( NOLOCK )
                                JOIN [CommonNationalExamCertificateCheck] AS cnecc
                                WITH ( NOLOCK ) ON cnecc.batchid = cneccb.id
												AND ISNULL(cneccb.[Type],0) = 0
                                INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cneccb.OwnerAccountId
                                INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                              AND GA.GroupId = 1
                                INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                           AND ORg.Id IS NOT NULL
                     )
        INSERT  INTO @report
                SELECT  '',
                        'Итого за все время',
                        [Пакетов (по паспорту)],
                        [Уникальных проверок (по паспорту)],
                        [Всего проверок (по паспорту)],
                        [Пакетов (по ТН)],
                        [Уникальных проверок (по ТН)],
                        [Всего проверок (по ТН)],
                        [Пакетов (по номеру)],
                        [Уникальных проверок (по номеру)],
                        [Всего проверок (по номеру)],
                        @PassportUnique_UI,
                        @NumberUnique_UI,
                        @TypNumberUnique_UI,
                        @MarksUnique_UI
                FROM    PassportChecks,
                        TypographicChecks,
                        NumberChecks

    RETURN

   END
GO
