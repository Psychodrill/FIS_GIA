-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (71, '071_2012_07_25_NoLockProc.sql')
-- =========================================================================
GO


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
                                      FROM      CommonNationalExamCertificateSubjectCheck b WITH ( NOLOCK)
                                      WHERE     --BatchId=@id and 
                                                b.CheckId = cnCheck.id
                                    FOR
                                      XML PATH(''),
                                          ROOT('subjects'),
                                          TYPE
                                    )
                          FROM      CommonNationalExamCertificateCheck cnCheck WITH ( NOLOCK)
						  left outer join dbo.ExamCertificateUniqueChecks unique_cheks WITH ( NOLOCK) 	on unique_cheks.Id = cnCheck.SourceCertificateId 
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
