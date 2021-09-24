-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (28, '028__2011_06_24__AlterSearchCommonNationalExamCertificate')
-- =========================================================================
GO


-- =============================================
-- �������� ���������� ���.
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

	-- �������� 0 ��������, ��� ����������� �� ������� ��� �� ������
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
    	'declare @CId bigint ' +
		'declare ' +
		'@sourceEntityIds nvarchar(4000) ' + 
		'declare @Search table ' +
		'( ' +
		'LastName nvarchar(255) ' +
		', FirstName nvarchar(255) ' +
		', PatronymicName nvarchar(255) ' +
		', CertificateId bigint ' +
		', CertificateNumber nvarchar(255) ' +
		', RegionId int ' +
		', PassportSeria nvarchar(255) ' +
		', PassportNumber nvarchar(255) ' +
		', TypographicNumber nvarchar(255) ' +
		', Year int ' +
		') ' +
		'insert into @Search ' +
		'select top 300 ' +
		'certificate.LastName ' +
		', certificate.FirstName ' +
		', certificate.PatronymicName ' +
		', certificate.Id ' +
		', certificate.Number ' +
		', certificate.RegionId ' +
		', isnull(certificate.PassportSeria, @internalPassportSeria) ' +
		', isnull(certificate.PassportNumber, @passportNumber) ' +
		', certificate.TypographicNumber ' +
		', certificate.Year ' +
		'from CommonNationalExamCertificate certificate with (nolock) ' +
		'where ' +
		'certificate.[Year] between @yearFrom and @yearTo ' 

	if not @lastName is null 
		set @commandText = @commandText +
			' and certificate.LastName collate cyrillic_general_ci_ai = @lastName '
	
	if not @firstName is null 
		set @commandText = @commandText +
			' and certificate.FirstName collate cyrillic_general_ci_ai = @firstName ' 

	if not @patronymicName is null 
		set @commandText = @commandText +
			' and certificate.PatronymicName collate cyrillic_general_ci_ai = @patronymicName ' 

	if not @internalPassportSeria is null
    begin
    	if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and certificate.InternalPassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.InternalPassportSeria = @internalPassportSeria '
        end
	end

	if not @passportNumber is null
    begin
    	if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
        	set @passportNumber = REPLACE(@passportNumber, '*', '%')
        	set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber '
        end
    	else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber '
        end
    end
	
	if not @typographicNumber is null
		set @commandText = @commandText +
			' and certificate.TypographicNumber = @typographicNumber '
	
	if @lastName is null and @firstName is null and @passportNumber is null
		set @commandText = @commandText +
			' and 0 = 1 '

	if not @subjectMarks is null
	begin
		set @commandText = @commandText + 
			'delete search ' + 
			'from @Search search ' +
			'where exists(select 1 ' +
			'from ' +
			'CommonNationalExamCertificateSubject certificate_subject with(nolock) ' +
			'inner join @Search inner_search ' +
			'on certificate_subject.[Year] between @yearFrom and @yearTo ' +
			'and search.CertificateId = certificate_subject.CertificateId ' +
			'and inner_search.CertificateId = search.CertificateId ' +
			'right join dbo.GetSubjectMarks(@subjectMarks) subject_mark ' +
			'on certificate_subject.SubjectId = subject_mark.SubjectId ' +
			'and certificate_subject.Mark = subject_mark.Mark ' +
			'where ' +
			'certificate_subject.SubjectId is null ' +
			'or certificate_subject.Mark is null) '
	end
	
	set @commandText = @commandText + 
		'set @sourceEntityIds = '''' ' +
		'select ' +
		'@sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(4000), search.CertificateId) ' +
		'from ' +
		'@Search search ' + 
		'set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) ' +
		'if @sourceEntityIds = '''' ' +
		'set @sourceEntityIds = null '

	-- ��������� ������� ���������� �������� 
    -- ��� ������� ���������� ����������� �������� �������� ��������� �������� ��������
	set @commandText = @commandText + 
    	'declare db_cursor cursor for ' +
		'select ' +
        	'distinct S.CertificateId ' +
        'from ' +
        	'@Search S ' +
        'where ' +
        	'S.CertificateId is not null ' +
		
        'open db_cursor ' +
		'fetch next from db_cursor INTO @CId ' +
		'while @@FETCH_STATUS = 0 ' +
        'begin ' +
        	'exec dbo.ExecuteChecksCount ' +
            	'@OrganizationId = @organizationId, ' +
                '@CertificateId = @CId ' +
			'fetch next from db_cursor into @CId ' +
        'end ' +
        
        ' close db_cursor ' +
		'deallocate db_cursor '

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
				'when ed.[ExpireDate] is null then ''�� �������'' ' +
                'else ' + 
					'case ' + 
                    	'when CD.Id is not null then ''������������'' ' + 
                        'else ' +
                        	'case ' + 
                            	'when getdate() <= ed.[ExpireDate] then ''�������������'' ' + 
                                'else ''����� ����'' ' + 
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
		'exec dbo.RegisterEvent ' +
			'@accountId = @editorAccountId, ' +
			'@ip = @ip, ' +
			'@eventCode = @eventCode, ' +
			'@sourceEntityIds = @sourceEntityIds, ' +
			'@eventParams = @eventParams, ' +
			'@updateId = null ' 
       
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
go