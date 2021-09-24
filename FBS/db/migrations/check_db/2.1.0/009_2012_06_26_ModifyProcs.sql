/****** Object:  Trigger [Migrations_tri]    Script Date: 06/26/2012 15:28:45 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Migrations_tri]'))
DROP TRIGGER [dbo].[Migrations_tri]
GO

SET CONCAT_NULL_YIELDS_NULL, --Управляет представлением результатов объединения в виде значений NULL или пустых строковых значений.
ANSI_NULLS, --Задает совместимое со стандартом ISO поведение операторов сравнения «равно» (=) и «не равно» (<>) при использовании со значениями NULL.
ANSI_PADDING, --Контролирует способ хранения в столбце значений короче, чем определенный размер столбца, и способ хранения в столбце значений, имеющих замыкающие пробелы, в данных char, varchar, binary и varbinary.
QUOTED_IDENTIFIER, --Заставляет SQL Server следовать правилам ISO относительно разделения кавычками идентификаторов и строк-литералов. Идентификаторы, заключенные в двойные кавычки, могут быть либо зарезервированными ключевыми словами Transact-SQL, либо могут содержать символы, которые обычно запрещены правилами синтаксиса для идентификаторов Transact-SQL.
ANSI_WARNINGS, --Задает поведение в соответствии со стандартом ISO для некоторых условий ошибок.
ARITHABORT, --Завершает запрос, если во время его выполнения возникла ошибка переполнения или деления на ноль.
XACT_ABORT ON --Указывает, выполняет ли SQL Server автоматический откат текущей транзакции, если инструкция языка Transact-SQL вызывает ошибку выполнения.
SET NUMERIC_ROUNDABORT, --Задает уровень отчетов об ошибках, создаваемых в тех случаях, когда округление в выражении приводит к потере точности.
IMPLICIT_TRANSACTIONS OFF --Устанавливает для соединения режим неявных транзакций.
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
/*
SERIALIZABLE
Указывает следующее.
Инструкции не могут считывать данные, которые были изменены другими транзакциями, но еще не были зафиксированы.
Другие транзакции не могут изменять данные, считываемые текущей транзакцией, до ее завершения.
Другие транзакции не могут вставлять новые строки со значениями ключа, которые входят в диапазон ключей, считываемых инструкциями текущей транзакции, до ее завершения.

Блокировка диапазона устанавливается в диапазоне значений ключа, соответствующих условиям поиска любой инструкции, выполненной во время транзакции. 
Обновление и вставка строк, удовлетворяющих инструкциям текущей транзакции, блокируется для других транзакций. Это гарантирует, что если какая-либо инструкция транзакции выполняется повторно, 
она будет считывать тот же самый набор строк. Блокировки диапазона сохраняются до завершения транзакции. Это самый строгий уровень изоляции, поскольку он блокирует целые диапазоны ключей и 
сохраняет блокировку до завершения транзакции. Из-за низкого параллелизма этот параметр рекомендуется использовать только при необходимости. Этот параметр действует так же, 
как и настройка HOLDLOCK всех таблиц во всех инструкциях SELECT в транзакции.
*/
GO
BEGIN TRANSACTION
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrepareCommonNationalExamCertificateCheckByPassport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByPassport]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

create PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByPassport]
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
	set @searchCommandText = 
		N'	
			
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
set @searchCommandText=replace(@searchCommandText,'<@selectPass>',@selectPass)		
set @searchCommandText=
@yearCommandText+
REPLACE(@searchCommandText,'<addCondition>',@addCondition)
		set @searchCommandText2 = ' 
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
--select * from @certificate_subject_correctness where ShareCertificateCheckingId=@idcert  and mark is not null and (SourceCertificateSubjectId is null or IsSubjectCorrect=0)
 if exists(select * from @certificate_subject_correctness_result where ShareCertificateCheckingId=@idcert  and mark is not null and (SourceCertificateSubjectId is null or IsSubjectCorrect=0))
 begin
 --print -999 
  if <@type>=2
  update @certificate_correctness 
  set CertificateNumber=NULL,CertificateId=null,SourceLastName=null,CertificateYear=null,RegionId=null,IsCertificateCorrect=null , SourceFirstName=null, SourcePatronymicName=null,TypographicNumber=null
  where ShareCertificateCheckingId=@idcert 
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
       
		-- Подсчет уникальных проверок
        exec CalculateUniqueChecksByBatchId @batchId = <BatchIdentifier>, @checkType = ''certificate'' 
        
       --select * from CommonNationalExamCertificateSubjectCheck where BatchId=<BatchIdentifier>    order by checkid,subjectid 
       --drop table rrr
       -- 	select * into rrr from #CommonNationalExamCertificateCheck
       -- 	drop table ttt
       -- 	select * into ttt from #CommonNationalExamCertificateSubjectCheck
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
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_cne_StartCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_cne_StartCheckBatch]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

