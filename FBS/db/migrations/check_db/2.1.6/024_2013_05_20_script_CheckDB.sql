insert into Migrations(MigrationVersion, MigrationName) values (24, '024_2013_05_20_script_CheckDB.sql')
go

if not exists(select * from sys.columns where name='SourceCertificateIdGuid' and object_name(object_id)='CommonNationalExamCertificateCheck')
alter table CommonNationalExamCertificateCheck add SourceCertificateIdGuid uniqueidentifier
go
if not exists(select * from sys.columns where name='SourceCertificateSubjectIdGuid' and object_name(object_id)='CommonNationalExamCertificateSubjectCheck')
alter table CommonNationalExamCertificateSubjectCheck add SourceCertificateSubjectIdGuid uniqueidentifier
go



/*
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
    select @yearFrom,@yearTo
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
    select 
      exam_certificate_check.CertificateNumber
      , exam_certificate.Id
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName 
      , case 
        when exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_check.LastName collate cyrillic_general_ci_ai
          and exam_certificate.FirstName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.FirstName, exam_certificate.FirstName) collate cyrillic_general_ci_ai
          and exam_certificate.PatronymicName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.PatronymicName, exam_certificate.PatronymicName) collate cyrillic_general_ci_ai
          and exam_certificate_deny.Id is null
          then 1
        else 0
      end 
      , <BatchIdentifier>
      , case
        when not exam_certificate_deny.Id is null then 1
        else 0
      end
      , exam_certificate_deny.Comment
      , exam_certificate_deny.NewCertificateNumber
      , exam_certificate.[Year]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.[Index]
      , exam_certificate.TypographicNumber
      , exam_certificate.RegionId
      , exam_certificate.PassportSeria
      , exam_certificate.PassportNumber
    from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
        left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
          on  exam_certificate.Number collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
            and exam_certificate.[Year] between @yearFrom and @yearTo
            and exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_check.LastName
            and exam_certificate.FirstName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.FirstName, exam_certificate.FirstName)
            and exam_certificate.PatronymicName collate cyrillic_general_ci_ai = isnull(exam_certificate_check.PatronymicName, exam_certificate.PatronymicName)
        left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
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
      , coalesce(certificate_correctness.[CertificateCheckingId], check_certificate_correctness.[CertificateCheckingId], exam_certificate_subject_check.[CertificateCheckingId])
      , coalesce(exam_certificate_subject.useyear, check_certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[UseYear]
        full outer join #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode 
            inner join @certificate_correctness check_certificate_correctness
              on check_certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
          on exam_certificate_subject.SubjectCode = subject.Id
            and certificate_correctness.CertificateCheckingId = check_certificate_correctness.CertificateCheckingId

    '

  
  set @executeCommandText = 
    N'
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
go


alter PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFIO]
    @batchId BIGINT=8774
AS 
    BEGIN
set nocount on
        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX)           
     declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
  
     
   set @declareCommandText='
   update a set SubjectId=b.Id
  from 
  #CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code
     --select * from #CommonNationalExamCertificateSubjectCheck
   declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
       --PrepareCommonNationalExamCertificateCheckByFIO    
       delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)
       select
       <@BatchId> BatchId,a.CertificateNumber Name,a.PassportNumber [Sum],      
        case when not exists(
        select * from 
        vw_Examcertificate ta with(nolock) 
        left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
        where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
      when tt.CertificateCheckingId is null then 0 else 1 end [Status],
      case when (select COUNT(distinct PassportNumber) from vw_Examcertificate  with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake   
       from
       #CommonNationalExamCertificateCheck a        
       left join        
       (
       select * 
       from     
       (select distinct a1.CertificateCheckingId,a1.CertificateNumber fio,a1.PassportNumber sum, b1.PassportNumber from 
       #CommonNationalExamCertificateCheck a1 join 
       vw_Examcertificate b1 with(nolock) on  b1.fio=a1.CertificateNumber
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b1.Number
       where b1.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
       ) tbl
       where  
       
       not exists(
       select * from
       (
       select SubjectCode,Mark from #CommonNationalExamCertificateSubjectCheck a 
       where a.CertificateCheckingId=tbl.CertificateCheckingId
       ) t1
       left join 
       (
       select
        distinct a.CertificateCheckingId,a.PassportNumber,a.Code,a.Mark       
        from
       (
       select a.CertificateCheckingId,b.PassportNumber,d.Code,c.mark
       from 
       #CommonNationalExamCertificateCheck a join 
       vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
       join prn.CertificatesMarks c with(nolock) on b.Id=c.CertificateFK
       join Subject d on d.id=c.SubjectCode
       where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join(
        select a.CertificateCheckingId,a.PassportNumber,c.Code,b.mark 
       from
       (select distinct a.CertificateCheckingId,a.CertificateNumber,b.PassportNumber from 
       #CommonNationalExamCertificateCheck a join 
       vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
              where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join #CommonNationalExamCertificateSubjectCheck b on b.CertificateCheckingId=a.CertificateCheckingId
       join Subject c on c.Code=b.SubjectCode

       ) b on a.PassportNumber=b.PassportNumber and a.Mark=b.mark and a.Code=b.Code
       where  a.PassportNumber=tbl.PassportNumber
       ) t2       
       on t1.SubjectCode=t2.code and t1.Mark=t2.Mark
       where t2.code is null
       )
       )       tt  on a.CertificateCheckingId=tt.CertificateCheckingId and a.CertificateNumber=tt.fio
    declare @id bigint,@PassportNumber  nvarchar(255) ,@sum numeric(12,2),@CertificateCheckingId  uniqueidentifier
    --select * from #CommonNationalExamCertificateCheck 
    
    select * into #CommonNationalExamCertificateSumCheck from  CommonNationalExamCertificateSumCheck a where a.BatchId=<@BatchId> and status=0
    
  --  select distinct a.id ,b.PassportNumber,a.sum,cc.CertificateCheckingId
    --from 
    --#CommonNationalExamCertificateSumCheck a 
    --join vw_Examcertificate b with(nolock) on  b.fio=a.name   
    --left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
    --join #CommonNationalExamCertificateCheck cc on cc.CertificateNumber=a.name and cast(cc.PassportSeria as numeric(12,2))=a.sum
    --where  de.CertificateNumber is null
    
  declare cc cursor local fast_forward for 
    select distinct a.id ,b.PassportNumber,a.sum,cc.CertificateCheckingId
    from 
    #CommonNationalExamCertificateSumCheck a 
    join vw_Examcertificate b with(nolock) on  b.fio=a.name   
    left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
    join #CommonNationalExamCertificateCheck cc on cc.CertificateNumber=a.name and cast(cc.PassportSeria as numeric(12,2))=a.sum
    where de.CertificateNumber is null
  open cc
  create table #t (SubjectId int,mark numeric(5,1))   
  declare @maxsum numeric(12,2)
  declare @t table(id bigint primary key)
  create index sdcjhsdklcj_sadfvsd_asdvd on #t(SubjectId)
  create table #tt (c varchar(4000) not null, s numeric(12,2) not null, level int not null, id int identity, primary key clustered(level, id))
  fetch next from cc into @id,@PassportNumber,@sum,@CertificateCheckingId
  while @@FETCH_STATUS=0
  begin 
    --select @id,@PassportNumber,@sum,@CertificateCheckingId
    -- print ''    10.''+right(convert(varchar,SYSDATETIME()),12)       
    
    
    truncate table #t
    insert #t
    select
    c.SubjectCode,c.mark  
    from 
    dbo.vw_Examcertificate b with(nolock)
    join prn.CertificatesMarks c with(nolock) on b.Id=c.CertificateFK   
    join #CommonNationalExamCertificateSubjectCheck d on  c.SubjectCode=d.SubjectId and d.CertificateCheckingId=@CertificateCheckingId
    where b.[YEAR] between  @yearFrom and @yearTo and b.PassportNumber=@PassportNumber 
    --print ''    20.''+right(convert(varchar,SYSDATETIME()),12)
    
    --select * from #t
  if not exists(select * from 
    #CommonNationalExamCertificateSubjectCheck b      
    left join #t c on c.SubjectId=b.SubjectId
    where b.CertificateCheckingId=@CertificateCheckingId and c.SubjectId is null )
  begin
    --select @id,@PassportNumber,@sum,@CertificateCheckingId
    truncate table #tt
    declare @i int, @l int
    select @i = min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where CertificateCheckingId=@CertificateCheckingId
    set @l = 0
    insert #tt(c, s, level) values('''', 0, 0)      
    while @i is not null 
    begin
      insert #tt(c, s, level) 
      select
      --'''',
       t.c+case t.c when '''' then '''' else ''+'' end+cast(x.mark as varchar(16)),
       t.s+x.mark, @l+1
      from (select * from #tt where level = @l) t
        cross join
        (select * from #t where SubjectId = @i) x
  
      set @i = (select min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where SubjectId>@i and CertificateCheckingId=@CertificateCheckingId)
      set @l = @l+1
    end
    --print ''    30.''+right(convert(varchar,SYSDATETIME()),12)  
    --select * from #tt where level = @l
    
     select @maxsum=max(s) from #tt where level = @l  
     if not exists(select * from @t where id=@id)
     begin
      insert @t values(@id)         
      update CommonNationalExamCertificateSumCheck set sum=@maxsum 
      where id=@id 
     end  
     else
     begin
     update CommonNationalExamCertificateSumCheck set sum=@maxsum 
      where id=@id and sum<@maxsum  
     end
  end
  else
  begin 
    if not exists(select * from @t where id=@id)
      update CommonNationalExamCertificateSumCheck set sum=-1 where id=@id
  end
       
    fetch next from cc into @id,@PassportNumber,@sum,@CertificateCheckingId
  end
close cc
deallocate cc 
drop table #CommonNationalExamCertificateSumCheck 
drop table #t
drop table #tt
       select * from CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       '
       
     --PrepareCommonNationalExamCertificateCheckByFIO    



        SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)
--set @chooseDbText=@chooseDbText+

--'
--drop table #CommonNationalExamCertificateCheck
--drop table #CommonNationalExamCertificateSubjectCheck
--select * into #CommonNationalExamCertificateCheck   from #CommonNationalExamCertificateCheck
--select * into #CommonNationalExamCertificateSubjectCheck   from #CommonNationalExamCertificateSubjectCheck
--'
set @declareCommandText=@chooseDbText+replace(@declareCommandText,'<@BatchId>',@BatchId)
EXEC sp_executesql @declareCommandText
return 

end
go


alter PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFioAndSubjects]
    @batchId BIGINT=8774
AS 
    BEGIN
set nocount on
        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX)           
     declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
  
     
   set @declareCommandText='
   drop table      tmp_CommonNationalExamCertificateCheck
   select * into tmp_CommonNationalExamCertificateCheck from #CommonNationalExamCertificateCheck
   
   drop table      tmp_CommonNationalExamCertificateSubjectCheck
   select * into tmp_CommonNationalExamCertificateSubjectCheck from #CommonNationalExamCertificateSubjectCheck
   return 
   update a set SubjectId=b.Id
  from 
  #CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code
     --select * from #CommonNationalExamCertificateSubjectCheck
   declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
       --PrepareCommonNationalExamCertificateCheckByFIO    
       delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)
       select
       <@BatchId> BatchId,a.CertificateNumber Name,a.PassportNumber [Sum],      
        case when not exists(
        select * from 
        dbo.vw_Examcertificate ta with(nolock) 
        left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
        where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
      when tt.CertificateCheckingId is null then 0 else 1 end [Status],
      case when (select COUNT(distinct PassportNumber) from dbo.vw_Examcertificate with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake    
       from
       #CommonNationalExamCertificateCheck a        
       left join        
       (
       select * 
       from     
       (select distinct a1.CertificateCheckingId,a1.CertificateNumber fio,a1.PassportNumber sum, b1.PassportNumber from 
       #CommonNationalExamCertificateCheck a1 join 
       dbo.vw_Examcertificate b1 with(nolock) on  b1.fio=a1.CertificateNumber
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b1.Number
       where b1.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
       ) tbl
       where  
       
       not exists(
       select * from
       (
       select SubjectCode,Mark from #CommonNationalExamCertificateSubjectCheck a 
       where a.CertificateCheckingId=tbl.CertificateCheckingId
       ) t1
       left join 
       (
       select
        distinct a.CertificateCheckingId,a.PassportNumber,a.Code,a.Mark       
        from
       (
       select a.CertificateCheckingId,b.PassportNumber,d.Code,c.mark
       from 
       #CommonNationalExamCertificateCheck a join 
       dbo.vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
       join [prn].[CertificatesMarks] c with(nolock) on b.Id=c.CertificateFK
       join Subject d on d.id=c.SubjectCode
       where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join(
        select a.CertificateCheckingId,a.PassportNumber,c.Code,b.mark 
       from
       (select distinct a.CertificateCheckingId,a.CertificateNumber,b.PassportNumber from 
       #CommonNationalExamCertificateCheck a join 
       dbo.vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber
              where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join #CommonNationalExamCertificateSubjectCheck b on b.CertificateCheckingId=a.CertificateCheckingId
       join Subject c on c.Code=b.SubjectCode

       ) b on a.PassportNumber=b.PassportNumber and a.Mark=b.mark and a.Code=b.Code
       where  a.PassportNumber=tbl.PassportNumber
       ) t2       
       on t1.SubjectCode=t2.code and t1.Mark=t2.Mark
       where t2.code is null
       )
       )       tt  on a.CertificateCheckingId=tt.CertificateCheckingId and a.CertificateNumber=tt.fio
    
       select * from CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       '
       
     --PrepareCommonNationalExamCertificateCheckByFIO    



        SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)
--set @chooseDbText=@chooseDbText+

--'
--drop table #CommonNationalExamCertificateCheck
--drop table #CommonNationalExamCertificateSubjectCheck
--select * into #CommonNationalExamCertificateCheck   from #CommonNationalExamCertificateCheck
--select * into #CommonNationalExamCertificateSubjectCheck   from #CommonNationalExamCertificateSubjectCheck
--'
set @declareCommandText=@chooseDbText+replace(@declareCommandText,'<@BatchId>',@BatchId)
EXEC sp_executesql @declareCommandText
--print @declareCommandText
return 

end
go


alter PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum]
    @batchId BIGINT=8774
AS 
    BEGIN
set nocount on
        DECLARE @chooseDbText NVARCHAR(MAX),
            @baseName NVARCHAR(255),
            @declareCommandText NVARCHAR(MAX)           
     
  
  --goto ttt   
   set @declareCommandText='
   
   declare @yearFrom int, @yearTo int
   select @yearFrom = 2010, @yearTo = 2012
  -- update CommonNationalExamCertificateCheckBatch set Executing=1 where id=<@BatchId>
         --[PrepareCommonNationalExamCertificateCheckByFIOBySum    
update a set SubjectId=b.Id
from 
#CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code

declare @CertificateCheckingId  uniqueidentifier,@PassportNumber  nvarchar(255) ,@sum numeric(12,2)     
 declare cc cursor local fast_forward for 
       select 
       distinct
       --top 1
        a.CertificateCheckingId,b.PassportNumber,cast(a.PassportSeria as numeric(12,2))
       from 
       #CommonNationalExamCertificateCheck a join 
       vw_Examcertificate b with(nolock) on  b.fio=a.CertificateNumber    
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
       where b.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
  open cc
  create table #t (SubjectId int,mark numeric(5,1))   
  declare @maxsum numeric(12,2)
  create index sdcjhsdklcj_sadfvsd_asdvd on #t(SubjectId)
  create table #tt (c varchar(4000) not null, s numeric(12,2) not null, level int not null, id int identity, primary key clustered(level, id))
  fetch next from cc into @CertificateCheckingId,@PassportNumber,@sum
  while @@FETCH_STATUS=0
  begin   
    -- print ''    10.''+right(convert(varchar,SYSDATETIME()),12)
    if(select LastName from #CommonNationalExamCertificateCheck where CertificateCheckingId=@CertificateCheckingId)=''1''
      goto endcur
    if exists(select * from #CommonNationalExamCertificateSubjectCheck where SubjectCode is null and CertificateCheckingId=@CertificateCheckingId)  
      goto endcur
    truncate table #t
    insert #t
    select
    c.SubjectCode,c.mark  
    from 
    dbo.vw_Examcertificate b with(nolock)
    join prn.CertificatesMarks c with(nolock) on b.Id=c.CertificateFK   
    join #CommonNationalExamCertificateSubjectCheck d on  c.SubjectCode=d.SubjectId and d.CertificateCheckingId=@CertificateCheckingId
    where b.[YEAR] between  @yearFrom and @yearTo and b.PassportNumber=@PassportNumber 
    --print ''    20.''+right(convert(varchar,SYSDATETIME()),12)
  if not exists(select * from 
    #CommonNationalExamCertificateSubjectCheck b      
    left join #t c on c.SubjectId=b.SubjectId
    where b.CertificateCheckingId=@CertificateCheckingId and c.SubjectId is null )
  begin
    
    truncate table #tt
    declare @i int, @l int
    select @i = min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where CertificateCheckingId=@CertificateCheckingId
    set @l = 0
    insert #tt(c, s, level) values('''', 0, 0)
    --print ''    30.''+right(convert(varchar,SYSDATETIME()),12)    
    while @i is not null 
    begin
      insert #tt(c, s, level) 
      select
      --'''',
       t.c+case t.c when '''' then '''' else ''+'' end+cast(x.mark as varchar(16)),
       t.s+x.mark, @l+1
      from (select * from #tt where level = @l) t
        cross join
        (select * from #t where SubjectId = @i) x
  
      set @i = (select min(SubjectId) from #CommonNationalExamCertificateSubjectCheck where SubjectId>@i and CertificateCheckingId=@CertificateCheckingId)
      set @l = @l+1
    end

    
    
    if exists(select * from #tt where level = @l and s=@sum)
    begin    
     update #CommonNationalExamCertificateCheck set lastname=''1'',FirstName=cast(@sum as varchar(255)) where CertificateCheckingId=@CertificateCheckingId    
    end
    else
    begin
  --  select c, s from #tt where level = @l
     select @maxsum=max(s) from #tt where level = @l  
     update #CommonNationalExamCertificateCheck set FirstName=cast(@maxsum as varchar(255)) 
     where CertificateCheckingId=@CertificateCheckingId and cast(isnull(FirstName,0) as numeric(12,2))<@maxsum
     end
  end
    
    
 endcur:
 --print SYSDATETIME()
         fetch next from cc into @CertificateCheckingId,@PassportNumber,@sum
   end   
    drop table #t
    drop table #tt
   --select * from #CommonNationalExamCertificateCheck  
  delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
  print SYSDATETIME()
  insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)  
  select <@BatchId>,CertificateNumber,isnull(cast(FirstName as numeric(12,1)),-1),
  case when not exists(
        select * from 
        vw_Examcertificate ta with(nolock) 
        left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
        where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
      when lastname is null then 0 else 1 end [Status],
      case when (select COUNT(distinct PassportNumber) from vw_Examcertificate with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake
      from #CommonNationalExamCertificateCheck a
      print SYSDATETIME()
      --update CommonNationalExamCertificateCheckBatch set Executing=0,IsProcess=1 where id=<@BatchId>
        select * from CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>     
        -- select * from CommonNationalExamCertificateCheckBatch where id=<@BatchId>  
  '
 -- print @declareCommandText
  
  SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
        SET @baseName = dbo.GetDataDbName(1, 1)
        set @declareCommandText=@chooseDbText+replace(@declareCommandText,'<@BatchId>',@BatchId)
       -- print @declareCommandText
EXEC sp_executesql @declareCommandText
  return 

       
     --[PrepareCommonNationalExamCertificateCheckByFIOBySum]    

--return
--ttt:
--        SET @chooseDbText = REPLACE('use <database>', '<database>', dbo.GetCheckDataDbName())
--        SET @baseName = dbo.GetDataDbName(1, 1)
--set @chooseDbText=@chooseDbText+
--'
--drop table #CommonNationalExamCertificateCheck
--drop table #CommonNationalExamCertificateSubjectCheck
--select * into #CommonNationalExamCertificateCheck   from #CommonNationalExamCertificateCheck
--select * into #CommonNationalExamCertificateSubjectCheck   from #CommonNationalExamCertificateSubjectCheck
--'
----set @declareCommandText=@chooseDbText+replace(@declareCommandText,'@batchId',@BatchId)
--EXEC sp_executesql @chooseDbText
--return 

end
go
/*
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
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , [Index] bigint
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
-- ================================================
alter PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByNumber]
@batchId bigint,@type int=1
as
begin
  declare
    @chooseDbText nvarchar(max)
    , @baseName nvarchar(255
    )
    , @declareCommandText nvarchar(max)
    , @yearCommandText nvarchar(max)
    , @executeCommandText nvarchar(max)
    , @searchCommandText nvarchar(max)
    , @searchCommandText2 nvarchar(max)
    , @addCondition nvarchar(max)
    , @selectPass nvarchar(500)
  if @type=1
  begin
  
   set @yearCommandText=N'
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
   set @addCondition='exam_certificate.Number collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai'
   set @selectPass=', exam_certificate.PassportSeria
      , exam_certificate.PassportNumber'
  end
  if @type=2
  begin
  update #CommonNationalExamCertificateCheck set PassportSeria=replace(PassportSeria, ' ', '')
   set @yearCommandText=N'
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
   set @addCondition='exam_certificate.InternalPassportSeria collate cyrillic_general_ci_ai = exam_certificate_check.PassportSeria collate cyrillic_general_ci_ai
   and exam_certificate.PassportNumber collate cyrillic_general_ci_ai = exam_certificate_check.PassportNumber collate cyrillic_general_ci_ai
   '
   set @selectPass=', exam_certificate_check.PassportSeria
      , exam_certificate_check.PassportNumber'
  end
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  set @baseName = dbo.GetDataDbName(1, 1)

  set @declareCommandText = 
    N'
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
      , SubjectCode nvarchar(255)
      )
    
    declare @certificate_correctness table
      (
        id int not null identity(1,1) primary key,
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
    select @yearFrom,@yearTo      
    
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
    select 
      isnull(exam_certificate_check.CertificateNumber,exam_certificate.Number)
      , exam_certificate.Id
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName 
      , case when exam_certificate.Number is not null and exam_certificate_deny.Id is null then 1 else 0  end 
      , <BatchIdentifier>
      , case
        when not exam_certificate_deny.Id is null then 1
        else 0
      end
      , exam_certificate_deny.Comment
      , exam_certificate_deny.NewCertificateNumber
      , exam_certificate.[Year]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.[Index]
      , exam_certificate.TypographicNumber
      , exam_certificate.RegionId
      <@selectPass>
    from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
        left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
          on  
          <addCondition>
            and exam_certificate.[Year] between @yearFrom and @yearTo           
        left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
    where
      exam_certificate_check.BatchId = <BatchIdentifier>
      
    select @yearFrom ,@yearTo,
      isnull(exam_certificate_check.CertificateNumber,exam_certificate.Number)
      , exam_certificate.Id
      , exam_certificate_check.LastName
      , exam_certificate_check.FirstName
      , exam_certificate_check.PatronymicName 
      , exam_certificate.LastName
      , exam_certificate.FirstName
      , exam_certificate.PatronymicName 
      , case when exam_certificate.Number is not null and exam_certificate_deny.Id is null then 1 else 0  end 
      , <BatchIdentifier>
      , case
        when not exam_certificate_deny.Id is null then 1
        else 0
      end
      , exam_certificate_deny.Comment
      , exam_certificate_deny.NewCertificateNumber
      , exam_certificate.[Year]
      , exam_certificate_check.CertificateCheckingId
      , exam_certificate_check.[Index]
      , exam_certificate.TypographicNumber
      , exam_certificate.RegionId
      <@selectPass>
    from
      #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)
        left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
          on  
          <addCondition>
            and exam_certificate.[Year] between @yearFrom and @yearTo           
        left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate_check.CertificateNumber collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
    where
      exam_certificate_check.BatchId = <BatchIdentifier>      
      
    '
set @searchCommandText=replace(@searchCommandText,'<@selectPass>',@selectPass)    
set @searchCommandText=
@yearCommandText+
REPLACE(@searchCommandText,'<addCondition>',@addCondition)
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
      ,SubjectCode
      )
    select 
      coalesce(certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateFK])
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
      , coalesce(exam_certificate_subject.useyear, certificate_correctness.CertificateYear, certificate_correctness.[CertificateYear])
      ,subject.Code
    from 
      <dataDbName>.prn.CertificatesMarks exam_certificate_subject with(nolock)
        inner join @certificate_correctness certificate_correctness
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[useyear]
        full outer join 
        ( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject with(nolock)
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai            
            )   on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
            and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId  
--select * from @certificate_subject_correctness order by 1
--select * from @certificate_correctness
declare @idcert uniqueidentifier
declare cur cursor for select CertificateCheckingId from @certificate_correctness
open cur
fetch next from cur into @idcert
while @@fetch_status=0
begin
--select @idcert
--select * from @certificate_subject_correctness where CertificateCheckingId=@idcert  and mark is not null and (SourceCertificateSubjectId is null or IsSubjectCorrect=0)
 if exists(select * from @certificate_subject_correctness where CertificateCheckingId=@idcert  and mark is not null and (SourceCertificateSubjectId is null or IsSubjectCorrect=0))
 begin
 --print -999
 if <@type>=1
  update @certificate_correctness 
  set CertificateId=null,SourceLastName=null,PassportNumber=null,PassportSeria=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where CertificateCheckingId=@idcert 
 
  update @certificate_subject_correctness 
  set IsSubjectCorrect=null,SourceMark =null, SourceCertificateSubjectId=null,year=null,SubjectCode=null,HasAppeal=null
  where CertificateCheckingId=@idcert 
 end
 fetch next from cur into @idcert
end
close cur deallocate cur

--select * from @certificate_correctness
--select * from @certificate_subject_correctness order by CertificateId,CertificateSubjectId
--select * from #CommonNationalExamCertificateSubjectCheck 

    '

  
  set @executeCommandText = 
    N'
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
          
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
        
       select * from CommonNationalExamCertificateSubjectCheck where BatchId=<BatchIdentifier>    order by checkid,subjectid 
       --drop table rrr
       --   select * into rrr from #CommonNationalExamCertificateCheck
       --   drop table ttt
       --   select * into ttt from #CommonNationalExamCertificateSubjectCheck
    '

  set @searchCommandText = replace(replace(@searchCommandText, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName)
  set @searchCommandText2 = replace(replace(replace(@searchCommandText2, '<BatchIdentifier>', Convert(nvarchar(255), @batchId)), '<dataDbName>', @baseName),'<@type>',CAST(@type as varchar(10)))
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
      
      CertificateId uniqueidentifier
      , CertificateSubjectId bigint
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectId uniqueidentifier
      , BatchId bigint
      , CertificateCheckingId uniqueidentifier
      , ShareCertificateCheckingId uniqueidentifier
      , Year int
      , SubjectCode nvarchar(255)
      )
      declare @certificate_subject_correctness_result table
      (
      
      CertificateId uniqueidentifier
      , CertificateSubjectId bigint
      , Mark numeric(5,1)
      , HasAppeal bit
      , IsSubjectCorrect bit
      , SourceMark numeric(5,1)
      , SourceCertificateSubjectId uniqueidentifier
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
      , ShareCertificateCheckingId uniqueidentifier
      , [Index] bigint
      , TypographicNumber nvarchar(255)
      , RegionId int
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
      '
        SET @searchCommandText = N' 
      
    select  exam_certificate.id as ID1,exam_certificate_check.id as ID2 into #tt
    from #CommonNationalExamCertificateCheck exam_certificate_check with(nolock)      
      left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock) 
      on  <addCondition>
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
      , case when exam_certificate.Number is not null and exam_certificate_deny.Id is null then 1 else 0  end 
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
      left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)  on a.ID1=exam_certificate.id     
        left join <dataDbName>.dbo.CommonNationalExamCertificateDeny exam_certificate_deny with(nolock)
          on exam_certificate_deny.CertificateNumber collate cyrillic_general_ci_ai = exam_certificate.Number collate cyrillic_general_ci_ai
            and exam_certificate_deny.[Year] between @yearFrom and @yearTo
        
      
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
      coalesce(certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateFK])
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
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[UseYear]
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
      coalesce(certificate_correctness.[CertificateId], exam_certificate_subject.[CertificateFK])
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
          on certificate_correctness.CertificateId = exam_certificate_subject.CertificateFK
            and certificate_correctness.CertificateYear = exam_certificate_subject.[useyear]
        full outer join 
        ( #CommonNationalExamCertificateSubjectCheck exam_certificate_subject_check with(nolock)
          inner join dbo.Subject subject with(nolock)
            on subject.Code collate cyrillic_general_ci_ai = exam_certificate_subject_check.SubjectCode collate cyrillic_general_ci_ai            
            )   
            on exam_certificate_subject.SubjectCode = subject.Id and certificate_correctness.CertificateCheckingId = exam_certificate_subject_check.CertificateCheckingId
              
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
--Сделал так чтобы если есть нет не одного совпадения то занулять проверку. Разделил на два блока так как не знаб логику работы первого блока
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
       
    -- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
        
       --select * from CommonNationalExamCertificateSubjectCheck where BatchId=<BatchIdentifier>    order by checkid,subjectid 
       --drop table rrr
       --   select * into rrr from #CommonNationalExamCertificateCheck
       --   drop table ttt
       --   select * into ttt from #CommonNationalExamCertificateSubjectCheck
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
    , SourceCertificateIdGuid
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
      , null/*isnull(exam_certificate_request.PassportSeria, exam_certificate.PassportSeria)*/
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
      left join <dataDbName>.dbo.vw_Examcertificate exam_certificate with(nolock)
        on  exam_certificate.LastName collate cyrillic_general_ci_ai = exam_certificate_request.LastName
          and 1= case when exam_certificate_request.FirstName is null then 1
            when exam_certificate.FirstName collate cyrillic_general_ci_ai = exam_certificate_request.FirstName then 1
            else 0
          end
          and 1 = case when exam_certificate_request.PatronymicName is null then 1
             when exam_certificate.PatronymicName collate cyrillic_general_ci_ai = exam_certificate_request.PatronymicName  then 1
             else 0
          end
          and 1 = case when exam_certificate_request.PassportSeria is null then 1 
             when exam_certificate.PassportSeria collate cyrillic_general_ci_ai = exam_certificate_request.PassportSeria then 1
             else 0
          end         
          and 1 = case when exam_certificate_request.PassportNumber is null then 1
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
  
