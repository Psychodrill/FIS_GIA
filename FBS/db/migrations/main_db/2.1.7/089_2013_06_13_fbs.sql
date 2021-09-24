insert into Migrations(MigrationVersion, MigrationName) values (89, '089_2013_06_13_fbs.sql')

alter table CommonNationalExamCertificateRequest alter column SourceCertificateIdGuid nvarchar(100)
go
if not exists(select * from sys.columns where name='ParticipantID' and object_id('CommonNationalExamCertificateRequest')=object_id)
 alter table CommonNationalExamCertificateRequest add ParticipantID uniqueidentifier
go

IF  EXISTS (SELECT * FROM sys.key_constraints WHERE name = N'CertificateIniqueChecksPK')
ALTER TABLE [dbo].[ExamCertificateUniqueChecks] DROP CONSTRAINT [CertificateIniqueChecksPK]
GO

if exists(select * from sys.key_constraints  where name='PK_ExamCertificateUniqueChecks')
ALTER TABLE [dbo].[ExamCertificateUniqueChecks] drop  CONSTRAINT [PK_ExamCertificateUniqueChecks]
alter table ExamCertificateUniqueChecks alter column Id bigint NULL

go
if not exists(select * from sys.default_constraints  where name='DF_ExamCertificateUniqueChecks_idGUID')
begin
update ExamCertificateUniqueChecks set idGUID =newid() where idGUID  is null
ALTER TABLE dbo.ExamCertificateUniqueChecks ADD CONSTRAINT
 DF_ExamCertificateUniqueChecks_idGUID DEFAULT newid() FOR idGUID
alter table ExamCertificateUniqueChecks alter column idGUID uniqueidentifier NOT NULL
end

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ExamCertificateUniqueChecks]') AND name = N'IExamCertificateUniqueChecks_Id')
DROP INDEX [IExamCertificateUniqueChecks_Id] ON [dbo].[ExamCertificateUniqueChecks] WITH ( ONLINE = OFF )
GO

if not exists(select * from sys.key_constraints  where name='PK_ExamCertificateUniqueChecks')
ALTER TABLE [dbo].[ExamCertificateUniqueChecks] ADD  CONSTRAINT [PK_ExamCertificateUniqueChecks] PRIMARY KEY CLUSTERED 
(
  [Year] ASC,
  [idGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

if not exists(select * from sys.columns where name='id' and object_id('OrganizationCertificateChecks')=object_id)
 alter table OrganizationCertificateChecks add id bigint IDENTITY(1,1) NOT NULL
go
IF  EXISTS (SELECT * FROM sys.key_constraints WHERE name = N'PK__Organiza__875517703D24DE9E')
ALTER TABLE [dbo].OrganizationCertificateChecks DROP CONSTRAINT [PK__Organiza__875517703D24DE9E]
GO

alter table OrganizationCertificateChecks alter column CertificateId bigint NULL
go
alter table OrganizationCertificateChecks alter column CertificateIdGuid uniqueidentifier NULL
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationCertificateChecks]') AND name = N'PK__OrganizationCert__537E3411')
ALTER TABLE [dbo].[OrganizationCertificateChecks] DROP CONSTRAINT [PK__OrganizationCert__537E3411]
GO

if not exists(select * from sys.key_constraints  where name='PK_OrganizationCertificateChecks')
ALTER TABLE [dbo].[OrganizationCertificateChecks] ADD  CONSTRAINT [PK_OrganizationCertificateChecks] PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[SearchAskedQuestion]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAskedQuestion

-- =============================================
-- Получение списка вопросов.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
alter proc [dbo].[SearchAskedQuestion]
  @name nvarchar(255) = null
  , @isActive bit = null
  , @contextCodes nvarchar(4000) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20)
  , @sortAsc bit = 1
  , @showCount bit = null
as
begin
  declare 
    @nameFormat nvarchar(255)
    , @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)

  if isnull(@name, '') <> ''
    set @nameFormat = '%' + replace(@name, ' ', '%') + '%'

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Id bigint ' +
      ' , Name nvarchar(255) ' +
      ' , Question ntext ' +
      ' , IsActive bit ' +
      ' , Popularity decimal(18,4) ' +
      ' ) ' 

  if isnull(@contextCodes, '') <> ''
    set @declareCommandText = @declareCommandText + 
      'declare @codes table '+
      ' ( ' +
      ' Code nvarchar(255) ' +
      ' ) ' +
      'insert @codes select value from dbo.GetDelimitedValues(@contextCodes) '

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' asked_question.Id Id ' +
        ' , asked_question.Name Name ' +
        ' , asked_question.Question Question ' +
        ' , asked_question.IsActive IsActive ' +
        ' , asked_question.Popularity Popularity ' +
        'from dbo.AskedQuestion asked_question with (nolock) ' +
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.AskedQuestion asked_question with (nolock) ' +
        'where 1 = 1 ' 

  if not @nameFormat is null  
    set @commandText = @commandText + ' and asked_question.Name like @nameFormat '

  if not @isActive is null
    set @commandText = @commandText + ' and asked_question.IsActive = @isActive '

  if not @contextCodes is null
    set @commandText = @commandText + ' and not exists(select 1 ' +
        '   from @codes context_codes ' +
        '     inner join dbo.Context context ' +
        '       on context.Code = context_codes.Code ' +
        '     left outer join dbo.AskedQuestionContext asked_question_context with(nolock) ' +
        '       on asked_question_context.ContextId = context.Id ' +
        '         and asked_question_context.AskedQuestionId = asked_question.Id ' +
        '   where asked_question_context.Id is null) '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'Name'
    begin
      set @innerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Name <orderDirection>, Id <orderDirection> '
    end
    else if @sortColumn = 'IsActive'
    begin
      set @innerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' dbo.GetExternalId(search.Id) Id ' +
      ' , search.Name ' +
      ' , search.Question ' +
      ' , search.IsActive ' +
      ' , search.Popularity ' +
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@nameFormat nvarchar(255), @isActive bit, @contextCodes nvarchar(4000)'
    , @nameFormat
    , @IsActive
    , @contextCodes

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountAuthenticationLog]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAccountAuthenticationLog
-- =============================================
-- Поиск в логе аутентификации записей об аккаунте
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- v.1.1: Modified by Fomin Dmitriy 20.05.2008
-- Добавлено поле IsVpnIp.
-- v.1.2: Modified by Fomin Dmitriy 20.05.2008
-- Анонимное событие на регистрацию проводит 
-- неявную аутентификацию.
-- v.1.3: Modified by Sedov A.G. 22.05.2008
-- Переделана выборка данных, выборка теперь 
-- выполняется из dbo.AuthenticationEventLog 
-- =============================================
alter procedure [dbo].[SearchAccountAuthenticationLog]
  @login nvarchar(255)
  , @startRowIndex int = null 
  , @maxRowCount int = null 
  , @showCount bit = null 
as
begin
  declare
    @declareCommandText nvarchar(4000)
    , @params nvarchar(4000)
    , @commandText nvarchar(4000) 
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortAsc bit
    , @verifyEventCode nvarchar(255)
    , @registrationEventCode nvarchar(255)

  set @verifyEventCode = 'USR_VERIFY'
  set @registrationEventCode = 'USR_REG'

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = '' 

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Date datetime ' +
      ' , Ip nvarchar(255) ' +
      ' , IsPasswordValid bit ' +
      ' , IsIpValid bit ' +
      ' ) ' 
  
  
  if isnull(@showCount, 0) = 0
    set @commandText = 
      'select <innerHeader> ' +
      ' auth_log.Date Date ' +
      ' , auth_log.Ip Ip ' + 
      '   , auth_log.IsPasswordValid ' + 
      ' , auth_log.IsIpValid ' + 
      'from ' + 
      ' dbo.AuthenticationEventLog auth_log with (nolock) ' + 
      '   inner join dbo.Account account with (nolock, fastfirstrow) ' + 
      '     on account.Id = auth_log.AccountId ' + 
      'where 1 = 1 ' 
  else
    set @commandText = 
      'select count(*) ' +
      'from ' + 
      ' dbo.AuthenticationEventLog auth_log with (nolock) ' +
      '   inner join dbo.Account account with (nolock, fastfirstrow) ' +
      '     on account.Id = auth_log.AccountId ' +
      'where 1 = 1 '

  set @commandText = @commandText +
    ' and account.[Login] = @login '

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = 'order by Date <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end   

  set @commandText = @commandText + 
    'option (keepfixed plan) '
  
  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' search.Date ' +
      ' , search.Ip ' +
      ' , search.IsPasswordValid ' +
      ' , search.IsIpValid ' +
      ' , case ' +
      '   when exists(select 1 ' +
      '       from dbo.VpnIp vpn_ip ' +
      '       where vpn_ip.Ip = search.Ip) then 1 ' +
      '   else 0 ' +
      ' end IsVpnIp ' +
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText

  set @params = 
      '@login nvarchar(255) ' +  
      ', @verifyEventCode varchar(100) ' +
      ', @registrationEventCode varchar(100) '

  exec sp_executesql @commandText, @params
      , @login 
      , @verifyEventCode
      , @registrationEventCode

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchDocument]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchDocument

-- =============================================
-- Получение списка документов.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- v.1.1: Modified by Fomin Dmitriy 18.04.2008
-- Добавлена фильтрация по наименованию.
-- v.1.2: Modified by Makarev Andrey 19.04.2008
-- Правильный вывод ИД.
-- v.1.3: Modified by Fomin Dmitriy 21.04.2008
-- Убраны лишние поля.
-- v.1.4: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле RelativeUrl.
-- =============================================
alter proc [dbo].[SearchDocument]
  @isActive bit = null
  , @contextCodes nvarchar(4000) = null
  , @name nvarchar(255) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = N'Id'
  , @sortAsc bit = 0
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @nameFormat nvarchar(255)

  if isnull(@name, '') <> ''
    set @nameFormat = '%' + replace(@name, ' ' , '%') + '%'

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Id bigint ' +
      ' , Name nvarchar(255) ' +
      ' , Description ntext ' +
      ' , IsActive bit ' +
      ' , ActivateDate datetime ' +
      ' , ContextCodes nvarchar(4000) ' +
      ' , RelativeUrl nvarchar(255) ' +
      '   , Date datetime ' +
      ' ) ' 

  if isnull(@contextCodes, '') <> ''
    set @declareCommandText = @declareCommandText + 
      'declare @codes table '+
      ' ( ' +
      ' Code nvarchar(255) ' +
      ' ) ' +
      'insert @codes select value from dbo.GetDelimitedValues(@contextCodes) '

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' document.Id Id ' +
        ' , document.Name Name ' +
        ' , document.Description Description ' +
        ' , document.IsActive IsActive ' +
        ' , document.ActivateDate ActivateDate ' +
        ' , document.ContextCodes ContextCodes ' +
        ' , document.RelativeUrl RelativeUrl ' +
        ' , document.UpdateDate Date ' +
        'from dbo.Document document with (nolock) ' +
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.Document document with (nolock) ' +
        'where 1 = 1 ' 
  
  if not @isActive is null
    set @commandText = @commandText + ' and document.IsActive = @isActive '

  if not @contextCodes is null
    set @commandText = @commandText + ' and not exists(select 1 ' +
        '   from @codes context_codes ' +
        '     inner join dbo.Context context ' +
        '       on context.Code = context_codes.Code ' +
        '     left outer join dbo.DocumentContext document_context with(nolock) ' +
        '       on document_context.ContextId = context.Id ' +
        '         and document_context.DocumentId = document.Id ' +
        '   where document_context.Id is null) '

  if not @nameFormat is null
    set @commandText = @commandText + ' and document.Name like @nameFormat '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'Name'
    begin
      set @innerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Name <orderDirection>, Id <orderDirection> '
    end
    else if @sortColumn = 'IsActive'
    begin
      set @innerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
    end
    else if @sortColumn = 'Date'
    begin
      set @innerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection>, Id <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by Id <orderDirection> '
      set @outerOrder = 'order by Id <orderDirection> '
      set @resultOrder = 'order by Id <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' dbo.GetExternalId(search.Id) Id ' +
      ' , search.Name ' +
      ' , search.Description ' +
      ' , search.IsActive ' +
      ' , search.ActivateDate ' +
      ' , search.ContextCodes ' +
      ' , search.RelativeUrl ' +
      ' , search.Date ' +
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@isActive bit, @contextCodes nvarchar(4000), @nameFormat nvarchar(255)'
    , @IsActive
    , @contextCodes
    , @nameFormat

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchDeliveries]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение списка рассылок.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
alter proc [dbo].[SearchDeliveries]
  @title nvarchar(255) = null
  , @createDateFrom datetime = null
  , @createDateTo datetime = null
  , @deliveryDateFrom datetime = null
  , @deliveryDateTo datetime = null
  , @status int = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = null
  , @sortAsc bit = 1
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @titleFormat nvarchar(255)

  if isnull(@title, '') <> ''
    set @titleFormat = '%' + replace(@title, ' ' , '%') + '%'

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Id bigint ' +
      ' , CreateDate datetime ' +
      ' , DeliveryDate datetime ' +
      ' , TypeCode nvarchar(20) ' +
      ' , Title nvarchar(255) ' +
      ' , Status int ' +
      ' , StatusName nvarchar(255) ' +
      ' ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' delivery.Id Id ' +
        ' , delivery.CreateDate CreateDate ' +
        ' , delivery.DeliveryDate DeliveryDate ' +
        ' , delivery.TypeCode TypeCode ' +
        ' , delivery.Title Title ' +
        ' , delivery.Status Status ' +
        ' , status.Name StatusName ' +
        'from dbo.Delivery delivery with (nolock) ' +
        'inner join DeliveryStatus status on delivery.Status=status.Id '+
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.Delivery delivery with (nolock) ' +
        'where 1 = 1 ' 
  
  if not @status is null
    set @commandText = @commandText + ' and delivery.Status = @status '

  if not @createDateFrom is null
    set @commandText = @commandText + ' and delivery.CreateDate >= @createDateFrom '

  if not @createDateTo is null
    set @commandText = @commandText + ' and delivery.CreateDate <= @createDateTo '

  if not @deliveryDateFrom is null
    set @commandText = @commandText + ' and delivery.DeliveryDate >= @deliveryDateFrom '

  if not @deliveryDateTo is null
    set @commandText = @commandText + ' and delivery.DeliveryDate <= @deliveryDateTo '

  if not @title is null
    set @commandText = @commandText + ' and delivery.Title like @titleFormat '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N'Title'
    begin
      set @innerOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
    end
    else if @sortColumn = N'StatusName'
    begin
      set @innerOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
    end
    else if @sortColumn = N'CreateDate'
    begin
      set @innerOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
    end
    else
    begin 
      set @innerOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '

    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' search.Id Id ' +
      ' , search.CreateDate ' +
      ' , search.DeliveryDate ' +
      ' , search.TypeCode ' +
      ' , search.Title ' +
      ' , search.Status ' +
      ' , search.StatusName ' +
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@status int, @createDateFrom datetime, @createDateTo datetime, @deliveryDateFrom datetime, @deliveryDateTo datetime, @titleFormat nvarchar(255)'
    , @status
    , @createDateFrom
    , @createDateTo
    , @deliveryDateFrom
    , @deliveryDateTo
    , @titleFormat

  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromStringWithId]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter funCTION [dbo].[ufn_ut_SplitFromStringWithId]
(
  @string nvarchar(max),
  @delimeter nvarchar(1) = ' ')
RETURNS @ret TABLE (id int identity(1,1) ,val nvarchar(4000) )
AS
BEGIN
  DECLARE @s int, @e int

  SET @s = 0
  WHILE CHARINDEX(@delimeter,@string,@s) <> 0
  BEGIN
    SET @e = CHARINDEX(@delimeter,@string,@s)
    INSERT @ret VALUES (SUBSTRING(@string,@s,@e - @s))
    SET @s = @e + 1
  END
  INSERT @ret VALUES (SUBSTRING(@string,@s,4000))
  RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromString]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ufn_ut_SplitFromString] 
