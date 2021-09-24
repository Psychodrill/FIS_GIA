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
      , isnull(b.UseYear,a.UseYear) UseYear
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
