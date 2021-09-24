-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (5, '005_2012_04_28_UpdateDBGlobal.sql')
-- =========================================================================
GO


DROP TRIGGER [dbo].[Migrations_tri];
GO

PRINT N'Dropping DF_Group_IsUserIS...';


GO
ALTER TABLE [dbo].[Group] DROP CONSTRAINT [DF_Group_IsUserIS];


GO
PRINT N'Dropping FK_Group_System...';


GO
ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_System];


GO
PRINT N'Dropping FK_GroupAccount_Group...';


GO
ALTER TABLE [dbo].[GroupAccount] DROP CONSTRAINT [FK_GroupAccount_Group];


GO
PRINT N'Dropping FK_OrganizationRequestAccount_Group...';


GO
ALTER TABLE [dbo].[OrganizationRequestAccount] DROP CONSTRAINT [FK_OrganizationRequestAccount_Group];


GO
PRINT N'Dropping FK_GroupAccount_Account...';


GO
ALTER TABLE [dbo].[GroupAccount] DROP CONSTRAINT [FK_GroupAccount_Account];


GO
PRINT N'Dropping FK_OrganizationRequestAccount_OrganizationRequest2010...';


GO
ALTER TABLE [dbo].[OrganizationRequestAccount] DROP CONSTRAINT [FK_OrganizationRequestAccount_OrganizationRequest2010];


GO
PRINT N'Dropping [dbo].[Attachment]...';


GO
DROP TABLE [dbo].[Attachment];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateFormActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateFormActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubjectFormActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubjectFormActivePartition];


GO
PRINT N'Dropping [dbo].[fbs_ip]...';


GO
DROP TABLE [dbo].[fbs_ip];


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
PRINT N'Dropping [dbo].[OrganizationRequestStatus]...';


GO
DROP TABLE [dbo].[OrganizationRequestStatus];


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
PRINT N'Dropping [dbo].[tmpVUZ]...';


GO
DROP TABLE [dbo].[tmpVUZ];


GO
PRINT N'Dropping [dbo].[UpdatingCommonNationalExamCertificateForm35]...';


GO
DROP TABLE [dbo].[UpdatingCommonNationalExamCertificateForm35];


GO
PRINT N'Dropping [dbo].[UpdatingCommonNationalExamCertificateSubjectForm35]...';


GO
DROP TABLE [dbo].[UpdatingCommonNationalExamCertificateSubjectForm35];


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
PRINT N'Dropping [dbo].[GetFirstNonRusPosition]...';


GO
DROP FUNCTION [dbo].[GetFirstNonRusPosition];


GO
PRINT N'Dropping [dbo].[GetFirstNonRusPosition2]...';


GO
DROP FUNCTION [dbo].[GetFirstNonRusPosition2];


GO
PRINT N'Dropping [dbo].[GetIp]...';


GO
DROP FUNCTION [dbo].[GetIp];


GO
PRINT N'Dropping [dbo].[GetOrganizationEducationInstitutionTypeIdByName]...';


GO
DROP FUNCTION [dbo].[GetOrganizationEducationInstitutionTypeIdByName];


GO
PRINT N'Dropping [dbo].[HasUserAccountAdminComment]...';


GO
DROP FUNCTION [dbo].[HasUserAccountAdminComment];


GO
PRINT N'Dropping [dbo].[DateOnly]...';


GO
DROP FUNCTION [dbo].[DateOnly];


GO
PRINT N'Dropping [dbo].[GetDataDbName]...';


GO
DROP FUNCTION [dbo].[GetDataDbName];


GO
PRINT N'Dropping [dbo].[GetHash_SHA1]...';


GO
DROP FUNCTION [dbo].[GetHash_SHA1];


GO
PRINT N'Dropping [dbo].[GetInternalPassportSeria]...';


GO
DROP FUNCTION [dbo].[GetInternalPassportSeria];


GO
PRINT N'Dropping [dbo].[GetDelimitedValues2]...';


GO
DROP FUNCTION [dbo].[GetDelimitedValues2];


GO
PRINT N'Dropping [dbo].[ReportCertificateLoadShortTVF]...';


GO
DROP FUNCTION [dbo].[ReportCertificateLoadShortTVF];


GO
PRINT N'Dropping [dbo].[GetCommonNationalExamCertificateActuality]...';


GO
DROP FUNCTION [dbo].[GetCommonNationalExamCertificateActuality];


GO
PRINT N'Dropping [dbo].[ReportOrganizationCheckStatisticsTVF]...';


GO
DROP FUNCTION [dbo].[ReportOrganizationCheckStatisticsTVF];


GO
PRINT N'Dropping [dbo].[ReportTopCheckingOrganizationsTVF]...';


GO
DROP FUNCTION [dbo].[ReportTopCheckingOrganizationsTVF];


GO
PRINT N'Dropping [dbo].[ReportUserStatusTVF]...';


GO
DROP FUNCTION [dbo].[ReportUserStatusTVF];


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
PRINT N'Dropping [dbo].[Отчет о наиболее активных организациях за год]...';


GO
DROP VIEW [dbo].[Отчет о наиболее активных организациях за год];


GO
PRINT N'Dropping [dbo].[Отчет о наиболее активных организациях за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о наиболее активных организациях за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о наиболее активных организациях за неделю]...';


GO
DROP VIEW [dbo].[Отчет о наиболее активных организациях за неделю];


GO
PRINT N'Dropping [dbo].[Отчет о регистрации пользователей за год]...';


GO
DROP VIEW [dbo].[Отчет о регистрации пользователей за год];


GO
PRINT N'Dropping [dbo].[Отчет о регистрации пользователей за 24 часа]...';


GO
DROP VIEW [dbo].[Отчет о регистрации пользователей за 24 часа];


GO
PRINT N'Dropping [dbo].[Отчет о регистрации пользователей за неделю]...';


GO
DROP VIEW [dbo].[Отчет о регистрации пользователей за неделю];


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
PRINT N'Dropping [dbo].[CheckEntrant]...';


GO
DROP PROCEDURE [dbo].[CheckEntrant];


GO
PRINT N'Dropping [dbo].[CheckSchoolLeavingCertificate]...';


GO
DROP PROCEDURE [dbo].[CheckSchoolLeavingCertificate];


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
PRINT N'Dropping [dbo].[GetEntrantCheckBatch]...';


GO
DROP PROCEDURE [dbo].[GetEntrantCheckBatch];


GO
PRINT N'Dropping [dbo].[GetNewUserLogin]...';


GO
DROP PROCEDURE [dbo].[GetNewUserLogin];


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
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificate]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificate2]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificate2];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateCheck2]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheck2];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateRequest2]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequest2];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateWildcard]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateWildcard];


GO
PRINT N'Dropping [dbo].[SearchCompetitionCertificate]...';


GO
DROP PROCEDURE [dbo].[SearchCompetitionCertificate];


GO
PRINT N'Dropping [dbo].[SearchCompetitionCertificateRequest]...';


GO
DROP PROCEDURE [dbo].[SearchCompetitionCertificateRequest];


GO
PRINT N'Dropping [dbo].[SearchCompetitionCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[SearchCompetitionCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[SearchCompetitionType]...';


GO
DROP PROCEDURE [dbo].[SearchCompetitionType];


GO
PRINT N'Dropping [dbo].[SearchDynamicIpUserAccount]...';


GO
DROP PROCEDURE [dbo].[SearchDynamicIpUserAccount];


GO
PRINT N'Dropping [dbo].[SearchEducationInstitutionType]...';


GO
DROP PROCEDURE [dbo].[SearchEducationInstitutionType];


GO
PRINT N'Dropping [dbo].[SearchEntrantCheck]...';


GO
DROP PROCEDURE [dbo].[SearchEntrantCheck];


GO
PRINT N'Dropping [dbo].[SearchEntrantCheckBatch]...';


GO
DROP PROCEDURE [dbo].[SearchEntrantCheckBatch];


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
PRINT N'Dropping [dbo].[SearchSchoolLeavingCertificateCheck]...';


GO
DROP PROCEDURE [dbo].[SearchSchoolLeavingCertificateCheck];


GO
PRINT N'Dropping [dbo].[SearchSchoolLeavingCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[SearchSchoolLeavingCertificateCheckBatch];


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
PRINT N'Dropping ImportingCertificatePK...';


GO
ALTER TABLE [dbo].[ImportingCommonNationalExamCertificate] DROP CONSTRAINT [ImportingCertificatePK];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificate].[cnecIndex]...';


GO
DROP INDEX [cnecIndex]
    ON [dbo].[ImportingCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificate].[IdxImportingCertificateNumber]...';


GO
DROP INDEX [IdxImportingCertificateNumber]
    ON [dbo].[ImportingCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificate].[IdxImportingCertificateOwner]...';


GO
DROP INDEX [IdxImportingCertificateOwner]
    ON [dbo].[ImportingCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificate].[indexForSearchCommonNationalExamCertificateProc]...';


GO
DROP INDEX [indexForSearchCommonNationalExamCertificateProc]
    ON [dbo].[ImportingCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateDenyActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateDenyActivePartition];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubjectActivePartition]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubjectActivePartition];


GO
PRINT N'Dropping [dbo].[CompetitionCertificate]...';


GO
DROP TABLE [dbo].[CompetitionCertificate];


GO
PRINT N'Dropping [dbo].[CompetitionCertificateRequest]...';


GO
DROP TABLE [dbo].[CompetitionCertificateRequest];


GO
PRINT N'Dropping [dbo].[CompetitionType]...';


GO
DROP TABLE [dbo].[CompetitionType];


GO
PRINT N'Dropping [dbo].[DeprecatedCommonNationalExamCertificate]...';


GO
DROP TABLE [dbo].[DeprecatedCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[DeprecatedCommonNationalExamCertificateDeny]...';


GO
DROP TABLE [dbo].[DeprecatedCommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[EntrantCheck]...';


GO
DROP TABLE [dbo].[EntrantCheck];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificateDeny]...';


GO
DROP TABLE [dbo].[ImportingCommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[SchoolLeavingCertificateCheck]...';


GO
DROP TABLE [dbo].[SchoolLeavingCertificateCheck];


GO
PRINT N'Dropping [dbo].[SchoolLeavingCertificateDeny]...';


GO
DROP TABLE [dbo].[SchoolLeavingCertificateDeny];


GO
PRINT N'Dropping [dbo].[UserAccountActivityCommonReport]...';


GO
DROP TABLE [dbo].[UserAccountActivityCommonReport];


GO
PRINT N'Dropping [work]...';


GO
DROP USER [work];


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datareader', @membername = N'tsokolova';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datareader', @membername = N'fogsoft\akonovalova';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datareader', @membername = N'esrp';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datareader', @membername = N'FBS_Login';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datawriter', @membername = N'fogsoft\akonovalova';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datawriter', @membername = N'esrp';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datawriter', @membername = N'FBS_Login';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_owner', @membername = N'FBSUser';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_owner', @membername = N'esrp';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_owner', @membername = N'FBS_Login';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_owner', @membername = N'fbs';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_owner', @membername = N'fbsdbUser';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_owner', @membername = N'rtrunov';


GO
PRINT N'Dropping [esrp]...';


GO
DROP USER [esrp];


GO
PRINT N'Dropping [fbs]...';


GO
DROP USER [fbs];


GO
PRINT N'Dropping [FBS_Login]...';


GO
DROP USER [FBS_Login];


GO
PRINT N'Dropping [fbsdbUser]...';


GO
DROP USER [fbsdbUser];


GO
PRINT N'Dropping [FBSUser]...';


GO
DROP USER [FBSUser];


GO
PRINT N'Dropping [rtrunov]...';


GO
DROP USER [rtrunov];


GO
PRINT N'Dropping [tsokolova]...';


GO
DROP USER [tsokolova];


GO
PRINT N'Creating [Organization2010]...';


GO
CREATE FULLTEXT CATALOG [Organization2010]
    WITH ACCENT_SENSITIVITY = ON
    AUTHORIZATION [dbo];


GO
PRINT N'Starting rebuilding table [dbo].[Group]...';


GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

BEGIN TRANSACTION;

CREATE TABLE [dbo].[tmp_ms_xx_Group] (
    [Id]       INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Code]     NVARCHAR (255) NOT NULL,
    [Name]     NVARCHAR (255) NOT NULL,
    [SystemID] INT            NOT NULL,
    [Default]  BIT            CONSTRAINT [DF_Group_Default] DEFAULT ((0)) NOT NULL,
    [IsUserIS] BIT            CONSTRAINT [DF_Group_IsUserIS] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

IF EXISTS (SELECT TOP 1 1
           FROM   [dbo].[Group])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Group] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Group] ([Id], [Code], [Name], [SystemID], [IsUserIS])
        SELECT   [Id],
                 [Code],
                 [Name],
                 [SystemID],
                 [IsUserIS]
        FROM     [dbo].[Group]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Group] OFF;
    END

DROP TABLE [dbo].[Group];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Group]', N'Group';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[Organization2010]...';


GO
ALTER TABLE [dbo].[Organization2010]
    ADD [StatusId]         INT            DEFAULT ((1)) NOT NULL,
        [Version]          INT            DEFAULT ((1)) NOT NULL,
        [DateChangeStatus] DATETIME       NULL,
        [Reason]           NVARCHAR (100) NULL;

GO
PRINT N'Starting rebuilding table [dbo].[System]...';


GO
/*
The column [dbo].[System].[AvailableRegistration] on table [dbo].[System] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue, you must add a default value to the column or mark it as allowing NULL values.

The column [dbo].[System].[FullName] on table [dbo].[System] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue, you must add a default value to the column or mark it as allowing NULL values.
*/
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

BEGIN TRANSACTION;

CREATE TABLE [dbo].[tmp_ms_xx_System] (
    [SystemID]              INT             IDENTITY (1, 1) NOT NULL,
    [Code]                  NVARCHAR (510)  NOT NULL,
    [Name]                  NVARCHAR (510)  NOT NULL,
    [FullName]              NVARCHAR (1000) DEFAULT ((''))NOT NULL,
    [AvailableRegistration] BIT             DEFAULT ((1)) NOT NULL
);

ALTER TABLE [dbo].[tmp_ms_xx_System]
    ADD CONSTRAINT [tmp_ms_xx_clusteredindex_PK_System] PRIMARY KEY CLUSTERED ([SystemID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

IF EXISTS (SELECT TOP 1 1
           FROM   [dbo].[System])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_System] ON;
        INSERT INTO [dbo].[tmp_ms_xx_System] ([SystemID], [Code], [Name])
        SELECT   [SystemID],
                 [Code],
                 [Name]
        FROM     [dbo].[System]
        ORDER BY [SystemID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_System] OFF;
    END

DROP TABLE [dbo].[System];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_System]', N'System';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_clusteredindex_PK_System]', N'PK_System', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[OrganizationOperatingStatus]...';


GO
CREATE TABLE [dbo].[OrganizationOperatingStatus] (
    [Id]   INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name] NVARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);


GO
PRINT N'Creating [dbo].[OrganizationUpdateHistory]...';


