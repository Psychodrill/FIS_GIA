-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (26, '026_2012_05_26_AlterAccount.sql')
-- =========================================================================

GO
PRINT N'Dropping [dbo].[Account].[accOrgIndex]...';


GO
DROP INDEX [accOrgIndex]
    ON [dbo].[Account];


GO
PRINT N'Dropping [dbo].[Account].[accStIdIndex]...';


GO
DROP INDEX [accStIdIndex]
    ON [dbo].[Account];


GO
PRINT N'Dropping [dbo].[Account].[accStatusIndex1]...';


GO
DROP INDEX [accStatusIndex1]
    ON [dbo].[Account];


GO
PRINT N'Dropping [dbo].[Account].[accStatusIndex2]...';


GO
DROP INDEX [accStatusIndex2]
    ON [dbo].[Account];


GO
PRINT N'Dropping [dbo].[UpdateUserAccount]...';


GO
PRINT N'Altering [dbo].[Account]...';


GO
ALTER TABLE [dbo].[Account] ALTER COLUMN [Status] NVARCHAR (255) NOT NULL;


GO
PRINT N'Creating [dbo].[Account].[accOrgIndex]...';


GO
CREATE NONCLUSTERED INDEX [accOrgIndex]
    ON [dbo].[Account]([OrganizationId] ASC)
    INCLUDE([Login], [LastName], [FirstName], [PatronymicName], [Email], [Status]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[Account].[accStIdIndex]...';


GO
CREATE NONCLUSTERED INDEX [accStIdIndex]
    ON [dbo].[Account]([Status] ASC, [Id] ASC)
    INCLUDE([Login], [LastName], [FirstName]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[Account].[accStatusIndex1]...';


GO
CREATE NONCLUSTERED INDEX [accStatusIndex1]
    ON [dbo].[Account]([Status] ASC)
    INCLUDE([Id], [CreateDate], [Login], [LastName], [FirstName], [PatronymicName], [OrganizationId], [Email]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[Account].[accStatusIndex2]...';


GO
CREATE NONCLUSTERED INDEX [accStatusIndex2]
    ON [dbo].[Account]([Status] ASC)
    INCLUDE([Id], [OrganizationId]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];




