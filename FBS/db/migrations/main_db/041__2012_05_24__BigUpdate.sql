-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (41, '041__2012_05_24__BigUpdate')
-- =========================================================================
GO

GO
PRINT N'Dropping [dbo].[Migrations_tri]...';


GO
DROP TRIGGER [dbo].[Migrations_tri];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificate].[IdxCertificateOwner]...';


GO
DROP INDEX [IdxCertificateOwner]
    ON [dbo].[CommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[Account].[accOrgIndex]...';


GO
DROP INDEX [accOrgIndex]
    ON [dbo].[Account];


GO
PRINT N'Dropping [dbo].[Account].[accStatusIndex2]...';


GO
DROP INDEX [accStatusIndex2]
    ON [dbo].[Account];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateCheck].[CNECC_BatchId_CNEId]...';


GO
DROP INDEX [CNECC_BatchId_CNEId]
    ON [dbo].[CommonNationalExamCertificateCheck];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateCheck].[cneccIndex3]...';


GO
DROP INDEX [cneccIndex3]
    ON [dbo].[CommonNationalExamCertificateCheck];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateDeny].[cnecdIdNewIndex]...';


GO
DROP INDEX [cnecdIdNewIndex]
    ON [dbo].[CommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificateSubject].[cnecsIndex]...';


GO
DROP INDEX [cnecsIndex]
    ON [dbo].[ImportingCommonNationalExamCertificateSubject];


GO
PRINT N'Dropping [dbo].[AccountKeyLog]...';


GO
DROP TABLE [dbo].[AccountKeyLog];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateDenyActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateDenyActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateFormActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateFormActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubjectActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubjectActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubjectFormActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubjectFormActivePartition];


GO
PRINT N'Dropping [dbo].[DeprecatedCommonNationalExamCertificateDeny]...';


GO
DROP TABLE [dbo].[DeprecatedCommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[fbs_ip]...';


GO
DROP TABLE [dbo].[fbs_ip];


GO
PRINT N'Dropping [dbo].[fbs_users]...';


GO
DROP TABLE [dbo].[fbs_users];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificateDeny]...';


GO
DROP TABLE [dbo].[ImportingCommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[old_CommonNationalExamCertificateForm]...';


GO
DROP TABLE [dbo].[old_CommonNationalExamCertificateForm];


GO
PRINT N'Dropping [dbo].[old_CommonNationalExamCertificateFormNumberRange]...';


GO
DROP TABLE [dbo].[old_CommonNationalExamCertificateFormNumberRange];


GO
PRINT N'Dropping [dbo].[old_CommonNationalExamCertificateSubjectForm]...';


GO
DROP TABLE [dbo].[old_CommonNationalExamCertificateSubjectForm];


GO
PRINT N'Dropping [dbo].[OldCommonNationalExamCertificateForm35]...';


GO
DROP TABLE [dbo].[OldCommonNationalExamCertificateForm35];


GO
PRINT N'Dropping [dbo].[OldCommonNationalExamCertificateSubjectForm35]...';


GO
DROP TABLE [dbo].[OldCommonNationalExamCertificateSubjectForm35];


GO
PRINT N'Dropping [dbo].[OrganizationStatus]...';


GO
DROP TABLE [dbo].[OrganizationStatus];


GO
PRINT N'Dropping [dbo].[RegionConvert]...';


GO
DROP TABLE [dbo].[RegionConvert];


GO
PRINT N'Dropping [dbo].[temp]...';


GO
DROP TABLE [dbo].[temp];


GO
PRINT N'Dropping [dbo].[TempCnecChecks]...';


GO
DROP TABLE [dbo].[TempCnecChecks];


GO
PRINT N'Dropping [dbo].[UpdatingCommonNationalExamCertificateForm35]...';


GO
DROP TABLE [dbo].[UpdatingCommonNationalExamCertificateForm35];


GO
PRINT N'Dropping [dbo].[UpdatingCommonNationalExamCertificateSubjectForm35]...';


GO
DROP TABLE [dbo].[UpdatingCommonNationalExamCertificateSubjectForm35];


GO
PRINT N'Dropping [dbo].[UserAccountActivityCommonReport]...';


GO
DROP TABLE [dbo].[UserAccountActivityCommonReport];


GO
PRINT N'Dropping [dbo].[DateOnly]...';


GO
DROP FUNCTION [dbo].[DateOnly];


GO
PRINT N'Dropping [dbo].[fn_SubstringCount]...';


GO
DROP FUNCTION [dbo].[fn_SubstringCount];


GO
PRINT N'Dropping [dbo].[GetCommonNationalExamCertificateFormPartition]...';


GO
DROP FUNCTION [dbo].[GetCommonNationalExamCertificateFormPartition];


GO
PRINT N'Dropping [dbo].[GetCorrectPassportSeria]...';


GO
DROP FUNCTION [dbo].[GetCorrectPassportSeria];


GO
PRINT N'Dropping [dbo].[GetDataDbName]...';


GO
DROP FUNCTION [dbo].[GetDataDbName];


GO
PRINT N'Dropping [dbo].[GetFirstNonRusPosition]...';


GO
DROP FUNCTION [dbo].[GetFirstNonRusPosition];


GO
PRINT N'Dropping [dbo].[GetFirstNonRusPosition2]...';


GO
DROP FUNCTION [dbo].[GetFirstNonRusPosition2];


GO
PRINT N'Dropping [dbo].[GetHash_SHA1]...';


GO
DROP FUNCTION [dbo].[GetHash_SHA1];


GO
PRINT N'Dropping [dbo].[GetIp]...';


GO
DROP FUNCTION [dbo].[GetIp];


GO
PRINT N'Dropping [dbo].[GetOrganizationEducationInstitutionTypeIdByName]...';


GO
DROP FUNCTION [dbo].[GetOrganizationEducationInstitutionTypeIdByName];


GO
PRINT N'Dropping [dbo].[GetDelimitedValues2]...';


GO
DROP FUNCTION [dbo].[GetDelimitedValues2];


GO
PRINT N'Dropping [dbo].[ReportOrganizationCheckStatisticsTVF]...';


GO
DROP FUNCTION [dbo].[ReportOrganizationCheckStatisticsTVF];


GO
PRINT N'Dropping [dbo].[Отчет о проверках свидетельств организациями за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о проверках свидетельств организациями за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о проверках свидетельств организациями за неделю]...';


GO
DROP VIEW [dbo].[Отчет о проверках свидетельств организациями за неделю];


GO
PRINT N'Dropping [dbo].[Отчет о проверках свидетельств организациями за год]...';


GO
DROP VIEW [dbo].[Отчет о проверках свидетельств организациями за год];


GO
PRINT N'Dropping [dbo].[Отчет о загрузках свидетельств за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о загрузках свидетельств за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о загрузках свидетельств за год]...';


GO
DROP VIEW [dbo].[Отчет о загрузках свидетельств за год];


GO
PRINT N'Dropping [dbo].[Отчет о загрузках свидетельств за неделю]...';


GO
DROP VIEW [dbo].[Отчет о загрузках свидетельств за неделю];


GO
PRINT N'Dropping [dbo].[Отчет о наиболее активных организациях за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о наиболее активных организациях за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о наиболее активных организациях за год]...';


GO
DROP VIEW [dbo].[Отчет о наиболее активных организациях за год];


GO
PRINT N'Dropping [dbo].[Отчет о наиболее активных организациях за неделю]...';


GO
DROP VIEW [dbo].[Отчет о наиболее активных организациях за неделю];


GO
PRINT N'Dropping [dbo].[Отчет о пользователях, превысивших лимит неправильных проверок за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о пользователях, превысивших лимит неправильных проверок за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о пользователях, превысивших лимит неправильных проверок за год]...';


GO
DROP VIEW [dbo].[Отчет о пользователях, превысивших лимит неправильных проверок за год];


GO
PRINT N'Dropping [dbo].[Отчет о пользователях, превысивших лимит неправильных проверок за неделю]...';


GO
DROP VIEW [dbo].[Отчет о пользователях, превысивших лимит неправильных проверок за неделю];


GO
PRINT N'Dropping [dbo].[Отчет о регистрации пользователей за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о регистрации пользователей за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о регистрации пользователей за год]...';


GO
DROP VIEW [dbo].[Отчет о регистрации пользователей за год];


GO
PRINT N'Dropping [dbo].[Отчет о регистрации пользователей за неделю]...';


GO
DROP VIEW [dbo].[Отчет о регистрации пользователей за неделю];


GO
PRINT N'Dropping [dbo].[Отчет об уникальных проверках свидетельств за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет об уникальных проверках свидетельств за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет об уникальных проверках свидетельств за год]...';


GO
DROP VIEW [dbo].[Отчет об уникальных проверках свидетельств за год];


GO
PRINT N'Dropping [dbo].[Отчет об уникальных проверках свидетельств за неделю]...';


GO
DROP VIEW [dbo].[Отчет об уникальных проверках свидетельств за неделю];


GO
PRINT N'Dropping [dbo].[Сводный отчет по типам запросов и уникальным проверкам за 24 часа]...';


GO
DROP VIEW [dbo].[Сводный отчет по типам запросов и уникальным проверкам за 24 часа];


GO
PRINT N'Dropping [dbo].[Сводный отчет по типам запросов и уникальным проверкам за неделю]...';


GO
DROP VIEW [dbo].[Сводный отчет по типам запросов и уникальным проверкам за неделю];


GO
PRINT N'Dropping [dbo].[CheckCommonNationalExamCertificateByNumber2]...';


GO
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByNumber2];


GO
PRINT N'Dropping [dbo].[DeleteExpiredCommonNationalExamCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[DeleteExpiredCommonNationalExamCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[DeleteExpiredCommonNationalExamCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[DeleteExpiredCommonNationalExamCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[EmailQuery]...';


GO
DROP PROCEDURE [dbo].[EmailQuery];


GO
PRINT N'Dropping [dbo].[ExecuteCommonNationalExamCertificateCheck]...';


GO
DROP PROCEDURE [dbo].[ExecuteCommonNationalExamCertificateCheck];


GO
PRINT N'Dropping [dbo].[ExecuteCommonNationalExamCertificateRequest]...';


GO
DROP PROCEDURE [dbo].[ExecuteCommonNationalExamCertificateRequest];


GO
PRINT N'Dropping [dbo].[ExecuteCompetitionCertificateRequest]...';


GO
DROP PROCEDURE [dbo].[ExecuteCompetitionCertificateRequest];


GO
PRINT N'Dropping [dbo].[ExecuteEntrantCheck]...';


GO
DROP PROCEDURE [dbo].[ExecuteEntrantCheck];


GO
PRINT N'Dropping [dbo].[ExecuteSchoolLeavingCertificateCheck]...';


GO
DROP PROCEDURE [dbo].[ExecuteSchoolLeavingCertificateCheck];


GO
PRINT N'Dropping [dbo].[GetCommonNationalExamCertificateByRegionReport]...';


GO
DROP PROCEDURE [dbo].[GetCommonNationalExamCertificateByRegionReport];


GO
PRINT N'Dropping [dbo].[GetCommonNationalExamCertificateCheckReport]...';


GO
DROP PROCEDURE [dbo].[GetCommonNationalExamCertificateCheckReport];


GO
PRINT N'Dropping [dbo].[GetCompetitionCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[GetCompetitionCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[GetEducationalOrganizationReport]...';


GO
DROP PROCEDURE [dbo].[GetEducationalOrganizationReport];


GO
PRINT N'Dropping [dbo].[GetEntrantCheckBatch]...';


GO
DROP PROCEDURE [dbo].[GetEntrantCheckBatch];


GO
PRINT N'Dropping [dbo].[GetSchoolLeavingCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[GetSchoolLeavingCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[GetUserAccountActivationReport]...';


GO
DROP PROCEDURE [dbo].[GetUserAccountActivationReport];


GO
PRINT N'Dropping [dbo].[GetUserAccountActivityCommonReport]...';


GO
DROP PROCEDURE [dbo].[GetUserAccountActivityCommonReport];


GO
PRINT N'Dropping [dbo].[ImportCommonNationalExamCertificate]...';


