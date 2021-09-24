

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
	 drop table 		 tmp_CommonNationalExamCertificateCheck
	 select * into tmp_CommonNationalExamCertificateCheck from #CommonNationalExamCertificateCheck
	 
	 drop table 		 tmp_CommonNationalExamCertificateSubjectCheck
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