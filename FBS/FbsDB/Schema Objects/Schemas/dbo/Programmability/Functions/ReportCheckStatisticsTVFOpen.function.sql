CREATE FUNCTION [dbo].[ReportCheckStatisticsTVFOpen]
    (
      @periodBegin DATETIME,
      @periodEnd DATETIME
    )
RETURNS @report TABLE
    (
      [Код региона] NVARCHAR(10) NULL,
      [Регион] NVARCHAR(100) NULL,
      [Уникальных пакетных проверок (по паспорту)] INT NULL,
      [Всего пакетных проверок (по паспорту)] INT NULL,
      [Уникальных пакетных проверок (по номеру)] INT NULL,
      [Всего пакетных проверок (по номеру)] INT NULL,
      [Уникальных интерактивных проверок (по паспорту)] INT NULL,
      [Всего интерактивных проверок (по паспорту)] INT NULL,
      [Уникальных интерактивных проверок (по номеру)] INT NULL,
      [Всего интерактивных проверок (по номеру)] INT NULL
    )
AS BEGIN
IF @periodBegin IS NULL 
 SET @periodBegin = '19000101'


    INSERT  INTO @report
            SELECT  ISNULL(r.code, '') [Код региона],
                    ISNULL(r.name, 'Не указан') [Регион],
                    SUM(p.UniqueBatchPassportCount) [Уникальных пакетных проверок (по паспорту)],
                    SUM(p.TotalBatchPassportCount) [Всего пакетных проверок (по паспорту)],
                    SUM(n.UniqueBatchNumberCount) [Уникальных пакетных проверок (по номеру)],
                    SUM(n.TotalBatchNumberCount) [Всего пакетных проверок (по номеру)],
                    SUM(iPassport.Uniq) [Уникальных интерактивных проверок (по паспорту)],
                    SUM(iPassport.Total) [Всего интерактивных проверок (по паспорту)],
                    SUM(iCNENumber.Uniq) [Уникальных интерактивных проверок (по номеру)],
                    SUM(iCNENumber.Total) [Всего интерактивных проверок (по номеру)]
            FROM    region r WITH ( NOLOCK )
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecc.SourceCertificateId)) UniqueBatchPassportCount,
                                        COUNT(*) TotalBatchPassportCount
                                FROM    [CommonNationalExamCertificateCheckBatch]
                                        AS cneccb WITH ( NOLOCK )
                                        JOIN [CommonNationalExamCertificateCheck]
                                        AS cnecc WITH ( NOLOCK ) ON cnecc.batchid = cneccb.id
																 AND cneccb.[Type] = 2
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cneccb.OwnerAccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                WHERE   cneccb.updatedate BETWEEN @periodBegin
                                                          AND     @periodEnd
                                GROUP BY ORg.regionid
                              ) p ON r.id = p.regionid
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecc.SourceCertificateId)) UniqueBatchNumberCount,
                                        COUNT(*) TotalBatchNumberCount
                                FROM    [CommonNationalExamCertificateCheckBatch]
                                        AS cneccb WITH ( NOLOCK )
                                        JOIN [CommonNationalExamCertificateCheck]
                                        AS cnecc WITH ( NOLOCK ) ON cnecc.batchid = cneccb.id
																 AND cneccb.[Type] = 1
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
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Uniq,
                                        COUNT(*) Total
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = 'CNENumberOpen'
                                GROUP BY ORg.regionid
                              ) iCNENumber ON iCNENumber.regionid = r.id
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Uniq,
                                        COUNT(*) Total 
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = 'PassportOpen'
                                GROUP BY ORg.regionid
                              ) iPassport ON iPassport.regionid = r.id
            GROUP BY r.code,r.name
            ORDER BY MAX(r.id)
    RETURN
   END
GO
