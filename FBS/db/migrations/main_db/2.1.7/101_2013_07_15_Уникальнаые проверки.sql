insert into Migrations(MigrationVersion, MigrationName) values (101, '101_2013_07_15_Уникальнаые проверки.sql')
go
alter proc [dbo].[SelectCNECCheckHystoryByOrgId]
	@idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
	@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max),@ss nvarchar(10),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max),@yearFrom int, @yearTo int
	set @pf=@pf-1

	create table #tt(id [int] NOT NULL primary key)
	create table #t1(id [int] NOT NULL, iscorrect bit not null default 0, primary key (id,iscorrect))
		
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
				select top (@pf) cb.id,case when cb.FoundedCNEId like 'Нет%' then 0 else 1 end
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id asc		
				
				select * from
				(
					select *,row_number() over (order by id asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   ISNULL(c.PassportSeria,'')+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
						    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
						    ) as Marks,c.SourceCertificateIdGuid, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join vw_Examcertificate CNE with(nolock) on c.SourceCertificateIdGuid=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGuid  and cc.[Year] between @yearFrom and @yearTo	
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end
						order by c.Id asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							null as Marks,c.SourceCertificateIdGuid,
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN vw_Examcertificate r with(nolock) ON c.SourceCertificateIdGuid=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.id
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,ISNULL(a.Surname, cb.LastName) LastName, 
								ISNULL(a.Name, cb.FirstName) FirstName, ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
								case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +' ' + cb.PassportNumber
									 else a.DocumentSeries + ' ' + a.DocumentNumber end PassportData, 
								b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
								b.CertificateID SourceCertificateIdGuid
							from 
								#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id															
								left JOIN prn.Certificates AS b with(nolock) on 1=case when cb1.iscorrect=0 then 0 
																					   when cb1.iscorrect=1 and cb.FoundedCNEId=b.CertificateID then 1
																					   else 0
																				  end/*FoundedCNEId=b.CertificateID*/ and b.[UseYear] between @yearFrom and @yearTo
								left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
								left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							where 1 = case when @LastName is null then 1 when a.Surname like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end																
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join prn.CancelledCertificates certificate_deny with (nolock)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid				
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
				select top (@pf) cb.id,case when cb.FoundedCNEId like 'Нет%' then 0 else 1 end
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id desc		
				
				select * from
				(
					select *,row_number() over (order by id desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
						    ) as Marks,c.SourceCertificateIdGuid,
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join vw_Examcertificate CNE with(nolock) on c.SourceCertificateIdGuid=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	
						where @idorg=Acc.OrganizationId 		
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end						
						order by c.Id desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							null as Marks,c.SourceCertificateIdGuid,
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN vw_Examcertificate r with(nolock) ON c.SourceCertificateIdGuid=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed ON c.SourceCertificateYear = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.id	
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,ISNULL(a.Surname, cb.LastName) LastName, 
								ISNULL(a.Name, cb.FirstName) FirstName, ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
								case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +' ' + cb.PassportNumber
									 else a.DocumentSeries + ' ' + a.DocumentNumber end PassportData, 
								b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
								b.CertificateID SourceCertificateIdGuid
							from 
								#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id															
								left JOIN prn.Certificates AS b with(nolock) on 1=case when cb1.iscorrect=0 then 0 
																					   when cb1.iscorrect=1 and cb.FoundedCNEId=b.CertificateID then 1
																					   else 0
																				  end/*FoundedCNEId=b.CertificateID*/ and b.[UseYear] between @yearFrom and @yearTo
								left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
								left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							where 1 = case when @LastName is null then 1 when a.Surname like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join prn.CancelledCertificates certificate_deny with (nolock)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid					
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
				select top (@pf) cb.id,case when cb.FoundedCNEId like 'Нет%' then 0 else 1 end
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id asc		
				
				select * from
				(
					select *,row_number() over (order by TypeCheck asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
						    ) as Marks,cast(c.SourceCertificateIdGuid as nvarchar(255)) SourceCertificateIdGuid,
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join vw_Examcertificate CNE with(nolock) on c.SourceCertificateIdGuid=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.Id asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
							null as Marks,c.SourceCertificateIdGuid, 
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN vw_Examcertificate r with(nolock) ON c.SourceCertificateIdGuid=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo	
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.id
						where @idorg=Acc.OrganizationId 		
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end						
						order by c.Id asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,ISNULL(a.Surname, cb.LastName) LastName, 
								ISNULL(a.Name, cb.FirstName) FirstName, ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
								case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +' ' + cb.PassportNumber
									 else a.DocumentSeries + ' ' + a.DocumentNumber end PassportData, 
								b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
								b.CertificateID SourceCertificateIdGuid
							from 
								#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id															
								left JOIN prn.Certificates AS b with(nolock) on 1=case when cb1.iscorrect=0 then 0 
																					   when cb1.iscorrect=1 and cb.FoundedCNEId=b.CertificateID then 1
																					   else 0
																				  end/*FoundedCNEId=b.CertificateID*/ and b.[UseYear] between @yearFrom and @yearTo
								left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
								left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							where 1 = case when @LastName is null then 1 when a.Surname like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end										
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid			
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
				select top (@pf) cb.id,case when cb.FoundedCNEId like 'Нет%' then 0 else 1 end
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id desc		
				
				select * from
				(
					select *,row_number() over (order by TypeCheck desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
						    ) as Marks,cast(c.SourceCertificateIdGuid as nvarchar(255)) SourceCertificateIdGuid,
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join vw_Examcertificate CNE with(nolock) on c.SourceCertificateIdGuid=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							null as Marks,c.SourceCertificateIdGuid,
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN vw_Examcertificate r with(nolock) ON c.SourceCertificateIdGuid=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.id
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,ISNULL(a.Surname, cb.LastName) LastName, 
								ISNULL(a.Name, cb.FirstName) FirstName, ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
								case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +' ' + cb.PassportNumber
									 else a.DocumentSeries + ' ' + a.DocumentNumber end PassportData, 
								b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
								b.CertificateID SourceCertificateIdGuid
							from 
								#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id															
								left JOIN prn.Certificates AS b with(nolock) on 1=case when cb1.iscorrect=0 then 0 
																					   when cb1.iscorrect=1 and cb.FoundedCNEId=b.CertificateID then 1
																					   else 0
																				  end/*FoundedCNEId=b.CertificateID*/ and b.[UseYear] between @yearFrom and @yearTo
								left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
								left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							where 1 = case when @LastName is null then 1 when a.Surname like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid					
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
				select top (@pf) cb.id,case when cb.FoundedCNEId like 'Нет%' then 0 else 1 end
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.LastName asc		
				
				select * from
				(
					select *,row_number() over (order by LastName asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
						    ) as Marks,cast(c.SourceCertificateIdGuid as nvarchar(255)) SourceCertificateIdGuid,
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join vw_Examcertificate CNE with(nolock) on c.SourceCertificateIdGuid=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							null as Marks,c.SourceCertificateIdGuid,
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN vw_Examcertificate r with(nolock) ON c.SourceCertificateIdGuid=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo	
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.id
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,ISNULL(a.Surname, cb.LastName) LastName, 
								ISNULL(a.Name, cb.FirstName) FirstName, ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
								case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +' ' + cb.PassportNumber
									 else a.DocumentSeries + ' ' + a.DocumentNumber end PassportData, 
								b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
								b.CertificateID SourceCertificateIdGuid
							from 
								#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id															
								left JOIN prn.Certificates AS b with(nolock) on 1=case when cb1.iscorrect=0 then 0 
																					   when cb1.iscorrect=1 and cb.FoundedCNEId=b.CertificateID then 1
																					   else 0
																				  end/*FoundedCNEId=b.CertificateID*/ and b.[UseYear] between @yearFrom and @yearTo
								left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
								left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							where 1 = case when @LastName is null then 1 when a.Surname like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[USeYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid				
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
				select top (@pf) cb.id,case when cb.FoundedCNEId like 'Нет%' then 0 else 1 end
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.LastName desc		
				
				select * from
				(
					select *,row_number() over (order by LastName desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
						    ) as Marks,cast(c.SourceCertificateIdGuid as nvarchar(255)) SourceCertificateIdGuid,
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join vw_Examcertificate CNE with(nolock) on c.SourceCertificateIdGuid=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							null as Marks,c.SourceCertificateIdGuid,
							case WHEN c.SourceCertificateId IS NULL THEN 'Не найдено' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
								else 'Истек срок' end end  STATUS
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN vw_Examcertificate r with(nolock) ON c.SourceCertificateIdGuid=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.id
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.LastName desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.CertificateFK is not null then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,ISNULL(a.Surname, cb.LastName) LastName, 
								ISNULL(a.Name, cb.FirstName) FirstName, ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
								case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +' ' + cb.PassportNumber
									 else a.DocumentSeries + ' ' + a.DocumentNumber end PassportData, 
								b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
								b.CertificateID SourceCertificateIdGuid
							from 
								#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id															
								left JOIN prn.Certificates AS b with(nolock) on 1=case when cb1.iscorrect=0 then 0 
																					   when cb1.iscorrect=1 and cb.FoundedCNEId=b.CertificateID then 1
																					   else 0
																				  end/*FoundedCNEId=b.CertificateID*/ and b.[UseYear] between @yearFrom and @yearTo
								left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.[UseYear] between @yearFrom and @yearTo
								left join ExamcertificateUniqueChecks CC on b.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
							where 1 = case when @LastName is null then 1 when a.Surname like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid				
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
					
				select * into #ttt
				from 
				(						
					select c.Id,cb.CreateDate,''Пакетная'' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
					   ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
					    ( SELECT ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + ''=''
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      '','', ''.'') + '','' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubjectCheck s
                                  WHERE     s.CheckId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''''),
                                      TYPE
                                ) marks
						    ) as Marks,cast(c.SourceCertificateIdGuid as nvarchar(255)) SourceCertificateIdGuid,
					   case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.CertificateFK is not null then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					FROM 
						(select min(c.id) id,c.CertificateNumber 
						 from CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
							join ExamCertificateUniqueChecks a on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo							
							'
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
						left JOIN prn.Certificates CNE with(nolock) ON c.SourceCertificateIdGuid=CNE.CertificateID			
						join ExamcertificateUniqueChecks CC on CNE.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
						left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
						left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = c.SourceCertificateIdGuid						
					union all
					select c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,r.LicenseNumber CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						null as Marks,c.SourceCertificateIdGuid,
						case WHEN c.SourceCertificateId IS NULL THEN ''Не найдено'' else
								case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
								else ''Истек срок'' end end  STATUS
					FROM 
						(select min(c.id) id, r.LicenseNumber 
						from CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
							join ExamCertificateUniqueChecks a on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
							left JOIN prn.Certificates r with(nolock) ON c.SourceCertificateIdGuid=r.CertificateID'
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
						group by r.LicenseNumber
						) cb1
						
						join CommonNationalExamCertificateRequest c on cb1.id=c.id   
						JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id	
						left JOIN prn.Certificates r with(nolock) ON c.SourceCertificateIdGuid=r.CertificateID
						left join rbd.Participants rr with(nolock) ON r.ParticipantFK=rr.ParticipantID
						join ExamcertificateUniqueChecks CC on r.CertificateID=cc.idGUID and cc.[Year] between @yearFrom and @yearTo				
						left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
						left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = r.CertificateID	'
		set @str=@str+'		
									
					union all
					select t.*,
						case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.CertificateFK is not null then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					from 
					(
						select cb.id,cb.EventDate,''Интерактивная'' TypeCheck, 
								ISNULL(c.LicenseNumber, cb.CNENumber) CertificateNumber, 
								ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
								ISNULL(rr.Surname, cb.LastName) LastName, 
								ISNULL(rr.Name, cb.FirstName) FirstName, ISNULL(rr.SecondName, cb.PatronymicName) PatronymicName,
							    case when rr.DocumentSeries is null and rr.DocumentNumber is null then cb.PassportSeria +'' ''+ cb.PassportNumber else rr.DocumentSeries+'' ''+rr.DocumentNumber end PassportData, 
							    c.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks,
							    cast(c.CertificateID as nvarchar(255)) SourceCertificateIdGuid
						from 
							ExamCertificateUniqueChecks CC
							join prn.Certificates AS c with(nolock) ON CC.idGUID=c.CertificateID
									and c.[UseYear] between @yearFrom and @yearTo and cc.[Year] between @yearFrom and @yearTo
							join CNEWebUICheckLog cb on cb.FoundedCNEId=cast(c.CertificateID as nvarchar(255))									
							join Account Acc on acc.id=cb.AccountId	
							join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 '	
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
						and 1=0 '	
		set @str=@str +'								
							left join rbd.Participants rr with(nolock) ON c.ParticipantFK=rr.ParticipantID and c.[UseYear] between @yearFrom and @yearTo'								
		set @str=@str+'						
							where 1=1
							'
		if @LastName is not null
			set @str=@str+'
								and rr.Surname like ''%''+@LastName+''%'''
		if @idorg<>-1	
			set @str=@str+'
								and @idorg=Acc.OrganizationId '										
							
		set @str=@str+'							
									
					) t
						left join [ExpireDate] as ed on ed.[Year] = t.[Year]
						left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[UseYear] between @yearFrom and @yearTo
									and certificate_deny.CertificateFK = t.SourceCertificateIdGuid	
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
		order by rn
		
		drop table #ttt	'
		
	end
	
	print  @str
	
	exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255)',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName
	
	drop table #tt
	drop table #t1	
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
		set @str='						
				declare @yearFrom int, @yearTo int		
				select @yearFrom = 2008, @yearTo = Year(GetDate())				
				
				select * into #ttt
				from 
				(						
					select  c.Id,c.CertificateNumber
					FROM 
						(select min(c.id) id,c.CertificateNumber 
						 from CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							join ExamCertificateUniqueChecks a on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo	 '
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
					union all
					select c.Id,cb1.Number CertificateNumber
					FROM 
						(select min(c.id) id, r.Number 
						from CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
							join ExamCertificateUniqueChecks a on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo	
							left JOIN vw_Examcertificate r with(nolock) ON c.ParticipantID=r.ParticipantID '
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
						JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id'
		set @str=@str+'	
					union all
					select t.*
					from 
					(
						select cb.id,ISNULL(c.LicenseNumber, cb.CNENumber) CertificateNumber
						from							 
							ExamCertificateUniqueChecks CC
							join prn.Certificates AS c with(nolock) ON CC.idGUID=c.CertificateID
									and c.[UseYear] between @yearFrom and @yearTo and cc.[Year] between @yearFrom and @yearTo
							join CNEWebUICheckLog cb on cb.FoundedCNEId=cast(c.CertificateID as nvarchar(255))	'
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
						and 1=0 '	
		if @idorg<>-1	
			set @str=@str+'
							join Account Acc on acc.id=cb.AccountId	
							join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 and @idorg=Acc.OrganizationId '							
		if @LastName is not null
			set @str=@str+'
							where a.Surname like ''%''+@LastName+''%'''
		set @str=@str+'							
					) t
					) t
					
		select count(distinct CertificateNumber) from 
		(
				select a.* from #ttt a
					join (select min(id)id,CertificateNumber from #ttt group by CertificateNumber) b on a.id=b.id
		) t		
		
		drop table #ttt '
		
	end
	print @str
	exec sp_executesql @str,N'@idorg int,@LastName nvarchar(255)=null',@idorg=@idorg,@LastName=@LastName
		
end


