-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (40, '040_2012_06_14_Organization2010')
-- =========================================================================

PRINT N'Altering [dbo].[Organization2010]...';


GO
ALTER TABLE [dbo].[Organization2010]
    ADD [LetterToReschedule]            IMAGE          NULL,
        [LetterToRescheduleName]        NVARCHAR (255) NULL,
        [LetterToRescheduleContentType] NVARCHAR (255) NULL,
        [TimeConnectionToSecureNetwork] DATETIME       NULL,
        [TimeEnterInformationInFIS]     DATETIME       NULL,
        [ConnectionSchemeId]            INT            DEFAULT ((1)) NOT NULL,
        [ConnectionStatusId]            INT            DEFAULT ((1)) NOT NULL;


GO

