IF (EXISTS(
    SELECT 
        name 
    FROM 
        master.dbo.sysdatabases 
    WHERE 
        '[' + name + ']' = 'CheckEgeUnitTest'
        OR name = 'CheckEgeUnitTest'
))
    alter database CheckEgeUnitTest set single_user with rollback immediate
go

use master
go

IF (EXISTS(
    SELECT 
        name 
    FROM 
        master.dbo.sysdatabases 
    WHERE 
        '[' + name + ']' = 'CheckEgeUnitTest'
        OR name = 'CheckEgeUnitTest'
))
    drop database CheckEgeUnitTest
go

/*==============================================================*/
/* Database: CheckEge                                           */
/*==============================================================*/
create database CheckEgeUnitTest
go

use CheckEgeUnitTest
go

/*==============================================================*/
/* Table: AnswerDto                                             */
/*==============================================================*/
create table AnswerDto (
   RbdId                uniqueidentifier     not null,
   Code                 nvarchar(50)         not null,
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   TaskTypeCode         int                  not null,
   TaskNumber           int                  not null,
   AnswerValue          nvarchar(255)        not null,
   Mark                 int                  not null
)
go

/*==============================================================*/
/* Table: AppealDto                                             */
/*==============================================================*/
create table AppealDto (
   RbdId                uniqueidentifier     not null,
   Code                 nvarchar(50)         not null,
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   Status               int                  not null,
   CreateDate           datetime             not null
)
go

/*==============================================================*/
/* Table: AppealStatuses                                        */
/*==============================================================*/
create table AppealStatuses (
   Status               int                  not null,
   Description          nvarchar(255)        not null,
   constraint PK_APPEALSTATUSES primary key (Status)
)
go

/*==============================================================*/
/* Table: BlankInfoDto                                          */
/*==============================================================*/
create table BlankInfoDto (
   RbdId                uniqueidentifier     not null,
   Code                 nvarchar(50)         not null,
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   BlankType            int                  not null,
   Answer               nvarchar(50)         null,
   PrimaryMark          int                  not null,
   Barcode              nvarchar(50)         null,
   PageCount            int                  not null,
   CreateDate           datetime             not null,
   ProjectBatchId       int                  null,
   ProjectName          nvarchar(256)        null
)
go

/*==============================================================*/
/* Table: ExamDto                                               */
/*==============================================================*/
create table ExamDto (
   Id                   int                  not null,
   Date                 datetime             not null,
   WaveCode             int                  not null,
   SubjectCode          int                  not null
)
go

/*==============================================================*/
/* Table: ParticipantDto                                        */
/*==============================================================*/
create table ParticipantDto (
   RbdId                uniqueidentifier     not null,
   Code                 nvarchar(50)         not null,
   Hash                 nvarchar(50)         not null,
   Document             nvarchar(50)         null,
   RegionId             int                  not null,
   IsDeleted            bit                  not null
)
go

/*==============================================================*/
/* Table: ParticipantExamDto                                    */
/*==============================================================*/
create table ParticipantExamDto (
   RbdId                uniqueidentifier     not null,
   Code                 nvarchar(50)         not null,
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   PrimaryMark          int                  not null,
   TestMark             int                  not null,
   Mark5                int                  not null,
   ProcessCondition     int                  not null,
   CreateDate           datetime             not null
)
go

/*==============================================================*/
/* Table: ParticipantExamLinkDto                                */
/*==============================================================*/
create table ParticipantExamLinkDto (
   RbdId                uniqueidentifier     not null,
   Code                 nvarchar(50)         not null,
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   OralExamGlobalId     int                  null
)
go

/*==============================================================*/
/* Table: RegionDto                                             */
/*==============================================================*/
create table RegionDto (
   Id                   int                  not null,
   Name                 nvarchar(255)        not null
)
go

/*==============================================================*/
/* Table: SubjectDto                                            */
/*==============================================================*/
create table SubjectDto (
   Code                 int                  not null,
   Name                 nvarchar(255)        not null,
   MinValue             int                  not null,
   IsComposition        bit                  not null
)
go