(
  @string nvarchar(max),
  @delimeter nvarchar(1) = ' '
)
returns @ret table (nam nvarchar(1000) )
as
begin
  if len(@string)=0 
    return 
  declare @s int, @e int
  set @s = 0
  while charindex(@delimeter,@string,@s) <> 0
  begin
    set @e = charindex(@delimeter,@string,@s)
    insert @ret values (rtrim(ltrim(substring(@string,@s,@e - @s))))
    set @s = @e + 1
  end
  insert @ret values (rtrim(ltrim(substring(@string,@s,300))))
  return
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateLoadingTask]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение списка заданий на загрузку сертификатов.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateLoadingTask]
  @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @server nvarchar(30)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  
  select @server = (select top 1 ss.name + '.fbs_loader_db' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = 'loader')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'declare @search table ' +
      N'  ( ' +
      N'  Id bigint ' +
      N'  , UpdateDate datetime ' +
      N'  , EditorAccountId bigint ' +
      N'  , EditorIp nvarchar(255) ' +
      N'  , SourceBatchUrl nvarchar(255) ' +
      N'  , IsActive bit ' +
      N'  , IsProcess bit ' +
      N'  , IsCorrect bit ' +
      N'  , IsLoaded bit ' +
      N'  , ErrorCount int ' +
      N'  ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N'select <innerHeader> ' +
        N'  cne_certificate_loading_task.Id Id ' +
        N'  , cne_certificate_loading_task.CreateDate UpdateDate ' +
        N'  , cne_certificate_loading_task.EditorAccountId EditorAccountId ' +
        N'  , cne_certificate_loading_task.EditorIp EditorIp ' +
        N'  , cne_certificate_loading_task.SourceBatchUrl SourceBatchUrl ' +
        N'  , cne_certificate_loading_task.IsActive IsActive ' +
        N'  , cne_certificate_loading_task.IsProcess IsProcess ' +
        N'  , cne_certificate_loading_task.IsCorrect IsCorrect ' +
        N'  , cne_certificate_loading_task.IsLoaded IsLoaded ' +
        N'  , 0 ErrorCount  ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task with (nolock) '
  else
    set @commandText = 
        N'select count(*) ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task with (nolock) ' 
  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = N'order by Id desc '
      set @outerOrder = N'order by Id asc '
      set @resultOrder = N'order by Id desc '
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N'top 10000000'
      set @outerSelectHeader = N'top 10000000'
    end

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @commandText = @commandText +
      N'update search '+
      N'set ' +
      N'  ErrorCount = ( '+
      N'    select '+ 
      N'      count(*)  '+
      N'    from  '+
      N'      ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error '+
      N'    where '+
      N'      cne_certificate_loading_task_error.TaskId = search.Id '+
      N'    ) '+
      N'from '+
      N'  @search search '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N'select ' +
      N'  dbo.GetExternalId(search.Id) Id ' +
      N'  , search.UpdateDate UpdateDate ' +
      N'  , account.Login EditorLogin ' +
      N'  , account.LastName EditorLastName ' +
      N'  , account.FirstName EditorFirstName ' +
      N'  , account.PatronymicName EditorPatronymicName ' +
      N'  , search.EditorIp EditorIp ' +
      N'  , search.SourceBatchUrl SourceBatchUrl ' +
      N'  , search.IsActive IsActive ' +
      N'  , search.IsProcess IsProcess ' +
      N'  , search.IsCorrect IsCorrect ' +
      N'  , search.IsLoaded IsLoaded ' +
      N'  , search.ErrorCount ErrorCount ' +
      N'from ' +
      N'  @search search ' + 
      N'    left join dbo.Account account ' +
      N'      on search.EditorAccountId = account.Id ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение списка заданий на загрузку закрытий сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]
  @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @server nvarchar(40)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''

  select @server = (select top 1 ss.name + '.fbs_loader_db' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = 'loader')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'declare @search table ' +
      N'  ( ' +
      N'  Id bigint ' +
      N'  , UpdateDate datetime ' +
      N'  , EditorAccountId bigint ' +
      N'  , EditorIp nvarchar(255) ' +
      N'  , SourceBatchUrl nvarchar(255) ' +
      N'  , IsActive bit ' +
      N'  , IsProcess bit ' +
      N'  , IsCorrect bit ' +
      N'  , IsLoaded bit ' +
      N'  , ErrorCount int ' +
      N'  ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N'select <innerHeader> ' +
        N'  cne_certificate_deny_loading_task.Id Id ' +
        N'  , cne_certificate_deny_loading_task.CreateDate UpdateDate ' +
        N'  , cne_certificate_deny_loading_task.EditorAccountId EditorAccountId ' +
        N'  , cne_certificate_deny_loading_task.EditorIp EditorIp ' +
        N'  , cne_certificate_deny_loading_task.SourceBatchUrl SourceBatchUrl ' +
        N'  , cne_certificate_deny_loading_task.IsActive IsActive ' +
        N'  , cne_certificate_deny_loading_task.IsProcess IsProcess ' +
        N'  , cne_certificate_deny_loading_task.IsCorrect IsCorrect ' +
        N'  , cne_certificate_deny_loading_task.IsLoaded IsLoaded ' +
        N'  , 0 ErrorCount  ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTask cne_certificate_deny_loading_task with (nolock) '
  else
    set @commandText = 
        N'select count(*) ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTask cne_certificate_deny_loading_task with (nolock) '
  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = N'order by Id desc '
      set @outerOrder = N'order by Id asc '
      set @resultOrder = N'order by Id desc '
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N'top 10000000'
      set @outerSelectHeader = N'top 10000000'
    end

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @commandText = @commandText +
      N'update search '+
      N'set ' +
      N'  ErrorCount = ( '+
      N'    select '+ 
      N'      count(*)  '+
      N'    from  '+
      N'      ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error '+
      N'    where '+
      N'      cne_certificate_deny_loading_task_error.TaskId = search.Id '+
      N'    ) '+
      N'from '+
      N'  @search search '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N'select ' +
      N'  dbo.GetExternalId(search.Id) Id ' +
      N'  , search.UpdateDate UpdateDate ' +
      N'  , account.Login EditorLogin ' +
      N'  , account.LastName EditorLastName ' +
      N'  , account.FirstName EditorFirstName ' +
      N'  , account.PatronymicName EditorPatronymicName ' +
      N'  , search.EditorIp EditorIp ' +
      N'  , search.SourceBatchUrl SourceBatchUrl ' +
      N'  , search.IsActive IsActive ' +
      N'  , search.IsProcess IsProcess ' +
      N'  , search.IsCorrect IsCorrect ' +
      N'  , search.IsLoaded IsLoaded ' +
      N'  , search.ErrorCount ErrorCount ' +
      N'from ' +
      N'  @search search ' + 
      N'    left join dbo.Account account ' +
      N'      on search.EditorAccountId = account.Id ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SelectCNECCheckHystoryByOrgIdCount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
        set @str=@str+' '         
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
          left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=cast(r.Id as nvarchar(255)) 
          JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
    if @LastName is not null
      set @str=@str+'
              and c.LastName like ''%''+@LastName+''%'' '           
    if @TypeCheck= 'Пакетная' or @TypeCheck is null
        set @str=@str+' '         
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
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '
    set @str=@str+'                         
          left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255))  where 1 = 1 '  
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
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id '
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
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=cast(r.Id as nvarchar(255)) '
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
            select cb.id,ISNULL(c.Number, cb.CNENumber) CertificateNumber
            from 
            (
              select max(cb.id) id,c.Number from
              (
                select cb.id,cb.FoundedCNEId
                from CNEWebUICheckLog cb  with(index(I_CNEWebUICheckLog_AccId))
                  join Account Acc with(index(accOrgIdIndex)) on acc.id=cb.AccountId  
                  join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 ' 
    if @TypeCheck= 'Интерактивная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '                   
    if @idorg<>-1 
      set @str=@str+'
                where @idorg=Acc.OrganizationId '               
    set @str=@str+'   
              ) cb
              left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255))             
              group by c.Number 
            ) cb1
              join CNEWebUICheckLog cb on cb1.id=cb.id
              left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255)) '               
    set @str=@str+'               
              left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'''
    set @str=@str+'             
          ) t
          ) t
          
    select count(distinct CertificateNumber) from
    (
      select * from
        (
        select a.* from #ttt a
          join (select min(id)id,CertificateNumber from #ttt group by CertificateNumber) b on a.id=b.id
        ) t
    ) t
    
    drop table #ttt '
    
  end
  print @str
  exec sp_executesql @str,N'@idorg int,@LastName nvarchar(255)=null',@idorg=@idorg,@LastName=@LastName
    
end
GO
/****** Object:  StoredProcedure [dbo].[SearchNews]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchNews

-- =============================================
-- Получение списка новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим название новости.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Название новости добавлено в фильтр.
-- =============================================
alter proc [dbo].[SearchNews]
  @isActive bit = null
  , @dateFrom datetime = null
  , @dateTo datetime = null
  , @name nvarchar(255) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = null
  , @sortAsc bit = 1
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @nameFormat nvarchar(255)

  if isnull(@name, '') <> ''
    set @nameFormat = '%' + replace(@name, ' ' , '%') + '%'

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Id bigint ' +
      ' , Date datetime ' +
      ' , Description ntext ' +
      ' , Name nvarchar(255) ' +
      ' , IsActive bit ' +
      ' ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' news.Id Id ' +
        ' , news.Date Date ' +
        ' , news.Description Description ' +
        ' , news.Name Name ' +
        ' , news.IsActive IsActive ' +
        'from dbo.News news with (nolock) ' +
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.News news with (nolock) ' +
        'where 1 = 1 ' 
  
  if not @isActive is null
    set @commandText = @commandText + ' and news.IsActive = @isActive '

  if not @dateFrom is null
    set @commandText = @commandText + ' and news.Date >= @dateFrom '

  if not @dateTo is null
    set @commandText = @commandText + ' and news.Date <= @dateTo '

  if not @name is null
    set @commandText = @commandText + ' and news.Name like @nameFormat '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N'Name'
    begin
      set @innerOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
    end
    else if @sortColumn = N'IsActive'
    begin
      set @innerOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
    end
    else 
    begin
      set @innerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection>, Id <orderDirection> '
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' dbo.GetExternalId(search.Id) Id ' +
      ' , search.Date ' +
      ' , search.Description ' +
      ' , search.Name ' +
      ' , search.IsActive ' +
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@isActive bit, @dateFrom datetime, @dateTo datetime, @nameFormat nvarchar(255)'
    , @IsActive
    , @dateFrom
    , @dateTo
    , @nameFormat

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchVUZ]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[SearchVUZ] 
(@orgNamePrefix varchar(256))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @mask varchar(260)
  SET @mask='%'+replace(replace(@orgNamePrefix,'%','[%]'),'*','%')+'%'
  
  SELECT OrgName 
  FROM dbo.OrgEtalon 
  WHERE OrgName LIKE @mask
  ORDER BY OrgName
END
GO
/****** Object:  UserDefinedFunction [dbo].[HasUserAccountAdminComment]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Имеет ли учетная запись пользователя комментарий администратора.
-- v1.0: Created by Makarev Andrey 04.04.2008
--------------------------------------------------
alter function [dbo].[HasUserAccountAdminComment]
  (
  @status nvarchar(255)
  )
returns bit 
as  
begin
  return case
      when @status in (N'deactivated', N'revision') then 1
      else 0
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatusOrder]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Порядковый номер статуса.
-- v.1.0: Created by Fomin Dmitriy 11.04.2008
--------------------------------------------------
alter function [dbo].[GetUserStatusOrder]
  (
  @status nvarchar(255)
  )
returns int
as
begin
  return case
      when @status = 'consideration' then 1
      when @status = 'revision' then 2
      when @status = 'activated' then 3
      when @status = 'registration' then 4
      when @status = 'deactivated' then 5
      else 5
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatus]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Возвращает статус пользователя.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Убран параметр isActive, теперь он вычисляется 
-- на основании статуса, а не наоборот.
-- v.1.2: Modified by Fomin Dmitriy 19.04.2008
-- Статус корректируется автоматически.
--------------------------------------------------
alter function [dbo].[GetUserStatus]
  (
  @confirmYear int
  , @status nvarchar(255)
  , @currentYear int
  , @registrationDocument image
  )
returns nvarchar(255) 
as  
begin
  set @status = isnull(@status, N'registration')
  if @confirmYear < Year(GetDate()) 
    set @status = N'registration'

  return case
      when not @registrationDocument is null and @status = N'registration'
        then N'consideration'
      when @registrationDocument is null and not @status in (N'activated', N'deactivated')
        then N'registration'
      else @status
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserIsActive]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Возвращает состояние действующей записи пользователя.
-- v1.0: Created by Fomin Dmitriy 10.04.2008
--------------------------------------------------
alter function [dbo].[GetUserIsActive]
  (
  @status nvarchar(255)
  )
returns nvarchar(255) 
as  
begin
  return case
      when @status = N'deactivated' then 0
      else 1
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetSubjectMarks]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Разбивает исходную строку на части, разделенные 
-- запятыми и знаками =.
-- v.1.0: Created by Makarev Andrey 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Приведение к стандарту.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал через xml для оптимизации
-- v.1.3: Rewritten by Yusupov Kirill 1.07.2010
-- Переписал через цикл для оптимизации
--------------------------------------------------
alter function [dbo].[GetSubjectMarks]
  (
  @subjectMarks nvarchar(4000)
  )
returns @SubjectMark table (SubjectId int, Mark numeric(5,1))
--returns @SubjectMark table (SubjectId NVARCHAR(20), Mark NVARCHAR(20))
as
begin
  DECLARE @RawMark NVARCHAR(20)
  DECLARE @EQIndex INT
  WHILE (CHARINDEX(',',@subjectMarks)>0)
  BEGIN
    SET @RawMark= SUBSTRING(@subjectMarks,1,CHARINDEX(',',@subjectMarks)-1)

    SET @EQIndex=CHARINDEX('=',@RawMark)

    INSERT INTO @SubjectMark (SubjectId,Mark)
    SELECT 
      SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)

    SET @subjectMarks = SUBSTRING(@subjectMarks,CHARINDEX(',',@subjectMarks)+1,LEN(@subjectMarks))
  END
  IF (LEN(@subjectMarks)>0)
  BEGIN
    SET @RawMark= @subjectMarks

    SET @EQIndex=CHARINDEX('=',@RawMark)

    INSERT INTO @SubjectMark (SubjectId,Mark)
    SELECT 
      SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)
  END
  RETURN
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetExternalId]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
alter function [dbo].[GetExternalId]
  (
  @internalId bigint
  )
returns bigint
as
begin
  if isnull(@internalId, -1) < 0
    return null
  if @internalId = 0
    return 0

  declare
    @base bigint
    , @shift bigint
    , @shiftedId bigint

  set @base = power(2, 20)
  set @shift = 11541954384
  
  set @shiftedId = @internalId + @shift
  return (@shiftedId / @base) + (@shiftedId % @base) * @base
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetEventParam]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Возвращает значение параметра по его номеру, 
-- из строки с разделителем |.
-- v.1.0: Created by Sedov Anton 15.05.2008
-- Измение размера выходного массива.
-- =============================================
alter function [dbo].[GetEventParam]
  (
  @eventParams nvarchar(4000)
  , @index int
  )
returns nvarchar(4000) with schemabinding
as
begin
  declare 
    @delimiterIndex int
    , @startIndex int
    , @sourceParams nvarchar(4000)
    , @result nvarchar(4000)
  
  set @delimiterIndex = 1
  set @startIndex = 1
  set @sourceParams = Convert(nvarchar(4000), @eventParams) 
  set @delimiterIndex = charindex('|', isnull(@sourceParams, ''))
  
  if @delimiterIndex = 0
    return @sourceParams

  set @delimiterIndex = 1

  while @index <> 0
  begin
    set @startIndex = 1
    set @delimiterIndex = charindex('|', isnull(@sourceParams, ''))

    if @delimiterIndex = 0
      set @result = @sourceParams
    else
    begin
      set @result = substring(@sourceParams, @startIndex, @delimiterIndex - @startIndex)
      set @startIndex = @delimiterIndex
      set @sourceParams = substring(@sourceParams
          , @startIndex + 1, len(@sourceParams) - @startIndex) 
    end

    set @index = @index - 1
  end 
  
  return @result
end
GO
/****** Object:  UserDefinedFunction [dbo].[CompareStrings]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Длина сравниваемых строк увеличена.
-- =============================================
alter function [dbo].[CompareStrings]
  (
  @string1 nvarchar(4000)
  , @string2 nvarchar(4000)
  -- Чувствительность: кол-во совпадающих символов подстроки.
  , @matchCount int 
  )
returns decimal(18, 4)
as
begin
  declare
    @compareStr1 nvarchar(4000)
    , @compareStr2 nvarchar(4000)
    , @i int
    , @j int
    , @count1 int
    , @count int

  set @matchCount = isnull(@matchCount, 3)
  set @compareStr1 = replace(isnull(@string1, ''), ' ', '')
  set @compareStr2 = replace(isnull(@string2, ''), ' ', '')
  set @count = 0

  if @compareStr1 = @compareStr2
    return 1

  if len(@compareStr1) = 0 or len(@compareStr2) = 0
    return 0

  set @i = 1
  while @i < len(@compareStr1)
  begin
    set @j = 1
    while @j < len(@compareStr2)  
    begin
      if substring(@compareStr1, @i, 1) = substring(@compareStr2, @j, 1)
      begin
        set @count1 = 1
        while (@i + @count1 <= len(@compareStr1)) and (@j + @count1 <= len(@compareStr2))
            and (substring(@compareStr1, @i + @count1, 1) = substring(@compareStr2, @j + @count1, 1))
          set @count1 = @count1 + 1
        set @i = @i + @count1 - 1
        set @j = @j + @count1 - 1
        if @count1 >= @matchCount
          set @count = @count + @count1
      end
      set @j = @j + 1
    end
    set @i = @i + 1
  end
  
  if len(@compareStr1) > len(@compareStr2)
    return cast(@count as decimal(18, 4)) / cast(len(@compareStr1) as decimal(18, 4))
  else
    return cast(@count as decimal(18, 4)) / cast(len(@compareStr2) as decimal(18, 4))

  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetDelimitedValues]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Разбивает исходную строку на части, разделенные запятыми.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Makarev Andrey 16.04.2008
-- Измение размера выходного массива.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал в рамках оптимизации через xml
--------------------------------------------------
alter function [dbo].[GetDelimitedValues]
  (
  @ids nvarchar(4000)
  )
returns @Values table ([value] nvarchar(4000))
as
begin
  if len(ltrim(rtrim(@ids))) > 0
  begin
    DECLARE @x xml
    set @x = '<root><v>' + replace(@ids, ',', '</v><v>') + '</v></root>'
    insert into @Values
    SELECT  T.c.value('.','nvarchar(4000)')
    FROM    @x.nodes('/root/v') T ( c )
  end
  return  
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetCommonNationalExamCertificateActuality]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Получить годы актуальности сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
--------------------------------------------------
alter function [dbo].[GetCommonNationalExamCertificateActuality]
  (
  )
returns @Actuality table (YearFrom int, YearTo int)
as 
begin
  insert into @Actuality
  select
    Year(GetDate()) - 5
    , Year(GetDate())

  return
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalPassportSeria]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------
-- Функция получения серии паспорта
-- без пробелов
-- v.1.0: Created by Fomin Dmitriy 21.06.2008
-----------------------------------------------
alter function [dbo].[GetInternalPassportSeria]
  (
  @passportSeria nvarchar(255)
  )
returns nvarchar(255)
as
begin
  return replace(@passportSeria, ' ', '')
end
GO
/****** Object:  UserDefinedFunction [dbo].[CanViewUserAccountRegistrationDocument]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Можно ли просматривать документ регистрации.
-- v1.0: Created by Fomin Dmitriy 07.04.2008
--------------------------------------------------
alter function [dbo].[CanViewUserAccountRegistrationDocument]
  (
  @confirmYear int
  )
returns bit
as
begin
  return case
      when @confirmYear = year(getdate()) then 1
      else 0
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccountRegistrationDocument]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Можно ли редактировать пользователю свой
-- документ регистрации.
-- v1.0: Created by Fomin Dmitriy 04.04.2008
--------------------------------------------------
alter function [dbo].[CanEditUserAccountRegistrationDocument]
  (
  @status nvarchar(255)
  )
returns bit
as
begin
  return case
      when not @status in ('activated', 'deactivated') then 1
      else 0
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccount]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Можно ли редактировать пользователю свою
-- учетную запись.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.0: Modified by Fomin Dmitriy 19.05.2008
-- Модифицировать анкету можно до утверждения ее
-- документом.
--------------------------------------------------
alter function [dbo].[CanEditUserAccount]
  (
  @status nvarchar(255)
  , @confirmYear int
  , @currentYear int
  )
returns bit
as
begin
  return case
      when not @status in ('activated', 'deactivated') then 1
      else 0
    end
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalId]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить внутренний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
alter function [dbo].[GetInternalId]
  (
  @externalId bigint
  )
returns bigint
as
begin
  declare
    @result bigint

  if isnull(@externalId, -1) < 0
    return null

  if @externalId = 0
    return 0

  declare
    @base bigint
    , @shift bigint

  set @base = power(2, 20)
  set @shift = 11541954384
  
  set @result = (@externalId / @base) + (@externalId % @base) * @base - @shift
  if dbo.GetExternalId(@result) <> @externalId
    return -1
  return @result
end
GO
/****** Object:  StoredProcedure [dbo].[GetDocumentByUrl]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetDocumentByUrl

-- =============================================
-- Получение документа по относительному Url.
-- v.1.0: Created by Fomin Dmitriy 24.04.2008
-- =============================================
alter proc [dbo].[GetDocumentByUrl]
  @relativeUrl nvarchar(255)
as
begin
  select top 1
    dbo.GetExternalId([document].Id) Id
  from 
    dbo.Document [document] with (nolock, fastfirstrow)
  where 
    [document].RelativeUrl = @relativeUrl
    and [document].IsActive = 1

end
GO
/****** Object:  StoredProcedure [dbo].[GetSubject]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetSubject

-- =============================================
-- Получение списка предметов.
-- v.1.0: Created by Makarev Andrey 04.05.2008
-- v.1.1: Modified by Makarev Andrey 05.05.2008
-- Добавлены хинты.
-- v.1.2: Modified by Fomin Dmitriy 30.05.2008
-- Отдавать ИД без шифрования.
-- =============================================
alter proc [dbo].[GetSubject]
as
begin
  select
    [subject].[Id] [Id], [subject].[Code] Code
    , [subject].[Name] [Name]
  from
    dbo.Subject [subject] with (nolock)
  where
    [subject].IsActive = 1
  order by 
    [subject].SortIndex

  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetShortOrganizationName]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить короткое наименование организации.
-- v.1.0: Created by Fomin Dmitriy 06.05.2008
-- v.1.1: Modified by Fomin Dmitriy 08.05.2008
-- Поле наименования организации увеличено.
-- =============================================
alter function [dbo].[GetShortOrganizationName]
  (
  @organizationName nvarchar(2000)
  )
returns nvarchar(2000)
as
begin
  -- Список сокращений.
  declare @word_abbreviation table
    (
    Word nvarchar(255)
    , Abbreviation nvarchar(255)
    )

  insert into @word_abbreviation values ('федеральный', 'Ф')
  insert into @word_abbreviation values ('республиканский', 'Р')
  insert into @word_abbreviation values ('областной', 'О')

  insert into @word_abbreviation values ('государственный', 'Г')
  insert into @word_abbreviation values ('негосударственный', 'НГ')
  insert into @word_abbreviation values ('образовательное', 'О')
  insert into @word_abbreviation values ('учреждение', 'У')
  insert into @word_abbreviation values ('высшего', 'В')
  insert into @word_abbreviation values ('среднего', 'С')
  insert into @word_abbreviation values ('профессионального', 'П')
  insert into @word_abbreviation values ('образования', 'О')

  insert into @word_abbreviation values ('университет', 'УНВ')
  insert into @word_abbreviation values ('академия', 'АКД')
  insert into @word_abbreviation values ('институт', 'ИНС')
  insert into @word_abbreviation values ('училище', 'УЧЛ')
  insert into @word_abbreviation values ('техникум', 'ТХК')
  insert into @word_abbreviation values ('колледж', 'КЛЖ')

  insert into @word_abbreviation values ('медицинский', 'МЕДИЦ')
  insert into @word_abbreviation values ('педагогический', 'ПЕДАГ')
  insert into @word_abbreviation values ('правосудия', 'ПРАВО')
  insert into @word_abbreviation values ('технический', 'ТЕХНЧ')
  insert into @word_abbreviation values ('технологический', 'ТЕХЛГ')
  insert into @word_abbreviation values ('политехнический', 'ПОЛТХ')
  insert into @word_abbreviation values ('юридический', 'ЮРИДЧ')
  insert into @word_abbreviation values ('текстильный', 'ТЕКСТ')
  insert into @word_abbreviation values ('сельскохозяйственная', 'СЕЛХЗ')
  insert into @word_abbreviation values ('машиностроительный', 'МАШСТ')
  insert into @word_abbreviation values ('приборостроительный', 'ПРБСТ')
  insert into @word_abbreviation values ('строительный', 'СТРОЙ')

  insert into @word_abbreviation values ('министерства', 'М')
  insert into @word_abbreviation values ('внутренних дел', 'ВД')
  insert into @word_abbreviation values ('юстиции', 'Ю')
  insert into @word_abbreviation values ('Российской Федерации', 'РФ')
  insert into @word_abbreviation values ('имени', 'им')

  -- Разбиение названия организации на слова.
  declare @organization_word table
    (
    Word nvarchar(255)
    )

  declare
    @startIndex int
    , @delimiterIndex int
    , @value nvarchar(4000)

  set @startIndex = 1
  set @delimiterIndex = charindex(' ', isnull(@organizationName, ''))
  while @delimiterIndex > 0
  begin
    set @value = ltrim(rtrim(substring(@organizationName, @startIndex, @delimiterIndex  - @startIndex)))
    if @value <> ''
      insert into @organization_word
      select @value
  
    set @startIndex = @delimiterIndex + 1
    set @delimiterIndex = charindex(' ', @organizationName, @startIndex)
  end

  if len(@organizationName) >= @startIndex 
  begin
    set @value = ltrim(rtrim(substring(@organizationName, @startIndex, len(@organizationName) - @startIndex + 1)))
    if @value <> ''
        insert into @organization_word
        select @value
  end

  -- Вывод результата.
  declare
    @word nvarchar(255)
    , @result nvarchar(2000)
    , @sameLevel decimal(18, 4)
    , @matchCount int

  set @sameLevel = 0.7
  set @matchCount = 3

  set @result = ''

  declare abbreviation_cursor cursor for
  select
    -- Вывести наилучшее сокращение, если такое есть.
    isnull((select top 1 word_abbreviation.Abbreviation 
        from (select word_abbreviation.Abbreviation 
              , dbo.CompareStrings(organization_word.Word, word_abbreviation.Word, @matchCount) SameLevel
            from @word_abbreviation word_abbreviation) word_abbreviation
        where word_abbreviation.SameLevel >= @sameLevel
        order by word_abbreviation.SameLevel desc), organization_word.Word)
  from @organization_word organization_word

  open abbreviation_cursor 
  fetch next from abbreviation_cursor into @word
  while @@fetch_status = 0
  begin
    if len(@result) > 0
      set @result = @result + ' '
    set @result = @result + @word

    fetch next from abbreviation_cursor into @word
  end
  close abbreviation_cursor
  deallocate abbreviation_cursor

  return @result
end
GO
/****** Object:  StoredProcedure [dbo].[ReportCnecLoading]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[ReportCnecLoading] 
( @type varchar(10) = null)
as
begin

if(@type is null or @type not in ('month', 'week'))
  set @type = 'month'

select  
  day(n.value('date[1]', 'datetime')) day
, convert(varchar(10), n.value('date[1]', 'datetime'), 104) date
, n.value('cnecNew[1]', 'int') cnecNew
, n.value('cnecUpdated[1]', 'int') cnecUpdated
, n.value('cnecdNew[1]', 'int') cnecdNew
, n.value('cnecdUpdated[1]', 'int') cnecdUpdated
from report rp
cross apply rp.xml.nodes('unit') r(n)
where name = 'CnecLoading' + @type 
and rp.created = (select top 1 created from report where name = 'CnecLoading' + @type order by created desc)  

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateExpireDates]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[UpdateExpireDates]
(@ExpireDatesString varchar(max))
as
begin

update ed
set expiredate = convert(datetime, substring(t.value, charindex('=',t.value)+1, len(t.value)), 104)
from [ExpireDate] as ed 
join getdelimitedvalues(@ExpireDatesString) t on ed.[Year] = substring(t.value, 0, charindex('=',t.value))
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateMinimalMarks]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[UpdateMinimalMarks]
( @MinimalMarksString varchar(4000))
as
begin
update mm
set minimalmark = nmm.mark
from [MinimalMark] as mm
join GetSubjectMarks(@MinimalMarksString) nmm on nmm.[SubjectId] = mm.[Id] --не очень красиво читается
end
GO
/****** Object:  StoredProcedure [dbo].[SearchMinimalMark]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure  [dbo].[SearchMinimalMark]
( @year int = null
, @getAvailableYears bit = 0)
as
begin 

--если настал новый учетный год, то нужно наплодить недостающие минимальные баллы за текущий год
insert into [MinimalMark] (
  [SubjectId],
  [Year],
  [MinimalMark]
) 
select s.[Id], year(getdate()), 0
from [Subject] as s with(nolock)
left join [MinimalMark] as mm with(nolock) on 
mm.[SubjectId] = s.[Id] 
and mm.year = year(getdate())
where mm.[Id] is null

if @getAvailableYears = 1
  select distinct year from [MinimalMark] as mm with(nolock)
else 
  select mm.id, mm.year, s.[Name], mm.[MinimalMark] 
  from minimalmark mm with(nolock)
  join subject s with(nolock) on s.id = mm.[SubjectId] and s.[IsActive] = 1
  where mm.year = isnull(@year, mm.year)
  order by mm.year, s.[SortIndex]

end
GO
/****** Object:  StoredProcedure [dbo].[SearchExpireDate]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[SearchExpireDate]
as
BEGIN
--добавляем новый срок действия в новом году
insert into [ExpireDate] (
  [Year],
  [ExpireDate]
)
select year(getdate()), cast(cast((year(getdate())+1) as varchar(4)) + '1231' as datetime)
where not exists (select top 1 1 from [ExpireDate] ed where ed.year = year(getdate()))

select ed.year, convert(varchar(max), ed.[ExpireDate], 104) ExpireDate
from [ExpireDate] as ed with(nolock)

END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCertificateLoadShortTVF]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Отчет: Общий отчет о загрузке сертификатов
alter funCTION [dbo].[ReportCertificateLoadShortTVF]()
RETURNS @report TABLE 
(
  [Код региона] nvarchar(10) null,
  [Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств за 2012] int null,
[Всего свидетельств] int null
)
as
begin
declare @PreResult table 
(
  [Код региона] nvarchar(10) null,
  [Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств за 2012] int null,
[Всего свидетельств] int null
)
insert into @PreResult
select
  replace(r.REGION,1000,'-') [Код региона]
  , r.RegionName [Регион]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2008) 
[Всего свидетельств за 2008]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2009) 
[Всего свидетельств за 2009]  
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2010) 
[Всего свидетельств за 2010]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2011) 
[Всего свидетельств за 2011]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2012) 
[Всего свидетельств за 2012]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION) 
[Всего свидетельств]
  
from rbdc.Regions r with (nolock)
--where r.InCertificate = 1


insert into @report
select * from @PreResult 
union
select 'Всего',
'-',
SUM([Всего свидетельств за 2008]),
SUM([Всего свидетельств за 2009]),
SUM([Всего свидетельств за 2010]),
SUM([Всего свидетельств за 2011]),
SUM([Всего свидетельств за 2012]),
SUM([Всего свидетельств])
from @PreResult
order by [Код региона]

return
end
GO
/****** Object:  StoredProcedure [dbo].[SearchContext]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchContext

-- =============================================
-- Поиск контекстов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Приведение к стандарту.
-- =============================================
alter proc [dbo].[SearchContext]
as
begin

  select 
    context.Code Code
    , context.[Name] [Name]
  from 
    dbo.Context context with (nolock)

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionType]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCompetitionType
-- =============================================
-- Процедура поиска типов олимпиад
-- v.1.0. Created by Fomin Dmitriy 23.07.2008
-- v.1.1. Modified by Fomin Dmitriy 25.07.2008
-- Не нужно возвращать пустое значение.
-- =============================================
alter procedure [dbo].[SearchCompetitionType]
as
begin
  select
    competition_type.Id
    , competition_type.Name
  from 
    dbo.CompetitionType competition_type
  order by
    [Name]

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAccount

-- =============================================
-- Поиск пользователей горячей линии.
-- v.1.0: Created by Makarev Andrey 09.04.2008
-- v.1.1: Modified by Makarev Andrey 11.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Sedov Anton 16.05.2008
-- добавлен параметр @email
-- =============================================
alter proc [dbo].[SearchAccount]
  @groupCode nvarchar(255)
  , @login nvarchar(255) = null
  , @lastName nvarchar(255) = null
  , @isActive bit = null
  , @email nvarchar(255) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = N'login'
  , @sortAsc bit = 1
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @params nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @userGroupId int
    , @lastNameFormat nvarchar(255)

  if isnull(@lastName, N'') <> N''
    set @lastNameFormat = N'%' + replace(@lastName, N' ', '%') + N'%'

  select
    @userGroupId = [group].[Id]
  from
    dbo.[Group] [group] with (nolock, fastfirstrow)
  where
    [group].Code = @groupCode

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Login nvarchar(255) ' +
      ' , LastName nvarchar(255) ' +
      ' , FirstName nvarchar(255) ' +
      ' , PatronymicName nvarchar(255) ' +
      ' , IsActive bit ' +
      '   , Email nvarchar(255) ' + 
      ' , Id bigint not null ' +
      ' ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' account.Login Login ' +
        ' , account.LastName LastName ' +
        ' , account.FirstName FirstName ' +
        ' , account.PatronymicName PatronymicName ' +
        ' , account.IsActive IsActive ' +
        '   , account.Email Email ' +
        ' , account.[Id] ' +
        'from dbo.Account account with (nolock) ' +
        ' inner join dbo.GroupAccount group_account with (nolock) ' +
        '   on account.[Id] = group_account.AccountId ' +
        'where ' +
        ' group_account.GroupId = @userGroupId '
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.Account account with (nolock, fastfirstrow) ' +
        ' inner join dbo.GroupAccount group_account with (nolock) ' +
        '   on account.[Id] = group_account.AccountId ' +
        'where ' + 
        ' group_account.GroupId = @userGroupId ' 
  
  if not @login is null
    set @commandText = @commandText + ' and account.Login = @login '

  if not @isActive is null
    set @commandText = @commandText + ' and account.IsActive = @isActive '

  if not @lastName is null
    set @commandText = @commandText + ' and account.LastName like @lastNameFormat '
  
  if not @email is null
    set @commandText = @commandText + ' and account.Email = @email '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'login'
    begin
      set @innerOrder = 'order by Login <orderDirection> '
      set @outerOrder = 'order by Login <orderDirection> '
      set @resultOrder = 'order by Login <orderDirection> '
    end
    else if @sortColumn = 'IsActive'
    begin
      set @innerOrder = 'order by IsActive <orderDirection> '
      set @outerOrder = 'order by IsActive <orderDirection> '
      set @resultOrder = 'order by IsActive <orderDirection> '
    end
    else if @sortColumn = 'name'
    begin
      set @innerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
      set @outerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
      set @resultOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
    end
    else if @sortColumn = 'email'
    begin
      set @innerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
      set @outerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
      set @resultOrder = 'order by Email <orderDirection>, Id <orderDirection> '
    end 
    else if @sortColumn = 'Id'
    begin
      set @innerOrder = 'order by Id <orderDirection> '
      set @outerOrder = 'order by Id <orderDirection> '
      set @resultOrder = 'order by Id <orderDirection> ' 
    end 
    else 
    begin
      set @innerOrder = 'order by Login <orderDirection> '
      set @outerOrder = 'order by Login <orderDirection> '
      set @resultOrder = 'order by Login <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' search.Login ' +
      ' , search.LastName ' +
      ' , search.FirstName ' +
      ' , search.PatronymicName ' +
      ' , search.IsActive ' +
      ' , search.Email ' + 
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 
  
  set @params = 
      '@userGroupId int ' +
      ', @login nvarchar(255) ' +
      ', @IsActive bit ' + 
      ', @lastNameFormat nvarchar(255) ' +
      ', @email nvarchar(255) ' 
  
  exec sp_executesql @commandText, @params, 
      @userGroupId
      , @login
      , @IsActive
      , @lastNameFormat
      , @email 

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountKey]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAccountKey
-- ====================================================
-- Поиск ключей.
-- v.1.0: Created by Fomin Dmitriy 28.08.2008
-- ====================================================
alter procedure [dbo].[SearchAccountKey]
  @login nvarchar(255)
as
begin
  declare
    @now datetime

  set @now = convert(nvarchar(8), GetDate(), 112)

  select
    account_key.[Key]
    , account_key.DateFrom
    , account_key.DateTo
    , account_key.IsActive
  from dbo.Account account
    inner join dbo.AccountKey account_key
      on account.Id = account_key.AccountId
  where
    account.Login = @login
  order by
    case
      when @now < account_key.DateFrom then 2
      when @now > account_key.DateTo then 1
      else 0
    end asc

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[ReportUserRegistration]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение отчета о регистрации пользователей.
-- =============================================
alter procedure [dbo].[ReportUserRegistration]
as
begin

DECLARE @StartDate DATETIME
SET @StartDate= '2010-05-15' -- dateadd(month, -1, getdate())

SELECT 
DAY(UpdateDay) AS [Day]
--, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(@MonthAgo))+'/'+CONVERT(NVARCHAR(50),MONTH(GETDATE()))+'/'+CONVERT(NVARCHAR(50),UpdateDay)) AS [date]
, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(UpdateDay))+'-'+CONVERT(NVARCHAR(50),MONTH(UpdateDay))+'-'+CONVERT(NVARCHAR(50),DAY(UpdateDay))) AS [date]
, SUM([Активирован]) AS [activated]
, SUM([На регистрации])  AS [registration]
, SUM([На доработке]) AS [revision]
, SUM([На согласовании]) AS [consideration]
, SUM([Отключен])AS [deactivated]


FROM(
  SELECT 
    CONVERT(NVARCHAR(4),YEAR(F.UpdateDate))+'-'+CONVERT(NVARCHAR(2),MONTH(F.UpdateDate))+'-'+CONVERT(NVARCHAR(2),DAY(F.UpdateDate)) 
  AS UpdateDay,
    case when F.[Status]='activated' then 1 else 0 end AS [Активирован],
    case when F.[Status]='registration' then 1 else 0 end AS [На регистрации],
    case when F.[Status]='revision' then 1 else 0 end AS [На доработке],
    case when F.[Status]='consideration' then 1 else 0 end AS [На согласовании],
    case when F.[Status]='deactivated' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID,UpdateDate,[Status]
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR ([Status]='registration' and VersionId=1)) AND UpdateDate >= @StartDate 
  ) F ON A.ID=F.AccountID
) T  
GROUP BY UpdateDay
ORDER BY [date]
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificateRequestBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCompetitionCertificateRequestBatch
-- =====================================================
-- Процедура поиска пакетов проверки медалистов
-- v.1.0: Created by Sedov Anton 30.07.2008
-- v.1.1: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- =====================================================
alter procedure [dbo].[SearchCompetitionCertificateRequestBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'
      declare @search table
        (
        Id bigint
        , CreateDate datetime
        , IsProcess bit 
        , IsCorrect bit
        )
        '

  if isnull(@showCount, 0) = 0 
    set @commandText = 
      N'
      select <innerHeader>
        batch.Id
        , batch.CreateDate
        , batch.IsProcess
        , batch.IsCorrect
      from 
        dbo.CompetitionCertificateRequestBatch batch
      where 
        batch.OwnerAccountId = @accountId 
      '
  else 
    set @commandText = 
      N'
      select count(*)
      from dbo.CompetitionCertificateRequestBatch batch
      where batch.OwnerAccountId = @accountId
      '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'CreateDate'
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' dbo.GetExternalId(search.Id) Id ' +
      ' , search.CreateDate ' +
      ' , search.IsProcess ' +
      ' , search.IsCorrect ' +
      'from @search search ' + @resultOrder
  
  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificateRequest]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCompetitionCertificateRequest
-- ========================================================
-- Получить список проверенных сертификатов  
-- олимпиадников пакета
-- v.1.0: Created by Sedov Anton 15.08.2008
-- v.1.1: Modified by Sedov Anton 18.08.2008
-- Добавлено поле IsExist
-- v.1.2: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- ========================================================
alter procedure [dbo].[SearchCompetitionCertificateRequest]
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.CompetitionCertificateRequestBatch batch with (nolock, fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on batch.OwnerAccountId = account.[Id]
      where 
        batch.Id = @internalBatchId
        and batch.IsProcess = 0
        and account.[Login] = @login)
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @viewSelectCommandText nvarchar(4000)
    , @viewSelectPivot1CommandText nvarchar(4000)
    , @viewSelectPivot2CommandText nvarchar(4000)
    , @pivotSubjectColumns nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @viewSelectCommandText = ''
  set @viewSelectPivot1CommandText = ''
  set @viewSelectPivot2CommandText = ''
  set @pivotSubjectColumns = ''
  set @sortColumn = N'LastName'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' CompetitionTypeId int ' +
      ' , Name nvarchar(255) ' +
      ' , LastName nvarchar(255) ' +
      ' , FirstName nvarchar(255) ' +
      ' , PatronymicName nvarchar(255) ' +
      ' , Degree nvarchar(255) ' +
      ' , RegionName nvarchar(255) ' +
      ' , City nvarchar(255) ' + 
      ' , School nvarchar(255) ' +
      ' , Class nvarchar(255) ' +
      ' , IsExist bit ' + 
      ' ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' competition_certificate.CompetitionTypeId' +
        ' , competition_type.[Name] ' +
        ' , competition_certificate_request.LastName ' +
        ' , competition_certificate_request.FirstName ' +
        ' , competition_certificate_request.PatronymicName ' +
        ' , competition_certificate.Degree ' +
        ' , region.[Name] RegionName ' +
        ' , competition_certificate.City ' +
        ' , competition_certificate.School ' +
        ' , competition_certificate.Class ' +
        ' , case ' +
        '   when competition_certificate.Id is null then 0 ' +
        '   else 1 ' + 
        ' end IsExist ' +  
        'from ' + 
        ' dbo.CompetitionCertificateRequest competition_certificate_request ' + 
        '   left join dbo.CompetitionCertificate competition_certificate ' +
        '     left join dbo.CompetitionType competition_type ' + 
        '       on competition_certificate.CompetitionTypeId = competition_type.Id ' +
        '     left join dbo.Region region ' + 
        '       on competition_certificate.RegionId = region.Id ' + 
        '     on competition_certificate_request.SourceCertificateId = competition_certificate.Id ' + 
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CompetitionCertificateRequest competition_certificate_request with (nolock) ' +
        'where 1 = 1 ' 

  set @commandText = @commandText + 
    ' and competition_certificate_request.BatchId = @internalBatchId '

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = 'order by LastName <orderDirection> '
      set @outerOrder = 'order by LastName <orderDirection> '
      set @resultOrder = 'order by LastName <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' search.CompetitionTypeId CompetitionTypeId ' +
      ' , search.[Name] CompetitiontypeName ' + 
      ' , search.LastName LastName ' +
      ' , search.FirstName FirstName ' + 
      ' , search.PatronymicName PatronymicName ' + 
      ' , search.Degree Degree ' + 
      ' , search.RegionName RegionName ' + 
      ' , search.City City ' + 
      ' , search.School School ' +
      ' , search.Class Class ' +  
      ' , search.IsExist ' + 
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@internalBatchId bigint'
    , @internalBatchId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null   -- если > 0, то выбирается общее кол-во
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  if exists ( select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login )
    set @accountId = null
  else 
    set @accountId = isnull(
      (select account.[Id] 
      from dbo.Account account with (nolock, fastfirstrow) 
      where account.[Login] = @login), 0)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table 
        ( 
        Id bigint 
        , CreateDate datetime 
        , IsProcess bit 
        , IsCorrect bit 
        , Login varchar(255) 
        , Total int
        , Found int
        ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> 
          b.Id Id 
          , b.CreateDate CreateDate 
          , b.IsProcess IsProcess 
          , b.IsCorrect IsCorrect 
          , a.login Login 
          , (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
          , (select count(SourceCertificateIdGuid) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
        from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
        left join account a on a.id = b.OwnerAccountId 
        where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) ' +
        'where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 

  if isnull(@showCount, 0) = 0
  begin
    set @innerOrder = 'order by CreateDate <orderDirection> '
    set @outerOrder = 'order by CreateDate <orderDirection> '
    set @resultOrder = 'order by CreateDate <orderDirection> '

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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select 
        dbo.GetExternalId(s.Id) Id 
        , s.Login 
        , s.CreateDate 
        , s.IsProcess 
        , s.IsCorrect 
        , s.Total
        , s.Found
      from @search s ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheck]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Предметы отсортированы по порядку и актуальности.
-- Разделение на части текста по мере заполнения предыдущей части.
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Убрано сравнение HasAppeal. Добавлены IsDeny, DenyComment.
-- v.1.3: Modified by Sedov Anton 04.07.2008
-- В результат запроса  добавлено поле 
-- DenyNewCertificateNumber
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateCheck]
  @login nvarchar(255)
  , @batchId bigint     -- id пакета
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null    -- если > 0, то выбирается общее кол-во
  , @certNumber nvarchar(50) = null -- опциональный номер сертификата
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.CommonNationalExamCertificateCheckBatch cne_certificate_check_batch with (nolock, 

fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on cne_certificate_check_batch.OwnerAccountId = account.[Id]
      where 
        cne_certificate_check_batch.Id = @internalBatchId
        and cne_certificate_check_batch.IsProcess = 0
        and (account.[Login] = @login or exists (select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login)))
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(max)
    , @commandText nvarchar(max)
    , @viewCommandText nvarchar(max)
    , @viewSelectCommandText nvarchar(max)
    , @viewSelectPivotCommandText nvarchar(max)
    , @pivotSubjectColumns nvarchar(max)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @viewSelectCommandText = ''
  set @viewSelectPivotCommandText = ''
  set @pivotSubjectColumns = ''
  set @sortColumn = N'Id'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table 
        ( 
        Id bigint 
        , BatchId bigint 
        , CertificateNumber nvarchar(255) 
        , CertificateId uniqueidentifier 
        , IsOriginal bit 
        , CheckLastName nvarchar(255) 
        , LastName nvarchar(255) 
        , CheckFirstName nvarchar(255) 
        , FirstName nvarchar(255) 
        , CheckPatronymicName nvarchar(255) 
        , PatronymicName nvarchar(255) 
        , IsCorrect bit 
        , IsDeny bit 
        , Year int
        , TypographicNumber nvarchar(255) 
        , [Status] nvarchar(255) 
          , RegionName nvarchar(255) 
          , RegionCode nvarchar(10) 
          , PassportSeria nvarchar(255) 
          , PassportNumber nvarchar(255) 
        , primary key(id)
        ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' cne_check.Id ' +
        ' , cne_check.BatchId ' +
        ' , cne_check.CertificateNumber ' +
        ' , cne_check.SourceCertificateIdGuid CertificateId ' +
        ' , cne_check.IsOriginal ' +
        ' , cne_check.LastName CheckLastName ' +
        ' , cne_check.SourceLastName LastName ' +
        ' , cne_check.FirstName CheckFirstName ' +
        ' , cne_check.SourceFirstName FirstName ' +
        ' , cne_check.PatronymicName CheckPatronymicName ' +
        ' , cne_check.SourcePatronymicName PatronymicName ' +
        ' , cne_check.IsCorrect ' +
        ' , cne_check.IsDeny ' +
        ' , cne_check.Year ' +
        ' , cne_check.TypographicNumber ' +
        ' , case when ed.[ExpireDate] is null then ''Не найдено'' else 
            case when isnull(cne_check.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 

''Действительно'' 
            else ''Истек срок'' end end as [Status] ' +
        ' , r.name as RegionName ' +
        ' , r.code as RegionCode ' +
        ' , cne_check.PassportSeria ' +
        ' , cne_check.PassportNumber ' +
        'from dbo.CommonNationalExamCertificateCheck cne_check with (nolock) ' +
        'left join [ExpireDate] as ed on cne_check.[Year] = ed.[Year] '+
        'left join [Region] as r on cne_check.regionid = r.[Id] '+
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateCheck cne_check with (nolock) ' +
        'where 1 = 1 ' 

  set @commandText = @commandText + 
    ' and cne_check.BatchId = @internalBatchId '
  if @certNumber is not null
    set @commandText = @commandText + 
    ' and cne_check.CertificateNumber = ''' + @certNumber + ' '''

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = 'order by Id <orderDirection> '
      set @outerOrder = 'order by Id <orderDirection> '
      set @resultOrder = 'order by Id <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
  begin
    set @declareCommandText = @declareCommandText +
      'declare @check_subject table
        ( 
          CheckId bigint 
        , SubjectId smallint
        , CertificateSubjectId uniqueidentifier 
        , SubjectCode nvarchar(255) 
        , CheckMark nvarchar(10) 
        , Mark nvarchar(10) 
        , HasAppeal int 
        , IsCorrect int 
        , primary key (CheckId, SubjectId)
        ) '

    set @commandText = @commandText +
      ' insert into @check_subject 
      select 
          subject_check.CheckId 
        , subject_check.SubjectId
        , subject_check.SourceCertificateSubjectIdGuid 
        , subject.Code 
        , case when subject_check.[Mark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[Mark] as nvarchar(9)),''.'','','')
        , case when subject_check.[SourceMark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[SourceMark] as nvarchar(9)),''.'','','')
        , cast(subject_check.SourceHasAppeal as int) 
        , cast(subject_check.IsCorrect as int) 
      from dbo.CommonNationalExamCertificateSubjectCheck subject_check with (nolock) 
      inner join dbo.Subject subject on subject.Id = subject_check.SubjectId 
      left join [MinimalMark] as mm on subject_check.[SubjectId] = mm.[SubjectId] and subject_check.Year = 

mm.[Year] 
      where subject_check.BatchId = @internalBatchId 
      order by subject.IsActive desc, subject.SortIndex asc ' 

    set @viewSelectCommandText = 
      ' select 
          dbo.GetExternalId(search.Id) Id 
        , dbo.GetExternalId(search.BatchId) BatchId 
        , search.CertificateNumber 
        , search.IsOriginal 
        , search.CheckLastName 
        , search.LastName 
        , case 
          when search.CheckLastName collate cyrillic_general_ci_ai = search.LastName then 1 
          else 0 
          end LastNameIsCorrect 
        , search.CheckFirstName 
        , search.FirstName 
        , case 
          when search.CheckFirstName collate cyrillic_general_ci_ai = search.FirstName then 1 
          else 0 
          end FirstNameIsCorrect 
        , search.CheckPatronymicName 
        , search.PatronymicName 
        , case 
          when search.CheckPatronymicName collate cyrillic_general_ci_ai = 

search.PatronymicName then 1 
          else 0 
          end PatronymicNameIsCorrect 
        , case when search.CertificateId is not null then 1 else 0 end IsExist 
        , search.IsCorrect 
        , cast(search.IsDeny as bit) IsDeny 
        , cne_check.DenyComment   
        , cne_check.DenyNewCertificateNumber 
        , search.Year SourceCertificateYear
        , search.TypographicNumber 
        , search.[Status] 
        , search.RegionName 
        , search.RegionCode
        , search.PassportSeria 
        , search.PassportNumber ' 
      

    declare
        @subjectCode nvarchar(255)
      , @pivotSelect nvarchar(max)

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
      set @pivotSubjectColumns = @pivotSubjectColumns + '[' + @subjectCode + ']'

      set @viewSelectPivotCommandText = @viewSelectPivotCommandText 
          + replace(
          ' , chk_mrk_pvt.[<code>] [<code>CheckMark]  
            , mrk_pvt.[<code>] [<code>Mark]  
            , case 
              when chk_mrk_pvt.[<code>] = mrk_pvt.[<code>] then 1 
              else 0 
            end [<code>MarkIsCorrect] 
            , apl_pvt.[<code>] [<code>HasAppeal] 
            , case 
              when not sbj_pvt.[<code>] is null then 1 
              else 0 
            end [<code>IsExist] 
            , crt_pvt.[<code>] [<code>IsCorrect] ' 
          , '<code>', @subjectCode)

      fetch next from subject_cursor into @subjectCode
    end
    close subject_cursor
    deallocate subject_cursor

    set @viewCommandText = replace(
       ',unique_cheks.UniqueIHEaFCheck
        from @search search 
        left outer join dbo.CommonNationalExamCertificateCheck cne_check with (nolock) 
          on cne_check.Id = search.Id
        left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on unique_cheks.idGUID = search.CertificateId
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.CheckMark 
            from @check_subject check_subject) check_subject 
            pivot (min(CheckMark) for SubjectCode in (<subject_columns>)) as 

chk_mrk_pvt 
          on search.Id = chk_mrk_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.Mark 
            from @check_subject check_subject) check_subject 
            pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt 
          on search.Id = mrk_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.HasAppeal 
            from @check_subject check_subject) check_subject 
            pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as apl_pvt 
          on search.Id = apl_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.IsCorrect 
            from @check_subject check_subject) check_subject 
            pivot (Sum(IsCorrect) for SubjectCode in (<subject_columns>)) as crt_pvt 
          on search.Id = crt_pvt.CheckId 
        left outer join (select 
              check_subject.CheckId 
              , check_subject.SubjectCode 
              , check_subject.CertificateSubjectId 
            from @check_subject check_subject) check_subject 
            pivot (count(CertificateSubjectId) for SubjectCode in (<subject_columns>)) as 

sbj_pvt 
          on search.Id = sbj_pvt.CheckId ' +
      @resultOrder, '<subject_columns>', @pivotSubjectColumns)
  end

  set @commandText = @declareCommandText + @commandText + @viewSelectCommandText + 
      @viewSelectPivotCommandText + @viewCommandText
  
  declare @params nvarchar(200)

  set @params = 
      '@internalBatchId int '
  print cast(@commandText as ntext)
  exec sp_executesql 
      @commandText
      ,@params
      ,@internalBatchId 
  
  PRINT @commandText
  PRINT @params
  print @internalBatchId
  
  return 0
end


