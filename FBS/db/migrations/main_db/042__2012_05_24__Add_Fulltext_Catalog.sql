-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (42, '042__2012_05_24__Add_Fulltext_Catalog.txt')
-- =========================================================================


GO
PRINT N'Creating [Organization2010]...';


GO
CREATE FULLTEXT CATALOG [Organization2010]
    WITH ACCENT_SENSITIVITY = ON
    AUTHORIZATION [dbo];


GO
PRINT N'Creating Full-text Index...';


GO
CREATE FULLTEXT INDEX ON [dbo].[Organization2010]
    ([FullName] LANGUAGE 1033, [ShortName] LANGUAGE 1033, [OwnerDepartment] LANGUAGE 1033)
    KEY INDEX [PK__Organization2010__24F84F52]
    ON [Organization2010];

