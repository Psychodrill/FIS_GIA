-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (4, '004__2011_01_14__SearchByPassport')
-- =========================================================================




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
		, @editorAccountId bigint
		, @eventParams nvarchar(4000)
		, @yearFrom int
		, @yearTo int
		, @commandText nvarchar(4000)
		, @internalPassportSeria nvarchar(255)

	set @eventParams = 
		isnull(@lastName,'') + '|' 
		+ isnull(@firstName,'') + '|' 
		+ isnull(@patronymicName,'') + '|' 
		+ isnull(@subjectMarks, '') + '|' 
		+ isnull(@passportSeria, '') + '|' 
		+ isnull(@passportNumber, '') + '|' 
		+ isnull(@typographicNumber, '')

	select
		@editorAccountId = account.[Id]
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
    	if CHARINDEX('?', @internalPassportSeria) > 0 or CHARINDEX('_', @internalPassportSeria) > 0
        begin
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '%')
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
    	if CHARINDEX('?', @passportNumber) > 0 or CHARINDEX('_', @passportNumber) > 0
        begin
        	set @passportNumber = REPLACE(@passportNumber, '?', '%')
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

	set @commandText = @commandText + 
		'select ' +
		'	search.CertificateId ' +
		'	,search.CertificateNumber ' +
		'	, search.LastName LastName ' +
		'	, search.FirstName FirstName ' +
		'	, search.PatronymicName PatronymicName ' +
		'	, search.PassportSeria PassportSeria ' +
		'	, search.PassportNumber PassportNumber ' +
		'	, search.TypographicNumber TypographicNumber ' +
		'	, region.Name RegionName ' +
		'	, case ' +
		'		when not search.CertificateId is null then 1 ' +
		'		else 0 ' +
		'	end IsExist ' +
		'	, case ' +
		'		when not cne_certificate_deny.Id is null then 1 ' +
		'		else 0 ' +
		'	end IsDeny ' +
		'	, cne_certificate_deny.Comment DenyComment ' +
		'	, cne_certificate_deny.NewCertificateNumber ' +
		'	, search.Year ' +
		'	, case when ed.[ExpireDate] is null then ''Не найдено'' else 
					  case when cne_certificate_deny.Id is not null then ''Аннулировано'' else
						case when getdate() <= ed.[ExpireDate] then ''Действительно'' 
					  else ''Истек срок'' end end end as [Status] ' +
		' from @Search search ' +
		'	left outer join CommonNationalExamCertificateDeny cne_certificate_deny with (nolock) ' +
		'		on cne_certificate_deny.[Year] between @yearFrom and @yearTo ' +
		'			and search.CertificateNumber = cne_certificate_deny.CertificateNumber ' +
		'	left outer join dbo.Region region with (nolock) ' +
		'		on region.[Id] = search.RegionId ' +
		'	left join ExpireDate ed on  ed.year = search.year ' +
		' exec dbo.RegisterEvent ' +
		'	@accountId = @editorAccountId ' + 
		'	, @ip = @ip ' +
		'	, @eventCode = @eventCode ' +
		'	, @sourceEntityIds = @sourceEntityIds ' + 
		'	, @eventParams = @eventParams ' +
		'	, @updateId = null ' 
		
	exec sp_executesql @commandText
		, N'@lastName nvarchar(255), @firstName nvarchar(255)
			, @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
			, @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @subjectMarks nvarchar(4000)
			, @yearFrom int, @yearTo int 
			, @editorAccountId bigint
			, @ip nvarchar(255) 
			, @eventCode nvarchar(255) 
			, @eventParams nvarchar(4000) '
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

	return 0
end