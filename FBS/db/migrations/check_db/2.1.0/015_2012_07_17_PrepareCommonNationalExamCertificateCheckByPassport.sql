-- =========================================================================
-- «апись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (15, '015_2012_07_17_PrepareCommonNationalExamCertificateCheckByPassport')
-- =========================================================================
GO 


ALTER PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByPassport]
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
                SET @addCondition = '(exam_certificate.InternalPassportSeria collate cyrillic_general_ci_ai = exam_certificate_check.PassportSeria collate cyrillic_general_ci_ai)
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
			
			CertificateId bigint
			, CertificateSubjectId bigint
			, Mark numeric(5,1)
			, HasAppeal bit
			, IsSubjectCorrect bit
			, SourceMark numeric(5,1)
			, SourceCertificateSubjectId bigint
			, BatchId bigint
			, CertificateCheckingId uniqueidentifier
			, ShareCertificateCheckingId uniqueidentifier
			, Year int
			, SubjectCode nvarchar(255)
			)
			declare @certificate_subject_correctness_result table
			(
			
			CertificateId bigint
			, CertificateSubjectId bigint
			, Mark numeric(5,1)
			, HasAppeal bit
			, IsSubjectCorrect bit
			, SourceMark numeric(5,1)
			, SourceCertificateSubjectId bigint
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
			, CertificateId bigint
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
			)
			'
        SET @searchCommandText = N'	
			
		select 	exam_certificate.id as ID1,exam_certificate_check.id as ID2 into #tt
		from #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)			
			left join <dataDbName>.dbo.CommonNationalExamCertificate exam_certificate with(nolock) 
			on	<addCondition>
			and exam_certificate.[Year] between @yearFrom and @yearTo	
		where
			exam_certificate_check.BatchId = <BatchIdentifier>						
		
		insert @certificate_correctness
			(
			CertificateNumber
			, CertificateId
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
			, case when exam_certificate.Number is not null and exam_certificate_deny.Id is null then 1 else 0	end	
			, <BatchIdentifier>
			, case
				when not exam_certificate_deny.Id is null then 1
				else 0
			end
			, exam_certificate_deny.Comment
			,  exam_certificate_deny.NewCertificateNumber
			, exam_certificate.[Year]
			, exam_certificate_check.CertificateCheckingId
			, exam_certificate_check.ShareCertificateCheckingId
			, exam_certificate_check.[Index]
			, exam_certificate.TypographicNumber
			, exam_certificate.RegionId
			<@selectPass>
		from 
			#CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
			left join #tt a on a.ID2=exam_certificate_check.id
			left join <dataDbName>.dbo.CommonNationalExamCertificate exam_certificate with(nolock)  on a.ID1=exam_certificate.id			
				left join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
					on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate.Number collate cyrillic_general_ci_ai
						and exam_certificate_deny.[Year] between @yearFrom and @yearTo
				
			
		drop table #tt
		/*from
			#CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
				left join <dataDbName>.dbo.CommonNationalExamCertificate exam_certificate with(nolock)
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
			CertificateId
			, CertificateSubjectId
			, Mark 
			, HasAppeal
			, IsSubjectCorrect
			, SourceMark 
			, SourceCertificateSubjectId	
			, BatchId
			, CertificateCheckingId
			,ShareCertificateCheckingId
			, Year
			,SubjectCode
			)
		select 
			coalesce(certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateId])
			, isnull(exam_certificate_subject.SubjectId, subject.Id)
			, exam_certificate_subject_check.Mark
			, exam_certificate_subject.HasAppeal
			, case
				when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
					then 1
				else 0
			end 
			, exam_certificate_subject.Mark
			, exam_certificate_subject.Id
			, <BatchIdentifier>
			, coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
			, coalesce(certificate_correctness.[ShareCertificateCheckingId], exam_certificate_subject_check.[ShareCertificateCheckingId])
			, coalesce(exam_certificate_subject.year, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
			,subject.Code
		from 
			<dataDbName>.dbo.CommonNationalExamCertificateSubject exam_certificate_subject with(nolock)
				inner join @certificate_correctness certificate_correctness
					on certificate_correctness.CertificateId = exam_certificate_subject.CertificateId
						and certificate_correctness.CertificateYear = exam_certificate_subject.[Year]
				full outer join 
				( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
					inner join dbo.Subject subject with(nolock)
						on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai	 					
						) 	
						on exam_certificate_subject.SubjectId = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
							
							
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
--select * from @certificate_correctness
--select * from #CommonNationalExamCertificateCheck--SubjectCheck 

		insert @certificate_subject_correctness
			(
			CertificateId
			, CertificateSubjectId
			, Mark 
			, HasAppeal
			, IsSubjectCorrect
			, SourceMark 
			, SourceCertificateSubjectId	
			, BatchId
			, CertificateCheckingId
			,ShareCertificateCheckingId
			, Year
			,SubjectCode
			)
		select 
			coalesce(certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateId])
			, isnull(exam_certificate_subject.SubjectId, subject.Id)
			, exam_certificate_subject_check.Mark
			, exam_certificate_subject.HasAppeal
			, case
				when exam_certificate_subject.Mark = exam_certificate_subject_check.Mark
					then 1
				else 0
			end 
			, exam_certificate_subject.Mark
			, exam_certificate_subject.Id
			, <BatchIdentifier>
			, coalesce(certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
			, coalesce(certificate_correctness.[ShareCertificateCheckingId], exam_certificate_subject_check.[ShareCertificateCheckingId])
			, coalesce(exam_certificate_subject.year, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
			,subject.Code
		from 
			<dataDbName>.dbo.CommonNationalExamCertificateSubject exam_certificate_subject with(nolock)
				inner join @certificate_correctness certificate_correctness
					on certificate_correctness.CertificateId = exam_certificate_subject.CertificateId
						and certificate_correctness.CertificateYear = exam_certificate_subject.[Year]
				full outer join 
				( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
					inner join dbo.Subject subject with(nolock)
						on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai 						
						) 	
						on exam_certificate_subject.SubjectId = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
							
--select * from @certificate_subject_correctness order by 1
--select * from @certificate_subject_correctness_result order by 1
declare @idcert uniqueidentifier
declare cur cursor for select ShareCertificateCheckingId from @certificate_correctness
open cur
fetch next from cur into @idcert
while @@fetch_status=0
begin
--select @idcert
 if exists(select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert  and mark is not null and SourceCertificateSubjectId is null)
 begin 
 --print -999 
  if <@type>=2
  update @certificate_correctness 
  set CertificateNumber=NULL,CertificateId=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
  select * from @certificate_correctness 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectId=null,year=null,SubjectCode=null,HasAppeal=null
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
  set CertificateNumber=NULL,CertificateId=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
  select 1
  --select * from @certificate_correctness 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectId=null,year=null,SubjectCode=null,HasAppeal=null
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
  where a.Certificateid is null and b.Certificateid is null
  and  b.id>a.id
 end
--select * from @certificate_correctness
--select * from @certificate_subject_correctness order by CertificateId,CertificateSubjectId


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
			, SourceCertificateId
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
			)
		output inserted.Id, inserted.CertificateCheckingId 
			into @certificateCheckId 
		select 
			certificate_correctness.CertificateCheckingId
			, <BatchIdentifier>
			, isnull(certificate_correctness.CertificateNumber,'''')
			, isnull(certificate_correctness.LastName,'''')
			, certificate_correctness.FirstName
			, certificate_correctness.PatronymicName
			, case 
				when (certificate_correctness.IsCertificateCorrect = 1 
					and not exists(select 1 
						from @certificate_subject_correctness subject_correctness
						where subject_correctness.IsSubjectCorrect = 0 
							and certificate_correctness.CertificateId = subject_correctness.CertificateId
							and certificate_correctness.CertificateCheckingId = subject_correctness.CertificateCheckingId)) 
					then 1
				else 0
			end 
			, certificate_correctness.CertificateId
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
			, SourceCertificateSubjectId
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
			, subject_correctness.SourceCertificateSubjectId
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