/*
exec dbo.SearchCommonNationalExamCertificateCheck @login=N'rick_box@mail.ru',@batchId=301638626047,@startRowIndex=N'1',@maxRowCount=N'20'
*/
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null   -- если > 0, то выбирается общее кол-во
  ,@type int
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  if exists ( select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login )
    set @accountId = null
  else 
    set @accountId = isnull(
      (select account.[Id] 
      from dbo.Account account with (nolock, fastfirstrow) 
      where account.[Login] = @login), 0)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table 
        ( 
        Id bigint 
        , CreateDate datetime 
        , IsProcess bit 
        , IsCorrect bit 
        , Login varchar(255) 
        , Total int
        , Found int
        ,outerId bigint
        ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> 
          b.Id Id 
          , b.CreateDate CreateDate 
          , b.IsProcess IsProcess 
          , b.IsCorrect IsCorrect 
          , a.login Login 
          , (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
          , (select count(SourceCertificateId) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
          ,outerId
        from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
        left join account a on a.id = b.OwnerAccountId 
        where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 
        and type=<@type>' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) ' +
        'where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 and type=<@type>' 
set @commandText=REPLACE(@commandText,'<@type>',@type)
  if isnull(@showCount, 0) = 0
  begin
    set @innerOrder = 'order by CreateDate <orderDirection> '
    set @outerOrder = 'order by CreateDate <orderDirection> '
    set @resultOrder = 'order by CreateDate <orderDirection> '

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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select 
        dbo.GetExternalId(s.Id) Id 
        , s.Login 
        , s.CreateDate 
        , s.IsProcess 
        , s.IsCorrect 
        , s.Total
        , s.Found
        ,outerId
      from @search s ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение списка ошибок задания на загрузку закрытий сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError] 
  @taskId bigint = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @internalTaskId bigint
    , @server nvarchar(30)

  set @internalTaskId = dbo.GetInternalId(@taskId)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'Date'
  set @sortAsc = 1

  select @server = (select top 1 ss.name + '.fbs_loader_db' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = 'loader')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'declare @search table ' +
      N'  ( ' +
      N'  TaskId bigint ' +
      N'  , Date datetime ' +
      N'  , RowIndex bigint ' +
      N'  , Error ntext ' +
      N'  ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N'select <innerHeader> ' +
        N'  cne_certificate_deny_loading_task_error.TaskId TaskId ' +
        N'  , cne_certificate_deny_loading_task_error.Date Date ' +
        N'  , cne_certificate_deny_loading_task_error.RowIndex RowIndex ' +
        N'  , cne_certificate_deny_loading_task_error.Error Error ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error with (nolock) ' +
        N'where 1 = 1'
  else
    set @commandText = 
        N'select count(*) ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error with (nolock) ' +
        N'where 1 = 1'

  if not @taskId is null
    set @commandText = @commandText + N' and cne_certificate_deny_loading_task_error.TaskId = @internalTaskId '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N'Date'
    begin
      set @innerOrder = 'order by Date <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection> '
    end
    else 
    begin
      set @innerOrder = 'order by Date <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection> '
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N'top 10000000'
      set @outerSelectHeader = N'top 10000000'
    end

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N'select ' +
      N'  dbo.GetExternalId(search.TaskId) TaskId ' +
      N'  , search.Date Date ' +
      N'  , search.RowIndex RowIndex ' +
      N'  , search.Error Error ' +
      N'from ' +
      N'  @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@internalTaskId bigint'
    , @internalTaskId

  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCertificateLoadTVF]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Отчет: Ежедневный отчет о загрузке сертификатов
alter funCTION [dbo].[ReportCertificateLoadTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [Код региона] nvarchar(10) null,
  [Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств] int null,
[Новых свидетельств] int null,
[Обновлено свидетельств] int null,
[Всего аннулированных свидетельств] int null,
[Новых аннулированных свидетельств] int null,
[Обновлено аннулированных свидетельств] int null
)
as
begin

insert into @report
select
  r.code [Код региона]
  , r.name [Регион]
, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2008) 
[Всего свидетельств за 2008]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2009) 
[Всего свидетельств за 2009]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2010) 
[Всего свидетельств за 2010]  
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2011) 
[Всего свидетельств за 2011]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id) 
[Всего свидетельств]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.createdate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[Новых свидетельств]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.updatedate <> c.createdate and c.updatedate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[Обновлено свидетельств]
  , (select count(*) from CommonNationalExamCertificateDeny d with(nolock)
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where c.regionid = r.id) 
[Всего аннулированных свидетельств]
  , (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[Новых аннулированных свидетельств]
  , (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate <> d.updatedate AND d.updatedate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[Обновлено аннулированных свидетельств]
from dbo.Region r with (nolock)
where r.InCertificate = 1
order by r.id asc


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '', 'Итого за ' + case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
,sum([Всего свидетельств за 2008])
,sum([Всего свидетельств за 2009])
,sum([Всего свидетельств за 2010])
,sum([Всего свидетельств за 2011])
,sum([Всего свидетельств])
,sum([Новых свидетельств])
,sum([Обновлено свидетельств])
,sum([Всего аннулированных свидетельств])
,sum([Новых аннулированных свидетельств])
,sum([Обновлено аннулированных свидетельств])
from @report


return
end
GO
/****** Object:  StoredProcedure [dbo].[SearchEntrantCheckBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchEntrantCheckBatch
-- ===============================================
-- Процедура поиска пакетов  проверки абитриентов
-- v.1.0: Created by Sedov Anton 09.07.2008
-- ===============================================
alter procedure [dbo].[SearchEntrantCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'
      declare @search table
        (
        Id bigint
        , CreateDate datetime
        , IsProcess bit 
        , IsCorrect bit
        )
        '

  if isnull(@showCount, 0) = 0 
    set @commandText = 
      N'
      select <innerHeader>
        entrant_check_batch.Id
        , entrant_check_batch.CreateDate
        , entrant_check_batch.IsProcess
        , entrant_check_batch.IsCorrect
      from 
        dbo.EntrantCheckBatch entrant_check_batch
      where 
        entrant_check_batch.OwnerAccountId = @accountId 
      '
  else 
    set @commandText = 
      N'
      select count(*)
      from dbo.EntrantCheckBatch entrant_check_batch
      where entrant_check_batch.OwnerAccountId = @accountid
      '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'CreateDate'
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' dbo.GetExternalId(search.Id) Id ' +
      ' , search.CreateDate ' +
      ' , search.IsProcess ' +
      ' , search.IsCorrect ' +
      'from @search search ' + @resultOrder
  
  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchEntrantCheck]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchEntrantCheck
-- ==============================================
-- Получение списка проверенных абитуриентов
-- v.1.0: Created by Sedov Anton 09.07.2008
-- ==============================================
alter procedure [dbo].[SearchEntrantCheck] 
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.EntrantCheckBatch entrant_check_batch with (nolock, fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on entrant_check_batch.OwnerAccountId = account.[Id]
      where 
        entrant_check_batch.Id = @internalBatchId
          and entrant_check_batch.IsProcess = 0
          and account.[Login] = @login)
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'Id'
  set @sortAsc = 0
  
  
  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Id bigint ' +
      ' , CertificateNumber nvarchar(255) ' +
      ' , LastName nvarchar(255) ' +
      ' , FirstName nvarchar(255)' +
      ' , PatronymicName nvarchar(255) ' +
      ' , OrganizationName nvarchar(255)' +
      ' , EntrantCreateDate datetime ' +
      ' , IsExist bit ' +
      ' ) '

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' entrant_check.Id ' +
        ' , entrant_check.CertificateNumber ' +
        ' , entrant_check.SourceLastName ' +
        ' , entrant_check.SourceFirstName ' +
        ' , entrant_check.SourcePatronymicName ' +
        ' , entrant_check.SourceOrganizationName ' +
        ' , entrant_check.SourceEntrantCreateDate ' +
        ' , case ' + 
        '   when not entrant_check.SourceEntrantId is null then 1 ' +
        '   else 0 ' +
        ' end IsExist ' + 
        'from dbo.EntrantCheck entrant_check with (nolock) ' +
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.EntrantCheck entrant_check with (nolock) ' +
        'where 1 = 1 ' 

  
  set @commandText = @commandText + 
    ' and entrant_check.BatchCheckId = @internalBatchId ' 

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = 'order by Id <orderDirection> '
      set @outerOrder = 'order by Id <orderDirection> '
      set @resultOrder = 'order by Id <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' search.CertificateNumber CertificateNumber' +
      ' , search.LastName LastName ' +
      ' , search.FirstName FirstName ' +
      ' , search.PatronymicName PatronymicName ' +
      ' , search.OrganizationName OrganizationName ' + 
      ' , search.EntrantCreateDate EntrantCreateDate ' + 
      ' , search.IsExist IsExist ' + 
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@internalBatchId bigint'
    , @internalBatchId
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCommonNationalExamCertificateRequestExtendedByExam

-- =============================================
-- Получить список проверок.
-- v.1.0: Created by Fomin Dmitriy 18.07.2008
-- Создана по SearchCommonNationalExamCertificateRequest.
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]
  @login nvarchar(255)
  , @batchId bigint
as
begin
  declare 
    @innerBatchId bigint
    , @accountId bigint
    , @commandText nvarchar(4000)
    , @declareCommandText nvarchar(4000)
    , @viewSelectCommandText nvarchar(4000)
    , @pivotSubjectColumns nvarchar(4000)
    , @viewSelectPivot1CommandText nvarchar(4000)
    , @viewSelectPivot2CommandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @sortColumn nvarchar(20) 
    , @sortAsc bit 

  set @commandText = ''
  set @pivotSubjectColumns = ''
  set @viewSelectPivot1CommandText = ''
  set @viewSelectPivot2CommandText = ''
  set @viewCommandText = ''
  set @declareCommandText = ''
  set @sortColumn = N'Id'
  set @sortAsc = 1
  
  if @batchId is not null
    set @innerBatchId = dbo.GetInternalId(@batchId)

  select
    @accountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  set @declareCommandText = 
    N'declare @search table 
      (
      BatchId bigint
      , CertificateNumber nvarchar(255)
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      , IsExist bit
      , SourceCertificateId  bigint
      , SourceCertificateYear int
      )
    '

  set @declareCommandText = @declareCommandText +
    N'declare @request table 
      (
      LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
    '

  set @commandText = @commandText +
    'insert into @request 
    select distinct
      cne_certificate_request.LastName 
      , cne_certificate_request.FirstName 
      , cne_certificate_request.PatronymicName 
      , cne_certificate_request.PassportSeria 
      , cne_certificate_request.PassportNumber 
    from 
      dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
        inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
          on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
    where
      cne_certificate_request_batch.OwnerAccountId = <accountId> 
      and cne_certificate_request_batch.[Id] = <innerBatchId>
      and cne_certificate_request_batch.IsProcess = 0 
    '
  
  set @commandText = @commandText +
    N'insert into @search
    select 
      dbo.GetExternalId(<innerBatchId>) BatchId
      , cne_certificate_request.SourceCertificateNumber CertificateNumber
      , request.LastName LastName
      , request.FirstName FirstName
      , request.PatronymicName PatronymicName
      , request.PassportSeria PassportSeria
      , request.PassportNumber PassportNumber
      , case
        when not cne_certificate_request.SourceCertificateId is null then 1
        else 0
      end IsExist
      , cne_certificate_request.SourceCertificateId
      , cne_certificate_request.SourceCertificateYear
    from @request request
      left outer join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
        inner join dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
            and cne_certificate_request_batch.OwnerAccountId = <accountId> 
            and cne_certificate_request_batch.[Id] = <innerBatchId>
            and cne_certificate_request_batch.IsProcess = 0 
        on request.FirstName = cne_certificate_request.FirstName
          and request.LastName = cne_certificate_request.LastName
          and request.PatronymicName = cne_certificate_request.PatronymicName
          and request.PassportSeria = cne_certificate_request.PassportSeria
          and request.PassportNumber = cne_certificate_request.PassportNumber
          and cne_certificate_request.IsDeny = 0
    '

  set @declareCommandText = @declareCommandText +
    N' declare @subjects table  
      ( 
      CertificateId bigint 
      , Mark numeric(5,1) 
      , HasAppeal bit  
      , SubjectCode nvarchar(255)  
      , HasExam bit
      ) 
    '

  set @commandText = @commandText +
    N'insert into @subjects  
    select
      cne_certificate_subject.CertificateId 
      , cne_certificate_subject.Mark
      , cne_certificate_subject.HasAppeal
      , subject.Code
      , 1 
    from  
      dbo.CommonNationalExamCertificateSubject cne_certificate_subject
        left outer join dbo.Subject subject
          on subject.Id = cne_certificate_subject.SubjectId
    where 
      exists(select 1 
          from @search search
          where cne_certificate_subject.CertificateId = search.SourceCertificateId
            and cne_certificate_subject.[Year] = search.SourceCertificateYear)
    ' 
  
  set @viewSelectCommandText = 
    N'select
      search.BatchId
      , search.CertificateNumber
      , search.LastName
      , search.FirstName
      , search.PatronymicName
      , search.PassportSeria
      , search.PassportNumber
      , search.IsExist
    '

  set @viewCommandText = 
    N'from @search search '

  declare
    @subjectCode nvarchar(255)
    , @pivotSelect nvarchar(4000)

  set @pivotSelect = ''

  declare subject_cursor cursor forward_only for
  select 
    [subject].Code
  from 
    dbo.Subject [subject]

  open subject_cursor 
  fetch next from subject_cursor into @subjectCode
  while @@fetch_status = 0
    begin
    if len(@pivotSubjectColumns) > 0
      set @pivotSubjectColumns = @pivotSubjectColumns + ','
    set @pivotSubjectColumns = @pivotSubjectColumns + replace('[<code>]', '<code>', @subjectCode)
    
    set @pivotSelect = @pivotSelect + 
      N' , isnull(exam_pvt.[<code>], 0) [<code>HasExam] '
        
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

  set @viewCommandText = @viewCommandText + 
    N'left outer join (select 
      subjects.CertificateId
      , subjects.SubjectCode
      , cast(subjects.HasExam as int) HasExam 
      from @subjects subjects) subjects
        pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
      on search.SourceCertificateId = exam_pvt.CertificateId '
      
  set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)

  set @viewCommandText = @viewCommandText

  set @commandText = replace(
      replace(@commandText, '<innerBatchId>', @innerBatchId), '<accountId>', @accountid)

  exec (@declareCommandText + @commandText + @viewSelectCommandText +
      @viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText)
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequestBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCommonNationalExamCertificateRequestBatch

-- =============================================
-- Получить список пакетных запросов сертификатов.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateRequestBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
  , @isTypographicNumber bit = 0
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(max)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  set @accountId = isnull(
    (select account.[Id] 
     from dbo.Account account with (nolock, fastfirstrow) 
     where account.[Login] = @login), 0)

  if exists ( select top 1 1 from [Account] as a2
        join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
        where a2.[Login] = @login )
    set @accountId = null

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table 
      ( 
        Id bigint 
        , CreateDate datetime 
        , IsProcess bit 
        , IsCorrect bit 
        , Login varchar(255) 
        , Year int
        , Total int
        , Found int
      ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> 
          rb.Id 
          , rb.CreateDate 
          , rb.IsProcess  
          , rb.IsCorrect  
          , a.login 
          , rb.year
          , (select count(*) from CommonNationalExamCertificateRequest r with(nolock) where r.batchid = rb.id) Total
          , (select count(SourceCertificateIdGuid) from CommonNationalExamCertificateRequest r with(nolock) where r.batchid = rb.id) Found
        from dbo.CommonNationalExamCertificateRequestBatch rb with (nolock) 
        left join account a on a.id = rb.OwnerAccountId 
        where rb.OwnerAccountId = isnull(@accountId, rb.OwnerAccountId) 
        and rb.IsTypographicNumber = @isTypographicNumber '
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.CommonNationalExamCertificateRequestBatch rb with (nolock) ' +
        'where rb.OwnerAccountId = isnull(@accountId, rb.OwnerAccountId) ' +
        'and rb.IsTypographicNumber = @isTypographicNumber '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'CreateDate'
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select 
        dbo.GetExternalId(s.Id) Id 
        , s.Login 
        , s.CreateDate 
        , s.IsProcess 
        , s.IsCorrect 
        , s.Year
        , s.Total
        , s.Found
      from @search s ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint, @isTypographicNumber bit'
    , @accountId
    , @isTypographicNumber

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequest]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
alter proc [dbo].[SearchCommonNationalExamCertificateRequest]
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
      Id bigint
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
      , SourceCertificateId nvarchar(255)
      , SourceCertificateYear int
      , TypographicNumber nvarchar(255)
      , [Status] nvarchar(255)
      , ParticipantID uniqueidentifier
      , primary key(id)
      )
    '

  if isnull(@showCount, 0) = 0
    set @commandText = 
      N'select <innerHeader>
        dbo.GetExternalId(cne_certificate_request.Id) [Id]
        , dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
        , cne_certificate_request.SourceCertificateNumber CertificateNumber
        , isnull(cnec.LastName, cne_certificate_request.LastName) LastName
        , isnull(cnec.FirstName, cne_certificate_request.FirstName) FirstName
        , isnull(cnec.PatronymicName, cne_certificate_request.PatronymicName) PatronymicName
        , isnull(cnec.PassportSeria, cne_certificate_request.PassportSeria) PassportSeria
        , isnull(cnec.PassportNumber, cne_certificate_request.PassportNumber) PassportNumber
        , case
          when not cne_certificate_request.SourceCertificateIdGuid is null then 1
          else 0
        end IsExist
        , region.Name RegionName
        , region.Code RegionCode
        , isnull(cne_certificate_request.IsDeny, 0) IsDeny 
        , cne_certificate_request.DenyComment DenyComment
        , cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
        , cne_certificate_request.SourceCertificateIdGuid
        , cne_certificate_request.SourceCertificateYear
        , cne_certificate_request.TypographicNumber
        , case when cne_certificate_request.SourceCertificateIdGuid is null then ''Не найдено'' else 
          case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= 

ed.[ExpireDate] then ''Действительно'' 
          else ''Истек срок'' end end as [Status]
        , cne_certificate_request.ParticipantID
      from 
        dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
            on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
          left outer join dbo.Region region with (nolock)
            on region.[Id] = cne_certificate_request.SourceRegionId
          left join [ExpireDate] as ed with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = ed.[Year] 
          left join vw_Examcertificate cnec with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateIdGuid = cast(cnec.id as nvarchar(255)) 
      where 1 = 1 '
  else
    set @commandText = 
      N'
      select count(*)
      from 
        dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
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

    set @commandText = replace(replace(replace(
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
          CertificateId uniqueidentifier 
          , Mark nvarchar(10)
          , HasAppeal bit  
          , SubjectCode nvarchar(255)  
          , HasExam bit
          , primary key(CertificateId, SubjectCode)
          ) 
        '

      set @commandText = @commandText +
        N'insert into @subjects  
        select
          cne_certificate_subject.CertificateFK 
          , case when cne_certificate_subject.[Mark] < mm.[MinimalMark] then ''!'' else '''' 

end + replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
          , cne_certificate_subject.HasAppeal
          , subject.Code
          , 1 
        from  
          [prn].CertificatesMarks cne_certificate_subject
          left outer join dbo.Subject subject on subject.Id = 

cne_certificate_subject.SubjectCode
          left join [MinimalMark] as mm on cne_certificate_subject.[SubjectCode] = 

mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
        where 
          exists(select 1 
              from @search search
              where cast(cne_certificate_subject.CertificateFK as nvarchar(255)) = 

search.SourceCertificateId
                and cne_certificate_subject.[UseYear] = 

search.SourceCertificateYear)
        ' 
    end
    
    set @viewSelectCommandText = 
      N'select
        search.Id 
        , search.BatchId
        , case when search.SourceCertificateId =''Нет свидетельства'' then ''Нет свидетества'' else search.CertificateNumber end CertificateNumber
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
          on cast(unique_cheks.idGUID as nvarchar(255)) = search.SourceCertificateId '

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
            N'  , mrk_pvt.[<code>] [<code>Mark]  
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
        N'left outer join (select 
          subjects.CertificateId
          , subjects.Mark 
          , subjects.SubjectCode
          from @subjects subjects) subjects
            pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt
          on search.SourceCertificateId = cast(mrk_pvt.CertificateId as nvarchar(255))
          left outer join (select 
            subjects.CertificateId
            , cast(subjects.HasAppeal as int) HasAppeal 
            , subjects.SubjectCode
            from @subjects subjects) subjects
              pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as 

apl_pvt
            on search.SourceCertificateId = cast(apl_pvt.CertificateId  as nvarchar(255)) '
        set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
    end
          
    if isnull(@isExtendedByExam, 0) = 1
    begin
      set @viewCommandText = @viewCommandText + 
        N'left outer join (select 
          subjects.CertificateId
          , subjects.SubjectCode
          , cast(subjects.HasExam as int) HasExam 
          from @subjects subjects) subjects
            pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
          on search.SourceCertificateId = cast(exam_pvt.CertificateId as nvarchar(255)) '
          
      set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
    end
  end

  set @viewCommandText = @viewCommandText + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
      @viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

  print @commandText

  exec sp_executesql @commandText
    , N'@innerBatchId bigint'
    , @innerBatchId
    
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchSchoolLeavingCertificateCheckBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchSchoolLeavingCertificateCheckBatch
-- ===================================================
-- Поиск пакетов проверки аттестатов 
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ===================================================
alter procedure [dbo].[SearchSchoolLeavingCertificateCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
    declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @accountId bigint

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'CreateDate'
  set @sortAsc = 0

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'
      declare @search table
        (
        Id bigint
        , CreateDate datetime
        , IsProcess bit 
        , IsCorrect bit
        )
        '

  if isnull(@showCount, 0) = 0 
    set @commandText = 
      N'
      select <innerHeader>
        schoolleaving_certificate_check_batch.Id
        , schoolleaving_certificate_check_batch.CreateDate
        , schoolleaving_certificate_check_batch.IsProcess
        , schoolleaving_certificate_check_batch.IsCorrect
      from 
        dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch
      where 
        schoolleaving_certificate_check_batch.OwnerAccountId = @accountId 
      '
  else 
    set @commandText = 
      N'
      select count(*)
      from dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch
      where schoolleaving_certificate_check_batch.OwnerAccountId = @accountid
      '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = 'CreateDate'
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
    end
    else
    begin
      set @innerOrder = 'order by CreateDate <orderDirection> '
      set @outerOrder = 'order by CreateDate <orderDirection> '
      set @resultOrder = 'order by CreateDate <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' dbo.GetExternalId(search.Id) Id ' +
      ' , search.CreateDate ' +
      ' , search.IsProcess ' +
      ' , search.IsCorrect ' +
      'from @search search ' + @resultOrder
  
  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@accountId bigint'
    , @accountId
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchSchoolLeavingCertificateCheck]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchSchoolLeavingCertificateCheck
-- ==============================================
-- Получение списка проверенных аттестатов
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ==============================================
alter procedure [dbo].[SearchSchoolLeavingCertificateCheck]
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null 
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch with (nolock, fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on schoolleaving_certificate_check_batch.OwnerAccountId = account.[Id]
      where 
        schoolleaving_certificate_check_batch.Id = @internalBatchId
          and schoolleaving_certificate_check_batch.IsProcess = 0
          and account.[Login] = @login)
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'Id'
  set @sortAsc = 0
  
  
  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      'declare @search table ' +
      ' ( ' +
      ' Id bigint ' +
      ' , CertificateNumber nvarchar(255) ' +
      ' , IsDeny bit' +
      ' , DenyComment ntext ' +
      ' ) '

  if isnull(@showCount, 0) = 0
    set @commandText = 
        'select <innerHeader> ' +
        ' schoolleaving_certificate_check.Id ' +
        ' , schoolleaving_certificate_check.CertificateNumber ' +
        ' , case when school_leaving_certificate_deny.Id is null ' +
        '     then 0 ' +
        '   else 1 ' +
        ' end IsDeny ' + 
        ' , school_leaving_certificate_deny.Comment ' + 
        'from dbo.SchoolLeavingCertificateCheck schoolleaving_certificate_check with (nolock) ' +
        ' left join dbo.SchoolLeavingCertificateDeny school_leaving_certificate_deny with(nolock) ' +
        '   on school_leaving_certificate_deny.Id = schoolleaving_certificate_check.SourceCertificateDenyId ' +   
        'where 1 = 1 ' 
  else
    set @commandText = 
        'select count(*) ' +
        'from dbo.SchoolLeavingCertificateCheck schoolleaving_certificate_check with (nolock) ' +
        'where 1 = 1 ' 

  
  set @commandText = @commandText + 
    ' and schoolleaving_certificate_check.BatchId = @internalBatchId ' 

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = 'order by Id <orderDirection> '
      set @outerOrder = 'order by Id <orderDirection> '
      set @resultOrder = 'order by Id <orderDirection> '
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

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      'select ' +
      ' search.CertificateNumber CertificateNumber' +
      ' , search.IsDeny IsDeny ' + 
      ' , search.DenyComment DenyComment ' + 
      'from @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@internalBatchId bigint'
    , @internalBatchId
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchSameUserAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchSameUserAccount

-- =============================================
-- Поиск похожих учетных записей.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Поле наименования организации увеличено.
-- =============================================
alter procedure [dbo].[SearchSameUserAccount]
  @organizationName nvarchar(2000)
as
begin
  declare
    @userGroupId bigint
    , @currentYear int
    , @sameLavel decimal(18, 4)
    , @matchCount int
    , @shortOrganizationName nvarchar(2000)

  select 
    @userGroupId = [group].Id
  from 
    dbo.[Group] [group]
  where 
    [group].Code = 'User'

  set @currentYear = Year(GetDate())
  set @sameLavel = 0.7
  set @matchCount = 3
  set @shortOrganizationName = dbo.GetShortOrganizationName(@organizationName)

  select top 100
    search.[Login]
    , search.OrganizationName
    , search.LastName 
    , search.Status 
  from (select
      search.[Login]
      , search.OrganizationName
      , search.LastName 
      , search.Status 
      , dbo.CompareStrings(search.ShortOrganizationName
          , @shortOrganizationName, @matchCount) SameLevel
    from (select
        account.[Login] [Login]
        , account.LastName LastName 
        , dbo.GetUserStatus(account.ConfirmYear, account.Status
              , @currentYear, account.RegistrationDocument) Status 
        , Organization.[Name] OrganizationName
        , Organization.[ShortName] ShortOrganizationName
      from 
        dbo.Account account with (nolock)
          inner join dbo.Organization organization with (nolock)
            on organization.Id = account.OrganizationId
      where 
        account.IsActive = 1
        and account.Id in (select group_account.AccountId
            from dbo.GroupAccount group_account with (nolock)
            where group_account.GroupId = @userGroupId)) search) search
  where
    search.SameLevel >= @sameLavel
  order by
    search.SameLevel desc

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchRegion]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[SearchRegion]
as
begin
  select
    region.[Id] RegionId
    , region.[Name] [Name]
  from dbo.Region region
  where region.InOrganization = 1
  order by region.[Name] --region.SortIndex

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение списка ошибок задания на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]
  @taskId bigint = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit
    , @internalTaskId bigint
    , @server nvarchar(30)

  set @internalTaskId = dbo.GetInternalId(@taskId)

  set @declareCommandText = ''
  set @commandText = ''
  set @viewCommandText = ''
  set @sortColumn = N'Date'
  set @sortAsc = 1

  select @server = (select top 1 ss.name + '.fbs_loader_db' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = 'loader')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N'declare @search table ' +
      N'  ( ' +
      N'  TaskId bigint ' +
      N'  , Date datetime ' +
      N'  , RowIndex bigint ' +
      N'  , Error ntext ' +
      N'  ) ' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N'select <innerHeader> ' +
        N'  cne_certificate_loading_task_error.TaskId TaskId ' +
        N'  , cne_certificate_loading_task_error.Date Date ' +
        N'  , cne_certificate_loading_task_error.RowIndex RowIndex ' +
        N'  , cne_certificate_loading_task_error.Error Error ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error with (nolock) ' +
        N'where 1 = 1'
  else
    set @commandText = 
        N'select count(*) ' +
        N'from ' + @server + N'.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error with (nolock) ' +
        N'where 1 = 1'

  if not @taskId is null
    set @commandText = @commandText + N' and cne_certificate_loading_task_error.TaskId = @internalTaskId '

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N'Date'
    begin
      set @innerOrder = 'order by Date <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection> '
    end
    else 
    begin
      set @innerOrder = 'order by Date <orderDirection> '
      set @outerOrder = 'order by Date <orderDirection> '
      set @resultOrder = 'order by Date <orderDirection> '
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
      set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
      set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
      set @outerSelectHeader = replace(N'top <count>', N'<count>', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N'top 10000000'
      set @outerSelectHeader = N'top 10000000'
    end

    set @commandText = replace(replace(replace(
        N'insert into @search ' +
        N'select <outerHeader> * ' + 
        N'from (<innerSelect>) as innerSelect ' + @outerOrder
        , N'<innerSelect>', @commandText + @innerOrder)
        , N'<innerHeader>', @innerSelectHeader)
        , N'<outerHeader>', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N'option (keepfixed plan) '

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N'select ' +
      N'  dbo.GetExternalID(search.TaskId) TaskId ' +
      N'  , search.Date Date ' +
      N'  , search.RowIndex RowIndex ' +
      N'  , search.Error Error ' +
      N'from ' +
      N'  @search search ' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N'@internalTaskId bigint'
    , @internalTaskId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetUserInfo]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------
-- Получение информации о пользователе
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
alter procEDURE [dbo].[Operator_GetUserInfo]
( @OperatorLogin nvarchar(255), 
  @UserLogin nvarchar(255), 
  @IsMainOperator bit out, 
  @MainOperatorName varchar(255) out, 
  @Comments varchar(1024) out)
AS 
  SET NOCOUNT ON
  DECLARE @UserID int, @OperatorID int
  SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin
  SELECT @UserID=ID FROM dbo.Account WHERE [Login]=@UserLogin

  -- вставляем, если нет связи и другим оператором
  INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
  SELECT A.ID CheckedUserID, @OperatorID OperatorID
  FROM dbo.Account A
  LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
  WHERE A.ID=@UserID AND OL.CheckedUserID IS NULL

  -- данные о текущем пользователе
  SELECT 
    @IsMainOperator=CASE WHEN A.ID=@OperatorID THEN 1 ELSE 0 END,
    @MainOperatorName=A.LastName+' '+A.FirstName +'('+A.Login+')',
    @Comments=OL.Comments
  FROM dbo.OperatorLog OL
  INNER JOIN dbo.Account A ON OL.OperatorID=A.ID
  WHERE CheckedUserID=@UserID

  PRINT @@ROWCOUNT
  
  -- данные об остальных 'моих' пользователях
  SELECT A.Login, A.LastName+' '+FirstName FIO
  FROM dbo.OperatorLog OL
  INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
  WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID 
    AND A.Status='consideration'
    AND (Comments IS NULL OR LEN(RTRIM(Comments))=0)
    

  -- данные об остальных 'моих' пользователях с комментариями
  SELECT A.Login, A.LastName+' '+FirstName FIO
  FROM dbo.OperatorLog OL
  INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
  WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID
    AND A.Status='consideration'
    AND LEN(RTRIM(Comments))>0
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetNewUser]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------
-- Получение 1-го "не обработанного пользователя"
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
alter procEDURE [dbo].[Operator_GetNewUser]
( @OperatorLogin nvarchar(255), 
  @UserID int out, 
  @UserLogin nvarchar(255) out
)
AS 
  SET NOCOUNT ON
  DECLARE @OperatorID int
  DECLARE  @T TABLE(CheckedUserID int) 

  SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin

  INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
    OUTPUT INSERTED.CheckedUserID INTO @T(CheckedUserID)
  SELECT TOP 1 
    A.ID CheckedUserID, 
    @OperatorID OperatorID
  FROM dbo.Account A
  INNER JOIN dbo.Organization O ON A.OrganizationID=O.Id AND O.EtalonOrgID IS NOT NULL
  INNER JOIN dbo.GroupAccount GA ON A.ID=GA.AccountId AND GA.GroupId=1
  LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
  WHERE A.Status='consideration' AND OL.CheckedUserID IS NULL
  ORDER BY A.CreateDate 

  SELECT TOP 1 @UserID=A.ID, @UserLogin=A.[Login]
  FROM dbo.Account A
  WHERE A.ID IN (SELECT CheckedUserID FROM @T)
GO
/****** Object:  StoredProcedure [dbo].[Operator_AddUserComment]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------
-- Добавление комментария пользователю
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
alter procEDURE [dbo].[Operator_AddUserComment]
(@UserLogin nvarchar(255), @Comment varchar(1024))
AS 
  SET NOCOUNT ON

  UPDATE dbo.OperatorLog
  SET Comments=@Comment, DTLastChange=GETDATE()
  WHERE CheckedUserID IN (SELECT ID FROM dbo.Account WHERE [Login]=@UserLogin)
GO
/****** Object:  StoredProcedure [dbo].[GetDelivery]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
alter proc [dbo].[GetDelivery]
  @id bigint
as
begin
  select
    @id [Id]
    , delivery.Title Title
    , delivery.[Message] [Message]
    , delivery.[CreateDate] [CreateDate]
    , delivery.DeliveryDate DeliveryDate
    , delivery.TypeCode TypeCode
  from 
    dbo.Delivery delivery with (nolock, fastfirstrow)
  where
    delivery.[Id] = @id

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:    Сулиманов А.М.
-- Create date: 2009-05-07
-- Description: Удаление из БД всего, что касается AccountId (не анализируются связи)
-- =============================================
alter procEDURE [dbo].[DeleteAccount]
(@AccountID int)
AS
BEGIN
  SET NOCOUNT ON;
  
  DELETE FROM dbo.AccountIp WHERE AccountId=@AccountID
  DELETE FROM dbo.AccountKey WHERE AccountId=@AccountID
  DELETE FROM dbo.AccountLog WHERE AccountId=@AccountID
  DELETE FROM dbo.AccountRoleActivity WHERE AccountId=@AccountID
  DELETE FROM dbo.GroupAccount WHERE AccountId=@AccountID
  DELETE FROM dbo.Organization WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Id=@AccountID)
  DELETE FROM dbo.UserAccountPassword WHERE AccountId=@AccountID
  DELETE FROM dbo.Account WHERE Id=@AccountID
END
GO
/****** Object:  StoredProcedure [dbo].[RefreshRoleActivity]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Обновить активность ролей.
-- v.1.0: Created by Fomin Dmitriy 13.06.2008
-- v.1.1: Modified by Makarev Andrey 23.06.2008
-- Добавлен параметр @accountLogin.
-- =============================================
alter proc [dbo].[RefreshRoleActivity]
  @accountId bigint = null
  , @accountLogin nvarchar(255) = null
as
begin
  declare
    @checkAccountId bigint
    , @checkRoleId int
    , @condition nvarchar(max)
    , @commandText nvarchar(max)

  declare @checkingAccount table
    (
    AccountId bigint
    , UpdateDate datetime
    )
  
  if @accountId is null
  begin
    if @accountLogin is null
      insert into @checkingAccount
      select
        account.Id
        , Account.UpdateDate
      from 
        dbo.Account account
      where 
        account.IsActive = 1
    else
      insert into @checkingAccount
      select
        account.Id
        , Account.UpdateDate
      from 
        dbo.Account account
      where 
        account.IsActive = 1
        and account.Login = @accountLogin
  end
  else
  begin
    if @accountLogin is null
      insert into @checkingAccount
      select
        account.Id
        , account.UpdateDate
      from dbo.Account account
      where account.IsActive = 1
        and account.Id = @accountId
    else
      insert into @checkingAccount
      select
        account.Id
        , account.UpdateDate
      from 
        dbo.Account account
      where 
        account.IsActive = 1
        and account.Id = @accountId
        and account.Login = @accountLogin       
  end

  create table #Activity 
    (
    AccountId bigint
    , RoleId int
    )

  declare activity_cursor cursor forward_only for 
  select
    account_role.AccountId
    , account_role.RoleId
    , account_role.IsActiveCondition
  from dbo.AccountRole account_role 
  where not account_role.IsActiveCondition is null
    and account_role.AccountId in (select AccountId from @checkingAccount)

  open activity_cursor
  fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
  while @@fetch_status <> -1
  begin
    set @commandText = replace(replace(replace(
      'insert into #Activity 
      select
        activity.AccountId
        , <roleId> RoleId
      from (select 
          account.Id AccountId
          , case
            when <condition> then 1
            else 0
          end IsActive
        from dbo.Account account 
        where account.Id = <accountId>) activity 
      where activity.IsActive = 1 '
      , '<accountId>', @checkAccountId)
      , '<roleId>', @checkRoleId)
      , '<condition>', @condition)

    exec (@commandText)

    fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
  end
  close activity_cursor
  deallocate activity_cursor

  if exists(select 1
      from (select
            account_activity.RoleId
            , account_activity.AccountId
          from dbo.AccountRoleActivity account_activity
          where account_activity.AccountId in (select AccountId from @checkingAccount)) account_activity
        full outer join #Activity activity
          on account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId)
  begin
    begin tran activity
      delete account_activity
      from dbo.AccountRoleActivity account_activity
      where 
        account_activity.AccountId in (select AccountId from @checkingAccount)
        and not exists(select 1
          from #Activity activity
          where account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId)

      update account_activity
      set UpdateDate = GetDate()
      from dbo.AccountRoleActivity account_activity with(rowlock)
        inner join @checkingAccount account
          on account.AccountId = account_activity.AccountId
        inner join #Activity activity
          on account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId
      where
        account.UpdateDate > account_activity.UpdateDate

      insert into dbo.AccountRoleActivity
        (
        CreateDate
        , UpdateDate
        , AccountId
        , RoleId
        )
      select
        GetDate()
        , GetDate()
        , activity.AccountId
        , activity.RoleId
      from #Activity activity
      where not exists(select 1
          from dbo.AccountRoleActivity account_activity
          where account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId)
    commit tran activity
  end

  drop table #Activity 

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountLog]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetUserAccountLog

-- =============================================
-- Получить лог учетной записи пользователя.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- v.1.3: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.4: Modified by Sedov Anton 25.06.2008
-- Удалено поле RegistrationDocument
-- вместо него возвращается null
-- v.1.5: Modified by Sedov Anton 10.07.2008
-- В результат добавлено поле
-- EducationInstitutionTypeName
-- =============================================
alter procedure [dbo].[GetUserAccountLog]
  @login nvarchar(255)
  , @versionId int
as
begin
  declare
    @accountId bigint

  select @accountId = account.Id
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  select
    account_log.[Login] [Login]
    , account_log.VersionId VersionId
    , account_log.UpdateDate UpdateDate
    , editor.[Login] EditorLogin
    , editor.LastName EditorLastName
    , editor.FirstName EditorFirstName
    , editor.PatronymicName EditorPatronymicName
    , account_log.EditorIp EditorIp
    , account_log.IsVpnEditorIp IsVpnEditorIp
    , account_log.LastName LastName
    , account_log.FirstName FirstName
    , account_log.PatronymicName PatronymicName
    , organization_log.RegionId OrganizationRegionId
    , region.[Name] OrganizationRegionName
    , organization_log.[Name] OrganizationName
    , organization_log.FounderName OrganizationFounderName
    , organization_log.Address OrganizationAddress
    , organization_log.ChiefName OrganizationChiefName
    , organization_log.Fax OrganizationFax
    , organization_log.Phone OrganizationPhone
    , account_log.Phone Phone
    , account_log.Email Email
    , account_log.IpAddresses IpAddresses
    , account_log.HasFixedIp HasFixedIp
    , null RegistrationDocument
    , account_log.AdminComment AdminComment
    , account_log.Status Status
    , account_log.HasCrocEgeIntegration HasCrocEgeIntegration
    , education_institution_type.[Name] EducationInstitutionTypeName
  from
    dbo.AccountLog account_log with (nolock, fastfirstrow)
      left outer join dbo.OrganizationLog organization_log with (nolock, fastfirstrow)
        left join dbo.OrganizationType2010 education_institution_type
          on education_institution_type.Id = organization_log.EducationInstitutionTypeId
        on account_log.OrganizationId = organization_log.OrganizationId
        and organization_log.UpdateId = (select top 1 last_linked_account_log.UpdateId
            from dbo.AccountLog last_linked_account_log with (nolock, fastfirstrow)
            where last_linked_account_log.AccountId = @accountId
              and last_linked_account_log.VersionId = (select max(inner_account_log.VersionId)
                  from dbo.AccountLog inner_account_log with (nolock)
                    inner join dbo.OrganizationLog inner_organization_log with (nolock)
                      on inner_account_log.OrganizationId = inner_organization_log.OrganizationId
                        and inner_account_log.UpdateId = inner_organization_log.UpdateId
                  where inner_account_log.AccountId = @accountId
                    and inner_account_log.VersionId <= @versionId))
      left outer join dbo.Region region with (nolock, fastfirstrow)
        on organization_log.RegionId = region.[Id]
      left outer join dbo.Account editor with (nolock, fastfirstrow)
        on editor.Id = account_log.EditorAccountId
  where
    account_log.AccountId = @accountId
    and account_log.VersionId = @versionId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetBatchStatusById]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[GetBatchStatusById]
    @batchUniqueId uniqueidentifier,
    @userLogin varchar(255),
    @isProcess bit output,
    @isCorrect bit output,
    @isFound bit output,
    @searchType int output
AS
BEGIN
    declare @externalId bigint
    declare @internalId bigint
    declare @accountId bigint

    set @isProcess = 0
    set @isCorrect = 0
    set @isFound = 0
    set @searchType = 0
    
    -- Выполняем перекодировку номера пакета из GUID во внешний код
    set @externalId = isnull((select B.Id from dbo.BatchGUID B where B.BatchUniqueId = @batchUniqueId), -1)
    if (@externalId = -1)
    begin
      return
    end
    
    -- Выполняем поиск кода пользователя
    set @accountId = isnull((select A.Id from dbo.Account A where A.[Login] = @userLogin), -1)
    if (@accountId = -1)
    begin
      return
    end

    -- Выполняем поиск сертификата по внешнему коду
    select 
      @isProcess = isnull(C.IsProcess, 0),
        @isCorrect = isnull(C.IsCorrect, 1),
        @searchType = C.SearchType,
        @isFound = 1
    from 
      (
        select CB.Id, CB.IsProcess, CB.IsCorrect, 1 SearchType
        from CommonNationalExamCertificateCheckBatch CB
        where CB.OwnerAccountId = @accountId
        union all
        select RB.Id, RB.IsProcess, RB.IsCorrect, (case RB.IsTypographicNumber when 1 then 3 else 2 end) SearchType
      from CommonNationalExamCertificateRequestBatch RB
        where RB.OwnerAccountId = @accountId
        ) C
    where 
      C.Id = dbo.GetInternalId(@externalId)
END
GO
/****** Object:  StoredProcedure [dbo].[GetAskedQuestion]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAskedQuestion

-- =============================================
-- Получение вопроса.
-- Если @isViewCount = 1, то ViewCount увеличить на 1 для показываемой записи.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
alter proc [dbo].[GetAskedQuestion]
  @id bigint
  , @isViewCount bit = 1
as
begin
  declare 
    @internalId bigint
  
  set @internalId = dbo.GetInternalId(@id)

  select
    @id [Id]
    , asked_question.Name [Name]
    , asked_question.Question Question
    , asked_question.Answer Answer
    , asked_question.IsActive IsActive
    , asked_question.ContextCodes ContextCodes
    , asked_question.Popularity Popularity
    , asked_question.ViewCount ViewCount
  from 
    dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
  where
    asked_question.[Id] = @internalId

  if @isViewCount = 1
    update asked_question
    set 
      ViewCount = ViewCount + 1
    from 
      dbo.AskedQuestion asked_question with (rowlock)
    where
      asked_question.[Id] = @internalId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountRole]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить роли учетной записи.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение офомления.
-- =============================================
alter procedure [dbo].[GetAccountRole]
  @login nvarchar(255) -- логин учетной записи
as
begin

  select
    @login [Login]
    , account_role.RoleCode RoleCode
  from
    dbo.AccountRole account_role with (nolock, fastfirstrow)
      inner join dbo.Account account with (nolock, fastfirstrow)
        on account_role.AccountId = account.[Id]
  where
    account.[Login] = @login
    and (account_role.IsActiveCondition is null
      or exists(select 1
        from dbo.AccountRoleActivity activity
        where activity.AccountId = account_role.AccountId
          and activity.RoleId = account_role.RoleId
          and activity.UpdateDate >= account.UpdateDate))

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountLog]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAccountLog

-- =============================================
-- Получить лог учетной записи.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- =============================================
alter procedure [dbo].[GetAccountLog]
  @login nvarchar(255)
  , @versionId int
as
begin

  select
    account_log.[Login] [Login]
    , account_log.VersionId VersionId
    , account_log.UpdateDate UpdateDate
    , editor.[Login] EditorLogin
    , editor.LastName EditorLastName
    , editor.FirstName EditorFirstName
    , editor.PatronymicName EditorPatronymicName
    , account_log.EditorIp EditorIp
    , account_log.IsVpnEditorIp IsVpnEditorIp
    , account_log.LastName LastName
    , account_log.FirstName FirstName
    , account_log.PatronymicName PatronymicName
    , account_log.Phone Phone
    , account_log.Email Email
    , account_log.IpAddresses IpAddresses
    , account_log.HasFixedIp HasFixedIp
    , account_log.IsActive IsActive
  from
    dbo.AccountLog account_log with (nolock, fastfirstrow)
      inner join dbo.Account account with (nolock, fastfirstrow)
        on account_log.AccountId = account.[Id]
      left outer join dbo.Account editor with (nolock, fastfirstrow)
        on editor.Id = account_log.EditorAccountId
  where
    account.[Login] = @login
    and account_log.VersionId = @versionId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountKey]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAccountKey
-- ====================================================
-- Получение ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
alter procedure [dbo].[GetAccountKey]
  @login nvarchar(255)
  , @key nvarchar(255)
as
begin
  select
    account.Login [Login]
    , account_key.[Key]
    , account_key.DateFrom
    , account_key.DateTo
    , account_key.IsActive
  from dbo.Account account
    inner join dbo.AccountKey account_key
      on account.Id = account_key.AccountId
  where
    account.Login = @login
    and account_key.[Key] = @key

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountGroup]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAccountGroup

-- =============================================
-- Получение группы пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- =============================================
alter procedure [dbo].[GetAccountGroup]
  @login nvarchar(50)
as
begin
  select top 1
    account.[Login] [Login]
    , [group].Code GroupCode
  from dbo.GroupAccount group_account with (nolock, fastfirstrow)
    inner join dbo.Account account with (nolock, fastfirstrow)
      on group_account.AccountId = account.Id
    inner join dbo.[Group] [group] with (nolock, fastfirstrow)
      on [group].Id = group_account.GroupId
  where account.[Login] = @login

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение информации об учетной записи 
-- внутреннего пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- v.1.1: Modified by Makarev Andrey 10.04.2008
-- Добавлено поле Account.IpAddresses
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.2.0: Modified by A. Vinichenko 12.04.2011
-- =============================================
alter procedure [dbo].[GetAccount]
  @login nvarchar(255)
as
begin
  select
    account.[Login] [Login]
    , account.LastName LastName 
    , account.FirstName FirstName
    , account.PatronymicName PatronymicName
    , account.Email Email
    , account.Phone Phone
    , account.IsActive IsActive
    , account.IpAddresses IpAddresses
    , account.HasFixedIp HasFixedIp
    , account.UpdateDate UpdateDate
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetNewUserLogin]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetNewUserLogin
-- =============================================
-- Генерируется новый логин пользователя. 
-- Логин генерируется в формате user<1..100000>'
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.03.2008
-- Приведение к стандарту
-- =============================================
alter proc [dbo].[GetNewUserLogin]
  @login nvarchar(255) output
as
begin
  declare 
    @newLogin nvarchar(255)
    , @needNewLogin int
  set @needNewLogin = 1
  while @needNewLogin = 1 begin
    set @newLogin = N'user' + convert(nvarchar, Floor(Rand(CheckSum(NewId())) * 100000))
    if not exists(select 1 
            from
              dbo.Account account with (nolock, fastfirstrow)
            where
              account.[Login] = @newLogin)
      set @needNewLogin = 0
  end
  
  set @login = @newLogin

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetNews]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetNews

-- =============================================
-- Получить детальную информацию о новости.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим наименование новости.
-- =============================================
alter proc [dbo].[GetNews]
  @id bigint
as
begin
  declare @internalId bigint

  set @internalId = dbo.GetInternalId(@id)

  select
    @id [Id]
    , news.Date Date
    , news.Description Description
    , news.[Text] [Text]
    , news.IsActive IsActive
    , news.[Name] [Name]
  from 
    dbo.News news with (nolock, fastfirstrow)
  where
    news.[Id] = @internalId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetDocument]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetDocument

-- =============================================
-- Получение документа.
-- v.1.0: Created by Makarev Andrey 17.04.2008
-- v.1.1: Modified by Makarev Andrey 19.04.2008
-- Получение данных по внутреннему id.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
alter proc [dbo].[GetDocument]
  @id bigint
as
begin

  declare @internalId bigint

  set @internalId = dbo.GetInternalId(@id)

  select
    @id [Id]
    , [document].[Name] [Name]
    , [document].Description Description
    , [document].[Content] [Content]
    , [document].ContentSize ContentSize
    , [document].ContentType ContentType
    , [document].IsActive IsActive
    , [document].ActivateDate ActivateDate
    , [document].ContextCodes ContextCodes
    , [document].RelativeUrl RelativeUrl
  from 
    dbo.[Document] [document] with (nolock, fastfirstrow)
  where
    [document].[Id] = @internalId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckNewUserAccountEmail]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckNewUserAccountEmail
-- =============================================
-- Проверка email нового пользователя 
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- =============================================
alter procedure [dbo].[CheckNewUserAccountEmail]
  @email nvarchar(255)
as
begin
  declare 
    @userGroupCode nvarchar(255)
    , @isValid bit
    , @currentYear int
    , @activatedStatus  nvarchar(255)

  set @userGroupCode = 'User'
  set @activatedStatus = 'activated'
  set @currentYear = Year(GetDate())

  if exists(select 1
      from 
        dbo.Account account with (nolock)
          inner join dbo.GroupAccount group_account with (nolock) 
            inner join dbo.[Group] [group] with (nolock)
              on [group].Id = group_account.GroupId
            on group_account.AccountId = account.Id
      where
        [group].Code = @userGroupCode
        and account.Email = @email
        and dbo.GetUserStatus(account.ConfirmYear, account.Status, @currentYear
          , account.RegistrationDocument) = @activatedStatus)
    set @isValid = 0
  else 
    set @isValid = 1

  select
    @email Email
    , @isValid IsValid
return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckNewLogin]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckNewLogin

-- =============================================
-- Проверка нового логина.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modofied by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- =============================================
alter procedure [dbo].[CheckNewLogin]
  @login nvarchar(255)
as
begin
  declare @isExists bit

  if exists(select 1
      from 
        dbo.Account account with (nolock, fastfirstrow)
      where
        account.[Login] = @login)
    set @isExists = 1
  else
    set @isExists = 0

  select
    @login [Login]
    , @isExists IsExists

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckUserAccountEmail]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[CheckUserAccountEmail]
  @login nvarchar(255)
  ,@email nvarchar(255)
  ,@IsUniq bit out
AS
BEGIN
  -- если e-mail не меняется, то считаем его уникальным
  IF EXISTS(  SELECT 1 FROM dbo.Account WITH (NOLOCK) 
        WHERE Email = @email and [Login]=@login)
    SET @IsUniq = 1
  ELSE 
  IF EXISTS(  SELECT 1 FROM dbo.Account WITH (NOLOCK) 
        WHERE Email = @email and [Status]!='deactivated' and [Login]!=@login)
    SET @IsUniq = 0
  ELSE 
    SET @IsUniq = 1
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]
  @id bigint
as
begin
  declare @internalId bigint
  set @internalId = dbo.GetInternalId(@id)

  DELETE FROM [dbo].[CommonNationalExamCertificateRequestBatch]
      WHERE [Id]=@internalId
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Удаление проверки из лога
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
alter procEDURE [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]
  @id bigint
as
begin
  declare @internalId bigint
  set @internalId = dbo.GetInternalId(@id)

  DELETE FROM [dbo].[CommonNationalExamCertificateCheckBatch]
      WHERE [Id]=@internalId
end
GO
/****** Object:  StoredProcedure [dbo].[ExecuteChecksCount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[ExecuteChecksCount]
    @OrganizationId bigint,
    @CertificateId bigint = null,
    @CertificateIdGuid uniqueidentifier =null
AS
BEGIN
  if ((@CertificateId is null and @CertificateIdGuid is null) or @OrganizationId is null or @OrganizationId=0)
    return -1

  -- Выполняла ли данная организация проверку данного сертификата
    declare @isExists bit

  -- Значения инкрементов
  declare @uniqueIHEaFCheck int
  declare @uniqueIHECheck int
  declare @uniqueIHEFCheck int 

  declare @uniqueTSSaFCheck int
  declare @uniqueTSSCheck int
  declare @uniqueTSSFCheck int
    
  declare @uniqueRCOICheck int
  declare @uniqueOUOCheck int
  declare @uniqueFounderCheck int
  declare @uniqueOtherCheck int 
    
    -- Тип организации
    declare @orgType int
    
    -- Является ли организация филлиалом
    declare @isFilial bit

  -- Инициализация переменных
    set @isExists = 0
  set @uniqueIHEaFCheck = 0
  set @uniqueIHECheck = 0
  set @uniqueIHEFCheck = 0
  set @uniqueTSSaFCheck = 0
  set @uniqueTSSCheck = 0
  set @uniqueTSSFCheck = 0
  set @uniqueRCOICheck = 0
  set @uniqueOUOCheck = 0
  set @uniqueFounderCheck = 0
  set @uniqueOtherCheck = 0
    set @orgType = 0
    set @isFilial = 0

  if @OrganizationId is null 
    set @isExists = 
        (
      select count(*) 
      from [dbo].[OrganizationCertificateChecks] OCC 
      where OCC.CertificateId = @CertificateId and OCC.OrganizationId = @OrganizationId
      )
  else  
    set @isExists = 
        (
      select count(*) 
      from [dbo].[OrganizationCertificateChecks] OCC 
      where OCC.CertificateId = @CertificateId and OCC.CertificateIdGuid = @CertificateIdGuid
      )

    if (@isExists = 0)
    begin
      insert into [dbo].[OrganizationCertificateChecks] (CertificateId, OrganizationId, CertificateIdGuid)
        values (@CertificateId, @OrganizationId, @CertificateIdGuid)
        
        select
          @orgType = isnull(O.TypeId, 0),
            @isFilial = (case when (O.MainId is null) then 0 else 1 end)
        from
          [dbo].[Organization2010] O
        where
          O.Id = @OrganizationId

        if (@orgType = 0)
          return -1

    if (@orgType = 1)
        begin
          set @uniqueIHEaFCheck = 1
            if (@isFilial = 1)
              set @uniqueIHEFCheck = 1
            else
              set @uniqueIHECheck = 1
        end           
        
    if (@orgType = 2)
        begin
          set @uniqueTSSaFCheck = 1
            if (@isFilial = 1)
              set @uniqueTSSFCheck = 1
            else
              set @uniqueTSSCheck = 1
        end           
        
    if (@orgType = 3)
        begin
          set @uniqueRCOICheck = 1
        end           

    if (@orgType = 4)
        begin
          set @uniqueOUOCheck = 1
        end           

    if (@orgType = 6)
        begin
          set @uniqueFounderCheck = 1
        end           

    if (@orgType = 5)
        begin
          set @uniqueOtherCheck = 1
        end
    
        declare @year int
        if @CertificateId is null
      set @year = 
        (select top 1 C.UseYear
        from [prn].[Certificates] C
        where C.CertificateID = @CertificateIdGuid)
        else
      set @year = 
        (select top 1 C.[Year]
        from CommonNationalExamCertificate C
        where C.Id = @CertificateId)
        
    if not exists 
      (select *
      from [dbo].[ExamCertificateUniqueChecks] EC
      where EC.Id = @CertificateId) 
      and
      not exists (select *
      from [dbo].[ExamCertificateUniqueChecks] EC
      where EC.idGUID = @CertificateIdGuid) 
    begin
      insert into [dbo].[ExamCertificateUniqueChecks] 
        (
        [Year], 
        Id, 
        UniqueChecks,
        UniqueIHEaFCheck,
        UniqueIHECheck,
        UniqueIHEFCheck,
        UniqueTSSaFCheck,
        UniqueTSSCheck,
        UniqueTSSFCheck,
        UniqueRCOICheck,
        UniqueOUOCheck,
        UniqueFounderCheck,
        UniqueOtherCheck,
        idGUID
        )
      values 
        (
        @year, 
        @CertificateId,
        1,
        @uniqueIHEaFCheck,
        @uniqueIHECheck,
        @uniqueIHEFCheck,
        @uniqueTSSaFCheck,
        @uniqueTSSCheck,
        @uniqueTSSFCheck,
        @uniqueRCOICheck,
        @uniqueOUOCheck,
        @uniqueFounderCheck,
        @uniqueOtherCheck,
        @CertificateIdGuid
        )
    end
    else begin
        if @CertificateId is not null  
        update 
              [dbo].[ExamCertificateUniqueChecks]
        set 
          UniqueChecks = UniqueChecks + 1,
          UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
          UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
          UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
          UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
          UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
          UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
          UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
          UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
          UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
          UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
        where
              idGUID = @CertificateIdGuid
              and [Year] = @year
          else
        update 
              [dbo].[ExamCertificateUniqueChecks]
        set 
          UniqueChecks = UniqueChecks + 1,
          UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
          UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
          UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
          UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
          UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
          UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
          UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
          UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
          UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
          UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
        where
              idGUID = @CertificateIdGuid
              and [Year] = @year          
        end

        return 1
    end
    else begin
      return 0
    end
END
GO
/****** Object:  StoredProcedure [dbo].[GetCertificateByFioAndPassport]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Получение свидетельст за все года по ФИО и паспортным данным
-- Возвращаемы значения
-- Id - идентификатор свидетельства
-- CreateDate - Дата добавления сертификата
-- Number - номер свидетельства
-- Year - год
alter procEDURE [dbo].[GetCertificateByFioAndPassport]
  @LastName NVARCHAR(255) = null,       -- фамилия сертифицируемого
  @FirstName NVARCHAR(255) = null,        -- имя сертифицируемого
  @PatronymicName NVARCHAR(255) = null,     -- отчетсво сертифицируемого
  @PassportSeria NVARCHAR(20) = null,   -- серия документа сертифицируемого (паспорта)
  @PassportNumber NVARCHAR(20) = null,    -- номер документа сертифицируемого (паспорта)  
  @CurrentCertificateNumber NVARCHAR(255)   -- номер свидетельства, его нужно исключить при выборке
AS
BEGIN 
  declare @yearFrom int, @yearTo int
  select @yearFrom = 2008, @yearTo = Year(GetDate())
  
  SELECT  c.Id,
        c.CreateDate,
        c.Number,
        c.Year,
        marks,
        case when ed.[ExpireDate] is null then 'Не найдено'
             else case when isnull(certificate_deny.UseYear, 0) <> 0
                       then 'Аннулировано'
                       else case when getdate() <= ed.[ExpireDate]
                                 then 'Действительно'
                                 else 'Истек срок'
                            end
                  end
        end as [Status]
FROM    dbo.vw_Examcertificate c
        CROSS APPLY ( SELECT    ( SELECT    CAST(s.SubjectCode AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      [prn].[CertificatesMarks] s
                                  WHERE     s.CertificateFK = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                    ) as marks
        LEFT JOIN dbo.ExpireDate ed ON ed.Year = c.Year
        LEFT OUTER JOIN prn.CancelledCertificates certificate_deny
        with ( nolock, fastfirstrow ) on certificate_deny.[UseYear] between @yearFrom and @yearTo
                                         and certificate_deny.CertificateFK = c.id
WHERE    ( LastName = @LastName
            or @LastName is null
          )
          AND ( FirstName = @FirstName
                or @FirstName is null
              )
          AND ( PatronymicName = null
                or @PatronymicName is null
              )
          AND PassportNumber = @PassportNumber
          AND PassportSeria = @PassportSeria
          AND c.Number <> @CurrentCertificateNumber
END
GO
/****** Object:  StoredProcedure [dbo].[GetDeliveryRecipients]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
alter proc [dbo].[GetDeliveryRecipients]
  @id bigint
as
begin
  select
    recipients.RecipientCode RecipientCode
  from 
    dbo.DeliveryRecipients recipients with (nolock)
  where
    recipients.[DeliveryId] = @id

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[GetNEWebUICheckLog]
    @login NVARCHAR(255),
    @startRowIndex INT = 1,
    @maxRowCount INT = NULL,
    @showCount BIT = NULL,   -- если > 0, то выбирается общее кол-во
    @TypeCode NVARCHAR(255) -- Тип проверки
AS 
    BEGIN
        DECLARE @accountId BIGINT,
            @endRowIndex INTEGER

        IF ISNULL(@maxRowCount, -1) = -1 
            SET @endRowIndex = 10000000
        ELSE 
            SET @endRowIndex = @startRowIndex + @maxRowCount

        IF EXISTS ( SELECT  1
                    FROM    [Account] AS a2
                            JOIN [GroupAccount] ga ON ga.[AccountId] = a2.[Id]
                            JOIN [Group] AS g ON ga.[GroupId] = g.[Id]
                                                 AND g.[Code] = 'Administrator'
                    WHERE   a2.[Login] = @login ) 
            SET @accountId = NULL
        ELSE 
            SET @accountId = ISNULL(( SELECT    account.[Id]
                                      FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                                      WHERE     account.[Login] = @login
                                    ), 0)

        IF ISNULL(@showCount, 0) = 0 
            BEGIN 
                IF @accountId IS NULL 
                    SELECT  *
                    FROM    ( SELECT    b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )
                ELSE 
                    SELECT  *
                    FROM    ( SELECT TOP ( @endRowIndex )
                                        b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     b.AccountId = @accountId
                                                    AND @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )   
            END
        ELSE 
            IF @accountId IS NULL 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE )
            ELSE 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   b.AccountId = @accountId
                        AND @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE ) 
        RETURN 0
    END
GO
/****** Object:  StoredProcedure [dbo].[AddCNEWebUICheckEvent]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:    Yusupov K.I.
-- Create date: 04-06-2010
-- Description: Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
alter procEDURE [dbo].[AddCNEWebUICheckEvent]
@AccountLogin NVARCHAR(255),          -- логин проверяющего
  @LastName NVARCHAR(255)=null,       -- фамилия сертифицируемого
  @FirstName NVARCHAR(255)=null,        -- имя сертифицируемого
  @PatronymicName NVARCHAR(255)=null,     -- отчетсво сертифицируемого
  @PassportSeria NVARCHAR(20)=NULL,     -- серия документа сертифицируемого (паспорта)
  @PassportNumber NVARCHAR(20)=NULL,      -- номер документа сертифицируемого (паспорта)
  @CNENumber NVARCHAR(20)=NULL,       -- номер сертификата
  @TypographicNumber NVARCHAR(20)=NULL,   -- типографический номер сертификата 
  @RawMarks NVARCHAR(500)=null,       -- средние оценки по предметам (через запятую, в определенном порядке)
  @IsOpenFbs bit=null,
  @EventId INT output             -- id зарегистрированного события
AS
BEGIN
  IF (SELECT disablelog 
    FROM dbo.Organization2010 
     WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Login = @AccountLogin)) = 1
  BEGIN
  SELECT @EventId = NULL
  RETURN
  END
  
  IF 
  (
    @TypographicNumber IS NULL AND
    @CNENumber IS NULL AND
    @PassportNumber IS NULL AND
    @RawMarks IS NULL
  )
  BEGIN
    RAISERROR (N'Не указаны паспортные данные, типографский номер, номер свидетельства и баллы по предметам одновременно',10,1)
    RETURN
  END

  DECLARE @AccountId BIGINT
  SELECT
    @AccountId = Acc.[Id]
  FROM
    dbo.Account Acc WITH (nolock, fastfirstrow)
  WHERE
    Acc.[Login] = @AccountLogin 

  IF (@TypographicNumber IS NOT NULL)
  BEGIN
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,TypographicNumber) 
        VALUES 
      (@AccountId,'Typographic',@FirstName,@LastName,@PatronymicName,@TypographicNumber)
  END
  ELSE IF (@CNENumber IS NOT NULL)
  BEGIN
    declare @logTypeNumber nvarchar(50)
    set @logTypeNumber = 'CNENumber' + case when isnull(@IsOpenFbs,0) = 1 then 'Open' else '' end
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,CNENumber,Marks) 
        VALUES 
      (@AccountId,@logTypeNumber,@FirstName,@LastName,@PatronymicName,@CNENumber,@RawMarks)
  END
  ELSE IF (@PassportNumber IS NOT NULL)
  BEGIN
    declare @logTypePassport nvarchar(50)
    set @logTypePassport = 'Passport' + case when isnull(@IsOpenFbs,0) = 1 then 'Open' else '' end

    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,PassportSeria,PassportNumber,Marks) 
        VALUES 
      (@AccountId,@logTypePassport,@FirstName,@LastName,@PatronymicName,@PassportSeria,@PassportNumber,@RawMarks)
  END
  ELSE IF (@RawMarks IS NOT NULL)
  BEGIN
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,Marks) 
        VALUES 
      (@AccountId,'Marks',@FirstName,@LastName,@PatronymicName,@RawMarks)
  END
    SELECT @EventId = @@Identity
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение информации о пользователе.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.1.2: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.3: Modified by Fomin Dmitriy 07.07.2008
-- Добавлены поля EducationInstitutionTypeId, 
-- EducationInstitutionTypeName.
-- v.2.0 Modified by A.Vinichenko 14.04.2011
-- Информация об организации пользоватетеля берется 
-- из таблицы Organization2010
-- =============================================
alter procedure [dbo].[GetUserAccount]
  @login nvarchar(255)
as
begin
  declare @currentYear int, @accountId bigint--, @userGroupId int

  set @currentYear = Year(GetDate())

--  select @userGroupId = [group].Id
--  from dbo.[Group] [group] with (nolock, fastfirstrow)
--  where [group].Code = 'User'

  select @accountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  select account.[Login]
    , account.LastName
    , account.FirstName
    , account.PatronymicName
    , region.[Id] OrganizationRegionId
    , region.[Name] OrganizationRegionName
    , OReq.Id OrganizationId
    , OReq.FullName OrganizationName
    , OReq.OwnerDepartment OrganizationFounderName
    , OReq.LawAddress OrganizationAddress
    , OReq.DirectorFullName OrganizationChiefName
    , OReq.Fax OrganizationFax
    , OReq.Phone OrganizationPhone
    , OReq.EMail OrganizationEmail
    , OReq.Site OrganizationSite
    , OReq.ShortName OrganizationShortName
    , OReq.FactAddress OrganizationFactAddress
    , OReq.DirectorPosition OrganizationDirectorPosition
    , OReq.IsPrivate OrganizationIsPrivate
    , OReq.IsFilial OrganizationIsFilial
    , OReq.PhoneCityCode OrganizationPhoneCode
    , OReq.AccreditationSertificate AccreditationSertificate
    , OReq.INN OrganizationINN
    , OReq.OGRN OrganizationOGRN
    , account.Phone
    , account.Email
    , account.IpAddresses IpAddresses 
    , account.Status 
    , case
      when account.CanViewUserAccountRegistrationDocument = 1 
        then account.RegistrationDocument 
      else null
    end RegistrationDocument 
    , case
      when account.CanViewUserAccountRegistrationDocument = 1 
        then account.RegistrationDocumentContentType
      else null
    end RegistrationDocumentContentType
    , account.AdminComment AdminComment
    , dbo.CanEditUserAccount(account.Status, account.ConfirmYear, @currentYear) CanEdit
    , dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument 
    , account.HasFixedIp HasFixedIp
    , account.HasCrocEgeIntegration HasCrocEgeIntegration
    , OrgType.Id OrgTypeId
    , OrgType.[Name] OrgTypeName
    , OrgKind.Id OrgKindId
    , OrgKind.[Name] OrgKindName
    , OReq.Id OReqId
  from (select
        account.[Login] [Login]
        , account.LastName LastName
        , account.FirstName FirstName
        , account.PatronymicName PatronymicName
        , account.OrganizationId OrganizationId
        , account.Phone Phone
        , account.Email Email
        , account.ConfirmYear ConfirmYear
        , account.RegistrationDocument RegistrationDocument
        , account.RegistrationDocumentContentType RegistrationDocumentContentType
        , account.AdminComment AdminComment
        , account.IpAddresses IpAddresses
        , account.HasFixedIp HasFixedIp
        , account.HasCrocEgeIntegration HasCrocEgeIntegration
        , dbo.GetUserStatus(account.ConfirmYear, account.Status
            , @currentYear, account.RegistrationDocument) Status 
        , dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument
      from dbo.Account account with (nolock, fastfirstrow)
      where account.[Id] = @accountId
--          and account.Id in (
--            select group_account.AccountId
--            from dbo.GroupAccount group_account
--            where group_account.GroupId = @userGroupId)
      ) account
      left outer join dbo.Organization2010 OReq with (nolock, fastfirstrow) 
      left outer join dbo.Region region with (nolock, fastfirstrow) on region.[Id] = OReq.RegionId
      left outer join dbo.OrganizationType2010 OrgType on OReq.TypeId = OrgType.Id
      left outer join dbo.OrganizationKind OrgKind on OReq.KindId = OrgKind.Id
        on OReq.[Id] = account.OrganizationId
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateGroupUserEsrp]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[UpdateGroupUserEsrp]
  @login nvarchar(255),
  @groupIdEsrp int,
  @groupsEsrp nvarchar(255)
AS
BEGIN
  declare @accountId int
  select @accountId = A.Id
  from Account A
  where A.[Login] = @login
  
  declare @groupId int
  select @groupId = G.Id
  from [Group] G
  where G.GroupIdEsrp = @groupIdEsrp  
  
  if (@groupsEsrp is not null)  
  begin
    declare @sql nvarchar(1000)
    set @sql =
    'delete from GroupAccount
    where 
      GroupAccount.GroupId in (select G.Id
                   from [Group] G
                   where
                    G.GroupIdEsrp not in (' + @groupsEsrp + ') 
                   ) '+
      'and GroupAccount.AccountId = ' + cast(@accountId as nvarchar(255))
    exec sp_executesql @sql
  end
  
  insert into GroupAccount (GroupId, AccountId)
  select @groupId, @accountId 
  where not exists(select *
         from GroupAccount GA
         where GA.AccountId = @accountId
         and GA.GroupId = @groupId) 
  
  exec RefreshRoleActivity @accountId, null
  
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountEsrp]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procEDURE [dbo].[UpdateAccountEsrp]
  @login nvarchar(255),
  @lastName nvarchar(255),
  @firstName nvarchar(255),
  @patronymicName nvarchar(255),
  @organizationId int,
  @phone nvarchar(255),
  @email nvarchar(255),
  @status nvarchar(255),
  @isActive bit
  --@ipAddresses nvarchar(400) = null
as
begin
  --существованние пользователя
  declare @exists table([login] nvarchar(255), isExists bit)
  insert @exists exec dbo.CheckNewLogin @login = @login
  
  declare 
    @isExists bit,
    @eventCode nvarchar(255),
    @accountId bigint,
    @innerStatus nvarchar(255),
    @confirmYear int,
    @currentYear int,
    @userGroupId int,
    @updateId uniqueidentifier,
    @accountIds nvarchar(255)

  set @updateId = newid()
  
  select @isExists = user_exists.isExists
  from  @exists user_exists

  select @accountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  set @currentYear = year(getdate())
  set @confirmYear = @currentYear

  --declare @oldIpAddress table (ip nvarchar(255))

  --declare @newIpAddress table (ip nvarchar(255))
  
  --если логина нет - добавляем запись
  --если логин есть - меняем данные
  if @isExists = 0  -- внесение нового пользователя
  begin
    select 
      @eventCode = N'USR_REG'
      
    select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
  end
  else
  begin -- update существующего пользователя
    select 
      @accountId = account.[Id]
    from 
      dbo.Account account with (nolock, fastfirstrow)
    where
      account.[Login] = @login

    /*insert @oldIpAddress
      (
      ip
      )
    select
      account_ip.Ip
    from
      dbo.AccountIp account_ip with (nolock, fastfirstrow)
    where
      account_ip.AccountId = @accountId
      
    set @eventCode = N'USR_EDIT'
    
    insert @newIpAddress
      (
      ip
      )
    select 
      ip_addresses.[value]
    from 
      dbo.GetDelimitedValues(@ipAddresses) ip_addresses*/
      
  end

  begin tran insert_update_account_tran

    if @isExists = 0  -- внесение нового пользователя
    begin
      insert dbo.Account
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Login]
        , PasswordHash
        , LastName
        , FirstName
        , PatronymicName
        , OrganizationId
        , IsOrganizationOwner
        , ConfirmYear
        , Phone
        , Email
        , RegistrationDocument
        , AdminComment
        , IsActive
        , Status
        , IpAddresses
        , HasFixedIp
        )
      select
        getdate()
        , getdate()
        , @updateId
        , null
        , null
        , @login
        , null
        , @lastName
        , @firstName
        , @patronymicName
        , @organizationId
        , 0
        , @confirmYear
        , @phone
        , @email
        , null
        , null
        , @isActive
        , @status
        , null
        , null

      if (@@error <> 0)
        goto undo

      select @accountId = scope_identity()

      if (@@error <> 0)
        goto undo

      /*insert dbo.AccountIp
        (
        AccountId
        , Ip
        )
      select
        @accountId
        , new_ip_address.ip
      from 
        @newIpAddress new_ip_address

      if (@@error <> 0)
        goto undo*/
    end
    else
    begin -- update существующего пользователя
      update account
      set
        UpdateDate = getdate()
        , UpdateId = @updateId
        , EditorAccountID = null
        , EditorIp = null
        , LastName = @lastName
        , FirstName = @firstName
        , PatronymicName = @patronymicName 
        , phone = @phone
        , email = @email
        , [Status] = @status
        , IsActive = @isActive
        , IpAddresses = null--@ipAddresses
        , HasFixedIp = null
      from
        dbo.Account account with (rowlock)
      where
        account.[Id] = @accountId

      if (@@error <> 0)
        goto undo

      /*if exists(select 
            1
          from
            @oldIpAddress old_ip_address
              full outer join @newIpAddress new_ip_address
                on old_ip_address.ip = new_ip_address.ip
          where
            old_ip_address.ip is null
            or new_ip_address.ip is null) 
      begin
        delete account_ip
        from 
          dbo.AccountIp account_ip
        where
          account_ip.AccountId = @accountId

        if (@@error <> 0)
          goto undo

        insert dbo.AccountIp
          (
          AccountId
          , Ip
          )
        select
          @accountId
          , new_ip_address.ip
        from 
          @newIpAddress new_ip_address

        if (@@error <> 0)
          goto undo
      end*/
    end

  if @@trancount > 0
    commit tran insert_update_account_tran

  /*set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId*/
  
  return 0

  undo:

  rollback tran insert_update_account_tran

  return 1
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportEditedOrgsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportEditedOrgsTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Импортирована из справочника] nvarchar(13) null
)
AS 
BEGIN


INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
  WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
  THEN 'Нет'
  ELSE 'Есть'
  END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
  WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
  THEN 'Есть'
  ELSE 'Нет'
  END AS [Аккредитация по факту]  
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,CASE WHEN (Org.WasImportedAtStart=1)
  THEN 'Да'
  ELSE 'Нет'
  END AS [Импортирована из справочника]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
WHERE (Org.CreateDate != Org.UpdateDate AND Org.WasImportedAtStart =1) OR (Org.WasImportedAtStart=0)
ORDER BY Org.WasImportedAtStart


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsBASE]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgsBASE]()
  
  
RETURNS @report TABLE 
(
[Id] INT 
,[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
,[Дата создания] datetime null
,[Создана из справочника] bit null
,[Имя ФО] nvarchar(255) null
,[Код ФО] int null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN
 
INSERT INTO @Report
SELECT 
Org.Id as [Id]
,Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,Org.WasImportedAtStart AS [Создана из справочника]
,FD.[Name] AS [Имя ФО]
,FD.Code AS [Код ФО]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
  WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
  THEN 'Нет'
  ELSE 'Есть'
  END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
  WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
  THEN 'Есть'
  ELSE 'Нет'
  END AS [Аккредитация по факту]  
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]



FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN FederalDistricts FD
ON FD.Id=Reg.FederalDistrictId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId

RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckHistory]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- получить все уникальные проверки сертификата вузами и их филиалами
alter proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
  @certificateId uniqueidentifier,  -- id сертификата
  @startRow INT = NULL, -- пейджинг, если null - то выбирается кол-во записей для этого сертификата всего
  @maxRow INT = NULL    -- пейджинг
AS
BEGIN
-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
  SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg 
        INNER JOIN vw_Examcertificate c ON c.Number = lg.CertificateNumber
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
      FROM dbo.CheckCommonNationalExamCertificateLog lg 
          INNER JOIN vw_Examcertificate c ON c.Number = lg.CertificateNumber
        INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
        INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
        WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0) rowTable 
      WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow 
      ORDER BY org, rowTable.[Date] 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportPotentialAbusersTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Отчет: Отчет о потенциальных пользователях, осуществляющих перебор
alter funCTION [dbo].[ReportPotentialAbusersTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [Проверок] int null,
  [Логин] nvarchar(255) null,
  [ФИО] nvarchar(255) null, 
  [Организация] nvarchar(255) null,
  [Email] nvarchar(255) null,
  [Телефон] nvarchar(255) null
)
as
begin
;with WrongRequestCount ([count], [user]) as
  (select 
    count(distinct 
      cnecr.lastname 
      + isnull(cnecr.firstname,'') 
      + isnull(cnecr.PatronymicName,'')
      + isnull(cnecr.PassportSeria,'')
      + isnull(cnecr.PassportNumber,'')
      + isnull(cnecr.TypographicNumber,'')
      )
    , a.login
    from [CommonNationalExamCertificateRequestBatch] as cnecrb with(nolock) 
    join [CommonNationalExamCertificateRequest] as cnecr with(nolock) on cnecr.batchid = cnecrb.id and cnecr.SourceCertificateId is null
    join account a with(nolock) on a.id = cnecrb.owneraccountid
    join groupaccount ga with(nolock) on ga.accountid = a.id and ga.groupid = 1
  where cnecrb.createdate BETWEEN @periodBegin and @periodEnd 
  group by a.login)
,WrongCheckCount([count], [user]) as
  (select 
    count(distinct 
      cnecc.lastname 
      + isnull(cnecc.firstname,'') 
      + isnull(cnecc.PatronymicName,'')
      + isnull(cnecc.PassportSeria,'')
      + isnull(cnecc.PassportNumber,'')
      + isnull(cnecc.TypographicNumber,'')
      )
    , a.login
    from [CommonNationalExamCertificateCheckBatch] as cneccb with(nolock) 
    join [CommonNationalExamCertificateCheck] as cnecc with(nolock) on cnecc.batchid = cneccb.id and cnecc.SourceCertificateId is null
    join account a with(nolock) on a.id = cneccb.owneraccountid
    join groupaccount ga with(nolock) on ga.accountid = a.id and ga.groupid = 1
  where cneccb.createdate BETWEEN @periodBegin and @periodEnd 
  group by a.login)
insert into @report
select 
isnull(wrc.[count],0) + isnull(wcc.[count],0)
, coalesce(wrc.[user],wcc.[user])
, a.lastname
, o.FullName
, a.email
, a.phone
from WrongRequestCount wrc
full join WrongCheckCount wcc on wrc.[user] = wcc.[user]
join Account a with(nolock) on a.login = coalesce(wrc.[user],wcc.[user])
join GroupAccount ga with(nolock) on ga.accountid = a.id and ga.groupid = 1
join OrganizationRequest2010 o with(nolock) on o.id = a.organizationid
where isnull(wrc.[count],0) + isnull(wcc.[count],0) >= 1000
order by 1 desc

