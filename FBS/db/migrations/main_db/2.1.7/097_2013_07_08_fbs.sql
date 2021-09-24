insert into Migrations(MigrationVersion, MigrationName) values (97, '097_2013_07_08_fbs.sql')
go

if not exists(select * from sys.indexes where name ='IX_CNEWebUICheckLog_LastName')
CREATE NONCLUSTERED INDEX [IX_CNEWebUICheckLog_LastName] ON [dbo].[CNEWebUICheckLog] 
(
	[LastName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

if not exists(select * from sys.indexes where name ='IX_Certificates_CertificateID')
CREATE NONCLUSTERED INDEX [IX_Certificates_CertificateID] ON [prn].[Certificates] 
(
	[CertificateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

if not exists(select * from sys.indexes where name ='IX_Certificates_LicenseNumber_UseYear')
CREATE NONCLUSTERED INDEX [IX_Certificates_LicenseNumber_UseYear] ON [prn].[Certificates] 
(
	[LicenseNumber] ASC,
	[UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
GO

if not exists(select * from sys.indexes where name ='IX_Certificates_ParticipantFK')
CREATE NONCLUSTERED INDEX [IX_Certificates_ParticipantFK] ON [prn].[Certificates] 
(
	[ParticipantFK] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
GO
if not exists(select * from sys.indexes where name ='IX_Certificates_UseYear')
CREATE NONCLUSTERED INDEX [IX_Certificates_UseYear] ON [prn].[Certificates] 
(
	[UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
if not exists(select * from sys.indexes where name ='IX_Certificates_UseYear_CertificateID_ParticipantFK')
CREATE NONCLUSTERED INDEX [IX_Certificates_UseYear_CertificateID_ParticipantFK] ON [prn].[Certificates] 
(
	[UseYear] ASC
)
INCLUDE ( [CertificateID],
[ParticipantFK]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
GO

if not exists(select * from sys.indexes where name ='IX_Participants_ParticipantID')
CREATE NONCLUSTERED INDEX [IX_Participants_ParticipantID] ON [rbd].[Participants] 
(
	[ParticipantID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

if not exists(select * from sys.indexes where name ='IX_Participants_ParticipantID_UseYear')
CREATE NONCLUSTERED INDEX [IX_Participants_ParticipantID_UseYear] ON [rbd].[Participants] 
(
	[ParticipantID] ASC,
	[UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

if not exists(select * from sys.indexes where name ='IX_Participants_UseYear')
CREATE NONCLUSTERED INDEX [IX_Participants_UseYear] ON [rbd].[Participants] 
(
	[UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
GO

if not exists(select * from sys.indexes where name ='IX_CNEWebUICheckLog_AccountId_Id_FoundedCNEId')
CREATE NONCLUSTERED INDEX [IX_CNEWebUICheckLog_AccountId_Id_FoundedCNEId]
ON [dbo].[CNEWebUICheckLog] ([AccountId])
INCLUDE ([Id],[FoundedCNEId])
GO

alter proc [dbo].[CheckCommonNationalExamCertificateByNumber]
   @number nvarchar(255) = null       -- номер сертификата
  , @checkLastName nvarchar(255) = null   -- фамилия сертифицируемого
  , @checkFirstName nvarchar(255) = null    -- имя сертифицируемого
  , @checkPatronymicName nvarchar(255) = null -- отчетсво сертифицируемого
  , @checkSubjectMarks nvarchar(max) = null -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @checkTypographicNumber nvarchar(20) = null -- типографический номер сертификата
  , @ParticipantID uniqueidentifier = null
as
begin 
  if @checkTypographicNumber is null and @number is null and @ParticipantID is null
  begin
    RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
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
      , @CId uniqueidentifier
    , @eventCode nvarchar(255)
    , @eventParams nvarchar(4000)
    , @sourceEntityIds nvarchar(4000) 
  
  declare @check_subject table
  (
  SubjectId int
  , Mark nvarchar(10)
  )
  
  create table #certificate_check
  (
    id int primary key identity(1,1)
  , Number nvarchar(255)
  , CheckLastName nvarchar(255)
  , LastName nvarchar(255)
  , LastNameIsCorrect bit
  , CheckFirstName nvarchar(255)
  , FirstName nvarchar(255)
  , FirstNameIsCorrect bit
  , CheckPatronymicName nvarchar(255)
  , PatronymicName nvarchar(255)
  , PatronymicNameIsCorrect bit
  , IsExist bit
  , certificateId uniqueidentifier
  , IsDeny bit
  , DenyComment ntext
  , DenyNewcertificateNumber nvarchar(255)
  , [Year] int
  , PassportSeria nvarchar(255)
  , PassportNumber nvarchar(255)
  , RegionId int
  , RegionName nvarchar(255)
  , TypographicNumber nvarchar(255)
  , ParticipantID uniqueidentifier
  )

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  if isnull(@checkTypographicNumber,'') <> ''
    select @yearFrom = 2009, @yearTo = Year(GetDate()) --2009-год появления типографского номера в БД
  else
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
    , @CheckLastName CheckLastName
    , [certificate].LastName 
    , case 
      when @CheckLastName is null then 1 
      when [certificate].LastName collate cyrillic_general_ci_ai = @CheckLastName then 1
      else 0
    end LastNameIsCorrect
    , @CheckFirstName CheckFirstName
    , [certificate].FirstName 
    , case 
      when @CheckFirstName is null then 1 
      when [certificate].FirstName collate cyrillic_general_ci_ai = @CheckFirstName then 1
      else 0
    end FirstNameIsCorrect
    , @CheckPatronymicName CheckPatronymicName 
    , [certificate].PatronymicName 
    , case 
      when @CheckPatronymicName is null then 1 
      when [certificate].PatronymicName collate cyrillic_general_ci_ai = @CheckPatronymicName then 1
      else 0
    end PatronymicNameIsCorrect
    , case'
    if @ParticipantID is not null 
      set @sql = @sql + ' when certificate.ParticipantID is not null then 1'
    else
      set @sql = @sql + ' when [certificate].Id is not null then 1 '    
    set @sql = @sql + ' else 0 
    end IsExist
    , [certificate].Id
    , case
      when certificate_deny.UseYear is not null then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Reason
    , null NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , certificate.ParticipantID
  from 
    (select null ''empty'') t 
    left join 
      (
      select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
            from rbd.Participants a with (nolock)       
        left join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID
        left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and b.[UseYear] between @yearFrom and @yearTo
        left join prn.CancelledCertificates c with (nolock) on c.CertificateFK=b.CertificateID and c.[UseYear] between @yearFrom and @yearTo
      where a.[UseYear] between @yearFrom and @yearTo '
      
  if @ParticipantID is not null 
    set @sql = @sql + ' and a.ParticipantID = @ParticipantID'   
  if @number is not null 
  begin
  if @number <> ''  
    set @sql = @sql + ' and b.LicenseNumber=@number '
  end 
  
  if @CheckTypographicNumber is not null 
    set @sql = @sql + ' and b.TypographicNumber=@CheckTypographicNumber'      
    
  set @sql = @sql + '    
     ) [certificate] on 1=1  '
  if @number = ''    
    set @sql = @sql + ' and [certificate].Number is null   '
  set @sql = @sql + '     
    left outer join dbo.Region region with (nolock) 
      on region.Id = [certificate].RegionId
    left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] between @yearFrom and @yearTo
        and certificate_deny.CertificateFK = [certificate].id'

  insert into #certificate_check    

  exec sp_executesql @sql,N'@checkLastName nvarchar(255),@number nvarchar(255),@checkTypographicNumber nvarchar(20),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier',@checkLastName=@checkLastName,@number = @number,@checkTypographicNumber=@checkTypographicNumber,@checkFirstName=@checkFirstName,@checkPatronymicName=@checkPatronymicName,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID
--select * from #certificate_check    

    set @eventParams = 
    isnull(@number, '') + '|' +
    isnull(@checkLastName, '') + '|' +
    isnull(@checkFirstName, '') + '|' +
    isnull(@checkPatronymicName, '') + '|' +
    isnull(@checkSubjectMarks, '') + '|' +
    isnull(@checkTypographicNumber, '')

  set @sourceEntityIds = '' 
  select 
    @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(100), certificate_check.certificateId) 
  from 
    #certificate_check certificate_check 

  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
  if @sourceEntityIds = '' 
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        #certificate_check S
    where
      S.certificateId is not null
    
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @certificateIdGuid = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
  -------------------------------------------------------------
  
  set @sql = '     
  select  
    isnull(cast(certificate_check.certificateId as nvarchar(250)),''Нет свидетельства'' ) certificateId
    ,certificate_check.Number
    , certificate_check.CheckLastName
    , certificate_check.LastName
    , certificate_check.LastNameIsCorrect
    , certificate_check.CheckFirstName
    , certificate_check.FirstName
    , certificate_check.FirstNameIsCorrect
    , certificate_check.CheckPatronymicName
    , certificate_check.PatronymicName
    , certificate_check.PatronymicNameIsCorrect
    , certificate_check.IsExist
    , [subject].Id SubjectId
    , [subject].Name SubjectName
    , case when check_subject.CheckSubjectMark < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),''.'','','') CheckSubjectMark
    , case when check_subject.SubjectMark < mm.[MinimalMark] then ''!'' else '''' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),''.'','','') SubjectMark
    , isnull(check_subject.SubjectMarkIsCorrect, 0) SubjectMarkIsCorrect
    , check_subject.HasAppeal
    , certificate_check.IsDeny
    , certificate_check.DenyComment
    , certificate_check.DenyNewcertificateNumber
    , certificate_check.PassportSeria
    , certificate_check.PassportNumber
    , certificate_check.RegionId
    , certificate_check.RegionName
    , certificate_check.[Year]
    , certificate_check.TypographicNumber
    , case when ed.[ExpireDate] is null then ''Не найдено'' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then ''Аннулировано'' else
      case when getdate() <= ed.[ExpireDate] then ''Действительно''
      else ''Истек срок'' end end end as [Status],
        isnull(CC.UniqueChecks, 0) UniqueChecks,
        isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
        isnull(CC.UniqueIHECheck, 0) UniqueIHECheck,
        isnull(CC.UniqueIHEFCheck, 0) UniqueIHEFCheck,
        isnull(CC.UniqueTSSaFCheck, 0) UniqueTSSaFCheck,
        isnull(CC.UniqueTSSCheck, 0) UniqueTSSCheck,
        isnull(CC.UniqueTSSFCheck, 0) UniqueTSSFCheck,
        isnull(CC.UniqueRCOICheck, 0) UniqueRCOICheck,
        isnull(CC.UniqueOUOCheck, 0) UniqueOUOCheck,
        isnull(CC.UniqueFounderCheck, 0) UniqueFounderCheck,
        isnull(CC.UniqueOtherCheck, 0) UniqueOtherCheck,
        certificate_check.ParticipantID,
        check_subject.certificateId
  from #certificate_check certificate_check '
  if @number = ''  
    set @sql=@sql + '    
    left join vw_Examcertificate C with(nolock) on C.ParticipantID =certificate_check.ParticipantID '
    set @sql=@sql + '     
        left outer join ExamcertificateUniqueChecks CC with (nolock) on CC.IdGuid = certificate_check.certificateId
    left join [ExpireDate] as ed with (nolock) on certificate_check.[Year] = ed.[Year]          
    join (
      select
        certificate_check.Number 
        , certificate_check.CheckLastName
        , certificate_check.LastName 
        , certificate_check.LastNameIsCorrect
        , certificate_check.CheckFirstName
        , certificate_check.FirstName 
        , certificate_check.FirstNameIsCorrect
        , certificate_check.CheckPatronymicName
        , certificate_check.PatronymicName 
        , certificate_check.PatronymicNameIsCorrect
        , certificate_check.IsExist
        , isnull(check_subject.SubjectId, [subject].SubjectId) SubjectId
        , check_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when check_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal,
        certificate_subject.certificatefk certificateId,
        certificate_subject.ParticipantFK ParticipantID
      from [prn].CertificatesMarks certificate_subject with (nolock)         
        inner join dbo.[Subject] [subject] with (nolock) on certificate_subject.SubjectCode = [subject].SubjectId
        inner join #certificate_check certificate_check
          on certificate_check.[Year] = certificate_subject.UseYear '
  if @ParticipantID is null   
    set @sql=@sql + ' and certificate_check.certificateId = certificate_subject.CertificateFK '
  else
    set @sql=@sql + ' and certificate_subject.ParticipantFK = certificate_check.ParticipantID '
    
  set @sql=@sql + ' 
        left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
          on check_subject.SubjectId = [subject].SubjectId
      ) check_subject on '  
  if @ParticipantID is null   
    set @sql=@sql + ' certificate_check.certificateId = check_subject.certificateId '
  else
    if @number <> ''  
      set @sql=@sql + ' certificate_check.ParticipantID=check_subject.ParticipantID and check_subject.certificateId=certificate_check.certificateId '
    else
      set @sql=@sql + ' certificate_check.ParticipantID=check_subject.ParticipantID
              and check_subject.certificateId <> isnull(C.Id,''2F49AD69-5852-4B65-9C98-8D5F5C861BE4'') '
             
  set @sql=@sql + '
      left outer join dbo.[Subject] [subject] with (nolock) on check_subject.SubjectId = [subject].SubjectId
      left join [MinimalMark] as mm with (nolock) on [subject].SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year] '      

  exec sp_executesql @sql,N'@checkSubjectMarks nvarchar(max)',@checkSubjectMarks=@checkSubjectMarks

  drop table #certificate_check 
    if @checkTypographicNumber is not null
      set @eventCode = 'CNE_CHK_TN'
    else
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
go
-- =============================================
-- Получить список проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- v.1.1: Modified by Makarev Andrey 06.05.2008
-- Добавлен параметр @AccountId в sp_executesql
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Добавлены поля IsDeny, DenyComment.
-- v.1.3: Modified by Fomin Dmitriy 02.06.2008
-- Испралена логика: Check -> Request.
-- v.1.4: Modified by Sedov Anton 03.06.2008
-- Добавлен пейджинг
-- Добавлены параметры:
-- @startRowIndex, @maxRowCount, @showCount
-- v.1.5: Modified by Sedov Anton 18.06.2008
-- В результат добавлена выборка данных
-- серии и номера паспорта
-- v.1.6 Modified by Sedov Anton 18.06.2008
-- добавлен параметр расширения запроса
-- @isExtended, при значении 1 возвращаются
-- оценки по экзаменам
-- v.1.7 Modified by Sedov Anton 20.06.2008
-- добавлен параметр расширения запроса
-- @isExtendedbyExam, при 1 получаем
-- список экзаменов в которых участвовал
-- выпускник
-- v.1.8 : Modified by Makarev Andrey 23.06.2008
-- Исправлен пейджинг.
-- v.1.9:  Modified by Sedov Anton 04.07.2008
-- в результат запроса добавлено поле 
-- DenyNewCertificateNumber
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificateRequest]
	@login nvarchar(255)
	, @batchId bigint
	, @startRowIndex int = 1
	, @maxRowCount int = null
	, @showCount bit = null
	, @isExtended bit = null
	, @isExtendedByExam bit = null
as
begin
	declare 
		@innerBatchId bigint
		, @accountId bigint
		, @commandText nvarchar(max)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @declareCommandText nvarchar(max)
		, @viewSelectCommandText nvarchar(max)
		, @pivotSubjectColumns nvarchar(max)
		, @viewSelectPivot1CommandText nvarchar(max)
		, @viewSelectPivot2CommandText nvarchar(max)
		, @viewCommandText nvarchar(max)
		, @sortColumn nvarchar(20) 
		, @sortAsc bit 

	set @commandText = ''
	set @pivotSubjectColumns = ''
	set @viewSelectPivot1CommandText = ''
	set @viewSelectPivot2CommandText = ''
	set @viewCommandText = ''
	set @viewSelectCommandText = ''
	set @declareCommandText = ''
	set @resultOrder = ''
	set @sortColumn = N'Id'
	set @sortAsc = 1
	
	if @batchId is not null
		set @innerBatchId = dbo.GetInternalId(@batchId)

	--если батч НЕ принадлежит пользователю, который пытается его посмотреть
	--или если смотрит НЕ админ, то не даем посмотреть
	if not exists(select top 1 1
			from dbo.CommonNationalExamCertificateRequestBatch cnecrb with (nolock, fastfirstrow)
				inner join dbo.Account a with (nolock, fastfirstrow)
					on cnecrb.OwnerAccountId = a.[Id]
			where 
				cnecrb.Id = @innerBatchId
				and cnecrb.IsProcess = 0
				and (a.[Login] = @login 
					or exists (select top 1 1 from [Account] as a2
					join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
					join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
					where a2.[Login] = @login)))
		set @innerBatchId = 0

	set @declareCommandText = 
		N'declare @search table 
			(
			pk int identity(1,1) primary key
			, Id bigint
			, BatchId bigint
			, CertificateNumber nvarchar(255)
			, LastName nvarchar(255)
			, FirstName nvarchar(255)
			, PatronymicName nvarchar(255)
			, PassportSeria nvarchar(255)
			, PassportNumber nvarchar(255)
			, IsExist bit
			, RegionName nvarchar(255)
			, RegionCode nvarchar(10)
			, IsDeny bit
			, DenyComment ntext
			, DenyNewCertificateNumber nvarchar(255)
			, SourceCertificateId uniqueidentifier
			, SourceCertificateYear int
			, TypographicNumber nvarchar(255)
			, [Status] nvarchar(255)
			, ParticipantID uniqueidentifier
			)
		'

	if isnull(@showCount, 0) = 0
		set @commandText = 
			N'select <innerHeader> 
				dbo.GetExternalId(cne_certificate_request.Id) [Id]
				, dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
				, cne_certificate_request.SourceCertificateNumber CertificateNumber
				, cne_certificate_request.LastName 
				, cne_certificate_request.FirstName 
				, cne_certificate_request.PatronymicName
				, cne_certificate_request.PassportSeria
				, cne_certificate_request.PassportNumber
				, case
					when not cne_certificate_request.ParticipantID is null then 1
					else 0
				  end IsExist
				, region.Name RegionName
				, region.Code RegionCode
				, isnull(cne_certificate_request.IsDeny, 0) IsDeny 
				, cne_certificate_request1.DenyComment DenyComment
				, cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
				, cne_certificate_request.SourceCertificateIdGuid
				, cne_certificate_request.SourceCertificateYear
				, cne_certificate_request.TypographicNumber
				, case when cne_certificate_request.ParticipantID is null then ''Не найдено'' 
					   else 
							case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно'' 
								 else ''Истек срок'' end end as [Status]
				, cne_certificate_request.ParticipantID
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)											
					join 
					(
						SELECT distinct cne_certificate_request.Id,cne_certificate_request.BatchId,cne_certificate_request.SourceCertificateYear,cne_certificate_request.SourceRegionId, 
							   cne_certificate_request.SourceCertificateNumber,cne_certificate_request.ParticipantID,
							   isnull(cne_certificate_request.LastName, a.Surname) LastName,
							   isnull(cne_certificate_request.FirstName, a.Name) FirstName,
							   isnull(cne_certificate_request.PatronymicName, a.SecondName) PatronymicName,
							   isnull(cne_certificate_request.PassportSeria, a.DocumentSeries) PassportSeria,
							   isnull(cne_certificate_request.PassportNumber, a.DocumentNumber) PassportNumber,
							   cne_certificate_request.IsDeny, cne_certificate_request.DenyNewCertificateNumber, 
							   cne_certificate_request.SourceCertificateIdGuid, cne_certificate_request.TypographicNumber,
							   b.LicenseNumber AS Number, b.UseYear AS Year
						FROM dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock) 
							left join prn.Certificates AS b with(nolock) on cne_certificate_request.[SourceCertificateYear] = b.useyear 
																				and cne_certificate_request.SourceCertificateIdGuid = b.CertificateID
							left JOIN rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID					
					) cne_certificate_request
					on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
					join dbo.CommonNationalExamCertificateRequest cne_certificate_request1 with (nolock) on cne_certificate_request1.Id = cne_certificate_request.[Id]
					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId		
					left join [ExpireDate] as ed with (nolock) 
						on cne_certificate_request.[SourceCertificateYear] = ed.[Year]										 
			where 1 = 1 '
	else
		set @commandText = 
			N'
			select count(distinct cne_certificate_request.Id)
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
						on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId
			where 1 = 1 ' 

	set @commandText = @commandText +
			'   and cne_certificate_request_batch.[Id] = @innerBatchId 
				and cne_certificate_request_batch.IsProcess = 0 '

	if isnull(@showCount, 0) = 0
	begin

		if @sortColumn = 'Id'
		begin
			set @innerOrder = ' order by Id <orderDirection> '
			set @outerOrder = ' order by Id <orderDirection> '
			set @resultOrder = ' order by Id <orderDirection> '
		end
		else 
		begin
			set @innerOrder = ' order by Id <orderDirection> '
			set @outerOrder = ' order by Id <orderDirection> '
			set @resultOrder = ' order by Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(
									replace(
											replace(
													N'insert into @search ' + 
													N' select <outerHeader> * ' + 
													N' from (<innerSelect>) as innerSelect ' + @outerOrder
												  , N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end

	if isnull(@showCount, 0) = 0
	begin
		if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
		begin
			set @declareCommandText = @declareCommandText +
				N' declare @subjects table  
					( 
					ParticipantID uniqueidentifier
					, CertificateId uniqueidentifier 
					, Mark nvarchar(10)
					, HasAppeal bit  
					, SubjectCode nvarchar(255)  
					, HasExam bit
					, isCorrect int
					, primary key(ParticipantID, SubjectCode)
					) 
				'

			set @commandText = @commandText +
				N'insert into @subjects  
				select
					cne_certificate_subject.ParticipantFK
					, cne_certificate_subject.CertificateFK 
					, case when cne_certificate_subject.[Mark] < mm.[MinimalMark] 
						   then ''!'' 
						   else '''' 
					  end + 
						replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
					, cne_certificate_subject.HasAppeal
					, subject.Code
					, 1 
					, case when search.SourceCertificateId is not null then 1 else 0 end
				from	
					[prn].CertificatesMarks cne_certificate_subject with(nolock)
					left join @search search on cne_certificate_subject.CertificateFK = search.SourceCertificateId
								and cne_certificate_subject.[UseYear] = search.SourceCertificateYear
					left join dbo.Subject subject on subject.SubjectId = cne_certificate_subject.SubjectCode
					left join [MinimalMark] as mm on cne_certificate_subject.[SubjectCode] = mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
				where 
					exists(select 1 
							from @search search
							where cne_certificate_subject.ParticipantFK = search.ParticipantID
								and cne_certificate_subject.[UseYear] = search.SourceCertificateYear)								
				' 
		end
		
		set @viewSelectCommandText = 
			N'select
				search.Id 
				, search.BatchId
				, case when search.SourceCertificateId is null and ParticipantID is not null then ''Нет свидетества'' else search.CertificateNumber end CertificateNumber
				, search.LastName
				, search.FirstName
				, search.PatronymicName
				, search.PassportSeria
				, search.PassportNumber
				, search.IsExist
				, search.RegionName
				, search.RegionCode
				, search.IsDeny 
				, search.DenyComment
				, search.DenyNewCertificateNumber
				, search.TypographicNumber
				, search.SourceCertificateYear
				, search.Status
				, search.ParticipantID
			'

		set @viewCommandText = 
			N' ,unique_cheks.UniqueIHEaFCheck
			from @search search 
			left outer join dbo.ExamCertificateUniqueChecks unique_cheks
					on unique_cheks.idGUID = search.SourceCertificateId '

		if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
		begin 
			declare
				@subjectCode nvarchar(255)
				, @pivotSelect nvarchar(4000)

			set @pivotSelect = ''

			declare subject_cursor cursor fast_forward for
			select s.Code
			from dbo.Subject s with(nolock)
			order by s.id asc 

			open subject_cursor 
			fetch next from subject_cursor into @subjectCode
			while @@fetch_status = 0
				begin
				if len(@pivotSubjectColumns) > 0
					set @pivotSubjectColumns = @pivotSubjectColumns + ','
				set @pivotSubjectColumns = @pivotSubjectColumns + replace('[<code>]', '<code>', 

@subjectCode)
				
				if isnull(@isExtended, 0) = 1
					set @pivotSelect =  
						N'	, mrk_pvt.[<code>] [<code>Mark]  
							, apl_pvt.[<code>] [<code>HasAppeal] '
				if isnull(@isExtendedByExam, 0) = 1
					set @pivotSelect = @pivotSelect + 
						N' 
							, isnull(exam_pvt.[<code>], 0) [<code>HasExam] '
						
				set @pivotSelect = replace(@pivotSelect, '<code>', @subjectCode)

				if len(@viewSelectPivot1CommandText) + len(@pivotSelect) <= 4000
					and @viewSelectPivot2CommandText = ''
					set @viewSelectPivot1CommandText = @viewSelectPivot1CommandText + @pivotSelect
				else
					set @viewSelectPivot2CommandText = @viewSelectPivot2CommandText + @pivotSelect

				fetch next from subject_cursor into @subjectCode
			end
			close subject_cursor
			deallocate subject_cursor
		end

		if isnull(@isExtended, 0) = 1
		begin
			set @viewCommandText = @viewCommandText + 
				N'
				left join (select 
					subjects.CertificateId
					, subjects.Mark 
					, subjects.SubjectCode
					, subjects.ParticipantID ParticipantId1
					, isCorrect
					from @subjects subjects) subjects
						pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt
							on 1= case when search.SourceCertificateId = mrk_pvt.CertificateId then 1 
									   when mrk_pvt.ParticipantId1=search.ParticipantId and search.SourceCertificateId is null and mrk_pvt.isCorrect=0 then 1
									   else 0 
								  end
				left join (select 
					subjects.CertificateId
					, cast(subjects.HasAppeal as int) HasAppeal 
					, subjects.SubjectCode
					, subjects.ParticipantID ParticipantId1
					, isCorrect
					from @subjects subjects) subjects
						pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as apl_pvt
							on 1= case when search.SourceCertificateId = mrk_pvt.CertificateId then 1 
									   when mrk_pvt.ParticipantId1=search.ParticipantId and search.SourceCertificateId is null and apl_pvt.isCorrect=0  then 1
									   else 0 
								  end 
								and mrk_pvt.CertificateId=apl_pvt.CertificateId '
				set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
					
		if isnull(@isExtendedByExam, 0) = 1
		begin
			set @viewCommandText = @viewCommandText + 
				N'left outer join (select 
					subjects.CertificateId
					, subjects.SubjectCode
					, cast(subjects.HasExam as int) HasExam 
					, subjects.ParticipantID ParticipantId1
					from @subjects subjects) subjects
						pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
					on search.SourceCertificateId = exam_pvt.CertificateId '
					
			set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
	end

	set @viewCommandText = @viewCommandText + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

	print @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText
	
	exec sp_executesql @commandText
		, N'@innerBatchId bigint'
		, @innerBatchId
		
	return 0
end

