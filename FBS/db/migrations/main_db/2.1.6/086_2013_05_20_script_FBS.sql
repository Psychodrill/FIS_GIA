insert into Migrations(MigrationVersion, MigrationName) values (86, '086_2013_05_20_script_FBS.sql')

if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where a.name = 'Subjects' and b.name='dat') drop TABLE [dat].[Subjects]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='dat' and a.name = 'Waves' ) drop TABLE [dat].[Waves]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='prn' and a.name = 'Certificates' ) drop TABLE [prn].[Certificates]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='prn' and a.name = 'CertificatesMarks' ) drop TABLE [prn].[CertificatesMarks]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='prn' and a.name = 'CancelledCertificates' ) drop TABLE [prn].[CancelledCertificates]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='rbd' and a.name = 'Participants' ) drop TABLE [rbd].[Participants]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='rbdc' and a.name = 'DocumentTypes' ) drop TABLE [rbdc].[DocumentTypes]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='rbdc' and a.name = 'ParticipantCategories' ) drop TABLE [rbdc].[ParticipantCategories]
GO
if exists(select * from sys.tables a join sys.schemas b on a.schema_id=b.schema_id where b.name='rbdc' and a.name = 'Regions' ) drop TABLE [rbdc].[Regions]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dat].[Subjects]') AND name = N'PK_dat_Subjects')
ALTER TABLE [dat].[Subjects] DROP CONSTRAINT [PK_dat_Subjects]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dat].[Subjects]') AND name = N'SubjectCode')
ALTER TABLE [dat].[Subjects] DROP CONSTRAINT [SubjectCode]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dat].[Waves]') AND name = N'PK_DAT_WAVES')
ALTER TABLE [dat].[Waves] DROP CONSTRAINT [PK_DAT_WAVES]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dat].[Waves]') AND name = N'IX_dat_Waves')
ALTER TABLE [dat].[Waves] DROP CONSTRAINT [IX_dat_Waves]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prn].[Certificates]') AND name = N'PK_prn_Certificates')
ALTER TABLE [prn].[Certificates] DROP CONSTRAINT [PK_prn_Certificates]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prn].[CertificatesMarks]') AND name = N'PK_prn_CertificatesMarks')
ALTER TABLE [prn].[CertificatesMarks] DROP CONSTRAINT [PK_prn_CertificatesMarks]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[prn].[CancelledCertificates]') AND name = N'PK_prn_CancelledCertificates')
ALTER TABLE [prn].[CancelledCertificates] DROP CONSTRAINT [PK_prn_CancelledCertificates]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[rbdc].[DocumentTypes]') AND name = N'PK_DocumentTypes')
ALTER TABLE [rbdc].[DocumentTypes] DROP CONSTRAINT [PK_DocumentTypes]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[rbdc].[ParticipantCategories]') AND name = N'PK_rbd_ParticipantCategories')
ALTER TABLE [rbdc].[ParticipantCategories] DROP CONSTRAINT [PK_rbd_ParticipantCategories]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[rbdc].[Regions]') AND name = N'PK_Regions1')
ALTER TABLE [rbdc].[Regions] DROP CONSTRAINT [PK_Regions1]
go
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[rbd].[Participants]') AND name = N'PK_Participants')
ALTER TABLE [rbd].[Participants] DROP CONSTRAINT [PK_Participants]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[rbd].[FK_rbd_Participants_rbd_DocumentTypes]') 
AND parent_object_id = OBJECT_ID(N'[rbd].[Participants]'))
ALTER TABLE [rbd].[Participants]  drop CONSTRAINT [FK_rbd_Participants_rbd_DocumentTypes]
go
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[rbd].[FK_rbd_Participants_rbdc_ParticipantCategories]') 
AND parent_object_id = OBJECT_ID(N'[rbd].[Participants]'))
ALTER TABLE [rbd].[Participants]  drop  CONSTRAINT [FK_rbd_Participants_rbdc_ParticipantCategories] 
go
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_rbd_Participants_ParticipantCode]') AND type = 'D')
ALTER TABLE [rbd].[Participants] DROP CONSTRAINT [DF_rbd_Participants_ParticipantCode]
go
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_rbd_Participants_BirthDay]') AND type = 'D')
ALTER TABLE [rbd].[Participants] DROP CONSTRAINT [DF_rbd_Participants_BirthDay]
go

