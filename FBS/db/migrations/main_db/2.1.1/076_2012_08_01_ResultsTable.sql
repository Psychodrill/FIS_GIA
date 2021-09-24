-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (76, '076_2012_08_01_ResultsTable.sql')
-- =========================================================================
GO
/****** Object:  Table [dbo].[CommonNationalExamCertificateSumCheck]    Script Date: 08/02/2012 17:21:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CommonNationalExamCertificateSumCheck](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[BatchId] [bigint] NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[Sum] [numeric](10, 2) NULL,
	[Status] [int] NOT NULL,
	[NameSake] [bit] NOT NULL,
 CONSTRAINT [PK_CommonNationalExamCertificateSumCheck] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

