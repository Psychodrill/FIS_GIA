insert into Migrations(MigrationVersion, MigrationName) values (102, '102_2013_07_16_Уникальнаые проверки версия 2.sql')
go
alter proc [dbo].[SelectCNECCheckHystoryByOrgId]
	@idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
	@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max),@ss nvarchar(10),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max)
	set @pf=@pf
	if @ps = 0 
		set @ps = 1

	if @fld='TypeCheck'
		set @ss='id'
	else
		set @ss=@fld
		
	if @isUnique =0 
	begin	
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
			select top (@pf) c.id id1,null id2,null id3,c.'+@ss+'
			from CommonNationalExamCertificateCheck c
		'
		if @idorg<>-1	
			set @str=@str+
		'				
				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 			
		'
		if @TypeCheck is not null or @TypeCheck <> 'Пакетная' 
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
			order by c.'+@ss+case when @so = 0 then ' asc' else ' desc' end + '
			union all
			select top (@pf) null id1,c.id id2,null id3,c.'+@ss+'
			FROM CommonNationalExamCertificateRequest c 
		'
		if @idorg<>-1	
			set @str=@str+
		'		
				JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId		
		'
		if @TypeCheck is not null or @TypeCheck <> 'Пакетная' 
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
			order by c.'+@ss+case when @so = 0 then ' asc' else ' desc' end + '				
			union all
			select top (@pf) null id1,null id2,c.id id3,c.'+@ss+'
			FROM CNEWebUICheckLog c
		'
		if @idorg<>-1	
			set @str=@str+
		'
				join Account acc on acc.id=c.AccountId 	
				join Organization2010 d on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg
		'		
		if @TypeCheck is not null or @TypeCheck <> 'Интерактивная' 
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
		'	where c.FoundedCNEId <> ''Нет свидетельства''	
			order by c.'+@ss+case when @so = 0 then ' asc' else ' desc' end+'
		) t
		order by '+@ss+case when @so = 0 then ' asc' else ' desc' end + '
		
		insert @table
		select id1,id2,id3 from @tab where id between @ps and @pf '
		
		set @str=@str+
		'	
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
		FROM CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
			JOIN @table c1 on c1.id1=c.id
			left join ExamcertificateUniqueChecks CC on c.SourceCertificateIdGuid=cc.idGuid and cc.[Year] between @yearFrom and @yearTo	
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
			case WHEN c.SourceCertificateId IS NULL THEN ''Не найдено'' else
				case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
				else ''Истек срок'' end end  STATUS,c1.id rn
		FROM CommonNationalExamCertificateRequest c 
			JOIN @table c1 on c1.id2=c.id
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
			left join ExamcertificateUniqueChecks CC on c.SourceCertificateIdGuid=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
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
			join CNEWebUICheckLog cb on cb1.id3=cb.id															
			JOIN prn.Certificates AS b with(nolock) on FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
			left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
			left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
			left join [ExpireDate] as ed on ed.[Year] = b.Useyear
			left join prn.CancelledCertificates certificate_deny with (nolock)
				on certificate_deny.[UseYear] between @yearFrom and @yearTo
					and certificate_deny.CertificateFK = b.CertificateID																
		'
		
		exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255)',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName
		return
	end					
	else
	begin				
		set @str=
				'
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())		
		declare @table table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)
		
		insert @table			
		select id1,id2,id3
		from 
			(				
			select min(c.id) id1, 0 id2,0 id3,c.SourceCertificateIdGuid
			from ExamCertificateUniqueChecks a
				join CommonNationalExamCertificateCheck c on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
				'
		if @LastName is not null
			set @str=@str+'												
					and c.LastName like ''%''+@LastName+''%'' 
						  '	
		if @TypeCheck <> 'Пакетная' or @TypeCheck is not null
				set @str=@str+'						
					and 1=0 
							  '					
		if @idorg<>-1 
			set @str=@str+'		
				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 				
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  '				
		set @str=@str+'
			group by c.SourceCertificateIdGuid'+case when @ss='LastName' then ',c.LastName' else '' end + '
			union all
			select 0 id1,min(c.id) id2,0 id3,c.SourceCertificateIdGuid
			from ExamCertificateUniqueChecks a
				join CommonNationalExamCertificateRequest c on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo									
					  '
		if @LastName is not null
			set @str=@str+'												
							and c.LastName like ''%''+@LastName+''%'' 
						  '	
		if @TypeCheck <> 'Пакетная' or @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '					
		if @idorg<>-1 
			set @str=@str+'				
				JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 		 		
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  '						
		set @str=@str+'
			group by c.SourceCertificateIdGuid'+case when @ss='LastName' then ',c.LastName' else '' end + '
			union all
			select 0 id1,0 id2,min(c.id) id3,cast(c.FoundedCNEId as uniqueidentifier)
			from ExamCertificateUniqueChecks a
				join CNEWebUICheckLog c with(nolock) on c.FoundedCNEId <> ''Нет свидетельства''	and c.FoundedCNEId=a.idGUID	and a.[Year] between @yearFrom and @yearTo												
					  '
		if @idorg<>-1 
			set @str=@str+
			          '							
				join Account Acc on acc.id=c.AccountId	
				join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
					  '		
		if @TypeCheck <> 'Интерактивная' or @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '						  			
		set @str=@str+
		'			
			group by c.FoundedCNEId'+case when @ss='LastName' then ',c.LastName' else '' end + '
		) t					
		'
	
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
			FROM CommonNationalExamCertificateCheck c
				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
				JOIN @table c1 on c1.id1=c.id
				left join ExamcertificateUniqueChecks CC on c.SourceCertificateIdGuid=cc.idGuid and cc.[Year] between @yearFrom and @yearTo	
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
				case WHEN c.SourceCertificateId IS NULL THEN ''Не найдено'' else
					case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
					else ''Истек срок'' end end  STATUS
			FROM CommonNationalExamCertificateRequest c 
				JOIN @table c1 on c1.id2=c.id
				JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
				left join ExamcertificateUniqueChecks CC on c.SourceCertificateIdGuid=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
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
				join CNEWebUICheckLog cb on cb1.id3=cb.id															
				JOIN prn.Certificates AS b with(nolock) on FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
				left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
				left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
				left join [ExpireDate] as ed on ed.[Year] = b.Useyear
				left join prn.CancelledCertificates certificate_deny with (nolock)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = b.CertificateID																
		) t
				
		select * from
		(
			select *,row_number() over (order by '+@ss+case when @so = 0 then ' asc' else ' desc' end + ') rn from
			(
				select a.* from #ttt a
					join (select min(id)id,SourceCertificateIdGuid from #ttt group by SourceCertificateIdGuid) b on a.id=b.id
			) t
		) t
		order by id
		
		drop table #ttt	'
		exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255)',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName		
		return
	end	