GO
CREATE TABLE [dbo].[OrganizationUpdateHistory] (
    [Id]                       INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [OriginalOrgId]            INT             NULL,
    [UpdateDescription]        NVARCHAR (MAX)  NULL,
    [Version]                  INT             NULL,
    [UpdateDate]               DATETIME        NULL,
    [CreateDate]               DATETIME        NOT NULL,
    [FullName]                 NVARCHAR (1000) NULL,
    [ShortName]                NVARCHAR (500)  NULL,
    [RegionId]                 INT             NULL,
    [TypeId]                   INT             NULL,
    [KindId]                   INT             NULL,
    [INN]                      NVARCHAR (10)   NULL,
    [OGRN]                     NVARCHAR (13)   NULL,
    [OwnerDepartment]          NVARCHAR (500)  NULL,
    [IsPrivate]                BIT             NOT NULL,
    [IsFilial]                 BIT             NOT NULL,
    [DirectorPosition]         NVARCHAR (255)  NULL,
    [DirectorFullName]         NVARCHAR (255)  NULL,
    [IsAccredited]             BIT             NULL,
    [AccreditationSertificate] NVARCHAR (255)  NULL,
    [LawAddress]               NVARCHAR (255)  NULL,
    [FactAddress]              NVARCHAR (255)  NULL,
    [PhoneCityCode]            NVARCHAR (10)   NULL,
    [Phone]                    NVARCHAR (100)  NULL,
    [Fax]                      NVARCHAR (100)  NULL,
    [EMail]                    NVARCHAR (100)  NULL,
    [Site]                     NVARCHAR (40)   NULL,
    [RCModel]                  INT             NULL,
    [RCDescription]            NVARCHAR (400)  NULL,
    [MainId]                   INT             NULL,
    [DepartmentId]             INT             NULL,
    [CNFBFullTime]             INT             NULL,
    [CNFBEvening]              INT             NULL,
    [CNFBPostal]               INT             NULL,
    [CNPayFullTime]            INT             NULL,
    [CNPayEvening]             INT             NULL,
    [CNPayPostal]              INT             NULL,
    [CNFederalBudget]          INT             NULL,
    [CNTargeted]               INT             NULL,
    [CNLocalBudget]            INT             NULL,
    [CNPaying]                 INT             NULL,
    [CNFullTime]               INT             NULL,
    [CNEvening]                INT             NULL,
    [CNPostal]                 INT             NULL,
    [NewOrgId]                 INT             NULL,
    [StatusId]                 INT             NULL,
    [EditorUserName]           NVARCHAR (50)   NULL,
    [DateChangeStatus]         DATETIME        NULL,
    [Reason]                   NVARCHAR (100)  NULL
);


GO
PRINT N'Creating Full-text Index...';


GO
CREATE FULLTEXT INDEX ON [dbo].[Organization2010]
    ([FullName] LANGUAGE 1033, [ShortName] LANGUAGE 1033, [OwnerDepartment] LANGUAGE 1033)
    KEY INDEX [PK__Organization2010__24F84F52]
    ON [Organization2010];


GO
PRINT N'Creating FK_Group_System...';


GO
ALTER TABLE [dbo].[Group] WITH NOCHECK
    ADD CONSTRAINT [FK_Group_System] FOREIGN KEY ([SystemID]) REFERENCES [dbo].[System] ([SystemID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_GroupAccount_Group...';


GO
ALTER TABLE [dbo].[GroupAccount] WITH NOCHECK
    ADD CONSTRAINT [FK_GroupAccount_Group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_OrganizationRequestAccount_Group...';


GO
ALTER TABLE [dbo].[OrganizationRequestAccount] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationRequestAccount_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_GroupAccount_Account...';


GO
ALTER TABLE [dbo].[GroupAccount] WITH NOCHECK
    ADD CONSTRAINT [FK_GroupAccount_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_OrganizationRequestAccount_OrganizationRequest2010...';


GO
ALTER TABLE [dbo].[OrganizationRequestAccount] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationRequestAccount_OrganizationRequest2010] FOREIGN KEY ([OrgRequestID]) REFERENCES [dbo].[OrganizationRequest2010] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;


GO
PRINT N'Creating FK_OrganizationUpdateHistory_RecruitmentCampaigns...';


GO
ALTER TABLE [dbo].[OrganizationUpdateHistory] WITH NOCHECK
    ADD CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns] FOREIGN KEY ([RCModel]) REFERENCES [dbo].[RecruitmentCampaigns] ([Id]) ON DELETE NO ACTION ON UPDATE CASCADE;


GO
PRINT N'Altering [dbo].[DeleteAccount]...';


GO
-- =============================================
-- Author:		Сулиманов А.М.
-- Create date: 2009-05-07
-- Description:	Удаление из БД всего, что касается AccountId (не анализируются связи)
-- =============================================
ALTER PROCEDURE [dbo].[DeleteAccount]
(@AccountID int)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM dbo.AccountIp WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountKey WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountLog WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountRoleActivity WHERE AccountId=@AccountID
	DELETE FROM dbo.GroupAccount WHERE AccountId=@AccountID
--	DELETE FROM dbo.Organization WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Id=@AccountID)
	DELETE FROM dbo.UserAccountPassword WHERE AccountId=@AccountID
	DELETE FROM dbo.Account WHERE Id=@AccountID
END
GO
PRINT N'Altering [dbo].[GetUserAccount]...';


GO

-- exec dbo.GetUserAccount

-- =============================================
-- Получение информации о пользователе.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.1.2: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.3: Modified by Fomin Dmitriy 07.07.2008
-- Добавлены поля EducationInstitutionTypeId, 
-- EducationInstitutionTypeName.
-- =============================================
ALTER PROCEDURE [dbo].[GetUserAccount] @login NVARCHAR(255)
AS 
    BEGIN
        DECLARE @currentYear INT ,
            @accountId BIGINT--, @userGroupId int

        SET @currentYear = YEAR(GETDATE())

--	select @userGroupId = [group].Id
--	from dbo.[Group] [group] with (nolock, fastfirstrow)
--	where [group].Code = 'User'

        SELECT  @accountId = account.[Id]
        FROM    dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
        WHERE   account.[Login] = @login

        SELECT  account.[Login] ,
                account.LastName ,
                account.FirstName ,
                account.PatronymicName ,
                region.[Id] OrganizationRegionId ,
                region.[Name] OrganizationRegionName ,
                OReq.Id OrganizationId ,
                OReq.FullName OrganizationName ,
                OReq.OwnerDepartment OrganizationFounderName ,
                OReq.LawAddress OrganizationAddress ,
                OReq.DirectorFullName OrganizationChiefName ,
                OReq.Fax OrganizationFax ,
                OReq.Phone OrganizationPhone ,
                OReq.EMail OrganizationEmail ,
                OReq.Site OrganizationSite ,
                OReq.ShortName OrganizationShortName ,
                OReq.FactAddress OrganizationFactAddress ,
                OReq.DirectorPosition OrganizationDirectorPosition ,
                OReq.IsPrivate OrganizationIsPrivate ,
                OReq.IsFilial OrganizationIsFilial ,
                OReq.PhoneCityCode OrganizationPhoneCode ,
                OReq.AccreditationSertificate AccreditationSertificate ,
                OReq.INN OrganizationINN ,
                OReq.OGRN OrganizationOGRN ,
                account.Phone ,
                account.Position ,
                account.Email ,
                account.IpAddresses IpAddresses ,
                account.Status ,
                CASE WHEN account.CanViewUserAccountRegistrationDocument = 1
                     THEN account.RegistrationDocument
                     ELSE NULL
                END RegistrationDocument ,
                CASE WHEN account.CanViewUserAccountRegistrationDocument = 1
                     THEN account.RegistrationDocumentContentType
                     ELSE NULL
                END RegistrationDocumentContentType ,
                account.AdminComment AdminComment ,
                dbo.CanEditUserAccount(account.Status, account.ConfirmYear,
                                       @currentYear) CanEdit ,
                dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument ,
                account.HasFixedIp HasFixedIp ,
                account.HasCrocEgeIntegration HasCrocEgeIntegration ,
                OrgType.Id OrgTypeId ,
                OrgType.[Name] OrgTypeName ,
                OrgKind.Id OrgKindId ,
                OrgKind.[Name] OrgKindName ,
                OReq.OrganizationId OReqId ,
                RCModel.ModelName ,
                OReq.RCDescription
        FROM    ( SELECT    account.[Login] [Login] ,
                            account.LastName LastName ,
                            account.FirstName FirstName ,
                            account.PatronymicName PatronymicName ,
                            account.OrganizationId OrganizationId ,
                            account.Phone Phone ,
                            account.Position Position ,
                            account.Email Email ,
                            account.ConfirmYear ConfirmYear ,
                            account.RegistrationDocument RegistrationDocument ,
                            account.RegistrationDocumentContentType RegistrationDocumentContentType ,
                            account.AdminComment AdminComment ,
                            account.IpAddresses IpAddresses ,
                            account.HasFixedIp HasFixedIp ,
                            account.HasCrocEgeIntegration HasCrocEgeIntegration ,
                            dbo.GetUserStatus(account.ConfirmYear,
                                              account.Status, @currentYear,
                                              account.RegistrationDocument) Status ,
                            dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument
                  FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                  WHERE     account.[Id] = @accountId
--					and account.Id in (
--						select group_account.AccountId
--						from dbo.GroupAccount group_account
--						where group_account.GroupId = @userGroupId)
                  
                ) account 
                LEFT OUTER JOIN dbo.OrganizationRequest2010 OReq WITH ( NOLOCK,
                                                              FASTFIRSTROW )
                LEFT OUTER JOIN dbo.Region region WITH ( NOLOCK, FASTFIRSTROW ) ON region.[Id] = OReq.RegionId
                LEFT OUTER JOIN dbo.OrganizationType2010 OrgType ON OReq.TypeId = OrgType.Id
                LEFT OUTER JOIN dbo.OrganizationKind OrgKind ON OReq.KindId = OrgKind.Id ON OReq.[Id] = account.OrganizationId
                LEFT OUTER JOIN dbo.RecruitmentCampaigns RCModel ON OReq.RCModelID = RCModel.Id
        RETURN 0
    END
GO
PRINT N'Altering [dbo].[UpdateUserAccount]...';


GO


-- =============================================
-- Modified 29.01.2011
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
@position NVARCHAR (255)=null,
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
@organizationRcModelId INT=null,
@orgRCDescription NVARCHAR(400)=NULL,
@ExistingOrgId INT=NULL,
@orgRequestID INT=null OUTPUT,
@accessToFbs BIT = 0,
@accessToFbd BIT = 0
AS
BEGIN	
	-- при добавлении пользователя - проверка есть ли уже такой?
	if exists (SELECT * FROM Account a WHERE a.Email = @email AND @login = '')
	BEGIN
		RAISERROR('$Пользователь с указанным логином уже существует.', 18, 1)
		RETURN -1
	END
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit		
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)		
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam BIT
		, @userStatusBefore NVARCHAR(510)
		, @isRegistrationDocumentExistsForUser BIT

	set @updateId = newid()
	
	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id], 
	  @isRegistrationDocumentExistsForUser = case when account.RegistrationDocument IS NULL THEN 0 ELSE 1 END
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
		
		-- в качестве логина пользователя используем email
		SET @login = @email

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @userStatusBefore = account.[Status]
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
			-- берем последнюю поданную заявку			
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
		
	-- определяем идентификатор статуса
	DECLARE @statusID INT
	SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @status

	begin tran insert_update_account_tran
	
		IF(@orgRequestID IS NULL)
		BEGIN
			-- заявка подается не зависимо от того, новый аккаунт создается или обновляется старый
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
				OrganizationId,
				StatusID,
				RCModelID,
				RCDescription
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
				@ExistingOrgId,
				@statusID,
				@organizationRcModelId,
				@orgRCDescription
				 
			if (@@error <> 0)
				goto undo

			select @orgRequestID = scope_identity()
			if (@@error <> 0)
				goto undo
		END
	
		if @newAccount = 1 -- внесение нового пользователя
		begin
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
				, Position
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
				, @orgRequestID
				, 1
				, @currentYear
				, @phone
				, @position
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
		end	
		else 
		begin -- update существующего пользователя			
			
			-- если пользователь получает доступ к ФБД и привязывается к организации, у которой уже есть активированный УС ОУ для ФБД, то выводим ошибку
			IF(@userStatusBefore = 'activated' AND @accessToFbd = 1 AND EXISTS(
				SELECT * FROM OrganizationRequest2010 or1 JOIN Account a ON or1.Id = a.OrganizationId
					JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id AND ora.AccountID = a.Id
					JOIN [GROUP] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @ExistingOrgId AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly') AND 
			      g.Code = 'fbd_^authorizedstaff'))
			BEGIN
				RAISERROR('$Сохранение не выполнено. У указанной организации уже есть активированный УС ОУ для ФБД.', 18, 1)
				goto undo
			END			
			
			if @isOrganizationOwner = 1
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
				-- GVUZ-779 При изменении статуса пользователя статус заявления не меняется.
				--StatusID = @StatusID
			from 
				dbo.OrganizationRequest2010 OReq with (rowlock)
			where
				OReq.[Id] = @orgRequestID

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
				, Position = @position
				, Email = @email
				, ConfirmYear = @currentYear
				-- GVUZ-761 Статус УС ОУ, который имеет доступ только чтение, после оставления заявки на активацию - меняем на "Registration" 
				, [Status] = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
				, OrganizationId = @orgRequestID
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
			END
		end	
		
		-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
		DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId		
		DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId IN (15, 3, 6, 7, 8, 9, 10, 11)
				
		-- установка группы пользователя.
		IF(@accessToFbd = 1)
		BEGIN
			-- fbd_^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	15, @accountId
			if (@@error <> 0) goto undo
			
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 15)
			if (@@error <> 0) goto undo
													
			-- esrp^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	3, @accountId
			if (@@error <> 0) goto undo
				
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 3)
			if (@@error <> 0) goto undo
											
		END	
		IF(@accessToFbs = 1)
		BEGIN
			-- ВУЗ
			IF(@organizationTypeId = 1)
			BEGIN
				-- fbs_^vuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	6, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 6)
				if (@@error <> 0) goto undo
			END
			-- ССУЗ
			ELSE IF(@organizationTypeId = 2)
			BEGIN
				-- fbs_^ssuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	7, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 7)
				if (@@error <> 0) goto undo						
			END
			-- РЦОИ
			ELSE IF(@organizationTypeId = 3)
			BEGIN
				-- fbs_^infoprocessing
				insert dbo.GroupAccount(GroupId, AccountID)
				select	8, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 8)
				if (@@error <> 0) goto undo						
			END
			-- Орган управления образованием
			ELSE IF(@organizationTypeId = 4)
			BEGIN
				-- fbs_^direction
				insert dbo.GroupAccount(GroupId, AccountID)
				select	9, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 9)
				if (@@error <> 0) goto undo						
			END
			-- Другое
			ELSE IF(@organizationTypeId = 5)
			BEGIN
				-- fbs_^other
				insert dbo.GroupAccount(GroupId, AccountID)				
				select	11, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 11)
				if (@@error <> 0) goto undo						
			END
			-- Учредитель
			ELSE IF(@organizationTypeId = 6)
			BEGIN
				-- fbs_^founder
				insert dbo.GroupAccount(GroupId, AccountID)
				select	10, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 10)
				if (@@error <> 0) goto undo						
			END				
		END	
								
	-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		BEGIN
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
	END
	
	-- GVUZ-761 оставляем в заявлении признак, если проводится активация пользователя
	-- флаг используется при выдаче шаблона документа для скана заявки	
	UPDATE OrganizationRequest2010
	SET IsForActivation = 1
	WHERE @accessToFbd = 1 AND @userStatusBefore = 'readonly' AND Id = @orgRequestID
	
	-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
	-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
	IF(@isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL)	
	BEGIN		
		IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
		BEGIN
			UPDATE OrganizationRequest2010 SET StatusID = 2 WHERE Id = @orgRequestID
		END
	END

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
PRINT N'Altering [dbo].[UpdateUserAccountStatus]...';