/*==============================================================*/
/* Table: UserRoles                                             */
/*==============================================================*/
create table UserRoles (
   Id                   int                  identity(1,1),
   UserId               int                  not null,
   Role                 int                  not null,
   constraint "PK_dbo.UserRoles" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Table: Users                                                 */
/*==============================================================*/
create table Users (
   UserId               int                  identity(1,1),
   RegionId             int                  null,
   LoginName            nvarchar(255)        not null,
   Password             nvarchar(255)        null,
   IsEnabled            bit                  not null,
   IsDeleted            bit                  not null default 0,
   constraint "PK_dbo.Users" primary key (UserId)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
on "PRIMARY"
go

/*==============================================================*/
/* Table: ap_Answers                                            */
/*==============================================================*/
create table ap_Answers (
   Id                   int                  identity(1,1),
   RegionId             int                  not null,
   ParticipantExamId    int                  not null,
   TaskTypeCode         int                  not null,
   TaskNumber           int                  not null,
   AnswerValue          nvarchar(255)        not null,
   Mark                 int                  not null,
   constraint "PK_dbo.ap_Answers" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_Answers_RegionId                               */
/*==============================================================*/
create index Idx_ap_Answers_RegionId on ap_Answers (
RegionId ASC
)
include (ParticipantExamId)
go

/*==============================================================*/
/* Index: Idx_ap_Answers_ParticipantExamId                      */
/*==============================================================*/
create index Idx_ap_Answers_ParticipantExamId on ap_Answers (
ParticipantExamId ASC
)
include (TaskTypeCode,TaskNumber,AnswerValue,Mark)
go

/*==============================================================*/
/* Table: ap_Appeals                                            */
/*==============================================================*/
create table ap_Appeals (
   Id                   int                  identity,
   ParticipantExamId    int                  not null,
   Status               int                  not null,
   CreateDate           datetime             not null,
   constraint PK_AP_APPEALS primary key (Id)
)
go

/*==============================================================*/
/* Index: Idx_ap_Appeals_ParticipantExamId                      */
/*==============================================================*/
create index Idx_ap_Appeals_ParticipantExamId on ap_Appeals (
ParticipantExamId ASC
)
include (Status)
go

/*==============================================================*/
/* Table: ap_BallSettings                                       */
/*==============================================================*/
create table ap_BallSettings (
   Id                   int                  identity(1,1),
   SubjectCode          int                  not null,
   TaskNumber           int                  not null,
   MaxValue             int                  not null,
   TaskTypeCode         int                  not null,
   LegalSymbols         nvarchar(512)        null,
   DisplayNumber        nvarchar(32)         null default null,
   constraint "PK_dbo.ap_BallSettings" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Table: ap_BlankInfo                                          */
/*==============================================================*/
create table ap_BlankInfo (
   Id                   int                  identity(1,1),
   ParticipantExamId    int                  not null,
   BlankType            int                  not null,
   Answer               nvarchar(50)         null,
   PrimaryMark          int                  not null,
   Barcode              nvarchar(50)         null,
   PageCount            int                  not null,
   CreateDate           datetime             not null,
   ProjectBatchId       int                  null,
   ProjectName          nvarchar(256)        null,
   CompositionPageCount int                  null,
   constraint "PK_dbo.ap_BlankInfo" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_ap_BlankInfo_ParticipantExamId                 */
/*==============================================================*/
create index Idx_ap_ap_BlankInfo_ParticipantExamId on ap_BlankInfo (
ParticipantExamId ASC
)
include (BlankType,Barcode,PageCount,CompositionPageCount)
go

/*==============================================================*/
/* Index: Idx_ap_BlankInfo_Barcode_ProjectBatchId_ProjectName   */
/*==============================================================*/
create index Idx_ap_BlankInfo_Barcode_ProjectBatchId_ProjectName on ap_BlankInfo (
Barcode ASC,
ProjectBatchId ASC,
ProjectName ASC
)
include (ParticipantExamId)
go

/*==============================================================*/
/* Table: ap_DocumentUrls                                       */
/*==============================================================*/
create table ap_DocumentUrls (
   Id                   int                  identity(1,1),
   Name                 nvarchar(255)        not null,
   Url                  nvarchar(255)        not null,
   constraint "PK_dbo.ap_DocumentUrl" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Table: ap_ExamSettings                                       */
/*==============================================================*/
create table ap_ExamSettings (
   Id                   int                  identity(1,1),
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   ShowResult           bit                  not null,
   ShowBlanks           bit                  not null,
   constraint "PK_dbo.ap_AppilsSettings" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_ExamSettings_RegionId                          */
/*==============================================================*/
create index Idx_ap_ExamSettings_RegionId on ap_ExamSettings (
RegionId ASC
)
go

/*==============================================================*/
/* Table: ap_GekDocuments                                       */
/*==============================================================*/
create table ap_GekDocuments (
   Id                   int                  identity(1,1),
   RegionId             int                  not null,
   ExamGlobalId         int                  not null,
   Number               nvarchar(50)         not null,
   CreateDate           datetime             not null,
   Url                  nvarchar(255)        null,
   constraint "PK_dbo.ap_GEKDocuments" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_GekDocuments_RegionId_ExamGlobalId             */
/*==============================================================*/
create unique index Idx_ap_GekDocuments_RegionId_ExamGlobalId on ap_GekDocuments (
RegionId ASC,
ExamGlobalId ASC
)
go

/*==============================================================*/
/* Table: ap_ParticipantExamLinks                               */
/*==============================================================*/
create table ap_ParticipantExamLinks (
   Id                   int                  identity,
   ParticipantExamId    int                  not null,
   OralParticipantExamId int                  not null,
   constraint PK_AP_PARTICIPANTEXAMLINKS primary key (Id)
)
go

/*==============================================================*/
/* Index: Idx_ap_ParticipantExamLinks_ParticipantExamId         */
/*==============================================================*/
create index Idx_ap_ParticipantExamLinks_ParticipantExamId on ap_ParticipantExamLinks (
ParticipantExamId ASC
)
include (OralParticipantExamId)
go

/*==============================================================*/
/* Index: Idx_ap_ParticipantExamLinks_OralParticipantExamId     */
/*==============================================================*/
create index Idx_ap_ParticipantExamLinks_OralParticipantExamId on ap_ParticipantExamLinks (
OralParticipantExamId ASC
)
go

/*==============================================================*/
/* Table: ap_ParticipantExams                                   */
/*==============================================================*/
create table ap_ParticipantExams (
   Id                   int                  identity(1,1),
   ParticipantId        int                  not null,
   ExamGlobalId         int                  not null,
   PrimaryMark          int                  not null,
   TestMark             int                  not null,
   Mark5                int                  not null,
   ProcessCondition     int                  not null,
   CreateDate           datetime             not null,
   IsHidden             bit                  not null default 0,
   constraint "PK_dbo.ap_ParticipantExams" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_ParticipantExams_ParticipantId                 */
/*==============================================================*/
create index Idx_ap_ParticipantExams_ParticipantId on ap_ParticipantExams (
ParticipantId ASC
)
include (Id,ExamGlobalId,TestMark,Mark5,ProcessCondition,IsHidden)
go

/*==============================================================*/
/* Index: Idx_ap_ParticipantExams_IsHidden                      */
/*==============================================================*/
create index Idx_ap_ParticipantExams_IsHidden on ap_ParticipantExams (
IsHidden ASC
)
include (ParticipantId,ExamGlobalId)
go

/*==============================================================*/
/* Index: Idx_ap_ParticipantExams_ParticipantId_GlobalExamId    */
/*==============================================================*/
create unique index Idx_ap_ParticipantExams_ParticipantId_GlobalExamId on ap_ParticipantExams (
ParticipantId ASC,
ExamGlobalId ASC
)
go

/*==============================================================*/
/* Table: ap_Participants                                       */
/*==============================================================*/
create table ap_Participants (
   Id                   int                  identity(1,1),
   ParticipantRbdId     uniqueidentifier     not null,
   ParticipantCode      nvarchar(50)         null,
   ParticipantHash      nvarchar(50)         not null,
   DocumentNumber       nvarchar(50)         null,
   RegionId             int                  not null,
   constraint "PK_dbo.ap_Participants" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_Participants_ParticipantHash                   */
/*==============================================================*/
create index Idx_ap_Participants_ParticipantHash on ap_Participants (
ParticipantHash ASC
)
go

/*==============================================================*/
/* Index: Idx_ap_Participants_ParticipantCode_RegionId          */
/*==============================================================*/
create index Idx_ap_Participants_ParticipantCode_RegionId on ap_Participants (
ParticipantCode ASC,
RegionId ASC
)
go

/*==============================================================*/
/* Index: Idx_ap_Participants_ParticipantRbdId                  */
/*==============================================================*/
create index Idx_ap_Participants_ParticipantRbdId on ap_Participants (
ParticipantRbdId ASC
)
include (ParticipantCode)
go

/*==============================================================*/
/* Table: ap_RegionInfo                                         */
/*==============================================================*/
create table ap_RegionInfo (
   Id                   int                  identity(1,1),
   Fio                  nvarchar(255)        null,
   Phone                nvarchar(255)        null,
   Email                nvarchar(255)        null,
   HotLineData          nvarchar(max)        null,
   BlanksServer         nvarchar(255)        null,
   CompositionBlanksServer nvarchar(255)        null,
   Description          nvarchar(max)        null,
   RegionId             int                  not null,
   constraint "PK_dbo.ap_RegionInfo" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
go

/*==============================================================*/
/* Index: Idx_ap_RegionInfo_RegionId                            */
/*==============================================================*/
create index Idx_ap_RegionInfo_RegionId on ap_RegionInfo (
RegionId ASC
)
go

/*==============================================================*/
/* Table: ap_TaskSettings                                       */
/*==============================================================*/
create table ap_TaskSettings (
   Id                   int                  identity(1,1),
   ParentTask           int                  null,
   SubjectCode          int                  not null,
   TaskTypeCode         int                  not null,
   TaskNumber           int                  null,
   MaxValue             int                  null,
   CriteriaName         nvarchar(150)        null,
   DisplayNumber        nvarchar(32)         null default null,
   constraint "PK_dbo.ap_TaskSettings" primary key (Id)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Table: dat_Exams                                             */
/*==============================================================*/
create table dat_Exams (
   ExamGlobalId         int                  not null,
   ExamDate             datetime             not null,
   WaveCode             int                  not null,
   SubjectCode          int                  not null,
   constraint "PK_dbo.dat_Exams" primary key (ExamGlobalId)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

/*==============================================================*/
/* Table: dat_Subjects                                          */
/*==============================================================*/
create table dat_Subjects (
   SubjectCode          int                  not null,
   SubjectName          nvarchar(255)        not null,
   MinValue             int                  not null,
   IsComposition        bit                  not null default 0,
   IsBasicMath          bit                  not null default 0,
   IsForeignLanguage    bit                  not null default 0,
   IsCompositionWithLoadableBlanks bit                  not null default 0,
   IsOral               bit                  not null default 0,
   SubjectDisplayName   nvarchar(255)        null,
   WrittenSubjectCode   int                  null,
   constraint "PK_dbo.dat_Subjects" primary key (SubjectCode)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('dat_Subjects') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty N'MS_Description',  
   N'user', @CurrentUser, N'table', N'dat_Subjects' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty N'MS_Description',  
   N'РџСЂРµРґРјРµС‚С‹', 
   N'user', @CurrentUser, N'table', N'dat_Subjects'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dat_Subjects')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsComposition')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsComposition'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РЇРІР»СЏРµС‚СЃСЏ Р»Рё СЃРѕС‡РёРЅРµРЅРёРµРј/РёР·Р»РѕР¶РµРЅРёРµРј',
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsComposition'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dat_Subjects')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsBasicMath')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsBasicMath'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РЇРІР»СЏРµС‚СЃСЏ Р»Рё Р±Р°Р·РѕРІРѕР№ РјР°С‚РµРјР°С‚РёРєРѕР№',
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsBasicMath'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dat_Subjects')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsCompositionWithLoadableBlanks')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsCompositionWithLoadableBlanks'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РЇРІР»СЏРµС‚СЃСЏ Р»Рё СЃРѕС‡РёРЅРµРЅРёРµРј, РїРѕ РєРѕС‚РѕСЂРѕРјСѓ РІСѓР·С‹ РјРѕРіСѓС‚ РІС‹РіСЂСѓР¶Р°С‚СЊ Р±Р»Р°РЅРєРё',
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsCompositionWithLoadableBlanks'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dat_Subjects')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsOral')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsOral'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РЇРІР»СЏРµС‚СЃСЏ Р»Рё СѓСЃС‚РЅС‹Рј РїСЃРµРІРґРѕРїСЂРµРґРјРµС‚РѕРј',
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'IsOral'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dat_Subjects')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'SubjectDisplayName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty N'MS_Description', 
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'SubjectDisplayName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty N'MS_Description', 
   N'РћС‚РѕР±СЂР°Р¶Р°РµРјРѕРµ РґР»СЏ СѓС‡Р°СЃС‚РЅРёРєР° РёРјСЏ РїСЂРµРґРјРµС‚Р° РІ СЃР»СѓС‡Р°Рµ, РєРѕРіРґР° РѕРЅРѕ РѕС‚Р»РёС‡Р°РµС‚СЃСЏ РѕС‚ SubjectName',
   N'user', @CurrentUser, N'table', N'dat_Subjects', N'column', N'SubjectDisplayName'
go

/*==============================================================*/
/* Table: rbdc_Regions                                          */
/*==============================================================*/
create table rbdc_Regions (
   REGION               int                  not null,
   RegionName           nvarchar(255)        not null,
   Enable               bit                  not null,
   constraint "PK_dbo.rbdc_Regions" primary key (REGION)
         WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
ON [PRIMARY]
go

alter table UserRoles
   add constraint "FK_dbo.UserRoles_dbo.Users_UserId" foreign key (UserId)
      references Users (UserId)
go

alter table Users
   add constraint FK_USERS_REF_REGIONS foreign key (RegionId)
      references rbdc_Regions (REGION)
go

alter table ap_Answers
   add constraint "FK_dbo.ap_Answers_dbo.ap_ParticipantExams_ParticipantExamId" foreign key (ParticipantExamId)
      references ap_ParticipantExams (Id)
         on delete cascade
go

alter table ap_Answers
   add constraint "FK_dbo.ap_Answers_dbo.rbdc_Regions_RegionId" foreign key (RegionId)
      references rbdc_Regions (REGION)
go

alter table ap_Appeals
   add constraint FK_ap_Appeals_ap_ParticipantExams_ParticipantExamId foreign key (ParticipantExamId)
      references ap_ParticipantExams (Id)
         on delete cascade
go

alter table ap_BallSettings
   add constraint "FK_dbo.ap_BallSettings_dbo.dat_Subjects_SubjectCode" foreign key (SubjectCode)
      references dat_Subjects (SubjectCode)
go

alter table ap_BlankInfo
   add constraint "FK_dbo.ap_BlankInfo_dbo.ap_ParticipantExams_ParticipantExamId" foreign key (ParticipantExamId)
      references ap_ParticipantExams (Id)
         on delete cascade
go

alter table ap_ExamSettings
   add constraint "FK_dbo.ap_AppilsSettings_dbo.dat_Exams_ExamGlobalId" foreign key (ExamGlobalId)
      references dat_Exams (ExamGlobalId)
go

alter table ap_ExamSettings
   add constraint "FK_dbo.ap_AppilsSettings_dbo.rbdc_Regions_RegionId" foreign key (RegionId)
      references rbdc_Regions (REGION)
go

alter table ap_GekDocuments
   add constraint "FK_dbo.ap_GEKDocuments_dbo.dat_Exams_ExamGlobalId" foreign key (ExamGlobalId)
      references dat_Exams (ExamGlobalId)
go

alter table ap_GekDocuments
   add constraint "FK_dbo.ap_GEKDocuments_dbo.rbdc_Regions_RegionId" foreign key (RegionId)
      references rbdc_Regions (REGION)
go

alter table ap_ParticipantExamLinks
   add constraint FK_PELINKS_REF_ORALPARTICIPANTEXAM foreign key (OralParticipantExamId)
      references ap_ParticipantExams (Id)
go

alter table ap_ParticipantExamLinks
   add constraint FK_PELINKS_REF_PARTICIPANTEXAMS foreign key (ParticipantExamId)
      references ap_ParticipantExams (Id)
go

alter table ap_ParticipantExams
   add constraint "FK_dbo.ap_ParticipantExams_dbo.ap_Participants_ParticipantId" foreign key (ParticipantId)
      references ap_Participants (Id)
         on delete cascade
go

alter table ap_ParticipantExams
   add constraint "FK_dbo.ap_ParticipantExams_dbo.dat_Exams_ExamGlobalId" foreign key (ExamGlobalId)
      references dat_Exams (ExamGlobalId)
go

alter table ap_Participants
   add constraint "FK_dbo.ap_Participants_dbo.rbdc_Regions_RegionId" foreign key (RegionId)
      references rbdc_Regions (REGION)
go

alter table ap_RegionInfo
   add constraint FK_REGIONINFO_REF_REGION foreign key (RegionId)
      references rbdc_Regions (REGION)
go

alter table ap_TaskSettings
   add constraint FK_TASKSETTINGS_REF_SUBJECTS foreign key (SubjectCode)
      references dat_Subjects (SubjectCode)
go

alter table dat_Exams
   add constraint FK_EXAMS_REF_SUBJECTS foreign key (SubjectCode)
      references dat_Subjects (SubjectCode)
go


create procedure CreateUser @Login nvarchar(255), @Password nvarchar(255), @Role int, @RegionId int = null, @IsEnabled bit as
begin
    insert into Users(RegionId, LoginName, Password, IsEnabled)
    values (@RegionId, @Login, @Password, @IsEnabled)
    
    insert into UserRoles(UserId, Role)
    select SCOPE_IDENTITY(), @Role
end
go


create procedure DeleteGekDocument @RegionId int, @ExamGlobalId int as
begin
    delete from
        ap_GekDocuments
    where
        RegionId = @RegionId
        and ExamGlobalId = @ExamGlobalId    
end
go


create procedure DeleteUser @Id int as
begin
    update
        Users
    set
        IsDeleted = 1
    where
        UserId = @Id        
   
end
go


create procedure GetAnswers 
@ParticipantCode nvarchar(50), 
@RegionId int, 
@ExamGlobalId int
as
begin
    with pep as
    (
    	select
    		pe.Id
    		,pe.PrimaryMark
    		,pe.TestMark
            ,pe.Mark5
            ,pe.IsHidden
            ,pel.OralParticipantExamId
            ,e.ExamDate
            ,e.SubjectCode
            ,s.IsComposition
    	from 
    		ap_ParticipantExams pe
    		join ap_Participants p on pe.ParticipantId = p.Id
            join dat_Exams e on pe.ExamGlobalId = e.ExamGlobalId
            join dat_Subjects s on e.SubjectCode = s.SubjectCode
            left join ap_ParticipantExamLinks pel on pe.Id = pel.ParticipantExamId
    	where 
    		p.ParticipantCode = @ParticipantCode
    		and p.RegionId = @RegionId
    		and pe.ExamGlobalId = @ExamGlobalId
            and (s.IsComposition = 0 or pe.Mark5 = 5)
    )
    select
    	pep.PrimaryMark
    	,pep.TestMark
        ,pep.Mark5
        ,pep.IsHidden
        ,pep.ExamDate
        ,pep.SubjectCode
    	,a.TaskTypeCode
    	,a.TaskNumber
    	,a.AnswerValue
    	,a.Mark
    	,cast (null as nvarchar(50)) as Barcode
    	,cast (null as int) as BlankType
    	,cast (null as int) as PageCount
		,cast (null as int) as ProjectBatchId
		,cast (null as nvarchar(500)) as ProjectName
    	,cast (0 as bit) as RowType
    from 
    	pep
    	join ap_Answers a on a.ParticipantExamId in (pep.Id, pep.OralParticipantExamId)
    union all
    select
    	pep.PrimaryMark
    	,pep.TestMark
    	,pep.Mark5
    	,pep.IsHidden
        ,pep.ExamDate
        ,pep.SubjectCode
    	,cast (null as int) as TaskTypeCode
    	,cast (null as int) as TaskNumber
    	,cast (null as nvarchar(255)) as AnswerValue
    	,cast (null as int) as Mark
    	,bi.Barcode
    	,bi.BlankType
    	,case when pep.IsComposition = 0 then bi.PageCount else bi.CompositionPageCount end as PageCount
		,bi.ProjectBatchId
		,bi.ProjectName
    	,cast (1 as bit) as RowType
    from 
    	pep
    	join ap_BlankInfo bi on pep.Id = bi.ParticipantExamId
    order by
        BlankType
end
go


create procedure GetAnswersCriteria @SubjectCode int = null
as
begin
   select  
        s.SubjectCode
    	,cp.HasCriteria
    	,cp.ParentId
    	,cp.TaskTypeCode
    	,cp.TaskNumber
        ,cp.DisplayNumber
    	,cp.MaxValue
    	,cp.CriteriaName
    	,s.MinValue
        ,s.IsComposition
        ,s.IsBasicMath
        ,s.IsForeignLanguage
    from
        dat_Subjects s
        left join
    	(select
    		bs.Id
    		,(cast (0 as bit)) as HasCriteria
    		,null as ParentId
    		,bs.SubjectCode
    		,bs.TaskTypeCode
    		,bs.TaskNumber
            ,bs.DisplayNumber
    		,bs.MaxValue
    		,bs.LegalSymbols as CriteriaName
    	from
    		ap_BallSettings bs
    	union all
    	select 
    		ts.Id
    		,(cast (1 as bit)) as HasCriteria
    		,ts.ParentTask as ParentId
    		,ts.SubjectCode
    		,ts.TaskTypeCode
    		,ts.TaskNumber
            ,ts.DisplayNumber
    		,ts.MaxValue
    		,ts.CriteriaName
    	from
    		ap_TaskSettings ts) as cp(Id, HasCriteria, ParentId, SubjectCode, TaskTypeCode, TaskNumber, DisplayNumber, MaxValue, CriteriaName) 
        on cp.SubjectCode = s.SubjectCode or (TaskTypeCode = 3 and cp.SubjectCode = s.WrittenSubjectCode)
		where @SubjectCode is null or s.SubjectCode = @SubjectCode
		order by s.SubjectCode, cp.HasCriteria, isnull(cp.ParentId, cp.Id), isnull(ParentId, 0)
end
go


create procedure GetAppeals 
@ParticipantCode nvarchar(50), 
@RegionId int, 
@ExamGlobalId int
as
begin
select
		app.CreateDate
		,app.Status as AppealStatus
	from 
		ap_ParticipantExams pe
		join ap_Participants p on pe.ParticipantId = p.Id
		join ap_Appeals app on pe.Id = app.ParticipantExamId
		
	where 
		p.ParticipantCode = @ParticipantCode
		and p.RegionId = @RegionId
		and pe.ExamGlobalId = @ExamGlobalId
end
go


create procedure GetBlanksWithCompositionPageCount
as
begin
	select
		bi.BlankType
		,bi.CompositionPageCount
		,p.RegionId
		,p.ParticipantCode
		,pe.ExamGlobalId
	from
		ap_BlankInfo bi
		join ap_ParticipantExams pe on pe.Id = bi.ParticipantExamId
		join ap_Participants p on p.Id = pe.ParticipantId 
    where
        bi.CompositionPageCount is not null    
end
go


create procedure GetCancelledExams 
@RegionId int = null,
@Skip int,
@Take int
as 
begin
	with t as
	(
		select
			row_number() over (order by p.ParticipantCode) as RowNumber
			,p.ParticipantCode
			,e.ExamGlobalId
			,e.ExamDate
			,s.SubjectName
			,r.RegionName
            ,p.RegionId
		from
			ap_ParticipantExams pe
			join dat_Exams e on pe.ExamGlobalId = e.ExamGlobalId
			join ap_Participants p on pe.ParticipantId = p.Id
			join rbdc_Regions r on p.RegionId = r.region
			join dat_Subjects s on e.SubjectCode = s.SubjectCode
		where
			pe.IsHidden = 1
			and (p.RegionId = @RegionId or @RegionId is null)
	)
	select
	 (select count(1) from t) as RecordCount
	 ,t.ParticipantCode
	 ,t.ExamGlobalId
	 ,t.ExamDate
	 ,t.SubjectName
	 ,t.RegionName
     ,t.RegionId
	from
	 t
	where
		RowNumber between @Skip + 1 and @Skip + @Take 
	order by
		t.ParticipantCode
end
go


create procedure GetCancelledParticipantExams
as
begin
	select 
		p.ParticipantCode
		,p.RegionId
		,pe.ExamGlobalId 
	from 
		ap_Participants p
		join ap_ParticipantExams pe on p.Id = pe.ParticipantId
	where
		pe.IsHidden = 1
end
go


create procedure GetDocumentUrls
as
begin
	select
		du.Id
		,du.Name
		,du.Url
	from
		ap_DocumentUrls du
end
go


create procedure GetExamList as
begin
    select
        cast (0 as bit) as RowType
        ,cast (null as int) as ExamGlobalId
        ,cast (null as datetime) as ExamDate
        ,s.SubjectCode
        ,s.SubjectName
        ,s.MinValue
        ,s.IsComposition
        ,s.IsBasicMath
        ,s.IsForeignLanguage
        ,coalesce(s.SubjectDisplayName, s.SubjectName) as SubjectDisplayName
        ,s.IsOral
		,s.WrittenSubjectCode
    from
        dat_Subjects s
    union all
    select
        cast (1 as bit) as RowType
        ,e.ExamGlobalId
        ,e.ExamDate
        ,e.SubjectCode
        ,cast (null as nvarchar(255)) as SubjectName
        ,cast (null as int) as MinValue
        ,cast (null as bit) as IsComposition
        ,cast (null as bit) as IsBasicMath
        ,cast (null as bit) as IsForeignLanguage
        ,cast (null as nvarchar(255)) as SubjectDisplayName
        ,cast (null as bit) as IsOral
		,cast (null as int) as WrittenSubjectCode
    from
        dat_Exams e
    order by
        RowType
        ,ExamDate     
end
go


create procedure GetExamSettings @WaveCode int, @RegionId int
as
begin
	select 
		e.ExamGlobalId
		,s.SubjectName
		,e.ExamDate
        ,s.IsComposition
		,isnull(es.ShowResult, cast (0 as bit)) as ShowResult
		,isnull(es.ShowBlanks, cast (0 as bit)) as ShowBlanks
		,cast(case
				when gd.ExamGlobalId is null then 0
				else 1
			  end as bit) as HasGekDocument
	from 
		dat_Exams e
		join dat_Subjects s on e.SubjectCode = s.SubjectCode
		left join ap_ExamSettings es on e.ExamGlobalId = es.ExamGlobalId and es.RegionId = @RegionId
		left join ap_GekDocuments gd on e.ExamGlobalId = gd.ExamGlobalId and gd.RegionId = @RegionId
	where 
		e.WaveCode = @WaveCode
end
go


create procedure GetExams 
@ParticipantCode nvarchar(50),
@RegionId int
as
begin
select
	pe.ExamGlobalId
	,pe.TestMark
    ,pe.Mark5
	,pe.ProcessCondition
	,pe.IsHidden
	,cast (	case
				when la.Status is null then 0
				else 1
			end as bit) as HasAppeal
	,cast(	case
				when ansa.AnswersCount is null then 0
				else 1
			end as bit) as HasResult
	,la.Status as AppealStatus
	,e.ExamDate
	,coalesce(s.SubjectDisplayName, s.SubjectName) as SubjectName
	,s.MinValue
    ,s.IsComposition
    ,s.IsBasicMath
    ,s.IsForeignLanguage
    ,ope.ExamGlobalId as OralExamGlobalId
	,cast(	case
				when oansa.AnswersCount is null then 0
				else 1
			end as bit) as HasOralResult
    ,ope.ProcessCondition as OralProcessCondition        
from 
	ap_Participants p
	join ap_ParticipantExams pe on p.Id = pe.ParticipantId
    left join ap_ParticipantExamLinks pel on pe.Id = pel.ParticipantExamId
    left join ap_ParticipantExams ope on ope.Id = pel.OralParticipantExamId
	join dat_Exams e on pe.ExamGlobalId = e.ExamGlobalId
	join dat_Subjects s on e.SubjectCode = s.SubjectCode
	left join (	select
			        ans.ParticipantExamId
			        ,count(1) as AnswersCount
		        from
			        ap_Answers ans
		        group by
			        ans.ParticipantExamId) as ansa(ParticipantExamId, AnswersCount) on pe.Id = ansa.ParticipantExamId
	left join (	select
			        oans.ParticipantExamId
			        ,count(1) as AnswersCount
		        from
			        ap_Answers oans
		        group by
			        oans.ParticipantExamId) as oansa(ParticipantExamId, AnswersCount) on pel.OralParticipantExamId = oansa.ParticipantExamId
	left join (select 
					ap.Id
					,ap.ParticipantExamId
					,ap.Status
				from 
					ap_Appeals ap
				where 
					ap.Id = (select 
								max(a.Id) as Id
							from
								ap_Appeals a
							where
								a.ParticipantExamId  = ap.ParticipantExamId)) as la on pe.Id = la.ParticipantExamId
where 
	p.ParticipantCode = @ParticipantCode
	and p.RegionId = @RegionId
    and not exists (select 1 from ap_ParticipantExamLinks opel where pe.Id = opel.OralParticipantExamId)
order by 
    e.ExamDate
end
go


create procedure GetGekDocuments @RegionId int, @ExamGlobalId int
as
begin
	select 
		e.ExamDate
		,s.SubjectName
		,dg.Number
		,dg.CreateDate
		,dg.Url
	from
		ap_GekDocuments dg
		join dat_Exams e on dg.ExamGlobalId = e.ExamGlobalId
		join dat_Subjects s on e.SubjectCode = s.SubjectCode
	where 
		dg.RegionId = @RegionId
		and dg.ExamGlobalId = @ExamGlobalId
end
go


create procedure GetParticipants @ParticipantHash nvarchar(50)
as
begin
	select
		p.ParticipantRbdId 
		,p.ParticipantHash
		,p.ParticipantCode
		,p.DocumentNumber
		,p.RegionId
	from 
		ap_Participants p
	where 
		p.ParticipantHash = @ParticipantHash 
end
go


create procedure GetRcoiInfo @RegionId int
as
begin
	select
		ri.Fio
		,ri.Phone
		,ri.Email
		,ri.HotLineData
		,ri.BlanksServer
        ,ri.CompositionBlanksServer
		,ri.Description
	from 
		ap_RegionInfo ri
	where 
		ri.RegionId = @RegionId	
end
go


create procedure GetRegionExamSettings
as
begin
 select
  se.RegionId
  ,se.ShowBlanks
  ,se.ShowResult
  ,se.ExamGlobalId
  ,gd.Number
  ,gd.CreateDate
  ,gd.Url
  ,cast (null as nvarchar(max)) as HotLineData
  ,cast (null as nvarchar(max)) as Description
  ,cast (null as nvarchar(255)) as BlanksServer
  ,cast (null as nvarchar(255)) as CompositionBlanksServer
  ,cast (0 as bit) as RowType
 from
  ap_ExamSettings se
  left join ap_GekDocuments gd on se.RegionId = gd.RegionId and se.ExamGlobalId = gd.ExamGlobalId
 union all
 select
  ri.RegionId
  ,cast (null as bit) as ShowBlanks
  ,cast (null as bit) as ShowResult
  ,cast (null as int) as ExamGlobalId
  ,cast (null as nvarchar(50)) as Number
  ,cast (null as datetime) as CreateDate
  ,cast (null as nvarchar(255)) as Url
  ,ri.HotLineData
  ,ri.Description
  ,ri.BlanksServer
  ,ri.CompositionBlanksServer
  ,cast (1 as bit) as RowType
 from
  ap_RegionInfo ri
 order by RegionId
end
go


create procedure GetUser @Id int = null, @Login nvarchar(255) = null
as
begin
    select
        u.UserId
        ,u.LoginName
        ,u.RegionId
        ,u.IsEnabled
        ,case when @Id is null then u.Password else '' end as Password
        ,ur.Role
    from
        Users u
        left join UserRoles ur on u.UserId = ur.UserId
    where
        (@Id is null or u.UserId = @Id)
        and (@Login is null or u.LoginName = @Login)
        and u.IsDeleted = 0
end
go


create procedure GetUsers 
@Id int = null,
@Login nvarchar(255) = null, 
@RegionId int = null, 
@Role int = null, 
@Take int = 1, 
@Skip int = 0 
as
begin
    with uu as
    (
        select
			row_number() over (order by u.LoginName) as RowNumber
            ,u.UserId
            ,u.LoginName
            ,u.RegionId
            ,r.RegionName
            ,ur.Role
            ,u.IsEnabled
        from
            Users u
            join UserRoles ur on u.UserId = ur.UserId
            left join rbdc_Regions r on u.RegionId = r.REGION
        where
            u.IsDeleted = 0
            and (@Id is null or u.UserId = @Id)
            and (@Login is null or u.LoginName like '%'+@Login+'%')
            and (@RegionId is null or u.RegionId = @RegionId)
            and (@Role is null or ur.Role = @Role)
    )
    select
        (select count(1) from uu) as UserCount
        ,uu.UserId
		,uu.LoginName 
		,uu.RegionId
		,uu.RegionName
		,uu.Role
		,uu.IsEnabled
    from
        uu 
	where 
		uu.RowNumber between @Skip + 1 and @Skip + @Take         
    order by
        uu.LoginName
end
go


create procedure LoadAnswers
as
begin
	merge into ap_Answers as s
	using AnswerDto as ts
	left join ap_Participants p on ts.Code = p.ParticipantCode and ts.RegionId = p.RegionId
	left join ap_ParticipantExams pe on p.Id = pe.ParticipantId and ts.ExamGlobalId = pe.ExamGlobalId
	on pe.Id = s.ParticipantExamId 
	and ts.TaskTypeCode = s.TaskTypeCode 
	and ts.TaskNumber = s.TaskNumber 
	when matched then 
	update set 
		s.AnswerValue = ts.AnswerValue
		,s.Mark = ts.Mark
	when not matched then 
	insert 
		(RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) 
	values 
		(ts.RegionId, pe.Id, ts.TaskTypeCode, ts.TaskNumber, ts.AnswerValue, ts.Mark);
end
go


create procedure LoadAppeals
as
begin
merge into ap_Appeals as s
		using AppealDto as ts
		left join ap_Participants p on ts.Code = p.ParticipantCode and ts.RegionId = p.RegionId
		left join ap_ParticipantExams pe on p.Id = pe.ParticipantId and ts.ExamGlobalId = pe.ExamGlobalId
		on pe.Id = s.ParticipantExamId 
		and ts.Status = s.Status 
		and ts.CreateDate = s.CreateDate 
		when not matched then 
		insert 
			(ParticipantExamId, Status, CreateDate) 
		values 
			(pe.Id, ts.Status, ts.CreateDate);
end
go


create procedure LoadBlankInfo
as
begin
	merge into ap_BlankInfo as s
	using BlankInfoDto as ts
	left join ap_Participants p on ts.Code = p.ParticipantCode and ts.RegionId = p.RegionId
	left join ap_ParticipantExams pe on p.Id = pe.ParticipantId and ts.ExamGlobalId = pe.ExamGlobalId
	on pe.Id = s.ParticipantExamId 
	and ts.BlankType = s.BlankType 
	when matched then 
	update set 
		s.Answer = ts.Answer
		,s.PrimaryMark = ts.PrimaryMark
		,s.Barcode = ts.Barcode
		,s.PageCount = ts.PageCount
		,s.CreateDate = ts.CreateDate
		,s.ProjectBatchId = ts.ProjectBatchId
		,s.ProjectName = ts.ProjectName
	when not matched then 
	insert 
		(ParticipantExamId, BlankType, Answer, PrimaryMark, Barcode, PageCount, CreateDate, ProjectBatchId, ProjectName) 
	values 
		(pe.Id, ts.BlankType, ts.Answer, ts.PrimaryMark, ts.Barcode, ts.PageCount, ts.CreateDate, ts.ProjectBatchId, ts.ProjectName);
end
go


create procedure LoadExams
as
begin
	merge into dat_Exams as s
	using 
    (
        select
            loaded.Id
            ,loaded.Date
            ,case when s.IsComposition = 1 then 1024 else loaded.WaveCode end as WaveCode
            ,loaded.SubjectCode
        from [ExamDto] as loaded
            left join dat_Subjects s on loaded.SubjectCode = s.SubjectCode
    ) as ts   
    on s.ExamGlobalId = ts.Id 
	when matched then 
	update set 
		s.ExamDate = ts.[Date]
		,s.WaveCode = ts.WaveCode
		,s.SubjectCode = ts.SubjectCode
	when not matched then 
	insert 
		(ExamGlobalId, ExamDate, WaveCode, SubjectCode) 
	values 
		(Id, [Date], WaveCode, SubjectCode);
end
go


create procedure LoadParticipantExamLinks 
as
begin
    merge into ap_ParticipantExamLinks as target
    using 
        ParticipantExamLinkDto as ts
        left join ap_Participants p on ts.Code = p.ParticipantCode and ts.RegionId = p.RegionId and ts.ExamGlobalId != ts.OralExamGlobalId
        left join ap_ParticipantExams pe on p.Id = pe.ParticipantId and ts.ExamGlobalId = pe.ExamGlobalId
        left join ap_ParticipantExams ope on p.Id = ope.ParticipantId and ts.OralExamGlobalId = ope.ExamGlobalId
    on pe.Id = target.ParticipantExamId
    when not matched and ope.Id is not null then
        insert (ParticipantExamId, OralParticipantExamId) values (pe.Id, ope.Id)
    when matched and ope.Id is not null then    
        update set OralParticipantExamId = ope.Id;
end
go


create procedure LoadParticipantExams 
as
begin
	merge into ap_ParticipantExams as s
	using ParticipantExamDto as ts
	left join ap_Participants p on ts.Code = p.ParticipantCode and ts.RegionId = p.RegionId
	on s.ParticipantId = p.Id and s.ExamGlobalId = ts.ExamGlobalId
	when matched then 
	update set 
		s.PrimaryMark = ts.PrimaryMark
		,s.TestMark = ts.TestMark
		,s.Mark5 = ts.Mark5
		,s.ProcessCondition = ts.ProcessCondition
		,s.CreateDate = ts.CreateDate
	when not matched then 
	insert 
		(ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) 
	values 
		(p.Id, ts.ExamGlobalId, ts.PrimaryMark, ts.TestMark, ts.Mark5, ts.ProcessCondition, ts.CreateDate, 0);
end
go


create procedure LoadParticipants 
as
begin
    delete from ap_ParticipantExamLinks
    where exists
    (
        select 1 from
            ParticipantDto dto
            join ap_Participants p 
                on dto.Code = p.ParticipantCode and dto.RegionId = p.RegionId and dto.IsDeleted = 1
            join ap_ParticipantExams pe
                on p.Id = pe.ParticipantId
        where
            pe.Id in (ParticipantExamId, OralParticipantExamId)
    )

	merge into ap_Participants as s
	using (select * from ParticipantDto
		   where IsDeleted = 1 ) as ts
	on ts.Code = s.ParticipantCode and ts.RegionId = s.RegionId
	when matched then 
	delete;

	merge into ap_Participants as s
	using (select * from ParticipantDto
		   where IsDeleted = 0 ) as ts
	on ts.Code = s.ParticipantCode and ts.RegionId = s.RegionId
	when matched then 
	update set 
		s.ParticipantCode = ts.Code
		,s.ParticipantHash = ts.Hash
		,s.DocumentNumber = ts.Document
		,s.RegionId = ts.RegionId
	when not matched then 
	insert 
        (ParticipantRbdId, ParticipantCode, ParticipantHash, DocumentNumber, RegionId) 
	values 
        (ts.RbdId, ts.Code, ts.Hash, ts.Document, ts.RegionId);
end
go


create procedure LoadRegions 
as
begin
	merge into rbdc_Regions as s
	using RegionDto as ts
	on s.Region = ts.Id
	when matched then 
	update 
	     set s.RegionName = ts.Name
	when not matched then 
	insert
		(Region, RegionName, Enable) 
	values 
		(ts.Id, ts.Name, 0);
end
go


create procedure LoadSubjects
as
begin
	merge into dat_Subjects as s
	using SubjectDto as ts
	on s.SubjectCode = ts.Code
	when matched then 
	update 
	set s.SubjectName = ts.Name
		,s.MinValue = ts.MinValue
		,s.IsComposition = ts.IsComposition
	when not matched then 
	insert 
		(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage)
	values
		(ts.Code, ts.Name, ts.MinValue, ts.IsComposition, 0, 0);
end
go

create type TaskSettingsTableType as table
(
    ClientId int not null
    ,ClientParentId int null
    ,TaskNumber int null
    ,DisplayNumber nvarchar(32) null
    ,MaxValue int null
    ,TaskTypeCode int not null
    ,SettingValue nvarchar(512) null
    ,IsTaskSetting bit not null
)    
go

create procedure MergeAnswersCriteria @Settings TaskSettingsTableType readonly, @SubjectCode int as
begin
begin tran

    delete from ap_BallSettings where SubjectCode = @SubjectCode
    delete from ap_TaskSettings where SubjectCode = @SubjectCode

    insert into ap_BallSettings(SubjectCode, TaskNumber, DisplayNumber, MaxValue, TaskTypeCode, LegalSymbols)
    select @SubjectCode, TaskNumber, DisplayNumber, MaxValue, TaskTypeCode, SettingValue
    from @Settings
    where IsTaskSetting = 0

    declare @IdMatching table(ClientId int, ClientParentId int, Id int)

    merge ap_TaskSettings ts
    using (select ClientId, ClientParentId, TaskNumber, DisplayNumber, MaxValue, TaskTypeCode, SettingValue from @Settings where IsTaskSetting = 1) as src
    on 0=1
    when not matched then
    insert (ParentTask, SubjectCode, TaskNumber, DisplayNumber, MaxValue, TaskTypeCode, CriteriaName)
    values(null, @SubjectCode, src.TaskNumber, src.DisplayNumber, src.MaxValue, src.TaskTypeCode, src.SettingValue)
    output src.ClientId as ClientId, src.ClientParentId as ClientParentId, inserted.Id as Id into @IdMatching;
    
    update
        ap_TaskSettings
    set
        ParentTask = 
        (
            select 
                p.Id
            from 
                @IdMatching p 
                join @IdMatching c on p.ClientId = c.ClientParentId
            where 
                c.Id = ap_TaskSettings.Id    
        )        
    where
        exists (select 1 from @IdMatching m where ap_TaskSettings.Id = m.Id)    
    
commit tran    
end
go

create type DocumentUrlsTableType as table
(
	Id		int				not null,
	Name	varchar(255)	not null,
	Url		varchar(255)	not null
)
go

create procedure MergeDocumentsUrls
@Documents DocumentUrlsTableType readonly
as
begin	
    merge into ap_DocumentUrls as du
    using @Documents as d
    on du.Id = d.Id
    when matched then update set du.Name = d.Name, du.Url = d.Url
    when not matched then insert values(Name, Url)
	when not matched by source then delete;
end
go


create procedure UpdateRegionState @RegionId int = null
as
begin
    update
        rbdc_Regions
    set
        Enable = case when 
            exists (select 1 from ap_ExamSettings where RegionId = REGION and ShowResult = 1)
            --and exists (select 1 from Users where IsEnabled = 1 and RegionId = REGION) 
            then 1 
            else 0 
        end
    where
        @RegionId is null or REGION = @RegionId
end
go

create type ExamSettingsTableType as table
(
	RegionId		int	not null,
	ExamGlobalId	int	not null,
	ShowResult		bit	not null,
	ShowBlanks		bit	not null
)
go

create procedure MergeExamSettings @ExamSettings ExamSettingsTableType readonly
as
begin	
    merge into ap_ExamSettings as es
    using 
        @ExamSettings as p
        join dat_Exams as e on p.ExamGlobalId = e.ExamGlobalId
        left join 
		(
            dat_Subjects as s 
            join dat_Subjects as ss on ss.IsComposition = s.IsComposition
            join dat_Exams as ee on ss.SubjectCode = ee.SubjectCode
        ) on s.SubjectCode = e.SubjectCode and s.IsComposition = 1    
    on (es.RegionId = p.RegionId and es.ExamGlobalId = isnull(ee.ExamGlobalId, p.ExamGlobalId))
    when matched then update set es.ShowResult = p.ShowResult, es.ShowBlanks = p.ShowBlanks
    when not matched then insert values(RegionId, isnull(ee.ExamGlobalId, p.ExamGlobalId), ShowResult, ShowBlanks);
    
    exec UpdateRegionState null
end
go


create procedure MergeGekDocuments @RegionId int, @ExamGlobalId int, @Number nvarchar(50), @CreateDate datetime, @Url nvarchar(255) = null
as
begin
	if exists(select 
					1
			  from 
					ap_GekDocuments gd 
			  where 
					gd.RegionId = @RegionId 
					and gd.ExamGlobalId = @ExamGlobalId)
		begin
		    update ap_GekDocuments 
			set Number = @Number,
				CreateDate = @CreateDate,
				Url = @Url
			where
				RegionId = @RegionId 
				and ExamGlobalId = @ExamGlobalId 
		end
	else
		begin
			insert into ap_GekDocuments (RegionId, ExamGlobalId, Number, CreateDate, Url)
			values (@RegionId, @ExamGlobalId, @Number, @CreateDate, @Url)
		end	
	end
go


create procedure MergeRcoiInfo 
@RegionId int, 
@Fio nvarchar(255) = null, 
@Phone nvarchar(255) = null, 
@Email nvarchar(255) = null, 
@HotLineData nvarchar(max) = null,
@BlanksServer nvarchar(255) = null,
@CompositionBlanksServer nvarchar(255) = null,
@Description nvarchar(max) = null 
as
begin
	if exists(select 
					1
			  from 
					ap_RegionInfo ri 
			  where 
					ri.RegionId = @RegionId)
		begin
		    update ap_RegionInfo 
			set Fio = @Fio,
				Phone = @Phone,
				Email = @Email,
				HotLineData = @HotLineData,
				BlanksServer = @BlanksServer,
                CompositionBlanksServer = @CompositionBlanksServer,
				Description = @Description 
			where
				RegionId = @RegionId
		end
	else
		begin
			insert into ap_RegionInfo (Fio, Phone, Email, HotLineData, BlanksServer, CompositionBlanksServer, Description, RegionId)
			values (@Fio, @Phone, @Email, @HotLineData, @BlanksServer, @CompositionBlanksServer, @Description, @RegionId)
		end	
	end
go


create procedure ResetPassword @Id int, @Password nvarchar(255) as
begin
    update
        Users
    set
        Password = @Password
    where
        UserId = @Id        
end
go


create procedure SetExamResultCancellation
@ParticipantCode nvarchar(50) = null, 
@RegionId int, 
@ExamGlobalId int, 
@IsCancelled bit as
begin
	declare @examId int
	declare @additionalExamId int
	select
		@examId = pe.Id, 
		@additionalExamId = ape.Id
	from
		ap_ParticipantExams pe
		join ap_Participants p on pe.ParticipantId = p.Id
		left join ap_ParticipantExamLinks pel on (pel.ParticipantExamId = pe.Id or pel.OralParticipantExamId = pe.Id)
		left join ap_ParticipantExams ape on (pel.ParticipantExamId = ape.Id or pel.OralParticipantExamId = ape.Id)
	where
        p.ParticipantCode = @ParticipantCode
        and p.RegionId = @RegionId
		and pe.ExamGlobalId = @ExamGlobalId
		and (ape.Id is null or ape.Id != pe.Id)

    update
        ap_ParticipantExams
    set
        IsHidden = @IsCancelled
    where
		Id in (@examId, @additionalExamId)
end
go


create procedure SetUserAndRegionActivation @Id int, @State bit as
begin
    update
        Users
    set
        IsEnabled = @State
    where
        UserId = @Id     
    
    exec UpdateRegionState (select RegionId from Users where @Id = UserId)
end
go

create type CompositionPageCountTableType as table
(
    Barcode nvarchar(50)
    ,ProjectBatchId int
    ,ProjectName nvarchar(500)
    ,PageCount int
)
go
    

create procedure UpdateCompositionPageCount @RegionId int, @ServerData CompositionPageCountTableType readonly
as
begin
	declare @UpdatedBlankInfos table 
	(
		ParticipantExamId int
		,BlankType int
		,CompositionPageCount int
	)

	merge
		ap_BlankInfo as target
	using
		(select distinct * from @ServerData) as source
	on 
	(
		source.Barcode = target.Barcode 
		and source.ProjectBatchId = target.ProjectBatchId 
		and source.ProjectName = target.ProjectName 
		and (target.CompositionPageCount is null or source.PageCount != target.CompositionPageCount)
		and exists
		(
			select
				1
			from
				ap_ParticipantExams pe
				join ap_Participants p on 
					pe.ParticipantId = p.Id 
				where
					pe.Id = target.ParticipantExamId 
					and p.RegionId = @RegionId    
		)
	)
	when matched then
		update set CompositionPageCount = source.PageCount
	output
		inserted.ParticipantExamId
		,inserted.BlankType
		,inserted.CompositionPageCount
	into @UpdatedBlankInfos;


	select
		ubi.BlankType
		,ubi.CompositionPageCount
		,p.RegionId
		,p.ParticipantCode
		,pe.ExamGlobalId
	from
		@UpdatedBlankInfos ubi
		join ap_ParticipantExams pe on pe.Id = ubi.ParticipantExamId
		join ap_Participants p on p.Id = pe.ParticipantId 
end
go


create procedure UpdateUser @Id int, @Password nvarchar(255) = null, @Role int, @RegionId int = null, @IsEnabled bit as
begin
    update
        Users
    set
        Password = isnull(@Password, Password)
        ,RegionId = @RegionId
        ,IsEnabled = @IsEnabled
    where
        UserId = @Id
           
    update
        UserRoles
    set
        Role = @Role
    where
        UserId = @Id        
end
go
