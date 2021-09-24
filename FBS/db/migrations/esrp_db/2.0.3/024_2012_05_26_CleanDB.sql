-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (24, '024_2012_05_26_CleanDB.sql')
-- =========================================================================
	
GO
PRINT N'Dropping [dbo].[tgCheckCommonNationalExamCertificateLog]...';


GO
DROP TRIGGER [dbo].[tgCheckCommonNationalExamCertificateLog];


GO
PRINT N'Dropping [dbo].[tgCommonNationalExamCertificateCheck]...';

GO
DROP TRIGGER [dbo].[tgCommonNationalExamCertificateCheck];


GO
PRINT N'Dropping FK__CNEWebUIC__Accou__62015D92...';


GO
ALTER TABLE [dbo].[CNEWebUICheckLog] DROP CONSTRAINT [FK__CNEWebUIC__Accou__62015D92];


GO
PRINT N'Dropping PK_ExpireDate...';


GO
ALTER TABLE [dbo].[ExpireDate] DROP CONSTRAINT [PK_ExpireDate];


GO
PRINT N'Dropping [dbo].[BatchGUID]...';


GO
DROP TABLE [dbo].[BatchGUID];

GO
DROP TABLE dbo.ExpireDate

GO
PRINT N'Dropping [dbo].[CheckCommonNationalExamCertificateLog]... ';


GO
DROP TABLE [dbo].[CheckCommonNationalExamCertificateLog];


GO
PRINT N'Dropping [dbo].[CNEWebUICheckLog]...';


GO
DROP TABLE [dbo].[CNEWebUICheckLog];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificate]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateCheck]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateCheck];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateCheckBatch]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateCheckLog]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateCheckLog];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateDeny]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateDeny];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateDenyLoadingTask]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateDenyLoadingTask];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateDenyLoadingTaskError]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateDenyLoadingTaskError];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateForm]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateForm];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateFormNumberRange]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateFormNumberRange];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateLoadingTask]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateLoadingTask];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateLoadingTaskError]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateLoadingTaskError];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateRequest]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateRequest];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateRequestBatch]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubject]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubject];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubjectCheck]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubjectCheck];


GO
PRINT N'Dropping [dbo].[CommonNationalExamCertificateSubjectForm]...';


GO
DROP TABLE [dbo].[CommonNationalExamCertificateSubjectForm];


GO
PRINT N'Dropping [dbo].[CompetitionCertificateRequestBatch]...';


GO
DROP TABLE [dbo].[CompetitionCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[DeprecatedCommonNationalExamCertificateSubject]...';


GO
DROP TABLE [dbo].[DeprecatedCommonNationalExamCertificateSubject];


GO
PRINT N'Dropping [dbo].[Entrant]...';


GO
DROP TABLE [dbo].[Entrant];


GO
PRINT N'Dropping [dbo].[EntrantCheckBatch]...';


GO
DROP TABLE [dbo].[EntrantCheckBatch];


GO
PRINT N'Dropping [dbo].[EntrantRenunciation]...';


GO
DROP TABLE [dbo].[EntrantRenunciation];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificate]...';


GO
DROP TABLE [dbo].[ImportingCommonNationalExamCertificate];


GO
PRINT N'Dropping [dbo].[ImportingCommonNationalExamCertificateSubject]...';


GO
DROP TABLE [dbo].[ImportingCommonNationalExamCertificateSubject];


GO
PRINT N'Dropping [dbo].[MinimalMark]...';


GO
DROP TABLE [dbo].[MinimalMark];


GO
PRINT N'Dropping [dbo].[OrganizationCertificateChecks]...';


GO
DROP TABLE [dbo].[OrganizationCertificateChecks];


GO
PRINT N'Dropping [dbo].[SchoolLeavingCertificateCheckBatch]...';


GO
DROP TABLE [dbo].[SchoolLeavingCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[Subject]...';


GO
DROP TABLE [dbo].[Subject];


GO
PRINT N'Dropping [dbo].[ReportCertificateLoadTVF]...';


GO
DROP FUNCTION [dbo].[ReportCertificateLoadTVF];

GO

PRINT N'Dropping [dbo].[ReportCertificateLoadTVF]...';
GO

DROP FUNCTION ReportCommonStatisticsTVF

GO
PRINT N'Dropping [dbo].[ReportCheckedCNEsAggregatedTVF]...';


GO
DROP FUNCTION [dbo].[ReportCheckedCNEsAggregatedTVF];


GO
PRINT N'Dropping [dbo].[ReportCheckedCNEsBASE]...';


GO
DROP FUNCTION [dbo].[ReportCheckedCNEsBASE];


GO
PRINT N'Dropping [dbo].[ReportCheckedCNEsDetailedTVF]...';


GO
DROP FUNCTION [dbo].[ReportCheckedCNEsDetailedTVF];


GO
PRINT N'Dropping [dbo].[ReportCheckedCNEsTVF]...';


GO
DROP FUNCTION [dbo].[ReportCheckedCNEsTVF];


GO
PRINT N'Dropping [dbo].[ReportChecksAllTVF]...';


GO
DROP FUNCTION [dbo].[ReportChecksAllTVF];


GO
PRINT N'Dropping [dbo].[ReportChecksByOrgsTVF]...';


GO
DROP FUNCTION [dbo].[ReportChecksByOrgsTVF];


GO
PRINT N'Dropping [dbo].[ReportChecksByPeriodTVF]...';


GO
DROP FUNCTION [dbo].[ReportChecksByPeriodTVF];


GO
PRINT N'Dropping [dbo].[ReportCheckStatisticsTVF]...';


GO
DROP FUNCTION [dbo].[ReportCheckStatisticsTVF];


GO
PRINT N'Dropping [dbo].[CheckCommonNationalExamCertificateByNumber]...';


GO
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByNumber];