return
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgsInfoTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование ФО] nvarchar(255) null
,[Код ФО] nvarchar(255) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Подана заявка на регистрацию] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
,[Количество проверок по номеру] int null
,[Количество уникальных проверок по номеру] INT NULL
,[Количество проверок по паспортным данным] INT NULL
,[Количество уникальных проверок по паспортным данным] INT NULL
,[Количество проверок по типографскому номеру] INT NULL
,[Количество уникальных проверок по типографскому номеру] INT NULL
,[Количество интерактивных проверок] INT NULL
,[Количество уникальных интерактивных проверок] INT NULL
,[Количество неправильных проверок] INT NULL
,[Первая проверка] datetime null
,[Последняя проверка] datetime null
,[Работа с ФБС] NVARCHAR(20)
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
  SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq   
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
  OrganizationId int,
  [Status] nvarchar(50),
  UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq   
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
  OrganizationId int,
  [activated] nvarchar(20),
  [deactivated] nvarchar(20),
  [consideration] nvarchar(20),
  [registration] nvarchar(20),
  [revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq   
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @NumberChecksByOrg TABLE
(
  OrganizationId INT,
  TotalNumberChecks INT,
  UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
  OrganizationId INT,
  TotalTNChecks INT,
  UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
  OrganizationId INT,
  TotalPassportChecks INT,
  UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
GROUP BY IOrgReq.OrganizationId


DECLARE @WrongChecksByOrg TABLE
(
  OrganizationId INT,
  WrongChecks INT
)

INSERT INTO @WrongChecksByOrg
SELECT IWrong.OrganizationId,SUM(IWrong.WrongChecks) FROM 
(
  SELECT IOrgReq.OrganizationId AS OrganizationId,COUNT(*) AS WrongChecks
  FROM CommonNationalExamCertificateRequestBatch cb 
  INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND c.SourceCertificateId IS NOT NULL
  GROUP BY IOrgReq.OrganizationId
  UNION 
  SELECT IOrgReq.OrganizationId,COUNT(*) AS WrongChecks
  FROM CommonNationalExamCertificateCheckBatch cb 
  INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND c.SourceCertificateId IS NOT NULL
  GROUP BY IOrgReq.OrganizationId
) AS IWrong
GROUP BY IWrong.OrganizationId





DECLARE @UIChecksByOrg TABLE
(
  OrganizationId INT,
  TotalUIChecks INT,
  UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
ISNULL(ChLog.PassportSeria,'')+
ISNULL(ChLog.PassportNumber,'')+
ISNULL(ChLog.CNENumber,'')+
ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId



DECLARE @CheckLimitDatesByOrg TABLE
(
  OrganizationId INT,
  FirstCheck DATETIME,
  LastCheck DATETIME
)
INSERT INTO @CheckLimitDatesByOrg
SELECT OrganizationId,MIN(FirstCheck),MAX(LastCheck)
FROM 
(
  SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
  FROM CommonNationalExamCertificateRequestBatch cb 
  INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  GROUP BY IOrgReq.OrganizationId
  UNION 
  SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
  FROM CommonNationalExamCertificateCheckBatch cb 
  INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  GROUP BY IOrgReq.OrganizationId
  UNION 
  SELECT IOrgReq.OrganizationId,MIN(ChLog.EventDate) AS FirstCheck,MAX(ChLog.EventDate) AS LastCheck
  FROM CNEWebUICheckLog ChLog
  INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
  WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
  GROUP BY IOrgReq.OrganizationId
) AS RawCheckLimitDates
GROUP BY OrganizationId






INSERT INTO @Report
SELECT 
Org.[Полное наименование] AS [Полное наименование]
,ISNULL(Org.[Краткое наименование],'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Org.[Имя ФО] AS [Имя ФО]
,Org.[Код ФО] AS [Код ФО]
,Org.[Имя региона] AS [Имя региона]
,Org.[Код региона] AS [Код региона]
,Org.[Тип] AS [Тип]
,Org.[Вид] AS [Вид]
,Org.[ОПФ] AS [ОПФ]
,Org.[Филиал] AS [Филиал]
,Org.[Аккредитация по справочнику] AS [Аккредитация по справочнику]
,Org.[Свидетельство об аккредитации] AS [Свидетельство об аккредитации]
,Org.[Аккредитация по факту] AS [Аккредитация по факту]   
,Org.[ФИО руководителя] AS [ФИО руководителя]
,Org.[Должность руководителя] AS [Должность руководителя]
,Org.[Ведомственная принадлежность] AS [Ведомственная принадлежность]
,Org.[Фактический адрес] AS [Фактический адрес]
,Org.[Юридический адрес] AS [Юридический адрес]
,Org.[Код города] AS[Код города]
,Org.[Телефон] AS [Телефон]
,Org.[EMail] AS [EMail]
,Org.[ИНН] AS [ИНН]
,Org.[ОГРН] AS [ОГРН]

,CASE 
  WHEN (ISNULL(UsersCnt.UsersCount,0)=0)
  THEN 'Нет'
  ELSE 'Да'
  END AS [Подана заявка на регистрацию]
,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]

,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]

,LimitDates.FirstCheck AS [Первая проверка]  
,LimitDates.LastCheck AS [Последняя проверка] 

,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС]

FROM 
ReportOrgsBASE() Org 
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId
LEFT JOIN @WrongChecksByOrg WrongChecks
ON Org.Id=WrongChecks.OrganizationId
LEFT JOIN @CheckLimitDatesByOrg LimitDates
ON Org.Id=LimitDates.OrganizationId

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoByRegionTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Добавлены поля кода организации, учредителя
alter function [dbo].[ReportOrgsInfoByRegionTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL
  , @arg NVARCHAR(50) = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
,[Дата создания] datetime null
,[Создана из справочника] NVARCHAR(20) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] NVARCHAR(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Код учредителя] int null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
,[Количество проверок по номеру] int null
,[Количество уникальных проверок по номеру] INT NULL
,[Количество проверок по паспортным данным] INT NULL
,[Количество уникальных проверок по паспортным данным] INT NULL
,[Количество проверок по типографскому номеру] INT NULL
,[Количество уникальных проверок по типографскому номеру] INT NULL
,[Количество интерактивных проверок] INT NULL
,[Количество уникальных интерактивных проверок] INT NULL
,[Количество неправильных проверок] INT NULL
,[Первая проверка] datetime null
,[Последняя проверка] datetime null
,[Работа с ФБС] NVARCHAR(20)
,[Код ОУ] int null
,[Код головного ОУ] int null
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
  SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq   
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
  OrganizationId int,
  [Status] nvarchar(50),
  UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq   
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
  OrganizationId int,
  [activated] nvarchar(20),
  [deactivated] nvarchar(20),
  [consideration] nvarchar(20),
  [registration] nvarchar(20),
  [revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq   
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @NumberChecksByOrg TABLE
(
  OrganizationId INT,
  TotalNumberChecks INT,
  UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
  OrganizationId INT,
  TotalTNChecks INT,
  UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
  OrganizationId INT,
  TotalPassportChecks INT,
  UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @WrongChecksByOrg TABLE
(
  OrganizationId INT,
  WrongChecks INT
)

INSERT INTO @WrongChecksByOrg
SELECT IWrong.OrganizationId,SUM(IWrong.WrongChecks) FROM 
(
  SELECT IOrgReq.OrganizationId AS OrganizationId,COUNT(*) AS WrongChecks
  FROM CommonNationalExamCertificateRequestBatch cb 
  INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND c.SourceCertificateId IS NOT NULL
  AND IOrgReq.RegionId=@arg
  GROUP BY IOrgReq.OrganizationId
  UNION 
  SELECT IOrgReq.OrganizationId,COUNT(*) AS WrongChecks
  FROM CommonNationalExamCertificateCheckBatch cb 
  INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND c.SourceCertificateId IS NOT NULL
  AND IOrgReq.RegionId=@arg
  GROUP BY IOrgReq.OrganizationId
) AS IWrong
GROUP BY IWrong.OrganizationId





DECLARE @UIChecksByOrg TABLE
(
  OrganizationId INT,
  TotalUIChecks INT,
  UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
ISNULL(ChLog.PassportSeria,'')+
ISNULL(ChLog.PassportNumber,'')+
ISNULL(ChLog.CNENumber,'')+
ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId



DECLARE @CheckLimitDatesByOrg TABLE
(
  OrganizationId INT,
  FirstCheck DATETIME,
  LastCheck DATETIME
)
INSERT INTO @CheckLimitDatesByOrg
SELECT OrganizationId,MIN(FirstCheck),MAX(LastCheck)
FROM 
(
  SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
  FROM CommonNationalExamCertificateRequestBatch cb 
  INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND IOrgReq.RegionId=@arg
  GROUP BY IOrgReq.OrganizationId
  UNION 
  SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
  FROM CommonNationalExamCertificateCheckBatch cb 
  INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  WHERE IOrgReq.OrganizationId IS NOT NULL
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND IOrgReq.RegionId=@arg
  GROUP BY IOrgReq.OrganizationId
  UNION 
  SELECT IOrgReq.OrganizationId,MIN(ChLog.EventDate) AS FirstCheck,MAX(ChLog.EventDate) AS LastCheck
  FROM CNEWebUICheckLog ChLog
  INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
  WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
  AND IOrgReq.RegionId=@arg
  GROUP BY IOrgReq.OrganizationId
) AS RawCheckLimitDates
GROUP BY OrganizationId







INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,REPLACE(REPLACE(Org.WasImportedAtStart,1,'Да'),0,'Нет') AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
  WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
  THEN 'Нет'
  ELSE 'Есть'
  END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
  WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
  THEN 'Есть'
  ELSE 'Нет'
  END AS [Аккредитация по факту]  
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.DepartmentId AS [Код учредителя]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,Org.PhoneCityCode AS [Код города] 
,Org.Phone AS [Телефон] 
,Org.EMail AS [EMail]  

,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]

,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]

,LimitDates.FirstCheck AS [Первая проверка]  
,LimitDates.LastCheck AS [Последняя проверка] 

,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС],
Org.Id [Код ОУ],
Org.MainId [Код головного ОУ]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId
LEFT JOIN @WrongChecksByOrg WrongChecks
ON Org.Id=WrongChecks.OrganizationId
LEFT JOIN @CheckLimitDatesByOrg LimitDates
ON Org.Id=LimitDates.OrganizationId
WHERE Org.RegionId=@arg
ORDER BY
  case when Org.MainId is null then Org.Id else Org.MainId end, Org.MainId, Org.FullName


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportUserStatusAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [ ] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @Statuses TABLE
(
  [Name] NVARCHAR (50),
  Code NVARCHAR (50),
  [Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
  [Name] NVARCHAR (50),
  Code BIT,
  [Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
  OrgTypeName NVARCHAR (50),
  OrgTypeCode INT,
  IsPrivateName NVARCHAR (50),
  IsPrivateCode INT,
  IsPrivateOrder INT,
  StatusName NVARCHAR(50),
  StatusCode NVARCHAR(50),
  StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
WHERE OrganizationType2010.Id<3
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
  OrgTypeName NVARCHAR (50),
  OrgTypeCode NVARCHAR (50),
  IsPrivateName NVARCHAR (50),
  IsPrivateOrder NVARCHAR (50),
  StatusName NVARCHAR(50),
  UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
ON Acc.OrganizationId=OReq.Id
INNER JOIN dbo.Organization2010 Org 
ON (Org.Id=OReq.OrganizationId 
  AND (
    Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      )
    )
  )
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
  AND (
    OReq.TypeId=Comb.OrgTypeCode 
    AND (
      (OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
      OR
      (Comb.IsPrivateCode IS NULL)
    )
    AND Comb.OrgTypeCode!=10
  ))
OR (
  (Acc.Status=Comb.StatusCode)
  AND (
    Comb.OrgTypeCode=10
  )
  AND (
    OReq.TypeId IS NOT NULL
  )
)
OR (
  Comb.StatusCode='total'
  AND ((
    OReq.TypeId=Comb.OrgTypeCode 
    AND (
      (OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
      OR
      (Comb.IsPrivateCode IS NULL)
    )
    AND Comb.OrgTypeCode!=10
  )
  OR
  ((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
( 
  MainOrder INT,
  OrgTypeName NVARCHAR (50),
  IsPrivateName NVARCHAR (50),
  [Всего] INT,
  [Активирован] INT,
  [На регистрации] INT,
  [На доработке] INT,
  [На согласовании] INT,
  [Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
    case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
    case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
    case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
    case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
  INNER JOIN Organization2010 Org ON 
  (
    Org.Id=OReq.OrganizationId 
    AND (
      Org.IsAccredited=1 
      OR (
        Org.AccreditationSertificate != '' 
        AND Org.AccreditationSertificate IS NOT NULL
        )
      )
  )
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportUserStatusTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [ ] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @Statuses TABLE
(
  [Name] NVARCHAR (50),
  Code NVARCHAR (50),
  [Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
  [Name] NVARCHAR (50),
  Code BIT,
  [Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
  OrgTypeName NVARCHAR (50),
  OrgTypeCode INT,
  IsPrivateName NVARCHAR (50),
  IsPrivateCode INT,
  IsPrivateOrder INT,
  StatusName NVARCHAR(50),
  StatusCode NVARCHAR(50),
  StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
  OrgTypeName NVARCHAR (50),
  OrgTypeCode NVARCHAR (50),
  IsPrivateName NVARCHAR (50),
  IsPrivateOrder NVARCHAR (50),
  StatusName NVARCHAR(50),
  UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
ON Acc.OrganizationId=OReq.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
  AND (
    OReq.TypeId=Comb.OrgTypeCode 
    AND (
      (OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
      OR
      (Comb.IsPrivateCode IS NULL)
    )
    AND Comb.OrgTypeCode!=10
  ))
OR (
  (Acc.Status=Comb.StatusCode)
  AND (
    Comb.OrgTypeCode=10
  )
  AND (
    OReq.TypeId IS NOT NULL
  )
)
OR (
  Comb.StatusCode='total'
  AND ((
    OReq.TypeId=Comb.OrgTypeCode 
    AND (
      (OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
      OR
      (Comb.IsPrivateCode IS NULL)
    )
    AND Comb.OrgTypeCode!=10
  )
  OR
  ((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
( 
  MainOrder INT,
  OrgTypeName NVARCHAR (50),
  IsPrivateName NVARCHAR (50),
  [Всего] INT,
  [Активирован] INT,
  [На регистрации] INT,
  [На доработке] INT,
  [На согласовании] INT,
  [Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
    case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
    case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
    case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
    case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult


return
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
alter proc [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]
    @batchId BIGINT,
    @xml XML OUT
AS 
    SET nocount ON
    DECLARE @id BIGINT,
        @isCorrect BIT
    SELECT  @id = id,
            @isCorrect = IsCorrect
    FROM    dbo.CommonNationalExamCertificateCheckBatch WITH ( NOLOCK, FASTFIRSTROW )
    WHERE   outerId = @batchId
            AND IsProcess = 0
            AND Executing = 0
    SET @xml = ( SELECT ( SELECT    cnCheck.Id AS '@Id',
                                    @isCorrect AS '@IsBatchCorrect',
                                    cnCheck.BatchId AS '@BatchId',
                                    cnCheck.CertificateCheckingId AS '@CertificateCheckingId',
                                    cnCheck.CertificateNumber AS '@CertificateNumber',
                                    cnCheck.IsOriginal AS '@IsOriginal',
                                    cnCheck.IsCorrect AS '@IsCorrect',
                                    cnCheck.SourceCertificateIdGuid AS '@SourceCertificateIdGuid',
                                    cnCheck.IsDeny AS '@IsDeny',
                                    cnCheck.DenyComment AS '@DenyComment',
                                    cnCheck.DenyNewCertificateNumber AS '@DenyNewCertificateNumber',
                                    cnCheck.Year AS '@Year',
                                    cnCheck.TypographicNumber AS '@TypographicNumber',
                                    cnCheck.RegionId AS '@RegionId',
                                    cnCheck.PassportSeria AS '@PassportSeria',
                                    cnCheck.PassportNumber AS '@PassportNumber',
                                    ISNULL(unique_cheks.IdGuid,null) AS '@UniqueCheckIdGuid',
                  ISNULL(unique_cheks.UniqueIHEaFCheck,0) AS '@UniqueIHEaFCheck',
                  ISNULL(unique_cheks.[year],0) AS '@UniqueCheckYear',
                                    ( SELECT    b.Id AS 'subject/@Id',
                                                b.CheckId AS 'subject/@CheckId',
                                                b.SubjectId AS 'subject/@SubjectId',
                                                b.Mark AS 'subject/@Mark',
                                                b.IsCorrect AS 'subject/@IsCorrect',
                                                b.SourceCertificateSubjectIdGuid AS 'subject/@SourceCertificateSubjectIdGuid',
                                                b.SourceMark AS 'subject/@SourceMark',
                                                b.SourceHasAppeal AS 'subject/@SourceHasAppeal',
                                                b.Year AS 'subject/@Year'
                                      FROM      CommonNationalExamCertificateSubjectCheck b
                                      WHERE     --BatchId=@id and 
                                                b.CheckId = cnCheck.id
                                    FOR
                                      XML PATH(''),
                                          ROOT('subjects'),
                                          TYPE
                                    )
                          FROM      CommonNationalExamCertificateCheck cnCheck WITH ( NOLOCK)
              left outer join dbo.ExamCertificateUniqueChecks unique_cheks WITH ( NOLOCK)   on unique_cheks.IdGuid = cnCheck.SourceCertificateIdGuid 
                          WHERE     --cnCheck.id in(3201511,3201512)
                                    BatchId = @id
                        FOR
                          XML PATH('check'),
                              TYPE
                        )
               FOR
                 XML PATH('root'),
                     TYPE
               )
SELECT @xml
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ_Accred]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgActivation_VUZ_Accred]()
RETURNS @VUZ TABLE 
(
  [Вид] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @VUZState INT
DECLARE @VUZPriv INT

SELECT @VUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

SELECT @VUZPriv = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

UNION ALL
SELECT
'ВУЗ','Всего',@VUZState+@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgActivation_VUZ]()
RETURNS @VUZ TABLE 
(
  [Вид] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @VUZStateMain INT
DECLARE @VUZStateFilial INT
DECLARE @VUZPrivMain INT
DECLARE @VUZPrivFilial INT

SELECT @VUZStateMain = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=0 AND IsPrivate=0
SELECT @VUZStateFilial = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=1 AND IsPrivate=0
SELECT @VUZPrivMain = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=0 AND IsPrivate=1
SELECT @VUZPrivFilial = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=1 AND IsPrivate=1

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZStateFilial+@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
'ВУЗ основной','Государственный',@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
'ВУЗ филиал','Государственный',@VUZStateFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPrivFilial+@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ основной','Негосударственный',@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ филиал','Негосударственный',@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ','Всего',@VUZStateMain+@VUZStateFilial+@VUZPrivMain+@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ_Accred]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgActivation_SSUZ_Accred]()
RETURNS @VUZ TABLE 
(
  [Вид] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @SSUZState INT
SELECT @SSUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=2 AND  Org.IsPrivate=0
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010  Org WHERE Org.TypeId=2 AND  Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))
INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ССУЗ','Государственный',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

UNION ALL
SELECT
'ССУЗ','Негосударственный',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))
UNION ALL
SELECT
'ССУЗ','Всего',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgActivation_SSUZ]()
RETURNS @VUZ TABLE 
(
  [Вид] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @SSUZState INT
SELECT @SSUZState = COUNT(*) FROM Organization2010 WHERE TypeId=2 AND  IsPrivate=0
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010 WHERE TypeId=2 AND  IsPrivate=1
INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ССУЗ','Государственный',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=0

UNION ALL
SELECT
'ССУЗ','Негосударственный',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ССУЗ','Всего',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_OTHER]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportOrgActivation_OTHER]()
RETURNS @OTHER TABLE 
(
  [Вид] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @RCOI INT
DECLARE @OUO INT
DECLARE @OtherOrg INT
SELECT @RCOI = COUNT(*) FROM Organization2010 WHERE TypeId=3 
SELECT @OUO = COUNT(*) FROM Organization2010 WHERE TypeId=4 
SELECT @OtherOrg = COUNT(*) FROM Organization2010 WHERE TypeId=5

INSERT INTO @OTHER
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'РЦОИ','',@RCOI,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=3

UNION ALL
SELECT
'Орган управления образованием','',@OUO,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=4 
UNION ALL
SELECT
'Другое','',@OtherOrg,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId<>1 AND OrgReq.TypeId<>2 AND OrgReq.TypeId<>3 AND OrgReq.TypeId<>4 AND OrgReq.TypeId<>5

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportNotRegistredOrgsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportNotRegistredOrgsTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN

 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq   
WHERE IOrgReq.OrganizationId IS NOT NULL 
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id NOT IN 
  (SELECT OReq.OrganizationId 
  FROM OrganizationRequest2010 OReq
  WHERE OReq.OrganizationId  IS NOT NULL)


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsBASE]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportCheckedCNEsBASE](
  )
RETURNS @report TABLE 
(
CNEId BIGINT
,CNENumber NVARCHAR(255)
,OrgId INT
)
AS 
BEGIN
DECLARE @PreReport TABLE
(
  CNEId BIGINT
  ,CNENumber NVARCHAR(255)
  ,OrgId INT
)
INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateCheck c  ON c.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
INNER JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=0
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=1
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT CNE.Id AS CNEId,CNE.Number AS CNENumber,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CNEWebUICheckLog ChLog ON ChLog.FoundedCNEId=CNE.Id 
INNER JOIN Account Acc ON ChLog.AccountId=Acc.Id 
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @Report
SELECT DISTINCT * FROM @PreReport

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTotalChecksTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportTotalChecksTVF](
  @from datetime = null
  , @to datetime = null)
RETURNS @report TABLE 
(           
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
  select @from = dateadd(year, -1, getdate()), @to = getdate()

--Пакетные по паспорту
DECLARE @TotalByPassport_Batch INT
DECLARE @UniqueByPassport_Batch INT

SELECT @TotalByPassport_Batch=COUNT(*), @UniqueByPassport_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to


--Пакетные по номеру св-ва
DECLARE @TotalByNumber_Batch INT
DECLARE @UniqueByNumber_Batch INT

SELECT @TotalByNumber_Batch=COUNT(*), @UniqueByNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,c.SourceCertificateId))
FROM [CommonNationalExamCertificateCheckBatch] AS b WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateCheck] AS c WITH(NOLOCK)  ON c.batchid = b.id
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=b.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE b.updatedate BETWEEN @from and @to


--Пакетные по типографскому номеру
DECLARE @TotalByTypNumber_Batch INT
DECLARE @UniqueByTypNumber_Batch INT

SELECT @TotalByTypNumber_Batch=COUNT(*), @UniqueByTypNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to

--Итого по пакетным
DECLARE @Total_Batch INT
DECLARE @TotalUnique_Batch INT

SET @Total_Batch=
@TotalByPassport_Batch+
@TotalByNumber_Batch+
@TotalByTypNumber_Batch
SET @TotalUnique_Batch=
@UniqueByPassport_Batch+
@UniqueByNumber_Batch+
@UniqueByTypNumber_Batch

--Интерактивные по паспорту
DECLARE @TotalByPassport_UI INT
DECLARE @UniqueByPassport_UI INT

SELECT @TotalByPassport_UI = COUNT(*), @UniqueByPassport_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Passport'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='CNENumber'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Typographic'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Marks'
AND ChLog.EventDate BETWEEN @from and @to


--Итого по интерактивным
DECLARE @Total_UI INT
DECLARE @TotalUnique_UI INT

SET @Total_UI=
@TotalByPassport_UI+
@TotalByNumber_UI+
@TotalByTypNumber_UI+
@TotalByMarks_UI
SET @TotalUnique_UI=
@UniqueByPassport_UI+
@UniqueByNumber_UI+
@UniqueByTypNumber_UI+
@UniqueByMarks_UI


INSERT INTO @report
SELECT
'Пакетная',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch
UNION
SELECT 
'Интерактивная',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI

RETURN
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTopCheckingOrganizationsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportTopCheckingOrganizationsTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [№] nvarchar(10) null,
  [Организация (за период)] nvarchar(500) null,
  [Всего проверок (за период)] int null,
  [Организация (за все время)] nvarchar(500) null,
  [Всего проверок (за все время)] int null  
)
as 
begin

DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

insert into @report
select '', 'ТОП 20 организаций за ' + case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end, null, 'ТОП 20 организаций за все время',null
insert into @report
select c.rowid [№], c.Организация, c.[Всего проверок], t.Организация, t.[Всего проверок]
from 
(
  select 
  row_number() over (order by [Всего проверок] desc) as rowid
  ,*  
  from 
  (
  select top 20 
  o.FullName [Организация]
  ,(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateCheckBatch] cb with(nolock)
  join [CommonNationalExamCertificateCheck] c with(nolock) on cb.id = c.batchid 
  join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
  where cb.updatedate BETWEEN @periodBegin and @periodEnd)
  +
  (select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateRequestBatch] cb with(nolock)
  join [CommonNationalExamCertificateRequest] c with(nolock) on cb.id = c.batchid 
  join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
  where cb.updatedate BETWEEN @periodBegin and @periodEnd)
  [Всего проверок]
  from 
  OrganizationRequest2010 o with(nolock)
  order by [Всего проверок] desc
  ) c2
) c 
full join (
select  
  row_number() over (order by [Всего проверок] desc) as rowid
  ,*  
from 
  (
    select top 20 
    o.FullName [Организация]
    ,(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateCheckBatch] cb with(nolock)
    join [CommonNationalExamCertificateCheck] c  with(nolock) on cb.id = c.batchid 
    join [Account] a  with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
    )
    +
    (select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateRequestBatch] cb with(nolock)
    join [CommonNationalExamCertificateRequest] c with(nolock) on cb.id = c.batchid 
    join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id)
    [Всего проверок]
    from 
    OrganizationRequest2010 o with(nolock)
    order by [Всего проверок] desc
  )t2
) t on t.rowid = c.rowid
order by c.rowid asc


return
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportStatisticSubordinateOrg]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter funCTION  [dbo].[ReportStatisticSubordinateOrg](
      @periodBegin datetime,
      @periodEnd datetime,
      @departmentId int)
RETURNS @Report TABLE
(
  Id int null,
  FullName nvarchar(Max) null,
  RegionId int null,
  RegionName nvarchar(255) null,
  AccreditationSertificate nvarchar(255) null,
  DirectorFullName nvarchar(255) null,
  CountUser int null,
  UserUpdateDate datetime null,
  CountUniqueChecks int null
)
AS BEGIN

/*

SET @periodBegin = DATEADD(YEAR, -1, GETDATE())
SET @periodEnd = GETDATE()

--Проверки по номеру
DECLARE @NumberChecksByOrg TABLE
(
  OrganizationId INT,
  UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT 
  IOrgReq.OrganizationId,
  COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM 
  CommonNationalExamCertificateCheckBatch cb 
  INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
  INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
  O.DepartmentId = @departmentId
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY 
  IOrgReq.OrganizationId

--Проверки по типографскому номеру
DECLARE @TNChecksByOrg TABLE
(
  OrganizationId INT,
  UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT 
  IOrgReq.OrganizationId,
  COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM 
  CommonNationalExamCertificateRequestBatch cb 
  INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
  INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE 
  O.DepartmentId = @departmentId
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND cb.IsTypographicNumber = 1
GROUP BY 
  IOrgReq.OrganizationId


--Провекри по паспорту
DECLARE @PassportChecksByOrg TABLE
(
  OrganizationId INT,
  UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT 
  IOrgReq.OrganizationId,
  COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM 
  CommonNationalExamCertificateRequestBatch cb 
  INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
  INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
  INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
  O.DepartmentId = @departmentId
  AND cb.updatedate BETWEEN @periodBegin and @periodEnd
  AND cb.IsTypographicNumber = 0
GROUP BY
  IOrgReq.OrganizationId

--Проверки интерактивные
DECLARE @UIChecksByOrg TABLE
(
  OrganizationId INT,
  UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT 
  IOrgReq.OrganizationId,
  COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
    ISNULL(ChLog.PassportSeria,'')+
    ISNULL(ChLog.PassportNumber,'')+
    ISNULL(ChLog.CNENumber,'')+
    ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM
  CNEWebUICheckLog ChLog
  INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
  INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
  INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
  O.DepartmentId = @departmentId
  AND ChLog.EventDate BETWEEN @periodBegin and @periodEnd
GROUP BY
  IOrgReq.OrganizationId

INSERT INTO @Report
SELECT
  O.Id,
  O.FullName,
  O.RegionId,
  R.Name as RegionName,
  O.AccreditationSertificate,
  O.DirectorFullName,
  COUNT(A.Id) CountUser,
  MIN(A.UpdateDate) UserUpdateDate,
  isnull(sum(NCByOrg.UniqueNumberChecks) + 
    sum(TNByOrg.UniqueTNChecks) +
    sum(PByOrg.UniquePassportChecks) +
    sum(UIByOrg.UniqueUIChecks), 0) as CountUniqueChecks
from
  Organization2010 O
  INNER JOIN Region R on R.Id = O.RegionId
  LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
  LEFT JOIN Account A on A.OrganizationId = OrR.Id
  LEFT JOIN @NumberChecksByOrg NCByOrg ON NCByOrg.OrganizationId = O.Id
  LEFT JOIN @TNChecksByOrg TNByOrg ON TNByOrg.OrganizationId = O.Id
  LEFT JOIN @PassportChecksByOrg PByOrg ON PByOrg.OrganizationId = O.Id
  LEFT JOIN @UIChecksByOrg UIByOrg ON UIByOrg.OrganizationId = O.Id
where
  O.DepartmentId = @departmentId
group by
  O.Id,
  O.FullName,
  O.RegionId,
  R.Name,
  O.AccreditationSertificate,
  O.DirectorFullName

*/

-- Количество уникальных проверок
DECLARE @UniqueChecks TABLE
(
  OrganizationId INT,
  UniqueNumberChecks INT
)

INSERT INTO @UniqueChecks
SELECT 
  O.Id,
  COUNT(*) AS UniqueNumberChecks
FROM 
  Organization2010 O 
  inner join OrganizationRequest2010 ORR on ORR.OrganizationId = O.Id
  inner join OrganizationCertificateChecks OCC on OCC.OrganizationId = ORR.Id
WHERE
  O.DepartmentId = @departmentId
GROUP BY 
  O.Id



INSERT INTO @Report
select
  A.Id,
  A.FullName,
  A.RegionId,
  A.RegionName,
  A.AccreditationSertificate,
  A.DirectorFullName,
  A.CountUser,
  A.UserUpdateDate,
  isnull(UC.UniqueNumberChecks, 0) as CountUniqueChecks
from
  (
  select
    O.Id,
    O.FullName,
    O.RegionId,
    R.Name as RegionName,
    O.AccreditationSertificate,
    O.DirectorFullName,
    COUNT(A.Id) CountUser,
    MIN(A.UpdateDate) UserUpdateDate
  from
    Organization2010 O
    INNER JOIN Region R on R.Id = O.RegionId
    LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
    LEFT JOIN Account A on A.OrganizationId = OrR.Id
  where
    O.DepartmentId = @departmentId
  group by
    O.Id,
    O.FullName,
    O.RegionId,
    R.Name,
    O.AccreditationSertificate,
    O.DirectorFullName
  ) A 
  left join @UniqueChecks UC on UC.OrganizationId = A.Id


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistredOrgsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportRegistredOrgsTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN

 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq   
WHERE IOrgReq.OrganizationId IS NOT NULL 
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id IN 
  (SELECT OReq.OrganizationId 
  FROM OrganizationRequest2010 OReq
  WHERE OReq.OrganizationId  IS NOT NULL)

RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[SelectCNECCheckHystoryByOrgId]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[SelectCNECCheckHystoryByOrgId]
  @idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
  @LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
  declare @str nvarchar(max),@ss nvarchar(10),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max),@yearFrom int, @yearTo int
  set @pf=@pf-1

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
                ) as Marks, CNE.id CertificateId,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGuid        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = CNE.id
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end
            order by c.Id asc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') + ' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks, r.id CertificateId,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid = CAST(r.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = r.id 
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id asc       
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
                 c.id CertificateId
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.id as nvarchar(255))
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID  
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                              
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join prn.CancelledCertificates certificate_deny 
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = t.CertificateId        
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
                ) as Marks, CNE.id CertificateId,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = CNE.id
            where @idorg=Acc.OrganizationId     
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end           
            order by c.Id desc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks, r.id CertificateId,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=CAST(r.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed ON c.SourceCertificateYear = ed.[Year] 
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = r.id
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id desc        
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
                 c.id CertificateId
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255)) 
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join prn.CancelledCertificates certificate_deny 
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = t.CertificateId
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
                ) as Marks, CNE.id CertificateId,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = CNE.id
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.Id asc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
              null as Marks, r.id CertificateId,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=CAST(r.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = r.id
            where @idorg=Acc.OrganizationId     
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end           
            order by c.Id asc       
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
                 c.id CertificateId
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255))   
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID                
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join prn.CancelledCertificates certificate_deny 
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = t.CertificateId
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
                ) as Marks, CNE.id CertificateId,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = CNE.id
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id desc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks, r.id CertificateId,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=CAST(r.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = r.id
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.Id desc        
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks,
                 c.id CertificateId
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255))   
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID                
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join prn.CancelledCertificates certificate_deny 
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = t.CertificateId
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
                ) as Marks, CNE.id CertificateId,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = CNE.id
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.LastName asc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks, r.id CertificateId,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=CAST(r.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = r.id
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.LastName asc       
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+' '+c.PassportNumber, cb.PassportSeria +' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks,
                 c.id CertificateId
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255))   
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID              
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join prn.CancelledCertificates certificate_deny 
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = t.CertificateId
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
                ) as Marks, CNE.id CertificateId,
               case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            FROM CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
              left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID        
              left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = CNE.id
            where @idorg=Acc.OrganizationId   
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end             
            order by c.LastName desc
            union all
            select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
              ISNULL(c.PassportSeria,'') +' '+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
              null as Marks, r.id CertificateId,
              case WHEN c.SourceCertificateIdGuid IS NULL THEN 'Не найдено' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 'Действительно'
                else 'Истек срок' end end  STATUS
            FROM CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
              left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=CAST(r.Id as nvarchar(100))
              left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID  
              JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
              left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
              left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = r.id
            where @idorg=Acc.OrganizationId 
              and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
              and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end               
            order by c.LastName desc        
            union all
            select t.*,
              case when ed.[ExpireDate] is null then 'Не найдено'  
                when certificate_deny.UseYear is not null then 'Аннулировано' 
                when getdate() <= ed.[ExpireDate] then 'Действительно'
              else 'Истек срок' end Status
            from 
            (
              select cb.id,cb.EventDate,'Интерактивная' TypeCheck,ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+ ' '+c.PassportNumber, cb.PassportSeria + ' ' + cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks,
                 c.id CertificateId
              from 
              #t1 cb1
                join CNEWebUICheckLog cb on cb1.id=cb.id
                left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255))   
                left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID                
              where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
                and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end                  
            ) t
              left join [ExpireDate] as ed on ed.[Year] = t.[Year]
              left join prn.CancelledCertificates certificate_deny 
                on certificate_deny.[UseYear] between @yearFrom and @yearTo
                  and certificate_deny.CertificateFK = t.CertificateId
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
                ) as Marks, CNE.id CertificateId,
             case when ed.[ExpireDate] is null then ''Не найдено''  
              when certificate_deny.UseYear is not null then ''Аннулировано'' 
              when getdate() <= ed.[ExpireDate] then ''Действительно''
            else ''Истек срок'' end Status
          FROM 
            (select min(c.id) id,c.CertificateNumber 
             from CommonNationalExamCertificateCheck c
              JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id '
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
              where c.LastName like ''%''+@LastName+''%'' '     
    set @str=@str+'               
            group by CertificateNumber) cb1 
                              
            join CommonNationalExamCertificateCheck c on cb1.id=c.id                              
            JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id                
            left join vw_Examcertificate CNE on c.SourceCertificateIdGuid=CAST(CNE.Id as nvarchar(100))     
            left join ExamcertificateUniqueChecks CC on CNE.id=cc.idGUID
            left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
            left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
              on certificate_deny.[UseYear] between @yearFrom and @yearTo
                and certificate_deny.CertificateFK = CNE.id
          union all
          select c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
            ISNULL(c.PassportSeria,'''') +'' ''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
            null as Marks, r.id CertificateId,
            case WHEN c.SourceCertificateIdGuid IS NULL THEN ''Не найдено'' else
                case when isnull(c.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then ''Действительно''
                else ''Истек срок'' end end  STATUS
          FROM 
            (select min(c.id) id, r.LicenseNumber
            from CommonNationalExamCertificateRequest c 
              JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
              left JOIN prn.Certificates r ON c.SourceCertificateIdGuid=CAST(r.CertificateID as nvarchar(100)) '
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
            left JOIN vw_Examcertificate r ON c.SourceCertificateIdGuid=CAST(r.Id as nvarchar(100))
            left join ExamcertificateUniqueChecks CC on r.id=cc.idGUID        
            left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
            left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
              on certificate_deny.[UseYear] between @yearFrom and @yearTo
                and certificate_deny.CertificateFK = r.id '
    set @str=@str+'   
                  
          union all
          select t.*,
            case when ed.[ExpireDate] is null then ''Не найдено''  
              when certificate_deny.UseYear is not null then ''Аннулировано'' 
              when getdate() <= ed.[ExpireDate] then ''Действительно''
            else ''Истек срок'' end Status
          from 
          (
            select cb.id,cb.EventDate,''Интерактивная'' TypeCheck, ISNULL(c.Number, cb.CNENumber) CertificateNumber, ISNULL(c.TypographicNumber, cb.TypographicNumber) TypographicNumber ,
                ISNULL(c.LastName, cb.LastName) LastName, ISNULL(c.FirstName, cb.FirstName) FirstName, ISNULL(c.PatronymicName, cb.PatronymicName) PatronymicName,
                 ISNULL(c.PassportSeria+'' ''+c.PassportNumber, cb.PassportSeria +'' ''+ cb.PassportNumber) PassportData, c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, cb.Marks as Marks,
                 c.id CertificateId
            from 
            (
              select max(cb.id) id,c.Number from
              (
                select cb.id,cb.FoundedCNEId
                from CNEWebUICheckLog cb  with(index(I_CNEWebUICheckLog_AccId))
                  join Account Acc with(index(accOrgIdIndex)) on acc.id=cb.AccountId  
                  join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 ' 
    if @TypeCheck= 'Интерактивная' or @TypeCheck is null
        set @str=@str+' '         
    else
        set @str=@str+'           
              and 1=0 '                   
    if @idorg<>-1 
      set @str=@str+'
                where @idorg=Acc.OrganizationId '               
    set @str=@str+'   
              ) cb
              left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.id as nvarchar(255))             
              group by c.Number 
            ) cb1
              join CNEWebUICheckLog cb on cb1.id=cb.id
              left join vw_Examcertificate c ON cb.FoundedCNEId=cast(c.Id as nvarchar(255)) '               
    set @str=@str+'               
              left join ExamcertificateUniqueChecks CC on c.id=cc.idGUID '
    if @LastName is not null
      set @str=@str+'
              where c.LastName like ''%''+@LastName+''%'''
    set @str=@str+'             
                  
          ) t
            left join [ExpireDate] as ed on ed.[Year] = t.[Year]
            left join prn.CancelledCertificates certificate_deny 
              on certificate_deny.[UseYear] between @yearFrom and @yearTo
                and certificate_deny.CertificateFK = t.CertificateId
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
    
    drop table #ttt '
    
  end
  
  --print  @str
  
  exec sp_executesql @str,N'@idorg int,@ps int,@pf int,@LastName nvarchar(255)',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName
  
  drop table #tt
  drop table #t1  
