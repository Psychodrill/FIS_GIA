

create PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheckByFIO]
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
    declare @id bigint,@PassportNumber	nvarchar(255) ,@sum numeric(12,2),@CertificateCheckingId	uniqueidentifier
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