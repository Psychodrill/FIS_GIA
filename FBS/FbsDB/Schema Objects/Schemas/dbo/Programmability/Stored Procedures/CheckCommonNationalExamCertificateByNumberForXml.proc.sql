alter proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
    @number nvarchar(255) = null        -- номер сертификата  
  , @checkSubjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
  , @ParticipantID uniqueidentifier = null
  , @xml xml out
as
begin 
  
  if @number='' and @ParticipantID is null
  begin
    RAISERROR (N'Номер св-ва не указан',10,1);
    return
  end

  declare @eventId int

  if @shouldCheckMarks = 1
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @checkSubjectMarks, @CNENumber = @number, @IsOpenFbs = 1, @eventId = @eventId output
  
  
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
   pk int identity(1,1) primary key
  , Number nvarchar(255)
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

  declare @ss nvarchar(max)
  
  set @ss='create index [IX_#certificate_check_'+cast(newid() as nvarchar(200))+'] on #certificate_check (ParticipantID, [Year])' 
  exec sp_executesql @ss
  print @ss
  set @ss='create index [IX_#certificate_check_'+cast(newid() as nvarchar(200))+'] on #certificate_check (certificateId, [Year])' 
  exec sp_executesql @ss
  print @ss
  
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
  insert into #certificate_check 
  select
    [certificate].Number 
    , case
      when [certificate].ParticipantID is not null or [certificate].Id is not null then 1
      else 0
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
    , [certificate].ParticipantID
  from 
    (select null ''empty'') t
    left join 
    (
      select distinct b.LicenseNumber AS Number, b.CertificateID AS id, 
           COALESCE(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
      from rbd.Participants a with (nolock)       
        join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID and a.[UseYear]=cm.UseYear 
        left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and a.[UseYear]=b.UseYear 
      where a.[UseYear] between @yearFrom and @yearTo 
     '
  if @ParticipantID is not null 
    set @sql = @sql + ' and a.ParticipantID = @ParticipantID'   
  if @number <> ''  
    set @sql = @sql + ' and b.LicenseNumber = @number'
  if @ParticipantID is null and @number = '' 
    goto nullresult
  set @sql = @sql + '     
    ) [certificate] on 1=1
    left join dbo.Region region
      on region.Id = [certificate].RegionId
    left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear]=[certificate].[Year]
        and certificate_deny.CertificateFK = [certificate].id'
 
  exec sp_executesql @sql,N'@number nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier',
							@number = @number,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID

  set @eventParams = 
    isnull(@number, '') + '||||' +
    isnull(@checkSubjectMarks, '') + '|' 

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
  
  create table #table(
    certificateId uniqueidentifier,
    Number nvarchar(255),
    IsExist bit,
    SubjectId int,
    SubjectName nvarchar(100),
    CheckSubjectMark nvarchar(100),
    SubjectMark nvarchar(100),
    SubjectMarkIsCorrect bit,
    HasAppeal bit,
    IsDeny bit,
    DenyComment ntext,
    DenyNewcertificateNumber nvarchar(255),
    PassportSeria nvarchar(255),
    PassportNumber nvarchar(255),
    RegionId int,
    RegionName nvarchar(255),
    [Year] int,
    TypographicNumber nvarchar(255),
    [Status]  nvarchar(255),    
      UniqueChecks int,
        UniqueIHEaFCheck int,
        UniqueIHECheck int,
        UniqueIHEFCheck int,
        UniqueTSSaFCheck int,
        UniqueTSSCheck int,
        UniqueTSSFCheck int,
        UniqueRCOICheck int,
        UniqueOUOCheck int,
        UniqueFounderCheck int,
        UniqueOtherCheck int,
        ParticipantID uniqueidentifier)
      
  set @sql = '      
  select
    certificate_check.certificateId
    ,certificate_check.Number Number
    , certificate_check.IsExist IsExist
    , check_subject.SubjectId  SubjectId
    , check_subject.Name  SubjectName
    , case when check_subject.CheckSubjectMark < check_subject.[MinimalMark] then ''!'' else '''' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),''.'','','')  CheckSubjectMark
    , case when check_subject.SubjectMark < check_subject.MinimalMark1 then ''!'' else '''' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),''.'','','')  SubjectMark
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
    , case when ed.[ExpireDate] is null then ''Не найдено'' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then ''Аннулировано'' else
      case when getdate() <= ed.[ExpireDate] then ''Действительно''
      else ''Истек срок'' end end end  as [Status],
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
        CC.UniqueOtherCheck UniqueOtherCheck,
        certificate_check.ParticipantID       
  from #certificate_check certificate_check
        left join ExamcertificateUniqueChecks CC on CC.IdGuid = certificate_check.certificateId
		left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
		join (
			select      
				getcheck_subject.SubjectId id,
				[subject].Name,
				certificate_subject.UseYear [Year],
				certificate_subject.certificateFK certificateId, 
				isnull(getcheck_subject.SubjectId, [subject].SubjectId) SubjectId,
				getcheck_subject.[Mark] CheckSubjectMark,
				certificate_subject.[Mark] SubjectMark,
				case
					when getcheck_subject.Mark = certificate_subject.Mark then 1
				else 0 end SubjectMarkIsCorrect,
				certificate_subject.HasAppeal,
				mm.[MinimalMark],
				mm1.[MinimalMark] MinimalMark1,
				certificate_subject.ParticipantFK
			from [prn].CertificatesMarks certificate_subject with (nolock) 
				join #certificate_check a on a.ParticipantID=certificate_subject.ParticipantFK
				join dbo.[Subject] [subject]  on [subject].SubjectId = certificate_subject.SubjectCode  
				left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject on getcheck_subject.SubjectId = [subject].Id
				left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.UseYear = mm.[Year] 
				left join [MinimalMark] as mm1 on certificate_subject.SubjectCode = mm1.[SubjectId] and certificate_subject.UseYear = mm1.[Year] 
			) check_subject
				on certificate_check.[Year] = check_subject.[Year] and '    
  if @ParticipantID is null   
    set @sql=@sql + ' certificate_check.certificateId = check_subject.certificateId '
  else
    if @number <> ''  
      set @sql=@sql + ' certificate_check.ParticipantID=check_subject.ParticipantFK and check_subject.certificateId=certificate_check.certificateId '
    else
      set @sql=@sql + ' certificate_check.ParticipantID=check_subject.ParticipantFK and check_subject.certificateId<>certificate_check.certificateId '
            
  print @sql 
  insert #table
  exec sp_executesql @sql,N'@checkSubjectMarks nvarchar(max)',@checkSubjectMarks=@checkSubjectMarks     
  --select * from #table
      
IF @shouldCheckMarks = 1 AND  (exists(select * from #table where  SubjectMarkIsCorrect=0 and SubjectId IS NOT null) or (select COUNT(*) from #table where SubjectId IS NOT null)<>(select COUNT(*) from dbo.GetSubjectMarks(@checkSubjectMarks)))
  delete from #table
  --SELECT * FROM #table
  select @xml=(
  select 
  (
  select * from #table
  for xml path('check'), ELEMENTS XSINIL,type
  ) 
  for xml path('root'),type
  )
  
goto result 
nullresult:
  select @xml=(
  select null 
  for xml path('root'),type
  )
result:
    -- записать в лог интерактивных проверок
    if @shouldCheckMarks = 1 and exists (select * from #table)
    UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 certificateId FROM #certificate_check)
      WHERE Id=@eventId
    drop table #table
      drop table #certificate_check
    
    -- записать в лог всех проверок
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