end
GO
/****** Object:  StoredProcedure [dbo].[usp_cne_AddCheckBatchResult]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[usp_cne_AddCheckBatchResult]
    @xml XML = NULL,
    @batchid BIGINT = 0
AS 
    SET nocount ON  
    IF @xml IS NULL 
        BEGIN
            SELECT  NULL IsProcess1,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END

    SELECT  item.ref.value('@Id', 'bigint') AS Id,      
            item.ref.value('@IsBatchCorrect', 'bit') AS IsBatchCorrect,
            item.ref.value('@BatchId', 'bigint') AS BatchId,
            item.ref.value('@CertificateCheckingId', 'uniqueidentifier') AS CertificateCheckingId,
            item.ref.value('@CertificateNumber', 'nvarchar(255)') AS CertificateNumber,
            item.ref.value('@IsOriginal', 'bit') AS IsOriginal,
            item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
            item.ref.value('@SourceCertificateId', 'uniqueidentifier') AS SourceCertificateId,
            item.ref.value('@IsDeny', 'bit') AS IsDeny,
            item.ref.value('@DenyComment', 'nvarchar(max)') AS DenyComment,
            item.ref.value('@DenyNewCertificateNumber', 'nvarchar(255)') AS DenyNewCertificateNumber,
            item.ref.value('@Year', 'int') AS Year,
            item.ref.value('@TypographicNumber', 'nvarchar(255)') AS TypographicNumber,
            item.ref.value('@RegionId', 'int') AS RegionId,
            item.ref.value('@PassportSeria', 'nvarchar(255)') AS PassportSeria,
            item.ref.value('@PassportNumber', 'nvarchar(255)') AS PassportNumber,
            item.ref.value('@UniqueCheckIdGuid', 'uniqueidentifier') AS UniqueCheckId,
      item.ref.value('@UniqueIHEaFCheck', 'int') AS UniqueIHEaFCheck,
      item.ref.value('@UniqueCheckYear', 'int') AS UniqueCheckYear
    INTO    #check
    FROM    ( SELECT    @xml
            ) feeds ( feedXml )
            CROSS APPLY feedXml.nodes('/root/check') AS item ( ref )
            SELECT * FROM #check

    IF NOT EXISTS ( SELECT  *
                    FROM    #check ) 
        BEGIN
            SELECT  NULL IsProcess2,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END


--select * from #check
    DECLARE @id BIGINT, @checkid BIGINT, @isCorrect BIT
    SELECT  @batchid = dbo.GetInternalId(@batchid)
    IF EXISTS ( SELECT  *
                FROM    CommonNationalExamCertificateCheck
                WHERE   BatchId = @batchid ) 
        BEGIN
            SELECT  NULL IsProcess4,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END
--select @batchid
    BEGIN TRY
    BEGIN TRAN
    -- обновление статистики для уже проверенных св-в
      update    ExamCertificateUniqueChecks
      set       UniqueIHEaFCheck = #check.UniqueIHEaFCheck
      from      ExamCertificateUniqueChecks ex
          inner join #check on #check.UniqueCheckIdGuid = ex.IdGuid
                     AND #check.[UniqueCheckYear] = ex.[year]
          WHERE #check.UniqueCheckId > 0
      -- добавление статистики для еще не проверенных св-в
      insert    ExamCertificateUniqueChecks
          (
            IdGuid,
            [year],
            UniqueIHEaFCheck
          )

          select  DISTINCT #check.UniqueCheckIdGuid,
                    #check.UniqueCheckYear,
                        #check.UniqueIHEaFCheck
          from    #check
              left join ExamCertificateUniqueChecks ex on ex.IdGuid = #check.UniqueCheckIdGuid
                                    AND #check.[UniqueCheckYear] = ex.[year]
          where   ex.Id is NULL AND #check.UniqueCheckIdGuid is not null

INSERT  CommonNationalExamCertificateCheck
                        (
                          BatchId,
                          CertificateCheckingId,
                          CertificateNumber,
                          IsOriginal,
                          IsCorrect,
                          SourceCertificateIdGuid,
                          IsDeny,
                          DenyComment,
                          DenyNewCertificateNumber,
                          Year,
                          TypographicNumber,
                          RegionId,
                          PassportSeria,
                          PassportNumber,
                          idtemp
                        )                        
                        SELECT  @batchid,
                                CertificateCheckingId,
                                CertificateNumber,
                                IsOriginal,
                                IsCorrect,
                                SourceCertificateId,
                                IsDeny,
                                DenyComment,
                                DenyNewCertificateNumber,
                                Year,
                                TypographicNumber,
                                RegionId,
                                PassportSeria,
                                PassportNumber,
                                a.id
                        FROM    #check a
                    
INSERT  CommonNationalExamCertificateSubjectCheck
                        (
                          BatchId,
                          CheckId,
                          SubjectId,
                          Mark,
                          IsCorrect,
                          SourceCertificateSubjectIdGuid,
                          SourceMark,
                          SourceHasAppeal,
                          Year
                        )
                        SELECT  @batchid,
                                b.id,
                                a.SubjectId,
                                a.Mark,
                                a.IsCorrect,
                                a.SourceCertificateSubjectIdGuid,
                                a.SourceMark,
                                a.SourceHasAppeal,
                                a.[Year]
                        FROM    
                        
                        (
                            SELECT  item.ref.value('@Id', 'bigint') AS Id,
                  item.ref.value('@CheckId', 'bigint') AS CheckId,
                  item.ref.value('@SubjectId', 'bigint') AS SubjectId,
                  item.ref.value('@Mark', 'numeric(5,1)') AS Mark,
                  item.ref.value('@IsCorrect', 'bit') AS IsCorrect,
                  item.ref.value('@SourceCertificateSubjectIdGuid', 'uniqueidentifier') AS SourceCertificateSubjectIdGuid,
                  item.ref.value('@SourceMark', 'numeric(5,1)') AS SourceMark,
                  item.ref.value('@SourceHasAppeal', 'bigint') AS SourceHasAppeal,
                  item.ref.value('@Year', 'int') AS [Year]
                FROM    ( SELECT    @xml)      
                feeds ( feedXml )
                            CROSS APPLY feedXml.nodes('/root/check/subjects/subject') AS item ( ref )
                        ) a join CommonNationalExamCertificateCheck b on a.CheckId=b.idtemp and b.BatchId=@batchid
                       
 
        SELECT TOP 1
                @isCorrect = IsBatchCorrect
        FROM    #check
        UPDATE  CommonNationalExamCertificateCheckBatch
        SET     IsProcess = 0,
                Executing = 0,
                IsCorrect = @isCorrect
        WHERE   id = @batchid
        
--select top 10 * from CommonNationalExamCertificateCheckBatch where id=@batchid
--select top 10 * from CommonNationalExamCertificateCheck where BatchId=@batchid  order by id desc
--select top 10 * from CommonNationalExamCertificateSubjectCheck where BatchId=@batchid order by id desc
        SELECT  IsProcess,
                ( SELECT    COUNT(*)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Total,
                ( SELECT    COUNT(SourceCertificateId)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Found
        FROM    CommonNationalExamCertificateCheckBatch b
        WHERE   IsProcess = 0
                AND id = @batchid
                
        IF @@trancount > 0 
            COMMIT TRAN

    END TRY
    BEGIN CATCH
        IF @@trancount > 0 
            ROLLBACK
        DECLARE @msg NVARCHAR(4000)
        SET @msg = ERROR_MESSAGE()
        RAISERROR ( @msg, 16, 1 )
        RETURN -1
    END CATCH
GO
/****** Object:  StoredProcedure [dbo].[RegisterEvent]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Добавление записи в таблицу EventLog.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлено поле UpdateId.
-- v.1.2: Modified by Makarev Andrey 18.04.2008
-- Регистрация событий по нескольким SourceEntityId.
-- v.1.3: Modified by Makarev Andrey 30.04.2008
-- Правильная работа с пустым @sourceEntityIds.
-- =============================================
alter proc [dbo].[RegisterEvent]
  @accountId bigint
  , @ip nvarchar(255)
  , @eventCode nvarchar(100)
  , @sourceEntityIds nvarchar(4000)
  , @eventParams ntext
  , @updateId uniqueidentifier = null
as
BEGIN
IF (ISNULL(@sourceEntityIds,'') = '' )
insert dbo.EventLog
    (
    date
    , accountId
    , ip
    , eventCode
    , sourceEntityId
    , eventParams
    , UpdateId
    )
    VALUES (GETDATE(), @accountId , @ip , @eventCode , null, @eventParams , @updateId) 
ELSE        
  insert dbo.EventLog
    (
    date
    , accountId
    , ip
    , eventCode
    , sourceEntityId
    , eventParams
    , UpdateId
    )
  select
    GetDate()
    , @accountId
    , @ip
    , @eventCode
    , ids.value
    , @eventParams
    , @updateId
  from
    dbo.GetDelimitedValues(@sourceEntityIds) ids

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetOrganizationTypeReport]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[GetOrganizationTypeReport]
as 
begin
  SELECT 
  OrgType.Id,
  OrgType.[Name] AS TypeName,
  REPLACE(REPLACE(ISNULL(IsPrivate,''),'1','Негосударственный'),'0','Государственный') AS OPF,
  ISNULL(UsersCount ,0) AS UsersCount
  FROM
    (SELECT OrgReq.TypeId AS TypeId,CONVERT(NVARCHAR(5),OrgReq.IsPrivate) AS IsPrivate, COUNT(Acc.Id) AS UsersCount
    FROM dbo.Account Acc
    INNER JOIN dbo.OrganizationRequest2010 OrgReq
    ON Acc.OrganizationId=OrgReq.Id
    GROUP BY OrgReq.TypeId,OrgReq.IsPrivate
    ) Rt
  RIGHT JOIN dbo.OrganizationType2010 OrgType
  ON OrgType.Id=TypeId
  UNION
  SELECT 6,'Итого','',COUNT(*) 
  FROM dbo.Account Acc 
  INNER JOIN dbo.OrganizationRequest2010 OrgReq
  ON Acc.OrganizationId=OrgReq.Id
  ORDER BY OrgType.Id
  --  declare
--    @year int
--
--  set @year = Year(GetDate())
--
--  select
--    [type].Name AS TypeName
--    , report.[Count]
--    , 0 IsSummary
--    , 0 IsTotal
--  from (select 
--      [type].Id OrganizationTypeId
--      , count(*) [Count]
--    from dbo.Account account
--      inner join dbo.OrganizationRequest2010 OrgReq
--        inner join dbo.OrganizationType2010 [type]
--          on [type].Id = OrgReq.TypeId
--        on OrgReq.Id = account.OrganizationId
--    where
--      account.Id in (select 
--            group_account.AccountId
--          from dbo.GroupAccount group_account
--            inner join dbo.[Group] [group]
--              on [group].Id = group_account.GroupId
--          where [group].Code = 'User')
--      and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
--          , account.RegistrationDocument) = 'activated'
--    group by
--      [type].Id
--    with cube) report
--      left outer join dbo.OrganizationType2010 [type]
--        on [type].Id = report.OrganizationTypeId
--  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksByPeriodTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportChecksByPeriodTVF](
  @from datetime = null
  , @to datetime = null)
RETURNS @report TABLE 
(           
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
,[order] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
  select @from = dateadd(year, -1, getdate()), @to = getdate()

--Пакетные по паспорту
DECLARE @TotalByPassport_Batch INT
DECLARE @UniqueByPassport_Batch INT

SELECT @TotalByPassport_Batch=COUNT(*), @UniqueByPassport_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to


--Пакетные по номеру св-ва
DECLARE @TotalByNumber_Batch INT
DECLARE @UniqueByNumber_Batch INT

SELECT @TotalByNumber_Batch=COUNT(*), @UniqueByNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,c.SourceCertificateId))
FROM [CommonNationalExamCertificateCheckBatch] AS b WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateCheck] AS c WITH(NOLOCK)  ON c.batchid = b.id
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=b.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE b.updatedate BETWEEN @from and @to


--Пакетные по типографскому номеру
DECLARE @TotalByTypNumber_Batch INT
DECLARE @UniqueByTypNumber_Batch INT

SELECT @TotalByTypNumber_Batch=COUNT(*), @UniqueByTypNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to

--Итого по пакетным
DECLARE @Total_Batch INT
DECLARE @TotalUnique_Batch INT

SET @Total_Batch=
@TotalByPassport_Batch+
@TotalByNumber_Batch+
@TotalByTypNumber_Batch
SET @TotalUnique_Batch=
@UniqueByPassport_Batch+
@UniqueByNumber_Batch+
@UniqueByTypNumber_Batch

--Интерактивные по паспорту
DECLARE @TotalByPassport_UI INT
DECLARE @UniqueByPassport_UI INT

SELECT @TotalByPassport_UI = COUNT(*), @UniqueByPassport_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Passport'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='CNENumber'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Typographic'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Marks'
AND ChLog.EventDate BETWEEN @from and @to


--Итого по интерактивным
DECLARE @Total_UI INT
DECLARE @TotalUnique_UI INT

SET @Total_UI=
@TotalByPassport_UI+
@TotalByNumber_UI+
@TotalByTypNumber_UI+
@TotalByMarks_UI
SET @TotalUnique_UI=
@UniqueByPassport_UI+
@UniqueByNumber_UI+
@UniqueByTypNumber_UI+
@UniqueByMarks_UI


INSERT INTO @report
SELECT
'Пакетная',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch,1
UNION
SELECT 
'Интерактивная',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI,2
UNION
SELECT 
'Итого',@Total_Batch+@Total_UI,@UniqueByNumber_Batch+@UniqueByNumber_UI,@UniqueByTypNumber_Batch+@UniqueByTypNumber_UI,@UniqueByPassport_Batch+@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_Batch+@TotalUnique_UI,3

RETURN
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksByOrgsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportChecksByOrgsTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Имя региона] NVARCHAR(255) null
,[Полное наименование] NVARCHAR(4000) NULL
,[Тип] NVARCHAR(255) null
,[ОПФ] NVARCHAR(50) null
,[Количество проверок] INT null
,[Количество уникальных проверок] INT NULL
,[Работа с ФБС] NVARCHAR(20) NULL
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
  SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 

DECLARE @NumberChecksByOrg TABLE
(
  OrganizationId INT,
  TotalNumberChecks INT,
  UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
  OrganizationId INT,
  TotalTNChecks INT,
  UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
  OrganizationId INT,
  TotalPassportChecks INT,
  UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
GROUP BY IOrgReq.OrganizationId




DECLARE @UIChecksByOrg TABLE
(
  OrganizationId INT,
  TotalUIChecks INT,
  UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ChLog.FoundedCNEId) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd AND ChLog.FoundedCNEId is not NULL
GROUP BY IOrgReq.OrganizationId








INSERT INTO @Report
SELECT 
Reg.[Name] AS [Имя региона]
,Org.FullName AS [Полное наименование]
,OrgType.[Name] AS [Тип]
,REPLACE(REPLACE(Org.IsPrivate,1,'Негосударственный'),0,'Государственный') AS [ОПФ]

,ISNULL(NumberChecks.TotalNumberChecks,0)
+ISNULL(PassportChecks.TotalPassportChecks,0)
+ISNULL(TNChecks.TotalTNChecks,0)
+ISNULL(UIChecks.TotalUIChecks,0) AS [Количество проверок]  
,ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных проверок] 
,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС]


FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId


LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId

ORDER BY Reg.Id,Org.TypeId,Org.IsPrivate

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksAllTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportChecksAllTVF]()
RETURNS @report TABLE 
(           
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
,[order] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам

--Пакетные по паспорту
DECLARE @TotalByPassport_Batch INT
DECLARE @UniqueByPassport_Batch INT

SELECT @TotalByPassport_Batch=COUNT(*), @UniqueByPassport_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL


--Пакетные по номеру св-ва
DECLARE @TotalByNumber_Batch INT
DECLARE @UniqueByNumber_Batch INT

SELECT @TotalByNumber_Batch=COUNT(*), @UniqueByNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,c.SourceCertificateId))
FROM [CommonNationalExamCertificateCheckBatch] AS b WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateCheck] AS c WITH(NOLOCK)  ON c.batchid = b.id
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=b.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL


--Пакетные по типографскому номеру
DECLARE @TotalByTypNumber_Batch INT
DECLARE @UniqueByTypNumber_Batch INT

SELECT @TotalByTypNumber_Batch=COUNT(*), @UniqueByTypNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

--Итого по пакетным
DECLARE @Total_Batch INT
DECLARE @TotalUnique_Batch INT

SET @Total_Batch=
@TotalByPassport_Batch+
@TotalByNumber_Batch+
@TotalByTypNumber_Batch
SET @TotalUnique_Batch=
@UniqueByPassport_Batch+
@UniqueByNumber_Batch+
@UniqueByTypNumber_Batch

--Интерактивные по паспорту
DECLARE @TotalByPassport_UI INT
DECLARE @UniqueByPassport_UI INT

SELECT @TotalByPassport_UI = COUNT(*), @UniqueByPassport_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Passport'


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='CNENumber'


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Typographic'


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Marks'


--Итого по интерактивным
DECLARE @Total_UI INT
DECLARE @TotalUnique_UI INT

SET @Total_UI=
@TotalByPassport_UI+
@TotalByNumber_UI+
@TotalByTypNumber_UI+
@TotalByMarks_UI
SET @TotalUnique_UI=
@UniqueByPassport_UI+
@UniqueByNumber_UI+
@UniqueByTypNumber_UI+
@UniqueByMarks_UI


INSERT INTO @report
SELECT
'Пакетная',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch,8
UNION
SELECT 
'Интерактивная',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI,9
UNION
SELECT 
'Итого',@Total_Batch+@Total_UI,@UniqueByNumber_Batch+@UniqueByNumber_UI,@UniqueByTypNumber_Batch+@UniqueByTypNumber_UI,@UniqueByPassport_Batch+@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_Batch+@TotalUnique_UI,10

RETURN
end
GO
/****** Object:  StoredProcedure [dbo].[CalculateUniqueChecksByBatchId]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[CalculateUniqueChecksByBatchId]
  @batchId bigint,
    @checkType varchar(200)
as
begin
  -- =========================================================================
  -- Передается код пакета и по этому коду определяется какие проверки 
  -- были выполнены в рамках этого пакета, какие свидетельства были найдены
  -- По успешно найденным свидетельствам выполняется перерасчет количества
  -- уникальных проверок
  -- =========================================================================

  -- Описание входных параметров
    --
  -- @batchId - код пакета. По хорошему надо сделать его GUID и тогда проверять можно 
    --            без указатия  указания типа проверки. А пока он bigint то код пакетов
    --            по разным типам проверок могут пересекаться
    -- @checkType - тип проверки: 'passport_or_typo' - по паспорту или типографскому номеру,
    --              'certificate' - по номеру сертификата
    
    -- 1. Объявляем 'константы'
    declare @passport_or_typo varchar(100)
    set @passport_or_typo = 'passport_or_typo'
      
    declare @certificate varchar(100)
    set @certificate = 'certificate'

  -- 2. Объявляем переменные
    declare @organizationId bigint
    set @organizationId = 0
    
    -- Используется в курсоре. В эту переменную пишется код свидетельства
    declare @CIdGuid uniqueidentifier 
    set @CIdGuid = null

  -- 3. По коду пакета определяем организацию, от которой выполнялась пакетная проверка        
    -- 
  -- Для начала надо понять, от имени какой организации выполнялась пакетная проверка, т.к.
    -- количество уникальных проверок - это сколько организаций проверило данное свидетельство.
    -- Существует три вида проверок. Информация о двух из них (по паспорту и типографскому 
    -- номеру) хранится в таблице CommonNationalExamCertificateRequestBatch, информация по 
    -- оставшейся (по номеру свидетельства) хранится в таблице 
    -- CommonNationalExamCertificateCheckBatch.
    --
    -- 3.1. По паспорту или типографскому номеру
    if (@checkType = @passport_or_typo)
    begin
        set @organizationId = 
            ISNULL(
                (select 
                    top 1 
                    A.OrganizationId 
                from 
                    Account A 
                where A.Id = 
                    (select 
                        top 1 ERB.OwnerAccountId 
                    from 
                        CommonNationalExamCertificateRequestBatch ERB 
                    where 
                        ERB.Id = @batchId))
                , 0
          )
  end
        
    -- 3.2. По номеру свидетельства
    if (@checkType = @certificate)
    begin
        set @organizationId = 
            ISNULL(
                (select 
                    top 1 
                    A.OrganizationId 
                from 
                    Account A 
                where A.Id = 
                    (select 
                        top 1 ECB.OwnerAccountId 
                    from 
                        CommonNationalExamCertificateCheckBatch ECB 
                    where 
                        ECB.Id = @batchId))
                , 0
          )
  end
    
    -- 3.3. Организацию не нашли (или тип поиска был задан неверно) то выходим с кодом 0
    if (@organizationId = 0)
      return 0
        
    
    -- 4. Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    
    -- 4.1. Выполняем действия для проверок по паспорту или типографскому номеру
    if (@checkType = @passport_or_typo)
    begin              
      -- Отбираем все найденные свидетельства
      declare db_cursor cursor for
      select
          distinct S.SourceCertificateIdGuid
      from 
          CommonNationalExamCertificateRequest S
      where
          isnull(S.SourceCertificateIdGuid,'Нет свидетельства') <> 'Нет свидетельства'
            and S.BatchId = @batchId
    
        -- Для каждого найденного свидетельства вызываем хранимую процедуру подсчета проверок
      open db_cursor   
      fetch next from db_cursor INTO @CIdGuid   
      while @@FETCH_STATUS = 0   
      begin
          exec dbo.ExecuteChecksCount
              @OrganizationId = @organizationId,
              @CertificateIdGuid = @CIdGuid
          fetch next from db_cursor into @CIdGuid
      end
        
      close db_cursor   
      deallocate db_cursor
    end
    
    -- 4.2. Выполняем действия для проверок по номеру свидетельства
    if (@checkType = @certificate)
    begin
      -- Отбираем все найденные свидетельства
      declare db_cursor cursor for
      select
          distinct S.SourceCertificateIdGuid
      from 
          CommonNationalExamCertificateCheck S
      where
          S.SourceCertificateIdGuid is not null
            and S.BatchId = @batchId
    
        -- Для каждого найденного свидетельства вызываем хранимую процедуру подсчета проверок
      open db_cursor   
      fetch next from db_cursor INTO @CIdGuid   
      while @@FETCH_STATUS = 0   
      begin
          exec dbo.ExecuteChecksCount
              @OrganizationId = @organizationId,
              @CertificateIdGuid = @CIdGuid
          fetch next from db_cursor into @CIdGuid
      end
        
      close db_cursor   
      deallocate db_cursor
    end

  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[ClearDataBase]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Приводит БД в состояние на момент начала эксплуатации системы
-- путем очистки таблиц
-- Очищаются только таблицы, связанные с проверками свидетельств
-- Создана: Юсупов Кирилл, 10.06.2010
-- ================================================
alter procedure [dbo].[ClearDataBase]
as
begin
  delete from dbo.EventLog where EventCode like 'CNE_%'
  delete from dbo.CNEWebUICheckLog
  delete from dbo.CheckCommonNationalExamCertificateLog
  delete from dbo.CommonNationalExamCertificateCheckLog

  delete from dbo.CommonNationalExamCertificateDenyLoadingTask
  delete from dbo.CommonNationalExamCertificateDenyLoadingTaskError
  delete from dbo.CommonNationalExamCertificateLoadingTask
  delete from dbo.CommonNationalExamCertificateLoadingTaskError

  delete from dbo.CommonNationalExamCertificateSubject
  delete from dbo.CommonNationalExamCertificate
    delete from dbo.CommonNationalExamCertificateDeny
  
  delete from dbo.ImportingCommonNationalExamCertificateSubject
  delete from dbo.ImportingCommonNationalExamCertificate

  delete from dbo.CommonNationalExamCertificateCheck
  delete from dbo.CommonNationalExamCertificateCheckBatch
  delete from dbo.CommonNationalExamCertificateRequest
  delete from dbo.CommonNationalExamCertificateRequestBatch

  delete from dbo.CommonNationalExamCertificateForm
  delete from dbo.CommonNationalExamCertificateFormNumberRange
  
  delete from dbo.CommonNationalExamCertificateSubjectCheck
  delete from dbo.CommonNationalExamCertificateSubjectForm

  delete from dbo.DeprecatedCommonNationalExamCertificateSubject
  delete from dbo.DeprecatedCommonNationalExamCertificate
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountByRegionReport]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetUserAccountByRegionReport
-- ========================================================
-- Отчет пользователей по регионам.
-- ========================================================
alter procedure [dbo].[GetUserAccountByRegionReport]
as 
begin

  ;with RegionUserCountCTE as
  (select 
    isnull(r.Code, '') RegionCode
    , isnull(r.Name, 'Не указано') RegionName
    , count(*) [Count]
  from dbo.Account a with(nolock)
    left join dbo.OrganizationRequest2010 OrgReq with(nolock) on OrgReq.Id = a.OrganizationId
    left join dbo.Region r with(nolock) on r.Id = OrgReq.RegionId
    inner join dbo.GroupAccount ga on ga.AccountId=a.id
  where ga.groupid=1
  group by
    r.Id, r.Code, r.Name
  )
  select *, 0 [IsTotal]
  from RegionUserCountCTE
  union all
  select NULL, NULL, sum(count), 1 from RegionUserCountCTE
  order by [IsTotal], RegionCode

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetLoginAttemptsInfo]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------------------------------
--Автор: Сулиманов А.М.
--Дата: 2009-06-02
--Проверка количества попыток залогинится
-------------------------------------------------
alter procEDURE [dbo].[GetLoginAttemptsInfo]
( @IP varchar(32), 
  @TimeInterval int
)
AS
  SET NOCOUNT ON

  DECLARE @startDate datetime, @endDate datetime, @eventCode varchar(20)
  SET @endDate=GETDATE()
  SET @startDate=DATEADD(ss,-@TimeInterval,@endDate)
  SET @eventCode='USR_VERIFY'

  SELECT 
      ISNULL(MAX(Date),CAST('1900-01-01' as datetime)) LastLoginDate, 
      @endDate as CheckedDate, 
      --COUNT(*) Attempts, 
      ISNULL(SUM([LoginFailResult]),0) AttemptsFail
  FROM (
    SELECT  
      --LEFT(EventParams,CHARINDEX('|',EventParams)-1) AS [Login],
      Date,
      CASE SUBSTRING(EventParams,LEN(EventParams)-2,1)
        WHEN '1' THEN 0
        ELSE 1
      END AS [LoginFailResult]
    FROM dbo.EventLog
    WHERE (Date between @startDate and @endDate) 
        AND EventCode=@eventCode AND IP=@IP
  ) T
GO
/****** Object:  StoredProcedure [dbo].[GetRemindAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetRemindAccount

-- =============================================
-- Получить забытую учетную запись.
-- v.1.0: Created by Makarev Andrey
-- =============================================
alter procedure [dbo].[GetRemindAccount]
  @email nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  
  declare
    @currentYear int
    , @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @login nvarchar(255) 
    , @accountId bigint
    , @accountIds nvarchar(255)

  set @currentYear = year(getdate())
  set @eventCode = N'USR_REMIND'

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin

  select top 1
    @login = account.[Login] 
    , @accountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.email = @email
  order by 
    dbo.GetUserStatusOrder(dbo.GetUserStatus(account.ConfirmYear , account.Status
        , @currentYear, account.RegistrationDocument)) desc
    , account.UpdateDate desc

  select 
    @login [Login]
    , @email email

  set @accountIds = isnull(convert(nvarchar(255), @accountId), '')

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = @email
    , @updateId = null

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountActivityByRegionReport]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetUserAccountActivityByRegionReport
-- ========================================================
-- Отчет по активности пользователей по регионам.
-- v.1.0: Create by Fomin Dmitriy 03.09.2008
-- ========================================================
alter procedure [dbo].[GetUserAccountActivityByRegionReport]
as 
begin
  declare
    @year int

  set @year = Year(GetDate())
  
  select
    region.Code RegionCode
    , region.Name RegionName
    , report.[Count]
    , case 
      when report.RegionId is null then 1
      else 0
    end IsTotal
  from (select 
      region.Id RegionId
      , count(*) [Count]
    from dbo.AuthenticationEventLog auth_log
      inner join dbo.Account account
        inner join dbo.Organization organization
          inner join dbo.Region region
            on region.Id = organization.RegionId
          on organization.Id = account.OrganizationId
        on auth_log.AccountId = account.Id
    where
      account.Id in (select 
            group_account.AccountId
          from dbo.GroupAccount group_account
            inner join dbo.[Group] [group]
              on [group].Id = group_account.GroupId
          where [group].Code = 'User')
      and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
          , account.RegistrationDocument) = 'activated'
      and auth_log.IsPasswordValid = 1
      and auth_log.IsIpValid = 1
    group by 
      region.Id
    with rollup) report
      left outer join dbo.Region region
        on region.Id = report.RegionId
  order by
    IsTotal
    , region.SortIndex

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Выполнить задание на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
alter proc [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]
  @id bigint
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  set @eventCode = N'CNE_LOAD'

  set @internalId = dbo.GetInternalId(@Id)

  update cne_certificate_loading_task
  set
    IsProcess = 0
    , UpdateDate = @currentDate
    , EditorAccountId = @accountId
    , EditorIp = @editorIp
  from
    dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task
  where
    cne_certificate_loading_task.Id = @internalId
    and cne_certificate_loading_task.IsActive <> 0
    and cne_certificate_loading_task.IsLoaded <> 1

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

end
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Выполнить задание на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
alter proc [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]
  @id bigint
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  set @eventCode = N'CNE_DNY_LOAD'

  set @internalId = dbo.GetInternalId(@Id)

  update cne_certificate_deny_loading_task
  set
    IsProcess = 0
    , UpdateDate = @currentDate
    , EditorAccountId = @accountId
    , EditorIp = @editorIp
  from
    dbo.CommonNationalExamCertificateDenyLoadingTask cne_certificate_deny_loading_task
  where
    cne_certificate_deny_loading_task.Id = @internalId
    and cne_certificate_deny_loading_task.IsActive <> 0
    and cne_certificate_deny_loading_task.IsLoaded <> 1

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

end
GO
/****** Object:  StoredProcedure [dbo].[DeleteNews]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteNews

-- =============================================
-- Удаление новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
alter proc [dbo].[DeleteNews]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  set @innerIds = ''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N'NWS_DEL'

  delete news
  from 
    dbo.News news
      inner join @idTable idTable
        on news.Id = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0

end
GO
/****** Object:  StoredProcedure [dbo].[DeleteDocument]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteDocument

-- =============================================
-- Удаление документов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
alter proc [dbo].[DeleteDocument]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  set @innerIds = ''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N'DOC_DEL'

  begin tran delete_document_tran

    delete document_context
    from 
      dbo.DocumentContext document_context
        inner join @idTable idTable
          on document_context.DocumentId = idTable.[id]

    if (@@error <> 0)
      goto undo

    delete [document]
    from 
      dbo.[Document] [document]
        inner join @idTable idTable
          on [document].[Id] = idTable.[id]

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran delete_document_tran

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0

  undo:

  rollback tran delete_document_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[DeleteDeliveries]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteNews

-- =============================================
-- Удаление рассылок.
-- v.1.0: Created by Yusupov Kirill 19.04.2010
-- =============================================

alter proc [dbo].[DeleteDeliveries]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select [value] from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N'DLV_DEL'

  --Удалим получателей рассылки
  delete recipient
    from 
      dbo.DeliveryRecipients recipient
        inner join @idTable idTable
          on recipient.DeliveryId = idTable.[id]

  --Удалим лог рассылки
  delete [log]
    from 
      dbo.DeliveryLog [log]
        inner join @idTable idTable
          on [log].DeliveryId = idTable.[id]

  --Удалим саму рассылку
  delete delivery
  from 
    dbo.Delivery delivery
      inner join @idTable idTable
        on delivery.Id = idTable.[id]


  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[DeleteAskedQuestion]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteAskedQuestion

-- =============================================
-- Удаление вопросов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
alter proc [dbo].[DeleteAskedQuestion]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  set @innerIds = ''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N'FAQ_DEL'

  begin tran delete_faq_tran

    delete asked_question_context
    from 
      dbo.AskedQuestionContext asked_question_context
        inner join @idTable idTable
          on asked_question_context.AskedQuestionId = idTable.[id]

    if (@@error <> 0)
      goto undo

    delete asked_question
    from 
      dbo.AskedQuestion asked_question
        inner join @idTable idTable
          on asked_question.[Id] = idTable.[id]

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran delete_faq_tran

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0

  undo:

  rollback tran delete_faq_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[CheckSchoolLeavingCertificate]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckSchoolLeavingCertificate
-- ============================================
-- Процедура проверки аттестатов
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ============================================
alter procedure [dbo].[CheckSchoolLeavingCertificate]
  @certificateNumber nvarchar(255)
  , @login nvarchar(255)
  , @ip nvarchar(255)
as
begin 
  select
    @certificateNumber CertificateNumber
    , case when school_leaving_certificate_deny.Id is null then 0
      else 1
    end IsDeny
    , school_leaving_certificate_deny.Comment DenyComment
  from 
    (select
      @certificateNumber CertificateNumber) as schoolleaving_certificate_check
      left join dbo.SchoolLeavingCertificateDeny school_leaving_certificate_deny
        on schoolleaving_certificate_check.CertificateNumber between 
          school_leaving_certificate_deny.CertificateNumberFrom
            and school_leaving_certificate_deny.CertificateNumberTo  
  
        
  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
  
  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login 

  set @eventCode = 'SLC_CHK'
  set @updateId = NewId()     
  
  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckLastAccountIp]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckLastAccountIp
-- ====================================================
-- Процедура проверки последнего адресса пользователя,
-- под которым он авторизовался
-- v.1.0: Created by Sedov Anton 08.07.2008
-- v.1.1: Modified by Fomin Dmitriy 28.08.2008
-- Добавлена регистрация события авторизации.
-- ====================================================
alter procedure [dbo].[CheckLastAccountIp] 
  @accountLogin nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare
    @isLastIp bit
    , @entityParams nvarchar(1000)
    , @sourceEntityIds nvarchar(255)
    , @accountId bigint

  set @isLastIp = null

  select top 1
    @isLastIp = case when auth_event_log.Ip = @ip
        then 1
      else 0
    end
    , @accountId = account.Id
  from 
    dbo.AuthenticationEventLog auth_event_log
      left join dbo.Account account
        on account.Id = auth_event_log.AccountId
  where
    account.[Login] = @accountLogin
      and auth_event_log.IsPasswordValid = 1
      and auth_event_log.IsIpValid = 1
  order by 
    auth_event_log.Date desc
    

  select
    @accountLogin AccountLogin
    , @ip Ip
    , isnull(@isLastIp, 0) IsLastIp           

  set @entityParams = @accountLogin + N'|' +
      @ip + N'||' +
      convert(nvarchar, case 
          when @isLastIp is null then 0 
          else 1 
        end)  + '|' +
      convert(nvarchar, isnull(@isLastIp, 0))

  set @sourceEntityIds = convert(nvarchar(255), @accountId)

  if isnull(@isLastIp, 0) = 1
    exec dbo.RegisterEvent 
      @accountId = @accountId
      , @ip = @ip
      , @eventCode = N'USR_VERIFY'
      , @sourceEntityIds = @sourceEntityIds
      , @eventParams = @entityParams
      , @updateId = null

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckEntrant]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckEntrant
-- ============================================
-- Процедура проверки поступивших абитуриентов
-- v.1.0: Create by Sedov Anton 08.07.2008
-- ============================================
alter procedure [dbo].[CheckEntrant] 
  @certificateNumber nvarchar(255)
  , @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  select  
    @certificateNumber CertificateNumber
    , entrant.LastName LastName
    , entrant.FirstName FirstName
    , entrant.PatronymicName PatronymicName
    , organization.[Name] OrganizationName
    , entrant.CreateDate EntrantCreateDate
    , case when entrant.CertificateNumber is null
        then 0
      else 1
    end IsExist
  from 
    (select @certificateNumber CertificateNumber) as check_entrant
      left join dbo.Entrant entrant with(nolock, fastfirstrow)
        inner join dbo.Organization organization
          on organization.Id = entrant.OwnerOrganizationId
        on check_entrant.CertificateNumber = entrant.CertificateNumber

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
  
  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login 

  set @eventCode = 'ENT_CHK'
  set @updateId = NewId()     
  
  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId
      
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByPassportForXml]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================

alter proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]  
   @subjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @shouldWriteLog BIT = 1         -- нужно ли записывать в лог интерактивных проверок 
  ,@xml xml out
as
begin 
set nocount on
  
  if @passportSeria is null and @passportNumber is null
  begin
    RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
    return
  end
    
  IF (@passportSeria is null)
    SET @passportSeria = ''

  declare @eventId INT
  IF @shouldWriteLog = 1
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @subjectMarks, @passportSeria = @passportSeria, @passportNumber = @passportNumber, @IsOpenFbs = 1, @eventId = @eventId output

declare 
    @eventCode nvarchar(255)
        , @organizationId bigint
    , @editorAccountId bigint
    , @eventParams nvarchar(4000)   
    , @commandText nvarchar(4000)
    , @internalPassportSeria nvarchar(255)

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  set @eventParams = 
    isnull(@subjectMarks, '') + '|' 
    + isnull(@passportSeria, '') + '|' 
    + isnull(@passportNumber, '') + '|' 

  select
    @editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login    

  if not @passportSeria is null
    set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

  set @commandText = ''
  set @eventCode = N'CNE_FND_P'

  declare @sourceEntityIds nvarchar(4000) 
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
    , ParticipantsID uniqueidentifier
  ) 
    
  set @commandText = @commandText +       
    'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year, ParticipantsID
    from 
    (
      select b.LicenseNumber AS Number, a.Surname AS FirstName, a.Name AS LastName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantsID
            from rbd.Participants a with (nolock)
        left join prn.Certificates b with (nolock) on b.ParticipantFK = a.ParticipantID
        left join prn.CancelledCertificates c on c.CertificateFK=b.CertificateID
    ) certificate
    where 
    ' 
    if not @internalPassportSeria is null
    begin
        set @commandText = @commandText + ' certificate.PassportSeria = @internalPassportSeria '
  end
  
  if not @passportNumber is null
    begin
        set @commandText = @commandText + ' and  certificate.PassportNumber = @passportNumber '
    end
  else
  begin
    goto nullresult
  end
  print @commandText
  insert into @Search
  exec sp_executesql @commandText
    , N' @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255) '
    ,@internalPassportSeria, @passportNumber    
    
/*
declare @xml xml
exec  [CheckCommonNationalExamCertificateByPassportForXml]  @login='SuperAdmin@sibmail.com',@ip='SuperAdmin@sibmail.com',@passportSeria='9205',@passportNumber ='527439',@subjectMarks='',@xml=@xml out
select @xml 
*/
  if @subjectMarks is not null
  begin
    
  if exists(
    select * from
    dbo.GetSubjectMarks(@subjectMarks)  t
    left join (
    select distinct b.SubjectCode SubjectId,b.Mark  from 
    @Search search
      join [prn].CertificatesMarks b with(nolock) 
      on search.CertificateId = b.CertificateFK
      ) tt
      on t.SubjectId=tt.SubjectId and t.Mark=tt.Mark
      where tt.SubjectId is null)
  delete from @search
      
  
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
           @certificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 
  
select @xml=(
  select 
  (
  select 
      S.CertificateId CertificateId,  
      S.CertificateNumber CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case when S.CertificateId is not null then 1 else 0 end IsExist, 
      case  when CD.Id >0 then 1 end IsDeny,  
      CD.Comment DenyComment, 
      CD.NewCertificateNumber NewCertificateNumber, 
      S.Year Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when CD.Id>0 then 'Аннулировано' 
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
      S.ParticipantsID
    from 
        @Search S 
            left join dbo.vw_Examcertificate C with (nolock) 
              on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.IdGuid  = S.certificateId 
      left outer join CommonNationalExamCertificateDeny CD with (nolock) 
        on S.CertificateNumber = CD.CertificateNumber 
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed
              on ed.[year] = S.[year]
            for xml path('check'), ELEMENTS XSINIL,type
  ) 
  for xml path('root'),type
  )

/*
declare @xml xml
exec  [CheckCommonNationalExamCertificateByPassportForXml]  @login='SuperAdmin@sibmail.com',@ip='SuperAdmin@sibmail.com',@passportSeria='9205',@passportNumber ='527439',@subjectMarks='',@xml=@xml out
select @xml
*/
    
goto result 
nullresult:
  select @xml=(
  select null 
  for xml path('root'),type
  )
result:

    --select * from @Search
    
    -- записать в лог интерактивных проверок
    IF EXISTS (SELECT * FROM @Search) AND @shouldWriteLog = 1
    BEGIN
      UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 cast(certificateId as nvarchar(510)) FROM @Search)
       WHERE Id=@eventId
    END
    

  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null    
  
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumberForXml]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
alter proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
    @number nvarchar(255) = null        -- номер сертификата  
  , @checkSubjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
  , @xml xml out
