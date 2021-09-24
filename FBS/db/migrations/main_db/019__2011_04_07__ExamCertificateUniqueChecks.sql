-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (19, '019__2011_04_07__ExamCertificateUniqueChecks')
-- =========================================================================





IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueChecks')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_ch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueChecks]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueIHEaFCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_iheafch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueIHEaFCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueIHECheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_ihech]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueIHECheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueIHEFCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_ihefch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueIHEFCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueTSSaFCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_tssafch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueTSSaFCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueTSSCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_tssch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueTSSCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueTSSFCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_tssfch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueTSSFCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueRCOICheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_rcoich]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueRCOICheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueOUOCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_ouoch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueOUOCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueFounderCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_founderch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueFounderCheck]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.[COLUMNS] A WHERE A.TABLE_SCHEMA = 'dbo' AND A.TABLE_NAME = 'CommonNationalExamCertificate' AND COLUMN_NAME = 'UniqueOtherCheck')
BEGIN
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP [cert_col_otherch]
	ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP COLUMN [UniqueOtherCheck]
END
GO


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES B WHERE B.TABLE_SCHEMA = 'dbo' 
		AND B.TABLE_NAME = 'ExamCertificateUniqueChecks'))
  DROP TABLE [dbo].[ExamCertificateUniqueChecks]
GO

CREATE TABLE [dbo].[ExamCertificateUniqueChecks](
	[Id] [bigint] NOT NULL,
	[Year] [int] NOT NULL,
	[UniqueChecks] [int] NOT NULL CONSTRAINT [cert_col_ch] DEFAULT 0,
	
	[UniqueIHEaFCheck] [int] NOT NULL CONSTRAINT [cert_col_iheafch] default 0,
	[UniqueIHECheck] [int] NOT NULL CONSTRAINT [cert_col_ihech] default 0,
	[UniqueIHEFCheck] [int] NOT NULL CONSTRAINT [cert_col_ihefch] default 0,
	
	[UniqueTSSaFCheck] [int] NOT NULL CONSTRAINT [cert_col_tssafch] default 0,
	[UniqueTSSCheck] [int] NOT NULL CONSTRAINT [cert_col_tssch] default 0,
	[UniqueTSSFCheck] [int] NOT NULL CONSTRAINT [cert_col_tssfch] default 0,
	
	[UniqueRCOICheck] [int] NOT NULL CONSTRAINT [cert_col_rcoich] default 0,
	[UniqueOUOCheck] [int] NOT NULL CONSTRAINT [cert_col_ouoch] default 0,
	[UniqueFounderCheck] [int] NOT NULL CONSTRAINT [cert_col_founderch] default 0,
	[UniqueOtherCheck] [int] NOT NULL CONSTRAINT [cert_col_otherch] default 0,
	CONSTRAINT [CertificateIniqueChecksPK] PRIMARY KEY CLUSTERED 
	(
		[Year] ASC,
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
		) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExamCertificateUniqueChecks]
	ADD CONSTRAINT fk_CNExamCertificate 
	FOREIGN KEY([Year],[Id]) 
	REFERENCES CommonNationalExamCertificate([Year], [Id]) 
		ON UPDATE CASCADE ON DELETE CASCADE
GO


