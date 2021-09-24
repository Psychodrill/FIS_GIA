-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (39, '039_2012_06_14_PK_ConnectionStatus')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionStatus]') AND name = N'PK_ConnectionStatus')
ALTER TABLE [dbo].[ConnectionStatus] DROP CONSTRAINT [PK_ConnectionStatus]
GO

ALTER TABLE [dbo].[ConnectionStatus] ADD  CONSTRAINT [PK_ConnectionStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