as
begin 
  
  if @number is null
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
  
  declare @certificate_check table
  (
  Number nvarchar(255)
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
      when [certificate].Id is not null then 1
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
    , [certificate].ParticipantID
  from 
    (select null ''empty'') t left join 
    dbo.vw_Examcertificate [certificate] with (nolock, fastfirstrow) on 
        [certificate].[Year] between @yearFrom and @yearTo '
  
    set @sql = @sql + ' and [certificate].Number = @number'
  set @sql = @sql + '     
    left outer join dbo.Region region
      on region.Id = [certificate].RegionId
    left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[Year] between @yearFrom and @yearTo
        and certificate_deny.certificateNumber = [certificate].Number'

  insert into @certificate_check  
  exec sp_executesql @sql,N'@number nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int',@number = @number,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo
  
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
            @certificateIdGuid = @CId
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
        CC.UniqueOtherCheck UniqueOtherCheck,
        certificate_check.ParticipantID
        into #table
  from @certificate_check certificate_check
      /*inner join CommonNationalExamcertificate C on C.Id = certificate_check.certificateId*/
        left outer join ExamcertificateUniqueChecks CC on CC.IdGuid = certificate_check.certificateId
    left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
    left outer join (
      select      
        getcheck_subject.SubjectId id,[subject].Name,
        certificate_subject.UseYear [Year],certificate_subject.certificateFK certificateId, 
        isnull(getcheck_subject.SubjectId, certificate_subject.SubjectCode) SubjectId
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
      [prn].CertificatesMarks certificate_subject with (nolock)
      join dbo.[Subject] [subject]  on [subject].Id = certificate_subject.SubjectCode   
      left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject  on getcheck_subject.SubjectId = certificate_subject.SubjectCode     
      left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.UseYear = mm.[Year] 
      left join [MinimalMark] as mm1 on certificate_subject.SubjectCode = mm1.[SubjectId] and certificate_subject.UseYear = mm1.[Year] 
      ) check_subject
      on certificate_check.[Year] = check_subject.[Year] and certificate_check.certificateId = check_subject.certificateId
      
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
    --SELECT * from @certificate_check
    -- записать в лог интерактивных проверок
    if @shouldCheckMarks = 1 and exists (select * from #table)
    UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 certificateId FROM @certificate_check)
      WHERE Id=@eventId
    drop table #table
    
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
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumber]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
  
  declare @certificate_check table
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
    , case
      when @ParticipantID is null and [certificate].Id is not null then 1
      when @ParticipantID is not null and certificate.ParticipantID is not null then 1      
      else 0
    end IsExist
    , [certificate].Id
    , case
      when isnull(certificate_deny.UseYear, 0) <> 0 then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Reason Comment
    , null NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , certificate.ParticipantID
  from 
    (select null ''empty'') t left join 
    (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, b.Cancelled,
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) [certificate] on 
        [certificate].[Year] between @yearFrom and @yearTo '
  if @ParticipantID is not null 
    set @sql = @sql + ' and [certificate].ParticipantID = @ParticipantID'   
  if @number is not null 
    set @sql = @sql + ' and [certificate].Number = @number'
  if @CheckTypographicNumber is not null 
    set @sql = @sql + ' and [certificate].TypographicNumber=@CheckTypographicNumber'
  
  set @sql = @sql + '     
    left outer join dbo.Region region
      on region.Id = [certificate].RegionId
    left outer join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] between @yearFrom and @yearTo
        and certificate_deny.CertificateFK = [certificate].id'

  insert into @certificate_check    
  exec sp_executesql @sql,N'@checkLastName nvarchar(255),@number nvarchar(255),@checkTypographicNumber nvarchar(20),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier',@checkLastName=@checkLastName,@number = @number,@checkTypographicNumber=@checkTypographicNumber,@checkFirstName=@checkFirstName,@checkPatronymicName=@checkPatronymicName,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID

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
            @certificateIdGuid = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
  -------------------------------------------------------------
  select
    isnull(cast(certificate_check.certificateId as nvarchar(250)),'Нет свидетельства' ) certificateId
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
    , case when check_subject.CheckSubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',') CheckSubjectMark
    , case when check_subject.SubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',') SubjectMark
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
    , case when ed.[ExpireDate] is null then 'Не найдено' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
      case when getdate() <= ed.[ExpireDate] then 'Действительно'
      else 'Истек срок' end end end as [Status],
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
        C.ParticipantID
  from @certificate_check certificate_check
      join ((SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
    FROM rbd.Participants a with (nolock, fastfirstrow) left JOIN
                      prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID)) C
          on 1= case when @ParticipantID is null and C.Id = certificate_check.certificateId then 1
                  when @ParticipantID is not null and C.ParticipantID=certificate_check.ParticipantID then 1 
             else 0 end
        left outer join ExamcertificateUniqueChecks CC
      on CC.IdGuid = C.Id
    left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
    left outer join (
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
        , isnull(check_subject.SubjectId, certificate_subject.SubjectCode) SubjectId
        , check_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when check_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal,
        certificate_check.certificateId,
        certificate_subject.ParticipantFK ParticipantID
      from [prn].CertificatesMarks certificate_subject with (nolock)         
        inner join @certificate_check certificate_check
          on certificate_check.[Year] = certificate_subject.UseYear
            and 1= case when @ParticipantID is null and certificate_check.certificateId = certificate_subject.CertificateFK then 1
                        when @ParticipantID is not null and certificate_subject.ParticipantFK = certificate_check.ParticipantID then 1
                    else 0 end
        left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
          on check_subject.SubjectId = certificate_subject.SubjectCode
            ) check_subject
      left outer join dbo.[Subject] [subject]
        on [subject].Id = check_subject.SubjectId
        
      on 1 = case when @ParticipantID is null 
                      and isnull(certificate_check.certificateId, '76A02C33-9A91-443A-996C-7640F7481B55') = isnull(check_subject.certificateId,'76A02C33-9A91-443A-996C-7640F7481B55') then 1
                  when @ParticipantID is not null 
                      and C.ParticipantID=check_subject.ParticipantID then 1
             else 0 end                 
                  
      left join [MinimalMark] as mm on check_subject.SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year]    
  
  
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
GO
/****** Object:  StoredProcedure [dbo].[CheckAccountKey]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
alter procedure [dbo].[CheckAccountKey]
  @key nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare
    @now datetime
    , @entityParams nvarchar(1000)
    , @sourceEntityIds nvarchar(255)
    , @accountId bigint
    , @isValid bit
    , @year int
    , @login nvarchar(255)

  set @now = convert(nvarchar(8), GetDate(), 112)
  set @year = Year(GetDate())

  select top 1
    @accountId = account.Id
    , @login = account.Login
  from dbo.Account account
    inner join dbo.AccountKey account_key
      on account.Id = account_key.AccountId
  where
    account_key.[Key] = @key
    and account_key.IsActive = 1
    and @now between isnull(account_key.DateFrom, @now) and isnull(account_key.DateTo, @now)
    and ((account.Id in (select group_account.AccountId
        from dbo.GroupAccount group_account
          inner join dbo.[Group] [group]
            on [group].Id = group_account.GroupId
        where [group].Code = 'User')
        and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
            , account.RegistrationDocument) = 'activated')
      or (account.Id in (select group_account.AccountId
        from dbo.GroupAccount group_account
          inner join dbo.[Group] [group]
            on [group].Id = group_account.GroupId
        where [group].Code = 'Administrator')
        and account.IsActive = 1))

  if not @login is null
    set @isValid = 1
  else
    set @isValid = 0
    
  select
    @key [Key]
    , @login [Login]
    , @isValid IsValid

  set @entityParams = @key + N'|' +
      convert(nvarchar, @isValid)

  set @sourceEntityIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = N'USR_KEY_VERIFY'
    , @sourceEntityIds = @sourceEntityIds
    , @eventParams = @entityParams
    , @updateId = null

  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportCheckedCNEsTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Номер свидетельства] NVARCHAR(255)
,[Проверок в различных регионах] INT
,[Проверок в различных ОУ] INT
,[Проверок в различных ВУЗах] INT
,[В негосударственных ВУЗах] INT
,[В государственных ВУЗах] INT
,[Проверок в различных ССУЗах] INT
,[В негосударственных ССУЗах] INT
,[В государственных ССУЗах] INT
)
AS 
BEGIN

INSERT INTO @Report
SELECT 
IChecks.CNENumber AS [Номер свидетельства]
,COUNT(DISTINCT IChecks.RegId) AS [Проверок в различных регионах]
,COUNT(*) AS [Проверок в различных ОУ] 
,COUNT(CASE WHEN IChecks.OrgType=1 THEN 1 ELSE NULL END) AS [Проверок в различных ВУЗах]
,COUNT(CASE WHEN IChecks.OrgType=1 AND IChecks.OPF=1 THEN 1 ELSE NULL END) AS [В негосударственных ВУЗах]
,COUNT(CASE  WHEN IChecks.OrgType=1 AND IChecks.OPF=0 THEN 1 ELSE NULL END) AS [В государственных ВУЗах]
,COUNT(CASE WHEN IChecks.OrgType=2 THEN 2 ELSE NULL END) AS [Проверок в различных ССУЗах]
,COUNT(CASE WHEN IChecks.OrgType=2 AND IChecks.OPF=1 THEN 1 ELSE NULL END) AS [В негосударственных ССУЗах]
,COUNT(CASE  WHEN IChecks.OrgType=2 AND IChecks.OPF=0 THEN 1 ELSE NULL END) AS [В государственных ССУЗах]
FROM 
(
  SELECT CNENumber ,OrgId,Org.TypeId AS OrgType,Org.IsPrivate AS OPF,Org.RegionId AS RegId  
  FROM [ReportCheckedCNEsBASE]() AS Rpt
  INNER JOIN Organization2010 Org ON Org.Id=Rpt.OrgId
) AS IChecks
GROUP BY IChecks.CNENumber HAVING COUNT(*)>=6


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsDetailedTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportCheckedCNEsDetailedTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Номер свидетельства] NVARCHAR(255)
,[Регион свидетельства] NVARCHAR(500)
,[Количество проверок] INT
,[Проверяющее ОУ] NVARCHAR(4000)
,[Тип ОУ/ОПФ ОУ] NVARCHAR(255)
,[Регион ОУ] NVARCHAR(500)
--,[Дата первой проверки] INT
)
AS 
BEGIN

DECLARE @BaseReport TABLE 
(
  CNENumber NVARCHAR(255),
  CNEId BIGINT,
  OrgId INT
)
INSERT INTO @BaseReport(CNENumber,CNEId,OrgId)
SELECT CNENumber,CNEId,OrgId FROM [ReportCheckedCNEsBASE]()

DECLARE @ChecksCount TABLE
(
  CNEId BIGINT,
  Checks INT
)
INSERT INTO @ChecksCount
SELECT 
  CNEId AS CNEId,
  COUNT(OrgId) AS OrgCount
FROM @BaseReport AS IChecks
GROUP BY IChecks.CNEId HAVING COUNT(IChecks.OrgId)>=6

DECLARE @ReportWithoutOrder TABLE
(
[Номер свидетельства] NVARCHAR(255)
,[Регион свидетельства] NVARCHAR(500)
,[Количество проверок] INT
,[Проверяющее ОУ] NVARCHAR(4000)
,[Тип ОУ/ОПФ ОУ] NVARCHAR(255)
,[Регион ОУ] NVARCHAR(500)
)

INSERT INTO @ReportWithoutOrder
SELECT 
IChecks.CNENumber AS [Номер свидетельства]
,IChecks.CNERegName AS [Регион свидетельства]
,IChecks.OrgCount AS [Количество проверок]
,IChecks.OrgName AS [Проверяющее ОУ]
,IChecks.OrgType+'/'+CASE WHEN IChecks.OPF=1 THEN 'Негосударственный' ELSE 'Государственный' END AS [Тип ОУ/ОПФ ОУ]
,IChecks.OrgRegName AS [Регион ОУ]
FROM 
(
  SELECT 
  CNENumber,
  Org.FullName AS OrgName,
  OrgType.Name AS OrgType,
  Org.IsPrivate AS OPF,
  OrgReg.Name AS OrgRegName,
  CNEReg.Name AS CNERegName,  
  CntRpt.Checks AS OrgCount
  FROM @ChecksCount CntRpt
  INNER JOIN @BaseReport AS Rpt ON CntRpt.CNEId=Rpt.CNEId
  INNER JOIN Organization2010 Org ON Org.Id=Rpt.OrgId
  INNER JOIN dbo.CommonNationalExamCertificate CNE ON CNE.Id=Rpt.CNEId
  INNER JOIN dbo.Region CNEReg ON CNEReg.Id=CNE.RegionId
  INNER JOIN dbo.Region OrgReg ON OrgReg.Id=Org.RegionId
  INNER JOIN dbo.OrganizationType2010 OrgType ON Org.TypeId=OrgType.Id
) AS IChecks

INSERT INTO @Report 
SELECT * FROM @ReportWithoutOrder
ORDER BY [Количество проверок] DESC,[Номер свидетельства]

RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[VerifyAccount]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[VerifyAccount]
  @login nvarchar(255)
  , @ip nvarchar(255)
AS
BEGIN

  DECLARE @isLoginValid bit
    , @isIpValid bit
    , @accountId bigint
    , @entityParams nvarchar(1000)
    , @sourceEntityIds nvarchar(255)
  
  SELECT @isLoginValid = 0, @isIpValid = 0

  SELECT @accountId = [Id], 
      @isLoginValid = 
        CASE 
          WHEN [Status] <> 'deactivated' 
          THEN 1 
          ELSE 0 
        END 
  FROM dbo.Account with (nolock)
  WHERE [Login] = @login

  -- IP не проверяем - он валидный при валидном пароле
  SET @isIpValid=@isLoginValid

  SET @entityParams = @login + N'|' + @ip + N'|' +
      CONVERT(nvarchar, @isLoginValid)  + '|' +
      CONVERT(nvarchar, @isIpValid)

  SET @sourceEntityIds = CONVERT(nvarchar(255), ISNULL(@accountId,0))

  SELECT @login [Login], @ip Ip, @isLoginValid IsLoginValid, @isIpValid IsIpValid

  EXEC dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = N'USR_VERIFY'
    , @sourceEntityIds = @sourceEntityIds
    , @eventParams = @entityParams
    , @updateId = null

  RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccount]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateAccount
-- =============================================
-- Сохранить учетную запись.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.3: Modified by Makarev Andrey 16.04.2008
-- Измение процедуры GetDelimitedValues().
-- v.1.4: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.5: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- =============================================
alter procedure [dbo].[UpdateAccount]
  @login nvarchar(255)
  , @passwordHash nvarchar(255) = null
  , @lastName nvarchar(255)
  , @firstName nvarchar(255)
  , @patronymicName nvarchar(255)
  , @phone nvarchar(255)
  , @email nvarchar(255)
  , @isActive bit
  , @ipAddresses nvarchar(4000) = null
  , @groupCode nvarchar(255) = null
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
  , @hasFixedIp bit = null
as
begin
  declare @exists table([login] nvarchar(255), isExists bit)

  insert @exists exec dbo.CheckNewLogin @login = @login
  
  declare 
    @isExists bit
    , @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @accountId bigint
    , @status nvarchar(255)
    , @innerStatus nvarchar(255)
    , @confirmYear int
    , @currentYear int
    , @userGroupId int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()

  select @userGroupId = [group].[Id]
  from dbo.[Group] [group] with (nolock, fastfirstrow)
  where [group].[Code] = @groupCode

  select @isExists = user_exists.isExists
  from  @exists user_exists

  select @editorAccountId = account.[Id]
  from  dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @editorLogin

  select @accountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  set @currentYear = year(getdate())

  set @confirmYear = @currentYear

  declare @oldIpAddress table (ip nvarchar(255))

  declare @newIpAddress table (ip nvarchar(255))

--если логина нет - добавляем запись и добавляем пользователя в группу
--если логин есть - меняем данные
  if @isExists = 0  -- внесение нового пользователя
  begin
    select 
      @status = case when @groupCode='User' then  null else 'activated' end,
      @hasFixedIp = isnull(@hasFixedIp, 1), @eventCode = N'USR_REG'

    select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
  end
  else
  begin -- update существующего пользователя
    select 
      @accountId = account.[Id]
      , @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
    from 
      dbo.Account account with (nolock, fastfirstrow)
    where
      account.[Login] = @login

    insert @oldIpAddress
      (
      ip
      )
    select
      account_ip.Ip
    from
      dbo.AccountIp account_ip with (nolock, fastfirstrow)
    where
      account_ip.AccountId = @accountId

    set @eventCode = N'USR_EDIT'
  end

  if @hasFixedIp = 1
    insert @newIpAddress
      (
      ip
      )
    select 
      ip_addresses.[value]
    from 
      dbo.GetDelimitedValues(@ipAddresses) ip_addresses

  begin tran insert_update_account_tran

    if @isExists = 0  -- внесение нового пользователя
    begin
      insert dbo.Account
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Login]
        , PasswordHash
        , LastName
        , FirstName
        , PatronymicName
        , OrganizationId
        , IsOrganizationOwner
        , ConfirmYear
        , Phone
        , Email
        , RegistrationDocument
        , AdminComment
        , IsActive
        , Status
        , IpAddresses
        , HasFixedIp
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @login
        , @passwordHash
        , @lastName
        , @firstName
        , @patronymicName
        , null
        , 0
        , @confirmYear
        , @phone
        , @email
        , null
        , null
        , @isActive
        , @status
        , @ipAddresses
        , @hasFixedIp

      if (@@error <> 0)
        goto undo

      select @accountId = scope_identity()

      if (@@error <> 0)
        goto undo

      insert dbo.AccountIp
        (
        AccountId
        , Ip
        )
      select
        @accountId
        , new_ip_address.ip
      from 
        @newIpAddress new_ip_address

      if (@@error <> 0)
        goto undo

      insert dbo.GroupAccount
        (
        GroupId
        , AccountID
        )
      select
        @userGroupId
        , @accountId

      if (@@error <> 0)
        goto undo

    end
    else
    begin -- update существующего пользователя
      update account
      set
        UpdateDate = getdate()
        , UpdateId = @updateId
        , EditorAccountID = @editorAccountId
        , EditorIp = @editorIp
        , LastName = @lastName
        , FirstName = @firstName
        , PatronymicName = @patronymicName 
        , phone = @phone
        , email = @email
        , IsActive = @isActive
        , IpAddresses = @ipAddresses
        , HasFixedIp = @hasFixedIp
      from
        dbo.Account account with (rowlock)
      where
        account.[Id] = @accountId

      if (@@error <> 0)
        goto undo

      if exists(select 
            1
          from
            @oldIpAddress old_ip_address
              full outer join @newIpAddress new_ip_address
                on old_ip_address.ip = new_ip_address.ip
          where
            old_ip_address.ip is null
            or new_ip_address.ip is null) 
      begin
        delete account_ip
        from 
          dbo.AccountIp account_ip
        where
          account_ip.AccountId = @accountId

        if (@@error <> 0)
          goto undo

        insert dbo.AccountIp
          (
          AccountId
          , Ip
          )
        select
          @accountId
          , new_ip_address.ip
        from 
          @newIpAddress new_ip_address

        if (@@error <> 0)
          goto undo
      end
    end

  if @@trancount > 0
    commit tran insert_update_account_tran

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_account_tran

  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountStatus]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateUserAccountStatus

-- =============================================
-- Изменить статус пользователя.
-- v.1.0: Created by Makarev Andrey 08.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Рефакторинг: выделена функция, изменеа логика 
-- означивания полей.
-- v.1.2: Modified by Fomin Dmitriy 11.04.2008
-- Удаляется документ регистрации, если он устарел.
-- v.1.3: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.4: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.5: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- =============================================
alter proc [dbo].[UpdateUserAccountStatus]
  @login nvarchar(255)
  , @status nvarchar(255)
  , @adminComment ntext 
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare
    @isActive bit
    , @eventCode nvarchar(255)
    , @accountId bigint
    , @editorAccountId bigint
    , @currentYear int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()
  set @eventCode = N'USR_STATE'
  set @currentYear = Year(GetDate())
  
  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  select
    @accountId = account.[Id]
    , @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, 
        account.RegistrationDocument)
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  update account
  set 
    Status = @status
    , AdminComment = case
      when dbo.HasUserAccountAdminComment(@status) = 0 then null
      else @adminComment
    end
    , IsActive = dbo.GetUserIsActive(@status)
    , UpdateDate = GetDate()
    , UpdateId = @updateId
    , ConfirmYear = @currentYear
    -- Удаляем документ регистрации, если он устарел.
    , RegistrationDocument = case
      when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 then null
      else account.RegistrationDocument
    end
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
  from 
    dbo.Account account with (rowlock)
  where
    account.[Id] = @accountId

  exec dbo.RefreshRoleActivity @accountId = @accountId

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountRegistrationDocument]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateUserAccountRegistrationDocument

-- =============================================
-- Изменить регистрационный документ пользователя.
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.5: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены Status output-параметр.
-- =============================================
alter proc [dbo].[UpdateUserAccountRegistrationDocument]
  @login nvarchar(255)
  , @registrationDocument image
  , @registrationDocumentContentType nvarchar(255)
  , @status nvarchar(255) output
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare
    @accountId bigint
    , @editorAccountId bigint
    , @currentYear int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()

  set @currentYear = Year(GetDate())

  select
    @accountId = a.[Id]
    , @status = dbo.GetUserStatus(a.ConfirmYear, isnull(@status, a.Status), @currentYear, @registrationDocument)
  from 
    dbo.Account a with (nolock, fastfirstrow)
  where 
    a.[Login] = @login

  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin

  update account
  set
    UpdateDate = GetDate()
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , RegistrationDocument = @registrationDocument
    , RegistrationDocumentContentType = @registrationDocumentContentType
    , [Status] = @status
  from 
    dbo.Account account with (rowlock)
  where 
    account.[Id] = @accountId

  exec dbo.RefreshRoleActivity @accountId = @accountId

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = N'USR_EDIT'
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  if @registrationDocument is not null
  begin
    RAISERROR (N'
    Загружена новая заявка на регистрацию:
    Пользователь: %s (https://www.fbsege.ru/Administration/Accounts/Users/View.aspx?login=%s)
    

    ----------------------------------------
    Данное письмо не является сообщением об ошибке, а служит для оповещения операторов.
    ', 7, 2, @login, @login) with log
  end

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccount]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modified 04.05.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
alter procEDURE [dbo].[UpdateUserAccount]
@login NVARCHAR (255)=null OUTPUT, 
@passwordHash NVARCHAR (255)=null, 
@lastName NVARCHAR (255)=null, 
@firstName NVARCHAR (255)=null, 
@patronymicName NVARCHAR (255)=null, 
@organizationRegionId INT=null, 
@organizationFullName NVARCHAR (2000)=null,
@organizationShortName NVARCHAR (2000)=null, 
@organizationINN NVARCHAR (10)=null, 
@organizationOGRN NVARCHAR (13)=null, 
@organizationFounderName NVARCHAR (2000)=null, 
@organizationFactAddress NVARCHAR (500)=null,
@organizationLawAddress NVARCHAR (500)=null, 
@organizationDirName NVARCHAR (255)=null,
@organizationDirPosition NVARCHAR (500)=null, 
@organizationPhoneCode NVARCHAR (255)=null,
@organizationFax NVARCHAR (255)=null,
@organizationIsAccred bit=0,
@organizationIsPrivate bit=0,
@organizationIsFilial bit=0,
@organizationAccredSert nvarchar(255)=null,
@organizationEMail nvarchar(255)=null,
@organizationSite nvarchar(255)=null, 
@organizationPhone NVARCHAR (255)=null, 
@phone NVARCHAR (255)=null, 
@email NVARCHAR (255)=null, 
@ipAddresses NVARCHAR (4000)=null, 
@status NVARCHAR (255)=null OUTPUT, 
@registrationDocument IMAGE=null, 
@registrationDocumentContentType NVARCHAR (225)=null, 
@editorLogin NVARCHAR (255)=null, 
@editorIp NVARCHAR (255)=null, 
@password NVARCHAR (255)=null, 
@hasFixedIp BIT=null, 
@hasCrocEgeIntegration BIT=null, 
@organizationTypeId INT=null,
@organizationKindId INT=null, 
@ExistingOrgId INT=null
AS
begin
  
  declare 
    @newAccount bit
    , @accountId bigint
    , @currentYear int
    , @isOrganizationOwner bit
    , @organizationId bigint
    , @editorAccountId bigint
    , @departmentOwnershipCode nvarchar(255)
    , @eventCode nvarchar(100)
    , @userGroupId int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)
    , @useOnlyDocumentParam bit

  set @updateId = newid()
  
  declare @groupCode nvarchar(255)
  set @groupCode = 
    case @organizationTypeId 
       when 6 then N'UserDepartment'
       when 4 then N'Auditor'
       when 3 then N'UserRCOI'
       else N'User'
    end
  
  select  top 1 @userGroupId = [group].[Id]
  from dbo.[Group] [group] with (nolock, fastfirstrow)
  where [group].[code] = @groupCode
  
  declare @oldIpAddress table (ip nvarchar(255))
  declare @newIpAddress table (ip nvarchar(255))

  set @currentYear = year(getdate())
  set @departmentOwnershipCode = null

  select @editorAccountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @editorLogin

  
  if isnull(@login, '') = ''
  begin 
    set @useOnlyDocumentParam = 1
    set @eventCode = N'USR_REG'
  end
  else
  begin
    set @useOnlyDocumentParam = 0
    set @eventCode = N'USR_EDIT'
  end

  if isnull(@login, '') = ''
    select top 1 @login = account.login
    from dbo.Account account with (nolock)
    where account.email = @email
      and dbo.GetUserStatus(@currentYear, 
        account.Status, account.ConfirmYear, account.RegistrationDocument) = 'registration'
    order by account.UpdateDate desc

  if isnull(@login, '') = '' -- внесение нового пользователя
  begin
    set @newAccount = 1

    exec dbo.GetNewUserLogin @login = @login output

    set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
    set @hasFixedIp = isnull(@hasFixedIp, 1)
    set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
  end
  else
  begin -- update существующего пользователя
    
    select 
      @accountId = account.[Id]
      , @status = dbo.GetUserStatus(@currentYear, isnull(@status, account.Status), @currentYear
        , @registrationDocument)
      , @registrationDocument = isnull(@registrationDocument, case
        -- Если документ нельзя просмотривать, то считаем, что его нет.
        when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
          or @useOnlyDocumentParam = 1 
          or isnull(datalength(account.RegistrationDocument),0)=0 
          then null
        else account.RegistrationDocument
      end)
      , @registrationDocumentContentType = case
        when not @registrationDocument is null then @registrationDocumentContentType
        -- Если документ нельзя просмотривать, то считаем, что его нет.
        when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
          or @useOnlyDocumentParam = 1        
          then null
        else account.RegistrationDocumentContentType
      end
      , @isOrganizationOwner = account.IsOrganizationOwner
      , @organizationId = account.OrganizationID
      , @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
      , @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, account.HasCrocEgeIntegration)
    from dbo.Account account with (nolock, fastfirstrow)
    where account.[Login] = @login


    if @accountId is null
      return 0

    
    insert @oldIpAddress(ip)
    select account_ip.ip
    from dbo.AccountIp account_ip with (nolock, fastfirstrow)
    where account_ip.AccountId = @accountId
  end

  if @hasFixedIp = 1
    insert @newIpAddress(ip)
    select ip_addresses.[value]
    from dbo.GetDelimitedValues(@ipAddresses) ip_addresses

  begin tran insert_update_account_tran

    
    if @newAccount = 1 -- внесение нового пользователя
    begin
      insert dbo.OrganizationRequest2010
        (
        FullName,
        ShortName,
        RegionId,
        TypeId,
        KindId,
        INN,
        OGRN,
        OwnerDepartment,
        IsPrivate,
        IsFilial,
        DirectorPosition,
        DirectorFullName,
        IsAccredited,
        AccreditationSertificate,
        LawAddress,
        FactAddress,
        PhoneCityCode,
        Phone,
        Fax,
        EMail,
        Site,
        OrganizationId
        )
      select
        @organizationFullName,
        @organizationShortName,
        @organizationRegionId,
        @organizationTypeId,
        @organizationKindId,
        @organizationINN,
        @organizationOGRN,    
        @organizationFounderName,
        @organizationIsPrivate,
        @organizationIsFilial,
        @organizationDirPosition,
        @organizationDirName,
        @organizationIsAccred,
        @organizationAccredSert,
        @organizationLawAddress,
        @organizationFactAddress,
        @organizationPhoneCode,
        @organizationPhone,
        @organizationFax,
        @organizationEMail,
        @organizationSite,  
        @ExistingOrgId
         
      if (@@error <> 0)
        goto undo

      select @organizationId = scope_identity()

      if (@@error <> 0)
        goto undo

      insert dbo.Account
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Login]
        , PasswordHash
        , LastName
        , FirstName
        , PatronymicName
        , OrganizationId
        , IsOrganizationOwner
        , ConfirmYear
        , Phone
        , Email
        , RegistrationDocument
        , RegistrationDocumentContentType
        , AdminComment
        , IsActive
        , Status
        , IpAddresses
        , HasFixedIp
        , HasCrocEgeIntegration
        )
      select
        GetDate()
        , GetDate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @login
        , @passwordHash
        , @lastName
        , @firstName
        , @patronymicName
        , @organizationId
        , 1
        , @currentYear
        , @phone
        , @email
        , @registrationDocument
        , @registrationDocumentContentType
        , null
        , 1
        , @status
        , @ipAddresses
        , @hasFixedIp
        , @hasCrocEgeIntegration

      if (@@error <> 0)
        goto undo

      select @accountId = scope_identity()

      if (@@error <> 0)
        goto undo

      insert dbo.AccountIp(AccountId, Ip)
      select  @accountId, new_ip_address.ip
      from @newIpAddress new_ip_address

      if (@@error <> 0)
        goto undo

      insert dbo.GroupAccount(GroupId, AccountID)
      select  @UserGroupId, @accountId

      if (@@error <> 0)
        goto undo
    end 
    else 
    begin -- update существующего пользователя
      if @isOrganizationOwner = 1
--        update organization
--        set 
--          UpdateDate = GetDate()
--          , UpdateId = @updateId
--          , EditorAccountId = @editorAccountId
--          , EditorIp = @editorIp
--          , RegionId = @organizationRegionId
--          , DepartmentOwnershipCode = @departmentOwnershipCode
--          , [Name] = @organizationFullName
--          , FounderName = @organizationFounderName
--          , Address = @organizationLawAddress
--          , ChiefName = @organizationDirName
--          , Fax = @organizationFax
--          , Phone = @organizationPhone
--          , ShortName = dbo.GetShortOrganizationName(@organizationFullName)
--          , EducationInstitutionTypeId = @organizationTypeId
--          , EtalonOrgId=@ExistingOrgId
--        from 
--          dbo.Organization organization with (rowlock)
--        where
--          organization.[Id] = @organizationId

        update OReq
        set 
          UpdateDate = GetDate(),
          FullName=@organizationFullName,
          ShortName=@organizationShortName,
          RegionId=@organizationRegionId,
          TypeId=@organizationTypeId,
          KindId=@organizationKindId,
          INN=@organizationINN,
          OGRN=@organizationOGRN,   
          OwnerDepartment=@organizationFounderName,
          IsPrivate=@organizationIsPrivate,
          IsFilial=@organizationIsFilial,
          DirectorPosition=@organizationDirPosition,
          DirectorFullName=@organizationDirName,
          IsAccredited=@organizationIsAccred,
          AccreditationSertificate=@organizationAccredSert,
          LawAddress=@organizationLawAddress,
          FactAddress=@organizationFactAddress,
          PhoneCityCode=@organizationPhoneCode,
          Phone=@organizationPhone,
          Fax=@organizationFax,
          EMail=@organizationEMail,
          Site=@organizationSite, 
          OrganizationId=@ExistingOrgId
        from 
          dbo.OrganizationRequest2010 OReq with (rowlock)
        where
          OReq.[Id] = @organizationId

      if (@@error <> 0)
        goto undo

      update account
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , PasswordHash=isnull(@passwordHash,PasswordHash)
        , EditorIp = @editorIp
        , LastName = @lastName
        , FirstName = @firstName
        , PatronymicName = @patronymicName
        , Phone = @phone
        , Email = @email
        , ConfirmYear = @currentYear
        , Status = @status
        , IpAddresses = @ipAddresses
        , RegistrationDocument = @registrationDocument
        , RegistrationDocumentContentType = @registrationDocumentContentType
        , HasFixedIp = @hasFixedIp
        , HasCrocEgeIntegration = @hasCrocEgeIntegration
      from dbo.Account account with (rowlock)
      where account.[Id] = @accountId

      if (@@error <> 0)
        goto undo

      if exists(  select 1 
            from @oldIpAddress old_ip_address
            full outer join @newIpAddress new_ip_address
            on old_ip_address.ip = new_ip_address.ip
            where old_ip_address.ip is null
              or new_ip_address.ip is null) 
      begin
        delete account_ip
        from dbo.AccountIp account_ip
        where account_ip.AccountId = @accountId

        if (@@error <> 0)
          goto undo

        insert dbo.AccountIp(AccountId, Ip)
        select @accountId, new_ip_address.ip
        from @newIpAddress new_ip_address

        if (@@error <> 0)
          goto undo
      end
    end 

-- временно
  if isnull(@password, '') <> '' 
  begin
    if exists(select 1 
        from dbo.UserAccountPassword user_account_password
        where user_account_password.AccountId = @accountId)
    begin
      update user_account_password
      set [Password] = @password
      from dbo.UserAccountPassword user_account_password
      where user_account_password.AccountId = @accountId

      if (@@error <> 0)
        goto undo
    end
    else
    begin
      insert dbo.UserAccountPassword(AccountId, [Password])
      select @accountId, @password

      if (@@error <> 0)
        goto undo
    end
  end

  if @@trancount > 0
    commit tran insert_update_account_tran

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RefreshRoleActivity @accountId = @accountId

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:
  rollback tran insert_update_account_tran
  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateSchoolLeavingCertificateCheckBatch]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateSchoolLeavingCertificateCheckBatch
-- ====================================================
-- Сохранение пакета проверки аттестатов в БД
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ====================================================
alter procedure [dbo].[UpdateSchoolLeavingCertificateCheckBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N'SLC_BCH_CHK'

  begin tran insert_check_batch_tran

    insert dbo.SchoolLeavingCertificateCheckBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_check_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_check_batch_tran

  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateNews]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateNews

-- =============================================
-- Сохранение изменений новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
alter proc [dbo].[UpdateNews]
  @id bigint output
  , @date datetime
  , @name nvarchar(255)
  , @description ntext
  , @text ntext
  , @isActive bit
  , @editorLogin nvarchar(255) = null
  , @editorIp nvarchar(255) = null
as
begin
  declare 
    @newNews bit
    , @currentDate datetime
    , @editorAccountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @oldIsActive bit
    , @public bit
    , @publicEventCode nvarchar(100)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  if isnull(@id, 0) = 0 -- новая новость
  begin
    set @newNews = 1
    set @eventCode = N'NWS_CREATE'
  end
  else
  begin -- update существующего документа
    set @internalId = dbo.GetInternalId(@id)

    select 
      @oldIsActive = news.IsActive
    from 
      dbo.News news with (nolock, fastfirstrow)
    where
      news.[Id] = @internalId

    set @eventCode = N'NWS_EDIT'

    if @oldIsActive <> @isActive
    begin
      set @public = 1
      if @isActive = 1
        set @publicEventCode = N'DOC_PUBLIC'
      else
        set @publicEventCode = N'DOC_UNPUBLIC'
    end
  end

  begin tran insert_update_news_tran

    if @newNews = 1 -- новая новость
    begin
      insert dbo.News
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , Date
        , [Name]
        , Description
        , [Text]
        , IsActive
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @date
        , @name
        , @description
        , @text
        , @isActive

      if (@@error <> 0)
        goto undo

      set @internalId = scope_identity()
      set @id = dbo.GetExternalId(@internalId)

      if (@@error <> 0)
        goto undo
    end 
    else 
    begin -- update существующей новости

      update news
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , EditorIp = @editorIp
        , Date = @date
        , [Name] = @name
        , Description = @description
        , [Text] = @text
        , IsActive = @isActive
      from
        dbo.News news with (rowlock)
      where
        news.[Id] = @internalId

      if (@@error <> 0)
        goto undo
    end 

  if @@trancount > 0
    commit tran insert_update_news_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  if @public = 1
    exec dbo.RegisterEvent 
      @accountId = @editorAccountId
      , @ip = @editorIp
      , @eventCode = @publicEventCode
      , @sourceEntityIds = @ids
      , @eventParams = null
      , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_news_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrantRenunciation]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 перед запуском ХП должна быть создана временнная таблица
  create table #EntrantRenunciation
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    )
*/
-- exec dbo.UpdateEntrantRenunciation
-- =======================================================
-- Добавляет данные в dbo.EntrantRenunciation.
-- v.1.0: Created by Makarev Andrey 26.06.2008
-- v.1.1: Modified by Fomin Dmitriy 27.06.2008
-- Поле Year - текущий год.
-- ========================================================
alter procedure [dbo].[UpdateEntrantRenunciation]
  @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @organizationId bigint
    , @year int

  set @updateId = NewId()
  set @currentDate = GetDate()
  set @eventCode = 'ENT_REN_EDIT'
  set @year = Year(GetDate())
  
  select
    @accountId = account.[Id]
    , @organizationId = account.OrganizationId
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if not exists(select 1 
      from dbo.Organization organization
      where organization.Id = @organizationId)
    return 0 

  declare @EntrantRenunciation table
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    , EntrantRenunciationId bigint
    )

  insert @EntrantRenunciation
  select 
    entrant_renunciation.LastName 
    , entrant_renunciation.FirstName 
    , entrant_renunciation.PatronymicName
    , entrant_renunciation.PassportNumber
    , entrant_renunciation.PassportSeria
    , old_entrant_renunciation.Id
  from (select distinct
      isnull(entrant_renunciation.LastName, '') LastName 
      , isnull(entrant_renunciation.FirstName, '') FirstName 
      , isnull(entrant_renunciation.PatronymicName, '') PatronymicName
      , isnull(entrant_renunciation.PassportNumber, '') PassportNumber
      , isnull(entrant_renunciation.PassportSeria, '') PassportSeria
    from #EntrantRenunciation entrant_renunciation) entrant_renunciation
    left outer join dbo.EntrantRenunciation old_entrant_renunciation
      on old_entrant_renunciation.[Year] = @year
        and old_entrant_renunciation.OwnerOrganizationId = @organizationId
        and old_entrant_renunciation.PassportNumber = entrant_renunciation.PassportNumber
        and old_entrant_renunciation.PassportSeria = entrant_renunciation.PassportSeria

  begin tran update_entrant_renunciation_tran

    insert dbo.EntrantRenunciation
      (
      CreateDate
      , UpdateDate
      , UpdateId
      , EditorAccountId
      , EditorIp
      , OwnerOrganizationId
      , [Year]
      , LastName
      , FirstName
      , PatronymicName
      , PassportNumber
      , PassportSeria
      )
    select
      @currentDate
      , @currentDate
      , @updateId
      , @accountId
      , @ip
      , @organizationId
      , @year
      , entrant_renunciation.LastName
      , entrant_renunciation.FirstName
      , entrant_renunciation.PatronymicName
      , entrant_renunciation.PassportNumber
      , entrant_renunciation.PassportSeria
    from
      @EntrantRenunciation entrant_renunciation
    where
      entrant_renunciation.EntrantRenunciationId is null

    if (@@error <> 0)
      goto undo

    update old_entrant_renunciation
    set
      UpdateDate = @currentDate
      , UpdateId = @updateId
      , EditorAccountId = @accountId
      , EditorIp = @ip
      , LastName = entrant_renunciation.LastName
      , FirstName = entrant_renunciation.FirstName
      , PatronymicName = entrant_renunciation.PatronymicName
    from
      dbo.EntrantRenunciation old_entrant_renunciation
        inner join @EntrantRenunciation entrant_renunciation
          on entrant_renunciation.EntrantRenunciationId = old_entrant_renunciation.Id
            and (old_entrant_renunciation.LastName <> entrant_renunciation.LastName
              or old_entrant_renunciation.FirstName <> entrant_renunciation.FirstName
              or old_entrant_renunciation.PatronymicName <> entrant_renunciation.PatronymicName)
    
    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran update_entrant_renunciation_tran

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran update_entrant_renunciation_tran

  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrantCheckBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateEntrantCheckBatch
-- =========================================
-- Сохранение пакета в БД
-- v.1.0: Created by Sedov Anton 08.07.2008
-- =========================================
alter procedure [dbo].[UpdateEntrantCheckBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N'ENT_BCH_CHK'

  begin tran insert_check_batch_tran

    insert dbo.EntrantCheckBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_check_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_check_batch_tran

  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrant]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateEntrant

/*
Перед запуском ХП должна быть создана временнная таблица
  create table #Entrant 
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , CertificateNumber nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    , GIFOCategoryName nvarchar(255)
    , DirectionCode nvarchar(255)
    , SpecialtyCode nvarchar(255)
    )
*/
-- =======================================================
-- Добавляет данные в dbo.Entrant.
-- v.1.0: Created by Makarev Andrey 26.06.2008
-- v.1.1: Modified by Fomin Dmitriy 27.06.2008
-- Поле Year - текущий год.
-- ========================================================
alter procedure [dbo].[UpdateEntrant]
  @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @organizationId bigint
    , @year int

  set @updateId = NewId()
  set @currentDate = GetDate()
  set @eventCode = 'ENT_EDIT'
  set @year = Year(GetDate())
  
  select
    @accountId = account.[Id]
    , @organizationId = account.OrganizationId
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if not exists(select 1 
      from dbo.Organization organization
      where organization.Id = @organizationId)
    return 0 

  declare @Entrant table
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , CertificateNumber nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    , GIFOCategoryName nvarchar(255)
    , DirectionCode nvarchar(255)
    , SpecialtyCode nvarchar(255)
    , EntrantId bigint
    )

  insert @Entrant
  select 
    entrant.LastName
    , entrant.FirstName
    , entrant.PatronymicName
    , entrant.CertificateNumber
    , entrant.PassportNumber
    , entrant.PassportSeria
    , entrant.GIFOCategoryName
    , entrant.DirectionCode
    , entrant.SpecialtyCode
    , old_entrant.Id
  from
    (select distinct
      isnull(entrant.LastName, '') LastName
      , isnull(entrant.FirstName, '') FirstName
      , isnull(entrant.PatronymicName, '') PatronymicName
      , entrant.CertificateNumber CertificateNumber
      , entrant.PassportNumber PassportNumber
      , isnull(entrant.PassportSeria, '') PassportSeria
      , entrant.GIFOCategoryName GIFOCategoryName
      , isnull(entrant.DirectionCode, '') DirectionCode
      , isnull(entrant.SpecialtyCode, '') SpecialtyCode
    from #Entrant entrant) entrant
      left outer join dbo.Entrant old_entrant
        on old_entrant.[Year] = @year
          and old_entrant.OwnerOrganizationId = @organizationId
          and (old_entrant.CertificateNumber = entrant.CertificateNumber
            or (old_entrant.CertificateNumber is null
              and entrant.CertificateNumber is null
              and old_entrant.PassportNumber = entrant.PassportNumber
              and old_entrant.PassportSeria = entrant.PassportSeria))

  begin tran update_entrant_tran

    insert dbo.Entrant
      (
      CreateDate
      , UpdateDate
      , UpdateId
      , EditorAccountId
      , EditorIp
      , OwnerOrganizationId
      , [Year]
      , LastName
      , FirstName
      , PatronymicName
      , CertificateNumber
      , PassportNumber
      , PassportSeria
      , GIFOCategoryName
      , DirectionCode
      , SpecialtyCode
      )
    select
      @currentDate
      , @currentDate
      , @updateId
      , @accountId
      , @ip
      , @organizationId
      , @year
      , entrant.LastName
      , entrant.FirstName
      , entrant.PatronymicName
      , entrant.CertificateNumber
      , entrant.PassportNumber
      , entrant.PassportSeria
      , entrant.GIFOCategoryName
      , entrant.DirectionCode
      , entrant.SpecialtyCode
    from
      @Entrant entrant
    where
      entrant.EntrantId is null

    if (@@error <> 0)
      goto undo

    update old_entrant
    set
      UpdateDate = @currentDate
      , UpdateId = @updateId
      , EditorAccountId = @accountId
      , EditorIp = @ip
      , LastName = entrant.LastName
      , FirstName = entrant.FirstName
      , PatronymicName = entrant.PatronymicName
      , PassportNumber = entrant.PassportNumber
      , PassportSeria = entrant.PassportSeria
      , GIFOCategoryName = entrant.GIFOCategoryName
      , DirectionCode = entrant.DirectionCode
      , SpecialtyCode = entrant.SpecialtyCode
    from
      dbo.Entrant old_entrant
        inner join @Entrant entrant
          on entrant.EntrantId = old_entrant.Id
            and (old_entrant.PassportNumber <> entrant.PassportNumber
                or old_entrant.PassportSeria <> entrant.PassportSeria
                or old_entrant.LastName <> entrant.LastName
                or old_entrant.FirstName <> entrant.FirstName
                or old_entrant.PatronymicName <> entrant.PatronymicName
                or isnull(old_entrant.GIFOCategoryName, '') <> isnull(entrant.GIFOCategoryName, '')
                or old_entrant.DirectionCode <> entrant.DirectionCode
                or old_entrant.SpecialtyCode <> entrant.SpecialtyCode)
    
    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran update_entrant_tran

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran update_entrant_tran

  return 1
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateDocument]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateDocument

