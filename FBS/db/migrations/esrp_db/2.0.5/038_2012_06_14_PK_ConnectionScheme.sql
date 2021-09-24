-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (38, '038_2012_06_14_PK_ConnectionScheme')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionScheme]') AND name = N'PK_ConnectionScheme')
ALTER TABLE [dbo].[ConnectionScheme] DROP CONSTRAINT [PK_ConnectionScheme]
GO

ALTER TABLE [dbo].[ConnectionScheme] ADD  CONSTRAINT [PK_ConnectionScheme] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


