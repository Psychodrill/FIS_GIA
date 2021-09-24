-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (48, '048_2012_06_18_ModifyCheckLog.sql')
-- =========================================================================


/****** Object:  StoredProcedure [dbo].[CheckNewUserAccountEmail]    Script Date: 06/18/2012 17:52:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNEWebUICheckLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNEWebUICheckLog]
GO

/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 06/18/2012 17:53:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) YearCertificate,
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
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) YearCertificate,
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

DROP INDEX [CNEWebUICheckLog_Id]
    ON [dbo].[CNEWebUICheckLog];
	
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__CNEWebUIC__Accou__4C67222E]') AND parent_object_id = OBJECT_ID(N'[dbo].[CNEWebUICheckLog]'))
ALTER TABLE [dbo].[CNEWebUICheckLog] DROP CONSTRAINT [FK__CNEWebUIC__Accou__4C67222E]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__CNEWebUIC__Accou__4E4F6AA0]') AND parent_object_id = OBJECT_ID(N'[dbo].[CNEWebUICheckLog]'))
ALTER TABLE [dbo].[CNEWebUICheckLog] DROP CONSTRAINT [FK__CNEWebUIC__Accou__4E4F6AA0]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__CNEWebUIC__Accou__4F438ED9]') AND parent_object_id = OBJECT_ID(N'[dbo].[CNEWebUICheckLog]'))
ALTER TABLE [dbo].[CNEWebUICheckLog] DROP CONSTRAINT [FK__CNEWebUIC__Accou__4F438ED9]
GO

ALTER TABLE [dbo].[CNEWebUICheckLog]
    ADD CONSTRAINT [PKCNEWebUICheckLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

GO

CREATE NONCLUSTERED INDEX [ind_CNEWebUICheckLog_typeaccount]
    ON [dbo].[CNEWebUICheckLog]([AccountId] ASC, [TypeCode] ASC, [EventDate] DESC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];

GO

ALTER TABLE [dbo].[CNEWebUICheckLog] WITH NOCHECK
    ADD FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

GO

/****** Object:  Index [IX_CommonNationalExamCertificateCheck_LastName_OtherFields2]    Script Date: 06/18/2012 19:49:51 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateCheck]') AND name = N'IX_CommonNationalExamCertificateCheck_LastName_OtherFields2')
DROP INDEX [IX_CommonNationalExamCertificateCheck_LastName_OtherFields2] ON [dbo].[CommonNationalExamCertificateCheck] WITH ( ONLINE = OFF )
GO


	
	