-- =============================================
-- Сохранение изменений документа.
-- v.1.0: Created by Makarev Andrey 18.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Передаются не ИД контекстов, а коды.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
alter proc [dbo].[UpdateDocument]
  @id bigint output
  , @name nvarchar(255)
  , @description ntext
  , @content image
  , @contentSize int
  , @contentType nvarchar(255)
  , @isActive bit
  , @contextCodes nvarchar(4000)
  , @relativeUrl nvarchar(255)
  , @editorLogin nvarchar(255) = null
  , @editorIp nvarchar(255) = null
as
begin
  declare 
    @newDocument bit
    , @currentDate datetime
    , @editorAccountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @activateDate datetime
    , @oldIsActive bit
    , @public bit
    , @publicEventCode nvarchar(100)
    , @internalId bigint

  set @updateId = newid()
  
  declare @oldContextId table 
    (
    ContextId int
    )

  declare @newContextId table 
    (
    ContextId int
    )

  insert @newContextId
  select 
    context.Id
  from 
    dbo.GetDelimitedValues(@contextCodes) codes
      inner join dbo.Context context
        on context.Code = codes.[value]

  set @currentDate = getdate()

  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  if isnull(@id, 0) = 0 -- новый документ
  begin
    set @newDocument = 1
    set @eventCode = N'DOC_CREATE'
    if @isActive = 1
      set @activateDate = @currentDate
  end
  else
  begin -- update существующего документа
    set @internalId = dbo.GetInternalId(@id)

    select 
      @oldIsActive = [document].IsActive
    from 
      dbo.[Document] [document] with (nolock, fastfirstrow)
    where
      [document].[Id] = @internalId

    insert @oldContextId
      (
      ContextId
      )
    select
      document_context.ContextId
    from
      dbo.DocumentContext document_context with (nolock)
    where
      document_context.DocumentId = @internalId

    set @eventCode = N'DOC_EDIT'

    if @oldIsActive <> @isActive
    begin
      set @public = 1
      if @isActive = 1
        select 
          @publicEventCode = N'DOC_PUBLIC'
          , @activateDate = @currentDate
      else
        select 
          @publicEventCode = N'DOC_UNPUBLIC'
          , @activateDate = null
    end
  end

  begin tran insert_update_document_tran

    if @newDocument = 1 -- новый документ
    begin
      insert dbo.[Document]
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Name]
        , Description
        , [Content]
        , ContentSize
        , ContentType
        , IsActive
        , ActivateDate
        , ContextCodes
        , RelativeUrl
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @name
        , @description
        , @content
        , @contentSize
        , @contentType
        , @isActive
        , @activateDate
        , @contextCodes
        , @relativeUrl

      if (@@error <> 0)
        goto undo

      set @internalId = scope_identity()
      set @id = dbo.GetExternalId(@internalId)

      if (@@error <> 0)
        goto undo

      insert dbo.DocumentContext
        (
        DocumentId
        , ContextId
        )
      select
        @internalId
        , new_context_id.ContextId
      from 
        @newContextId new_context_id

      if (@@error <> 0)
        goto undo

    end 
    else 
    begin -- update существующего документа

      update [document]
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , EditorIp = @editorIp
        , [Name] = @name
        , Description = @description
        , [Content] = @content
        , ContentSize = @contentSize
        , ContentType = @contentType
        , IsActive = @isActive
        , ActivateDate = case
            when @public = 1 then @activateDate
            else [document].ActivateDate
        end
        , ContextCodes = @contextCodes
        , RelativeUrl = @relativeUrl
      from
        dbo.[Document] [document] with (rowlock)
      where
        [document].[Id] = @internalId

      if (@@error <> 0)
        goto undo

      if exists(select 
            1
          from
            @oldContextId old_context_id
              full outer join @newContextId new_context_id
                on old_context_id.ContextId = new_context_id.ContextId
          where
            old_context_id.ContextId is null
            or new_context_id.ContextId is null) 
      begin
        delete document_context
        from 
          dbo.DocumentContext document_context
        where
          document_context.DocumentId = @internalId

        if (@@error <> 0)
          goto undo

        insert dbo.DocumentContext
          (
          DocumentId
          , ContextId
          )
        select
          @internalId
          , new_context_id.ContextId
        from 
          @newContextId new_context_id

        if (@@error <> 0)
          goto undo
      end
    end 

  if @@trancount > 0
    commit tran insert_update_document_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  if @public = 1
    exec dbo.RegisterEvent 
      @accountId = @editorAccountId
      , @ip = @editorIp
      , @eventCode = @publicEventCode
      , @sourceEntityIds = @ids
      , @eventParams = null
      , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_document_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateDelivery]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Создание или редактирование рассылки.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
alter proc [dbo].[UpdateDelivery]
  @id bigint output
  , @title nvarchar(255)
  , @message nvarchar(4000)
  , @deliveryDate datetime
  , @deliveryType nvarchar(20)
  , @recipientIds nvarchar(max)
  , @editorLogin nvarchar(255) 
  , @editorIp nvarchar(255)
as
begin
  declare @eventCode nvarchar(100)
  
  if isnull(@id, 0) = 0 -- новая рассылка
  begin
    insert dbo.Delivery
      (
      Title
      , [Message]
      , DeliveryDate
      , TypeCode
      )
    select
      @title
      , @message
      , @deliveryDate
      , @deliveryType

    set @id = scope_identity()
    set @eventCode= N'DLV_CREATE'
  end 
  else 
  begin -- update существующей рассылки
    update delivery
    set
      Title = @title
      , [Message] = @message
      , DeliveryDate = @deliveryDate
      , TypeCode = @deliveryType
    from
      dbo.Delivery delivery with (rowlock)
    where
      delivery.[Id] = @id
    
    set @eventCode= N'DLV_EDIT'
  end 

  --Удалим старых получателей рассылки
  delete from dbo.DeliveryRecipients where DeliveryId = @id
  
  if (@recipientIds is not null)
  begin
    --[value] - recipientCode, @internalId - Id рассылки
    insert into dbo.DeliveryRecipients select [value],@id from dbo.GetDelimitedValues(@recipientIds)
  end

  declare @editorAccountId bigint
  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  declare @updateId uniqueidentifier
  set @updateId = newid()

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @id
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCompetitionCertificateRequestBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateCompetitionCertificateRequestBatch
-- =====================================================
-- Сохранение пакета в БД
-- v.1.0: Created by Sedov Anton 30.07.2008
-- v.1.1: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- ======================================================
alter procedure [dbo].[UpdateCompetitionCertificateRequestBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N'SCC_BCH_CHK'

  begin tran insert_request_batch_tran

    insert dbo.CompetitionCertificateRequestBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_request_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_request_batch_tran

  return 1 
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateRequestBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
alter proc [dbo].[UpdateCommonNationalExamCertificateRequestBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
  , @filter nvarchar(255)
  , @IsTypographicNumber bit
  , @year nvarchar(10)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N'CNE_BCH_FND'

  begin tran insert_request_batch_tran

    insert dbo.CommonNationalExamCertificateRequestBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      , [Filter]
      , IsTypographicNumber
      , [Year]
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch
      , @filter
      , @IsTypographicNumber
      , @year

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_request_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_request_batch_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateCheckBatch]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch

-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
alter proc [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
  , @filter nvarchar(255)
  , @type int=0
  , @outerId bigint=null
  , @year nvarchar(10)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N'CNE_BCH_CHK'

  begin tran insert_check_batch_tran

    insert dbo.CommonNationalExamCertificateCheckBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      , [Filter]
      ,[Type]
      ,outerId
      , [Year]
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch
      , @filter
      ,@type
      ,@outerId
      , @year
      
    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_check_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_check_batch_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateAskedQuestion]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateAskedQuestion

-- =============================================
-- Сохранение изменений вопроса.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
alter proc [dbo].[UpdateAskedQuestion]
  @id bigint output
  , @name nvarchar(255)
  , @question ntext
  , @answer ntext
  , @isActive bit
  , @contextCodes nvarchar(4000)
  , @editorLogin nvarchar(255) = null
  , @editorIp nvarchar(255) = null
as
begin
  declare 
    @newAskedQuestion bit
    , @currentDate datetime
    , @editorAccountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @oldIsActive bit
    , @public bit
    , @publicEventCode nvarchar(100)
    , @internalId bigint

  set @updateId = newid()

  declare @oldContextId table 
    (
    ContextId int
    )

  declare @newContextId table 
    (
    ContextId int
    )

  insert @newContextId
  select 
    context.Id
  from 
    dbo.GetDelimitedValues(@contextCodes) codes
      inner join dbo.Context context
        on context.Code = codes.[value]

  set @currentDate = getdate()

  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  if isnull(@id, 0) = 0 -- новая новость
  begin
    set @newAskedQuestion = 1
    set @eventCode = N'FAQ_CREATE'
  end
  else
  begin -- update существующей новости
    set @internalId = dbo.GetInternalId(@id)

    select 
      @oldIsActive = asked_question.IsActive
    from 
      dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
    where
      asked_question.[Id] = @internalId

    insert @oldContextId
      (
      ContextId
      )
    select
      asked_question_context.ContextId
    from
      dbo.AskedQuestionContext asked_question_context with (nolock)
    where
      asked_question_context.AskedQuestionId = @internalId

    set @eventCode = N'FAQ_EDIT'

    if @oldIsActive <> @isActive
    begin
      set @public = 1
      if @isActive = 1
        set @publicEventCode = N'FAQ_PUBLIC'
      else
        set @publicEventCode = N'FAQ_UNPUBLIC'
    end
  end

  begin tran insert_update_faq_tran

    if @newAskedQuestion = 1 -- новый документ
    begin
      insert dbo.AskedQuestion
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Name]
        , Question
        , Answer
        , IsActive
        , ViewCount
        , Popularity
        , ContextCodes
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @name
        , @question
        , @answer
        , @isActive
        , 0
        , 0
        , @contextCodes

      if (@@error <> 0)
        goto undo

      set @internalId = scope_identity()
      set @id = dbo.GetExternalId(@internalId)

      if (@@error <> 0)
        goto undo

      insert dbo.AskedQuestionContext
        (
        AskedQuestionId
        , ContextId
        )
      select
        @internalId
        , new_context_id.ContextId
      from 
        @newContextId new_context_id

      if (@@error <> 0)
        goto undo

    end 
    else 
    begin -- update существующего вопроса

      update asked_question
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , EditorIp = @editorIp
        , [Name] = @name
        , Question = @question
        , Answer = @answer
        , IsActive = @isActive
        , ContextCodes = @contextCodes
      from
        dbo.AskedQuestion asked_question with (rowlock)
      where
        asked_question.[Id] = @internalId

      if (@@error <> 0)
        goto undo

      if exists(select 
            1
          from
            @oldContextId old_context_id
              full outer join @newContextId new_context_id
                on old_context_id.ContextId = new_context_id.ContextId
          where
            old_context_id.ContextId is null
            or new_context_id.ContextId is null) 
      begin
        delete asked_question_context
        from 
          dbo.AskedQuestionContext asked_question_context
        where
          asked_question_context.AskedQuestionId = @internalId

        if (@@error <> 0)
          goto undo

        insert dbo.AskedQuestionContext
          (
          AskedQuestionId
          , ContextId
          )
        select
          @internalId
          , new_context_id.ContextId
        from 
          @newContextId new_context_id

        if (@@error <> 0)
          goto undo
      end
    end 

  if @@trancount > 0
    commit tran insert_update_faq_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  if @public = 1
    exec dbo.RegisterEvent 
      @accountId = @editorAccountId
      , @ip = @editorIp
      , @eventCode = @publicEventCode
      , @sourceEntityIds = @ids
      , @eventParams = null
      , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_faq_tran

  return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountPassword]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateAccountPassword

-- =============================================
-- Сохранить пароль пользователя.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 04.05.2008
-- Добавлен параметр password для обратной совместимости систем.
-- =============================================
alter proc [dbo].[UpdateAccountPassword]
  @login nvarchar(255)
  , @passwordHash nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
  , @password nvarchar(255) = null -- !временно
as
begin

  declare
    @editorAccountId bigint
    , @accountId bigint
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()

  select 
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  update account
  set
    PasswordHash = @passwordHash
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , UpdateDate = GetDate()
    , UpdateId = @updateId
  from
    dbo.Account account with (rowlock)
  where
    account.[Id] = @accountId

-- временно
  if isnull(@password, '') <> '' and N'User' = (select 
            [group].[code]
          from
            dbo.[Group] [group]
              inner join dbo.GroupAccount group_account
                on [group].[Id] = group_account.GroupId
          where
            group_account.AccountId = @accountId)
  begin
    if exists(select 
          1
        from
          dbo.UserAccountPassword user_account_password
        where
          user_account_password.AccountId = @accountId)
    begin
      update user_account_password
      set
        [Password] = @password
      from
        dbo.UserAccountPassword user_account_password
      where
        user_account_password.AccountId = @accountId
    end
    else
    begin
      insert dbo.UserAccountPassword
        (
        AccountId
        , [Password]
        )
      select 
        @accountId
        , @password
    end
  end

  exec dbo.RefreshRoleActivity @accountId = @accountId

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = N'USR_PASSW'
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountKey]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 01.09.2008
-- ====================================================
alter procedure [dbo].[UpdateAccountKey]
  @login nvarchar(255)
  , @key nvarchar(255)
  , @dateFrom datetime
  , @dateTo datetime
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare
    @accountId bigint
    , @editorAccountId bigint
    , @eventCode nvarchar(255)
    , @keyId bigint
    , @updateId uniqueidentifier
    , @keyIds nvarchar(255)

  set @updateId = newid()
  
  select @accountId = account.Id
  from dbo.Account account
  where account.[Login] = @login

  select @editorAccountId = account.Id
  from dbo.Account account
  where account.[Login] = @editorLogin

  select @keyId = account_key.Id
  from dbo.AccountKey account_key
  where account_key.[Key] = @key
    and account_key.AccountId = @accountId

  if @keyId is null
  begin
    insert into dbo.AccountKey
      (
      CreateDate
      , UpdateDate
      , UpdateId
      , EditorAccountId
      , EditorIp
      , AccountId
      , [Key]
      , DateFrom
      , DateTo
      , IsActive
      )
    select
      GetDate()
      , GetDate()
      , @updateId
      , @editorAccountId
      , @editorip
      , @accountId
      , @key
      , @dateFrom
      , @dateTo
      , @isActive

    set @keyId = scope_identity()

    set @eventCode = 'USR_KEY_CREATE'
  end
  else
  begin
    update account_key
    set
      UpdateDate = GetDate()
      , UpdateId = @updateId
      , EditorAccountId = @editorAccountId
      , EditorIp = @editorIp
      , DateFrom = @dateFrom
      , DateTo = @dateTo
      , IsActive = @isActive
    from dbo.AccountKey account_key
    where account_key.Id = @keyId

    set @eventCode = 'USR_KEY_EDIT'
  end

  set @keyIds = @keyId

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @keyIds
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SetActiveNews]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SetActiveNews

-- =============================================
-- Установка активности новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
alter proc [dbo].[SetActiveNews]
  @ids nvarchar(255)
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @currentDate datetime
    , @innerIds nvarchar(4000)

  set @innerIds = ''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()
  set @currentDate = getdate()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  if @isActive = 1
    set @eventCode = N'NWS_PUBLIC'
  else
    set @eventCode = N'NWS_UNPUBLIC'

  update news
  set
    UpdateDate = @currentDate
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , IsActive = @isActive
  from 
    dbo.News news with (rowlock)
      inner join @idTable idTable
        on news.[id] = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SetActiveDocument]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SetActiveDocument

-- =============================================
-- Установка активности документа.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.2: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.3: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
alter proc [dbo].[SetActiveDocument]
  @ids nvarchar(255)
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @activateDate datetime
    , @currentDate datetime
    , @innerIds nvarchar(4000)

  set @innerIds = ''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()
  set @currentDate = getdate()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  if @isActive = 1
    select 
      @eventCode = N'DOC_PUBLIC'
      , @activateDate = @currentDate
  else
    select
      @eventCode = N'DOC_UNPUBLIC'
      , @activateDate = null

  update [document]
  set
    UpdateDate = @currentDate
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , IsActive = @isActive
    , ActivateDate = @activateDate
  from 
    dbo.[Document] [document] with (rowlock)
      inner join @idTable idTable
        on [document].[id] = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SetActiveAskedQuestion]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SetActiveAskedQuestion

-- =============================================
-- Установка активности вопроса.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
alter proc [dbo].[SetActiveAskedQuestion]
  @ids nvarchar(255)
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @currentDate datetime
    , @innerIds nvarchar(4000)

  set @innerIds = ''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()
  set @currentDate = getdate()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  if @isActive = 1
    set @eventCode = N'FAQ_PUBLIC'
  else
    set @eventCode = N'FAQ_UNPUBLIC'

  update asked_question
  set
    UpdateDate = @currentDate
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , IsActive = @isActive
  from 
    dbo.AskedQuestion asked_question with (rowlock)
      inner join @idTable idTable
        on asked_question.[id] = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportUserStatusWithAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [ ] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @Statuses TABLE
(
  [Name] NVARCHAR (50),
  Code NVARCHAR (50),
  [Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
  [Name] NVARCHAR (50),
  Code BIT,
  [Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
  OrgTypeName NVARCHAR (50),
  OrgTypeCode INT,
  IsPrivateName NVARCHAR (50),
  IsPrivateCode INT,
  IsPrivateOrder INT,
  StatusName NVARCHAR(50),
  StatusCode NVARCHAR(50),
  StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
  OrgTypeName NVARCHAR (50),
  OrgTypeCode NVARCHAR (50),
  IsPrivateName NVARCHAR (50),
  IsPrivateOrder NVARCHAR (50),
  StatusName NVARCHAR(50),
  UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
ON Acc.OrganizationId=OReq.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
  AND (
    OReq.TypeId=Comb.OrgTypeCode 
    AND (
      (OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
      OR
      (Comb.IsPrivateCode IS NULL)
    )
    AND Comb.OrgTypeCode!=10
  ))
OR (
  (Acc.Status=Comb.StatusCode)
  AND (
    Comb.OrgTypeCode=10
  )
  AND (
    OReq.TypeId IS NOT NULL
  )
)
OR (
  Comb.StatusCode='total'
  AND ((
    OReq.TypeId=Comb.OrgTypeCode 
    AND (
      (OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
      OR
      (Comb.IsPrivateCode IS NULL)
    )
    AND Comb.OrgTypeCode!=10
  )
  OR
  ((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
( 
  MainOrder INT,
  OrgTypeName NVARCHAR (50),
  IsPrivateName NVARCHAR (50),
  [Всего] INT,
  [Активирован] INT,
  [На регистрации] INT,
  [На доработке] INT,
  [На согласовании] INT,
  [Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
    case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
    case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
    case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
    case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF (@periodBegin ,@periodEnd)

return
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificatePassport]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter proc [dbo].[SearchCommonNationalExamCertificatePassport]
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
    'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year, ParticipantID 
    from 
    (
      select b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
            from rbd.Participants a with (nolock)
        left join prn.Certificates b with (nolock) on b.ParticipantFK = a.ParticipantID
        left join prn.CancelledCertificates c on c.CertificateFK=b.CertificateID
    ) certificate
    where 
    certificate.[Year] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and certificate.LastName collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and certificate.FirstName collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and certificate.PatronymicName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.PassportSeria = @internalPassportSeria '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber '
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
        when isnull(CD.UseYear,0) <> 0 then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when isnull(CD.UseYear,0) <> 0 then 'Аннулировано' 
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
      isnull(C.ParticipantID, S.ParticipantID) ParticipantID
    from 
      @Search S 
            left join dbo.vw_Examcertificate C with (nolock) 
              on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.idGUID  = C.Id 
      left outer join prn.CancelledCertificates CD with (nolock) 
        on CD.[UseYear] between @yearFrom and @yearTo 
                and S.CertificateId = CD.CertificateFK
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed
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
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistrationShortTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportRegistrationShortTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
( 
[Правовая форма] NVARCHAR(255) NULL
,[Зарегистрировано] INT null
,[Не зарегистрировано] INT null
,[Всего] INT null
)
AS 
BEGIN

 
DECLARE @RegistredOrgsPrivCount INT
SELECT @RegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ'
DECLARE @NotRegistredOrgsPrivCount INT
SELECT @NotRegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ'
DECLARE @RegistredOrgsStateCount INT
SELECT @RegistredOrgsStateCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ'
DECLARE @NotRegistredOrgsStateCount INT
SELECT @NotRegistredOrgsStateCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ'


DECLARE @RegistredOrgsPrivAccredCount INT
SELECT @RegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @NotRegistredOrgsPrivAccredCount INT
SELECT @NotRegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @RegistredOrgsStateAccredCount INT
SELECT @RegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @NotRegistredOrgsStateAccredCount INT
SELECT @NotRegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'

INSERT INTO @Report
SELECT 'Государственный',@RegistredOrgsStateCount,@NotRegistredOrgsStateCount,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount
INSERT INTO @Report
SELECT 'Негосударственный',@RegistredOrgsPrivCount,@NotRegistredOrgsPrivCount,@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount
INSERT INTO @Report
SELECT 'Итого'
,@RegistredOrgsStateCount+@RegistredOrgsPrivCount
,@NotRegistredOrgsStateCount+@NotRegistredOrgsPrivCount
,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount+@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount


INSERT INTO @Report
SELECT '',null,null,null
INSERT INTO @Report
SELECT 'Аккредитованных',null,null,null


INSERT INTO @Report
SELECT 'Государственный',@RegistredOrgsStateAccredCount,@NotRegistredOrgsStateAccredCount,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount
INSERT INTO @Report
SELECT 'Негосударственный',@RegistredOrgsPrivAccredCount,@NotRegistredOrgsPrivAccredCount,@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount
INSERT INTO @Report
SELECT 'Итого'
,@RegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount
,@NotRegistredOrgsStateAccredCount+@NotRegistredOrgsPrivAccredCount
,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCommonStatisticsTVF]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportCommonStatisticsTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Всего свидетельств] INT
,[Зарегистрировано пользователей] INT
,[Проверок всего] INT
,[Уникальных проверок] INT
,[Уникальных пакетных проверок] INT
,[Уникальных интерактивных проверок] INT
)
AS 
BEGIN

DECLARE @CNEsCount INT
SELECT @CNEsCount=COUNT(*) 
FROM CommonNationalExamCertificate

DECLARE @UsersCount INT
SELECT @UsersCount=COUNT(*) 
FROM Account Acc
INNER JOIN GroupAccount GA ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId AND OReq.OrganizationId IS NOT NULL

DECLARE @TotalChecks INT
DECLARE @TotalUniqueChecks INT

SELECT @TotalChecks=SUM([всего])
,@TotalUniqueChecks=SUM([всего уникальных проверок])
FROM ReportTotalChecksTVF(null,null)


DECLARE @UniqueChecks_Batch INT

SELECT @UniqueChecks_Batch=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]='Пакетная'


DECLARE @UniqueChecks_UI INT

SELECT @UniqueChecks_UI=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]='Интерактивная'


INSERT INTO @Report
SELECT @CNEsCount,@UsersCount,@TotalChecks,@TotalUniqueChecks,@UniqueChecks_Batch,@UniqueChecks_UI

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsAggregatedTVF]    Script Date: 06/13/2013 18:35:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportCheckedCNEsAggregatedTVF](
  @periodBegin DATETIME = NULL
  , @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Количество свидетельств] INT
,[Проверок в различных ОУ] INT
)
AS 
BEGIN


INSERT INTO @Report ([Количество свидетельств],[Проверок в различных ОУ])
SELECT COUNT(IAggrChecks.CNEId) AS CNECount,IAggrChecks.OrgCount AS OrgCount FROM 
(
  SELECT 
  CNEId AS CNEId
  ,COUNT(OrgId) AS OrgCount
  FROM [ReportCheckedCNEsBASE]() AS IChecks
  GROUP BY IChecks.CNEId HAVING COUNT(IChecks.OrgId)>=1
) AS IAggrChecks
GROUP BY IAggrChecks.OrgCount 
ORDER BY IAggrChecks.OrgCount 

RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificate]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchCompetitionCertificate
-- =============================================
-- Процедура  поиска олимпиадников.
-- Created by Sedov Anton 10.07.2008
-- Modified by Fomin Dmitriy 23.07.2008
-- Поле SubjectId заменено CompetitionTypeId.
-- Тип олимпиады необязателен.
-- =============================================
alter procedure [dbo].[SearchCompetitionCertificate]
  @competitionTypeId int = null
  , @lastName nvarchar(255)
  , @firstName nvarchar(255)
  , @patronymicName nvarchar(255)
  , @regionId int = null
  , @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare 
    @year int 
  
  set @year = Year(GetDate())

  select
    searching_competition_certificate.CompetitionTypeId CompetitionTypeId
    , competition_type.[Name] CompetitionTypeName
    , searching_competition_certificate.LastName LastName
    , searching_competition_certificate.FirstName FirstName
    , searching_competition_certificate.PatronymicName PatronymicName
    , competition_certificate.Degree Degree
    , isnull(region.[Name], searching_region.[Name]) RegionName
    , competition_certificate.City City
    , competition_certificate.School School
    , competition_certificate.Class Class
    , case 
      when competition_certificate.Id is null then 0
      else 1
    end IsExist 
  from
    (select
      @competitionTypeId CompetitionTypeId
      , @lastName LastName
      , @firstName FirstName
      , @patronymicName PatronymicName
      , @regionId RegionId) as searching_competition_certificate
      left join dbo.CompetitionCertificate competition_certificate
        left join dbo.CompetitionType competition_type
          on competition_certificate.CompetitionTypeId = competition_type.Id
        left join dbo.Region region
          on competition_certificate.RegionId = region.Id
        on searching_competition_certificate.LastName = competition_certificate.LastName
          and searching_competition_certificate.FirstName = competition_certificate.FirstName
          and searching_competition_certificate.PatronymicName = competition_certificate.PatronymicName
          and competition_certificate.[Year] = @year
          -- Лучше использовать диманический SQL, но здесь не критично.
          and (searching_competition_certificate.CompetitionTypeId = competition_certificate.CompetitionTypeId
            or searching_competition_certificate.CompetitionTypeId is null) 
          and (searching_competition_certificate.RegionId = competition_certificate.RegionId
            or searching_competition_certificate.RegionId is null) 
      left join dbo.Region searching_region
        on searching_competition_certificate.RegionId = region.Id

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
  
  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login 

  set @eventCode = 'SCC_FND'
  set @updateId = NewId()     
  
  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId
        
  return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateWildcard]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
        row_number() over (order by c.year, c.id) as row
      , c.LastName 
      , c.FirstName 
      , c.PatronymicName 
      , c.Id 
      , c.Number 
      , c.RegionId 
      , isnull(c.PassportSeria, @internalPassportSeria) PassportSeria 
      , isnull(c.PassportNumber, @passportNumber) PassportNumber
      , c.TypographicNumber 
      , c.Year 
      , c.ParticipantID
    '
  if @showCount = 1
    set @commandText = ' select count(*) '
  
  set @commandText = @commandText + 
    '
    from (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
            b.TypographicNumber, a.ParticipantID, b.CreateDate
        FROM rbd.Participants a with (nolock, fastfirstrow) 
          left JOIN prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) c  
    where 
      c.[Year] between @yearFrom and @yearTo '
  
  if @lastName is not null 
    set @commandText = @commandText + '
      and c.LastName collate cyrillic_general_ci_ai = @lastName'
  if @firstName is not null 
    set @commandText = @commandText + '     
      and c.FirstName collate cyrillic_general_ci_ai = @firstName'
  if @patronymicName is not null 
    set @commandText = @commandText + '           
      and c.PatronymicName collate cyrillic_general_ci_ai = @patronymicName'
  if @internalPassportSeria is not null 
    set @commandText = @commandText + '                 
      and c.InternalPassportSeria = @internalPassportSeria'
  if @passportNumber is not null 
    set @commandText = @commandText + '                       
      and c.PassportNumber = @passportNumber'
  if @typographicNumber is not null 
    set @commandText = @commandText + '                             
      and c.TypographicNumber = @typographicNumber'
  if @Number is not null 
    set @commandText = @commandText + '                                   
      and c.Number = @Number '  
      
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
          when isnull(cne_certificate_deny.UseYear,0) <> 0 then 1 
          else 0 
        end IsDeny 
        , cne_certificate_deny.Reason DenyComment 
        , null NewCertificateNumber 
        , search.[Year] 
        , case when ed.[ExpireDate] is null then 'Не найдено'  
             when isnull(cne_certificate_deny.UseYear,0) <> 0 then 'Аннулировано' 
             when getdate() <= ed.[ExpireDate] then 'Действительно'
             else 'Истек срок' 
          end as [Status]
        , unique_cheks.UniqueIHEaFCheck,
        search.ParticipantFK ParticipantID
       from @Search search
        left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on unique_cheks.idGUID = search.CertificateId 
        left outer join prn.CancelledCertificates cne_certificate_deny with (nolock) 
          on cne_certificate_deny.[UseYear] between @yearFrom and @yearTo 
            and search.CertificateId = cne_certificate_deny.CertificateFK 
        left outer join dbo.Region region with (nolock) 
          on region.[Id] = search.RegionId 
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
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificate]    Script Date: 06/13/2013 18:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
    from    
      (SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
            isnull(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, isnull(b.REGION,a.REGION) AS RegionId, 
          b.TypographicNumber, a.ParticipantID, b.CreateDate
       FROM rbd.Participants a with (nolock, fastfirstrow) 
        left JOIN prn.Certificates b with (nolock, fastfirstrow) ON b.ParticipantFK = a.ParticipantID) [certificate] 
    where 
    certificate.[Year] between @yearFrom and @yearTo ' 

  if not @lastName is null 
    set @commandText = @commandText +
      ' and certificate.LastName collate cyrillic_general_ci_ai = @lastName '
  
  if not @firstName is null 
    set @commandText = @commandText +
      ' and certificate.FirstName collate cyrillic_general_ci_ai = @firstName ' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      ' and certificate.PatronymicName collate cyrillic_general_ci_ai = @patronymicName ' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportSeria like @internalPassportSeria and [certificate].id is not null '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.PassportSeria = @internalPassportSeria and [certificate].id is not null '
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, '*', '%')
          set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber and [certificate].id is not null '
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber and [certificate].id is not null '
        end
    end
  
  if not @typographicNumber is null
    set @commandText = @commandText +
      ' and certificate.TypographicNumber = @typographicNumber and [certificate].id is not null '
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      ' and 0 = 1 '

  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int '
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo

  if not @subjectMarks is null
  begin       
    delete search 
    from @Search search 
    where exists(select 1 
          from [prn].CertificatesMarks certificate_subject with(nolock) 
            inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                and search.CertificateId = certificate_subject.CertificateFK
                and inner_search.CertificateId = search.CertificateId 
            right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                and certificate_subject.Mark = subject_mark.Mark 
          where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)    
       and CertificateId is not null    
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
      isnull(cast(S.certificateId as nvarchar(250)),'Нет свидетельства' ) certificateId,
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
        when isnull(CD.UseYear,0) <> 0 then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then 'Не найдено'
                when isnull(CD.UseYear,0) <> 0 then 'Аннулировано' 
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
      isnull(C.ParticipantID, S.ParticipantID) ParticipantID
    from 
      @Search S 
            left join dbo.vw_Examcertificate C with (nolock) 
              on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.idGUID  = C.Id 
      left outer join prn.CancelledCertificates CD with (nolock) 
        on CD.[UseYear] between @yearFrom and @yearTo 
                and S.CertificateId = CD.CertificateFK 
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed
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
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF_New]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportUserStatusAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [ ] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ_Accred()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ_Accred()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего'

INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
    case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
    case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
    case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
    case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
  INNER JOIN Organization2010 Org ON 
  (
    Org.Id=OReq.OrganizationId 
    AND (
      Org.IsAccredited=1 
      OR (
        Org.AccreditationSertificate != '' 
        AND Org.AccreditationSertificate IS NOT NULL
        )
      )
  )
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTotalChecksTVF_New]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportTotalChecksTVF_New](
  @from datetime = null
  , @to datetime = null)
RETURNS @report TABLE 
(           
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
  select @from = dateadd(year, -1, getdate()), @to = getdate()


INSERT INTO @report
SELECT [Тип проверки] 
,[всего] 
,[уникальных проверок по РН] 
,[уникальных проверок по ТН] 
,[уникальных проверок по документу] 
,[уникальных проверок по ФИО и баллам] 
,[всего уникальных проверок] 
FROM(
SELECT * FROM dbo.ReportChecksByPeriodTVF(@from,@to)
UNION ALL
SELECT NULL,NULL,NULL,NULL,NULL,NULL,NULL,5
UNION ALL
SELECT * FROM dbo.ReportChecksAllTVF()
) INN ORDER BY [order]

RETURN
end
GO
/****** Object:  UserDefinedFunction [dbo].[ReportXMLSubordinateOrg]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Функция по подведомственным учреждениям для экспорта в XML
alter funCTION  [dbo].[ReportXMLSubordinateOrg](
      @periodBegin datetime,
      @periodEnd datetime,
      @departmentId int)
RETURNS @Report TABLE
(
  [Код ОУ] int null,
  [Полное наименование] nvarchar(Max) null,
  [Код региона] int null,
  [Наименование региона] nvarchar(255) null,
  [Свидетельство об аккредитации] nvarchar(255) null,
  [ФИО руководителя] nvarchar(255) null,
  [Количество пользователей] int null,
  [Дата активации пользователя] datetime null,
  [Количество уникальных проверок] int null
)
AS BEGIN
INSERT INTO @Report
SELECT
  A.Id [Код ОУ],
  A.FullName [Полное наименование],
  A.RegionId [Код региона] ,
  A.RegionName [Наименование региона],
  A.AccreditationSertificate [Свидетельство об аккредитации],
  A.DirectorFullName [ФИО руководителя],
  A.CountUser [Количество пользователей],
  A.UserUpdateDate [Дата активации],
  A.CountUniqueChecks [Уникальных проверок]
FROM
  dbo.ReportStatisticSubordinateOrg ( null, null, @departmentId) A
  inner join dbo.Organization2010 O on O.Id = A.Id
ORDER BY
  case when O.MainId is null then O.Id else O.MainId end, O.MainId, A.FullName
  
RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF_New]    Script Date: 06/13/2013 18:35:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter function [dbo].[ReportUserStatusWithAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [ ] nvarchar(500) null, 
  [Правовая форма] nvarchar(50) null, 
  [В БД] int null,
  [Всего] int null,
  [из них на регистрации] int null, 
  [из них на согласовании] int null,
  [из них на доработке] int null, 
  [из них действующие] int null,
  [из них отключенные] int null
)
AS
BEGIN
  
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)



INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_OTHER()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего' OR [ ]='РЦОИ' OR [ ]='Орган управления образованием' OR [ ]='Другое'


INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
    case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
    case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
    case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
    case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
  INNER JOIN Organization2010 Org ON Org.Id=OReq.OrganizationId 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF_New (@periodBegin ,@periodEnd)

return
END
GO
