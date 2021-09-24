IF (EXISTS(
    SELECT 
        name 
    FROM 
        master.dbo.sysdatabases 
    WHERE 
        '[' + name + ']' = 'HscUnitTest'
        OR name = 'HscUnitTest'
))
    alter database HscUnitTest set single_user with rollback immediate
go

use master
go

IF (EXISTS(
    SELECT 
        name 
    FROM 
        master.dbo.sysdatabases 
    WHERE 
        '[' + name + ']' = 'HscUnitTest'
        OR name = 'HscUnitTest'
))
    drop database HscUnitTest
go

/*==============================================================*/
/* Database: Hsc                                                */
/*==============================================================*/
create database HscUnitTest
go

use HscUnitTest
go

/*==============================================================*/
/* Table: BlankRequests                                         */
/*==============================================================*/
create table BlankRequests (
   Id                   uniqueidentifier     not null,
   State                int                  not null,
   CreateDate           datetimeoffset       not null default SYSDATETIMEOFFSET(),
   OperatorUserId       int                  null,
   EsrpUserId           int                  null,
   UpdateDate           datetimeoffset       null default SYSDATETIMEOFFSET(),
   Note                 nvarchar(max)        null,
   constraint PK_BLANKREQUESTS primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('BlankRequests') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty N'MS_Description',  
   N'user', @CurrentUser, N'table', N'BlankRequests' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty N'MS_Description',  
   N'Р’С‹РіСЂСѓР·РєРё РІСѓР·РѕРІ', 
   N'user', @CurrentUser, N'table', N'BlankRequests'
go

/*==============================================================*/
/* Table: BlanksDownload                                        */
/*==============================================================*/
create table BlanksDownload (
   Id                   int                  identity,
   ParticipantId        int                  not null,
   RegionId             int                  not null,
   State                int                  not null,
   "Order"              int                  not null,
   RelativePath         nvarchar(max)        not null,
   Code                 nvarchar(255)        not null,
   ExamDate             datetime             not null,
   SubjectCode          int                  not null,
   CreateDate           datetimeoffset       not null default SYSDATETIMEOFFSET(),
   constraint PK_BLANKSDOWNLOAD primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('BlanksDownload') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty N'MS_Description',  
   N'user', @CurrentUser, N'table', N'BlanksDownload' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty N'MS_Description',  
   N'РћС‡РµСЂРµРґСЊ Р·Р°РіСЂСѓР·РѕРє Р±Р»Р°РЅРєРѕРІ СЃ СЃРµСЂРІРµСЂРѕРІ СЂРµРіРёРѕРЅРѕРІ', 
   N'user', @CurrentUser, N'table', N'BlanksDownload'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('BlanksDownload')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ Р±Р»Р°РЅРєР°',
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('BlanksDownload')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParticipantId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'ParticipantId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СѓС‡Р°СЃС‚РЅРёРєР°',
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'ParticipantId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('BlanksDownload')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RegionId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'RegionId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СЂРµРіРёРѕРЅР°',
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'RegionId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('BlanksDownload')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'State')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'State'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РЎРѕСЃС‚РѕСЏРЅРёРµ Р·Р°РіСЂСѓР·РєРё',
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'State'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('BlanksDownload')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreateDate')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'CreateDate'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'Р”Р°С‚Р° Рё РІСЂРµРјСЏ СЃРѕР·РґР°РЅРёСЏ Р·Р°РїРёСЃРё',
   N'user', @CurrentUser, N'table', N'BlanksDownload', N'column', N'CreateDate'
go

/*==============================================================*/
/* Index: Idx_BlanksDownload_RegionId                           */
/*==============================================================*/
create index Idx_BlanksDownload_RegionId on BlanksDownload (
RegionId ASC
)
include (Code,ExamDate,SubjectCode,ParticipantId)
go

/*==============================================================*/
/* Index: Idx_BlanksDownload_ParticipantId                      */
/*==============================================================*/
create index Idx_BlanksDownload_ParticipantId on BlanksDownload (
ParticipantId ASC
)
include (RegionId,State)
go

/*==============================================================*/
/* Index: Idx_BlanksDownload_Code                               */
/*==============================================================*/
create index Idx_BlanksDownload_Code on BlanksDownload (
Code ASC
)
include (ParticipantId)
go

/*==============================================================*/
/* Table: PagesCount                                            */
/*==============================================================*/
create table PagesCount (
   Id                   int                  identity,
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   ExamDate             datetime             not null,
   Barcode              nvarchar(50)         not null,
   ProjectBatchId       int                  not null,
   ProjectName          nvarchar(256)        not null,
   PageCount            int                  not null,
   constraint PK_PAGESCOUNT primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('PagesCount') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty N'MS_Description',  
   N'user', @CurrentUser, N'table', N'PagesCount' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty N'MS_Description',  
   N'Р”Р°РЅРЅС‹Рµ Рѕ РєРѕР»РёС‡РµСЃС‚РІРµ СЃС‚СЂР°РЅРёС† СЃ СЃРµСЂРІРµСЂРѕРІ', 
   N'user', @CurrentUser, N'table', N'PagesCount'
go

/*==============================================================*/
/* Table: PagesCountBulk                                        */
/*==============================================================*/
create table PagesCountBulk (
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   ExamDate             datetime             not null,
   Barcode              nvarchar(50)         not null,
   ProjectBatchId       int                  not null,
   ProjectName          nvarchar(256)        not null,
   PageCount            int                  not null
)
go

/*==============================================================*/
/* Table: ParticipantInRequest                                  */
/*==============================================================*/
create table ParticipantInRequest (
   Id                   int                  identity,
   RequestId            uniqueidentifier     not null,
   ParticipantId        int                  null,
   ParticipantName      nvarchar(max)        not null,
   IsCollision          bit                  not null,
   constraint PK_PARTICIPANTINREQUEST primary key (Id)
)
go

/*==============================================================*/
/* Index: Idx_ParticipantInRequest_RequestId_ParticipantId      */
/*==============================================================*/
create unique index Idx_ParticipantInRequest_RequestId_ParticipantId on ParticipantInRequest (
RequestId ASC,
ParticipantId ASC
)
include (ParticipantName,IsCollision)
go

/*==============================================================*/
/* Table: Participants                                          */
/*==============================================================*/
create table Participants (
   Id                   int                  identity,
   ParticipantRbdId     uniqueidentifier     not null,
   ParticipantHash      nvarchar(50)         not null,
   DocumentNumber       nvarchar(50)         not null,
   RegionId             int                  not null,
   ParticipantCode      nvarchar(50)         not null,
   ParticipantExamId    int                  null,
   constraint PK_PARTICIPANTS primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Participants') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty N'MS_Description',  
   N'user', @CurrentUser, N'table', N'Participants' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty N'MS_Description',  
   N'РЈС‡Р°СЃС‚РЅРёРєРё СЌРєР·Р°РјРµРЅРѕРІ', 
   N'user', @CurrentUser, N'table', N'Participants'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Participants')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ РІ СЃРёСЃС‚РµРјРµ РІС‹РіСЂСѓР·РєРё Р±Р»Р°РЅРєРѕРІ',
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Participants')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParticipantRbdId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'ParticipantRbdId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'Р“Р»РѕР±Р°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СѓС‡Р°СЃС‚РЅРёРєР°',
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'ParticipantRbdId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Participants')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParticipantHash')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'ParticipantHash'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РҐСЌС€ Р¤РРћ СѓС‡Р°СЃС‚РЅРёРєР°',
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'ParticipantHash'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Participants')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DocumentNumber')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'DocumentNumber'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РќРѕРјРµСЂ РґРѕРєСѓРјРµРЅС‚Р°',
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'DocumentNumber'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Participants')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RegionId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'RegionId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СЂРµРіРёРѕРЅР°',
   N'user', @CurrentUser, N'table', N'Participants', N'column', N'RegionId'
go

/*==============================================================*/
/* Index: Idx_Participants_ParticipantHash_DocumentNumber       */
/*==============================================================*/
create index Idx_Participants_ParticipantHash_DocumentNumber on Participants (
ParticipantHash ASC,
DocumentNumber ASC
)
include (ParticipantRbdId,RegionId)
go

/*==============================================================*/
/* Index: Idx_Participants_ParticipantCode_RegionId             */
/*==============================================================*/
create index Idx_Participants_ParticipantCode_RegionId on Participants (
ParticipantCode ASC,
RegionId ASC
)
go

/*==============================================================*/
/* Table: RegionServerErrors                                    */
/*==============================================================*/
create table RegionServerErrors (
   Id                   int                  identity,
   RegionId             int                  not null,
   Code                 nvarchar(255)        not null,
   ExamDate             datetime             not null,
   Error                int                  not null,
   RbdId                uniqueidentifier     null,
   constraint PK_REGIONSERVERERRORS primary key (Id)
)
go

/*==============================================================*/
/* Table: RegionServers                                         */
/*==============================================================*/
create table RegionServers (
   RegionId             int                  not null,
   Url                  nvarchar(max)        null,
   Name                 nvarchar(255)        not null,
   IsAvailable          bit                  not null,
   LastAvailabilityCheck datetimeoffset       null,
   ServerBlankCount     int                  not null,
   LastFileCheck        datetimeoffset       null,
   constraint PK_REGIONSERVERS primary key (RegionId)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('RegionServers')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RegionId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'RegionServers', N'column', N'RegionId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СЂРµРіРёРѕРЅР°',
   N'user', @CurrentUser, N'table', N'RegionServers', N'column', N'RegionId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('RegionServers')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Url')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'RegionServers', N'column', N'Url'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РЈСЂР» СЃРµСЂРІРµСЂР°',
   N'user', @CurrentUser, N'table', N'RegionServers', N'column', N'Url'
go

/*==============================================================*/
/* Table: SingleParticipantRequests                             */
/*==============================================================*/
create table SingleParticipantRequests (
   Id                   uniqueidentifier     not null,
   CreateDate           datetimeoffset       not null default SYSDATETIMEOFFSET(),
   OperatorUserId       int                  null,
   EsrpUserId           int                  null,
   IsDeleted            bit                  not null default 0,
   ParticipantId        int                  null,
   constraint PK_SINGLEPARTICIPANTREQUESTS primary key (Id)
)
go

/*==============================================================*/
/* Index: Idx_SingleParticipantRequests_IsDeleted               */
/*==============================================================*/
create index Idx_SingleParticipantRequests_IsDeleted on SingleParticipantRequests (
IsDeleted ASC
)
include (Id,CreateDate)
go

/*==============================================================*/
/* Table: Users                                                 */
/*==============================================================*/
create table Users (
   Id                   int                  identity,
   Login                nvarchar(450)        not null,
   Ticket               uniqueidentifier     null,
   LastLoginDate        datetimeoffset       null default SYSDATETIMEOFFSET(),
   FirstLoginDate       datetimeoffset       null default SYSDATETIMEOFFSET(),
   constraint PK_USERS primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Users') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty N'MS_Description',  
   N'user', @CurrentUser, N'table', N'Users' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty N'MS_Description',  
   N'РџРѕР»СЊР·РѕРІР°С‚РµР»Рё Р•РЎР Рџ', 
   N'user', @CurrentUser, N'table', N'Users'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Users', N'column', N'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СЃРёСЃС‚РµРјС‹ РІС‹РіСЂСѓР·РєРё Р±Р»Р°РЅРєРѕРІ',
   N'user', @CurrentUser, N'table', N'Users', N'column', N'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Login')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Users', N'column', N'Login'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'Р›РѕРіРёРЅ РІ Р•РЎР Рџ',
   N'user', @CurrentUser, N'table', N'Users', N'column', N'Login'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Users')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Ticket')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'Users', N'column', N'Ticket'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РўРёРєРµС‚ РїРѕР»СЊР·РѕРІР°С‚РµР»СЏ',
   N'user', @CurrentUser, N'table', N'Users', N'column', N'Ticket'
go

/*==============================================================*/
/* Index: Idx_Users_LoginName                                   */
/*==============================================================*/
create index Idx_Users_LoginName on Users (
Login ASC
)
go

alter table BlankRequests
   add constraint FK_BLANKREQUESTS_REF_USERS foreign key (EsrpUserId)
      references Users (Id)
go

alter table BlanksDownload
   add constraint FK_BLANKDOWNLOAD_PARTICIPANT foreign key (ParticipantId)
      references Participants (Id)
go

alter table BlanksDownload
   add constraint FK_BLANKDOWNLOAD_SERVER foreign key (RegionId)
      references RegionServers (RegionId)
go

alter table ParticipantInRequest
   add constraint FK_PARTICIPANTUPLOAD_PARTICIPANT foreign key (ParticipantId)
      references Participants (Id)
         on update cascade on delete cascade
go

alter table ParticipantInRequest
   add constraint FK_PARTICIPANTUPLOAD_UPLOAD foreign key (RequestId)
      references BlankRequests (Id)
         on update cascade on delete cascade
go

alter table RegionServerErrors
   add constraint FK_REGIONSERVERERRORS_REF_REGIONSERVERS foreign key (RegionId)
      references RegionServers (RegionId)
go

alter table SingleParticipantRequests
   add constraint FK_SINGLEPARTICIPANTREQUESTS_REF_PARTICIPANTS foreign key (ParticipantId)
      references Participants (Id)
go

alter table SingleParticipantRequests
   add constraint FK_SINGLEPA_FK_SINGLE_USERS foreign key (EsrpUserId)
      references Users (Id)
go

create type ParticipantTableType as table
(
    Hash nvarchar(50)
    ,DocumentNumber nvarchar(50)
    ,Name nvarchar(max)
)    
go

create procedure CreateRequest @RequestId uniqueidentifier, @UserId int = null, @EsrpUserLogin nvarchar(450) = null, @State int, @Participants ParticipantTableType readonly, @Note nvarchar(max) = null as
begin
    insert into BlankRequests(Id, State, OperatorUserId, EsrpUserId, Note)
    values(@RequestId, @State, @UserId, (select top 1 Id from Users where Login = @EsrpUserLogin), @Note)
    
    insert into ParticipantInRequest(RequestId, ParticipantId, ParticipantName, IsCollision)
    select distinct
        @RequestId
        ,p.Id
        ,pp.[Name]
        ,cast (case when (count(1) over(partition by pp.Hash, pp.DocumentNumber)) > 1 then 1 else 0 end as bit)
    from
        @Participants pp
        left join Participants p on p.ParticipantHash = pp.Hash and p.DocumentNumber = pp.DocumentNumber    
end
go


create procedure CreateSingleParticipantRequest 
@RequestId uniqueidentifier
,@UserId int = null
,@EsrpUserLogin nvarchar(450) = null
,@ParticipantId int = null
as
begin
    insert into SingleParticipantRequests(Id, OperatorUserId, EsrpUserId, ParticipantId) 
    values(@RequestId, @UserId, (select top 1 Id from Users where Login = @EsrpUserLogin), @ParticipantId)
end
go

create type BlankDownloadTableType as table
(
    ParticipantId int not null
    ,RegionId int not null
    ,State int not null
    ,[Order] int not null
    ,RelativePath nvarchar(max) not null
    ,Code nvarchar(255) not null
    ,ExamDate datetime not null
    ,SubjectCode int not null
)
go

create procedure FixPageCountInconsistencies @Corrections BlankDownloadTableType readonly
as
begin
    delete from
        BlanksDownload
    where 
        State != 2
        and exists (select 1 from @Corrections c where c.ParticipantId = BlanksDownload.ParticipantId)
        and not exists (select 1 from @Corrections c where c.ParticipantId = BlanksDownload.ParticipantId and c.Code = BlanksDownload.Code)
        
    insert into
        BlanksDownload(ParticipantId, RegionId, State, [Order], RelativePath, Code, ExamDate, SubjectCode)
    select 
        ParticipantId, RegionId, State, [Order], RelativePath, Code, ExamDate, SubjectCode
    from
        @Corrections c    
    where
        not exists (select 1 from BlanksDownload bd where c.ParticipantId = bd.ParticipantId and c.Code = bd.Code)
end
go


create procedure GetOldRequests @HoursToLive int, @State int
as
begin
    declare @now datetimeoffset = SYSDATETIMEOFFSET()

    select
        Id
    from
        BlankRequests    
    where
        DATEDIFF(hour, UpdateDate, @now) > @HoursToLive
        and State = @State
    union all
    select
        Id
    from
        SingleParticipantRequests
    where
        DATEDIFF(hour, CreateDate, @now) > @HoursToLive   
        and IsDeleted = 0             
        
end
go


create procedure GetPageCountInconsistencies
as
begin
    select
        p.Id
        ,p.RegionId
        ,cs.SubjectCode
        ,ce.ExamDate
        ,bi.Barcode
        ,bi.BlankType
        ,bi.CompositionPageCount as PageCount
        ,bi.ProjectBatchId
        ,bi.ProjectName
        --,bd.Cnt
    from 
    	(select COUNT(1) Cnt, ParticipantId from BlanksDownload group by ParticipantId) bd
    	join Participants p on bd.ParticipantId = p.Id
    	join [CheckEge].dbo.ap_ParticipantExams cpe on cpe.Id = p.ParticipantExamId
        join [CheckEge].dbo.dat_Exams ce on cpe.ExamGlobalId = ce.ExamGlobalId
        join [CheckEge].dbo.dat_Subjects cs on ce.SubjectCode = cs.SubjectCode and cs.IsCompositionWithLoadableBlanks = 1
    	join [CheckEge].dbo.ap_BlankInfo bi on bi.ParticipantExamId = cpe.Id
    where
    	bi.CompositionPageCount is not null and bi.CompositionPageCount != bd.Cnt
end
go


create procedure GetParticipant
@Hash nvarchar(50)
,@DocumentNumber nvarchar(50)
as
begin
    select
        p.ParticipantRbdId
        ,p.ParticipantHash
        ,p.DocumentNumber
        ,p.RegionId
        ,bd.State
        ,rs.Name as RegionName
        ,p.Id
    from
        Participants p
        left join BlanksDownload bd on p.Id = bd.ParticipantId   
        left join RegionServers rs on p.RegionId = rs.RegionId
    where
        p.ParticipantHash = @Hash
        and p.DocumentNumber = @DocumentNumber    
end
go


create procedure GetRequestState @UserId int = null, @EsrpUserLogin nvarchar(450) = null, @Take int, @Skip int
as
begin
    with t as
    (
        select
            row_number() over (order by br.CreateDate desc) as RowNumber
            ,br.*
        from
            BlankRequests br
        where
            br.OperatorUserId = @UserId
            or br.EsrpUserId = (select top 1 Id from Users where Login = @EsrpUserLogin)
    )        
    select
        (select count(1) from t) as RecordCount
        ,t.Id
        ,t.State
        ,cast (t.CreateDate as datetime) as CreateDate
        ,pir.ParticipantId as ParticipantId 
        ,bd.State as BlankState
        ,t.Note
    from
        t
        left join ParticipantInRequest pir on t.Id = pir.RequestId
        left join Participants p on p.Id = pir.ParticipantId
        left join BlanksDownload bd on bd.ParticipantId = p.Id
    where
        RowNumber between @Skip + 1 and @Skip + @Take    
    order by
        t.CreateDate desc
end
go


create procedure GetServerErrorDetails @RegionId int
as
begin
    select
        Code
        ,ExamDate
        ,[Error]
        ,RbdId
    from
        RegionServerErrors rse
    where
        rse.RegionId = @RegionId        
end
go


create procedure GetServerStatuses @RegionId int = null
as
begin
    select
        rs.RegionId
        ,rs.[Name]
        ,rs.Url
        ,rs.IsAvailable
        ,rs.ServerBlankCount
        ,(select count(1) from BlanksDownload bd where bd.RegionId = rs.RegionId) as DbCount
        ,cast (LastAvailabilityCheck as datetime) as LastAvailabilityCheck
        ,cast (LastFileCheck as datetime) as LastFileCheck
        ,cast (case when exists (select * from RegionServerErrors rse where rse.RegionId = rs.RegionId) then 1 else 0 end as bit) as HasErrors
    from
        RegionServers rs    
    where
        (@RegionId is null or @RegionId = rs.RegionId)    
        and rs.Url is not null
end
go


create procedure GetServersHavingBlanks
as
begin
    select
        rs.RegionId
        ,rs.Url
        ,bd.ExamDate
        ,bd.SubjectCode
    from
        RegionServers rs
        cross apply (select top 1 * from BlanksDownload bdbd where bdbd.RegionId = rs.RegionId) bd
    where
        rs.Url is not null    
end
go


create procedure GetServersWithBlanks @RegionId int = null
as
begin
    select
        rs.RegionId
        ,rs.Url
        ,bd.Code
        ,bd.ExamDate
        ,bd.SubjectCode
    from
        RegionServers rs
        join BlanksDownload bd on rs.RegionId = bd.RegionId
    where
        rs.Url is not null
        and (@RegionId is null or @RegionId = rs.RegionId)        
    order by
        rs.RegionId, bd.SubjectCode, bd.ExamDate    
end
go


create procedure GetTopBlankRequestsToZip
@Top int
,@DownloadSuccessState int
,@DownloadErrorState int
,@RequestState int
,@RequestNewState int
as
begin
    declare @ready table (Id uniqueidentifier)
    
    insert into @ready
    select Top (@Top)
        brbr.Id 
    from
        BlankRequests brbr
    where
        brbr.State = @RequestState
        and not exists 
        (
            select 
                1
            from 
                ParticipantInRequest pirpir
                join Participants pp on pp.Id = pirpir.ParticipantId
                left join BlanksDownload bdbd on bdbd.ParticipantId = pp.Id
                left join RegionServers rs on pp.RegionId = rs.RegionId
            where 
                pirpir.RequestId = brbr.Id
                and rs.Url is not null
                and bdbd.State not in (@DownloadSuccessState, @DownloadErrorState)
        )
        and exists (select 1 from pirpir where pirpir.RequestId = brbr.Id)
    order by
        brbr.CreateDate

    update
        BlankRequests
    set
        State = @RequestNewState
        ,UpdateDate = SYSDATETIMEOFFSET()
    where
        exists (select 1 from @ready r where r.Id = BlankRequests.Id)        

    select
        br.Id
        ,br.Note
        ,pir.ParticipantName
        ,pir.IsCollision
        ,p.ParticipantHash
        ,p.ParticipantRbdId
        ,p.DocumentNumber
        ,p.RegionId
        ,rs.Name as RegionName
        ,cast (case when p.Id is not null and exists (select 1 from BlanksDownload bd where bd.ParticipantId = p.Id and bd.State = @DownloadErrorState) then 1 else 0 end as bit) as HasErrors
        ,cast (case when rs.Url is null then 1 else 0 end as bit) as HasNoServerUrl
        ,cast (case when p.Id is not null and not exists (select 1 from BlanksDownload bd where bd.ParticipantId = p.Id) then 0 else 1 end as bit) as HasNoBlanks
    from
        @ready ready
        join BlankRequests br on ready.Id = br.Id
        join ParticipantInRequest pir on br.Id = pir.RequestId
        left join Participants p on pir.ParticipantId = p.Id
        left join RegionServers rs on p.RegionId = rs.RegionId
end
go


create procedure GetTopBlanksDownloadByStatusAndSetStatus @Top int, @NeededState int, @UpdatedState int
as
begin
Merge 
    BlanksDownload as target
using 
    ( 
    select top (@Top)
        bd.Id,
        bd.RelativePath,
        bd.[Order],
        rs.Url as ServerUrl,
        p.ParticipantRbdId,
        p.ParticipantHash,
        p.DocumentNumber
    from 
        BlanksDownload bd
    join 
        RegionServers rs on rs.RegionId = bd.RegionId and rs.Url is not null and rs.Url != '' and rs.IsAvailable = 1
    join
        Participants p on p.Id = bd.ParticipantId
    where 
        bd.State = (@NeededState)
    order by 
        bd.Createdate
    ) as source
on  (target.Id = source.Id)
when matched then
    Update set target.State = @UpdatedState
output
    source.Id, 
    source.RelativePath,
    source.[Order],
    source.ServerUrl,
    source.ParticipantRbdId,
    source.ParticipantHash,
    source.DocumentNumber;
end
go


create procedure GetUserByLogin @Login nvarchar(450)
as
begin
	select
        u.Id,
		u.Login,
        u.Ticket
	from 
		Users u
	where 
		u.Login = @Login 	
end
go


create procedure IsRequestOwner @RequestId uniqueidentifier, @UserId int = null, @EsrpUserLogin nvarchar(450) = null
as
begin
    select 
    (
        case when 
        exists (
            select 1 from 
                BlankRequests 
            where 
                OperatorUserId = @UserId or EsrpUserId = (select top 1 Id from Users where Login = @EsrpUserLogin)
                and @RequestId = Id
        ) 
        then cast (1 as bit)
        else cast (0 as bit)
    	end
    ) as MultiRequestPermission,
    (
        case when
        exists (
            select 1 from 
                SingleParticipantRequests 
            where 
                OperatorUserId = @UserId or EsrpUserId = (select top 1 Id from Users where Login = @EsrpUserLogin)
                and @RequestId = Id
        )
        then cast (1 as bit)
        else cast (0 as bit)
    	end
    ) as SingleRequestPermission
end
go


create procedure LoadBlanks
as
begin
    select
        p.Id
        ,p.RegionId
        ,s.SubjectCode
        ,e.ExamDate
        ,bi.Barcode
        ,bi.BlankType
        ,bi.CompositionPageCount as PageCount
		,bi.ProjectBatchId
		,bi.ProjectName
    from
        [CheckEge].dbo.dat_Subjects s 
        join [CheckEge].dbo.dat_Exams e on e.SubjectCode = s.SubjectCode and s.IsCompositionWithLoadableBlanks = 1
        join [CheckEge].dbo.ap_ParticipantExams pe on e.ExamGlobalId = pe.ExamGlobalId and pe.Mark5 = 5
        join Participants p on p.ParticipantExamId = pe.Id
        join [CheckEge].dbo.ap_BlankInfo bi on bi.ParticipantExamId = pe.Id
        left join BlanksDownload bd on bd.ParticipantId = p.Id
    where
        bd.Id is null  
        and bi.CompositionPageCount is not null   
end
go


create procedure LoadParticipants
as
begin
    insert into
        Participants(ParticipantRbdId, ParticipantHash, DocumentNumber, RegionId, ParticipantCode, ParticipantExamId)
    select
        p.ParticipantRbdId
        ,p.ParticipantHash
        ,p.DocumentNumber
        ,p.RegionId
        ,p.ParticipantCode
        ,pe.Id
    from
        [CheckEge].dbo.dat_Subjects s
        join [CheckEge].dbo.dat_Exams e on e.SubjectCode = s.SubjectCode and s.IsCompositionWithLoadableBlanks = 1
        join [CheckEge].dbo.ap_ParticipantExams pe on pe.ExamGlobalId = e.ExamGlobalId and pe.Mark5 = 5
    	join [CheckEge].dbo.ap_Participants p on p.Id = pe.ParticipantId
        left join Participants rp on rp.ParticipantCode = p.ParticipantCode and rp.RegionId = p.RegionId
    where
        rp.Id is null    
		and not exists
		(
			select 1 from
				[CheckEge].dbo.dat_Subjects ss
				join [CheckEge].dbo.dat_Exams ee on ee.SubjectCode = ss.SubjectCode and ss.IsCompositionWithLoadableBlanks = 1
				join [CheckEge].dbo.ap_ParticipantExams pepe on pepe.ExamGlobalId = ee.ExamGlobalId and pepe.Mark5 = 5
			where
				ee.ExamDate < e.ExamDate
                and pepe.ParticipantId = p.Id
		)
        --not exists (select 1 from Participants where ParticipantRbdId = p.ParticipantRbdId)
end
go


create procedure LoadRegions
as
begin
    merge
        RegionServers as target
    using
    (
        select
            r.REGION as RegionId
            ,case when right(ri.CompositionBlanksServer, 1) = '/' then ri.CompositionBlanksServer else ri.CompositionBlanksServer + '/' end as Url
            ,r.RegionName as Name
        from    
            [CheckEge].dbo.rbdc_Regions r
            left join [CheckEge].dbo.ap_RegionInfo ri on r.REGION = ri.RegionId
    ) source
    on
        source.RegionId = target.RegionId
    when matched then
        update set target.Url = source.Url
    when not matched then
        insert values(RegionId, Url, [Name], 0, null, 0, null);
end
go


create procedure MergePagesCount as
begin
    Merge 
        PagesCount as target
    using 
    ( 
        select distinct *
        from PagesCountBulk
    ) as source    
    on 
    (
        target.RegionId = source.RegionId 
        and target.Barcode = source.Barcode 
        and target.ProjectBatchId = source.ProjectBatchId 
        and target.ProjectName = source.ProjectName
    )
    when matched then
        update set target.PageCount = source.PageCount
    when not matched then
	insert 
		(RegionId, ExamGlobalId, ExamDate, Barcode, ProjectBatchId, ProjectName, PageCount) 
	values 
		(RegionId, ExamGlobalId, ExamDate, Barcode, ProjectBatchId, ProjectName, PageCount);
end
go


create procedure MergeUser @Login nvarchar(450), @Ticket uniqueidentifier
as
begin
    merge into Users as u
    	using (select @Login as Login, @Ticket as Ticket) ins
    	on u.Login = ins.Login
    	when matched then 
    		update set u.Ticket = ins.Ticket, u.[Login] = ins.[Login], u.LastLoginDate = SYSDATETIMEOFFSET()
    	when not matched then 
        	insert 
        		(Login, Ticket)
        	values
        		(ins.Login, ins.Ticket)
    	output Inserted.Id, Inserted.Login, ins.Ticket;
end
go


create procedure ResetState @DownloadProcessingState int, @DownloadNotProcessedState int, @RequestProcessingState int, @RequestNotProcessedState int
as
begin
    update 
        BlanksDownload
    set
        State = @DownloadNotProcessedState
    where
        State = @DownloadProcessingState          
        
    update
        BlankRequests
    set
        State = @RequestNotProcessedState
    where
        State = @RequestProcessingState            
end
go


create procedure SetRequestStateAndDeleteParticipants @RequestId uniqueidentifier, @State int as
begin
begin tran
    update
        BlankRequests
    set
        State = @State
        ,UpdateDate = SYSDATETIMEOFFSET()
    where
        Id = @RequestId            
        
    update
        ParticipantInRequest
    set
        ParticipantName = ''    
    where
        RequestId = @RequestId        
commit tran        
end
go

create type RequestIdTableType as table
(
    Id uniqueidentifier not null
)
go
    

create procedure SetRequestStateAndDeleteSingleParticipantRequests @State int, @RequestId RequestIdTableType readonly
as
begin
    update
        BlankRequests
    set
        State = @State
    where
        Id in (select Id from @RequestId)
        
    update
        SingleParticipantRequests
    set
        IsDeleted = 1    
    where
        Id in (select Id from @RequestId)        
end
go

create type ParticipantAndOrderTableType as table
(
    RbdId uniqueidentifier not null
    ,Hash nvarchar(50) not null
    ,DocumentNumber nvarchar(50) not null
    ,[Order] int not null
)
go

create procedure SetStateByParticipantAndOrder @State int, @Blanks ParticipantAndOrderTableType readonly
as
begin
    merge
        BlanksDownload as target
    using
    (
        select
            bd.Id
        from
            BlanksDownload bd
            join Participants p on bd.ParticipantId = p.Id
            join @Blanks b on
                b.RbdId = p.ParticipantRbdId
                and b.DocumentNumber = p.DocumentNumber
                and b.Hash = p.ParticipantHash
                and b.[Order] = bd.[Order]
    ) as source
    on target.Id = source.Id
    when matched then
        update set State = @State;
end
go

create type ServerAvailabilityTableType as table
(
    RegionId int not null
    ,IsAvailable bit not null
)
go
    

create procedure UpdateServerAvailability @Availability ServerAvailabilityTableType readonly
as
begin
    merge
        RegionServers as target
    using
        @Availability as source
    on
        target.RegionId = source.RegionId
    when matched then
        update set 
            IsAvailable = source.IsAvailable
            ,LastAvailabilityCheck = SYSDATETIMEOFFSET();
end
go

create type ErrorTableType as table
(
    Code nvarchar(255) not null
    ,ExamDate datetime not null
    ,[Error] int not null
)
go

create procedure UpdateServerErrors 
@RegionId int, @ServerBlankCount int, @Errors ErrorTableType readonly, @IsAvailable bit, @NotProcessedState int, @ErrorState int
as
begin
begin tran
    delete from 
        RegionServerErrors
    where 
        RegionId = @RegionId
    
    insert into RegionServerErrors(RegionId, Code, ExamDate, [Error], RbdId)
    select
        @RegionId
        ,s.Code
        ,s.ExamDate
        ,s.[Error]
        ,p.ParticipantRbdId
    from
        @Errors s
        left join BlanksDownload bd on s.Code = bd.Code
        left join Participants p on bd.ParticipantId = p.Id
        
    update 
        RegionServers
    set 
        ServerBlankCount = @ServerBlankCount
        ,LastFileCheck = SYSDATETIMEOFFSET()
        ,LastAvailabilityCheck = SYSDATETIMEOFFSET()
        ,IsAvailable = @IsAvailable
    where
        RegionId = @RegionId    
        
    update
        BlanksDownload
    set
        State = @ErrorState
    where
        RegionId = @RegionId
        and Code in (select Code from @Errors)
        and State = @NotProcessedState
    
    update
        BlanksDownload
    set
        State = @NotProcessedState                    
    where
        RegionId = @RegionId
        and Code not in (select Code from @Errors)
        and State = @ErrorState
            
commit tran    
end
go
