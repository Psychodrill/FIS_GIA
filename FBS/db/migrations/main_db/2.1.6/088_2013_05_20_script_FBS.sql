insert into Migrations(MigrationVersion, MigrationName) values (88, '088_2013_05_20_script_FBS_1.sql')
go

-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
alter PROC [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]
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
                                    cnCheck.SourceCertificateIdGuid AS '@SourceCertificateIdGuid',
                                    cnCheck.IsDeny AS '@IsDeny',
                                    cnCheck.DenyComment AS '@DenyComment',
                                    cnCheck.DenyNewCertificateNumber AS '@DenyNewCertificateNumber',
                                    cnCheck.Year AS '@Year',
                                    cnCheck.TypographicNumber AS '@TypographicNumber',
                                    cnCheck.RegionId AS '@RegionId',
                                    cnCheck.PassportSeria AS '@PassportSeria',
                                    cnCheck.PassportNumber AS '@PassportNumber',
                                    ISNULL(unique_cheks.IdGuid,null) AS '@UniqueCheckId',
                  ISNULL(unique_cheks.UniqueIHEaFCheck,0) AS '@UniqueIHEaFCheck',
                  ISNULL(unique_cheks.[year],0) AS '@UniqueCheckYear',
                                    ( SELECT    b.Id AS 'subject/@Id',
                                                b.CheckId AS 'subject/@CheckId',
                                                b.SubjectId AS 'subject/@SubjectId',
                                                b.Mark AS 'subject/@Mark',
                                                b.IsCorrect AS 'subject/@IsCorrect',
                                                b.SourceCertificateSubjectIdGuid AS 'subject/@SourceCertificateSubjectIdGuid',
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
                          FROM      CommonNationalExamCertificateCheck cnCheck WITH ( NOLOCK)
              left outer join dbo.ExamCertificateUniqueChecks unique_cheks WITH ( NOLOCK)   on unique_cheks.IdGuid = cnCheck.SourceCertificateIdGuid 
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