GO
DROP PROCEDURE [dbo].[ImportCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[ImportCommonNationalExamCertificateDeny]...';


GO
DROP PROCEDURE [dbo].[ImportCommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[ImportCompetitionCertificate]...';


GO
DROP PROCEDURE [dbo].[ImportCompetitionCertificate];


GO
PRINT N'Dropping [dbo].[ImportSchoolLeavingCertificateDeny]...';


GO
DROP PROCEDURE [dbo].[ImportSchoolLeavingCertificateDeny];


GO
PRINT N'Dropping [dbo].[PrepareCommonNationalExamCertificateCheck]...';


GO
DROP PROCEDURE [dbo].[PrepareCommonNationalExamCertificateCheck];


GO
PRINT N'Dropping [dbo].[PrepareCommonNationalExamCertificateRequest]...';


GO
DROP PROCEDURE [dbo].[PrepareCommonNationalExamCertificateRequest];


GO
PRINT N'Dropping [dbo].[PrepareCompetitionCertificateRequest]...';


GO
DROP PROCEDURE [dbo].[PrepareCompetitionCertificateRequest];


GO
PRINT N'Dropping [dbo].[PrepareEntrantCheck]...';


GO
DROP PROCEDURE [dbo].[PrepareEntrantCheck];


GO
PRINT N'Dropping [dbo].[PrepareSchoolLeavingCertificateCheck]...';


GO
DROP PROCEDURE [dbo].[PrepareSchoolLeavingCertificateCheck];


GO
PRINT N'Dropping [dbo].[RecreateEmptyPasswordHash]...';


GO
DROP PROCEDURE [dbo].[RecreateEmptyPasswordHash];


GO
PRINT N'Dropping [dbo].[RefreshCommonNationalExamCertificateFormPartition]...';


GO
DROP PROCEDURE [dbo].[RefreshCommonNationalExamCertificateFormPartition];


GO
PRINT N'Dropping [dbo].[RefreshUserAccountActivityCommonReport]...';


GO
DROP PROCEDURE [dbo].[RefreshUserAccountActivityCommonReport];


GO
PRINT N'Dropping [dbo].[Report_GetUsersByStatus]...';


GO
DROP PROCEDURE [dbo].[Report_GetUsersByStatus];


GO
PRINT N'Dropping [dbo].[Report_GetUsersByStatus_currentDay]...';


GO
DROP PROCEDURE [dbo].[Report_GetUsersByStatus_currentDay];


GO
PRINT N'Dropping [dbo].[ReportCertificateDenyLoad]...';


GO
DROP PROCEDURE [dbo].[ReportCertificateDenyLoad];


GO
PRINT N'Dropping [dbo].[ReportCertificateLoad]...';


GO
DROP PROCEDURE [dbo].[ReportCertificateLoad];


GO
PRINT N'Dropping [dbo].[ReportCertificateLoadDaily]...';


GO
DROP PROCEDURE [dbo].[ReportCertificateLoadDaily];


GO
PRINT N'Dropping [dbo].[ReportCheckStatistics]...';


GO
DROP PROCEDURE [dbo].[ReportCheckStatistics];


GO
PRINT N'Dropping [dbo].[ReportTopCheckingOrganizations]...';


GO
DROP PROCEDURE [dbo].[ReportTopCheckingOrganizations];


GO
PRINT N'Dropping [dbo].[ReportTopCheckingUsers]...';


GO
DROP PROCEDURE [dbo].[ReportTopCheckingUsers];


GO
PRINT N'Dropping [dbo].[ScheduleCreateReportCertificatesByRegion]...';


GO
DROP PROCEDURE [dbo].[ScheduleCreateReportCertificatesByRegion];


GO
PRINT N'Dropping [dbo].[ScheduleCreateReportCnecLoading]...';


GO
DROP PROCEDURE [dbo].[ScheduleCreateReportCnecLoading];


GO
PRINT N'Dropping [dbo].[ScheduleCreateReportUserRegistration]...';


GO
DROP PROCEDURE [dbo].[ScheduleCreateReportUserRegistration];


GO
PRINT N'Dropping [dbo].[SearchAccountLog]...';


GO
DROP PROCEDURE [dbo].[SearchAccountLog];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificate2]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificate2];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateCheck2]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheck2];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateRequest2]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequest2];


GO
PRINT N'Dropping [dbo].[SearchDynamicIpUserAccount]...';


GO
DROP PROCEDURE [dbo].[SearchDynamicIpUserAccount];


GO
PRINT N'Dropping [dbo].[SearchEducationInstitutionType]...';


GO
DROP PROCEDURE [dbo].[SearchEducationInstitutionType];


GO
PRINT N'Dropping [dbo].[SearchProcessingCompetitionCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[SearchProcessingCompetitionCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[SearchProcessingEntrantCheckBatch]...';


GO
DROP PROCEDURE [dbo].[SearchProcessingEntrantCheckBatch];


GO
PRINT N'Dropping [dbo].[SearchProcessingSchoolLeavingCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[SearchProcessingSchoolLeavingCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[SearchUserAccount]...';


GO
DROP PROCEDURE [dbo].[SearchUserAccount];


GO
PRINT N'Dropping [dbo].[SearchUserAccountEmail]...';


GO
DROP PROCEDURE [dbo].[SearchUserAccountEmail];


GO
PRINT N'Dropping [dbo].[SearchVUZOrgs]...';


GO
DROP PROCEDURE [dbo].[SearchVUZOrgs];


GO
PRINT N'Dropping [dbo].[UpdateCompetitionCertificateRequestBatchExecuting]...';


GO
DROP PROCEDURE [dbo].[UpdateCompetitionCertificateRequestBatchExecuting];


GO
PRINT N'Dropping [dbo].[UpdateEntrantCheckBatchExecuting]...';


GO
DROP PROCEDURE [dbo].[UpdateEntrantCheckBatchExecuting];


GO
PRINT N'Dropping [dbo].[UpdateSchoolLeavingCertificateCheckBatchExecuting]...';


GO
DROP PROCEDURE [dbo].[UpdateSchoolLeavingCertificateCheckBatchExecuting];


GO
PRINT N'Creating [Organization2010]...';


GO
CREATE FULLTEXT CATALOG [Organization2010]
    WITH ACCENT_SENSITIVITY = ON
    AUTHORIZATION [dbo];


GO
PRINT N'Altering [dbo].[Organization2010]...';


GO
ALTER TABLE [dbo].[Organization2010]
    ADD [DisableLog] BIT DEFAULT ((0)) NOT NULL;


GO
PRINT N'Altering [dbo].[OrganizationUpdateHistory]...';


GO
ALTER TABLE [dbo].[OrganizationUpdateHistory]
    ADD [EditorUserName] NVARCHAR (50) NULL,
        [DisableLog]     BIT           DEFAULT ((0)) NOT NULL;


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificate].[IdxCertificateOwner]...';


GO
CREATE NONCLUSTERED INDEX [IdxCertificateOwner]
    ON [dbo].[CommonNationalExamCertificate]([Year] ASC)
    INCLUDE([LastName], [FirstName], [PatronymicName]) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificate].[CNE_FirstName]...';


GO
CREATE NONCLUSTERED INDEX [CNE_FirstName]
    ON [dbo].[CommonNationalExamCertificate]([FirstName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificate].[CNE_LastName]...';


GO
CREATE NONCLUSTERED INDEX [CNE_LastName]
    ON [dbo].[CommonNationalExamCertificate]([LastName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificate].[CNE_PatronymicName]...';


GO
CREATE NONCLUSTERED INDEX [CNE_PatronymicName]
    ON [dbo].[CommonNationalExamCertificate]([PatronymicName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificate].[IX_CommonNationalExamCertificate_PassportNumber]...';


GO
CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificate_PassportNumber]
    ON [dbo].[CommonNationalExamCertificate]([PassportNumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CNEWebUICheckLog].[IX_CNEWebUICheckLog_EventDate]...';


GO
CREATE NONCLUSTERED INDEX [IX_CNEWebUICheckLog_EventDate]
    ON [dbo].[CNEWebUICheckLog]([EventDate] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificateCheckBatch].[IX_CNECCB_IsProcess]...';


GO
CREATE NONCLUSTERED INDEX [IX_CNECCB_IsProcess]
    ON [dbo].[CommonNationalExamCertificateCheckBatch]([IsProcess] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificateDeny].[CommonNationalExamCertificateDeny_CertificateNumber]...';


GO
CREATE NONCLUSTERED INDEX [CommonNationalExamCertificateDeny_CertificateNumber]
    ON [dbo].[CommonNationalExamCertificateDeny]([CertificateNumber] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating [dbo].[CommonNationalExamCertificateDeny].[CommonNationalExamCertificateDeny_Year]...';


GO
CREATE NONCLUSTERED INDEX [CommonNationalExamCertificateDeny_Year]
    ON [dbo].[CommonNationalExamCertificateDeny]([Year] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, ONLINE = OFF, MAXDOP = 0)
    ON [PRIMARY];


GO
PRINT N'Creating Full-text Index...';


GO
CREATE FULLTEXT INDEX ON [dbo].[Organization2010]
    ([FullName] LANGUAGE 1033, [ShortName] LANGUAGE 1033, [OwnerDepartment] LANGUAGE 1033)
    KEY INDEX [PK__Organization2010__24F84F52]
    ON [Organization2010];


GO
PRINT N'Altering [dbo].[AddCNEWebUICheckEvent]...';


GO
-- =============================================
-- Author:		Yusupov K.I.
-- Create date: 04-06-2010
-- Description:	Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
ALTER PROCEDURE [dbo].[AddCNEWebUICheckEvent]
	@AccountLogin NVARCHAR(255),			-- логин проверяющего
	@LastName NVARCHAR(255),				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255),				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255),			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20)=NULL,		-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20)=NULL,		-- номер документа сертифицируемого (паспорта)
	@CNENumber NVARCHAR(20)=NULL,			-- номер сертификата
	@TypographicNumber NVARCHAR(20)=NULL,   -- типографический номер сертификата 
	@RawMarks NVARCHAR(500)=null			-- средние оценки по предметам (через запятую, в определенном порядке)
AS
BEGIN
	IF 
	(
		@TypographicNumber IS NULL AND
		@CNENumber IS NULL AND
		(@PassportSeria IS NULL OR @PassportNumber IS NULL) AND
		@RawMarks IS NULL
	)
	BEGIN
		RAISERROR (N'Не указаны паспортные данные, типографский номер, номер свидетельства и баллы по предметам одновременно',10,1)
		RETURN
	END

	DECLARE @AccountId BIGINT
	SELECT
		@AccountId = Acc.[Id]
	FROM
		dbo.Account Acc WITH (nolock, fastfirstrow)
	WHERE
		Acc.[Login] = @AccountLogin	

	IF (@TypographicNumber IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,TypographicNumber) 
        VALUES 
			(@AccountId,'Typographic',@FirstName,@LastName,@PatronymicName,@TypographicNumber)
	END
	ELSE IF (@CNENumber IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,CNENumber) 
        VALUES 
			(@AccountId,'CNENumber',@FirstName,@LastName,@PatronymicName,@CNENumber)
	END
	ELSE IF (@PassportNumber IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,PassportSeria,PassportNumber) 
        VALUES 
			(@AccountId,'Passport',@FirstName,@LastName,@PatronymicName,@PassportSeria,@PassportNumber)
	END
	ELSE IF (@RawMarks IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,Marks) 
        VALUES 
			(@AccountId,'Marks',@FirstName,@LastName,@PatronymicName,@RawMarks)
	END
	  SELECT @@Identity
END
GO
PRINT N'Altering [dbo].[CheckCommonNationalExamCertificateByNumber]...';


GO
-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
ALTER proc [dbo].[CheckCommonNationalExamCertificateByNumber]
	 @number nvarchar(255) = null				-- номер сертификата
	, @checkLastName nvarchar(255) = null		-- фамилия сертифицируемого
	, @checkFirstName nvarchar(255) = null		-- имя сертифицируемого
	, @checkPatronymicName nvarchar(255) = null -- отчетсво сертифицируемого
	, @checkSubjectMarks nvarchar(max) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @checkTypographicNumber nvarchar(20) = null -- типографический номер сертификата
