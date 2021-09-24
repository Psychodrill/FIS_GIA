CREATE PROC [dbo].[GetNEWebUICheckLog]
    @login NVARCHAR(255),
    @startRowIndex INT = 1,
    @maxRowCount INT = NULL,
    @showCount BIT = NULL,   -- если > 0, то выбирается общее кол-во
    @TypeCode NVARCHAR(255) -- Тип проверки
AS 
    BEGIN
        DECLARE @accountId BIGINT,
            @endRowIndex INTEGER

        IF ISNULL(@maxRowCount, -1) = -1 
            SET @endRowIndex = 10000000
        ELSE 
            SET @endRowIndex = @startRowIndex + @maxRowCount

        IF EXISTS ( SELECT  1
                    FROM    [Account] AS a2
                            JOIN [GroupAccount] ga ON ga.[AccountId] = a2.[Id]
                            JOIN [Group] AS g ON ga.[GroupId] = g.[Id]
                                                 AND g.[Code] = 'Administrator'
                    WHERE   a2.[Login] = @login ) 
            SET @accountId = NULL
        ELSE 
            SET @accountId = ISNULL(( SELECT    account.[Id]
                                      FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                                      WHERE     account.[Login] = @login
                                    ), 0)

        IF ISNULL(@showCount, 0) = 0 
            BEGIN	
                IF @accountId IS NULL 
                    SELECT  *
                    FROM    ( SELECT    b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,   
                                        case when isnumeric(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2))=1 then
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) end YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )
                ELSE 
                    SELECT  *
                    FROM    ( SELECT TOP ( @endRowIndex )
                                        b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,
                                        case when isnumeric(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2))=1 then
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) end YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     b.AccountId = @accountId
                                                    AND @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )		
            END
        ELSE 
            IF @accountId IS NULL 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE )
            ELSE 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   b.AccountId = @accountId
                        AND @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE )	
        RETURN 0
    END
GO