GO

-- exec dbo.UpdateUserAccountStatus

-- =============================================
-- Изменить статус пользователя.
-- v.1.0: Created by Makarev Andrey 08.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Рефакторинг: выделена функция, изменеа логика 
-- означивания полей.
-- v.1.2: Modified by Fomin Dmitriy 11.04.2008
-- Удаляется документ регистрации, если он устарел.
-- v.1.3: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.4: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.5: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- =============================================
ALTER proc [dbo].[UpdateUserAccountStatus]
	@login nvarchar(255)
	, @status nvarchar(255)
	, @adminComment ntext 
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @changeStatusByOrganizationRequest BIT = 0 -- 1 если статус пользователя меняется через заявку на регистрацию
												 -- в этом случае игнорируем часть проверок
as
BEGIN
	declare
		@isActive bit
		, @eventCode nvarchar(255)
		, @accountId bigint
		, @editorAccountId bigint
		, @currentYear int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @isValidEmail BIT
		, @userEmail NVARCHAR(510)
		, @orgRequestID int
			

	set @updateId = newid()
	set @eventCode = N'USR_STATE'
	set @currentYear = Year(GetDate())
	
	select
		@editorAccountId = account.[Id],
		@userEmail = account.Email
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	SELECT
		@accountId = account.[Id],
		@orgRequestID = account.OrganizationId		
		/*
		* старый функционал. в процедуру передается статус, который уже необходимо установить пользьвателю, дополнительных проверок не требуется.
		* , @status = case when @changeStatusByOrganizationRequest = 1 then @status else dbo.GetUserStatus(@currentYear, @status, @currentYear, 
				account.RegistrationDocument) END*/
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login	

	if(@changeStatusByOrganizationRequest = 0)
	BEGIN	
		-- если деактивируют проверить на допустимые статусы
		IF (@status = 'deactivated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('activated', 'readonly')))
		BEGIN
			RAISERROR('$Невозможно деактивировать пользователя в текущем статусе.', 18, 1)
			RETURN -1
		END	
		
		IF (@status = 'activated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('deactivated', 'readonly')))
		BEGIN	
			RAISERROR('$Невозможно активировать пользователя в текущем статусе.', 18, 1)
			RETURN -1
		END		
	END
	
	-- проверка на сущестование пользователя с указанным email
	EXEC @isValidEmail = CheckNewUserAccountEmail @email = @userEmail
	IF(@isValidEmail = 0)
	BEGIN
		RAISERROR('$Существуют пользователи с таким же e-mail.', 18, 1)
		RETURN -1
	END	
	
	-- при установке статуса "Активировать" проверка на наличие скана заявки
	IF(@status = 'activated' AND EXISTS(SELECT * FROM Account WHERE [Login] = @login AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$Пользователь не приложил скан заявки.', 18, 1)
		RETURN -1
	END	
	
	-- GVUZ-595. При работе с заблокированной учетной записью уполномоченного сотрудника (статус «Deactivated») исключить возможность ее активации, 
    -- если для этого же ОУ есть учетная запись уполномоченного сотрудника со статусом, отличным от значения «Deactivated». (редактирование)		            
    -- Правило работает только для УС ОУ ФБД GVUZ-780.
    IF (@status = 'activated' AND 
		-- заблокированный пользователь является УС ОУ ФБД для организации
		EXISTS(
			SELECT * 
			FROM Account a JOIN GroupAccount ga ON ga.AccountId = a.Id
				JOIN [Group] g ON g.Id = ga.GroupID
			WHERE a.[Login] = @login AND a.[Status] = 'deactivated' AND g.Code = 'fbd_^authorizedstaff'
		) AND
		-- есть незаблокированный УС ОУ ФБД для организации данного пользователя
		EXISTS(
    	SELECT or1.OrganizationId, orgUser.[Login]
		FROM OrganizationRequest2010 or1 
			JOIN OrganizationRequest2010 orReqUsr ON orReqUsr.OrganizationId = or1.OrganizationId
			JOIN account orgUser ON orgUser.OrganizationId = orReqUsr.Id
			JOIN GroupAccount ga ON ga.AccountId = orgUser.Id
			JOIN [Group] g ON ga.GroupID = g.Id
    	WHERE or1.Id = @orgRequestID AND orgUser.[Status] <> 'deactivated' AND g.Code = 'fbd_^authorizedstaff' AND orReqUsr.Id <> @orgRequestID))
    BEGIN
		RAISERROR('$Невозможно активировать заблокированного уполномоченного сотрудника для доступа к ФБД, т.к. для данного ОУ уже есть учетная запись незаблокированного уполномоченного сотрудника.', 18, 1)
		RETURN -1    	
    END
    
    -- GVUZ-624. Исключается возможность активации учетной записи пользователя, если заявление, с которым она связана, не активировано.
    IF (@status = 'activated' AND NOT exists(
    		SELECT *
			FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.Id
			WHERE a.Id = @accountId	AND or1.StatusID = 5 -- активировано
			))
    BEGIN
		RAISERROR('$Невозможно активировать пользователя, т.к. заявление для данного пользователя не активировано.', 18, 1)
		RETURN -1
    END

	BEGIN TRAN
	
		-- GVUZ-761 при активации пользователя УС ОУ в ФБД - старого блокируем, нового активируем.
		IF(@status = 'activated')
		BEGIN
			DECLARE @existOrgUserLogin nvarchar(255), @organizationID INT
			SELECT @organizationID = or1.OrganizationId 
			FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.id
			WHERE a.[Login] = @login
			
			-- активируемый пользователь должен входить в группу УС ОУ для ФБД
			IF EXISTS(
				SELECT *
				FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
					JOIN Account a ON a.Id = ora.AccountID
					JOIN [Group] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @organizationID AND a.Id = @accountId AND g.code IN ('fbd_^authorizedstaff'))
			BEGIN
				-- находим существующего активного УС ОУ для ФБД.
				SELECT @existOrgUserLogin = a.[login]
				FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
					JOIN Account a ON a.Id = ora.AccountID
					JOIN [Group] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @organizationID AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly')
					AND g.code IN ('fbd_^authorizedstaff')
				
				-- если нашли активного УС ОУ для ФБД, то блокируем его.
				IF(@existOrgUserLogin IS NOT NULL)
				BEGIN
					exec UpdateUserAccountStatus @login = @existOrgUserLogin, @status = 'deactivated', 
					@adminComment = 'заблокирован по причине активации нового УС ОУ для ФБД', 
					@editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1			
					if (@@error <> 0)
						goto undo
				END				
			END
		END			
	
		update account
		set 
			-- GVUZ-810 При отправке на доработку из статусов Registration и Revision статус не меняется.
			Status = CASE WHEN @status = 'revision' AND [Status] IN ('registration', 'revision') THEN [Status] ELSE @status END
			, AdminComment = @adminComment
			, IsActive = dbo.GetUserIsActive(@status)
			, UpdateDate = GetDate()
			, UpdateId = @updateId
			, ConfirmYear = @currentYear
			-- Удаляем документ регистрации, если он устарел.
			, RegistrationDocument = case
				when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 then null
				else account.RegistrationDocument
			end
			, EditorAccountId = @editorAccountId
			, EditorIp = @editorIp
		from 
			dbo.Account account with (rowlock)
		where
			account.[Id] = @accountId
		if (@@error <> 0)
			goto undo
								
	if @@trancount > 0 COMMIT TRAN

	exec dbo.RefreshRoleActivity @accountId = @accountId

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0
	
	undo:
		rollback tran
		return 1	
end
GO
PRINT N'Creating [dbo].[GetSystemNameByLogin]...';


GO
CREATE proc [dbo].[GetSystemNameByLogin]
@login nvarchar(255) = null
AS
BEGIN
  SELECT [system].[Name]
  FROM [dbo].[Account] [account]
  JOIN [dbo].[GroupAccount] [groupAccount] ON groupAccount.[AccountId] = account.[Id]
  JOIN [dbo].[Group][group] ON [group].[Id] = [groupAccount].[GroupId] 
  JOIN [dbo].[System] [system] ON  [system].[SystemID] = [group].[SystemID]
  WHERE [account].[Login]=@login
END
GO
PRINT N'Creating [dbo].[SelectInformationSystems]...';


GO
CREATE proc [dbo].[SelectInformationSystems]
AS
BEGIN
SELECT s.Name as ShortName, s.SystemID as SystemID, COUNT(g.SystemID) as NumberGroups, s.FullName, s.AvailableRegistration
	FROM [dbo].[System] s
	LEFT JOIN [dbo].[Group] g ON s.SystemID=g.SystemID
	GROUP BY s.Name, s.SystemID, s.FullName, s.AvailableRegistration
END
GO
PRINT N'Altering [dbo].[UpdateOrganizationRequestStatus]...';


GO

ALTER PROCEDURE [dbo].[UpdateOrganizationRequestStatus]	
	  @orgRequestID INT
	, @statusID INT
	, @needConsiderLinkedUsers BIT = 1 -- если 1, то меняем статусы прикрепленных пользователей к заявке
	, @comment VARCHAR(MAX)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
AS
BEGIN	
	DECLARE
		@curRequestStatus INT,
		@statusCode NVARCHAR(1020),
		@editorID INT
	
	SELECT @editorID = id FROM Account WHERE [login] = @editorLogin 
	SELECT @statusCode = Code FROM AccountStatus WHERE StatusID = @statusID		
	SELECT @curRequestStatus = or1.StatusID
	FROM OrganizationRequest2010 or1
	WHERE or1.Id = @statusID	
		
	-- если активируют проверить на допустимые статусы
	IF (@statusID = 5 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (2,3,6)))
	BEGIN
		RAISERROR('$Невозможно активировать заявку в текущем статусе.', 18, 1)
		RETURN -1
	END		
			
	-- GVUZ-595 Исключить подтверждение заявки без приложенных документов.
	IF (@statusID = 5 AND EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$Невозможно активировать заявку, т.к. не приложены сканы документов для регистрируемых пользователей.', 18, 1)
		RETURN -1
	END			

	-- если деактивируют проверить на допустимые статусы
	IF (@statusID = 6 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5)))
	BEGIN
		RAISERROR('$Невозможно деактивировать заявку в текущем статусе.', 18, 1)
		RETURN -1
	END		

	-- если отправляют на доработку проверить на допустимые статусы
	IF (@statusID = 3 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5,6)))
	BEGIN
		RAISERROR('$Невозможно отправить на доработку заявку в текущем статусе.', 18, 1)
		RETURN -1
	END
	
	BEGIN TRAN		
		-- смена статуса заявления
		-- GVUZ-810 При отправке на доработку ('revision') из статусов Registration и Revision статус не меняется.
		UPDATE OrganizationRequest2010 
		SET StatusID = CASE WHEN @statusCode = 'revision' AND StatusID IN (1, 3) THEN StatusID ELSE @statusID END		
		WHERE id = @orgRequestID
		
		DECLARE 
			@suggestedRCModelID INT,
			@suggestedRCDescription NVARCHAR(400),
			@orgID INT
		
		IF (@statusID = 5)
		BEGIN
			SELECT @orgID = OrganizationId, @suggestedRCModelID = RCModelID, @suggestedRCDescription = RCDescription			
			FROM dbo.OrganizationRequest2010
			WHERE Id = @orgRequestID
			
			UPDATE dbo.Organization2010
			SET
				RCModel = CASE WHEN @suggestedRCModelID = NULL THEN 999 ELSE @suggestedRCModelID END,
				RCDescription = @suggestedRCDescription
			WHERE Id = @orgID
		END
		
		if (@@error <> 0) goto undo
				
		-- если учитываем пользователей в заявке, то идем по всем пользователям заявления и меняем им статус на новый статус для заявления
		if(@needConsiderLinkedUsers = 1)
		BEGIN
			DECLARE @curUserLogin NVARCHAR(510)			
			DECLARE linkedUsers CURSOR FOR
			  SELECT a.[login] FROM Account a WHERE a.OrganizationId = @orgRequestID
			
			OPEN linkedUsers
			FETCH NEXT FROM linkedUsers INTO @curUserLogin
			
			WHILE(@@FETCH_STATUS = 0)
			BEGIN				
				EXEC [UpdateUserAccountStatus] @login = @curUserLogin, @status = @statusCode, @adminComment = @comment,
				  @editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1
				if (@@error <> 0) goto undo
				FETCH NEXT FROM linkedUsers INTO @curUserLogin
			END			
			
			CLOSE linkedUsers
			DEALLOCATE linkedUsers
		END
		
	if @@trancount > 0 commit tran

	DECLARE @requestIds nvarchar(1024), @eventCode nvarchar(255), @updateId	UNIQUEIDENTIFIER
	set @updateId = newid()
	set @eventCode = N'REQ_STATUS'	
	set @requestIds = convert(nvarchar(1024), @orgRequestID)	
	exec dbo.RegisterEvent 
		  @accountId = @editorId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @requestIds
		, @eventParams = null
		, @updateId = @updateId
	
	RETURN 0
		
	undo:
		CLOSE linkedUsers
		DEALLOCATE linkedUsers	
		if @@trancount > 0 rollback tran update_organizationrequest_tran
		return 1
END
GO
PRINT N'Altering [dbo].[ReportOrgActivation_OTHER]...';


GO
ALTER function [dbo].[ReportOrgActivation_OTHER]()
RETURNS @OTHER TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @RCOI INT
DECLARE @OUO INT
DECLARE @OtherOrg INT
SELECT @RCOI = COUNT(*) FROM Organization2010 WHERE TypeId=3 
SELECT @OUO = COUNT(*) FROM Organization2010 WHERE TypeId=4 
SELECT @OtherOrg = COUNT(*) FROM Organization2010 WHERE TypeId=5

INSERT INTO @OTHER
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'РЦОИ','',@RCOI,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=3

UNION ALL
SELECT
'Орган управления образованием','',@OUO,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=4 
UNION ALL
SELECT
'Другое','',@OtherOrg,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId<>1 AND OrgReq.TypeId<>2 AND OrgReq.TypeId<>3 AND OrgReq.TypeId<>4 AND OrgReq.TypeId<>5

