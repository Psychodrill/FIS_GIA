insert into Migrations(MigrationVersion, MigrationName) values (94, '094_2013_06_26_fbs.sql')
go

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubjectMapping_SubjectMapping]') AND parent_object_id = OBJECT_ID(N'[dbo].[SubjectMapping]'))
ALTER TABLE [dbo].[SubjectMapping] DROP CONSTRAINT [FK_SubjectMapping_SubjectMapping]
GO

/****** Object:  Table [dbo].[SubjectMapping]    Script Date: 06/26/2013 10:31:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubjectMapping]') AND type in (N'U'))
DROP TABLE [dbo].[SubjectMapping]
GO

/****** Object:  Table [dbo].[SubjectMapping]    Script Date: 06/26/2013 10:31:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SubjectMapping](
  [id] [int] IDENTITY(1,1) NOT NULL,
  [name] [nvarchar](250) NULL,
  [id_subject_new] [int] NULL,
  [code] [nvarchar](250) NULL,
 CONSTRAINT [PK_SubjectMapping] PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SubjectMapping]  WITH CHECK ADD  CONSTRAINT [FK_SubjectMapping_SubjectMapping] FOREIGN KEY([id_subject_new])
REFERENCES [dat].[Subjects] ([SubjectCode])
GO

ALTER TABLE [dbo].[SubjectMapping] CHECK CONSTRAINT [FK_SubjectMapping_SubjectMapping]
GO

INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Русский язык',1,'Russian')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Математика',2,'Mathematics')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Физика',3,'Physics')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Химия',4,'Chemistry')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Биология',6,'Biology')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('История',7,'RussiaHistory')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('География',8,'Geography')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Английский язык',9,'English')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Немецкий язык',10,'German')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Французcкий язык',11,'Franch')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Обществознание',12,'SocialScience')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Литература',18,'Literature')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Испанский язык',13,'Spanish')
INSERT INTO [dbo].[SubjectMapping] ([name],[id_subject_new],[code]) VALUES ('Информатика и ИКТ',5,'InformationScience')
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Subject]'))
DROP VIEW [dbo].[Subject]
GO

CREATE VIEW [dbo].[Subject]
AS
SELECT     a.id, a.code AS Code, c.SubjectName AS Name, a.id AS SortIndex, 1 AS IsActive
FROM         dbo.SubjectMapping AS a INNER JOIN
                      dat.Subjects AS c ON c.SubjectCode = a.id_subject_new

GO