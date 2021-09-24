-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (64, '064_2012_07_06_CheckCount.sql')
-- =========================================================================
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_CNExamCertificate]') AND parent_object_id = OBJECT_ID(N'[dbo].[ExamCertificateUniqueChecks]'))
ALTER TABLE [dbo].[ExamCertificateUniqueChecks] DROP CONSTRAINT [fk_CNExamCertificate]
GO

GO
Alter PROC [dbo].[usp_cne_AddCheckBatchResult]
    @xml XML = NULL,
    @batchid BIGINT = 0
AS 
    SET nocount ON	
    IF @xml IS NULL 
        BEGIN
            SELECT  NULL IsProcess1,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END

    SELECT  item.ref.value('@Id', 'bigint') AS Id,
            item.ref.value('@IsBatchCorrect', 'bit') AS IsBatchCorrect,
            item.ref.value('@BatchId', 'bigint') AS BatchId,
            item.ref.value('@CertificateCheckingId', 'uniqueidentifier') AS CertificateCheckingId,
            item.ref.value('@CertificateNumber', 'nvarchar(255)') AS CertificateNumber,
            item.ref.value('@IsOriginal', 'bit') AS IsOriginal,
            item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
            item.ref.value('@SourceCertificateId', 'bigint') AS SourceCertificateId,
            item.ref.value('@IsDeny', 'bit') AS IsDeny,
            item.ref.value('@DenyComment', 'nvarchar(max)') AS DenyComment,
            item.ref.value('@DenyNewCertificateNumber', 'nvarchar(255)') AS DenyNewCertificateNumber,
            item.ref.value('@Year', 'int') AS Year,
            item.ref.value('@TypographicNumber', 'nvarchar(255)') AS TypographicNumber,
            item.ref.value('@RegionId', 'int') AS RegionId,
            item.ref.value('@PassportSeria', 'nvarchar(255)') AS PassportSeria,
            item.ref.value('@PassportNumber', 'nvarchar(255)') AS PassportNumber,
            item.ref.value('@UniqueCheckId', 'bigint') AS UniqueCheckId,
			item.ref.value('@UniqueIHEaFCheck', 'int') AS UniqueIHEaFCheck,
			item.ref.value('@UniqueCheckYear', 'int') AS UniqueCheckYear
    INTO    #check
    FROM    ( SELECT    @xml
            ) feeds ( feedXml )
            CROSS APPLY feedXml.nodes('/root/check') AS item ( ref )