return
END
GO
PRINT N'Altering [dbo].[ReportOrgActivation_SSUZ]...';


GO
ALTER function [dbo].[ReportOrgActivation_SSUZ]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @SSUZState INT
SELECT @SSUZState = COUNT(*) FROM Organization2010 WHERE TypeId=2 AND  IsPrivate=0
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010 WHERE TypeId=2 AND  IsPrivate=1
INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ССУЗ','Государственный',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=0

UNION ALL
SELECT
'ССУЗ','Негосударственный',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ССУЗ','Всего',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2

return
END
GO
PRINT N'Altering [dbo].[ReportOrgActivation_SSUZ_Accred]...';


GO
ALTER function [dbo].[ReportOrgActivation_SSUZ_Accred]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @SSUZState INT
SELECT @SSUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=2 AND  Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010  Org WHERE Org.TypeId=2 AND  Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))
INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ССУЗ','Государственный',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT
'ССУЗ','Негосударственный',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))
UNION ALL
SELECT
'ССУЗ','Всего',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

return
END
GO
PRINT N'Altering [dbo].[ReportOrgActivation_VUZ]...';


GO
ALTER function [dbo].[ReportOrgActivation_VUZ]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @VUZStateMain INT
DECLARE @VUZStateFilial INT
DECLARE @VUZPrivMain INT
DECLARE @VUZPrivFilial INT

SELECT @VUZStateMain = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=0 AND IsPrivate=0
SELECT @VUZStateFilial = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=1 AND IsPrivate=0
SELECT @VUZPrivMain = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=0 AND IsPrivate=1
SELECT @VUZPrivFilial = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=1 AND IsPrivate=1

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZStateFilial+@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
'ВУЗ основной','Государственный',@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
'ВУЗ филиал','Государственный',@VUZStateFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPrivFilial+@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ основной','Негосударственный',@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ филиал','Негосударственный',@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ','Всего',@VUZStateMain+@VUZStateFilial+@VUZPrivMain+@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1

return
END
GO
PRINT N'Altering [dbo].[ReportOrgActivation_VUZ_Accred]...';


GO
ALTER function [dbo].[ReportOrgActivation_VUZ_Accred]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @VUZState INT
DECLARE @VUZPriv INT

SELECT @VUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

SELECT @VUZPriv = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT
'ВУЗ','Всего',@VUZState+@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

return
END
GO
PRINT N'Altering [dbo].[ReportUserStatusAccredTVF_New]...';


GO
ALTER function [dbo].[ReportUserStatusAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ_Accred()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ_Accred()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего'

INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON 
	(
		Org.Id=OReq.OrganizationId 
		AND (
			Org.IsAccredited=1 
			OR (
				Org.AccreditationSertificate != '' 
				AND Org.AccreditationSertificate IS NOT NULL
				)
			)
	)
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  

return
END
GO
PRINT N'Altering [dbo].[ReportEditedOrgsTVF]...';


GO
ALTER function [dbo].[ReportEditedOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Импортирована из справочника] nvarchar(13) null
)
AS 
BEGIN


INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN 'Есть'
	ELSE 'Нет'
	END AS [Аккредитация по факту] 	
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,CASE WHEN (Org.WasImportedAtStart=1)
	THEN 'Да'
	ELSE 'Нет'
	END AS [Импортирована из справочника]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
WHERE (Org.CreateDate != Org.UpdateDate AND Org.WasImportedAtStart =1) OR (Org.WasImportedAtStart=0)
ORDER BY Org.WasImportedAtStart


RETURN
END
GO
PRINT N'Altering [dbo].[ReportOrgsBASE]...';


