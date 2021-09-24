/****** Object:  Index [UK_ParentDirection_Code]    Script Date: 02/20/2014 13:05:34 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ParentDirection]') AND name = N'UK_ParentDirection_Code')
ALTER TABLE [dbo].[ParentDirection] DROP CONSTRAINT [UK_ParentDirection_Code]
GO

ALTER TABLE ParentDirection
ALTER COLUMN Code char(8)

/****** Object:  Index [UK_ParentDirection_Code]    Script Date: 02/20/2014 13:05:34 ******/
ALTER TABLE [dbo].[ParentDirection] ADD  CONSTRAINT [UK_ParentDirection_Code] UNIQUE CLUSTERED 
(
 [Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
GO


ALTER TABLE Direction
ALTER COLUMN UGSCODE CHAR(8)