--SELECT * FROM #check

    IF NOT EXISTS ( SELECT  *
                    FROM    #check ) 
        BEGIN
            SELECT  NULL IsProcess2,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END
    SELECT  item.ref.value('@Id', 'bigint') AS Id,
            item.ref.value('@CheckId', 'bigint') AS CheckId,
            item.ref.value('@SubjectId', 'bigint') AS SubjectId,
            item.ref.value('@Mark', 'numeric(5,1)') AS Mark,
            item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
            item.ref.value('@SourceCertificateSubjectId', 'bigint') AS SourceCertificateSubjectId,
            item.ref.value('@SourceMark', 'numeric(5,1)') AS SourceMark,
            item.ref.value('@SourceHasAppeal', 'bigint') AS SourceHasAppeal,
            item.ref.value('@Year', 'int') AS [Year]
    INTO    #mark
    FROM    ( SELECT    @xml
            ) feeds ( feedXml )
            CROSS APPLY feedXml.nodes('/root/check/subjects/subject') AS item ( ref )

--select * from #check
--select * from #mark
    DECLARE @id BIGINT,
        @checkid BIGINT,
        @isCorrect BIT
    SELECT  @batchid = dbo.GetInternalId(@batchid)
    IF EXISTS ( SELECT  *
                FROM    CommonNationalExamCertificateCheck
                WHERE   BatchId = @batchid ) 
        BEGIN
            SELECT  NULL IsProcess4,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END
--select @batchid
    BEGIN TRY
        BEGIN TRAN
		-- обновление статистики для уже проверенных св-в
		  update    ExamCertificateUniqueChecks
		  set       UniqueIHEaFCheck = #check.UniqueIHEaFCheck
		  from      ExamCertificateUniqueChecks ex
					inner join #check on #check.UniqueCheckId = ex.Id
										 AND #check.[UniqueCheckYear] = ex.[year]
					WHERE #check.UniqueCheckId > 0
    	-- добавление статистики для еще не проверенных св-в
		  insert    ExamCertificateUniqueChecks
					(
					  Id,
					  [year],
					  UniqueIHEaFCheck
					)

					select  DISTINCT #check.UniqueCheckId,
									  #check.UniqueCheckYear,
							          #check.UniqueIHEaFCheck
					from    #check
							left join ExamCertificateUniqueChecks ex on ex.Id = #check.UniqueCheckId
																		AND #check.[UniqueCheckYear] = ex.[year]
					where   ex.Id is NULL AND #check.UniqueCheckId > 0


        DECLARE cur CURSOR local fast_forward
            FOR SELECT  id
                FROM    #check
        OPEN cur
        FETCH NEXT FROM cur INTO @id
        WHILE @@FETCH_STATUS = 0
			BEGIN
		
                INSERT  CommonNationalExamCertificateCheck
                        (
                          BatchId,
                          CertificateCheckingId,
                          CertificateNumber,
                          IsOriginal,
                          IsCorrect,
                          SourceCertificateId,
                          IsDeny,
                          DenyComment,
                          DenyNewCertificateNumber,
                          Year,
                          TypographicNumber,
                          RegionId,
                          PassportSeria,
                          PassportNumber
                        )
                        SELECT  @batchid,
                                CertificateCheckingId,
                                CertificateNumber,
                                IsOriginal,
                                IsCorrect,
                                SourceCertificateId,
                                IsDeny,
                                DenyComment,
                                DenyNewCertificateNumber,
                                Year,
                                TypographicNumber,
                                RegionId,
                                PassportSeria,
                                PassportNumber
                        FROM    #check
                        WHERE   id = @id
                SET @checkid = SCOPE_IDENTITY()
                INSERT  CommonNationalExamCertificateSubjectCheck
                        (
                          BatchId,
                          CheckId,
                          SubjectId,
                          Mark,
                          IsCorrect,
                          SourceCertificateSubjectId,
                          SourceMark,
                          SourceHasAppeal,
                          Year
                        )
                        SELECT  @batchid,
                                @checkid,
                                SubjectId,
                                Mark,
                                IsCorrect,
                                SourceCertificateSubjectId,
                                SourceMark,
                                SourceHasAppeal,
                                Year
                        FROM    #mark
                        WHERE   CheckId = @id 
                FETCH NEXT FROM cur INTO @id
            END
        CLOSE cur
        DEALLOCATE cur
        SELECT TOP 1
                @isCorrect = IsBatchCorrect
        FROM    #check
        UPDATE  CommonNationalExamCertificateCheckBatch
        SET     IsProcess = 0,
                Executing = 0,
                IsCorrect = @isCorrect
        WHERE   id = @batchid
--select top 10 * from CommonNationalExamCertificateCheckBatch where id=@batchid
--select top 10 * from CommonNationalExamCertificateCheck where BatchId=@batchid  order by id desc
--select top 10 * from CommonNationalExamCertificateSubjectCheck where BatchId=@batchid order by id desc
        SELECT  IsProcess,
                ( SELECT    COUNT(*)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Total,
                ( SELECT    COUNT(SourceCertificateId)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Found
        FROM    CommonNationalExamCertificateCheckBatch b
        WHERE   IsProcess = 0
                AND id = @batchid
        IF @@trancount > 0 
            COMMIT TRAN

    END TRY
    BEGIN CATCH
        IF @@trancount > 0 
            ROLLBACK
        DECLARE @msg NVARCHAR(4000)
        SET @msg = ERROR_MESSAGE()
        RAISERROR ( @msg, 16, 1 )
        RETURN -1
    END CATCH

GO

-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
ALTER PROC [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]
    @batchId BIGINT,
    @xml XML OUT
AS 
    SET nocount ON
    DECLARE @id BIGINT,
        @isCorrect BIT
    SELECT  @id = id,
            @isCorrect = IsCorrect
    FROM    dbo.CommonNationalExamCertificateCheckBatch WITH ( NOLOCK, FASTFIRSTROW )
    WHERE   outerId = @batchId
            AND IsProcess = 0
            AND Executing = 0
    SET @xml = ( SELECT ( SELECT    cnCheck.Id AS '@Id',
                                    @isCorrect AS '@IsBatchCorrect',
                                    cnCheck.BatchId AS '@BatchId',
                                    cnCheck.CertificateCheckingId AS '@CertificateCheckingId',
                                    cnCheck.CertificateNumber AS '@CertificateNumber',
                                    cnCheck.IsOriginal AS '@IsOriginal',
                                    cnCheck.IsCorrect AS '@IsCorrect',
                                    cnCheck.SourceCertificateId AS '@SourceCertificateId',
                                    cnCheck.IsDeny AS '@IsDeny',
                                    cnCheck.DenyComment AS '@DenyComment',
                                    cnCheck.DenyNewCertificateNumber AS '@DenyNewCertificateNumber',
                                    cnCheck.Year AS '@Year',
                                    cnCheck.TypographicNumber AS '@TypographicNumber',
                                    cnCheck.RegionId AS '@RegionId',
                                    cnCheck.PassportSeria AS '@PassportSeria',
                                    cnCheck.PassportNumber AS '@PassportNumber',
                                    ISNULL(unique_cheks.Id,0) AS '@UniqueCheckId',
									ISNULL(unique_cheks.UniqueIHEaFCheck,0) AS '@UniqueIHEaFCheck',
									ISNULL(unique_cheks.[year],0) AS '@UniqueCheckYear',
                                    ( SELECT    b.Id AS 'subject/@Id',
                                                b.CheckId AS 'subject/@CheckId',
                                                b.SubjectId AS 'subject/@SubjectId',
                                                b.Mark AS 'subject/@Mark',
                                                b.IsCorrect AS 'subject/@IsCorrect',
                                                b.SourceCertificateSubjectId AS 'subject/@SourceCertificateSubjectId',
                                                b.SourceMark AS 'subject/@SourceMark',
                                                b.SourceHasAppeal AS 'subject/@SourceHasAppeal',
                                                b.Year AS 'subject/@Year'
                                      FROM      CommonNationalExamCertificateSubjectCheck b
                                      WHERE     --BatchId=@id and 
                                                b.CheckId = cnCheck.id
                                    FOR
                                      XML PATH(''),
                                          ROOT('subjects'),
                                          TYPE
                                    )
                          FROM      CommonNationalExamCertificateCheck cnCheck
						  left outer join dbo.ExamCertificateUniqueChecks unique_cheks 	on unique_cheks.Id = cnCheck.SourceCertificateId
                          WHERE     --cnCheck.id in(3201511,3201512)
                                    BatchId = @id
                        FOR
                          XML PATH('check'),
                              TYPE
                        )
               FOR
                 XML PATH('root'),
                     TYPE
               )
SELECT @xml               


