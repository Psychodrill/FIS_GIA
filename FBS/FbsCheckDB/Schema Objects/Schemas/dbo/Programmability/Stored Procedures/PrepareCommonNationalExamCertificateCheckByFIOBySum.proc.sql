

create PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFIOBySum]
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