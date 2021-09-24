USE [fbs]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificate]    Script Date: 07/19/2013 14:30:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[SearchCommonNationalExamCertificate]
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
