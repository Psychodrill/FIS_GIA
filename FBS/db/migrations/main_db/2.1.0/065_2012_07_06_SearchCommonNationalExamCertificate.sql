-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (65, '065_2012_07_06_SearchCommonNationalExamCertificate')
-- =========================================================================

/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 06/08/2012 12:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificate]
GO

/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 06/08/2012 12:20:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Получить сертификат ЕГЭ.
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificate]
	@lastName nvarchar(255) = null			  -- фамилия сертифицируемого
	, @firstName nvarchar(255) = null		  -- имя сертифицируемого
	, @patronymicName nvarchar(255) = null	  -- отчетсво сертифицируемого
	, @subjectMarks nvarchar(4000) = null	  -- средние оценки по предметам (через запятую, в определенном порядке)
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
	, @typographicNumber nvarchar(255) = null -- типографический номер сертификата 
	, @login nvarchar(255)					  -- логин проверяющего
	, @ip nvarchar(255)						  -- ip проверяющего
	, @year int = null					      -- год выдачи сертификата
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

	declare @CId bigint,@sourceEntityIds nvarchar(4000) 
	declare @Search table 
	( 
		LastName nvarchar(255) 
		, FirstName nvarchar(255) 
		, PatronymicName nvarchar(255) 
		, CertificateId bigint 
		, CertificateNumber nvarchar(255) 
		, RegionId int 
		, PassportSeria nvarchar(255)
		, PassportNumber nvarchar(255) 
		, TypographicNumber nvarchar(255) 
		, Year int 
	) 
		
	set @commandText = @commandText +     	
		'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
						certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
						isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year 
		from CommonNationalExamCertificate certificate with (nolock)
		where 
		certificate.[Year] between @yearFrom and @yearTo ' 

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

	print @commandText 
	
	insert into @Search
	exec sp_executesql @commandText
		, N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
			, @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int '
		, @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo

	if not @subjectMarks is null
	begin
		delete search 
		from @Search search 
		where exists(select 1 
					from CommonNationalExamCertificateSubject certificate_subject with(nolock) 
						inner join @Search inner_search on certificate_subject.[Year] between @yearFrom and @yearTo 
							and search.CertificateId = certificate_subject.CertificateId and inner_search.CertificateId = search.CertificateId 
						right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectId = subject_mark.SubjectId 
							and certificate_subject.Mark = subject_mark.Mark 
					where certificate_subject.SubjectId is null or certificate_subject.Mark is null)	
	end
	
	set @sourceEntityIds = ''
	
	select @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), search.CertificateId) 
	from @Search search 
	
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
		
	if @sourceEntityIds = ''
		set @sourceEntityIds = null 

	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

	declare @Search1 table 
	( pkid int identity(1,1) primary key, CertificateId bigint
	)     
	insert @Search1
    select distinct S.CertificateId 
		from @Search S   
	where CertificateId > 0	
	
	declare @CertificateId bigint,@pkid int
	while exists(select * from @Search1)
	begin
	  select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateId = @CertificateId 
                	  
	  delete @Search1 where pkid=@pkid
	end 

	select 
			S.CertificateId, 
			S.CertificateNumber,
			S.LastName LastName,
			S.FirstName FirstName,
			S.PatronymicName PatronymicName,
			S.PassportSeria PassportSeria,
			S.PassportNumber PassportNumber,
			S.TypographicNumber TypographicNumber,
			region.Name RegionName, 
			case 
				when S.CertificateId>0 then 1 
				else 0 
			end IsExist, 
			case 
				when CD.Id >0 then 1 
			end IsDeny,  
			CD.Comment DenyComment, 
			CD.NewCertificateNumber, 
			S.Year,
			case 
				when ed.[ExpireDate] is null then 'Не найдено'
               	when CD.Id>0 then 'Аннулировано' 
               	when getdate() <= ed.[ExpireDate] then 'Действительно' 
                else 'Истек срок' 
			end as [Status], 
            CC.UniqueChecks UniqueChecks, 
			CC.UniqueIHEaFCheck UniqueIHEaFCheck,
			CC.UniqueIHECheck UniqueIHECheck, 
			CC.UniqueIHEFCheck UniqueIHEFCheck, 
			CC.UniqueTSSaFCheck UniqueTSSaFCheck,
			CC.UniqueTSSCheck UniqueTSSCheck, 
			CC.UniqueTSSFCheck UniqueTSSFCheck,
			CC.UniqueRCOICheck UniqueRCOICheck,
			CC.UniqueOUOCheck UniqueOUOCheck, 
			CC.UniqueFounderCheck UniqueFounderCheck,
			CC.UniqueOtherCheck UniqueOtherCheck 
		from 
			@Search S 
            inner join CommonNationalExamCertificate C with (nolock) 
            	on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
				on CC.Id  = C.Id 
			left outer join CommonNationalExamCertificateDeny CD with (nolock) 
				on CD.[Year] between @yearFrom and @yearTo 
                and S.CertificateNumber = CD.CertificateNumber 
			left outer join dbo.Region region with (nolock)
				on region.[Id] = S.RegionId 
			left join [ExpireDate] ed
            	on ed.[year] = S.[year]
            
	exec dbo.RegisterEvent 
			@accountId = @editorAccountId, 
			@ip = @ip, 
			@eventCode = @eventCode, 
			@sourceEntityIds = @sourceEntityIds, 
			@eventParams = @eventParams, 
			@updateId = null 
			
	return 0
end

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END

GO