--  print @chooseDbText
--  print @declareCommandText
--  print @executeCommandText

  declare @CommonText nvarchar(max)
  set @CommonText=@chooseDbText + @IndexText + @declareCommandText + @executeCommandText
  print  @CommonText
  exec sp_executesql @CommonText

  return 0
end
go


alter PROCEDURE [dbo].[usp_cne_StartCheckBatch]
@batchId bigint
as
set nocount on
  declare
    @chooseDbText nvarchar(max), @command nvarchar(max)
  
  set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
  
create table #CommonNationalExamCertificateCheck
    ( 
    id int identity(1,1) not null primary key,
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , CertificateNumber nvarchar(255)
    , LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255)
    , [Index] bigint
    , ShareCertificateCheckingId uniqueidentifier
    )
CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateCheck_1] ON [dbo].[#CommonNationalExamCertificateCheck] 
(
  [CertificateNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateCheck_2] ON [dbo].[#CommonNationalExamCertificateCheck] 
(
  [BatchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateCheck_3] ON [dbo].[#CommonNationalExamCertificateCheck] 
(
  [PassportSeria] ASC 
)
include ([PassportNumber])
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
      
  create table #CommonNationalExamCertificateSubjectCheck
    (
    CertificateCheckingId uniqueidentifier
    , BatchId bigint
    , SubjectCode nvarchar(255)
    , Mark numeric(5,1)
    , ShareCertificateCheckingId uniqueidentifier
    , SubjectId int
    )
CREATE NONCLUSTERED INDEX [IX_#CommonNationalExamCertificateSubjectCheck_1SubjectId] ON [dbo].[#CommonNationalExamCertificateSubjectCheck]
(
  [SubjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]    
declare @typeStr varchar(200),@subjectStartStr varchar(200),@setStr varchar(200)



set @command='
declare cur cursor local fast_forward for 
  select id,Batch from dbo.CommonNationalExamCertificateCheckBatch 
  where <typeStr> and id=@batchId
open cur
declare @id bigint,@batch nvarchar(max),@certificateCheckingId uniqueidentifier,@index int,@subjectNum int,@row nvarchar(4000),@subjectStart int
declare @batches table (val nvarchar(max))
declare @tab table (id int,val nvarchar(300))
select @subjectNum=<@subjectNum>,@subjectStart=<subjectStartStr>
fetch next from cur into @id,@batch
while @@FETCH_STATUS=0
begin     
  declare cc cursor local fast_forward for select val from [dbo].[ufn_ut_SplitFromStringWithId](REPLACE(REPLACE(@batch, CHAR(13) + CHAR(10), CHAR(13)), CHAR(10), CHAR(13)),CHAR(13)) order by id  
  open cc
  select @index=1
  fetch next from cc into @row
  while @@FETCH_STATUS=0
  begin     
    if len(isnull(@row,''''))=0
      goto endcur1    
    delete from @tab
    select @certificateCheckingId=NEWID()
    /*insert #CommonNationalExamCertificateCheck(ShareCertificateCheckingId,CertificateCheckingId,BatchId, [Index]) values(@certificateCheckingId,@certificateCheckingId,@id,@index)
    insert @tab( id,val)  select id,val from  [dbo].[ufn_ut_SplitFromStringWithId](@row,''%'') order by id
    --select * from @tab
  
  
    update #CommonNationalExamCertificateCheck 
    set 
    <setStr>
    from @tab
    where CertificateCheckingId=@certificateCheckingId
  */
      
   insert @tab( id,val)  select id,val from  [dbo].[ufn_ut_SplitFromStringWithId](@row,''%'') order by id
    
    declare @PassportSeria varchar(100)
    declare @PassportNumber varchar(100)
    
    declare @CertificateNumber varchar(100)
    declare @LastName varchar(100)
    declare @FirstName varchar(100)
    declare @PatronymicName varchar(100)
    
    select @PassportSeria = val from @tab where id=1
    set @CertificateNumber = @PassportSeria
    
    select @PassportNumber = val from @tab where id=2
    select @FirstName = val from @tab where id=3
    select @PatronymicName = val from @tab where id=4
  
    insert #CommonNationalExamCertificateCheck
      (ShareCertificateCheckingId,CertificateCheckingId,BatchId, [Index],
      CertificateNumber,
      LastName,
      FirstName,
      PatronymicName,
      PassportSeria,
      PassportNumber
      ) 
    
    values(@certificateCheckingId,@certificateCheckingId,@id,@index,
     @CertificateNumber,
       @PassportNumber,
       @FirstName,
       @PatronymicName,
       @PassportSeria,  
       @PassportNumber
    )
  
  
    insert #CommonNationalExamCertificateSubjectCheck(ShareCertificateCheckingId,CertificateCheckingId ,BatchId,SubjectCode, Mark)
    select @certificateCheckingId,@certificateCheckingId,@id,b.Code,cast(replace(val,'','',''.'') as numeric(5,1))
    from  @tab a 
    --left  
    join dbo.Subject b on a.id-@subjectStart+1=b.Id
    where a.id between @subjectStart and @subjectNum+@subjectStart-1 and val<>''''
    
    select @index=@index+1
    endcur1:
    fetch next from cc into @row
  end
  close cc
  deallocate cc
  ttt:
  fetch next from cur into @id,@batch
end
close cur
deallocate cur  
--select * from #CommonNationalExamCertificateCheck 
--select * from #CommonNationalExamCertificateSubjectCheck
--delete from #CommonNationalExamCertificateCheck 


'

----------------------------------------------------------------------------------------------    
select  @typeStr= 'ISNULL(type,0)=0'
    ,@subjectStartStr='5'
    ,@setStr='CertificateNumber=(select val from @tab where id=1),
    LastName=(select val from @tab where id=2),
    FirstName=(select val from @tab where id=3),
    PatronymicName=(select val from @tab where id=4)'
declare @commandStr nvarchar(max)       
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
      print '001000'
      
      update #CommonNationalExamCertificateCheck
      set 
        PassportSeria = null,
        PassportNumber = null
      
      exec [dbo].PrepareCommonNationalExamCertificateCheck @batchId
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck
    end
    
----------------------------------------------------------------------------------------------    
select  @typeStr= 'type=1'
    ,@subjectStartStr='2'
    ,@setStr='CertificateNumber=(select val from @tab where id=1)'    
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 11111
     
     update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria = null,
        PassportNumber = null
     
--     raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
      exec [dbo].PrepareCommonNationalExamCertificateCheckByNumber @batchId,1
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=2'
    ,@subjectStartStr='3'
    ,@setStr='PassportSeria=(select val from @tab where id=1),  
    PassportNumber=(select val from @tab where id=2)'   
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 22222
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        CertificateNumber = null
     
--     raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
      exec [dbo].PrepareCommonNationalExamCertificateCheckByPassport @batchId,2
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=3'
    ,@subjectStartStr='3'
    ,@setStr='PassportSeria=(select val from @tab where id=1),  
    PassportNumber=(select val from @tab where id=2)'   
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','15')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 3333
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria=PassportNumber
     
--select * from #CommonNationalExamCertificateCheck
--select  * from #CommonNationalExamCertificateSubjectCheck
      exec [dbo].[PrepareCommonNationalExamCertificateCheckByFIO] @batchId
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
--[usp_cne_StartCheckBatch] 8774
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=4'
    ,@subjectStartStr='3'
    ,@setStr='PassportSeria=(select val from @tab where id=1),  
    PassportNumber=(select val from @tab where id=2)'   
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','15')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 4444
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria=PassportNumber
     
--select * from #CommonNationalExamCertificateCheck
--select  * from #CommonNationalExamCertificateSubjectCheck
      exec [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum] @batchId
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end
    
----------------------------------------------------------------------------------------------        


select  @typeStr= 'type=5'
    ,@subjectStartStr='2'
    ,@setStr='PassportSeria=(select val from @tab where id=1)'    
set @commandStr=@chooseDbText +
replace(
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
,'<@subjectNum>','14')
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
    begin
     print 4444
     
      update #CommonNationalExamCertificateCheck
      set 
        LastName = null,
        FirstName = null,
        PatronymicName = null,
        PassportSeria=null,PassportNumber=null

--select * from #CommonNationalExamCertificateCheck
--select  * from #CommonNationalExamCertificateSubjectCheck
      exec [dbo].[PrepareCommonNationalExamCertificateCheckByFioAndSubjects] @batchId
      
      truncate table #CommonNationalExamCertificateCheck
      truncate table #CommonNationalExamCertificateSubjectCheck    
    end   
drop table #CommonNationalExamCertificateCheck
drop table #CommonNationalExamCertificateSubjectCheck
--[usp_cne_StartCheckBatch] 8774
 --select getdate() exec dbo.usp_cne_StartCheckBatch @batchId=8351
 go    