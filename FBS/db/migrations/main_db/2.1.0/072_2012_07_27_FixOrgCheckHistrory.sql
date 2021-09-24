-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (72, '072_2012_07_27_NoLockProc.sql')
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
						   ISNULL(c.PassportSeria,'')+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
							ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
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
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
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
						   ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed ON c.SourceCertificateYear = ed.[Year] 
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
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		 	   
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
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
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
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		 
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
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
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
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		 
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
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
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
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		 		   
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
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
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
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+ ' '+c.PassportNumber, cb.PassportSeria + ' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		 	   
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
					   ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
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
						ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						case WHEN c.SourceCertificateId IS NULL THEN ''Не найдено'' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
								else ''Истек срок'' end end  STATUS
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
						left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
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
						select top (@pf) cb.id,cb.EventDate,''Интерактивная'' TypeCheck, ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
							   ISNULL(c.PassportSeria+'' ''+c.PassportNumber, cb.PassportSeria +'' ''+ cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
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