as
begin 

	if @checkTypographicNumber is null and @number is null
	begin
		RAISERROR (N'Не могут быть одновременно неуказанными и номер свидетельства и типографский номер',10,1);
		return
	end
    
	declare 
		@commandText nvarchar(max)
		, @declareCommandText nvarchar(max)
		, @selectCommandText nvarchar(max)
		, @baseName nvarchar(255)
		, @yearFrom int
		, @yearTo int
		, @accountId bigint
        , @organizationId bigint
    	, @CId bigint
		, @eventCode nvarchar(255)
		, @eventParams nvarchar(4000)
		, @sourceEntityIds nvarchar(4000) 
	
	declare @check_subject table
	(
	SubjectId int
	, Mark nvarchar(10)
	)
	
	declare @certificate_check table
	(
	Number nvarchar(255)
	, CheckLastName nvarchar(255)
	, LastName nvarchar(255)
	, LastNameIsCorrect bit
	, CheckFirstName nvarchar(255)
	, FirstName nvarchar(255)
	, FirstNameIsCorrect bit
	, CheckPatronymicName nvarchar(255)
	, PatronymicName nvarchar(255)
	, PatronymicNameIsCorrect bit
	, IsExist bit
	, certificateId bigint
	, IsDeny bit
	, DenyComment ntext
	, DenyNewcertificateNumber nvarchar(255)
	, [Year] int
	, PassportSeria nvarchar(255)
	, PassportNumber nvarchar(255)
	, RegionId int
	, RegionName nvarchar(255)
	, TypographicNumber nvarchar(255)
	)

	-- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

	if isnull(@checkTypographicNumber,'') <> ''
		select @yearFrom = 2009, @yearTo = Year(GetDate()) --2009-год появления типографского номера в БД
	else
		select @yearFrom = 2008, @yearTo = Year(GetDate())

	select
		@accountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	declare @sql nvarchar(max)
	
	set @sql = '
	select
		[certificate].Number 
		, @CheckLastName CheckLastName
		, [certificate].LastName 
		, case 
			when @CheckLastName is null then 1 
			when [certificate].LastName collate cyrillic_general_ci_ai = @CheckLastName then 1
			else 0
		end LastNameIsCorrect
		, @CheckFirstName CheckFirstName
		, [certificate].FirstName 
		, case 
			when @CheckFirstName is null then 1 
			when [certificate].FirstName collate cyrillic_general_ci_ai = @CheckFirstName then 1
			else 0
		end FirstNameIsCorrect
		, @CheckPatronymicName CheckPatronymicName 
		, [certificate].PatronymicName 
		, case 
			when @CheckPatronymicName is null then 1 
			when [certificate].PatronymicName collate cyrillic_general_ci_ai = @CheckPatronymicName then 1
			else 0
		end PatronymicNameIsCorrect
		, case
			when [certificate].Id>0 then 1
			else 0
		end IsExist
		, [certificate].Id
		, case
			when certificate_deny.Id>0 then 1
			else 0
		end iscertificate_deny
		, certificate_deny.Comment
		, certificate_deny.NewcertificateNumber
		, [certificate].[Year]
		, [certificate].PassportSeria
		, [certificate].PassportNumber
		, [certificate].RegionId
		, region.Name
		, [certificate].TypographicNumber
	from 
		(select null ''empty'') t left join 
		dbo.CommonNationalExamcertificate [certificate] with (nolock, fastfirstrow) on 
				[certificate].[Year] between @yearFrom and @yearTo '
	
	if @number is not null 
		set @sql = @sql + '	and [certificate].Number = @number'
	if @CheckTypographicNumber is not null 
		set @sql = @sql + '	and [certificate].TypographicNumber=@CheckTypographicNumber'
	
	set @sql = @sql + '			
		left outer join dbo.Region region
			on region.Id = [certificate].RegionId
		left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
			on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.certificateNumber = [certificate].Number'
	
	insert into @certificate_check 		
	exec sp_executesql @sql,N'@checkLastName nvarchar(255),@number nvarchar(255),@checkTypographicNumber nvarchar(20),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int',@checkLastName=@checkLastName,@number = @number,@checkTypographicNumber=@checkTypographicNumber,@checkFirstName=@checkFirstName,@checkPatronymicName=@checkPatronymicName,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo
	
	set @eventParams = 
		isnull(@number, '') + '|' +
		isnull(@checkLastName, '') + '|' +
		isnull(@checkFirstName, '') + '|' +
		isnull(@checkPatronymicName, '') + '|' +
		isnull(@checkSubjectMarks, '') + '|' +
		isnull(@checkTypographicNumber, '')

	set @sourceEntityIds = '' 
	select 
		@sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(100), certificate_check.certificateId) 
	from 
		@certificate_check certificate_check 
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	if @sourceEntityIds = '' 
		set @sourceEntityIds = null 


	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        @certificate_check S
    where
    	S.certificateId is not null
		
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @certificateId = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
	-------------------------------------------------------------

	select
		certificate_check.certificateId
		,certificate_check.Number
		, certificate_check.CheckLastName
		, certificate_check.LastName
		, certificate_check.LastNameIsCorrect
		, certificate_check.CheckFirstName
		, certificate_check.FirstName
		, certificate_check.FirstNameIsCorrect
		, certificate_check.CheckPatronymicName
		, certificate_check.PatronymicName
		, certificate_check.PatronymicNameIsCorrect
		, certificate_check.IsExist
		, [subject].Id SubjectId
		, [subject].Name SubjectName
		, case when check_subject.CheckSubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),'.',',') CheckSubjectMark
		, case when check_subject.SubjectMark < mm.[MinimalMark] then '!' else '' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),'.',',') SubjectMark
		, isnull(check_subject.SubjectMarkIsCorrect, 0) SubjectMarkIsCorrect
		, check_subject.HasAppeal
		, certificate_check.IsDeny
		, certificate_check.DenyComment
		, certificate_check.DenyNewcertificateNumber
		, certificate_check.PassportSeria
		, certificate_check.PassportNumber
		, certificate_check.RegionId
		, certificate_check.RegionName
		, certificate_check.[Year]
		, certificate_check.TypographicNumber
		, case when ed.[ExpireDate] is null then 'Не найдено' else 
			case when isnull(certificate_check.isdeny,0) <> 0 then 'Аннулировано' else
			case when getdate() <= ed.[ExpireDate] then 'Действительно'
			else 'Истек срок' end end end as [Status],
        isnull(CC.UniqueChecks, 0) UniqueChecks,
        isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
        isnull(CC.UniqueIHECheck, 0) UniqueIHECheck,
        isnull(CC.UniqueIHEFCheck, 0) UniqueIHEFCheck,
        isnull(CC.UniqueTSSaFCheck, 0) UniqueTSSaFCheck,
        isnull(CC.UniqueTSSCheck, 0) UniqueTSSCheck,
        isnull(CC.UniqueTSSFCheck, 0) UniqueTSSFCheck,
        isnull(CC.UniqueRCOICheck, 0) UniqueRCOICheck,
        isnull(CC.UniqueOUOCheck, 0) UniqueOUOCheck,
        isnull(CC.UniqueFounderCheck, 0) UniqueFounderCheck,
        isnull(CC.UniqueOtherCheck, 0) UniqueOtherCheck
	from @certificate_check certificate_check
    	inner join CommonNationalExamcertificate C
        	on C.Id = certificate_check.certificateId
        left outer join ExamcertificateUniqueChecks CC
			on CC.Id = C.Id
		left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]					
		left outer join (
			select
				certificate_check.Number 
				, certificate_check.CheckLastName
				, certificate_check.LastName 
				, certificate_check.LastNameIsCorrect
				, certificate_check.CheckFirstName
				, certificate_check.FirstName 
				, certificate_check.FirstNameIsCorrect
				, certificate_check.CheckPatronymicName
				, certificate_check.PatronymicName 
				, certificate_check.PatronymicNameIsCorrect
				, certificate_check.IsExist
				, isnull(check_subject.SubjectId, certificate_subject.SubjectId) SubjectId
				, check_subject.[Mark] CheckSubjectMark
				, certificate_subject.[Mark] SubjectMark
				, case
					when check_subject.Mark = certificate_subject.Mark then 1
					else 0
				end SubjectMarkIsCorrect
				, certificate_subject.HasAppeal
			from CommonNationalExamcertificateSubject certificate_subject with (nolock)
				inner join @certificate_check certificate_check
					on certificate_check.[Year] = certificate_subject.[Year]
						and certificate_check.certificateId = certificate_subject.certificateId
				left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
					on check_subject.SubjectId = certificate_subject.SubjectId
						) check_subject
			left outer join dbo.[Subject] [subject]
				on [subject].Id = check_subject.SubjectId
			on 1 = 1
			left join [MinimalMark] as mm on check_subject.SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year] 
	
		if @checkTypographicNumber is not null
			set @eventCode = 'CNE_CHK_TN'
		else
			set @eventCode = 'CNE_CHK'
			
		exec dbo.RegisterEvent 
			@accountId
			, @ip
			, @eventCode
			, @sourceEntityIds
			, @eventParams
			, @updateId = null
	
	return 0
end
GO
PRINT N'Altering [dbo].[GetAccountRole]...';


GO
-- =============================================
-- Получить роли учетной записи.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение офомления.
-- =============================================
ALTER procedure dbo.GetAccountRole
	@login nvarchar(255) -- логин учетной записи
as
begin

	select
		@login [Login]
		, account_role.RoleCode RoleCode
	from
		dbo.AccountRole account_role with (nolock, fastfirstrow)
			inner join dbo.Account account with (nolock, fastfirstrow)
				on account_role.AccountId = account.[Id]
	where
		account.[Login] = @login
		and (account_role.IsActiveCondition is null
			or exists(select 1
				from dbo.AccountRoleActivity activity
				where activity.AccountId = account_role.AccountId
					and activity.RoleId = account_role.RoleId
					and activity.UpdateDate >= account.UpdateDate))

	return 0
end
GO
PRINT N'Altering [dbo].[SearchCommonNationalExamCertificate]...';


GO
-- =============================================
-- Получить сертификат ЕГЭ.
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificate]
	@lastName nvarchar(255) = null			  -- фамилия сертифицируемого
	, @firstName nvarchar(255) = null		  -- имя сертифицируемого
	, @patronymicName nvarchar(255) = null	  -- отчетсво сертифицируемого
	, @subjectMarks nvarchar(4000) = null	  -- средние оценки по предметам (через запятую, в определенном порядке)
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
	, @typographicNumber nvarchar(255) = null -- типографический номер сертификата 
	, @login nvarchar(255)					  -- логин проверяющего
	, @ip nvarchar(255)						  -- ip проверяющего
	, @year int = null					      -- год выдачи сертификата
as
begin
	declare 
		@eventCode nvarchar(255)
        , @organizationId bigint
		, @editorAccountId bigint
		, @eventParams nvarchar(4000)
		, @yearFrom int
		, @yearTo int
		, @commandText nvarchar(4000)
		, @internalPassportSeria nvarchar(255)

	-- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

	set @eventParams = 
		isnull(@lastName,'') + '|' 
		+ isnull(@firstName,'') + '|' 
		+ isnull(@patronymicName,'') + '|' 
		+ isnull(@subjectMarks, '') + '|' 
		+ isnull(@passportSeria, '') + '|' 
		+ isnull(@passportNumber, '') + '|' 
		+ isnull(@typographicNumber, '')

	select
		@editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	if @year is not null
		select @yearFrom = @year, @yearTo = @year
	else
		select @yearFrom = 2008
	    select @yearTo = Year(GetDate())

	if not @passportSeria is null
		set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

	set @commandText = ''
	if isnull(@typographicNumber, '') <> ''
		set @eventCode = N'CNE_FND_TN'
	else 
		set @eventCode = N'CNE_FND_P'

	declare @CId bigint,@sourceEntityIds nvarchar(4000) 
	declare @Search table 
	( 
		LastName nvarchar(255) 
		, FirstName nvarchar(255) 
		, PatronymicName nvarchar(255) 
		, CertificateId bigint 
		, CertificateNumber nvarchar(255) 
		, RegionId int 
		, PassportSeria nvarchar(255)
		, PassportNumber nvarchar(255) 
		, TypographicNumber nvarchar(255) 
		, Year int 
	) 
		
	set @commandText = @commandText +     	
		'select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
						certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
						isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year 
		from CommonNationalExamCertificate certificate with (nolock)
		where 
		certificate.[Year] between @yearFrom and @yearTo ' 

	if not @lastName is null 
		set @commandText = @commandText +
			' and certificate.LastName collate cyrillic_general_ci_ai = @lastName '
	
	if not @firstName is null 
		set @commandText = @commandText +
			' and certificate.FirstName collate cyrillic_general_ci_ai = @firstName ' 

	if not @patronymicName is null 
		set @commandText = @commandText +
			' and certificate.PatronymicName collate cyrillic_general_ci_ai = @patronymicName ' 

	if not @internalPassportSeria is null
    begin
    	if CHARINDEX('*', @internalPassportSeria) > 0 or CHARINDEX('?', @internalPassportSeria) > 0
        begin
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '*', '%')
        	set @internalPassportSeria = REPLACE(@internalPassportSeria, '?', '_')
            set @commandText = @commandText +
                ' and certificate.InternalPassportSeria like @internalPassportSeria '
        end
        else begin
            set @commandText = @commandText +
                ' and certificate.InternalPassportSeria = @internalPassportSeria '
        end
	end

	if not @passportNumber is null
    begin
    	if CHARINDEX('*', @passportNumber) > 0 or CHARINDEX('?', @passportNumber) > 0
        begin
        	set @passportNumber = REPLACE(@passportNumber, '*', '%')
        	set @passportNumber = REPLACE(@passportNumber, '?', '_')
            set @commandText = @commandText +
                ' and certificate.PassportNumber like @passportNumber '
        end
    	else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    ' and certificate.PassportNumber = @passportNumber '
        end
    end
	
	if not @typographicNumber is null
		set @commandText = @commandText +
			' and certificate.TypographicNumber = @typographicNumber '
	
	if @lastName is null and @firstName is null and @passportNumber is null
		set @commandText = @commandText +
			' and 0 = 1 '

	print @commandText 
	
	insert into @Search
	exec sp_executesql @commandText
		, N'@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
			, @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int '
		, @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo

	if not @subjectMarks is null
	begin
		delete search 
		from @Search search
			join CommonNationalExamCertificateSubject certificate_subject with(nolock) on certificate_subject.[Year] between @yearFrom and @yearTo 
																			      and search.CertificateId = certificate_subject.CertificateId 
			right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectId = subject_mark.SubjectId 
																	  and certificate_subject.Mark = subject_mark.Mark 
		where certificate_subject.SubjectId + certificate_subject.Mark is null	
	end
	
	set @sourceEntityIds = ''
	
	select @sourceEntityIds = @sourceEntityIds + ',' + Convert(nvarchar(4000), search.CertificateId) 
	from @Search search 
	
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
		
	if @sourceEntityIds = ''
		set @sourceEntityIds = null 

	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

	declare @Search1 table 
	( pkid int identity(1,1) primary key, CertificateId bigint
	)     
	insert @Search1
    select distinct S.CertificateId 
		from @Search S   
	where CertificateId > 0	
	
	declare @CertificateId bigint,@pkid int
	while exists(select * from @Search1)
	begin
	  select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateId = @CertificateId 
                	  
	  delete @Search1 where pkid=@pkid
	end 

	select 
			S.CertificateId, 
			S.CertificateNumber,
			S.LastName LastName,
			S.FirstName FirstName,
			S.PatronymicName PatronymicName,
			S.PassportSeria PassportSeria,
			S.PassportNumber PassportNumber,
			S.TypographicNumber TypographicNumber,
			region.Name RegionName, 
			case 
				when S.CertificateId>0 then 1 
				else 0 
			end IsExist, 
			case 
				when CD.Id >0 then 1 
			end IsDeny,  
			CD.Comment DenyComment, 
			CD.NewCertificateNumber, 
			S.Year,
			case 
				when ed.[ExpireDate] is null then 'Не найдено'
               	when CD.Id>0 then 'Аннулировано' 
               	when getdate() <= ed.[ExpireDate] then 'Действительно' 
                else 'Истек срок' 
			end as [Status], 
            CC.UniqueChecks UniqueChecks, 
			CC.UniqueIHEaFCheck UniqueIHEaFCheck,
			CC.UniqueIHECheck UniqueIHECheck, 
			CC.UniqueIHEFCheck UniqueIHEFCheck, 
			CC.UniqueTSSaFCheck UniqueTSSaFCheck,
			CC.UniqueTSSCheck UniqueTSSCheck, 
			CC.UniqueTSSFCheck UniqueTSSFCheck,
			CC.UniqueRCOICheck UniqueRCOICheck,
			CC.UniqueOUOCheck UniqueOUOCheck, 
			CC.UniqueFounderCheck UniqueFounderCheck,
			CC.UniqueOtherCheck UniqueOtherCheck 
		from 
			@Search S 
            inner join CommonNationalExamCertificate C with (nolock) 
            	on C.Id = S.CertificateId 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
				on CC.Id  = C.Id 
			left outer join CommonNationalExamCertificateDeny CD with (nolock) 
				on CD.[Year] between @yearFrom and @yearTo 
                and S.CertificateNumber = CD.CertificateNumber 
			left outer join dbo.Region region with (nolock)
				on region.[Id] = S.RegionId 
			left join [ExpireDate] ed
            	on ed.[year] = S.[year]
            
	exec dbo.RegisterEvent 
			@accountId = @editorAccountId, 
			@ip = @ip, 
			@eventCode = @eventCode, 
			@sourceEntityIds = @sourceEntityIds, 
			@eventParams = @eventParams, 
			@updateId = null 
			
	return 0
