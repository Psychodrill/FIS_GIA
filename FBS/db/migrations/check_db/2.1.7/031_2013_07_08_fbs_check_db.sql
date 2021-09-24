insert into Migrations(MigrationVersion, MigrationName) values (31, '031_2013_07_08_fbs_check_db.sql')
go


/* 
   Для работы  процедуры требуются следующая 
   временная таблица:
   create table #CommonNationalExamCertificateRequest
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , Index bigint
    )
*/

-- exec dbo.PrepareCommonNationalExamCertificateRequest
-- ====================================================
-- Подготовка пакетов для проверки сертификатов ЕГЭ
-- v.1.0: Created by Sedov A.G. 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 31.05.2008
-- Добавлено удаление старых данных.
-- v.1.2: Modified by Sedov Anton 18.06.2008
-- Добавлен выбор запросов  для проверки
-- v.1.3: Modified by Sedov Anton 18.06.2008
-- Добавлено поле IsExtended
-- v.1.4: Modified by Sedov Anton 19.06.2008
-- Оптимизирована работа процедуры
-- v.1.5: Modified by Fomin Dmitriy 19.06.2008
-- Исправление дефектов.
-- v.1.6: Modified by Fomin Dmitriy 21.06.2008
-- Серия паспорта сравнивается с приведением к корректному виду.
-- v.1.7: Modified by Sedov Anton 04.07.2008
-- Добавлено поле NewCertificateNumber для аннулированных
-- сертификатов
-- v.1.8: Modified by Sedov Anton 09.07.2008
-- Исправлена  логика  динамического выбора БД
-- таблицы которой используются  для получения
-- данных о сертификатах
-- v.1.9: Modified by Sedov Anton 28.07.2008
-- Добавлен параметр Index во временную таблицу 
-- проверки запросов
-- v.1.10: Modified by Fomin Dmitriy 29.07.2008
-- Изменен порядок сортировки: сначала аннулированные,
-- затем актуальные. Добавлена сортировка по номеру.
-- v.1.11: Modified by Valeev Denis 03.06.2009
-- Добавлена проверка по типографскому номеру
-- ====================================================
alter procedure [dbo].[PrepareCommonNationalExamCertificateRequest]
  @batchId bigint
