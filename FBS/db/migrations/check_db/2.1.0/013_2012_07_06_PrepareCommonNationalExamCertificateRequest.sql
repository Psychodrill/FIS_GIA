-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (13, '013_2012_07_06_PrepareCommonNationalExamCertificateRequest')
-- =========================================================================
GO 


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
ALTER procedure [dbo].[PrepareCommonNationalExamCertificateRequest]
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

		select @IsTypographicNumber = cnecrb.IsTypographicNumber, @year = cnecrb.year
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
		
		
		insert dbo.CommonNationalExamCertificateRequest
		(
		BatchId
		, LastName
		, FirstName
		, PatronymicName
		, PassportSeria
		, PassportNumber
		, IsCorrect
		, SourceCertificateId
		, SourceCertificateYear
		, SourceCertificateNumber
		, SourceRegionId
		, IsDeny
		, DenyComment
		, DenyNewCertificateNumber
		, TypographicNumber
		)
		select 
			<BatchIdentifier>
			, exam_certificate_request.LastName
			, exam_certificate_request.FirstName
			, exam_certificate_request.PatronymicName
			, isnull(exam_certificate_request.PassportSeria, exam_certificate.PassportSeria)
			, isnull(exam_certificate_request.PassportNumber, exam_certificate.PassportNumber)
			, case 
				when not exam_certificate.Id is null and exam_certificate_deny.Id is null then 1
				else 0
			end
			, exam_certificate.Id
			, isnull(exam_certificate.[Year], @year)
			, exam_certificate.Number
			, exam_certificate.RegionId
			, isnull(exam_certificate_deny.IsDeny, 0)
			, exam_certificate_deny.Comment
			, exam_certificate_deny.NewCertificateNumber
			, isnull(exam_certificate_request.TypographicNumber, exam_certificate.TypographicNumber)
		from (select 
				exam_certificate_request.[Index]
				, exam_certificate_request.LastName 
				, exam_certificate_request.FirstName 
				, exam_certificate_request.PatronymicName 
				, exam_certificate_request.PassportSeria
				, dbo.GetInternalPassportSeria(exam_certificate_request.PassportSeria) InternalPassportSeria
				, exam_certificate_request.PassportNumber
				, exam_certificate_request.TypographicNumber 
			from #CommonNationalExamCertificateRequest exam_certificate_request) exam_certificate_request
			left join <dataDbName>.dbo.CommonNationalExamCertificate exam_certificate with(nolock)
				on  exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_request.LastName
					and 1= case when exam_certificate_request.FirstName is null then 1
						when exam_certificate.FirstName collate cyrillic_general_ci_ai = exam_certificate_request.FirstName then 1
						else 0
					end
					and 1 = case when exam_certificate_request.PatronymicName is null then 1
						 when exam_certificate.PatronymicName collate cyrillic_general_ci_ai = exam_certificate_request.PatronymicName  then 1
						 else 0
					end
					and 1 = case when exam_certificate_request.PassportSeria  is null then 1 
						 when exam_certificate.InternalPassportSeria collate cyrillic_general_ci_ai = exam_certificate_request.PassportSeria then 1
						 else 0
					end					
					and 1 = case when exam_certificate_request.PassportNumber  is null then 1
						 when exam_certificate.PassportNumber collate cyrillic_general_ci_ai = exam_certificate_request.PassportNumber then 1
						 else 0
					end
					and 1 = case when @IsTypographicNumber = 0 then 1 
								 when exam_certificate_request.TypographicNumber is null then 1 
								 when @IsTypographicNumber = 1   
									and exam_certificate.TypographicNumber collate cyrillic_general_ci_ai = exam_certificate_request.TypographicNumber then 1 
					 			else 0
					end
					and exam_certificate.[Year] between @yearFrom and @yearTo
					
				left join (
					select 
						exam_certificate_deny.Id
						, exam_certificate_deny.Comment
						, exam_certificate_deny.NewCertificateNumber
						, 1 IsDeny
						, exam_certificate_deny.CertificateNumber CertificateNumber
						, exam_certificate_deny.[Year] [Year]
					from <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)) as exam_certificate_deny
					on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate.Number
						and exam_certificate_deny.[Year] between @yearFrom and @yearTo									

		-- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''passport_or_typo'' 
		'
	
	set @declareCommandText = replace(@declareCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId))
	set @executeCommandText = replace(replace(@executeCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
	
--	print @chooseDbText
--	print @declareCommandText
--	print @executeCommandText

	declare @CommonText nvarchar(max)
	set @CommonText=@chooseDbText + @IndexText + @declareCommandText + @executeCommandText
	print @CommonText
	exec sp_executesql @CommonText

	return 0
end


