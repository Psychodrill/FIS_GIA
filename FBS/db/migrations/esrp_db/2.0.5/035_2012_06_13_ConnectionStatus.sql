-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (35, '035_2012_06_13_ConnectionStatus')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionStatus]') AND type in (N'U'))
DROP TABLE [dbo].[ConnectionStatus]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ConnectionStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](510) NOT NULL,
 CONSTRAINT [PK_ConnectionStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Не обращался')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Первичное обращение')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Предоставлена схема в электронном виде')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Готовит пакет')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Направил пакет в электронном виде')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Направлен ответ в электронном виде')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Направил пакет на бумажном носителе')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Направлен ответ на бумажном носителе')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Выдан ключевой материал')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('Направил протокол испытаний')
GO



