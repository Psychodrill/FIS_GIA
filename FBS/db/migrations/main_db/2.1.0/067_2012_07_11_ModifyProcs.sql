-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (67, '067_2012_07_11_ModifyProcs.sql')
-- =========================================================================
GO

alter proc [dbo].[SelectCNECCheckHystoryByOrgId]
	@idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
	@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max),@ss nvarchar(10),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max),@yearFrom int, @yearTo int

	create table #tt(id [int] NOT NULL primary key)
	create table #t1(id [int] NOT NULL primary key)
		
	select @yearFrom = 2008, @yearTo = Year(GetDate())
				
	if @isUnique =0 
	begin
		if @fld='id'	
		begin
			if @so = 0 
			begin
				insert #tt
				select c.id 
				from Account c
					join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
				where d.id=@idorg and d.DisableLog=0
				
				insert #t1
				select top (@pf) cb.id 
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id asc		
				
				select * from
				(
					select *,row_number() over (order by id asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end
						order by c.Id asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id	
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end															
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end
			else	
			begin	
				insert #tt
				select c.id 
				from Account c
					join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
				where d.id=@idorg and d.DisableLog=0
			
				insert #t1
				select top (@pf) cb.id
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id desc		
				
				select * from
				(
					select *,row_number() over (order by id desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 		
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end						
						order by c.Id desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf			
			end	
		end
		if @fld='TypeCheck'
		begin
			if @so = 0 
			begin
				insert #tt
				select c.id 
				from Account c
					join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
				where d.id=@idorg and d.DisableLog=0
				
				insert #t1
				select top (@pf) cb.id
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id asc		
				
				select * from
				(
					select *,row_number() over (order by TypeCheck asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.Id asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 		
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end						
						order by c.Id asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end
			else	
			begin	
				insert #tt
				select c.id 
				from Account c
					join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
				where d.id=@idorg and d.DisableLog=0
			
				insert #t1
				select top (@pf) cb.id 
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id desc		
				
				select * from
				(
					select *,row_number() over (order by TypeCheck desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf			
			end		
		end			
		if @fld='LastName'	
		begin
			if @so = 0 
			begin
				insert #tt
				select c.id 
				from Account c
					join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
				where d.id=@idorg and d.DisableLog=0
				
				insert #t1
				select top (@pf) cb.id
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.LastName asc		
				
				select * from
				(
					select *,row_number() over (order by LastName asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end		
			else
			begin
				insert #tt
				select c.id 
				from Account c
					join Organization2010 d on d.id=c.OrganizationId and d.DisableLog=0
				where d.id=@idorg and d.DisableLog=0
				
				insert #t1
				select top (@pf) cb.id 
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.LastName desc		
				
				select * from
				(
					select *,row_number() over (order by LastName desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.LastName desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end	
		end					
	end					
	else
	begin
		if @fld='id'	
		begin
			set @fldso1='c.Id'
			set @fldso2='c.Id'		
			set @fldso3='cb.Id'				
		end	
		if @fld='TypeCheck'
		begin
			set @fldso1='c.Id'
			set @fldso2='c.Id'
			set @fldso3='cb.Id'				
		end			
	
		if @fld='LastName'	
		begin
			set @fldso1='c.LastName'
			set @fldso2='c.LastName'
			set @fldso3='c.LastName'				
		end			
			
		if @so=1 
			set @ss=' desc'
		else
			set @ss=' asc'	
	
		set @str='
				declare @yearFrom int, @yearTo int		
				select @yearFrom = 2008, @yearTo = Year(GetDate())
				
				select top (@pf) * into #ttt
				from 
				(						
					select top (@pf) c.Id,cb.CreateDate,''Пакетная'' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
					   c.PassportSeria+'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
					   case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.Id>0 then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					FROM 
						(select min(c.id) id,c.CertificateNumber 
						 from CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id '
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '														
		if @idorg<>-1 
			set @str=@str+'						
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
		if @LastName is not null
			set @str=@str+'
							where c.LastName like ''%''+@LastName+''%'' '			
		set @str=@str+'								
						group by CertificateNumber) cb1	
															
						join CommonNationalExamCertificateCheck c on cb1.id=c.id 															
						JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 								
						left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id			
						left join ExamcertificateUniqueChecks CC on CNE.id=cc.id
						left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
						left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
							on certificate_deny.[Year] between @yearFrom and @yearTo
								and certificate_deny.certificateNumber = c.CertificateNumber 	

					order by '+@fldso1+@ss + '
					union all
					select top (@pf) c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						c.PassportSeria+'' ''+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.Id>0 then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					FROM 
						(select min(c.id) id, r.Number 
						from CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id '
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+' '					
		else
				set @str=@str+'						
							and 1=0 '								
		if @idorg<>-1 
			set @str=@str+'								
				  			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
		if @LastName is not null
			set @str=@str+'
							where c.LastName like ''%''+@LastName+''%'''						  			
		set @str=@str+'					  		
						group by r.Number
						) cb1
						
						join CommonNationalExamCertificateRequest c on cb1.id=c.id   
						JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id						 
						left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
						left join ExamcertificateUniqueChecks CC on r.id=cc.id				
						left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
						left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
							on certificate_deny.[Year] between @yearFrom and @yearTo
								and certificate_deny.certificateNumber = r.Number '
		set @str=@str+'		
					order by '+@fldso1+@ss + '				
					union all
					select t.*,
						case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.Id>0 then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					from 
					(
						select top (@pf) cb.id,cb.EventDate,''Интерактивная'' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+'' ''+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
						from 
						(
							select max(cb.id) id,c.Number from
							(
								select cb.id,cb.FoundedCNEId
								from CNEWebUICheckLog cb  with(index(I_CNEWebUICheckLog_AccId))
									join Account Acc with(index(accOrgIdIndex)) on acc.id=cb.AccountId	
									join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 '	
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '										
		if @idorg<>-1	
			set @str=@str+'
								where @idorg=Acc.OrganizationId '								
		set @str=@str+'		
							) cb
							left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 						
							group by c.Number	
						) cb1
							join CNEWebUICheckLog cb on cb1.id=cb.id
							left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id '								
		set @str=@str+'								
							left join ExamcertificateUniqueChecks CC on c.id=cc.id '
		if @LastName is not null
			set @str=@str+'
							where c.LastName like ''%''+@LastName+''%'''
		set @str=@str+'							
						order by '+@fldso1+@ss + '				
					) t
						left join [ExpireDate] as ed on ed.[Year] = t.[Year]
						left join CommonNationalExamcertificateDeny certificate_deny 
							on certificate_deny.[Year] between @yearFrom and @yearTo
								and certificate_deny.certificateNumber = t.CertificateNumber 				
					) t
					
		select * from
		(
			select *,row_number() over (order by '+@fld+@ss+') rn from
				(
				select a.* from #ttt a
					join (select min(id)id,CertificateNumber from #ttt group by CertificateNumber) b on a.id=b.id
				) t
		) t
		where rn between @ps and @pf
		
		drop table #ttt	'
		
	end
	
	print  @str
	
	exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255)',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName
	
	drop table #tt
	drop table #t1	
end

GO

-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

alter proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
	  @number nvarchar(255) = null				-- номер сертификата	
	, @checkSubjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
	, @xml xml out
as
begin 
	
	if @number is null
	begin
		RAISERROR (N'Номер св-ва не указан',10,1);
		return
	end
    
	declare 
		@commandText nvarchar(max)
		, @declareCommandText nvarchar(max)
		, @selectCommandText nvarchar(max)
		, @baseName nvarchar(255)
		, @yearFrom int
		, @yearTo int
		, @accountId bigint
        , @organizationId bigint
    	, @CId bigint
		, @eventCode nvarchar(255)
		, @eventParams nvarchar(4000)
		, @sourceEntityIds nvarchar(4000) 
	
	declare @check_subject table
	(
	SubjectId int
	, Mark nvarchar(10)
	)
	
	declare @certificate_check table
	(
	Number nvarchar(255)
	, IsExist bit
	, certificateId bigint
	, IsDeny bit
	, DenyComment ntext
	, DenyNewcertificateNumber nvarchar(255)
	, [Year] int
	, PassportSeria nvarchar(255)
	, PassportNumber nvarchar(255)
	, RegionId int
	, RegionName nvarchar(255)
	, TypographicNumber nvarchar(255)
	)

	-- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

	select @yearFrom = 2008, @yearTo = Year(GetDate())

	select
		@accountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	declare @sql nvarchar(max)
	
	set @sql = '
	select
		[certificate].Number 
		, case
			when [certificate].Id>0 then 1
			else 0
		end IsExist
		, [certificate].Id
		, case
			when certificate_deny.Id>0 then 1
			else 0
		end iscertificate_deny
		, certificate_deny.Comment
		, certificate_deny.NewcertificateNumber
		, [certificate].[Year]
		, [certificate].PassportSeria
		, [certificate].PassportNumber
		, [certificate].RegionId
		, region.Name
		, [certificate].TypographicNumber
	from 
		(select null ''empty'') t left join 
		dbo.CommonNationalExamcertificate [certificate] with (nolock, fastfirstrow) on 
				[certificate].[Year] between @yearFrom and @yearTo '
	
    set @sql = @sql + '	and [certificate].Number = @number'
	set @sql = @sql + '			
		left outer join dbo.Region region
			on region.Id = [certificate].RegionId
		left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
			on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.certificateNumber = [certificate].Number'

	insert into @certificate_check 	
	exec sp_executesql @sql,N'@number nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int',@number = @number,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo
	
	--SELECT * FROM @certificate_check
	
	set @eventParams = 
		isnull(@number, '') + '||||' +
		isnull(@checkSubjectMarks, '') + '|' 

	set @sourceEntityIds = '' 
	select 
		@sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(100), certificate_check.certificateId) 
	from 
		@certificate_check certificate_check 
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	if @sourceEntityIds = '' 
		set @sourceEntityIds = null 


	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        @certificate_check S
    where
    	S.certificateId is not null
		
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @certificateId = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
	-------------------------------------------------------------
	
			
	select
		certificate_check.certificateId
		,certificate_check.Number Number
		, certificate_check.IsExist IsExist
		, check_subject.Id  SubjectId
		, check_subject.Name  SubjectName
		, case when check_subject.CheckSubjectMark < check_subject.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',')  CheckSubjectMark
		, case when check_subject.SubjectMark < check_subject.MinimalMark1 then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',')  SubjectMark
		,check_subject.SubjectMarkIsCorrect SubjectMarkIsCorrect
		, check_subject.HasAppeal HasAppeal
		, certificate_check.IsDeny IsDeny
		, certificate_check.DenyComment DenyComment
		, certificate_check.DenyNewcertificateNumber DenyNewcertificateNumber
		, certificate_check.PassportSeria PassportSeria
		, certificate_check.PassportNumber PassportNumber
		, certificate_check.RegionId RegionId
		, certificate_check.RegionName RegionName
		, certificate_check.[Year] [Year]
		, certificate_check.TypographicNumber TypographicNumber
		, case when ed.[ExpireDate] is null then 'Не найдено' else 
			case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
			case when getdate() <= ed.[ExpireDate] then 'Действительно'
			else 'Истек срок' end end end  as [Status],
        CC.UniqueChecks UniqueChecks,
        CC.UniqueIHEaFCheck UniqueIHEaFCheck,
        CC.UniqueIHECheck UniqueIHECheck,
        CC.UniqueIHEFCheck UniqueIHEFCheck,
        CC.UniqueTSSaFCheck UniqueTSSaFCheck,
        CC.UniqueTSSCheck UniqueTSSCheck,
        CC.UniqueTSSFCheck UniqueTSSFCheck,
        CC.UniqueRCOICheck UniqueRCOICheck,
        CC.UniqueOUOCheck UniqueOUOCheck,
        CC.UniqueFounderCheck UniqueFounderCheck,
        CC.UniqueOtherCheck UniqueOtherCheck
        into #table
	from @certificate_check certificate_check
    	inner join CommonNationalExamcertificate C on C.Id = certificate_check.certificateId
        left outer join ExamcertificateUniqueChecks CC on CC.Id = C.Id
		left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]					
		left outer join (
			select			
				getcheck_subject.SubjectId id,[subject].Name,
				certificate_subject.[Year],certificate_subject.certificateId,	
				isnull(getcheck_subject.SubjectId, certificate_subject.SubjectId) SubjectId
				, getcheck_subject.[Mark] CheckSubjectMark
				, certificate_subject.[Mark] SubjectMark
				, case
					when getcheck_subject.Mark = certificate_subject.Mark then 1
					else 0
				end SubjectMarkIsCorrect
				, certificate_subject.HasAppeal
				,mm.[MinimalMark]
				,mm1.[MinimalMark] MinimalMark1
			from 
			CommonNationalExamcertificateSubject certificate_subject with (nolock)
			join dbo.[Subject] [subject]	on [subject].Id = certificate_subject.SubjectId		
			left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject	on getcheck_subject.SubjectId = certificate_subject.SubjectId			
			left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.[Year] = mm.[Year] 
			left join [MinimalMark] as mm1 on certificate_subject.SubjectId = mm1.[SubjectId] and certificate_subject.[Year] = mm1.[Year] 
			) check_subject
			on certificate_check.[Year] = check_subject.[Year] and certificate_check.certificateId = check_subject.certificateId
			
			--select * from #table
			
IF @shouldCheckMarks = 1 AND  (exists(select * from #table where  SubjectMarkIsCorrect=0 and SubjectId IS NOT null) or (select COUNT(*) from #table where SubjectId IS NOT null)<>(select COUNT(*) from dbo.GetSubjectMarks(@checkSubjectMarks)))
	delete from #table

	select @xml=(
	select 
	(
	select * from #table
	for xml path('check'), ELEMENTS XSINIL,type
	) 
	for xml path('root'),type
	)
	drop table #table
goto result	
nullresult:
	select @xml=(
	select null 
	for xml path('root'),type
	)
result:

		set @eventCode = 'CNE_CHK'
		exec dbo.RegisterEvent 
			@accountId
			, @ip
			, @eventCode
			, @sourceEntityIds
			, @eventParams
			, @updateId = null
	
	return 0
end

GO