end
GO
PRINT N'Altering [dbo].[SearchCommonNationalExamCertificateCheck]...';


GO
-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Предметы отсортированы по порядку и актуальности.
-- Разделение на части текста по мере заполнения предыдущей части.
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Убрано сравнение HasAppeal. Добавлены IsDeny, DenyComment.
-- v.1.3: Modified by Sedov Anton 04.07.2008
-- В результат запроса  добавлено поле 
-- DenyNewCertificateNumber
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificateCheck]
	@login nvarchar(255)
	, @batchId bigint			-- id пакета
	, @startRowIndex int = 1	-- пейджинг
	, @maxRowCount int = null	-- пейджинг
	, @showCount bit = null		-- если > 0, то выбирается общее кол-во
as
begin
	declare
		@accountId bigint
		, @internalBatchId bigint

	set @internalBatchId = dbo.GetInternalId(@batchId)

	if not exists(select 1
			from dbo.CommonNationalExamCertificateCheckBatch cne_certificate_check_batch with (nolock, 

fastfirstrow)
				inner join dbo.Account account with (nolock, fastfirstrow)
					on cne_certificate_check_batch.OwnerAccountId = account.[Id]
			where 
				cne_certificate_check_batch.Id = @internalBatchId
				and cne_certificate_check_batch.IsProcess = 0
				and (account.[Login] = @login or exists (select top 1 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
				where a2.[Login] = @login)))
		set @internalBatchId = 0

	declare 
		@declareCommandText nvarchar(max)
		, @commandText nvarchar(max)
		, @viewCommandText nvarchar(max)
		, @viewSelectCommandText nvarchar(max)
		, @viewSelectPivotCommandText nvarchar(max)
		, @pivotSubjectColumns nvarchar(max)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortColumn nvarchar(20)
		, @sortAsc bit

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @viewSelectCommandText = ''
	set @viewSelectPivotCommandText = ''
	set @pivotSubjectColumns = ''
	set @sortColumn = N'Id'
	set	@sortAsc = 0

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table 
				( 
				Id bigint 
				, BatchId bigint 
				, CertificateNumber nvarchar(255) 
				, CertificateId bigint 
				, IsOriginal bit 
				, CheckLastName nvarchar(255) 
				, LastName nvarchar(255) 
				, CheckFirstName nvarchar(255) 
				, FirstName nvarchar(255) 
				, CheckPatronymicName nvarchar(255) 
				, PatronymicName nvarchar(255) 
				, IsCorrect bit 
				, IsDeny bit 
				, Year int
				, TypographicNumber nvarchar(255) 
				, [Status] nvarchar(255) 
			    , RegionName nvarchar(255) 
			    , RegionCode nvarchar(10) 
			    , PassportSeria nvarchar(255) 
			    , PassportNumber nvarchar(255) 
				, primary key(id)
				) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	cne_check.Id ' +
				'	, cne_check.BatchId ' +
				'	, cne_check.CertificateNumber ' +
				'	, cne_check.SourceCertificateId CertificateId ' +
				'	, cne_check.IsOriginal ' +
				'	, cne_check.LastName CheckLastName ' +
				'	, cne_check.SourceLastName LastName ' +
				'	, cne_check.FirstName CheckFirstName ' +
				'	, cne_check.SourceFirstName FirstName ' +
				'	, cne_check.PatronymicName CheckPatronymicName ' +
				'	, cne_check.SourcePatronymicName PatronymicName ' +
				'	, cne_check.IsCorrect ' +
				'	, cne_check.IsDeny ' +
				'	, cne_check.Year ' +
				'	, cne_check.TypographicNumber ' +
				'	, case when ed.[ExpireDate] is null then ''Не найдено'' else 
					  case when isnull(cne_check.IsDeny, 0) = 0 and getdate() <= ed.[ExpireDate] then 

