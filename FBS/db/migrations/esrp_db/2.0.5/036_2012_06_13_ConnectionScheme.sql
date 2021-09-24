-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (36, '036_2012_06_13_ConnectionScheme')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionScheme]') AND type in (N'U'))
DROP TABLE [dbo].[ConnectionScheme]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ConnectionScheme](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](510) NOT NULL,
 CONSTRAINT [PK_ConnectionScheme] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[ConnectionScheme]
           ([Name])
     VALUES
           ('Не выбрана')
GO

INSERT INTO [dbo].[ConnectionScheme]
           ([Name])
     VALUES
           ('Схема № 1')
GO

INSERT INTO [dbo].[ConnectionScheme]
           ([Name])
     VALUES
           ('Схема № 2')
GO

INSERT INTO [dbo].[ConnectionScheme]
           ([Name])
     VALUES
           ('Схема № 3')
GO


