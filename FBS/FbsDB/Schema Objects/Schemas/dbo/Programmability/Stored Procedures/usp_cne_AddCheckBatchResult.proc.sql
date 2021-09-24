create PROC [dbo].[usp_cne_AddCheckBatchResult]
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
            item.ref.value('@SourceCertificateId', 'uniqueidentifier') AS SourceCertificateId,
            item.ref.value('@IsDeny', 'bit') AS IsDeny,
            item.ref.value('@DenyComment', 'nvarchar(max)') AS DenyComment,
            item.ref.value('@DenyNewCertificateNumber', 'nvarchar(255)') AS DenyNewCertificateNumber,
            item.ref.value('@Year', 'int') AS Year,
            item.ref.value('@TypographicNumber', 'nvarchar(255)') AS TypographicNumber,
            item.ref.value('@RegionId', 'int') AS RegionId,
            item.ref.value('@PassportSeria', 'nvarchar(255)') AS PassportSeria,
            item.ref.value('@PassportNumber', 'nvarchar(255)') AS PassportNumber,
            item.ref.value('@UniqueCheckIdGuid', 'uniqueidentifier') AS UniqueCheckId,
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


--select * from #check
    DECLARE @id BIGINT, @checkid BIGINT, @isCorrect BIT
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
					inner join #check on #check.UniqueCheckIdGuid = ex.IdGuid
										 AND #check.[UniqueCheckYear] = ex.[year]
					WHERE #check.UniqueCheckId > 0
    	-- добавление статистики для еще не проверенных св-в
		  insert    ExamCertificateUniqueChecks
					(
					  IdGuid,
					  [year],
					  UniqueIHEaFCheck
					)

					select  DISTINCT #check.UniqueCheckIdGuid,
									  #check.UniqueCheckYear,
							          #check.UniqueIHEaFCheck
					from    #check
							left join ExamCertificateUniqueChecks ex on ex.IdGuid = #check.UniqueCheckIdGuid
																		AND #check.[UniqueCheckYear] = ex.[year]
					where   ex.Id is NULL AND #check.UniqueCheckIdGuid is not null

INSERT  CommonNationalExamCertificateCheck
                        (
                          BatchId,
                          CertificateCheckingId,
                          CertificateNumber,
                          IsOriginal,
                          IsCorrect,
                          SourceCertificateIdGuid,
                          IsDeny,
                          DenyComment,
                          DenyNewCertificateNumber,
                          Year,
                          TypographicNumber,
                          RegionId,
                          PassportSeria,
                          PassportNumber,
                          idtemp
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
                                PassportNumber,
                                a.id
                        FROM    #check a
                    
INSERT  CommonNationalExamCertificateSubjectCheck
                        (
                          BatchId,
                          CheckId,
                          SubjectId,
                          Mark,
                          IsCorrect,
                          SourceCertificateSubjectIdGuid,
                          SourceMark,
                          SourceHasAppeal,
                          Year
                        )
                        SELECT  @batchid,
                                b.id,
                                a.SubjectId,
                                a.Mark,
                                a.IsCorrect,
                                a.SourceCertificateSubjectIdGuid,
                                a.SourceMark,
                                a.SourceHasAppeal,
                                a.[Year]
                        FROM    
                        
                        (
                            SELECT  item.ref.value('@Id', 'bigint') AS Id,
									item.ref.value('@CheckId', 'bigint') AS CheckId,
									item.ref.value('@SubjectId', 'bigint') AS SubjectId,
									item.ref.value('@Mark', 'numeric(5,1)') AS Mark,
									item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
									item.ref.value('@SourceCertificateSubjectIdGuid', 'uniqueidentifier') AS SourceCertificateSubjectIdGuid,
									item.ref.value('@SourceMark', 'numeric(5,1)') AS SourceMark,
									item.ref.value('@SourceHasAppeal', 'bigint') AS SourceHasAppeal,
									item.ref.value('@Year', 'int') AS [Year]
                FROM    ( SELECT    @xml)			 
                feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/root/check/subjects/subject') AS item ( ref )
                        ) a join CommonNationalExamCertificateCheck b on a.CheckId=b.idtemp and b.BatchId=@batchid
                       
 
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
