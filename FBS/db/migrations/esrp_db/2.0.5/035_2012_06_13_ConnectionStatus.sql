-- =========================================================================
-- ������ ���������� � ������� �������� � ���
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
           ('�� ���������')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('��������� ���������')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('������������� ����� � ����������� ����')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('������� �����')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('�������� ����� � ����������� ����')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('��������� ����� � ����������� ����')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('�������� ����� �� �������� ��������')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('��������� ����� �� �������� ��������')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('����� �������� ��������')
GO

INSERT INTO [dbo].[ConnectionStatus]
           ([Name])
     VALUES
           ('�������� �������� ���������')
GO