''Действительно'' 
					  else ''Истек срок'' end end as [Status] ' +
				'	, r.name as RegionName ' +
				'	, r.code as RegionCode ' +
				'	, cne_check.PassportSeria ' +
				'	, cne_check.PassportNumber ' +
				'from dbo.CommonNationalExamCertificateCheck cne_check with (nolock) ' +
				'left join [ExpireDate] as ed on cne_check.[Year] = ed.[Year] '+
				'left join [Region] as r on cne_check.regionid = r.[Id] '+
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.CommonNationalExamCertificateCheck cne_check with (nolock) ' +
				'where 1 = 1 ' 

	set @commandText = @commandText + 
		'	and cne_check.BatchId = @internalBatchId '

	if isnull(@showCount, 0) = 0
	begin
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end

	if isnull(@showCount, 0) = 0
	begin
		set @declareCommandText = @declareCommandText +
			'declare @check_subject table
				( 
				  CheckId bigint 
				, SubjectId smallint
				, CertificateSubjectId bigint 
				, SubjectCode nvarchar(255) 
				, CheckMark nvarchar(10) 
				, Mark nvarchar(10) 
				, HasAppeal int 
				, IsCorrect int 
				, primary key (CheckId, SubjectId)
				) '

		set @commandText = @commandText +
			' insert into @check_subject 
			select 
				  subject_check.CheckId 
				, subject_check.SubjectId
				, subject_check.SourceCertificateSubjectId 
				, subject.Code 
				, case when subject_check.[Mark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[Mark] as nvarchar(9)),''.'','','')
				, case when subject_check.[SourceMark] < mm.[MinimalMark] then ''!'' else '''' end + 

replace(cast(subject_check.[SourceMark] as nvarchar(9)),''.'','','')
				, cast(subject_check.SourceHasAppeal as int) 
				, cast(subject_check.IsCorrect as int) 
			from dbo.CommonNationalExamCertificateSubjectCheck subject_check with (nolock) 
			inner join dbo.Subject subject on subject.Id = subject_check.SubjectId 
			left join [MinimalMark] as mm on subject_check.[SubjectId] = mm.[SubjectId] and subject_check.Year = 

mm.[Year] 
			where subject_check.BatchId = @internalBatchId 
			order by subject.IsActive desc, subject.SortIndex asc ' 

		set @viewSelectCommandText = 
			' select 
				  dbo.GetExternalId(search.Id) Id 
				, dbo.GetExternalId(search.BatchId) BatchId 
				, search.CertificateNumber 
				, search.IsOriginal 
				, search.CheckLastName 
				, search.LastName 
				, case 
					when search.CheckLastName collate cyrillic_general_ci_ai = search.LastName then 1 
					else 0 
				  end LastNameIsCorrect 
				, search.CheckFirstName 
				, search.FirstName 
				, case 
					when search.CheckFirstName collate cyrillic_general_ci_ai = search.FirstName then 1 
					else 0 
				  end FirstNameIsCorrect 
				, search.CheckPatronymicName 
				, search.PatronymicName 
				, case 
					when search.CheckPatronymicName collate cyrillic_general_ci_ai = 

search.PatronymicName then 1 
					else 0 
				  end PatronymicNameIsCorrect 
				, case when search.CertificateId is not null then 1 else 0 end IsExist 
				, search.IsCorrect 
				, cast(search.IsDeny as bit) IsDeny 
				, cne_check.DenyComment   
				, cne_check.DenyNewCertificateNumber 
				, search.Year SourceCertificateYear
				, search.TypographicNumber 
				, search.[Status] 
				, search.RegionName 
				, search.RegionCode
				, search.PassportSeria 
				, search.PassportNumber ' 
			

		declare
			  @subjectCode nvarchar(255)
			, @pivotSelect nvarchar(max)

		declare subject_cursor cursor fast_forward for
		select s.Code 
		from dbo.Subject s with(nolock) 
		order by s.id asc 

		open subject_cursor 
		fetch next from subject_cursor into @subjectCode
		while @@fetch_status = 0
		begin
			if len(@pivotSubjectColumns) > 0
				set @pivotSubjectColumns = @pivotSubjectColumns + ','
			set @pivotSubjectColumns = @pivotSubjectColumns + '[' + @subjectCode + ']'

			set @viewSelectPivotCommandText = @viewSelectPivotCommandText 
					+ replace(
					'	, chk_mrk_pvt.[<code>] [<code>CheckMark]  
						, mrk_pvt.[<code>] [<code>Mark]  
						, case 
							when chk_mrk_pvt.[<code>] = mrk_pvt.[<code>] then 1 
							else 0 
						end [<code>MarkIsCorrect] 
						, apl_pvt.[<code>] [<code>HasAppeal] 
						, case 
							when not sbj_pvt.[<code>] is null then 1 
							else 0 
						end [<code>IsExist] 
						, crt_pvt.[<code>] [<code>IsCorrect] ' 
					, '<code>', @subjectCode)

			fetch next from subject_cursor into @subjectCode
		end
		close subject_cursor
		deallocate subject_cursor

		set @viewCommandText = replace(
			 ',unique_cheks.UniqueIHEaFCheck
			  from @search search 
				left outer join dbo.CommonNationalExamCertificateCheck cne_check with (nolock) 
					on cne_check.Id = search.Id
				left outer join dbo.ExamCertificateUniqueChecks unique_cheks
					on unique_cheks.Id = search.CertificateId
				left outer join (select 
							check_subject.CheckId 
							, check_subject.SubjectCode 
							, check_subject.CheckMark 
						from @check_subject check_subject) check_subject 
						pivot (min(CheckMark) for SubjectCode in (<subject_columns>)) as 

chk_mrk_pvt 
					on search.Id = chk_mrk_pvt.CheckId 
				left outer join (select 
							check_subject.CheckId 
							, check_subject.SubjectCode 
							, check_subject.Mark 
						from @check_subject check_subject) check_subject 
						pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt 
					on search.Id = mrk_pvt.CheckId 
				left outer join (select 
							check_subject.CheckId 
							, check_subject.SubjectCode 
							, check_subject.HasAppeal 
						from @check_subject check_subject) check_subject 
						pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as apl_pvt 
					on search.Id = apl_pvt.CheckId 
				left outer join (select 
							check_subject.CheckId 
							, check_subject.SubjectCode 
							, check_subject.IsCorrect 
						from @check_subject check_subject) check_subject 
						pivot (Sum(IsCorrect) for SubjectCode in (<subject_columns>)) as crt_pvt 
					on search.Id = crt_pvt.CheckId 
				left outer join (select 
							check_subject.CheckId 
							, check_subject.SubjectCode 
							, check_subject.CertificateSubjectId 
						from @check_subject check_subject) check_subject 
						pivot (Sum(CertificateSubjectId) for SubjectCode in (<subject_columns>)) as 

sbj_pvt 
					on search.Id = sbj_pvt.CheckId ' +
			@resultOrder, '<subject_columns>', @pivotSubjectColumns)
	end

	set @commandText = @declareCommandText + @commandText + @viewSelectCommandText + 
			@viewSelectPivotCommandText + @viewCommandText
	
	declare @params nvarchar(200)

	set @params = 
			'@internalBatchId int '
	print cast(@commandText as ntext)
	exec sp_executesql 
			@commandText
			,@params
			,@internalBatchId 
	
	PRINT @commandText
	PRINT @params
	print @internalBatchId
	
	return 0
end
GO
PRINT N'Altering [dbo].[SearchCommonNationalExamCertificateCheckBatch]...';


GO
-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificateCheckBatch]
	@login nvarchar(255)
	, @startRowIndex int = 1  -- пейджинг
	, @maxRowCount int = null -- пейджинг
	, @showCount bit = null   -- если > 0, то выбирается общее кол-во
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortColumn nvarchar(20)
		, @sortAsc bit
		, @accountId bigint

	if exists ( select top 1 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
				where a2.[Login] = @login )
		set @accountId = null
	else 
		set @accountId = isnull(
			(select account.[Id] 
			from dbo.Account account with (nolock, fastfirstrow) 
			where account.[Login] = @login), 0)

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''
	set @sortColumn = N'CreateDate'
	set	@sortAsc = 0

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table 
				( 
				Id bigint 
				, CreateDate datetime 
				, IsProcess bit 
				, IsCorrect bit 
				, Login varchar(255) 
				, Total int
				, Found int
				) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> 
					b.Id Id 
					, b.CreateDate CreateDate 
					, b.IsProcess IsProcess 
					, b.IsCorrect IsCorrect 
					, a.login Login 
					, (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
					, (select count(SourceCertificateId) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
				from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
				left join account a on a.id = b.OwnerAccountId 
				where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) ' +
				'where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 ' 

	if isnull(@showCount, 0) = 0
	begin
		set @innerOrder = 'order by CreateDate <orderDirection> '
		set @outerOrder = 'order by CreateDate <orderDirection> '
		set @resultOrder = 'order by CreateDate <orderDirection> '

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select 
				dbo.GetExternalId(s.Id) Id 
				, s.Login 
				, s.CreateDate 
				, s.IsProcess 
				, s.IsCorrect 
				, s.Total
				, s.Found
			from @search s ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@accountId bigint'
		, @accountId

	return 0
end
GO
PRINT N'Altering [dbo].[SearchCommonNationalExamCertificateCheckHistory]...';


GO
-- получить все уникальные проверки сертификата вузами и их филиалами
ALTER proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
	@certificateId BIGINT,  -- id сертификата
	@startRow INT = NULL,	-- пейджинг, если null - то выбирается кол-во записей для этого сертификата всего
	@maxRow INT = NULL		-- пейджинг
AS
BEGIN
-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
	SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg 
				INNER JOIN dbo.CommonNationalExamCertificate c ON c.Number = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.Id = @certificateId
	RETURN
END
SELECT   @certificateId AS CertificateId, * FROM (
			select org.Id AS OrganizationId,
			org.FullName AS OrganizationFullName,
			lg.[Date] AS [Date],
			lg.IsBatch AS CheckType,
			DENSE_RANK() OVER(ORDER BY org.FullName) AS org
			FROM dbo.CheckCommonNationalExamCertificateLog lg INNER JOIN dbo.CommonNationalExamCertificate c ON c.Number = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.Id = @certificateId ) rowTable
			WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow
			ORDER BY org, rowTable.[Date] 
END
GO
PRINT N'Altering [dbo].[SearchCommonNationalExamCertificateRequest]...';


GO



















-- =============================================
-- Получить список проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- v.1.1: Modified by Makarev Andrey 06.05.2008
-- Добавлен параметр @AccountId в sp_executesql
-- v.1.2: Modified by Fomin Dmitriy 31.05.2008
-- Добавлены поля IsDeny, DenyComment.
-- v.1.3: Modified by Fomin Dmitriy 02.06.2008
-- Испралена логика: Check -> Request.
-- v.1.4: Modified by Sedov Anton 03.06.2008
-- Добавлен пейджинг
-- Добавлены параметры:
-- @startRowIndex, @maxRowCount, @showCount
-- v.1.5: Modified by Sedov Anton 18.06.2008
-- В результат добавлена выборка данных
-- серии и номера паспорта
-- v.1.6 Modified by Sedov Anton 18.06.2008
-- добавлен параметр расширения запроса
-- @isExtended, при значении 1 возвращаются
-- оценки по экзаменам
-- v.1.7 Modified by Sedov Anton 20.06.2008
-- добавлен параметр расширения запроса
-- @isExtendedbyExam, при 1 получаем
-- список экзаменов в которых участвовал
-- выпускник
-- v.1.8 : Modified by Makarev Andrey 23.06.2008
-- Исправлен пейджинг.
-- v.1.9:  Modified by Sedov Anton 04.07.2008
-- в результат запроса добавлено поле 
-- DenyNewCertificateNumber
-- =============================================
ALTER proc [dbo].[SearchCommonNationalExamCertificateRequest]
	@login nvarchar(255)
	, @batchId bigint
	, @startRowIndex int = 1
	, @maxRowCount int = null
	, @showCount bit = null
	, @isExtended bit = null
	, @isExtendedByExam bit = null
as
begin
	declare 
		@innerBatchId bigint
		, @accountId bigint
		, @commandText nvarchar(max)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @declareCommandText nvarchar(max)
		, @viewSelectCommandText nvarchar(max)
		, @pivotSubjectColumns nvarchar(max)
		, @viewSelectPivot1CommandText nvarchar(max)
		, @viewSelectPivot2CommandText nvarchar(max)
		, @viewCommandText nvarchar(max)
		, @sortColumn nvarchar(20) 
		, @sortAsc bit 

	set @commandText = ''
	set @pivotSubjectColumns = ''
	set @viewSelectPivot1CommandText = ''
	set @viewSelectPivot2CommandText = ''
	set @viewCommandText = ''
	set @viewSelectCommandText = ''
	set @declareCommandText = ''
	set @resultOrder = ''
	set @sortColumn = N'Id'
	set @sortAsc = 1
	
	if @batchId is not null
		set @innerBatchId = dbo.GetInternalId(@batchId)

	--если батч НЕ принадлежит пользователю, который пытается его посмотреть
	--или если смотрит НЕ админ, то не даем посмотреть
	if not exists(select top 1 1
			from dbo.CommonNationalExamCertificateRequestBatch cnecrb with (nolock, fastfirstrow)
				inner join dbo.Account a with (nolock, fastfirstrow)
					on cnecrb.OwnerAccountId = a.[Id]
			where 
				cnecrb.Id = @innerBatchId
				and cnecrb.IsProcess = 0
				and (a.[Login] = @login 
					or exists (select top 1 1 from [Account] as a2
					join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
					join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
					where a2.[Login] = @login)))
		set @innerBatchId = 0

	set @declareCommandText = 
		N'declare @search table 
			(
			Id bigint
			, BatchId bigint
			, CertificateNumber nvarchar(255)
			, LastName nvarchar(255)
			, FirstName nvarchar(255)
			, PatronymicName nvarchar(255)
			, PassportSeria nvarchar(255)
			, PassportNumber nvarchar(255)
			, IsExist bit
			, RegionName nvarchar(255)
			, RegionCode nvarchar(10)
			, IsDeny bit
			, DenyComment ntext
			, DenyNewCertificateNumber nvarchar(255)
			, SourceCertificateId  bigint
			, SourceCertificateYear int
			, TypographicNumber nvarchar(255)
			, [Status] nvarchar(255)
			, primary key(id)
			)
		'

	if isnull(@showCount, 0) = 0
		set @commandText = 
			N'select <innerHeader>
				dbo.GetExternalId(cne_certificate_request.Id) [Id]
				, dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
				, cne_certificate_request.SourceCertificateNumber CertificateNumber
				, isnull(cnec.LastName, cne_certificate_request.LastName) LastName
				, isnull(cnec.FirstName, cne_certificate_request.FirstName) FirstName
				, isnull(cnec.PatronymicName, cne_certificate_request.PatronymicName) PatronymicName
				, isnull(cnec.PassportSeria, cne_certificate_request.PassportSeria) PassportSeria
				, isnull(cnec.PassportNumber, cne_certificate_request.PassportNumber) PassportNumber
				, case
					when not cne_certificate_request.SourceCertificateId is null then 1
					else 0
				end IsExist
				, region.Name RegionName
				, region.Code RegionCode
				, isnull(cne_certificate_request.IsDeny, 0) IsDeny 
				, cne_certificate_request.DenyComment DenyComment
				, cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
				, cne_certificate_request.SourceCertificateId
				, cne_certificate_request.SourceCertificateYear
				, cne_certificate_request.TypographicNumber
				, case when cne_certificate_request.SourceCertificateId is null then ''Не найдено'' else 
					case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= 

ed.[ExpireDate] then ''Действительно'' 
					else ''Истек срок'' end end as [Status]
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
						on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId
					left join [ExpireDate] as ed with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = ed.[Year]	
					left join dbo.CommonNationalExamCertificate cnec with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateId = cnec.id
			where 1 = 1 '
	else
		set @commandText = 
			N'
			select count(*)
			from 
				dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
					inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
						on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
					left outer join dbo.Region region with (nolock)
						on region.[Id] = cne_certificate_request.SourceRegionId
			where 1 = 1 ' 

	set @commandText = @commandText +
			'   and cne_certificate_request_batch.[Id] = @innerBatchId 
				and cne_certificate_request_batch.IsProcess = 0 '

	if isnull(@showCount, 0) = 0
	begin

		if @sortColumn = 'Id'
		begin
			set @innerOrder = ' order by Id <orderDirection> '
			set @outerOrder = ' order by Id <orderDirection> '
			set @resultOrder = ' order by Id <orderDirection> '
		end
		else 
		begin
			set @innerOrder = ' order by Id <orderDirection> '
			set @outerOrder = ' order by Id <orderDirection> '
			set @resultOrder = ' order by Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' + 
				N' select <outerHeader> * ' + 
				N' from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end

	if isnull(@showCount, 0) = 0
	begin
		if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
		begin
			set @declareCommandText = @declareCommandText +
				N' declare @subjects table  
					( 
					CertificateId bigint 
					, Mark nvarchar(10)
					, HasAppeal bit  
					, SubjectCode nvarchar(255)  
					, HasExam bit
					, primary key(CertificateId, SubjectCode)
					) 
				'

			set @commandText = @commandText +
				N'insert into @subjects  
				select
					cne_certificate_subject.CertificateId 
					, case when cne_certificate_subject.[Mark] < mm.[MinimalMark] then ''!'' else '''' 

end + replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''.'','','')
					, cne_certificate_subject.HasAppeal
					, subject.Code
					, 1 
				from	
					dbo.CommonNationalExamCertificateSubject cne_certificate_subject
					left outer join dbo.Subject subject on subject.Id = 

cne_certificate_subject.SubjectId
					left join [MinimalMark] as mm on cne_certificate_subject.[SubjectId] = 

