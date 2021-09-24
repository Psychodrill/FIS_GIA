﻿/*
   Для работы  процедуры  требуются  следующие 
   временные таблицы:
   create table #CommonNationalExamCertificateCheck
    ( 
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , CertificateNumber nvarchar(255)
    , LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , Index bigint
    )
  create table #CommonNationalExamCertificateSubjectCheck
    (
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , SubjectCode nvarchar(255)
    , Mark numeric(5,1)
    )
*/

-- exec dbo.PrepareCommonNationalExamCertificateCheck 
-- ================================================
-- Подготовка пакетов для проверки сертификатов ЕГЭ.
-- v.1.0: Created by Sedov A.G. 22.05.2008
-- v.1.1: Modified by Fomin Dmitriy 31.05.2008
-- Удаление старых данных по пакету, убрано поле HasAppeal.
-- v.1.2: Modified by Sedov Anton 03.06.2008
-- Удалёно использование параметра CertificateCheckingId
-- v.1.3: Modified by Sedov Anton 17.06.2008
-- Добавлена проверка пакетов
-- v.1.4: Modified by Sedov Anton 18.06.2008
-- Выполнена оптимизация  процедуры
-- v.1.5: Modified by Sedov Anton 04.07.2008
-- Добавлено поле DenyNewCertificateNumber
-- и NewСertificateNumber в таблицы проверок
-- сертификатов и аннулированных сертифкатов 
-- соответствено. 
-- v.1.6: Modified by Sedov Anton 09.07.2008
-- Исправлена логика динамического выбора БД
-- таблицы котрой  используются для получения
-- данных о сертификатах
-- v.1.7: Modified by Sedov Anton 28.07.2008
-- Добавлен параметр Index во временную таблицу
-- сертификатов.
-- ================================================
alter procedure [dbo].[PrepareCommonNationalExamCertificateCheck] 
  @batchId bigint
as
begin

  declare
    @chooseDbText nvarchar(max)
    , @baseName nvarchar(255
    )
    , @declareCommandText nvarchar(max)
    , @executeCommandText nvarchar(max)
    , @searchCommandText nvarchar(max)
    , @searchCommandText2 nvarchar(max)
  
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1)

  set @declareCommandText = 
    N' 
    if (select count(*) from #CommonNationalExamCertificateCheck)=0
    begin
     raiserror(''CommonNationalExamCertificateCheck is empty'',16,1)
     return
    end
 
    declare 
      @yearFrom int
      , @yearTo int  

    declare @certificate_subject_correctness table
      (
      pk int identity(1,1) primary key,
      CertificateId uniqueidentifier
      , CertificateSubjectId int
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectId uniqueidentifier
      , BatchId bigint
      , CertificateCheckingId uniqueidentifier
      , Year int
      )
    
    declare @certificate_correctness table
      (
      pk int identity(1,1) primary key,
      CertificateNumber nvarchar(255)
      , CertificateId uniqueidentifier
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
      , [Index] bigint
      , TypographicNumber nvarchar(255)
      , RegionId int
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
      '
    
  set @searchCommandText = 
    N'
    select  
      @yearFrom = actuality_years.YearFrom
      , @yearTo = actuality_years.YearTo
    from
      dbo.GetCommonNationalExamCertificateActuality() actuality_years
    
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
      , [Index]
      , TypographicNumber
      , RegionId
      , PassportSeria
      , PassportNumber
      )
    select distinct 
      exam_certificate_check.CertificateNumber
      , b.CertificateID
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.Surname
      , exam_certificate.Name
      , exam_certificate.SecondName 
      , case 
        when exam_certificate.Surname collate cyrillic_general_ci_ai = exam_certificate_check.LastName collate cyrillic_general_ci_ai
			and case when exam_certificate_check.FirstName is null then 1 
				     when exam_certificate.Name collate cyrillic_general_ci_ai = exam_certificate_check.FirstName collate cyrillic_general_ci_ai then 1
				     else 0
				end = 1
			and case when exam_certificate_check.PatronymicName is null then 1
			 	     when exam_certificate.SecondName collate cyrillic_general_ci_ai = exam_certificate_check.PatronymicName collate cyrillic_general_ci_ai then 1
				     else 0
			    end = 1
			and exam_certificate_deny.UseYear is null
			then 1
			else 0
		end 
      , <BatchIdentifier>
      , case
        when not exam_certificate_deny.UseYear is null then 1
        else 0
      end
      , exam_certificate_deny.Reason
      , null NewCertificateNumber
      , exam_certificate.[UseYear]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.[Index]
      , b.TypographicNumber
      , b.Region
      , exam_certificate.DocumentSeries
      , exam_certificate.DocumentNumber
    from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
		left join (<dataDbName>.rbd.Participants AS exam_certificate  with(nolock) 			
					JOIN <dataDbName>.prn.Certificates AS b with(nolock) 
						ON b.ParticipantFK = exam_certificate.ParticipantID 
							and b.[useYear]=exam_certificate.UseYear )
			on exam_certificate.Surname collate cyrillic_general_ci_ai = exam_certificate_check.LastName
	            and 1=case when exam_certificate_check.FirstName is null then 1 
						   when exam_certificate.Name collate cyrillic_general_ci_ai = exam_certificate_check.FirstName then 1
					  else 0 end
				and 1= case when exam_certificate_check.PatronymicName is null then 1 
						 when exam_certificate.SecondName collate cyrillic_general_ci_ai = exam_certificate_check.PatronymicName then 1
					else 0 end
				and exam_certificate.[useYear] between @yearFrom and @yearTo 														
				and b.LicenseNumber collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
        left join <dataDbName>.prn.CancelledCertificates exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateFK = b.CertificateID
            and exam_certificate_deny.[UseYear]=exam_certificate.UseYear
    where 
      exam_certificate_check.BatchId = <BatchIdentifier>      
    '

    set @searchCommandText2 = '
    
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
      , Year
      )
    select 
      coalesce(certificate_correctness.[CertificateId], check_certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateFK])
      , isnull(exam_certificate_subject.SubjectCode, subject.SubjectId)
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
      , coalesce(certificate_correctness.[CertificateCheckingId], check_certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
      , coalesce(exam_certificate_subject.useyear, check_certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[UseYear]
        full outer join #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join <dataDbName>.dbo.Subject subject with(nolock)
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode 
            inner join @certificate_correctness check_certificate_correctness
              on check_certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
          on exam_certificate_subject.SubjectCode = subject.SubjectId
            and certificate_correctness.CertificateCheckingId = check_certificate_correctness.CertificateCheckingId

    '

  
  set @executeCommandText = 
    N'
    delete exam_certificate_check
    from dbo.CommonNationalExamCertificateCheck exam_certificate_check
    where exam_certificate_check.BatchId = <BatchIdentifier>

    declare @certificateCheckId table
      (
      id int identity(1,1) primary key,
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
      , SourceCertificateSubjectIdGuid
      , SourceMark
      , SourceHasAppeal
      , Year
      ) 
    select 
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
  
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
        
    '

  set @searchCommandText = replace(replace(@searchCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
  set @searchCommandText2 = replace(replace(@searchCommandText2, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)     
  set @executeCommandText = replace(replace(@executeCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)

--print '---begin: PCNECC---\n'
--print @chooseDbText
--print @declareCommandText
--print @searchCommandText
--print @searchCommandText2
--print @executeCommandText
--print '\n---end: PCNECC---'

  declare @commandText nvarchar(max)

  set @commandText=@chooseDbText +@declareCommandText + @searchCommandText + @searchCommandText2 + @executeCommandText
  
  exec sp_executesql @commandText

  return 0
end
