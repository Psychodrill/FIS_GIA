-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (10, '010_2012_06_27_FixLineBreaks.sql')
-- =========================================================================
GO

-- ================================================
-- Парсинг данных и запуск проверок
-- ================================================
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
  declare cc cursor for select val from [dbo].[ufn_ut_SplitFromStringWithId](REPLACE(REPLACE(@batch, CHAR(13) + CHAR(10), CHAR(13)), CHAR(10), CHAR(13)),CHAR(13)) order by id  
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