if exists(select * from sys.schemas where name='dat' ) drop SCHEMA [dat]
go
if exists(select * from sys.schemas where name='prn' ) drop SCHEMA [prn]
go
if exists(select * from sys.schemas where name='rbdc' ) drop SCHEMA [rbdc]
go
if exists(select * from sys.schemas where name='rbd' ) drop SCHEMA [rbd]
go
CREATE SCHEMA [dat] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [prn]    Script Date: 04/08/2013 11:15:58 ******/
CREATE SCHEMA [prn] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [rbdc]    Script Date: 04/08/2013 11:15:58 ******/
CREATE SCHEMA [rbdc] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [rbd]    Script Date: 04/08/2013 11:15:58 ******/
CREATE SCHEMA [rbd] AUTHORIZATION [dbo]
GO

CREATE TABLE [dat].[Subjects](
  [SubjectCode] [int] NOT NULL,
  [SubjectName] [varchar](100) NULL,
 CONSTRAINT [PK_dat_Subjects] PRIMARY KEY CLUSTERED 
(
  [SubjectCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [SubjectCode] UNIQUE NONCLUSTERED 
(
  [SubjectCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (1, N'������� ����')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (2, N'����������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (3, N'������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (4, N'�����')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (5, N'����������� � ���')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (6, N'��������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (7, N'�������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (8, N'���������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (9, N'���������� ����')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (10, N'�������� ����')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (11, N'�������c��� ����')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (12, N'��������������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (18, N'����������')
INSERT [dat].[Subjects] ([SubjectCode], [SubjectName]) VALUES (13, N'��������� ����')
go
CREATE TABLE [dat].[Waves](
  [WaveCode] [int] NOT NULL,
  [WaveName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_DAT_WAVES] PRIMARY KEY CLUSTERED 
(
  [WaveCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_dat_Waves] UNIQUE NONCLUSTERED 
(
  [WaveCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dat].[Waves] ([WaveCode], [WaveName]) VALUES (0, N'������� ����')
INSERT [dat].[Waves] ([WaveCode], [WaveName]) VALUES (1, N'��������� ����')
INSERT [dat].[Waves] ([WaveCode], [WaveName]) VALUES (2, N'�������� ����')
INSERT [dat].[Waves] ([WaveCode], [WaveName]) VALUES (3, N'�������������� ����')
GO

CREATE TABLE [prn].[Certificates](
  [CertificateID] [uniqueidentifier] NOT NULL,
  [UseYear] [int] NOT NULL,
  [REGION] [int] NOT NULL,
  [Wave] [int] NOT NULL,
  [LicenseNumber] [nvarchar](18) NOT NULL,
  [TypographicNumber] [nvarchar](12) NULL,
  [ParticipantFK] [uniqueidentifier],
  [Cancelled] [bit] NOT NULL,
  [CreateDate] [datetime] NOT NULL,
  [UpdateDate] [datetime] NOT NULL,
  [ImportCreateDate] [datetime] NULL,
  [ImportUpdateDate] [datetime] NULL,
 CONSTRAINT [PK_prn_Certificates] PRIMARY KEY CLUSTERED 
(
  [CertificateID] ASC,
  [REGION] ASC,
  [UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
) ON [CRT]

GO

CREATE TABLE [prn].[CertificatesMarks](
  [CertificateMarkID] [uniqueidentifier] NOT NULL,
  [UseYear] [int] NOT NULL,
  [REGION] [int] NOT NULL,
  [CertificateFK] [uniqueidentifier] NULL,
  [ParticipantFK] [uniqueidentifier] NOT NULL,
  [SubjectCode] [int] NOT NULL,
  [Mark] [int] NOT NULL,
  [HasAppeal] [bit] NOT NULL,
  [PrintedMarkID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_prn_CertificatesMarks] PRIMARY KEY CLUSTERED 
(
  [CertificateMarkID] ASC,
  [REGION] ASC,
  [UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
) ON [CRT]
GO

CREATE TABLE [prn].[CancelledCertificates](
  [UseYear] [int] NOT NULL,
  [REGION] [int] NOT NULL,
  [CertificateFK] [uniqueidentifier] NOT NULL,
  [Reason] [nvarchar](255) NULL,
 CONSTRAINT [PK_prn_CancelledCertificates] PRIMARY KEY CLUSTERED 
(
  [CertificateFK] ASC,
  [REGION] ASC,
  [UseYear] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [CRT]
) ON [CRT]
GO

CREATE TABLE [rbdc].[DocumentTypes](
  [DocumentTypeCode] [int] NOT NULL,
  [DocumentTypeName] [varchar](255) NULL,
 CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED 
(
  [DocumentTypeCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
) ON [RBD]
GO

CREATE TABLE [rbdc].[ParticipantCategories](
  [CategoryID] [int] NOT NULL,
  [CategoryCode] [int] NOT NULL,
  [CategoryName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_rbd_ParticipantCategories] PRIMARY KEY CLUSTERED 
(
  [CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
) ON [RBD]

CREATE TABLE [rbd].[Participants](
  [ParticipantID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
    [UseYear] [int] NOT NULL,
  [REGION] [int] NOT NULL,
  [ParticipantCode] [varchar](16) NULL,
  [Surname] [varchar](80) NOT NULL,
  [Name] [varchar](80) NOT NULL,
  [SecondName] [varchar](80) NULL,
  [DocumentSeries] [varchar](9) NULL,
  [DocumentNumber] [varchar](10) NOT NULL,
  [DocumentTypeCode] [int] NOT NULL,
  [Sex] [bit] NOT NULL,
  [BirthDay] [datetime] NOT NULL,
  [FinishRegion] [int] NULL,
  [ParticipantCategoryFK] [int] NOT NULL,
  [CreateDate] [datetime] NOT NULL,
  [UpdateDate] [datetime] NOT NULL,
  [ImportCreateDate] [datetime] NULL,
  [ImportUpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Participants] PRIMARY KEY CLUSTERED 
(
  [ParticipantID] ASC,
  [Region] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
) ON [RBD]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [rbd].[Participants]  WITH NOCHECK ADD  CONSTRAINT [FK_rbd_Participants_rbd_DocumentTypes] FOREIGN KEY([DocumentTypeCode])
REFERENCES [rbdc].[DocumentTypes] ([DocumentTypeCode])
GO

ALTER TABLE [rbd].[Participants] CHECK CONSTRAINT [FK_rbd_Participants_rbd_DocumentTypes]
GO

ALTER TABLE [rbd].[Participants]  WITH NOCHECK ADD  CONSTRAINT [FK_rbd_Participants_rbdc_ParticipantCategories] FOREIGN KEY([ParticipantCategoryFK])
REFERENCES [rbdc].[ParticipantCategories] ([CategoryID])
GO

ALTER TABLE [rbd].[Participants] CHECK CONSTRAINT [FK_rbd_Participants_rbdc_ParticipantCategories]
GO

ALTER TABLE [rbd].[Participants] ADD  CONSTRAINT [DF_rbd_Participants_ParticipantCode]  DEFAULT ('') FOR [ParticipantCode]
GO

ALTER TABLE [rbd].[Participants] ADD  CONSTRAINT [DF_rbd_Participants_BirthDay]  DEFAULT ('19000101') FOR [BirthDay]
GO

CREATE TABLE [rbdc].[Regions](
  [REGION] [int] NOT NULL,
  [RegionName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Regions1] PRIMARY KEY CLUSTERED 
(
  [REGION] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [RBD]
) ON [RBD]
GO
SET ANSI_PADDING OFF
GO
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (1, N'���������� ������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (2, N'���������� ������������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (3, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (4, N'���������� �����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (5, N'���������� ��������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (6, N'���������� ���������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (7, N'���������-���������� ����������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (8, N'���������� ��������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (9, N'���������-���������� ����������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (10, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (11, N'���������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (12, N'���������� ����� ��')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (13, N'���������� ��������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (14, N'���������� ���� (������)')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (15, N'���������� �������� ������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (16, N'���������� ���������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (17, N'���������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (18, N'���������� ����������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (19, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (20, N'��������� ����������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (21, N'��������� ����������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (22, N'��������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (23, N'������������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (24, N'������������ ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (25, N'���������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (26, N'�������������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (27, N'����������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (28, N'�������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (29, N'������������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (30, N'������������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (31, N'������������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (32, N'�������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (33, N'������������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (34, N'������������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (35, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (36, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (37, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (38, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (39, N'��������������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (40, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (41, N'���������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (42, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (43, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (44, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (45, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (46, N'������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (47, N'������������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (48, N'�������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (49, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (50, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (51, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (52, N'������������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (53, N'������������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (54, N'������������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (55, N'������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (56, N'������������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (57, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (58, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (59, N'�������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (60, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (61, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (62, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (63, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (64, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (65, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (66, N'������������ �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (67, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (68, N'���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (69, N'�������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (70, N'������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (71, N'�������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (72, N'��������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (73, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (74, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (75, N'������������� ����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (76, N'����������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (77, N'�. ������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (78, N'�. �����-���������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (79, N'��������� ���������� �������')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (83, N'�������� ���������� �����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (86, N'�����-���������� ���������� �����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (87, N'��������� ���������� �����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (89, N'�����-�������� ���������� �����')
INSERT [rbdc].[Regions] ([REGION], [RegionName]) VALUES (90, N'��, ����������� �� ��������� ��')
go

if not exists(select * from sys.columns where name='SourceCertificateIdGuid' and object_name(object_id)='CommonNationalExamCertificateRequest')
alter table CommonNationalExamCertificateRequest add SourceCertificateIdGuid uniqueidentifier
go
if not exists(select * from sys.columns where name='SourceCertificateIdGuid' and object_name(object_id)='CommonNationalExamCertificateCheck')
alter table CommonNationalExamCertificateCheck add SourceCertificateIdGuid uniqueidentifier
go
if not exists(select * from sys.columns where name='CertificateIdGuid' and object_name(object_id)='OrganizationCertificateChecks')
alter table OrganizationCertificateChecks add CertificateIdGuid uniqueidentifier
go
if not exists(select * from sys.columns where name='idGUID' and object_name(object_id)='ExamCertificateUniqueChecks')
alter table ExamCertificateUniqueChecks add idGUID uniqueidentifier
go
if not exists(select * from sys.columns where name='idtemp' and object_name(object_id)='CommonNationalExamCertificateCheck')
alter table CommonNationalExamCertificateCheck add idtemp bigint
go
if not exists(select * from sys.columns where name='SourceCertificateSubjectIdGuid' and object_name(object_id)='CommonNationalExamCertificateSubjectCheck')
alter table CommonNationalExamCertificateSubjectCheck add SourceCertificateSubjectIdGuid uniqueidentifier
go

drop view [dbo].[AuthenticationEventLog] 
go
alter table EventLog alter column SourceEntityId nvarchar(4000)
go
create view [dbo].[AuthenticationEventLog]   
as
select
  event_log.Id EventId 
  , event_log.Date Date
  , event_log.Ip Ip
  , event_log.SourceEntityId AccountId
  , case 
    when event_log.EventCode = 'USR_VERIFY'  
      then Convert(bit, dbo.GetEventParam(event_log.EventParams, 4)) 
      else 1
  end IsPasswordValid
  , case 
    when event_log.EventCode = 'USR_VERIFY'
      then Convert(bit, dbo.GetEventParam(event_log.EventParams, 5)) 
      else 1
  end IsIpValid
from   
  dbo.EventLog event_log
where 
  ((event_log.EventCode = 'USR_VERIFY'
      and not event_log.AccountId is null)
    or (event_log.EventCode = 'USR_REG'
      and event_log.AccountId is null))
GO                      
                      
CREATE VIEW [dbo].[vw_Examcertificate]
AS
SELECT     b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, b.UseYear AS Year, 
                      a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, b.TypographicNumber, a.ParticipantID AS ParticipantsID, 
                      REPLACE(REPLACE(LTRIM(RTRIM(a.Name)) + LTRIM(RTRIM(a.Surname)) + LTRIM(RTRIM(a.SecondName)), '�', '�'), ' ', '') AS FIO, a.ParticipantID, b.CreateDate
FROM         rbd.Participants AS a INNER JOIN
                      prn.Certificates AS b ON b.ParticipantFK = a.ParticipantID 
go