as
begin

  declare 
    @chooseDbText nvarchar(max)
    , @declareCommandText nvarchar(max)
    , @executeCommandText nvarchar(max)
    , @baseName nvarchar(255)
    , @IndexText nvarchar(max)
    , @CUID nvarchar(1000)
    
  update #CommonNationalExamCertificateRequest set PassportSeria=replace(PassportSeria, ' ', '')
    
  set @CUID = cast(NEWID() as nvarchar(1000))
  set @IndexText = '      
    CREATE NONCLUSTERED INDEX [IX_CNECR_LastName'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [LastName] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
      
    CREATE NONCLUSTERED INDEX [IX_CNECR_FirstName'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [FirstName] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
          
    CREATE NONCLUSTERED INDEX [IX_CNECR_PatronymicName'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [PatronymicName] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
        
    CREATE NONCLUSTERED INDEX [IX_CNECR_PassportSeria'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [PassportSeria] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    
    CREATE NONCLUSTERED INDEX [IX_CNECR_PassportNumber'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [PassportNumber] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    
    CREATE NONCLUSTERED INDEX [IX_CNECR_Index'+@CUID+'] ON [dbo].[#CommonNationalExamCertificateRequest] 
    (
      [Index] ASC
    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    
    '

  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1) 
    	
  set @declareCommandText =
    N'
    declare
      @yearFrom int
      , @yearTo int   
      , @IsTypographicNumber bit   
      , @year int

    select @IsTypographicNumber = cnecrb.IsTypographicNumber,@year = cnecrb.year
    from [CommonNationalExamCertificateRequestBatch] as cnecrb
    where cnecrb.id = <BatchIdentifier>

    if @year is not null
      select @yearFrom = @year, @yearTo = @year
    else
      select @yearFrom = Year(GetDate()) - 5, @yearTo = Year(GetDate())
        
    '

  set @executeCommandText = 
    N'
    delete exam_certificate_request
    from dbo.CommonNationalExamCertificateRequest exam_certificate_request
    where exam_certificate_request.BatchId = <BatchIdentifier>
    
    declare @ncount int
    
    create table #tt
	(
		id int identity(1,1) primary key,     
		[Index] bigint,
		LastName nvarchar(255),
		FirstName nvarchar(255) null,
		PatronymicName nvarchar(255) null,
		PassportSeria nvarchar(255) null,
		InternalPassportSeria nvarchar(255) null,
		PassportNumber nvarchar(255) null,		
		TypographicNumber nvarchar(255) null,
		ParticipantID uniqueidentifier,
		Year int
	)
	
    insert #tt
    select exam_certificate_request.[Index],
					exam_certificate_request.LastName, 
					exam_certificate_request.FirstName,
					exam_certificate_request.PatronymicName, 
					isnull(exam_certificate_request.PassportSeria, a.DocumentSeries) PassportSeria,
					dbo.GetInternalPassportSeria(exam_certificate_request.PassportSeria) InternalPassportSeria,
					isnull(exam_certificate_request.PassportNumber, a.DocumentNumber) PassportNumber,
					exam_certificate_request.TypographicNumber,
				    a.ParticipantID AS ParticipantID, a.UseYear
	from #CommonNationalExamCertificateRequest exam_certificate_request
				left join <dataDbName>.rbd.Participants a with (nolock) on a.[UseYear] between @yearFrom and @yearTo
					and a.Surname collate cyrillic_general_ci_ai = exam_certificate_request.LastName
					and 1 = case when exam_certificate_request.FirstName is null then 1
								when a.Name collate cyrillic_general_ci_ai = exam_certificate_request.FirstName then 1
						   else 0
						   end
					and 1 = case when exam_certificate_request.PatronymicName is null then 1
								 when a.SecondName collate cyrillic_general_ci_ai = exam_certificate_request.PatronymicName  then 1
							else 0
							end
					and 1 = case when exam_certificate_request.PassportSeria is null then 1 
								 when a.DocumentSeries collate cyrillic_general_ci_ai = exam_certificate_request.PassportSeria then 1
							else 0
							end         
					and 1 = case when exam_certificate_request.PassportNumber is null then 1
								 when a.DocumentNumber collate cyrillic_general_ci_ai = exam_certificate_request.PassportNumber then 1
							else 0
							end									
							    
    insert dbo.CommonNationalExamCertificateRequest
    (
    BatchId
    , LastName
    , FirstName
    , PatronymicName
    , PassportSeria
    , PassportNumber
    , IsCorrect
    , SourceCertificateIdGuid
    , SourceCertificateYear
    , SourceCertificateNumber
    , SourceRegionId
    , IsDeny
    , DenyComment
    , DenyNewCertificateNumber
    , TypographicNumber
    , ParticipantID
    )
    select 
      <BatchIdentifier>
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName
      , exam_certificate.PassportSeria
      , exam_certificate.PassportNumber
      , case 
        when (exam_certificate.Id is null and exam_certificate.ParticipantID is not null) 
              and exam_certificate_deny.Year is null then 1
        else 0
      end
      , exam_certificate.Id
      , isnull(exam_certificate.[Year], @year)
      , exam_certificate.Number
      , exam_certificate.RegionId
      , isnull(exam_certificate_deny.IsDeny, 0)
      , exam_certificate_deny.Reason
      , exam_certificate_deny.NewCertificateNumber
      , exam_certificate.TypographicNumber
      , exam_certificate.ParticipantID
    from 
	   (
			select distinct 			
					exam_certificate_request.[Index],
					exam_certificate_request.LastName, 
					exam_certificate_request.FirstName,
					exam_certificate_request.PatronymicName, 
					exam_certificate_request.PassportSeria,
					dbo.GetInternalPassportSeria(exam_certificate_request.PassportSeria) InternalPassportSeria,
					exam_certificate_request.PassportNumber,
					exam_certificate_request.TypographicNumber,			
					b.LicenseNumber AS Number, b.CertificateID AS id, 
				    isnull(b.UseYear,exam_certificate_request.year) AS Year, b.REGION AS RegionId,
				    exam_certificate_request.ParticipantID AS ParticipantID
            from #tt exam_certificate_request							
				left join <dataDbName>.prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=exam_certificate_request.ParticipantID
				left join <dataDbName>.prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and b.[UseYear] between @yearFrom and @yearTo   													 				
			        and 1 = case when @IsTypographicNumber = 0 then 1 
					   	         when exam_certificate_request.TypographicNumber is null then 1 
								 when @IsTypographicNumber = 1   
								  		and b.TypographicNumber collate cyrillic_general_ci_ai = exam_certificate_request.TypographicNumber then 1 
								 else 0
							end					
		) exam_certificate         
        left join (
          select 
            exam_certificate_deny.Reason
            , null NewCertificateNumber
            , exam_certificate_deny.CertificateFK
            , 1 IsDeny
            , exam_certificate_deny.[UseYear] [Year]
          from <dataDbName>.prn.CancelledCertificates exam_certificate_deny with(nolock)) as exam_certificate_deny
          on exam_certificate_deny.CertificateFK = exam_certificate.id
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo     
                     
       drop table #tt
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''passport_or_typo'' 
    '
  
  set @declareCommandText = replace(@declareCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))
  set @executeCommandText = replace(replace(@executeCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
  
--  print @chooseDbText
--  print @declareCommandText
--  print @executeCommandText

  declare @CommonText nvarchar(max)
  set @CommonText=@chooseDbText + @IndexText + @declareCommandText + @executeCommandText
  print  @CommonText
  exec sp_executesql @CommonText

  return 0
end