-- =============================================
-- Получить сертификат ЕГЭ.
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificate]
	@lastName nvarchar(255) = null
	, @firstName nvarchar(255) = null
	, @patronymicName nvarchar(255) = null
	, @subjectMarks nvarchar(4000) = null
	, @passportSeria nvarchar(255) = null
	, @passportNumber nvarchar(255) = null
	, @typographicNumber nvarchar(255) = null
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @year int = null
as
begin

	declare 
		@eventCode nvarchar(255)
        , @organizationId bigint
		, @editorAccountId bigint
		, @eventParams nvarchar(4000)
		, @yearFrom int
		, @yearTo int
		, @commandText nvarchar(4000)
		, @internalPassportSeria nvarchar(255)

	-- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

	set @eventParams = 
		isnull(@lastName,'') + '|' 
		+ isnull(@firstName,'') + '|' 
		+ isnull(@patronymicName,'') + '|' 
		+ isnull(@subjectMarks, '') + '|' 
		+ isnull(@passportSeria, '') + '|' 
		+ isnull(@passportNumber, '') + '|' 
		+ isnull(@typographicNumber, '')

	select
		@editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	if @year is not null
		select @yearFrom = @year, @yearTo = @year
	else
		select @yearFrom = 2008
	    select @yearTo = Year(GetDate())

	if not @passportSeria is null
		set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

	set @commandText = ''
	if isnull(@typographicNumber, '') <> ''
		set @eventCode = N'CNE_FND_TN'
	else 
		set @eventCode = N'CNE_FND_P'

	set @commandText = @commandText + 
    	' declare @CId bigint ' +
		' declare ' +
		'	@sourceEntityIds nvarchar(4000) ' + 
		'declare @Search table ' +
		'	( ' +
		'	LastName nvarchar(255) ' +
		'	, FirstName nvarchar(255) ' +
		'	, PatronymicName nvarchar(255) ' +
		'	, CertificateId bigint ' +
		'	, CertificateNumber nvarchar(255) ' +
		'	, RegionId int ' +
		'	, PassportSeria nvarchar(255) ' +
		'	, PassportNumber nvarchar(255) ' +
		'	, TypographicNumber nvarchar(255) ' +
		'	, Year int ' +
		'	) ' +
		'insert into @Search ' +
		'select top 300 ' +
		'	certificate.LastName ' +
		'	, certificate.FirstName ' +
		'	, certificate.PatronymicName ' +
		'	, certificate.Id ' +
		'	, certificate.Number ' +
		'	, certificate.RegionId ' +
		'	, isnull(certificate.PassportSeria, @internalPassportSeria) ' +
		'	, isnull(certificate.PassportNumber, @passportNumber) ' +
		'	, certificate.TypographicNumber ' +
		'	, certificate.Year ' +
		'from CommonNationalExamCertificate certificate with (nolock) ' +
		'where ' +
		'	certificate.[Year] between @yearFrom and @yearTo ' 

	if not @lastName is null 
		set @commandText = @commandText +
			'	and certificate.LastName collate cyrillic_general_ci_ai = @lastName '
	
	if not @firstName is null 
		set @commandText = @commandText +
			'	and certificate.FirstName collate cyrillic_general_ci_ai = @firstName ' 

	if not @patronymicName is null 
		set @commandText = @commandText +
			'	and certificate.PatronymicName collate cyrillic_general_ci_ai = @patronymicName ' 

	if not @internalPassportSeria is null
    begin
    	if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                '	and certificate.InternalPassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                '	and certificate.InternalPassportSeria = @internalPassportSeria '
        end
	end

	if not @passportNumber is null
    begin
    	if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
        	set @passportNumber = REPLACE(@passportNumber, '*', '%')
        	set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                '	and certificate.PassportNumber like @passportNumber '
        end
    	else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    '	and certificate.PassportNumber = @passportNumber '
        end
    end
	
	if not @typographicNumber is null
		set @commandText = @commandText +
			'	and certificate.TypographicNumber = @typographicNumber '
	
	if @lastName is null and @firstName is null and @passportNumber is null
		set @commandText = @commandText +
			'	and 0 = 1 '

	if not @subjectMarks is null
	begin
		set @commandText = @commandText + 
			'delete search ' + 
			'from @Search search ' +
			'where exists(select 1 ' +
			'		from ' +
			'				CommonNationalExamCertificateSubject certificate_subject with(nolock) ' +
			'					inner join @Search inner_search ' +
			'						on certificate_subject.[Year] between @yearFrom and @yearTo ' +
			'							and search.CertificateId = certificate_subject.CertificateId ' +
			'							and inner_search.CertificateId = search.CertificateId ' +
			'					right join dbo.GetSubjectMarks(@subjectMarks) subject_mark ' +
			'						on certificate_subject.SubjectId = subject_mark.SubjectId ' +
			'							and certificate_subject.Mark = subject_mark.Mark ' +
			'			where ' +
			'				certificate_subject.SubjectId is null ' +
			'				or certificate_subject.Mark is null) '
	end
	
	set @commandText = @commandText + 
		'set @sourceEntityIds = '''' ' +
		'select ' +
		'	@sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(4000), search.CertificateId) ' +
		'from ' +
		'	@Search search ' + 
		'set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) ' +
		'if @sourceEntityIds = '''' ' +
		'	set @sourceEntityIds = null ' --+
		--'if (select count(*) from @Search) = 0 ' +
		--'	insert into @Search ' +
		--'	values (@lastName, @firstName, @patronymicName, null, null, null, null, null, null, null) '

	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
	set @commandText = @commandText + 
    	N'
		declare db_cursor cursor for
		select
        	distinct S.CertificateId
        from 
        	@Search S
        where
        	S.CertificateId is not null
		
        open db_cursor   
		fetch next from db_cursor INTO @CId   
		while @@FETCH_STATUS = 0   
        begin
        	exec dbo.ExecuteChecksCount
            	@OrganizationId = @organizationId,
                @CertificateId = @CId
			fetch next from db_cursor into @CId
        end
        
        close db_cursor   
		deallocate db_cursor
        '

	set @commandText = @commandText + 
		'select ' +
			'S.CertificateId, ' +
			'S.CertificateNumber, ' + 
			'S.LastName LastName, ' +
			'S.FirstName FirstName, ' + 
			'S.PatronymicName PatronymicName, ' + 
			'S.PassportSeria PassportSeria, ' + 
			'S.PassportNumber PassportNumber, ' + 
			'S.TypographicNumber TypographicNumber, ' + 
			'region.Name RegionName, ' + 
			'case ' +
				'when not S.CertificateId is null then 1 ' +
				'else 0 ' +
			'end IsExist, ' + 
			'case ' + 
				'when not CD.Id is null then 1 ' +
				'else 0 ' +
			'end IsDeny, ' + 
			'CD.Comment DenyComment, ' + 
			'CD.NewCertificateNumber, ' + 
			'S.Year, ' + 
			'case ' + 
				'when ed.[ExpireDate] is null then ''Не найдено'' ' +
                'else ' + 
					'case ' + 
                    	'when CD.Id is not null then ''Аннулировано'' ' + 
                        'else ' +
                        	'case ' + 
                            	'when getdate() <= ed.[ExpireDate] then ''Действительно'' ' + 
                                'else ''Истек срок'' ' + 
							'end ' + 
					'end ' + 
			'end as [Status], ' +
             'isnull(CC.UniqueChecks,0) UniqueChecks, ' +
			'isnull(CC.UniqueIHEaFCheck,0) UniqueIHEaFCheck, ' +
			'isnull(CC.UniqueIHECheck,0) UniqueIHECheck, ' +
			'isnull(CC.UniqueIHEFCheck,0) UniqueIHEFCheck, ' +
			'isnull(CC.UniqueTSSaFCheck,0) UniqueTSSaFCheck, ' +
			'isnull(CC.UniqueTSSCheck,0) UniqueTSSCheck, ' +
			'isnull(CC.UniqueTSSFCheck,0) UniqueTSSFCheck, ' +
			'isnull(CC.UniqueRCOICheck,0) UniqueRCOICheck, ' +
			'isnull(CC.UniqueOUOCheck,0) UniqueOUOCheck, ' +
			'isnull(CC.UniqueFounderCheck,0) UniqueFounderCheck, ' +
			'isnull(CC.UniqueOtherCheck,0) UniqueOtherCheck ' +
		'from ' +
			'@Search S ' +
            'inner join CommonNationalExamCertificate C with (nolock) ' +
            	'on C.Id = S.CertificateId ' +
            'left outer join ExamCertificateUniqueChecks CC with (nolock) ' +
				'on CC.Id  = C.Id ' +
			'left outer join CommonNationalExamCertificateDeny CD with (nolock) ' +
				'on CD.[Year] between @yearFrom and @yearTo ' +
                'and S.CertificateNumber = CD.CertificateNumber ' + 
			'left outer join dbo.Region region with (nolock) ' +
				'on region.[Id] = S.RegionId ' +
			'left join ExpireDate ed ' + 
            	'on ed.year = S.year '
            
	set @commandText = @commandText + 
		N'
		exec dbo.RegisterEvent 
			@accountId = @editorAccountId,
			@ip = @ip,
			@eventCode = @eventCode,
			@sourceEntityIds = @sourceEntityIds,
			@eventParams = @eventParams,
			@updateId = null
		' 
       
	exec sp_executesql @commandText
		, N'@lastName nvarchar(255), @firstName nvarchar(255)
			, @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
			, @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @subjectMarks nvarchar(4000)
			, @yearFrom int, @yearTo int 
			, @editorAccountId bigint
			, @ip nvarchar(255) 
			, @eventCode nvarchar(255) 
			, @eventParams nvarchar(4000)
            , @organizationId bigint '
		, @lastName
		, @firstName
		, @patronymicName
		, @internalPassportSeria
		, @passportNumber
		, @typographicNumber
		, @subjectMarks
		, @yearFrom
		, @YearTo
		, @editorAccountId
		, @ip
		, @eventCode
		, @eventParams
        , @organizationId

	return 0
