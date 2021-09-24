insert into Migrations(MigrationVersion, MigrationName) values (3, '003_2012_0723_CheckByMarkSumLog.sql')
GO
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

CREATE PROCEDURE [dbo].[AddCheckByMarkSumEvent]
	 @LastName nvarchar(1000), @FirstName nvarchar(1000), @PatronymicName nvarchar(1000)=null,@ListSubject nvarchar(1000), @SumMark int,@OrgId int
as
BEGIN
	INSERT INTO CheckByMarkSumLog (LastName,FirstName,GivenName,OrgId,MarkSum,Subjects) values(@LastName,@FirstName,@PatronymicName,@OrgId,@SumMark,@ListSubject)
	select 	@@IDENTITY
END

GO