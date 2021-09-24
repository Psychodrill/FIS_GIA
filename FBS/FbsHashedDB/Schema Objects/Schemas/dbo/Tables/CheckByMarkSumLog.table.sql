CREATE TABLE [dbo].[CheckByMarkSumLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](1000) NULL,
	[LastName] [nvarchar](1000) NULL,
	[GivenName] [nvarchar](1000) NULL,
	[OrgId] [int] NULL,
	[MarkSum] [int] NULL,
	[Result] [int] NULL,
	[Subjects] [nvarchar](1000) NULL,
 CONSTRAINT [PK_CheckByMarkSumLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
