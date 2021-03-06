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
  , @Year int = null
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

  if @Year is not null
	select @yearFrom = @Year, @yearTo = @Year
	
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
           COALESCE(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
		from rbd.Participants a with (nolock)       
			left join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID and cm.[UseYear]=a.UseYear
			left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and b.[UseYear]=a.UseYear
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
    left join dbo.Region region with (nolock) 
      on region.Id = [certificate].RegionId
    left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] = [certificate].[Year]
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
    left join prn.Certificates C with(nolock) on C.ParticipantFK =certificate_check.ParticipantID and c.[UseYear] = '+cast(@yearFrom as nvarchar(255)) 
    
   set @sql=@sql + '     
        left outer join ExamcertificateUniqueChecks CC with (nolock) on CC.IdGuid = certificate_check.certificateId and cc.[Year]=certificate_check.[Year]
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
        certificate_subject.ParticipantFK ParticipantID,
        certificate_subject.UseYear
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
      ) check_subject on check_subject.UseYear=certificate_check.[Year] and '  
  if @ParticipantID is null   
    set @sql=@sql + ' certificate_check.certificateId = check_subject.certificateId '
  else
    if @number <> ''  
      set @sql=@sql + ' certificate_check.ParticipantID=check_subject.ParticipantID and check_subject.certificateId=certificate_check.certificateId '
    else
      set @sql=@sql + ' certificate_check.ParticipantID=check_subject.ParticipantID
              and check_subject.certificateId <> isnull(C.CertificateID,''2F49AD69-5852-4B65-9C98-8D5F5C861BE4'') '
             
  set @sql=@sql + '
      left join dbo.[Subject] [subject] with (nolock) on check_subject.SubjectId = [subject].SubjectId
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