GO
PRINT N'Dropping [dbo].[ExecuteChecksCount]...';


GO
DROP PROCEDURE [dbo].[ExecuteChecksCount];


GO
PRINT N'Dropping [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]...';


GO
DROP PROCEDURE [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask];


GO
PRINT N'Dropping [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]...';


GO
DROP PROCEDURE [dbo].[ExecuteCommonNationalExamCertificateLoadingTask];


GO
PRINT N'Dropping [dbo].[GetBatchStatusById]...';


GO
DROP PROCEDURE [dbo].[GetBatchStatusById];


GO
PRINT N'Dropping [dbo].[GetSubject]...';


GO
DROP PROCEDURE [dbo].[GetSubject];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateCheck]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheck];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateLoadingTask]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateLoadingTask];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateLoadingTaskError];


GO
PRINT N'Dropping [dbo].[SearchCommonNationalExamCertificateRequest]...';


GO
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequest];


GO
PRINT N'Dropping [dbo].[SearchMinimalMark]...';


GO
DROP PROCEDURE [dbo].[SearchMinimalMark];


GO
PRINT N'Dropping [dbo].[UpdateCommonNationalExamCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[UpdateCommonNationalExamCertificateCheckBatch];


GO
PRINT N'Dropping [dbo].[UpdateCommonNationalExamCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[UpdateCommonNationalExamCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[UpdateCompetitionCertificateRequestBatch]...';


GO
DROP PROCEDURE [dbo].[UpdateCompetitionCertificateRequestBatch];


GO
PRINT N'Dropping [dbo].[UpdateEntrant]...';


GO
DROP PROCEDURE [dbo].[UpdateEntrant];


GO
PRINT N'Dropping [dbo].[UpdateEntrantCheckBatch]...';


GO
DROP PROCEDURE [dbo].[UpdateEntrantCheckBatch];


GO
PRINT N'Dropping [dbo].[UpdateEntrantRenunciation]...';


GO
DROP PROCEDURE [dbo].[UpdateEntrantRenunciation];


GO
PRINT N'Dropping [dbo].[UpdateExpireDates]...';


GO
DROP PROCEDURE [dbo].[UpdateExpireDates];


GO
PRINT N'Dropping [dbo].[UpdateMinimalMarks]...';


GO
DROP PROCEDURE [dbo].[UpdateMinimalMarks];


GO
PRINT N'Dropping [dbo].[UpdateSchoolLeavingCertificateCheckBatch]...';


GO
DROP PROCEDURE [dbo].[UpdateSchoolLeavingCertificateCheckBatch];

GO
PRINT N'Dropping [dbo].[ReportOrgsInfoByRegionTVF]...';


GO
DROP FUNCTION [dbo].[ReportOrgsInfoByRegionTVF];


GO
PRINT N'Dropping [dbo].[ReportOrgsInfoTVF]...';


GO
DROP FUNCTION [dbo].[ReportOrgsInfoTVF];


GO
PRINT N'Dropping [dbo].[ReportPotentialAbusersTVF]...';


GO
DROP FUNCTION [dbo].[ReportPotentialAbusersTVF];

GO
PRINT N'Dropping [dbo].[ReportTotalChecksTVF]...';


GO
DROP FUNCTION [dbo].[ReportTotalChecksTVF];


GO
PRINT N'Dropping [dbo].[ReportTotalChecksTVF_New]...';


GO
DROP FUNCTION [dbo].[ReportTotalChecksTVF_New];

GO
PRINT N'Dropping [dbo].[AddCNEWebUICheckEvent]...';


GO
DROP PROCEDURE [dbo].[AddCNEWebUICheckEvent];


GO
PRINT N'Dropping [dbo].[ClearDataBase]...';


GO
DROP PROCEDURE [dbo].[ClearDataBase];


GO
PRINT N'Dropping [dbo].[GetEducationalOrganizationReport]...';


GO
DROP PROCEDURE [dbo].[GetEducationalOrganizationReport];


GO
PRINT N'Dropping [dbo].[GetUserAccountActivityByRegionReport]...';


GO
DROP PROCEDURE [dbo].[GetUserAccountActivityByRegionReport];


GO
PRINT N'Dropping [dbo].[SearchExpireDate]...';


GO
DROP PROCEDURE [dbo].[SearchExpireDate];


GO
PRINT N'Dropping [dbo].[SearchSameUserAccount]...';


GO
DROP PROCEDURE [dbo].[SearchSameUserAccount];


GO
PRINT N'Dropping [dbo].[SearchVUZ]...';


GO
DROP PROCEDURE [dbo].[SearchVUZ];


GO


