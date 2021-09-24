insert into Migrations(MigrationVersion, MigrationName) values (30, '030_2013_06_27_fbs_check_db.sql')
go

alter PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByPassport]
    @batchId BIGINT,
    @type INT = 1
AS 
    BEGIN

        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX),
            @yearCommandText NVARCHAR(MAX),
            @executeCommandText NVARCHAR(MAX),
            @searchCommandText NVARCHAR(MAX),
            @searchCommandText2 NVARCHAR(MAX),
            @addCondition NVARCHAR(MAX),
            @selectPass NVARCHAR(500)
        IF @type = 1 
            BEGIN
	
                SET @yearCommandText = N'
	 declare
			@yearFrom int
			, @yearTo int			
			, @year int
		select  
			@yearFrom = actuality_years.YearFrom,
			@yearTo = actuality_years.YearTo
		from
			dbo.GetCommonNationalExamCertificateActuality() actuality_years
		'		
                SET @addCondition = 'exam_certificate.Number collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai'
                SET @selectPass = ', exam_certificate.PassportSeria
			, exam_certificate.PassportNumber'
            END
        IF @type = 2 
            BEGIN
                UPDATE  #CommonNationalExamCertificateCheck
                SET PassportSeria = REPLACE(PassportSeria, ' ', '')
                
                SET @yearCommandText = N'
					declare
							@yearFrom int
							, @yearTo int			
							, @year int

						select @year = cnecrb.year
						from CommonNationalExamCertificateCheckBatch as cnecrb
						where cnecrb.id = <BatchIdentifier>

						if @year is not null
							select @yearFrom = @year, @yearTo = @year
						else
							select @yearFrom = Year(GetDate()) - 5, @yearTo = Year(GetDate())
						'		
                SET @addCondition = '(exam_certificate.PassportSeria collate cyrillic_general_ci_ai = exam_certificate_check.PassportSeria collate cyrillic_general_ci_ai)
					 and exam_certificate.PassportNumber collate cyrillic_general_ci_ai = exam_certificate_check.PassportNumber collate cyrillic_general_ci_ai
					'
                SET @selectPass = ', exam_certificate_check.PassportSeria
								   , exam_certificate_check.PassportNumber'
            END
        SET @chooseDbText = REPLACE('use <database>', '<database>',
                                    dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)

        SET @declareCommandText = N'
		if (select count(*) from #CommonNationalExamCertificateCheck)=0
		begin
		 raiserror(''CommonNationalExamCertificateCheck is empty'',16,1)
		 return
		end
		
		declare @certificateCheckIds table
			(
			Id bigint
			, CertificateNumber nvarchar(255)
			)

		declare @certificate_subject_correctness table
			(
			
			CertificateIdGuid uniqueidentifier
			, CertificateSubjectId bigint
			, Mark numeric(5,1)
			, HasAppeal bit
			, IsSubjectCorrect bit
			, SourceMark numeric(5,1)
			, SourceCertificateSubjectIdGuid uniqueidentifier
			, BatchId bigint
			, CertificateCheckingId uniqueidentifier
			, ShareCertificateCheckingId uniqueidentifier
			, Year int
			, SubjectCode nvarchar(255)
			)
			declare @certificate_subject_correctness_result table
			(
			
			CertificateIdGuid uniqueidentifier
			, CertificateSubjectId bigint
			, Mark numeric(5,1)
			, HasAppeal bit
			, IsSubjectCorrect bit
			, SourceMark numeric(5,1)
			, SourceCertificateSubjectIdGuid uniqueidentifier
			, BatchId bigint
			, CertificateCheckingId uniqueidentifier
			, ShareCertificateCheckingId uniqueidentifier
			, Year int
			, SubjectCode nvarchar(255)
			)
		
		declare @certificate_correctness table
			(
				id int not null identity(1,1) primary key,
			CertificateNumber nvarchar(255)
			, CertificateIdGuid uniqueidentifier
			, LastName nvarchar(255)
			, FirstName nvarchar(255)
			, PatronymicName nvarchar(255)
			, SourceLastName nvarchar(255)
			, SourceFirstName nvarchar(255)
			, SourcePatronymicName nvarchar(255)
			, IsCertificateCorrect bit
			, BatchId bigint
			, IsDeny bit
			, DenyComment ntext 
			, DenyNewCertificateNumber nvarchar(255)
			, CertificateYear int
			, CertificateCheckingId uniqueidentifier
			, ShareCertificateCheckingId uniqueidentifier
			, [Index] bigint
			, TypographicNumber nvarchar(255)
			, RegionId int
			, PassportSeria nvarchar(255)
			, PassportNumber nvarchar(255)
			, ParticipantID uniqueidentifier
			)
			'
        SET @searchCommandText = N'	
			
		select 	exam_certificate.id as ID1,exam_certificate_check.id as ID2,ParticipantID into #tt
		from #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
			left join (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
		        isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
		        b.TypographicNumber, a.ParticipantID, b.CreateDate
			FROM <dataDbName>.rbd.Participants a with (nolock, fastfirstrow) 
				left JOIN <dataDbName>.prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID)	exam_certificate
			on	<addCondition>
			and exam_certificate.[Year] between @yearFrom and @yearTo	
		where
			exam_certificate_check.BatchId = <BatchIdentifier>					
				 		
		insert @certificate_correctness
			(
			CertificateNumber
			, CertificateIdGuid
			, LastName
			, FirstName
			, PatronymicName
			, SourceLastName
			, SourceFirstName
			, SourcePatronymicName
			, IsCertificateCorrect
			, BatchId
			, IsDeny
			, DenyComment
			, DenyNewCertificateNumber
			, CertificateYear
			, CertificateCheckingId
			,ShareCertificateCheckingId
			, [Index]
			, TypographicNumber
			, RegionId
			, PassportSeria
			, PassportNumber
			, ParticipantID
			)
		select 
			isnull(exam_certificate_check.CertificateNumber,exam_certificate.Number)
			, exam_certificate.Id
			, exam_certificate_check.LastName
			, exam_certificate_check.FirstName
			, exam_certificate_check.PatronymicName 
			, exam_certificate.LastName
			, exam_certificate.FirstName
			, exam_certificate.PatronymicName 
			, case when exam_certificate.Number is not null and exam_certificate_deny.CertificateFK is null then 1 else 0	end	
			, <BatchIdentifier>
			, case
				when not exam_certificate_deny.CertificateFK is null then 1
				else 0
			end
			, exam_certificate_deny.Reason
			, exam_certificate_deny.CertificateFK
			, exam_certificate.[Year]
			, exam_certificate_check.CertificateCheckingId
			, exam_certificate_check.ShareCertificateCheckingId
			, exam_certificate_check.[Index]
			, exam_certificate.TypographicNumber
			, exam_certificate.RegionId
			<@selectPass>
			, exam_certificate.ParticipantID
		from 
			#CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
			left join #tt a on a.ID2=exam_certificate_check.id
			left join (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
		        isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
		        b.TypographicNumber, a.ParticipantID, b.CreateDate
			FROM <dataDbName>.rbd.Participants a with (nolock, fastfirstrow) 
				left JOIN <dataDbName>.prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) exam_certificate on case when a.ID1=exam_certificate.id	then 1 when a.ParticipantID=exam_certificate.ParticipantID then 1 else 0 end =1
				left join <dataDbName>.prn.CancelledCertificates exam_certificate_deny with(nolock)
					on exam_certificate_deny.CertificateFK = exam_certificate.id
						and exam_certificate_deny.[UseYear] between @yearFrom and @yearTo
				

		drop table #tt
		/*from
			#CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
				left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
					on  
					<addCondition>
						and exam_certificate.[Year] between @yearFrom and @yearTo						
				left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
					on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate.Number collate cyrillic_general_ci_ai
						and exam_certificate_deny.[Year] between @yearFrom and @yearTo
		where
			exam_certificate_check.BatchId = <BatchIdentifier>*/		
			
		'
        SET @searchCommandText = REPLACE(@searchCommandText, '<@selectPass>',
                                         @selectPass)		
        SET @searchCommandText = @yearCommandText + REPLACE(@searchCommandText,
                                                            '<addCondition>',
                                                            @addCondition)
        SET @searchCommandText2 = ' 
		insert @certificate_subject_correctness_result
			(
			CertificateIdGuid
			, CertificateSubjectId
			, Mark 
			, HasAppeal
			, IsSubjectCorrect
			, SourceMark 
			, SourceCertificateSubjectIdGuid	
			, BatchId
			, CertificateCheckingId
			,ShareCertificateCheckingId
			, Year
			,SubjectCode
			)
		select 
			isnull(certificate_correctness.[CertificateIdGuid], exam_certificate_subject.[CertificateFK])
			, isnull(exam_certificate_subject.SubjectCode, subject.Id)
			, exam_certificate_subject_check.Mark
			, exam_certificate_subject.HasAppeal
			, case
				when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
					then 1
				else 0
			end 
			, exam_certificate_subject.Mark
			, exam_certificate_subject.CertificateMarkID
			, <BatchIdentifier>
			, coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
			, coalesce(certificate_correctness.[ShareCertificateCheckingId], exam_certificate_subject_check.[ShareCertificateCheckingId])
			, coalesce(exam_certificate_subject.Useyear, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
			,subject.Code
		from 
			<dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
				inner join @certificate_correctness certificate_correctness
					on case when certificate_correctness.CertificateIdGuid = exam_certificate_subject.CertificateFK then 1 
					        when certificate_correctness.ParticipantID = exam_certificate_subject.ParticipantFK then 1 else 0 end =1
						and certificate_correctness.CertificateYear = exam_certificate_subject.[useyear]
				full outer join 
				( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
					inner join dbo.Subject subject with(nolock)
						on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai	 					
						) 	
						on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
				
		declare @CertificateCheckingId uniqueidentifier,@id int,@NewCertificateCheckingId uniqueidentifier
		declare certificate_correctness_cursor cursor local fast_forward for select id,CertificateCheckingId from @certificate_correctness			
		open certificate_correctness_cursor
		fetch next from certificate_correctness_cursor into @id,@CertificateCheckingId
		while @@FETCH_STATUS=0
		begin
			if exists(select * from @certificate_correctness where CertificateCheckingId=@CertificateCheckingId and id<>@id)
			begin
				set @NewCertificateCheckingId=NEWID()
				update @certificate_correctness set CertificateCheckingId=@NewCertificateCheckingId where id=@id
				insert #CommonNationalExamCertificateSubjectCheck(ShareCertificateCheckingId,CertificateCheckingId,BatchId,SubjectCode,Mark) 
				select @CertificateCheckingId,@NewCertificateCheckingId,BatchId,SubjectCode,Mark from #CommonNationalExamCertificateSubjectCheck where CertificateCheckingId=@CertificateCheckingId
			end
			fetch next from certificate_correctness_cursor into @id,@CertificateCheckingId
		end 
close certificate_correctness_cursor
deallocate certificate_correctness_cursor
--select * from @certificate_subject_correctness_result
--select * from #CommonNationalExamCertificateCheck--SubjectCheck 

		insert @certificate_subject_correctness
			(
			CertificateIdGuid
			, CertificateSubjectId
			, Mark 
			, HasAppeal
			, IsSubjectCorrect
			, SourceMark 
			, SourceCertificateSubjectIdGuid	
			, BatchId
			, CertificateCheckingId
			,ShareCertificateCheckingId
			, Year
			,SubjectCode
			)
		select 
			coalesce(certificate_correctness.[CertificateIdGuid], exam_certificate_subject.[CertificateFK])
			, isnull(exam_certificate_subject.SubjectCode, subject.Id)
			, exam_certificate_subject_check.Mark
			, exam_certificate_subject.HasAppeal
			, case
				when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
					then 1
				else 0
			end 
			, exam_certificate_subject.Mark
			, exam_certificate_subject.CertificateMarkID
			, <BatchIdentifier>
			, coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
			, coalesce(certificate_correctness.[ShareCertificateCheckingId], exam_certificate_subject_check.[ShareCertificateCheckingId])
			, coalesce(exam_certificate_subject.useyear, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
			,subject.Code
		from 
			<dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
				inner join @certificate_correctness certificate_correctness
					on case when certificate_correctness.CertificateIdGuid = exam_certificate_subject.CertificateFK then 1 
					        when certificate_correctness.ParticipantID = exam_certificate_subject.ParticipantFK then 1 else 0 end =1
						and certificate_correctness.CertificateYear = exam_certificate_subject.[useyear]
				full outer join 
				( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
					inner join dbo.Subject subject with(nolock)
						on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai 						
						) 	
						on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId

--select * from @certificate_subject_correctness order by 1
select * from @certificate_subject_correctness_result order by 1
declare @idcert uniqueidentifier
declare cur cursor for select ShareCertificateCheckingId from @certificate_correctness
open cur
fetch next from cur into @idcert
while @@fetch_status=0
begin
--select @idcert
 if exists(select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert  and mark is not null and SourceCertificateSubjectIdGuid is null)
 begin 
 --print -999 
  if <@type>=2
  update @certificate_correctness 
  set CertificateNumber=NULL,CertificateIdGuid=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
  select * from @certificate_correctness 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectIdGuid=null,year=null,SubjectCode=null,HasAppeal=null
  where ShareCertificateCheckingId=@idcert 
 end
--select * from @certificate_correctness where ShareCertificateCheckingId=@idcert 
--select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert 
--—делал так чтобы если есть нет не одного совпадени€ то занул€ть проверку. –азделил на два блока так как не знаб логику работы первого блока
 if exists(select * from 
				(
					select max(cast(IsSubjectCorrect as int)) IsSubjectCorrect 
					from @certificate_subject_correctness_result 
					where ShareCertificateCheckingId=@idcert and mark is not null
					group by SubjectCode
				) t 
				where IsSubjectCorrect=0)
	and exists(select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert and mark is not null)
 begin 
 --print -999 
  if <@type>=2
  update @certificate_correctness 
  set CertificateNumber=NULL,CertificateIdGuid=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
  select 1
  --select * from @certificate_correctness 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectIdGuid=null,year=null,SubjectCode=null,HasAppeal=null
  where ShareCertificateCheckingId=@idcert 
 end
 
 fetch next from cur into @idcert
end
close cur deallocate cur

 if <@type>=2
 begin
  delete b from  
  @certificate_correctness a 
  join @certificate_correctness b on a.PassportNumber=b.PassportNumber and a.PassportSeria=b.PassportSeria
  where a.CertificateIdGuid is null and b.CertificateIdGuid is null
  and  b.id>a.id
 end
--select * from @certificate_correctness
--select * from @certificate_subject_correctness order by CertificateIdGuid,CertificateSubjectId


		'

	
        SET @executeCommandText = N'
		delete exam_certificate_check
		from dbo.CommonNationalExamCertificateCheck exam_certificate_check
		where exam_certificate_check.BatchId = <BatchIdentifier>

		declare @certificateCheckId table
			(
			CertificateCheckId bigint
			, CheckingCertificateId uniqueidentifier
			)
		
		insert dbo.CommonNationalExamCertificateCheck
			(
			CertificateCheckingId
			, BatchId
			, CertificateNumber
			, LastName
			, FirstName
			, PatronymicName
			, IsCorrect
			, SourceCertificateIdGuid
			, SourceLastName
			, SourceFirstName
			, SourcePatronymicName
			, IsDeny
			, DenyComment
			, DenyNewCertificateNumber
			, Year
			, TypographicNumber
			, RegionId
			, PassportSeria
			, PassportNumber
			, ParticipantFK
			)
		output inserted.Id, inserted.CertificateCheckingId 
			into @certificateCheckId 
		select 
			certificate_correctness.CertificateCheckingId
			, <BatchIdentifier>
			, isnull(certificate_correctness.CertificateNumber,''Ќет свидетельства'')
			, isnull(certificate_correctness.LastName,'''')
			, certificate_correctness.FirstName
			, certificate_correctness.PatronymicName
			, case 
				when (certificate_correctness.IsCertificateCorrect = 1 
					and not exists(select 1 
						from @certificate_subject_correctness subject_correctness
						where subject_correctness.IsSubjectCorrect = 0 
							and certificate_correctness.CertificateIdGuid = subject_correctness.CertificateIdGuid
							and certificate_correctness.CertificateCheckingId = subject_correctness.CertificateCheckingId)) 
					then 1
				else 0
			end 
			, certificate_correctness.CertificateIdGuid
			, certificate_correctness.SourceLastName
			, certificate_correctness.SourceFirstName
			, certificate_correctness.SourcePatronymicName
			, certificate_correctness.IsDeny
			, certificate_correctness.DenyComment
			, certificate_correctness.DenyNewCertificateNumber
			, certificate_correctness.CertificateYear
			, certificate_correctness.TypographicNumber
			, certificate_correctness.RegionId
			, certificate_correctness.PassportSeria
			, certificate_correctness.PassportNumber
			, certificate_correctness.ParticipantID
		from @certificate_correctness certificate_correctness	
						
		delete exam_certificate_subject_check
		from dbo.CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check
		where exam_certificate_subject_check.BatchId = <BatchIdentifier>

		insert dbo.CommonNationalExamCertificateSubjectCheck
			(
			BatchId
			, CheckId
			, SubjectId
			, Mark
			, IsCorrect
			, SourceCertificateSubjectIdGuid
			, SourceMark
			, SourceHasAppeal
			, Year
			) 
		select 
		distinct
			<BatchIdentifier>
			, certificate_check_id.CertificateCheckId
			, subject_correctness.CertificateSubjectId 
			, subject_correctness.Mark
			, subject_correctness.IsSubjectCorrect 
			, subject_correctness.SourceCertificateSubjectIdGuid
			, subject_correctness.SourceMark
			, subject_correctness.HasAppeal
			, subject_correctness.Year
		from 
			@certificate_subject_correctness subject_correctness
				inner join @certificateCheckId certificate_check_id
					on certificate_check_id.CheckingCertificateId = subject_correctness.CertificateCheckingId 
      
		-- ѕодсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
   
       --select * from CommonNationalExamCertificateSubjectCheck where BatchId=<BatchIdentifier>    order by checkid,subjectid 
       --drop table rrr
       -- 	select * into rrr from #CommonNationalExamCertificateCheck
       -- 	drop table ttt
       -- 	select * into ttt from #CommonNationalExamCertificateSubjectCheck
		'

        SET @searchCommandText = REPLACE(REPLACE(@searchCommandText,
                                                 '<BatchIdentifier>',
                                                 CONVERT(NVARCHAR(255), @batchId)),
                                         '<dataDbName>', @baseName)
        SET @searchCommandText2 = REPLACE(REPLACE(REPLACE(@searchCommandText2,
                                                          '<BatchIdentifier>',
                                                          CONVERT(NVARCHAR(255), @batchId)),
                                                  '<dataDbName>', @baseName),
                                          '<@type>',
                                          CAST(@type AS VARCHAR(10)))
        SET @executeCommandText = REPLACE(REPLACE(@executeCommandText,
                                                  '<BatchIdentifier>',
                                                  CONVERT(NVARCHAR(255), @batchId)),
                                          '<dataDbName>', @baseName)

--print '---begin: PCNECC---\n'
--print @chooseDbText
--print @declareCommandText
--print @searchCommandText
--print @searchCommandText2
--print @executeCommandText
--print '\n---end: PCNECC---'

        DECLARE @commandText NVARCHAR(MAX)
        SET @commandText = @chooseDbText + @declareCommandText
            + @searchCommandText + @searchCommandText2 + @executeCommandText
	
        EXEC sp_executesql @commandText

        RETURN 0
    END