GO
ALTER function [dbo].[ReportOrgsBASE]()
	
	
RETURNS @report TABLE 
(
[Id] INT 
,[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
,[Дата создания] datetime null
,[Создана из справочника] bit null
,[Имя ФО] nvarchar(255) null
,[Код ФО] int null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN
 
INSERT INTO @Report
SELECT 
Org.Id as [Id]
,Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,Org.WasImportedAtStart AS [Создана из справочника]
,FD.[Name] AS [Имя ФО]
,FD.Code AS [Код ФО]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN 'Есть'
	ELSE 'Нет'
	END AS [Аккредитация по факту] 	
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]



FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN FederalDistricts FD
ON FD.Id=Reg.FederalDistrictId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId

RETURN
END
GO
PRINT N'Altering [dbo].[ReportStatisticSubordinateOrg]...';


GO

ALTER FUNCTION  [dbo].[ReportStatisticSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	Id int null,
	FullName nvarchar(Max) null,
	RegionId int null,
	RegionName nvarchar(255) null,
	AccreditationSertificate nvarchar(255) null,
	DirectorFullName nvarchar(255) null,
	CountUser int null,
	UserUpdateDate datetime null,
	CountUniqueChecks int null
)
AS BEGIN

SET @periodBegin = DATEADD(YEAR, -1, GETDATE())
SET @periodEnd = GETDATE()

--Проверки по номеру
DECLARE @NumberChecksByOrg TABLE
(
	OrganizationId INT,
	UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM 
	CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
	O.DepartmentId = @departmentId
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY 
	IOrgReq.OrganizationId

--Проверки по типографскому номеру
DECLARE @TNChecksByOrg TABLE
(
	OrganizationId INT,
	UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM 
	CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE 
	O.DepartmentId = @departmentId
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND cb.IsTypographicNumber = 1
GROUP BY 
	IOrgReq.OrganizationId


--Провекри по паспорту
DECLARE @PassportChecksByOrg TABLE
(
	OrganizationId INT,
	UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM 
	CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
	O.DepartmentId = @departmentId
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND cb.IsTypographicNumber = 0
GROUP BY
	IOrgReq.OrganizationId

--Проверки интерактивные
DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT 
	IOrgReq.OrganizationId,
	COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
		ISNULL(ChLog.PassportSeria,'')+
		ISNULL(ChLog.PassportNumber,'')+
		ISNULL(ChLog.CNENumber,'')+
		ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM
	CNEWebUICheckLog ChLog
	INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id = Acc.OrganizationId
	INNER JOIN Organization2010 O ON O.Id = IOrgReq.OrganizationId
WHERE
	O.DepartmentId = @departmentId
	AND ChLog.EventDate BETWEEN @periodBegin and @periodEnd
GROUP BY
	IOrgReq.OrganizationId

INSERT INTO @Report
SELECT
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name as RegionName,
	O.AccreditationSertificate,
	O.DirectorFullName,
	COUNT(A.Id) CountUser,
	MIN(A.UpdateDate) UserUpdateDate,
	isnull(sum(NCByOrg.UniqueNumberChecks) + 
		sum(TNByOrg.UniqueTNChecks) +
		sum(PByOrg.UniquePassportChecks) +
		sum(UIByOrg.UniqueUIChecks), 0) as CountUniqueChecks
from
	Organization2010 O
	INNER JOIN Region R on R.Id = O.RegionId
	LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
	LEFT JOIN Account A on A.OrganizationId = OrR.Id
	LEFT JOIN @NumberChecksByOrg NCByOrg ON NCByOrg.OrganizationId = O.Id
	LEFT JOIN @TNChecksByOrg TNByOrg ON TNByOrg.OrganizationId = O.Id
	LEFT JOIN @PassportChecksByOrg PByOrg ON PByOrg.OrganizationId = O.Id
	LEFT JOIN @UIChecksByOrg UIByOrg ON UIByOrg.OrganizationId = O.Id
where
	O.DepartmentId = @departmentId
group by
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name,
	O.AccreditationSertificate,
	O.DirectorFullName

RETURN
END
GO
PRINT N'Altering [dbo].[ReportUserStatusAccredTVF]...';


GO
ALTER function [dbo].[ReportUserStatusAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @Statuses TABLE
(
	[Name] NVARCHAR (50),
	Code NVARCHAR (50),
	[Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
	[Name] NVARCHAR (50),
	Code BIT,
	[Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode INT,
	IsPrivateName NVARCHAR (50),
	IsPrivateCode INT,
	IsPrivateOrder INT,
	StatusName NVARCHAR(50),
	StatusCode NVARCHAR(50),
	StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
WHERE OrganizationType2010.Id<3
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	IsPrivateOrder NVARCHAR (50),
	StatusName NVARCHAR(50),
	UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
ON Acc.OrganizationId=OReq.Id
INNER JOIN dbo.Organization2010 Org 
ON (Org.Id=OReq.OrganizationId 
	AND (
		Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			)
		)
	)
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
	AND (
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	))
OR (
	(Acc.Status=Comb.StatusCode)
	AND (
		Comb.OrgTypeCode=10
	)
	AND (
		OReq.TypeId IS NOT NULL
	)
)
OR (
	Comb.StatusCode='total'
	AND ((
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	)
	OR
	((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
(	
	MainOrder INT,
	OrgTypeName NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	[Всего] INT,
	[Активирован] INT,
	[На регистрации] INT,
	[На доработке] INT,
	[На согласовании] INT,
	[Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON 
	(
		Org.Id=OReq.OrganizationId 
		AND (
			Org.IsAccredited=1 
			OR (
				Org.AccreditationSertificate != '' 
				AND Org.AccreditationSertificate IS NOT NULL
				)
			)
	)
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID in (6,7)
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult

return
END
GO
PRINT N'Altering [dbo].[ReportChecksByOrgsTVF]...';


GO
ALTER function [dbo].[ReportChecksByOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Имя региона] NVARCHAR(255) null
,[Полное наименование] NVARCHAR(4000) NULL
,[Тип] NVARCHAR(255) null
,[ОПФ] NVARCHAR(50) null
,[Количество проверок] INT null
,[Количество уникальных проверок] INT NULL
,[Работа с ФБС] NVARCHAR(20) NULL
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 

DECLARE @NumberChecksByOrg TABLE
(
	OrganizationId INT,
	TotalNumberChecks INT,
	UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
	OrganizationId INT,
	TotalTNChecks INT,
	UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
	OrganizationId INT,
	TotalPassportChecks INT,
	UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
GROUP BY IOrgReq.OrganizationId




DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	TotalUIChecks INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ChLog.FoundedCNEId) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd AND ChLog.FoundedCNEId is not NULL
GROUP BY IOrgReq.OrganizationId








INSERT INTO @Report
SELECT 
Reg.[Name] AS [Имя региона]
,Org.FullName AS [Полное наименование]
,OrgType.[Name] AS [Тип]
,REPLACE(REPLACE(Org.IsPrivate,1,'Негосударственный'),0,'Государственный') AS [ОПФ]

,ISNULL(NumberChecks.TotalNumberChecks,0)
+ISNULL(PassportChecks.TotalPassportChecks,0)
+ISNULL(TNChecks.TotalTNChecks,0)
+ISNULL(UIChecks.TotalUIChecks,0) AS [Количество проверок]  
,ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных проверок] 
,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС]


FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId


LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId

ORDER BY Reg.Id,Org.TypeId,Org.IsPrivate

RETURN
END
GO
PRINT N'Altering [dbo].[ReportOrgsInfoByRegionTVF]...';


GO


--Добавлены поля кода организации, учредителя
ALTER function [dbo].[ReportOrgsInfoByRegionTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL
	, @arg NVARCHAR(50) = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
,[Дата создания] datetime null
,[Создана из справочника] NVARCHAR(20) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] NVARCHAR(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Код учредителя] int null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
,[Количество проверок по номеру] int null
,[Количество уникальных проверок по номеру] INT NULL
,[Количество проверок по паспортным данным] INT NULL
,[Количество уникальных проверок по паспортным данным] INT NULL
,[Количество проверок по типографскому номеру] INT NULL
,[Количество уникальных проверок по типографскому номеру] INT NULL
,[Количество интерактивных проверок] INT NULL
,[Количество уникальных интерактивных проверок] INT NULL
,[Количество неправильных проверок] INT NULL
,[Первая проверка] datetime null
,[Последняя проверка] datetime null
,[Работа с ФБС] NVARCHAR(20)
,[Код ОУ] int null
,[Код головного ОУ] int null
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
	OrganizationId int,
	[Status] nvarchar(50),
	UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
	OrganizationId int,
	[activated] nvarchar(20),
	[deactivated] nvarchar(20),
	[consideration] nvarchar(20),
	[registration] nvarchar(20),
	[revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @NumberChecksByOrg TABLE
(
	OrganizationId INT,
	TotalNumberChecks INT,
	UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
	OrganizationId INT,
	TotalTNChecks INT,
	UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
	OrganizationId INT,
	TotalPassportChecks INT,
	UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId


DECLARE @WrongChecksByOrg TABLE
(
	OrganizationId INT,
	WrongChecks INT
)

INSERT INTO @WrongChecksByOrg
SELECT IWrong.OrganizationId,SUM(IWrong.WrongChecks) FROM 
(
	SELECT IOrgReq.OrganizationId AS OrganizationId,COUNT(*) AS WrongChecks
	FROM CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND c.SourceCertificateId IS NOT NULL
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,COUNT(*) AS WrongChecks
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND c.SourceCertificateId IS NOT NULL
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
) AS IWrong
GROUP BY IWrong.OrganizationId





DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	TotalUIChecks INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
ISNULL(ChLog.PassportSeria,'')+
ISNULL(ChLog.PassportNumber,'')+
ISNULL(ChLog.CNENumber,'')+
ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
AND IOrgReq.RegionId=@arg
GROUP BY IOrgReq.OrganizationId



DECLARE @CheckLimitDatesByOrg TABLE
(
	OrganizationId INT,
	FirstCheck DATETIME,
	LastCheck DATETIME
)
INSERT INTO @CheckLimitDatesByOrg
SELECT OrganizationId,MIN(FirstCheck),MAX(LastCheck)
FROM 
(
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(ChLog.EventDate) AS FirstCheck,MAX(ChLog.EventDate) AS LastCheck
	FROM CNEWebUICheckLog ChLog
	INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
	WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
	AND IOrgReq.RegionId=@arg
	GROUP BY IOrgReq.OrganizationId
) AS RawCheckLimitDates
GROUP BY OrganizationId







INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,REPLACE(REPLACE(Org.WasImportedAtStart,1,'Да'),0,'Нет') AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN 'Есть'
	ELSE 'Нет'
	END AS [Аккредитация по факту] 	
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.DepartmentId AS [Код учредителя]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,Org.PhoneCityCode AS [Код города] 
,Org.Phone AS [Телефон] 
,Org.EMail AS [EMail]  

,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]

,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]

,LimitDates.FirstCheck AS [Первая проверка]  
,LimitDates.LastCheck AS [Последняя проверка] 

,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС],
Org.Id [Код ОУ],
Org.MainId [Код головного ОУ]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId
LEFT JOIN @WrongChecksByOrg WrongChecks
ON Org.Id=WrongChecks.OrganizationId
LEFT JOIN @CheckLimitDatesByOrg LimitDates
ON Org.Id=LimitDates.OrganizationId
WHERE Org.RegionId=@arg

RETURN
END
GO
PRINT N'Altering [dbo].[ReportNotRegistredOrgsTVF]...';


GO
ALTER function [dbo].[ReportNotRegistredOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN

 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id NOT IN 
	(SELECT OReq.OrganizationId 
	FROM OrganizationRequest2010 OReq
	WHERE OReq.OrganizationId  IS NOT NULL)


RETURN
END
GO
PRINT N'Altering [dbo].[ReportRegistredOrgsTVF]...';


GO
ALTER function [dbo].[ReportRegistredOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN

 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id IN 
	(SELECT OReq.OrganizationId 
	FROM OrganizationRequest2010 OReq
	WHERE OReq.OrganizationId  IS NOT NULL)

RETURN
END
GO
PRINT N'Altering [dbo].[ReportOrgsInfoTVF_WithoutChecks]...';


GO
ALTER function [dbo].[ReportOrgsInfoTVF_WithoutChecks](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование ФО] nvarchar(255) null
,[Код ФО] nvarchar(255) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Есть пользователи] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
--,[Количество проверок по номеру] int null
--,[Количество уникальных проверок по номеру] INT NULL
--,[Количество проверок по паспортным данным] INT NULL
--,[Количество уникальных проверок по паспортным данным] INT NULL
--,[Количество проверок по типографскому номеру] INT NULL
--,[Количество уникальных проверок по типографскому номеру] INT NULL
--,[Количество интерактивных проверок] INT NULL
--,[Количество уникальных интерактивных проверок] INT NULL
--,[Количество неправильных проверок] INT NULL
--,[Первая проверка] datetime null
--,[Последняя проверка] datetime null
--,[Работа с ФБС] NVARCHAR(20)
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
	OrganizationId int,
	[Status] nvarchar(50),
	UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
	OrganizationId int,
	[activated] nvarchar(20),
	[deactivated] nvarchar(20),
	[consideration] nvarchar(20),
	[registration] nvarchar(20),
	[revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId







INSERT INTO @Report
SELECT 
Org.[Полное наименование] AS [Полное наименование]
,ISNULL(Org.[Краткое наименование],'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Org.[Имя ФО] AS [Имя ФО]
,Org.[Код ФО] AS [Код ФО]
,Org.[Имя региона] AS [Имя региона]
,Org.[Код региона] AS [Код региона]
,Org.[Тип] AS [Тип]
,Org.[Вид] AS [Вид]
,Org.[ОПФ] AS [ОПФ]
,Org.[Филиал] AS [Филиал]
,Org.[Аккредитация по справочнику] AS [Аккредитация по справочнику]
,Org.[Свидетельство об аккредитации] AS [Свидетельство об аккредитации]
,Org.[Аккредитация по факту] AS [Аккредитация по факту] 	
,Org.[ФИО руководителя] AS [ФИО руководителя]
,Org.[Должность руководителя] AS [Должность руководителя]
,Org.[Ведомственная принадлежность] AS [Ведомственная принадлежность]
,Org.[Фактический адрес] AS [Фактический адрес]
,Org.[Юридический адрес] AS [Юридический адрес]
,Org.[Код города] AS[Код города]
,Org.[Телефон] AS [Телефон]
,Org.[EMail] AS [EMail]
,Org.[ИНН] AS [ИНН]
,Org.[ОГРН] AS [ОГРН]

,CASE 
	WHEN (ISNULL(UsersCnt.UsersCount,0)=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Есть пользователи]
,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]
--
--,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
--,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
--,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
--,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
--,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
--,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
--,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
--,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
--,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]
--
--,LimitDates.FirstCheck AS [Первая проверка]  
--,LimitDates.LastCheck AS [Последняя проверка] 

--,CASE WHEN 
--ISNULL(NumberChecks.UniqueNumberChecks,0)
--+ISNULL(PassportChecks.UniquePassportChecks,0)
--+ISNULL(TNChecks.UniqueTNChecks,0)
--+ISNULL(UIChecks.UniqueUIChecks,0) 
--= 0 
--THEN 'Не работает'
--WHEN 
--ISNULL(NumberChecks.UniqueNumberChecks,0)
--+ISNULL(PassportChecks.UniquePassportChecks,0)
--+ISNULL(TNChecks.UniqueTNChecks,0)
--+ISNULL(UIChecks.UniqueUIChecks,0) 
--< 10 
--THEN 'Работа неактивна'
--ELSE 'Работает'
--END
--AS [Работа с ФБС]

FROM 
ReportOrgsBASE() Org 
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

--LEFT JOIN @NumberChecksByOrg NumberChecks
--ON Org.Id=NumberChecks.OrganizationId
--LEFT JOIN @TNChecksByOrg TNChecks
--ON Org.Id=TNChecks.OrganizationId
--LEFT JOIN @PassportChecksByOrg PassportChecks
--ON Org.Id=PassportChecks.OrganizationId
--LEFT JOIN @UIChecksByOrg UIChecks
--ON Org.Id=UIChecks.OrganizationId
--LEFT JOIN @WrongChecksByOrg WrongChecks
--ON Org.Id=WrongChecks.OrganizationId
--LEFT JOIN @CheckLimitDatesByOrg LimitDates
--ON Org.Id=LimitDates.OrganizationId

RETURN
END
GO
PRINT N'Altering [dbo].[ReportOrgsInfoTVF]...';


GO
ALTER function [dbo].[ReportOrgsInfoTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование ФО] nvarchar(255) null
,[Код ФО] nvarchar(255) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Подана заявка на регистрацию] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
,[Количество проверок по номеру] int null
,[Количество уникальных проверок по номеру] INT NULL
,[Количество проверок по паспортным данным] INT NULL
,[Количество уникальных проверок по паспортным данным] INT NULL
,[Количество проверок по типографскому номеру] INT NULL
,[Количество уникальных проверок по типографскому номеру] INT NULL
,[Количество интерактивных проверок] INT NULL
,[Количество уникальных интерактивных проверок] INT NULL
,[Количество неправильных проверок] INT NULL
,[Первая проверка] datetime null
,[Последняя проверка] datetime null
,[Работа с ФБС] NVARCHAR(20)
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
	OrganizationId int,
	[Status] nvarchar(50),
	UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
	OrganizationId int,
	[activated] nvarchar(20),
	[deactivated] nvarchar(20),
	[consideration] nvarchar(20),
	[registration] nvarchar(20),
	[revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @NumberChecksByOrg TABLE
(
	OrganizationId INT,
	TotalNumberChecks INT,
	UniqueNumberChecks INT
)

INSERT INTO @NumberChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalNumberChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueNumberChecks
FROM CommonNationalExamCertificateCheckBatch cb 
INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @TNChecksByOrg TABLE
(
	OrganizationId INT,
	TotalTNChecks INT,
	UniqueTNChecks INT
)

INSERT INTO @TNChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalTNChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniqueTNChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=1
GROUP BY IOrgReq.OrganizationId



DECLARE @PassportChecksByOrg TABLE
(
	OrganizationId INT,
	TotalPassportChecks INT,
	UniquePassportChecks INT
)

INSERT INTO @PassportChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalPassportChecks,COUNT(DISTINCT c.SourceCertificateId) AS UniquePassportChecks
FROM CommonNationalExamCertificateRequestBatch cb 
INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
WHERE IOrgReq.OrganizationId IS NOT NULL
AND cb.updatedate BETWEEN @periodBegin and @periodEnd
AND cb.IsTypographicNumber=0
GROUP BY IOrgReq.OrganizationId


DECLARE @WrongChecksByOrg TABLE
(
	OrganizationId INT,
	WrongChecks INT
)

INSERT INTO @WrongChecksByOrg
SELECT IWrong.OrganizationId,SUM(IWrong.WrongChecks) FROM 
(
	SELECT IOrgReq.OrganizationId AS OrganizationId,COUNT(*) AS WrongChecks
	FROM CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND c.SourceCertificateId IS NOT NULL
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,COUNT(*) AS WrongChecks
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	AND c.SourceCertificateId IS NOT NULL
	GROUP BY IOrgReq.OrganizationId
) AS IWrong
GROUP BY IWrong.OrganizationId





DECLARE @UIChecksByOrg TABLE
(
	OrganizationId INT,
	TotalUIChecks INT,
	UniqueUIChecks INT
)
INSERT INTO @UIChecksByOrg
SELECT IOrgReq.OrganizationId,COUNT(*) AS TotalUIChecks 
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'')+
ISNULL(ChLog.PassportSeria,'')+
ISNULL(ChLog.PassportNumber,'')+
ISNULL(ChLog.CNENumber,'')+
ISNULL(ChLog.Marks,'')) AS UniqueUIChecks 
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId



DECLARE @CheckLimitDatesByOrg TABLE
(
	OrganizationId INT,
	FirstCheck DATETIME,
	LastCheck DATETIME
)
INSERT INTO @CheckLimitDatesByOrg
SELECT OrganizationId,MIN(FirstCheck),MAX(LastCheck)
FROM 
(
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateRequestBatch cb 
	INNER JOIN CommonNationalExamCertificateRequest c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(cb.updatedate) AS FirstCheck,MAX(cb.updatedate) AS LastCheck
	FROM CommonNationalExamCertificateCheckBatch cb 
	INNER JOIN CommonNationalExamCertificateCheck c ON cb.id = c.batchid 
	INNER JOIN Account Acc ON cb.owneraccountid = Acc.id
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id=Acc.OrganizationId
	WHERE IOrgReq.OrganizationId IS NOT NULL
	AND cb.updatedate BETWEEN @periodBegin and @periodEnd
	GROUP BY IOrgReq.OrganizationId
	UNION 
	SELECT IOrgReq.OrganizationId,MIN(ChLog.EventDate) AS FirstCheck,MAX(ChLog.EventDate) AS LastCheck
	FROM CNEWebUICheckLog ChLog
	INNER JOIN Account Acc ON Acc.Id=ChLog.AccountId
	INNER JOIN OrganizationRequest2010 IOrgReq ON IOrgReq.Id= Acc.OrganizationId
	WHERE ChLog.EventDate BETWEEN @periodBegin and @periodEnd
	GROUP BY IOrgReq.OrganizationId
) AS RawCheckLimitDates
GROUP BY OrganizationId






INSERT INTO @Report
SELECT 
Org.[Полное наименование] AS [Полное наименование]
,ISNULL(Org.[Краткое наименование],'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Org.[Имя ФО] AS [Имя ФО]
,Org.[Код ФО] AS [Код ФО]
,Org.[Имя региона] AS [Имя региона]
,Org.[Код региона] AS [Код региона]
,Org.[Тип] AS [Тип]
,Org.[Вид] AS [Вид]
,Org.[ОПФ] AS [ОПФ]
,Org.[Филиал] AS [Филиал]
,Org.[Аккредитация по справочнику] AS [Аккредитация по справочнику]
,Org.[Свидетельство об аккредитации] AS [Свидетельство об аккредитации]
,Org.[Аккредитация по факту] AS [Аккредитация по факту] 	
,Org.[ФИО руководителя] AS [ФИО руководителя]
,Org.[Должность руководителя] AS [Должность руководителя]
,Org.[Ведомственная принадлежность] AS [Ведомственная принадлежность]
,Org.[Фактический адрес] AS [Фактический адрес]
,Org.[Юридический адрес] AS [Юридический адрес]
,Org.[Код города] AS[Код города]
,Org.[Телефон] AS [Телефон]
,Org.[EMail] AS [EMail]
,Org.[ИНН] AS [ИНН]
,Org.[ОГРН] AS [ОГРН]

,CASE 
	WHEN (ISNULL(UsersCnt.UsersCount,0)=0)
	THEN 'Нет'
	ELSE 'Да'
	END AS [Подана заявка на регистрацию]
,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]

,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]

,LimitDates.FirstCheck AS [Первая проверка]  
,LimitDates.LastCheck AS [Последняя проверка] 

,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN 'Не работает'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN 'Работа неактивна'
ELSE 'Работает'
END
AS [Работа с ФБС]

FROM 
ReportOrgsBASE() Org 
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

LEFT JOIN @NumberChecksByOrg NumberChecks
ON Org.Id=NumberChecks.OrganizationId
LEFT JOIN @TNChecksByOrg TNChecks
ON Org.Id=TNChecks.OrganizationId
LEFT JOIN @PassportChecksByOrg PassportChecks
ON Org.Id=PassportChecks.OrganizationId
LEFT JOIN @UIChecksByOrg UIChecks
ON Org.Id=UIChecks.OrganizationId
LEFT JOIN @WrongChecksByOrg WrongChecks
ON Org.Id=WrongChecks.OrganizationId
LEFT JOIN @CheckLimitDatesByOrg LimitDates
ON Org.Id=LimitDates.OrganizationId

RETURN
END
GO
PRINT N'Altering [dbo].[ReportXMLSubordinateOrg]...';


GO

--Функция по подведомственным учреждениям для экспорта в XML
ALTER FUNCTION  [dbo].[ReportXMLSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	[Код ОУ] int null,
	[Полное наименование] nvarchar(Max) null,
	[Код региона] int null,
	[Наименование региона] nvarchar(255) null,
	[Свидетельство об аккредитации] nvarchar(255) null,
	[ФИО руководителя] nvarchar(255) null,
	[Количество пользователей] int null,
	[Дата активации пользователя] datetime null,
	[Количество уникальных проверок] int null
)
AS BEGIN
INSERT INTO @Report
SELECT
	Id [Код ОУ],
	FullName [Полное наименование],
	RegionId [Код региона] ,
	RegionName [Наименование региона],
	AccreditationSertificate [Свидетельство об аккредитации],
	DirectorFullName [ФИО руководителя],
	CountUser [Количество пользователей],
	UserUpdateDate [Дата активации],
	CountUniqueChecks [Уникальных проверок]
FROM
	dbo.ReportStatisticSubordinateOrg ( @periodBegin, @periodEnd, @departmentId)
RETURN
END
GO
PRINT N'Altering [dbo].[ReportUserStatusWithAccredTVF]...';


GO
ALTER function [dbo].[ReportUserStatusWithAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @Statuses TABLE
(
	[Name] NVARCHAR (50),
	Code NVARCHAR (50),
	[Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
	[Name] NVARCHAR (50),
	Code BIT,
	[Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode INT,
	IsPrivateName NVARCHAR (50),
	IsPrivateCode INT,
	IsPrivateOrder INT,
	StatusName NVARCHAR(50),
	StatusCode NVARCHAR(50),
	StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	IsPrivateOrder NVARCHAR (50),
	StatusName NVARCHAR(50),
	UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
ON Acc.OrganizationId=OReq.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
	AND (
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	))
OR (
	(Acc.Status=Comb.StatusCode)
	AND (
		Comb.OrgTypeCode=10
	)
	AND (
		OReq.TypeId IS NOT NULL
	)
)
OR (
	Comb.StatusCode='total'
	AND ((
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	)
	OR
	((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
(	
	MainOrder INT,
	OrgTypeName NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	[Всего] INT,
	[Активирован] INT,
	[На регистрации] INT,
	[На доработке] INT,
	[На согласовании] INT,
	[Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID in (6,7)
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF (@periodBegin ,@periodEnd)

return
END
GO
PRINT N'Altering [dbo].[ReportRegistrationShortTVF]...';


GO
ALTER function [dbo].[ReportRegistrationShortTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(	
[Правовая форма] NVARCHAR(255) NULL
,[Зарегистрировано] INT null
,[Не зарегистрировано] INT null
,[Всего] INT null
)
AS 
BEGIN

 
DECLARE @RegistredOrgsPrivCount INT
SELECT @RegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ'
DECLARE @NotRegistredOrgsPrivCount INT
SELECT @NotRegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ'
DECLARE @RegistredOrgsStateCount INT
SELECT @RegistredOrgsStateCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ'
DECLARE @NotRegistredOrgsStateCount INT
SELECT @NotRegistredOrgsStateCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ'


DECLARE @RegistredOrgsPrivAccredCount INT
SELECT @RegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @NotRegistredOrgsPrivAccredCount INT
SELECT @NotRegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @RegistredOrgsStateAccredCount INT
SELECT @RegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @NotRegistredOrgsStateAccredCount INT
SELECT @NotRegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'

INSERT INTO @Report
SELECT 'Государственный',@RegistredOrgsStateCount,@NotRegistredOrgsStateCount,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount
INSERT INTO @Report
SELECT 'Негосударственный',@RegistredOrgsPrivCount,@NotRegistredOrgsPrivCount,@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount
INSERT INTO @Report
SELECT 'Итого'
,@RegistredOrgsStateCount+@RegistredOrgsPrivCount
,@NotRegistredOrgsStateCount+@NotRegistredOrgsPrivCount
,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount+@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount


INSERT INTO @Report
SELECT '',null,null,null
INSERT INTO @Report
SELECT 'Аккредитованных',null,null,null


INSERT INTO @Report
SELECT 'Государственный',@RegistredOrgsStateAccredCount,@NotRegistredOrgsStateAccredCount,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount
INSERT INTO @Report
SELECT 'Негосударственный',@RegistredOrgsPrivAccredCount,@NotRegistredOrgsPrivAccredCount,@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount
INSERT INTO @Report
SELECT 'Итого'
,@RegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount
,@NotRegistredOrgsStateAccredCount+@NotRegistredOrgsPrivAccredCount
,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount

RETURN
END
GO
PRINT N'Altering [dbo].[ReportOrgsStatusWithAccredTVF]...';


GO
ALTER function [dbo].[ReportOrgsStatusWithAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[Тип ОУ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Филиал] nvarchar(50) null,
	[В БД] int null,
	[Из них действующих] int null
)
AS
BEGIN

DECLARE @PreReport TABLE
(
	[Тип ОУ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Филиал] nvarchar(50) null,
	[Аккредитация] nvarchar(50) null,
	[В БД] int null,
	[Из них действующих] int null
)
INSERT INTO @PreReport
SELECT 
OrgInfo.[Тип],
OrgInfo.[ОПФ],
OrgInfo.[Филиал],
OrgInfo.[Аккредитация по факту],
COUNT(*),
COUNT(CASE WHEN OrgInfo.[Пользователей активировано]>0 THEN 1 ELSE NULL END) 
FROM dbo.[ReportOrgsInfoTVF_WithoutChecks](null,null) AS OrgInfo
GROUP BY 
OrgInfo.[Тип],
OrgInfo.[ОПФ],
OrgInfo.[Филиал],
OrgInfo.[Аккредитация по факту]


INSERT INTO @Report
SELECT [Тип ОУ],'Государственный',[Филиал],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма],[Филиал]
UNION ALL
SELECT 'ВУЗ','Государственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный',[Филиал],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма],[Филиал]
UNION ALL
SELECT 'ВУЗ','Негосударственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'

INSERT INTO @Report
SELECT 'ВУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' 


INSERT INTO @Report
SELECT [Тип ОУ],'Государственный','-',SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма]

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный','-',SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма]

INSERT INTO @Report
SELECT 'ССУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' 


INSERT INTO @Report
SELECT [Тип ОУ],'-','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]<>'ССУЗ'  AND [Тип ОУ]<>'ВУЗ'
GROUP BY [Тип ОУ]

INSERT INTO @Report
SELECT 'Итого','-','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport


INSERT INTO @Report
SELECT NULL, NULL, NULL, NULL, NULL
INSERT INTO @Report
SELECT NULL, NULL, NULL, NULL, NULL
INSERT INTO @Report
SELECT 'В разрезе наличия аккредитации', NULL, NULL, NULL, NULL

INSERT INTO @Report
SELECT 'Тип ОУ','Правовая форма','Аккредитация',NULL, NULL

INSERT INTO @Report
SELECT [Тип ОУ],'Государственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ВУЗ','Государственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ВУЗ','Негосударственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'

INSERT INTO @Report
SELECT 'ВУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' 

INSERT INTO @Report
SELECT [Тип ОУ],'Государственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ССУЗ','Государственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Гос-ный'

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ССУЗ','Негосударственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Частный'

INSERT INTO @Report
SELECT 'ССУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ'  

INSERT INTO @Report
SELECT 'Итого','-','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport 
WHERE  [Тип ОУ]='ССУЗ' OR [Тип ОУ]='ВУЗ'

return
END
GO
PRINT N'Altering [dbo].[ReportCheckedCNEsBASE]...';


GO
ALTER function [dbo].[ReportCheckedCNEsBASE](
	)
RETURNS @report TABLE 
(
CNEId BIGINT
,CNENumber NVARCHAR(255)
,OrgId INT
)
AS 
BEGIN
DECLARE @PreReport TABLE
(
	CNEId BIGINT
	,CNENumber NVARCHAR(255)
	,OrgId INT
)
INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.OrganizationId AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateCheck c  ON c.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
INNER JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
INNER JOIN GroupAccount GA ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId 
AND OReq.OrganizationId IS NOT NULL

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.OrganizationId AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=0
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN GroupAccount GA ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId
AND OReq.OrganizationId IS NOT NULL

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.OrganizationId AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=1
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN GroupAccount GA ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId
AND OReq.OrganizationId IS NOT NULL

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT CNE.Id AS CNEId,CNE.Number AS CNENumber,OReq.OrganizationId AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CNEWebUICheckLog ChLog ON ChLog.FoundedCNEId=CNE.Id 
INNER JOIN Account Acc ON ChLog.AccountId=Acc.Id 
INNER JOIN GroupAccount GA 	ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId
AND OReq.OrganizationId IS NOT NULL

INSERT INTO @Report
SELECT DISTINCT * FROM @PreReport

RETURN
END
GO
PRINT N'Altering [dbo].[ReportChecksAllTVF]...';


GO
ALTER function [dbo].[ReportChecksAllTVF]()
RETURNS @report TABLE 
(	 					
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
,[order] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам

--Пакетные по паспорту
DECLARE @TotalByPassport_Batch INT
DECLARE @UniqueByPassport_Batch INT

SELECT @TotalByPassport_Batch=COUNT(*), @UniqueByPassport_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL


--Пакетные по номеру св-ва
DECLARE @TotalByNumber_Batch INT
DECLARE @UniqueByNumber_Batch INT

SELECT @TotalByNumber_Batch=COUNT(*), @UniqueByNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,c.SourceCertificateId))
FROM [CommonNationalExamCertificateCheckBatch] AS b WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateCheck] AS c WITH(NOLOCK)  ON c.batchid = b.id
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=b.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL


--Пакетные по типографскому номеру
DECLARE @TotalByTypNumber_Batch INT
DECLARE @UniqueByTypNumber_Batch INT

SELECT @TotalByTypNumber_Batch=COUNT(*), @UniqueByTypNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

--Итого по пакетным
DECLARE @Total_Batch INT
DECLARE @TotalUnique_Batch INT

SET @Total_Batch=
@TotalByPassport_Batch+
@TotalByNumber_Batch+
@TotalByTypNumber_Batch
SET @TotalUnique_Batch=
@UniqueByPassport_Batch+
@UniqueByNumber_Batch+
@UniqueByTypNumber_Batch

--Интерактивные по паспорту
DECLARE @TotalByPassport_UI INT
DECLARE @UniqueByPassport_UI INT

SELECT @TotalByPassport_UI = COUNT(*), @UniqueByPassport_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Passport'


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='CNENumber'


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Typographic'


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Marks'


--Итого по интерактивным
DECLARE @Total_UI INT
DECLARE @TotalUnique_UI INT

SET @Total_UI=
@TotalByPassport_UI+
@TotalByNumber_UI+
@TotalByTypNumber_UI+
@TotalByMarks_UI
SET @TotalUnique_UI=
@UniqueByPassport_UI+
@UniqueByNumber_UI+
@UniqueByTypNumber_UI+
@UniqueByMarks_UI


INSERT INTO @report
SELECT
'Пакетная',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch,8
UNION
SELECT 
'Интерактивная',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI,9
UNION
SELECT 
'Итого',@Total_Batch+@Total_UI,@UniqueByNumber_Batch+@UniqueByNumber_UI,@UniqueByTypNumber_Batch+@UniqueByTypNumber_UI,@UniqueByPassport_Batch+@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_Batch+@TotalUnique_UI,10

RETURN
end
GO
PRINT N'Altering [dbo].[ReportChecksByPeriodTVF]...';


GO
ALTER function [dbo].[ReportChecksByPeriodTVF](
	@from datetime = null
	, @to datetime = null)
RETURNS @report TABLE 
(	 					
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
,[order] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
	select @from = dateadd(year, -1, getdate()), @to = getdate()

--Пакетные по паспорту
DECLARE @TotalByPassport_Batch INT
DECLARE @UniqueByPassport_Batch INT

SELECT @TotalByPassport_Batch=COUNT(*), @UniqueByPassport_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to


--Пакетные по номеру св-ва
DECLARE @TotalByNumber_Batch INT
DECLARE @UniqueByNumber_Batch INT

SELECT @TotalByNumber_Batch=COUNT(*), @UniqueByNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,c.SourceCertificateId))
FROM [CommonNationalExamCertificateCheckBatch] AS b WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateCheck] AS c WITH(NOLOCK)  ON c.batchid = b.id
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=b.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE b.updatedate BETWEEN @from and @to


--Пакетные по типографскому номеру
DECLARE @TotalByTypNumber_Batch INT
DECLARE @UniqueByTypNumber_Batch INT

SELECT @TotalByTypNumber_Batch=COUNT(*), @UniqueByTypNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to

--Итого по пакетным
DECLARE @Total_Batch INT
DECLARE @TotalUnique_Batch INT

SET @Total_Batch=
@TotalByPassport_Batch+
@TotalByNumber_Batch+
@TotalByTypNumber_Batch
SET @TotalUnique_Batch=
@UniqueByPassport_Batch+
@UniqueByNumber_Batch+
@UniqueByTypNumber_Batch

--Интерактивные по паспорту
DECLARE @TotalByPassport_UI INT
DECLARE @UniqueByPassport_UI INT

SELECT @TotalByPassport_UI = COUNT(*), @UniqueByPassport_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Passport'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='CNENumber'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Typographic'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Marks'
AND ChLog.EventDate BETWEEN @from and @to


--Итого по интерактивным
DECLARE @Total_UI INT
DECLARE @TotalUnique_UI INT

SET @Total_UI=
@TotalByPassport_UI+
@TotalByNumber_UI+
@TotalByTypNumber_UI+
@TotalByMarks_UI
SET @TotalUnique_UI=
@UniqueByPassport_UI+
@UniqueByNumber_UI+
@UniqueByTypNumber_UI+
@UniqueByMarks_UI


INSERT INTO @report
SELECT
'Пакетная',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch,1
UNION
SELECT 
'Интерактивная',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI,2
UNION
SELECT 
'Итого',@Total_Batch+@Total_UI,@UniqueByNumber_Batch+@UniqueByNumber_UI,@UniqueByTypNumber_Batch+@UniqueByTypNumber_UI,@UniqueByPassport_Batch+@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_Batch+@TotalUnique_UI,3

RETURN
end
GO
PRINT N'Altering [dbo].[ReportCheckStatisticsTVF]...';


GO
ALTER FUNCTION [dbo].[ReportCheckStatisticsTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[Код региона] nvarchar(10) null,
	[Регион] nvarchar(100) null,
	[Пакетов (по паспорту)] int null,
	[Уникальных проверок (по паспорту)] int null,
	[Всего проверок (по паспорту)] int null,

	[Пакетов (по ТН)] int null,
	[Уникальных проверок (по ТН)] int null,
	[Всего проверок (по ТН)] int null,

	[Пакетов (по номеру)] int null,
	[Уникальных проверок (по номеру)] int null,
	[Всего проверок (по номеру)] int null,

	[Интерактивных проверок по паспорту] int null,
	[Интерактивных проверок по номеру] int null,
	[Интерактивных проверок по ТН] int null,
	[Интерактивных проверок по баллам] int null
)
AS 
begin

insert into @report
select 
isnull(r.code,'') [Код региона]
,isnull(r.name,'Не указан') [Регион]

,sum(p.PassportBatchCount) [Пакетов (по паспорту)]
,sum(p.UniquePassportCount) [Уникальных проверок (по паспорту)]
,sum(p.TotalPassportCount) [Всего проверок (по паспорту)]

,sum(t.TypographicBatchCount) [Пакетов (по ТН)]
,sum(t.UniqueTypographicCount) [Уникальных проверок (по ТН)]
,sum(t.TotalTypographicCount) [Всего проверок (по ТН)]

,sum(n.NumberBatchCount) [Пакетов (по номеру)]
,sum(n.UniqueNumberCount) [Уникальных проверок (по номеру)]
,sum(n.TotalNumberCount) [Всего проверок (по номеру)]

,sum(iPassport.Cnt) [Интерактивных проверок по паспорту]
,sum(iCNENumber.Cnt) [Интерактивных проверок по номеру]
,sum(iTyp.Cnt) [Интерактивных проверок по ТН]
,sum(iMarks.Cnt) [Интерактивных проверок по баллым]

from region r with(nolock)
full join 
	(
select 
OReq.regionid
, count(distinct cnecrb.id) PassportBatchCount
, count(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.[SourceCertificateId])) UniquePassportCount
, count(*) TotalPassportCount 
from [CommonNationalExamCertificateRequestBatch] as cnecrb with(nolock) 
INNER JOIN [CommonNationalExamCertificateRequest] as cnecr with(nolock) on cnecr.batchid = cnecrb.id and cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
where cnecrb.updatedate BETWEEN @periodBegin and @periodEnd
group by OReq.regionid
	) p on r.id = p.regionid
	full join 
	(
select 
OReq.regionid
, count(distinct cnecrb.id) TypographicBatchCount
, count(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.[SourceCertificateId])) UniqueTypographicCount
, count(*) TotalTypographicCount
from [CommonNationalExamCertificateRequestBatch] as cnecrb with(nolock) 
join [CommonNationalExamCertificateRequest] as cnecr with(nolock) on cnecr.batchid = cnecrb.id and cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

where cnecrb.updatedate BETWEEN @periodBegin and @periodEnd
group by OReq.regionid
	) t on r.id = t.regionid
	full join (
select 
OReq.regionid
, count(distinct cneccb.id) NumberBatchCount
, count(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecc.SourceCertificateId)) UniqueNumberCount
, count(*) TotalNumberCount
from [CommonNationalExamCertificateCheckBatch] as cneccb with(nolock) 
join [CommonNationalExamCertificateCheck] as cnecc with(nolock) on cnecc.batchid = cneccb.id 
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cneccb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
where cneccb.updatedate BETWEEN @periodBegin and @periodEnd
group by OReq.regionid
	) n on r.id = n.regionid
FULL JOIN (
SELECT
	OReq.regionid
	,COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId)) AS Cnt
	FROM dbo.CNEWebUICheckLog as ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
	WHERE ChLog.EventDate BETWEEN @periodBegin AND @periodEnd AND ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'CNENumber'
	GROUP BY OReq.regionid
	) iCNENumber on iCNENumber.regionid = r.id
FULL JOIN (
SELECT
	OReq.regionid
	,COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))AS Cnt
	FROM dbo.CNEWebUICheckLog as ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
	WHERE ChLog.EventDate BETWEEN @periodBegin AND @periodEnd AND ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Passport'
	GROUP BY OReq.regionid
	) iPassport on iPassport.regionid = r.id
FULL JOIN (
SELECT
	OReq.regionid
	,COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))AS Cnt
	FROM dbo.CNEWebUICheckLog as ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
	WHERE ChLog.EventDate BETWEEN @periodBegin AND @periodEnd AND ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Typographic'
	GROUP BY OReq.regionid
	) iTyp on iTyp.regionid = r.id
FULL JOIN (
SELECT
	OReq.regionid
	,COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))AS Cnt
	FROM dbo.CNEWebUICheckLog as ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
	WHERE ChLog.EventDate BETWEEN @periodBegin AND @periodEnd AND ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Marks'
	GROUP BY OReq.regionid
	) iMarks on iMarks.regionid = r.id
