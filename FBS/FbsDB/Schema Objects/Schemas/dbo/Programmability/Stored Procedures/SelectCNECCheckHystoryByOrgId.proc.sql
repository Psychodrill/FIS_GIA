alter proc [dbo].[SelectCNECCheckHystoryByOrgId]
	@idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
	@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null, @dats datetime=null,@datf datetime=null
as
begin
	declare @str nvarchar(max),@ss nvarchar(20),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max)

	if @fld='date' 	
		set @ss='cb.CreateDate'
	else 
		if @fld='TypeCheck' 
			set @ss='c.id'
		else 
			set @ss='c.'+@fld
			
		
	if @isUnique =0 
	begin	
		if @ps = 0 	
			set @ps = 1
		else
			set @pf=@pf - 1	
			
		set @str=
		'
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())			
		
		declare @tab table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)
		declare @table table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)
		
		insert @tab
		select top (@pf) id1,id2,id3
			from 
			(
			select top (@pf) c.id id1,null id2,null id3,'+@ss+'
			from CommonNationalExamCertificateCheck c
				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
		'
		if @dats is not null 
			set @str=@str+' and cb.CreateDate >= @dats'
		if @datf is not null 
			set @str=@str+' and cb.CreateDate <= @datf'
			
		if @idorg<>-1	
			set @str=@str+
		'						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg			
		'
		if @TypeCheck is not null and @TypeCheck <> 'Пакетная' 
			set @str=@str+
		'
				and 1=0
		'
		if @LastName is not null
			set @str=@str+
		'
				and c.LastName like ''%''+@LastName+''%'' 
		'			
		set @str=@str+
		'
			order by '+@ss+case when @so = 0 then ' asc' else ' desc' end + '
			union all
			select top (@pf) null id1,c.id id2,null id3,'+@ss+'
			FROM CommonNationalExamCertificateRequest c with(nolock)
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
		'
		if @dats is not null 
			set @str=@str+' and cb.CreateDate >= @dats'
		if @datf is not null 
			set @str=@str+' and cb.CreateDate <= @datf'
					
		if @idorg<>-1	
			set @str=@str+
		'						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg		
		'
		if @TypeCheck is not null and @TypeCheck <> 'Пакетная' 
			set @str=@str+
		'
				and 1=0
		'		
		if @LastName is not null
			set @str=@str+
		'
				and c.LastName like ''%''+@LastName+''%'' 
		'					
		set @str=@str+
		'		
			order by '+@ss+case when @so = 0 then ' asc' else ' desc' end + '				
			union all
			select top (@pf) null id1,null id2,c.id id3,'+case when @fld='date' then 'EventDate CreateDate' else @ss end+'
			FROM CNEWebUICheckLog c with(nolock)
		'
		if @idorg<>-1	
			set @str=@str+
		'
				join Account acc with(nolock) on acc.id=c.AccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg
		'		
		if @TypeCheck is not null and @TypeCheck <> 'Интерактивная' 
			set @str=@str+
		'
				and 1=0
		'
		if @dats is not null 
			set @str=@str+' and c.EventDate >= @dats'
		if @datf is not null 
			set @str=@str+' and c.EventDate <= @datf'	
							
		if @LastName is not null
			set @str=@str+
		'
				and c.LastName like ''%''+@LastName+''%'' 
		'				
		set @str=@str+
		'		
			order by '+case when @fld='date' then 'EventDate' else @ss end+case when @so = 0 then ' asc' else ' desc' end+'
		) c
		order by '+case when @fld='date' then 'CreateDate' else @ss end+case when @so = 0 then ' asc' else ' desc' end + '
			
		insert @table
		select id1,id2,id3 from @tab where id between @ps and @pf '
		print @str
		set @str=@str+
		'	
		select * from 
		(
			select c.Id,cb.CreateDate,''Пакетная'' TypeCheck,c.CertificateNumber,c.TypographicNumber,
				  c.LastName,c.FirstName,c.PatronymicName,
				  ISNULL(c.PassportSeria,'''')+'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
					( SELECT ( 
								SELECT CAST(s.SubjectId AS VARCHAR(20))
									+ ''=''
									+ REPLACE(CAST(s.Mark AS VARCHAR(20)),
											  '','', ''.'') + '','' AS [text()]
								FROM dbo.CommonNationalExamCertificateSubjectCheck s
								WHERE s.CheckId = c.Id AND s.Mark IS NOT NULL
								FOR
								XML PATH(''''),
								TYPE
							 ) marks
					) as Marks,c.SourceCertificateIdGuid, 
			   case when ed.[ExpireDate] is null then ''Не найдено''  
					when certificate_deny.CertificateFK is not null then ''Аннулировано'' 
					when getdate() <= ed.[ExpireDate] then ''Действительно''
				else ''Истек срок'' end Status,c1.id rn
			FROM CommonNationalExamCertificateCheck c with(nolock)
				JOIN CommonNationalExamCertificateCheckBatch cb with(nolock) ON c.BatchId=cb.Id
				JOIN @table c1 on c1.id1=c.id
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGuid and cc.[Year] between @yearFrom and @yearTo	
				left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid '
		set @str=@str+
		'					
			union all
			select c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,c.SourceCertificateNumber CertificateNumber,c.TypographicNumber,
				c.LastName,c.FirstName,c.PatronymicName,
				ISNULL(c.PassportSeria,'''') + '' ''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
				null as Marks,c.SourceCertificateIdGuid,
				case WHEN ed.[ExpireDate] is null THEN ''Не найдено'' 
					when certificate_deny.CertificateFK is not null then ''Аннулировано'' 
					when getdate() <= ed.[ExpireDate] then ''Действительно''
					else ''Истек срок'' end STATUS,
				c1.id rn
			FROM CommonNationalExamCertificateRequest c with(nolock)
				JOIN @table c1 on c1.id2=c.id
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
				left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	'
		set @str=@str+
		'						
			union all
			select cb.id,cb.EventDate,''Интерактивная'' TypeCheck,
				ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
				ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,
				ISNULL(a.Surname, cb.LastName) LastName, 
				ISNULL(a.Name, cb.FirstName) FirstName, 
				ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
				case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +'' '' + cb.PassportNumber
					 else a.DocumentSeries + '' '' + a.DocumentNumber end PassportData, 
				b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
				b.CertificateID SourceCertificateIdGuid,
				case when ed.[ExpireDate] is null then ''Не найдено''  
					when certificate_deny.CertificateFK is not null then ''Аннулировано''
					when getdate() <= ed.[ExpireDate] then ''Действительно''
				else ''Истек срок'' end Status,cb1.id rn		
			from 
				@table cb1
				join CNEWebUICheckLog cb with(nolock) on cb1.id3=cb.id															
				JOIN prn.Certificates AS b with(nolock) on cb.FoundedCNEId <> ''Нет свидетельства'' and cb.FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
				left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.UseYear=b.UseYear
				left join ExamcertificateUniqueChecks CC with(nolock) on b.CertificateID=cc.idGUID and cc.[Year]=b.UseYear
				left join [ExpireDate] as ed on ed.[Year] = b.Useyear
				left join prn.CancelledCertificates certificate_deny with (nolock)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = b.CertificateID
		) c																
		order by '+case when @fld='date' then 'CreateDate' else @ss end+case when @so = 0 then ' asc' else ' desc' end				
		
		exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255),@dats datetime,@datf datetime',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName,@dats=@dats,@datf=@datf
		return
	end					
	else
	begin	
	    set @pf=@pf - 1			
	    
		set @str=
				'
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())		
		declare @tab table(id int identity(1,1) primary key,id1 int,idtype int,idguid uniqueidentifier)		
		declare @table table(id int identity(1,1) primary key,id1 int,idtype int)
			
		insert @tab			
		select id1,idtype,SourceCertificateIdGuid
		from 
		(
			select top (1000000) min(id1) id1,idtype,SourceCertificateIdGuid'+
				case when @fld='LastName' then ',LastName' 
					 when @fld = 'date' or @dats is not null or @datf is not null then ',min(CreateDate) CreateDate'
					 else '' 
				end
		  +' from 
				(				
				select min(c.id) id1, 1 idtype,c.SourceCertificateIdGuid'+
				case when @fld='LastName' then ',c.LastName' 
					 when @fld = 'date' or @dats is not null or @datf is not null then ',min(cb.CreateDate) CreateDate'
					 else '' 
				end
		  +' from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateCheck c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 		
				'							
		if @LastName is not null
			set @str=@str+'												
					and c.LastName like ''%''+@LastName+''%'' 
						  '	
		if @TypeCheck <> 'Пакетная' and @TypeCheck is not null
				set @str=@str+'						
					and 1=0 
							  '					
		if @idorg<>-1 
			set @str=@str+'									
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  '				
		set @str=@str+'
				group by c.SourceCertificateIdGuid'+
				case when @fld='LastName' then ',c.LastName' 
					 else '' 
				end
		  +' union all
				select min(c.id) id1,2 idtype,c.SourceCertificateIdGuid'+
				case when @fld='LastName' then ',c.LastName' 
					 when @fld = 'date' or @dats is not null or @datf is not null then ',min(cb.CreateDate) CreateDate'
					 else '' 
				end
		  +' from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateRequest c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo									
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
					  '								  
		if @LastName is not null
			set @str=@str+'												
							and c.LastName like ''%''+@LastName+''%'' 
						  '	
		if @TypeCheck <> 'Пакетная' and @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '					
		if @idorg<>-1 
			set @str=@str+'											 		
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  '						
		set @str=@str+'
				group by c.SourceCertificateIdGuid'+
				case when @fld='LastName' then ',c.LastName' 
					 else '' 
				end
		  +' union all
				select min(c.id) id1,3 idtype, cast(c.FoundedCNEId as uniqueidentifier)'+
				case when @fld='LastName' then ',c.LastName' 
					 when @fld = 'date' or @dats is not null or @datf is not null then ',min(c.EventDate) CreateDate '
					 else '' 
				end
		  +' from ExamCertificateUniqueChecks a with(nolock)
					join CNEWebUICheckLog c with(nolock) on c.FoundedCNEId <> ''Нет свидетельства''	and c.FoundedCNEId=a.idGUID	and a.[Year] between @yearFrom and @yearTo												
					  '								  
		if @idorg<>-1 
			set @str=@str+
			          '							
					join Account Acc on acc.id=c.AccountId and @idorg=Acc.OrganizationId 	
					join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
					  '		
		if @TypeCheck <> 'Интерактивная' and @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '						  			
		set @str=@str+
		'			
				group by c.FoundedCNEId'+
				case when @fld='LastName' then ',c.LastName' 
					 else '' 
				end
		  +') c		
			group by SourceCertificateIdGuid, idtype'+
				case when @fld='LastName' then ',c.LastName'
					 else '' 
				end
		  +' order by '+case when @fld='LastName' then 'LastName' else ' id1' end + case when @so = 0 then ' asc' else ' desc' end + '
		) c 
		where 1=1
		'
		if @dats is not null 
			set @str=@str+' and c.CreateDate >= @dats'
		if @datf is not null 
			set @str=@str+' and c.CreateDate <= @datf'	
		if @fld='date'
		set @str=@str+' order by c.CreateDate '+ case when @so = 0 then ' asc' else ' desc' end 	
			
		
		set @str=@str+'						
		insert @table
		select id1,idtype from
		(
			select a.id1,a.idtype,row_number() over (order by a.id) rn from @tab a
				join (select min(id1) id1,idguid from @tab group by idguid) b on a.id1=b.id1 and a.idguid=b.idguid		
		) t
		where rn between @ps and @pf			
		
		/*select * from @table
		select * from @tab*/
		'
	print @str
		set @str=@str+
		'	
		select * into #ttt 
		from 
		(		
			select c.Id,cb.CreateDate,''Пакетная'' TypeCheck,c.CertificateNumber,c.TypographicNumber,
				  c.LastName,c.FirstName,c.PatronymicName,
				  ISNULL(c.PassportSeria,'''')+'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
					( SELECT ( 
								SELECT CAST(s.SubjectId AS VARCHAR(20))
									+ ''=''
									+ REPLACE(CAST(s.Mark AS VARCHAR(20)),
											  '','', ''.'') + '','' AS [text()]
								FROM dbo.CommonNationalExamCertificateSubjectCheck s
								WHERE s.CheckId = c.Id AND s.Mark IS NOT NULL
								FOR
								XML PATH(''''),
								TYPE
							 ) marks
					) as Marks,c.SourceCertificateIdGuid, 
			   case when ed.[ExpireDate] is null then ''Не найдено''  
					when certificate_deny.CertificateFK is not null then ''Аннулировано'' 
					when getdate() <= ed.[ExpireDate] then ''Действительно''
				else ''Истек срок'' end Status 
			FROM CommonNationalExamCertificateCheck c with(nolock)
				JOIN CommonNationalExamCertificateCheckBatch cb with(nolock) ON c.BatchId=cb.Id
				JOIN @table c1 on c1.id1=c.id and c1.idtype=1
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGuid and cc.[Year] between @yearFrom and @yearTo	
				left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid '
		set @str=@str+
		'					
			union all
			select c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,c.SourceCertificateNumber CertificateNumber,c.TypographicNumber,
				c.LastName,c.FirstName,c.PatronymicName,
				ISNULL(c.PassportSeria,'''') + '' ''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
				null as Marks,c.SourceCertificateIdGuid,
				case WHEN ed.[ExpireDate] is null THEN ''Не найдено'' 
					when certificate_deny.CertificateFK is not null then ''Аннулировано'' 
					when getdate() <= ed.[ExpireDate] then ''Действительно''
					else ''Истек срок'' end STATUS
			FROM CommonNationalExamCertificateRequest c  with(nolock)
				JOIN @table c1 on c1.id1=c.id and c1.idtype=2
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
				left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	'
		set @str=@str+
		'						
			union all
			select cb.id,cb.EventDate,''Интерактивная'' TypeCheck,
				ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
				ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,
				ISNULL(a.Surname, cb.LastName) LastName, 
				ISNULL(a.Name, cb.FirstName) FirstName, 
				ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
				case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +'' '' + cb.PassportNumber
					 else a.DocumentSeries + '' '' + a.DocumentNumber end PassportData, 
				b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
				b.CertificateID SourceCertificateIdGuid,
				case when ed.[ExpireDate] is null then ''Не найдено''  
					when certificate_deny.CertificateFK is not null then ''Аннулировано''
					when getdate() <= ed.[ExpireDate] then ''Действительно''
				else ''Истек срок'' end Status				
			from 
				@table cb1
				join CNEWebUICheckLog cb with(nolock) on cb1.id1=cb.id and idtype=3
				JOIN prn.Certificates AS b with(nolock) on cb.FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
				left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.UseYear=b.UseYear
				left join ExamcertificateUniqueChecks CC with(nolock) on b.CertificateID=cc.idGUID and b.UseYear=b.UseYear
				left join [ExpireDate] as ed on ed.[Year] = b.Useyear
				left join prn.CancelledCertificates certificate_deny with (nolock)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = b.CertificateID																
		) t
				
				--select * from #ttt order by id desc
		select * from
		(
			select *,row_number() over (order by '+case when @fld='date' then 'CreateDate' else @ss end+case when @so = 0 then ' asc' else ' desc' end + ') rn from
			(
				select a.* from #ttt a
					join (select min(id)id,SourceCertificateIdGuid from #ttt group by SourceCertificateIdGuid) b on a.id=b.id
			) c
		) t
		order by rn
		
		drop table #ttt	'
		exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255),@dats datetime,@datf datetime',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName,@dats=@dats,@datf=@datf		
		return
	end	
end

GO