mm.[SubjectId] and cne_certificate_subject.Year = mm.[Year]
				where 
					exists(select 1 
							from @search search
							where cne_certificate_subject.CertificateId = 

search.SourceCertificateId
								and cne_certificate_subject.[Year] = 

search.SourceCertificateYear)
				' 
		end
		
		set @viewSelectCommandText = 
			N'select
				search.Id 
				, search.BatchId
				, search.CertificateNumber
				, search.LastName
				, search.FirstName
				, search.PatronymicName
				, search.PassportSeria
				, search.PassportNumber
				, search.IsExist
				, search.RegionName
				, search.RegionCode
				, search.IsDeny 
				, search.DenyComment
				, search.DenyNewCertificateNumber
				, search.TypographicNumber
				, search.SourceCertificateYear
				, search.Status
			'

		set @viewCommandText = 
			N' ,unique_cheks.UniqueIHEaFCheck
			from @search search 
			left outer join dbo.ExamCertificateUniqueChecks unique_cheks
					on unique_cheks.Id = search.SourceCertificateId '

		if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
		begin 
			declare
				@subjectCode nvarchar(255)
				, @pivotSelect nvarchar(4000)

			set @pivotSelect = ''

			declare subject_cursor cursor fast_forward for
			select s.Code
			from dbo.Subject s with(nolock)
			order by s.id asc 

			open subject_cursor 
			fetch next from subject_cursor into @subjectCode
			while @@fetch_status = 0
				begin
				if len(@pivotSubjectColumns) > 0
					set @pivotSubjectColumns = @pivotSubjectColumns + ','
				set @pivotSubjectColumns = @pivotSubjectColumns + replace('[<code>]', '<code>', 

@subjectCode)
				
				if isnull(@isExtended, 0) = 1
					set @pivotSelect =  
						N'	, mrk_pvt.[<code>] [<code>Mark]  
							, apl_pvt.[<code>] [<code>HasAppeal] '
				if isnull(@isExtendedByExam, 0) = 1
					set @pivotSelect = @pivotSelect + 
						N' 
							, isnull(exam_pvt.[<code>], 0) [<code>HasExam] '
						
				set @pivotSelect = replace(@pivotSelect, '<code>', @subjectCode)

				if len(@viewSelectPivot1CommandText) + len(@pivotSelect) <= 4000
					and @viewSelectPivot2CommandText = ''
					set @viewSelectPivot1CommandText = @viewSelectPivot1CommandText + @pivotSelect
				else
					set @viewSelectPivot2CommandText = @viewSelectPivot2CommandText + @pivotSelect

				fetch next from subject_cursor into @subjectCode
			end
			close subject_cursor
			deallocate subject_cursor
		end

		if isnull(@isExtended, 0) = 1
		begin
			set @viewCommandText = @viewCommandText + 
				N'left outer join (select 
					subjects.CertificateId
					, subjects.Mark 
					, subjects.SubjectCode
					from @subjects subjects) subjects
						pivot (min(Mark) for SubjectCode in (<subject_columns>)) as mrk_pvt
					on search.SourceCertificateId = mrk_pvt.CertificateId
					left outer join (select 
						subjects.CertificateId
						, cast(subjects.HasAppeal as int) HasAppeal 
						, subjects.SubjectCode
						from @subjects subjects) subjects
							pivot (Sum(HasAppeal) for SubjectCode in (<subject_columns>)) as 

apl_pvt
						on search.SourceCertificateId = apl_pvt.CertificateId '
				set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
					
		if isnull(@isExtendedByExam, 0) = 1
		begin
			set @viewCommandText = @viewCommandText + 
				N'left outer join (select 
					subjects.CertificateId
					, subjects.SubjectCode
					, cast(subjects.HasExam as int) HasExam 
					from @subjects subjects) subjects
						pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
					on search.SourceCertificateId = exam_pvt.CertificateId '
					
			set @viewCommandText = replace(@viewCommandText, '<subject_columns>', @pivotSubjectColumns)
		end
	end

	set @viewCommandText = @viewCommandText + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
			@viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

	print @commandText

	exec sp_executesql @commandText
		, N'@innerBatchId bigint'
		, @innerBatchId
		
	return 0
end
GO
PRINT N'Altering [dbo].[SearchCommonNationalExamCertificateWildcard]...';


GO


ALTER proc [dbo].[SearchCommonNationalExamCertificateWildcard]
	@lastName nvarchar(255) = null
	, @firstName nvarchar(255) = null
	, @patronymicName nvarchar(255) = null
	, @passportSeria nvarchar(255) = null
	, @passportNumber nvarchar(255) = null
	, @typographicNumber nvarchar(255) = null
	, @Number nvarchar(255) = null
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @year int = null
	, @startRowIndex int = 1
	, @maxRowCount int = 20
	, @showCount bit = 0
as
begin
	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @eventParams nvarchar(4000)
		, @yearFrom int
		, @yearTo int
		, @commandText nvarchar(max)
		, @internalPassportSeria nvarchar(255)

	set @eventParams = 
		@lastName + '|' 
		+ @firstName + '|' 
		+ @patronymicName + '|' 
		+ isnull(@passportSeria, '') + '|' 
		+ isnull(@passportNumber, '') + '|' 
		+ isnull(@Number, '') + '|' 
		+ isnull(@typographicNumber, '') + '|' 
		+ isnull(cast(@year as varchar(max)), '')

	select
		@editorAccountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login

	if @year is not null
		select @yearFrom = @year, @yearTo = @year
	else
		select @yearFrom = 2008 --Первые св-ва датируются 2008 годом
		select @yearTo = Year(GetDate())

	if not @passportSeria is null
		set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

	select 
		@commandText = ''
		,@eventCode = N'CNE_FND_WLDCRD'

	declare @sourceEntityIds nvarchar(4000)  
	declare @Search table 
	( 
		row int,
		LastName nvarchar(255), 
		FirstName nvarchar(255), 
		PatronymicName nvarchar(255), 
		CertificateId bigint, 
		CertificateNumber nvarchar(255),
		RegionId int, 
		PassportSeria nvarchar(255), 
		PassportNumber nvarchar(255), 
		TypographicNumber nvarchar(255),
		Year int,
		primary key(CertificateNumber, row) 
			) 
			
	if @showCount = 0
	set @commandText = @commandText + 
		' 
		select top (@startRowIndex+@maxRowCount-1)
			  row_number() over (order by c.year, c.id) as row
			, c.LastName 
			, c.FirstName 
			, c.PatronymicName 
			, c.Id 
			, c.Number 
			, c.RegionId 
			, isnull(c.PassportSeria, @internalPassportSeria) PassportSeria 
			, isnull(c.PassportNumber, @passportNumber) PassportNumber
			, c.TypographicNumber 
			, c.Year 
		'
	if @showCount = 1
		set @commandText = ' select count(*) '
	
	set @commandText = @commandText + 
		'
		from dbo.CommonNationalExamCertificate c with (nolock) 
		where 
			c.[Year] between @yearFrom and @yearTo '
	
	if @lastName is not null 
		set @commandText = @commandText + '
			and c.LastName collate cyrillic_general_ci_ai = @lastName'
	if @firstName is not null 
		set @commandText = @commandText + '			
			and c.FirstName collate cyrillic_general_ci_ai = @firstName'
	if @patronymicName is not null 
		set @commandText = @commandText + '						
			and c.PatronymicName collate cyrillic_general_ci_ai = @patronymicName'
	if @internalPassportSeria is not null 
		set @commandText = @commandText + '									
			and c.InternalPassportSeria = @internalPassportSeria'
	if @passportNumber is not null 
		set @commandText = @commandText + '												
			and c.PassportNumber = @passportNumber'
	if @typographicNumber is not null 
		set @commandText = @commandText + '															
			and c.TypographicNumber = @typographicNumber'
	if @Number is not null 
		set @commandText = @commandText + '																		
			and c.Number = @Number '	
			
	if @showCount = 1			
		exec sp_executesql @commandText
			, N'@lastName nvarchar(255)
				, @firstName nvarchar(255)
				, @patronymicName nvarchar(255)
				, @internalPassportSeria nvarchar(255)
				, @passportNumber nvarchar(255)
				, @typographicNumber nvarchar(255) 
				, @Number nvarchar(255)
				, @yearFrom int, @yearTo int 
				, @startRowIndex int
				, @maxRowCount int'
			, @lastName
			, @firstName
			, @patronymicName
			, @internalPassportSeria
			, @passportNumber
			, @typographicNumber
			, @Number
			, @yearFrom
			, @YearTo
			, @startRowIndex
			, @maxRowCount		
	else
	begin	  
		insert into @Search 
		exec sp_executesql @commandText
			, N'@lastName nvarchar(255)
				, @firstName nvarchar(255)
				, @patronymicName nvarchar(255)
				, @internalPassportSeria nvarchar(255)
				, @passportNumber nvarchar(255)
				, @typographicNumber nvarchar(255) 
				, @Number nvarchar(255)
				, @yearFrom int, @yearTo int 
				, @startRowIndex int
				, @maxRowCount int'
			, @lastName
			, @firstName
			, @patronymicName
			, @internalPassportSeria
			, @passportNumber
			, @typographicNumber
			, @Number
			, @yearFrom
			, @YearTo
			, @startRowIndex
			, @maxRowCount		
		
		select 
				search.CertificateNumber 
				, search.LastName LastName 
				, search.FirstName FirstName 
				, search.PatronymicName PatronymicName 
				, search.PassportSeria PassportSeria 
				, search.PassportNumber PassportNumber 
				, search.TypographicNumber TypographicNumber 
				, region.Name RegionName 
				, case 
					when not search.CertificateId is null then 1 
					else 0 
				end IsExist 
				, case 
					when not cne_certificate_deny.Id is null then 1 
					else 0 
				end IsDeny 
				, cne_certificate_deny.Comment DenyComment 
				, cne_certificate_deny.NewCertificateNumber 
				, search.[Year] 
				, case when ed.[ExpireDate] is null then 'Не найдено'  
					   when cne_certificate_deny.Id is not null then 'Аннулировано' 
					   when getdate() <= ed.[ExpireDate] then 'Действительно'
					   else 'Истек срок' 
				  end as [Status]
				, unique_cheks.UniqueIHEaFCheck
			 from @Search search
				left outer join dbo.ExamCertificateUniqueChecks unique_cheks
					on unique_cheks.Id = search.CertificateId 
				left outer join dbo.CommonNationalExamCertificateDeny cne_certificate_deny with (nolock) 
					on cne_certificate_deny.[Year] between @yearFrom and @yearTo 
						and search.CertificateNumber = cne_certificate_deny.CertificateNumber 
				left outer join dbo.Region region with (nolock) 
					on region.[Id] = search.RegionId 
				left join [ExpireDate] ed on  ed.[year] = search.[year] 
			 where row between @startRowIndex and (@startRowIndex+@maxRowCount-1)
			 
			 
			 exec dbo.RegisterEvent 
				@accountId = @editorAccountId  
				, @ip = @ip 
				, @eventCode = @eventCode 
				, @sourceEntityIds = '0'
				, @eventParams = @eventParams 
				, @updateId = null 
	end
				
	return 0
end
GO
PRINT N'Altering [dbo].[UpdateGroupUserEsrp]...';


GO


ALTER PROCEDURE [dbo].[UpdateGroupUserEsrp]
	@login nvarchar(255),
	@groupIdEsrp int,
	@groupsEsrp nvarchar(255)
AS
BEGIN
	declare @accountId int
	select @accountId = A.Id
	from Account A
	where A.[Login] = @login
	
	declare @groupId int
	select @groupId = G.Id
	from [Group] G
	where G.GroupIdEsrp = @groupIdEsrp	
	
	if (@groupsEsrp is not null)	
	begin
		declare @sql nvarchar(1000)
		set @sql =
		'delete from GroupAccount
		where 
			GroupAccount.GroupId in (select G.Id
									 from [Group] G
									 where
										G.GroupIdEsrp not in (' + @groupsEsrp + ') 
									 ) '+
			'and GroupAccount.AccountId = ' + cast(@accountId as nvarchar(255))
		exec sp_executesql @sql
	end
	
	insert into GroupAccount (GroupId, AccountId)
	select @groupId, @accountId 
	where not exists(select *
			   from GroupAccount GA
			   where GA.AccountId = @accountId
			   and GA.GroupId = @groupId)	
	
	exec RefreshRoleActivity @accountId, null
	
END
GO
PRINT N'Altering [dbo].[UpdateUserAccount]...';


GO

