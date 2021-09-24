insert into Migrations(MigrationVersion, MigrationName) values (103, '103_2013_07_17_Исправление дублей.sql')
go
alter proc [dbo].[SearchCommonNationalExamCertificateWildcard]
  @lastName nvarchar(255) = null
  , @firstName nvarchar(255) = null
  , @patronymicName nvarchar(255) = null
  , @passportSeria nvarchar(255) = null
  , @passportNumber nvarchar(255) = null
  , @typographicNumber nvarchar(255) = null
  , @Number nvarchar(255) = null
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @year int = null
  , @startRowIndex int = 1
  , @maxRowCount int = 20
  , @showCount bit = 0
as
begin
  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(max)
    , @internalPassportSeria nvarchar(255)

  set @eventParams = 
    @lastName + '|' 
    + @firstName + '|' 
    + @patronymicName + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') + '|' 
    + isnull(@Number, '') + '|' 
    + isnull(@typographicNumber, '') + '|' 
    + isnull(cast(@year as varchar(max)), '')

  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  if @year is not null
    select @yearFrom = @year, @yearTo = @year
  else
    select @yearFrom = 2008 --Первые св-ва датируются 2008 годом
    select @yearTo = Year(GetDate())

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  select 
    @commandText = ''
    ,@eventCode = N'CNE_FND_WLDCRD'

  declare @sourceEntityIds nvarchar(4000)  
  declare @Search table 
  ( 
    row int,
    LastName nvarchar(255), 
    FirstName nvarchar(255), 
    PatronymicName nvarchar(255), 
    CertificateId uniqueidentifier, 
    CertificateNumber nvarchar(255),
    RegionId int, 
    PassportSeria nvarchar(255), 
    PassportNumber nvarchar(255), 
    TypographicNumber nvarchar(255),
    Year int,
    ParticipantFK uniqueidentifier,
    primary key(row) 
      ) 
      
  if @showCount = 0
  set @commandText = @commandText + 
    ' 
    select top (@startRowIndex+@maxRowCount-1) 
        row_number() over (order by a.useyear, a.CertificateID) as row,a.*
    from ( select distinct
      a.Surname 
      , a.Name 
      , a.SecondName 
      , b.CertificateID 
      , b.LicenseNumber 
      , b.Region
      , isnull(a.DocumentSeries, @internalPassportSeria) PassportSeria 
      , isnull(a.DocumentNumber, @passportNumber) PassportNumber
      , b.TypographicNumber 
      , b.UseYear 
      , a.ParticipantID
    '
  if @showCount = 1
    set @commandText = ' select count(*) '
  
  set @commandText = @commandText + 
    '
    from 
		rbd.Participants AS a WITH (nolock) 
		JOIN prn.CertificatesMarks AS cm WITH (nolock) ON cm.ParticipantFK = a.ParticipantID AND cm.UseYear = a.UseYear 
		LEFT JOIN prn.Certificates AS b WITH (nolock) ON b.CertificateID = cm.CertificateFK AND b.UseYear = cm.UseYear 
		LEFT JOIN prn.CancelledCertificates AS c WITH (nolock) ON c.CertificateFK = b.CertificateID AND c.UseYear = b.UseYear    
    where 
      a.[UseYear] between @yearFrom and @yearTo '
  
  if @lastName is not null 
    set @commandText = @commandText + '
      and a.Surname collate cyrillic_general_ci_ai = @lastName'
  if @firstName is not null 
    set @commandText = @commandText + '     
      and a.Name collate cyrillic_general_ci_ai = @firstName'
  if @patronymicName is not null 
    set @commandText = @commandText + '           
      and a.SecondName collate cyrillic_general_ci_ai = @patronymicName'
  if @internalPassportSeria is not null 
    set @commandText = @commandText + '                 
      and a.DocumentSeries = @internalPassportSeria'
  if @passportNumber is not null 
    set @commandText = @commandText + '                       
      and a.DocumentNumber = @passportNumber'
  if @typographicNumber is not null 
    set @commandText = @commandText + '                             
      and b.TypographicNumber = @typographicNumber'
  if @Number is not null 
    set @commandText = @commandText + '                                   
      and b.LicenseNumber = @Number '  
      
  if @showCount = 1     
    exec sp_executesql @commandText
      , N'@lastName nvarchar(255)
        , @firstName nvarchar(255)
        , @patronymicName nvarchar(255)
        , @internalPassportSeria nvarchar(255)
        , @passportNumber nvarchar(255)
        , @typographicNumber nvarchar(255) 
        , @Number nvarchar(255)
        , @yearFrom int, @yearTo int 
        , @startRowIndex int
        , @maxRowCount int'
      , @lastName
      , @firstName
      , @patronymicName
      , @internalPassportSeria
      , @passportNumber
      , @typographicNumber
      , @Number
      , @yearFrom
      , @YearTo
      , @startRowIndex
      , @maxRowCount    
  else
  begin   
	set @commandText=@commandText +' ) a'
    insert into @Search 
    exec sp_executesql @commandText
      , N'@lastName nvarchar(255)
        , @firstName nvarchar(255)
        , @patronymicName nvarchar(255)
        , @internalPassportSeria nvarchar(255)
        , @passportNumber nvarchar(255)
        , @typographicNumber nvarchar(255) 
        , @Number nvarchar(255)
        , @yearFrom int, @yearTo int 
        , @startRowIndex int
        , @maxRowCount int'
      , @lastName
      , @firstName
      , @patronymicName
      , @internalPassportSeria
      , @passportNumber
      , @typographicNumber
      , @Number
      , @yearFrom
      , @YearTo
      , @startRowIndex
      , @maxRowCount  
      
    select         
        isnull(cast(search.CertificateNumber as nvarchar(250)),'Нет свидетельства' ) CertificateNumber
        , search.LastName LastName 
        , search.FirstName FirstName 
        , search.PatronymicName PatronymicName 
        , search.PassportSeria PassportSeria 
        , search.PassportNumber PassportNumber 
        , search.TypographicNumber TypographicNumber 
        , region.Name RegionName 
        , case
          when search.CertificateId is not null or search.ParticipantFK is not null then 1
          else 0
        end IsExist
        , case 
          when not cne_certificate_deny.UseYear is null then 1 
          else 0 
        end IsDeny 
        , cne_certificate_deny.Reason DenyComment 
        , null NewCertificateNumber 
        , search.[Year] 
        , case when ed.[ExpireDate] is null then 'Не найдено'  
             when cne_certificate_deny.UseYear is not null then 'Аннулировано' 
             when getdate() <= ed.[ExpireDate] then 'Действительно'
             else 'Истек срок' 
          end as [Status]
        , unique_cheks.UniqueIHEaFCheck,
        search.ParticipantFK ParticipantID
       from @Search search
        left outer join dbo.ExamCertificateUniqueChecks unique_cheks on unique_cheks.idGUID = search.CertificateId 
        left outer join prn.CancelledCertificates cne_certificate_deny with (nolock) on cne_certificate_deny.[UseYear] = search.[year] and search.CertificateId = cne_certificate_deny.CertificateFK 
        left outer join dbo.Region region with (nolock) on region.[Id] = search.RegionId 
        left join [ExpireDate] ed on  ed.[year] = search.[year] 
       where row between @startRowIndex and (@startRowIndex+@maxRowCount-1)       
       
       exec dbo.RegisterEvent 
        @accountId = @editorAccountId  
        , @ip = @ip 
        , @eventCode = @eventCode 
        , @sourceEntityIds = '0'
        , @eventParams = @eventParams 
        , @updateId = null 
  end
        
  return 0
end
go
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
    left join vw_Examcertificate C with(nolock) on C.ParticipantID =certificate_check.ParticipantID '
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
			FROM CommonNationalExamCertificateRequest c with(nolock)
		'
		if @idorg<>-1	
			set @str=@str+
		'		
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
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
			FROM CNEWebUICheckLog c with(nolock)
		'
		if @idorg<>-1	
			set @str=@str+
		'
				join Account acc with(nolock) on acc.id=c.AccountId 	
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg
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
			case WHEN c.SourceCertificateId IS NULL THEN ''Не найдено'' else
				case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
				else ''Истек срок'' end end  STATUS,c1.id rn
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
			JOIN prn.Certificates AS b with(nolock) on FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
			left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.UseYear=b.UseYear
			left join ExamcertificateUniqueChecks CC with(nolock) on b.CertificateID=cc.idGUID and cc.[Year]=b.UseYear
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
		declare @tab table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)		
		declare @table table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)
			
		insert @tab			
		select id1,id2,id3
		from 
		(
			select top (@pf) min(id1) id1,min(id2) id2,min(id3) id3,SourceCertificateIdGuid'+case when @ss='LastName' then ',LastName' else '' end + '
			from 
				(				
				select min(c.id) id1, null id2,null id3,c.SourceCertificateIdGuid'+case when @ss='LastName' then ',c.LastName' else '' end + '
				from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateCheck c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
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
				select null id1,min(c.id) id2,null id3,c.SourceCertificateIdGuid'+case when @ss='LastName' then ',c.LastName' else '' end + '
				from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateRequest c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo									
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
				select null id1,null id2,min(c.id) id3,cast(c.FoundedCNEId as uniqueidentifier)'+case when @ss='LastName' then ',c.LastName' else '' end + '
				from ExamCertificateUniqueChecks a with(nolock)
					join CNEWebUICheckLog c with(nolock) on c.FoundedCNEId <> ''Нет свидетельства''	and c.FoundedCNEId=a.idGUID	and a.[Year] between @yearFrom and @yearTo												
					  '
		if @idorg<>-1 
			set @str=@str+
			          '							
					join Account Acc on acc.id=c.AccountId and @idorg=Acc.OrganizationId 	
					join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
					  '		
		if @TypeCheck <> 'Интерактивная' or @TypeCheck is not null
				set @str=@str+'						
							and 1=0 
							  '						  			
		set @str=@str+
		'			
				group by c.FoundedCNEId'+case when @ss='LastName' then ',c.LastName' else '' end + '			
			) c		
			group by SourceCertificateIdGuid'+case when @ss='LastName' then ',c.LastName' else '' end + '		
			order by '+case when @ss='LastName' then 'LastName' else ' id1' end + case when @so = 0 then ' asc' else ' desc' end + '
		) t		
		
		insert @table
		select id1,id2,id3 from @tab
		where id between @ps and @pf			
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
				case WHEN c.SourceCertificateId IS NULL THEN ''Не найдено'' else
					case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
					else ''Истек срок'' end end  STATUS
			FROM CommonNationalExamCertificateRequest c  with(nolock)
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
				else ''Истек срок'' end Status				
			from 
				@table cb1
				join CNEWebUICheckLog cb with(nolock) on cb1.id3=cb.id															
				JOIN prn.Certificates AS b with(nolock) on FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
				left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.UseYear=b.UseYear
				left join ExamcertificateUniqueChecks CC with(nolock) on b.CertificateID=cc.idGUID and b.UseYear=b.UseYear
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
		order by rn
		
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
			    left join prn.Certificates AS b with(nolock) on cb.FoundedCNEId <> ''Нет свидетельства'' and cb.FoundedCNEId=b.CertificateID
			    left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID
				where a.Surname like ''%''+@LastName+''%'' '				
				
		if @idorg<>-1 
			set @str=@str+'
				and d.id=@idorg and d.DisableLog=0 '
																
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
				join Account Acc on acc.id=c.AccountId and @idorg=Acc.OrganizationId 	
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
go
alter proc [dbo].[SearchCommonNationalExamCertificate]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @subjectMarks nvarchar(4000) = null   -- средние оценки по предметам (через запятую, в определенном порядке)
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @typographicNumber nvarchar(255) = null -- типографический номер сертификата 
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
  , @ParticipantID uniqueidentifier = null  