end
GO

IF OBJECT_ID('ExecuteChecksCount') IS NOT NULL
	DROP PROC ExecuteChecksCount
GO

CREATE PROCEDURE [dbo].[ExecuteChecksCount]
    @OrganizationId bigint,
    @CertificateId bigint
AS
BEGIN
	-- Выполняла ли данная организация проверку данного сертификата
    declare @isExists bit

	-- Значения инкрементов
	declare @uniqueIHEaFCheck int
	declare @uniqueIHECheck int
	declare @uniqueIHEFCheck int 

	declare @uniqueTSSaFCheck int
	declare @uniqueTSSCheck int
	declare @uniqueTSSFCheck int
    
	declare @uniqueRCOICheck int
	declare @uniqueOUOCheck int
	declare @uniqueFounderCheck int
	declare @uniqueOtherCheck int 
    
    -- Тип организации
    declare @orgType int
    
    -- Является ли организация филлиалом
    declare @isFilial bit

	-- Инициализация переменных
    set @isExists = 0
	set @uniqueIHEaFCheck = 0
	set @uniqueIHECheck = 0
	set @uniqueIHEFCheck = 0
	set @uniqueTSSaFCheck = 0
	set @uniqueTSSCheck = 0
	set @uniqueTSSFCheck = 0
	set @uniqueRCOICheck = 0
	set @uniqueOUOCheck = 0
	set @uniqueFounderCheck = 0
	set @uniqueOtherCheck = 0
    set @orgType = 0
    set @isFilial = 0


    set @isExists = 
    	(
        select count(*) 
        from [dbo].[OrganizationCertificateChecks] OCC 
        where OCC.CertificateId = @CertificateId and OCC.OrganizationId = @OrganizationId
        )

    if (@isExists = 0)
    begin
    	insert into [dbo].[OrganizationCertificateChecks] (CertificateId, OrganizationId)
        values (@CertificateId, @OrganizationId)
        
        select
        	@orgType = isnull(O.TypeId, 0),
            @isFilial = (case when (O.MainId is null) then 0 else 1 end)
        from
        	[dbo].[Organization2010] O
        where
        	O.Id = @OrganizationId

        if (@orgType = 0)
        	return -1

		if (@orgType = 1)
        begin
        	set @uniqueIHEaFCheck = 1
            if (@isFilial = 1)
            	set @uniqueIHEFCheck = 1
            else
            	set @uniqueIHECheck = 1
        end           
        
		if (@orgType = 2)
        begin
        	set @uniqueTSSaFCheck = 1
            if (@isFilial = 1)
            	set @uniqueTSSFCheck = 1
            else
            	set @uniqueTSSCheck = 1
        end           
        
		if (@orgType = 3)
        begin
        	set @uniqueRCOICheck = 1
        end           

		if (@orgType = 4)
        begin
        	set @uniqueOUOCheck = 1
        end           

		if (@orgType = 6)
        begin
        	set @uniqueFounderCheck = 1
        end           

		if (@orgType = 5)
        begin
        	set @uniqueOtherCheck = 1
        end
        
        declare @year int
        set @year = (select top 1 C.[Year]
					 from CommonNationalExamCertificate C
					 where C.Id = @CertificateId)
					
		 if not exists (select *
				   from [dbo].[ExamCertificateUniqueChecks]
				   where Id = @CertificateId)
		begin
			insert into [dbo].[ExamCertificateUniqueChecks] ([Year], Id)
			values (@year, @CertificateId)
		end
		
		update 
        	[dbo].[ExamCertificateUniqueChecks]
		set 
			UniqueChecks = UniqueChecks + 1,
			UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
			UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
			UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
			UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
			UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
			UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
			UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
			UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
			UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
			UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
		where
        	Id = @CertificateId
        	and [Year] = @year

        return 1
    end
    else begin
    	return 0
    end
