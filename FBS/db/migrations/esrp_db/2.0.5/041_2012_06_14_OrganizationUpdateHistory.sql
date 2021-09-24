-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (41, '041_2012_06_14_OrganizationUpdateHistory')
-- =========================================================================

PRINT N'Altering [dbo].[OrganizationUpdateHistory]...';


GO
ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD [LetterToReschedule]            IMAGE          NULL,
        [LetterToRescheduleName]        NVARCHAR (255) NULL,
        [LetterToRescheduleContentType] NVARCHAR (255) NULL,
        [TimeConnectionToSecureNetwork] DATETIME       NULL,
        [TimeEnterInformationInFIS]     DATETIME       NULL,
        [ConnectionSchemeId]            INT            DEFAULT ((1)) NOT NULL,
        [ConnectionStatusId]            INT            DEFAULT ((1)) NOT NULL;


GO