-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (19, '019_2012_08_03_ModifyProcs')
-- =========================================================================
GO 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[usp_cne_StartCheckBatch]
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
			print 00000
			
			update #CommonNationalExamCertificateCheck
			set 
				PassportSeria = null,
				PassportNumber = null
			
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
		 
--		 raiserror('[usp_cne_StartCheckBatch]: CommonNationalExamCertificateCheck is empty',16,1)
			exec [dbo].PrepareCommonNationalExamCertificateCheckByPassport @batchId,2
			truncate table #CommonNationalExamCertificateCheck
			truncate table #CommonNationalExamCertificateSubjectCheck		 
		end
----------------------------------------------------------------------------------------------				


select	@typeStr= 'type=3'
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


select	@typeStr= 'type=4'
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


select	@typeStr= 'type=5'
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
ALTER PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFIO]
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
				CommonNationalExamCertificate ta with(nolock) 
				left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
				where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
			when tt.CertificateCheckingId is null then 0 else 1 end [Status],
			case when (select COUNT(distinct PassportNumber) from CommonNationalExamCertificate  with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake		
       from
       #CommonNationalExamCertificateCheck a        
       left join        
       (
       select * 
       from     
       (select distinct a1.CertificateCheckingId,a1.CertificateNumber fio,a1.PassportNumber sum, b1.PassportNumber from 
       #CommonNationalExamCertificateCheck a1 join 
       CommonNationalExamCertificate b1 with(nolock) on  b1.fio=a1.CertificateNumber
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
       CommonNationalExamCertificate b with(nolock) on  b.fio=a.CertificateNumber
       join CommonNationalExamCertificateSubject c with(nolock) on b.Id=c.CertificateId
       join Subject d on d.id=c.SubjectId
       where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join(
        select a.CertificateCheckingId,a.PassportNumber,c.Code,b.mark 
       from
       (select distinct a.CertificateCheckingId,a.CertificateNumber,b.PassportNumber from 
       #CommonNationalExamCertificateCheck a join 
       CommonNationalExamCertificate b with(nolock) on  b.fio=a.CertificateNumber
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
    declare @id bigint,@PassportNumber	nvarchar(255) ,@sum numeric(12,2),@CertificateCheckingId	uniqueidentifier
    --select * from #CommonNationalExamCertificateCheck 
    
    select * into #CommonNationalExamCertificateSumCheck from  CommonNationalExamCertificateSumCheck a where a.BatchId=<@BatchId> and status=0
		
	declare cc cursor local fast_forward for 
		select distinct a.id ,b.PassportNumber,a.sum,cc.CertificateCheckingId
		from 
		#CommonNationalExamCertificateSumCheck a 
		join CommonNationalExamCertificate b with(nolock) on  b.fio=a.name		
		left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
		join #CommonNationalExamCertificateCheck cc on cc.CertificateNumber=a.name and cast(cc.PassportSeria as numeric(12,2))=a.sum
		where de.CertificateNumber is null
	open cc
	create table #t (SubjectId int,mark numeric(5,1))   
	declare @maxsum numeric(12,2)
	declare @t table(id bigint primary key)
	declare @r table(SubjectId int,mark numeric(5,1))
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
		c.SubjectId,c.mark  
		from 
		dbo.CommonNationalExamCertificate b with(nolock)
		join dbo.CommonNationalExamCertificateSubject c with(nolock) on b.Id=c.CertificateId  	
		join #CommonNationalExamCertificateSubjectCheck d on  c.SubjectId=d.SubjectId and d.CertificateCheckingId=@CertificateCheckingId
		where b.[YEAR] between  @yearFrom and @yearTo and b.PassportNumber=@PassportNumber 
		--print ''    20.''+right(convert(varchar,SYSDATETIME()),12)
		
		delete from @r
		insert @r
		select b.SubjectId,b.mark from 
		#CommonNationalExamCertificateSubjectCheck b  		
		join #t c on c.SubjectId=b.SubjectId
		where b.CertificateCheckingId=@CertificateCheckingId
	if exists(select * from @r )
	begin
		--select @id,@PassportNumber,@sum,@CertificateCheckingId
		truncate table #tt
		declare @i int, @l int
		select @i = min(SubjectId) from @r
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
  
			set @i = (select min(SubjectId) from @r where SubjectId>@i)
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
			update CommonNationalExamCertificateSumCheck set sum=0 where id=@id
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
ALTER PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum]
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

declare @CertificateCheckingId	uniqueidentifier,@PassportNumber	nvarchar(255) ,@sum numeric(12,2)   	
 declare cc cursor local fast_forward for 
       select 
       distinct
       --top 1
        a.CertificateCheckingId,b.PassportNumber,cast(a.PassportSeria as numeric(12,2))
       from 
       #CommonNationalExamCertificateCheck a join 
       CommonNationalExamCertificate b with(nolock) on  b.fio=a.CertificateNumber    
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b.Number
       where b.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
  open cc
  create table #t (SubjectId int,mark numeric(5,1))   
  declare @r table(SubjectId int,mark numeric(5,1))
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
		c.SubjectId,c.mark  
		from 
		dbo.CommonNationalExamCertificate b with(nolock)
		join dbo.CommonNationalExamCertificateSubject c with(nolock) on b.Id=c.CertificateId  	
		join #CommonNationalExamCertificateSubjectCheck d on  c.SubjectId=d.SubjectId and d.CertificateCheckingId=@CertificateCheckingId
		where b.[YEAR] between  @yearFrom and @yearTo and b.PassportNumber=@PassportNumber 
		delete from @r
		insert @r
		select b.SubjectId,b.mark from 
		#CommonNationalExamCertificateSubjectCheck b  		
		join #t c on c.SubjectId=b.SubjectId
		where b.CertificateCheckingId=@CertificateCheckingId
		--print ''    20.''+right(convert(varchar,SYSDATETIME()),12)
	if exists(select * from @r)
	begin
		
		truncate table #tt
		declare @i int, @l int
		select @i = min(SubjectId) from @r
		set @l = 0
		insert #tt(c, s, level) values('''', 0, 0)
		--print ''    30.''+right(convert(varchar,SYSDATETIME()),12)		
		while @i is not null 
		begin
			insert #tt(c, s, level) 
			select
			'''',
		--	 t.c+case t.c when '''' then '''' else ''+'' end+cast(x.SubjectId as varchar(16))+''-''+cast(x.mark as varchar(16)),
			 t.s+x.mark, @l+1
			from (select * from #tt where level = @l) t
				cross join
				(select * from #t where SubjectId = @i) x
  
			set @i = (select min(SubjectId) from @r where SubjectId>@i)
			set @l = @l+1
		end
		
		--select * from #tt where level = @l 
		if exists(select * from #tt where level = @l and s=@sum
		and not exists(select * from 
		#CommonNationalExamCertificateSubjectCheck b  		
		left join #t c on c.SubjectId=b.SubjectId
		where b.CertificateCheckingId=@CertificateCheckingId and c.SubjectId is null ))
		begin		 
		 update #CommonNationalExamCertificateCheck set lastname=''1'',FirstName=cast(@sum as varchar(255)) where CertificateCheckingId=@CertificateCheckingId		
		end
		else
		begin
	--	select c, s from #tt where level = @l
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
  select <@BatchId>,CertificateNumber,isnull(cast(FirstName as numeric(12,1)),0),
  case when not exists(
				select * from 
				CommonNationalExamCertificate ta with(nolock) 
				left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
				where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
			when lastname is null then 0 else 1 end [Status],
			case when (select COUNT(distinct PassportNumber) from CommonNationalExamCertificate with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake
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
create PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFioAndSubjects]
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
	 --drop table 		 tmp_CommonNationalExamCertificateCheck
	 --select * into tmp_CommonNationalExamCertificateCheck from #CommonNationalExamCertificateCheck
	 
	 --drop table 		tmp_CommonNationalExamCertificateSubjectCheck
	 --select * into tmp_CommonNationalExamCertificateSubjectCheck from #CommonNationalExamCertificateSubjectCheck
	 --return 
	 update a set SubjectId=b.Id
	from 
	#CommonNationalExamCertificateSubjectCheck a join subject b on a.SubjectCode=b.Code
	 	 --select * from #CommonNationalExamCertificateSubjectCheck
	 declare @yearFrom int, @yearTo int
	 select @yearFrom = 2010, @yearTo = 2012
       --PrepareCommonNationalExamCertificateCheckByFIO    
       delete CommonNationalExamCertificateSumCheck where BatchId=<@BatchId>
       insert CommonNationalExamCertificateSumCheck(BatchId,Name,[Sum],[Status],NameSake)
       select tt.BatchId,tt.name,tt.sum,tt.Status,tt.NameSake
       from(
       select distinct
       <@BatchId> BatchId,a.CertificateNumber Name,a.PassportNumber [Sum],      
       case when not exists(
				select * from 
				CommonNationalExamCertificate ta with(nolock) 
				left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = ta.Number
				where ta.fio=a.CertificateNumber and ta.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null)  then 2
			when tt.CertificateCheckingId is null then 0 else 1 end [Status],
			case when (select COUNT(distinct PassportNumber) from CommonNationalExamCertificate  with(nolock) where fio=a.CertificateNumber and [YEAR] between  @yearFrom and @yearTo)>1  then 1 else 0 end NameSake		
       from
       #CommonNationalExamCertificateCheck a        
       left join        
       (
       select * 
       from     
       (select distinct a1.CertificateCheckingId,a1.CertificateNumber fio,a1.PassportNumber sum, b1.PassportNumber from 
       #CommonNationalExamCertificateCheck a1 join 
       CommonNationalExamCertificate b1 with(nolock) on  b1.fio=a1.CertificateNumber
       left outer join dbo.CommonNationalExamCertificateDeny de with(nolock) on de.CertificateNumber = b1.Number
       where b1.[YEAR] between  @yearFrom and @yearTo and de.CertificateNumber is null
       ) tbl
       where  
       
       not exists(
       select * from
       (
       select SubjectId from #CommonNationalExamCertificateSubjectCheck a 
       where a.CertificateCheckingId=tbl.CertificateCheckingId
       ) t1
       left join 
       (
       select
        distinct a.CertificateCheckingId,a.PassportNumber,a.SubjectId       
        from
       (
       select a.CertificateCheckingId,b.PassportNumber,c.SubjectId
       from 
       #CommonNationalExamCertificateCheck a join 
       CommonNationalExamCertificate b with(nolock) on  b.fio=a.CertificateNumber
       join CommonNationalExamCertificateSubject c with(nolock) on b.Id=c.CertificateId       
       where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join(
        select a.CertificateCheckingId,a.PassportNumber,b.SubjectId
       from
       (select distinct a.CertificateCheckingId,a.CertificateNumber,b.PassportNumber from 
       #CommonNationalExamCertificateCheck a join 
       CommonNationalExamCertificate b with(nolock) on  b.fio=a.CertificateNumber
              where b.[YEAR] between  @yearFrom and @yearTo
       ) a
       join #CommonNationalExamCertificateSubjectCheck b on b.CertificateCheckingId=a.CertificateCheckingId       

       ) b on a.PassportNumber=b.PassportNumber and a.SubjectId=b.SubjectId
       where  a.PassportNumber=tbl.PassportNumber
       ) t2       
       on t1.SubjectId=t2.SubjectId 
       where t2.SubjectId is null
       )
       )       tt  on a.CertificateCheckingId=tt.CertificateCheckingId and a.CertificateNumber=tt.fio
   
    ) tt
    join (select CertificateNumber,min(id) id from #CommonNationalExamCertificateCheck group by CertificateNumber)  a on tt.name=a.CertificateNumber
    order by a.id
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