group by r.code, r.name
	order by max(r.id)


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '', 'Итого за ' + case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end 
,sum([Пакетов (по паспорту)])
,sum([Уникальных проверок (по паспорту)])
,sum([Всего проверок (по паспорту)])
,sum([Пакетов (по ТН)])
,sum([Уникальных проверок (по ТН)])
,sum([Всего проверок (по ТН)])
,sum([Пакетов (по номеру)])
,sum([Уникальных проверок (по номеру)])
,sum([Всего проверок (по номеру)])
,sum([Интерактивных проверок по паспорту])
,sum([Интерактивных проверок по номеру])
,sum([Интерактивных проверок по ТН])
,sum([Интерактивных проверок по баллам])
from @report

--Статистика за все время
	DECLARE @NumberUnique_UI INT
	SELECT @NumberUnique_UI = COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'CNENumber'
	
	DECLARE @PassportUnique_UI INT
	SELECT @PassportUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR, ChLog.FoundedCNEId))
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Passport'
	
	DECLARE @TypNumberUnique_UI INT
	SELECT @TypNumberUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Typographic'
	
	DECLARE @MarksUnique_UI INT
	SELECT @MarksUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR, ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Marks'

;with 
PassportChecks ([Пакетов (по паспорту)], [Уникальных проверок (по паспорту)], [Всего проверок (по паспорту)]) as
	(select 
	count(distinct cnecrb.id) [Пакетов (по паспорту)]
	, count(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.[SourceCertificateId])) [Уникальных проверок (по паспорту)]
	, count(*) [Всего проверок (по паспорту)] 
	from [CommonNationalExamCertificateRequestBatch] as cnecrb with(nolock) 
	join [CommonNationalExamCertificateRequest] as cnecr with(nolock) on cnecr.batchid = cnecrb.id and cnecrb.[IsTypographicNumber] = 0
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL)