-- =============================================
-- Modified 04.05.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
ALTER PROCEDURE [dbo].[UpdateUserAccount]
@login NVARCHAR (255)=null OUTPUT, 
@passwordHash NVARCHAR (255)=null, 
@lastName NVARCHAR (255)=null, 
@firstName NVARCHAR (255)=null, 
@patronymicName NVARCHAR (255)=null, 
@organizationRegionId INT=null, 
@organizationFullName NVARCHAR (2000)=null,
@organizationShortName NVARCHAR (2000)=null, 
@organizationINN NVARCHAR (10)=null, 
@organizationOGRN NVARCHAR (13)=null, 
@organizationFounderName NVARCHAR (2000)=null, 
@organizationFactAddress NVARCHAR (500)=null,
@organizationLawAddress NVARCHAR (500)=null, 
@organizationDirName NVARCHAR (255)=null,
@organizationDirPosition NVARCHAR (500)=null, 
@organizationPhoneCode NVARCHAR (255)=null,
@organizationFax NVARCHAR (255)=null,
@organizationIsAccred bit=0,
@organizationIsPrivate bit=0,
@organizationIsFilial bit=0,
@organizationAccredSert nvarchar(255)=null,
@organizationEMail nvarchar(255)=null,
@organizationSite nvarchar(255)=null, 
@organizationPhone NVARCHAR (255)=null, 
@phone NVARCHAR (255)=null, 
@email NVARCHAR (255)=null, 
@ipAddresses NVARCHAR (4000)=null, 
@status NVARCHAR (255)=null OUTPUT, 
@registrationDocument IMAGE=null, 
@registrationDocumentContentType NVARCHAR (225)=null, 
@editorLogin NVARCHAR (255)=null, 
@editorIp NVARCHAR (255)=null, 
@password NVARCHAR (255)=null, 
@hasFixedIp BIT=null, 
@hasCrocEgeIntegration BIT=null, 
@organizationTypeId INT=null,
@organizationKindId INT=null, 
@ExistingOrgId INT=null
AS
begin
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit
		, @organizationId bigint
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)
		, @userGroupId int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam bit

	set @updateId = newid()
	
	declare @groupCode nvarchar(255)
	set @groupCode = 
		case @organizationTypeId 
			 when 6 then N'UserDepartment'
			 when 4 then N'Auditor'
			 when 3 then N'UserRCOI'
			 else N'User'
		end
	
	select	top 1 @userGroupId = [group].[Id]
	from dbo.[Group] [group] with (nolock, fastfirstrow)
	where [group].[code] = @groupCode
	
	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @editorLogin

	
	if isnull(@login, '') = ''
	begin 
		set @useOnlyDocumentParam = 1
		set @eventCode = N'USR_REG'
	end
	else
	begin
		set @useOnlyDocumentParam = 0
		set @eventCode = N'USR_EDIT'
	end

	if isnull(@login, '') = ''
		select top 1 @login = account.login
		from dbo.Account account with (nolock)
		where account.email = @email
			and dbo.GetUserStatus(@currentYear, 
				account.Status, account.ConfirmYear, account.RegistrationDocument) = 'registration'
		order by account.UpdateDate desc

	if isnull(@login, '') = '' -- внесение нового пользователя
	begin
		set @newAccount = 1

		exec dbo.GetNewUserLogin @login = @login output

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @status = dbo.GetUserStatus(@currentYear, isnull(@status, account.Status), @currentYear
				, @registrationDocument)
			, @registrationDocument = isnull(@registrationDocument, case
				-- Если документ нельзя просмотривать, то считаем, что его нет.
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
					or @useOnlyDocumentParam = 1 
					or isnull(datalength(account.RegistrationDocument),0)=0 
					then null
				else account.RegistrationDocument
			end)
			, @registrationDocumentContentType = case
				when not @registrationDocument is null then @registrationDocumentContentType
				-- Если документ нельзя просмотривать, то считаем, что его нет.
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
					or @useOnlyDocumentParam = 1 				
					then null
				else account.RegistrationDocumentContentType
			end
			, @isOrganizationOwner = account.IsOrganizationOwner
			, @organizationId = account.OrganizationID
			, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
			, @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, account.HasCrocEgeIntegration)
		from dbo.Account account with (nolock, fastfirstrow)
		where account.[Login] = @login


		if @accountId is null
			return 0

		
		insert @oldIpAddress(ip)
		select account_ip.ip
		from dbo.AccountIp account_ip with (nolock, fastfirstrow)
		where account_ip.AccountId = @accountId
	end

	if @hasFixedIp = 1
		insert @newIpAddress(ip)
		select ip_addresses.[value]
		from dbo.GetDelimitedValues(@ipAddresses) ip_addresses

	begin tran insert_update_account_tran

		
		if @newAccount = 1 -- внесение нового пользователя
		begin
			insert dbo.OrganizationRequest2010
				(
				FullName,
				ShortName,
				RegionId,
				TypeId,
				KindId,
				INN,
				OGRN,
				OwnerDepartment,
				IsPrivate,
				IsFilial,
				DirectorPosition,
				DirectorFullName,
				IsAccredited,
				AccreditationSertificate,
				LawAddress,
				FactAddress,
				PhoneCityCode,
				Phone,
				Fax,
				EMail,
				Site,
				OrganizationId
				)
			select
				@organizationFullName,
				@organizationShortName,
				@organizationRegionId,
				@organizationTypeId,
				@organizationKindId,
				@organizationINN,
				@organizationOGRN,		
				@organizationFounderName,
				@organizationIsPrivate,
				@organizationIsFilial,
				@organizationDirPosition,
				@organizationDirName,
				@organizationIsAccred,
				@organizationAccredSert,
				@organizationLawAddress,
				@organizationFactAddress,
				@organizationPhoneCode,
				@organizationPhone,
				@organizationFax,
				@organizationEMail,
				@organizationSite,	
				@ExistingOrgId
				 
			if (@@error <> 0)
				goto undo

			select @organizationId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.Account
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Login]
				, PasswordHash
				, LastName
				, FirstName
				, PatronymicName
				, OrganizationId
				, IsOrganizationOwner
				, ConfirmYear
				, Phone
				, Email
				, RegistrationDocument
				, RegistrationDocumentContentType
				, AdminComment
				, IsActive
				, Status
				, IpAddresses
				, HasFixedIp
				, HasCrocEgeIntegration
				)
			select
				GetDate()
				, GetDate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @login
				, @passwordHash
				, @lastName
				, @firstName
				, @patronymicName
				, @organizationId
				, 1
				, @currentYear
				, @phone
				, @email
				, @registrationDocument
				, @registrationDocumentContentType
				, null
				, 1
				, @status
				, @ipAddresses
				, @hasFixedIp
				, @hasCrocEgeIntegration

			if (@@error <> 0)
				goto undo

			select @accountId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.AccountIp(AccountId, Ip)
			select	@accountId, new_ip_address.ip
			from @newIpAddress new_ip_address

			if (@@error <> 0)
				goto undo

			insert dbo.GroupAccount(GroupId, AccountID)
			select	@UserGroupId, @accountId

			if (@@error <> 0)
				goto undo
		end	
		else 
		begin -- update существующего пользователя
			if @isOrganizationOwner = 1
--				update organization
--				set 
--					UpdateDate = GetDate()
--					, UpdateId = @updateId
--					, EditorAccountId = @editorAccountId
--					, EditorIp = @editorIp
--					, RegionId = @organizationRegionId
--					, DepartmentOwnershipCode = @departmentOwnershipCode
--					, [Name] = @organizationFullName
--					, FounderName = @organizationFounderName
--					, Address = @organizationLawAddress
--					, ChiefName = @organizationDirName
--					, Fax = @organizationFax
--					, Phone = @organizationPhone
--					, ShortName = dbo.GetShortOrganizationName(@organizationFullName)
--					, EducationInstitutionTypeId = @organizationTypeId
--					, EtalonOrgId=@ExistingOrgId
--				from 
--					dbo.Organization organization with (rowlock)
--				where
--					organization.[Id] = @organizationId

				update OReq
				set 
					UpdateDate = GetDate(),
					FullName=@organizationFullName,
					ShortName=@organizationShortName,
					RegionId=@organizationRegionId,
					TypeId=@organizationTypeId,
					KindId=@organizationKindId,
					INN=@organizationINN,
					OGRN=@organizationOGRN,		
					OwnerDepartment=@organizationFounderName,
					IsPrivate=@organizationIsPrivate,
					IsFilial=@organizationIsFilial,
					DirectorPosition=@organizationDirPosition,
					DirectorFullName=@organizationDirName,
					IsAccredited=@organizationIsAccred,
					AccreditationSertificate=@organizationAccredSert,
					LawAddress=@organizationLawAddress,
					FactAddress=@organizationFactAddress,
					PhoneCityCode=@organizationPhoneCode,
					Phone=@organizationPhone,
					Fax=@organizationFax,
					EMail=@organizationEMail,
					Site=@organizationSite,	
					OrganizationId=@ExistingOrgId
				from 
					dbo.OrganizationRequest2010 OReq with (rowlock)
				where
					OReq.[Id] = @organizationId

			if (@@error <> 0)
				goto undo

			update account
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, PasswordHash=isnull(@passwordHash,PasswordHash)
				, EditorIp = @editorIp
				, LastName = @lastName
				, FirstName = @firstName
				, PatronymicName = @patronymicName
				, Phone = @phone
				, Email = @email
				, ConfirmYear = @currentYear
				, Status = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
			from dbo.Account account with (rowlock)
			where account.[Id] = @accountId

			if (@@error <> 0)
				goto undo

			if exists(	select 1 
						from @oldIpAddress old_ip_address
						full outer join @newIpAddress new_ip_address
						on old_ip_address.ip = new_ip_address.ip
						where old_ip_address.ip is null
							or new_ip_address.ip is null) 
			begin
				delete account_ip
				from dbo.AccountIp account_ip
				where account_ip.AccountId = @accountId

				if (@@error <> 0)
					goto undo

				insert dbo.AccountIp(AccountId, Ip)
				select @accountId, new_ip_address.ip
				from @newIpAddress new_ip_address

				if (@@error <> 0)
					goto undo
			end
		end	

-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		begin
			update user_account_password
			set [Password] = @password
			from dbo.UserAccountPassword user_account_password
			where user_account_password.AccountId = @accountId

			if (@@error <> 0)
				goto undo
		end
		else
		begin
			insert dbo.UserAccountPassword(AccountId, [Password])
			select @accountId, @password

			if (@@error <> 0)
				goto undo
		end
	end

	if @@trancount > 0
		commit tran insert_update_account_tran

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RefreshRoleActivity @accountId = @accountId

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:
	rollback tran insert_update_account_tran
	return 1
end
GO
PRINT N'Creating [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]...';


GO
-- =============================================
-- Удаление проверки из лога
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE PROCEDURE dbo.DeleteCheckFromCommonNationalExamCertificateCheckBatchById
	@id bigint
as
begin
	declare @internalId bigint
	set @internalId = dbo.GetInternalId(@id)

	DELETE FROM [dbo].[CommonNationalExamCertificateCheckBatch]
      WHERE [Id]=@internalId
end
GO
PRINT N'Creating [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]...';


GO

CREATE PROCEDURE dbo.DeleteCheckFromCommonNationalExamCertificateRequestBatchById
	@id bigint
as
begin
	declare @internalId bigint
	set @internalId = dbo.GetInternalId(@id)

	DELETE FROM [dbo].[CommonNationalExamCertificateRequestBatch]
      WHERE [Id]=@internalId
end
GO
PRINT N'Creating [dbo].[GetCertificateByFioAndPassport]...';


GO
-- Получение свидетельст за все года по ФИО и паспортным данным
-- Возвращаемы значения
-- Id - идентификатор свидетельства
-- CreateDate - Дата добавления сертификата
-- Number - номер свидетельства
-- Year - год
CREATE PROCEDURE [dbo].[GetCertificateByFioAndPassport]
	@LastName NVARCHAR(255),				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255),				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255),			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20) = NULL,		-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20) = NULL,		-- номер документа сертифицируемого (паспорта)	
	@CurrentCertificateNumber NVARCHAR(255)		-- номер свидетельства, его нужно исключить при выборке
AS
BEGIN	
	declare @yearFrom int, @yearTo int
	select @yearFrom = 2008, @yearTo = Year(GetDate())
	
	SELECT [certificate].[Id], [certificate].[CreateDate], [certificate].[Number], [certificate].[Year], 
		case when ed.[ExpireDate] is null then 'Не найдено' else 
	    case when isnull(certificate_deny.[Id],0) <> 0 then 'Аннулировано' else
	    case when getdate() <= ed.[ExpireDate] then 'Действительно'
	    else 'Истек срок' end end end as [Status]
	FROM (SELECT [Id],[CreateDate],[Number],[Year]
			FROM [dbo].[CommonNationalExamCertificate]
			WHERE (LastName=@LastName AND FirstName=@FirstName AND PatronymicName=@PatronymicName AND PassportNumber=@PassportNumber AND PassportSeria=@PassportSeria)) [certificate]
		  LEFT JOIN [ExpireDate] as ed on [certificate].[Year] = ed.[Year]	
		  LEFT OUTER JOIN CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow) on certificate_deny.[Year] between @yearFrom and @yearTo
				and certificate_deny.certificateNumber = [certificate].[Number]
	WHERE [certificate].Number <> @CurrentCertificateNumber
END
GO
PRINT N'Creating [dbo].[GetNEWebUICheckLog]...';


GO
create proc [dbo].[GetNEWebUICheckLog]
	@login nvarchar(255), @startRowIndex int = 1,@maxRowCount int = null, @showCount bit = null,   -- если > 0, то выбирается общее кол-во
	@TypeCode nvarchar(255) -- Тип проверки
as
begin
	declare @sortAsc bit, @accountId bigint, @endRowIndex integer
	set @sortAsc=0

	if isnull(@maxRowCount, -1) = -1 
		set @endRowIndex = 10000000
	else
		set @endRowIndex = @startRowIndex + @maxRowCount

	if exists ( select 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
				where a2.[Login] = @login )
		set @accountId = null
	else 
		set @accountId = isnull(
			(select account.[Id] 
			from dbo.Account account with (nolock, fastfirstrow) 
			where account.[Login] = @login), 0)

	if isnull(@showCount, 0) = 0
	begin	
		if @sortAsc = 1
			if @accountId is null 
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber, 
							2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
							c.[login] [login],EventDate,
						row_number() over (order by b.EventDate asc) rn 
					from 
					(
						select top (@endRowIndex) b.id from dbo.CNEWebUICheckLog b with (nolock) 
						where @TypeCode=TypeCode
						order by b.id
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id
				) s 
				where s.rn between @startRowIndex and @endRowIndex
			else
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber, 
							2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
							c.[login] [login],EventDate,
						row_number() over (order by b.EventDate asc) rn 
					from 
					(
						select top (@endRowIndex) b.id from dbo.CNEWebUICheckLog b with (nolock) 
						where b.AccountId = @accountId and @TypeCode=TypeCode
						order by b.id
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id
				) s 
				where s.rn between @startRowIndex and @endRowIndex			
		else
			if @accountId is null 
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber,
						2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
						c.[login] [login],EventDate,		
						row_number() over (order by b.EventDate desc) rn 
					from
					(
						select top (@endRowIndex) b.id from dbo.CNEWebUICheckLog b with (nolock) 
						where @TypeCode=TypeCode
						order by b.EventDate desc					
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id								
				) s 
				where s.rn between @startRowIndex and @endRowIndex				
			else
				select *
				from 
				(
					select top (@endRowIndex) b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber,
						2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
						c.[login] [login],EventDate,		
						row_number() over (order by b.EventDate desc) rn 
					from
					(
						select top (@endRowIndex) b.id from dbo.CNEWebUICheckLog b with (nolock) 
						where b.AccountId = @accountId and @TypeCode=TypeCode
						order by b.id						
					) a join CNEWebUICheckLog b on a.id=b.id	
						join Account c on b.AccountId=c.id
					order by b.Id				
				) s 
				where s.rn between @startRowIndex and @endRowIndex							
	end
	else
		if @accountId is null 
			select count(*) 
			from dbo.CNEWebUICheckLog b with (nolock) 
			where @TypeCode=TypeCode
		else
			select count(*) 
			from dbo.CNEWebUICheckLog b with (nolock) 
			where b.AccountId = @accountId and @TypeCode=TypeCode

	return 0
