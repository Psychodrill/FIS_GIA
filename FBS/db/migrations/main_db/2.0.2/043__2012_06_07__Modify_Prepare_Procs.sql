SET CONCAT_NULL_YIELDS_NULL, --Управляет представлением результатов объединения в виде значений NULL или пустых строковых значений.
ANSI_NULLS, --Задает совместимое со стандартом ISO поведение операторов сравнения «равно» (=) и «не равно» (<>) при использовании со значениями NULL.
ANSI_PADDING, --Контролирует способ хранения в столбце значений короче, чем определенный размер столбца, и способ хранения в столбце значений, имеющих замыкающие пробелы, в данных char, varchar, binary и varbinary.
QUOTED_IDENTIFIER, --Заставляет SQL Server следовать правилам ISO относительно разделения кавычками идентификаторов и строк-литералов. Идентификаторы, заключенные в двойные кавычки, могут быть либо зарезервированными ключевыми словами Transact-SQL, либо могут содержать символы, которые обычно запрещены правилами синтаксиса для идентификаторов Transact-SQL.
ANSI_WARNINGS, --Задает поведение в соответствии со стандартом ISO для некоторых условий ошибок.
ARITHABORT, --Завершает запрос, если во время его выполнения возникла ошибка переполнения или деления на ноль.
XACT_ABORT ON --Указывает, выполняет ли SQL Server автоматический откат текущей транзакции, если инструкция языка Transact-SQL вызывает ошибку выполнения.
SET NUMERIC_ROUNDABORT, --Задает уровень отчетов об ошибках, создаваемых в тех случаях, когда округление в выражении приводит к потере точности.
IMPLICIT_TRANSACTIONS OFF --Устанавливает для соединения режим неявных транзакций.
GO

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
/*
SERIALIZABLE
Указывает следующее.
Инструкции не могут считывать данные, которые были изменены другими транзакциями, но еще не были зафиксированы.
Другие транзакции не могут изменять данные, считываемые текущей транзакцией, до ее завершения.
Другие транзакции не могут вставлять новые строки со значениями ключа, которые входят в диапазон ключей, считываемых инструкциями текущей транзакции, до ее завершения.

Блокировка диапазона устанавливается в диапазоне значений ключа, соответствующих условиям поиска любой инструкции, выполненной во время транзакции. 
Обновление и вставка строк, удовлетворяющих инструкциям текущей транзакции, блокируется для других транзакций. Это гарантирует, что если какая-либо инструкция транзакции выполняется повторно, 
она будет считывать тот же самый набор строк. Блокировки диапазона сохраняются до завершения транзакции. Это самый строгий уровень изоляции, поскольку он блокирует целые диапазоны ключей и 
сохраняет блокировку до завершения транзакции. Из-за низкого параллелизма этот параметр рекомендуется использовать только при необходимости. Этот параметр действует так же, 
как и настройка HOLDLOCK всех таблиц во всех инструкциях SELECT в транзакции.
*/
GO
BEGIN TRANSACTION
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateCheck]') AND name = N'IX_CommonNationalExamCertificateCheck_LastName_OtherFields2')
DROP INDEX [IX_CommonNationalExamCertificateCheck_LastName_OtherFields2] ON [dbo].[CommonNationalExamCertificateCheck] WITH ( ONLINE = OFF )
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificateCheck_LastName_OtherFields2]
ON [dbo].[CommonNationalExamCertificateCheck] ([LastName])
INCLUDE ([Id],[BatchId],[CertificateNumber])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateRequest]') AND name = N'IX_CommonNationalExamCertificateRequest_LastName_OtherFields')
DROP INDEX [IX_CommonNationalExamCertificateRequest_LastName_OtherFields] ON [dbo].[CommonNationalExamCertificateRequest] WITH ( ONLINE = OFF )
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificateRequest_LastName_OtherFields]
ON [dbo].[CommonNationalExamCertificateRequest] ([LastName])
INCLUDE ([Id],[BatchId],[SourceCertificateId])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

if  exists (select * from sys.objects where object_id = object_id(N'[dbo].[GetNEWebUICheckLog]') and type in (N'p', N'pc'))
drop procedure [dbo].[GetNEWebUICheckLog]
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