end

GO
alter proc [dbo].[SelectCNECCheckHystoryByOrgIdCount]
	@idorg int, @isUnique bit=0,@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max),@s nvarchar(100)
	
	set @s=cast(newid() as nvarchar(100))
	if @isUnique =0 
	begin
		set @str='	
		
		select count(*) from
				(
				select 1 t
				FROM CommonNationalExamCertificateCheck c
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '	
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '													
		set @str=@str+'	
				union all
				select 1 t 
				FROM CommonNationalExamCertificateRequest c 
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
					left JOIN vw_Examcertificate r with(nolock) ON c.ParticipantID=r.ParticipantID
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '							
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '
		set @str=@str+'
				union all
				select 1 t
				from CNEWebUICheckLog cb 
					join Account Acc on acc.id=cb.AccountId  					
					join Organization2010 d on d.id=Acc.OrganizationId '					
				
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '

		if @LastName is not null
			set @str=@str+'
			    left join vw_Examcertificate c with(nolock) ON cb.FoundedCNEId=cast(c.Id as nvarchar(255)) where 1 = 1 
				and c.LastName like ''%''+@LastName+''%'' '				
				
		if @idorg<>-1 
			set @str=@str+'
				where d.id=@idorg and d.DisableLog=0 '
																
		set @str=@str+'				
				) t
		
				'
	end
	else
	begin
				set @str=
				'
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())		
		declare @table table(id int identity(1,1) primary key,idguid uniqueidentifier)
		
		insert @table		
		select SourceCertificateIdGuid
		from 
			(				
			select min(c.id) id1, 0 id2,0 id3,c.SourceCertificateIdGuid
			from ExamCertificateUniqueChecks a
				join CommonNationalExamCertificateCheck c on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
				'
		if @LastName is not null
			set @str=@str+'												
					and c.LastName like ''%''+@LastName+''%'' 
						  '	
		if @TypeCheck <> 'Пакетная' or @TypeCheck is not null
				set @str=@str+'						
					and 1=0 
							  '					
		if @idorg<>-1 
			set @str=@str+'		
				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 				
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  '				
		set @str=@str+'
			group by c.SourceCertificateIdGuid
			union all
			select 0 id1,min(c.id) id2,0 id3,c.SourceCertificateIdGuid
			from ExamCertificateUniqueChecks a
				join CommonNationalExamCertificateRequest c on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo									
					  '
		if @LastName is not null
			set @str=@str+'												
							and c.LastName like ''%''+@LastName+''%'' 
						  '	
		if @TypeCheck <> 'Пакетная' or @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '					
		if @idorg<>-1 
			set @str=@str+'				
				JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 		 		
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  '						
		set @str=@str+'
			group by c.SourceCertificateIdGuid
			union all
			select 0 id1,0 id2,min(c.id) id3,cast(c.FoundedCNEId as uniqueidentifier)
			from ExamCertificateUniqueChecks a
				join CNEWebUICheckLog c with(nolock) on c.FoundedCNEId <> ''Нет свидетельства''	and c.FoundedCNEId=a.idGUID	and a.[Year] between @yearFrom and @yearTo												
					  '
		if @idorg<>-1 
			set @str=@str+
			          '							
				join Account Acc on acc.id=c.AccountId	
				join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
					  '		
		if @TypeCheck <> 'Интерактивная' or @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '						  			
		set @str=@str+
		'			
			group by c.FoundedCNEId
		) t							
					
		select count(distinct idguid) from 
		(
				select a.* from @table a
					join (select min(id)id,idguid from @table group by idguid) b on a.id=b.id
		) t		
		'
		
	end
	print @str
	exec sp_executesql @str,N'@idorg int,@LastName nvarchar(255)=null',@idorg=@idorg,@LastName=@LastName
		
end


