alter proc [dbo].[SelectCNECCheckHystoryByOrgIdCount]
	@idorg int, @isUnique bit=0,@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null, @dats datetime=null,@datf datetime=null
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
					'
		if @dats is not null 
			set @str=@str+' and cb.CreateDate >= @dats'
		if @datf is not null 
			set @str=@str+' and cb.CreateDate <= @datf'
								
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '	
		if @idorg<>-1	
			set @str=@str+
		'						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg			
		'													
		set @str=@str+'	
				union all
				select 1 t 
				FROM CommonNationalExamCertificateRequest c 
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
		'
		if @dats is not null 
			set @str=@str+' and cb.CreateDate >= @dats'
		if @datf is not null 
			set @str=@str+' and cb.CreateDate <= @datf'
								
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '							
		if @idorg<>-1	
			set @str=@str+
		'						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg		
		'
		set @str=@str+'
				union all
				select 1 t
				from CNEWebUICheckLog cb '
		if @idorg<>-1	
			set @str=@str+
		'
				join Account acc with(nolock) on acc.id=cb.AccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg
		'					
		if @dats is not null 
			set @str=@str+' and cb.EventDate >= @dats'
		if @datf is not null 
			set @str=@str+' and cb.EventDate <= @datf'									
				
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '

		if @LastName is not null
			set @str=@str+'
			    left join prn.Certificates AS b with(nolock) on cb.FoundedCNEId <> ''Нет свидетельства'' and cb.FoundedCNEId=b.CertificateID
			    left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID
				where a.Surname like ''%''+@LastName+''%'' '							
																
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
			select min(id1) id1,idtype id3,SourceCertificateIdGuid'+
				case when  @dats is not null or @datf is not null then ',min(CreateDate) CreateDate'
					 else '' 
				end
		  +' from 
				(				
				select min(c.id) id1, 1 idtype,c.SourceCertificateIdGuid'+
				case when @dats is not null or @datf is not null then ',min(cb.CreateDate) CreateDate'
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
				group by c.SourceCertificateIdGuid
				union all
				select min(c.id) id2, 2 idtype,c.SourceCertificateIdGuid'+
				case when @dats is not null or @datf is not null then ',min(cb.CreateDate) CreateDate'
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
				group by c.SourceCertificateIdGuid
				union all
				select min(c.id) id1, 3 idtype,cast(c.FoundedCNEId as uniqueidentifier)'+
				case when @dats is not null or @datf is not null then ',min(c.EventDate) CreateDate '
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
				group by c.FoundedCNEId
			) c		
			group by SourceCertificateIdGuid,idtype
		) c 
		where 1=1
		'
		if @dats is not null 
			set @str=@str+' and c.CreateDate >= @dats'
		if @datf is not null 
			set @str=@str+' and c.CreateDate <= @datf'	
			
		set @str=@str+'	
		select count(distinct idguid) from 
		(
				select a.* from @table a
					join (select min(id)id,idguid from @table group by idguid) b on a.id=b.id
		) t		
		'
		
	end
	print @str
	exec sp_executesql @str,N'@idorg int,@LastName nvarchar(255)=null,@dats datetime,@datf datetime',@idorg=@idorg,@LastName=@LastName,@dats=@dats,@datf=@datf
		
end