create proc [dbo].[GetNEWebUICheckLog]
	@login nvarchar(255), @startRowIndex int = 1,@maxRowCount int = null, @showCount bit = null,   -- если > 0, то выбирается общее кол-во
	@TypeCode nvarchar(255) -- Тип проверки
as
begin
	declare @sortAsc bit, @accountId bigint, @endRowIndex integer
	set @sortAsc=0

	if isnull(@maxRowCount, -1) = -1 
		set @endRowIndex = 10000000
	else
		set @endRowIndex = @startRowIndex + @maxRowCount

	if exists ( select 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
				where a2.[Login] = @login )
		set @accountId = null
	else 
		set @accountId = isnull(
			(select account.[Id] 
			from dbo.Account account with (nolock, fastfirstrow) 
			where account.[Login] = @login), 0)

	if isnull(@showCount, 0) = 0
	begin	
		if @sortAsc = 1
			if @accountId is null 
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber, 
							2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
							c.[login] [login],EventDate,
						row_number() over (order by b.EventDate asc) rn 
					from 
					(
						select top (@endRowIndex) b.id 
						from dbo.CNEWebUICheckLog b with (nolock) 
							join Account c on b.AccountId=c.id
							join Organization2010 d on d.id=c.OrganizationId
						where @TypeCode=TypeCode and d.DisableLog=0
						order by b.id
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id
				) s 
				where s.rn between @startRowIndex and @endRowIndex
			else
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber, 
							2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
							c.[login] [login],EventDate,
						row_number() over (order by b.EventDate asc) rn 
					from 
					(
						select top (@endRowIndex) b.id 
						from dbo.CNEWebUICheckLog b with (nolock) 
							join Account c on b.AccountId=c.id
							join Organization2010 d on d.id=c.OrganizationId						
						where b.AccountId = @accountId and @TypeCode=TypeCode and d.DisableLog=0
						order by b.id
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id
				) s 
				where s.rn between @startRowIndex and @endRowIndex			
		else
			if @accountId is null 
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber,
						2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
						c.[login] [login],EventDate,		
						row_number() over (order by b.EventDate desc) rn 
					from
					(
						select top (@endRowIndex) b.id 
						from dbo.CNEWebUICheckLog b with (nolock) 
							join Account c on b.AccountId=c.id
							join Organization2010 d on d.id=c.OrganizationId
						where @TypeCode=TypeCode and d.DisableLog=0												
						order by b.EventDate desc					
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id								
				) s 
				where s.rn between @startRowIndex and @endRowIndex				
			else
				select *
				from 
				(
					select top (@endRowIndex) b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber,
						2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
						c.[login] [login],EventDate,		
						row_number() over (order by b.EventDate desc) rn 
					from
					(
						select top (@endRowIndex) b.id 
						from dbo.CNEWebUICheckLog b with (nolock) 
							join Account c on b.AccountId=c.id
							join Organization2010 d on d.id=c.OrganizationId						
						where b.AccountId = @accountId and @TypeCode=TypeCode and d.DisableLog=0						
						order by b.id						
					) a join CNEWebUICheckLog b on a.id=b.id	
						join Account c on b.AccountId=c.id
					order by b.Id				
				) s 
				where s.rn between @startRowIndex and @endRowIndex							
	end
	else
		if @accountId is null 
			select count(*) 
			from dbo.CNEWebUICheckLog b with (nolock) 
				join Account c on b.AccountId=c.id
				join Organization2010 d on d.id=c.OrganizationId	
			where @TypeCode=TypeCode and d.DisableLog=0	
		else
			select count(*) 
			from dbo.CNEWebUICheckLog b with (nolock) 
				join Account c on b.AccountId=c.id
				join Organization2010 d on d.id=c.OrganizationId	
			where b.AccountId = @accountId and @TypeCode=TypeCode and d.DisableLog=0	

	return 0
end

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO


if  exists (select * from sys.objects where object_id = object_id(N'[dbo].[SearchCommonNationalExamCertificateCheckHistory]') and type in (N'p', N'pc'))
drop procedure [dbo].[SearchCommonNationalExamCertificateCheckHistory]
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- получить все уникальные проверки сертификата вузами и их филиалами
CREATE proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
	@certificateId BIGINT,  -- id сертификата
	@startRow INT = NULL,	-- пейджинг, если null - то выбирается кол-во записей для этого сертификата всего
	@maxRow INT = NULL		-- пейджинг
AS
BEGIN
-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
	SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg 
				INNER JOIN dbo.CommonNationalExamCertificate c ON c.Number = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0
	RETURN
END
SELECT   @certificateId AS CertificateId, * FROM (
			select org.Id AS OrganizationId,
			org.FullName AS OrganizationFullName,
			lg.[Date] AS [Date],
			lg.IsBatch AS CheckType,
			DENSE_RANK() OVER(ORDER BY org.FullName) AS org
			FROM dbo.CheckCommonNationalExamCertificateLog lg INNER JOIN dbo.CommonNationalExamCertificate c ON c.Number = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0) rowTable 
			WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow 
			ORDER BY org, rowTable.[Date] 
END
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

if  exists (select * from sys.objects where object_id = object_id(N'[dbo].[SelectCNECCheckHystoryByOrgId]') and type in (N'p', N'pc'))
drop procedure [dbo].[SelectCNECCheckHystoryByOrgId]
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

create proc [dbo].[SelectCNECCheckHystoryByOrgId]
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
							join CNEWebUICheckLog cb with(index(CNEWebUICheckLog_Id)) on cb1.id=cb.id
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


IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO


if  exists (select * from sys.objects where object_id = object_id(N'[dbo].[SelectCNECCheckHystoryByOrgIdCount]') and type in (N'p', N'pc'))
drop procedure [dbo].[SelectCNECCheckHystoryByOrgIdCount]
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

create proc [dbo].[SelectCNECCheckHystoryByOrgIdCount]
	@idorg int, @isUnique bit=0,@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max)
	
	if @isUnique =0 
	begin
		set @str='
			create table #tt(id [int] NOT NULL primary key)
			create table #t1(id [int] NOT NULL primary key)
			
			insert #tt
			select c.id 
			from Account c
				join Organization2010 d on d.id=c.OrganizationId '
		if @idorg<>-1 
			set @str=@str+'
				where d.id=@idorg and d.DisableLog=0 '
				
		set @str=@str+'		
			insert #t1
			select cb.id 
			from CNEWebUICheckLog cb
				join #tt Acc on acc.id=cb.AccountId 		
			order by cb.Id asc			
		
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
					left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
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
				from #t1 cb1
					join CNEWebUICheckLog cb on cb1.id=cb.id '
					
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '
		set @str=@str+'													
					left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id  where 1 = 1 '	
		if @LastName is not null
			set @str=@str+'
				and c.LastName like ''%''+@LastName+''%'' '												
		set @str=@str+'				
				) t
		drop table #tt
		drop table #t1				
				'
	end
	else
	begin
		set @str='select count(distinct t) from
				(
				select distinct c.CertificateNumber t
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
				select distinct r.Number t 
				FROM CommonNationalExamCertificateRequest c 
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
					left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
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
				select distinct c.Number t 	
				from(
					select distinct cb.FoundedCNEId,cb.AccountId
					from CNEWebUICheckLog cb '
		if @idorg<>-1 
			set @str=@str+'					
						jOIN Account Acc ON cb.AccountId=Acc.Id	and @idorg=Acc.OrganizationId	
						join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 '											
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							where 1=0 '							
		set @str=@str+'							
					) cb
					left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id '
		if @LastName is not null
			set @str=@str+'
						where c.LastName like ''%''+@LastName+''%'' '							
		set @str=@str+'		) t'	
	end
	print @str
	exec sp_executesql @str,N'@idorg int,@LastName nvarchar(255)=null',@idorg=@idorg,@LastName=@LastName
		
end
go
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (43, '043__2012_06_07__Modify_Prepare_Procs')
-- =========================================================================
GO

IF @@TRANCOUNT>0 BEGIN
	PRINT 'The database update succeeded.'
	--rollback TRANSACTION
	COMMIT TRANSACTION
	end
ELSE PRINT 'The database update failed.'
GO
SET NOEXEC OFF
GO