, TypographicChecks ([Пакетов (по ТН)], [Уникальных проверок (по ТН)], [Всего проверок (по ТН)]) as
	(select 
	count(distinct cnecrb.id) [Пакетов (по ТН)]
	, count(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.[SourceCertificateId])) [Уникальных проверок (по ТН)]
	, count(*) [Всего проверок (по ТН)]
	from [CommonNationalExamCertificateRequestBatch] as cnecrb with(nolock) 
	join [CommonNationalExamCertificateRequest] as cnecr with(nolock) on cnecr.batchid = cnecrb.id and cnecrb.[IsTypographicNumber] = 1
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL)

, NumberChecks ([Пакетов (по номеру)], [Уникальных проверок (по номеру)], [Всего проверок (по номеру)]) as
	(select 
	count(distinct cneccb.id) [Пакетов (по номеру)]
	, count(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecc.SourceCertificateId)) [Уникальных проверок (по номеру)]
	, count(*) [Всего проверок (по номеру)]
	from [CommonNationalExamCertificateCheckBatch] as cneccb with(nolock) 
	join [CommonNationalExamCertificateCheck] as cnecc with(nolock) on cnecc.batchid = cneccb.id 
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cneccb.OwnerAccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL)

	
	
insert into @report
select '', 'Итого за все время'
,[Пакетов (по паспорту)]
,[Уникальных проверок (по паспорту)]
,[Всего проверок (по паспорту)]
,[Пакетов (по ТН)]
,[Уникальных проверок (по ТН)]
,[Всего проверок (по ТН)]
,[Пакетов (по номеру)]
,[Уникальных проверок (по номеру)]
,[Всего проверок (по номеру)]
,@PassportUnique_UI
,@NumberUnique_UI
,@TypNumberUnique_UI
,@MarksUnique_UI
from PassportChecks, TypographicChecks, NumberChecks

return

end
GO
PRINT N'Altering [dbo].[ReportPotentialAbusersTVF]...';


GO
--Отчет: Отчет о потенциальных пользователях, осуществляющих перебор
ALTER FUNCTION [dbo].[ReportPotentialAbusersTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[Проверок] int null,
	[Логин] nvarchar(255) null,
	[ФИО] nvarchar(255) null, 
	[Организация] nvarchar(255) null,
	[Email] nvarchar(255) null,
	[Телефон] nvarchar(255) null
)
as
begin
;with WrongRequestCount ([count], [user]) as
	(select 
		count(distinct 
			cnecr.lastname 
			+ isnull(cnecr.firstname,'') 
			+ isnull(cnecr.PatronymicName,'')
			+ isnull(cnecr.PassportSeria,'')
			+ isnull(cnecr.PassportNumber,'')
			+ isnull(cnecr.TypographicNumber,'')
			)
		, a.login
		from [CommonNationalExamCertificateRequestBatch] as cnecrb with(nolock) 
		join [CommonNationalExamCertificateRequest] as cnecr with(nolock) on cnecr.batchid = cnecrb.id and cnecr.SourceCertificateId is null
		join account a with(nolock) on a.id = cnecrb.owneraccountid
		join groupaccount ga with(nolock) on ga.accountid = a.id and ga.groupid = 1
	where cnecrb.createdate BETWEEN @periodBegin and @periodEnd 
	group by a.login)
,WrongCheckCount([count], [user]) as
	(select 
		count(distinct 
			cnecc.lastname 
			+ isnull(cnecc.firstname,'') 
			+ isnull(cnecc.PatronymicName,'')
			+ isnull(cnecc.PassportSeria,'')
			+ isnull(cnecc.PassportNumber,'')
			+ isnull(cnecc.TypographicNumber,'')
			)
		, a.login
		from [CommonNationalExamCertificateCheckBatch] as cneccb with(nolock) 
		join [CommonNationalExamCertificateCheck] as cnecc with(nolock) on cnecc.batchid = cneccb.id and cnecc.SourceCertificateId is null
		join account a with(nolock) on a.id = cneccb.owneraccountid
		join groupaccount ga with(nolock) on ga.accountid = a.id and ga.groupid = 1
	where cneccb.createdate BETWEEN @periodBegin and @periodEnd 
	group by a.login)
insert into @report
select 
isnull(wrc.[count],0) + isnull(wcc.[count],0)
, coalesce(wrc.[user],wcc.[user])
, a.lastname
, o.FullName
, a.email
, a.phone
from WrongRequestCount wrc
full join WrongCheckCount wcc on wrc.[user] = wcc.[user]
join Account a with(nolock) on a.login = coalesce(wrc.[user],wcc.[user])
join GroupAccount ga with(nolock) on ga.accountid = a.id and ga.groupid = 1
join OrganizationRequest2010 o with(nolock) on o.id = a.organizationid
where isnull(wrc.[count],0) + isnull(wcc.[count],0) >= 1000
order by 1 desc

return
end
GO
PRINT N'Altering [dbo].[ReportTotalChecksTVF]...';


GO
ALTER function [dbo].[ReportTotalChecksTVF](
	@from datetime = null
	, @to datetime = null)
RETURNS @report TABLE 
(	 					
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
	select @from = dateadd(year, -1, getdate()), @to = getdate()

--Пакетные по паспорту
DECLARE @TotalByPassport_Batch INT
DECLARE @UniqueByPassport_Batch INT

SELECT @TotalByPassport_Batch=COUNT(*), @UniqueByPassport_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 0
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to


--Пакетные по номеру св-ва
DECLARE @TotalByNumber_Batch INT
DECLARE @UniqueByNumber_Batch INT

SELECT @TotalByNumber_Batch=COUNT(*), @UniqueByNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,c.SourceCertificateId))
FROM [CommonNationalExamCertificateCheckBatch] AS b WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateCheck] AS c WITH(NOLOCK)  ON c.batchid = b.id
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=b.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE b.updatedate BETWEEN @from and @to


--Пакетные по типографскому номеру
DECLARE @TotalByTypNumber_Batch INT
DECLARE @UniqueByTypNumber_Batch INT

SELECT @TotalByTypNumber_Batch=COUNT(*), @UniqueByTypNumber_Batch=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,cnecr.SourceCertificateId))
FROM [CommonNationalExamCertificateRequestBatch] AS cnecrb WITH(NOLOCK) 
INNER JOIN [CommonNationalExamCertificateRequest] AS cnecr WITH(NOLOCK) ON cnecr.batchid = cnecrb.id AND cnecrb.[IsTypographicNumber] = 1
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=cnecrb.OwnerAccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE cnecrb.updatedate BETWEEN @from and @to

--Итого по пакетным
DECLARE @Total_Batch INT
DECLARE @TotalUnique_Batch INT

SET @Total_Batch=
@TotalByPassport_Batch+
@TotalByNumber_Batch+
@TotalByTypNumber_Batch
SET @TotalUnique_Batch=
@UniqueByPassport_Batch+
@UniqueByNumber_Batch+
@UniqueByTypNumber_Batch

--Интерактивные по паспорту
DECLARE @TotalByPassport_UI INT
DECLARE @UniqueByPassport_UI INT

SELECT @TotalByPassport_UI = COUNT(*), @UniqueByPassport_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Passport'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='CNENumber'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Typographic'
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode='Marks'
AND ChLog.EventDate BETWEEN @from and @to


--Итого по интерактивным
DECLARE @Total_UI INT
DECLARE @TotalUnique_UI INT

SET @Total_UI=
@TotalByPassport_UI+
@TotalByNumber_UI+
@TotalByTypNumber_UI+
@TotalByMarks_UI
SET @TotalUnique_UI=
@UniqueByPassport_UI+
@UniqueByNumber_UI+
@UniqueByTypNumber_UI+
@UniqueByMarks_UI


INSERT INTO @report
SELECT
'Пакетная',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch
UNION
SELECT 
'Интерактивная',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI

RETURN
end
GO
PRINT N'Altering [dbo].[ReportCheckedCNEsAggregatedTVF]...';


GO
ALTER function [dbo].[ReportCheckedCNEsAggregatedTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Количество свидетельств] INT
,[Проверок в различных ОУ] INT
)
AS 
BEGIN


INSERT INTO @Report ([Количество свидетельств],[Проверок в различных ОУ])
SELECT COUNT(IAggrChecks.CNEId) AS CNECount,IAggrChecks.OrgCount AS OrgCount FROM 
(
	SELECT 
	CNEId AS CNEId
	,COUNT(OrgId) AS OrgCount
	FROM [ReportCheckedCNEsBASE]() AS IChecks
	GROUP BY IChecks.CNEId HAVING COUNT(IChecks.OrgId)>=1
) AS IAggrChecks
GROUP BY IAggrChecks.OrgCount 
ORDER BY IAggrChecks.OrgCount 

RETURN
END
GO
PRINT N'Altering [dbo].[ReportTotalChecksTVF_New]...';


GO
ALTER function [dbo].[ReportTotalChecksTVF_New](
	@from datetime = null
	, @to datetime = null)
RETURNS @report TABLE 
(	 					
[Тип проверки] NVARCHAR(20) NULL
,[всего] INT NULL
,[уникальных проверок по РН] INT NULL
,[уникальных проверок по ТН] INT NULL
,[уникальных проверок по документу] INT NULL
,[уникальных проверок по ФИО и баллам] INT NULL
,[всего уникальных проверок] INT NULL
)
AS 
begin

--если не определены временные границы, то указывается промежуток = 1 суткам
if(@from is null or @to is null)
	select @from = dateadd(year, -1, getdate()), @to = getdate()


INSERT INTO @report
SELECT [Тип проверки] 
,[всего] 
,[уникальных проверок по РН] 
,[уникальных проверок по ТН] 
,[уникальных проверок по документу] 
,[уникальных проверок по ФИО и баллам] 
,[всего уникальных проверок] 
FROM(
SELECT * FROM dbo.ReportChecksByPeriodTVF(@from,@to)
UNION ALL
SELECT NULL,NULL,NULL,NULL,NULL,NULL,NULL,5
UNION ALL
SELECT * FROM dbo.ReportChecksAllTVF()
) INN ORDER BY [order]

RETURN
end
GO
PRINT N'Creating [dbo].[ufn_ut_SplitFromString]...';


GO
create function [dbo].[ufn_ut_SplitFromString] 
(
	@string nvarchar(max),
	@delimeter nvarchar(1) = ' '
)
returns @ret table (nam nvarchar(1000) )
as
begin
	if len(@string)=0 
		return 
	declare @s int, @e int
	set @s = 0
	while charindex(@delimeter,@string,@s) <> 0
	begin
		set @e = charindex(@delimeter,@string,@s)
		insert @ret values (rtrim(ltrim(substring(@string,@s,@e - @s))))
		set @s = @e + 1
	end
	insert @ret values (rtrim(ltrim(substring(@string,@s,300))))
	return
end
GO
PRINT N'Creating [dbo].[UpdateUserAccountNew]...';