END
GO

-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
ALTER proc [dbo].[CheckCommonNationalExamCertificateByNumber]
	  @number nvarchar(255) = null
	, @checkLastName nvarchar(255) = null
	, @checkFirstName nvarchar(255) = null
	, @checkPatronymicName nvarchar(255) = null
	, @checkSubjectMarks nvarchar(max) = null
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @checkTypographicNumber nvarchar(20) = null
as
begin 

	if @checkTypographicNumber is null and @number is null
	begin
		RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
		return
	end
    
	declare 
		@commandText nvarchar(max)
		, @declareCommandText nvarchar(max)
		, @selectCommandText nvarchar(max)
		, @baseName nvarchar(255)
		, @yearFrom int
		, @yearTo int
		, @accountId bigint
        , @organizationId bigint
    	, @CId bigint
		, @eventCode nvarchar(255)
		, @eventParams nvarchar(4000)
		, @sourceEntityIds nvarchar(4000) 
	
	declare @check_subject table
	(
	SubjectId int
	, Mark nvarchar(10)
	)
	
	declare @certificate_check table
	(
	Number nvarchar(255)
	, CheckLastName nvarchar(255)
	, LastName nvarchar(255)
	, LastNameIsCorrect bit
	, CheckFirstName nvarchar(255)
	, FirstName nvarchar(255)
	, FirstNameIsCorrect bit
	, CheckPatronymicName nvarchar(255)
	, PatronymicName nvarchar(255)
	, PatronymicNameIsCorrect bit
	, IsExist bit
	, CertificateId bigint
	, IsDeny bit
	, DenyComment ntext
	, DenyNewCertificateNumber nvarchar(255)
	, [Year] int
	, PassportSeria nvarchar(255)
	, PassportNumber nvarchar(255)
	, RegionId int
	, RegionName nvarchar(255)
	, TypographicNumber nvarchar(255)
	)

	-- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

	if isnull(@checkTypographicNumber,'') <> ''
		select @yearFrom = 2009, @yearTo = Year(GetDate()) --2009-год появления типографского номера в БД
	else
		select @yearFrom = 2008, @yearTo = Year(GetDate())

	select
		@accountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	insert into @certificate_check 
	select
		certificate.Number 
		, @CheckLastName 
		, [certificate].LastName 
		, case 
			when [certificate].LastName collate cyrillic_general_ci_ai = isnull(@CheckLastName, [certificate].LastName) then 1
			else 0
		end LastNameIsCorrect
		, @CheckFirstName 
		, [certificate].FirstName 
		, case 
			when [certificate].FirstName collate cyrillic_general_ci_ai = isnull(@CheckFirstName, [certificate].FirstName) then 1
			else 0
		end FirstNameIsCorrect
		, @CheckPatronymicName 
		, [certificate].PatronymicName 
		, case 
			when [certificate].PatronymicName collate cyrillic_general_ci_ai = isnull(@CheckPatronymicName,[certificate].PatronymicName) then 1
			else 0
		end PatronymicNameIsCorrect
		, case
			when not [certificate].Id is null then 1
			else 0
		end IsExist
		, [certificate].Id
		, case
			when not certificate_deny.Id is null then 1
			else 0
		end
		, certificate_deny.Comment
		, certificate_deny.NewCertificateNumber
		, certificate.[Year]
		, certificate.PassportSeria
		, certificate.PassportNumber
		, [certificate].RegionId
		, region.Name
		, certificate.TypographicNumber

	from 
		(select null 'empty') t left join 
		dbo.CommonNationalExamCertificate [certificate] with (nolock, fastfirstrow) on 
				certificate.[Year] between @yearFrom and @yearTo
				and certificate.Number = isnull(@number, certificate.Number)
				and isnull(certificate.TypographicNumber,'') = coalesce(@CheckTypographicNumber,certificate.TypographicNumber,'')
				--and certificate.Lastname collate cyrillic_general_ci_ai = @CheckLastName 
		left outer join dbo.Region region
			on region.Id = [certificate].RegionId
		left outer join CommonNationalExamCertificateDeny certificate_deny with (nolock, fastfirstrow)
			on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.CertificateNumber = certificate.Number
				--and certificate.Lastname collate cyrillic_general_ci_ai = @CheckLastName 

	set @eventParams = 
		isnull(@number, '') + '|' +
		isnull(@checkLastName, '') + '|' +
		isnull(@checkFirstName, '') + '|' +
		isnull(@checkPatronymicName, '') + '|' +
		isnull(@checkSubjectMarks, '') + '|' +
		isnull(@checkTypographicNumber, '')

	set @sourceEntityIds = '' 
	select 
		@sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), certificate_check.CertificateId) 
	from 
		@certificate_check certificate_check 
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	if @sourceEntityIds = '' 
		set @sourceEntityIds = null 


	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.CertificateId
    from 
        @certificate_check S
    where
    	S.CertificateId is not null
		
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @CertificateId = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
	-------------------------------------------------------------

	select
		certificate_check.CertificateId
		,certificate_check.Number
		, certificate_check.CheckLastName
		, certificate_check.LastName
		, certificate_check.LastNameIsCorrect
		, certificate_check.CheckFirstName
		, certificate_check.FirstName
		, certificate_check.FirstNameIsCorrect
		, certificate_check.CheckPatronymicName
		, certificate_check.PatronymicName
		, certificate_check.PatronymicNameIsCorrect
		, certificate_check.IsExist
		, subject.Id SubjectId
		, subject.Name SubjectName
		--, check_subject.CheckSubjectMark
		--, check_subject.SubjectMark
		, case when check_subject.CheckSubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',') CheckSubjectMark
		, case when check_subject.SubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',') SubjectMark
		, isnull(check_subject.SubjectMarkIsCorrect, 0) SubjectMarkIsCorrect
		, check_subject.HasAppeal
		, certificate_check.IsDeny
		, certificate_check.DenyComment
		, certificate_check.DenyNewCertificateNumber
		, certificate_check.PassportSeria
		, certificate_check.PassportNumber
		, certificate_check.RegionId
		, certificate_check.RegionName
		, certificate_check.Year
		, certificate_check.TypographicNumber
		, case when ed.[ExpireDate] is null then 'Не найдено' else 
			case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
			case when getdate() <= ed.[ExpireDate] then 'Действительно'
			else 'Истек срок' end end end as [Status],
        isnull(CC.UniqueChecks, 0) UniqueChecks,
        isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
        isnull(CC.UniqueIHECheck, 0) UniqueIHECheck,
        isnull(CC.UniqueIHEFCheck, 0) UniqueIHEFCheck,
        isnull(CC.UniqueTSSaFCheck, 0) UniqueTSSaFCheck,
        isnull(CC.UniqueTSSCheck, 0) UniqueTSSCheck,
        isnull(CC.UniqueTSSFCheck, 0) UniqueTSSFCheck,
        isnull(CC.UniqueRCOICheck, 0) UniqueRCOICheck,
        isnull(CC.UniqueOUOCheck, 0) UniqueOUOCheck,
        isnull(CC.UniqueFounderCheck, 0) UniqueFounderCheck,
        isnull(CC.UniqueOtherCheck, 0) UniqueOtherCheck
	from @certificate_check certificate_check
    	inner join CommonNationalExamCertificate C
        	on C.Id = certificate_check.CertificateId
        left outer join ExamCertificateUniqueChecks CC
			on CC.Id = C.Id
		left join [ExpireDate] as ed on certificate_check.Year = ed.[Year]					
		left outer join (select
				certificate_check.Number 
				, certificate_check.CheckLastName
				, certificate_check.LastName 
				, certificate_check.LastNameIsCorrect
				, certificate_check.CheckFirstName
				, certificate_check.FirstName 
				, certificate_check.FirstNameIsCorrect
				, certificate_check.CheckPatronymicName
				, certificate_check.PatronymicName 
				, certificate_check.PatronymicNameIsCorrect
				, certificate_check.IsExist
				, isnull(check_subject.SubjectId, certificate_subject.SubjectId) SubjectId
				, check_subject.[Mark] CheckSubjectMark
				, certificate_subject.[Mark] SubjectMark
				, case
					when check_subject.Mark = certificate_subject.Mark then 1
					else 0
				end SubjectMarkIsCorrect
				, certificate_subject.HasAppeal
			from CommonNationalExamCertificateSubject certificate_subject with (nolock)
				inner join @certificate_check certificate_check
					on certificate_check.[Year] = certificate_subject.[Year]
						and certificate_check.CertificateId = certificate_subject.CertificateId
				left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
					on check_subject.SubjectId = certificate_subject.SubjectId) check_subject
			left outer join dbo.Subject [subject]
				on [subject].Id = check_subject.SubjectId
			on 1 = 1
			left join [MinimalMark] as mm on check_subject.SubjectId = mm.[SubjectId] and year(getdate()) = mm.[Year] 
	
		if @checkTypographicNumber is not null
			set @eventCode = 'CNE_CHK_TN'
		else
			set @eventCode = 'CNE_CHK'
			
		exec dbo.RegisterEvent 
			@accountId
			, @ip
			, @eventCode
			, @sourceEntityIds
			, @eventParams
			, @updateId = null
	
	return 0
end
GO