end
GO
PRINT N'Creating [dbo].[SelectCNECCheckHystoryByOrgId]...';


GO
create proc [dbo].[SelectCNECCheckHystoryByOrgId]
	@idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)='id', @isUnique bit=0,
	@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max),@ss nvarchar(10),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max),@yearFrom int, @yearTo int

	create table #tt(id [int] NOT NULL primary key)
	create table #t1(id [int] NOT NULL primary key)
		
	select @yearFrom = 2008, @yearTo = Year(GetDate())
				
	if @isUnique =0 
	begin
		if @fld='id'	
		begin
			if @so = 0 
			begin
				insert #tt
				select id from Account where OrganizationId=@idorg
				
				insert #t1
				select top (@pf) cb.id 
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id asc		
				
				select * from
				(
					select *,row_number() over (order by id asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end
						order by c.Id asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id	
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end															
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end
			else	
			begin	
				insert #tt
				select id from Account where OrganizationId=@idorg
			
				insert #t1
				select top (@pf) cb.id
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id desc		
				
				select * from
				(
					select *,row_number() over (order by id desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 		
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end						
						order by c.Id desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf			
			end	
		end
		if @fld='TypeCheck'
		begin
			if @so = 0 
			begin
				insert #tt
				select id from Account where OrganizationId=@idorg
				
				insert #t1
				select top (@pf) cb.id
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id asc		
				
				select * from
				(
					select *,row_number() over (order by TypeCheck asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.Id asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 		
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end						
						order by c.Id asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end
			else	
			begin	
				insert #tt
				select id from Account where OrganizationId=@idorg
			
				insert #t1
				select top (@pf) cb.id 
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.Id desc		
				
				select * from
				(
					select *,row_number() over (order by TypeCheck desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.Id desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf			
			end		
		end			
		if @fld='LastName'	
		begin
			if @so = 0 
			begin
				insert #tt
				select id from Account where OrganizationId=@idorg
				
				insert #t1
				select top (@pf) cb.id
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.LastName asc		
				
				select * from
				(
					select *,row_number() over (order by LastName asc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName asc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName asc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end		
			else
			begin
				insert #tt
				select id from Account where OrganizationId=@idorg
				
				insert #t1
				select top (@pf) cb.id 
				from CNEWebUICheckLog cb
					join #tt Acc on acc.id=cb.AccountId 		
				order by cb.LastName desc		
				
				select * from
				(
					select *,row_number() over (order by LastName desc) rn from
						(				
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						   case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
							left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id
							left join ExamcertificateUniqueChecks CC on CNE.id=cc.id				
							left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = c.CertificateNumber 
						where @idorg=Acc.OrganizationId 	
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end							
						order by c.LastName desc
						union all
						select top (@pf) c.Id,cb.CreateDate,'Пакетная' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							c.PassportSeria+' '+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						FROM CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
							left join ExamcertificateUniqueChecks CC on r.id=cc.id
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
							left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
							left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = r.Number 
						where @idorg=Acc.OrganizationId 
							and 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
							and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Пакетная' then 1 else 0 end								
						order by c.LastName desc				
						union all
						select t.*,
							case when ed.[ExpireDate] is null then 'Не найдено'  
								when certificate_deny.Id>0 then 'Аннулировано' 
								when getdate() <= ed.[ExpireDate] then 'Действительно'
							else 'Истек срок' end Status
						from 
						(
							select cb.id,cb.EventDate,'Интерактивная' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
							   c.PassportSeria+' '+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
							from 
							#t1 cb1
								join CNEWebUICheckLog cb on cb1.id=cb.id
								left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
								left join ExamcertificateUniqueChecks CC on c.id=cc.id								
							where 1 = case when @LastName is null then 1 when c.LastName like '%'+@LastName+'%' then 1 else 0 end
								and 1 = case when @TypeCheck is null then 1 when @TypeCheck= 'Интерактивная' then 1 else 0 end									
						) t
							left join [ExpireDate] as ed on ed.[Year] = t.[Year]
							left join CommonNationalExamcertificateDeny certificate_deny 
								on certificate_deny.[Year] between @yearFrom and @yearTo
									and certificate_deny.certificateNumber = t.CertificateNumber 				
						) t
				) t
				where rn between @ps and @pf		
			end	
		end					
	end					
	else
	begin
		if @fld='id'	
		begin
			set @fldso1='c.Id'
			set @fldso2='c.Id'		
			set @fldso3='cb.Id'				
		end	
		if @fld='TypeCheck'
		begin
			set @fldso1='c.Id'
			set @fldso2='c.Id'
			set @fldso3='cb.Id'				
		end			
	
		if @fld='LastName'	
		begin
			set @fldso1='c.LastName'
			set @fldso2='c.LastName'
			set @fldso3='c.LastName'				
		end			
			
		if @so=1 
			set @ss=' desc'
		else
			set @ss=' asc'	
	
		set @str='
				declare @yearFrom int, @yearTo int		
				select @yearFrom = 2008, @yearTo = Year(GetDate())
				
				select top (@pf) * into #ttt
				from 
				(						
					select top (@pf) c.Id,cb.CreateDate,''Пакетная'' TypeCheck,c.CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
					   c.PassportSeria+'' ''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
					   case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.Id>0 then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					FROM 
						(select min(c.id) id,c.CertificateNumber 
						 from CommonNationalExamCertificateCheck c
							JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id '
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '														
		if @idorg<>-1 
			set @str=@str+'						
							JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
		if @LastName is not null
			set @str=@str+'
							where c.LastName like ''%''+@LastName+''%'' '			
		set @str=@str+'								
						group by CertificateNumber) cb1	
															
						join CommonNationalExamCertificateCheck c on cb1.id=c.id 															
						JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 								
						left join CommonNationalExamCertificate CNE on c.SourceCertificateId=CNE.Id			
						left join ExamcertificateUniqueChecks CC on CNE.id=cc.id
						left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
						left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
							on certificate_deny.[Year] between @yearFrom and @yearTo
								and certificate_deny.certificateNumber = c.CertificateNumber 	

					order by '+@fldso1+@ss + '
					union all
					select top (@pf) c.Id,cb.CreateDate,''Пакетная'' TypeCheckk,r.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						c.PassportSeria+'' ''+c.PassportNumber PassportData,cb.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
						case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.Id>0 then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					FROM 
						(select min(c.id) id, r.Number 
						from CommonNationalExamCertificateRequest c 
							JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
							left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id '
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+' '					
		else
				set @str=@str+'						
							and 1=0 '								
		if @idorg<>-1 
			set @str=@str+'								
				  			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId '
		if @LastName is not null
			set @str=@str+'
							where c.LastName like ''%''+@LastName+''%'''						  			
		set @str=@str+'					  		
						group by r.Number
						) cb1
						
						join CommonNationalExamCertificateRequest c on cb1.id=c.id   
						JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id						 
						left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
						left join ExamcertificateUniqueChecks CC on r.id=cc.id				
						left join [ExpireDate] as ed on ed.[Year] = cb.[Year]
						left outer join CommonNationalExamcertificateDeny certificate_deny with (nolock, fastfirstrow)
							on certificate_deny.[Year] between @yearFrom and @yearTo
								and certificate_deny.certificateNumber = r.Number '
		set @str=@str+'		
					order by '+@fldso1+@ss + '				
					union all
					select t.*,
						case when ed.[ExpireDate] is null then ''Не найдено''  
							when certificate_deny.Id>0 then ''Аннулировано'' 
							when getdate() <= ed.[ExpireDate] then ''Действительно''
						else ''Истек срок'' end Status
					from 
					(
						select top (@pf) cb.id,cb.EventDate,''Интерактивная'' TypeCheck,c.Number CertificateNumber,c.TypographicNumber,c.LastName,c.FirstName,c.PatronymicName,
						   c.PassportSeria+'' ''+c.PassportNumber PassportData,c.[year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck		   
						from 
						(
							select max(cb.id) id,c.Number from
							(
								select cb.id,cb.FoundedCNEId
								from CNEWebUICheckLog cb  with(index(I_CNEWebUICheckLog_AccId))
									join Account Acc with(index(accOrgIdIndex)) on acc.id=cb.AccountId	'	
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '										
		if @idorg<>-1	
			set @str=@str+'
								where @idorg=Acc.OrganizationId '								
		set @str=@str+'		
							) cb
							left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id '
							
		if @LastName is not null
			set @str=@str+'
							where c.LastName like ''%''+@LastName+''%'''									
		set @str=@str+'							
							group by c.Number	
						) cb1
							join CNEWebUICheckLog cb with(index(CNEWebUICheckLog_Id)) on cb1.id=cb.id
							left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id		
							left join ExamcertificateUniqueChecks CC on c.id=cc.id
						order by '+@fldso1+@ss + '				
					) t
						left join [ExpireDate] as ed on ed.[Year] = t.[Year]
						left join CommonNationalExamcertificateDeny certificate_deny 
							on certificate_deny.[Year] between @yearFrom and @yearTo
								and certificate_deny.certificateNumber = t.CertificateNumber 				
					) t
					
		select * from
		(
			select *,row_number() over (order by '+@fld+@ss+') rn from
				(
				select a.* from #ttt a
					join (select min(id)id,CertificateNumber from #ttt group by CertificateNumber) b on a.id=b.id
				) t
		) t
		where rn between @ps and @pf
		
		drop table #ttt	'
		
	end
	
	print @str
	
	exec sp_executesql @str,N'@idorg int,@ps int,@pf int',@idorg=@idorg,@ps=@ps,@pf=@pf
	
	drop table #tt
	drop table #t1	
end
GO
PRINT N'Creating [dbo].[SelectCNECCheckHystoryByOrgIdCount]...';


GO
create proc [dbo].[SelectCNECCheckHystoryByOrgIdCount]
	@idorg int, @isUnique bit=0,@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null
as
begin
	declare @str nvarchar(max)

	if @isUnique =0 
	begin
		set @str='select count(*) from
				(
				select 1 t
				FROM CommonNationalExamCertificateCheck c
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '							
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '			
		set @str=@str+'	
				union all
				select 1 t 
				FROM CommonNationalExamCertificateRequest c 
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
					left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '							
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '
		set @str=@str+'
				union all
				select 1 t
				from CNEWebUICheckLog cb with(index(I_CNEWebUICheckLog_AccId))
					JOIN Account Acc with(index(accOrgIdIndex)) ON cb.AccountId=Acc.Id			
					left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id '	
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '						
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '							
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '
		set @str=@str+'				
				) t'
	end
	else
	begin
		set @str='select count(distinct t) from
				(
				select distinct c.CertificateNumber t
				FROM CommonNationalExamCertificateCheck c
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '							
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '								
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '			
		set @str=@str+'	
				union all
				select distinct r.Number t 
				FROM CommonNationalExamCertificateRequest c 
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
					left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId '
		if @LastName is not null
			set @str=@str+'
							and c.LastName like ''%''+@LastName+''%'' '							
		if @TypeCheck= 'Пакетная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							and 1=0 '								
		if @idorg<>-1 
			set @str=@str+'
				where @idorg=Acc.OrganizationId '
		set @str=@str+'
				union all
				select distinct c.Number t 	
				from(
					select distinct cb.FoundedCNEId,cb.AccountId
					from CNEWebUICheckLog cb '
		if @idorg<>-1 
			set @str=@str+'					
						jOIN Account Acc ON cb.AccountId=Acc.Id	and 7245=Acc.OrganizationId	'						
		if @TypeCheck= 'Интерактивная' or @TypeCheck is null
				set @str=@str+'	'					
		else
				set @str=@str+'						
							where 1=0 '							
		set @str=@str+'							
					) cb
					left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id '
		if @LastName is not null
			set @str=@str+'
						and c.LastName like ''%''+@LastName+''%'' '							
		set @str=@str+'		) t'	
	end
	print @str
	exec sp_executesql @str,N'@idorg int',@idorg=@idorg
		
end
GO
