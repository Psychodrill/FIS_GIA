USE [ApilationService_2014_Test]
GO
/****** Object:  Table [dbo].[rbdc_Regions]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rbdc_Regions](
	[REGION] [int] NOT NULL,
	[RegionName] [nvarchar](255) NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.rbdc_Regions] PRIMARY KEY CLUSTERED 
(
	[REGION] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dat_Subjects]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dat_Subjects](
	[SubjectCode] [int] NOT NULL,
	[SubjectName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_dbo.dat_Subjects] PRIMARY KEY CLUSTERED 
(
	[SubjectCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dat_Exams]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dat_Exams](
	[ExamGlobalId] [int] NOT NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[ExamDate] [datetime] NOT NULL,
	[WaveCode] [int] NOT NULL,
	[WaveName] [nvarchar](255) NOT NULL,
	[SubjectCode] [int] NOT NULL,
 CONSTRAINT [PK_dbo.dat_Exams] PRIMARY KEY CLUSTERED 
(
	[ExamGlobalId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_TaskSettings]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_TaskSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentTask] [int] NULL,
	[SubjectCode] [int] NOT NULL,
	[TaskTypeCode] [int] NOT NULL,
	[TaskNumber] [int] NULL,
	[MaxValue] [int] NULL,
	[CriteriaName] [nvarchar](150) NULL,
 CONSTRAINT [PK_dbo.ap_TaskSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_RegionInfo]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_RegionInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fio] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[HotLineData] [nvarchar](max) NULL,
	[WebServerBlanks] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[Region] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ap_RegionInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_ExamLoaded]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_ExamLoaded](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExamGlobalId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ap_ExamLoaded] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_DocumentUrl]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_DocumentUrl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Url] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.ap_DocumentUrl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_MarkBorders]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_MarkBorders](
	[SubjectCode] [int] IDENTITY(1,1) NOT NULL,
	[MaxValue] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ap_MarkBorders] PRIMARY KEY CLUSTERED 
(
	[SubjectCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [nvarchar](255) NOT NULL,
	[Fio] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[Email] [nvarchar](max) NULL,
	[Password] [nvarchar](255) NULL,
	[Enable] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_Operators]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_Operators](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OperatorPosition] [nvarchar](150) NULL,
	[UserId] [int] NOT NULL,
	[RegionId] [int] NULL,
	[Family] [nvarchar](150) NULL,
	[Name] [nvarchar](150) NULL,
	[SecondName] [nvarchar](150) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ap_Operators] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_BallSettings]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_BallSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubjectCode] [int] NOT NULL,
	[TaskNumber] [int] NOT NULL,
	[MaxValue] [int] NOT NULL,
	[TaskTypeCode] [int] NOT NULL,
	[LegalSymbols] [nvarchar](512) NULL,
 CONSTRAINT [PK_dbo.ap_BallSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_AppilsSettings]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_AppilsSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[ExamGlobalId] [int] NOT NULL,
	[ShowResult] [bit] NOT NULL,
	[ApplayAppils] [bit] NOT NULL,
	[ApilDateFrom] [datetime] NULL,
	[ApilDateTo] [datetime] NULL,
	[ShowBlanks] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ap_AppilsSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_GEKDocuments]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_GEKDocuments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[ExamGlobalId] [int] NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Url] [nvarchar](255) NULL,
 CONSTRAINT [PK_dbo.ap_GEKDocuments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_FctUser]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_FctUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RegionId] [int] NULL,
	[Family] [nvarchar](150) NULL,
	[Name] [nvarchar](150) NULL,
	[SecondName] [nvarchar](150) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ap_FctUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_Participants]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_Participants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantRbdId] [uniqueidentifier] NOT NULL,
	[ParticipantCode] [nvarchar](12) NULL,
	[ParticipantHash] [nvarchar](50) NULL,
	[DocumentNumber] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserId] [int] NULL,
	[RegionId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ap_Participants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_ParticipantExams]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_ParticipantExams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantId] [int] NOT NULL,
	[ExamGlobalId] [int] NOT NULL,
	[PrimaryMark] [int] NOT NULL,
	[TestMark] [int] NOT NULL,
	[Mark5] [int] NOT NULL,
	[ProcessCondition] [int] NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ap_ParticipantExams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_ParticipantExamHideSettings]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_ParticipantExamHideSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantId] [int] NOT NULL,
	[ExamGlobalId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ap_ParticipantExamHideSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_BlankInfo]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_BlankInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantExamId] [int] NOT NULL,
	[BlankType] [int] NOT NULL,
	[Answer] [nvarchar](50) NULL,
	[PrimaryMark] [int] NOT NULL,
	[Barcode] [nvarchar](50) NULL,
	[PageCount] [int] NOT NULL,
	[DateCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.ap_BlankInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_Appils]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_Appils](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantExams] [int] NOT NULL,
	[AppilType] [bit] NOT NULL,
	[Station] [int] NULL,
	[DateCreate] [datetime] NOT NULL,
	[ReviewType] [int] NULL,
	[Agent] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Mail] [nvarchar](max) NULL,
	[AgentType] [int] NULL,
 CONSTRAINT [PK_dbo.ap_Appils] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_Answers]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_Answers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[ParticipantExamId] [int] NOT NULL,
	[TaskTypeCode] [int] NOT NULL,
	[TaskNumber] [int] NOT NULL,
	[AnswerValue] [nvarchar](255) NOT NULL,
	[Mark] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ap_Answers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ap_AppilsHistory]    Script Date: 02/17/2015 11:20:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ap_AppilsHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppilId] [int] NOT NULL,
	[AppilStatus] [int] NOT NULL,
	[OperatorId] [int] NULL,
	[DateCreate] [datetime] NOT NULL,
	[SignDocument] [bit] NOT NULL,
	[AgentType] [int] NULL,
 CONSTRAINT [PK_dbo.ap_AppilsHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Answers_dbo.ap_ParticipantExams_ParticipantExamId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Answers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Answers_dbo.ap_ParticipantExams_ParticipantExamId] FOREIGN KEY([ParticipantExamId])
REFERENCES [dbo].[ap_ParticipantExams] ([Id])
GO
ALTER TABLE [dbo].[ap_Answers] CHECK CONSTRAINT [FK_dbo.ap_Answers_dbo.ap_ParticipantExams_ParticipantExamId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Answers_dbo.rbdc_Regions_RegionId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Answers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Answers_dbo.rbdc_Regions_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[rbdc_Regions] ([REGION])
GO
ALTER TABLE [dbo].[ap_Answers] CHECK CONSTRAINT [FK_dbo.ap_Answers_dbo.rbdc_Regions_RegionId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Appils_dbo.ap_ParticipantExams_ParticipantExams]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Appils]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Appils_dbo.ap_ParticipantExams_ParticipantExams] FOREIGN KEY([ParticipantExams])
REFERENCES [dbo].[ap_ParticipantExams] ([Id])
GO
ALTER TABLE [dbo].[ap_Appils] CHECK CONSTRAINT [FK_dbo.ap_Appils_dbo.ap_ParticipantExams_ParticipantExams]
GO
/****** Object:  ForeignKey [FK_dbo.ap_AppilsHistory_dbo.ap_Appils_AppilId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_AppilsHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_AppilsHistory_dbo.ap_Appils_AppilId] FOREIGN KEY([AppilId])
REFERENCES [dbo].[ap_Appils] ([Id])
GO
ALTER TABLE [dbo].[ap_AppilsHistory] CHECK CONSTRAINT [FK_dbo.ap_AppilsHistory_dbo.ap_Appils_AppilId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_AppilsHistory_dbo.ap_Operators_OperatorId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_AppilsHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_AppilsHistory_dbo.ap_Operators_OperatorId] FOREIGN KEY([OperatorId])
REFERENCES [dbo].[ap_Operators] ([Id])
GO
ALTER TABLE [dbo].[ap_AppilsHistory] CHECK CONSTRAINT [FK_dbo.ap_AppilsHistory_dbo.ap_Operators_OperatorId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_AppilsSettings_dbo.dat_Exams_ExamGlobalId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_AppilsSettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_AppilsSettings_dbo.dat_Exams_ExamGlobalId] FOREIGN KEY([ExamGlobalId])
REFERENCES [dbo].[dat_Exams] ([ExamGlobalId])
GO
ALTER TABLE [dbo].[ap_AppilsSettings] CHECK CONSTRAINT [FK_dbo.ap_AppilsSettings_dbo.dat_Exams_ExamGlobalId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_AppilsSettings_dbo.rbdc_Regions_RegionId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_AppilsSettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_AppilsSettings_dbo.rbdc_Regions_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[rbdc_Regions] ([REGION])
GO
ALTER TABLE [dbo].[ap_AppilsSettings] CHECK CONSTRAINT [FK_dbo.ap_AppilsSettings_dbo.rbdc_Regions_RegionId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_BallSettings_dbo.dat_Subjects_SubjectCode]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_BallSettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_BallSettings_dbo.dat_Subjects_SubjectCode] FOREIGN KEY([SubjectCode])
REFERENCES [dbo].[dat_Subjects] ([SubjectCode])
GO
ALTER TABLE [dbo].[ap_BallSettings] CHECK CONSTRAINT [FK_dbo.ap_BallSettings_dbo.dat_Subjects_SubjectCode]
GO
/****** Object:  ForeignKey [FK_dbo.ap_BlankInfo_dbo.ap_ParticipantExams_ParticipantExamId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_BlankInfo]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_BlankInfo_dbo.ap_ParticipantExams_ParticipantExamId] FOREIGN KEY([ParticipantExamId])
REFERENCES [dbo].[ap_ParticipantExams] ([Id])
GO
ALTER TABLE [dbo].[ap_BlankInfo] CHECK CONSTRAINT [FK_dbo.ap_BlankInfo_dbo.ap_ParticipantExams_ParticipantExamId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_FctUser_dbo.rbdc_Regions_RegionId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_FctUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_FctUser_dbo.rbdc_Regions_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[rbdc_Regions] ([REGION])
GO
ALTER TABLE [dbo].[ap_FctUser] CHECK CONSTRAINT [FK_dbo.ap_FctUser_dbo.rbdc_Regions_RegionId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_FctUser_dbo.Users_UserId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_FctUser]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_FctUser_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ap_FctUser] CHECK CONSTRAINT [FK_dbo.ap_FctUser_dbo.Users_UserId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_GEKDocuments_dbo.dat_Exams_ExamGlobalId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_GEKDocuments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_GEKDocuments_dbo.dat_Exams_ExamGlobalId] FOREIGN KEY([ExamGlobalId])
REFERENCES [dbo].[dat_Exams] ([ExamGlobalId])
GO
ALTER TABLE [dbo].[ap_GEKDocuments] CHECK CONSTRAINT [FK_dbo.ap_GEKDocuments_dbo.dat_Exams_ExamGlobalId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_GEKDocuments_dbo.rbdc_Regions_RegionId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_GEKDocuments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_GEKDocuments_dbo.rbdc_Regions_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[rbdc_Regions] ([REGION])
GO
ALTER TABLE [dbo].[ap_GEKDocuments] CHECK CONSTRAINT [FK_dbo.ap_GEKDocuments_dbo.rbdc_Regions_RegionId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Operators_dbo.rbdc_Regions_RegionId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Operators]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Operators_dbo.rbdc_Regions_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[rbdc_Regions] ([REGION])
GO
ALTER TABLE [dbo].[ap_Operators] CHECK CONSTRAINT [FK_dbo.ap_Operators_dbo.rbdc_Regions_RegionId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Operators_dbo.Users_UserId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Operators]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Operators_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ap_Operators] CHECK CONSTRAINT [FK_dbo.ap_Operators_dbo.Users_UserId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_ParticipantExamHideSettings_dbo.ap_Participants_ParticipantId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_ParticipantExamHideSettings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_ParticipantExamHideSettings_dbo.ap_Participants_ParticipantId] FOREIGN KEY([ParticipantId])
REFERENCES [dbo].[ap_Participants] ([Id])
GO
ALTER TABLE [dbo].[ap_ParticipantExamHideSettings] CHECK CONSTRAINT [FK_dbo.ap_ParticipantExamHideSettings_dbo.ap_Participants_ParticipantId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_ParticipantExams_dbo.ap_Participants_ParticipantId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_ParticipantExams]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_ParticipantExams_dbo.ap_Participants_ParticipantId] FOREIGN KEY([ParticipantId])
REFERENCES [dbo].[ap_Participants] ([Id])
GO
ALTER TABLE [dbo].[ap_ParticipantExams] CHECK CONSTRAINT [FK_dbo.ap_ParticipantExams_dbo.ap_Participants_ParticipantId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_ParticipantExams_dbo.dat_Exams_ExamGlobalId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_ParticipantExams]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_ParticipantExams_dbo.dat_Exams_ExamGlobalId] FOREIGN KEY([ExamGlobalId])
REFERENCES [dbo].[dat_Exams] ([ExamGlobalId])
GO
ALTER TABLE [dbo].[ap_ParticipantExams] CHECK CONSTRAINT [FK_dbo.ap_ParticipantExams_dbo.dat_Exams_ExamGlobalId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Participants_dbo.rbdc_Regions_RegionId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Participants]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Participants_dbo.rbdc_Regions_RegionId] FOREIGN KEY([RegionId])
REFERENCES [dbo].[rbdc_Regions] ([REGION])
GO
ALTER TABLE [dbo].[ap_Participants] CHECK CONSTRAINT [FK_dbo.ap_Participants_dbo.rbdc_Regions_RegionId]
GO
/****** Object:  ForeignKey [FK_dbo.ap_Participants_dbo.Users_UserId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[ap_Participants]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ap_Participants_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ap_Participants] CHECK CONSTRAINT [FK_dbo.ap_Participants_dbo.Users_UserId]
GO
/****** Object:  ForeignKey [FK_dbo.UserRoles_dbo.Users_UserId]    Script Date: 02/17/2015 11:20:13 ******/
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId]
GO
