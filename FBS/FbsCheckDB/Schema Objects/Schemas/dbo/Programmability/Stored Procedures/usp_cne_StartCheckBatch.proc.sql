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

declare @baseName nvarchar(255)
set @baseName = dbo.GetDataDbName(1, 1)

set @command='
declare cur cursor local fast_forward for 
  select id,Batch from dbo.CommonNationalExamCertificateCheckBatch with(nolock)
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
    join '+@baseName+'.dbo.Subject b on a.Id-@subjectStart+1=b.Id
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
--select @commandStr
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