create PROCEDURE [dbo].[usp_cne_StartCheckBatch]
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
		)
		
declare @typeStr varchar(200),@subjectStartStr varchar(200),@setStr varchar(200)



set @command='
declare cur cursor local fast_forward for 
	select id,Batch from dbo.CommonNationalExamCertificateCheckBatch 
	where <typeStr> and id=@batchId
open cur
declare @id bigint,@batch nvarchar(max),@certificateCheckingId uniqueidentifier,@index int,@subjectNum int,@row nvarchar(4000),@subjectStart int
declare @batches table (val nvarchar(max))
declare @tab table (id int,val nvarchar(300))
select @subjectNum=14,@subjectStart=<subjectStartStr>
fetch next from cur into @id,@batch
while @@FETCH_STATUS=0
begin    
  declare cc cursor for select replace(val,CHAR(10),'''') from [dbo].[ufn_ut_SplitFromStringWithId](@batch,CHAR(13)) order by id  
  open cc
  select @index=1
  fetch next from cc into @row
  while @@FETCH_STATUS=0
	begin 		
		if len(isnull(@row,''''))=0
			goto endcur1		
		delete from @tab
		select @certificateCheckingId=NEWID()
		insert #CommonNationalExamCertificateCheck(ShareCertificateCheckingId,CertificateCheckingId,BatchId, [Index]) values(@certificateCheckingId,@certificateCheckingId,@id,@index)
		insert @tab( id,val)  select id,val from  [dbo].[ufn_ut_SplitFromStringWithId](@row,''%'') order by id
		--select * from @tab
		update #CommonNationalExamCertificateCheck 
		set 
		<setStr>
		from @tab
		where CertificateCheckingId=@certificateCheckingId
  
		insert #CommonNationalExamCertificateSubjectCheck(ShareCertificateCheckingId,CertificateCheckingId ,BatchId,SubjectCode, Mark)
		select @certificateCheckingId,@certificateCheckingId,@id,b.Code,cast(replace(val,'','',''.'') as numeric(5,1))
		from  @tab a 
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
select	@typeStr= 'ISNULL(type,0)=0'
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
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
		begin
			print 00000
			exec [dbo].PrepareCommonNationalExamCertificateCheck @batchId
			truncate table #CommonNationalExamCertificateCheck
			truncate table #CommonNationalExamCertificateSubjectCheck
		end
		
----------------------------------------------------------------------------------------------		
select	@typeStr= 'type=1'
		,@subjectStartStr='2'
		,@setStr='CertificateNumber=(select val from @tab where id=1)'		
set @commandStr=@chooseDbText +
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
		begin
		 print 11111
--		 raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
			exec [dbo].PrepareCommonNationalExamCertificateCheckByNumber @batchId,1
			truncate table #CommonNationalExamCertificateCheck
			truncate table #CommonNationalExamCertificateSubjectCheck		 
		end
----------------------------------------------------------------------------------------------				


select	@typeStr= 'type=2'
		,@subjectStartStr='3'
		,@setStr='PassportSeria=(select val from @tab where id=1),	
		PassportNumber=(select val from @tab where id=2)'		
set @commandStr=@chooseDbText +
replace(
replace(
replace(
@command
,'<typeStr>',@typeStr)
,'<subjectStartStr>',@subjectStartStr)
,'<setStr>',@setStr)
--print @commandStr
exec sp_executesql @commandStr,N'@batchId bigint',@batchId=@batchId
if exists(select * from #CommonNationalExamCertificateCheck)
		begin
		 --print 22222
--		 raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
			exec [dbo].PrepareCommonNationalExamCertificateCheckByPassport @batchId,2
			truncate table #CommonNationalExamCertificateCheck
			truncate table #CommonNationalExamCertificateSubjectCheck		 
		end

drop table #CommonNationalExamCertificateCheck
drop table #CommonNationalExamCertificateSubjectCheck

 --select getdate() exec dbo.usp_cne_StartCheckBatch @batchId=8351
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (9, '009_2012_06_26_ModifyProcs.sql')
-- =========================================================================
GO

IF @@TRANCOUNT>0 BEGIN
	PRINT 'The database update succeeded.'
	--rollback TRANSACTION
	COMMIT TRANSACTION
	end
ELSE PRINT 'The database update failed.'
GO
SET NOEXEC OFF
GO
