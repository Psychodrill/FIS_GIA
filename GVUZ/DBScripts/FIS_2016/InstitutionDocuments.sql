USE [gvuz_develop_2016]
GO

/****** Object:  Table [dbo].[InstitutionDocuments]    Script Date: 16.02.2016 16:07:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InstitutionDocuments](
	[InstitutionId] [int] NOT NULL,
	[AttachmentId] [int] NOT NULL,
 CONSTRAINT [PK_InstitutionDocuments] PRIMARY KEY CLUSTERED 
(
	[InstitutionId] ASC,
	[AttachmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[InstitutionDocuments]  WITH CHECK ADD  CONSTRAINT [FK_InstitutionDocuments_Attachment] FOREIGN KEY([AttachmentId])
REFERENCES [dbo].[Attachment] ([AttachmentID])
GO

ALTER TABLE [dbo].[InstitutionDocuments] CHECK CONSTRAINT [FK_InstitutionDocuments_Attachment]
GO

ALTER TABLE [dbo].[InstitutionDocuments]  WITH CHECK ADD  CONSTRAINT [FK_InstitutionDocuments_Institution] FOREIGN KEY([InstitutionId])
REFERENCES [dbo].[Institution] ([InstitutionID])
GO

ALTER TABLE [dbo].[InstitutionDocuments] CHECK CONSTRAINT [FK_InstitutionDocuments_Institution]
GO