GO
-- =============================================
-- Modified 29.01.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUserAccountNew]
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
@position NVARCHAR (255)=null,
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
@organizationRcModelId INT=null,
@orgRCDescription NVARCHAR(400)=NULL,
@ExistingOrgId INT=NULL,
@orgRequestID INT=null OUTPUT,
@ListSystemId nvarchar(max),
@accessToFbd BIT = 0
AS
BEGIN	
	-- при добавлении пользователя - проверка есть ли уже такой?
	if exists (SELECT * FROM Account a WHERE a.Email = @email AND @login = '')
	BEGIN
		RAISERROR('$Пользователь с указанным логином уже существует.', 18, 1)
		RETURN -1
	END
	
	declare 
		@newAccount bit
		, @accountId bigint
		, @currentYear int
		, @isOrganizationOwner bit		
		, @editorAccountId bigint
		, @departmentOwnershipCode nvarchar(255)
		, @eventCode nvarchar(100)		
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)
		, @useOnlyDocumentParam BIT
		, @userStatusBefore NVARCHAR(510)
		, @isRegistrationDocumentExistsForUser BIT

	set @updateId = newid()
	
	declare @tableSystemId table (SystemId int)
	insert @tableSystemId(SystemId)	
	select * from ufn_ut_SplitFromString(@ListSystemId,',') 	

	declare @oldIpAddress table (ip nvarchar(255))
	declare @newIpAddress table (ip nvarchar(255))

	set @currentYear = year(getdate())
	set @departmentOwnershipCode = null

	select @editorAccountId = account.[Id], 
	  @isRegistrationDocumentExistsForUser = case when account.RegistrationDocument IS NULL THEN 0 ELSE 1 END
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
		
		-- в качестве логина пользователя используем email
		SET @login = @email

		set @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		set @hasFixedIp = isnull(@hasFixedIp, 1)
		set @hasCrocEgeIntegration = isnull(@hasCrocEgeIntegration, 0)
	end
	else
	begin -- update существующего пользователя
		
		select 
			@accountId = account.[Id]
			, @userStatusBefore = account.[Status]
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
			-- берем последнюю поданную заявку			
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
		
	-- определяем идентификатор статуса
	DECLARE @statusID INT
	SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @status

	begin tran insert_update_account_tran
	
		IF(@orgRequestID IS NULL)
		BEGIN
			-- заявка подается не зависимо от того, новый аккаунт создается или обновляется старый
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
				OrganizationId,
				StatusID,
				RCModelID,
				RCDescription
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
				@ExistingOrgId,
				@statusID,
				@organizationRcModelId,
				@orgRCDescription
				 
			if (@@error <> 0)
				goto undo

			select @orgRequestID = scope_identity()
			if (@@error <> 0)
				goto undo
		END
	
		if @newAccount = 1 -- внесение нового пользователя
		begin
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
				, Position
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
				, @orgRequestID
				, 1
				, @currentYear
				, @phone
				, @position
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
		end	
		else 
		begin -- update существующего пользователя			
			
			-- если пользователь получает доступ к ФБД и привязывается к организации, у которой уже есть активированный УС ОУ для ФБД, то выводим ошибку
			IF(@userStatusBefore = 'activated' AND @accessToFbd = 1 AND EXISTS(
				SELECT * FROM OrganizationRequest2010 or1 JOIN Account a ON or1.Id = a.OrganizationId
					JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id AND ora.AccountID = a.Id
					JOIN [GROUP] g ON g.Id = ora.GroupID
				WHERE or1.OrganizationId = @ExistingOrgId AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly') AND 
			      g.Code = 'fbd_^authorizedstaff'))
			BEGIN
				RAISERROR('$Сохранение не выполнено. У указанной организации уже есть активированный УС ОУ для ФБД.', 18, 1)
				goto undo
			END			
			
			if @isOrganizationOwner = 1
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
				-- GVUZ-779 При изменении статуса пользователя статус заявления не меняется.
				--StatusID = @StatusID
			from 
				dbo.OrganizationRequest2010 OReq with (rowlock)
			where
				OReq.[Id] = @orgRequestID

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
				, Position = @position
				, Email = @email
				, ConfirmYear = @currentYear
				-- GVUZ-761 Статус УС ОУ, который имеет доступ только чтение, после оставления заявки на активацию - меняем на "Registration" 
				, [Status] = @status
				, IpAddresses = @ipAddresses
				, RegistrationDocument = @registrationDocument
				, RegistrationDocumentContentType = @registrationDocumentContentType
				, HasFixedIp = @hasFixedIp
				, HasCrocEgeIntegration = @hasCrocEgeIntegration
				, OrganizationId = @orgRequestID
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
			END
		end	
		
		-- устанавливаем информацию о привязке заявки, аккаунта и организации в OrganizationRequestAccount		
		DELETE FROM OrganizationRequestAccount WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId		
		DELETE FROM GroupAccount WHERE AccountId = @accountId AND GroupId IN (15, 3, 6, 7, 8, 9, 10, 11)		
			
		-- установка группы пользователя.
		IF exists(select * from @tableSystemId where SystemId = 3)
		BEGIN
			-- fbd_^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	15, @accountId
			if (@@error <> 0) goto undo
			
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 15)
			if (@@error <> 0) goto undo
													
			-- esrp^authorizedstaff
			insert dbo.GroupAccount(GroupId, AccountID)
			select	3, @accountId
			if (@@error <> 0) goto undo
				
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			VALUES (@orgRequestID, @accountId, 3)
			if (@@error <> 0) goto undo
			
			delete @tableSystemId where SystemId = 3								
		END	
		IF exists(select * from @tableSystemId where SystemId = 2)
		BEGIN
			-- ВУЗ
			IF(@organizationTypeId = 1)
			BEGIN
				-- fbs_^vuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	6, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 6)
				if (@@error <> 0) goto undo
			END
			-- ССУЗ
			ELSE IF(@organizationTypeId = 2)
			BEGIN
				-- fbs_^ssuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	7, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 7)
				if (@@error <> 0) goto undo						
			END
			-- РЦОИ
			ELSE IF(@organizationTypeId = 3)
			BEGIN
				-- fbs_^infoprocessing
				insert dbo.GroupAccount(GroupId, AccountID)
				select	8, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 8)
				if (@@error <> 0) goto undo						
			END
			-- Орган управления образованием
			ELSE IF(@organizationTypeId = 4)
			BEGIN
				-- fbs_^direction
				insert dbo.GroupAccount(GroupId, AccountID)
				select	9, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 9)
				if (@@error <> 0) goto undo						
			END
			-- Другое
			ELSE IF(@organizationTypeId = 5)
			BEGIN
				-- fbs_^other
				insert dbo.GroupAccount(GroupId, AccountID)				
				select	11, @accountId
				if (@@error <> 0) goto undo
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 11)
				if (@@error <> 0) goto undo						
			END
			-- Учредитель
			ELSE IF(@organizationTypeId = 6)
			BEGIN
				-- fbs_^founder
				insert dbo.GroupAccount(GroupId, AccountID)
				select	10, @accountId
				if (@@error <> 0) goto undo
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				VALUES (@orgRequestID, @accountId, 10)
				if (@@error <> 0) goto undo						
			END	
			
			delete @tableSystemId where SystemId = 3			
		END	
		
		IF exists(select * from @tableSystemId)
		begin
			insert dbo.GroupAccount(GroupId, AccountID)
			select	b.id, @accountId from @tableSystemId a join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			if (@@error <> 0) goto undo
					
			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			select @orgRequestID, @accountId,b.id from @tableSystemId a join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			if (@@error <> 0) goto undo			  
		end
								
	-- временно
	if isnull(@password, '') <> '' 
	begin
		if exists(select 1 
				from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId)
		BEGIN
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
	END
	
	-- GVUZ-761 оставляем в заявлении признак, если проводится активация пользователя
	-- флаг используется при выдаче шаблона документа для скана заявки	
	UPDATE OrganizationRequest2010
	SET IsForActivation = 1
	WHERE @accessToFbd = 1 AND @userStatusBefore = 'readonly' AND Id = @orgRequestID
	
	-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
	-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
	IF(@isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL)	
	BEGIN		
		IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
		BEGIN
			UPDATE OrganizationRequest2010 SET StatusID = 2 WHERE Id = @orgRequestID
		END
	END

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
PRINT N'Altering [dbo].[ReportUserStatusWithAccredTVF_New]...';


GO
ALTER function [dbo].[ReportUserStatusWithAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)



INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_OTHER()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего' OR [ ]='РЦОИ' OR [ ]='Орган управления образованием' OR [ ]='Другое'


INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON Org.Id=OReq.OrganizationId 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF_New (@periodBegin ,@periodEnd)

return
END
GO
PRINT N'Altering [dbo].[ReportCheckedCNEsDetailedTVF]...';


GO
ALTER function [dbo].[ReportCheckedCNEsDetailedTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Номер свидетельства] NVARCHAR(255)
,[Регион свидетельства] NVARCHAR(500)
,[Количество проверок] INT
,[Проверяющее ОУ] NVARCHAR(4000)
,[Тип ОУ/ОПФ ОУ] NVARCHAR(255)
,[Регион ОУ] NVARCHAR(500)
--,[Дата первой проверки] INT
)
AS 
BEGIN

DECLARE @BaseReport TABLE 
(
	CNENumber NVARCHAR(255),
	CNEId BIGINT,
	OrgId INT
)
INSERT INTO @BaseReport(CNENumber,CNEId,OrgId)
SELECT CNENumber,CNEId,OrgId FROM [ReportCheckedCNEsBASE]()

DECLARE @ChecksCount TABLE
(
	CNEId BIGINT,
	Checks INT
)
INSERT INTO @ChecksCount
SELECT 
	CNEId AS CNEId,
	COUNT(OrgId) AS OrgCount
FROM @BaseReport AS IChecks
GROUP BY IChecks.CNEId HAVING COUNT(IChecks.OrgId)>=6

DECLARE @ReportWithoutOrder TABLE
(
[Номер свидетельства] NVARCHAR(255)
,[Регион свидетельства] NVARCHAR(500)
,[Количество проверок] INT
,[Проверяющее ОУ] NVARCHAR(4000)
,[Тип ОУ/ОПФ ОУ] NVARCHAR(255)
,[Регион ОУ] NVARCHAR(500)
)

INSERT INTO @ReportWithoutOrder
SELECT 
IChecks.CNENumber AS [Номер свидетельства]
,IChecks.CNERegName AS [Регион свидетельства]
,IChecks.OrgCount AS [Количество проверок]
,IChecks.OrgName AS [Проверяющее ОУ]
,IChecks.OrgType+'/'+CASE WHEN IChecks.OPF=1 THEN 'Негосударственный' ELSE 'Государственный' END AS [Тип ОУ/ОПФ ОУ]
,IChecks.OrgRegName AS [Регион ОУ]
FROM 
(
	SELECT 
	CNENumber,
	Org.FullName AS OrgName,
	OrgType.Name AS OrgType,
	Org.IsPrivate AS OPF,
	OrgReg.Name AS OrgRegName,
	CNEReg.Name AS CNERegName,	
	CntRpt.Checks AS OrgCount
	FROM @ChecksCount CntRpt
	INNER JOIN @BaseReport AS Rpt ON CntRpt.CNEId=Rpt.CNEId
	INNER JOIN Organization2010 Org ON Org.Id=Rpt.OrgId
	INNER JOIN dbo.CommonNationalExamCertificate CNE ON CNE.Id=Rpt.CNEId
	INNER JOIN dbo.Region CNEReg ON CNEReg.Id=CNE.RegionId
	INNER JOIN dbo.Region OrgReg ON OrgReg.Id=Org.RegionId
	INNER JOIN dbo.OrganizationType2010 OrgType ON Org.TypeId=OrgType.Id
) AS IChecks

INSERT INTO @Report 
SELECT * FROM @ReportWithoutOrder
ORDER BY [Количество проверок] DESC,[Номер свидетельства]

RETURN
END
GO
PRINT N'Altering [dbo].[ReportCheckedCNEsTVF]...';


GO
ALTER function [dbo].[ReportCheckedCNEsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Номер свидетельства] NVARCHAR(255)
,[Проверок в различных регионах] INT
,[Проверок в различных ОУ] INT
,[Проверок в различных ВУЗах] INT
,[В негосударственных ВУЗах] INT
,[В государственных ВУЗах] INT
,[Проверок в различных ССУЗах] INT
,[В негосударственных ССУЗах] INT
,[В государственных ССУЗах] INT
)
AS 
BEGIN

INSERT INTO @Report
SELECT 
IChecks.CNENumber AS [Номер свидетельства]
,COUNT(DISTINCT IChecks.RegId) AS [Проверок в различных регионах]
,COUNT(*) AS [Проверок в различных ОУ] 
,COUNT(CASE WHEN IChecks.OrgType=1 THEN 1 ELSE NULL END) AS [Проверок в различных ВУЗах]
,COUNT(CASE WHEN IChecks.OrgType=1 AND IChecks.OPF=1 THEN 1 ELSE NULL END) AS [В негосударственных ВУЗах]
,COUNT(CASE  WHEN IChecks.OrgType=1 AND IChecks.OPF=0 THEN 1 ELSE NULL END) AS [В государственных ВУЗах]
,COUNT(CASE WHEN IChecks.OrgType=2 THEN 2 ELSE NULL END) AS [Проверок в различных ССУЗах]
,COUNT(CASE WHEN IChecks.OrgType=2 AND IChecks.OPF=1 THEN 1 ELSE NULL END) AS [В негосударственных ССУЗах]
,COUNT(CASE  WHEN IChecks.OrgType=2 AND IChecks.OPF=0 THEN 1 ELSE NULL END) AS [В государственных ССУЗах]
FROM 
(
	SELECT CNENumber ,OrgId,Org.TypeId AS OrgType,Org.IsPrivate AS OPF,Org.RegionId AS RegId  
	FROM [ReportCheckedCNEsBASE]() AS Rpt
	INNER JOIN Organization2010 Org ON Org.Id=Rpt.OrgId
) AS IChecks
GROUP BY IChecks.CNENumber HAVING COUNT(*)>=6


RETURN
END
GO
PRINT N'Altering [dbo].[ReportCommonStatisticsTVF]...';


GO
ALTER function [dbo].[ReportCommonStatisticsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Всего свидетельств] INT
,[Зарегистрировано пользователей] INT
,[Проверок всего] INT
,[Уникальных проверок] INT
,[Уникальных пакетных проверок] INT
,[Уникальных интерактивных проверок] INT
)
AS 
BEGIN

DECLARE @CNEsCount INT
SELECT @CNEsCount=COUNT(*) 
FROM CommonNationalExamCertificate

DECLARE @UsersCount INT
SELECT @UsersCount=COUNT(*) 
FROM Account Acc
INNER JOIN GroupAccount GA ON Acc.Id=GA.AccountId AND GA.GroupId=1
INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=Acc.OrganizationId AND OReq.OrganizationId IS NOT NULL

DECLARE @TotalChecks INT
DECLARE @TotalUniqueChecks INT

SELECT @TotalChecks=SUM([всего])
,@TotalUniqueChecks=SUM([всего уникальных проверок])
FROM ReportTotalChecksTVF(null,null)


DECLARE @UniqueChecks_Batch INT

SELECT @UniqueChecks_Batch=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]='Пакетная'


DECLARE @UniqueChecks_UI INT

SELECT @UniqueChecks_UI=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]='Интерактивная'


INSERT INTO @Report
SELECT @CNEsCount,@UsersCount,@TotalChecks,@TotalUniqueChecks,@UniqueChecks_Batch,@UniqueChecks_UI

RETURN
END
GO