as
begin
  declare 
    @eventCode nvarchar(255)
        , @organizationId bigint
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(4000)
    , @internalPassportSeria nvarchar(255)

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  set @eventParams = 
    isnull(@lastName,'') + '|' 
    + isnull(@firstName,'') + '|' 
    + isnull(@patronymicName,'') + '|' 
    + isnull(@subjectMarks, '') + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') + '|' 
    + isnull(@typographicNumber, '')

  select
    @editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  if @year is not null
    select @yearFrom = @year, @yearTo = @year
  else
    select @yearFrom = 2008
      select @yearTo = Year(GetDate())

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  set @commandText = ''
  if isnull(@typographicNumber, '') <> ''
    set @eventCode = N'CNE_FND_TN'
  else 
    set @eventCode = N'CNE_FND_P'

  declare @CId bigint,@sourceEntityIds nvarchar(4000) 
  declare @Search table 
  ( 
    LastName nvarchar(255) 
    , FirstName nvarchar(255) 
    , PatronymicName nvarchar(255) 
    , CertificateId uniqueidentifier
    , CertificateNumber nvarchar(255) 
    , RegionId int 
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255) 
    , TypographicNumber nvarchar(255) 
    , Year int 
    , ParticipantID uniqueidentifier
  ) 
    
  set @commandText = @commandText +       
    'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year,ParticipantID
    from (
		select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
        from rbd.Participants a with (nolock)       
			left join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID and a.UseYear=cm.UseYear
			left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and a.UseYear=b.UseYear
			left join prn.CancelledCertificates c with (nolock) on c.CertificateFK=b.CertificateID and c.UseYear=b.UseYear
      where a.[UseYear] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and a.Surname collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and a.Name collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and a.SecondName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentSeries like @internalPassportSeria and a.ParticipantID is not null '
        end
        else begin
            set @commandText = @commandText +
                ' and a.DocumentSeries = @internalPassportSeria and a.ParticipantID is not null '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentNumber like @passportNumber and a.ParticipantID is not null '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and a.DocumentNumber = @passportNumber and a.ParticipantID is not null '
        end
    end
  
  if not @typographicNumber is null
    set @commandText = @commandText +
      ' and b.TypographicNumber = @typographicNumber and a.ParticipantID is not null '
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  set @commandText = @commandText + ') [certificate] '
  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo                                 

  if @subjectMarks is not null
  begin 
      
/*
  Для потомков и наверное для меня самого на будущее.
  Описание задания
  
  Нужно проверять если оценки полностью совпадают с одним из свидетельств (с номером или без номера), то выдавать это свидетельство. 
  Если есть оценки и с того и другого свидетельства то возвращать пусто
  
  Кроме того тут заложена логика проверки корректности введенных баллов. Если один из баллов введен не правильно, то вовращать пусто.
  Эта проверка должна осуществляться после первого правила (см выше) 
  
  Проблема 
  Так как для некоторых проверок нет номера свидетельства то приходится связываться по участнику. 
  Но если связываться по участнику, то выдаются все свидетества для этого учатника. Проблема была решена как....
  
  Описание логики работы
  Делаем два запроса для свидетельства с номером и без него.
  Запоминаем ИД сертификата и код предмета
  Смотрим в сформированную таблицу если есть повторяющиеся баллы то удаляем те записи где есть это повторение и нет свидетельства
  
  Смотрим есть после этого в таблице есть два разных свидетельства (без номера тоже считается), то вовращаем пустую проверку
    Иначе смотрим есть свидетельство с номером, то удаляем из @Search свидетельство без номера и осуществляем проверку кооректности введеных баллов
                          иначе удаляем из @Search свидетельство с номером и осуществляем проверку кооректности введеных баллов
*/                       
    create table #tt(id int primary key identity(1,1),code int ,CertificateId uniqueidentifier) 
                                    
    insert #tt
    select distinct certificate_subject.SubjectCode,inner_search.CertificateId 
    from [prn].CertificatesMarks certificate_subject with(nolock) 
      join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
          and 1=case when inner_search.CertificateId = certificate_subject.CertificateFK then 1             
                else 0  end 
      join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark                         
    where inner_search.CertificateId is not null 
    union all
    select distinct certificate_subject.SubjectCode,inner_search.CertificateId 
    from [prn].CertificatesMarks certificate_subject with(nolock) 
      join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                 and 1= case when inner_search.ParticipantID = certificate_subject.ParticipantFK then 1 
                else 0 end 
      join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark                         
    where inner_search.CertificateId is null        
            
    if exists(select * from #tt group by code having count(code)>1)
      delete a from #tt a 
        join (select code from #tt group by code having count(code)>1) b on a.code=b.code 
      where CertificateId is null 
                
    if exists(select * from #tt having count(distinct isnull(CertificateId,'22655BE0-C368-4EB8-8835-5E0F7BA807B5'))>1)
    begin     
      delete search 
      from @Search search 
      
      delete #tt
    end     
    
    if exists(select * from #tt where CertificateId is not null)  
    begin               
      delete search 
      from @Search search 
      where CertificateId is null
      
      delete search 
      from @Search search 
      where exists(select 1 
            from [prn].CertificatesMarks certificate_subject with(nolock) 
              inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                  and search.CertificateId = inner_search.CertificateId
                  and inner_search.CertificateId = certificate_subject.CertificateFK
              right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark 
            where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)      
            
    end       
            
    if exists(select * from #tt where CertificateId is null)  
    begin               
      delete search 
      from @Search search 
      where CertificateId is not null
                    
      delete search 
      from @Search search 
      where exists(select 1 
            from [prn].CertificatesMarks certificate_subject with(nolock) 
              inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                  and search.ParticipantID = inner_search.ParticipantID
                  and inner_search.ParticipantID = certificate_subject.ParticipantFK
              right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark 
            where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)            
    end
    
    drop table #tt    
  end

  set @sourceEntityIds = ''
  
  select @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), search.CertificateId) 
  from @Search search 
  
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
    
  if @sourceEntityIds = ''
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

  declare @Search1 table 
  ( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
  )     
  insert @Search1
    select distinct S.CertificateId 
    from @Search S   
  where CertificateId is not null

  declare @CertificateId uniqueidentifier,@pkid int
  while exists(select * from @Search1)
  begin
    select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 
      
  select 
      isnull(cast(S.certificateId as nvarchar(255)),'Нет свидетельства') CertificateId,
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.LastName is not null then 1 
        else 0 
      end IsExist, 
      case 
        when CD.UseYear is not null then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when CD.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно' 
                else 'Истек срок' 
      end as [Status], 
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
      S.ParticipantID ParticipantID
    from 
      @Search S 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.idGUID  = S.CertificateId 
      left outer join prn.CancelledCertificates CD with (nolock) 
        on CD.[UseYear]=S.[Year] 
                and S.CertificateId = CD.CertificateFK 
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed with(nolock)
              on ed.[year] = S.[year]
            
  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null 
      
  return 0
end
go
ALTER proc [dbo].[SearchCommonNationalExamCertificatePassport]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
as
begin
  declare 
    @eventCode nvarchar(255)
        , @organizationId bigint
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(4000)
    , @internalPassportSeria nvarchar(255)

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  set @eventParams = 
    isnull(@lastName,'') + '|' 
    + isnull(@firstName,'') + '|' 
    + isnull(@patronymicName,'') + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') 

  select
    @editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  if @year is not null
    select @yearFrom = @year, @yearTo = @year
  else
    select @yearFrom = 2008
      select @yearTo = Year(GetDate())

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  set @commandText = ''
  set @eventCode = N'CNE_FND_TN'

  declare @CId bigint,@sourceEntityIds nvarchar(4000) 
  declare @Search table 
  ( 
    LastName nvarchar(255) 
    , FirstName nvarchar(255) 
    , PatronymicName nvarchar(255) 
    , CertificateId uniqueidentifier 
    , CertificateNumber nvarchar(255) 
    , RegionId int 
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255) 
    , TypographicNumber nvarchar(255) 
    , Year int
    , ParticipantID uniqueidentifier
  ) 
    
  set @commandText = @commandText +       
    'select top 300 a.Surname, a.Name, a.SecondName, b.CertificateID, b.LicenseNumber,
            b.Region, isnull(a.DocumentSeries, @internalPassportSeria), 
            isnull(a.DocumentNumber, @passportNumber), b.TypographicNumber, b.UseYear, a.ParticipantID 
    from 
		rbd.Participants AS a WITH (nolock) 
		JOIN prn.CertificatesMarks AS cm WITH (nolock) ON cm.ParticipantFK = a.ParticipantID AND cm.UseYear = a.UseYear 
		LEFT JOIN prn.Certificates AS b WITH (nolock) ON b.CertificateID = cm.CertificateFK AND b.UseYear = cm.UseYear 
		LEFT JOIN prn.CancelledCertificates AS c WITH (nolock) ON c.CertificateFK = b.CertificateID AND c.UseYear = b.UseYear   
    where 
    a.[UseYear] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and a.Surname collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and a.Name collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and a.SecondName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentSeries like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and a.DocumentSeries = @internalPassportSeria '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentNumber like @passportNumber '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and a.DocumentNumber = @passportNumber '
        end
    end
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @yearFrom, @YearTo

  set @sourceEntityIds = ''

  select @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), search.CertificateId) 
  from @Search search 
  
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
    
  if @sourceEntityIds = ''
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

  declare @Search1 table 
  ( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
  )     
  insert @Search1
    select distinct S.CertificateId 
    from @Search S   
  where CertificateId is not null
  
  declare @CertificateId uniqueidentifier,@pkid int
  while exists(select * from @Search1)
  begin
    select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 

  select 
      isnull(cast(S.CertificateId as nvarchar(500)),'Нет свидетельства')CertificateId, 
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.LastName is not null then 1 
        else 0 
      end IsExist, 
      case 
        when CD.UseYear is not null then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when CD.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно' 
                else 'Истек срок' 
      end as [Status], 
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
      S.ParticipantID ParticipantID
    from 
      @Search S 
		left outer join ExamCertificateUniqueChecks CC with (nolock) on CC.idGUID  = S.CertificateId 
		left outer join prn.CancelledCertificates CD with (nolock) on CD.[UseYear] = S.[year] and S.CertificateId = CD.CertificateFK
		left join dbo.Region region with (nolock) on region.[Id] = S.RegionId 
		left join [ExpireDate] ed on ed.[year] = S.[year]
            
  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null 
      
  return 0
end

go
ALTER proc [dbo].[SearchCommonNationalExamCertificatePassport]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
as
begin
  declare 
    @eventCode nvarchar(255)
        , @organizationId bigint
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)
    , @yearFrom int
    , @yearTo int
    , @commandText nvarchar(4000)
    , @internalPassportSeria nvarchar(255)

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  set @eventParams = 
    isnull(@lastName,'') + '|' 
    + isnull(@firstName,'') + '|' 
    + isnull(@patronymicName,'') + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') 

  select
    @editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  if @year is not null
    select @yearFrom = @year, @yearTo = @year
  else
    select @yearFrom = 2008
      select @yearTo = Year(GetDate())

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  set @commandText = ''
  set @eventCode = N'CNE_FND_TN'

  declare @CId bigint,@sourceEntityIds nvarchar(4000) 
  declare @Search table 
  ( 
    LastName nvarchar(255) 
    , FirstName nvarchar(255) 
    , PatronymicName nvarchar(255) 
    , CertificateId uniqueidentifier 
    , CertificateNumber nvarchar(255) 
    , RegionId int 
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255) 
    , TypographicNumber nvarchar(255) 
    , Year int
    , ParticipantID uniqueidentifier
  ) 
    
  set @commandText = @commandText +         
    'select * from 
     (
		select distinct a.Surname, a.Name, a.SecondName, b.CertificateID, b.LicenseNumber,
            b.Region, isnull(a.DocumentSeries, @internalPassportSeria) DocumentSeries, 
            isnull(a.DocumentNumber, @passportNumber) DocumentNumber, b.TypographicNumber, b.UseYear, a.ParticipantID 
		from 
			rbd.Participants AS a WITH (nolock) 
			JOIN prn.CertificatesMarks AS cm WITH (nolock) ON cm.ParticipantFK = a.ParticipantID AND cm.UseYear = a.UseYear 
			LEFT JOIN prn.Certificates AS b WITH (nolock) ON b.CertificateID = cm.CertificateFK AND b.UseYear = cm.UseYear 
			LEFT JOIN prn.CancelledCertificates AS c WITH (nolock) ON c.CertificateFK = b.CertificateID AND c.UseYear = b.UseYear   
		where a.[UseYear] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and a.Surname collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and a.Name collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and a.SecondName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentSeries like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and a.DocumentSeries = @internalPassportSeria '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and a.DocumentNumber like @passportNumber '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and a.DocumentNumber = @passportNumber '
        end
    end
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  set @commandText = @commandText + 
  ' ) t'
  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @yearFrom, @YearTo

  set @sourceEntityIds = ''

  select @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), search.CertificateId) 
  from @Search search 
  
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
    
  if @sourceEntityIds = ''
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

  declare @Search1 table 
  ( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
  )     
  insert @Search1
    select distinct S.CertificateId 
    from @Search S   
  where CertificateId is not null
  
  declare @CertificateId uniqueidentifier,@pkid int
  while exists(select * from @Search1)
  begin
    select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 

  select 
      isnull(cast(S.CertificateId as nvarchar(500)),'Нет свидетельства')CertificateId, 
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.LastName is not null then 1 
        else 0 
      end IsExist, 
      case 
        when CD.UseYear is not null then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when CD.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно' 
                else 'Истек срок' 
      end as [Status], 
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
      S.ParticipantID ParticipantID
    from 
      @Search S 
		left outer join ExamCertificateUniqueChecks CC with (nolock) on CC.idGUID  = S.CertificateId 
		left outer join prn.CancelledCertificates CD with (nolock) on CD.[UseYear] = S.[year] and S.CertificateId = CD.CertificateFK
		left join dbo.Region region with (nolock) on region.[Id] = S.RegionId 
		left join [ExpireDate] ed on ed.[year] = S.[year]
            
  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null 
      
  return 0
end