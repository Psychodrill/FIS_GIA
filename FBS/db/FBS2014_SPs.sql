USE [FBS_2015_Debug]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF_New]    Script Date: 05/07/2015 18:13:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusWithAccredTVF_New]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportUserStatusWithAccredTVF_New]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportXMLSubordinateOrg]    Script Date: 05/07/2015 18:13:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportXMLSubordinateOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportXMLSubordinateOrg]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCommonStatisticsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCommonStatisticsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCommonStatisticsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTotalChecksTVF_New]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportTotalChecksTVF_New]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportTotalChecksTVF_New]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF_New]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusAccredTVF_New]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportUserStatusAccredTVF_New]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsDetailedTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsDetailedTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCheckedCNEsDetailedTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCheckedCNEsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistrationShortTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportRegistrationShortTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportRegistrationShortTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsAggregatedTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsAggregatedTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCheckedCNEsAggregatedTVF]
GO
/****** Object:  StoredProcedure [dbo].[GetRemindAccount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRemindAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetRemindAccount]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificate]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificate]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusWithAccredTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportUserStatusWithAccredTVF]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificatePassport]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificatePassport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificatePassport]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateWildcard]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateWildcard]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateWildcard]
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificate]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionCertificate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCompetitionCertificate]
GO
/****** Object:  StoredProcedure [dbo].[SetActiveAskedQuestion]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetActiveAskedQuestion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetActiveAskedQuestion]
GO
/****** Object:  StoredProcedure [dbo].[SetActiveDocument]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetActiveDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetActiveDocument]
GO
/****** Object:  StoredProcedure [dbo].[SetActiveNews]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetActiveNews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetActiveNews]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountKey]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccountKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateAccountKey]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountPassword]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccountPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateAccountPassword]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAskedQuestion]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAskedQuestion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateAskedQuestion]
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCommonNationalExamCertificateCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCommonNationalExamCertificateRequestBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateCommonNationalExamCertificateRequestBatch]
GO
/****** Object:  StoredProcedure [dbo].[UpdateCompetitionCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCompetitionCertificateRequestBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateCompetitionCertificateRequestBatch]
GO
/****** Object:  StoredProcedure [dbo].[UpdateDelivery]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDelivery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDelivery]
GO
/****** Object:  StoredProcedure [dbo].[UpdateDocument]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateDocument]
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrant]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntrant]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateEntrant]
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrantCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntrantCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateEntrantCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrantRenunciation]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntrantRenunciation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateEntrantRenunciation]
GO
/****** Object:  StoredProcedure [dbo].[UpdateNews]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateNews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateNews]
GO
/****** Object:  StoredProcedure [dbo].[UpdateSchoolLeavingCertificateCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSchoolLeavingCertificateCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateSchoolLeavingCertificateCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccount]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserAccount]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountRegistrationDocument]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccountRegistrationDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserAccountRegistrationDocument]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountStatus]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccountStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateUserAccountStatus]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccount]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateAccount]
GO
/****** Object:  StoredProcedure [dbo].[VerifyAccount]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VerifyAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[VerifyAccount]
GO
/****** Object:  StoredProcedure [dbo].[CheckAccountKey]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckAccountKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckAccountKey]
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumber]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByNumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByNumber]
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumber_2]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByNumber_2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByNumber_2]
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumberForXml]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByNumberForXml]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByPassport_2]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByPassport_2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByPassport_2]
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByPassportForXml]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByPassportForXml]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckCommonNationalExamCertificateByPassportForXml]
GO
/****** Object:  StoredProcedure [dbo].[CheckEntrant]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckEntrant]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckEntrant]
GO
/****** Object:  StoredProcedure [dbo].[CheckLastAccountIp]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckLastAccountIp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckLastAccountIp]
GO
/****** Object:  StoredProcedure [dbo].[CheckSchoolLeavingCertificate]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSchoolLeavingCertificate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckSchoolLeavingCertificate]
GO
/****** Object:  StoredProcedure [dbo].[DeleteAskedQuestion]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAskedQuestion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAskedQuestion]
GO
/****** Object:  StoredProcedure [dbo].[DeleteDeliveries]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDeliveries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteDeliveries]
GO
/****** Object:  StoredProcedure [dbo].[DeleteDocument]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteDocument]
GO
/****** Object:  StoredProcedure [dbo].[DeleteNews]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteNews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteNews]
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecuteCommonNationalExamCertificateLoadingTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountActivityByRegionReport]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccountActivityByRegionReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserAccountActivityByRegionReport]
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountByRegionReport]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccountByRegionReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserAccountByRegionReport]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryPaged]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsHistoryPaged]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryPaged_Obsolete]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryPaged_Obsolete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsHistoryPaged_Obsolete]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryPagesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsHistoryPagesCount]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryPaged]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckHistoryPaged]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryPagesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckHistoryPagesCount]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsGroupMarks]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsGroupMarks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsGroupMarks]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsGroupsPaged]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsGroupsPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsGroupsPaged]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsGroupsPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsGroupsPagesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsGroupsPagesCount]
GO
/****** Object:  StoredProcedure [dbo].[DeleteBatchCheckHistoryById]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteBatchCheckHistoryById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteBatchCheckHistoryById]
GO
/****** Object:  StoredProcedure [dbo].[BatchProcess_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchProcess_FioDocumentNumberSeries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BatchProcess_FioDocumentNumberSeries]
GO
/****** Object:  StoredProcedure [dbo].[BatchProcess_LicenseNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchProcess_LicenseNumberFio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BatchProcess_LicenseNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[BatchProcess_TypographicNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchProcess_TypographicNumberFio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BatchProcess_TypographicNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[ClearDataBase]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClearDataBase]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ClearDataBase]
GO
/****** Object:  StoredProcedure [dbo].[_Check_Applicant]    Script Date: 05/07/2015 18:13:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Check_Applicant]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[_Check_Applicant]
GO
/****** Object:  StoredProcedure [dbo].[BanOrgs]    Script Date: 05/07/2015 18:13:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanOrgs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BanOrgs]
GO
/****** Object:  StoredProcedure [dbo].[CalculateUniqueChecksByBatchId]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateUniqueChecksByBatchId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CalculateUniqueChecksByBatchId]
GO
/****** Object:  StoredProcedure [dbo].[usp_cne_AddCheckBatchResult]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_cne_AddCheckBatchResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_cne_AddCheckBatchResult]
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_FioDocumentNumberSeries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SingleCheck_FioDocumentNumberSeries]
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_FioSubjectsMarks]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_FioSubjectsMarks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SingleCheck_FioSubjectsMarks]
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_LicenseNumberFio]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_LicenseNumberFio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SingleCheck_LicenseNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_TypographicNumberFio]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_TypographicNumberFio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SingleCheck_TypographicNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_Wildcard]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_Wildcard]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SingleCheck_Wildcard]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckByOuterId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckByOuterId_tmp1]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckByOuterId_tmp1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckByOuterId_tmp1]
GO
/****** Object:  StoredProcedure [dbo].[GetOrganizationTypeReport]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationTypeReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrganizationTypeReport]
GO
/****** Object:  StoredProcedure [dbo].[GetLoginAttemptsInfo]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLoginAttemptsInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetLoginAttemptsInfo]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsBASE]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsBASE]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCheckedCNEsBASE]
GO
/****** Object:  StoredProcedure [dbo].[RegisterEvent]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegisterEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RegisterEvent]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistredOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportRegistredOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportRegistredOrgsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportStatisticSubordinateOrg]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportStatisticSubordinateOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportStatisticSubordinateOrg]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTopCheckingOrganizationsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportTopCheckingOrganizationsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportTopCheckingOrganizationsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTotalChecksTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportTotalChecksTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportTotalChecksTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksAllTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportChecksAllTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportChecksAllTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksByOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportChecksByOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportChecksByOrgsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksByPeriodTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportChecksByPeriodTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportChecksByPeriodTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckStatisticsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckStatisticsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCheckStatisticsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckStatisticsTVFOpen]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckStatisticsTVFOpen]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCheckStatisticsTVFOpen]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportNotRegistredOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportNotRegistredOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportNotRegistredOrgsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_OTHER]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_OTHER]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgActivation_OTHER]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_SSUZ]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgActivation_SSUZ]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ_Accred]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_SSUZ_Accred]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgActivation_SSUZ_Accred]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_VUZ]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgActivation_VUZ]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ_Accred]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_VUZ_Accred]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgActivation_VUZ_Accred]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgErrorRequests]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgErrorRequests]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgErrorRequests]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgRequests]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgRequests]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgRequests]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportUserStatusTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusAccredTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportUserStatusAccredTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoByRegionTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgsInfoByRegionTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgsInfoByRegionTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgsInfoTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgsInfoTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportPotentialAbusersTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportPotentialAbusersTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportPotentialAbusersTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportEditedOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportEditedOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportEditedOrgsTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsBASE]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgsBASE]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportOrgsBASE]
GO
/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNEWebUICheckLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNEWebUICheckLog]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryOrgPaged]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryOrgPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckHistoryOrgPaged]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryOrgPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryOrgPagesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckHistoryOrgPagesCount]
GO
/****** Object:  UserDefinedFunction [dbo].[GetOrganizationAccountsIds]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationAccountsIds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetOrganizationAccountsIds]
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserAccount]
GO
/****** Object:  StoredProcedure [dbo].[GetSubject]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetSubject]
GO
/****** Object:  UserDefinedFunction [dbo].[IsUserOrgLogDisabled]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsUserOrgLogDisabled]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IsUserOrgLogDisabled]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckHistory]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckHistory]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheck]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheck]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheck]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequest]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequest]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequest_test]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequest_test]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequest_test]
GO
/****** Object:  StoredProcedure [dbo].[SearchMinimalMark]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchMinimalMark]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchMinimalMark]
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountEsrp]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccountEsrp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateAccountEsrp]
GO
/****** Object:  StoredProcedure [dbo].[UpdateGroupUserEsrp]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateGroupUserEsrp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateGroupUserEsrp]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]
GO
/****** Object:  StoredProcedure [dbo].[BatchCheck_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchCheck_FioDocumentNumberSeries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BatchCheck_FioDocumentNumberSeries]
GO
/****** Object:  StoredProcedure [dbo].[BatchCheck_LicenseNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchCheck_LicenseNumberFio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BatchCheck_LicenseNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[BatchCheck_TypographicNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchCheck_TypographicNumberFio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BatchCheck_TypographicNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[AddCNEWebUICheckEvent]    Script Date: 05/07/2015 18:13:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddCNEWebUICheckEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AddCNEWebUICheckEvent]
GO
/****** Object:  StoredProcedure [dbo].[CheckOlympicResults]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckOlympicResults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckOlympicResults]
GO
/****** Object:  StoredProcedure [dbo].[ExecuteChecksCount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecuteChecksCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExecuteChecksCount]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryOrgPaged]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryOrgPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsHistoryOrgPaged]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryOrgPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryOrgPagesCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCheckResultsHistoryOrgPagesCount]
GO
/****** Object:  StoredProcedure [dbo].[GetBatchCheckReady]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBatchCheckReady]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBatchCheckReady]
GO
/****** Object:  StoredProcedure [dbo].[GetDeliveryRecipients]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDeliveryRecipients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDeliveryRecipients]
GO
/****** Object:  StoredProcedure [dbo].[GetDocument]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDocument]
GO
/****** Object:  StoredProcedure [dbo].[GetBatchStatusById]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBatchStatusById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetBatchStatusById]
GO
/****** Object:  StoredProcedure [dbo].[GetCertificateByFioAndPassport]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCertificateByFioAndPassport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCertificateByFioAndPassport]
GO
/****** Object:  StoredProcedure [dbo].[CheckUserAccountEmail]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckUserAccountEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckUserAccountEmail]
GO
/****** Object:  StoredProcedure [dbo].[GetAccount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAccount]
GO
/****** Object:  StoredProcedure [dbo].[GetAccountGroup]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAccountGroup]
GO
/****** Object:  StoredProcedure [dbo].[GetAccountKey]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAccountKey]
GO
/****** Object:  StoredProcedure [dbo].[GetAccountLog]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAccountLog]
GO
/****** Object:  StoredProcedure [dbo].[GetAccountRole]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAccountRole]
GO
/****** Object:  StoredProcedure [dbo].[GetAskedQuestion]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAskedQuestion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAskedQuestion]
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountLog]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccountLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserAccountLog]
GO
/****** Object:  StoredProcedure [dbo].[CheckNewLogin]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckNewLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckNewLogin]
GO
/****** Object:  StoredProcedure [dbo].[CheckNewUserAccountEmail]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckNewUserAccountEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckNewUserAccountEmail]
GO
/****** Object:  StoredProcedure [dbo].[CommonNationalExamCertificateSumCheckResult]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateSumCheckResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CommonNationalExamCertificateSumCheckResult]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]
GO
/****** Object:  StoredProcedure [dbo].[SearchEntrantCheck]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEntrantCheck]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchEntrantCheck]
GO
/****** Object:  StoredProcedure [dbo].[SearchEntrantCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEntrantCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchEntrantCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[SearchRegion]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchRegion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchRegion]
GO
/****** Object:  StoredProcedure [dbo].[SearchSameUserAccount]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchSameUserAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchSameUserAccount]
GO
/****** Object:  StoredProcedure [dbo].[SearchSchoolLeavingCertificateCheck]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchSchoolLeavingCertificateCheck]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchSchoolLeavingCertificateCheck]
GO
/****** Object:  StoredProcedure [dbo].[SearchSchoolLeavingCertificateCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchSchoolLeavingCertificateCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchSchoolLeavingCertificateCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificateRequest]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionCertificateRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCompetitionCertificateRequest]
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionCertificateRequestBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCompetitionCertificateRequestBatch]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequestBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateRequestBatch]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateLoadingTaskError]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckBatch]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]
GO
/****** Object:  StoredProcedure [dbo].[RefreshRoleActivity]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshRoleActivity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RefreshRoleActivity]
GO
/****** Object:  StoredProcedure [dbo].[DeleteAccount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteAccount]
GO
/****** Object:  StoredProcedure [dbo].[GetDelivery]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDelivery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDelivery]
GO
/****** Object:  StoredProcedure [dbo].[GetNews]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNews]
GO
/****** Object:  StoredProcedure [dbo].[GetNewUserLogin]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewUserLogin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNewUserLogin]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCertificateLoadShortTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCertificateLoadShortTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCertificateLoadShortTVF]
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCertificateLoadTVF]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCertificateLoadTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ReportCertificateLoadTVF]
GO
/****** Object:  StoredProcedure [dbo].[ReportUserRegistration]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserRegistration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReportUserRegistration]
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountKey]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAccountKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchAccountKey]
GO
/****** Object:  UserDefinedFunction [dbo].[IsUserBanned]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsUserBanned]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[IsUserBanned]
GO
/****** Object:  StoredProcedure [dbo].[Operator_AddUserComment]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operator_AddUserComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Operator_AddUserComment]
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetNewUser]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operator_GetNewUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Operator_GetNewUser]
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetUserInfo]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operator_GetUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Operator_GetUserInfo]
GO
/****** Object:  StoredProcedure [dbo].[SearchAccount]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchAccount]
GO
/****** Object:  StoredProcedure [dbo].[ReportCnecLoading]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCnecLoading]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ReportCnecLoading]
GO
/****** Object:  UserDefinedFunction [dbo].[GetShortOrganizationName]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetShortOrganizationName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetShortOrganizationName]
GO
/****** Object:  StoredProcedure [dbo].[UpdateExpireDates]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExpireDates]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateExpireDates]
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionType]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCompetitionType]
GO
/****** Object:  StoredProcedure [dbo].[SearchContext]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchContext]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchContext]
GO
/****** Object:  StoredProcedure [dbo].[SearchExpireDate]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchExpireDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchExpireDate]
GO
/****** Object:  StoredProcedure [dbo].[WriteToBatchProcessLog]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WriteToBatchProcessLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[WriteToBatchProcessLog]
GO
/****** Object:  StoredProcedure [dbo].[UpdateMinimalMarks]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMinimalMarks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateMinimalMarks]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_FioDocumentNumberSeries]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Parse_FioDocumentNumberSeries]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_LicenseNumberFio]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_LicenseNumberFio]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Parse_LicenseNumberFio]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_TypographicNumberFio]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_TypographicNumberFio]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Parse_TypographicNumberFio]
GO
/****** Object:  StoredProcedure [dbo].[dba_indexDefragStandard_sp]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dba_indexDefragStandard_sp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[dba_indexDefragStandard_sp]
GO
/****** Object:  StoredProcedure [dbo].[GetDocumentByUrl]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDocumentByUrl]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetDocumentByUrl]
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalId]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInternalId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetInternalId]
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalPassportSeria]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInternalPassportSeria]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetInternalPassportSeria]
GO
/****** Object:  UserDefinedFunction [dbo].[GetEventParam]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEventParam]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetEventParam]
GO
/****** Object:  UserDefinedFunction [dbo].[GetExternalId]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExternalId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetExternalId]
GO
/****** Object:  UserDefinedFunction [dbo].[GetCommonNationalExamCertificateActuality]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCommonNationalExamCertificateActuality]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetCommonNationalExamCertificateActuality]
GO
/****** Object:  UserDefinedFunction [dbo].[GetDelimitedValues]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDelimitedValues]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetDelimitedValues]
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserIsActive]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserIsActive]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetUserIsActive]
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatus]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStatus]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetUserStatus]
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatusOrder]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStatusOrder]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetUserStatusOrder]
GO
/****** Object:  UserDefinedFunction [dbo].[HasUserAccountAdminComment]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HasUserAccountAdminComment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[HasUserAccountAdminComment]
GO
/****** Object:  UserDefinedFunction [dbo].[CompareStrings]    Script Date: 05/07/2015 18:13:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompareStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CompareStrings]
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccount]    Script Date: 05/07/2015 18:13:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanEditUserAccount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanEditUserAccount]
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccountRegistrationDocument]    Script Date: 05/07/2015 18:13:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanEditUserAccountRegistrationDocument]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanEditUserAccountRegistrationDocument]
GO
/****** Object:  UserDefinedFunction [dbo].[CanViewUserAccountRegistrationDocument]    Script Date: 05/07/2015 18:13:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanViewUserAccountRegistrationDocument]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CanViewUserAccountRegistrationDocument]
GO
/****** Object:  UserDefinedFunction [dbo].[CapitalizeFirstLetter]    Script Date: 05/07/2015 18:13:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CapitalizeFirstLetter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CapitalizeFirstLetter]
GO
/****** Object:  StoredProcedure [dbo].[usp_ut_lock2]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ut_lock2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_ut_lock2]
GO
/****** Object:  StoredProcedure [dbo].[SearchNews]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchNews]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchNews]
GO
/****** Object:  StoredProcedure [dbo].[SearchVUZ]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchVUZ]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchVUZ]
GO
/****** Object:  StoredProcedure [dbo].[SelectCNECCheckHystoryByOrgId]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectCNECCheckHystoryByOrgId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SelectCNECCheckHystoryByOrgId]
GO
/****** Object:  StoredProcedure [dbo].[SelectCNECCheckHystoryByOrgIdCount]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectCNECCheckHystoryByOrgIdCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SelectCNECCheckHystoryByOrgIdCount]
GO
/****** Object:  StoredProcedure [dbo].[SearchDeliveries]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchDeliveries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchDeliveries]
GO
/****** Object:  StoredProcedure [dbo].[SearchDocument]    Script Date: 05/07/2015 18:13:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchDocument]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateLoadingTask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateLoadingTask]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromString]    Script Date: 05/07/2015 18:13:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_ut_SplitFromString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ufn_ut_SplitFromString]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromStringWithId]    Script Date: 05/07/2015 18:13:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_ut_SplitFromStringWithId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[ufn_ut_SplitFromStringWithId]
GO
/****** Object:  UserDefinedFunction [dbo].[GetSubjectMarks]    Script Date: 05/07/2015 18:13:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubjectMarks]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetSubjectMarks]
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountAuthenticationLog]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAccountAuthenticationLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchAccountAuthenticationLog]
GO
/****** Object:  StoredProcedure [dbo].[SearchAskedQuestion]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAskedQuestion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchAskedQuestion]
GO
/****** Object:  StoredProcedure [dbo].[RemoveInjections]    Script Date: 05/07/2015 18:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveInjections]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RemoveInjections]
GO
/****** Object:  StoredProcedure [dbo].[RemoveInjections]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveInjections]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[RemoveInjections] (@text varchar(255))
AS
BEGIN
	SELECT REPLACE(	     
	     REPLACE(@text, '''''''', ''''), ''--'', '''')
	RETURN 0
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchAskedQuestion]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAskedQuestion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchAskedQuestion

-- =============================================
-- Получение списка вопросов.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc [dbo].[SearchAskedQuestion]
  @name nvarchar(255) = null
  , @isActive bit = null
  , @contextCodes nvarchar(4000) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20)
  , @sortAsc bit = 1
  , @showCount bit = null
as
begin
  declare 
    @nameFormat nvarchar(255)
    , @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)

  if isnull(@name, '''') <> ''''
    set @nameFormat = ''%'' + replace(@name, '' '', ''%'') + ''%''

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Id bigint '' +
      '' , Name nvarchar(255) '' +
      '' , Question ntext '' +
      '' , IsActive bit '' +
      '' , Popularity decimal(18,4) '' +
      '' ) '' 

  if isnull(@contextCodes, '''') <> ''''
    set @declareCommandText = @declareCommandText + 
      ''declare @codes table ''+
      '' ( '' +
      '' Code nvarchar(255) '' +
      '' ) '' +
      ''insert @codes select value from dbo.GetDelimitedValues(@contextCodes) ''

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' asked_question.Id Id '' +
        '' , asked_question.Name Name '' +
        '' , asked_question.Question Question '' +
        '' , asked_question.IsActive IsActive '' +
        '' , asked_question.Popularity Popularity '' +
        ''from dbo.AskedQuestion asked_question with (nolock) '' +
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.AskedQuestion asked_question with (nolock) '' +
        ''where 1 = 1 '' 

  if not @nameFormat is null  
    set @commandText = @commandText + '' and asked_question.Name like @nameFormat ''

  if not @isActive is null
    set @commandText = @commandText + '' and asked_question.IsActive = @isActive ''

  if not @contextCodes is null
    set @commandText = @commandText + '' and not exists(select 1 '' +
        ''   from @codes context_codes '' +
        ''     inner join dbo.Context context '' +
        ''       on context.Code = context_codes.Code '' +
        ''     left outer join dbo.AskedQuestionContext asked_question_context with(nolock) '' +
        ''       on asked_question_context.ContextId = context.Id '' +
        ''         and asked_question_context.AskedQuestionId = asked_question.Id '' +
        ''   where asked_question_context.Id is null) ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = ''Name''
    begin
      set @innerOrder = ''order by Name <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Name <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Name <orderDirection>, Id <orderDirection> ''
    end
    else if @sortColumn = ''IsActive''
    begin
      set @innerOrder = ''order by IsActive <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by IsActive <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by IsActive <orderDirection>, Id <orderDirection> ''
    end
    else
    begin
      set @innerOrder = ''order by Popularity <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Popularity <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Popularity <orderDirection>, Id <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' dbo.GetExternalId(search.Id) Id '' +
      '' , search.Name '' +
      '' , search.Question '' +
      '' , search.IsActive '' +
      '' , search.Popularity '' +
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@nameFormat nvarchar(255), @isActive bit, @contextCodes nvarchar(4000)''
    , @nameFormat
    , @IsActive
    , @contextCodes

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountAuthenticationLog]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAccountAuthenticationLog]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchAccountAuthenticationLog
-- =============================================
-- Поиск в логе аутентификации записей об аккаунте
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- v.1.1: Modified by Fomin Dmitriy 20.05.2008
-- Добавлено поле IsVpnIp.
-- v.1.2: Modified by Fomin Dmitriy 20.05.2008
-- Анонимное событие на регистрацию проводит 
-- неявную аутентификацию.
-- v.1.3: Modified by Sedov A.G. 22.05.2008
-- Переделана выборка данных, выборка теперь 
-- выполняется из dbo.AuthenticationEventLog 
-- =============================================
CREATE procedure [dbo].[SearchAccountAuthenticationLog]
  @login nvarchar(255)
  , @startRowIndex int = null 
  , @maxRowCount int = null 
  , @showCount bit = null 
as
begin
  declare
    @declareCommandText nvarchar(4000)
    , @params nvarchar(4000)
    , @commandText nvarchar(4000) 
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortAsc bit
    , @verifyEventCode nvarchar(255)
    , @registrationEventCode nvarchar(255)

  set @verifyEventCode = ''USR_VERIFY''
  set @registrationEventCode = ''USR_REG''

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = '''' 

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Date datetime '' +
      '' , Ip nvarchar(255) '' +
      '' , IsPasswordValid bit '' +
      '' , IsIpValid bit '' +
      '' ) '' 
  
  
  if isnull(@showCount, 0) = 0
    set @commandText = 
      ''select <innerHeader> '' +
      '' auth_log.Date Date '' +
      '' , auth_log.Ip Ip '' + 
      ''   , auth_log.IsPasswordValid '' + 
      '' , auth_log.IsIpValid '' + 
      ''from '' + 
      '' dbo.AuthenticationEventLog auth_log with (nolock) '' + 
      ''   inner join dbo.Account account with (nolock, fastfirstrow) '' + 
      ''     on account.Id = auth_log.AccountId '' + 
      ''where 1 = 1 '' 
  else
    set @commandText = 
      ''select count(*) '' +
      ''from '' + 
      '' dbo.AuthenticationEventLog auth_log with (nolock) '' +
      ''   inner join dbo.Account account with (nolock, fastfirstrow) '' +
      ''     on account.Id = auth_log.AccountId '' +
      ''where 1 = 1 ''

  set @commandText = @commandText +
    '' and account.[Login] = @login ''

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = ''order by Date <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection> ''
    end
    
    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end   

  set @commandText = @commandText + 
    ''option (keepfixed plan) ''
  
  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' search.Date '' +
      '' , search.Ip '' +
      '' , search.IsPasswordValid '' +
      '' , search.IsIpValid '' +
      '' , case '' +
      ''   when exists(select 1 '' +
      ''       from dbo.VpnIp vpn_ip '' +
      ''       where vpn_ip.Ip = search.Ip) then 1 '' +
      ''   else 0 '' +
      '' end IsVpnIp '' +
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText

  set @params = 
      ''@login nvarchar(255) '' +  
      '', @verifyEventCode varchar(100) '' +
      '', @registrationEventCode varchar(100) ''

  exec sp_executesql @commandText, @params
      , @login 
      , @verifyEventCode
      , @registrationEventCode

  return 0
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetSubjectMarks]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubjectMarks]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Разбивает исходную строку на части, разделенные 
-- запятыми и знаками =.
-- v.1.0: Created by Makarev Andrey 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Приведение к стандарту.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал через xml для оптимизации
-- v.1.3: Rewritten by Yusupov Kirill 1.07.2010
-- Переписал через цикл для оптимизации
--------------------------------------------------
CREATE function [dbo].[GetSubjectMarks]
  (
  @subjectMarks nvarchar(4000)
  )
returns @SubjectMark table (SubjectId int, Mark numeric(5,1))
--returns @SubjectMark table (SubjectId NVARCHAR(20), Mark NVARCHAR(20))
as
begin
  DECLARE @RawMark NVARCHAR(20)
  DECLARE @EQIndex INT
  WHILE (CHARINDEX('','',@subjectMarks)>0)
  BEGIN
    SET @RawMark= SUBSTRING(@subjectMarks,1,CHARINDEX('','',@subjectMarks)-1)

    SET @EQIndex=CHARINDEX(''='',@RawMark)

    INSERT INTO @SubjectMark (SubjectId,Mark)
    SELECT 
      SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)

    SET @subjectMarks = SUBSTRING(@subjectMarks,CHARINDEX('','',@subjectMarks)+1,LEN(@subjectMarks))
  END
  IF (LEN(@subjectMarks)>0)
  BEGIN
    SET @RawMark= @subjectMarks

    SET @EQIndex=CHARINDEX(''='',@RawMark)

    INSERT INTO @SubjectMark (SubjectId,Mark)
    SELECT 
      SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)
  END
  RETURN
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromStringWithId]    Script Date: 05/07/2015 18:13:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_ut_SplitFromStringWithId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE funCTION [dbo].[ufn_ut_SplitFromStringWithId]
(
  @string nvarchar(max),
  @delimeter nvarchar(1) = '' '')
RETURNS @ret TABLE (id int identity(1,1) ,val nvarchar(4000) )
AS
BEGIN
  DECLARE @s int, @e int

  SET @s = 0
  WHILE CHARINDEX(@delimeter,@string,@s) <> 0
  BEGIN
    SET @e = CHARINDEX(@delimeter,@string,@s)
    INSERT @ret VALUES (SUBSTRING(@string,@s,@e - @s))
    SET @s = @e + 1
  END
  INSERT @ret VALUES (SUBSTRING(@string,@s,4000))
  RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromString]    Script Date: 05/07/2015 18:13:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_ut_SplitFromString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ufn_ut_SplitFromString] 
(
  @string nvarchar(max),
  @delimeter nvarchar(1) = '' ''
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
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateLoadingTask]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение списка заданий на загрузку сертификатов.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateLoadingTask]
  @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
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
    , @server nvarchar(30)

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  
  select @server = (select top 1 ss.name + ''.fbs_loader_db'' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = ''loader'')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''declare @search table '' +
      N''  ( '' +
      N''  Id bigint '' +
      N''  , UpdateDate datetime '' +
      N''  , EditorAccountId bigint '' +
      N''  , EditorIp nvarchar(255) '' +
      N''  , SourceBatchUrl nvarchar(255) '' +
      N''  , IsActive bit '' +
      N''  , IsProcess bit '' +
      N''  , IsCorrect bit '' +
      N''  , IsLoaded bit '' +
      N''  , ErrorCount int '' +
      N''  ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N''select <innerHeader> '' +
        N''  cne_certificate_loading_task.Id Id '' +
        N''  , cne_certificate_loading_task.CreateDate UpdateDate '' +
        N''  , cne_certificate_loading_task.EditorAccountId EditorAccountId '' +
        N''  , cne_certificate_loading_task.EditorIp EditorIp '' +
        N''  , cne_certificate_loading_task.SourceBatchUrl SourceBatchUrl '' +
        N''  , cne_certificate_loading_task.IsActive IsActive '' +
        N''  , cne_certificate_loading_task.IsProcess IsProcess '' +
        N''  , cne_certificate_loading_task.IsCorrect IsCorrect '' +
        N''  , cne_certificate_loading_task.IsLoaded IsLoaded '' +
        N''  , 0 ErrorCount  '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task with (nolock) ''
  else
    set @commandText = 
        N''select count(*) '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task with (nolock) '' 
  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = N''order by Id desc ''
      set @outerOrder = N''order by Id asc ''
      set @resultOrder = N''order by Id desc ''
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N''top 10000000''
      set @outerSelectHeader = N''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @commandText = @commandText +
      N''update search ''+
      N''set '' +
      N''  ErrorCount = ( ''+
      N''    select ''+ 
      N''      count(*)  ''+
      N''    from  ''+
      N''      '' + @server + N''.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error ''+
      N''    where ''+
      N''      cne_certificate_loading_task_error.TaskId = search.Id ''+
      N''    ) ''+
      N''from ''+
      N''  @search search ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N''select '' +
      N''  dbo.GetExternalId(search.Id) Id '' +
      N''  , search.UpdateDate UpdateDate '' +
      N''  , account.Login EditorLogin '' +
      N''  , account.LastName EditorLastName '' +
      N''  , account.FirstName EditorFirstName '' +
      N''  , account.PatronymicName EditorPatronymicName '' +
      N''  , search.EditorIp EditorIp '' +
      N''  , search.SourceBatchUrl SourceBatchUrl '' +
      N''  , search.IsActive IsActive '' +
      N''  , search.IsProcess IsProcess '' +
      N''  , search.IsCorrect IsCorrect '' +
      N''  , search.IsLoaded IsLoaded '' +
      N''  , search.ErrorCount ErrorCount '' +
      N''from '' +
      N''  @search search '' + 
      N''    left join dbo.Account account '' +
      N''      on search.EditorAccountId = account.Id '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение списка заданий на загрузку закрытий сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateDenyLoadingTask]
  @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
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
    , @server nvarchar(40)

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''

  select @server = (select top 1 ss.name + ''.fbs_loader_db'' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = ''loader'')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''declare @search table '' +
      N''  ( '' +
      N''  Id bigint '' +
      N''  , UpdateDate datetime '' +
      N''  , EditorAccountId bigint '' +
      N''  , EditorIp nvarchar(255) '' +
      N''  , SourceBatchUrl nvarchar(255) '' +
      N''  , IsActive bit '' +
      N''  , IsProcess bit '' +
      N''  , IsCorrect bit '' +
      N''  , IsLoaded bit '' +
      N''  , ErrorCount int '' +
      N''  ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N''select <innerHeader> '' +
        N''  cne_certificate_deny_loading_task.Id Id '' +
        N''  , cne_certificate_deny_loading_task.CreateDate UpdateDate '' +
        N''  , cne_certificate_deny_loading_task.EditorAccountId EditorAccountId '' +
        N''  , cne_certificate_deny_loading_task.EditorIp EditorIp '' +
        N''  , cne_certificate_deny_loading_task.SourceBatchUrl SourceBatchUrl '' +
        N''  , cne_certificate_deny_loading_task.IsActive IsActive '' +
        N''  , cne_certificate_deny_loading_task.IsProcess IsProcess '' +
        N''  , cne_certificate_deny_loading_task.IsCorrect IsCorrect '' +
        N''  , cne_certificate_deny_loading_task.IsLoaded IsLoaded '' +
        N''  , 0 ErrorCount  '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateDenyLoadingTask cne_certificate_deny_loading_task with (nolock) ''
  else
    set @commandText = 
        N''select count(*) '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateDenyLoadingTask cne_certificate_deny_loading_task with (nolock) ''
  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = N''order by Id desc ''
      set @outerOrder = N''order by Id asc ''
      set @resultOrder = N''order by Id desc ''
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N''top 10000000''
      set @outerSelectHeader = N''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @commandText = @commandText +
      N''update search ''+
      N''set '' +
      N''  ErrorCount = ( ''+
      N''    select ''+ 
      N''      count(*)  ''+
      N''    from  ''+
      N''      '' + @server + N''.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error ''+
      N''    where ''+
      N''      cne_certificate_deny_loading_task_error.TaskId = search.Id ''+
      N''    ) ''+
      N''from ''+
      N''  @search search ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N''select '' +
      N''  dbo.GetExternalId(search.Id) Id '' +
      N''  , search.UpdateDate UpdateDate '' +
      N''  , account.Login EditorLogin '' +
      N''  , account.LastName EditorLastName '' +
      N''  , account.FirstName EditorFirstName '' +
      N''  , account.PatronymicName EditorPatronymicName '' +
      N''  , search.EditorIp EditorIp '' +
      N''  , search.SourceBatchUrl SourceBatchUrl '' +
      N''  , search.IsActive IsActive '' +
      N''  , search.IsProcess IsProcess '' +
      N''  , search.IsCorrect IsCorrect '' +
      N''  , search.IsLoaded IsLoaded '' +
      N''  , search.ErrorCount ErrorCount '' +
      N''from '' +
      N''  @search search '' + 
      N''    left join dbo.Account account '' +
      N''      on search.EditorAccountId = account.Id '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchDocument]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchDocument]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchDocument

-- =============================================
-- Получение списка документов.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- v.1.1: Modified by Fomin Dmitriy 18.04.2008
-- Добавлена фильтрация по наименованию.
-- v.1.2: Modified by Makarev Andrey 19.04.2008
-- Правильный вывод ИД.
-- v.1.3: Modified by Fomin Dmitriy 21.04.2008
-- Убраны лишние поля.
-- v.1.4: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле RelativeUrl.
-- =============================================
CREATE proc [dbo].[SearchDocument]
  @isActive bit = null
  , @contextCodes nvarchar(4000) = null
  , @name nvarchar(255) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = N''Id''
  , @sortAsc bit = 0
  , @showCount bit = null
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
    , @nameFormat nvarchar(255)

  if isnull(@name, '''') <> ''''
    set @nameFormat = ''%'' + replace(@name, '' '' , ''%'') + ''%''

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Id bigint '' +
      '' , Name nvarchar(255) '' +
      '' , Description ntext '' +
      '' , IsActive bit '' +
      '' , ActivateDate datetime '' +
      '' , ContextCodes nvarchar(4000) '' +
      '' , RelativeUrl nvarchar(255) '' +
      ''   , Date datetime '' +
      '' ) '' 

  if isnull(@contextCodes, '''') <> ''''
    set @declareCommandText = @declareCommandText + 
      ''declare @codes table ''+
      '' ( '' +
      '' Code nvarchar(255) '' +
      '' ) '' +
      ''insert @codes select value from dbo.GetDelimitedValues(@contextCodes) ''

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' document.Id Id '' +
        '' , document.Name Name '' +
        '' , document.Description Description '' +
        '' , document.IsActive IsActive '' +
        '' , document.ActivateDate ActivateDate '' +
        '' , document.ContextCodes ContextCodes '' +
        '' , document.RelativeUrl RelativeUrl '' +
        '' , document.UpdateDate Date '' +
        ''from dbo.Document document with (nolock) '' +
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.Document document with (nolock) '' +
        ''where 1 = 1 '' 
  
  if not @isActive is null
    set @commandText = @commandText + '' and document.IsActive = @isActive ''

  if not @contextCodes is null
    set @commandText = @commandText + '' and not exists(select 1 '' +
        ''   from @codes context_codes '' +
        ''     inner join dbo.Context context '' +
        ''       on context.Code = context_codes.Code '' +
        ''     left outer join dbo.DocumentContext document_context with(nolock) '' +
        ''       on document_context.ContextId = context.Id '' +
        ''         and document_context.DocumentId = document.Id '' +
        ''   where document_context.Id is null) ''

  if not @nameFormat is null
    set @commandText = @commandText + '' and document.Name like @nameFormat ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = ''Name''
    begin
      set @innerOrder = ''order by Name <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Name <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Name <orderDirection>, Id <orderDirection> ''
    end
    else if @sortColumn = ''IsActive''
    begin
      set @innerOrder = ''order by IsActive <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by IsActive <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by IsActive <orderDirection>, Id <orderDirection> ''
    end
    else if @sortColumn = ''Date''
    begin
      set @innerOrder = ''order by Date <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection>, Id <orderDirection> ''
    end
    else
    begin
      set @innerOrder = ''order by Id <orderDirection> ''
      set @outerOrder = ''order by Id <orderDirection> ''
      set @resultOrder = ''order by Id <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' dbo.GetExternalId(search.Id) Id '' +
      '' , search.Name '' +
      '' , search.Description '' +
      '' , search.IsActive '' +
      '' , search.ActivateDate '' +
      '' , search.ContextCodes '' +
      '' , search.RelativeUrl '' +
      '' , search.Date '' +
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@isActive bit, @contextCodes nvarchar(4000), @nameFormat nvarchar(255)''
    , @IsActive
    , @contextCodes
    , @nameFormat

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchDeliveries]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchDeliveries]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение списка рассылок.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[SearchDeliveries]
  @title nvarchar(255) = null
  , @createDateFrom datetime = null
  , @createDateTo datetime = null
  , @deliveryDateFrom datetime = null
  , @deliveryDateTo datetime = null
  , @status int = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = null
  , @sortAsc bit = 1
  , @showCount bit = null
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
    , @titleFormat nvarchar(255)

  if isnull(@title, '''') <> ''''
    set @titleFormat = ''%'' + replace(@title, '' '' , ''%'') + ''%''

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Id bigint '' +
      '' , CreateDate datetime '' +
      '' , DeliveryDate datetime '' +
      '' , TypeCode nvarchar(20) '' +
      '' , Title nvarchar(255) '' +
      '' , Status int '' +
      '' , StatusName nvarchar(255) '' +
      '' ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' delivery.Id Id '' +
        '' , delivery.CreateDate CreateDate '' +
        '' , delivery.DeliveryDate DeliveryDate '' +
        '' , delivery.TypeCode TypeCode '' +
        '' , delivery.Title Title '' +
        '' , delivery.Status Status '' +
        '' , status.Name StatusName '' +
        ''from dbo.Delivery delivery with (nolock) '' +
        ''inner join DeliveryStatus status on delivery.Status=status.Id ''+
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.Delivery delivery with (nolock) '' +
        ''where 1 = 1 '' 
  
  if not @status is null
    set @commandText = @commandText + '' and delivery.Status = @status ''

  if not @createDateFrom is null
    set @commandText = @commandText + '' and delivery.CreateDate >= @createDateFrom ''

  if not @createDateTo is null
    set @commandText = @commandText + '' and delivery.CreateDate <= @createDateTo ''

  if not @deliveryDateFrom is null
    set @commandText = @commandText + '' and delivery.DeliveryDate >= @deliveryDateFrom ''

  if not @deliveryDateTo is null
    set @commandText = @commandText + '' and delivery.DeliveryDate <= @deliveryDateTo ''

  if not @title is null
    set @commandText = @commandText + '' and delivery.Title like @titleFormat ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N''Title''
    begin
      set @innerOrder = ''order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
    end
    else if @sortColumn = N''StatusName''
    begin
      set @innerOrder = ''order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
    end
    else if @sortColumn = N''CreateDate''
    begin
      set @innerOrder = ''order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> ''
    end
    else
    begin 
      set @innerOrder = ''order by DeliveryDate <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by DeliveryDate <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by DeliveryDate <orderDirection>, Id <orderDirection> ''

    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' search.Id Id '' +
      '' , search.CreateDate '' +
      '' , search.DeliveryDate '' +
      '' , search.TypeCode '' +
      '' , search.Title '' +
      '' , search.Status '' +
      '' , search.StatusName '' +
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@status int, @createDateFrom datetime, @createDateTo datetime, @deliveryDateFrom datetime, @deliveryDateTo datetime, @titleFormat nvarchar(255)''
    , @status
    , @createDateFrom
    , @createDateTo
    , @deliveryDateFrom
    , @deliveryDateTo
    , @titleFormat

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SelectCNECCheckHystoryByOrgIdCount]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectCNECCheckHystoryByOrgIdCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SelectCNECCheckHystoryByOrgIdCount]
	@idorg int, @isUnique bit=0,@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null, @dats datetime=null,@datf datetime=null
as
begin
	declare @str nvarchar(max),@s nvarchar(100)
	
	set @s=cast(newid() as nvarchar(100))
	if @isUnique =0 
	begin
		set @str=''	
		
		select count(*) from
				(
				select 1 t
				FROM CommonNationalExamCertificateCheck c
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
					''
		if @dats is not null 
			set @str=@str+'' and cb.CreateDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and cb.CreateDate <= @datf''
								
		if @LastName is not null
			set @str=@str+''
							and c.LastName like ''''%''''+@LastName+''''%'''' ''						
		if @TypeCheck= ''Пакетная'' or @TypeCheck is null
				set @str=@str+''	''					
		else
				set @str=@str+''						
							and 1=0 ''	
		if @idorg<>-1	
			set @str=@str+
		''						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg			
		''													
		set @str=@str+''	
				union all
				select 1 t 
				FROM CommonNationalExamCertificateRequest c 
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
		''
		if @dats is not null 
			set @str=@str+'' and cb.CreateDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and cb.CreateDate <= @datf''
								
		if @LastName is not null
			set @str=@str+''
							and c.LastName like ''''%''''+@LastName+''''%'''' ''						
		if @TypeCheck= ''Пакетная'' or @TypeCheck is null
				set @str=@str+''	''					
		else
				set @str=@str+''						
							and 1=0 ''							
		if @idorg<>-1	
			set @str=@str+
		''						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg		
		''
		set @str=@str+''
				union all
				select 1 t
				from CNEWebUICheckLog cb ''
		if @idorg<>-1	
			set @str=@str+
		''
				join Account acc with(nolock) on acc.id=cb.AccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg
		''					
		if @dats is not null 
			set @str=@str+'' and cb.EventDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and cb.EventDate <= @datf''									
				
		if @TypeCheck= ''Интерактивная'' or @TypeCheck is null
				set @str=@str+''	''					
		else
				set @str=@str+''						
							and 1=0 ''

		if @LastName is not null
			set @str=@str+''
			    left join prn.Certificates AS b with(nolock) on cb.FoundedCNEId <> ''''Нет свидетельства'''' and cb.FoundedCNEId=b.CertificateID
			    left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID
				where a.Surname like ''''%''''+@LastName+''''%'''' ''							
																
		set @str=@str+''				
				) t
		
				''
	end
	else
	begin
				set @str=
				''
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())		
		declare @table table(id int identity(1,1) primary key,idguid uniqueidentifier)		
		
		insert @table		
		select SourceCertificateIdGuid
		from 
		(
			select min(id1) id1,idtype id3,SourceCertificateIdGuid''+
				case when  @dats is not null or @datf is not null then '',min(CreateDate) CreateDate''
					 else '''' 
				end
		  +'' from 
				(				
				select min(c.id) id1, 1 idtype,c.SourceCertificateIdGuid''+
				case when @dats is not null or @datf is not null then '',min(cb.CreateDate) CreateDate''
					 else '''' 
				end
		  +'' from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateCheck c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 		
				''							
		if @LastName is not null
			set @str=@str+''												
					and c.LastName like ''''%''''+@LastName+''''%'''' 
						  ''	
		if @TypeCheck <> ''Пакетная'' and @TypeCheck is not null
				set @str=@str+''						
					and 1=0 
							  ''					
		if @idorg<>-1 
			set @str=@str+''									
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  ''				
		set @str=@str+''
				group by c.SourceCertificateIdGuid
				union all
				select min(c.id) id2, 2 idtype,c.SourceCertificateIdGuid''+
				case when @dats is not null or @datf is not null then '',min(cb.CreateDate) CreateDate''
					 else '''' 
				end
		  +'' from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateRequest c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo									
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
					  ''								  
		if @LastName is not null
			set @str=@str+''												
							and c.LastName like ''''%''''+@LastName+''''%'''' 
						  ''	
		if @TypeCheck <> ''Пакетная'' and @TypeCheck is not null
				set @str=@str+''						
							and 1=0 
							  ''					
		if @idorg<>-1 
			set @str=@str+''											 		
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  ''						
		set @str=@str+''
				group by c.SourceCertificateIdGuid
				union all
				select min(c.id) id1, 3 idtype,cast(c.FoundedCNEId as uniqueidentifier)''+
				case when @dats is not null or @datf is not null then '',min(c.EventDate) CreateDate ''
					 else '''' 
				end
		  +'' from ExamCertificateUniqueChecks a with(nolock)
					join CNEWebUICheckLog c with(nolock) on c.FoundedCNEId <> ''''Нет свидетельства''''	and c.FoundedCNEId=a.idGUID	and a.[Year] between @yearFrom and @yearTo												
					  ''								  
		if @idorg<>-1 
			set @str=@str+
			          ''							
					join Account Acc on acc.id=c.AccountId and @idorg=Acc.OrganizationId 	
					join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
					  ''		
		if @TypeCheck <> ''Интерактивная'' and @TypeCheck is not null
				set @str=@str+''						
							and 1=0 
							  ''						  			
		set @str=@str+
		''			
				group by c.FoundedCNEId
			) c		
			group by SourceCertificateIdGuid,idtype
		) c 
		where 1=1
		''
		if @dats is not null 
			set @str=@str+'' and c.CreateDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and c.CreateDate <= @datf''	
			
		set @str=@str+''	
		select count(distinct idguid) from 
		(
				select a.* from @table a
					join (select min(id)id,idguid from @table group by idguid) b on a.id=b.id
		) t		
		''
		
	end
	print @str
	exec sp_executesql @str,N''@idorg int,@LastName nvarchar(255)=null,@dats datetime,@datf datetime'',@idorg=@idorg,@LastName=@LastName,@dats=@dats,@datf=@datf
		
end


' 
END
GO
/****** Object:  StoredProcedure [dbo].[SelectCNECCheckHystoryByOrgId]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectCNECCheckHystoryByOrgId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SelectCNECCheckHystoryByOrgId]
	@idorg int, @ps int=1,@pf int=100,@so int=1,@fld nvarchar(250)=''id'', @isUnique bit=0,
	@LastName nvarchar(255)=null, @TypeCheck nvarchar(255)=null, @dats datetime=null,@datf datetime=null
as
begin
	declare @str nvarchar(max),@ss nvarchar(20),@fldso1 nvarchar(max),@fldso2 nvarchar(max),@fldso3 nvarchar(max)

	if @fld=''date'' 	
		set @ss=''cb.CreateDate''
	else 
		if @fld=''TypeCheck'' 
			set @ss=''c.id''
		else 
			set @ss=''c.''+@fld
			
		
	if @isUnique =0 
	begin	
		if @ps = 0 	
			set @ps = 1
		else
			set @pf=@pf - 1	
			
		set @str=
		''
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())			
		
		declare @tab table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)
		declare @table table(id int identity(1,1) primary key,id1 int,id2 int, id3 int)
		
		insert @tab
		select top (@pf) id1,id2,id3
			from 
			(
			select top (@pf) c.id id1,null id2,null id3,''+@ss+''
			from CommonNationalExamCertificateCheck c
				JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
		''
		if @dats is not null 
			set @str=@str+'' and cb.CreateDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and cb.CreateDate <= @datf''
			
		if @idorg<>-1	
			set @str=@str+
		''						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg			
		''
		if @TypeCheck is not null and @TypeCheck <> ''Пакетная'' 
			set @str=@str+
		''
				and 1=0
		''
		if @LastName is not null
			set @str=@str+
		''
				and c.LastName like ''''%''''+@LastName+''''%'''' 
		''			
		set @str=@str+
		''
			order by ''+@ss+case when @so = 0 then '' asc'' else '' desc'' end + ''
			union all
			select top (@pf) null id1,c.id id2,null id3,''+@ss+''
			FROM CommonNationalExamCertificateRequest c with(nolock)
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
		''
		if @dats is not null 
			set @str=@str+'' and cb.CreateDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and cb.CreateDate <= @datf''
					
		if @idorg<>-1	
			set @str=@str+
		''						
				JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg		
		''
		if @TypeCheck is not null and @TypeCheck <> ''Пакетная'' 
			set @str=@str+
		''
				and 1=0
		''		
		if @LastName is not null
			set @str=@str+
		''
				and c.LastName like ''''%''''+@LastName+''''%'''' 
		''					
		set @str=@str+
		''		
			order by ''+@ss+case when @so = 0 then '' asc'' else '' desc'' end + ''				
			union all
			select top (@pf) null id1,null id2,c.id id3,''+case when @fld=''date'' then ''EventDate CreateDate'' else @ss end+''
			FROM CNEWebUICheckLog c with(nolock)
		''
		if @idorg<>-1	
			set @str=@str+
		''
				join Account acc with(nolock) on acc.id=c.AccountId
				join Organization2010 d with(nolock) on d.id=acc.OrganizationId and d.DisableLog=0 and d.id=@idorg
		''		
		if @TypeCheck is not null and @TypeCheck <> ''Интерактивная'' 
			set @str=@str+
		''
				and 1=0
		''
		if @dats is not null 
			set @str=@str+'' and c.EventDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and c.EventDate <= @datf''	
							
		if @LastName is not null
			set @str=@str+
		''
				and c.LastName like ''''%''''+@LastName+''''%'''' 
		''				
		set @str=@str+
		''		
			order by ''+case when @fld=''date'' then ''EventDate'' else @ss end+case when @so = 0 then '' asc'' else '' desc'' end+''
		) c
		order by ''+case when @fld=''date'' then ''CreateDate'' else @ss end+case when @so = 0 then '' asc'' else '' desc'' end + ''
			
		insert @table
		select id1,id2,id3 from @tab where id between @ps and @pf ''
		print @str
		set @str=@str+
		''	
		select * from 
		(
			select c.Id,cb.CreateDate,''''Пакетная'''' TypeCheck,c.CertificateNumber,c.TypographicNumber,
				  c.LastName,c.FirstName,c.PatronymicName,
				  ISNULL(c.PassportSeria,'''''''')+'''' ''''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
					( SELECT ( 
								SELECT CAST(s.SubjectId AS VARCHAR(20))
									+ ''''=''''
									+ REPLACE(CAST(s.Mark AS VARCHAR(20)),
											  '''','''', ''''.'''') + '''','''' AS [text()]
								FROM dbo.CommonNationalExamCertificateSubjectCheck s
								WHERE s.CheckId = c.Id AND s.Mark IS NOT NULL
								FOR
								XML PATH(''''''''),
								TYPE
							 ) marks
					) as Marks,c.SourceCertificateIdGuid, 
			   case when ed.[ExpireDate] is null then ''''Не найдено''''  
					when certificate_deny.CertificateFK is not null then ''''Аннулировано'''' 
					when getdate() <= ed.[ExpireDate] then ''''Действительно''''
				else ''''Истек срок'''' end Status,c1.id rn
			FROM CommonNationalExamCertificateCheck c with(nolock)
				JOIN CommonNationalExamCertificateCheckBatch cb with(nolock) ON c.BatchId=cb.Id
				JOIN @table c1 on c1.id1=c.id
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGuid and cc.[Year] between @yearFrom and @yearTo	
				left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid ''
		set @str=@str+
		''					
			union all
			select c.Id,cb.CreateDate,''''Пакетная'''' TypeCheckk,c.SourceCertificateNumber CertificateNumber,c.TypographicNumber,
				c.LastName,c.FirstName,c.PatronymicName,
				ISNULL(c.PassportSeria,'''''''') + '''' ''''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
				null as Marks,c.SourceCertificateIdGuid,
				case WHEN ed.[ExpireDate] is null THEN ''''Не найдено'''' 
					when certificate_deny.CertificateFK is not null then ''''Аннулировано'''' 
					when getdate() <= ed.[ExpireDate] then ''''Действительно''''
					else ''''Истек срок'''' end STATUS,
				c1.id rn
			FROM CommonNationalExamCertificateRequest c with(nolock)
				JOIN @table c1 on c1.id2=c.id
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
				left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	''
		set @str=@str+
		''						
			union all
			select cb.id,cb.EventDate,''''Интерактивная'''' TypeCheck,
				ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
				ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,
				ISNULL(a.Surname, cb.LastName) LastName, 
				ISNULL(a.Name, cb.FirstName) FirstName, 
				ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
				case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +'''' '''' + cb.PassportNumber
					 else a.DocumentSeries + '''' '''' + a.DocumentNumber end PassportData, 
				b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
				b.CertificateID SourceCertificateIdGuid,
				case when ed.[ExpireDate] is null then ''''Не найдено''''  
					when certificate_deny.CertificateFK is not null then ''''Аннулировано''''
					when getdate() <= ed.[ExpireDate] then ''''Действительно''''
				else ''''Истек срок'''' end Status,cb1.id rn		
			from 
				@table cb1
				join CNEWebUICheckLog cb with(nolock) on cb1.id3=cb.id															
				JOIN prn.Certificates AS b with(nolock) on cb.FoundedCNEId <> ''''Нет свидетельства'''' and cb.FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
				left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.UseYear=b.UseYear
				left join ExamcertificateUniqueChecks CC with(nolock) on b.CertificateID=cc.idGUID and cc.[Year]=b.UseYear
				left join [ExpireDate] as ed on ed.[Year] = b.Useyear
				left join prn.CancelledCertificates certificate_deny with (nolock)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = b.CertificateID
		) c																
		order by ''+case when @fld=''date'' then ''CreateDate'' else @ss end+case when @so = 0 then '' asc'' else '' desc'' end				
		
		exec sp_executesql @str,N''@idorg int,@ps int,@pf int,@LastName nvarchar(255),@dats datetime,@datf datetime'',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName,@dats=@dats,@datf=@datf
		return
	end					
	else
	begin	
	    set @pf=@pf - 1			
	    
		set @str=
				''
		declare @yearFrom int, @yearTo int		
		select @yearFrom = 2008, @yearTo = Year(GetDate())		
		declare @tab table(id int identity(1,1) primary key,id1 int,idtype int,idguid uniqueidentifier)		
		declare @table table(id int identity(1,1) primary key,id1 int,idtype int)
			
		insert @tab			
		select id1,idtype,SourceCertificateIdGuid
		from 
		(
			select top (1000000) min(id1) id1,idtype,SourceCertificateIdGuid''+
				case when @fld=''LastName'' then '',LastName'' 
					 when @fld = ''date'' or @dats is not null or @datf is not null then '',min(CreateDate) CreateDate''
					 else '''' 
				end
		  +'' from 
				(				
				select min(c.id) id1, 1 idtype,c.SourceCertificateIdGuid''+
				case when @fld=''LastName'' then '',c.LastName'' 
					 when @fld = ''date'' or @dats is not null or @datf is not null then '',min(cb.CreateDate) CreateDate''
					 else '''' 
				end
		  +'' from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateCheck c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo
					JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 		
				''							
		if @LastName is not null
			set @str=@str+''												
					and c.LastName like ''''%''''+@LastName+''''%'''' 
						  ''	
		if @TypeCheck <> ''Пакетная'' and @TypeCheck is not null
				set @str=@str+''						
					and 1=0 
							  ''					
		if @idorg<>-1 
			set @str=@str+''									
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  ''				
		set @str=@str+''
				group by c.SourceCertificateIdGuid''+
				case when @fld=''LastName'' then '',c.LastName'' 
					 else '''' 
				end
		  +'' union all
				select min(c.id) id1,2 idtype,c.SourceCertificateIdGuid''+
				case when @fld=''LastName'' then '',c.LastName'' 
					 when @fld = ''date'' or @dats is not null or @datf is not null then '',min(cb.CreateDate) CreateDate''
					 else '''' 
				end
		  +'' from ExamCertificateUniqueChecks a with(nolock)
					join CommonNationalExamCertificateRequest c with(nolock) on c.SourceCertificateIdGuid=a.idguid and a.[Year] between @yearFrom and @yearTo									
					JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
					  ''								  
		if @LastName is not null
			set @str=@str+''												
							and c.LastName like ''''%''''+@LastName+''''%'''' 
						  ''	
		if @TypeCheck <> ''Пакетная'' and @TypeCheck is not null
				set @str=@str+''						
							and 1=0 
							  ''					
		if @idorg<>-1 
			set @str=@str+''											 		
					JOIN Account Acc ON Acc.Id=cb.OwnerAccountId and @idorg=Acc.OrganizationId 	
						  ''						
		set @str=@str+''
				group by c.SourceCertificateIdGuid''+
				case when @fld=''LastName'' then '',c.LastName'' 
					 else '''' 
				end
		  +'' union all
				select min(c.id) id1,3 idtype, cast(c.FoundedCNEId as uniqueidentifier)''+
				case when @fld=''LastName'' then '',c.LastName'' 
					 when @fld = ''date'' or @dats is not null or @datf is not null then '',min(c.EventDate) CreateDate ''
					 else '''' 
				end
		  +'' from ExamCertificateUniqueChecks a with(nolock)
					join CNEWebUICheckLog c with(nolock) on c.FoundedCNEId <> ''''Нет свидетельства''''	and c.FoundedCNEId=a.idGUID	and a.[Year] between @yearFrom and @yearTo												
					  ''								  
		if @idorg<>-1 
			set @str=@str+
			          ''							
					join Account Acc on acc.id=c.AccountId and @idorg=Acc.OrganizationId 	
					join Organization2010 d on d.id=Acc.OrganizationId and d.DisableLog=0 
					  ''		
		if @TypeCheck <> ''Интерактивная'' and @TypeCheck is not null
				set @str=@str+''						
							and 1=0 
							  ''						  			
		set @str=@str+
		''			
				group by c.FoundedCNEId''+
				case when @fld=''LastName'' then '',c.LastName'' 
					 else '''' 
				end
		  +'') c		
			group by SourceCertificateIdGuid, idtype''+
				case when @fld=''LastName'' then '',c.LastName''
					 else '''' 
				end
		  +'' order by ''+case when @fld=''LastName'' then ''LastName'' else '' id1'' end + case when @so = 0 then '' asc'' else '' desc'' end + ''
		) c 
		where 1=1
		''
		if @dats is not null 
			set @str=@str+'' and c.CreateDate >= @dats''
		if @datf is not null 
			set @str=@str+'' and c.CreateDate <= @datf''	
		if @fld=''date''
		set @str=@str+'' order by c.CreateDate ''+ case when @so = 0 then '' asc'' else '' desc'' end 	
			
		
		set @str=@str+''						
		insert @table
		select id1,idtype from
		(
			select a.id1,a.idtype,row_number() over (order by a.id) rn from @tab a
				join (select min(id1) id1,idguid from @tab group by idguid) b on a.id1=b.id1 and a.idguid=b.idguid		
		) t
		where rn between @ps and @pf			
		
		/*select * from @table
		select * from @tab*/
		''
	print @str
		set @str=@str+
		''	
		select * into #ttt 
		from 
		(		
			select c.Id,cb.CreateDate,''''Пакетная'''' TypeCheck,c.CertificateNumber,c.TypographicNumber,
				  c.LastName,c.FirstName,c.PatronymicName,
				  ISNULL(c.PassportSeria,'''''''')+'''' ''''+c.PassportNumber PassportData,c.[year], isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,
					( SELECT ( 
								SELECT CAST(s.SubjectId AS VARCHAR(20))
									+ ''''=''''
									+ REPLACE(CAST(s.Mark AS VARCHAR(20)),
											  '''','''', ''''.'''') + '''','''' AS [text()]
								FROM dbo.CommonNationalExamCertificateSubjectCheck s
								WHERE s.CheckId = c.Id AND s.Mark IS NOT NULL
								FOR
								XML PATH(''''''''),
								TYPE
							 ) marks
					) as Marks,c.SourceCertificateIdGuid, 
			   case when ed.[ExpireDate] is null then ''''Не найдено''''  
					when certificate_deny.CertificateFK is not null then ''''Аннулировано'''' 
					when getdate() <= ed.[ExpireDate] then ''''Действительно''''
				else ''''Истек срок'''' end Status 
			FROM CommonNationalExamCertificateCheck c with(nolock)
				JOIN CommonNationalExamCertificateCheckBatch cb with(nolock) ON c.BatchId=cb.Id
				JOIN @table c1 on c1.id1=c.id and c1.idtype=1
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGuid and cc.[Year] between @yearFrom and @yearTo	
				left join [ExpireDate] as ed on c.[Year] = ed.[Year] 
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid ''
		set @str=@str+
		''					
			union all
			select c.Id,cb.CreateDate,''''Пакетная'''' TypeCheckk,c.SourceCertificateNumber CertificateNumber,c.TypographicNumber,
				c.LastName,c.FirstName,c.PatronymicName,
				ISNULL(c.PassportSeria,'''''''') + '''' ''''+c.PassportNumber PassportData,c.SourceCertificateYear, isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck, 
				null as Marks,c.SourceCertificateIdGuid,
				case WHEN ed.[ExpireDate] is null THEN ''''Не найдено'''' 
					when certificate_deny.CertificateFK is not null then ''''Аннулировано'''' 
					when getdate() <= ed.[ExpireDate] then ''''Действительно''''
					else ''''Истек срок'''' end STATUS
			FROM CommonNationalExamCertificateRequest c  with(nolock)
				JOIN @table c1 on c1.id1=c.id and c1.idtype=2
				JOIN CommonNationalExamCertificateRequestBatch cb with(nolock) ON c.BatchId=cb.Id  
				left join ExamcertificateUniqueChecks CC with(nolock) on c.SourceCertificateIdGuid=cc.idGUID and cc.[Year] between @yearFrom and @yearTo
				left join [ExpireDate] as ed on ed.[Year] = c.SourceCertificateYear
				left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = c.SourceCertificateIdGuid	''
		set @str=@str+
		''						
			union all
			select cb.id,cb.EventDate,''''Интерактивная'''' TypeCheck,
				ISNULL(b.LicenseNumber, cb.CNENumber) CertificateNumber, 
				ISNULL(b.TypographicNumber, cb.TypographicNumber) TypographicNumber,
				ISNULL(a.Surname, cb.LastName) LastName, 
				ISNULL(a.Name, cb.FirstName) FirstName, 
				ISNULL(a.SecondName, cb.PatronymicName) PatronymicName,
				case when a.DocumentSeries is null and a.DocumentNumber is null then cb.PassportSeria +'''' '''' + cb.PassportNumber
					 else a.DocumentSeries + '''' '''' + a.DocumentNumber end PassportData, 
				b.Useyear [Year],isnull(CC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck,cb.Marks as Marks,
				b.CertificateID SourceCertificateIdGuid,
				case when ed.[ExpireDate] is null then ''''Не найдено''''  
					when certificate_deny.CertificateFK is not null then ''''Аннулировано''''
					when getdate() <= ed.[ExpireDate] then ''''Действительно''''
				else ''''Истек срок'''' end Status				
			from 
				@table cb1
				join CNEWebUICheckLog cb with(nolock) on cb1.id1=cb.id and idtype=3
				JOIN prn.Certificates AS b with(nolock) on cb.FoundedCNEId=b.CertificateID and b.[UseYear] between @yearFrom and @yearTo
				left join rbd.Participants AS a with(nolock) ON b.ParticipantFK = a.ParticipantID and a.UseYear=b.UseYear
				left join ExamcertificateUniqueChecks CC with(nolock) on b.CertificateID=cc.idGUID and b.UseYear=b.UseYear
				left join [ExpireDate] as ed on ed.[Year] = b.Useyear
				left join prn.CancelledCertificates certificate_deny with (nolock)
					on certificate_deny.[UseYear] between @yearFrom and @yearTo
						and certificate_deny.CertificateFK = b.CertificateID																
		) t
				
				--select * from #ttt order by id desc
		select * from
		(
			select *,row_number() over (order by ''+case when @fld=''date'' then ''CreateDate'' else @ss end+case when @so = 0 then '' asc'' else '' desc'' end + '') rn from
			(
				select a.* from #ttt a
					join (select min(id)id,SourceCertificateIdGuid from #ttt group by SourceCertificateIdGuid) b on a.id=b.id
			) c
		) t
		order by rn
		
		drop table #ttt	''
		exec sp_executesql @str,N''@idorg int,@ps int,@pf int,@LastName nvarchar(255),@dats datetime,@datf datetime'',@idorg=@idorg,@ps=@ps,@pf=@pf,@LastName=@LastName,@dats=@dats,@datf=@datf		
		return
	end	
end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchVUZ]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchVUZ]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procEDURE [dbo].[SearchVUZ] 
(@orgNamePrefix varchar(256))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @mask varchar(260)
  SET @mask=''%''+replace(replace(@orgNamePrefix,''%'',''[%]''),''*'',''%'')+''%''
  
  SELECT OrgName 
  FROM dbo.OrgEtalon 
  WHERE OrgName LIKE @mask
  ORDER BY OrgName
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchNews]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchNews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchNews

-- =============================================
-- Получение списка новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим название новости.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Название новости добавлено в фильтр.
-- =============================================
CREATE proc [dbo].[SearchNews]
  @isActive bit = null
  , @dateFrom datetime = null
  , @dateTo datetime = null
  , @name nvarchar(255) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = null
  , @sortAsc bit = 1
  , @showCount bit = null
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
    , @nameFormat nvarchar(255)

  if isnull(@name, '''') <> ''''
    set @nameFormat = ''%'' + replace(@name, '' '' , ''%'') + ''%''

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Id bigint '' +
      '' , Date datetime '' +
      '' , Description ntext '' +
      '' , Name nvarchar(255) '' +
      '' , IsActive bit '' +
      '' ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' news.Id Id '' +
        '' , news.Date Date '' +
        '' , news.Description Description '' +
        '' , news.Name Name '' +
        '' , news.IsActive IsActive '' +
        ''from dbo.News news with (nolock) '' +
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.News news with (nolock) '' +
        ''where 1 = 1 '' 
  
  if not @isActive is null
    set @commandText = @commandText + '' and news.IsActive = @isActive ''

  if not @dateFrom is null
    set @commandText = @commandText + '' and news.Date >= @dateFrom ''

  if not @dateTo is null
    set @commandText = @commandText + '' and news.Date <= @dateTo ''

  if not @name is null
    set @commandText = @commandText + '' and news.Name like @nameFormat ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N''Name''
    begin
      set @innerOrder = ''order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> ''
    end
    else if @sortColumn = N''IsActive''
    begin
      set @innerOrder = ''order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> ''
    end
    else 
    begin
      set @innerOrder = ''order by Date <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection>, Id <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' dbo.GetExternalId(search.Id) Id '' +
      '' , search.Date '' +
      '' , search.Description '' +
      '' , search.Name '' +
      '' , search.IsActive '' +
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@isActive bit, @dateFrom datetime, @dateTo datetime, @nameFormat nvarchar(255)''
    , @IsActive
    , @dateFrom
    , @dateTo
    , @nameFormat

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ut_lock2]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ut_lock2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

create   PROC [dbo].[usp_ut_lock2] 
( 
@dbname sysname = null, 
@spid int = NULL 
) 
AS 
/************************************************************************************************* 

Examples: 

To see all the locks: 
EXEC usp_lock2 

To see all the locks in a particular database, say ''pubs'': 
EXEC sp_lock2 pubs 

To see all the locks held by a particular spid, say 53: 
EXEC sp_lock2 @spid = 53 

To see all the locks held by a particular spid (23), in a particular database (pubs): 
EXEC sp_lock2 pubs, 23 
*************************************************************************************************/ 

BEGIN 
SET NOCOUNT ON 
CREATE TABLE #lock 
( 
spid int, 
dbid int, 
ObjId int, 
IndId int, 
Type char(5), 
Resource char(200), 
Mode char(10), 
Status char(10)
) 

INSERT INTO #lock EXEC sp_lock 

IF @dbname IS NULL 
BEGIN 
IF @spid IS NULL 
BEGIN 
SELECT a.spid AS SPID, 
(SELECT DISTINCT program_name FROM master..sysprocesses WHERE spid = a.spid) AS [Program Name], 
cast((SELECT DISTINCT loginame FROM master..sysprocesses WHERE spid = a.spid)as varchar(10)) AS loginame,
db_name(dbid) AS [Database Name], ISNULL(object_name(ObjId),'''') AS [Object Name],IndId, Type, Resource, Mode, Status 
FROM #lock a order by [Program Name]
END 
ELSE 
BEGIN 
SELECT a.spid AS SPID, 
(SELECT DISTINCT program_name FROM master..sysprocesses WHERE spid = a.spid) AS [Program Name], 
db_name(dbid) AS [Database Name], ISNULL(object_name(ObjId),'''') AS [Object Name],IndId, Type, Resource, Mode, Status 
FROM #lock a 
WHERE spid = @spid 
END 
END 
ELSE 
BEGIN 
IF @spid IS NULL 
BEGIN 
SELECT a.spid AS SPID, 
(SELECT DISTINCT program_name FROM master..sysprocesses WHERE spid = a.spid) AS [Program Name], 
ISNULL(object_name(a.ObjId),'''') AS [Object Name],
a.IndId, 
ISNULL((SELECT name FROM sysindexes WHERE id = a.objid and indid = a.indid ),'''') AS [Index Name], 
a.Type, a.Resource, a.Mode, a.Status 
FROM #lock a 
WHERE dbid = db_id(@dbname) 
END 
ELSE 
BEGIN 
SELECT a.spid AS SPID, 
--(SELECT DISTINCT program_name FROM master..sysprocesses WHERE spid = a.spid) AS [Program Name], 
ISNULL(object_name(a.ObjId),'''') AS [Object Name],
a.IndId, 
ISNULL((SELECT name FROM sysindexes WHERE id = a.objid and indid = a.indid ),'''') AS [Index Name], 
a.Type, a.Resource, a.Mode, a.Status 
FROM #lock a 
WHERE dbid = db_id(@dbname) AND spid = @spid 
END 
END 

DROP TABLE #lock 

END' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[CapitalizeFirstLetter]    Script Date: 05/07/2015 18:13:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CapitalizeFirstLetter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[CapitalizeFirstLetter]
(
--string need to format
@string VARCHAR(200)--increase the variable size depending on your needs.
)
RETURNS VARCHAR(200)
AS

BEGIN
--Declare Variables
DECLARE @Index INT,
@ResultString VARCHAR(200)--result string size should equal to the @string variable size
--Initialize the variables
SET @Index = 1
SET @ResultString = ''''
--Run the Loop until END of the string

WHILE (@Index <LEN(@string)+1)
BEGIN
IF (@Index = 1)--first letter of the string
BEGIN

--make the first letter capital
SET @ResultString =
@ResultString + UPPER(SUBSTRING(@string, @Index, 1))
SET @Index = @Index+ 1--increase the index
END

-- IF the previous character is space or ''-'' or next character is ''-''

ELSE IF ((SUBSTRING(@string, @Index-1, 1) ='' '' or 
SUBSTRING(@string, @Index-1, 1) =''-'') and @Index+1 <> LEN(@string))
BEGIN
--make the letter capital
SET
@ResultString = @ResultString + UPPER(SUBSTRING(@string,@Index, 1))
SET
@Index = @Index +1--increase the index
END
ELSE-- all others
BEGIN
-- make the letter simple
SET
@ResultString = @ResultString + LOWER(SUBSTRING(@string,@Index, 1))
SET
@Index = @Index +1--incerase the index
END
END--END of the loop

IF (@@ERROR
<> 0)-- any error occur return the sEND string
BEGIN
SET
@ResultString = @string
END
-- IF no error found return the new string
RETURN @ResultString
END' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[CanViewUserAccountRegistrationDocument]    Script Date: 05/07/2015 18:13:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanViewUserAccountRegistrationDocument]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Можно ли просматривать документ регистрации.
-- v1.0: Created by Fomin Dmitriy 07.04.2008
--------------------------------------------------
CREATE function [dbo].[CanViewUserAccountRegistrationDocument]
  (
  @confirmYear int
  )
returns bit
as
begin
  return case
      when @confirmYear = year(getdate()) then 1
      else 0
    end
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccountRegistrationDocument]    Script Date: 05/07/2015 18:13:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanEditUserAccountRegistrationDocument]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Можно ли редактировать пользователю свой
-- документ регистрации.
-- v1.0: Created by Fomin Dmitriy 04.04.2008
--------------------------------------------------
CREATE function [dbo].[CanEditUserAccountRegistrationDocument]
  (
  @status nvarchar(255)
  )
returns bit
as
begin
  return case
      when not @status in (''activated'', ''deactivated'') then 1
      else 0
    end
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccount]    Script Date: 05/07/2015 18:13:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CanEditUserAccount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Можно ли редактировать пользователю свою
-- учетную запись.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.0: Modified by Fomin Dmitriy 19.05.2008
-- Модифицировать анкету можно до утверждения ее
-- документом.
--------------------------------------------------
CREATE function [dbo].[CanEditUserAccount]
  (
  @status nvarchar(255)
  , @confirmYear int
  , @currentYear int
  )
returns bit
as
begin
  return case
      when not @status in (''activated'', ''deactivated'') then 1
      else 0
    end
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[CompareStrings]    Script Date: 05/07/2015 18:13:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompareStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Длина сравниваемых строк увеличена.
-- =============================================
CREATE function [dbo].[CompareStrings]
  (
  @string1 nvarchar(4000)
  , @string2 nvarchar(4000)
  -- Чувствительность: кол-во совпадающих символов подстроки.
  , @matchCount int 
  )
returns decimal(18, 4)
as
begin
  declare
    @compareStr1 nvarchar(4000)
    , @compareStr2 nvarchar(4000)
    , @i int
    , @j int
    , @count1 int
    , @count int

  set @matchCount = isnull(@matchCount, 3)
  set @compareStr1 = replace(isnull(@string1, ''''), '' '', '''')
  set @compareStr2 = replace(isnull(@string2, ''''), '' '', '''')
  set @count = 0

  if @compareStr1 = @compareStr2
    return 1

  if len(@compareStr1) = 0 or len(@compareStr2) = 0
    return 0

  set @i = 1
  while @i < len(@compareStr1)
  begin
    set @j = 1
    while @j < len(@compareStr2)  
    begin
      if substring(@compareStr1, @i, 1) = substring(@compareStr2, @j, 1)
      begin
        set @count1 = 1
        while (@i + @count1 <= len(@compareStr1)) and (@j + @count1 <= len(@compareStr2))
            and (substring(@compareStr1, @i + @count1, 1) = substring(@compareStr2, @j + @count1, 1))
          set @count1 = @count1 + 1
        set @i = @i + @count1 - 1
        set @j = @j + @count1 - 1
        if @count1 >= @matchCount
          set @count = @count + @count1
      end
      set @j = @j + 1
    end
    set @i = @i + 1
  end
  
  if len(@compareStr1) > len(@compareStr2)
    return cast(@count as decimal(18, 4)) / cast(len(@compareStr1) as decimal(18, 4))
  else
    return cast(@count as decimal(18, 4)) / cast(len(@compareStr2) as decimal(18, 4))

  return 0
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[HasUserAccountAdminComment]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HasUserAccountAdminComment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Имеет ли учетная запись пользователя комментарий администратора.
-- v1.0: Created by Makarev Andrey 04.04.2008
--------------------------------------------------
CREATE function [dbo].[HasUserAccountAdminComment]
  (
  @status nvarchar(255)
  )
returns bit 
as  
begin
  return case
      when @status in (N''deactivated'', N''revision'') then 1
      else 0
    end
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatusOrder]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStatusOrder]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Порядковый номер статуса.
-- v.1.0: Created by Fomin Dmitriy 11.04.2008
--------------------------------------------------
CREATE function [dbo].[GetUserStatusOrder]
  (
  @status nvarchar(255)
  )
returns int
as
begin
  return case
      when @status = ''consideration'' then 1
      when @status = ''revision'' then 2
      when @status = ''activated'' then 3
      when @status = ''registration'' then 4
      when @status = ''deactivated'' then 5
      else 5
    end
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatus]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserStatus]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[GetUserStatus]
  (
    @confirmYear int
  , @status nvarchar(255)
  , @currentYear int
  , @registrationDocument image
  )
returns nvarchar(255) 
as  
begin
  set @status = isnull(@status, N''registration'')
  --if @confirmYear < Year(GetDate()) 
  --  set @status = N''registration''

  return case
      when not @registrationDocument is null and @status = N''registration''
        then N''consideration''
      --when @registrationDocument is null and not @status in (N''activated'', N''deactivated'')
      when @status not in (N''activated'', N''deactivated'')
        then N''registration''
      else @status
    end
end' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserIsActive]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserIsActive]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Возвращает состояние действующей записи пользователя.
-- v1.0: Created by Fomin Dmitriy 10.04.2008
--------------------------------------------------
CREATE function [dbo].[GetUserIsActive]
  (
  @status nvarchar(255)
  )
returns nvarchar(255) 
as  
begin
  return case
      when @status = N''deactivated'' then 0
      else 1
    end
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDelimitedValues]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDelimitedValues]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Разбивает исходную строку на части, разделенные запятыми.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Makarev Andrey 16.04.2008
-- Измение размера выходного массива.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал в рамках оптимизации через xml
--------------------------------------------------
CREATE function [dbo].[GetDelimitedValues]
  (
  @ids nvarchar(4000)
  )
returns @Values table ([value] nvarchar(4000))
as
begin
  if len(ltrim(rtrim(@ids))) > 0
  begin
    DECLARE @x xml
    set @x = ''<root><v>'' + replace(@ids, '','', ''</v><v>'') + ''</v></root>''
    insert into @Values
    SELECT  T.c.value(''.'',''nvarchar(4000)'')
    FROM    @x.nodes(''/root/v'') T ( c )
  end
  return  
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetCommonNationalExamCertificateActuality]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCommonNationalExamCertificateActuality]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--------------------------------------------------
-- Получить годы актуальности сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 27.05.2008
--------------------------------------------------
CREATE function [dbo].[GetCommonNationalExamCertificateActuality]
  (
  )
returns @Actuality table (YearFrom int, YearTo int)
as 
begin
  insert into @Actuality
  select
    Year(GetDate()) - 5
    , Year(GetDate())

  return
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetExternalId]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExternalId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
CREATE function [dbo].[GetExternalId]
  (
  @internalId bigint
  )
returns bigint
as
begin
  if isnull(@internalId, -1) < 0
    return null
  if @internalId = 0
    return 0

  declare
    @base bigint
    , @shift bigint
    , @shiftedId bigint

  set @base = power(2, 20)
  set @shift = 11541954384
  
  set @shiftedId = @internalId + @shift
  return (@shiftedId / @base) + (@shiftedId % @base) * @base
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetEventParam]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetEventParam]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Возвращает значение параметра по его номеру, 
-- из строки с разделителем |.
-- v.1.0: Created by Sedov Anton 15.05.2008
-- Измение размера выходного массива.
-- =============================================
CREATE function [dbo].[GetEventParam]
  (
  @eventParams nvarchar(4000)
  , @index int
  )
returns nvarchar(4000) with schemabinding
as
begin
  declare 
    @delimiterIndex int
    , @startIndex int
    , @sourceParams nvarchar(4000)
    , @result nvarchar(4000)
  
  set @delimiterIndex = 1
  set @startIndex = 1
  set @sourceParams = Convert(nvarchar(4000), @eventParams) 
  set @delimiterIndex = charindex(''|'', isnull(@sourceParams, ''''))
  
  if @delimiterIndex = 0
    return @sourceParams

  set @delimiterIndex = 1

  while @index <> 0
  begin
    set @startIndex = 1
    set @delimiterIndex = charindex(''|'', isnull(@sourceParams, ''''))

    if @delimiterIndex = 0
      set @result = @sourceParams
    else
    begin
      set @result = substring(@sourceParams, @startIndex, @delimiterIndex - @startIndex)
      set @startIndex = @delimiterIndex
      set @sourceParams = substring(@sourceParams
          , @startIndex + 1, len(@sourceParams) - @startIndex) 
    end

    set @index = @index - 1
  end 
  
  return @result
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalPassportSeria]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInternalPassportSeria]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'------------------------------------------------
-- Функция получения серии паспорта
-- без пробелов
-- v.1.0: Created by Fomin Dmitriy 21.06.2008
-----------------------------------------------
CREATE function [dbo].[GetInternalPassportSeria]
  (
  @passportSeria nvarchar(255)
  )
returns nvarchar(255)
as
begin
  return replace(@passportSeria, '' '', '''')
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalId]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInternalId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Получить внутренний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
CREATE function [dbo].[GetInternalId]
  (
  @externalId bigint
  )
returns bigint
as
begin
  declare
    @result bigint

  if isnull(@externalId, -1) < 0
    return null

  if @externalId = 0
    return 0

  declare
    @base bigint
    , @shift bigint

  set @base = power(2, 20)
  set @shift = 11541954384
  
  set @result = (@externalId / @base) + (@externalId % @base) * @base - @shift
  if dbo.GetExternalId(@result) <> @externalId
    return -1
  return @result
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetDocumentByUrl]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDocumentByUrl]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetDocumentByUrl

-- =============================================
-- Получение документа по относительному Url.
-- v.1.0: Created by Fomin Dmitriy 24.04.2008
-- =============================================
CREATE proc [dbo].[GetDocumentByUrl]
  @relativeUrl nvarchar(255)
as
begin
  select top 1
    dbo.GetExternalId([document].Id) Id
  from 
    dbo.Document [document] with (nolock, fastfirstrow)
  where 
    [document].RelativeUrl = @relativeUrl
    and [document].IsActive = 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[dba_indexDefragStandard_sp]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[dba_indexDefragStandard_sp]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N' 
CREATE PROCEDURE [dbo].[dba_indexDefragStandard_sp]
 
    /* Declare Parameters */
      @minFragmentation     FLOAT           = 10.0
        /* in percent, will not defrag if fragmentation
           less than specified */
    , @rebuildThreshold     FLOAT           = 30.0
        /* in percent, greater than @rebuildThreshold
           will result in rebuild instead of reorg */
    , @executeSQL           BIT             = 1
        /* 1 = execute; 0 = print command only */
    , @tableName            VARCHAR(4000)   = Null 
        /* Option to specify a table name */
    , @printCommands        BIT             = 0
        /* 1 = print commands; 0 = do not print commands */
    , @printFragmentation   BIT             = 0
        /* 1 = print fragmentation prior to defrag; 
           0 = do not print */
    , @defragDelay          CHAR(8)         = ''00:00:05''
        /* time to wait between defrag commands */
AS
/********************************************************************
    Name:       dba_indexDefragStandard_sp
 
    Author:     Michelle F. Ufford
 
    Purpose:    Defrags all indexes for the current database
 
    Notes:      This script was designed for SQL Server 2005
                Standard edition.
 
    CAUTION: Monitor transaction log if executing for the first time!
 
      @minFragmentation     defaulted to 10%, will not defrag if
                            fragmentation if less than specified.
 
      @rebuildThreshold     defaulted to 30% as recommended by
                            Microsoft in BOL;
                            > than 30% will result in rebuild instead
 
      @executeSQL           1 = execute the SQL generated by this proc;
                            0 = print command only
 
      @tableName            Specify if you only want to defrag indexes
                            for a specific table
 
      @printCommands        1 = print commands to screen;
                            0 = do not print commands
 
      @printFragmentation   1 = print fragmentation to screen;
                            0 = do not print fragmentation
 
      @defragDelay          time to wait between defrag commands;
                            gives the server some time to catch up
 
    Called by:  SQL Agent Job or DBA
 
    Date        Initials  Description
    ----------------------------------------------------------------
    2008-10-27  MFU       Initial Release
    2008-11-17  MFU       Added page_count to log table
                          , added @printFragmentation option
********************************************************************
    Exec dbo.dba_indexDefragStandard_sp
          @executeSQL         = 1
        , @printCommands      = 1
        , @minFragmentation   = 0
        , @printFragmentation = 1;
********************************************************************/
 
SET NOCOUNT ON;
SET XACT_Abort ON;
 
BEGIN
 
    /* Declare our variables */
    DECLARE   @objectID         INT
            , @indexID          INT
            , @schemaName       NVARCHAR(130)
            , @objectName       NVARCHAR(130)
            , @indexName        NVARCHAR(130)
            , @fragmentation    FLOAT
            , @pageCount        INT
            , @sqlCommand       NVARCHAR(4000)
            , @rebuildCommand   NVARCHAR(200)
            , @dateTimeStart    DATETIME
            , @dateTimeEnd      DATETIME
            , @containsLOB      BIT;
 
    /* Just a little validation... */
    IF @minFragmentation Not Between 0.00 And 100.0
        SET @minFragmentation = 10.0;
 
    IF @rebuildThreshold Not Between 0.00 And 100.0
        SET @rebuildThreshold = 30.0;
 
    IF @defragDelay Not Like ''00:[0-5][0-9]:[0-5][0-9]''
        SET @defragDelay = ''00:00:05'';
 
    /* Determine which indexes to defrag using our
       user-defined parameters */
    SELECT
          OBJECT_ID AS objectID
        , index_id AS indexID
        , avg_fragmentation_in_percent AS fragmentation
        , page_count 
        , 0 AS ''defragStatus''
            /* 0 = unprocessed, 1 = processed */
    INTO #indexDefragList
    FROM sys.dm_db_index_physical_stats
        (DB_ID(), OBJECT_ID(@tableName), NULL , NULL, N''Limited'')
    WHERE avg_fragmentation_in_percent > @minFragmentation
        And index_id > 0
    OPTION (MaxDop 1);
 
    /* Create a clustered index to boost performance a little */
    CREATE CLUSTERED INDEX CIX_temp_indexDefragList
        ON #indexDefragList(objectID, indexID);
 
    /* Begin our loop for defragging */
    WHILE (SELECT COUNT(*) FROM #indexDefragList
            WHERE defragStatus = 0) > 0
    BEGIN
 
        /* Grab the most fragmented index first to defrag */
        SELECT TOP 1
              @objectID         = objectID
            , @fragmentation    = fragmentation
            , @indexID          = indexID
            , @pageCount        = page_count
        FROM #indexDefragList
        WHERE defragStatus = 0
        ORDER BY fragmentation DESC;
 
        /* Look up index information */
        SELECT @objectName = QUOTENAME(o.name)
             , @schemaName = QUOTENAME(s.name)
        FROM sys.objects AS o
        Inner Join sys.schemas AS s
            ON s.schema_id = o.schema_id
        WHERE o.OBJECT_ID = @objectID;
 
        SELECT @indexName = QUOTENAME(name)
        FROM sys.indexes
        WHERE OBJECT_ID = @objectID
            And index_id = @indexID
            And type > 0;
 
        /* Look for LOBs */
        SELECT TOP 1
            @containsLOB = column_id
        FROM sys.columns WITH (NOLOCK)
        WHERE 
            [OBJECT_ID] = @objectID
            And (system_type_id In (34, 35, 99)
            -- 34 = image, 35 = text, 99 = ntext
                    Or max_length = -1);
            -- varbinary(max), varchar(max), nvarchar(max), xml
 
        /* See if we should rebuild or reorganize; handle thusly */
        IF @fragmentation < @rebuildThreshold 
            Or IsNull(@containsLOB, 0) > 0 
            -- Cannot rebuild if the table has one or more LOB
            SET @sqlCommand = N''Alter Index '' + @indexName + N'' On ''
                + @schemaName + N''.'' + @objectName + N'' ReOrganize;''
        ELSE
            SET @sqlCommand = N''Alter Index '' + @indexName + N'' On ''
                + @schemaName + N''.'' + @objectName +  '' Rebuild ''
                + ''With (MaxDop = 1)''; -- minimize impact on server
 
        /* Are we executing the SQL?  If so, do it */
        IF @executeSQL = 1
        BEGIN
 
            /* Grab the time for logging purposes */
            SET @dateTimeStart  = GETDATE();
            EXECUTE (@sqlCommand);
            SET @dateTimeEnd  = GETDATE();
 
            /* Log our actions */
            INSERT INTO dbo.dba_indexDefragLog
            (
                  objectID
                , objectName
                , indexID
                , indexName
                , fragmentation
                , page_count
                , dateTimeStart
                , durationSeconds
            )
            SELECT
                  @objectID
                , @objectName
                , @indexID
                , @indexName
                , @fragmentation
                , @pageCount
                , @dateTimeStart
                , DATEDIFF(SECOND, @dateTimeStart, @dateTimeEnd);
 
            /* Just a little breather for the server */
            WAITFOR Delay @defragDelay;
 
            /* Print if specified to do so */
            IF @printCommands = 1
                PRINT N''Executed: '' + @sqlCommand;
        END
        ELSE
        /* Looks like we''re not executing, just print
            the commands */
        BEGIN
            IF @printCommands = 1
                PRINT @sqlCommand;
        END
 
        /* Update our index defrag list when we''ve
            finished with that index */
        UPDATE #indexDefragList
        SET defragStatus = 1
        WHERE objectID  = @objectID
          And indexID   = @indexID;
 
    END
 
    /* Do we want to output our fragmentation results? */
    IF @printFragmentation = 1
        SELECT idl.objectID
            , o.name As ''tableName''
            , idl.indexID
            , i.name As ''indexName''
            , idl.fragmentation
            , idl.page_count
        FROM #indexDefragList AS idl
        JOIN sys.objects AS o
            ON idl.objectID = o.object_id
        JOIN sys.indexes As i
            ON idl.objectID = i.object_id
            AND idl.indexID = i.index_id;
 
    /* When everything is done, make sure to get rid of
        our temp table */
    DROP TABLE #indexDefragList;
 
    SET NOCOUNT OFF;
    RETURN 0
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_TypographicNumberFio]    Script Date: 05/07/2015 18:13:35 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_TypographicNumberFio]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_Parse_TypographicNumberFio](@sourceString [nvarchar](max), @delimiter [nchar](1))
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[TypographicNumber] [nvarchar](80) NULL,
	[FirstName] [nvarchar](80) NULL,
	[LastName] [nvarchar](80) NULL,
	[MiddleName] [nvarchar](80) NULL,
	[CheckTypographicNumber] [nvarchar](80) NULL,
	[CheckFirstName] [nvarchar](80) NULL,
	[CheckLastName] [nvarchar](80) NULL,
	[CheckMiddleName] [nvarchar](80) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SQLRegEx].[SQLRegEx.StringSplit].[Parse_TypographicNumberFio]' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_LicenseNumberFio]    Script Date: 05/07/2015 18:13:35 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_LicenseNumberFio]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_Parse_LicenseNumberFio](@sourceString [nvarchar](max), @delimiter [nchar](1))
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[LicenseNumber] [nvarchar](80) NULL,
	[FirstName] [nvarchar](80) NULL,
	[LastName] [nvarchar](80) NULL,
	[MiddleName] [nvarchar](80) NULL,
	[CheckLicenseNumber] [nvarchar](80) NULL,
	[CheckFirstName] [nvarchar](80) NULL,
	[CheckLastName] [nvarchar](80) NULL,
	[CheckMiddleName] [nvarchar](80) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SQLRegEx].[SQLRegEx.StringSplit].[Parse_LicenseNumberFio]' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:35 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_FioDocumentNumberSeries]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_Parse_FioDocumentNumberSeries](@sourceString [nvarchar](max), @delimiter [nchar](1))
RETURNS  TABLE (
	[LineNumber] [int] NULL,
	[FirstName] [nvarchar](80) NULL,
	[LastName] [nvarchar](80) NULL,
	[MiddleName] [nvarchar](80) NULL,
	[DocumentSeries] [nvarchar](80) NULL,
	[DocumentNumber] [nvarchar](80) NULL,
	[CheckFirstName] [nvarchar](80) NULL,
	[CheckLastName] [nvarchar](80) NULL,
	[CheckMiddleName] [nvarchar](80) NULL,
	[CheckDocumentSeries] [nvarchar](80) NULL,
	[CheckDocumentNumber] [nvarchar](80) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SQLRegEx].[SQLRegEx.StringSplit].[Parse_FioDocumentNumberSeries]' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateMinimalMarks]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMinimalMarks]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UpdateMinimalMarks]
( @MinimalMarksString varchar(4000))
as
begin
update mm
set minimalmark = nmm.mark
from [MinimalMark] as mm
join GetSubjectMarks(@MinimalMarksString) nmm on nmm.[SubjectId] = mm.[Id] --не очень красиво читается
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[WriteToBatchProcessLog]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WriteToBatchProcessLog]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[WriteToBatchProcessLog]
	@Instance INT,
	@BatchId BIGINT,
	@CheckType INT,
	@IsError BIT,
	@Message VARCHAR(2048)
AS
BEGIN	
	INSERT INTO BatchProcessLog(Instance, BatchId, CheckType, IsError, [Message])
	VALUES (@Instance, @BatchId, @CheckType, @IsError, @Message)
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchExpireDate]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchExpireDate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SearchExpireDate]
as
BEGIN
--добавляем новый срок действия в новом году
insert into [ExpireDate] (
  [Year],
  [ExpireDate]
)
select year(getdate()), cast(cast((year(getdate())+1) as varchar(4)) + ''1231'' as datetime)
where not exists (select top 1 1 from [ExpireDate] ed where ed.year = year(getdate()))

select ed.year, convert(varchar(max), ed.[ExpireDate], 104) ExpireDate
from [ExpireDate] as ed with(nolock)

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchContext]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchContext]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchContext

-- =============================================
-- Поиск контекстов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Приведение к стандарту.
-- =============================================
CREATE proc [dbo].[SearchContext]
as
begin

  select 
    context.Code Code
    , context.[Name] [Name]
  from 
    dbo.Context context with (nolock)

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionType]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionType]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCompetitionType
-- =============================================
-- Процедура поиска типов олимпиад
-- v.1.0. Created by Fomin Dmitriy 23.07.2008
-- v.1.1. Modified by Fomin Dmitriy 25.07.2008
-- Не нужно возвращать пустое значение.
-- =============================================
CREATE procedure [dbo].[SearchCompetitionType]
as
begin
  select
    competition_type.Id
    , competition_type.Name
  from 
    dbo.CompetitionType competition_type
  order by
    [Name]

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateExpireDates]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateExpireDates]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[UpdateExpireDates]
(@ExpireDatesString varchar(max))
as
begin

update ed
set expiredate = convert(datetime, substring(t.value, charindex(''='',t.value)+1, len(t.value)), 104)
from [ExpireDate] as ed 
join getdelimitedvalues(@ExpireDatesString) t on ed.[Year] = substring(t.value, 0, charindex(''='',t.value))
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetShortOrganizationName]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetShortOrganizationName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Получить короткое наименование организации.
-- v.1.0: Created by Fomin Dmitriy 06.05.2008
-- v.1.1: Modified by Fomin Dmitriy 08.05.2008
-- Поле наименования организации увеличено.
-- =============================================
CREATE function [dbo].[GetShortOrganizationName]
  (
  @organizationName nvarchar(2000)
  )
returns nvarchar(2000)
as
begin
  -- Список сокращений.
  declare @word_abbreviation table
    (
    Word nvarchar(255)
    , Abbreviation nvarchar(255)
    )

  insert into @word_abbreviation values (''федеральный'', ''Ф'')
  insert into @word_abbreviation values (''республиканский'', ''Р'')
  insert into @word_abbreviation values (''областной'', ''О'')

  insert into @word_abbreviation values (''государственный'', ''Г'')
  insert into @word_abbreviation values (''негосударственный'', ''НГ'')
  insert into @word_abbreviation values (''образовательное'', ''О'')
  insert into @word_abbreviation values (''учреждение'', ''У'')
  insert into @word_abbreviation values (''высшего'', ''В'')
  insert into @word_abbreviation values (''среднего'', ''С'')
  insert into @word_abbreviation values (''профессионального'', ''П'')
  insert into @word_abbreviation values (''образования'', ''О'')

  insert into @word_abbreviation values (''университет'', ''УНВ'')
  insert into @word_abbreviation values (''академия'', ''АКД'')
  insert into @word_abbreviation values (''институт'', ''ИНС'')
  insert into @word_abbreviation values (''училище'', ''УЧЛ'')
  insert into @word_abbreviation values (''техникум'', ''ТХК'')
  insert into @word_abbreviation values (''колледж'', ''КЛЖ'')

  insert into @word_abbreviation values (''медицинский'', ''МЕДИЦ'')
  insert into @word_abbreviation values (''педагогический'', ''ПЕДАГ'')
  insert into @word_abbreviation values (''правосудия'', ''ПРАВО'')
  insert into @word_abbreviation values (''технический'', ''ТЕХНЧ'')
  insert into @word_abbreviation values (''технологический'', ''ТЕХЛГ'')
  insert into @word_abbreviation values (''политехнический'', ''ПОЛТХ'')
  insert into @word_abbreviation values (''юридический'', ''ЮРИДЧ'')
  insert into @word_abbreviation values (''текстильный'', ''ТЕКСТ'')
  insert into @word_abbreviation values (''сельскохозяйственная'', ''СЕЛХЗ'')
  insert into @word_abbreviation values (''машиностроительный'', ''МАШСТ'')
  insert into @word_abbreviation values (''приборостроительный'', ''ПРБСТ'')
  insert into @word_abbreviation values (''строительный'', ''СТРОЙ'')

  insert into @word_abbreviation values (''министерства'', ''М'')
  insert into @word_abbreviation values (''внутренних дел'', ''ВД'')
  insert into @word_abbreviation values (''юстиции'', ''Ю'')
  insert into @word_abbreviation values (''Российской Федерации'', ''РФ'')
  insert into @word_abbreviation values (''имени'', ''им'')

  -- Разбиение названия организации на слова.
  declare @organization_word table
    (
    Word nvarchar(255)
    )

  declare
    @startIndex int
    , @delimiterIndex int
    , @value nvarchar(4000)

  set @startIndex = 1
  set @delimiterIndex = charindex('' '', isnull(@organizationName, ''''))
  while @delimiterIndex > 0
  begin
    set @value = ltrim(rtrim(substring(@organizationName, @startIndex, @delimiterIndex  - @startIndex)))
    if @value <> ''''
      insert into @organization_word
      select @value
  
    set @startIndex = @delimiterIndex + 1
    set @delimiterIndex = charindex('' '', @organizationName, @startIndex)
  end

  if len(@organizationName) >= @startIndex 
  begin
    set @value = ltrim(rtrim(substring(@organizationName, @startIndex, len(@organizationName) - @startIndex + 1)))
    if @value <> ''''
        insert into @organization_word
        select @value
  end

  -- Вывод результата.
  declare
    @word nvarchar(255)
    , @result nvarchar(2000)
    , @sameLevel decimal(18, 4)
    , @matchCount int

  set @sameLevel = 0.7
  set @matchCount = 3

  set @result = ''''

  declare abbreviation_cursor cursor for
  select
    -- Вывести наилучшее сокращение, если такое есть.
    isnull((select top 1 word_abbreviation.Abbreviation 
        from (select word_abbreviation.Abbreviation 
              , dbo.CompareStrings(organization_word.Word, word_abbreviation.Word, @matchCount) SameLevel
            from @word_abbreviation word_abbreviation) word_abbreviation
        where word_abbreviation.SameLevel >= @sameLevel
        order by word_abbreviation.SameLevel desc), organization_word.Word)
  from @organization_word organization_word

  open abbreviation_cursor 
  fetch next from abbreviation_cursor into @word
  while @@fetch_status = 0
  begin
    if len(@result) > 0
      set @result = @result + '' ''
    set @result = @result + @word

    fetch next from abbreviation_cursor into @word
  end
  close abbreviation_cursor
  deallocate abbreviation_cursor

  return @result
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ReportCnecLoading]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCnecLoading]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[ReportCnecLoading] 
( @type varchar(10) = null)
as
begin

if(@type is null or @type not in (''month'', ''week''))
  set @type = ''month''

select  
  day(n.value(''date[1]'', ''datetime'')) day
, convert(varchar(10), n.value(''date[1]'', ''datetime''), 104) date
, n.value(''cnecNew[1]'', ''int'') cnecNew
, n.value(''cnecUpdated[1]'', ''int'') cnecUpdated
, n.value(''cnecdNew[1]'', ''int'') cnecdNew
, n.value(''cnecdUpdated[1]'', ''int'') cnecdUpdated
from report rp
cross apply rp.xml.nodes(''unit'') r(n)
where name = ''CnecLoading'' + @type 
and rp.created = (select top 1 created from report where name = ''CnecLoading'' + @type order by created desc)  

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchAccount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchAccount

-- =============================================
-- Поиск пользователей горячей линии.
-- v.1.0: Created by Makarev Andrey 09.04.2008
-- v.1.1: Modified by Makarev Andrey 11.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Sedov Anton 16.05.2008
-- добавлен параметр @email
-- =============================================
CREATE proc [dbo].[SearchAccount]
  @groupCode nvarchar(255)
  , @login nvarchar(255) = null
  , @lastName nvarchar(255) = null
  , @isActive bit = null
  , @email nvarchar(255) = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @sortColumn nvarchar(20) = N''login''
  , @sortAsc bit = 1
  , @showCount bit = null
as
begin
  declare 
    @declareCommandText nvarchar(4000)
    , @params nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @userGroupId int
    , @lastNameFormat nvarchar(255)

  if isnull(@lastName, N'''') <> N''''
    set @lastNameFormat = N''%'' + replace(@lastName, N'' '', ''%'') + N''%''

  select
    @userGroupId = [group].[Id]
  from
    dbo.[Group] [group] with (nolock, fastfirstrow)
  where
    [group].Code = @groupCode

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Login nvarchar(255) '' +
      '' , LastName nvarchar(255) '' +
      '' , FirstName nvarchar(255) '' +
      '' , PatronymicName nvarchar(255) '' +
      '' , IsActive bit '' +
      ''   , Email nvarchar(255) '' + 
      '' , Id bigint not null '' +
      '' ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' account.Login Login '' +
        '' , account.LastName LastName '' +
        '' , account.FirstName FirstName '' +
        '' , account.PatronymicName PatronymicName '' +
        '' , account.IsActive IsActive '' +
        ''   , account.Email Email '' +
        '' , account.[Id] '' +
        ''from dbo.Account account with (nolock) '' +
        '' inner join dbo.GroupAccount group_account with (nolock) '' +
        ''   on account.[Id] = group_account.AccountId '' +
        ''where '' +
        '' group_account.GroupId = @userGroupId ''
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.Account account with (nolock, fastfirstrow) '' +
        '' inner join dbo.GroupAccount group_account with (nolock) '' +
        ''   on account.[Id] = group_account.AccountId '' +
        ''where '' + 
        '' group_account.GroupId = @userGroupId '' 
  
  if not @login is null
    set @commandText = @commandText + '' and account.Login = @login ''

  if not @isActive is null
    set @commandText = @commandText + '' and account.IsActive = @isActive ''

  if not @lastName is null
    set @commandText = @commandText + '' and account.LastName like @lastNameFormat ''
  
  if not @email is null
    set @commandText = @commandText + '' and account.Email = @email ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = ''login''
    begin
      set @innerOrder = ''order by Login <orderDirection> ''
      set @outerOrder = ''order by Login <orderDirection> ''
      set @resultOrder = ''order by Login <orderDirection> ''
    end
    else if @sortColumn = ''IsActive''
    begin
      set @innerOrder = ''order by IsActive <orderDirection> ''
      set @outerOrder = ''order by IsActive <orderDirection> ''
      set @resultOrder = ''order by IsActive <orderDirection> ''
    end
    else if @sortColumn = ''name''
    begin
      set @innerOrder = ''order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> ''
      set @outerOrder = ''order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> ''
      set @resultOrder = ''order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> ''
    end
    else if @sortColumn = ''email''
    begin
      set @innerOrder = ''order by Email <orderDirection>, Id <orderDirection> ''
      set @outerOrder = ''order by Email <orderDirection>, Id <orderDirection> ''
      set @resultOrder = ''order by Email <orderDirection>, Id <orderDirection> ''
    end 
    else if @sortColumn = ''Id''
    begin
      set @innerOrder = ''order by Id <orderDirection> ''
      set @outerOrder = ''order by Id <orderDirection> ''
      set @resultOrder = ''order by Id <orderDirection> '' 
    end 
    else 
    begin
      set @innerOrder = ''order by Login <orderDirection> ''
      set @outerOrder = ''order by Login <orderDirection> ''
      set @resultOrder = ''order by Login <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' search.Login '' +
      '' , search.LastName '' +
      '' , search.FirstName '' +
      '' , search.PatronymicName '' +
      '' , search.IsActive '' +
      '' , search.Email '' + 
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 
  
  set @params = 
      ''@userGroupId int '' +
      '', @login nvarchar(255) '' +
      '', @IsActive bit '' + 
      '', @lastNameFormat nvarchar(255) '' +
      '', @email nvarchar(255) '' 
  
  exec sp_executesql @commandText, @params, 
      @userGroupId
      , @login
      , @IsActive
      , @lastNameFormat
      , @email 

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetUserInfo]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operator_GetUserInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'--------------------------------------------
-- Получение информации о пользователе
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE procEDURE [dbo].[Operator_GetUserInfo]
( @OperatorLogin nvarchar(255), 
  @UserLogin nvarchar(255), 
  @IsMainOperator bit out, 
  @MainOperatorName varchar(255) out, 
  @Comments varchar(1024) out)
AS 
  SET NOCOUNT ON
  DECLARE @UserID int, @OperatorID int
  SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin
  SELECT @UserID=ID FROM dbo.Account WHERE [Login]=@UserLogin

  -- вставляем, если нет связи и другим оператором
  INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
  SELECT A.ID CheckedUserID, @OperatorID OperatorID
  FROM dbo.Account A
  LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
  WHERE A.ID=@UserID AND OL.CheckedUserID IS NULL

  -- данные о текущем пользователе
  SELECT 
    @IsMainOperator=CASE WHEN A.ID=@OperatorID THEN 1 ELSE 0 END,
    @MainOperatorName=A.LastName+'' ''+A.FirstName +''(''+A.Login+'')'',
    @Comments=OL.Comments
  FROM dbo.OperatorLog OL
  INNER JOIN dbo.Account A ON OL.OperatorID=A.ID
  WHERE CheckedUserID=@UserID

  PRINT @@ROWCOUNT
  
  -- данные об остальных ''моих'' пользователях
  SELECT A.Login, A.LastName+'' ''+FirstName FIO
  FROM dbo.OperatorLog OL
  INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
  WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID 
    AND A.Status=''consideration''
    AND (Comments IS NULL OR LEN(RTRIM(Comments))=0)
    

  -- данные об остальных ''моих'' пользователях с комментариями
  SELECT A.Login, A.LastName+'' ''+FirstName FIO
  FROM dbo.OperatorLog OL
  INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
  WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID
    AND A.Status=''consideration''
    AND LEN(RTRIM(Comments))>0
' 
END
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetNewUser]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operator_GetNewUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'--------------------------------------------
-- Получение 1-го "не обработанного пользователя"
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE procEDURE [dbo].[Operator_GetNewUser]
( @OperatorLogin nvarchar(255), 
  @UserID int out, 
  @UserLogin nvarchar(255) out
)
AS 
  SET NOCOUNT ON
  DECLARE @OperatorID int
  DECLARE  @T TABLE(CheckedUserID int) 

  SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin

  INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
    OUTPUT INSERTED.CheckedUserID INTO @T(CheckedUserID)
  SELECT TOP 1 
    A.ID CheckedUserID, 
    @OperatorID OperatorID
  FROM dbo.Account A
  INNER JOIN dbo.Organization O ON A.OrganizationID=O.Id AND O.EtalonOrgID IS NOT NULL
  INNER JOIN dbo.GroupAccount GA ON A.ID=GA.AccountId AND GA.GroupId=1
  LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
  WHERE A.Status=''consideration'' AND OL.CheckedUserID IS NULL
  ORDER BY A.CreateDate 

  SELECT TOP 1 @UserID=A.ID, @UserLogin=A.[Login]
  FROM dbo.Account A
  WHERE A.ID IN (SELECT CheckedUserID FROM @T)
' 
END
GO
/****** Object:  StoredProcedure [dbo].[Operator_AddUserComment]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Operator_AddUserComment]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'--------------------------------------------
-- Добавление комментария пользователю
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE procEDURE [dbo].[Operator_AddUserComment]
(@UserLogin nvarchar(255), @Comment varchar(1024))
AS 
  SET NOCOUNT ON

  UPDATE dbo.OperatorLog
  SET Comments=@Comment, DTLastChange=GETDATE()
  WHERE CheckedUserID IN (SELECT ID FROM dbo.Account WHERE [Login]=@UserLogin)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[IsUserBanned]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsUserBanned]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[IsUserBanned]
	(
	@login nvarchar(255)
	)
returns bit 
as  
begin
declare @result bit
set @result = 0
select top 1 @result = IsBanned from Account where [Login] = @login
return @result
	
end

' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountKey]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchAccountKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchAccountKey
-- ====================================================
-- Поиск ключей.
-- v.1.0: Created by Fomin Dmitriy 28.08.2008
-- ====================================================
CREATE procedure [dbo].[SearchAccountKey]
  @login nvarchar(255)
as
begin
  declare
    @now datetime

  set @now = convert(nvarchar(8), GetDate(), 112)

  select
    account_key.[Key]
    , account_key.DateFrom
    , account_key.DateTo
    , account_key.IsActive
  from dbo.Account account
    inner join dbo.AccountKey account_key
      on account.Id = account_key.AccountId
  where
    account.Login = @login
  order by
    case
      when @now < account_key.DateFrom then 2
      when @now > account_key.DateTo then 1
      else 0
    end asc

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ReportUserRegistration]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserRegistration]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение отчета о регистрации пользователей.
-- =============================================
CREATE procedure [dbo].[ReportUserRegistration]
as
begin

DECLARE @StartDate DATETIME
SET @StartDate= ''2010-05-15'' -- dateadd(month, -1, getdate())

SELECT 
DAY(UpdateDay) AS [Day]
--, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(@MonthAgo))+''/''+CONVERT(NVARCHAR(50),MONTH(GETDATE()))+''/''+CONVERT(NVARCHAR(50),UpdateDay)) AS [date]
, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(UpdateDay))+''-''+CONVERT(NVARCHAR(50),MONTH(UpdateDay))+''-''+CONVERT(NVARCHAR(50),DAY(UpdateDay))) AS [date]
, SUM([Активирован]) AS [activated]
, SUM([На регистрации])  AS [registration]
, SUM([На доработке]) AS [revision]
, SUM([На согласовании]) AS [consideration]
, SUM([Отключен])AS [deactivated]


FROM(
  SELECT 
    CONVERT(NVARCHAR(4),YEAR(F.UpdateDate))+''-''+CONVERT(NVARCHAR(2),MONTH(F.UpdateDate))+''-''+CONVERT(NVARCHAR(2),DAY(F.UpdateDate)) 
  AS UpdateDay,
    case when F.[Status]=''activated'' then 1 else 0 end AS [Активирован],
    case when F.[Status]=''registration'' then 1 else 0 end AS [На регистрации],
    case when F.[Status]=''revision'' then 1 else 0 end AS [На доработке],
    case when F.[Status]=''consideration'' then 1 else 0 end AS [На согласовании],
    case when F.[Status]=''deactivated'' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID,UpdateDate,[Status]
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR ([Status]=''registration'' and VersionId=1)) AND UpdateDate >= @StartDate 
  ) F ON A.ID=F.AccountID
) T  
GROUP BY UpdateDay
ORDER BY [date]
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCertificateLoadTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCertificateLoadTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--Отчет: Ежедневный отчет о загрузке сертификатов
CREATE funCTION [dbo].[ReportCertificateLoadTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [Код региона] nvarchar(10) null,
  [Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств] int null,
[Новых свидетельств] int null,
[Обновлено свидетельств] int null,
[Всего аннулированных свидетельств] int null,
[Новых аннулированных свидетельств] int null,
[Обновлено аннулированных свидетельств] int null
)
as
begin

insert into @report
select
  r.code [Код региона]
  , r.name [Регион]
, (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2008) 
[Всего свидетельств за 2008]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2009) 
[Всего свидетельств за 2009]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2010) 
[Всего свидетельств за 2010]  
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id and c.year = 2011) 
[Всего свидетельств за 2011]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.regionid = r.id) 
[Всего свидетельств]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.createdate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[Новых свидетельств]
  , (select count(*) from CommonNationalExamCertificate c with(nolock) where c.updatedate <> c.createdate and c.updatedate BETWEEN @periodBegin and @periodEnd and c.regionid = r.id) 
[Обновлено свидетельств]
  , (select count(*) from CommonNationalExamCertificateDeny d with(nolock)
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where c.regionid = r.id) 
[Всего аннулированных свидетельств]
  , (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[Новых аннулированных свидетельств]
  , (select count(*) from CommonNationalExamCertificateDeny d with(nolock) 
join CommonNationalExamCertificate c with(nolock) on d.year = c.year and d.certificatenumber = c.number
where d.createdate <> d.updatedate AND d.updatedate BETWEEN @periodBegin and @periodEnd AND c.regionid = r.id) 
[Обновлено аннулированных свидетельств]
from dbo.Region r with (nolock)
where r.InCertificate = 1
order by r.id asc


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '''', ''Итого за '' + case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end
,sum([Всего свидетельств за 2008])
,sum([Всего свидетельств за 2009])
,sum([Всего свидетельств за 2010])
,sum([Всего свидетельств за 2011])
,sum([Всего свидетельств])
,sum([Новых свидетельств])
,sum([Обновлено свидетельств])
,sum([Всего аннулированных свидетельств])
,sum([Новых аннулированных свидетельств])
,sum([Обновлено аннулированных свидетельств])
from @report


return
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCertificateLoadShortTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCertificateLoadShortTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--Отчет: Общий отчет о загрузке сертификатов
CREATE funCTION [dbo].[ReportCertificateLoadShortTVF]()
RETURNS @report TABLE 
(
  [Код региона] nvarchar(10) null,
  [Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств за 2012] int null,
[Всего свидетельств] int null
)
as
begin
declare @PreResult table 
(
  [Код региона] nvarchar(10) null,
  [Регион] nvarchar(100) null,
[Всего свидетельств за 2008] int null,
[Всего свидетельств за 2009] int null,
[Всего свидетельств за 2010] int null,
[Всего свидетельств за 2011] int null,
[Всего свидетельств за 2012] int null,
[Всего свидетельств] int null
)
insert into @PreResult
select
  replace(r.REGION,1000,''-'') [Код региона]
  , r.RegionName [Регион]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2008) 
[Всего свидетельств за 2008]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2009) 
[Всего свидетельств за 2009]  
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2010) 
[Всего свидетельств за 2010]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2011) 
[Всего свидетельств за 2011]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION and c.UseYear = 2012) 
[Всего свидетельств за 2012]
  , (select count(*) from prn.Certificates c with(nolock) where c.REGION = r.REGION) 
[Всего свидетельств]
  
from rbdc.Regions r with (nolock)
--where r.InCertificate = 1


insert into @report
select * from @PreResult 
union
select ''Всего'',
''-'',
SUM([Всего свидетельств за 2008]),
SUM([Всего свидетельств за 2009]),
SUM([Всего свидетельств за 2010]),
SUM([Всего свидетельств за 2011]),
SUM([Всего свидетельств за 2012]),
SUM([Всего свидетельств])
from @PreResult
order by [Код региона]

return
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetNewUserLogin]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewUserLogin]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetNewUserLogin
-- =============================================
-- Генерируется новый логин пользователя. 
-- Логин генерируется в формате user<1..100000>''
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.03.2008
-- Приведение к стандарту
-- =============================================
CREATE proc [dbo].[GetNewUserLogin]
  @login nvarchar(255) output
as
begin
  declare 
    @newLogin nvarchar(255)
    , @needNewLogin int
  set @needNewLogin = 1
  while @needNewLogin = 1 begin
    set @newLogin = N''user'' + convert(nvarchar, Floor(Rand(CheckSum(NewId())) * 100000))
    if not exists(select 1 
            from
              dbo.Account account with (nolock, fastfirstrow)
            where
              account.[Login] = @newLogin)
      set @needNewLogin = 0
  end
  
  set @login = @newLogin

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetNews]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetNews

-- =============================================
-- Получить детальную информацию о новости.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим наименование новости.
-- =============================================
CREATE proc [dbo].[GetNews]
  @id bigint
as
begin
  declare @internalId bigint

  set @internalId = dbo.GetInternalId(@id)

  select
    @id [Id]
    , news.Date Date
    , news.Description Description
    , news.[Text] [Text]
    , news.IsActive IsActive
    , news.[Name] [Name]
  from 
    dbo.News news with (nolock, fastfirstrow)
  where
    news.[Id] = @internalId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetDelivery]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[GetDelivery]
  @id bigint
as
begin
  select
    @id [Id]
    , delivery.Title Title
    , delivery.[Message] [Message]
    , delivery.[CreateDate] [CreateDate]
    , delivery.DeliveryDate DeliveryDate
    , delivery.TypeCode TypeCode
  from 
    dbo.Delivery delivery with (nolock, fastfirstrow)
  where
    delivery.[Id] = @id

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAccount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:    Сулиманов А.М.
-- Create date: 2009-05-07
-- Description: Удаление из БД всего, что касается AccountId (не анализируются связи)
-- =============================================
CREATE procEDURE [dbo].[DeleteAccount]
(@AccountID int)
AS
BEGIN
  SET NOCOUNT ON;
  
  DELETE FROM dbo.AccountIp WHERE AccountId=@AccountID
  DELETE FROM dbo.AccountKey WHERE AccountId=@AccountID
  DELETE FROM dbo.AccountLog WHERE AccountId=@AccountID
  DELETE FROM dbo.AccountRoleActivity WHERE AccountId=@AccountID
  DELETE FROM dbo.GroupAccount WHERE AccountId=@AccountID
  DELETE FROM dbo.Organization WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Id=@AccountID)
  DELETE FROM dbo.UserAccountPassword WHERE AccountId=@AccountID
  DELETE FROM dbo.Account WHERE Id=@AccountID
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[RefreshRoleActivity]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshRoleActivity]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Обновить активность ролей.
-- v.1.0: Created by Fomin Dmitriy 13.06.2008
-- v.1.1: Modified by Makarev Andrey 23.06.2008
-- Добавлен параметр @accountLogin.
-- =============================================
CREATE proc [dbo].[RefreshRoleActivity]
  @accountId bigint = null
  , @accountLogin nvarchar(255) = null
as
begin
  declare
    @checkAccountId bigint
    , @checkRoleId int
    , @condition nvarchar(max)
    , @commandText nvarchar(max)

  declare @checkingAccount table
    (
    AccountId bigint
    , UpdateDate datetime
    )
  
  if @accountId is null
  begin
    if @accountLogin is null
      insert into @checkingAccount
      select
        account.Id
        , Account.UpdateDate
      from 
        dbo.Account account
      where 
        account.IsActive = 1
    else
      insert into @checkingAccount
      select
        account.Id
        , Account.UpdateDate
      from 
        dbo.Account account
      where 
        account.IsActive = 1
        and account.Login = @accountLogin
  end
  else
  begin
    if @accountLogin is null
      insert into @checkingAccount
      select
        account.Id
        , account.UpdateDate
      from dbo.Account account
      where account.IsActive = 1
        and account.Id = @accountId
    else
      insert into @checkingAccount
      select
        account.Id
        , account.UpdateDate
      from 
        dbo.Account account
      where 
        account.IsActive = 1
        and account.Id = @accountId
        and account.Login = @accountLogin       
  end

  create table #Activity 
    (
    AccountId bigint
    , RoleId int
    )

  declare activity_cursor cursor forward_only for 
  select
    account_role.AccountId
    , account_role.RoleId
    , account_role.IsActiveCondition
  from dbo.AccountRole account_role 
  where not account_role.IsActiveCondition is null
    and account_role.AccountId in (select AccountId from @checkingAccount)

  open activity_cursor
  fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
  while @@fetch_status <> -1
  begin
    set @commandText = replace(replace(replace(
      ''insert into #Activity 
      select
        activity.AccountId
        , <roleId> RoleId
      from (select 
          account.Id AccountId
          , case
            when <condition> then 1
            else 0
          end IsActive
        from dbo.Account account 
        where account.Id = <accountId>) activity 
      where activity.IsActive = 1 ''
      , ''<accountId>'', @checkAccountId)
      , ''<roleId>'', @checkRoleId)
      , ''<condition>'', @condition)

    exec (@commandText)

    fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
  end
  close activity_cursor
  deallocate activity_cursor

  if exists(select 1
      from (select
            account_activity.RoleId
            , account_activity.AccountId
          from dbo.AccountRoleActivity account_activity
          where account_activity.AccountId in (select AccountId from @checkingAccount)) account_activity
        full outer join #Activity activity
          on account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId)
  begin
    begin tran activity
      delete account_activity
      from dbo.AccountRoleActivity account_activity
      where 
        account_activity.AccountId in (select AccountId from @checkingAccount)
        and not exists(select 1
          from #Activity activity
          where account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId)

      update account_activity
      set UpdateDate = GetDate()
      from dbo.AccountRoleActivity account_activity with(rowlock)
        inner join @checkingAccount account
          on account.AccountId = account_activity.AccountId
        inner join #Activity activity
          on account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId
      where
        account.UpdateDate > account_activity.UpdateDate

      insert into dbo.AccountRoleActivity
        (
        CreateDate
        , UpdateDate
        , AccountId
        , RoleId
        )
      select
        GetDate()
        , GetDate()
        , activity.AccountId
        , activity.RoleId
      from #Activity activity
      where not exists(select 1
          from dbo.AccountRoleActivity account_activity
          where account_activity.RoleId = activity.RoleId
            and account_activity.AccountId = activity.AccountId)
    commit tran activity
  end

  drop table #Activity 

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateByNumberCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1  -- пейджинг
  , @maxRowCount int = null -- пейджинг
  , @showCount bit = null   -- если > 0, то выбирается общее кол-во
  ,@type int
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
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = ''Administrator''
        where a2.[Login] = @login )
    set @accountId = null
  else 
    set @accountId = isnull(
      (select account.[Id] 
      from dbo.Account account with (nolock, fastfirstrow) 
      where account.[Login] = @login), 0)

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''CreateDate''
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table 
        ( 
        Id bigint 
        , CreateDate datetime 
        , IsProcess bit 
        , IsCorrect bit 
        , Login varchar(255) 
        , Total int
        , Found int
        ,outerId bigint
        ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> 
          b.Id Id 
          , b.CreateDate CreateDate 
          , b.IsProcess IsProcess 
          , b.IsCorrect IsCorrect 
          , a.login Login 
          , (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
          , (select count(SourceCertificateId) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
          ,outerId
        from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
        left join account a on a.id = b.OwnerAccountId 
        where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 
        and type=<@type>'' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) '' +
        ''where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 and type=<@type>'' 
set @commandText=REPLACE(@commandText,''<@type>'',@type)
  if isnull(@showCount, 0) = 0
  begin
    set @innerOrder = ''order by CreateDate <orderDirection> ''
    set @outerOrder = ''order by CreateDate <orderDirection> ''
    set @resultOrder = ''order by CreateDate <orderDirection> ''

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select 
        dbo.GetExternalId(s.Id) Id 
        , s.Login 
        , s.CreateDate 
        , s.IsProcess 
        , s.IsCorrect 
        , s.Total
        , s.Found
        ,outerId
      from @search s '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@accountId bigint''
    , @accountId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckBatch]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCommonNationalExamCertificateCheckBatch

-- =============================================
-- Получить список пакетных проверок.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateCheckBatch]
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
        join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = ''Administrator''
        where a2.[Login] = @login )
    set @accountId = null
  else 
    set @accountId = isnull(
      (select account.[Id] 
      from dbo.Account account with (nolock, fastfirstrow) 
      where account.[Login] = @login), 0)

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''CreateDate''
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table 
        ( 
        Id bigint 
        , CreateDate datetime 
        , IsProcess bit 
        , IsCorrect bit 
        , Login varchar(255) 
        , Total int
        , Found int
        ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> 
          b.Id Id 
          , b.CreateDate CreateDate 
          , b.IsProcess IsProcess 
          , b.IsCorrect IsCorrect 
          , a.login Login 
          , (select count(*) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Total
          , (select count(SourceCertificateIdGuid) from CommonNationalExamCertificateCheck c with(nolock) where c.batchid = b.id) Found
        from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) 
        left join account a on a.id = b.OwnerAccountId 
        where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.CommonNationalExamCertificateCheckBatch b with (nolock) '' +
        ''where case when @accountId is null then 1 when b.OwnerAccountId = @accountId then 1 else 0 end=1 '' 

  if isnull(@showCount, 0) = 0
  begin
    set @innerOrder = ''order by CreateDate <orderDirection> ''
    set @outerOrder = ''order by CreateDate <orderDirection> ''
    set @resultOrder = ''order by CreateDate <orderDirection> ''

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select 
        dbo.GetExternalId(s.Id) Id 
        , s.Login 
        , s.CreateDate 
        , s.IsProcess 
        , s.IsCorrect 
        , s.Total
        , s.Found
      from @search s '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@accountId bigint''
    , @accountId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение списка ошибок задания на загрузку закрытий сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateDenyLoadingTaskError] 
  @taskId bigint = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
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
    , @internalTaskId bigint
    , @server nvarchar(30)

  set @internalTaskId = dbo.GetInternalId(@taskId)

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''Date''
  set @sortAsc = 1

  select @server = (select top 1 ss.name + ''.fbs_loader_db'' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = ''loader'')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''declare @search table '' +
      N''  ( '' +
      N''  TaskId bigint '' +
      N''  , Date datetime '' +
      N''  , RowIndex bigint '' +
      N''  , Error ntext '' +
      N''  ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N''select <innerHeader> '' +
        N''  cne_certificate_deny_loading_task_error.TaskId TaskId '' +
        N''  , cne_certificate_deny_loading_task_error.Date Date '' +
        N''  , cne_certificate_deny_loading_task_error.RowIndex RowIndex '' +
        N''  , cne_certificate_deny_loading_task_error.Error Error '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error with (nolock) '' +
        N''where 1 = 1''
  else
    set @commandText = 
        N''select count(*) '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateDenyLoadingTaskError cne_certificate_deny_loading_task_error with (nolock) '' +
        N''where 1 = 1''

  if not @taskId is null
    set @commandText = @commandText + N'' and cne_certificate_deny_loading_task_error.TaskId = @internalTaskId ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N''Date''
    begin
      set @innerOrder = ''order by Date <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection> ''
    end
    else 
    begin
      set @innerOrder = ''order by Date <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N''top 10000000''
      set @outerSelectHeader = N''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N''select '' +
      N''  dbo.GetExternalId(search.TaskId) TaskId '' +
      N''  , search.Date Date '' +
      N''  , search.RowIndex RowIndex '' +
      N''  , search.Error Error '' +
      N''from '' +
      N''  @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@internalTaskId bigint''
    , @internalTaskId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateLoadingTaskError]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение списка ошибок задания на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateLoadingTaskError]
  @taskId bigint = null
  , @startRowIndex int = null
  , @maxRowCount int = null
  , @showCount bit = null
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
    , @internalTaskId bigint
    , @server nvarchar(30)

  set @internalTaskId = dbo.GetInternalId(@taskId)

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''Date''
  set @sortAsc = 1

  select @server = (select top 1 ss.name + ''.fbs_loader_db'' from task_db..SystemServer ss
    join sys.servers s on s.name = ss.name
    where rolecode = ''loader'')

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''declare @search table '' +
      N''  ( '' +
      N''  TaskId bigint '' +
      N''  , Date datetime '' +
      N''  , RowIndex bigint '' +
      N''  , Error ntext '' +
      N''  ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        N''select <innerHeader> '' +
        N''  cne_certificate_loading_task_error.TaskId TaskId '' +
        N''  , cne_certificate_loading_task_error.Date Date '' +
        N''  , cne_certificate_loading_task_error.RowIndex RowIndex '' +
        N''  , cne_certificate_loading_task_error.Error Error '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error with (nolock) '' +
        N''where 1 = 1''
  else
    set @commandText = 
        N''select count(*) '' +
        N''from '' + @server + N''.dbo.CommonNationalExamCertificateLoadingTaskError cne_certificate_loading_task_error with (nolock) '' +
        N''where 1 = 1''

  if not @taskId is null
    set @commandText = @commandText + N'' and cne_certificate_loading_task_error.TaskId = @internalTaskId ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = N''Date''
    begin
      set @innerOrder = ''order by Date <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection> ''
    end
    else 
    begin
      set @innerOrder = ''order by Date <orderDirection> ''
      set @outerOrder = ''order by Date <orderDirection> ''
      set @resultOrder = ''order by Date <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
    end
    else
    begin
      set @innerOrder = replace(replace(@innerOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
      set @outerOrder = replace(replace(@outerOrder, ''<orderDirection>'', ''asc''), ''<backOrderDirection>'', ''desc'')
      set @resultOrder = replace(replace(@resultOrder, ''<orderDirection>'', ''desc''), ''<backOrderDirection>'', ''asc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(N''top <count>'', N''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = N''top 10000000''
      set @outerSelectHeader = N''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    N''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      N''select '' +
      N''  dbo.GetExternalID(search.TaskId) TaskId '' +
      N''  , search.Date Date '' +
      N''  , search.RowIndex RowIndex '' +
      N''  , search.Error Error '' +
      N''from '' +
      N''  @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@internalTaskId bigint''
    , @internalTaskId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequestBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCommonNationalExamCertificateRequestBatch

-- =============================================
-- Получить список пакетных запросов сертификатов.
-- v.1.0: Created by Makarev Andrey 05.05.2008
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateRequestBatch]
	@login nvarchar(255)
	, @startRowIndex int = 1
	, @maxRowCount int = null
	, @showCount bit = null
	, @isTypographicNumber bit = 0
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(max)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortColumn nvarchar(20)
		, @sortAsc bit
		, @accountId bigint

	set @accountId = isnull(
		(select account.[Id] 
		 from dbo.Account account with (nolock, fastfirstrow) 
		 where account.[Login] = @login), 0)

	if exists ( select top 1 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = ''Administrator''
				where a2.[Login] = @login )
		set @accountId = null

	set @declareCommandText = ''''
	set @commandText = ''''
	set @viewCommandText = ''''
	set @sortColumn = N''CreateDate''
	set	@sortAsc = 0

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			''declare @search table 
			( 
				Id bigint 
				, CreateDate datetime 
				, IsProcess bit 
				, IsCorrect bit 
				, Login varchar(255) 
				, Year int
				, Total int
				, Found int
			) '' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				''select <innerHeader> 
					rb.Id 
					, rb.CreateDate 
					, rb.IsProcess  
					, rb.IsCorrect  
					, a.login 
					, rb.year
					, (select count(*) from CommonNationalExamCertificateRequest r with(nolock) where r.batchid = rb.id) Total
					, (select count(ParticipantID) from CommonNationalExamCertificateRequest r with(nolock) where r.batchid = rb.id) Found
				from dbo.CommonNationalExamCertificateRequestBatch rb with (nolock) 
				left join account a on a.id = rb.OwnerAccountId 
				where rb.OwnerAccountId = isnull(@accountId, rb.OwnerAccountId) 
				and rb.IsTypographicNumber = @isTypographicNumber ''
	else
		set @commandText = 
				''select count(*) '' +
				''from dbo.CommonNationalExamCertificateRequestBatch rb with (nolock) '' +
				''where rb.OwnerAccountId = isnull(@accountId, rb.OwnerAccountId) '' +
				''and rb.IsTypographicNumber = @isTypographicNumber ''

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = ''CreateDate''
		begin
			set @innerOrder = ''order by CreateDate <orderDirection> ''
			set @outerOrder = ''order by CreateDate <orderDirection> ''
			set @resultOrder = ''order by CreateDate <orderDirection> ''
		end
		else
		begin
			set @innerOrder = ''order by CreateDate <orderDirection> ''
			set @outerOrder = ''order by CreateDate <orderDirection> ''
			set @resultOrder = ''order by CreateDate <orderDirection> ''
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
			set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
			set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
			set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
			set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
			set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = ''top 10000000''
			set @outerSelectHeader = ''top 10000000''
		end

		set @commandText = replace(replace(replace(
				N''insert into @search '' +
				N''select <outerHeader> * '' + 
				N''from (<innerSelect>) as innerSelect '' + @outerOrder
				, N''<innerSelect>'', @commandText + @innerOrder)
				, N''<innerHeader>'', @innerSelectHeader)
				, N''<outerHeader>'', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		''option (keepfixed plan) ''

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			''select 
				dbo.GetExternalId(s.Id) Id 
				, s.Login 
				, s.CreateDate 
				, s.IsProcess 
				, s.IsCorrect 
				, s.Year
				, s.Total
				, s.Found
			from @search s '' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N''@accountId bigint, @isTypographicNumber bit''
		, @accountId
		, @isTypographicNumber

	return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionCertificateRequestBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCompetitionCertificateRequestBatch
-- =====================================================
-- Процедура поиска пакетов проверки медалистов
-- v.1.0: Created by Sedov Anton 30.07.2008
-- v.1.1: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- =====================================================
CREATE procedure [dbo].[SearchCompetitionCertificateRequestBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
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

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''CreateDate''
  set @sortAsc = 0

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''
      declare @search table
        (
        Id bigint
        , CreateDate datetime
        , IsProcess bit 
        , IsCorrect bit
        )
        ''

  if isnull(@showCount, 0) = 0 
    set @commandText = 
      N''
      select <innerHeader>
        batch.Id
        , batch.CreateDate
        , batch.IsProcess
        , batch.IsCorrect
      from 
        dbo.CompetitionCertificateRequestBatch batch
      where 
        batch.OwnerAccountId = @accountId 
      ''
  else 
    set @commandText = 
      N''
      select count(*)
      from dbo.CompetitionCertificateRequestBatch batch
      where batch.OwnerAccountId = @accountId
      ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = ''CreateDate''
    begin
      set @innerOrder = ''order by CreateDate <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection> ''
    end
    else
    begin
      set @innerOrder = ''order by CreateDate <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' dbo.GetExternalId(search.Id) Id '' +
      '' , search.CreateDate '' +
      '' , search.IsProcess '' +
      '' , search.IsCorrect '' +
      ''from @search search '' + @resultOrder
  
  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@accountId bigint''
    , @accountId
  
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificateRequest]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionCertificateRequest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCompetitionCertificateRequest
-- ========================================================
-- Получить список проверенных сертификатов  
-- олимпиадников пакета
-- v.1.0: Created by Sedov Anton 15.08.2008
-- v.1.1: Modified by Sedov Anton 18.08.2008
-- Добавлено поле IsExist
-- v.1.2: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- ========================================================
CREATE procedure [dbo].[SearchCompetitionCertificateRequest]
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.CompetitionCertificateRequestBatch batch with (nolock, fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on batch.OwnerAccountId = account.[Id]
      where 
        batch.Id = @internalBatchId
        and batch.IsProcess = 0
        and account.[Login] = @login)
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @viewSelectCommandText nvarchar(4000)
    , @viewSelectPivot1CommandText nvarchar(4000)
    , @viewSelectPivot2CommandText nvarchar(4000)
    , @pivotSubjectColumns nvarchar(4000)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @viewSelectCommandText = ''''
  set @viewSelectPivot1CommandText = ''''
  set @viewSelectPivot2CommandText = ''''
  set @pivotSubjectColumns = ''''
  set @sortColumn = N''LastName''
  set @sortAsc = 0

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' CompetitionTypeId int '' +
      '' , Name nvarchar(255) '' +
      '' , LastName nvarchar(255) '' +
      '' , FirstName nvarchar(255) '' +
      '' , PatronymicName nvarchar(255) '' +
      '' , Degree nvarchar(255) '' +
      '' , RegionName nvarchar(255) '' +
      '' , City nvarchar(255) '' + 
      '' , School nvarchar(255) '' +
      '' , Class nvarchar(255) '' +
      '' , IsExist bit '' + 
      '' ) '' 

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' competition_certificate.CompetitionTypeId'' +
        '' , competition_type.[Name] '' +
        '' , competition_certificate_request.LastName '' +
        '' , competition_certificate_request.FirstName '' +
        '' , competition_certificate_request.PatronymicName '' +
        '' , competition_certificate.Degree '' +
        '' , region.[Name] RegionName '' +
        '' , competition_certificate.City '' +
        '' , competition_certificate.School '' +
        '' , competition_certificate.Class '' +
        '' , case '' +
        ''   when competition_certificate.Id is null then 0 '' +
        ''   else 1 '' + 
        '' end IsExist '' +  
        ''from '' + 
        '' dbo.CompetitionCertificateRequest competition_certificate_request '' + 
        ''   left join dbo.CompetitionCertificate competition_certificate '' +
        ''     left join dbo.CompetitionType competition_type '' + 
        ''       on competition_certificate.CompetitionTypeId = competition_type.Id '' +
        ''     left join dbo.Region region '' + 
        ''       on competition_certificate.RegionId = region.Id '' + 
        ''     on competition_certificate_request.SourceCertificateId = competition_certificate.Id '' + 
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.CompetitionCertificateRequest competition_certificate_request with (nolock) '' +
        ''where 1 = 1 '' 

  set @commandText = @commandText + 
    '' and competition_certificate_request.BatchId = @internalBatchId ''

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = ''order by LastName <orderDirection> ''
      set @outerOrder = ''order by LastName <orderDirection> ''
      set @resultOrder = ''order by LastName <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' search.CompetitionTypeId CompetitionTypeId '' +
      '' , search.[Name] CompetitiontypeName '' + 
      '' , search.LastName LastName '' +
      '' , search.FirstName FirstName '' + 
      '' , search.PatronymicName PatronymicName '' + 
      '' , search.Degree Degree '' + 
      '' , search.RegionName RegionName '' + 
      '' , search.City City '' + 
      '' , search.School School '' +
      '' , search.Class Class '' +  
      '' , search.IsExist '' + 
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@internalBatchId bigint''
    , @internalBatchId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchSchoolLeavingCertificateCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchSchoolLeavingCertificateCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchSchoolLeavingCertificateCheckBatch
-- ===================================================
-- Поиск пакетов проверки аттестатов 
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ===================================================
CREATE procedure [dbo].[SearchSchoolLeavingCertificateCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
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

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''CreateDate''
  set @sortAsc = 0

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''
      declare @search table
        (
        Id bigint
        , CreateDate datetime
        , IsProcess bit 
        , IsCorrect bit
        )
        ''

  if isnull(@showCount, 0) = 0 
    set @commandText = 
      N''
      select <innerHeader>
        schoolleaving_certificate_check_batch.Id
        , schoolleaving_certificate_check_batch.CreateDate
        , schoolleaving_certificate_check_batch.IsProcess
        , schoolleaving_certificate_check_batch.IsCorrect
      from 
        dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch
      where 
        schoolleaving_certificate_check_batch.OwnerAccountId = @accountId 
      ''
  else 
    set @commandText = 
      N''
      select count(*)
      from dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch
      where schoolleaving_certificate_check_batch.OwnerAccountId = @accountid
      ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = ''CreateDate''
    begin
      set @innerOrder = ''order by CreateDate <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection> ''
    end
    else
    begin
      set @innerOrder = ''order by CreateDate <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' dbo.GetExternalId(search.Id) Id '' +
      '' , search.CreateDate '' +
      '' , search.IsProcess '' +
      '' , search.IsCorrect '' +
      ''from @search search '' + @resultOrder
  
  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@accountId bigint''
    , @accountId
  
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchSchoolLeavingCertificateCheck]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchSchoolLeavingCertificateCheck]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchSchoolLeavingCertificateCheck
-- ==============================================
-- Получение списка проверенных аттестатов
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ==============================================
CREATE procedure [dbo].[SearchSchoolLeavingCertificateCheck]
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null 
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.SchoolLeavingCertificateCheckBatch schoolleaving_certificate_check_batch with (nolock, fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on schoolleaving_certificate_check_batch.OwnerAccountId = account.[Id]
      where 
        schoolleaving_certificate_check_batch.Id = @internalBatchId
          and schoolleaving_certificate_check_batch.IsProcess = 0
          and account.[Login] = @login)
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''Id''
  set @sortAsc = 0
  
  
  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Id bigint '' +
      '' , CertificateNumber nvarchar(255) '' +
      '' , IsDeny bit'' +
      '' , DenyComment ntext '' +
      '' ) ''

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' schoolleaving_certificate_check.Id '' +
        '' , schoolleaving_certificate_check.CertificateNumber '' +
        '' , case when school_leaving_certificate_deny.Id is null '' +
        ''     then 0 '' +
        ''   else 1 '' +
        '' end IsDeny '' + 
        '' , school_leaving_certificate_deny.Comment '' + 
        ''from dbo.SchoolLeavingCertificateCheck schoolleaving_certificate_check with (nolock) '' +
        '' left join dbo.SchoolLeavingCertificateDeny school_leaving_certificate_deny with(nolock) '' +
        ''   on school_leaving_certificate_deny.Id = schoolleaving_certificate_check.SourceCertificateDenyId '' +   
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.SchoolLeavingCertificateCheck schoolleaving_certificate_check with (nolock) '' +
        ''where 1 = 1 '' 

  
  set @commandText = @commandText + 
    '' and schoolleaving_certificate_check.BatchId = @internalBatchId '' 

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = ''order by Id <orderDirection> ''
      set @outerOrder = ''order by Id <orderDirection> ''
      set @resultOrder = ''order by Id <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' search.CertificateNumber CertificateNumber'' +
      '' , search.IsDeny IsDeny '' + 
      '' , search.DenyComment DenyComment '' + 
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@internalBatchId bigint''
    , @internalBatchId
  
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchSameUserAccount]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchSameUserAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchSameUserAccount

-- =============================================
-- Поиск похожих учетных записей.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Поле наименования организации увеличено.
-- =============================================
CREATE procedure [dbo].[SearchSameUserAccount]
  @organizationName nvarchar(2000)
as
begin
  declare
    @userGroupId bigint
    , @currentYear int
    , @sameLavel decimal(18, 4)
    , @matchCount int
    , @shortOrganizationName nvarchar(2000)

  select 
    @userGroupId = [group].Id
  from 
    dbo.[Group] [group]
  where 
    [group].Code = ''User''

  set @currentYear = Year(GetDate())
  set @sameLavel = 0.7
  set @matchCount = 3
  set @shortOrganizationName = dbo.GetShortOrganizationName(@organizationName)

  select top 100
    search.[Login]
    , search.OrganizationName
    , search.LastName 
    , search.Status 
  from (select
      search.[Login]
      , search.OrganizationName
      , search.LastName 
      , search.Status 
      , dbo.CompareStrings(search.ShortOrganizationName
          , @shortOrganizationName, @matchCount) SameLevel
    from (select
        account.[Login] [Login]
        , account.LastName LastName 
        , dbo.GetUserStatus(account.ConfirmYear, account.Status
              , @currentYear, account.RegistrationDocument) Status 
        , Organization.[Name] OrganizationName
        , Organization.[ShortName] ShortOrganizationName
      from 
        dbo.Account account with (nolock)
          inner join dbo.Organization organization with (nolock)
            on organization.Id = account.OrganizationId
      where 
        account.IsActive = 1
        and account.Id in (select group_account.AccountId
            from dbo.GroupAccount group_account with (nolock)
            where group_account.GroupId = @userGroupId)) search) search
  where
    search.SameLevel >= @sameLavel
  order by
    search.SameLevel desc

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchRegion]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchRegion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SearchRegion]
as
begin
  select
    region.[Id] RegionId
    , region.[Name] [Name]
  from dbo.Region region
  where region.InOrganization = 1
  order by region.[Name] --region.SortIndex

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchEntrantCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEntrantCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchEntrantCheckBatch
-- ===============================================
-- Процедура поиска пакетов  проверки абитриентов
-- v.1.0: Created by Sedov Anton 09.07.2008
-- ===============================================
CREATE procedure [dbo].[SearchEntrantCheckBatch]
  @login nvarchar(255)
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
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

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''CreateDate''
  set @sortAsc = 0

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      N''
      declare @search table
        (
        Id bigint
        , CreateDate datetime
        , IsProcess bit 
        , IsCorrect bit
        )
        ''

  if isnull(@showCount, 0) = 0 
    set @commandText = 
      N''
      select <innerHeader>
        entrant_check_batch.Id
        , entrant_check_batch.CreateDate
        , entrant_check_batch.IsProcess
        , entrant_check_batch.IsCorrect
      from 
        dbo.EntrantCheckBatch entrant_check_batch
      where 
        entrant_check_batch.OwnerAccountId = @accountId 
      ''
  else 
    set @commandText = 
      N''
      select count(*)
      from dbo.EntrantCheckBatch entrant_check_batch
      where entrant_check_batch.OwnerAccountId = @accountid
      ''

  if isnull(@showCount, 0) = 0
  begin
    if @sortColumn = ''CreateDate''
    begin
      set @innerOrder = ''order by CreateDate <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection> ''
    end
    else
    begin
      set @innerOrder = ''order by CreateDate <orderDirection> ''
      set @outerOrder = ''order by CreateDate <orderDirection> ''
      set @resultOrder = ''order by CreateDate <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end
  set @commandText = @commandText + 
    ''option (keepfixed plan) ''

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' dbo.GetExternalId(search.Id) Id '' +
      '' , search.CreateDate '' +
      '' , search.IsProcess '' +
      '' , search.IsCorrect '' +
      ''from @search search '' + @resultOrder
  
  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@accountId bigint''
    , @accountId
  
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchEntrantCheck]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchEntrantCheck]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchEntrantCheck
-- ==============================================
-- Получение списка проверенных абитуриентов
-- v.1.0: Created by Sedov Anton 09.07.2008
-- ==============================================
CREATE procedure [dbo].[SearchEntrantCheck] 
  @login nvarchar(255)
  , @batchId bigint
  , @startRowIndex int = 1
  , @maxRowCount int = null
  , @showCount bit = null
as
begin
  declare
    @accountId bigint
    , @internalBatchId bigint

  set @internalBatchId = dbo.GetInternalId(@batchId)

  if not exists(select 1
      from dbo.EntrantCheckBatch entrant_check_batch with (nolock, fastfirstrow)
        inner join dbo.Account account with (nolock, fastfirstrow)
          on entrant_check_batch.OwnerAccountId = account.[Id]
      where 
        entrant_check_batch.Id = @internalBatchId
          and entrant_check_batch.IsProcess = 0
          and account.[Login] = @login)
    set @internalBatchId = 0

  declare 
    @declareCommandText nvarchar(4000)
    , @commandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @innerSelectHeader nvarchar(10)
    , @outerSelectHeader nvarchar(10)
    , @innerOrder nvarchar(1000)
    , @outerOrder nvarchar(1000)
    , @resultOrder nvarchar(1000)
    , @sortColumn nvarchar(20)
    , @sortAsc bit

  set @declareCommandText = ''''
  set @commandText = ''''
  set @viewCommandText = ''''
  set @sortColumn = N''Id''
  set @sortAsc = 0
  
  
  if isnull(@showCount, 0) = 0
    set @declareCommandText = 
      ''declare @search table '' +
      '' ( '' +
      '' Id bigint '' +
      '' , CertificateNumber nvarchar(255) '' +
      '' , LastName nvarchar(255) '' +
      '' , FirstName nvarchar(255)'' +
      '' , PatronymicName nvarchar(255) '' +
      '' , OrganizationName nvarchar(255)'' +
      '' , EntrantCreateDate datetime '' +
      '' , IsExist bit '' +
      '' ) ''

  if isnull(@showCount, 0) = 0
    set @commandText = 
        ''select <innerHeader> '' +
        '' entrant_check.Id '' +
        '' , entrant_check.CertificateNumber '' +
        '' , entrant_check.SourceLastName '' +
        '' , entrant_check.SourceFirstName '' +
        '' , entrant_check.SourcePatronymicName '' +
        '' , entrant_check.SourceOrganizationName '' +
        '' , entrant_check.SourceEntrantCreateDate '' +
        '' , case '' + 
        ''   when not entrant_check.SourceEntrantId is null then 1 '' +
        ''   else 0 '' +
        '' end IsExist '' + 
        ''from dbo.EntrantCheck entrant_check with (nolock) '' +
        ''where 1 = 1 '' 
  else
    set @commandText = 
        ''select count(*) '' +
        ''from dbo.EntrantCheck entrant_check with (nolock) '' +
        ''where 1 = 1 '' 

  
  set @commandText = @commandText + 
    '' and entrant_check.BatchCheckId = @internalBatchId '' 

  if isnull(@showCount, 0) = 0
  begin
    begin
      set @innerOrder = ''order by Id <orderDirection> ''
      set @outerOrder = ''order by Id <orderDirection> ''
      set @resultOrder = ''order by Id <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' +
        N''select <outerHeader> * '' + 
        N''from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
    set @viewCommandText = 
      ''select '' +
      '' search.CertificateNumber CertificateNumber'' +
      '' , search.LastName LastName '' +
      '' , search.FirstName FirstName '' +
      '' , search.PatronymicName PatronymicName '' +
      '' , search.OrganizationName OrganizationName '' + 
      '' , search.EntrantCreateDate EntrantCreateDate '' + 
      '' , search.IsExist IsExist '' + 
      ''from @search search '' + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewCommandText 

  exec sp_executesql @commandText
    , N''@internalBatchId bigint''
    , @internalBatchId
  
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procEDURE [dbo].[DeleteCheckFromCommonNationalExamCertificateRequestBatchById]
  @id bigint
as
begin
  declare @internalId bigint
  set @internalId = dbo.GetInternalId(@id)

  DELETE FROM [dbo].[CommonNationalExamCertificateRequestBatch]
      WHERE [Id]=@internalId
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Удаление проверки из лога
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE procEDURE [dbo].[DeleteCheckFromCommonNationalExamCertificateCheckBatchById]
  @id bigint
as
begin
  declare @internalId bigint
  set @internalId = dbo.GetInternalId(@id)

  DELETE FROM [dbo].[CommonNationalExamCertificateCheckBatch]
      WHERE [Id]=@internalId
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CommonNationalExamCertificateSumCheckResult]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommonNationalExamCertificateSumCheckResult]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[CommonNationalExamCertificateSumCheckResult] @batchId BIGINT = 301519088383
AS 
    set nocount on
   
    select  @batchId BatchId,
            Name,
            [Sum],
           [Status],
            NameSake
            from
            (
 select    Name,max([Sum]) sum,
                        max(case when [Status]=1 then 1 when [Status]=2 then 3 end) [Status],
                        min([Id]) id,
                        NameSake
              from      CommonNationalExamCertificateSumCheck
              where     [Status] in(1,2) and
              BatchId = 
             dbo.GetInternalId(@batchId)
            --  @batchId
               group by  Name,NameSake
union all              
select    Name,max([Sum]) sum,
                        2 [Status],
                        min([Id]) id,
                        NameSake
              from      CommonNationalExamCertificateSumCheck a
              where     a.[Status]=0 and
              BatchId = 
               dbo.GetInternalId(@batchId)
            and not exists(select * from CommonNationalExamCertificateSumCheck where 
            BatchId = 
              dbo.GetInternalId(@batchId)
            and Name=a.Name and [Status]=1
            )
               group by  Name,NameSake
               ) tt
              ORDER BY tt.Id ' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckNewUserAccountEmail]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckNewUserAccountEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.CheckNewUserAccountEmail
-- =============================================
-- Проверка email нового пользователя 
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- =============================================
CREATE procedure [dbo].[CheckNewUserAccountEmail]
  @email nvarchar(255)
as
begin
  declare 
    @userGroupCode nvarchar(255)
    , @isValid bit
    , @currentYear int
    , @activatedStatus  nvarchar(255)

  set @userGroupCode = ''User''
  set @activatedStatus = ''activated''
  set @currentYear = Year(GetDate())

  if exists(select 1
      from 
        dbo.Account account with (nolock)
          inner join dbo.GroupAccount group_account with (nolock) 
            inner join dbo.[Group] [group] with (nolock)
              on [group].Id = group_account.GroupId
            on group_account.AccountId = account.Id
      where
        [group].Code = @userGroupCode
        and account.Email = @email
        and dbo.GetUserStatus(account.ConfirmYear, account.Status, @currentYear
          , account.RegistrationDocument) = @activatedStatus)
    set @isValid = 0
  else 
    set @isValid = 1

  select
    @email Email
    , @isValid IsValid
return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckNewLogin]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckNewLogin]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.CheckNewLogin

-- =============================================
-- Проверка нового логина.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modofied by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- =============================================
CREATE procedure [dbo].[CheckNewLogin]
  @login nvarchar(255)
as
begin
  declare @isExists bit

  if exists(select 1
      from 
        dbo.Account account with (nolock, fastfirstrow)
      where
        account.[Login] = @login)
    set @isExists = 1
  else
    set @isExists = 0

  select
    @login [Login]
    , @isExists IsExists

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountLog]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccountLog]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetUserAccountLog

-- =============================================
-- Получить лог учетной записи пользователя.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- v.1.3: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.4: Modified by Sedov Anton 25.06.2008
-- Удалено поле RegistrationDocument
-- вместо него возвращается null
-- v.1.5: Modified by Sedov Anton 10.07.2008
-- В результат добавлено поле
-- EducationInstitutionTypeName
-- =============================================
CREATE procedure [dbo].[GetUserAccountLog]
  @login nvarchar(255)
  , @versionId int
as
begin
  declare
    @accountId bigint

  select @accountId = account.Id
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  select
    account_log.[Login] [Login]
    , account_log.VersionId VersionId
    , account_log.UpdateDate UpdateDate
    , editor.[Login] EditorLogin
    , editor.LastName EditorLastName
    , editor.FirstName EditorFirstName
    , editor.PatronymicName EditorPatronymicName
    , account_log.EditorIp EditorIp
    , account_log.IsVpnEditorIp IsVpnEditorIp
    , account_log.LastName LastName
    , account_log.FirstName FirstName
    , account_log.PatronymicName PatronymicName
    , organization_log.RegionId OrganizationRegionId
    , region.[Name] OrganizationRegionName
    , organization_log.[Name] OrganizationName
    , organization_log.FounderName OrganizationFounderName
    , organization_log.Address OrganizationAddress
    , organization_log.ChiefName OrganizationChiefName
    , organization_log.Fax OrganizationFax
    , organization_log.Phone OrganizationPhone
    , account_log.Phone Phone
    , account_log.Email Email
    , account_log.IpAddresses IpAddresses
    , account_log.HasFixedIp HasFixedIp
    , null RegistrationDocument
    , account_log.AdminComment AdminComment
    , account_log.Status Status
    , account_log.HasCrocEgeIntegration HasCrocEgeIntegration
    , education_institution_type.[Name] EducationInstitutionTypeName
  from
    dbo.AccountLog account_log with (nolock, fastfirstrow)
      left outer join dbo.OrganizationLog organization_log with (nolock, fastfirstrow)
        left join dbo.OrganizationType2010 education_institution_type
          on education_institution_type.Id = organization_log.EducationInstitutionTypeId
        on account_log.OrganizationId = organization_log.OrganizationId
        and organization_log.UpdateId = (select top 1 last_linked_account_log.UpdateId
            from dbo.AccountLog last_linked_account_log with (nolock, fastfirstrow)
            where last_linked_account_log.AccountId = @accountId
              and last_linked_account_log.VersionId = (select max(inner_account_log.VersionId)
                  from dbo.AccountLog inner_account_log with (nolock)
                    inner join dbo.OrganizationLog inner_organization_log with (nolock)
                      on inner_account_log.OrganizationId = inner_organization_log.OrganizationId
                        and inner_account_log.UpdateId = inner_organization_log.UpdateId
                  where inner_account_log.AccountId = @accountId
                    and inner_account_log.VersionId <= @versionId))
      left outer join dbo.Region region with (nolock, fastfirstrow)
        on organization_log.RegionId = region.[Id]
      left outer join dbo.Account editor with (nolock, fastfirstrow)
        on editor.Id = account_log.EditorAccountId
  where
    account_log.AccountId = @accountId
    and account_log.VersionId = @versionId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAskedQuestion]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAskedQuestion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetAskedQuestion

-- =============================================
-- Получение вопроса.
-- Если @isViewCount = 1, то ViewCount увеличить на 1 для показываемой записи.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc [dbo].[GetAskedQuestion]
  @id bigint
  , @isViewCount bit = 1
as
begin
  declare 
    @internalId bigint
  
  set @internalId = dbo.GetInternalId(@id)

  select
    @id [Id]
    , asked_question.Name [Name]
    , asked_question.Question Question
    , asked_question.Answer Answer
    , asked_question.IsActive IsActive
    , asked_question.ContextCodes ContextCodes
    , asked_question.Popularity Popularity
    , asked_question.ViewCount ViewCount
  from 
    dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
  where
    asked_question.[Id] = @internalId

  if @isViewCount = 1
    update asked_question
    set 
      ViewCount = ViewCount + 1
    from 
      dbo.AskedQuestion asked_question with (rowlock)
    where
      asked_question.[Id] = @internalId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAccountRole]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountRole]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[GetAccountRole]
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
    --and (account_role.IsActiveCondition is NOT null
    --  or exists(select 1
    --    from dbo.AccountRoleActivity activity
    --    where activity.AccountId = account_role.AccountId
    --      and activity.RoleId = account_role.RoleId
    --      and activity.UpdateDate >= account.UpdateDate
    --      ))

  return 0
end' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAccountLog]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountLog]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetAccountLog

-- =============================================
-- Получить лог учетной записи.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- =============================================
CREATE procedure [dbo].[GetAccountLog]
  @login nvarchar(255)
  , @versionId int
as
begin

  select
    account_log.[Login] [Login]
    , account_log.VersionId VersionId
    , account_log.UpdateDate UpdateDate
    , editor.[Login] EditorLogin
    , editor.LastName EditorLastName
    , editor.FirstName EditorFirstName
    , editor.PatronymicName EditorPatronymicName
    , account_log.EditorIp EditorIp
    , account_log.IsVpnEditorIp IsVpnEditorIp
    , account_log.LastName LastName
    , account_log.FirstName FirstName
    , account_log.PatronymicName PatronymicName
    , account_log.Phone Phone
    , account_log.Email Email
    , account_log.IpAddresses IpAddresses
    , account_log.HasFixedIp HasFixedIp
    , account_log.IsActive IsActive
  from
    dbo.AccountLog account_log with (nolock, fastfirstrow)
      inner join dbo.Account account with (nolock, fastfirstrow)
        on account_log.AccountId = account.[Id]
      left outer join dbo.Account editor with (nolock, fastfirstrow)
        on editor.Id = account_log.EditorAccountId
  where
    account.[Login] = @login
    and account_log.VersionId = @versionId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAccountKey]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetAccountKey
-- ====================================================
-- Получение ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
CREATE procedure [dbo].[GetAccountKey]
  @login nvarchar(255)
  , @key nvarchar(255)
as
begin
  select
    account.Login [Login]
    , account_key.[Key]
    , account_key.DateFrom
    , account_key.DateTo
    , account_key.IsActive
  from dbo.Account account
    inner join dbo.AccountKey account_key
      on account.Id = account_key.AccountId
  where
    account.Login = @login
    and account_key.[Key] = @key

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAccountGroup]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountGroup]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetAccountGroup

-- =============================================
-- Получение группы пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- =============================================
CREATE procedure [dbo].[GetAccountGroup]
  @login nvarchar(50)
as
begin
  select top 1
    account.[Login] [Login]
    , [group].Code GroupCode
  from dbo.GroupAccount group_account with (nolock, fastfirstrow)
    inner join dbo.Account account with (nolock, fastfirstrow)
      on group_account.AccountId = account.Id
    inner join dbo.[Group] [group] with (nolock, fastfirstrow)
      on [group].Id = group_account.GroupId
  where account.[Login] = @login

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetAccount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение информации об учетной записи 
-- внутреннего пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- v.1.1: Modified by Makarev Andrey 10.04.2008
-- Добавлено поле Account.IpAddresses
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.2.0: Modified by A. Vinichenko 12.04.2011
-- =============================================
CREATE procedure [dbo].[GetAccount]
  @login nvarchar(255)
as
begin
  select
    account.[Login] [Login]
    , account.LastName LastName 
    , account.FirstName FirstName
    , account.PatronymicName PatronymicName
    , account.Email Email
    , account.Phone Phone
    , account.IsActive IsActive
    , account.IpAddresses IpAddresses
    , account.HasFixedIp HasFixedIp
    , account.UpdateDate UpdateDate
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckUserAccountEmail]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckUserAccountEmail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procEDURE [dbo].[CheckUserAccountEmail]
  @login nvarchar(255)
  ,@email nvarchar(255)
  ,@IsUniq bit out
AS
BEGIN
  -- если e-mail не меняется, то считаем его уникальным
  IF EXISTS(  SELECT 1 FROM dbo.Account WITH (NOLOCK) 
        WHERE Email = @email and [Login]=@login)
    SET @IsUniq = 1
  ELSE 
  IF EXISTS(  SELECT 1 FROM dbo.Account WITH (NOLOCK) 
        WHERE Email = @email and [Status]!=''deactivated'' and [Login]!=@login)
    SET @IsUniq = 0
  ELSE 
    SET @IsUniq = 1
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCertificateByFioAndPassport]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCertificateByFioAndPassport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- Получение свидетельст за все года по ФИО и паспортным данным
-- Возвращаемы значения
-- Id - идентификатор свидетельства
-- CreateDate - Дата добавления сертификата
-- Number - номер свидетельства
-- Year - год
CREATE PROCEDURE [dbo].[GetCertificateByFioAndPassport]
	@LastName NVARCHAR(255) = null,				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255) = null,				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255) = null,			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20) = null,		-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20) = null,		-- номер документа сертифицируемого (паспорта)	
	@CurrentCertificateNumber NVARCHAR(255)		-- номер свидетельства, его нужно исключить при выборке
AS
BEGIN	
	declare @yearFrom int, @yearTo int
	select @yearFrom = 2008, @yearTo = Year(GetDate())
	
	SELECT  c.Id,
        c.CreateDate,
        c.Number,
        c.Year,
        marks,
        case when ed.[ExpireDate] is null then ''Не найдено''
             else case when certificate_deny.CertificateFK is not null
                       then ''Аннулировано''
                       else case when getdate() <= ed.[ExpireDate]
                                 then ''Действительно''
                                 else ''Истек срок''
                            end
                  end
        end as [Status]
FROM    (
		 SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, b.UseYear AS Year, 
                a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, b.TypographicNumber, a.ParticipantID AS ParticipantsID, 
                REPLACE(REPLACE(LTRIM(RTRIM(a.Name)) + LTRIM(RTRIM(a.Surname)) + LTRIM(RTRIM(a.SecondName)), ''ё'', ''е''), '' '', '''') AS FIO, 
                a.ParticipantID, b.CreateDate
		 FROM rbd.Participants AS a with(nolock)
			INNER JOIN prn.Certificates AS b with(nolock) ON b.ParticipantFK = a.ParticipantID and b.UseYear=a.UseYear
		 where a.UseYear between @yearFrom and @yearTo and ( Surname = @LastName
            or @LastName is null
          )
          AND ( Name = @FirstName
                or @FirstName is null
              )
          AND ( SecondName = null
                or @PatronymicName is null
              )
          AND DocumentNumber = @PassportNumber
          AND DocumentSeries = @PassportSeria
          AND LicenseNumber <> @CurrentCertificateNumber			
		) c
        CROSS APPLY ( SELECT    ( SELECT    CAST(s.SubjectCode AS VARCHAR(20))
                                            + ''=''
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      '','', ''.'') + '','' AS [text()]
                                  FROM      [prn].[CertificatesMarks] s with(nolock)
                                  WHERE     s.CertificateFK = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''''),
                                      TYPE
                                ) marks
                    ) as marks
        LEFT JOIN dbo.ExpireDate ed ON ed.Year = c.Year
        LEFT OUTER JOIN prn.CancelledCertificates certificate_deny
        with ( nolock, fastfirstrow ) on certificate_deny.UseYear=c.Year
                                         and certificate_deny.CertificateFK=c.id
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetBatchStatusById]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBatchStatusById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procEDURE [dbo].[GetBatchStatusById]
    @batchUniqueId uniqueidentifier,
    @userLogin varchar(255),
    @isProcess bit output,
    @isCorrect bit output,
    @isFound bit output,
    @searchType int OUTPUT
AS
BEGIN
    declare @externalId bigint
    declare @internalId bigint
    declare @accountId bigint

    set @isProcess = 0
    set @isCorrect = 0
    set @isFound = 0
    set @searchType = 0
    
    -- Выполняем перекодировку номера пакета из GUID во внешний код
    set @externalId = isnull((select B.Id from dbo.BatchGUID B where B.BatchUniqueId = @batchUniqueId), -1)
    if (@externalId = -1)
    begin
      return
    end
    
    -- Выполняем поиск кода пользователя
    set @accountId = isnull((select A.Id from dbo.Account A where A.[Login] = @userLogin), -1)
    if (@accountId = -1)
    begin
      return
    end

	-- Выполняем поиск сертификата по внешнему коду
    select 
        @isProcess = isnull(C.IsProcess, 0),
        @isCorrect = isnull(C.IsCorrect, 1),
        @searchType = C.SearchType,
        @isFound = 1
    from 
      (
        select bg.BatchUniqueId, CB.IsProcess, CB.IsCorrect, 1 SearchType
        from CommonNationalExamCertificateCheckBatch CB
			JOIN dbo.BatchGUID bg ON CB.Id = bg.Id
        where CB.OwnerAccountId = @accountId AND bg.WSSearchType = 1
        union all
        select bg.BatchUniqueId, RB.IsProcess, RB.IsCorrect, (case RB.IsTypographicNumber when 1 then 3 else 2 end) SearchType
      from CommonNationalExamCertificateRequestBatch RB
		JOIN dbo.BatchGUID bg ON RB.Id = bg.Id
        where RB.OwnerAccountId = @accountId AND bg.WSSearchType IN (2, 3)
        ) C
    where 
      C.BatchUniqueId = @batchUniqueId  
      
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetDocument]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDocument]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetDocument

-- =============================================
-- Получение документа.
-- v.1.0: Created by Makarev Andrey 17.04.2008
-- v.1.1: Modified by Makarev Andrey 19.04.2008
-- Получение данных по внутреннему id.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
CREATE proc [dbo].[GetDocument]
  @id bigint
as
begin

  declare @internalId bigint

  set @internalId = dbo.GetInternalId(@id)

  select
    @id [Id]
    , [document].[Name] [Name]
    , [document].Description Description
    , [document].[Content] [Content]
    , [document].ContentSize ContentSize
    , [document].ContentType ContentType
    , [document].IsActive IsActive
    , [document].ActivateDate ActivateDate
    , [document].ContextCodes ContextCodes
    , [document].RelativeUrl RelativeUrl
  from 
    dbo.[Document] [document] with (nolock, fastfirstrow)
  where
    [document].[Id] = @internalId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetDeliveryRecipients]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetDeliveryRecipients]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[GetDeliveryRecipients]
  @id bigint
as
begin
  select
    recipients.RecipientCode RecipientCode
  from 
    dbo.DeliveryRecipients recipients with (nolock)
  where
    recipients.[DeliveryId] = @id

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetBatchCheckReady]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBatchCheckReady]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetBatchCheckReady]
    @batchId BIGINT,
    @checkType INT
AS
BEGIN
	DECLARE @id BIGINT

	SELECT @id = ch.Id
	FROM dbo.CheckHistory ch
	WHERE BatchId = @batchId AND CheckTypeId = @checkType
	
	SELECT @id
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryOrgPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryOrgPagesCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetCheckResultsHistoryOrgPagesCount]
	@organizationId BIGINT,
	@uniqueChecks BIT
AS 
BEGIN
	SET NOCOUNT ON;

	set @uniqueChecks = ISNULL(@uniqueChecks, 0);
	
	if (@uniqueChecks = 1) 
	begin
		INSERT INTO #BPIDS
		SELECT 
			cr.Id,			
			ch.SenderTypeId
		FROM
			dbo.CheckHistory ch	
			JOIN CheckResultsHistory cr ON 
				ch.Id = cr.CheckId
			JOIN (
				SELECT
					MAX(cr.Id) AS Id				
				FROM 
					dbo.Account a 
					JOIN dbo.CheckHistory ch WITH (INDEX (PK_CheckHistoryId), INDEX(idx_OwnerId)) 
						ON a.Id = ch.OwnerId
					JOIN dbo.CheckResultsHistory cr	ON 	
						ch.Id = cr.CheckId
				WHERE 
					a.OrganizationId = @organizationId
					AND cr.ParticipantId IS NOT NULL			
				GROUP BY 
					ch.SenderTypeId,			
					cr.ParticipantId) AS A1	
			ON A1.Id = cr.Id				
		ORDER BY	
			cr.CreateDate DESC,
			cr.ParticipantId ASC
		end
	else
	begin
		INSERT INTO #BPIDS
		SELECT 
			cr.Id,					
			ch.SenderTypeId
		FROM	
			dbo.Account a 
			JOIN dbo.CheckHistory ch WITH (INDEX (PK_CheckHistoryId), INDEX(idx_OwnerId)) 
				ON	a.Id = ch.OwnerId		
			JOIN CheckResultsHistory cr ON ch.Id = cr.CheckId
		WHERE
			a.OrganizationId = @organizationId 						
			AND cr.ParticipantId IS NOT NULL
		ORDER BY	
			cr.CreateDate DESC,
			cr.ParticipantId ASC
		end
	
    SET NOCOUNT OFF;

SELECT 
		count(RowNumber) 
	FROM
		#BPIDS
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryOrgPaged]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryOrgPaged]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[GetCheckResultsHistoryOrgPaged]
	@organizationId BIGINT,
    @startRowIndex INT = 0, -- Начиная с какой строки выбирать
    @participantsOnPageCount INT = NULL  -- Сколько участников на странице
AS 
BEGIN

DECLARE @endRowIndex INT = ISNULL(@startRowIndex, 1) + ISNULL(@participantsOnPageCount, 10)

SELECT  
			ch.Id,
			ch.GroupId,	
			ch.ParticipantId,
			ch.UseYear,
			ch.RegionId,
			r.RegionName,
			ch.Surname,
			ch.Name,
			ch.SecondName,
			ch.DocumentSeries,
			ch.DocumentNumber,
			s.SubjectCode,
			s.SubjectName,
			cm.Mark,	
			cm.ProcessCondition,	
			rsg.GlobalStatusID,
			rsg.StatusName AS StatusName,
			cm.HasAppeal,
			cer.CertificateID,
			cer.LicenseNumber,
			cer.TypographicNumber,
			cer.Cancelled
		FROM 
			dbo.CheckHistory h 
			JOIN dbo.CheckResultsHistory ch ON
				h.Id = ch.CheckId
			JOIN rbdc.Regions r ON
				r.REGION = ch.RegionId
			JOIN prn.CertificatesMarks cm ON 
				cm.ParticipantFK = ch.ParticipantId AND
				cm.REGION = ch.RegionId AND
				ch.UseYear = cm.UseYear
			JOIN prn.Certificates cer ON
				cer.CertificateID = cm.CertificateFK AND
				cer.UseYear = cm.UseYear AND
				cer.REGION = cm.REGION
			JOIN dat.Subjects s ON
				s.SubjectCode = cm.SubjectCode
			JOIN dbo.ResultStatuses rs ON
				rs.ProcessCondition = cm.ProcessCondition
			JOIN dbo.ResultGlobalStatuses rsg ON 
				rsg.GlobalStatusID = rs.GlobalStatusID	
			JOIN #BPIDS ix on ix.Id = ch.Id	 
		WHERE 
			ix.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
		ORDER by ix.RowNumber
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[ExecuteChecksCount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecuteChecksCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ExecuteChecksCount]
    @OrganizationId bigint,
    @CertificateId bigint = null,
    @CertificateIdGuid uniqueidentifier =null
AS
BEGIN
	if (@OrganizationId = 0 or @OrganizationId is null or @CertificateIdGuid is null)
		return -1

	-- Выполняла ли данная организация проверку данного сертификата
    declare @isExists bit

	-- Значения инкрементов
	declare @uniqueIHEaFCheck int
	declare @uniqueIHECheck int
	declare @uniqueIHEFCheck int 

	declare @uniqueTSSaFCheck int
	declare @uniqueTSSCheck int
	declare @uniqueTSSFCheck int
    
	declare @uniqueRCOICheck int
	declare @uniqueOUOCheck int
	declare @uniqueFounderCheck int
	declare @uniqueOtherCheck int 
    
    -- Тип организации
    declare @orgType int
    
    -- Является ли организация филлиалом
    declare @isFilial bit

	-- Инициализация переменных
    set @isExists = 0
	set @uniqueIHEaFCheck = 0
	set @uniqueIHECheck = 0
	set @uniqueIHEFCheck = 0
	set @uniqueTSSaFCheck = 0
	set @uniqueTSSCheck = 0
	set @uniqueTSSFCheck = 0
	set @uniqueRCOICheck = 0
	set @uniqueOUOCheck = 0
	set @uniqueFounderCheck = 0
	set @uniqueOtherCheck = 0
    set @orgType = 0
    set @isFilial = 0

    set @isExists = 
    	(
        select count(*) 
        from [dbo].[OrganizationCertificateChecks] OCC 
        where OCC.CertificateIdGuid = @CertificateIdGuid and OCC.OrganizationId = @OrganizationId
        )

    if (@isExists = 0)
    begin
		IF exists(SELECT * FROM dbo.Organization2010 WHERE Id =@OrganizationId and disablelog=0)
		begin
	    	insert into [dbo].[OrganizationCertificateChecks] (CertificateId, OrganizationId, CertificateIdGuid)
		    values (@CertificateId, @OrganizationId, @CertificateIdGuid)
		end
        
        select
        	@orgType = isnull(O.TypeId, 0),
            @isFilial = (case when (O.MainId is null) then 0 else 1 end)
        from
        	[dbo].[Organization2010] O
        where
        	O.Id = @OrganizationId

        if (@orgType = 0)
        	return -1

		if (@orgType = 1)
        begin
        	set @uniqueIHEaFCheck = 1
            if (@isFilial = 1)
            	set @uniqueIHEFCheck = 1
            else
            	set @uniqueIHECheck = 1
        end           
        
		if (@orgType = 2)
        begin
        	set @uniqueTSSaFCheck = 1
            if (@isFilial = 1)
            	set @uniqueTSSFCheck = 1
            else
            	set @uniqueTSSCheck = 1
        end           
        
		if (@orgType = 3)
        begin
        	set @uniqueRCOICheck = 1
        end           

		if (@orgType = 4)
        begin
        	set @uniqueOUOCheck = 1
        end           

		if (@orgType = 6)
        begin
        	set @uniqueFounderCheck = 1
        end           

		if (@orgType = 5)
        begin
        	set @uniqueOtherCheck = 1
        end
    
        declare @year int
        if @CertificateId is null
			set @year = 
				(select top 1 C.UseYear
				from [prn].[Certificates] C with(nolock)
				where C.CertificateID = @CertificateIdGuid)
        else
			set @year = 
				(select top 1 C.[Year]
				from CommonNationalExamCertificate C
				where C.Id = @CertificateId)
				
		if not exists 
			(select *
			from [dbo].[ExamCertificateUniqueChecks] EC
			where EC.Id = @CertificateId) 
			and
			not exists (select *
			from [dbo].[ExamCertificateUniqueChecks] EC
			where EC.idGUID = @CertificateIdGuid) 
		begin
			insert into [dbo].[ExamCertificateUniqueChecks] 
				(
				[Year], 
				Id, 
				UniqueChecks,
				UniqueIHEaFCheck,
				UniqueIHECheck,
				UniqueIHEFCheck,
				UniqueTSSaFCheck,
				UniqueTSSCheck,
				UniqueTSSFCheck,
				UniqueRCOICheck,
				UniqueOUOCheck,
				UniqueFounderCheck,
				UniqueOtherCheck,
				idGUID
				)
			values 
				(
				@year, 
				@CertificateId,
				1,
				@uniqueIHEaFCheck,
				@uniqueIHECheck,
				@uniqueIHEFCheck,
				@uniqueTSSaFCheck,
				@uniqueTSSCheck,
				@uniqueTSSFCheck,
				@uniqueRCOICheck,
				@uniqueOUOCheck,
				@uniqueFounderCheck,
				@uniqueOtherCheck,
				@CertificateIdGuid
				)
		end
		else begin
		    if @CertificateId is not null  
				update 
        			[dbo].[ExamCertificateUniqueChecks]
				set 
					UniqueChecks = UniqueChecks + 1,
					UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
					UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
					UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
					UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
					UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
					UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
					UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
					UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
					UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
					UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
				where
        			idGUID = @CertificateIdGuid
        			and [Year] = @year
        	else
				update 
        			[dbo].[ExamCertificateUniqueChecks]
				set 
					UniqueChecks = UniqueChecks + 1,
					UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
					UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
					UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
					UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
					UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
					UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
					UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
					UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
					UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
					UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
				where
        			idGUID = @CertificateIdGuid
        			and [Year] = @year        	
        end

        return 1
    end
    else begin
    	return 0
    end
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckOlympicResults]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckOlympicResults]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Проверка результатов олимпиад
-- =============================================
CREATE PROCEDURE [dbo].[CheckOlympicResults] 
	@diplomantLastName varchar(64),
	@diplomaNumber varchar(32)
AS
BEGIN
	
	if (@diplomaNumber is null OR @diplomantLastName is null)
	BEGIN
	    return 0;
	END
	
	SET NOCOUNT ON;
	
	DECLARE @olympicId BIGINT
	DECLARE @diplomantId BIGINT

    select TOP 1
		@olympicId = o.id, 
		@diplomantId = d.id
	from 
		[dbo].Olympics o
		inner join [dbo].[OlympicDiplomants] d on d.olympics_id = o.id
	where 
		d.reg_code = @diplomaNumber and d.last_name = @diplomantLastName;
		
    SET NOCOUNT OFF;
    
    IF (@olympicId is not null and @diplomantId is not null)
    begin
		select 
			d.id,
			d.result_level diplomaLevel, 
			d.reg_code documentNumber,
			rtrim(d.last_name + '' '' + ISNULL(d.first_name, '''') + '' '' + ISNULL(d.middle_name, '''')) diplomantName,
			o.olympiad_name olympiadFullName,
			o.olympiad_level olympiadLevel,
			o.olympiad_number olympiadNumber,
			o.olympiad_subject_name olympiadSubjectName,
			o.olympiad_year olympiadYear
		from 
			[dbo].[OlympicDiplomants] d
			inner join [dbo].[Olympics] o on d.olympics_id = o.id
		where 
			d.id = @diplomantId;
			
		
		with olympic_subjects AS(
			select olsubj.subject_name, map.ege_subject_id 
			from 
				[dbo].[OlympicSubjects] olsubj
				inner join [dbo].[OlympicProfileSubjects] olprof on olprof.olympic_subject_id = olsubj.id
				inner join [dbo].[Olympics] ol on olprof.olympics_id = ol.id
				inner join [dbo].[OlympicToFbsSubjectMap] map on map.olympic_subject_id = olsubj.id
			where 
				ol.id = @olympicId
		)
		select
			ISNULL(sbj.SubjectName, o.subject_name) SubjectName,
			c.Mark,
			c.UseYear [Year],
			gs.StatusName,
			gs.GlobalStatusId StatusId
		from
			[prn].[CertificatesMarks] c
			left join ResultStatuses s on c.ProcessCondition = s.ProcessCondition
			left join ResultGlobalStatuses gs on s.GlobalStatusID = gs.GlobalStatusID
			inner join [dat].[Subjects] sbj on c.SubjectCode = sbj.SubjectCode
			inner join olympic_subjects o on sbj.SubjectCode = o.ege_subject_id
		where
			c.ParticipantFK in 
			(select participant_id from [dbo].[OlympicParticipants] where diplomant_id = @diplomantId);

    end
	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[AddCNEWebUICheckEvent]    Script Date: 05/07/2015 18:13:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddCNEWebUICheckEvent]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:    Yusupov K.I.
-- Create date: 04-06-2010
-- Description: Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
CREATE procEDURE [dbo].[AddCNEWebUICheckEvent]
@AccountLogin NVARCHAR(255),          -- логин проверяющего
  @LastName NVARCHAR(255)=null,       -- фамилия сертифицируемого
  @FirstName NVARCHAR(255)=null,        -- имя сертифицируемого
  @PatronymicName NVARCHAR(255)=null,     -- отчетсво сертифицируемого
  @PassportSeria NVARCHAR(20)=NULL,     -- серия документа сертифицируемого (паспорта)
  @PassportNumber NVARCHAR(20)=NULL,      -- номер документа сертифицируемого (паспорта)
  @CNENumber NVARCHAR(20)=NULL,       -- номер сертификата
  @TypographicNumber NVARCHAR(20)=NULL,   -- типографический номер сертификата 
  @RawMarks NVARCHAR(500)=null,       -- средние оценки по предметам (через запятую, в определенном порядке)
  @IsOpenFbs bit=null,
  @EventId INT output             -- id зарегистрированного события
AS
BEGIN
  IF (SELECT disablelog 
    FROM dbo.Organization2010 
     WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Login = @AccountLogin)) = 1
  BEGIN
  SELECT @EventId = 0
  RETURN
  END
  
  IF 
  (
    @TypographicNumber IS NULL AND
    @CNENumber IS NULL AND
    @PassportNumber IS NULL AND
    @RawMarks IS NULL
  )
  BEGIN
    RAISERROR (N''Не указаны паспортные данные, типографский номер, номер свидетельства и баллы по предметам одновременно'',10,1)
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
      (@AccountId,''Typographic'',@FirstName,@LastName,@PatronymicName,@TypographicNumber)
  END
  ELSE IF (@CNENumber IS NOT NULL)
  BEGIN
    declare @logTypeNumber nvarchar(50)
    set @logTypeNumber = ''CNENumber'' + case when isnull(@IsOpenFbs,0) = 1 then ''Open'' else '''' end
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,CNENumber,Marks) 
        VALUES 
      (@AccountId,@logTypeNumber,@FirstName,@LastName,@PatronymicName,@CNENumber,@RawMarks)
  END
  ELSE IF (@PassportNumber IS NOT NULL)
  BEGIN
    declare @logTypePassport nvarchar(50)
    set @logTypePassport = ''Passport'' + case when isnull(@IsOpenFbs,0) = 1 then ''Open'' else '''' end

    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,PassportSeria,PassportNumber,Marks) 
        VALUES 
      (@AccountId,@logTypePassport,@FirstName,@LastName,@PatronymicName,@PassportSeria,@PassportNumber,@RawMarks)
  END
  ELSE IF (@RawMarks IS NOT NULL)
  BEGIN
    INSERT INTO CNEWebUICheckLog 
      (AccountId,TypeCode,FirstName,LastName,PatronymicName,Marks) 
        VALUES 
      (@AccountId,''Marks'',@FirstName,@LastName,@PatronymicName,@RawMarks)
  END
    SELECT @EventId = @@Identity
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[BatchCheck_TypographicNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchCheck_TypographicNumberFio]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[BatchCheck_TypographicNumberFio]
	@senderType INT,					-- тип отправителя
	@batchId BIGINT,					-- внешний id пакета	
	@login nvarchar(255)				-- логин проверяющего
AS
BEGIN
SET NOCOUNT ON

DECLARE @count INT
DECLARE @finded INT

DECLARE @checkId BIGINT
DECLARE @checkType INT = 2	
DECLARE @batch VARCHAR(MAX) = NULL		

SELECT @checkId = Id
FROM dbo.CheckHistory
WHERE BatchId = @batchId AND CheckTypeId = @checkType
	
IF (@checkId IS NOT NULL)
BEGIN
	RAISERROR (N''Пакет уже обработан'', 16, 1);		
	RETURN
END	

BEGIN TRY

	SELECT @batch = Batch
	FROM CommonNationalExamCertificateRequestBatch
	WHERE Id = @batchId
		
	IF (@batch IS NULL)
	BEGIN
		RAISERROR (N''Отсутствует пакет с указанным идентификатором'', 16, 1);
		RETURN
	END

	--------------------------------------------------------------------------------
	DECLARE @ownerId INT
	DECLARE @ownerIsBlocked BIT
	DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

	-- Пользователь, отправивший запрос
	SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
	FROM dbo.Account a
	WHERE a.LoginTrimmed = @loginTrimmed	

	IF (@ownerIsBlocked IS NULL)
	BEGIN
		RAISERROR (N''Пользователь не активирован в системе'', 16, 1);
		RETURN
	END
	--------------------------------------------------------------------------------

	CREATE TABLE #TypographicNumberFioFilter (		
		FilterId INT IDENTITY(1, 1),
		OriginalSurname varchar(80) NULL,			-- то что пришло в оригинале из csv
		OriginalName varchar(80) NULL,				-- то что пришло в оригинале из csv
		OriginalSecondName varchar(80) NULL,		-- то что пришло в оригинале из csv
		OriginalTypographicNumber VARCHAR(80) NULL,	-- то что пришло в оригинале из csv 			
		CheckSurname varchar(80) NULL, 
		CheckName varchar(80) NULL, 
		CheckSecondName varchar(80) NULL, 
		CheckTypographicNumber VARCHAR(80) NULL, 
		IsBroken BIT DEFAULT 0	
	) 	
	CREATE UNIQUE NONCLUSTERED INDEX idx ON #TypographicNumberFioFilter (FilterId)

	INSERT INTO #TypographicNumberFioFilter ( 
		OriginalSurname, 
		OriginalName, 
		OriginalSecondName, 
		OriginalTypographicNumber,	
		CheckSurname, 
		CheckName, 
		CheckSecondName, 
		CheckTypographicNumber,
		IsBroken)
	SELECT		
		ISNULL(f.FirstName, ''''), 
		ISNULL(f.LastName, ''''), 
		ISNULL(f.MiddleName, ''''), 
		ISNULL(f.TypographicNumber, ''''), 	
		ISNULL(f.CheckFirstName, ''''), 		
		ISNULL(f.CheckLastName, ''''), 
		ISNULL(f.CheckMiddleName, ''''), 
		ISNULL(f.CheckTypographicNumber, ''''), 
		CASE WHEN (
			f.CheckFirstName IS NULL OR 
			f.CheckFirstName = '''' OR 
			f.CheckTypographicNumber IS NULL OR 
			f.CheckTypographicNumber = '''' OR
			LEN(f.CheckTypographicNumber) > 12) THEN 1 ELSE 0 END		
	FROM fn_Parse_TypographicNumberFio(@batch, CHAR(10)) f	

	SET @count = @@ROWCOUNT
	IF (@count > 10000) 
	BEGIN
		INSERT INTO dbo.CheckHistory WITH (ROWLOCK) (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
		VALUES (@batchId, @ownerId, @senderType, @checkType, ''Ошибка: пакет > 10 тыс строк'')	
		RAISERROR (N''Размер пакета больше 10 тыс строк'', 16, 1);
		RETURN	
	END

	CREATE TABLE #TypographicNumberFioResults (	
		FilterId INT,		
		OriginalSurname varchar(80), 
		OriginalName varchar(80) NULL, 
		OriginalSecondName varchar(80) NULL, 
		OriginalTypographicNumber varchar(80),		
		GroupId INT NULL,
		ParticipantId UNIQUEIDENTIFIER NULL,
		UseYear INT NULL,
		RegionId INT NULL,
		Surname varchar(80) NULL,
		Name varchar(80) NULL,
		SecondName varchar(80) NULL,
		DocumentSeries varchar(9) NULL,
		DocumentNumber varchar(10) NULL,
		IsBroken BIT DEFAULT 0,
		CertificateID UNIQUEIDENTIFIER NULL,
		LicenseNumber NVARCHAR(18) NULL,
		TypographicNumber NVARCHAR(12) NULL,
		SurnameTrimmed varchar(80) NULL,
		NameTrimmed varchar(80) NULL,
		SecondNameTrimmed varchar(80) NULL
	)
	CREATE NONCLUSTERED INDEX idx_0 ON #TypographicNumberFioResults (FilterId)		
	CREATE NONCLUSTERED INDEX idx_1 ON #TypographicNumberFioResults
		(DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed)	

	SET NOCOUNT OFF
	INSERT INTO #TypographicNumberFioResults
	SELECT 	
		ch.FilterId,		
		ch.OriginalSurname, 
		ch.OriginalName, 
		ch.OriginalSecondName, 
		ch.OriginalTypographicNumber,		
		NULL,
		p.ParticipantID,
		p.UseYear,
		p.REGION,			
		p.Surname,
		p.Name,
		p.SecondName,		
		p.DocumentSeries,
		p.DocumentNumber,				
		ch.IsBroken,		
		cer.CertificateID,
		cer.LicenseNumber,
		cer.TypographicNumber,
		p.SurnameTrimmed,
		p.NameTrimmed,
		p.SecondNameTrimmed		
	FROM 
		#TypographicNumberFioFilter ch
		JOIN prn.Certificates cer ON
			cer.TypographicNumber = ch.CheckTypographicNumber
		JOIN rbd.Participants p ON 
			p.ParticipantID = cer.ParticipantFK AND
			p.REGION = cer.REGION AND
			p.UseYear = cer.UseYear AND				
			p.SurnameTrimmed = ch.CheckSurname AND 
			p.NameTrimmed = (CASE WHEN ch.CheckName = '''' THEN p.NameTrimmed ELSE ch.CheckName END) AND
			p.SecondNameTrimmed = (CASE WHEN ch.CheckSecondName = '''' THEN p.SecondNameTrimmed ELSE ch.CheckSecondName END)		
	WHERE ch.IsBroken = 0	

	SET NOCOUNT ON
	INSERT INTO #TypographicNumberFioResults (
		FilterId,		
		OriginalSurname, 
		OriginalName, 
		OriginalSecondName, 
		OriginalTypographicNumber,			
		IsBroken
	)
	SELECT 
		f.FilterId,		
		f.OriginalSurname, 
		f.OriginalName, 
		f.OriginalSecondName, 
		f.OriginalTypographicNumber, 
		f.IsBroken
	FROM 
		#TypographicNumberFioFilter f 
		LEFT JOIN #TypographicNumberFioResults r ON 
			r.FilterId = f.FilterId
	WHERE r.FilterId IS NULL			

	--------------------------------------------
	UPDATE #TypographicNumberFioResults
	SET GroupId = A1.Number
	FROM 
		(SELECT row_number() over (
			ORDER BY DocumentSeries, DocumentNumber) AS Number,
			DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
		FROM #TypographicNumberFioResults
		WHERE SurnameTrimmed IS NOT NULL
		GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
		JOIN #TypographicNumberFioResults r ON 
			r.DocumentSeries = A1.DocumentSeries AND
			r.DocumentNumber = A1.DocumentNumber AND
			r.SurnameTrimmed = A1.SurnameTrimmed AND 
			r.NameTrimmed = A1.NameTrimmed AND
			r.SecondNameTrimmed = A1.SecondNameTrimmed

	DECLARE @maxGroupId INT
	SELECT @maxGroupId = ISNULL(MAX(GroupId), 0)
	FROM #TypographicNumberFioResults
	WHERE GroupId IS NOT NULL	

	/* Нумеруем группы которые NULL */
	UPDATE #TypographicNumberFioResults
	SET GroupId = A1.GroupId
	FROM 
		(SELECT (@maxGroupId + row_number() over (ORDER BY FilterId)) AS GroupId, FilterId
		 FROM #TypographicNumberFioResults
		 WHERE GroupId IS NULL
		 GROUP BY FilterId) AS A1		 
		JOIN #TypographicNumberFioResults r ON 
			r.FilterId = A1.FilterId	

	--------------------------------------------
	SELECT @finded = COUNT (DISTINCT FilterId) 
	FROM #TypographicNumberFioResults
	WHERE ParticipantId IS NOT NULL		
	--------------------------------------------

	BEGIN TRANSACTION
		INSERT INTO dbo.CheckHistory (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
		VALUES (@batchId, @ownerId, @senderType, @checkType, ''Найдено '' + CAST(@finded AS VARCHAR(20)) + '' из '' + CAST(@count AS VARCHAR(20)))
		SELECT @checkId = @@IDENTITY		

		INSERT INTO dbo.CheckResultsHistory (
		  CheckId,
		  CheckSurname,
		  CheckName,
		  CheckSecondName,
		  CheckTypographicNumber,
		  GroupId,
		  ParticipantId,
		  UseYear,
		  RegionId,
		  Surname,
		  Name,
		  SecondName,
		  DocumentSeries,
		  DocumentNumber,
		  IsBroken,
		  StatusMessage)
		SELECT 
			@checkId,
			r.OriginalSurname,
			r.OriginalName,
			r.OriginalSecondName,
			r.OriginalTypographicNumber,
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber,
			r.IsBroken,
			CASE WHEN r.IsBroken = 1 THEN ''Необходимо указать фамилию и типографский номер'' ELSE NULL END
		FROM #TypographicNumberFioResults r  
	COMMIT TRANSACTION
END TRY
BEGIN CATCH

	INSERT INTO dbo.CheckHistory (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@batchId, @ownerId, @senderType, @checkType, ''Возникла ошибка во время обработки пакета'')
	
	DECLARE @message VARCHAR(2048) = ERROR_MESSAGE()
	RAISERROR (@message, 16, 1)
	
END CATCH
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[BatchCheck_LicenseNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchCheck_LicenseNumberFio]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[BatchCheck_LicenseNumberFio]
	@senderType INT,					-- тип отправителя
	@batchId BIGINT,					-- внешний id пакета	
	@login nvarchar(255)				-- логин проверяющего
AS
BEGIN
SET NOCOUNT ON

DECLARE @count INT
DECLARE @finded INT

DECLARE @checkId BIGINT
DECLARE @checkType INT = 1	
DECLARE @batch VARCHAR(MAX) = NULL		

SELECT @checkId = Id
FROM dbo.CheckHistory
WHERE BatchId = @batchId AND CheckTypeId = @checkType
	
IF (@checkId IS NOT NULL)
BEGIN
	RAISERROR (N''Пакет уже обработан'', 16, 1);		
	RETURN
END	

BEGIN TRY

	SELECT @batch = Batch
	FROM dbo.CommonNationalExamCertificateCheckBatch
	WHERE Id = @batchId	
		
	IF (@batch IS NULL)
	BEGIN
		RAISERROR (N''Отсутствует пакет с указанным идентификатором'', 16, 1);		
		RETURN
	END

	--------------------------------------------------------------------------------
	DECLARE @ownerId INT
	DECLARE @ownerIsBlocked BIT
	DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

	-- Пользователь, отправивший запрос
	SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
	FROM dbo.Account a
	WHERE a.LoginTrimmed = @loginTrimmed	

	IF (@ownerIsBlocked IS NULL)
	BEGIN
		RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
		RETURN
	END
	--------------------------------------------------------------------------------

	CREATE TABLE #LicenseNumberFioFilter (		
		FilterId INT IDENTITY(1, 1),
		OriginalSurname varchar(80) NULL,			-- то что пришло в оригинале из csv
		OriginalName varchar(80) NULL,				-- то что пришло в оригинале из csv
		OriginalSecondName varchar(80) NULL,		-- то что пришло в оригинале из csv
		OriginalCertificateNumber VARCHAR(80) NULL,	-- то что пришло в оригинале из csv 	
		CheckSurname varchar(80) NULL, 
		CheckName varchar(80) NULL, 
		CheckSecondName varchar(80) NULL, 
		CheckCertificateNumber VARCHAR(80) NULL, 
		IsBroken BIT DEFAULT 0	
	) 	
	CREATE UNIQUE NONCLUSTERED INDEX idx ON #LicenseNumberFioFilter (FilterId)

	INSERT INTO #LicenseNumberFioFilter ( 
		OriginalSurname,			-- то что пришло в оригинале из csv
		OriginalName,				-- то что пришло в оригинале из csv
		OriginalSecondName,			-- то что пришло в оригинале из csv
		OriginalCertificateNumber,	-- то что пришло в оригинале из csv
		CheckSurname, 
		CheckName, 
		CheckSecondName, 
		CheckCertificateNumber,
		IsBroken)
	SELECT	
		ISNULL(f.FirstName, ''''), 
		ISNULL(f.LastName, ''''), 
		ISNULL(f.MiddleName, ''''), 
		ISNULL(f.LicenseNumber, ''''), 		
		ISNULL(f.CheckFirstName, ''''), 
		ISNULL(f.CheckLastName, ''''), 
		ISNULL(f.CheckMiddleName, ''''), 
		ISNULL(f.CheckLicenseNumber, ''''), 
		CASE WHEN (
			f.CheckFirstName IS NULL OR 
			f.CheckFirstName = '''' OR 
			f.CheckLicenseNumber IS NULL OR 
			f.CheckLicenseNumber = '''' OR 
			LEN(f.CheckLicenseNumber) > 18) THEN 1 ELSE 0 END		
	FROM fn_Parse_LicenseNumberFio(@batch, CHAR(10)) f	

	SET @count = @@ROWCOUNT
	IF (@count > 10000) 
	BEGIN
		INSERT INTO dbo.CheckHistory WITH (ROWLOCK) (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
		VALUES (@batchId, @ownerId, @senderType, @checkType, ''Ошибка: пакет > 10 тыс строк'')	
		RAISERROR (N''Размер пакета больше 10 тыс строк'', 16, 1);		
		RETURN	
	END

	CREATE TABLE #LicenseNumberFioResults (	
		FilterId INT,		
		OriginalSurname varchar(80),			-- то что пришло в оригинале из csv
		OriginalName varchar(80) NULL,			-- то что пришло в оригинале из csv
		OriginalSecondName varchar(80) NULL,	-- то что пришло в оригинале из csv
		OriginalCertificateNumber varchar(80),	-- то что пришло в оригинале из csv		
		GroupId INT NULL,
		ParticipantId UNIQUEIDENTIFIER NULL,
		UseYear INT NULL,
		RegionId INT NULL,
		Surname varchar(80) NULL,
		Name varchar(80) NULL,
		SecondName varchar(80) NULL,
		DocumentSeries varchar(9) NULL,
		DocumentNumber varchar(10) NULL,
		IsBroken BIT DEFAULT 0,
		CertificateID UNIQUEIDENTIFIER NULL,
		LicenseNumber NVARCHAR(18) NULL,
		TypographicNumber NVARCHAR(12) NULL,
		SurnameTrimmed varchar(80) NULL,
		NameTrimmed varchar(80) NULL,
		SecondNameTrimmed varchar(80) NULL
	)
	CREATE NONCLUSTERED INDEX idx_0 ON #LicenseNumberFioResults (FilterId)		
	CREATE NONCLUSTERED INDEX idx_1 ON #LicenseNumberFioResults 
		(DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed)	

	SET NOCOUNT OFF
	INSERT INTO #LicenseNumberFioResults
	SELECT 	
		ch.FilterId,		
		ch.OriginalSurname, 
		ch.OriginalName, 
		ch.OriginalSecondName, 
		ch.OriginalCertificateNumber,		
		NULL,
		p.ParticipantID,
		p.UseYear,
		p.REGION,			
		p.Surname,
		p.Name,
		p.SecondName,		
		p.DocumentSeries,
		p.DocumentNumber,				
		ch.IsBroken,		
		cer.CertificateID,
		cer.LicenseNumber,
		cer.TypographicNumber,
		p.SurnameTrimmed,
		p.NameTrimmed,
		p.SecondNameTrimmed		
	FROM 
		#LicenseNumberFioFilter ch
		JOIN prn.Certificates cer ON
			cer.LicenseNumber = ch.CheckCertificateNumber
		JOIN rbd.Participants p ON 
			p.ParticipantID = cer.ParticipantFK AND
			p.REGION = cer.REGION AND
			p.UseYear = cer.UseYear AND				
			p.SurnameTrimmed = ch.CheckSurname AND 
			p.NameTrimmed = (CASE WHEN ch.CheckName = '''' THEN p.NameTrimmed ELSE ch.CheckName END) AND
			p.SecondNameTrimmed = (CASE WHEN ch.CheckSecondName = '''' THEN p.SecondNameTrimmed ELSE ch.CheckSecondName END)		
	WHERE ch.IsBroken = 0	

	SET NOCOUNT ON
	INSERT INTO #LicenseNumberFioResults (
		FilterId,		
		OriginalSurname,			-- то что пришло в оригинале из csv
		OriginalName,				-- то что пришло в оригинале из csv
		OriginalSecondName,			-- то что пришло в оригинале из csv
		OriginalCertificateNumber,	-- то что пришло в оригинале из csv			
		IsBroken
	)
	SELECT 
		f.FilterId,		
		f.OriginalSurname, 
		f.OriginalName, 
		f.OriginalSecondName, 
		f.OriginalCertificateNumber, 
		f.IsBroken
	FROM 
		#LicenseNumberFioFilter f 
		LEFT JOIN #LicenseNumberFioResults r ON 
			r.FilterId = f.FilterId
	WHERE r.FilterId IS NULL			

	--------------------------------------------
	UPDATE #LicenseNumberFioResults
	SET GroupId = A1.Number
	FROM 
		(SELECT row_number() over (
			ORDER BY DocumentSeries, DocumentNumber) AS Number,
			DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
		FROM #LicenseNumberFioResults
		WHERE SurnameTrimmed IS NOT NULL
		GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
		JOIN #LicenseNumberFioResults r ON 
			r.DocumentSeries = A1.DocumentSeries AND
			r.DocumentNumber = A1.DocumentNumber AND
			r.SurnameTrimmed = A1.SurnameTrimmed AND 
			r.NameTrimmed = A1.NameTrimmed AND
			r.SecondNameTrimmed = A1.SecondNameTrimmed

	DECLARE @maxGroupId INT
	SELECT @maxGroupId = ISNULL(MAX(GroupId), 0)
	FROM #LicenseNumberFioResults
	WHERE GroupId IS NOT NULL	

	/* Нумеруем группы которые NULL */
	UPDATE #LicenseNumberFioResults
	SET GroupId = A1.GroupId
	FROM 
		(SELECT (@maxGroupId + row_number() over (ORDER BY FilterId)) AS GroupId, FilterId
		 FROM #LicenseNumberFioResults
		 WHERE GroupId IS NULL
		 GROUP BY FilterId) AS A1		 
		JOIN #LicenseNumberFioResults r ON 
			r.FilterId = A1.FilterId	

	--------------------------------------------
	SELECT @finded = COUNT (DISTINCT FilterId) 
	FROM #LicenseNumberFioResults
	WHERE ParticipantId IS NOT NULL		
	--------------------------------------------

	BEGIN TRANSACTION
		INSERT INTO dbo.CheckHistory (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
		VALUES (@batchId, @ownerId, @senderType, @checkType, ''Найдено '' + CAST(@finded AS VARCHAR(20)) + '' из '' + CAST(@count AS VARCHAR(20)))
		SELECT @checkId = @@IDENTITY		

		INSERT INTO dbo.CheckResultsHistory (
		  CheckId,
		  CheckSurname,
		  CheckName,
		  CheckSecondName,
		  CheckCertificateNumber,
		  GroupId,
		  ParticipantId,
		  UseYear,
		  RegionId,
		  Surname,
		  Name,
		  SecondName,
		  DocumentSeries,
		  DocumentNumber,
		  IsBroken,
		  StatusMessage)
		SELECT 
			@checkId,
			r.OriginalSurname,
			r.OriginalName,
			r.OriginalSecondName,
			r.OriginalCertificateNumber,
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber,
			r.IsBroken,
			CASE WHEN r.IsBroken = 1 THEN ''Необходимо указать фамилию и номер свидетельства'' ELSE NULL END
		FROM #LicenseNumberFioResults r  
	COMMIT TRANSACTION
END TRY
BEGIN CATCH

	INSERT INTO dbo.CheckHistory (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@batchId, @ownerId, @senderType, @checkType, ''Возникла ошибка во время обработки пакета'')
	
	DECLARE @message VARCHAR(2048) = ERROR_MESSAGE()
	RAISERROR (@message, 16, 1)
	
END CATCH
SET NOCOUNT OFF	
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[BatchCheck_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchCheck_FioDocumentNumberSeries]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[BatchCheck_FioDocumentNumberSeries]
	@senderType INT,					-- тип отправителя
	@batchId BIGINT,					-- внешний id пакета	
	@login nvarchar(255)				-- логин проверяющего
AS
BEGIN
SET NOCOUNT ON

DECLARE @count INT
DECLARE @finded INT

DECLARE @checkId BIGINT
DECLARE @checkType INT = 3	
DECLARE @batch VARCHAR(MAX) = NULL		

SELECT @checkId = Id
FROM dbo.CheckHistory
WHERE BatchId = @batchId AND CheckTypeId = @checkType

IF (@checkId IS NOT NULL)
BEGIN
	RAISERROR (N''Пакет уже обработан'', 16, 1);		
	RETURN
END	

BEGIN TRY

	SELECT @batch = Batch
	FROM CommonNationalExamCertificateRequestBatch
	WHERE Id = @batchId
		
	IF (@batch IS NULL)
	BEGIN
		RAISERROR (N''Отсутствует пакет с указанным идентификатором'', 16, 1);		
		RETURN
	END

	--------------------------------------------------------------------------------
	DECLARE @ownerId INT
	DECLARE @ownerIsBlocked BIT
	DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

	-- Пользователь, отправивший запрос
	SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
	FROM dbo.Account a
	WHERE a.LoginTrimmed = @loginTrimmed	

	IF (@ownerIsBlocked IS NULL)
	BEGIN
		RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
		RETURN
	END
	--------------------------------------------------------------------------------

	CREATE TABLE #FioDocumentNumberSeriesFilter (		
		FilterId INT IDENTITY(1, 1),
		OriginalSurname varchar(80) NULL,			-- то что пришло в оригинале из csv
		OriginalName varchar(80) NULL,				-- то что пришло в оригинале из csv
		OriginalSecondName varchar(80) NULL,		-- то что пришло в оригинале из csv	
		OriginalDocumentSeries varchar(80) NULL,	-- то что пришло в оригинале из csv
		OriginalDocumentNumber varchar(80) NULL,	-- то что пришло в оригинале из csv		
		CheckSurname varchar(80) NULL, 
		CheckName varchar(80) NULL, 
		CheckSecondName varchar(80) NULL, 
		CheckDocumentSeries varchar(80) NULL, 
		CheckDocumentNumber varchar(80) NULL,
		IsBroken BIT DEFAULT 0	
	) 	
	CREATE UNIQUE NONCLUSTERED INDEX idx ON #FioDocumentNumberSeriesFilter (FilterId)

	INSERT INTO #FioDocumentNumberSeriesFilter ( 
		OriginalSurname, 
		OriginalName, 
		OriginalSecondName, 
		OriginalDocumentSeries, 
		OriginalDocumentNumber,
		CheckSurname, 
		CheckName, 
		CheckSecondName, 
		CheckDocumentSeries, 
		CheckDocumentNumber,
		IsBroken)
	SELECT		
		ISNULL(f.FirstName, ''''),		-- то что пришло в оригинале из csv
		ISNULL(f.LastName, ''''),			-- то что пришло в оригинале из csv
		ISNULL(f.MiddleName, ''''),		-- то что пришло в оригинале из csv
		ISNULL(f.DocumentSeries, ''''),	-- то что пришло в оригинале из csv	
		ISNULL(f.DocumentNumber, ''''),	-- то что пришло в оригинале из csv
		ISNULL(f.CheckFirstName, ''''), 
		ISNULL(f.CheckLastName, ''''), 
		ISNULL(f.CheckMiddleName, ''''), 
		ISNULL(f.CheckDocumentSeries, ''''), 
		ISNULL(f.CheckDocumentNumber, ''''),
		CASE WHEN (
			f.CheckFirstName IS NULL OR 
			f.CheckFirstName = '''' OR 
			f.CheckDocumentNumber IS NULL OR 
			f.CheckDocumentNumber = '''' OR 
			LEN(f.CheckDocumentSeries) > 9 OR
			LEN(f.CheckDocumentNumber) > 10) THEN 1 ELSE 0 END		
	FROM fn_Parse_FioDocumentNumberSeries(@batch, CHAR(10)) f

	SET @count = @@ROWCOUNT
	IF (@count > 10000) 
	BEGIN
		INSERT INTO dbo.CheckHistory WITH (ROWLOCK) (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
		VALUES (@batchId, @ownerId, @senderType, @checkType, ''Ошибка: пакет > 10 тыс строк'')	
		RAISERROR (N''Размер пакета больше 10 тыс строк'', 16, 1);		
		RETURN	
	END

	DECLARE @commandText nvarchar(4000)
	DECLARE @isCheckNameExists BIT
	DECLARE @isCheckSecondNameExists BIT
	DECLARE @isCheckDocumentSeriesExists BIT

	SELECT TOP 1 
		@isCheckNameExists = CASE WHEN CheckName = '''' THEN 0 ELSE 1 END,
		@isCheckSecondNameExists = CASE WHEN CheckSecondName = '''' THEN 0 ELSE 1 END,
		@isCheckDocumentSeriesExists = CASE WHEN CheckDocumentSeries = '''' THEN 0 ELSE 1 END
	FROM #FioDocumentNumberSeriesFilter

	CREATE TABLE #FioDocumentNumberSeriesResults (	
		FilterId INT,		
		OriginalSurname varchar(80),				-- то что пришло в оригинале из csv
		OriginalName varchar(80) NULL,				-- то что пришло в оригинале из csv
		OriginalSecondName varchar(80) NULL,		-- то что пришло в оригинале из csv
		OriginalDocumentSeries varchar(80) NULL,	-- то что пришло в оригинале из csv
		OriginalDocumentNumber varchar(80),			-- то что пришло в оригинале из csv
		GroupId INT NULL,
		ParticipantId UNIQUEIDENTIFIER NULL,
		UseYear INT NULL,
		RegionId INT NULL,
		Surname varchar(80) NULL,
		Name varchar(80) NULL,
		SecondName varchar(80) NULL,
		DocumentSeries varchar(9) NULL,
		DocumentNumber varchar(10) NULL,
		IsBroken BIT DEFAULT 0,				
		CertificateID UNIQUEIDENTIFIER NULL,
		LicenseNumber NVARCHAR(18) NULL,
		TypographicNumber NVARCHAR(12) NULL,
		SurnameTrimmed varchar(80) NULL,
		NameTrimmed varchar(80) NULL,
		SecondNameTrimmed varchar(80) NULL
	)
	CREATE NONCLUSTERED INDEX idx_0 ON #FioDocumentNumberSeriesResults (FilterId)		
	CREATE NONCLUSTERED INDEX idx_1 ON #FioDocumentNumberSeriesResults 
		(DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed)			

	SET NOCOUNT OFF
	SET @commandText = ''
	INSERT INTO #FioDocumentNumberSeriesResults
	SELECT DISTINCT	
		ch.FilterId,		
		ch.OriginalSurname,			-- то что пришло в оригинале из csv
		ch.OriginalName,			-- то что пришло в оригинале из csv
		ch.OriginalSecondName,		-- то что пришло в оригинале из csv
		ch.OriginalDocumentSeries,	-- то что пришло в оригинале из csv
		ch.OriginalDocumentNumber,	-- то что пришло в оригинале из csv	
		NULL,
		p.ParticipantID,
		p.UseYear,
		p.REGION,			
		p.Surname,
		p.Name,
		p.SecondName,				
		p.DocumentSeries,
		p.DocumentNumber,				
		ch.IsBroken,		
		NULL,
		NULL,
		NULL,		
		p.SurnameTrimmed,
		p.NameTrimmed,
		p.SecondNameTrimmed
	FROM 
		#FioDocumentNumberSeriesFilter ch
		JOIN rbd.Participants p ON 
			p.DocumentNumber = ch.CheckDocumentNumber''
			
		IF (@isCheckDocumentSeriesExists = 1)	
			SET @commandText = @commandText + '' AND p.DocumentSeries = ch.CheckDocumentSeries''
			
		SET @commandText = @commandText + '' AND p.SurnameTrimmed = ch.CheckSurname''
			
		IF (@isCheckNameExists = 1)	
			SET @commandText = @commandText + '' AND p.NameTrimmed = ch.CheckName''
		IF (@isCheckSecondNameExists = 1)	
			SET @commandText = @commandText + '' AND p.SecondNameTrimmed = ch.CheckSecondName''
			
		SET @commandText = @commandText + '' 
		JOIN prn.CertificatesMarks cm ON 
			cm.ParticipantFK = p.ParticipantID AND
			cm.UseYear = p.UseYear AND
			cm.REGION = p.REGION	
		WHERE ch.IsBroken = 0''	

	EXEC sp_executesql @commandText

	SET NOCOUNT ON
	INSERT INTO #FioDocumentNumberSeriesResults (
		FilterId,		
		OriginalSurname, 
		OriginalName, 
		OriginalSecondName, 
		OriginalDocumentSeries, 
		OriginalDocumentNumber,			
		IsBroken
	)
	SELECT 
		f.FilterId,		
		f.OriginalSurname,			-- то что пришло в оригинале из csv 
		f.OriginalName,				-- то что пришло в оригинале из csv
		f.OriginalSecondName,		-- то что пришло в оригинале из csv
		f.OriginalDocumentSeries,	-- то что пришло в оригинале из csv
		f.OriginalDocumentNumber,	-- то что пришло в оригинале из csv		
		f.IsBroken
	FROM 
		#FioDocumentNumberSeriesFilter f 
		LEFT JOIN #FioDocumentNumberSeriesResults r ON 
			r.FilterId = f.FilterId
	WHERE r.FilterId IS NULL			

	--------------------------------------------
	UPDATE #FioDocumentNumberSeriesResults
	SET GroupId = A1.Number
	FROM 
		(SELECT row_number() over (
			ORDER BY DocumentSeries, DocumentNumber) AS Number,
			DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed 
		FROM #FioDocumentNumberSeriesResults
		WHERE SurnameTrimmed IS NOT NULL
		GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
		JOIN #FioDocumentNumberSeriesResults r ON 
			r.DocumentSeries = A1.DocumentSeries AND
			r.DocumentNumber = A1.DocumentNumber AND		
			r.SurnameTrimmed = A1.SurnameTrimmed AND 
			r.NameTrimmed = A1.NameTrimmed AND
			r.SecondNameTrimmed = A1.SecondNameTrimmed

	DECLARE @maxGroupId INT
	SELECT @maxGroupId = ISNULL(MAX(GroupId), 0)
	FROM #FioDocumentNumberSeriesResults
	WHERE GroupId IS NOT NULL	

	/* Нумеруем группы которые NULL */
	UPDATE #FioDocumentNumberSeriesResults
	SET GroupId = A1.GroupId
	FROM 
		(SELECT (@maxGroupId + row_number() over (ORDER BY FilterId)) AS GroupId, FilterId
		 FROM #FioDocumentNumberSeriesResults
		 WHERE GroupId IS NULL
		 GROUP BY FilterId) AS A1		 
		JOIN #FioDocumentNumberSeriesResults r ON 
			r.FilterId = A1.FilterId	

	--------------------------------------------
	SELECT @finded = COUNT (DISTINCT FilterId) 
	FROM #FioDocumentNumberSeriesResults
	WHERE ParticipantId IS NOT NULL		
	--------------------------------------------

	BEGIN TRANSACTION
		INSERT INTO dbo.CheckHistory (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
		VALUES (@batchId, @ownerId, @senderType, @checkType, ''Найдено '' + CAST(@finded AS VARCHAR(20)) + '' из '' + CAST(@count AS VARCHAR(20)))
		SELECT @checkId = @@IDENTITY		

		INSERT INTO dbo.CheckResultsHistory (
			  CheckId,
			  CheckSurname,			-- то что пришло в оригинале из csv 
			  CheckName,			-- то что пришло в оригинале из csv 
			  CheckSecondName,		-- то что пришло в оригинале из csv 
			  CheckDocumentSeries,	-- то что пришло в оригинале из csv 
			  CheckDocumentNumber,	-- то что пришло в оригинале из csv 
			  GroupId,
			  ParticipantId,
			  UseYear,
			  RegionId,
			  Surname,
			  Name,
			  SecondName,
			  DocumentSeries,
			  DocumentNumber,
			  IsBroken,
			  StatusMessage)
		SELECT 
			@checkId,
			r.OriginalSurname,
			r.OriginalName,
			r.OriginalSecondName,
			r.OriginalDocumentSeries,
			r.OriginalDocumentNumber,
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber,
			r.IsBroken,
			CASE WHEN r.IsBroken = 1 THEN ''Необходимо указать фамилию и номер документа'' ELSE NULL END
		FROM #FioDocumentNumberSeriesResults r  
	COMMIT TRANSACTION
END TRY
BEGIN CATCH

	INSERT INTO dbo.CheckHistory (BatchId, OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@batchId, @ownerId, @senderType, @checkType, ''Возникла ошибка во время обработки пакета'')
	
	DECLARE @message VARCHAR(2048) = ERROR_MESSAGE()
	RAISERROR (@message, 16, 1)
	
END CATCH
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCommonNationalExamCertificateRequestExtendedByExam

-- =============================================
-- Получить список проверок.
-- v.1.0: Created by Fomin Dmitriy 18.07.2008
-- Создана по SearchCommonNationalExamCertificateRequest.
-- =============================================
CREATE proc [dbo].[SearchCommonNationalExamCertificateRequestExtendedByExam]
  @login nvarchar(255)
  , @batchId bigint
as
begin
  declare 
    @innerBatchId bigint
    , @accountId bigint
    , @commandText nvarchar(4000)
    , @declareCommandText nvarchar(4000)
    , @viewSelectCommandText nvarchar(4000)
    , @pivotSubjectColumns nvarchar(4000)
    , @viewSelectPivot1CommandText nvarchar(4000)
    , @viewSelectPivot2CommandText nvarchar(4000)
    , @viewCommandText nvarchar(4000)
    , @sortColumn nvarchar(20) 
    , @sortAsc bit 

  set @commandText = ''''
  set @pivotSubjectColumns = ''''
  set @viewSelectPivot1CommandText = ''''
  set @viewSelectPivot2CommandText = ''''
  set @viewCommandText = ''''
  set @declareCommandText = ''''
  set @sortColumn = N''Id''
  set @sortAsc = 1
  
  if @batchId is not null
    set @innerBatchId = dbo.GetInternalId(@batchId)

  select
    @accountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  set @declareCommandText = 
    N''declare @search table 
      (
      BatchId bigint
      , CertificateNumber nvarchar(255)
      , LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      , IsExist bit
      , SourceCertificateId  bigint
      , SourceCertificateYear int
      )
    ''

  set @declareCommandText = @declareCommandText +
    N''declare @request table 
      (
      LastName nvarchar(255)
      , FirstName nvarchar(255)
      , PatronymicName nvarchar(255)
      , PassportSeria nvarchar(255)
      , PassportNumber nvarchar(255)
      )
    ''

  set @commandText = @commandText +
    ''insert into @request 
    select distinct
      cne_certificate_request.LastName 
      , cne_certificate_request.FirstName 
      , cne_certificate_request.PatronymicName 
      , cne_certificate_request.PassportSeria 
      , cne_certificate_request.PassportNumber 
    from 
      dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
        inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
          on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
    where
      cne_certificate_request_batch.OwnerAccountId = <accountId> 
      and cne_certificate_request_batch.[Id] = <innerBatchId>
      and cne_certificate_request_batch.IsProcess = 0 
    ''
  
  set @commandText = @commandText +
    N''insert into @search
    select 
      dbo.GetExternalId(<innerBatchId>) BatchId
      , cne_certificate_request.SourceCertificateNumber CertificateNumber
      , request.LastName LastName
      , request.FirstName FirstName
      , request.PatronymicName PatronymicName
      , request.PassportSeria PassportSeria
      , request.PassportNumber PassportNumber
      , case
        when not cne_certificate_request.SourceCertificateId is null then 1
        else 0
      end IsExist
      , cne_certificate_request.SourceCertificateId
      , cne_certificate_request.SourceCertificateYear
    from @request request
      left outer join dbo.CommonNationalExamCertificateRequest cne_certificate_request with (nolock)
        inner join dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
            and cne_certificate_request_batch.OwnerAccountId = <accountId> 
            and cne_certificate_request_batch.[Id] = <innerBatchId>
            and cne_certificate_request_batch.IsProcess = 0 
        on request.FirstName = cne_certificate_request.FirstName
          and request.LastName = cne_certificate_request.LastName
          and request.PatronymicName = cne_certificate_request.PatronymicName
          and request.PassportSeria = cne_certificate_request.PassportSeria
          and request.PassportNumber = cne_certificate_request.PassportNumber
          and cne_certificate_request.IsDeny = 0
    ''

  set @declareCommandText = @declareCommandText +
    N'' declare @subjects table  
      ( 
      CertificateId bigint 
      , Mark numeric(5,1) 
      , HasAppeal bit  
      , SubjectCode nvarchar(255)  
      , HasExam bit
      ) 
    ''

  set @commandText = @commandText +
    N''insert into @subjects  
    select
      cne_certificate_subject.CertificateId 
      , cne_certificate_subject.Mark
      , cne_certificate_subject.HasAppeal
      , subject.Code
      , 1 
    from  
      dbo.CommonNationalExamCertificateSubject cne_certificate_subject
        left outer join dbo.Subject subject
          on subject.Id = cne_certificate_subject.SubjectId
    where 
      exists(select 1 
          from @search search
          where cne_certificate_subject.CertificateId = search.SourceCertificateId
            and cne_certificate_subject.[Year] = search.SourceCertificateYear)
    '' 
  
  set @viewSelectCommandText = 
    N''select
      search.BatchId
      , search.CertificateNumber
      , search.LastName
      , search.FirstName
      , search.PatronymicName
      , search.PassportSeria
      , search.PassportNumber
      , search.IsExist
    ''

  set @viewCommandText = 
    N''from @search search ''

  declare
    @subjectCode nvarchar(255)
    , @pivotSelect nvarchar(4000)

  set @pivotSelect = ''''

  declare subject_cursor cursor forward_only for
  select 
    [subject].Code
  from 
    dbo.Subject [subject]

  open subject_cursor 
  fetch next from subject_cursor into @subjectCode
  while @@fetch_status = 0
    begin
    if len(@pivotSubjectColumns) > 0
      set @pivotSubjectColumns = @pivotSubjectColumns + '',''
    set @pivotSubjectColumns = @pivotSubjectColumns + replace(''[<code>]'', ''<code>'', @subjectCode)
    
    set @pivotSelect = @pivotSelect + 
      N'' , isnull(exam_pvt.[<code>], 0) [<code>HasExam] ''
        
    set @pivotSelect = replace(@pivotSelect, ''<code>'', @subjectCode)

    if len(@viewSelectPivot1CommandText) + len(@pivotSelect) <= 4000
        and @viewSelectPivot2CommandText = ''''
      set @viewSelectPivot1CommandText = @viewSelectPivot1CommandText + @pivotSelect
    else
      set @viewSelectPivot2CommandText = @viewSelectPivot2CommandText + @pivotSelect

    fetch next from subject_cursor into @subjectCode
  end
  close subject_cursor
  deallocate subject_cursor

  set @viewCommandText = @viewCommandText + 
    N''left outer join (select 
      subjects.CertificateId
      , subjects.SubjectCode
      , cast(subjects.HasExam as int) HasExam 
      from @subjects subjects) subjects
        pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
      on search.SourceCertificateId = exam_pvt.CertificateId ''
      
  set @viewCommandText = replace(@viewCommandText, ''<subject_columns>'', @pivotSubjectColumns)

  set @viewCommandText = @viewCommandText

  set @commandText = replace(
      replace(@commandText, ''<innerBatchId>'', @innerBatchId), ''<accountId>'', @accountid)

  exec (@declareCommandText + @commandText + @viewSelectCommandText +
      @viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText)
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateGroupUserEsrp]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateGroupUserEsrp]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procEDURE [dbo].[UpdateGroupUserEsrp]
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
    ''delete from GroupAccount
    where 
      GroupAccount.GroupId in (select G.Id
                   from [Group] G
                   where
                    G.GroupIdEsrp not in ('' + @groupsEsrp + '') 
                   ) ''+
      ''and GroupAccount.AccountId = '' + cast(@accountId as nvarchar(255))
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
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountEsrp]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccountEsrp]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procEDURE [dbo].[UpdateAccountEsrp]
  @login nvarchar(255),
  @lastName nvarchar(255),
  @firstName nvarchar(255),
  @patronymicName nvarchar(255),
  @organizationId int,
  @phone nvarchar(255),
  @email nvarchar(255),
  @status nvarchar(255),
  @isActive bit
  --@ipAddresses nvarchar(400) = null
as
begin
  --существованние пользователя
  declare @exists table([login] nvarchar(255), isExists bit)
  insert @exists exec dbo.CheckNewLogin @login = @login
  
  declare 
    @isExists bit,
    @eventCode nvarchar(255),
    @accountId bigint,
    @innerStatus nvarchar(255),
    @confirmYear int,
    @currentYear int,
    @userGroupId int,
    @updateId uniqueidentifier,
    @accountIds nvarchar(255)

  set @updateId = newid()
  
  select @isExists = user_exists.isExists
  from  @exists user_exists

  select @accountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  set @currentYear = year(getdate())
  set @confirmYear = @currentYear

  --declare @oldIpAddress table (ip nvarchar(255))

  --declare @newIpAddress table (ip nvarchar(255))
  
  --если логина нет - добавляем запись
  --если логин есть - меняем данные
  if @isExists = 0  -- внесение нового пользователя
  begin
    select 
      @eventCode = N''USR_REG''
      
    select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
  end
  else
  begin -- update существующего пользователя
    select 
      @accountId = account.[Id]
    from 
      dbo.Account account with (nolock, fastfirstrow)
    where
      account.[Login] = @login

    /*insert @oldIpAddress
      (
      ip
      )
    select
      account_ip.Ip
    from
      dbo.AccountIp account_ip with (nolock, fastfirstrow)
    where
      account_ip.AccountId = @accountId
      
    set @eventCode = N''USR_EDIT''
    
    insert @newIpAddress
      (
      ip
      )
    select 
      ip_addresses.[value]
    from 
      dbo.GetDelimitedValues(@ipAddresses) ip_addresses*/
      
  end

  begin tran insert_update_account_tran

    if @isExists = 0  -- внесение нового пользователя
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
        , Email
        , RegistrationDocument
        , AdminComment
        , IsActive
        , Status
        , IpAddresses
        , HasFixedIp
        )
      select
        getdate()
        , getdate()
        , @updateId
        , null
        , null
        , @login
        , null
        , @lastName
        , @firstName
        , @patronymicName
        , @organizationId
        , 0
        , @confirmYear
        , @phone
        , @email
        , null
        , null
        , @isActive
        , @status
        , null
        , null

      if (@@error <> 0)
        goto undo

      select @accountId = scope_identity()

      if (@@error <> 0)
        goto undo

      /*insert dbo.AccountIp
        (
        AccountId
        , Ip
        )
      select
        @accountId
        , new_ip_address.ip
      from 
        @newIpAddress new_ip_address

      if (@@error <> 0)
        goto undo*/
    end
    else
    begin -- update существующего пользователя
      update account
      set
        UpdateDate = getdate()
        , UpdateId = @updateId
        , EditorAccountID = null
        , EditorIp = null
        , LastName = @lastName
        , FirstName = @firstName
        , PatronymicName = @patronymicName 
        , phone = @phone
        , email = @email
        , [Status] = @status
        , IsActive = @isActive
        , IpAddresses = null--@ipAddresses
        , HasFixedIp = null
      from
        dbo.Account account with (rowlock)
      where
        account.[Id] = @accountId

      if (@@error <> 0)
        goto undo

      /*if exists(select 
            1
          from
            @oldIpAddress old_ip_address
              full outer join @newIpAddress new_ip_address
                on old_ip_address.ip = new_ip_address.ip
          where
            old_ip_address.ip is null
            or new_ip_address.ip is null) 
      begin
        delete account_ip
        from 
          dbo.AccountIp account_ip
        where
          account_ip.AccountId = @accountId

        if (@@error <> 0)
          goto undo

        insert dbo.AccountIp
          (
          AccountId
          , Ip
          )
        select
          @accountId
          , new_ip_address.ip
        from 
          @newIpAddress new_ip_address

        if (@@error <> 0)
          goto undo
      end*/
    end

  if @@trancount > 0
    commit tran insert_update_account_tran

  /*set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId*/
  
  return 0

  undo:

  rollback tran insert_update_account_tran

  return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchMinimalMark]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchMinimalMark]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure  [dbo].[SearchMinimalMark]
( @year int = null
, @getAvailableYears bit = 0)
as
begin 

--если настал новый учетный год, то нужно наплодить недостающие минимальные баллы за текущий год
insert into [MinimalMark] (
 [SubjectId],
 [Year],
 [MinimalMark]
) 
select s.[SubjectId], year(getdate()), 0
from [Subject] as s with(nolock)
left join [MinimalMark] as mm with(nolock) on 
mm.[SubjectId] = s.SubjectId 
and mm.year = year(getdate())
where mm.[Id] is null

if @getAvailableYears = 1
 select distinct year from [MinimalMark] as mm with(nolock)
else 
 select mm.id, mm.year, s.[Name], mm.[MinimalMark] 
 from minimalmark mm with(nolock)
 join subject s with(nolock) on s.SubjectId = mm.[SubjectId] and s.[IsActive] = 1
 where mm.year = isnull(@year, mm.year)
 order by mm.year, s.[SortIndex]

end' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequest_test]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequest_test]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
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
CREATE proc [dbo].[SearchCommonNationalExamCertificateRequest_test]
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

  set @commandText = ''''
  set @pivotSubjectColumns = ''''
  set @viewSelectPivot1CommandText = ''''
  set @viewSelectPivot2CommandText = ''''
  set @viewCommandText = ''''
  set @viewSelectCommandText = ''''
  set @declareCommandText = ''''
  set @resultOrder = ''''
  set @sortColumn = N''Id''
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
          join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = ''Administrator''
          where a2.[Login] = @login)))
    set @innerBatchId = 0

  set @declareCommandText = 
    N''declare @search table 
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
      , SourceCertificateId /*nvarchar(255)*/ uniqueidentifier
      , SourceCertificateYear int
      , TypographicNumber nvarchar(255)
      , [Status] nvarchar(255)
      , ParticipantID uniqueidentifier
      , primary key(id)
      )
    ''

  if isnull(@showCount, 0) = 0
    set @commandText = 
      N''select <innerHeader>
        dbo.GetExternalId(cne_certificate_request.Id) [Id]
        , dbo.GetExternalId(cne_certificate_request.BatchId) BatchId
        , cne_certificate_request.SourceCertificateNumber CertificateNumber
        , isnull(cnec.LastName, cne_certificate_request.LastName) LastName
        , isnull(cnec.FirstName, cne_certificate_request.FirstName) FirstName
        , isnull(cnec.PatronymicName, cne_certificate_request.PatronymicName) PatronymicName
        , isnull(cnec.PassportSeria, cne_certificate_request.PassportSeria) PassportSeria
        , isnull(cnec.PassportNumber, cne_certificate_request.PassportNumber) PassportNumber
        , case
          when not cne_certificate_request.ParticipantID is null then 1
          else 0
        end IsExist
        , region.Name RegionName
        , region.Code RegionCode
        , isnull(cne_certificate_request.IsDeny, 0) IsDeny 
        , cne_certificate_request.DenyComment DenyComment
        , cne_certificate_request.DenyNewCertificateNumber DenyNewCertificateNumber
        , cne_certificate_request.SourceCertificateIdGuid_test
        , cne_certificate_request.SourceCertificateYear
        , cne_certificate_request.TypographicNumber
        , case when cne_certificate_request.ParticipantID is null then ''''Не найдено'''' else 
          case when isnull(cne_certificate_request.IsDeny, 0) = 0 and getdate() <= 

ed.[ExpireDate] then ''''Действительно'''' 
          else ''''Истек срок'''' end end as [Status]
        , cne_certificate_request.ParticipantID
      from 
        dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
            on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id]
          left outer join dbo.Region region with (nolock)
            on region.[Id] = cne_certificate_request.SourceRegionId
          left join [ExpireDate] as ed with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = ed.[Year] 
          left join vw_Examcertificate cnec with (nolock) on 

cne_certificate_request.[SourceCertificateYear] = cnec.year and cne_certificate_request.SourceCertificateIdGuid_test = cast(cnec.id as nvarchar(255)) 
      where 1 = 1 ''
  else
    set @commandText = 
      N''
      select count(*)
      from 
        dbo.CommonNationalExamCertificateRequestBatch cne_certificate_request_batch with (nolock)
          inner join dbo.CommonNationalExamCertificateRequest cne_certificate_request with 

(nolock)
            on cne_certificate_request.BatchId = cne_certificate_request_batch.[Id] 
          left outer join dbo.Region region with (nolock)
            on region.[Id] = cne_certificate_request.SourceRegionId
      where 1 = 1 '' 

  set @commandText = @commandText +
      ''   and cne_certificate_request_batch.[Id] = @innerBatchId 
        and cne_certificate_request_batch.IsProcess = 0 ''

  if isnull(@showCount, 0) = 0
  begin

    if @sortColumn = ''Id''
    begin
      set @innerOrder = '' order by Id <orderDirection> ''
      set @outerOrder = '' order by Id <orderDirection> ''
      set @resultOrder = '' order by Id <orderDirection> ''
    end
    else 
    begin
      set @innerOrder = '' order by Id <orderDirection> ''
      set @outerOrder = '' order by Id <orderDirection> ''
      set @resultOrder = '' order by Id <orderDirection> ''
    end

    if @sortAsc = 1
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''asc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''desc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''asc'')
    end
    else
    begin
      set @innerOrder = replace(@innerOrder, ''<orderDirection>'', ''desc'')
      set @outerOrder = replace(@outerOrder, ''<orderDirection>'', ''asc'')
      set @resultOrder = replace(@resultOrder, ''<orderDirection>'', ''desc'')
    end

    if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @startRowIndex - 1 + @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
    begin
      set @innerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
      set @outerSelectHeader = replace(''top <count>'', ''<count>'', @maxRowCount)
    end
    else if isnull(@maxRowCount, -1) = -1 
    begin
      set @innerSelectHeader = ''top 10000000''
      set @outerSelectHeader = ''top 10000000''
    end

    set @commandText = replace(replace(replace(
        N''insert into @search '' + 
        N'' select <outerHeader> * '' + 
        N'' from (<innerSelect>) as innerSelect '' + @outerOrder
        , N''<innerSelect>'', @commandText + @innerOrder)
        , N''<innerHeader>'', @innerSelectHeader)
        , N''<outerHeader>'', @outerSelectHeader)
  end

  if isnull(@showCount, 0) = 0
  begin
    if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
    begin
      set @declareCommandText = @declareCommandText +
        N'' declare @subjects table  
          ( 
          CertificateId uniqueidentifier 
          , Mark nvarchar(10)
          , HasAppeal bit  
          , SubjectCode nvarchar(255)  
          , HasExam bit
          , primary key(CertificateId, SubjectCode)
          ) 
        ''

      set @commandText = @commandText +
        N''insert into @subjects  
        select
          cne_certificate_subject.CertificateFK 
          , case when cne_certificate_subject.[Mark] < mm.[MinimalMark] then ''''!'''' else '''''''' 

end + replace(cast(cne_certificate_subject.[Mark] as nvarchar(9)),''''.'''','''','''')
          , cne_certificate_subject.HasAppeal
          , subject.Code
          , 1 
        from  
          [prn].CertificatesMarks cne_certificate_subject
          left join dbo.Subject subject on subject.SubjectId = 

cne_certificate_subject.SubjectCode
          left join [MinimalMark] as mm on cne_certificate_subject.[SubjectCode] = 

mm.[SubjectId] and cne_certificate_subject.UseYear = mm.[Year]
        where 
          exists(select 1 
              from @search search
              where 
              /*cast(cne_certificate_subject.CertificateFK as nvarchar(255)) = */
cne_certificate_subject.CertificateFK=
search.SourceCertificateId
                and cne_certificate_subject.[UseYear] = 

search.SourceCertificateYear)
        '' 
    end
    
    set @viewSelectCommandText = 
      N''select
        search.Id 
        , search.BatchId
       /* , case when search.SourceCertificateId =''''Нет свидетельства'''' then ''''Нет свидетества'''' else search.CertificateNumber 
			end CertificateNumber*/
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
        , search.ParticipantID
      ''

    set @viewCommandText = 
      N'' ,unique_cheks.UniqueIHEaFCheck
      from @search search 
      left outer join dbo.ExamCertificateUniqueChecks unique_cheks
          on unique_cheks.idGUID = search.SourceCertificateId ''

    if ((isnull(@isExtended, 0) = 1) or (isnull(@isExtendedByExam, 0) = 1))
    begin 
      declare
        @subjectCode nvarchar(255)
        , @pivotSelect nvarchar(4000)

      set @pivotSelect = ''''

      declare subject_cursor cursor fast_forward for
      select s.Code
      from dbo.Subject s with(nolock)
      order by s.id asc 

      open subject_cursor 
      fetch next from subject_cursor into @subjectCode
      while @@fetch_status = 0
        begin
        if len(@pivotSubjectColumns) > 0
          set @pivotSubjectColumns = @pivotSubjectColumns + '',''
        set @pivotSubjectColumns = @pivotSubjectColumns + replace(''[<code>]'', ''<code>'', 

@subjectCode)
        
        if isnull(@isExtended, 0) = 1
          set @pivotSelect =  
            N''  , mrk_pvt.[<code>] [<code>Mark]  
              , apl_pvt.[<code>] [<code>HasAppeal] ''
        if isnull(@isExtendedByExam, 0) = 1
          set @pivotSelect = @pivotSelect + 
            N'' 
              , isnull(exam_pvt.[<code>], 0) [<code>HasExam] ''
            
        set @pivotSelect = replace(@pivotSelect, ''<code>'', @subjectCode)

        if len(@viewSelectPivot1CommandText) + len(@pivotSelect) <= 4000
          and @viewSelectPivot2CommandText = ''''
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
        N''left outer join (select 
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
            on search.SourceCertificateId = apl_pvt.CertificateId ''
        set @viewCommandText = replace(@viewCommandText, ''<subject_columns>'', @pivotSubjectColumns)
    end
          
    if isnull(@isExtendedByExam, 0) = 1
    begin
      set @viewCommandText = @viewCommandText + 
        N''left outer join (select 
          subjects.CertificateId
          , subjects.SubjectCode
          , cast(subjects.HasExam as int) HasExam 
          from @subjects subjects) subjects
            pivot (Sum(HasExam) for SubjectCode in (<subject_columns>)) as exam_pvt
          on search.SourceCertificateId = exam_pvt.CertificateId  ''
          
      set @viewCommandText = replace(@viewCommandText, ''<subject_columns>'', @pivotSubjectColumns)
    end
  end

  set @viewCommandText = @viewCommandText + @resultOrder

  set @commandText = @declareCommandText + @commandText + @viewSelectCommandText +
      @viewSelectPivot1CommandText + @viewSelectPivot2CommandText + @viewCommandText

  --select @commandText

  exec sp_executesql @commandText
    , N''@innerBatchId bigint''
    , @innerBatchId
    
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateRequest]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateRequest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE proc [dbo].[SearchCommonNationalExamCertificateRequest]
	@login nvarchar(255)
	, @batchId bigint
	, @startRowIndex int = 1
	, @maxRowCount int = null
	, @showCount bit = null
	, @isExtended bit = null
	, @isExtendedByExam bit = null
as
begin
SELECT 
	ch.Id,
	ch.GroupId,	
	ch.ParticipantId,
	ch.UseYear,
	ch.RegionId,
	A1.RegionName,
	ISNULL(ch.Surname, ch.CheckSurname) AS Surname,
	ISNULL(ch.Name, ch.CheckName) AS Name,
	ISNULL(ch.SecondName, ch.CheckSecondName) AS SecondName,
	ISNULL(ch.DocumentSeries, ch.CheckDocumentSeries) AS DocumentSeries,
	ISNULL(ch.DocumentNumber, ch.CheckDocumentNumber) AS DocumentNumber,
	A1.CertificateID,
	ISNULL(A1.Cancelled, 0) AS Cancelled,
	'''' AS DenyNewCertificateNumber,
	A1.StatusName,
	A1.GlobalStatusID,
	'''' AS DenyComment,
	0 AS UniqueIHEaFCheck,
	A1.LicenseNumber AS CertificateNumber,
	A1.TypographicNumber AS TypographicNumber,		
	A1.RussianMark,
	A1.RussianHasAppeal,
	A1.MathematicsMark,
	A1.MathematicsHasAppeal,
	A1.PhysicsMark,
	A1.PhysicsHasAppeal,
	A1.ChemistryMark,
	A1.ChemistryHasAppeal,
	A1.InformationScienceMark,
	A1.InformationScienceHasAppeal,
	A1.BiologyMark,
	A1.BiologyHasAppeal,
	A1.RussiaHistoryMark,
	A1.RussiaHistoryHasAppeal,
	A1.GeographyMark,
	A1.GeographyHasAppeal,
	A1.EnglishMark,
	A1.EnglishHasAppeal,
	A1.GermanMark,
	A1.GermanHasAppeal,
	A1.FranchMark,
	A1.FranchHasAppeal,
	A1.SocialScienceMark,
	A1.SocialScienceHasAppeal,
	A1.SpanishMark,
	A1.SpanishHasAppeal,
	A1.LiteratureMark,
	A1.LiteratureHasAppeal
FROM 
	dbo.CheckHistory h
	JOIN dbo.Account a ON 
		h.OwnerId = a.Id	
	JOIN CheckResultsHistory ch ON 
		h.Id = ch.CheckId
	LEFT JOIN 
	(SELECT  
		ch.Id,
		r.RegionName,
		rsg.StatusName,
		rsg.GlobalStatusID,		
		cer.CertificateID,
		cer.Cancelled,
		cer.LicenseNumber,
		cer.TypographicNumber,				
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 1) as RussianMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 1) as RussianHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 2) as MathematicsMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 3) as MathematicsHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 3) as PhysicsMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 3) as PhysicsHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 4) as ChemistryMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 4) as ChemistryHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 5) as InformationScienceMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 5) as InformationScienceHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 6) as BiologyMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 6) as BiologyHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 7) as RussiaHistoryMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 7) as RussiaHistoryHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 8) as GeographyMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 8) as GeographyHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 9) as EnglishMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 9) as EnglishHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 10) as GermanMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 10) as GermanHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 11) as FranchMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 11) as FranchHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 12) as SocialScienceMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 12) as SocialScienceHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 13) as SpanishMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 13) as SpanishHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 18) as LiteratureMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 18) as LiteratureHasAppeal
	FROM
		dbo.CheckHistory h 
		JOIN dbo.Account a ON 
			h.OwnerId = a.Id
		JOIN dbo.CheckResultsHistory ch ON
			h.Id = ch.CheckId
		JOIN rbdc.Regions r ON
			r.REGION = ch.RegionId
		JOIN prn.CertificatesMarks cm ON 
			cm.ParticipantFK = ch.ParticipantId AND
			cm.REGION = ch.RegionId AND
			ch.UseYear = cm.UseYear
		JOIN prn.Certificates cer ON
			cer.CertificateID = cm.CertificateFK AND
			cer.UseYear = cm.UseYear AND
			cer.REGION = cm.REGION
		JOIN dat.Subjects s ON
			s.SubjectCode = cm.SubjectCode
		JOIN dbo.ResultStatuses rs ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg ON 
			rsg.GlobalStatusID = rs.GlobalStatusID		 
	WHERE 
		h.BatchId = @batchId AND h.CheckTypeId IN (2, 3) AND a.LoginTrimmed = LTRIM(RTRIM(UPPER(@login)))
	GROUP BY 	
		ch.Id,
		r.RegionName,	
		rsg.StatusName,
		rsg.GlobalStatusID,				
		cer.CertificateID,
		cer.Cancelled,
		cer.LicenseNumber,
		cer.TypographicNumber) AS A1
	ON ch.Id = A1.Id
WHERE h.BatchId = @batchId AND h.CheckTypeId IN (2, 3) AND a.LoginTrimmed = LTRIM(RTRIM(UPPER(@login)))	
		
	return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheck]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheck]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SearchCommonNationalExamCertificateCheck]
	  @login nvarchar(255)
	, @batchId bigint			-- id пакета
	, @startRowIndex int = 1	-- пейджинг
	, @maxRowCount int = null	-- пейджинг
	, @showCount bit = null    -- если > 0, то выбирается общее кол-во
	, @certNumber nvarchar(50) = null -- опциональный номер сертификата
as
BEGIN

SELECT 
	ch.Id,
	ch.GroupId,	
	ch.ParticipantId,
	ch.UseYear,
	ch.RegionId,
	A1.RegionName,
	ISNULL(ch.Surname, ch.CheckSurname) AS Surname,
	ISNULL(ch.Name, ch.CheckName) AS Name,
	ISNULL(ch.SecondName, ch.CheckSecondName) AS SecondName,
	ISNULL(ch.DocumentSeries, ch.CheckDocumentSeries) AS DocumentSeries,
	ISNULL(ch.DocumentNumber, ch.CheckDocumentNumber) AS DocumentNumber,
	A1.CertificateID,
	ISNULL(A1.Cancelled, 0) AS Cancelled,
	'''' AS DenyNewCertificateNumber,
	A1.StatusName,
	A1.GlobalStatusID,	
	'''' AS DenyComment,
	0 AS UniqueIHEaFCheck,
	A1.LicenseNumber AS CertificateNumber,
	A1.TypographicNumber AS TypographicNumber,		
	A1.RussianMark,
	A1.RussianHasAppeal,
	A1.MathematicsMark,
	A1.MathematicsHasAppeal,
	A1.PhysicsMark,
	A1.PhysicsHasAppeal,
	A1.ChemistryMark,
	A1.ChemistryHasAppeal,
	A1.InformationScienceMark,
	A1.InformationScienceHasAppeal,
	A1.BiologyMark,
	A1.BiologyHasAppeal,
	A1.RussiaHistoryMark,
	A1.RussiaHistoryHasAppeal,
	A1.GeographyMark,
	A1.GeographyHasAppeal,
	A1.EnglishMark,
	A1.EnglishHasAppeal,
	A1.GermanMark,
	A1.GermanHasAppeal,
	A1.FranchMark,
	A1.FranchHasAppeal,
	A1.SocialScienceMark,
	A1.SocialScienceHasAppeal,
	A1.SpanishMark,
	A1.SpanishHasAppeal,
	A1.LiteratureMark,
	A1.LiteratureHasAppeal
FROM 
	dbo.CheckHistory h
	JOIN dbo.Account a ON 
		h.OwnerId = a.Id	
	JOIN CheckResultsHistory ch ON 
		h.Id = ch.CheckId
	LEFT JOIN 
	(SELECT  
		ch.Id,
		r.RegionName,
		rsg.StatusName,
		rsg.GlobalStatusID,
		cer.CertificateID,		
		cer.Cancelled,
		cer.LicenseNumber,
		cer.TypographicNumber,	
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 1) as RussianMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 1) as RussianHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 2) as MathematicsMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 3) as MathematicsHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 3) as PhysicsMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 3) as PhysicsHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 4) as ChemistryMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 4) as ChemistryHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 5) as InformationScienceMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 5) as InformationScienceHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 6) as BiologyMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 6) as BiologyHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 7) as RussiaHistoryMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 7) as RussiaHistoryHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 8) as GeographyMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 8) as GeographyHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 9) as EnglishMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 9) as EnglishHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 10) as GermanMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 10) as GermanHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 11) as FranchMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 11) as FranchHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 12) as SocialScienceMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 12) as SocialScienceHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 13) as SpanishMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 13) as SpanishHasAppeal,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 18) as LiteratureMark,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 18) as LiteratureHasAppeal
	FROM
		dbo.CheckHistory h 
		JOIN dbo.Account a ON 
			h.OwnerId = a.Id
		JOIN dbo.CheckResultsHistory ch ON
			h.Id = ch.CheckId
		JOIN rbdc.Regions r ON
			r.REGION = ch.RegionId
		JOIN prn.CertificatesMarks cm ON 
			cm.ParticipantFK = ch.ParticipantId AND
			cm.REGION = ch.RegionId AND
			ch.UseYear = cm.UseYear
		JOIN prn.Certificates cer ON
			cer.CertificateID = cm.CertificateFK AND
			cer.UseYear = cm.UseYear AND
			cer.REGION = cm.REGION
		JOIN dat.Subjects s ON
			s.SubjectCode = cm.SubjectCode
		JOIN dbo.ResultStatuses rs ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg ON 
			rsg.GlobalStatusID = rs.GlobalStatusID		 
	WHERE 
		h.BatchId = @batchId AND h.CheckTypeId = 1
	GROUP BY 	
		ch.Id,
		r.RegionName,	
		rsg.StatusName,
		rsg.GlobalStatusID,
		cer.CertificateID,
		cer.Cancelled,
		cer.LicenseNumber,
		cer.TypographicNumber) AS A1
	ON ch.Id = A1.Id
WHERE h.BatchId = @batchId AND h.CheckTypeId = 1
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckHistory]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckHistory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- получить все уникальные проверки сертификата вузами и их филиалами
CREATE proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
  @certificateId uniqueidentifier,  -- id сертификата
  @startRow INT = NULL, -- пейджинг, если null - то выбирается кол-во записей для этого сертификата всего
  @maxRow INT = NULL    -- пейджинг
AS
BEGIN
-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
  SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg 
        INNER JOIN vw_Examcertificate c ON c.Number = lg.CertificateNumber
        INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
        INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
        WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0
  RETURN
END
SELECT   @certificateId AS CertificateId, * FROM (
      select org.Id AS OrganizationId,
      org.FullName AS OrganizationFullName,
      lg.[Date] AS [Date],
      lg.IsBatch AS CheckType,
      DENSE_RANK() OVER(ORDER BY org.FullName) AS org
      FROM dbo.CheckCommonNationalExamCertificateLog lg 
          INNER JOIN vw_Examcertificate c ON c.Number = lg.CertificateNumber
        INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
        INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
        WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0) rowTable 
      WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow 
      ORDER BY org, rowTable.[Date] 
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[IsUserOrgLogDisabled]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IsUserOrgLogDisabled]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- Проверка того, что для организации, к которой принадлежит пользователь с указанным логином 
-- отключено логирование истории проверок
CREATE FUNCTION [dbo].[IsUserOrgLogDisabled]
(
	@login nvarchar(255)
)
RETURNS BIT -- 1 - логирование отключено, 0 - логирование включено
AS
BEGIN
	declare @logDisabled BIT
	
	select TOP 1
		@logDisabled = ISNULL(org.DisableLog, 0)
		from dbo.Organization2010 org
		inner join dbo.Account acc on acc.OrganizationId = org.Id
	where acc.Login = @login

	return ISNULL(@logDisabled, 0)

END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetSubject]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubject]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetSubject

-- =============================================
-- Получение списка предметов.
-- v.1.0: Created by Makarev Andrey 04.05.2008
-- v.1.1: Modified by Makarev Andrey 05.05.2008
-- Добавлены хинты.
-- v.1.2: Modified by Fomin Dmitriy 30.05.2008
-- Отдавать ИД без шифрования.
-- =============================================
CREATE proc [dbo].[GetSubject]
as
begin
  select
    [subject].[Id] [Id], [subject].[Code] Code
    , [subject].[Name] [Name]
  from
    dbo.Subject [subject] with (nolock)
  where
    [subject].IsActive = 1
  order by 
    [subject].SortIndex

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получение информации о пользователе.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- v.1.2: Modified by Makarev Andrey 23.06.2008
-- Добавлено поле HasCrocEgeIntegration.
-- v.1.3: Modified by Fomin Dmitriy 07.07.2008
-- Добавлены поля EducationInstitutionTypeId, 
-- EducationInstitutionTypeName.
-- v.2.0 Modified by A.Vinichenko 14.04.2011
-- Информация об организации пользоватетеля берется 
-- из таблицы Organization2010
-- =============================================
CREATE procedure [dbo].[GetUserAccount]
  @login nvarchar(255)
as
begin
  declare @currentYear int, @accountId bigint--, @userGroupId int

  set @currentYear = Year(GetDate())

--  select @userGroupId = [group].Id
--  from dbo.[Group] [group] with (nolock, fastfirstrow)
--  where [group].Code = ''User''

  select @accountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  select account.[Login]
    , account.LastName
    , account.FirstName
    , account.PatronymicName
    , region.[Id] OrganizationRegionId
    , region.[Name] OrganizationRegionName
    , OReq.Id OrganizationId
    , OReq.FullName OrganizationName
    , OReq.OwnerDepartment OrganizationFounderName
    , OReq.LawAddress OrganizationAddress
    , OReq.DirectorFullName OrganizationChiefName
    , OReq.Fax OrganizationFax
    , OReq.Phone OrganizationPhone
    , OReq.EMail OrganizationEmail
    , OReq.Site OrganizationSite
    , OReq.ShortName OrganizationShortName
    , OReq.FactAddress OrganizationFactAddress
    , OReq.DirectorPosition OrganizationDirectorPosition
    , OReq.IsPrivate OrganizationIsPrivate
    , OReq.IsFilial OrganizationIsFilial
    , OReq.PhoneCityCode OrganizationPhoneCode
    , OReq.AccreditationSertificate AccreditationSertificate
    , OReq.INN OrganizationINN
    , OReq.OGRN OrganizationOGRN
    , account.Phone
    , account.Email
    , account.IpAddresses IpAddresses 
    , account.Status 
    , case
      when account.CanViewUserAccountRegistrationDocument = 1 
        then account.RegistrationDocument 
      else null
    end RegistrationDocument 
    , case
      when account.CanViewUserAccountRegistrationDocument = 1 
        then account.RegistrationDocumentContentType
      else null
    end RegistrationDocumentContentType
    , account.AdminComment AdminComment
    , dbo.CanEditUserAccount(account.Status, account.ConfirmYear, @currentYear) CanEdit
    , dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument 
    , account.HasFixedIp HasFixedIp
    , account.HasCrocEgeIntegration HasCrocEgeIntegration
    , OrgType.Id OrgTypeId
    , OrgType.[Name] OrgTypeName
    , OrgKind.Id OrgKindId
    , OrgKind.[Name] OrgKindName
    , OReq.Id OReqId
  from (select
        account.[Login] [Login]
        , account.LastName LastName
        , account.FirstName FirstName
        , account.PatronymicName PatronymicName
        , account.OrganizationId OrganizationId
        , account.Phone Phone
        , account.Email Email
        , account.ConfirmYear ConfirmYear
        , account.RegistrationDocument RegistrationDocument
        , account.RegistrationDocumentContentType RegistrationDocumentContentType
        , account.AdminComment AdminComment
        , account.IpAddresses IpAddresses
        , account.HasFixedIp HasFixedIp
        , account.HasCrocEgeIntegration HasCrocEgeIntegration
        , dbo.GetUserStatus(account.ConfirmYear, account.Status
            , @currentYear, account.RegistrationDocument) Status 
        , dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument
      from dbo.Account account with (nolock, fastfirstrow)
      where account.[Id] = @accountId
--          and account.Id in (
--            select group_account.AccountId
--            from dbo.GroupAccount group_account
--            where group_account.GroupId = @userGroupId)
      ) account
      left outer join dbo.Organization2010 OReq with (nolock, fastfirstrow) 
      left outer join dbo.Region region with (nolock, fastfirstrow) on region.[Id] = OReq.RegionId
      left outer join dbo.OrganizationType2010 OrgType on OReq.TypeId = OrgType.Id
      left outer join dbo.OrganizationKind OrgKind on OReq.KindId = OrgKind.Id
        on OReq.[Id] = account.OrganizationId
  return 0
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOrganizationAccountsIds]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationAccountsIds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetOrganizationAccountsIds] (@login nvarchar(255))
RETURNS @accounts table (
	AccountId BIGINT,
	OrganizationId BIGINT NULL,
	OrganizationName NVARCHAR(500) NULL
)
AS
BEGIN
	IF (@login IS NULL)
	BEGIN 
		INSERT INTO @accounts
		SELECT a.Id, org.Id, org.ShortName
		FROM 
			dbo.Account a
			LEFT JOIN dbo.Organization2010 org ON
				a.OrganizationId = org.Id
	END
	ELSE
	BEGIN
		---------------------------------------------------------
		-- Все акаунты организации
		INSERT INTO @accounts
		SELECT DISTINCT ISNULL(a_org.Id, a.Id), org.Id, org.ShortName
		FROM dbo.Account a WITH (NOLOCK)
			LEFT JOIN dbo.Account a_org WITH (NOLOCK) ON
				a.OrganizationId = a_org.OrganizationId
			LEFT JOIN dbo.Organization2010 org ON
				a_org.OrganizationId = org.Id
		WHERE a.LoginTrimmed = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')			
		---------------------------------------------------------
	END
	RETURN
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryOrgPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryOrgPagesCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetCheckHistoryOrgPagesCount]
	@organizationId BIGINT,
	@uniqueOnly BIT = 0
AS 

IF (@uniqueOnly = 0)
BEGIN
	SELECT COUNT(*) 
	FROM 
		dbo.Account a
		JOIN dbo.CheckHistory ch ON a.Id = ch.OwnerId
		JOIN dbo.CheckResultsHistory cr ON cr.CheckId = ch.Id
	WHERE 
		a.OrganizationId = @organizationId
		AND cr.ParticipantId IS NOT NULL	
END		
ELSE BEGIN
	;WITH f AS (
		SELECT 1 AS Id		
		FROM 
			dbo.Account a 
			JOIN dbo.CheckHistory ch WITH (INDEX (PK_CheckHistoryId), INDEX(idx_OwnerId)) 
				ON a.Id = ch.OwnerId
			JOIN dbo.CheckResultsHistory cr	ON 	
				ch.Id = cr.CheckId
		WHERE 
			a.OrganizationId = @organizationId
			AND cr.ParticipantId IS NOT NULL			
		GROUP BY 
			ch.SenderTypeId,			
			cr.ParticipantId)
	SELECT COUNT(*)
	FROM f
END 
		' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryOrgPaged]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryOrgPaged]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[GetCheckHistoryOrgPaged]
	@organizationId BIGINT,
    @startRowIndex INT = 1, -- Начиная с какой строки выбирать
    @rowsOnPageCount INT = NULL, -- Сколько строк на странице    
    @uniqueOnly BIT = 0
AS 
BEGIN
DECLARE @totalRows INT
DECLARE @endRowIndex INT = @startRowIndex + @rowsOnPageCount

DECLARE @Ids TABLE (Id BIGINT, SenderTypeId INT, RowNumber INT IDENTITY(1, 1))

IF (@uniqueOnly = 0)
BEGIN
	INSERT INTO @Ids
	SELECT TOP (@endRowIndex)
		cr.Id,					
		ch.SenderTypeId
	FROM	
		dbo.Account a 
		JOIN dbo.CheckHistory ch WITH (INDEX (PK_CheckHistoryId), INDEX(idx_OwnerId)) 
			ON	a.Id = ch.OwnerId		
		JOIN CheckResultsHistory cr ON ch.Id = cr.CheckId
	WHERE
		a.OrganizationId = @organizationId 						
		AND cr.ParticipantId IS NOT NULL
	ORDER BY	
		cr.CreateDate DESC,
		cr.ParticipantId ASC
	
	SELECT 
		cr.CheckId,
		cr.GroupId,
		CASE WHEN f.SenderTypeId = 1 THEN ''Интерактивная'' ELSE ''Пакетная'' END AS SenderTypeName,
		cr.CreateDate,
		cr.Surname,
		cr.Name,
		cr.SecondName,
		cr.DocumentSeries,
		cr.DocumentNumber		
	FROM @Ids f
		JOIN CheckResultsHistory cr ON 
			f.Id = cr.Id
	WHERE f.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
END
ELSE BEGIN

	INSERT INTO @Ids
	SELECT TOP (@endRowIndex)
		cr.Id,			
		ch.SenderTypeId
	FROM
		dbo.CheckHistory ch	
		JOIN CheckResultsHistory cr ON 
			ch.Id = cr.CheckId
		JOIN (
			SELECT
				MAX(cr.Id) AS Id				
			FROM 
				dbo.Account a 
				JOIN dbo.CheckHistory ch WITH (INDEX (PK_CheckHistoryId), INDEX(idx_OwnerId)) 
					ON a.Id = ch.OwnerId
				JOIN dbo.CheckResultsHistory cr	ON 	
					ch.Id = cr.CheckId
			WHERE 
				a.OrganizationId = @organizationId
				AND cr.ParticipantId IS NOT NULL			
			GROUP BY 
				ch.SenderTypeId,			
				cr.ParticipantId) AS A1	
		ON A1.Id = cr.Id				
	ORDER BY	
		cr.CreateDate DESC,
		cr.ParticipantId ASC
		
	SELECT 
		cr.CheckId,
		cr.GroupId,
		CASE WHEN f.SenderTypeId = 1 THEN ''Интерактивная'' ELSE ''Пакетная'' END AS SenderTypeName,
		cr.CreateDate,
		cr.Surname,
		cr.Name,
		cr.SecondName,
		cr.DocumentSeries,
		cr.DocumentNumber		
	FROM @Ids f
		JOIN CheckResultsHistory cr ON 
			f.Id = cr.Id
	WHERE f.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
END	
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNEWebUICheckLog]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetNEWebUICheckLog]
    @login NVARCHAR(255),
    @startRowIndex INT = 1,
    @maxRowCount INT = NULL,
    @showCount BIT = NULL,   -- если > 0, то выбирается общее кол-во
    @TypeCode NVARCHAR(255) -- Тип проверки
AS 
    BEGIN
        DECLARE @accountId BIGINT,
            @endRowIndex INTEGER

        IF ISNULL(@maxRowCount, -1) = -1 
            SET @endRowIndex = 10000000
        ELSE 
            SET @endRowIndex = @startRowIndex + @maxRowCount

        IF EXISTS ( SELECT  1
                    FROM    [Account] AS a2
                            JOIN [GroupAccount] ga ON ga.[AccountId] = a2.[Id]
                            JOIN [Group] AS g ON ga.[GroupId] = g.[Id]
                                                 AND g.[Code] = ''Administrator''
                    WHERE   a2.[Login] = @login ) 
            SET @accountId = NULL
        ELSE 
            SET @accountId = ISNULL(( SELECT    account.[Id]
                                      FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
                                      WHERE     account.[Login] = @login
                                    ), 0)

        IF ISNULL(@showCount, 0) = 0 
            BEGIN 
                IF @accountId IS NULL 
                    SELECT  *
                    FROM    ( SELECT    b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,   
                                        case when isnumeric(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2))=1 then
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) end YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )
                ELSE 
                    SELECT  *
                    FROM    ( SELECT TOP ( @endRowIndex )
                                        b.Id,
                                        b.CNENumber,
                                        b.LastName,
                                        b.FirstName,
                                        b.PatronymicName,
                                        b.Marks,
                                        b.TypographicNumber,
                                        b.PassportSeria,
                                        b.PassportNumber,
                                        case when isnumeric(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2))=1 then
                                        2000
                                        + CAST(SUBSTRING(b.CNENumber,
                                                         LEN(b.CNENumber) - 1,
                                                         2) AS INT) end YearCertificate,
                                        CASE WHEN FoundedCNEId IS NULL THEN 0
                                             ELSE 1
                                        END CheckCertificate,
                                        c.[login] [login],
                                        EventDate,
                                        row_number() OVER ( ORDER BY b.EventDate DESC ) rn
                              FROM      ( SELECT TOP ( @endRowIndex )
                                                    b.id
                                          FROM      dbo.CNEWebUICheckLog b
                                                    WITH ( NOLOCK )
                                                    JOIN Account c ON b.AccountId = c.id
                                                    JOIN Organization2010 d ON d.id = c.OrganizationId
                                          WHERE     b.AccountId = @accountId
                                                    AND @TypeCode = TypeCode
                                                    AND d.DisableLog = 0
                                          ORDER BY  b.EventDate DESC
                                        ) a
                                        JOIN CNEWebUICheckLog b ON a.id = b.id
                                        JOIN Account c ON b.AccountId = c.id
                            ) s
                    WHERE   s.rn BETWEEN @startRowIndex AND @endRowIndex
                    OPTION  ( RECOMPILE )   
            END
        ELSE 
            IF @accountId IS NULL 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE )
            ELSE 
                SELECT  COUNT(*)
                FROM    dbo.CNEWebUICheckLog b WITH ( NOLOCK )
                        JOIN Account c ON b.AccountId = c.id
                        JOIN Organization2010 d ON d.id = c.OrganizationId
                WHERE   b.AccountId = @accountId
                        AND @TypeCode = TypeCode
                        AND d.DisableLog = 0
                OPTION  ( RECOMPILE ) 
        RETURN 0
    END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsBASE]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgsBASE]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgsBASE]()
  
  
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
,ISNULL(Org.ShortName,'''') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,Org.WasImportedAtStart AS [Создана из справочника]
,FD.[Name] AS [Имя ФО]
,FD.Code AS [Код ФО]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,''Частный''),0,''Гос-ный'') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,''Да''),0,''Нет'') AS [Филиал]
,CASE 
  WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
  THEN ''Нет''
  ELSE ''Есть''
  END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'''') AS [Свидетельство об аккредитации]
,CASE 
  WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''''))
  THEN ''Есть''
  ELSE ''Нет''
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportEditedOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportEditedOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportEditedOrgsTVF](
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
,ISNULL(Org.ShortName,'''') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,''Частный''),0,''Гос-ный'') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,''Да''),0,''Нет'') AS [Филиал]
,CASE 
  WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
  THEN ''Нет''
  ELSE ''Есть''
  END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'''') AS [Свидетельство об аккредитации]
,CASE 
  WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''''))
  THEN ''Есть''
  ELSE ''Нет''
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
  THEN ''Да''
  ELSE ''Нет''
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportPotentialAbusersTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportPotentialAbusersTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--Отчет: Отчет о потенциальных пользователях, осуществляющих перебор
CREATE funCTION [dbo].[ReportPotentialAbusersTVF](@periodBegin datetime,@periodEnd datetime)
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
      + isnull(cnecr.firstname,'''') 
      + isnull(cnecr.PatronymicName,'''')
      + isnull(cnecr.PassportSeria,'''')
      + isnull(cnecr.PassportNumber,'''')
      + isnull(cnecr.TypographicNumber,'''')
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
      + isnull(cnecc.firstname,'''') 
      + isnull(cnecc.PatronymicName,'''')
      + isnull(cnecc.PassportSeria,'''')
      + isnull(cnecc.PassportNumber,'''')
      + isnull(cnecc.TypographicNumber,'''')
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgsInfoTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgsInfoTVF](
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
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'''')+
ISNULL(ChLog.PassportSeria,'''')+
ISNULL(ChLog.PassportNumber,'''')+
ISNULL(ChLog.CNENumber,'''')+
ISNULL(ChLog.Marks,'''')) AS UniqueUIChecks 
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
,ISNULL(Org.[Краткое наименование],'''') AS [Краткое наименование]
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
  THEN ''Нет''
  ELSE ''Да''
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
THEN ''Не работает''
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN ''Работа неактивна''
ELSE ''Работает''
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoByRegionTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgsInfoByRegionTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--Добавлены поля кода организации, учредителя
CREATE function [dbo].[ReportOrgsInfoByRegionTVF](
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
,COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'''')+
ISNULL(ChLog.PassportSeria,'''')+
ISNULL(ChLog.PassportNumber,'''')+
ISNULL(ChLog.CNENumber,'''')+
ISNULL(ChLog.Marks,'''')) AS UniqueUIChecks 
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
,ISNULL(Org.ShortName,'''') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,REPLACE(REPLACE(Org.WasImportedAtStart,1,''Да''),0,''Нет'') AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,''Частный''),0,''Гос-ный'') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,''Да''),0,''Нет'') AS [Филиал]
,CASE 
  WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
  THEN ''Нет''
  ELSE ''Есть''
  END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'''') AS [Свидетельство об аккредитации]
,CASE 
  WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''''))
  THEN ''Есть''
  ELSE ''Нет''
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
THEN ''Не работает''
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN ''Работа неактивна''
ELSE ''Работает''
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
ORDER BY
  case when Org.MainId is null then Org.Id else Org.MainId end, Org.MainId, Org.FullName


RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusAccredTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportUserStatusAccredTVF](@periodBegin datetime,@periodEnd datetime)
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
SELECT ''На доработке'',''revision'',3
UNION
SELECT ''На регистрации'',''registration'',1
UNION
SELECT ''Отключен'',''deactivated'',5
UNION
SELECT ''Активирован'',''activated'',4
UNION
SELECT ''На согласовании'',''consideration'',2
UNION
SELECT ''Всего'',''total'',10

DECLARE @OPF TABLE
(
  [Name] NVARCHAR (50),
  Code BIT,
  [Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT ''Негосударственный'',1,1
UNION
SELECT ''Государственный'',0,0

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
SELECT ''ВУЗ'',1,''Всего'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT ''ССУЗ'',2,''Всего'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT ''Итого'',10,''-'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

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
      Org.AccreditationSertificate != '''' 
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
  Comb.StatusCode=''total''
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
,''В том числе на ''+convert(varchar(16),@periodEnd, 120)+'' за '' 
+ case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end
+'':'' 
, ''-''
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]=''activated'' then 1 else 0 end AS [Активирован],
    case when A.[Status]=''registration'' then 1 else 0 end AS [На регистрации],
    case when A.[Status]=''revision'' then 1 else 0 end AS [На доработке],
    case when A.[Status]=''consideration'' then 1 else 0 end AS [На согласовании],
    case when A.[Status]=''deactivated'' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
  INNER JOIN Organization2010 Org ON 
  (
    Org.Id=OReq.OrganizationId 
    AND (
      Org.IsAccredited=1 
      OR (
        Org.AccreditationSertificate != '''' 
        AND Org.AccreditationSertificate IS NOT NULL
        )
      )
  )
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status=''registration'' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportUserStatusTVF](@periodBegin datetime,@periodEnd datetime)
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
SELECT ''На доработке'',''revision'',3
UNION
SELECT ''На регистрации'',''registration'',1
UNION
SELECT ''Отключен'',''deactivated'',5
UNION
SELECT ''Активирован'',''activated'',4
UNION
SELECT ''На согласовании'',''consideration'',2
UNION
SELECT ''Всего'',''total'',10

DECLARE @OPF TABLE
(
  [Name] NVARCHAR (50),
  Code BIT,
  [Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT ''Негосударственный'',1,1
UNION
SELECT ''Государственный'',0,0

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
SELECT ''ВУЗ'',1,''Всего'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT ''ССУЗ'',2,''Всего'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT ''Итого'',10,''-'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

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
  Comb.StatusCode=''total''
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
,''В том числе на ''+convert(varchar(16),@periodEnd, 120)+'' за '' 
+ case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end
+'':'' 
, ''-''
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]=''activated'' then 1 else 0 end AS [Активирован],
    case when A.[Status]=''registration'' then 1 else 0 end AS [На регистрации],
    case when A.[Status]=''revision'' then 1 else 0 end AS [На доработке],
    case when A.[Status]=''consideration'' then 1 else 0 end AS [На согласовании],
    case when A.[Status]=''deactivated'' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status=''registration'' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgRequests]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgRequests]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
create FUNCTION [dbo].[ReportOrgRequests]
(	
	@periodBegin DATETIME,
    @periodEnd DATETIME
)
RETURNS @report TABLE 
([Наименование организации] nvarchar(4000),
 [ТИП ОУ] nvarchar(4000), 
 [ОПФ] nvarchar(4000),
 [Учредитель] nvarchar(4000),
 [Головное ОУ] nvarchar(4000),
 [Филиал] nvarchar(4000),
 [Статус регистрации] nvarchar(4000) null,
 [Обязательность регистрации] nvarchar(10) null,
 [Код субъекта] nvarchar(1000) null,
 [Регион учредителя(код)] int null,
 [Регион Филиала(код)] int null,
 [Количество ошибочных запросов] int,
 [Количество интерактивных проверок] int,
 [Количество пакетных проверок] int,
 [Количество уникальных проверок] int  
 )

AS begin

	IF @periodBegin IS NULL 
		SET @periodBegin = ''19000101''

	DECLARE @Temp_CNEWebUICheckLog TABLE(pk int PRIMARY KEY CLUSTERED identity(1,1),accountId BIGINT, count INT, FoundedCNEId nvarchar(510))
	
	insert @Temp_CNEWebUICheckLog
	select cb.AccountId, count(cb.id),cb.FoundedCNEId 
	FROM CNEWebUICheckLog cb with(index(IX_CNEWebUICheckLog_FoundedCNEId_EventDate_AccountId))
	where cb.eventdate between @periodBegin and @periodEnd
	group by cb.AccountId,FoundedCNEId
	
	--DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	--INSERT INTO @singleWrongCheck (accountId, count)
	--SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData 
	--FROM dbo.EventLog 
	--WHERE 
	---- это значит что ничего не найдено
	--	SourceEntityId IS NULL
	---- ''|%|%|'' - это значит что поиск по паспорту и оценок нет
	--	AND EventParams LIKE ''|%|%|'' 
	--	AND AccountId IS NOT null
	--	and [date] between @periodBegin and @periodEnd
	--GROUP BY AccountId

	DECLARE @batchWrongCheck TABLE(pk int PRIMARY KEY CLUSTERED identity(1,1),accountId BIGINT, count INT)

	INSERT INTO @batchWrongCheck(accountId, count)
	select t.AccountId, sum(cnt)
	from 
		(	
		select cb.OwnerAccountId AccountId, count(*) cnt
		FROM CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
		where c.[Year] is null and cb.UpdateDate between @periodBegin and @periodEnd and OwnerAccountId is not null
		group by cb.OwnerAccountId
		union all
		select cb.OwnerAccountId, count(*) cnt
		FROM CommonNationalExamCertificateRequest c 
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
		where cb.UpdateDate between @periodBegin and @periodEnd and c.SourceCertificateId IS NULL
		group by cb.OwnerAccountId
		union all
		select cb.AccountId, sum([count])
		FROM @Temp_CNEWebUICheckLog cb
		where cb.FoundedCNEId is null
		group by cb.AccountId
		) t
	group by t.AccountId
	
	--DECLARE @allWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

	--INSERT INTO @allWrongCheck( accountId, count )
	--SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleWrongCheck s LEFT JOIN @batchWrongCheck b ON s.accountId = b.accountId

	--INSERT INTO @allWrongCheck
	--SELECT accountId, count FROM @batchWrongCheck WHERE accountId NOT IN (SELECT accountId FROM @allWrongCheck)

	declare @table_cnts table(AccountId bigint,cnt1 int, cnt2 int)
	
	insert @table_cnts
	select t.AccountId, sum(cnt1),sum(cnt2)
	from 
		(
		select cb.OwnerAccountId AccountId, count(*) cnt1, 0 cnt2
		from CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
		where cb.UpdateDate between @periodBegin and @periodEnd
		group by cb.OwnerAccountId
		union all
		select cb.OwnerAccountId, count(*) cnt, 0
		FROM CommonNationalExamCertificateRequest c 
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
		where cb.UpdateDate between @periodBegin and @periodEnd
		group by cb.OwnerAccountId
		union all
		select cb.OwnerAccountId, count(*) cnt, 0
		FROM CommonNationalExamCertificateSumCheck c 
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id  
		where cb.UpdateDate between @periodBegin and @periodEnd
		group by cb.OwnerAccountId
		union all
		select cb.AccountId, 0, sum([count]) 
		FROM @Temp_CNEWebUICheckLog cb
		group by cb.AccountId							
	) t
	group by t.AccountId
	
	declare @tbl_unic table (pk int primary key identity(1,1),id int, OrganizationId bigint,cnt int)
	
	--insert @tbl_unic 
	--select 
	--select OrganizationId,count(distinct TypographicNumber)
	--from CommonNationalExamCertificateCheck c
	--	JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
	--	JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--where OrganizationId is not null
	--	and TypographicNumber is not null
	--	and isnull(CertificateNumber,'''')=''''
	--group by OrganizationId
	--union all
	--select OrganizationId,count( distinct CertificateNumber)
	--from CommonNationalExamCertificateCheck c
	--	JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
	--	JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--where OrganizationId is not null
	--	and isnull(CertificateNumber,'''') <>''''
	--group by OrganizationId
	--union all
	--select OrganizationId,count( distinct isnull(PassportSeria,'''')+PassportNumber)
	--from CommonNationalExamCertificateCheck c
	--	JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
	--	JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--where OrganizationId is not null
	--	and PassportNumber  is not null
	--	and isnull(CertificateNumber,'''')=''''
	--group by OrganizationId								
	--union all
	--select OrganizationId,count(distinct CNENumber) from CNEWebUICheckLog c
	--	inner join account a on a.id=c.AccountId
	--where CNENumber is not null
	--group by OrganizationId
	--union all
	--select OrganizationId,count(distinct isnull(PassportSeria,'''')+PassportNumber) from CNEWebUICheckLog c
	--	inner join account a on a.id=c.AccountId
	--where PassportNumber is not null
	--group by OrganizationId
	--union all
	--select OrganizationId,count(distinct TypographicNumber) from CNEWebUICheckLog c
	--	inner join account a on a.id=c.AccountId
	--where TypographicNumber is not null
	--group by OrganizationId
	--union all
	--select OrganizationId,count(distinct TypographicNumber)
	--from CommonNationalExamCertificateRequest c 
	--	JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
	--	JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--where TypographicNumber is not null and SourceCertificateNumber is null and PassportNumber is null
	--group by OrganizationId
	--union all
	--select OrganizationId,count( distinct SourceCertificateNumber)
	--from CommonNationalExamCertificateRequest c 
	--	JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
	--	JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--where SourceCertificateNumber is not null and TypographicNumber is null and PassportNumber is null
	--group by OrganizationId
	--union all
	--select OrganizationId,count( distinct isnull(PassportSeria,'''')+PassportNumber)
	--from CommonNationalExamCertificateRequest c 
	--	JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id  
	--	JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
	--where PassportNumber is not null and SourceCertificateNumber is null 
	--group by OrganizationId	
	
	insert @tbl_unic(OrganizationId,cnt)
	select OrganizationId, count(distinct CertificateNumber) from
	(
		select distinct Acc.OrganizationId,c.CertificateNumber 
		from CommonNationalExamCertificateCheck c
			JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id 
			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 					
		union all
		select distinct Acc.OrganizationId,r.Number
		from CommonNationalExamCertificateRequest c 
			JOIN CommonNationalExamCertificateRequestBatch cb ON c.BatchId=cb.Id 
			left JOIN CommonNationalExamCertificate r ON c.SourceCertificateId=r.Id 
  			JOIN Account Acc ON Acc.Id=cb.OwnerAccountId 
		union all
		select distinct Acc.OrganizationId,cb.CNENumber
			from CNEWebUICheckLog cb
				left join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 
				JOIN Account Acc ON Acc.Id=cb.AccountId 
			where FoundedCNEId is not null
		union all
		select distinct Acc.OrganizationId,c.Number
			from CNEWebUICheckLog cb
				join CommonNationalExamCertificate c ON cb.FoundedCNEId=c.Id 
				JOIN Account Acc ON Acc.Id=cb.AccountId 											
	) t
	group by OrganizationId	
	
	insert into @report
	select org.FullName, 
		   OrgType.[Name], 
		   REPLACE(REPLACE(Org.IsPrivate, 1, ''Частный''), 0, ''Гос-ный'') AS [ОПФ],
		    isnull(Dep.FullName,''Не установлено'') AS [Учредитель],
		   case when isnull(OrgMain.FullName,'''') ='''' then org.FullName else OrgMain.FullName end [Головное ОУ],
		   REPLACE(REPLACE(Org.IsFilial, 1, ''Да''), 0, ''Нет'') AS [Филиал],
	       ISNULL(ORS.Status, ''Регистрацию не начинали'') AS [Статус регистрации],
		   ISNULL(MDL.ModelType, ''Да'') [Обязательность регистрации],
		   Reg.Code AS [Код субъекта],
		   CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE Reg.Code END as [Регион учредителя(код)],
		   CASE WHEN OrgMain.FullName IS NOT NULL THEN OrgMain.RegionId ELSE Org.RegionId END as [Регион филиала(код)],
	       isnull(SUM(wc.[count]),0), isnull(sum(cnt2),0), isnull(sum(cnt1),0),
	       isnull(SUM(uc.cnt),0)	       
	from @table_cnts tc 
		left join @batchWrongCheck wc on tc.accountId=wc.accountId
		join dbo.Account acc on acc.Id = tc.accountId 
		join dbo.Organization2010 org on org.Id = acc.OrganizationId
		left join @tbl_unic uc on uc.OrganizationId=org.id		
		JOIN Region Reg ON Reg.Id = Org.RegionId
		JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
		LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
		left JOIN Organization2010 OrgMain ON OrgMain.Id = Org.MainId	
		LEFT JOIN ( select distinct orq.OrganizationId, cast(''Отключенно'' as nvarchar(4000)) as Status
                    from    Account A 
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                    where   Status = ''deactivated'' 
						and orq.OrganizationId not in (
										                select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in (''activated'',''consideration'',''revision'',''registration'') )
					union all
					select distinct orq.OrganizationId,cast(''На регистрации''as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = ''registration''
						and orq.OrganizationId not in (
														select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in( ''activated'',''consideration'',''revision'' ))
					union all
					select distinct orq.OrganizationId,cast(''На доработке''as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = ''revision''
						and orq.OrganizationId not in (
														select distinct orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status in(''activated'', ''consideration'' ))
					union all
					select distinct orq.OrganizationId, cast(''На согласовании''as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = ''consideration''
						and orq.OrganizationId not in (
														select  orq.OrganizationId
														from    Account A
															inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
														where   Status = ''activated'' )
					union all
					select distinct	orq.OrganizationId,	cast(''Активировано''as nvarchar(4000))
					from    Account A
						inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
					where   Status = ''activated''
					) ORS ON ORS.OrganizationId = Org.Id	
					LEFT JOIN ( select distinct
                                        a.Id,
                                        a.ModelType
                                from    ( select    o.Id,
                                                    ''Нет'' as ModelType
                                          from      Organization2010 O
                                          where     o.IsFilial = 1
                                                    and o.MainId in (
                                                    select  O.id
                                                    from    Organization2010 O
                                                            inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                                    )
                                          union all
                                          select    O.id,
                                                    ''Нет''
                                          from      Organization2010 O
                                                    inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id
                                          where     o.IsFilial = 1
                                                   
                                        ) A
                              ) MDL ON MDL.Id = Org.Id                              
group by org.Id, org.FullName,OrgType.[Name],Org.IsPrivate,Dep.FullName,OrgMain.FullName,Org.IsFilial,ORS.Status,
		 MDL.ModelType,Reg.Code,Dep.RegionId,OrgMain.RegionId,Org.RegionId,OrgType.SortOrder
order by OrgType.SortOrder

return
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgErrorRequests]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgErrorRequests]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
create FUNCTION [dbo].[ReportOrgErrorRequests]
(	
	@periodBegin DATETIME,
    @periodEnd DATETIME
)
RETURNS @report TABLE 
([Наименование организации] nvarchar(4000),
 [Количество запросов] int)

AS begin

IF @periodBegin IS NULL 
 SET @periodBegin = ''19000101''

DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @singleWrongCheck (accountId, count)
SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData FROM dbo.EventLog WHERE 
-- это значит что ничего не найдено
SourceEntityId IS NULL
-- ''|%|%|'' - это значит что поиск по паспорту и оценок нет
AND EventParams LIKE ''|%|%|'' 
AND AccountId IS NOT null
and [date] between @periodBegin and @periodEnd
GROUP BY AccountId

DECLARE @batchWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @batchWrongCheck(accountId, count)
SELECT  batch.OwnerAccountId,
        COUNT(DISTINCT batchCheck.PassportNumber
              + ISNULL(batchCheck.PassportSeria, ''''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   batchCheck.SourceCertificateId IS NULL
        AND batch.OwnerAccountId IS NOT NULL
        AND batchCheck.PassportNumber IS NOT null
		AND batch.UpdateDate between @periodBegin and @periodEnd
        AND batch.Type = 2
        AND NOT EXISTS ( SELECT *
                         FROM   dbo.CommonNationalExamCertificateSubjectCheck
                         WHERE  Mark IS NOT NULL
                                AND CheckId = batchCheck.id )
GROUP BY batch.OwnerAccountId


DECLARE @allWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @allWrongCheck
        ( accountId, count )
SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleWrongCheck s LEFT JOIN @batchWrongCheck b ON s.accountId = b.accountId

INSERT INTO @allWrongCheck
SELECT accountId, count FROM @batchWrongCheck WHERE accountId NOT IN (SELECT accountId FROM @allWrongCheck)

insert into @report
select org.FullName, SUM(wc.[count]) from @allWrongCheck wc inner join dbo.Account acc on acc.Id = wc.accountId 
inner join dbo.Organization2010 org on org.Id = acc.OrganizationId
where wc.[count] > 0
group by org.Id, org.FullName 
order by org.FullName

return
end

' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ_Accred]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_VUZ_Accred]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgActivation_VUZ_Accred]()
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
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

SELECT @VUZPriv = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
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
SELECT ''ВУЗ'',''Государственный'',@VUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

UNION ALL
SELECT ''ВУЗ'',''Негосударственный'',@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

UNION ALL
SELECT
''ВУЗ'',''Всего'',@VUZState+@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND Org.TypeId=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

return
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_VUZ]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgActivation_VUZ]()
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
SELECT ''ВУЗ'',''Государственный'',@VUZStateFilial+@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
''ВУЗ основной'',''Государственный'',@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
''ВУЗ филиал'',''Государственный'',@VUZStateFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT ''ВУЗ'',''Негосударственный'',@VUZPrivFilial+@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
''ВУЗ основной'',''Негосударственный'',@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
''ВУЗ филиал'',''Негосударственный'',@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
''ВУЗ'',''Всего'',@VUZStateMain+@VUZStateFilial+@VUZPrivMain+@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=1

return
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ_Accred]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_SSUZ_Accred]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgActivation_SSUZ_Accred]()
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
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010  Org WHERE Org.TypeId=2 AND  Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
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
SELECT ''ССУЗ'',''Государственный'',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND Org.TypeId=2 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

UNION ALL
SELECT
''ССУЗ'',''Негосударственный'',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND Org.TypeId=2 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))
UNION ALL
SELECT
''ССУЗ'',''Всего'',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND Org.TypeId=2
AND (Org.IsAccredited=1 
    OR (
      Org.AccreditationSertificate != '''' 
      AND Org.AccreditationSertificate IS NOT NULL
      ))

return
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_SSUZ]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgActivation_SSUZ]()
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
SELECT ''ССУЗ'',''Государственный'',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=0

UNION ALL
SELECT
''ССУЗ'',''Негосударственный'',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
''ССУЗ'',''Всего'',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=2

return
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_OTHER]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportOrgActivation_OTHER]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportOrgActivation_OTHER]()
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
SELECT ''РЦОИ'','''',@RCOI,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=3

UNION ALL
SELECT
''Орган управления образованием'','''',@OUO,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId=4 
UNION ALL
SELECT
''Другое'','''',@OtherOrg,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status=''registration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''consideration'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''revision'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''activated'' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status=''deactivated'' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code=''User'' AND OrgReq.TypeId<>1 AND OrgReq.TypeId<>2 AND OrgReq.TypeId<>3 AND OrgReq.TypeId<>4 AND OrgReq.TypeId<>5

return
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportNotRegistredOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportNotRegistredOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportNotRegistredOrgsTVF](
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckStatisticsTVFOpen]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckStatisticsTVFOpen]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[ReportCheckStatisticsTVFOpen]
    (
      @periodBegin DATETIME,
      @periodEnd DATETIME
    )
RETURNS @report TABLE
    (
      [Код региона] NVARCHAR(10) NULL,
      [Регион] NVARCHAR(100) NULL,
      [Уникальных пакетных проверок (по паспорту)] INT NULL,
      [Всего пакетных проверок (по паспорту)] INT NULL,
      [Уникальных пакетных проверок (по номеру)] INT NULL,
      [Всего пакетных проверок (по номеру)] INT NULL,
      [Уникальных интерактивных проверок (по паспорту)] INT NULL,
      [Всего интерактивных проверок (по паспорту)] INT NULL,
      [Уникальных интерактивных проверок (по номеру)] INT NULL,
      [Всего интерактивных проверок (по номеру)] INT NULL
    )
AS BEGIN
IF @periodBegin IS NULL 
 SET @periodBegin = ''19000101''


    INSERT  INTO @report
            SELECT  ISNULL(r.code, '''') [Код региона],
                    ISNULL(r.name, ''Не указан'') [Регион],
                    SUM(p.UniqueBatchPassportCount) [Уникальных пакетных проверок (по паспорту)],
                    SUM(p.TotalBatchPassportCount) [Всего пакетных проверок (по паспорту)],
                    SUM(n.UniqueBatchNumberCount) [Уникальных пакетных проверок (по номеру)],
                    SUM(n.TotalBatchNumberCount) [Всего пакетных проверок (по номеру)],
                    SUM(iPassport.Uniq) [Уникальных интерактивных проверок (по паспорту)],
                    SUM(iPassport.Total) [Всего интерактивных проверок (по паспорту)],
                    SUM(iCNENumber.Uniq) [Уникальных интерактивных проверок (по номеру)],
                    SUM(iCNENumber.Total) [Всего интерактивных проверок (по номеру)]
            FROM    region r WITH ( NOLOCK )
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecc.SourceCertificateId)) UniqueBatchPassportCount,
                                        COUNT(*) TotalBatchPassportCount
                                FROM    [CommonNationalExamCertificateCheckBatch]
                                        AS cneccb WITH ( NOLOCK )
                                        JOIN [CommonNationalExamCertificateCheck]
                                        AS cnecc WITH ( NOLOCK ) ON cnecc.batchid = cneccb.id
																 AND cneccb.[Type] = 2
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cneccb.OwnerAccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                WHERE   cneccb.updatedate BETWEEN @periodBegin
                                                          AND     @periodEnd
                                GROUP BY ORg.regionid
                              ) p ON r.id = p.regionid
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, cnecc.SourceCertificateId)) UniqueBatchNumberCount,
                                        COUNT(*) TotalBatchNumberCount
                                FROM    [CommonNationalExamCertificateCheckBatch]
                                        AS cneccb WITH ( NOLOCK )
                                        JOIN [CommonNationalExamCertificateCheck]
                                        AS cnecc WITH ( NOLOCK ) ON cnecc.batchid = cneccb.id
																 AND cneccb.[Type] = 1
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = cneccb.OwnerAccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                WHERE   cneccb.updatedate BETWEEN @periodBegin
                                                          AND     @periodEnd
                                GROUP BY ORg.regionid
                              ) n ON r.id = n.regionid
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Uniq,
                                        COUNT(*) Total
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = ''CNENumberOpen''
                                GROUP BY ORg.regionid
                              ) iCNENumber ON iCNENumber.regionid = r.id
                    FULL JOIN ( SELECT  ORg.regionid,
                                        COUNT(DISTINCT CONVERT(NVARCHAR, ORg.Id)
                                              + CONVERT(NVARCHAR, ChLog.FoundedCNEId)) AS Uniq,
                                        COUNT(*) Total 
                                FROM    dbo.CNEWebUICheckLog AS ChLog WITH ( NOLOCK )
                                        INNER JOIN Account Acc WITH ( NOLOCK ) ON Acc.Id = ChLog.AccountId
                                        INNER JOIN GroupAccount GA WITH ( NOLOCK ) ON GA.AccountId = Acc.Id
                                                                                      AND GA.GroupId = 1
                                        INNER JOIN Organization2010 ORg ON Acc.OrganizationId = ORg.Id
                                                                                   AND ORg.Id IS NOT NULL
                                WHERE   ChLog.EventDate BETWEEN @periodBegin
                                                        AND     @periodEnd
                                        AND ChLog.FoundedCNEId IS NOT NULL
                                        AND ChLog.TypeCode = ''PassportOpen''
                                GROUP BY ORg.regionid
                              ) iPassport ON iPassport.regionid = r.id
            GROUP BY r.code,r.name
            ORDER BY MAX(r.id)
    RETURN
   END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckStatisticsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckStatisticsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[ReportCheckStatisticsTVF](@periodBegin datetime,@periodEnd datetime)
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
isnull(r.code,'''') [Код региона]
,isnull(r.name,''Не указан'') [Регион]

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
	AND ChLog.TypeCode= ''CNENumber''
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
	AND ChLog.TypeCode= ''Passport''
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
	AND ChLog.TypeCode= ''Typographic''
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
	AND ChLog.TypeCode= ''Marks''
	GROUP BY OReq.regionid
	) iMarks on iMarks.regionid = r.id
group by r.code, r.name
	order by max(r.id)


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '''', ''Итого за '' + case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end 
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
	AND ChLog.TypeCode= ''CNENumber''
	
	DECLARE @PassportUnique_UI INT
	SELECT @PassportUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR, ChLog.FoundedCNEId))
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= ''Passport''
	
	DECLARE @TypNumberUnique_UI INT
	SELECT @TypNumberUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= ''Typographic''
	
	DECLARE @MarksUnique_UI INT
	SELECT @MarksUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR, ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= ''Marks''

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
select '''', ''Итого за все время''
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

end' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksByPeriodTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportChecksByPeriodTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportChecksByPeriodTVF](
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
WHERE ChLog.TypeCode=''Passport''
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''CNENumber''
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''Typographic''
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''Marks''
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
''Пакетная'',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch,1
UNION
SELECT 
''Интерактивная'',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI,2
UNION
SELECT 
''Итого'',@Total_Batch+@Total_UI,@UniqueByNumber_Batch+@UniqueByNumber_UI,@UniqueByTypNumber_Batch+@UniqueByTypNumber_UI,@UniqueByPassport_Batch+@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_Batch+@TotalUnique_UI,3

RETURN
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksByOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportChecksByOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportChecksByOrgsTVF](
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
,REPLACE(REPLACE(Org.IsPrivate,1,''Негосударственный''),0,''Государственный'') AS [ОПФ]

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
THEN ''Не работает''
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN ''Работа неактивна''
ELSE ''Работает''
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportChecksAllTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportChecksAllTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportChecksAllTVF]()
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
WHERE ChLog.TypeCode=''Passport''


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''CNENumber''


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''Typographic''


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''Marks''


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
''Пакетная'',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch,8
UNION
SELECT 
''Интерактивная'',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI,9
UNION
SELECT 
''Итого'',@Total_Batch+@Total_UI,@UniqueByNumber_Batch+@UniqueByNumber_UI,@UniqueByTypNumber_Batch+@UniqueByTypNumber_UI,@UniqueByPassport_Batch+@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_Batch+@TotalUnique_UI,10

RETURN
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTotalChecksTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportTotalChecksTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportTotalChecksTVF](
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
WHERE ChLog.TypeCode=''Passport''
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по номеру
DECLARE @TotalByNumber_UI INT
DECLARE @UniqueByNumber_UI INT

SELECT @TotalByNumber_UI = COUNT(*), @UniqueByNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''CNENumber''
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по типографскому номеру
DECLARE @TotalByTypNumber_UI INT
DECLARE @UniqueByTypNumber_UI INT

SELECT @TotalByTypNumber_UI = COUNT(*), @UniqueByTypNumber_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''Typographic''
AND ChLog.EventDate BETWEEN @from and @to


--Интерактивные по баллам
DECLARE @TotalByMarks_UI INT
DECLARE @UniqueByMarks_UI INT

SELECT @TotalByMarks_UI = COUNT(*), @UniqueByMarks_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId))
FROM CNEWebUICheckLog ChLog
INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL
WHERE ChLog.TypeCode=''Marks''
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
''Пакетная'',@Total_Batch,@UniqueByNumber_Batch,@UniqueByTypNumber_Batch,@UniqueByPassport_Batch,0,@TotalUnique_Batch
UNION
SELECT 
''Интерактивная'',@Total_UI,@UniqueByNumber_UI,@UniqueByTypNumber_UI,@UniqueByPassport_UI,@UniqueByMarks_UI,@TotalUnique_UI

RETURN
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTopCheckingOrganizationsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportTopCheckingOrganizationsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportTopCheckingOrganizationsTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
  [№] nvarchar(10) null,
  [Организация (за период)] nvarchar(500) null,
  [Всего проверок (за период)] int null,
  [Организация (за все время)] nvarchar(500) null,
  [Всего проверок (за все время)] int null  
)
as 
begin

DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

insert into @report
select '''', ''ТОП 20 организаций за '' + case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end, null, ''ТОП 20 организаций за все время'',null
insert into @report
select c.rowid [№], c.Организация, c.[Всего проверок], t.Организация, t.[Всего проверок]
from 
(
  select 
  row_number() over (order by [Всего проверок] desc) as rowid
  ,*  
  from 
  (
  select top 20 
  o.FullName [Организация]
  ,(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateCheckBatch] cb with(nolock)
  join [CommonNationalExamCertificateCheck] c with(nolock) on cb.id = c.batchid 
  join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
  where cb.updatedate BETWEEN @periodBegin and @periodEnd)
  +
  (select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateRequestBatch] cb with(nolock)
  join [CommonNationalExamCertificateRequest] c with(nolock) on cb.id = c.batchid 
  join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
  where cb.updatedate BETWEEN @periodBegin and @periodEnd)
  [Всего проверок]
  from 
  OrganizationRequest2010 o with(nolock)
  order by [Всего проверок] desc
  ) c2
) c 
full join (
select  
  row_number() over (order by [Всего проверок] desc) as rowid
  ,*  
from 
  (
    select top 20 
    o.FullName [Организация]
    ,(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateCheckBatch] cb with(nolock)
    join [CommonNationalExamCertificateCheck] c  with(nolock) on cb.id = c.batchid 
    join [Account] a  with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
    )
    +
    (select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateRequestBatch] cb with(nolock)
    join [CommonNationalExamCertificateRequest] c with(nolock) on cb.id = c.batchid 
    join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id)
    [Всего проверок]
    from 
    OrganizationRequest2010 o with(nolock)
    order by [Всего проверок] desc
  )t2
) t on t.rowid = c.rowid
order by c.rowid asc


return
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportStatisticSubordinateOrg]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportStatisticSubordinateOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE funCTION  [dbo].[ReportStatisticSubordinateOrg](
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

/*

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
  COUNT(DISTINCT ISNULL(ChLog.TypographicNumber,'''')+
    ISNULL(ChLog.PassportSeria,'''')+
    ISNULL(ChLog.PassportNumber,'''')+
    ISNULL(ChLog.CNENumber,'''')+
    ISNULL(ChLog.Marks,'''')) AS UniqueUIChecks 
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

*/

-- Количество уникальных проверок
DECLARE @UniqueChecks TABLE
(
  OrganizationId INT,
  UniqueNumberChecks INT
)

INSERT INTO @UniqueChecks
SELECT 
  O.Id,
  COUNT(*) AS UniqueNumberChecks
FROM 
  Organization2010 O 
  inner join OrganizationRequest2010 ORR on ORR.OrganizationId = O.Id
  inner join OrganizationCertificateChecks OCC on OCC.OrganizationId = ORR.Id
WHERE
  O.DepartmentId = @departmentId
GROUP BY 
  O.Id



INSERT INTO @Report
select
  A.Id,
  A.FullName,
  A.RegionId,
  A.RegionName,
  A.AccreditationSertificate,
  A.DirectorFullName,
  A.CountUser,
  A.UserUpdateDate,
  isnull(UC.UniqueNumberChecks, 0) as CountUniqueChecks
from
  (
  select
    O.Id,
    O.FullName,
    O.RegionId,
    R.Name as RegionName,
    O.AccreditationSertificate,
    O.DirectorFullName,
    COUNT(A.Id) CountUser,
    MIN(A.UpdateDate) UserUpdateDate
  from
    Organization2010 O
    INNER JOIN Region R on R.Id = O.RegionId
    LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
    LEFT JOIN Account A on A.OrganizationId = OrR.Id
  where
    O.DepartmentId = @departmentId
  group by
    O.Id,
    O.FullName,
    O.RegionId,
    R.Name,
    O.AccreditationSertificate,
    O.DirectorFullName
  ) A 
  left join @UniqueChecks UC on UC.OrganizationId = A.Id


RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistredOrgsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportRegistredOrgsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportRegistredOrgsTVF](
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
' 
END
GO
/****** Object:  StoredProcedure [dbo].[RegisterEvent]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegisterEvent]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Добавление записи в таблицу EventLog.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлено поле UpdateId.
-- v.1.2: Modified by Makarev Andrey 18.04.2008
-- Регистрация событий по нескольким SourceEntityId.
-- v.1.3: Modified by Makarev Andrey 30.04.2008
-- Правильная работа с пустым @sourceEntityIds.
-- =============================================
CREATE proc [dbo].[RegisterEvent]
  @accountId bigint
  , @ip nvarchar(255)
  , @eventCode nvarchar(100)
  , @sourceEntityIds nvarchar(4000)
  , @eventParams ntext
  , @updateId uniqueidentifier = null
as
BEGIN
IF (ISNULL(@sourceEntityIds,'''') = '''' )
insert dbo.EventLog
    (
    date
    , accountId
    , ip
    , eventCode
    , sourceEntityId
    , eventParams
    , UpdateId
    )
    VALUES (GETDATE(), @accountId , @ip , @eventCode , null, @eventParams , @updateId) 
ELSE        
  insert dbo.EventLog
    (
    date
    , accountId
    , ip
    , eventCode
    , sourceEntityId
    , eventParams
    , UpdateId
    )
  select
    GetDate()
    , @accountId
    , @ip
    , @eventCode
    , ids.value
    , @eventParams
    , @updateId
  from
    dbo.GetDelimitedValues(@sourceEntityIds) ids

  return 0
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsBASE]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsBASE]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportCheckedCNEsBASE](
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
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateCheck c  ON c.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateCheckBatch cb ON c.BatchId=cb.Id
INNER JOIN Account Acc ON Acc.Id=cb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=0
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT   CNE.Id,CNE.Number,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CommonNationalExamCertificateRequest r ON r.SourceCertificateId=CNE.Id
INNER JOIN CommonNationalExamCertificateRequestBatch rb ON r.BatchId=rb.Id  AND rb.IsTypographicNumber=1
INNER JOIN Account Acc ON Acc.Id=rb.OwnerAccountId
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @PreReport(CNEId,CNENumber,OrgId)
SELECT CNE.Id AS CNEId,CNE.Number AS CNENumber,OReq.Id AS OrgId
FROM CommonNationalExamCertificate CNE
INNER JOIN CNEWebUICheckLog ChLog ON ChLog.FoundedCNEId=CNE.Id 
INNER JOIN Account Acc ON ChLog.AccountId=Acc.Id 
INNER JOIN Organization2010 OReq ON OReq.Id=Acc.OrganizationId and OReq.TypeId = 1

INSERT INTO @Report
SELECT DISTINCT * FROM @PreReport

RETURN
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetLoginAttemptsInfo]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLoginAttemptsInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-------------------------------------------------
--Автор: Сулиманов А.М.
--Дата: 2009-06-02
--Проверка количества попыток залогинится
-------------------------------------------------
CREATE procEDURE [dbo].[GetLoginAttemptsInfo]
( @IP varchar(32), 
  @TimeInterval int
)
AS
  SET NOCOUNT ON

  DECLARE @startDate datetime, @endDate datetime, @eventCode varchar(20)
  SET @endDate=GETDATE()
  SET @startDate=DATEADD(ss,-@TimeInterval,@endDate)
  SET @eventCode=''USR_VERIFY''

  SELECT 
      ISNULL(MAX(Date),CAST(''1900-01-01'' as datetime)) LastLoginDate, 
      @endDate as CheckedDate, 
      --COUNT(*) Attempts, 
      ISNULL(SUM([LoginFailResult]),0) AttemptsFail
  FROM (
    SELECT  
      --LEFT(EventParams,CHARINDEX(''|'',EventParams)-1) AS [Login],
      Date,
      CASE SUBSTRING(EventParams,LEN(EventParams)-2,1)
        WHEN ''1'' THEN 0
        ELSE 1
      END AS [LoginFailResult]
    FROM dbo.EventLog
    WHERE (Date between @startDate and @endDate) 
        AND EventCode=@eventCode AND IP=@IP
  ) T
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetOrganizationTypeReport]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrganizationTypeReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[GetOrganizationTypeReport]
as 
begin
  SELECT 
  OrgType.Id,
  OrgType.[Name] AS TypeName,
  REPLACE(REPLACE(ISNULL(IsPrivate,''''),''1'',''Негосударственный''),''0'',''Государственный'') AS OPF,
  ISNULL(UsersCount ,0) AS UsersCount
  FROM
    (SELECT OrgReq.TypeId AS TypeId,CONVERT(NVARCHAR(5),OrgReq.IsPrivate) AS IsPrivate, COUNT(Acc.Id) AS UsersCount
    FROM dbo.Account Acc
    INNER JOIN dbo.OrganizationRequest2010 OrgReq
    ON Acc.OrganizationId=OrgReq.Id
    GROUP BY OrgReq.TypeId,OrgReq.IsPrivate
    ) Rt
  RIGHT JOIN dbo.OrganizationType2010 OrgType
  ON OrgType.Id=TypeId
  UNION
  SELECT 6,''Итого'','''',COUNT(*) 
  FROM dbo.Account Acc 
  INNER JOIN dbo.OrganizationRequest2010 OrgReq
  ON Acc.OrganizationId=OrgReq.Id
  ORDER BY OrgType.Id
  --  declare
--    @year int
--
--  set @year = Year(GetDate())
--
--  select
--    [type].Name AS TypeName
--    , report.[Count]
--    , 0 IsSummary
--    , 0 IsTotal
--  from (select 
--      [type].Id OrganizationTypeId
--      , count(*) [Count]
--    from dbo.Account account
--      inner join dbo.OrganizationRequest2010 OrgReq
--        inner join dbo.OrganizationType2010 [type]
--          on [type].Id = OrgReq.TypeId
--        on OrgReq.Id = account.OrganizationId
--    where
--      account.Id in (select 
--            group_account.AccountId
--          from dbo.GroupAccount group_account
--            inner join dbo.[Group] [group]
--              on [group].Id = group_account.GroupId
--          where [group].Code = ''User'')
--      and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
--          , account.RegistrationDocument) = ''activated''
--    group by
--      [type].Id
--    with cube) report
--      left outer join dbo.OrganizationType2010 [type]
--        on [type].Id = report.OrganizationTypeId
--  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckByOuterId_tmp1]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckByOuterId_tmp1]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
CREATE PROC [dbo].[SearchCommonNationalExamCertificateCheckByOuterId_tmp1]
    @batchId BIGINT
AS 
    SET nocount ON
    DECLARE @id BIGINT,
        @isCorrect BIT
    SELECT  @id = id,
            @isCorrect = IsCorrect
    FROM    dbo.CommonNationalExamCertificateCheckBatch --WITH ( NOLOCK, FASTFIRSTROW )
    WHERE   outerId = @batchId
            AND IsProcess = 0
            AND Executing = 0
            
 select
    cnCheck.Id,
      
        cnCheck.BatchId,
        cnCheck.CertificateCheckingId,
        cnCheck.CertificateNumber,
        cnCheck.IsOriginal,
         cnCheck.IsCorrect ,
         cnCheck.IsDeny,
       ISNULL(unique_cheks.Id,0) UniqueCheckId,
    ISNULL(unique_cheks.UniqueIHEaFCheck,0) UniqueIHEaFCheck,
      ISNULL(unique_cheks.[year],0) UniqueCheckYear into #x
FROM      CommonNationalExamCertificateCheck cnCheck 
                          left outer join dbo.ExamCertificateUniqueChecks unique_cheks WITH ( NOLOCK) 	
						  on unique_cheks.Id = cnCheck.SourceCertificateId 
                          WHERE  cnCheck.BatchId = @id

CREATE NONCLUSTERED INDEX #x_id ON #x
(
	[Id] ASC
)  
  


   
SELECT  1 as Tag,
        0 as Parent,
        null  as [root!1!],
        -1  as [check!2!Id],
        null     as [check!2!IsBatchCorrect],
        null   as [check!2!BatchId],
       null  as [check!2!CertificateCheckingId],
        null   as [check!2!CertificateNumber],
       null  as [check!2!IsOriginal],
       null  as [check!2!IsCorrect],
       null  as [check!2!IsDeny],
        null   as [check!2!UniqueCheckId],
        null   as [check!2!UniqueIHEaFCheck],
        null   as [check!2!UniqueCheckYear],
        NULL          as [subjects!3!],
        null          as [subject!4!Id],
        NULL          as [subject!4!CheckId],
        NULL          as [subject!4!SubjectID],
        NULL          as [subject!4!Mark]
union all
SELECT  2 as Tag,
        1 as Parent,
        null  as [root!1!],
        Id  as [check!2!Id],
       @isCorrect     as [check!2!IsBatchCorrect],
        BatchId    as [check!2!BatchId],
       CertificateCheckingId    as [check!2!CertificateCheckingId],
        CertificateNumber    as [check!2!CertificateNumber],
        IsOriginal    as [check!2!IsOriginal],
         IsCorrect    as [check!2!IsCorrect],
         IsDeny    as [check!2!IsDeny],
        UniqueCheckId    as [check!2!UniqueCheckId],
         UniqueIHEaFCheck   as [check!2!UniqueIHEaFCheck],
        UniqueCheckYear    as [check!2!UniqueCheckYear],
       
        NULL          as [subjects!3!],
       null          as [subject!4!Id],
        NULL          as [subject!4!LineTotal],
        NULL          as [subject!4!ProductID],
        NULL          as [subject!4!OrderQty]
FROM      #x
union all
SELECT  3 as Tag,
        2 as Parent,
        null  as [root!1!],
        Id  as [check!2!Id],
        null     as [check!2!IsBatchCorrect],
        null   as [check!2!BatchId],
       null  as [check!2!CertificateCheckingId],
        null   as [check!2!CertificateNumber],
       null  as [check!2!IsOriginal],
       null  as [check!2!IsCorrect],
       null  as [check!2!IsDeny],
        null   as [check!2!UniqueCheckId],
        null   as [check!2!UniqueIHEaFCheck],
        null   as [check!2!UniqueCheckYear],
        null          as [subjects!3!],
        null          as [subject!4!Id],
        NULL          as [subject!4!LineTotal],
        NULL          as [subject!4!ProductID],
        NULL          as [subject!4!OrderQty]
FROM   #x
union all
SELECT  4 as Tag,
        3 as Parent,
        null  as [root!1!],
        #x.Id  as [check!2!Id],
        null     as [check!2!IsBatchCorrect],
        null   as [check!2!BatchId],
       null  as [check!2!CertificateCheckingId],
        null   as [check!2!CertificateNumber],
       null  as [check!2!IsOriginal],
       null  as [check!2!IsCorrect],
       null  as [check!2!IsDeny],
        null   as [check!2!UniqueCheckId],
        null   as [check!2!UniqueIHEaFCheck],
        null   as [check!2!UniqueCheckYear],
        null          as [subjects!3!],
        b.Id           as [subject!4!Id],
        b.CheckId          as [subject!4!CheckId],
        b.SubjectId          as [subject!4!SubjectId],
        b.Mark          as [subject!4!Mark]
FROM   #x
 join  CommonNationalExamCertificateSubjectCheck b on b.CheckId = #x.id
       
              

ORDER BY [check!2!Id],Tag
FOR XML EXPLICIT , TYPE


' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckByOuterId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Получить список проверенных сертификатов ЕГЭ пакета.
-- =============================================
CREATE PROC [dbo].[SearchCommonNationalExamCertificateCheckByOuterId]
    @batchId BIGINT,
    @xml XML OUT
AS 
    SET nocount ON
    DECLARE @id BIGINT,
        @isCorrect BIT
    SELECT  @id = id,
            @isCorrect = IsCorrect
    FROM    dbo.CommonNationalExamCertificateCheckBatch WITH ( NOLOCK, FASTFIRSTROW )
    WHERE   outerId = @batchId
            AND IsProcess = 0
            AND Executing = 0
    SET @xml = ( SELECT ( SELECT    cnCheck.Id AS ''@Id'',
                                    @isCorrect AS ''@IsBatchCorrect'',
                                    cnCheck.BatchId AS ''@BatchId'',
                                    cnCheck.CertificateCheckingId AS ''@CertificateCheckingId'',
                                    cnCheck.CertificateNumber AS ''@CertificateNumber'',
                                    cnCheck.IsOriginal AS ''@IsOriginal'',
                                    cnCheck.IsCorrect AS ''@IsCorrect'',
                                    cnCheck.SourceCertificateIdGuid AS ''@SourceCertificateIdGuid'',
                                    cnCheck.IsDeny AS ''@IsDeny'',
                                    cnCheck.DenyComment AS ''@DenyComment'',
                                    cnCheck.DenyNewCertificateNumber AS ''@DenyNewCertificateNumber'',
                                    cnCheck.Year AS ''@Year'',
                                    cnCheck.TypographicNumber AS ''@TypographicNumber'',
                                    cnCheck.RegionId AS ''@RegionId'',
                                    cnCheck.PassportSeria AS ''@PassportSeria'',
                                    cnCheck.PassportNumber AS ''@PassportNumber'',
                                    cnCheck.ParticipantFK AS ''@ParticipantFK'',
                                    ISNULL(unique_cheks.IdGuid,null) AS ''@UniqueCheckIdGuid'',
									ISNULL(unique_cheks.UniqueIHEaFCheck,0) AS ''@UniqueIHEaFCheck'',
									ISNULL(unique_cheks.[year],0) AS ''@UniqueCheckYear'',
                                    ( SELECT    b.Id AS ''subject/@Id'',
                                                b.CheckId AS ''subject/@CheckId'',
                                                b.SubjectId AS ''subject/@SubjectId'',
                                                b.Mark AS ''subject/@Mark'',
                                                b.IsCorrect AS ''subject/@IsCorrect'',
                                                b.SourceCertificateSubjectIdGuid AS ''subject/@SourceCertificateSubjectIdGuid'',
                                                b.SourceMark AS ''subject/@SourceMark'',
                                                b.SourceHasAppeal AS ''subject/@SourceHasAppeal'',
                                                b.Year AS ''subject/@Year''
                                      FROM      CommonNationalExamCertificateSubjectCheck b
                                      WHERE     --BatchId=@id and 
                                                b.CheckId = cnCheck.id
                                    FOR
                                      XML PATH(''''),
                                          ROOT(''subjects''),
                                          TYPE
                                    )
                          FROM      CommonNationalExamCertificateCheck cnCheck WITH ( NOLOCK)
						  left outer join dbo.ExamCertificateUniqueChecks unique_cheks WITH ( NOLOCK) 	on unique_cheks.IdGuid = cnCheck.SourceCertificateIdGuid 
                          WHERE     --cnCheck.id in(3201511,3201512)
                                    BatchId = @id
                        FOR
                          XML PATH(''check''),
                              TYPE
                        )
               FOR
                 XML PATH(''root''),
                     TYPE
               )
SELECT @xml               
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_Wildcard]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_Wildcard]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- 4.1.5.	Запрос по неполным данным
CREATE PROC [dbo].[SingleCheck_Wildcard]
	@senderType INT,					    -- тип отправителя
	@surname varchar(80) = null,		    -- фамилия сертифицируемого
	@name varchar(80) = null,			    -- имя сертифицируемого
	@secondName varchar(80) = null,		    -- отчетсво сертифицируемого
	@documentNumber varchar(10) = null,		-- номер документа
	@documentSeries varchar(9) = null,		-- серия документа
	@licenseNumber nvarchar(18) = null,		-- номер сертификата
	@typographicNumber nvarchar(12) = null,	-- номер типограф
	@login nvarchar(255)				    -- логин проверяющего
AS
BEGIN

IF LTRIM(RTRIM(@surname)) = '''' SET @surname = NULL
IF LTRIM(RTRIM(@name)) = '''' SET @name = NULL
IF LTRIM(RTRIM(@secondName)) = '''' SET @secondName = NULL
IF LTRIM(RTRIM(@documentNumber)) = '''' SET @documentNumber = NULL
IF LTRIM(RTRIM(@documentSeries)) = '''' SET @documentSeries = NULL
IF LTRIM(RTRIM(@licenseNumber)) = '''' SET @licenseNumber = NULL
IF LTRIM(RTRIM(@typographicNumber)) = '''' SET @typographicNumber = NULL

IF 
	@surname IS NULL AND 
	@name IS NULL AND
	@secondName IS NULL AND
	@documentNumber IS NULL AND
	@documentSeries IS NULL AND
	@licenseNumber IS NULL AND
	@typographicNumber IS NULL
BEGIN
	RAISERROR (N''Необходимо указать хотя один параметр для поиска'', 16, 1);
	RETURN
END

SET @surname = REPLACE(REPLACE(@surname, '''''''', ''''), ''--'', '''')
SET @name = REPLACE(REPLACE(@name, '''''''', ''''), ''--'', '''')
SET @secondName = REPLACE(REPLACE(@secondName, '''''''', ''''), ''--'', '''')
SET @documentNumber = REPLACE(REPLACE(@documentNumber, '''''''', ''''), ''--'', '''')
SET @documentSeries = REPLACE(REPLACE(@documentSeries, '''''''', ''''), ''--'', '''')
SET @licenseNumber = REPLACE(REPLACE(@licenseNumber, '''''''', ''''), ''--'', '''')
SET @typographicNumber = REPLACE(REPLACE(@typographicNumber, '''''''', ''''), ''--'', '''')

SET NOCOUNT ON

-- ЕГЭ
DECLARE @testType INT = 4 	
DECLARE @checkType INT = 5

DECLARE @checkId BIGINT
DECLARE @count INT

DECLARE @surnameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@surname))), ''Ё'', ''Е''), ''Й'', ''И''), ''*'', ''%''), ''?'', ''_''), '''')
DECLARE @nameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@name))), ''Ё'', ''Е''), ''Й'', ''И''), ''*'', ''%''), ''?'', ''_''), '''')
DECLARE @secondNameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@secondName))), ''Ё'', ''Е''), ''Й'', ''И''), ''*'', ''%''), ''?'', ''_''), '''')
DECLARE @documentNumberTrimmed varchar(10) = ISNULL(LTRIM(RTRIM(@documentNumber)), '''')
DECLARE @documentSeriesTrimmed varchar(9) = ISNULL(LTRIM(RTRIM(@documentSeries)), '''')
DECLARE @licenseNumberTrimmed varchar(18) = ISNULL(LTRIM(RTRIM(@licenseNumber)), '''')
DECLARE @typographicNumberTrimmed varchar(12) = ISNULL(LTRIM(RTRIM(@typographicNumber)), '''')

DECLARE @commandText nvarchar(4000)

--------------------------------------------------------------------------------
DECLARE @ownerId INT
DECLARE @ownerIsBlocked BIT
DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

-- Пользователь, отправивший запрос
SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
FROM dbo.Account a
WHERE a.LoginTrimmed = @loginTrimmed	

IF (@ownerIsBlocked IS NULL)
BEGIN
	RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
	RETURN
END
--------------------------------------------------------------------------------

CREATE TABLE #Results (
	GroupId INT NULL,
	ParticipantId UNIQUEIDENTIFIER,
	UseYear INT,
	RegionId INT,
	RegionName VARCHAR(50),
	Surname varchar(80),
	Name varchar(80),
	SecondName varchar(80) NULL,
	DocumentSeries varchar(9) NULL,
	DocumentNumber varchar(10),
	SubjectCode INT,
	SubjectName varchar(100),
	Mark INT,
	ProcessCondition INT,
	GlobalStatusID INT,
	StatusName varchar(255),
	HasAppeal BIT,
	CertificateId UNIQUEIDENTIFIER NULL,
	LicenseNumber NVARCHAR(18) NULL,
	TypographicNumber NVARCHAR(12) NULL,
	SurnameTrimmed varchar(80),
	NameTrimmed varchar(80),
	SecondNameTrimmed varchar(80)			
) 
CREATE NONCLUSTERED INDEX idx_1 ON #Results (DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed)	
	
SET @commandText = ''
	INSERT INTO #Results
	SELECT 
		NULL,
		p.ParticipantID,
		p.UseYear,
		p.REGION,
		r.RegionName,
		p.Surname,
		p.Name,
		p.SecondName,		
		p.DocumentSeries,
		p.DocumentNumber,
		s.SubjectCode,
		s.SubjectName,
		cm.Mark,	
		cm.ProcessCondition,	
		rsg.GlobalStatusID,
		rsg.StatusName,
		cm.HasAppeal,
		cm.CertificateFK,
		cer.LicenseNumber,
		cer.TypographicNumber,
		p.SurnameTrimmed,
		p.NameTrimmed,
		p.SecondNameTrimmed						
	FROM 
		rbd.Participants p WITH (NOLOCK)
		JOIN prn.CertificatesMarks cm WITH (NOLOCK) ON 
			cm.ParticipantFK = p.ParticipantId AND			
			cm.UseYear = p.UseYear AND
			cm.REGION = p.REGION			
		JOIN prn.Certificates cer WITH (NOLOCK) ON 
			cer.CertificateID = cm.CertificateFK AND			
			cer.UseYear = cm.UseYear AND						
			cer.REGION = cm.REGION 
		JOIN dbo.ResultStatuses rs WITH (NOLOCK) ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg WITH (NOLOCK) ON
			rs.GlobalStatusID = rsg.GlobalStatusID	 
		JOIN dat.Subjects s WITH (NOLOCK) ON
			s.SubjectCode = cm.SubjectCode						
		JOIN rbdc.Regions r WITH (NOLOCK) ON
			r.REGION = cm.REGION	
	WHERE 1=1'' 	
	
-- @surname	
IF (@surnameTrimmed <> ''%'' AND @surname IS NOT NULL AND (CHARINDEX(''%'', @surnameTrimmed) > 0 OR CHARINDEX(''_'', @surnameTrimmed) > 0))	
	SET @commandText = @commandText + '' AND p.SurnameTrimmed LIKE '''''' + @surnameTrimmed + ''''''''
ELSE IF (@surnameTrimmed <> ''%'' AND @surname IS NOT NULL AND CHARINDEX(''%'', @surnameTrimmed) = 0 AND CHARINDEX(''_'', @surnameTrimmed) = 0)
	SET @commandText = @commandText + '' AND p.SurnameTrimmed = '''''' + @surnameTrimmed + ''''''''

-- @name	
IF (@nameTrimmed <> ''%'' AND @name IS NOT NULL AND (CHARINDEX(''%'', @nameTrimmed) > 0 OR CHARINDEX(''_'', @nameTrimmed) > 0))	
	SET @commandText = @commandText + '' AND p.NameTrimmed LIKE '''''' + @nameTrimmed + ''''''''
ELSE IF (@nameTrimmed <> ''%'' AND @name IS NOT NULL AND CHARINDEX(''%'', @nameTrimmed) = 0 AND CHARINDEX(''_'', @nameTrimmed) = 0)
	SET @commandText = @commandText + '' AND p.NameTrimmed = '''''' + @nameTrimmed + ''''''''

-- @secondName	
IF (@nameTrimmed <> ''%'' AND @secondName IS NOT NULL AND (CHARINDEX(''%'', @secondNameTrimmed) > 0 OR CHARINDEX(''_'', @secondNameTrimmed) > 0))	
	SET @commandText = @commandText + '' AND p.SecondNameTrimmed LIKE '''''' + @secondNameTrimmed + ''''''''
ELSE IF (@nameTrimmed <> ''%'' AND @secondName IS NOT NULL AND CHARINDEX(''%'', @secondNameTrimmed) = 0 AND CHARINDEX(''_'', @secondNameTrimmed) = 0)
	SET @commandText = @commandText + '' AND p.SecondNameTrimmed = '''''' + @secondNameTrimmed + ''''''''
	
-- @documentNumber	
IF (@documentNumber IS NOT NULL)	
	SET @commandText = @commandText + '' AND p.DocumentNumber = '''''' + @documentNumberTrimmed + ''''''''

-- @documentSeries	
IF (@documentSeries IS NOT NULL)	
	SET @commandText = @commandText + '' AND p.DocumentSeries = '''''' + @documentSeriesTrimmed + ''''''''

-- @licenseNumber	
IF (@licenseNumber IS NOT NULL)	
	SET @commandText = @commandText + '' AND cer.LicenseNumber = '''''' + @licenseNumberTrimmed + ''''''''
	
-- @licenseNumber	
IF (@typographicNumber IS NOT NULL)	
	SET @commandText = @commandText + '' AND cer.TypographicNumber = '''''' + @typographicNumberTrimmed + ''''''''
	
SET @commandText = @commandText + '' AND cm.TestTypeID = 4''

EXEC sp_executesql @commandText
SET @count = @@ROWCOUNT

UPDATE #Results
SET GroupId = A1.GroupId
FROM 
	(SELECT row_number() over (
		ORDER BY DocumentSeries, DocumentNumber) AS GroupId,
		DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
	FROM #Results
	GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
	JOIN #Results r ON 
		r.DocumentSeries = A1.DocumentSeries AND		
		r.DocumentNumber = A1.DocumentNumber AND			
		r.SurnameTrimmed = A1.SurnameTrimmed AND 
		r.NameTrimmed = A1.NameTrimmed AND
		r.SecondNameTrimmed = A1.SecondNameTrimmed

if (@login is null or [dbo].[IsUserOrgLogDisabled](@login) = 0)
begin
BEGIN TRANSACTION			
	-- запрос		
	INSERT INTO dbo.CheckHistory (OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@ownerId, @senderType, @checkType, CASE WHEN @count > 0 THEN ''Найдено'' ELSE ''Не найдено'' END)
	SELECT @checkId = @@IDENTITY
		
	-- результат обработки запроса
	IF (@count > 0)
	BEGIN
		INSERT INTO dbo.CheckResultsHistory ( 
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckDocumentSeries,		
			CheckDocumentNumber,		
			CheckCertificateNumber,		
			CheckTypographicNumber,
			GroupId,
			ParticipantId,
			UseYear,
			RegionId,
			Surname,
			Name,
			SecondName,
			DocumentSeries,
			DocumentNumber)
		SELECT
			@checkId,
			@surname,
			@name,
			@secondName,
			@documentSeries,		
			@documentNumber,		
			@licenseNumber,		
			@typographicNumber,			
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber
		FROM #Results r		
		GROUP BY 
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber				
	END ELSE 
	BEGIN
		INSERT INTO dbo.CheckResultsHistory (
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckDocumentSeries,		
			CheckDocumentNumber,		
			CheckCertificateNumber,		
			CheckTypographicNumber)
		VALUES (
			@checkId,
			@surname,
			@name,
			@secondName,
			@documentSeries,		
			@documentNumber,		
			@licenseNumber,		
			@typographicNumber)
	END
COMMIT TRANSACTION
end
SELECT * FROM #Results	
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_TypographicNumberFio]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_TypographicNumberFio]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- 4.1.2.	Запрос по типографскому номеру и ФИО
CREATE PROC [dbo].[SingleCheck_TypographicNumberFio]
	@senderType INT,					    -- тип отправителя
	@typographicNumber nvarchar(12),		-- номер сертификата
	@surname varchar(80) = null,		    -- фамилия сертифицируемого
	@name varchar(80) = null,			    -- имя сертифицируемого
	@secondName varchar(80) = null,		    -- отчетсво сертифицируемого
	@login nvarchar(255)				    -- логин проверяющего
AS
BEGIN
--1. Ищем в таблице Certificates по полю TypographicNumber свидетельство с указанным типографским номером. Если такого свидетельства нет, то выдаем, что ничего не найдено.
--2. Если свидетельство с таким номером есть, то ищем в таблице Participants участника с этим свидетельством по ParticipantFK. Проверяем, что введенные в запроса Фамилия, Имя, Отчество (последние два – если они введены) совпадают с полями Surname, Name, SecondName. Если хотя бы одно поле не совпадает – выдаем, что ничего не найдено.
--3. Если всё совпадает, то берем из таблицы CertificatesMarks все оценки, привязанные одновременно к данному свидетельству по полю CertificateFK и к данному участнику по полю ParticipantFK. То есть оценки, которые привязаны к участнику, но не привязаны к свидетельству, брать не надо. Названия предметов берем из таблицы Subjects по полю SubjectCode.
--5. Важно! Сейчас это реализовано, необходимо сохранить:
--а) При проверке не должен учитываться регистр букв. 
--б) При проверке должны считаться одинаковыми буквы е/ё и и/й. То есть, если пользователь ввёл фамилию «Ёлкин», то ему должны выдаться и участники с фамилией «Ёлкин» и участники с фамилией «Елкин». Аналогично и для и/й

IF LTRIM(RTRIM(@surname)) = '''' SET @surname = NULL
IF LTRIM(RTRIM(@name)) = '''' SET @name = NULL
IF LTRIM(RTRIM(@secondName)) = '''' SET @secondName = NULL
IF LTRIM(RTRIM(@typographicNumber)) = '''' SET @typographicNumber = NULL

IF @surname IS NULL OR @typographicNumber IS NULL
BEGIN
	RAISERROR (N''Необходимо указать фамилию и типограф. номер'', 16, 1);
	RETURN
END

SET NOCOUNT ON

-- ЕГЭ
DECLARE @testType INT = 4 	
DECLARE @checkType INT = 2

DECLARE @checkId BIGINT
DECLARE @count INT

DECLARE @surnameTrimmed varchar(80) = REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@surname))), ''Ё'', ''Е''), ''Й'', ''И'')
DECLARE @nameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@name))), ''Ё'', ''Е''), ''Й'', ''И''), '''')
DECLARE @secondNameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@secondName))), ''Ё'', ''Е''), ''Й'', ''И''), '''')
DECLARE @typographicNumberTrimmed nvarchar(12) = LTRIM(RTRIM(UPPER(@typographicNumber)))

--------------------------------------------------------------------------------
DECLARE @ownerId INT
DECLARE @ownerIsBlocked BIT
DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

-- Пользователь, отправивший запрос
SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
FROM dbo.Account a
WHERE a.LoginTrimmed = @loginTrimmed	

IF (@ownerIsBlocked IS NULL)
BEGIN
	RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
	RETURN
END
--------------------------------------------------------------------------------

DECLARE @Results TABLE (
	GroupId INT NULL,
	ParticipantId UNIQUEIDENTIFIER,
	UseYear INT,
	RegionId INT,
	RegionName VARCHAR(50),
	Surname varchar(80),
	Name varchar(80),
	SecondName varchar(80) NULL,
	DocumentSeries varchar(9) NULL,
	DocumentNumber varchar(10),
	SubjectCode INT,
	SubjectName varchar(100),
	Mark INT,
	ProcessCondition INT,
	GlobalStatusID INT,
	StatusName varchar(255),
	HasAppeal BIT,
	CertificateId UNIQUEIDENTIFIER,
	LicenseNumber NVARCHAR(18),
	TypographicNumber NVARCHAR(12),
	SurnameTrimmed varchar(80),
	NameTrimmed varchar(80),
	SecondNameTrimmed varchar(80),
	Cancelled BIT NULL,	
	DenyNewCertificateNumber NVARCHAR(18) NULL,
	DenyComment NVARCHAR(255),
	UniqueIHEaFCheck INT
) 

;WITH f AS (SELECT 	
		cer.ParticipantFK,
		cer.UseYear,
		cer.REGION,
		r.RegionName,
		s.SubjectCode,
		s.SubjectName,
		cm.Mark,	
		cm.ProcessCondition,	
		rsg.GlobalStatusID,
		rsg.StatusName,
		cm.HasAppeal,
		cer.CertificateID,
		cer.LicenseNumber,
		cer.TypographicNumber,
		cer.Cancelled
	FROM 
		prn.Certificates cer WITH (NOLOCK)
		JOIN prn.CertificatesMarks cm WITH (NOLOCK) ON 
			cm.CertificateFK = cer.CertificateID AND
			cm.REGION = cer.REGION AND
			cm.UseYear = cer.UseYear											
		JOIN dbo.ResultStatuses rs WITH (NOLOCK) ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg WITH (NOLOCK) ON
			rs.GlobalStatusID = rsg.GlobalStatusID	 
		JOIN dat.Subjects s WITH (NOLOCK) ON
			s.SubjectCode = cm.SubjectCode						
		JOIN rbdc.Regions r WITH (NOLOCK) ON
			r.REGION = cm.REGION	
	WHERE 			
		cer.TypographicNumber = @typographicNumberTrimmed AND
		cm.TestTypeID = @testType
	) 
INSERT INTO @Results			
SELECT 
	NULL,
	p.ParticipantID,
	p.UseYear,
	p.REGION,			
	f.RegionName,
	p.Surname,
	p.Name,
	p.SecondName,		
	p.DocumentSeries,
	p.DocumentNumber,
	f.SubjectCode,
	f.SubjectName,
	f.Mark,	
	f.ProcessCondition,	
	f.GlobalStatusID,
	f.StatusName,
	f.HasAppeal,
	f.CertificateID,
	f.LicenseNumber,
	f.TypographicNumber,
	p.SurnameTrimmed,
	p.NameTrimmed,
	p.SecondNameTrimmed,
	f.Cancelled,
	'''',
	'''',
	0		
FROM f	
	JOIN rbd.Participants p WITH (NOLOCK) ON 
		f.ParticipantFK = p.ParticipantID AND								
		f.UseYear = p.UseYear AND
		f.REGION = p.REGION 
WHERE	
	p.SurnameTrimmed = @surnameTrimmed AND
	p.NameTrimmed = (CASE WHEN @name IS NULL THEN p.NameTrimmed ELSE @nameTrimmed END) AND
	p.SecondNameTrimmed = (CASE WHEN @secondName IS NULL THEN p.SecondNameTrimmed ELSE @secondNameTrimmed END)				

SET @count = @@ROWCOUNT

UPDATE @Results
SET GroupId = A1.GroupId
FROM 
	(SELECT row_number() over (
		ORDER BY DocumentSeries, DocumentNumber) AS GroupId,
		DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
	FROM @Results
	GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
	JOIN @Results r ON 
		r.DocumentSeries = A1.DocumentSeries AND		
		r.DocumentNumber = A1.DocumentNumber AND			
		r.SurnameTrimmed = A1.SurnameTrimmed AND 
		r.NameTrimmed = A1.NameTrimmed AND
		r.SecondNameTrimmed = A1.SecondNameTrimmed
		
if (@login is null or [dbo].[IsUserOrgLogDisabled](@login) = 0)
begin
BEGIN TRANSACTION			
	-- запрос		
	INSERT INTO dbo.CheckHistory (OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@ownerId, @senderType, @checkType, CASE WHEN @count > 0 THEN ''Найдено'' ELSE ''Не найдено'' END)
	SELECT @checkId = @@IDENTITY
	
	-- результат обработки запроса
	IF (@count > 0)
	BEGIN
		INSERT INTO dbo.CheckResultsHistory ( 
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckTypographicNumber,
			GroupId,
			ParticipantId,
			UseYear,
			RegionId,
			Surname,
			Name,
			SecondName,
			DocumentSeries,
			DocumentNumber)
		SELECT
			@checkId,
			@surname,
			@name,
			@secondName,
			@typographicNumber,			
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber
		FROM @Results r		
		GROUP BY 
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber			
	END ELSE 
	BEGIN
		INSERT INTO dbo.CheckResultsHistory (
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckTypographicNumber)
		VALUES (
			@checkId,
			@surname,
			@name,
			@secondName,
			@typographicNumber)
	END			
COMMIT TRANSACTION
end
SELECT * FROM @Results	
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_LicenseNumberFio]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_LicenseNumberFio]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- 4.1.1.	Запрос по регистрационному номеру и ФИО (интерактивная проверка)
CREATE PROC [dbo].[SingleCheck_LicenseNumberFio]
	@senderType INT,					-- тип отправителя
	@licenseNumber nvarchar(18),		-- номер сертификата
	@surname varchar(80) = null,		-- фамилия сертифицируемого
	@name varchar(80) = null,			-- имя сертифицируемого
	@secondName varchar(80) = null,		-- отчетсво сертифицируемого
	@login nvarchar(255)				-- логин проверяющего
AS
BEGIN
--1. Ищем в таблице Certificates по полю LicenseNumber свидетельство с указанным номером. Если такого свидетельства нет, то выдаем текст «Свидетельство с заданными параметрами не найдено».
--2. Если свидетельство с таким номером есть, то ищем в таблице Participants участника с этим свидетельством по ParticipantFK. 
--	 Проверяем, что введенные в запросе Фамилия, Имя, Отчество (последние два – если они введены) совпадают с полями Surname, Name, SecondName. 
--   Если хотя бы одно поле не совпадает – выдаем текст «Свидетельство с заданными параметрами не найдено».
--3. Если всё совпадает, то берем из таблицы CertificatesMarks все оценки, привязанные одновременно к данному свидетельству по полю CertificateFK и к данному участнику по полю ParticipantFK. 
--   То есть оценки, которые привязаны к участнику, но не привязаны к свидетельству, брать не надо. Названия предметов берем из таблицы Subjects по полю SubjectCode.
--4. Если свидетельство аннулировано (Cancelled=1), то берем из таблицы CancelledCerfificates по полю SertificateFK причину аннулирования.
--5. Важно! Сейчас это реализовано, необходимо сохранить:
--а) При проверке не должен учитываться регистр букв. 
--б) При проверке должны считаться одинаковыми буквы е/ё и и/й. То есть, если пользователь ввёл фамилию «Ёлкин», то ему должны выдаться и участники с фамилией «Ёлкин» и участники с фамилией «Елкин». Аналогично и для и/й

IF LTRIM(RTRIM(@surname)) = '''' SET @surname = NULL
IF LTRIM(RTRIM(@name)) = '''' SET @name = NULL
IF LTRIM(RTRIM(@secondName)) = '''' SET @secondName = NULL
IF LTRIM(RTRIM(@licenseNumber)) = '''' SET @licenseNumber = NULL

IF @surname IS NULL OR @licenseNumber IS NULL
BEGIN
	RAISERROR (N''Необходимо указать фамилию и номер св-ва'', 16, 1);
	RETURN
END

SET NOCOUNT ON

-- ЕГЭ
DECLARE @testType INT = 4 	
DECLARE @checkType INT = 1

DECLARE @checkId BIGINT
DECLARE @checkParamsId BIGINT
DECLARE @count INT

DECLARE @licenseNumberTrimmed nvarchar(18) = LTRIM(RTRIM(UPPER(@licenseNumber)))
DECLARE @surnameTrimmed varchar(80) = REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@surname))), ''Ё'', ''Е''), ''Й'', ''И'')
DECLARE @nameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@name))), ''Ё'', ''Е''), ''Й'', ''И''), '''')
DECLARE @secondNameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@secondName))), ''Ё'', ''Е''), ''Й'', ''И''), '''')

--------------------------------------------------------------------------------
DECLARE @ownerId INT
DECLARE @ownerIsBlocked BIT
DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

-- Пользователь, отправивший запрос
SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
FROM dbo.Account a
WHERE a.LoginTrimmed = @loginTrimmed	

IF (@ownerIsBlocked IS NULL)
BEGIN
	RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
	RETURN
END
--------------------------------------------------------------------------------

DECLARE @Results TABLE (
	GroupId INT NULL,
	ParticipantId UNIQUEIDENTIFIER,
	UseYear INT,
	RegionId INT,
	RegionName VARCHAR(50),
	Surname varchar(80),
	Name varchar(80),
	SecondName varchar(80) NULL,
	DocumentSeries varchar(9) NULL,
	DocumentNumber varchar(10),
	SubjectCode INT,
	SubjectName varchar(100),
	Mark INT,
	ProcessCondition INT,
	GlobalStatusID INT,
	StatusName varchar(255),
	HasAppeal BIT,
	CertificateId UNIQUEIDENTIFIER,
	LicenseNumber NVARCHAR(18),
	TypographicNumber NVARCHAR(12),				
	SurnameTrimmed varchar(80),
	NameTrimmed varchar(80),
	SecondNameTrimmed varchar(80),
	Cancelled BIT NULL,	
	DenyNewCertificateNumber NVARCHAR(18) NULL,
	DenyComment NVARCHAR(255),
	UniqueIHEaFCheck INT	
) 

;WITH f AS (SELECT 	
		cer.ParticipantFK,
		cer.UseYear,
		cer.REGION,
		r.RegionName,
		s.SubjectCode,
		s.SubjectName,
		cm.Mark,	
		cm.ProcessCondition,	
		rsg.GlobalStatusID,
		rsg.StatusName,
		cm.HasAppeal,
		cer.CertificateID,
		cer.LicenseNumber,
		cer.TypographicNumber,
		cer.Cancelled				
	FROM 
		prn.Certificates cer WITH (NOLOCK)
		JOIN prn.CertificatesMarks cm WITH (NOLOCK) ON 
			cm.CertificateFK = cer.CertificateID AND
			cm.REGION = cer.REGION AND
			cm.UseYear = cer.UseYear											
		JOIN dbo.ResultStatuses rs WITH (NOLOCK) ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg WITH (NOLOCK) ON
			rs.GlobalStatusID = rsg.GlobalStatusID	 
		JOIN dat.Subjects s WITH (NOLOCK) ON
			s.SubjectCode = cm.SubjectCode						
		JOIN rbdc.Regions r WITH (NOLOCK) ON
			r.REGION = cm.REGION	
	WHERE 			
		cer.LicenseNumber = @licenseNumberTrimmed AND
		cm.TestTypeID = @testType
	) 
INSERT INTO @Results			
SELECT 
	NULL,
	p.ParticipantID,
	p.UseYear,
	p.REGION,			
	f.RegionName,
	p.Surname,
	p.Name,
	p.SecondName,		
	p.DocumentSeries,
	p.DocumentNumber,
	f.SubjectCode,
	f.SubjectName,
	f.Mark,	
	f.ProcessCondition,	
	f.GlobalStatusID,
	f.StatusName,
	f.HasAppeal,
	f.CertificateID,
	f.LicenseNumber,
	f.TypographicNumber,
	p.SurnameTrimmed,
	p.NameTrimmed,
	p.SecondNameTrimmed,
	f.Cancelled,
	'''',
	'''',
	0	
FROM f	
	JOIN rbd.Participants p WITH (NOLOCK) ON 
		f.ParticipantFK = p.ParticipantID AND								
		f.UseYear = p.UseYear AND
		f.REGION = p.REGION 
WHERE	
	p.SurnameTrimmed = @surnameTrimmed AND
	p.NameTrimmed = (CASE WHEN @name IS NULL THEN p.NameTrimmed ELSE @nameTrimmed END) AND
	p.SecondNameTrimmed = (CASE WHEN @secondName IS NULL THEN p.SecondNameTrimmed ELSE @secondNameTrimmed END)					

SET @count = @@ROWCOUNT

UPDATE @Results
SET GroupId = A1.GroupId
FROM 
	(SELECT row_number() over (
		ORDER BY DocumentSeries, DocumentNumber) AS GroupId,
		DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
	FROM @Results
	GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
	JOIN @Results r ON 
		r.DocumentSeries = A1.DocumentSeries AND			
		r.DocumentNumber = A1.DocumentNumber AND			
		r.SurnameTrimmed = A1.SurnameTrimmed AND 
		r.NameTrimmed = A1.NameTrimmed AND
		r.SecondNameTrimmed = A1.SecondNameTrimmed

if (@login is null or [dbo].[IsUserOrgLogDisabled](@login) = 0)
begin
BEGIN TRANSACTION			
	-- запрос		
	INSERT INTO dbo.CheckHistory (OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@ownerId, @senderType, @checkType, CASE WHEN @count > 0 THEN ''Найдено'' ELSE ''Не найдено'' END)
	SELECT @checkId = @@IDENTITY
	
	-- результат обработки запроса
	IF (@count > 0)
	BEGIN
		INSERT INTO dbo.CheckResultsHistory ( 
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckCertificateNumber,
			GroupId,
			ParticipantId,
			UseYear,
			RegionId,
			Surname,
			Name,
			SecondName,
			DocumentSeries,
			DocumentNumber)
		SELECT
			@checkId,
			@surname,
			@name,
			@secondName,
			@licenseNumber,
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber
		FROM @Results r		
		GROUP BY
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber		
	END ELSE 
	BEGIN
		INSERT INTO dbo.CheckResultsHistory (
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckCertificateNumber)
		VALUES (
			@checkId,
			@surname,
			@name,
			@secondName,
			@licenseNumber)
	END			
COMMIT TRANSACTION
end
SELECT * FROM @Results	
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_FioSubjectsMarks]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_FioSubjectsMarks]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- 4.1.4.	Запрос по ФИО и баллам по предметам
CREATE PROC [dbo].[SingleCheck_FioSubjectsMarks]
	@senderType INT,					     -- тип отправителя
	@surname nvarchar(80) = null,		     -- фамилия сертифицируемого
	@name nvarchar(80) = null,			     -- имя сертифицируемого
	@secondName nvarchar(80) = null,		 -- отчетсво сертифицируемого
	@subjectsMarks varchar(1024),			 -- средние оценки по предметам (через запятую, в определенном порядке)
	@login nvarchar(255)				     -- логин проверяющего
AS
BEGIN
--1. Ищем в таблице Participants участника с таким ФИО (по полям Surname, Name, SecondName).
--2. Если нашли одного или нескольких участников, то берем их ParticipantID и ищем по полю ParticipantFK в таблице 
--   CertificatesMarks результаты с заданными баллами по данным предметам.
--Необходимо найти одного или нескольких участников, у которого (которых) имеются все введенные в запросе баллы.
--Если таких участников нет, то выводим сообщение, что результаты не найдены.
--Если есть один участник (даже если у него разные ParticipantID, но одинаковые ФИО и паспортные данные), 
--  то выводим сразу его карточку со всеми результатами (не только с теми, которые запрашивались) для всех его ParticipantID. 
--  В данном случае под одним участником подразумевается уникальная комбинация значений ФИО и паспортных данных, 
--  которой могут соответствовать несколько  ParticipantID.
--Если таких участников несколько (то есть разные не только ParticipantID, но и паспортные данные), то выводим несколько строк 
--  со ссылками на карточки (см. пример ниже).
--Важно! Сейчас это реализовано, необходимо сохранить:
--а) При проверке не должен учитываться регистр букв. 
--б) При проверке должны считаться одинаковыми буквы е/ё и и/й. То есть, если пользователь ввёл фамилию «Ёлкин», то ему должны выдаться и участники с фамилией «Ёлкин» и участники с фамилией «Елкин». Аналогично и для и/й

IF LTRIM(RTRIM(@surname)) = '''' SET @surname = NULL
IF LTRIM(RTRIM(@name)) = '''' SET @name = NULL
IF LTRIM(RTRIM(@secondName)) = '''' SET @secondName = NULL
IF LTRIM(RTRIM(@subjectsMarks)) = '''' SET @subjectsMarks = NULL

IF @surname IS NULL OR @name IS NULL OR @subjectsMarks IS NULL -- OR @secondName IS NULL 
BEGIN
	RAISERROR (N''Необходимо указать ФИО и балл по предмету'', 16, 1);
	RETURN
END

SET NOCOUNT ON

-- ЕГЭ
DECLARE @testType INT = 4 	
DECLARE @checkType INT = 4

DECLARE @checkId BIGINT
DECLARE @count INT

DECLARE @surnameTrimmed varchar(80) = REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@surname))), ''Ё'', ''Е''), ''Й'', ''И'')
DECLARE @nameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@name))), ''Ё'', ''Е''), ''Й'', ''И''), '''')
DECLARE @secondNameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@secondName))), ''Ё'', ''Е''), ''Й'', ''И''), '''')

--------------------------------------------------------------------------------
DECLARE @ownerId INT
DECLARE @ownerIsBlocked BIT
DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

-- Пользователь, отправивший запрос
SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
FROM dbo.Account a
WHERE a.LoginTrimmed = @loginTrimmed	

IF (@ownerIsBlocked IS NULL)
BEGIN
	RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
	RETURN
END
--------------------------------------------------------------------------------

DECLARE @Results TABLE  (
	GroupId INT NULL,
	ParticipantId UNIQUEIDENTIFIER,
	UseYear INT,
	RegionId INT,
	RegionName VARCHAR(50),
	Surname varchar(80),
	Name varchar(80),
	SecondName varchar(80) NULL,
	DocumentSeries varchar(9) NULL,
	DocumentNumber varchar(10),
	SubjectCode INT,
	SubjectName varchar(100),
	Mark INT,
	ProcessCondition INT,
	GlobalStatusID INT,
	StatusName varchar(255),
	HasAppeal BIT,
	CertificateId UNIQUEIDENTIFIER NULL,
	LicenseNumber NVARCHAR(18) NULL,
	TypographicNumber NVARCHAR(12) NULL,
	SurnameTrimmed varchar(80),
	NameTrimmed varchar(80),
	SecondNameTrimmed varchar(80)		
) 

DECLARE @SubjectMarks TABLE (
	SubjectCode INT,
	MARK INT
)

INSERT INTO @SubjectMarks
SELECT sm.SubjectId, sm.Mark
FROM dbo.GetSubjectMarks(@subjectsMarks) sm

;WITH f AS (SELECT 
		NULL AS GroupId,
		p.ParticipantID,
		p.UseYear,
		p.REGION,
		r.RegionName,
		p.Surname,
		p.Name,
		p.SecondName,		
		p.DocumentSeries,
		p.DocumentNumber,
		s.SubjectCode,
		s.SubjectName,
		cm.Mark,	
		cm.ProcessCondition,	
		rsg.GlobalStatusID,
		rsg.StatusName,
		cm.HasAppeal,
		cer.CertificateID,
		cer.LicenseNumber,
		cer.TypographicNumber,
		p.SurnameTrimmed,
		p.NameTrimmed,
		p.SecondNameTrimmed
	FROM 
		rbd.Participants p WITH (NOLOCK)
		JOIN prn.CertificatesMarks cm WITH (NOLOCK) ON 
			cm.ParticipantFK = p.ParticipantId AND			
			cm.UseYear = p.UseYear AND
			cm.REGION = p.REGION
		JOIN prn.Certificates cer WITH (NOLOCK) ON
			cer.CertificateID = cm.CertificateFK AND			
			cer.REGION = cm.REGION AND			
			cer.UseYear = cm.UseYear 	
		JOIN dbo.ResultStatuses rs WITH (NOLOCK) ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg WITH (NOLOCK) ON
			rs.GlobalStatusID = rsg.GlobalStatusID	 
		JOIN dat.Subjects s WITH (NOLOCK) ON
			s.SubjectCode = cm.SubjectCode						
		JOIN rbdc.Regions r WITH (NOLOCK) ON
			r.REGION = cm.REGION	
	WHERE 			
		p.SurnameTrimmed = @surnameTrimmed AND
		p.NameTrimmed = @nameTrimmed AND
		p.SecondNameTrimmed = (CASE WHEN @secondName IS NULL THEN p.SecondNameTrimmed ELSE @secondNameTrimmed END) AND
		cm.TestTypeID = @testType) 
INSERT INTO @Results			
SELECT f.*
FROM f		
	LEFT JOIN 
		(SELECT DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
		 FROM f r
			JOIN @SubjectMarks sm ON
				r.SubjectCode = sm.SubjectCode AND
				r.Mark = sm.Mark
		 GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
		 HAVING COUNT(*) > 0) AS A1
	ON 
		f.DocumentSeries = A1.DocumentSeries AND
		f.DocumentNumber = A1.DocumentNumber AND			
		f.SurnameTrimmed = A1.SurnameTrimmed AND
		f.NameTrimmed = A1.NameTrimmed AND
		f.SecondNameTrimmed = A1.SecondNameTrimmed
WHERE A1.SurnameTrimmed IS NOT NULL	

SET @count = @@ROWCOUNT

UPDATE @Results
SET GroupId = A1.GroupId
FROM 
	(SELECT row_number() over (
		ORDER BY DocumentSeries, DocumentNumber) AS GroupId,
		DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
	FROM @Results
	GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
	JOIN @Results r ON 
		r.DocumentSeries = A1.DocumentSeries AND
		r.DocumentNumber = A1.DocumentNumber AND			
		r.SurnameTrimmed = A1.SurnameTrimmed AND 
		r.NameTrimmed = A1.NameTrimmed AND
		r.SecondNameTrimmed = A1.SecondNameTrimmed

if (@login is null or [dbo].[IsUserOrgLogDisabled](@login) = 0)
begin
BEGIN TRANSACTION			
	-- запрос		
	INSERT INTO dbo.CheckHistory (OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@ownerId, @senderType, @checkType, CASE WHEN @count > 0 THEN ''Найдено'' ELSE ''Не найдено'' END)
	SELECT @checkId = @@IDENTITY
	
	-- результат обработки запроса
	IF (@count > 0)
	BEGIN
		INSERT INTO dbo.CheckResultsHistory ( 
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckSubjectsMarks,
			GroupId,				
			ParticipantId,
			UseYear,
			RegionId,
			Surname,
			Name,
			SecondName,
			DocumentSeries,
			DocumentNumber)
		SELECT
			@checkId,
			@surname,
			@name,
			@secondName,
			@subjectsMarks,
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber
		FROM @Results r		
		GROUP BY 
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber			
	END ELSE 
	BEGIN
		INSERT INTO dbo.CheckResultsHistory (
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckSubjectsMarks)
		VALUES (
			@checkId,
			@surname,
			@name,
			@secondName,
			@subjectsMarks)
	END
COMMIT TRANSACTION
end
SELECT * FROM @Results
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SingleCheck_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SingleCheck_FioDocumentNumberSeries]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[SingleCheck_FioDocumentNumberSeries]
	@senderType INT,					    -- тип отправителя
	@surname varchar(80),				    -- фамилия сертифицируемого
	@name varchar(80) = null,			    -- имя сертифицируемого
	@secondName varchar(80) = null,		    -- отчетсво сертифицируемого
	@documentNumber varchar(10),			-- номер документа
	@documentSeries varchar(9) = null,		-- серия документа
	@login nvarchar(255)				    -- логин проверяющего
AS
BEGIN
--Возможность использовать специальные подстановочные символы «*» (любая подстрока) и «?» (любой символ). Эту возможность нужно сохранить.
--Обязательные поля – Фамилия и Номер документа.
--Имеющийся алгоритм поиска должен быть изменен следующим образом:
--1. Ищем в таблице Participants по полям Surname, Name, SecondName, DocumentSeries, DocumentNumber участника с указанными данными. 
--   Если такого участника нет нет, то выдаем, что ничего не найдено.
--2. Если такой участник есть, то берем его ParticipantID и ищем по полю ParticipantFK все его результаты в таблице CertificatesMarks. 
--   Внимание! Может быть несколько участников с такими данными (с одинаковыми или с разными ФИО и паспортными данными). 
--   Тогда ищем результаты по всем им. Если результатов по участнику нет (теоретически такое возможно), 
--   то выводим в результатах запроса только самого участника, без оценок. 
--3. Для всех найденных результатов берем CertificateFK и ищем по полю CertificateID в таблице Certificates. 
--   Если CertificateFK для результата ЕГЭ равен «FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF», то это значит, что результат не привязан к свидетельству.
--В итоговые результаты запроса выводим все результаты, независимо от того, есть по ним свидетельства или нет.
--4. Важно! Сейчас это реализовано, необходимо сохранить:
--а) При проверке не должен учитываться регистр букв. 
--б) При проверке должны считаться одинаковыми буквы е/ё и и/й. То есть, если пользователь ввёл фамилию «Ёлкин», то ему должны выдаться и участники с фамилией «Ёлкин» и участники с фамилией «Елкин». Аналогично и для и/й

IF LTRIM(RTRIM(@surname)) = '''' SET @surname = NULL
IF LTRIM(RTRIM(@name)) = '''' SET @name = NULL
IF LTRIM(RTRIM(@secondName)) = '''' SET @secondName = NULL
IF LTRIM(RTRIM(@documentNumber)) = '''' SET @documentNumber = NULL
IF LTRIM(RTRIM(@documentSeries)) = '''' SET @documentSeries = NULL

IF @surname IS NULL OR @documentNumber IS NULL
BEGIN
	RAISERROR (N''Необходимо указать фамилию и номер документа'', 16, 1);
	RETURN
END

SET NOCOUNT ON

-- ЕГЭ
DECLARE @testType INT = 4 	
DECLARE @checkType INT = 3

DECLARE @checkId BIGINT
DECLARE @count INT

DECLARE @surnameTrimmed varchar(80) = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@surname))), ''Ё'', ''Е''), ''Й'', ''И''), ''*'', ''%''), ''?'', ''_''), '''''''', ''''), ''--'', '''')
DECLARE @nameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@name))), ''Ё'', ''Е''), ''Й'', ''И''), ''*'', ''%''), ''?'', ''_''), '''''''', ''''), ''--'', ''''), '''')
DECLARE @secondNameTrimmed varchar(80) = ISNULL(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(LTRIM(RTRIM(UPPER(@secondName))), ''Ё'', ''Е''), ''Й'', ''И''), ''*'', ''%''), '''''''', ''''), ''--'', ''''), ''?'', ''_''), '''')
DECLARE @documentNumberTrimmed varchar(10) = LTRIM(RTRIM(REPLACE(REPLACE(@documentNumber, '''''''', ''''), ''--'', '''')))
DECLARE @documentSeriesTrimmed varchar(9) = ISNULL(LTRIM(RTRIM(REPLACE(REPLACE(@documentSeries, '''''''', ''''), ''--'', ''''))), '''')

DECLARE @commandText nvarchar(4000)

--------------------------------------------------------------------------------
DECLARE @ownerId INT
DECLARE @ownerIsBlocked BIT
DECLARE @loginTrimmed varchar(80) = ISNULL(LTRIM(RTRIM(UPPER(@login))), '''')

-- Пользователь, отправивший запрос
SELECT @ownerId = a.[Id], @ownerIsBlocked = CASE WHEN [Status] = ''activated'' THEN 0 ELSE 1 END
FROM dbo.Account a
WHERE a.LoginTrimmed = @loginTrimmed	

IF (@ownerIsBlocked IS NULL)
BEGIN
	RAISERROR (N''Пользователь не активирован в системе'', 16, 1);		
	RETURN
END
--------------------------------------------------------------------------------

CREATE TABLE #Results (
	GroupId INT NULL,
	ParticipantId UNIQUEIDENTIFIER,
	UseYear INT,
	RegionId INT,
	RegionName VARCHAR(50),
	Surname varchar(80),
	Name varchar(80),
	SecondName varchar(80) NULL,
	DocumentSeries varchar(9) NULL,
	DocumentNumber varchar(10),
	SubjectCode INT,
	SubjectName varchar(100),
	Mark INT,
	ProcessCondition INT,
	GlobalStatusID INT,
	StatusName varchar(255),
	HasAppeal BIT,
	CertificateId UNIQUEIDENTIFIER NULL,
	LicenseNumber NVARCHAR(18) NULL,
	TypographicNumber NVARCHAR(12) NULL,
	SurnameTrimmed varchar(80),
	NameTrimmed varchar(80),
	SecondNameTrimmed varchar(80),
	Cancelled BIT NULL,	
	DenyNewCertificateNumber NVARCHAR(18) NULL,
	DenyComment NVARCHAR(255),
	UniqueIHEaFCheck INT
) 
CREATE NONCLUSTERED INDEX idx_1 ON #Results (DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed)

SET @commandText = ''
	INSERT INTO #Results
	SELECT 
		NULL AS GroupId,
		p.ParticipantID,
		p.UseYear,
		p.REGION,
		r.RegionName,
		p.Surname,
		p.Name,
		p.SecondName,		
		p.DocumentSeries,
		p.DocumentNumber,
		s.SubjectCode,
		s.SubjectName,
		cm.Mark,	
		cm.ProcessCondition,	
		rsg.GlobalStatusID,
		rsg.StatusName,
		cm.HasAppeal,
		cm.CertificateFK,
		cer.LicenseNumber,
		cer.TypographicNumber,
		p.SurnameTrimmed,
		p.NameTrimmed,
		p.SecondNameTrimmed,
		cer.Cancelled,
		'''''''',
		'''''''',
		0
	FROM 
		rbd.Participants p WITH (NOLOCK)
		JOIN prn.CertificatesMarks cm WITH (NOLOCK) ON 
			cm.ParticipantFK = p.ParticipantId AND			
			cm.UseYear = p.UseYear AND
			cm.REGION = p.REGION
		JOIN prn.Certificates cer WITH (NOLOCK) ON 
			cer.CertificateID = cm.CertificateFK AND			
			cer.UseYear = cm.UseYear AND						
			cer.REGION = cm.REGION 
		JOIN dbo.ResultStatuses rs WITH (NOLOCK) ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg WITH (NOLOCK) ON
			rs.GlobalStatusID = rsg.GlobalStatusID	 
		JOIN dat.Subjects s WITH (NOLOCK) ON
			s.SubjectCode = cm.SubjectCode						
		JOIN rbdc.Regions r WITH (NOLOCK) ON
			r.REGION = cm.REGION	
	WHERE 1=1''			

-- @surname	
IF (@surnameTrimmed <> ''%'' AND @surname IS NOT NULL AND (CHARINDEX(''%'', @surnameTrimmed) > 0 OR CHARINDEX(''_'', @surnameTrimmed) > 0))	
	SET @commandText = @commandText + '' AND p.SurnameTrimmed LIKE '''''' + @surnameTrimmed + ''''''''
ELSE IF (@surnameTrimmed <> ''%'' AND @surname IS NOT NULL AND CHARINDEX(''%'', @surnameTrimmed) = 0 AND CHARINDEX(''_'', @surnameTrimmed) = 0)
	SET @commandText = @commandText + '' AND p.SurnameTrimmed = '''''' + @surnameTrimmed + ''''''''

-- @name	
IF (@nameTrimmed <> ''%'' AND @name IS NOT NULL AND (CHARINDEX(''%'', @nameTrimmed) > 0 OR CHARINDEX(''_'', @nameTrimmed) > 0))	
	SET @commandText = @commandText + '' AND p.NameTrimmed LIKE '''''' + @nameTrimmed + ''''''''
ELSE IF (@nameTrimmed <> ''%'' AND @name IS NOT NULL AND CHARINDEX(''%'', @nameTrimmed) = 0 AND CHARINDEX(''_'', @nameTrimmed) = 0)
	SET @commandText = @commandText + '' AND p.NameTrimmed = '''''' + @nameTrimmed + ''''''''

-- @secondName	
IF (@nameTrimmed <> ''%'' AND @secondName IS NOT NULL AND (CHARINDEX(''%'', @secondNameTrimmed) > 0 OR CHARINDEX(''_'', @secondNameTrimmed) > 0))	
	SET @commandText = @commandText + '' AND p.SecondNameTrimmed LIKE '''''' + @secondNameTrimmed + ''''''''
ELSE IF (@nameTrimmed <> ''%'' AND @secondName IS NOT NULL AND CHARINDEX(''%'', @secondNameTrimmed) = 0 AND CHARINDEX(''_'', @secondNameTrimmed) = 0)
	SET @commandText = @commandText + '' AND p.SecondNameTrimmed = '''''' + @secondNameTrimmed + ''''''''
	
-- @documentNumber	
IF (@documentNumber IS NOT NULL)	
	SET @commandText = @commandText + '' AND p.DocumentNumber = '''''' + @documentNumberTrimmed + ''''''''

-- @documentSeries	
IF (@documentSeries IS NOT NULL)	
	SET @commandText = @commandText + '' AND p.DocumentSeries = '''''' + @documentSeriesTrimmed + ''''''''
	
SET @commandText = @commandText + '' AND cm.TestTypeID = 4''

EXEC sp_executesql @commandText
SET @count = @@ROWCOUNT

UPDATE #Results
SET GroupId = A1.GroupId
FROM 
	(SELECT row_number() over (
		ORDER BY DocumentSeries, DocumentNumber) AS GroupId,
		DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed
	FROM #Results
	GROUP BY DocumentSeries, DocumentNumber, SurnameTrimmed, NameTrimmed, SecondNameTrimmed) AS A1
	JOIN #Results r ON 
		r.DocumentSeries = A1.DocumentSeries AND		
		r.DocumentNumber = A1.DocumentNumber AND
		r.SurnameTrimmed = A1.SurnameTrimmed AND 
		r.NameTrimmed = A1.NameTrimmed AND
		r.SecondNameTrimmed = A1.SecondNameTrimmed

if (@login is null or [dbo].[IsUserOrgLogDisabled](@login) = 0)
begin
BEGIN TRANSACTION			
	-- запрос		
	INSERT INTO dbo.CheckHistory (OwnerId, SenderTypeId, CheckTypeId, [Status])
	VALUES (@ownerId, @senderType, @checkType, CASE WHEN @count > 0 THEN ''Найдено'' ELSE ''Не найдено'' END)
	SELECT @checkId = @@IDENTITY
	
	-- результат обработки запроса
	IF (@count > 0)
	BEGIN
		INSERT INTO dbo.CheckResultsHistory ( 
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckDocumentSeries,
			CheckDocumentNumber,
			GroupId,
			ParticipantId,
			UseYear,
			RegionId,
			Surname,
			Name,
			SecondName,
			DocumentSeries,
			DocumentNumber)
		SELECT
			@checkId,
			@surname,
			@name,
			@secondName,
			@documentSeries,
			@documentNumber,
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber
		FROM #Results r		
		GROUP BY 
			r.GroupId,
			r.ParticipantId,
			r.UseYear,
			r.RegionId,
			r.Surname,
			r.Name,
			r.SecondName,
			r.DocumentSeries,
			r.DocumentNumber	
	END ELSE 
	BEGIN
		INSERT INTO dbo.CheckResultsHistory (
			CheckId,
			CheckSurname,
			CheckName,
			CheckSecondName,
			CheckDocumentSeries,
			CheckDocumentNumber			
		)
		VALUES (
			@checkId,
			@surname,
			@name,
			@secondName,
			@documentSeries,
			@documentNumber)
	END	
COMMIT TRANSACTION
end
SELECT * FROM #Results
SET NOCOUNT OFF
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_cne_AddCheckBatchResult]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_cne_AddCheckBatchResult]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[usp_cne_AddCheckBatchResult]
    @xml XML = NULL,
    @batchid BIGINT = 0
AS 
    SET nocount ON  
    IF @xml IS NULL 
        BEGIN
            SELECT  NULL IsProcess1,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END

    SELECT  item.ref.value(''@Id'', ''bigint'') AS Id,      
            item.ref.value(''@IsBatchCorrect'', ''bit'') AS IsBatchCorrect,
            item.ref.value(''@BatchId'', ''bigint'') AS BatchId,
            item.ref.value(''@CertificateCheckingId'', ''uniqueidentifier'') AS CertificateCheckingId,
            item.ref.value(''@CertificateNumber'', ''nvarchar(255)'') AS CertificateNumber,
            item.ref.value(''@IsOriginal'', ''bit'') AS IsOriginal,
            item.ref.value(''@IsCorrect'', ''bit'') AS IsCorrect,
            item.ref.value(''@SourceCertificateId'', ''uniqueidentifier'') AS SourceCertificateId,
            item.ref.value(''@IsDeny'', ''bit'') AS IsDeny,
            item.ref.value(''@DenyComment'', ''nvarchar(max)'') AS DenyComment,
            item.ref.value(''@DenyNewCertificateNumber'', ''nvarchar(255)'') AS DenyNewCertificateNumber,
            item.ref.value(''@Year'', ''int'') AS Year,
            item.ref.value(''@TypographicNumber'', ''nvarchar(255)'') AS TypographicNumber,
            item.ref.value(''@RegionId'', ''int'') AS RegionId,
            item.ref.value(''@PassportSeria'', ''nvarchar(255)'') AS PassportSeria,
            item.ref.value(''@PassportNumber'', ''nvarchar(255)'') AS PassportNumber,
            item.ref.value(''@UniqueCheckIdGuid'', ''uniqueidentifier'') AS UniqueCheckId,
      item.ref.value(''@UniqueIHEaFCheck'', ''int'') AS UniqueIHEaFCheck,
      item.ref.value(''@UniqueCheckYear'', ''int'') AS UniqueCheckYear
    INTO    #check
    FROM    ( SELECT    @xml
            ) feeds ( feedXml )
            CROSS APPLY feedXml.nodes(''/root/check'') AS item ( ref )
            SELECT * FROM #check

    IF NOT EXISTS ( SELECT  *
                    FROM    #check ) 
        BEGIN
            SELECT  NULL IsProcess2,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END


--select * from #check
    DECLARE @id BIGINT, @checkid BIGINT, @isCorrect BIT
    SELECT  @batchid = dbo.GetInternalId(@batchid)
    IF EXISTS ( SELECT  *
                FROM    CommonNationalExamCertificateCheck
                WHERE   BatchId = @batchid ) 
        BEGIN
            SELECT  NULL IsProcess4,
                    NULL Total,
                    NULL Found
            WHERE   1 = 0
            RETURN
        END
--select @batchid
    BEGIN TRY
    BEGIN TRAN
    -- обновление статистики для уже проверенных св-в
      update    ExamCertificateUniqueChecks
      set       UniqueIHEaFCheck = #check.UniqueIHEaFCheck
      from      ExamCertificateUniqueChecks ex
          inner join #check on #check.UniqueCheckIdGuid = ex.IdGuid
                     AND #check.[UniqueCheckYear] = ex.[year]
          WHERE #check.UniqueCheckId > 0
      -- добавление статистики для еще не проверенных св-в
      insert    ExamCertificateUniqueChecks
          (
            IdGuid,
            [year],
            UniqueIHEaFCheck
          )

          select  DISTINCT #check.UniqueCheckIdGuid,
                    #check.UniqueCheckYear,
                        #check.UniqueIHEaFCheck
          from    #check
              left join ExamCertificateUniqueChecks ex on ex.IdGuid = #check.UniqueCheckIdGuid
                                    AND #check.[UniqueCheckYear] = ex.[year]
          where   ex.Id is NULL AND #check.UniqueCheckIdGuid is not null

INSERT  CommonNationalExamCertificateCheck
                        (
                          BatchId,
                          CertificateCheckingId,
                          CertificateNumber,
                          IsOriginal,
                          IsCorrect,
                          SourceCertificateIdGuid,
                          IsDeny,
                          DenyComment,
                          DenyNewCertificateNumber,
                          Year,
                          TypographicNumber,
                          RegionId,
                          PassportSeria,
                          PassportNumber,
                          idtemp
                        )                        
                        SELECT  @batchid,
                                CertificateCheckingId,
                                CertificateNumber,
                                IsOriginal,
                                IsCorrect,
                                SourceCertificateId,
                                IsDeny,
                                DenyComment,
                                DenyNewCertificateNumber,
                                Year,
                                TypographicNumber,
                                RegionId,
                                PassportSeria,
                                PassportNumber,
                                a.id
                        FROM    #check a
                    
INSERT  CommonNationalExamCertificateSubjectCheck
                        (
                          BatchId,
                          CheckId,
                          SubjectId,
                          Mark,
                          IsCorrect,
                          SourceCertificateSubjectIdGuid,
                          SourceMark,
                          SourceHasAppeal,
                          Year
                        )
                        SELECT  @batchid,
                                b.id,
                                a.SubjectId,
                                a.Mark,
                                a.IsCorrect,
                                a.SourceCertificateSubjectIdGuid,
                                a.SourceMark,
                                a.SourceHasAppeal,
                                a.[Year]
                        FROM    
                        
                        (
                            SELECT  item.ref.value(''@Id'', ''bigint'') AS Id,
                  item.ref.value(''@CheckId'', ''bigint'') AS CheckId,
                  item.ref.value(''@SubjectId'', ''bigint'') AS SubjectId,
                  item.ref.value(''@Mark'', ''numeric(5,1)'') AS Mark,
                  item.ref.value(''@IsCorrect'', ''bit'') AS IsCorrect,
                  item.ref.value(''@SourceCertificateSubjectIdGuid'', ''uniqueidentifier'') AS SourceCertificateSubjectIdGuid,
                  item.ref.value(''@SourceMark'', ''numeric(5,1)'') AS SourceMark,
                  item.ref.value(''@SourceHasAppeal'', ''bigint'') AS SourceHasAppeal,
                  item.ref.value(''@Year'', ''int'') AS [Year]
                FROM    ( SELECT    @xml)      
                feeds ( feedXml )
                            CROSS APPLY feedXml.nodes(''/root/check/subjects/subject'') AS item ( ref )
                        ) a join CommonNationalExamCertificateCheck b on a.CheckId=b.idtemp and b.BatchId=@batchid
                       
 
        SELECT TOP 1
                @isCorrect = IsBatchCorrect
        FROM    #check
        UPDATE  CommonNationalExamCertificateCheckBatch
        SET     IsProcess = 0,
                Executing = 0,
                IsCorrect = @isCorrect
        WHERE   id = @batchid
        
--select top 10 * from CommonNationalExamCertificateCheckBatch where id=@batchid
--select top 10 * from CommonNationalExamCertificateCheck where BatchId=@batchid  order by id desc
--select top 10 * from CommonNationalExamCertificateSubjectCheck where BatchId=@batchid order by id desc
        SELECT  IsProcess,
                ( SELECT    COUNT(*)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Total,
                ( SELECT    COUNT(SourceCertificateId)
                  FROM      CommonNationalExamCertificateCheck c WITH ( NOLOCK )
                  WHERE     c.batchid = b.id
                ) Found
        FROM    CommonNationalExamCertificateCheckBatch b
        WHERE   IsProcess = 0
                AND id = @batchid
                
        IF @@trancount > 0 
            COMMIT TRAN

    END TRY
    BEGIN CATCH
        IF @@trancount > 0 
            ROLLBACK
        DECLARE @msg NVARCHAR(4000)
        SET @msg = ERROR_MESSAGE()
        RAISERROR ( @msg, 16, 1 )
        RETURN -1
    END CATCH
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CalculateUniqueChecksByBatchId]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateUniqueChecksByBatchId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[CalculateUniqueChecksByBatchId]
	@batchId bigint,
    @checkType varchar(200)
as
begin
	-- =========================================================================
	-- Передается код пакета и по этому коду определяется какие проверки 
	-- были выполнены в рамках этого пакета, какие свидетельства были найдены
	-- По успешно найденным свидетельствам выполняется перерасчет количества
	-- уникальных проверок
	-- =========================================================================

	-- Описание входных параметров
    --
	-- @batchId - код пакета. По хорошему надо сделать его GUID и тогда проверять можно 
    --            без указатия  указания типа проверки. А пока он bigint то код пакетов
    --            по разным типам проверок могут пересекаться
    -- @checkType - тип проверки: ''passport_or_typo'' - по паспорту или типографскому номеру,
    --              ''certificate'' - по номеру сертификата
    
    -- 1. Объявляем ''константы''
    declare @passport_or_typo varchar(100)
    set @passport_or_typo = ''passport_or_typo''
    	
    declare @certificate varchar(100)
    set @certificate = ''certificate''

	-- 2. Объявляем переменные
    declare @organizationId bigint
    set @organizationId = 0
    
    -- Используется в курсоре. В эту переменную пишется код свидетельства
    declare @CIdGuid uniqueidentifier 
    set @CIdGuid = null

	-- 3. По коду пакета определяем организацию, от которой выполнялась пакетная проверка        
    -- 
	-- Для начала надо понять, от имени какой организации выполнялась пакетная проверка, т.к.
    -- количество уникальных проверок - это сколько организаций проверило данное свидетельство.
    -- Существует три вида проверок. Информация о двух из них (по паспорту и типографскому 
    -- номеру) хранится в таблице CommonNationalExamCertificateRequestBatch, информация по 
    -- оставшейся (по номеру свидетельства) хранится в таблице 
    -- CommonNationalExamCertificateCheckBatch.
    --
    -- 3.1. По паспорту или типографскому номеру
    if (@checkType = @passport_or_typo)
    begin
	      set @organizationId = 
	          ISNULL(
	              (select 
	                  top 1 
	                  A.OrganizationId 
	              from 
	                  Account A 
	              where A.Id = 
	                  (select 
	                      top 1 ERB.OwnerAccountId 
	                  from 
	                      CommonNationalExamCertificateRequestBatch ERB 
	                  where 
	                      ERB.Id = @batchId))
	              , 0
	        )
	end
        
    -- 3.2. По номеру свидетельства
    if (@checkType = @certificate)
    begin
	      set @organizationId = 
	          ISNULL(
	              (select 
	                  top 1 
	                  A.OrganizationId 
	              from 
	                  Account A 
	              where A.Id = 
	                  (select 
	                      top 1 ECB.OwnerAccountId 
	                  from 
	                      CommonNationalExamCertificateCheckBatch ECB 
	                  where 
	                      ECB.Id = @batchId))
	              , 0
	        )
	end
    
    -- 3.3. Организацию не нашли (или тип поиска был задан неверно) то выходим с кодом 0
    if (@organizationId = 0)
    	return 0
        
    
    -- 4. Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    
    -- 4.1. Выполняем действия для проверок по паспорту или типографскому номеру
    if (@checkType = @passport_or_typo)
    begin              
    	-- Отбираем все найденные свидетельства
	    declare db_cursor cursor for
	    select
	        distinct S.SourceCertificateIdGuid
	    from 
	        CommonNationalExamCertificateRequest S
	    where
	        S.SourceCertificateIdGuid is not null
            and S.BatchId = @batchId
		
        -- Для каждого найденного свидетельства вызываем хранимую процедуру подсчета проверок
	    open db_cursor   
	    fetch next from db_cursor INTO @CIdGuid   
	    while @@FETCH_STATUS = 0   
	    begin
	        exec dbo.ExecuteChecksCount
	            @OrganizationId = @organizationId,
	            @CertificateIdGuid = @CIdGuid
	        fetch next from db_cursor into @CIdGuid
	    end
        
	    close db_cursor   
	    deallocate db_cursor
    end
    
    -- 4.2. Выполняем действия для проверок по номеру свидетельства
    if (@checkType = @certificate)
    begin
    	-- Отбираем все найденные свидетельства
	    declare db_cursor cursor for
	    select
	        distinct S.SourceCertificateIdGuid
	    from 
	        CommonNationalExamCertificateCheck S
	    where
	        S.SourceCertificateIdGuid is not null
            and S.BatchId = @batchId
		
        -- Для каждого найденного свидетельства вызываем хранимую процедуру подсчета проверок
	    open db_cursor   
	    fetch next from db_cursor INTO @CIdGuid   
	    while @@FETCH_STATUS = 0   
	    begin
	        exec dbo.ExecuteChecksCount
	            @OrganizationId = @organizationId,
	            @CertificateIdGuid = @CIdGuid
	        fetch next from db_cursor into @CIdGuid
	    end
        
	    close db_cursor   
	    deallocate db_cursor
    end

	return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[BanOrgs]    Script Date: 05/07/2015 18:13:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BanOrgs]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[BanOrgs]
as
begin
	
DECLARE @wrongChecksLimit INT, @wrongChecksPercent FLOAT
SET @wrongChecksLimit = 3000
set @wrongChecksPercent = 0.3

DECLARE @singleWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @singleWrongCheck (accountId, count)
SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData FROM dbo.EventLog WHERE 
-- это значит что ничего не найдено
SourceEntityId IS NULL
-- ''|%|%|'' - это значит что поиск по паспорту и оценок нет
AND EventParams LIKE ''|%|%|'' 
AND AccountId IS NOT null
GROUP BY AccountId

DECLARE @batchWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @batchWrongCheck( accountId, count )
SELECT  batch.OwnerAccountId,
        COUNT(DISTINCT batchCheck.PassportNumber
              + ISNULL(batchCheck.PassportSeria, ''''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   batchCheck.SourceCertificateId IS NULL
        AND batch.OwnerAccountId IS NOT NULL
        AND batchCheck.PassportNumber IS NOT null
        AND batch.Type = 2
        AND NOT EXISTS ( SELECT *
                         FROM   dbo.CommonNationalExamCertificateSubjectCheck
                         WHERE  Mark IS NOT NULL
                                AND CheckId = batchCheck.id )
GROUP BY batch.OwnerAccountId


DECLARE @allWrongCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @allWrongCheck
        ( accountId, count )
SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleWrongCheck s LEFT JOIN @batchWrongCheck b ON s.accountId = b.accountId

INSERT INTO @allWrongCheck
SELECT accountId, count FROM @batchWrongCheck WHERE accountId NOT IN (SELECT accountId FROM @allWrongCheck)

-- выбрать все успешние проверки (без баллов)

DECLARE @singleSuccessCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @singleSuccessCheck (accountId, count)
SELECT AccountId, COUNT(DISTINCT EventParams) AS passportData FROM dbo.EventLog WHERE 
-- это значит что ничего не найдено
SourceEntityId IS NOT NULL
-- ''|%|%|'' - это значит что поиск по паспорту и оценок нет
AND EventParams LIKE ''|%|%|'' 
AND AccountId IN (SELECT accountId FROM @allWrongCheck)
GROUP BY AccountId

DECLARE @batchSuccessCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @batchSuccessCheck( accountId, count )
SELECT  batch.OwnerAccountId,
        COUNT(DISTINCT batchCheck.PassportNumber
              + ISNULL(batchCheck.PassportSeria, ''''))
FROM    dbo.CommonNationalExamCertificateCheck batchCheck WITH ( NOLOCK )
        INNER JOIN dbo.CommonNationalExamCertificateCheckBatch batch WITH ( NOLOCK ) ON batchCheck.BatchId = batch.Id
WHERE   batchCheck.SourceCertificateId IS NOT NULL
        AND batch.OwnerAccountId IS NOT NULL 
        AND batchCheck.PassportNumber IS NOT null
        AND batch.Type = 2
        AND NOT EXISTS ( SELECT *
                         FROM   dbo.CommonNationalExamCertificateSubjectCheck
                         WHERE  Mark IS NOT NULL
                                AND CheckId = batchCheck.id )
GROUP BY batch.OwnerAccountId


DECLARE @allSuccessCheck TABLE(accountId BIGINT PRIMARY KEY CLUSTERED, count INT)

INSERT INTO @allSuccessCheck
        ( accountId, count )
SELECT s.accountId, s.count + ISNULL(b.count,0) FROM @singleSuccessCheck s LEFT JOIN @batchSuccessCheck b ON s.accountId = b.accountId

INSERT INTO @allSuccessCheck
SELECT accountId, count FROM @batchSuccessCheck WHERE accountId NOT IN (SELECT accountId FROM @allSuccessCheck)

INSERT INTO dbo.BanData
        ( AccountId ,
          WrongCheckCount ,
          SuccessCheckCount
        )
SELECT w.accountId, w.[count], ISNULL(s.[count],0) FROM @allSuccessCheck s RIGHT JOIN @allWrongCheck w ON s.accountId = w.accountId

-- всех разрешаем
UPDATE dbo.Account SET IsBanned = 0

-- когото запрещаем
UPDATE  Account
SET     IsBanned = 1
WHERE   OrganizationId IN (
        SELECT  organizationId
        FROM    ( SELECT    SUM(s.[count]) s,
                            SUM(w.[count]) w,
                            OrganizationId
                  FROM      @allWrongCheck w
							INNER JOIN dbo.Account acc ON w.accountId = acc.Id
                            left JOIN @allSuccessCheck s ON s.accountId = w.accountId
                  GROUP BY  acc.OrganizationId
                ) checks
        WHERE   w >= @wrongChecksLimit
                AND w > ISNULL(s,0) * @wrongChecksPercent )
		AND organizationId IS NOT null

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[_Check_Applicant]    Script Date: 05/07/2015 18:13:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_Check_Applicant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[_Check_Applicant]
@lastname nvarchar(255), 
@firstname nvarchar(255), 
@patronymicname nvarchar(255), 
@passportseria nvarchar(255), 
@passportnumber nvarchar(255)

AS
BEGIN

    
insert into ApplicantChecks     

SELECT Organization2010.Id organizationid, Organization2010.shortname organizationname,  @firstname firstnname, @lastname lastname, 
 @patronymicname patronymicname, @passportseria passportseria, @passportnumber passportnumber, 
 CommonNationalExamCertificateCheckBatch.CreateDate checkdate, CommonNationalExamCertificate.Number certnumber   from Organization2010
inner join dbo.Account on Account.OrganizationId=Organization2010.Id
inner join dbo.CommonNationalExamCertificateCheckBatch on CommonNationalExamCertificateCheckBatch.OwnerAccountId = Account.Id
inner join dbo.CommonNationalExamCertificateCheck on CommonNationalExamCertificateCheck.BatchId = CommonNationalExamCertificateCheckBatch.Id
inner join dbo.CommonNationalExamCertificate on CommonNationalExamCertificate.Id = CommonNationalExamCertificateCheck.SourceCertificateId
 and  CommonNationalExamCertificate.FirstName = @firstname and CommonNationalExamCertificate.LastName = @lastname and CommonNationalExamCertificate.PatronymicName = @patronymicname
 and CommonNationalExamCertificate.PassportSeria = @passportseria and CommonNationalExamCertificate.PassportNumber = @passportnumber
UNION
SELECT Organization2010.Id organizationid, Organization2010.shortname organizationname,  @firstname firstnname, @lastname lastname, 
 @patronymicname patronymicname, @passportseria passportseria, @passportnumber passportnumber, 
 CommonNationalExamCertificateRequestBatch.CreateDate checkdate, CommonNationalExamCertificate.Number certnumber  from Organization2010
inner join dbo.Account on Account.OrganizationId=Organization2010.Id
inner join dbo.CommonNationalExamCertificateRequestBatch on  CommonNationalExamCertificateRequestBatch.OwnerAccountId = Account.Id
inner join dbo.CommonNationalExamCertificateRequest on dbo.CommonNationalExamCertificateRequest.BatchId = CommonNationalExamCertificateRequestBatch.Id
inner join dbo.CommonNationalExamCertificate on CommonNationalExamCertificate.Id = CommonNationalExamCertificateRequest.SourceCertificateId
 and  CommonNationalExamCertificate.FirstName = @firstname and CommonNationalExamCertificate.LastName = @lastname and CommonNationalExamCertificate.PatronymicName = @patronymicname 
 and CommonNationalExamCertificate.PassportSeria = @passportseria and CommonNationalExamCertificate.PassportNumber = @passportnumber
UNION
SELECT Organization2010.Id organizationid, Organization2010.shortname organizationname,  @firstname firstnname, @lastname lastname, 
 @patronymicname patronymicname, @passportseria passportseria, @passportnumber passportnumber, 
 CNEWebUICheckLog.EventDate checkdate, CommonNationalExamCertificate.Number  certnumber from Organization2010
inner join dbo.Account on Account.OrganizationId=Organization2010.Id
inner join dbo.CNEWebUICheckLog on CNEWebUICheckLog.AccountId = Account.Id 
inner join dbo.CommonNationalExamCertificate on CAST(CommonNationalExamCertificate.Id as nvarchar(255)) = CNEWebUICheckLog.FoundedCNEId
 and  CommonNationalExamCertificate.FirstName = @firstname and CommonNationalExamCertificate.LastName = @lastname and CommonNationalExamCertificate.PatronymicName = @patronymicname
 and CommonNationalExamCertificate.PassportSeria = @passportseria and CommonNationalExamCertificate.PassportNumber = @passportnumber
 
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ClearDataBase]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClearDataBase]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- ================================================
-- Приводит БД в состояние на момент начала эксплуатации системы
-- путем очистки таблиц
-- Очищаются только таблицы, связанные с проверками свидетельств
-- Создана: Юсупов Кирилл, 10.06.2010
-- ================================================
CREATE procedure [dbo].[ClearDataBase]
as
begin
  delete from dbo.EventLog where EventCode like ''CNE_%''
  delete from dbo.CNEWebUICheckLog
  delete from dbo.CheckCommonNationalExamCertificateLog
  delete from dbo.CommonNationalExamCertificateCheckLog

  delete from dbo.CommonNationalExamCertificateDenyLoadingTask
  delete from dbo.CommonNationalExamCertificateDenyLoadingTaskError
  delete from dbo.CommonNationalExamCertificateLoadingTask
  delete from dbo.CommonNationalExamCertificateLoadingTaskError

  delete from dbo.CommonNationalExamCertificateSubject
  delete from dbo.CommonNationalExamCertificate
    delete from dbo.CommonNationalExamCertificateDeny
  
  delete from dbo.ImportingCommonNationalExamCertificateSubject
  delete from dbo.ImportingCommonNationalExamCertificate

  delete from dbo.CommonNationalExamCertificateCheck
  delete from dbo.CommonNationalExamCertificateCheckBatch
  delete from dbo.CommonNationalExamCertificateRequest
  delete from dbo.CommonNationalExamCertificateRequestBatch

  delete from dbo.CommonNationalExamCertificateForm
  delete from dbo.CommonNationalExamCertificateFormNumberRange
  
  delete from dbo.CommonNationalExamCertificateSubjectCheck
  delete from dbo.CommonNationalExamCertificateSubjectForm

  delete from dbo.DeprecatedCommonNationalExamCertificateSubject
  delete from dbo.DeprecatedCommonNationalExamCertificate
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[BatchProcess_TypographicNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchProcess_TypographicNumberFio]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[BatchProcess_TypographicNumberFio] 
	@instance INT
AS
BEGIN	
	DECLARE @checkType INT = 2

	DECLARE @count INT
	DECLARE @current_id BIGINT
	DECLARE @current_login VARCHAR(255)
	DECLARE @name VARCHAR(50) = ''TypographicNumberFio_'' + CAST(@instance AS VARCHAR(10))
	DECLARE @message VARCHAR(2048)
	DECLARE @ids TABLE (Id BIGINT, OwnerAccountId BIGINT);

	WHILE 1=1
	BEGIN
		BEGIN TRANSACTION @name
			UPDATE CommonNationalExamCertificateRequestBatch WITH (ROWLOCK, UPDLOCK)
			SET 
				IsProcess = 0,
				IsCorrect = 0
			OUTPUT inserted.Id, inserted.OwnerAccountId INTO @ids
			WHERE Id IN			
				(SELECT TOP 10 Id
				 FROM CommonNationalExamCertificateRequestBatch cer WITH (NOLOCK)
				 WHERE IsProcess = 1 AND IsTypographicNumber = 1 AND IsCorrect IS NULL)			
		
			SELECT @count = COUNT(*) FROM @ids
			IF (@count = 0)
			BEGIN
				ROLLBACK TRANSACTION @name
				RETURN
			END
			
			WHILE @count > 0
			BEGIN
				BEGIN TRY
					SELECT TOP 1 @current_id = i.Id, @current_login = a.[Login]
					FROM @ids i LEFT JOIN dbo.Account a WITH (NOLOCK) ON a.Id = i.OwnerAccountId
						
					IF (@current_login IS NULL)
					BEGIN
						SET @message = ''Не найден Login: '' + @current_login
						RAISERROR (@message, 16, 1)
					END
					
					EXEC BatchCheck_TypographicNumberFio 2, @current_id, @current_login
					EXEC WriteToBatchProcessLog @instance, @current_id, @checkType, 0, ''DONE''
					
					-- Блокируем ошибочную строку, чтобы она не хваталась остальными транзакциями
					UPDATE CommonNationalExamCertificateRequestBatch
					SET IsCorrect = 1					
					WHERE Id = @current_id					
				END TRY
				BEGIN CATCH								
					SELECT @message = ''[ERROR].[BatchProcess_TypographicNumberFio].['' + CAST(@current_id AS VARCHAR(255)) + '']: '' + ERROR_MESSAGE()
					EXEC WriteToBatchProcessLog @instance, @current_id, @checkType, 1, @message
					RAISERROR (@message, 16, 1) WITH LOG			
				END CATCH			
				
				DELETE FROM @ids
				WHERE Id = @current_id	
				SET @count = @count - 1			
			END		
			
			DELETE FROM @ids		
		COMMIT TRANSACTION @name
	END
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[BatchProcess_LicenseNumberFio]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchProcess_LicenseNumberFio]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[BatchProcess_LicenseNumberFio] 
	@instance INT
AS
BEGIN	
	DECLARE @checkType INT = 1

	DECLARE @count INT
	DECLARE @current_id BIGINT
	DECLARE @current_login VARCHAR(255)
	DECLARE @name VARCHAR(50) = ''LicenseNumberFio''
	DECLARE @message VARCHAR(2048)
	DECLARE @ids TABLE (Id BIGINT, OwnerAccountId BIGINT);

	WHILE 1=1
	BEGIN
		BEGIN TRANSACTION @name
			UPDATE CommonNationalExamCertificateCheckBatch WITH (ROWLOCK, UPDLOCK)
			SET 
				IsProcess = 0,
				IsCorrect = 0
			OUTPUT inserted.Id, inserted.OwnerAccountId INTO @ids
			WHERE Id IN			
				(SELECT TOP 10 Id
				 FROM CommonNationalExamCertificateCheckBatch cer WITH (NOLOCK)
				 WHERE IsProcess = 1 AND [Type] IN (0, 1) AND IsCorrect IS NULL)			
		
			SELECT @count = COUNT(*) FROM @ids
			IF (@count = 0)
			BEGIN
				ROLLBACK TRANSACTION @name
				RETURN
			END
			
			WHILE @count > 0
			BEGIN
				BEGIN TRY
					SELECT TOP 1 @current_id = i.Id, @current_login = a.[Login]
					FROM @ids i LEFT JOIN dbo.Account a WITH (NOLOCK) ON a.Id = i.OwnerAccountId
						
					IF (@current_login IS NULL)
					BEGIN
						SET @message = ''Не найден Login: '' + @current_login
						RAISERROR (@message, 16, 1)
					END
						
					EXEC BatchCheck_LicenseNumberFio 2, @current_id, @current_login
					EXEC WriteToBatchProcessLog @instance, @current_id, @checkType, 0, ''DONE''
					
					-- Блокируем ошибочную строку, чтобы она не хваталась остальными транзакциями
					UPDATE CommonNationalExamCertificateCheckBatch
					SET IsCorrect = 1
					WHERE Id = @current_id					
				END TRY
				BEGIN CATCH								
					SELECT @message = ''[ERROR].[BatchProcess_LicenseNumberFio].['' + CAST(@current_id AS VARCHAR(255)) + '']: '' + ERROR_MESSAGE()
					EXEC WriteToBatchProcessLog @instance, @current_id, @checkType, 1, @message
					RAISERROR (@message, 16, 1) WITH LOG			
				END CATCH			
				
				DELETE FROM @ids
				WHERE Id = @current_id	
				SET @count = @count - 1			
			END		
			
			DELETE FROM @ids	
		COMMIT TRANSACTION @name	
	END
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[BatchProcess_FioDocumentNumberSeries]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BatchProcess_FioDocumentNumberSeries]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[BatchProcess_FioDocumentNumberSeries] 
	@instance INT
AS
BEGIN	
	DECLARE @checkType INT = 3

	DECLARE @count INT
	DECLARE @current_id BIGINT
	DECLARE @current_login VARCHAR(255)
	DECLARE @name VARCHAR(50) = ''FioDocumentNumberSeries_'' + CAST(@instance AS VARCHAR(10))
	DECLARE @message VARCHAR(2048)
	DECLARE @ids TABLE (Id BIGINT, OwnerAccountId BIGINT);

	WHILE 1=1
	BEGIN
		BEGIN TRANSACTION @name
			UPDATE CommonNationalExamCertificateRequestBatch WITH (ROWLOCK, UPDLOCK)
			SET 
				IsProcess = 0,
				IsCorrect = 0
			OUTPUT inserted.Id, inserted.OwnerAccountId INTO @ids
			WHERE Id IN			
				(SELECT TOP 10 Id
				 FROM CommonNationalExamCertificateRequestBatch cer WITH (NOLOCK)
				 WHERE IsProcess = 1 AND IsTypographicNumber = 0 AND IsCorrect IS NULL)			
		
			SELECT @count = COUNT(*) FROM @ids
			IF (@count = 0)
			BEGIN
				ROLLBACK TRANSACTION @name
				RETURN
			END
			
			WHILE @count > 0
			BEGIN
				BEGIN TRY
					SELECT TOP 1 @current_id = i.Id, @current_login = a.[Login]
					FROM @ids i LEFT JOIN dbo.Account a WITH (NOLOCK) ON a.Id = i.OwnerAccountId
						
					IF (@current_login IS NULL)
					BEGIN
						SET @message = ''Не найден Login: '' + @current_login
						RAISERROR (@message, 16, 1)
					END
						
					EXEC BatchCheck_FioDocumentNumberSeries 2, @current_id, @current_login
					EXEC WriteToBatchProcessLog @instance, @current_id, @checkType, 0, ''DONE''
					
					-- Блокируем ошибочную строку, чтобы она не хваталась остальными транзакциями
					UPDATE CommonNationalExamCertificateRequestBatch
					SET IsCorrect = 1						
					WHERE Id = @current_id
				END TRY
				BEGIN CATCH								
					SELECT @message = ''[ERROR].[BatchProcess_FioDocumentNumberSeries].['' + CAST(@current_id AS VARCHAR(255)) + '']: '' + ERROR_MESSAGE()
					EXEC WriteToBatchProcessLog @instance, @current_id, @checkType, 1, @message
					RAISERROR (@message, 16, 1) WITH LOG			
				END CATCH			
				
				DELETE FROM @ids
				WHERE Id = @current_id	
				SET @count = @count - 1			
			END		
			
			DELETE FROM @ids		
		COMMIT TRANSACTION @name
	END
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteBatchCheckHistoryById]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteBatchCheckHistoryById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- Удаление истории пакетных проверок по id проверки
CREATE PROCEDURE [dbo].[DeleteBatchCheckHistoryById]
	@checkId BIGINT, -- id проверки
	@login nvarchar(255) -- логин пользователя
AS
BEGIN

	SET NOCOUNT ON;

	declare @batchId BIGINT
	declare @isBatchCheck BIT

	if (@login is null or [dbo].[IsUserOrgLogDisabled](@login) = 1)
	begin
		select TOP 1 @batchId = ch.BatchId, @isBatchCheck = 1
		from CheckHistory ch
		inner join [dbo].[CheckSenderTypes] st on ch.SenderTypeId = st.Id
		where ch.id = @checkId and st.Id = 2

		set @isBatchCheck = ISNULL(@isBatchCheck, 0)
		
		if (@isBatchCheck = 1)
		begin
		begin transaction
		delete from [dbo].[CheckResultsHistory] where [CheckId] = @checkId
		if (@batchId is not null) 
		begin
			delete from [dbo].[CommonNationalExamCertificateRequestBatch] where [Id] = @batchId
			delete from [dbo].[CommonNationalExamCertificateCheckBatch] where [Id] = @batchId
		end
		delete from [dbo].[CheckHistory] where id = @checkId

		commit transaction
		end
	end

    SET NOCOUNT OFF;
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsGroupsPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsGroupsPagesCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetCheckResultsGroupsPagesCount]
	@login nvarchar(255),
	@checkId BIGINT
AS 

SELECT COUNT(DISTINCT chr.GroupId) 
FROM 
	dbo.CheckHistory ch 
	JOIN dbo.GetOrganizationAccountsIds(@login) org ON ch.OwnerId = org.AccountId
	JOIN dbo.CheckResultsHistory chr ON ch.Id = chr.CheckId	
WHERE ch.Id = @checkId' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsGroupsPaged]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsGroupsPaged]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- вернет distinct по groupId
CREATE PROC [dbo].[GetCheckResultsGroupsPaged]
	@login nvarchar(255),
	@checkId BIGINT, -- Id проверки
    @startRowIndex INT = 1, -- Начиная с какой строки выбирать
    @countOnPage INT = NULL  -- Сколько участников на странице
AS 
BEGIN
DECLARE @totalRows INT
IF (@countOnPage IS NULL)
BEGIN	
	SELECT @totalRows = COUNT(DISTINCT chr.GroupId) 
	FROM 
		dbo.CheckHistory ch 
		JOIN dbo.GetOrganizationAccountsIds(@login) org ON ch.OwnerId = org.AccountId
		JOIN dbo.Account a ON org.AccountId = a.Id
		JOIN dbo.CheckResultsHistory chr ON ch.Id = chr.CheckId	
	WHERE ch.Id = @checkId
END
DECLARE @endRowIndex INT = ISNULL(@startRowIndex, 1) + ISNULL(@countOnPage, @totalRows)

;WITH f AS (SELECT 
	A1.GroupId,	
	A1.Surname,
	A1.Name,
	A1.SecondName,		
	A1.DocumentSeries,
	A1.DocumentNumber,		
	row_number() OVER (ORDER BY A1.GroupId) AS RowNumber
FROM (
	SELECT DISTINCT
		ch.GroupId,	
		ch.CheckSurname AS Surname,
		CASE WHEN ISNULL(ch.CheckName, '''') = '''' THEN ch.Name ELSE ch.CheckName END AS Name,
		CASE WHEN ISNULL(ch.CheckSecondName, '''') = '''' THEN ch.SecondName ELSE ch.CheckSecondName END AS SecondName,		
		CASE WHEN ISNULL(ch.CheckDocumentSeries, '''') = '''' THEN ch.DocumentSeries ELSE ch.CheckDocumentSeries END AS DocumentSeries,
		CASE WHEN ISNULL(ch.CheckDocumentNumber, '''') = '''' THEN ch.DocumentNumber ELSE ch.CheckDocumentNumber END AS DocumentNumber		
	FROM 
		dbo.CheckHistory h 
		JOIN dbo.GetOrganizationAccountsIds(@login) org ON
			h.OwnerId = org.AccountId
		JOIN dbo.CheckResultsHistory ch ON
			h.Id = ch.CheckId
		LEFT JOIN rbdc.Regions r ON
			r.REGION = ch.RegionId
	WHERE h.Id = @checkId) AS A1)
SELECT
	f.GroupId,	
	f.Surname,
	f.Name,
	f.SecondName,		
	f.DocumentSeries,
	f.DocumentNumber
FROM f 	
WHERE f.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsGroupMarks]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsGroupMarks]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[GetCheckResultsGroupMarks]
	@login nvarchar(255),
	@checkId BIGINT,
    @groupId INT
AS 
BEGIN
SELECT 
	ch.Id,	
	ch.GroupId,	
	ch.ParticipantId,
	ch.UseYear,
	ch.RegionId,
	A1.RegionName,
	ISNULL(ch.Surname, ch.CheckSurname) AS Surname,
	ISNULL(ch.Name, ch.CheckName) AS Name,
	ISNULL(ch.SecondName, ch.CheckSecondName) AS SecondName,
	ISNULL(ch.DocumentSeries, ch.CheckDocumentSeries) AS DocumentSeries,
	ISNULL(ch.DocumentNumber, ch.CheckDocumentNumber) AS DocumentNumber,
	A1.SubjectCode,
	A1.SubjectName,
	A1.Mark,	
	A1.ProcessCondition,	
	A1.GlobalStatusID,
	ISNULL(A1.StatusName, ''Не найдено'') AS StatusName,
	A1.HasAppeal,
	A1.CertificateID,
	ISNULL(A1.LicenseNumber, ch.CheckCertificateNumber) AS LicenseNumber,
	ISNULL(A1.TypographicNumber, ch.CheckTypographicNumber) AS TypographicNumber,
	A1.Cancelled
FROM 
	dbo.CheckHistory h 
	JOIN dbo.GetOrganizationAccountsIds(@login) org ON
		h.OwnerId = org.AccountId	
	JOIN dbo.CheckResultsHistory ch ON
		h.Id = ch.CheckId
	LEFT JOIN (
		SELECT  
			ch.Id,
			ch.GroupId,	
			ch.ParticipantId,
			ch.UseYear,
			ch.RegionId,
			r.RegionName,
			ch.Surname,
			ch.Name,
			ch.SecondName,
			ch.DocumentSeries,
			ch.DocumentNumber,
			s.SubjectCode,
			s.SubjectName,
			cm.Mark,	
			cm.ProcessCondition,	
			rsg.GlobalStatusID,
			rsg.StatusName AS StatusName,
			cm.HasAppeal,
			cer.CertificateID,
			cer.LicenseNumber,
			cer.TypographicNumber,
			cer.Cancelled
		FROM 
			dbo.CheckHistory h 
			JOIN dbo.CheckResultsHistory ch ON
				h.Id = ch.CheckId
			JOIN rbdc.Regions r ON
				r.REGION = ch.RegionId
			JOIN prn.CertificatesMarks cm ON 
				cm.ParticipantFK = ch.ParticipantId AND
				cm.REGION = ch.RegionId AND
				ch.UseYear = cm.UseYear
			JOIN prn.Certificates cer ON
				cer.CertificateID = cm.CertificateFK AND
				cer.UseYear = cm.UseYear AND
				cer.REGION = cm.REGION
			JOIN dat.Subjects s ON
				s.SubjectCode = cm.SubjectCode
			JOIN dbo.ResultStatuses rs ON
				rs.ProcessCondition = cm.ProcessCondition
			JOIN dbo.ResultGlobalStatuses rsg ON 
				rsg.GlobalStatusID = rs.GlobalStatusID		 
		WHERE 
			h.Id = @checkId
			AND ch.GroupId = @groupId) AS A1
	ON ch.Id = A1.Id
WHERE 
	h.Id = @checkId
	AND ch.GroupId = @groupId
ORDER BY ch.ParticipantId, A1.CertificateID	
END
	
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryPagesCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetCheckHistoryPagesCount]
	@login nvarchar(255),
	@rowsOnPageCount INT, -- кол-во строк на странице
    @senderType INT, -- Источник проверки (интерактив, пакет, сервис)
    @checkType INT -- Тип проверки 
AS 

-- Интерактивная проверка
IF (@senderType = 1)
BEGIN
	SELECT COUNT(*) 
	FROM 
		dbo.GetOrganizationAccountsIds(@login) org		
		JOIN dbo.Account a ON org.AccountId = a.Id
		JOIN dbo.CheckHistory ch ON a.Id = ch.OwnerId
	WHERE SenderTypeId = @senderType AND CheckTypeId = @checkType
END
ELSE
-- Пакетная проверка, Проверка через сервис
BEGIN
	-- Запрос по регистрационному номеру и ФИО
	IF (@checkType = 1)
	BEGIN
		SELECT COUNT(*)
		FROM 
			dbo.GetOrganizationAccountsIds(@login) org		
			JOIN dbo.Account a ON org.AccountId = a.Id
			JOIN dbo.CheckHistory ch ON a.Id = ch.OwnerId
		WHERE ch.SenderTypeId = @senderType AND ch.CheckTypeId = @checkType
	END
	-- Запрос по типографскому номеру и ФИО
	ELSE IF (@checkType = 2)
	BEGIN
		SELECT COUNT(*) 
		FROM 
			dbo.GetOrganizationAccountsIds(@login) org		
			JOIN dbo.Account a ON org.AccountId = a.Id
			JOIN dbo.CommonNationalExamCertificateRequestBatch ch ON a.Id = ch.OwnerAccountId
		WHERE IsTypographicNumber = 1
	END
	-- Запрос по ФИО, номеру и серии документа
	ELSE IF (@checkType = 3 OR @checkType = 5)
	BEGIN
		SELECT COUNT(*) 
		FROM 
			dbo.GetOrganizationAccountsIds(@login) org		
			JOIN dbo.Account a ON org.AccountId = a.Id
			JOIN dbo.CommonNationalExamCertificateRequestBatch ch ON a.Id = ch.OwnerAccountId
		WHERE IsTypographicNumber = 0
	END
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckHistoryPaged]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckHistoryPaged]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetCheckHistoryPaged]
	@login nvarchar(255),
    @startRowIndex INT = 1, -- Начиная с какой строки выбирать
    @rowsOnPageCount INT = NULL, -- Сколько строк на странице
    @senderType INT, -- Источник проверки (интерактив, пакет, сервис)
    @checkType INT -- Тип проверки 
AS 

DECLARE @totalRows INT
DECLARE @endRowIndex INT

-- Интерактивная проверка
IF (@senderType = 1)
BEGIN
	IF (@rowsOnPageCount IS NULL)
	BEGIN
		SELECT @totalRows = COUNT(*) 
		FROM 
			dbo.GetOrganizationAccountsIds(@login) org		
			JOIN dbo.Account a ON org.AccountId = a.Id
			JOIN dbo.CheckHistory ch ON a.Id = ch.OwnerId
		WHERE SenderTypeId = @senderType AND CheckTypeId = @checkType
	END		
	SET @endRowIndex = ISNULL(@startRowIndex, 1) + ISNULL(@rowsOnPageCount, @totalRows)
	
	SELECT * FROM (
		SELECT  
			ch.Id AS CheckId,					-- Id
			ch.BatchId,
			ch.CreateDate,						-- Дата
			ch.OwnerId,							-- Пользователь
			a.[Login],
			a.FirstName,
			a.LastName,
			a.PatronymicName,
			org.OrganizationId,				-- Организация
			org.OrganizationName,	  
			ch.[Status],						-- Статус		
			row_number() OVER (ORDER BY ch.CreateDate DESC) AS RowNumber
		FROM 
			dbo.GetOrganizationAccountsIds(@login) org		
			JOIN dbo.Account a ON org.AccountId = a.Id
			JOIN dbo.CheckHistory ch ON a.Id = ch.OwnerId						
		WHERE ch.SenderTypeId = @senderType AND ch.CheckTypeId = @checkType) AS A1
	WHERE A1.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
END
ELSE
-- Пакетная проверка, Проверка через сервис
BEGIN
	-- Запрос по регистрационному номеру и ФИО
	IF (@checkType = 1)
	BEGIN
		IF (@rowsOnPageCount IS NULL)
		BEGIN
			SELECT @totalRows = COUNT(*) 
			FROM 
				dbo.GetOrganizationAccountsIds(@login) org		
				JOIN dbo.Account a ON org.AccountId = a.Id
				JOIN dbo.CommonNationalExamCertificateCheckBatch ch ON a.Id = ch.OwnerAccountId
			WHERE [Type] IN (0, 1)
		END		
		SET @endRowIndex = ISNULL(@startRowIndex, 1) + ISNULL(@rowsOnPageCount, @totalRows)	
	
		SELECT * FROM (
			SELECT  
				ch.Id AS CheckId,					-- Id
				b.Id AS BatchId,
				b.CreateDate,						-- Дата
				b.OwnerAccountId AS  OwnerId,		-- Пользователь
				a.[Login],
				a.FirstName,
				a.LastName,
				a.PatronymicName,
				org.OrganizationId,					-- Организация
				org.OrganizationName,	  
				ISNULL(ch.[Status], ''Ожидает проверки'') AS [Status],	-- Статус		
				row_number() OVER (ORDER BY b.CreateDate DESC) AS RowNumber
			FROM 
				dbo.GetOrganizationAccountsIds(@login) org		
				JOIN dbo.Account a ON org.AccountId = a.Id
				JOIN CommonNationalExamCertificateCheckBatch b ON b.OwnerAccountId = a.Id
				LEFT JOIN dbo.CheckHistory ch ON b.Id = ch.BatchId
					AND ch.SenderTypeId IN (2, 3) AND ch.CheckTypeId = 1
			WHERE b.[Type] IN (0, 1)) AS A1
		WHERE A1.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
	END
	-- Запрос по типографскому номеру и ФИО
	ELSE IF (@checkType = 2)
	BEGIN
		IF (@rowsOnPageCount IS NULL)
		BEGIN	
			SELECT @totalRows = COUNT(*) 
			FROM 
				dbo.GetOrganizationAccountsIds(@login) org		
				JOIN dbo.Account a ON org.AccountId = a.Id
				JOIN dbo.CommonNationalExamCertificateRequestBatch ch ON a.Id = ch.OwnerAccountId
			WHERE IsTypographicNumber = 1
		END
		SET @endRowIndex = ISNULL(@startRowIndex, 1) + ISNULL(@rowsOnPageCount, @totalRows)	
	
		SELECT * FROM (
			SELECT  
				ch.Id AS CheckId,					-- Id
				b.Id AS BatchId,
				b.CreateDate,						-- Дата
				b.OwnerAccountId AS  OwnerId,		-- Пользователь
				a.[Login],
				a.FirstName,
				a.LastName,
				a.PatronymicName,
				org.OrganizationId,					-- Организация
				org.OrganizationName,	  
				ISNULL(ch.[Status], ''Ожидает проверки'') AS [Status],	-- Статус		
				row_number() OVER (ORDER BY b.CreateDate DESC) AS RowNumber
			FROM 
				dbo.GetOrganizationAccountsIds(@login) org		
				JOIN dbo.Account a ON org.AccountId = a.Id
				JOIN CommonNationalExamCertificateRequestBatch b ON b.OwnerAccountId = a.Id
				LEFT JOIN dbo.CheckHistory ch ON b.Id = ch.BatchId
					AND ch.SenderTypeId IN (2, 3) AND ch.CheckTypeId = 2
			WHERE 
				b.IsTypographicNumber = 1		
			) AS A1
		WHERE A1.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
	END
	-- Запрос по ФИО, номеру и серии документа
	ELSE IF (@checkType = 3 OR @checkType = 5)
	BEGIN
		IF (@rowsOnPageCount IS NULL)
		BEGIN	
			SELECT @totalRows = COUNT(*) 
			FROM 
				dbo.GetOrganizationAccountsIds(@login) org		
				JOIN dbo.Account a ON org.AccountId = a.Id
				JOIN dbo.CommonNationalExamCertificateRequestBatch ch ON a.Id = ch.OwnerAccountId
			WHERE IsTypographicNumber = 0
		END
		SET @endRowIndex = ISNULL(@startRowIndex, 1) + ISNULL(@rowsOnPageCount, @totalRows)	
	
		SELECT * FROM (
			SELECT  
				ch.Id AS CheckId,					-- Id
				b.Id AS BatchId,
				b.CreateDate,						-- Дата
				b.OwnerAccountId AS  OwnerId,		-- Пользователь
				a.[Login],
				a.FirstName,
				a.LastName,
				a.PatronymicName,
				org.OrganizationId,					-- Организация
				org.OrganizationName,	  
				ISNULL(ch.[Status], ''Ожидает проверки'') AS [Status],	-- Статус		
				row_number() OVER (ORDER BY b.CreateDate DESC) AS RowNumber
			FROM 
				dbo.GetOrganizationAccountsIds(@login) org		
				JOIN dbo.Account a ON org.AccountId = a.Id
				JOIN CommonNationalExamCertificateRequestBatch b ON b.OwnerAccountId = a.Id
				LEFT JOIN dbo.CheckHistory ch ON b.Id = ch.BatchId
					AND ch.SenderTypeId IN (2, 3) AND ch.CheckTypeId IN (3, 5)
			WHERE b.IsTypographicNumber = 0) AS A1
		WHERE A1.RowNumber BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1	
	END
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryPagesCount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryPagesCount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[GetCheckResultsHistoryPagesCount]
	@login nvarchar(255),
	@checkId BIGINT
AS 

SELECT MAX(GroupId) 
FROM 
	dbo.CheckHistory ch
	JOIN dbo.GetOrganizationAccountsIds(@login) org ON ch.OwnerId = org.AccountId	
	JOIN dbo.CheckResultsHistory chr ON ch.Id = chr.CheckId
WHERE ch.Id = @checkId' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryPaged_Obsolete]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryPaged_Obsolete]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- 4.2.4.	Запрос по ФИО, номеру и серии документа (пакетный) (с сохранением старого формата)
CREATE PROC [dbo].[GetCheckResultsHistoryPaged_Obsolete]
	@login nvarchar(255),
	@checkId BIGINT,
    @startRowIndex INT = 0, -- Начиная с какой строки выбирать
    @participantsOnPageCount INT = NULL  -- Сколько участников на странице
AS 
BEGIN
DECLARE @totalRows INT
IF (@participantsOnPageCount IS NULL)
BEGIN	
	SELECT @totalRows = MAX(GroupId) 
	FROM
		dbo.CheckHistory ch 
		JOIN dbo.GetOrganizationAccountsIds(@login) org ON ch.OwnerId = org.AccountId
		JOIN dbo.Account a ON org.AccountId = a.Id
		JOIN dbo.CheckResultsHistory chr ON ch.Id = chr.CheckId	
	WHERE ch.Id = @checkId
END
DECLARE @endRowIndex INT = ISNULL(@startRowIndex, 1) + ISNULL(@participantsOnPageCount, @totalRows)

SELECT 
	ch.Id,
	ch.GroupId,	
	ch.ParticipantId,
	ch.UseYear,
	ch.RegionId,
	A1.RegionName,
	ISNULL(ch.Surname, ch.CheckSurname) AS Surname,
	ISNULL(ch.Name, ch.CheckName) AS Name,
	ISNULL(ch.SecondName, ch.CheckSecondName) AS SecondName,
	ISNULL(ch.DocumentSeries, ch.CheckDocumentSeries) AS DocumentSeries,
	ISNULL(ch.DocumentNumber, ch.CheckDocumentNumber) AS DocumentNumber,
	A1.CertificateID,
	A1.Cancelled,
	ISNULL(A1.LicenseNumber, ch.CheckCertificateNumber) AS LicenseNumber,
	ISNULL(A1.TypographicNumber, ch.CheckTypographicNumber) AS TypographicNumber,		
	ISNULL(A1.StatusName, ''Не найдено'') AS StatusName,
	0 AS UniqueChecks,
	A1.sbj1,
	A1.sbj1ap,
	A1.sbj2,
	A1.sbj2ap,
	A1.sbj3,
	A1.sbj3ap,
	A1.sbj4,
	A1.sbj4ap,
	A1.sbj5,
	A1.sbj5ap,
	A1.sbj6,
	A1.sbj6ap,
	A1.sbj7,
	A1.sbj7ap,
	A1.sbj8,
	A1.sbj8ap,
	A1.sbj9,
	A1.sbj9ap,
	A1.sbj10,
	A1.sbj10ap,
	A1.sbj11,
	A1.sbj11ap,
	A1.sbj12,
	A1.sbj12ap,
	A1.sbj13,
	A1.sbj13ap,
	A1.sbj18,
	A1.sbj18ap
FROM 
	dbo.CheckHistory h 
	JOIN dbo.GetOrganizationAccountsIds(@login) org ON
		h.OwnerId = org.AccountId	
	JOIN CheckResultsHistory ch ON 
		h.Id = ch.CheckId
	LEFT JOIN 
	(SELECT  
		ch.Id,
		r.RegionName,
		cer.CertificateID,
		cer.Cancelled,
		cer.LicenseNumber,
		cer.TypographicNumber,	
		rsg.StatusName AS StatusName,	
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 1) as sbj1,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 1) as sbj1ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 2) as sbj2,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 3) as sbj2ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 3) as sbj3,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 3) as sbj3ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 4) as sbj4,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 4) as sbj4ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 5) as sbj5,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 5) as sbj5ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 6) as sbj6,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 6) as sbj6ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 7) as sbj7,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 7) as sbj7ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 8) as sbj8,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 8) as sbj8ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 9) as sbj9,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 9) as sbj9ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 10) as sbj10,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 10) as sbj10ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 11) as sbj11,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 11) as sbj11ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 12) as sbj12,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 12) as sbj12ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 13) as sbj13,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 13) as sbj13ap,
		dbo.AggregateSubject(cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, 18) as sbj18,
		dbo.AggregateAppeal(cm.HasAppeal, cm.SubjectCode, 18) as sbj18ap
	FROM 
		dbo.CheckHistory h 
		JOIN dbo.CheckResultsHistory ch ON
			h.Id = ch.CheckId
		JOIN rbdc.Regions r ON
			r.REGION = ch.RegionId
		JOIN prn.CertificatesMarks cm ON 
			cm.ParticipantFK = ch.ParticipantId AND
			cm.REGION = ch.RegionId AND
			ch.UseYear = cm.UseYear
		JOIN prn.Certificates cer ON
			cer.CertificateID = cm.CertificateFK AND
			cer.UseYear = cm.UseYear AND
			cer.REGION = cm.REGION
		JOIN dat.Subjects s ON
			s.SubjectCode = cm.SubjectCode
		JOIN dbo.ResultStatuses rs ON
			rs.ProcessCondition = cm.ProcessCondition
		JOIN dbo.ResultGlobalStatuses rsg ON 
			rsg.GlobalStatusID = rs.GlobalStatusID		 
	WHERE 
		h.Id = @checkId
		AND ch.GroupId BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
	GROUP BY 	
		ch.Id,
		r.RegionName,	
		cer.CertificateID,
		cer.Cancelled,
		cer.LicenseNumber,
		cer.TypographicNumber,
		rsg.StatusName) AS A1
	ON ch.Id = A1.Id
WHERE 
	h.Id = @checkId
	AND ch.GroupId BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheckResultsHistoryPaged]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCheckResultsHistoryPaged]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[GetCheckResultsHistoryPaged]
	@login nvarchar(255),
	@checkId BIGINT, -- Id проверки
    @startRowIndex INT = 0, -- Начиная с какой строки выбирать
    @participantsOnPageCount INT = NULL  -- Сколько участников на странице
AS 
BEGIN
DECLARE @totalRows INT
IF (@participantsOnPageCount IS NULL)
BEGIN	
	SELECT @totalRows = MAX(GroupId) 
	FROM 
		dbo.CheckHistory ch 
		JOIN dbo.GetOrganizationAccountsIds(@login) org ON ch.OwnerId = org.AccountId
		JOIN dbo.Account a ON org.AccountId = a.Id
		JOIN dbo.CheckResultsHistory chr ON ch.Id = chr.CheckId	
	WHERE ch.Id = @checkId
END
DECLARE @endRowIndex INT = ISNULL(@startRowIndex, 1) + ISNULL(@participantsOnPageCount, @totalRows)

SELECT 
	ch.Id,	
	ch.GroupId,	
	ch.ParticipantId,
	ch.UseYear,
	ch.RegionId,
	A1.RegionName,
	ISNULL(ch.Surname, ch.CheckSurname) AS Surname,
	ISNULL(ch.Name, ch.CheckName) AS Name,
	ISNULL(ch.SecondName, ch.CheckSecondName) AS SecondName,
	ISNULL(ch.DocumentSeries, ch.CheckDocumentSeries) AS DocumentSeries,
	ISNULL(ch.DocumentNumber, ch.CheckDocumentNumber) AS DocumentNumber,
	A1.SubjectCode,
	A1.SubjectName,
	A1.Mark,	
	A1.ProcessCondition,	
	A1.GlobalStatusID,
	ISNULL(A1.StatusName, ''Не найдено'') AS StatusName,
	A1.HasAppeal,
	A1.CertificateID,
	ISNULL(A1.LicenseNumber, ch.CheckCertificateNumber) AS LicenseNumber,
	ISNULL(A1.TypographicNumber, ch.CheckTypographicNumber) AS TypographicNumber,
	A1.Cancelled
FROM 
	dbo.CheckHistory h 
	JOIN dbo.GetOrganizationAccountsIds(@login) org ON
		h.OwnerId = org.AccountId	
	JOIN dbo.CheckResultsHistory ch ON
		h.Id = ch.CheckId
	LEFT JOIN (
		SELECT  
			ch.Id,
			ch.GroupId,	
			ch.ParticipantId,
			ch.UseYear,
			ch.RegionId,
			r.RegionName,
			ch.Surname,
			ch.Name,
			ch.SecondName,
			ch.DocumentSeries,
			ch.DocumentNumber,
			s.SubjectCode,
			s.SubjectName,
			cm.Mark,	
			cm.ProcessCondition,	
			rsg.GlobalStatusID,
			rsg.StatusName AS StatusName,
			cm.HasAppeal,
			cer.CertificateID,
			cer.LicenseNumber,
			cer.TypographicNumber,
			cer.Cancelled
		FROM 
			dbo.CheckHistory h 
			JOIN dbo.CheckResultsHistory ch ON
				h.Id = ch.CheckId
			JOIN rbdc.Regions r ON
				r.REGION = ch.RegionId
			JOIN prn.CertificatesMarks cm ON 
				cm.ParticipantFK = ch.ParticipantId AND
				cm.REGION = ch.RegionId AND
				ch.UseYear = cm.UseYear
			JOIN prn.Certificates cer ON
				cer.CertificateID = cm.CertificateFK AND
				cer.UseYear = cm.UseYear AND
				cer.REGION = cm.REGION
			JOIN dat.Subjects s ON
				s.SubjectCode = cm.SubjectCode
			JOIN dbo.ResultStatuses rs ON
				rs.ProcessCondition = cm.ProcessCondition
			JOIN dbo.ResultGlobalStatuses rsg ON 
				rsg.GlobalStatusID = rs.GlobalStatusID		 
		WHERE 
			h.Id = @checkId
			AND ch.GroupId BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1) AS A1
	ON ch.Id = A1.Id
WHERE 
	h.Id = @checkId
	AND ch.GroupId BETWEEN ISNULL(@startRowIndex, 1) AND @endRowIndex - 1		
ORDER BY ch.GroupId
END' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountByRegionReport]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccountByRegionReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetUserAccountByRegionReport
-- ========================================================
-- Отчет пользователей по регионам.
-- ========================================================
CREATE procedure [dbo].[GetUserAccountByRegionReport]
as 
begin

  ;with RegionUserCountCTE as
  (select 
    isnull(r.Code, '''') RegionCode
    , isnull(r.Name, ''Не указано'') RegionName
    , count(*) [Count]
  from dbo.Account a with(nolock)
    left join dbo.OrganizationRequest2010 OrgReq with(nolock) on OrgReq.Id = a.OrganizationId
    left join dbo.Region r with(nolock) on r.Id = OrgReq.RegionId
    inner join dbo.GroupAccount ga on ga.AccountId=a.id
  where ga.groupid=1
  group by
    r.Id, r.Code, r.Name
  )
  select *, 0 [IsTotal]
  from RegionUserCountCTE
  union all
  select NULL, NULL, sum(count), 1 from RegionUserCountCTE
  order by [IsTotal], RegionCode

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountActivityByRegionReport]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserAccountActivityByRegionReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetUserAccountActivityByRegionReport
-- ========================================================
-- Отчет по активности пользователей по регионам.
-- v.1.0: Create by Fomin Dmitriy 03.09.2008
-- ========================================================
CREATE procedure [dbo].[GetUserAccountActivityByRegionReport]
as 
begin
  declare
    @year int

  set @year = Year(GetDate())
  
  select
    region.Code RegionCode
    , region.Name RegionName
    , report.[Count]
    , case 
      when report.RegionId is null then 1
      else 0
    end IsTotal
  from (select 
      region.Id RegionId
      , count(*) [Count]
    from dbo.AuthenticationEventLog auth_log
      inner join dbo.Account account
        inner join dbo.Organization organization
          inner join dbo.Region region
            on region.Id = organization.RegionId
          on organization.Id = account.OrganizationId
        on auth_log.AccountId = account.Id
    where
      account.Id in (select 
            group_account.AccountId
          from dbo.GroupAccount group_account
            inner join dbo.[Group] [group]
              on [group].Id = group_account.GroupId
          where [group].Code = ''User'')
      and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
          , account.RegistrationDocument) = ''activated''
      and auth_log.IsPasswordValid = 1
      and auth_log.IsIpValid = 1
    group by 
      region.Id
    with rollup) report
      left outer join dbo.Region region
        on region.Id = report.RegionId
  order by
    IsTotal
    , region.SortIndex

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecuteCommonNationalExamCertificateLoadingTask]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Выполнить задание на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[ExecuteCommonNationalExamCertificateLoadingTask]
  @id bigint
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  set @eventCode = N''CNE_LOAD''

  set @internalId = dbo.GetInternalId(@Id)

  update cne_certificate_loading_task
  set
    IsProcess = 0
    , UpdateDate = @currentDate
    , EditorAccountId = @accountId
    , EditorIp = @editorIp
  from
    dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task
  where
    cne_certificate_loading_task.Id = @internalId
    and cne_certificate_loading_task.IsActive <> 0
    and cne_certificate_loading_task.IsLoaded <> 1

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Выполнить задание на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc [dbo].[ExecuteCommonNationalExamCertificateDenyLoadingTask]
  @id bigint
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  set @eventCode = N''CNE_DNY_LOAD''

  set @internalId = dbo.GetInternalId(@Id)

  update cne_certificate_deny_loading_task
  set
    IsProcess = 0
    , UpdateDate = @currentDate
    , EditorAccountId = @accountId
    , EditorIp = @editorIp
  from
    dbo.CommonNationalExamCertificateDenyLoadingTask cne_certificate_deny_loading_task
  where
    cne_certificate_deny_loading_task.Id = @internalId
    and cne_certificate_deny_loading_task.IsActive <> 0
    and cne_certificate_deny_loading_task.IsLoaded <> 1

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteNews]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteNews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.DeleteNews

-- =============================================
-- Удаление новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[DeleteNews]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  set @innerIds = ''''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N'',''
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N''NWS_DEL''

  delete news
  from 
    dbo.News news
      inner join @idTable idTable
        on news.Id = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteDocument]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDocument]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.DeleteDocument

-- =============================================
-- Удаление документов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[DeleteDocument]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  set @innerIds = ''''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N'',''
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N''DOC_DEL''

  begin tran delete_document_tran

    delete document_context
    from 
      dbo.DocumentContext document_context
        inner join @idTable idTable
          on document_context.DocumentId = idTable.[id]

    if (@@error <> 0)
      goto undo

    delete [document]
    from 
      dbo.[Document] [document]
        inner join @idTable idTable
          on [document].[Id] = idTable.[id]

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran delete_document_tran

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0

  undo:

  rollback tran delete_document_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteDeliveries]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteDeliveries]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.DeleteNews

-- =============================================
-- Удаление рассылок.
-- v.1.0: Created by Yusupov Kirill 19.04.2010
-- =============================================

CREATE proc [dbo].[DeleteDeliveries]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select [value] from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N''DLV_DEL''

  --Удалим получателей рассылки
  delete recipient
    from 
      dbo.DeliveryRecipients recipient
        inner join @idTable idTable
          on recipient.DeliveryId = idTable.[id]

  --Удалим лог рассылки
  delete [log]
    from 
      dbo.DeliveryLog [log]
        inner join @idTable idTable
          on [log].DeliveryId = idTable.[id]

  --Удалим саму рассылку
  delete delivery
  from 
    dbo.Delivery delivery
      inner join @idTable idTable
        on delivery.Id = idTable.[id]


  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAskedQuestion]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAskedQuestion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.DeleteAskedQuestion

-- =============================================
-- Удаление вопросов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[DeleteAskedQuestion]
  @ids nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @innerIds nvarchar(4000)

  set @innerIds = ''''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N'',''
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  set @eventCode = N''FAQ_DEL''

  begin tran delete_faq_tran

    delete asked_question_context
    from 
      dbo.AskedQuestionContext asked_question_context
        inner join @idTable idTable
          on asked_question_context.AskedQuestionId = idTable.[id]

    if (@@error <> 0)
      goto undo

    delete asked_question
    from 
      dbo.AskedQuestion asked_question
        inner join @idTable idTable
          on asked_question.[Id] = idTable.[id]

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran delete_faq_tran

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0

  undo:

  rollback tran delete_faq_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckSchoolLeavingCertificate]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSchoolLeavingCertificate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.CheckSchoolLeavingCertificate
-- ============================================
-- Процедура проверки аттестатов
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ============================================
CREATE procedure [dbo].[CheckSchoolLeavingCertificate]
  @certificateNumber nvarchar(255)
  , @login nvarchar(255)
  , @ip nvarchar(255)
as
begin 
  select
    @certificateNumber CertificateNumber
    , case when school_leaving_certificate_deny.Id is null then 0
      else 1
    end IsDeny
    , school_leaving_certificate_deny.Comment DenyComment
  from 
    (select
      @certificateNumber CertificateNumber) as schoolleaving_certificate_check
      left join dbo.SchoolLeavingCertificateDeny school_leaving_certificate_deny
        on schoolleaving_certificate_check.CertificateNumber between 
          school_leaving_certificate_deny.CertificateNumberFrom
            and school_leaving_certificate_deny.CertificateNumberTo  
  
        
  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
  
  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login 

  set @eventCode = ''SLC_CHK''
  set @updateId = NewId()     
  
  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckLastAccountIp]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckLastAccountIp]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.CheckLastAccountIp
-- ====================================================
-- Процедура проверки последнего адресса пользователя,
-- под которым он авторизовался
-- v.1.0: Created by Sedov Anton 08.07.2008
-- v.1.1: Modified by Fomin Dmitriy 28.08.2008
-- Добавлена регистрация события авторизации.
-- ====================================================
CREATE procedure [dbo].[CheckLastAccountIp] 
  @accountLogin nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare
    @isLastIp bit
    , @entityParams nvarchar(1000)
    , @sourceEntityIds nvarchar(255)
    , @accountId bigint

  set @isLastIp = null

  select top 1
    @isLastIp = case when auth_event_log.Ip = @ip
        then 1
      else 0
    end
    , @accountId = account.Id
  from 
    dbo.AuthenticationEventLog auth_event_log
      left join dbo.Account account
        on account.Id = auth_event_log.AccountId
  where
    account.[Login] = @accountLogin
      and auth_event_log.IsPasswordValid = 1
      and auth_event_log.IsIpValid = 1
  order by 
    auth_event_log.Date desc
    

  select
    @accountLogin AccountLogin
    , @ip Ip
    , isnull(@isLastIp, 0) IsLastIp           

  set @entityParams = @accountLogin + N''|'' +
      @ip + N''||'' +
      convert(nvarchar, case 
          when @isLastIp is null then 0 
          else 1 
        end)  + ''|'' +
      convert(nvarchar, isnull(@isLastIp, 0))

  set @sourceEntityIds = convert(nvarchar(255), @accountId)

  if isnull(@isLastIp, 0) = 1
    exec dbo.RegisterEvent 
      @accountId = @accountId
      , @ip = @ip
      , @eventCode = N''USR_VERIFY''
      , @sourceEntityIds = @sourceEntityIds
      , @eventParams = @entityParams
      , @updateId = null

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckEntrant]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckEntrant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.CheckEntrant
-- ============================================
-- Процедура проверки поступивших абитуриентов
-- v.1.0: Create by Sedov Anton 08.07.2008
-- ============================================
CREATE procedure [dbo].[CheckEntrant] 
  @certificateNumber nvarchar(255)
  , @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  select  
    @certificateNumber CertificateNumber
    , entrant.LastName LastName
    , entrant.FirstName FirstName
    , entrant.PatronymicName PatronymicName
    , organization.[Name] OrganizationName
    , entrant.CreateDate EntrantCreateDate
    , case when entrant.CertificateNumber is null
        then 0
      else 1
    end IsExist
  from 
    (select @certificateNumber CertificateNumber) as check_entrant
      left join dbo.Entrant entrant with(nolock, fastfirstrow)
        inner join dbo.Organization organization
          on organization.Id = entrant.OwnerOrganizationId
        on check_entrant.CertificateNumber = entrant.CertificateNumber

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
  
  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login 

  set @eventCode = ''ENT_CHK''
  set @updateId = NewId()     
  
  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId
      
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByPassportForXml]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByPassportForXml]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
CREATE proc [dbo].[CheckCommonNationalExamCertificateByPassportForXml]	 	 
	 @subjectMarks nvarchar(4000) = null	-- средние оценки по предметам (через запятую, в определенном порядке)
	, @login nvarchar(255)						-- логин проверяющего
	, @ip nvarchar(255)							-- ip проверяющего
	, @passportSeria nvarchar(255) = null	  -- серия документа сертифицируемого (паспорта)
	, @passportNumber nvarchar(255) = null	  -- номер документа сертифицируемого (паспорта)
	, @shouldWriteLog BIT = 1				  -- нужно ли записывать в лог интерактивных проверок	
	, @ParticipantID uniqueidentifier = null
	,@xml xml out
as
begin 
set nocount on
	
	if 
	(@passportSeria is null OR @passportSeria = '''') and
	(@passportNumber is null OR @passportNumber = '''') and
	 @ParticipantID is null
	begin
		RAISERROR (N''Не могут быть одновременно неуказанными и номер свидетельства и типографский номер'',10,1);
		return
	end
    
	IF (@passportSeria is null)
		SET @passportSeria = ''''

	declare @eventId INT
	IF @shouldWriteLog = 1
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @subjectMarks, @passportSeria = @passportSeria, @passportNumber = @passportNumber, @IsOpenFbs = 1, @eventId = @eventId output

declare 
		@eventCode nvarchar(255)
        , @organizationId bigint
		, @editorAccountId bigint
		, @eventParams nvarchar(4000)		
		, @commandText nvarchar(4000)
		, @internalPassportSeria nvarchar(255)

	-- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

	set @eventParams = 
		isnull(@subjectMarks, '''') + ''|'' 
		+ isnull(@passportSeria, '''') + ''|'' 
		+ isnull(@passportNumber, '''') + ''|'' 

	select
		@editorAccountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login		

	if not @passportSeria is null
		set @internalPassportSeria = dbo.GetInternalPassportSeria(@passportSeria)

	set @commandText = ''''
	set @eventCode = N''CNE_FND_P''

	declare @sourceEntityIds nvarchar(4000) 
	declare @Search table 
	( 
		LastName nvarchar(255) 
		, FirstName nvarchar(255) 
		, PatronymicName nvarchar(255) 
		, CertificateId uniqueidentifier 
		, CertificateNumber nvarchar(255) 
		, RegionId int 
		, PassportSeria nvarchar(255)
		, PassportNumber nvarchar(255) 
		, TypographicNumber nvarchar(255) 
		, Year int
		, ParticipantsID uniqueidentifier
	) 
		
	set @commandText = @commandText +     	
		''select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
						certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
						isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year, ParticipantID
		from (
			select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
				   COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
				   COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
				   a.ParticipantID AS ParticipantID
            from rbd.Participants a with (nolock)				
				left join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID
				left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK 
				left join prn.CancelledCertificates c with (nolock) on c.CertificateFK=b.CertificateID 
			where 1=1		 
		'' 
	if @ParticipantID is not null 
		set @commandText = @commandText + ''	and a.ParticipantID = @ParticipantID''		
				
    if not @internalPassportSeria is null
    begin
        set @commandText = @commandText + '' and a.DocumentSeries = @internalPassportSeria ''
	end
	
	if not @passportNumber is null
    begin
        set @commandText = @commandText + '' and  a.DocumentNumber = @passportNumber ''
    end
	else
	begin
		goto nullresult
	end	
	
	set @commandText = @commandText + '' ) certificate ''
	print @commandText
	insert into @Search
	exec sp_executesql @commandText
		, N'' @internalPassportSeria nvarchar(255)
			, @passportNumber nvarchar(255), @ParticipantID uniqueidentifier ''
		,@internalPassportSeria, @passportNumber, @ParticipantID

		
/*
declare @xml xml
exec  [CheckCommonNationalExamCertificateByPassportForXml]  @login=''SuperAdmin@sibmail.com'',@ip=''SuperAdmin@sibmail.com'',@passportSeria=''9205'',@passportNumber =''527439'',@subjectMarks='''',@xml=@xml out
select @xml 
*/
	

	if @subjectMarks is not null
	begin
		
	if exists(
		select * from
		dbo.GetSubjectMarks(@subjectMarks)  t
		left join (
		select distinct b.SubjectCode SubjectId,b.Mark  from 
		@Search search
			join [prn].CertificatesMarks b with(nolock) 
			on case when search.CertificateId is null and search.ParticipantsID = b.ParticipantFK then 1
			        when search.CertificateId is not null and search.CertificateId = b.CertificateFK then 1
			        else 0 end  = 1 
			) tt
			on t.SubjectId=tt.SubjectId and t.Mark=tt.Mark
			where tt.SubjectId is null)
	delete from @search
			
	end
	
	set @sourceEntityIds = ''''
	
	select @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(4000), search.CertificateId) 
	from @Search search 
	
	set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	
	if @sourceEntityIds = ''''
		set @sourceEntityIds = null 

	-- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

	declare @Search1 table 
	( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
	)     
	insert @Search1
    select distinct S.CertificateId 
		from @Search S   
	where CertificateId is not null
	
	declare @CertificateId uniqueidentifier,@pkid int
	while exists(select * from @Search1)
	begin
	  select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @certificateIdGuid = @CertificateId 
                	  
	  delete @Search1 where pkid=@pkid
	end 

select @xml=(
	select 
	(
	select 
			isnull(cast(S.certificateId as nvarchar(250)),''Нет свидетельства'' ) certificateId,
			S.CertificateNumber CertificateNumber,
			S.LastName LastName,
			S.FirstName FirstName,
			S.PatronymicName PatronymicName,
			S.PassportSeria PassportSeria,
			S.PassportNumber PassportNumber,
			S.TypographicNumber TypographicNumber,
			region.Name RegionName, 
			case when S.CertificateId is not null or S.ParticipantsID is not null then 1 else 0 end IsExist, 
			case	when CD.UseYear is not null then 1 end IsDeny,  
			CD.Reason DenyComment, 
			null NewCertificateNumber, 
			S.Year Year,
			case 
				when ed.[ExpireDate] is null then ''Не найдено''
               	when CD.UseYear is not null then ''Аннулировано'' 
               	when getdate() <= ed.[ExpireDate] then ''Действительно'' 
                else ''Истек срок'' 
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
			CC.UniqueOtherCheck UniqueOtherCheck,
			S.ParticipantsID
		from 
				@Search S 				
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
				on CC.IdGuid  = S.certificateId 
			left outer join prn.CancelledCertificates CD with (nolock) 
				on S.CertificateId = CD.CertificateFK 
			left outer join dbo.Region region with (nolock)
				on region.[Id] = S.RegionId 
			left join [ExpireDate] ed
            	on ed.[year] = S.[year]
            for xml path(''check''), ELEMENTS XSINIL,type
	) 
	for xml path(''root''),type
	)

/*
declare @xml xml
exec  [CheckCommonNationalExamCertificateByPassportForXml]  @login=''SuperAdmin@sibmail.com'',@ip=''SuperAdmin@sibmail.com'',@passportSeria=''9205'',@passportNumber =''527439'',@subjectMarks='''',@xml=@xml out
select @xml
*/
		
goto result	
nullresult:
	select @xml=(
	select null 
	for xml path(''root''),type
	)
result:

		--select * from @Search
		
		-- записать в лог интерактивных проверок
		IF EXISTS (SELECT * FROM @Search) AND @shouldWriteLog = 1
		BEGIN
			UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 isnull(cast(certificateId as nvarchar(510)),''Нет свидетельства'' ) FROM @Search where ParticipantsID is not null)
			 WHERE Id=@eventId
		END
		

	exec dbo.RegisterEvent 
			@accountId = @editorAccountId, 
			@ip = @ip, 
			@eventCode = @eventCode, 
			@sourceEntityIds = @sourceEntityIds, 
			@eventParams = @eventParams, 
			@updateId = null 		
	
	return 0
end' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByPassport_2]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByPassport_2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[CheckCommonNationalExamCertificateByPassport_2]
	@passportSeria nvarchar(255) = null,			-- серия документа сертифицируемого (паспорта)
	@passportNumber nvarchar(255),					-- номер документа сертифицируемого (паспорта)
	@lastName nvarchar(255) = null,					-- фамилия сертифицируемого
	@firstName nvarchar(255) = null,				-- имя сертифицируемого
	@patronymicName nvarchar(255) = null,			-- отчетсво сертифицируемого
	@login nvarchar(255),							-- логин проверяющего
	@ip nvarchar(255)								-- ip проверяющего
with recompile
as
	set nocount on

declare 
	@yearFrom int,
	@yearTo int
select 
	@yearFrom = 2008,
	@yearTo = Year(GetDate())
  
  
create table #certificate_check
(
	Number nvarchar(18) not null,
	CertificateId uniqueidentifier not null,
	IsDeny bit not null,
	DenyComment nvarchar(255) null,
	[Year] int not null,
	TypographicNumber nvarchar(12) null
)


declare @sql nvarchar(max)
set @sql=
N''insert into #certificate_check(
	Number,
	certificateId,
	IsDeny,
	[Year],
	TypographicNumber)
select  
	C.LicenseNumber,
	C.CertificateID, 
	C.Cancelled,
	C.UseYear,
	C.TypographicNumber
from rbd.Participants P with (nolock)       
	inner join prn.Certificates C with (nolock) on P.ParticipantID=C.ParticipantFK
where P.DocumentNumber = @passportNumber and
	P.Surname collate cyrillic_general_ci_ai = @checkLastName and
	P.Name collate cyrillic_general_ci_ai = @checkFirstName and
''

	if @passportSeria is not null
		set @sql = @sql+
N''	P.DocumentSeries = @passportSeria and
''

	if @patronymicName is not null
		set @sql = @sql+
N''	P.SecondName collate cyrillic_general_ci_ai = @checkPatronymicName and
''
	set @sql = @sql+
N''	C.UseYear between @yearFrom and @yearTo
''


exec sp_executesql @sql,N''@passportSeria nvarchar(255),@passportNumber nvarchar(20),@yearFrom int,@yearTo int,
@checkLastName nvarchar(255),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255)'',
	@passportSeria,@passportNumber,@yearFrom,@yearTo,@lastName,@firstName,@patronymicName

if exists(select 1 from #certificate_check where IsDeny=1)
	update #certificate_check
		set DenyComment=CC.Reason
	from #certificate_check C inner join prn.CancelledCertificates CC with (nolock) on C.CertificateID=CC.CertificateFK



--Выполняем подсчет уникальных проверок 
--Значение 0 означает, что организация не найдена или не задана
declare 
	@accountId bigint,
	@organizationId bigint

set @organizationId = 0

select
	@accountId = Id,
	@organizationId = ISNULL(OrganizationId,0)
from dbo.Account with (nolock, fastfirstrow)
where [Login] = @login

declare @CId uniqueidentifier
	
declare db_cursor cursor for
    select distinct certificateId from #certificate_check
    
open db_cursor   
fetch next from db_cursor INTO @CId   

while @@FETCH_STATUS = 0   
begin
    exec dbo.ExecuteChecksCount
        @OrganizationId = @organizationId,
        @certificateIdGuid = @CId
        
    fetch next from db_cursor into @CId
end
    
close db_cursor   
deallocate db_cursor
-------------------------


--итоговый Select
set @sql=
N''select  
	--CM.CertificateFK CertificateId,
	isnull(cast(C.certificateId as nvarchar(255)),''''Нет свидетельства'''') CertificateId,
	C.Number,
	P.Surname LastName,
	P.Name FirstName,
	P.SecondName PatronymicName,
	S.SubjectName,
	CM.Mark SubjectMark,
	CM.HasAppeal,
	C.IsDeny,
	C.DenyComment,
	null DenyNewcertificateNumber,
	P.DocumentSeries PassportSeria,
	P.DocumentNumber PassportNumber,
	COALESCE(C.Year,CM.UseYear) Year,
	C.TypographicNumber,
	case
		when C.IsDeny=1 then ''''Аннулировано''''
		when ed.ExpireDate is null then ''''Не найдено''''
		when getdate() <= ed.ExpireDate then ''''Действительно''''
		else ''''Истек срок''''
	end Status,
	isnull(EC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck
from rbd.Participants P
    inner join [prn].CertificatesMarks CM with (nolock) on P.ParticipantID = CM.ParticipantFK
	inner join dat.Subjects S with (nolock) on CM.SubjectCode = S.SubjectCode
	left join #certificate_check C on CM.CertificateFK=C.CertificateId
    left join ExamcertificateUniqueChecks EC with (nolock) on EC.IdGuid = CM.CertificateFK
	left join ExpireDate as ed with (nolock) on CM.UseYear = ed.Year          
where P.DocumentNumber = @passportNumber and
	P.Surname collate cyrillic_general_ci_ai = @checkLastName and
	P.Name collate cyrillic_general_ci_ai = @checkFirstName and
''

	if @passportSeria is not null
		set @sql = @sql+
N''	P.DocumentSeries = @passportSeria and
''

	if @patronymicName is not null
		set @sql = @sql+
N''	P.SecondName collate cyrillic_general_ci_ai = @checkPatronymicName and
''
	set @sql = @sql+
N''	CM.UseYear between @yearFrom and @yearTo
''

exec sp_executesql @sql,N''@passportSeria nvarchar(255),@passportNumber nvarchar(20),@yearFrom int,@yearTo int,
@checkLastName nvarchar(255),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255)'',
	@passportSeria,@passportNumber,@yearFrom,@yearTo,@lastName,@firstName,@patronymicName


--запись в log
declare
	@eventCode nvarchar(255),
	@eventParams nvarchar(4000),
	@sourceEntityIds nvarchar(4000) 


set @eventParams = 
    isnull(@lastName,'''') + ''|'' 
    + isnull(@firstName,'''') + ''|'' 
    + isnull(@patronymicName,'''') + ''|'' 
    + ''|'' 
    + isnull(@passportSeria, '''') + ''|'' 
    + isnull(@passportNumber, '''') + ''|'' 


set @sourceEntityIds = '''' 

select @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(100), certificate_check.certificateId) 
from #certificate_check certificate_check 

drop table #certificate_check

set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	
if @sourceEntityIds = '''' 
	set @sourceEntityIds = null 


	set @eventCode = N''CNE_FND_P''

    exec dbo.RegisterEvent 
      @accountId
      , @ip
      , @eventCode
      , @sourceEntityIds
      , @eventParams
      , @updateId = null
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumberForXml]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByNumberForXml]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[CheckCommonNationalExamCertificateByNumberForXml]
    @number nvarchar(255) = null        -- номер сертификата  
  , @checkSubjectMarks nvarchar(4000) = null  -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @shouldCheckMarks BIT = 1                 -- нужно ли проверять оценки
  , @ParticipantID uniqueidentifier = null
  , @xml xml out
as
begin 
  
  if @number is null and @ParticipantID is null
  begin
    RAISERROR (N''Номер св-ва не указан'',10,1);
    return
  end

  declare @eventId int

  if @shouldCheckMarks = 1
    exec AddCNEWebUICheckEvent @AccountLogin = @login, @RawMarks = @checkSubjectMarks, @CNENumber = @number, @IsOpenFbs = 1, @eventId = @eventId output
  
  
  declare 
    @commandText nvarchar(max)
    , @declareCommandText nvarchar(max)
    , @selectCommandText nvarchar(max)
    , @baseName nvarchar(255)
    , @yearFrom int
    , @yearTo int
    , @accountId bigint
        , @organizationId bigint
      , @CId uniqueidentifier
    , @eventCode nvarchar(255)
    , @eventParams nvarchar(4000)
    , @sourceEntityIds nvarchar(4000) 
  
  declare @check_subject table
  (
  SubjectId int
  , Mark nvarchar(10)
  )
  
  create table #certificate_check
  (
   pk int identity(1,1) primary key
  , Number nvarchar(255)
  , IsExist bit
  , certificateId uniqueidentifier
  , IsDeny bit
  , DenyComment ntext
  , DenyNewcertificateNumber nvarchar(255)
  , [Year] int
  , PassportSeria nvarchar(255)
  , PassportNumber nvarchar(255)
  , RegionId int
  , RegionName nvarchar(255)
  , TypographicNumber nvarchar(255)
  , ParticipantID uniqueidentifier
  )

  declare @ss nvarchar(max)
  
  set @ss=''create index [IX_#certificate_check_''+cast(newid() as nvarchar(200))+''] on #certificate_check (ParticipantID, [Year])'' 
  exec sp_executesql @ss
  print @ss
  set @ss=''create index [IX_#certificate_check_''+cast(newid() as nvarchar(200))+''] on #certificate_check (certificateId, [Year])'' 
  exec sp_executesql @ss
  print @ss
  
  -- Значение 0 означает, что организация не найдена или не задана
  set @organizationId = 0

  select @yearFrom = 2008, @yearTo = Year(GetDate())

  select
    @accountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  declare @sql nvarchar(max)
  
  set @sql = ''
  insert into #certificate_check 
  select
    [certificate].Number 
    , case
      when [certificate].ParticipantID is not null or [certificate].Id is not null then 1
      else 0
    end IsExist
    , [certificate].Id
    , case
      when certificate_deny.UseYear is not null then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Reason
    , null NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , [certificate].ParticipantID
  from 
    (select null ''''empty'''') t
    left join 
    (
      select distinct b.LicenseNumber AS Number, b.CertificateID AS id, 
           COALESCE(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
      from rbd.Participants a with (nolock)       
        join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID
        left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and b.[UseYear] between @yearFrom and @yearTo
      where a.[UseYear] between @yearFrom and @yearTo 
     ''
  if @ParticipantID is not null 
    set @sql = @sql + '' and a.ParticipantID = @ParticipantID''   
  if @number <> ''''  
    set @sql = @sql + '' and b.LicenseNumber = @number''
  if @ParticipantID is null and @number = '''' 
    set @sql = @sql + '' and 1=0''  
  set @sql = @sql + ''     
    ) [certificate] on 1=1
    left join dbo.Region region
      on region.Id = [certificate].RegionId
    left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] between @yearFrom and @yearTo
        and certificate_deny.CertificateFK = [certificate].id''
 
  exec sp_executesql @sql,N''@number nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier'',
							@number = @number,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID

  set @eventParams = 
    isnull(@number, '''') + ''||||'' +
    isnull(@checkSubjectMarks, '''') + ''|'' 

  set @sourceEntityIds = '''' 
  select 
    @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(100), certificate_check.certificateId) 
  from 
    #certificate_check certificate_check 
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
  if @sourceEntityIds = '''' 
    set @sourceEntityIds = null 


  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        #certificate_check S
    where
      S.certificateId is not null
    
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @certificateIdGuid = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
  -------------------------------------------------------------
  
  create table #table(
    certificateId uniqueidentifier,
    Number nvarchar(255),
    IsExist bit,
    SubjectId int,
    SubjectName nvarchar(100),
    CheckSubjectMark nvarchar(100),
    SubjectMark nvarchar(100),
    SubjectMarkIsCorrect bit,
    HasAppeal bit,
    IsDeny bit,
    DenyComment ntext,
    DenyNewcertificateNumber nvarchar(255),
    PassportSeria nvarchar(255),
    PassportNumber nvarchar(255),
    RegionId int,
    RegionName nvarchar(255),
    [Year] int,
    TypographicNumber nvarchar(255),
    [Status]  nvarchar(255),    
      UniqueChecks int,
        UniqueIHEaFCheck int,
        UniqueIHECheck int,
        UniqueIHEFCheck int,
        UniqueTSSaFCheck int,
        UniqueTSSCheck int,
        UniqueTSSFCheck int,
        UniqueRCOICheck int,
        UniqueOUOCheck int,
        UniqueFounderCheck int,
        UniqueOtherCheck int,
        ParticipantID uniqueidentifier)
      
  set @sql = ''      
  select
    certificate_check.certificateId
    ,certificate_check.Number Number
    , certificate_check.IsExist IsExist
    , check_subject.SubjectId  SubjectId
    , check_subject.Name  SubjectName
    , case when check_subject.CheckSubjectMark < check_subject.[MinimalMark] then ''''!'''' else '''''''' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),''''.'''','''','''')  CheckSubjectMark
    , case when check_subject.SubjectMark < check_subject.MinimalMark1 then ''''!'''' else '''''''' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),''''.'''','''','''')  SubjectMark
    ,check_subject.SubjectMarkIsCorrect SubjectMarkIsCorrect
    , check_subject.HasAppeal HasAppeal
    , certificate_check.IsDeny IsDeny
    , certificate_check.DenyComment DenyComment
    , certificate_check.DenyNewcertificateNumber DenyNewcertificateNumber
    , certificate_check.PassportSeria PassportSeria
    , certificate_check.PassportNumber PassportNumber
    , certificate_check.RegionId RegionId
    , certificate_check.RegionName RegionName
    , certificate_check.[Year] [Year]
    , certificate_check.TypographicNumber TypographicNumber
    , case when ed.[ExpireDate] is null then ''''Не найдено'''' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then ''''Аннулировано'''' else
      case when getdate() <= ed.[ExpireDate] then ''''Действительно''''
      else ''''Истек срок'''' end end end  as [Status],
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
        CC.UniqueOtherCheck UniqueOtherCheck,
        certificate_check.ParticipantID       
  from #certificate_check certificate_check
        left join ExamcertificateUniqueChecks CC on CC.IdGuid = certificate_check.certificateId
		left join [ExpireDate] as ed on certificate_check.[Year] = ed.[Year]          
		join (
			select      
				getcheck_subject.SubjectId id,
				[subject].Name,
				certificate_subject.UseYear [Year],
				certificate_subject.certificateFK certificateId, 
				isnull(getcheck_subject.SubjectId, [subject].SubjectId) SubjectId,
				getcheck_subject.[Mark] CheckSubjectMark,
				certificate_subject.[Mark] SubjectMark,
				case
					when getcheck_subject.Mark = certificate_subject.Mark then 1
				else 0 end SubjectMarkIsCorrect,
				certificate_subject.HasAppeal,
				mm.[MinimalMark],
				mm1.[MinimalMark] MinimalMark1,
				certificate_subject.ParticipantFK
			from [prn].CertificatesMarks certificate_subject with (nolock) 
				join #certificate_check a on a.ParticipantID=certificate_subject.ParticipantFK
				join dbo.[Subject] [subject]  on [subject].SubjectId = certificate_subject.SubjectCode  
				left join dbo.GetSubjectMarks(@checkSubjectMarks) getcheck_subject on getcheck_subject.SubjectId = [subject].subjectId
				left join [MinimalMark] as mm on getcheck_subject.SubjectId = mm.[SubjectId] and certificate_subject.UseYear = mm.[Year] 
				left join [MinimalMark] as mm1 on certificate_subject.SubjectCode = mm1.[SubjectId] and certificate_subject.UseYear = mm1.[Year] 
			) check_subject
				on certificate_check.[Year] = check_subject.[Year] and ''    
  if @ParticipantID is null   
    set @sql=@sql + '' certificate_check.certificateId = check_subject.certificateId ''
  else
    if @number <> ''''  
      set @sql=@sql + '' certificate_check.ParticipantID=check_subject.ParticipantFK and check_subject.certificateId=certificate_check.certificateId ''
    else
      set @sql=@sql + '' certificate_check.ParticipantID=check_subject.ParticipantFK and check_subject.certificateId<>certificate_check.certificateId ''
            
  print @sql 
  insert #table
  exec sp_executesql @sql,N''@checkSubjectMarks nvarchar(max)'',@checkSubjectMarks=@checkSubjectMarks     
  --select * from #table
      
IF @shouldCheckMarks = 1 AND  (exists(select * from #table where  SubjectMarkIsCorrect=0 and SubjectId IS NOT null) or (select COUNT(*) from #table where SubjectId IS NOT null)<>(select COUNT(*) from dbo.GetSubjectMarks(@checkSubjectMarks)))
  delete from #table
  --SELECT * FROM #table
  select @xml=(
  select 
  (
  select * from #table
  for xml path(''check''), ELEMENTS XSINIL,type
  ) 
  for xml path(''root''),type
  )
  
goto result 
nullresult:
  select @xml=(
  select null 
  for xml path(''root''),type
  )
result:
    -- записать в лог интерактивных проверок
    if @shouldCheckMarks = 1 and exists (select * from #table)
    UPDATE CNEWebUICheckLog SET FoundedCNEId= (SELECT TOP 1 certificateId FROM #certificate_check)
      WHERE Id=@eventId
    drop table #table
      drop table #certificate_check
    
    -- записать в лог всех проверок
    set @eventCode = ''CNE_CHK''
    exec dbo.RegisterEvent 
      @accountId
      , @ip
      , @eventCode
      , @sourceEntityIds
      , @eventParams
      , @updateId = null
  
  return 0
end' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumber_2]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByNumber_2]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE procedure [dbo].[CheckCommonNationalExamCertificateByNumber_2]
	@number nvarchar(255) = null,					-- номер сертификата
	@checkLastName nvarchar(255) = null,			-- фамилия сертифицируемого
	@checkFirstName nvarchar(255) = null,			-- имя сертифицируемого
	@checkPatronymicName nvarchar(255) = null,		-- отчетсво сертифицируемого
	@checkSubjectMarks nvarchar(max) = null,		-- средние оценки по предметам (через запятую, в определенном порядке)
	@login nvarchar(255),							-- логин проверяющего
	@ip nvarchar(255),								-- ip проверяющего
	@checkTypographicNumber nvarchar(20) = null,	-- типографический номер сертификата
	@ParticipantID uniqueidentifier = null
with recompile
as
	set nocount on

if @checkTypographicNumber is null and @number is null
begin
	RAISERROR (N''Не могут быть одновременно неуказанными и номер свидетельства и типографский номер'',10,1);
	return
end
  
    
declare 
	@yearFrom int,
	@yearTo int
  
  
create table #certificate_check
(
	Number nvarchar(18) not null,
	LastName varchar(80) not null,
	FirstName varchar(80) not null,
	PatronymicName varchar(80) null,
	CertificateId uniqueidentifier not null,
	IsDeny bit not null,
	DenyComment nvarchar(255) null,
	[Year] int not null,
	PassportSeria varchar(9) null,
	PassportNumber varchar(10) not null,
	TypographicNumber nvarchar(12) null,
	Status varchar(13) not null
)


if @checkTypographicNumber is not null
	select @yearFrom = 2009, @yearTo = Year(GetDate()) --2009-год появления типографского номера в БД
else
	select @yearFrom = 2008, @yearTo = Year(GetDate())


declare @sql nvarchar(max)
set @sql=
N''insert into #certificate_check(
	Number,
	LastName,
	FirstName,
	PatronymicName,
	certificateId,
	IsDeny,
	[Year],
	PassportSeria,
	PassportNumber,
	TypographicNumber,
	Status)
select  
	C.LicenseNumber,
	P.Surname,
	P.Name,
	P.SecondName,
	C.CertificateID, 
	C.Cancelled,
	C.UseYear,
	P.DocumentSeries,
	P.DocumentNumber,
	C.TypographicNumber,
	case
		when C.Cancelled=1 then ''''Аннулировано''''
		when ed.ExpireDate is null then ''''Не найдено''''
		when getdate() <= ed.ExpireDate then ''''Действительно''''
		else ''''Истек срок''''
	end
from rbd.Participants P with (nolock)       
	inner join prn.Certificates C with (nolock) on P.ParticipantID=C.ParticipantFK
	left join ExpireDate as ed with (nolock) on C.UseYear = ed.Year          
where
''

	if @number is not null
		set @sql = @sql+
N''		C.LicenseNumber=@number and
''

	if @checkTypographicNumber is not null
		set @sql = @sql+
N''		C.TypographicNumber=@checkTypographicNumber and
''

	set @sql = @sql+
N''	P.Surname collate cyrillic_general_ci_ai = @checkLastName and
	P.Name collate cyrillic_general_ci_ai = @checkFirstName and
''
	if @checkPatronymicName is not null
		set @sql = @sql+
N''	P.SecondName collate cyrillic_general_ci_ai = @checkPatronymicName and
''
	set @sql = @sql+
N''	C.UseYear between @yearFrom and @yearTo
''

exec sp_executesql @sql,N''@number nvarchar(255),@checkTypographicNumber nvarchar(20),@yearFrom int,@yearTo int,
@checkLastName nvarchar(255),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255)'',
	@number,@checkTypographicNumber,@yearFrom,@yearTo,@checkLastName,@checkFirstName,@checkPatronymicName

if exists(select 1 from #certificate_check where IsDeny=1)
	update #certificate_check
		set DenyComment=CC.Reason
	from #certificate_check C inner join prn.CancelledCertificates CC with (nolock) on C.CertificateID=CC.CertificateFK



--Выполняем подсчет уникальных проверок 
--Значение 0 означает, что организация не найдена или не задана
declare 
	@accountId bigint,
	@organizationId bigint

set @organizationId = 0

select
	@accountId = Id,
	@organizationId = ISNULL(OrganizationId,0)
from dbo.Account with (nolock, fastfirstrow)
where [Login] = @login

declare @CId uniqueidentifier
	
declare db_cursor cursor for
    select distinct certificateId from #certificate_check
    
open db_cursor   
fetch next from db_cursor INTO @CId   

while @@FETCH_STATUS = 0   
begin
    exec dbo.ExecuteChecksCount
        @OrganizationId = @organizationId,
        @certificateIdGuid = @CId
        
    fetch next from db_cursor into @CId
end
    
close db_cursor   
deallocate db_cursor
-------------------------

--итоговый Select
select  
	C.CertificateId,
	C.Number,
	C.LastName,
	C.FirstName,
	C.PatronymicName,
	S.SubjectName,
	CM.Mark SubjectMark,
	CM.HasAppeal,
	C.IsDeny,
	C.DenyComment,
	null DenyNewcertificateNumber,
	C.PassportSeria,
	C.PassportNumber,
	C.Year,
	C.TypographicNumber,
	C.Status,
	isnull(EC.UniqueIHEaFCheck, 0) UniqueIHEaFCheck
from #certificate_check C
    inner join [prn].CertificatesMarks CM with (nolock) on C.certificateId = CM.CertificateFK and C.Year = CM.UseYear
	inner join dat.Subjects S with (nolock) on CM.SubjectCode = S.SubjectCode
    left join ExamcertificateUniqueChecks EC with (nolock) on EC.IdGuid = C.certificateId


--запись в log
declare
	@eventCode nvarchar(255),
	@eventParams nvarchar(4000),
	@sourceEntityIds nvarchar(4000) 


set @eventParams = 
    isnull(@number, '''') + ''|'' +
    isnull(@checkLastName, '''') + ''|'' +
    isnull(@checkFirstName, '''') + ''|'' +
    isnull(@checkPatronymicName, '''') + ''|'' +
    isnull(@checkSubjectMarks, '''') + ''|'' +
    isnull(@checkTypographicNumber, '''')

set @sourceEntityIds = '''' 

select @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(100), certificate_check.certificateId) 
from #certificate_check certificate_check 

drop table #certificate_check

set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
	
if @sourceEntityIds = '''' 
	set @sourceEntityIds = null 


if @checkTypographicNumber is not null
	set @eventCode = ''CNE_CHK_TN''
else
	set @eventCode = ''CNE_CHK''

    exec dbo.RegisterEvent 
      @accountId
      , @ip
      , @eventCode
      , @sourceEntityIds
      , @eventParams
      , @updateId = null
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonNationalExamCertificateByNumber]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckCommonNationalExamCertificateByNumber]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[CheckCommonNationalExamCertificateByNumber]
   @number nvarchar(255) = null       -- номер сертификата
  , @checkLastName nvarchar(255) = null   -- фамилия сертифицируемого
  , @checkFirstName nvarchar(255) = null    -- имя сертифицируемого
  , @checkPatronymicName nvarchar(255) = null -- отчетсво сертифицируемого
  , @checkSubjectMarks nvarchar(max) = null -- средние оценки по предметам (через запятую, в определенном порядке)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @checkTypographicNumber nvarchar(20) = null -- типографический номер сертификата
  , @ParticipantID uniqueidentifier = null
  , @Year int = null
as
begin 
  if @checkTypographicNumber is null and @number is null and @ParticipantID is null
  begin
    RAISERROR (N''Не могут быть одновременно неуказанными и номер свидетельства и типографский номер'',10,1);
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
      , @CId uniqueidentifier
    , @eventCode nvarchar(255)
    , @eventParams nvarchar(4000)
    , @sourceEntityIds nvarchar(4000) 
  
  declare @check_subject table
  (
  SubjectId int
  , Mark nvarchar(10)
  )
  
  create table #certificate_check
  (
    id int primary key identity(1,1)
  , Number nvarchar(255)
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
  , certificateId uniqueidentifier
  , IsDeny bit
  , DenyComment ntext
  , DenyNewcertificateNumber nvarchar(255)
  , [Year] int
  , PassportSeria nvarchar(255)
  , PassportNumber nvarchar(255)
  , RegionId int
  , RegionName nvarchar(255)
  , TypographicNumber nvarchar(255)
  , ParticipantID uniqueidentifier
  )

  -- Значение 0 означает, что организация не найдена или не задана
    set @organizationId = 0

  if isnull(@checkTypographicNumber,'''') <> ''''
    select @yearFrom = 2009, @yearTo = Year(GetDate()) --2009-год появления типографского номера в БД
  else
    select @yearFrom = 2008, @yearTo = Year(GetDate())

  if @Year is not null
	select @yearFrom = @Year, @yearTo = @Year
	
  select
    @accountId = account.[Id],
        @organizationId = ISNULL(account.[OrganizationId], 0)
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login

  declare @sql nvarchar(max)
  
  set @sql = ''
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
    , case''
    if @ParticipantID is not null 
      set @sql = @sql + '' when certificate.ParticipantID is not null then 1''
    else
      set @sql = @sql + '' when [certificate].Id is not null then 1 ''    
    set @sql = @sql + '' else 0 
    end IsExist
    , [certificate].Id
    , case
      when certificate_deny.UseYear is not null then 1
      else 0
    end iscertificate_deny
    , certificate_deny.Reason
    , null NewcertificateNumber
    , [certificate].[Year]
    , [certificate].PassportSeria
    , [certificate].PassportNumber
    , [certificate].RegionId
    , region.Name
    , [certificate].TypographicNumber
    , certificate.ParticipantID
  from 
    (select null ''''empty'''') t 
    left join 
      (
		select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
		from rbd.Participants a with (nolock)       
			left join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID and cm.[UseYear]=a.UseYear
			left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and b.[UseYear]=a.UseYear
      where a.[UseYear] between @yearFrom and @yearTo ''
      
  if @ParticipantID is not null 
    set @sql = @sql + '' and a.ParticipantID = @ParticipantID''   
  if @number is not null 
  begin
  if @number <> ''''  
    set @sql = @sql + '' and b.LicenseNumber=@number ''
  end 
  
  if @CheckTypographicNumber is not null 
    set @sql = @sql + '' and b.TypographicNumber=@CheckTypographicNumber''      
    
  set @sql = @sql + ''    
     ) [certificate] on 1=1  ''
  if @number = ''''    
    set @sql = @sql + '' and [certificate].Number is null   ''
  set @sql = @sql + ''     
    left join dbo.Region region with (nolock) 
      on region.Id = [certificate].RegionId
    left join prn.CancelledCertificates certificate_deny with (nolock, fastfirstrow)
      on certificate_deny.[UseYear] = [certificate].[Year]
        and certificate_deny.CertificateFK = [certificate].id''

  insert into #certificate_check    

  exec sp_executesql @sql,N''@checkLastName nvarchar(255),@number nvarchar(255),@checkTypographicNumber nvarchar(20),@checkFirstName nvarchar(255),@checkPatronymicName nvarchar(255),@checkSubjectMarks nvarchar(max),@yearFrom int,@yearTo int,@ParticipantID uniqueidentifier'',@checkLastName=@checkLastName,@number = @number,@checkTypographicNumber=@checkTypographicNumber,@checkFirstName=@checkFirstName,@checkPatronymicName=@checkPatronymicName,@checkSubjectMarks=@checkSubjectMarks,@yearFrom=@yearFrom,@yearTo=@yearTo,@ParticipantID=@ParticipantID
--select * from #certificate_check    

    set @eventParams = 
    isnull(@number, '''') + ''|'' +
    isnull(@checkLastName, '''') + ''|'' +
    isnull(@checkFirstName, '''') + ''|'' +
    isnull(@checkPatronymicName, '''') + ''|'' +
    isnull(@checkSubjectMarks, '''') + ''|'' +
    isnull(@checkTypographicNumber, '''')

  set @sourceEntityIds = '''' 
  select 
    @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(100), certificate_check.certificateId) 
  from 
    #certificate_check certificate_check 
    
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
  if @sourceEntityIds = '''' 
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок
    declare db_cursor cursor for
    select
        distinct S.certificateId
    from 
        #certificate_check S
    where
      S.certificateId is not null    
    
    open db_cursor   
    fetch next from db_cursor INTO @CId   
    while @@FETCH_STATUS = 0   
    begin
        exec dbo.ExecuteChecksCount
            @OrganizationId = @organizationId,
            @certificateIdGuid = @CId
        fetch next from db_cursor into @CId
    end
        
    close db_cursor   
    deallocate db_cursor
  -------------------------------------------------------------
  
  set @sql = ''                       
  select  
    isnull(cast(certificate_check.certificateId as nvarchar(250)),''''Нет свидетельства'''' ) certificateId
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
    , case when check_subject.CheckSubjectMark < mm.[MinimalMark] then ''''!'''' else '''''''' end + replace(cast(check_subject.CheckSubjectMark as nvarchar(9)),''''.'''','''','''') CheckSubjectMark
    , case when check_subject.SubjectMark < mm.[MinimalMark] then ''''!'''' else '''''''' end + replace(cast(check_subject.SubjectMark as nvarchar(9)),''''.'''','''','''') SubjectMark
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
    , case when ed.[ExpireDate] is null then ''''Не найдено'''' else 
      case when isnull(certificate_check.isdeny,0) <> 0 then ''''Аннулировано'''' else
      case when getdate() <= ed.[ExpireDate] then ''''Действительно''''
      else ''''Истек срок'''' end end end as [Status],
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
        isnull(CC.UniqueOtherCheck, 0) UniqueOtherCheck,
        certificate_check.ParticipantID,
        check_subject.certificateId
  from #certificate_check certificate_check ''
  if @number = ''''  
    set @sql=@sql + ''    
    left join prn.Certificates C with(nolock) on C.ParticipantFK =certificate_check.ParticipantID and c.[UseYear] = ''+cast(@yearFrom as nvarchar(255)) 
    
   set @sql=@sql + ''     
        left outer join ExamcertificateUniqueChecks CC with (nolock) on CC.IdGuid = certificate_check.certificateId and cc.[Year]=certificate_check.[Year]
    left join [ExpireDate] as ed with (nolock) on certificate_check.[Year] = ed.[Year]          
    join (
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
        , isnull(check_subject.SubjectId, [subject].SubjectId) SubjectId
        , check_subject.[Mark] CheckSubjectMark
        , certificate_subject.[Mark] SubjectMark
        , case
          when check_subject.Mark = certificate_subject.Mark then 1
          else 0
        end SubjectMarkIsCorrect
        , certificate_subject.HasAppeal,
        certificate_subject.certificatefk certificateId,
        certificate_subject.ParticipantFK ParticipantID,
        certificate_subject.UseYear
      from [prn].CertificatesMarks certificate_subject with (nolock)         
        inner join dbo.[Subject] [subject] with (nolock) on certificate_subject.SubjectCode = [subject].SubjectId
        inner join #certificate_check certificate_check
          on certificate_check.[Year] = certificate_subject.UseYear ''
  if @ParticipantID is null   
    set @sql=@sql + '' and certificate_check.certificateId = certificate_subject.CertificateFK ''
  else
    set @sql=@sql + '' and certificate_subject.ParticipantFK = certificate_check.ParticipantID ''
    
  set @sql=@sql + '' 
        left join dbo.GetSubjectMarks(@checkSubjectMarks) check_subject
          on check_subject.SubjectId = [subject].SubjectId
      ) check_subject on check_subject.UseYear=certificate_check.[Year] and ''  
  if @ParticipantID is null   
    set @sql=@sql + '' certificate_check.certificateId = check_subject.certificateId ''
  else
    if @number <> ''''  
      set @sql=@sql + '' certificate_check.ParticipantID=check_subject.ParticipantID and check_subject.certificateId=certificate_check.certificateId ''
    else
      set @sql=@sql + '' certificate_check.ParticipantID=check_subject.ParticipantID
              and check_subject.certificateId <> isnull(C.CertificateID,''''2F49AD69-5852-4B65-9C98-8D5F5C861BE4'''') ''
             
  set @sql=@sql + ''
      left join dbo.[Subject] [subject] with (nolock) on check_subject.SubjectId = [subject].SubjectId
      left join [MinimalMark] as mm with (nolock) on [subject].SubjectId = mm.[SubjectId] and certificate_check.[Year] = mm.[Year] ''      

  exec sp_executesql @sql,N''@checkSubjectMarks nvarchar(max)'',@checkSubjectMarks=@checkSubjectMarks

  drop table #certificate_check 
    if @checkTypographicNumber is not null
      set @eventCode = ''CNE_CHK_TN''
    else
      set @eventCode = ''CNE_CHK''

    exec dbo.RegisterEvent 
      @accountId
      , @ip
      , @eventCode
      , @sourceEntityIds
      , @eventParams
      , @updateId = null
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckAccountKey]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckAccountKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.CheckAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
CREATE procedure [dbo].[CheckAccountKey]
  @key nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare
    @now datetime
    , @entityParams nvarchar(1000)
    , @sourceEntityIds nvarchar(255)
    , @accountId bigint
    , @isValid bit
    , @year int
    , @login nvarchar(255)

  set @now = convert(nvarchar(8), GetDate(), 112)
  set @year = Year(GetDate())

  select top 1
    @accountId = account.Id
    , @login = account.Login
  from dbo.Account account
    inner join dbo.AccountKey account_key
      on account.Id = account_key.AccountId
  where
    account_key.[Key] = @key
    and account_key.IsActive = 1
    and @now between isnull(account_key.DateFrom, @now) and isnull(account_key.DateTo, @now)
    and ((account.Id in (select group_account.AccountId
        from dbo.GroupAccount group_account
          inner join dbo.[Group] [group]
            on [group].Id = group_account.GroupId
        where [group].Code = ''User'')
        and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
            , account.RegistrationDocument) = ''activated'')
      or (account.Id in (select group_account.AccountId
        from dbo.GroupAccount group_account
          inner join dbo.[Group] [group]
            on [group].Id = group_account.GroupId
        where [group].Code = ''Administrator'')
        and account.IsActive = 1))

  if not @login is null
    set @isValid = 1
  else
    set @isValid = 0
    
  select
    @key [Key]
    , @login [Login]
    , @isValid IsValid

  set @entityParams = @key + N''|'' +
      convert(nvarchar, @isValid)

  set @sourceEntityIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = N''USR_KEY_VERIFY''
    , @sourceEntityIds = @sourceEntityIds
    , @eventParams = @entityParams
    , @updateId = null

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[VerifyAccount]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VerifyAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[VerifyAccount]
  @login nvarchar(255)
  , @ip nvarchar(255)
AS
BEGIN

  DECLARE @isLoginValid bit
    , @isIpValid bit
    , @accountId bigint
    , @entityParams nvarchar(1000)
    , @sourceEntityIds nvarchar(255)
  
  SELECT @isLoginValid = 0, @isIpValid = 0

  SELECT @accountId = [Id], 
      @isLoginValid = 
        CASE 
          WHEN [Status] <> ''deactivated'' 
          THEN 1 
          ELSE 0 
        END 
  FROM dbo.Account with (nolock)
  WHERE [Login] = @login

  -- IP не проверяем - он валидный при валидном пароле
  SET @isIpValid=@isLoginValid

  SET @entityParams = @login + N''|'' + @ip + N''|'' +
      CONVERT(nvarchar, @isLoginValid)  + ''|'' +
      CONVERT(nvarchar, @isIpValid)

  SET @sourceEntityIds = CONVERT(nvarchar(255), ISNULL(@accountId,0))

  SELECT @login [Login], @ip Ip, @isLoginValid IsLoginValid, @isIpValid IsIpValid

  EXEC dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = N''USR_VERIFY''
    , @sourceEntityIds = @sourceEntityIds
    , @eventParams = @entityParams
    , @updateId = null

  RETURN 0
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccount]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateAccount
-- =============================================
-- Сохранить учетную запись.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.3: Modified by Makarev Andrey 16.04.2008
-- Измение процедуры GetDelimitedValues().
-- v.1.4: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.5: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- =============================================
CREATE procedure [dbo].[UpdateAccount]
  @login nvarchar(255)
  , @passwordHash nvarchar(255) = null
  , @lastName nvarchar(255)
  , @firstName nvarchar(255)
  , @patronymicName nvarchar(255)
  , @phone nvarchar(255)
  , @email nvarchar(255)
  , @isActive bit
  , @ipAddresses nvarchar(4000) = null
  , @groupCode nvarchar(255) = null
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
  , @hasFixedIp bit = null
as
begin
  declare @exists table([login] nvarchar(255), isExists bit)

  insert @exists exec dbo.CheckNewLogin @login = @login
  
  declare 
    @isExists bit
    , @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @accountId bigint
    , @status nvarchar(255)
    , @innerStatus nvarchar(255)
    , @confirmYear int
    , @currentYear int
    , @userGroupId int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()

  select @userGroupId = [group].[Id]
  from dbo.[Group] [group] with (nolock, fastfirstrow)
  where [group].[Code] = @groupCode

  select @isExists = user_exists.isExists
  from  @exists user_exists

  select @editorAccountId = account.[Id]
  from  dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @editorLogin

  select @accountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @login

  set @currentYear = year(getdate())

  set @confirmYear = @currentYear

  declare @oldIpAddress table (ip nvarchar(255))

  declare @newIpAddress table (ip nvarchar(255))

--если логина нет - добавляем запись и добавляем пользователя в группу
--если логин есть - меняем данные
  if @isExists = 0  -- внесение нового пользователя
  begin
    select 
      @status = case when @groupCode=''User'' then  null else ''activated'' end,
      @hasFixedIp = isnull(@hasFixedIp, 1), @eventCode = N''USR_REG''

    select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
  end
  else
  begin -- update существующего пользователя
    select 
      @accountId = account.[Id]
      , @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
    from 
      dbo.Account account with (nolock, fastfirstrow)
    where
      account.[Login] = @login

    insert @oldIpAddress
      (
      ip
      )
    select
      account_ip.Ip
    from
      dbo.AccountIp account_ip with (nolock, fastfirstrow)
    where
      account_ip.AccountId = @accountId

    set @eventCode = N''USR_EDIT''
  end

  if @hasFixedIp = 1
    insert @newIpAddress
      (
      ip
      )
    select 
      ip_addresses.[value]
    from 
      dbo.GetDelimitedValues(@ipAddresses) ip_addresses

  begin tran insert_update_account_tran

    if @isExists = 0  -- внесение нового пользователя
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
        , Email
        , RegistrationDocument
        , AdminComment
        , IsActive
        , Status
        , IpAddresses
        , HasFixedIp
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @login
        , @passwordHash
        , @lastName
        , @firstName
        , @patronymicName
        , null
        , 0
        , @confirmYear
        , @phone
        , @email
        , null
        , null
        , @isActive
        , @status
        , @ipAddresses
        , @hasFixedIp

      if (@@error <> 0)
        goto undo

      select @accountId = scope_identity()

      if (@@error <> 0)
        goto undo

      insert dbo.AccountIp
        (
        AccountId
        , Ip
        )
      select
        @accountId
        , new_ip_address.ip
      from 
        @newIpAddress new_ip_address

      if (@@error <> 0)
        goto undo

      insert dbo.GroupAccount
        (
        GroupId
        , AccountID
        )
      select
        @userGroupId
        , @accountId

      if (@@error <> 0)
        goto undo

    end
    else
    begin -- update существующего пользователя
      update account
      set
        UpdateDate = getdate()
        , UpdateId = @updateId
        , EditorAccountID = @editorAccountId
        , EditorIp = @editorIp
        , LastName = @lastName
        , FirstName = @firstName
        , PatronymicName = @patronymicName 
        , phone = @phone
        , email = @email
        , IsActive = @isActive
        , IpAddresses = @ipAddresses
        , HasFixedIp = @hasFixedIp
      from
        dbo.Account account with (rowlock)
      where
        account.[Id] = @accountId

      if (@@error <> 0)
        goto undo

      if exists(select 
            1
          from
            @oldIpAddress old_ip_address
              full outer join @newIpAddress new_ip_address
                on old_ip_address.ip = new_ip_address.ip
          where
            old_ip_address.ip is null
            or new_ip_address.ip is null) 
      begin
        delete account_ip
        from 
          dbo.AccountIp account_ip
        where
          account_ip.AccountId = @accountId

        if (@@error <> 0)
          goto undo

        insert dbo.AccountIp
          (
          AccountId
          , Ip
          )
        select
          @accountId
          , new_ip_address.ip
        from 
          @newIpAddress new_ip_address

        if (@@error <> 0)
          goto undo
      end
    end

  if @@trancount > 0
    commit tran insert_update_account_tran

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

  rollback tran insert_update_account_tran

  return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountStatus]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccountStatus]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateUserAccountStatus

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
CREATE proc [dbo].[UpdateUserAccountStatus]
  @login nvarchar(255)
  , @status nvarchar(255)
  , @adminComment ntext 
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare
    @isActive bit
    , @eventCode nvarchar(255)
    , @accountId bigint
    , @editorAccountId bigint
    , @currentYear int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()
  set @eventCode = N''USR_STATE''
  set @currentYear = Year(GetDate())
  
  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  select
    @accountId = account.[Id]
    , @status = dbo.GetUserStatus(@currentYear, @status, @currentYear, 
        account.RegistrationDocument)
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  update account
  set 
    Status = @status
    , AdminComment = case
      when dbo.HasUserAccountAdminComment(@status) = 0 then null
      else @adminComment
    end
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
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountRegistrationDocument]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccountRegistrationDocument]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateUserAccountRegistrationDocument

-- =============================================
-- Изменить регистрационный документ пользователя.
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.5: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены Status output-параметр.
-- =============================================
CREATE proc [dbo].[UpdateUserAccountRegistrationDocument]
  @login nvarchar(255)
  , @registrationDocument image
  , @registrationDocumentContentType nvarchar(255)
  , @status nvarchar(255) output
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare
    @accountId bigint
    , @editorAccountId bigint
    , @currentYear int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()

  set @currentYear = Year(GetDate())

  select
    @accountId = a.[Id]
    , @status = dbo.GetUserStatus(a.ConfirmYear, isnull(@status, a.Status), @currentYear, @registrationDocument)
  from 
    dbo.Account a with (nolock, fastfirstrow)
  where 
    a.[Login] = @login

  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin

  update account
  set
    UpdateDate = GetDate()
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , RegistrationDocument = @registrationDocument
    , RegistrationDocumentContentType = @registrationDocumentContentType
    , [Status] = @status
  from 
    dbo.Account account with (rowlock)
  where 
    account.[Id] = @accountId

  exec dbo.RefreshRoleActivity @accountId = @accountId

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = N''USR_EDIT''
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  if @registrationDocument is not null
  begin
    RAISERROR (N''
    Загружена новая заявка на регистрацию:
    Пользователь: %s (https://www.fbsege.ru/Administration/Accounts/Users/View.aspx?login=%s)
    

    ----------------------------------------
    Данное письмо не является сообщением об ошибке, а служит для оповещения операторов.
    '', 7, 2, @login, @login) with log
  end

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccount]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUserAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Modified 04.05.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
CREATE procEDURE [dbo].[UpdateUserAccount]
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
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)
    , @useOnlyDocumentParam bit

  set @updateId = newid()
  
  declare @groupCode nvarchar(255)
  set @groupCode = 
    case @organizationTypeId 
       when 6 then N''UserDepartment''
       when 4 then N''Auditor''
       when 3 then N''UserRCOI''
       else N''User''
    end
  
  select  top 1 @userGroupId = [group].[Id]
  from dbo.[Group] [group] with (nolock, fastfirstrow)
  where [group].[code] = @groupCode
  
  declare @oldIpAddress table (ip nvarchar(255))
  declare @newIpAddress table (ip nvarchar(255))

  set @currentYear = year(getdate())
  set @departmentOwnershipCode = null

  select @editorAccountId = account.[Id]
  from dbo.Account account with (nolock, fastfirstrow)
  where account.[Login] = @editorLogin

  
  if isnull(@login, '''') = ''''
  begin 
    set @useOnlyDocumentParam = 1
    set @eventCode = N''USR_REG''
  end
  else
  begin
    set @useOnlyDocumentParam = 0
    set @eventCode = N''USR_EDIT''
  end

  if isnull(@login, '''') = ''''
    select top 1 @login = account.login
    from dbo.Account account with (nolock)
    where account.email = @email
      and dbo.GetUserStatus(@currentYear, 
        account.Status, account.ConfirmYear, account.RegistrationDocument) = ''registration''
    order by account.UpdateDate desc

  if isnull(@login, '''') = '''' -- внесение нового пользователя
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
      select  @accountId, new_ip_address.ip
      from @newIpAddress new_ip_address

      if (@@error <> 0)
        goto undo

      insert dbo.GroupAccount(GroupId, AccountID)
      select  @UserGroupId, @accountId

      if (@@error <> 0)
        goto undo
    end 
    else 
    begin -- update существующего пользователя
      if @isOrganizationOwner = 1
--        update organization
--        set 
--          UpdateDate = GetDate()
--          , UpdateId = @updateId
--          , EditorAccountId = @editorAccountId
--          , EditorIp = @editorIp
--          , RegionId = @organizationRegionId
--          , DepartmentOwnershipCode = @departmentOwnershipCode
--          , [Name] = @organizationFullName
--          , FounderName = @organizationFounderName
--          , Address = @organizationLawAddress
--          , ChiefName = @organizationDirName
--          , Fax = @organizationFax
--          , Phone = @organizationPhone
--          , ShortName = dbo.GetShortOrganizationName(@organizationFullName)
--          , EducationInstitutionTypeId = @organizationTypeId
--          , EtalonOrgId=@ExistingOrgId
--        from 
--          dbo.Organization organization with (rowlock)
--        where
--          organization.[Id] = @organizationId

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

      if exists(  select 1 
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
  if isnull(@password, '''') <> '''' 
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
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateSchoolLeavingCertificateCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSchoolLeavingCertificateCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateSchoolLeavingCertificateCheckBatch
-- ====================================================
-- Сохранение пакета проверки аттестатов в БД
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ====================================================
CREATE procedure [dbo].[UpdateSchoolLeavingCertificateCheckBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N''SLC_BCH_CHK''

  begin tran insert_check_batch_tran

    insert dbo.SchoolLeavingCertificateCheckBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_check_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_check_batch_tran

  return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateNews]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateNews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateNews

-- =============================================
-- Сохранение изменений новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE proc [dbo].[UpdateNews]
  @id bigint output
  , @date datetime
  , @name nvarchar(255)
  , @description ntext
  , @text ntext
  , @isActive bit
  , @editorLogin nvarchar(255) = null
  , @editorIp nvarchar(255) = null
as
begin
  declare 
    @newNews bit
    , @currentDate datetime
    , @editorAccountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @oldIsActive bit
    , @public bit
    , @publicEventCode nvarchar(100)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  if isnull(@id, 0) = 0 -- новая новость
  begin
    set @newNews = 1
    set @eventCode = N''NWS_CREATE''
  end
  else
  begin -- update существующего документа
    set @internalId = dbo.GetInternalId(@id)

    select 
      @oldIsActive = news.IsActive
    from 
      dbo.News news with (nolock, fastfirstrow)
    where
      news.[Id] = @internalId

    set @eventCode = N''NWS_EDIT''

    if @oldIsActive <> @isActive
    begin
      set @public = 1
      if @isActive = 1
        set @publicEventCode = N''DOC_PUBLIC''
      else
        set @publicEventCode = N''DOC_UNPUBLIC''
    end
  end

  begin tran insert_update_news_tran

    if @newNews = 1 -- новая новость
    begin
      insert dbo.News
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , Date
        , [Name]
        , Description
        , [Text]
        , IsActive
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @date
        , @name
        , @description
        , @text
        , @isActive

      if (@@error <> 0)
        goto undo

      set @internalId = scope_identity()
      set @id = dbo.GetExternalId(@internalId)

      if (@@error <> 0)
        goto undo
    end 
    else 
    begin -- update существующей новости

      update news
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , EditorIp = @editorIp
        , Date = @date
        , [Name] = @name
        , Description = @description
        , [Text] = @text
        , IsActive = @isActive
      from
        dbo.News news with (rowlock)
      where
        news.[Id] = @internalId

      if (@@error <> 0)
        goto undo
    end 

  if @@trancount > 0
    commit tran insert_update_news_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  if @public = 1
    exec dbo.RegisterEvent 
      @accountId = @editorAccountId
      , @ip = @editorIp
      , @eventCode = @publicEventCode
      , @sourceEntityIds = @ids
      , @eventParams = null
      , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_news_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrantRenunciation]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntrantRenunciation]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
 перед запуском ХП должна быть создана временнная таблица
  create table #EntrantRenunciation
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    )
*/
-- exec dbo.UpdateEntrantRenunciation
-- =======================================================
-- Добавляет данные в dbo.EntrantRenunciation.
-- v.1.0: Created by Makarev Andrey 26.06.2008
-- v.1.1: Modified by Fomin Dmitriy 27.06.2008
-- Поле Year - текущий год.
-- ========================================================
CREATE procedure [dbo].[UpdateEntrantRenunciation]
  @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @organizationId bigint
    , @year int

  set @updateId = NewId()
  set @currentDate = GetDate()
  set @eventCode = ''ENT_REN_EDIT''
  set @year = Year(GetDate())
  
  select
    @accountId = account.[Id]
    , @organizationId = account.OrganizationId
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if not exists(select 1 
      from dbo.Organization organization
      where organization.Id = @organizationId)
    return 0 

  declare @EntrantRenunciation table
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    , EntrantRenunciationId bigint
    )

  insert @EntrantRenunciation
  select 
    entrant_renunciation.LastName 
    , entrant_renunciation.FirstName 
    , entrant_renunciation.PatronymicName
    , entrant_renunciation.PassportNumber
    , entrant_renunciation.PassportSeria
    , old_entrant_renunciation.Id
  from (select distinct
      isnull(entrant_renunciation.LastName, '''') LastName 
      , isnull(entrant_renunciation.FirstName, '''') FirstName 
      , isnull(entrant_renunciation.PatronymicName, '''') PatronymicName
      , isnull(entrant_renunciation.PassportNumber, '''') PassportNumber
      , isnull(entrant_renunciation.PassportSeria, '''') PassportSeria
    from #EntrantRenunciation entrant_renunciation) entrant_renunciation
    left outer join dbo.EntrantRenunciation old_entrant_renunciation
      on old_entrant_renunciation.[Year] = @year
        and old_entrant_renunciation.OwnerOrganizationId = @organizationId
        and old_entrant_renunciation.PassportNumber = entrant_renunciation.PassportNumber
        and old_entrant_renunciation.PassportSeria = entrant_renunciation.PassportSeria

  begin tran update_entrant_renunciation_tran

    insert dbo.EntrantRenunciation
      (
      CreateDate
      , UpdateDate
      , UpdateId
      , EditorAccountId
      , EditorIp
      , OwnerOrganizationId
      , [Year]
      , LastName
      , FirstName
      , PatronymicName
      , PassportNumber
      , PassportSeria
      )
    select
      @currentDate
      , @currentDate
      , @updateId
      , @accountId
      , @ip
      , @organizationId
      , @year
      , entrant_renunciation.LastName
      , entrant_renunciation.FirstName
      , entrant_renunciation.PatronymicName
      , entrant_renunciation.PassportNumber
      , entrant_renunciation.PassportSeria
    from
      @EntrantRenunciation entrant_renunciation
    where
      entrant_renunciation.EntrantRenunciationId is null

    if (@@error <> 0)
      goto undo

    update old_entrant_renunciation
    set
      UpdateDate = @currentDate
      , UpdateId = @updateId
      , EditorAccountId = @accountId
      , EditorIp = @ip
      , LastName = entrant_renunciation.LastName
      , FirstName = entrant_renunciation.FirstName
      , PatronymicName = entrant_renunciation.PatronymicName
    from
      dbo.EntrantRenunciation old_entrant_renunciation
        inner join @EntrantRenunciation entrant_renunciation
          on entrant_renunciation.EntrantRenunciationId = old_entrant_renunciation.Id
            and (old_entrant_renunciation.LastName <> entrant_renunciation.LastName
              or old_entrant_renunciation.FirstName <> entrant_renunciation.FirstName
              or old_entrant_renunciation.PatronymicName <> entrant_renunciation.PatronymicName)
    
    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran update_entrant_renunciation_tran

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran update_entrant_renunciation_tran

  return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrantCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntrantCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateEntrantCheckBatch
-- =========================================
-- Сохранение пакета в БД
-- v.1.0: Created by Sedov Anton 08.07.2008
-- =========================================
CREATE procedure [dbo].[UpdateEntrantCheckBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N''ENT_BCH_CHK''

  begin tran insert_check_batch_tran

    insert dbo.EntrantCheckBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_check_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_check_batch_tran

  return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateEntrant]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateEntrant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateEntrant

/*
Перед запуском ХП должна быть создана временнная таблица
  create table #Entrant 
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , CertificateNumber nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    , GIFOCategoryName nvarchar(255)
    , DirectionCode nvarchar(255)
    , SpecialtyCode nvarchar(255)
    )
*/
-- =======================================================
-- Добавляет данные в dbo.Entrant.
-- v.1.0: Created by Makarev Andrey 26.06.2008
-- v.1.1: Modified by Fomin Dmitriy 27.06.2008
-- Поле Year - текущий год.
-- ========================================================
CREATE procedure [dbo].[UpdateEntrant]
  @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @organizationId bigint
    , @year int

  set @updateId = NewId()
  set @currentDate = GetDate()
  set @eventCode = ''ENT_EDIT''
  set @year = Year(GetDate())
  
  select
    @accountId = account.[Id]
    , @organizationId = account.OrganizationId
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  if not exists(select 1 
      from dbo.Organization organization
      where organization.Id = @organizationId)
    return 0 

  declare @Entrant table
    (
    LastName nvarchar(255)
    , FirstName nvarchar(255)
    , PatronymicName nvarchar(255)
    , CertificateNumber nvarchar(255)
    , PassportNumber nvarchar(255)
    , PassportSeria nvarchar(255)
    , GIFOCategoryName nvarchar(255)
    , DirectionCode nvarchar(255)
    , SpecialtyCode nvarchar(255)
    , EntrantId bigint
    )

  insert @Entrant
  select 
    entrant.LastName
    , entrant.FirstName
    , entrant.PatronymicName
    , entrant.CertificateNumber
    , entrant.PassportNumber
    , entrant.PassportSeria
    , entrant.GIFOCategoryName
    , entrant.DirectionCode
    , entrant.SpecialtyCode
    , old_entrant.Id
  from
    (select distinct
      isnull(entrant.LastName, '''') LastName
      , isnull(entrant.FirstName, '''') FirstName
      , isnull(entrant.PatronymicName, '''') PatronymicName
      , entrant.CertificateNumber CertificateNumber
      , entrant.PassportNumber PassportNumber
      , isnull(entrant.PassportSeria, '''') PassportSeria
      , entrant.GIFOCategoryName GIFOCategoryName
      , isnull(entrant.DirectionCode, '''') DirectionCode
      , isnull(entrant.SpecialtyCode, '''') SpecialtyCode
    from #Entrant entrant) entrant
      left outer join dbo.Entrant old_entrant
        on old_entrant.[Year] = @year
          and old_entrant.OwnerOrganizationId = @organizationId
          and (old_entrant.CertificateNumber = entrant.CertificateNumber
            or (old_entrant.CertificateNumber is null
              and entrant.CertificateNumber is null
              and old_entrant.PassportNumber = entrant.PassportNumber
              and old_entrant.PassportSeria = entrant.PassportSeria))

  begin tran update_entrant_tran

    insert dbo.Entrant
      (
      CreateDate
      , UpdateDate
      , UpdateId
      , EditorAccountId
      , EditorIp
      , OwnerOrganizationId
      , [Year]
      , LastName
      , FirstName
      , PatronymicName
      , CertificateNumber
      , PassportNumber
      , PassportSeria
      , GIFOCategoryName
      , DirectionCode
      , SpecialtyCode
      )
    select
      @currentDate
      , @currentDate
      , @updateId
      , @accountId
      , @ip
      , @organizationId
      , @year
      , entrant.LastName
      , entrant.FirstName
      , entrant.PatronymicName
      , entrant.CertificateNumber
      , entrant.PassportNumber
      , entrant.PassportSeria
      , entrant.GIFOCategoryName
      , entrant.DirectionCode
      , entrant.SpecialtyCode
    from
      @Entrant entrant
    where
      entrant.EntrantId is null

    if (@@error <> 0)
      goto undo

    update old_entrant
    set
      UpdateDate = @currentDate
      , UpdateId = @updateId
      , EditorAccountId = @accountId
      , EditorIp = @ip
      , LastName = entrant.LastName
      , FirstName = entrant.FirstName
      , PatronymicName = entrant.PatronymicName
      , PassportNumber = entrant.PassportNumber
      , PassportSeria = entrant.PassportSeria
      , GIFOCategoryName = entrant.GIFOCategoryName
      , DirectionCode = entrant.DirectionCode
      , SpecialtyCode = entrant.SpecialtyCode
    from
      dbo.Entrant old_entrant
        inner join @Entrant entrant
          on entrant.EntrantId = old_entrant.Id
            and (old_entrant.PassportNumber <> entrant.PassportNumber
                or old_entrant.PassportSeria <> entrant.PassportSeria
                or old_entrant.LastName <> entrant.LastName
                or old_entrant.FirstName <> entrant.FirstName
                or old_entrant.PatronymicName <> entrant.PatronymicName
                or isnull(old_entrant.GIFOCategoryName, '''') <> isnull(entrant.GIFOCategoryName, '''')
                or old_entrant.DirectionCode <> entrant.DirectionCode
                or old_entrant.SpecialtyCode <> entrant.SpecialtyCode)
    
    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran update_entrant_tran

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran update_entrant_tran

  return 1
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateDocument]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDocument]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateDocument

-- =============================================
-- Сохранение изменений документа.
-- v.1.0: Created by Makarev Andrey 18.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Передаются не ИД контекстов, а коды.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
CREATE proc [dbo].[UpdateDocument]
  @id bigint output
  , @name nvarchar(255)
  , @description ntext
  , @content image
  , @contentSize int
  , @contentType nvarchar(255)
  , @isActive bit
  , @contextCodes nvarchar(4000)
  , @relativeUrl nvarchar(255)
  , @editorLogin nvarchar(255) = null
  , @editorIp nvarchar(255) = null
as
begin
  declare 
    @newDocument bit
    , @currentDate datetime
    , @editorAccountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @activateDate datetime
    , @oldIsActive bit
    , @public bit
    , @publicEventCode nvarchar(100)
    , @internalId bigint

  set @updateId = newid()
  
  declare @oldContextId table 
    (
    ContextId int
    )

  declare @newContextId table 
    (
    ContextId int
    )

  insert @newContextId
  select 
    context.Id
  from 
    dbo.GetDelimitedValues(@contextCodes) codes
      inner join dbo.Context context
        on context.Code = codes.[value]

  set @currentDate = getdate()

  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  if isnull(@id, 0) = 0 -- новый документ
  begin
    set @newDocument = 1
    set @eventCode = N''DOC_CREATE''
    if @isActive = 1
      set @activateDate = @currentDate
  end
  else
  begin -- update существующего документа
    set @internalId = dbo.GetInternalId(@id)

    select 
      @oldIsActive = [document].IsActive
    from 
      dbo.[Document] [document] with (nolock, fastfirstrow)
    where
      [document].[Id] = @internalId

    insert @oldContextId
      (
      ContextId
      )
    select
      document_context.ContextId
    from
      dbo.DocumentContext document_context with (nolock)
    where
      document_context.DocumentId = @internalId

    set @eventCode = N''DOC_EDIT''

    if @oldIsActive <> @isActive
    begin
      set @public = 1
      if @isActive = 1
        select 
          @publicEventCode = N''DOC_PUBLIC''
          , @activateDate = @currentDate
      else
        select 
          @publicEventCode = N''DOC_UNPUBLIC''
          , @activateDate = null
    end
  end

  begin tran insert_update_document_tran

    if @newDocument = 1 -- новый документ
    begin
      insert dbo.[Document]
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Name]
        , Description
        , [Content]
        , ContentSize
        , ContentType
        , IsActive
        , ActivateDate
        , ContextCodes
        , RelativeUrl
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @name
        , @description
        , @content
        , @contentSize
        , @contentType
        , @isActive
        , @activateDate
        , @contextCodes
        , @relativeUrl

      if (@@error <> 0)
        goto undo

      set @internalId = scope_identity()
      set @id = dbo.GetExternalId(@internalId)

      if (@@error <> 0)
        goto undo

      insert dbo.DocumentContext
        (
        DocumentId
        , ContextId
        )
      select
        @internalId
        , new_context_id.ContextId
      from 
        @newContextId new_context_id

      if (@@error <> 0)
        goto undo

    end 
    else 
    begin -- update существующего документа

      update [document]
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , EditorIp = @editorIp
        , [Name] = @name
        , Description = @description
        , [Content] = @content
        , ContentSize = @contentSize
        , ContentType = @contentType
        , IsActive = @isActive
        , ActivateDate = case
            when @public = 1 then @activateDate
            else [document].ActivateDate
        end
        , ContextCodes = @contextCodes
        , RelativeUrl = @relativeUrl
      from
        dbo.[Document] [document] with (rowlock)
      where
        [document].[Id] = @internalId

      if (@@error <> 0)
        goto undo

      if exists(select 
            1
          from
            @oldContextId old_context_id
              full outer join @newContextId new_context_id
                on old_context_id.ContextId = new_context_id.ContextId
          where
            old_context_id.ContextId is null
            or new_context_id.ContextId is null) 
      begin
        delete document_context
        from 
          dbo.DocumentContext document_context
        where
          document_context.DocumentId = @internalId

        if (@@error <> 0)
          goto undo

        insert dbo.DocumentContext
          (
          DocumentId
          , ContextId
          )
        select
          @internalId
          , new_context_id.ContextId
        from 
          @newContextId new_context_id

        if (@@error <> 0)
          goto undo
      end
    end 

  if @@trancount > 0
    commit tran insert_update_document_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  if @public = 1
    exec dbo.RegisterEvent 
      @accountId = @editorAccountId
      , @ip = @editorIp
      , @eventCode = @publicEventCode
      , @sourceEntityIds = @ids
      , @eventParams = null
      , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_document_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateDelivery]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Создание или редактирование рассылки.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[UpdateDelivery]
  @id bigint output
  , @title nvarchar(255)
  , @message nvarchar(4000)
  , @deliveryDate datetime
  , @deliveryType nvarchar(20)
  , @recipientIds nvarchar(max)
  , @editorLogin nvarchar(255) 
  , @editorIp nvarchar(255)
as
begin
  declare @eventCode nvarchar(100)
  
  if isnull(@id, 0) = 0 -- новая рассылка
  begin
    insert dbo.Delivery
      (
      Title
      , [Message]
      , DeliveryDate
      , TypeCode
      )
    select
      @title
      , @message
      , @deliveryDate
      , @deliveryType

    set @id = scope_identity()
    set @eventCode= N''DLV_CREATE''
  end 
  else 
  begin -- update существующей рассылки
    update delivery
    set
      Title = @title
      , [Message] = @message
      , DeliveryDate = @deliveryDate
      , TypeCode = @deliveryType
    from
      dbo.Delivery delivery with (rowlock)
    where
      delivery.[Id] = @id
    
    set @eventCode= N''DLV_EDIT''
  end 

  --Удалим старых получателей рассылки
  delete from dbo.DeliveryRecipients where DeliveryId = @id
  
  if (@recipientIds is not null)
  begin
    --[value] - recipientCode, @internalId - Id рассылки
    insert into dbo.DeliveryRecipients select [value],@id from dbo.GetDelimitedValues(@recipientIds)
  end

  declare @editorAccountId bigint
  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  declare @updateId uniqueidentifier
  set @updateId = newid()

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @id
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCompetitionCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCompetitionCertificateRequestBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateCompetitionCertificateRequestBatch
-- =====================================================
-- Сохранение пакета в БД
-- v.1.0: Created by Sedov Anton 30.07.2008
-- v.1.1: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- ======================================================
CREATE procedure [dbo].[UpdateCompetitionCertificateRequestBatch]
  @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N''SCC_BCH_CHK''

  begin tran insert_request_batch_tran

    insert dbo.CompetitionCertificateRequestBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch

    if (@@error <> 0)
      goto undo

    set @internalId = scope_identity()
    set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_request_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_request_batch_tran

  return 1 
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateRequestBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCommonNationalExamCertificateRequestBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE proc [dbo].[UpdateCommonNationalExamCertificateRequestBatch]
    @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
  , @filter nvarchar(255)
  , @IsTypographicNumber bit
  , @year nvarchar(10)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N''CNE_BCH_FND''

  begin tran insert_request_batch_tran

    insert dbo.CommonNationalExamCertificateRequestBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      , [Filter]
      , IsTypographicNumber
      , [Year]
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch
      , @filter
      , @IsTypographicNumber
      , @year

    if (@@error <> 0)
      goto undo

	SET @id = scope_identity()
    --set @internalId = scope_identity()
    --set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_request_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_request_batch_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateCheckBatch]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateCommonNationalExamCertificateCheckBatch]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch

-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE PROC [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
    @id bigint output
  , @login nvarchar(255)
  , @ip nvarchar(255)
  , @batch ntext
  , @filter nvarchar(255)
  , @type int=0
  , @outerId bigint=null
  , @year nvarchar(10)
as
begin
  declare 
    @currentDate datetime
    , @accountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @internalId bigint

  set @updateId = newid()
  
  set @currentDate = getdate()

  select
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  set @eventCode = N''CNE_BCH_CHK''

  begin tran insert_check_batch_tran

    insert dbo.CommonNationalExamCertificateCheckBatch
      (
      CreateDate
      , UpdateDate
      , OwnerAccountId
      , IsProcess
      , IsCorrect
      , Batch
      , [Filter]
      ,[Type]
      ,outerId
      , [Year]
      )
    select
      @currentDate
      , @currentDate
      , @accountId
      , 1
      , null
      , @batch
      , @filter
      ,@type
      ,@outerId
      , @year
      
    if (@@error <> 0)
      goto undo

	SET @id = SCOPE_IDENTITY()
    --set @internalId = scope_identity()
    --set @id = dbo.GetExternalId(@internalId)

    if (@@error <> 0)
      goto undo

  if @@trancount > 0
    commit tran insert_check_batch_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @accountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_check_batch_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAskedQuestion]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAskedQuestion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateAskedQuestion

-- =============================================
-- Сохранение изменений вопроса.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc [dbo].[UpdateAskedQuestion]
  @id bigint output
  , @name nvarchar(255)
  , @question ntext
  , @answer ntext
  , @isActive bit
  , @contextCodes nvarchar(4000)
  , @editorLogin nvarchar(255) = null
  , @editorIp nvarchar(255) = null
as
begin
  declare 
    @newAskedQuestion bit
    , @currentDate datetime
    , @editorAccountId bigint
    , @eventCode nvarchar(100)
    , @updateId uniqueidentifier
    , @ids nvarchar(255)
    , @oldIsActive bit
    , @public bit
    , @publicEventCode nvarchar(100)
    , @internalId bigint

  set @updateId = newid()

  declare @oldContextId table 
    (
    ContextId int
    )

  declare @newContextId table 
    (
    ContextId int
    )

  insert @newContextId
  select 
    context.Id
  from 
    dbo.GetDelimitedValues(@contextCodes) codes
      inner join dbo.Context context
        on context.Code = codes.[value]

  set @currentDate = getdate()

  select
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  if isnull(@id, 0) = 0 -- новая новость
  begin
    set @newAskedQuestion = 1
    set @eventCode = N''FAQ_CREATE''
  end
  else
  begin -- update существующей новости
    set @internalId = dbo.GetInternalId(@id)

    select 
      @oldIsActive = asked_question.IsActive
    from 
      dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
    where
      asked_question.[Id] = @internalId

    insert @oldContextId
      (
      ContextId
      )
    select
      asked_question_context.ContextId
    from
      dbo.AskedQuestionContext asked_question_context with (nolock)
    where
      asked_question_context.AskedQuestionId = @internalId

    set @eventCode = N''FAQ_EDIT''

    if @oldIsActive <> @isActive
    begin
      set @public = 1
      if @isActive = 1
        set @publicEventCode = N''FAQ_PUBLIC''
      else
        set @publicEventCode = N''FAQ_UNPUBLIC''
    end
  end

  begin tran insert_update_faq_tran

    if @newAskedQuestion = 1 -- новый документ
    begin
      insert dbo.AskedQuestion
        (
        CreateDate
        , UpdateDate
        , UpdateId
        , EditorAccountId
        , EditorIp
        , [Name]
        , Question
        , Answer
        , IsActive
        , ViewCount
        , Popularity
        , ContextCodes
        )
      select
        getdate()
        , getdate()
        , @updateId
        , @editorAccountId
        , @editorIp
        , @name
        , @question
        , @answer
        , @isActive
        , 0
        , 0
        , @contextCodes

      if (@@error <> 0)
        goto undo

      set @internalId = scope_identity()
      set @id = dbo.GetExternalId(@internalId)

      if (@@error <> 0)
        goto undo

      insert dbo.AskedQuestionContext
        (
        AskedQuestionId
        , ContextId
        )
      select
        @internalId
        , new_context_id.ContextId
      from 
        @newContextId new_context_id

      if (@@error <> 0)
        goto undo

    end 
    else 
    begin -- update существующего вопроса

      update asked_question
      set
        UpdateDate = GetDate()
        , UpdateId = @updateId
        , EditorAccountId = @editorAccountId
        , EditorIp = @editorIp
        , [Name] = @name
        , Question = @question
        , Answer = @answer
        , IsActive = @isActive
        , ContextCodes = @contextCodes
      from
        dbo.AskedQuestion asked_question with (rowlock)
      where
        asked_question.[Id] = @internalId

      if (@@error <> 0)
        goto undo

      if exists(select 
            1
          from
            @oldContextId old_context_id
              full outer join @newContextId new_context_id
                on old_context_id.ContextId = new_context_id.ContextId
          where
            old_context_id.ContextId is null
            or new_context_id.ContextId is null) 
      begin
        delete asked_question_context
        from 
          dbo.AskedQuestionContext asked_question_context
        where
          asked_question_context.AskedQuestionId = @internalId

        if (@@error <> 0)
          goto undo

        insert dbo.AskedQuestionContext
          (
          AskedQuestionId
          , ContextId
          )
        select
          @internalId
          , new_context_id.ContextId
        from 
          @newContextId new_context_id

        if (@@error <> 0)
          goto undo
      end
    end 

  if @@trancount > 0
    commit tran insert_update_faq_tran

  set @ids = convert(nvarchar(255), @internalId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @ids
    , @eventParams = null
    , @updateId = @updateId

  if @public = 1
    exec dbo.RegisterEvent 
      @accountId = @editorAccountId
      , @ip = @editorIp
      , @eventCode = @publicEventCode
      , @sourceEntityIds = @ids
      , @eventParams = null
      , @updateId = @updateId

  return 0

  undo:

  rollback tran insert_update_faq_tran

  return 1

end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountPassword]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccountPassword]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateAccountPassword

-- =============================================
-- Сохранить пароль пользователя.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 04.05.2008
-- Добавлен параметр password для обратной совместимости систем.
-- =============================================
CREATE proc [dbo].[UpdateAccountPassword]
  @login nvarchar(255)
  , @passwordHash nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
  , @password nvarchar(255) = null -- !временно
as
begin

  declare
    @editorAccountId bigint
    , @accountId bigint
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)

  set @updateId = newid()

  select 
    @accountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  update account
  set
    PasswordHash = @passwordHash
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , UpdateDate = GetDate()
    , UpdateId = @updateId
  from
    dbo.Account account with (rowlock)
  where
    account.[Id] = @accountId

-- временно
  if isnull(@password, '''') <> '''' and N''User'' = (select 
            [group].[code]
          from
            dbo.[Group] [group]
              inner join dbo.GroupAccount group_account
                on [group].[Id] = group_account.GroupId
          where
            group_account.AccountId = @accountId)
  begin
    if exists(select 
          1
        from
          dbo.UserAccountPassword user_account_password
        where
          user_account_password.AccountId = @accountId)
    begin
      update user_account_password
      set
        [Password] = @password
      from
        dbo.UserAccountPassword user_account_password
      where
        user_account_password.AccountId = @accountId
    end
    else
    begin
      insert dbo.UserAccountPassword
        (
        AccountId
        , [Password]
        )
      select 
        @accountId
        , @password
    end
  end

  exec dbo.RefreshRoleActivity @accountId = @accountId

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = N''USR_PASSW''
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountKey]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAccountKey]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.UpdateAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 01.09.2008
-- ====================================================
CREATE procedure [dbo].[UpdateAccountKey]
  @login nvarchar(255)
  , @key nvarchar(255)
  , @dateFrom datetime
  , @dateTo datetime
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare
    @accountId bigint
    , @editorAccountId bigint
    , @eventCode nvarchar(255)
    , @keyId bigint
    , @updateId uniqueidentifier
    , @keyIds nvarchar(255)

  set @updateId = newid()
  
  select @accountId = account.Id
  from dbo.Account account
  where account.[Login] = @login

  select @editorAccountId = account.Id
  from dbo.Account account
  where account.[Login] = @editorLogin

  select @keyId = account_key.Id
  from dbo.AccountKey account_key
  where account_key.[Key] = @key
    and account_key.AccountId = @accountId

  if @keyId is null
  begin
    insert into dbo.AccountKey
      (
      CreateDate
      , UpdateDate
      , UpdateId
      , EditorAccountId
      , EditorIp
      , AccountId
      , [Key]
      , DateFrom
      , DateTo
      , IsActive
      )
    select
      GetDate()
      , GetDate()
      , @updateId
      , @editorAccountId
      , @editorip
      , @accountId
      , @key
      , @dateFrom
      , @dateTo
      , @isActive

    set @keyId = scope_identity()

    set @eventCode = ''USR_KEY_CREATE''
  end
  else
  begin
    update account_key
    set
      UpdateDate = GetDate()
      , UpdateId = @updateId
      , EditorAccountId = @editorAccountId
      , EditorIp = @editorIp
      , DateFrom = @dateFrom
      , DateTo = @dateTo
      , IsActive = @isActive
    from dbo.AccountKey account_key
    where account_key.Id = @keyId

    set @eventCode = ''USR_KEY_EDIT''
  end

  set @keyIds = @keyId

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @keyIds
    , @eventParams = null
    , @updateId = @updateId

  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SetActiveNews]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetActiveNews]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SetActiveNews

-- =============================================
-- Установка активности новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[SetActiveNews]
  @ids nvarchar(255)
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @currentDate datetime
    , @innerIds nvarchar(4000)

  set @innerIds = ''''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N'',''
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()
  set @currentDate = getdate()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  if @isActive = 1
    set @eventCode = N''NWS_PUBLIC''
  else
    set @eventCode = N''NWS_UNPUBLIC''

  update news
  set
    UpdateDate = @currentDate
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , IsActive = @isActive
  from 
    dbo.News news with (rowlock)
      inner join @idTable idTable
        on news.[id] = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SetActiveDocument]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetActiveDocument]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SetActiveDocument

-- =============================================
-- Установка активности документа.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.2: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.3: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[SetActiveDocument]
  @ids nvarchar(255)
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @activateDate datetime
    , @currentDate datetime
    , @innerIds nvarchar(4000)

  set @innerIds = ''''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N'',''
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()
  set @currentDate = getdate()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  if @isActive = 1
    select 
      @eventCode = N''DOC_PUBLIC''
      , @activateDate = @currentDate
  else
    select
      @eventCode = N''DOC_UNPUBLIC''
      , @activateDate = null

  update [document]
  set
    UpdateDate = @currentDate
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , IsActive = @isActive
    , ActivateDate = @activateDate
  from 
    dbo.[Document] [document] with (rowlock)
      inner join @idTable idTable
        on [document].[id] = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SetActiveAskedQuestion]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetActiveAskedQuestion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SetActiveAskedQuestion

-- =============================================
-- Установка активности вопроса.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[SetActiveAskedQuestion]
  @ids nvarchar(255)
  , @isActive bit
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  declare @idTable table
    (
    [id] bigint
    )

  insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
    , @currentDate datetime
    , @innerIds nvarchar(4000)

  set @innerIds = ''''
  
  select 
    @innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N'',''
  from 
    @idTable id_table
  
  if len(@innerIds) > 0
    set @innerIds = left(@innerIds, len(@innerIds) - 1)

  set @updateId = newid()
  set @currentDate = getdate()

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin
  
  if @isActive = 1
    set @eventCode = N''FAQ_PUBLIC''
  else
    set @eventCode = N''FAQ_UNPUBLIC''

  update asked_question
  set
    UpdateDate = @currentDate
    , UpdateId = @updateId
    , EditorAccountId = @editorAccountId
    , EditorIp = @editorIp
    , IsActive = @isActive
  from 
    dbo.AskedQuestion asked_question with (rowlock)
      inner join @idTable idTable
        on asked_question.[id] = idTable.[id]

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @innerIds
    , @eventParams = null
    , @updateId = @updateId
    
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCompetitionCertificate]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCompetitionCertificate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.SearchCompetitionCertificate
-- =============================================
-- Процедура  поиска олимпиадников.
-- Created by Sedov Anton 10.07.2008
-- Modified by Fomin Dmitriy 23.07.2008
-- Поле SubjectId заменено CompetitionTypeId.
-- Тип олимпиады необязателен.
-- =============================================
CREATE procedure [dbo].[SearchCompetitionCertificate]
  @competitionTypeId int = null
  , @lastName nvarchar(255)
  , @firstName nvarchar(255)
  , @patronymicName nvarchar(255)
  , @regionId int = null
  , @login nvarchar(255)
  , @ip nvarchar(255)
as
begin
  declare 
    @year int 
  
  set @year = Year(GetDate())

  select
    searching_competition_certificate.CompetitionTypeId CompetitionTypeId
    , competition_type.[Name] CompetitionTypeName
    , searching_competition_certificate.LastName LastName
    , searching_competition_certificate.FirstName FirstName
    , searching_competition_certificate.PatronymicName PatronymicName
    , competition_certificate.Degree Degree
    , isnull(region.[Name], searching_region.[Name]) RegionName
    , competition_certificate.City City
    , competition_certificate.School School
    , competition_certificate.Class Class
    , case 
      when competition_certificate.Id is null then 0
      else 1
    end IsExist 
  from
    (select
      @competitionTypeId CompetitionTypeId
      , @lastName LastName
      , @firstName FirstName
      , @patronymicName PatronymicName
      , @regionId RegionId) as searching_competition_certificate
      left join dbo.CompetitionCertificate competition_certificate
        left join dbo.CompetitionType competition_type
          on competition_certificate.CompetitionTypeId = competition_type.Id
        left join dbo.Region region
          on competition_certificate.RegionId = region.Id
        on searching_competition_certificate.LastName = competition_certificate.LastName
          and searching_competition_certificate.FirstName = competition_certificate.FirstName
          and searching_competition_certificate.PatronymicName = competition_certificate.PatronymicName
          and competition_certificate.[Year] = @year
          -- Лучше использовать диманический SQL, но здесь не критично.
          and (searching_competition_certificate.CompetitionTypeId = competition_certificate.CompetitionTypeId
            or searching_competition_certificate.CompetitionTypeId is null) 
          and (searching_competition_certificate.RegionId = competition_certificate.RegionId
            or searching_competition_certificate.RegionId is null) 
      left join dbo.Region searching_region
        on searching_competition_certificate.RegionId = region.Id

  declare 
    @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @updateId uniqueidentifier
  
  select
    @editorAccountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @login 

  set @eventCode = ''SCC_FND''
  set @updateId = NewId()     
  
  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @ip
    , @eventCode = @eventCode
    , @sourceEntityIds = null
    , @eventParams = null
    , @updateId = @updateId
        
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateWildcard]    Script Date: 05/07/2015 18:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateWildcard]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SearchCommonNationalExamCertificateWildcard]
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
    @lastName + ''|'' 
    + @firstName + ''|'' 
    + @patronymicName + ''|'' 
    + isnull(@passportSeria, '''') + ''|'' 
    + isnull(@passportNumber, '''') + ''|'' 
    + isnull(@Number, '''') + ''|'' 
    + isnull(@typographicNumber, '''') + ''|'' 
    + isnull(cast(@year as varchar(max)), '''')

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
    @commandText = ''''
    ,@eventCode = N''CNE_FND_WLDCRD''

  declare @sourceEntityIds nvarchar(4000)  
  declare @Search table 
  ( 
    row int,
    LastName nvarchar(255), 
    FirstName nvarchar(255), 
    PatronymicName nvarchar(255), 
    CertificateId uniqueidentifier, 
    CertificateNumber nvarchar(255),
    RegionId int, 
    PassportSeria nvarchar(255), 
    PassportNumber nvarchar(255), 
    TypographicNumber nvarchar(255),
    Year int,
    ParticipantFK uniqueidentifier,
    primary key(row) 
      ) 
      
  if @showCount = 0
  set @commandText = @commandText + 
    '' 
    select top (@startRowIndex+@maxRowCount-1) 
        row_number() over (order by a.useyear, a.CertificateID) as row,a.*
    from ( select distinct
      a.Surname 
      , a.Name 
      , a.SecondName 
      , b.CertificateID 
      , b.LicenseNumber 
      , b.Region
      , isnull(a.DocumentSeries, @internalPassportSeria) PassportSeria 
      , isnull(a.DocumentNumber, @passportNumber) PassportNumber
      , b.TypographicNumber 
      , isnull(b.UseYear,a.UseYear) UseYear
      , a.ParticipantID
    ''
  if @showCount = 1
    set @commandText = '' select count(*) ''
  
  set @commandText = @commandText + 
    ''
    from 
		rbd.Participants AS a WITH (nolock) 
		JOIN prn.CertificatesMarks AS cm WITH (nolock) ON cm.ParticipantFK = a.ParticipantID AND cm.UseYear = a.UseYear 
		LEFT JOIN prn.Certificates AS b WITH (nolock) ON b.CertificateID = cm.CertificateFK AND b.UseYear = cm.UseYear 
		LEFT JOIN prn.CancelledCertificates AS c WITH (nolock) ON c.CertificateFK = b.CertificateID AND c.UseYear = b.UseYear    
    where 
      a.[UseYear] between @yearFrom and @yearTo ''
  
  if @lastName is not null 
    set @commandText = @commandText + ''
      and a.Surname collate cyrillic_general_ci_ai = @lastName''
  if @firstName is not null 
    set @commandText = @commandText + ''     
      and a.Name collate cyrillic_general_ci_ai = @firstName''
  if @patronymicName is not null 
    set @commandText = @commandText + ''           
      and a.SecondName collate cyrillic_general_ci_ai = @patronymicName''
  if @internalPassportSeria is not null 
    set @commandText = @commandText + ''                 
      and a.DocumentSeries = @internalPassportSeria''
  if @passportNumber is not null 
    set @commandText = @commandText + ''                       
      and a.DocumentNumber = @passportNumber''
  if @typographicNumber is not null 
    set @commandText = @commandText + ''                             
      and b.TypographicNumber = @typographicNumber''
  if @Number is not null 
    set @commandText = @commandText + ''                                   
      and b.LicenseNumber = @Number ''  
      
  if @showCount = 1     
    exec sp_executesql @commandText
      , N''@lastName nvarchar(255)
        , @firstName nvarchar(255)
        , @patronymicName nvarchar(255)
        , @internalPassportSeria nvarchar(255)
        , @passportNumber nvarchar(255)
        , @typographicNumber nvarchar(255) 
        , @Number nvarchar(255)
        , @yearFrom int, @yearTo int 
        , @startRowIndex int
        , @maxRowCount int''
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
	set @commandText=@commandText +'' ) a''
    insert into @Search 
    exec sp_executesql @commandText
      , N''@lastName nvarchar(255)
        , @firstName nvarchar(255)
        , @patronymicName nvarchar(255)
        , @internalPassportSeria nvarchar(255)
        , @passportNumber nvarchar(255)
        , @typographicNumber nvarchar(255) 
        , @Number nvarchar(255)
        , @yearFrom int, @yearTo int 
        , @startRowIndex int
        , @maxRowCount int''
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
        isnull(cast(search.CertificateNumber as nvarchar(250)),''Нет свидетельства'' ) CertificateNumber
        , search.LastName LastName 
        , search.FirstName FirstName 
        , search.PatronymicName PatronymicName 
        , search.PassportSeria PassportSeria 
        , search.PassportNumber PassportNumber 
        , search.TypographicNumber TypographicNumber 
        , region.Name RegionName 
        , case
          when search.CertificateId is not null or search.ParticipantFK is not null then 1
          else 0
        end IsExist
        , case 
          when not cne_certificate_deny.UseYear is null then 1 
          else 0 
        end IsDeny 
        , cne_certificate_deny.Reason DenyComment 
        , null NewCertificateNumber 
        , search.[Year] 
        , case when ed.[ExpireDate] is null then ''Не найдено''  
             when cne_certificate_deny.UseYear is not null then ''Аннулировано'' 
             when getdate() <= ed.[ExpireDate] then ''Действительно''
             else ''Истек срок'' 
          end as [Status]
        , unique_cheks.UniqueIHEaFCheck,
        search.ParticipantFK ParticipantID
       from @Search search
        left outer join dbo.ExamCertificateUniqueChecks unique_cheks on unique_cheks.idGUID = search.CertificateId 
        left outer join prn.CancelledCertificates cne_certificate_deny with (nolock) on cne_certificate_deny.[UseYear] = search.[year] and search.CertificateId = cne_certificate_deny.CertificateFK 
        left outer join dbo.Region region with (nolock) on region.[Id] = search.RegionId 
        left join [ExpireDate] ed on  ed.[year] = search.[year] 
       where row between @startRowIndex and (@startRowIndex+@maxRowCount-1)       
       
       exec dbo.RegisterEvent 
        @accountId = @editorAccountId  
        , @ip = @ip 
        , @eventCode = @eventCode 
        , @sourceEntityIds = ''0''
        , @eventParams = @eventParams 
        , @updateId = null 
  end
        
  return 0
end
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificatePassport]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificatePassport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SearchCommonNationalExamCertificatePassport]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
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
    isnull(@lastName,'''') + ''|'' 
    + isnull(@firstName,'''') + ''|'' 
    + isnull(@patronymicName,'''') + ''|'' 
    + isnull(@passportSeria, '''') + ''|'' 
    + isnull(@passportNumber, '''') 

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

  set @commandText = ''''
  set @eventCode = N''CNE_FND_TN''

  declare @CId bigint,@sourceEntityIds nvarchar(4000) 
  declare @Search table 
  ( 
    LastName nvarchar(255) 
    , FirstName nvarchar(255) 
    , PatronymicName nvarchar(255) 
    , CertificateId uniqueidentifier 
    , CertificateNumber nvarchar(255) 
    , RegionId int 
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255) 
    , TypographicNumber nvarchar(255) 
    , Year int
    , ParticipantID uniqueidentifier
  ) 
    
  set @commandText = @commandText +         
    ''select * from 
     (
		select distinct a.Surname, a.Name, a.SecondName, b.CertificateID, b.LicenseNumber,
            b.Region, isnull(a.DocumentSeries, @internalPassportSeria) DocumentSeries, 
            isnull(a.DocumentNumber, @passportNumber) DocumentNumber, b.TypographicNumber, isnull(b.UseYear,a.UseYear) UseYear, a.ParticipantID 
		from 
			rbd.Participants AS a WITH (nolock) 
			JOIN prn.CertificatesMarks AS cm WITH (nolock) ON cm.ParticipantFK = a.ParticipantID AND cm.UseYear = a.UseYear 
			LEFT JOIN prn.Certificates AS b WITH (nolock) ON b.CertificateID = cm.CertificateFK AND b.UseYear = cm.UseYear 
			LEFT JOIN prn.CancelledCertificates AS c WITH (nolock) ON c.CertificateFK = b.CertificateID AND c.UseYear = b.UseYear   
		where a.[UseYear] between @yearFrom and @yearTo '' 

  if not @lastName is null 
    set @commandText = @commandText +
      '' and a.Surname collate cyrillic_general_ci_ai = @lastName ''
  
  if not @firstName is null 
    set @commandText = @commandText +
      '' and a.Name collate cyrillic_general_ci_ai = @firstName '' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      '' and a.SecondName collate cyrillic_general_ci_ai = @patronymicName '' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX(''*'', @internalPassportSeria) > 0 or CHARINDEX(''?'', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, ''*'', ''%'')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, ''?'', ''_'')
            set @commandText = @commandText +
                '' and a.DocumentSeries like @internalPassportSeria ''
        end
        else begin
            set @commandText = @commandText +
                '' and a.DocumentSeries = @internalPassportSeria ''
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX(''*'', @passportNumber) > 0 or CHARINDEX(''?'', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, ''*'', ''%'')
          set @passportNumber = REPLACE(@passportNumber, ''?'', ''_'')
            set @commandText = @commandText +
                '' and a.DocumentNumber like @passportNumber ''
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    '' and a.DocumentNumber = @passportNumber ''
        end
    end
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      '' and 0 = 1 ''

  set @commandText = @commandText + 
  '' ) t''
  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N''@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @yearFrom int, @yearTo int ''
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @yearFrom, @YearTo

  set @sourceEntityIds = ''''

  select @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(4000), search.CertificateId) 
  from @Search search 
  
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
    
  if @sourceEntityIds = ''''
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

  declare @Search1 table 
  ( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
  )     
  insert @Search1
    select distinct S.CertificateId 
    from @Search S   
  where CertificateId is not null
  
  declare @CertificateId uniqueidentifier,@pkid int
  while exists(select * from @Search1)
  begin
    select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 

  select 
      isnull(cast(S.CertificateId as nvarchar(500)),''Нет свидетельства'')CertificateId, 
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.LastName is not null then 1 
        else 0 
      end IsExist, 
      case 
        when CD.UseYear is not null then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then ''Не найдено''
                when CD.UseYear is not null then ''Аннулировано'' 
                when getdate() <= ed.[ExpireDate] then ''Действительно'' 
                else ''Истек срок'' 
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
      CC.UniqueOtherCheck UniqueOtherCheck,
      S.ParticipantID ParticipantID
    from 
      @Search S 
		left outer join ExamCertificateUniqueChecks CC with (nolock) on CC.idGUID  = S.CertificateId 
		left outer join prn.CancelledCertificates CD with (nolock) on CD.[UseYear] = S.[year] and S.CertificateId = CD.CertificateFK
		left join dbo.Region region with (nolock) on region.[Id] = S.RegionId 
		left join [ExpireDate] ed on ed.[year] = S.[year]
            
  exec dbo.RegisterEvent 
      @accountId = @editorAccountId, 
      @ip = @ip, 
      @eventCode = @eventCode, 
      @sourceEntityIds = @sourceEntityIds, 
      @eventParams = @eventParams, 
      @updateId = null 
      
  return 0
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusWithAccredTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportUserStatusWithAccredTVF](@periodBegin datetime,@periodEnd datetime)
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
SELECT ''На доработке'',''revision'',3
UNION
SELECT ''На регистрации'',''registration'',1
UNION
SELECT ''Отключен'',''deactivated'',5
UNION
SELECT ''Активирован'',''activated'',4
UNION
SELECT ''На согласовании'',''consideration'',2
UNION
SELECT ''Всего'',''total'',10

DECLARE @OPF TABLE
(
  [Name] NVARCHAR (50),
  Code BIT,
  [Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT ''Негосударственный'',1,1
UNION
SELECT ''Государственный'',0,0

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
SELECT ''ВУЗ'',1,''Всего'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT ''ССУЗ'',2,''Всего'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT ''Итого'',10,''-'',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

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
  Comb.StatusCode=''total''
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
,''В том числе на ''+convert(varchar(16),@periodEnd, 120)+'' за '' 
+ case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end
+'':'' 
, ''-''
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
  SELECT 
    1 AS [Всего],
    case when A.[Status]=''activated'' then 1 else 0 end AS [Активирован],
    case when A.[Status]=''registration'' then 1 else 0 end AS [На регистрации],
    case when A.[Status]=''revision'' then 1 else 0 end AS [На доработке],
    case when A.[Status]=''consideration'' then 1 else 0 end AS [На согласовании],
    case when A.[Status]=''deactivated'' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status=''registration'' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
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
''Из них аккредитованных'',NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF (@periodBegin ,@periodEnd)

return
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificate]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[SearchCommonNationalExamCertificate]
  @lastName nvarchar(255) = null        -- фамилия сертифицируемого
  , @firstName nvarchar(255) = null     -- имя сертифицируемого
  , @patronymicName nvarchar(255) = null    -- отчетсво сертифицируемого
  , @subjectMarks nvarchar(4000) = null   -- средние оценки по предметам (через запятую, в определенном порядке)
  , @passportSeria nvarchar(255) = null   -- серия документа сертифицируемого (паспорта)
  , @passportNumber nvarchar(255) = null    -- номер документа сертифицируемого (паспорта)
  , @typographicNumber nvarchar(255) = null -- типографический номер сертификата 
  , @login nvarchar(255)            -- логин проверяющего
  , @ip nvarchar(255)             -- ip проверяющего
  , @year int = null                -- год выдачи сертификата
  , @ParticipantID uniqueidentifier = null  
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
    isnull(@lastName,'''') + ''|'' 
    + isnull(@firstName,'''') + ''|'' 
    + isnull(@patronymicName,'''') + ''|'' 
    + isnull(@subjectMarks, '''') + ''|'' 
    + isnull(@passportSeria, '''') + ''|'' 
    + isnull(@passportNumber, '''') + ''|'' 
    + isnull(@typographicNumber, '''')

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

  set @commandText = ''''
  if isnull(@typographicNumber, '''') <> ''''
    set @eventCode = N''CNE_FND_TN''
  else 
    set @eventCode = N''CNE_FND_P''

  declare @CId bigint,@sourceEntityIds nvarchar(4000) 
  declare @Search table 
  ( 
    LastName nvarchar(255) 
    , FirstName nvarchar(255) 
    , PatronymicName nvarchar(255) 
    , CertificateId uniqueidentifier
    , CertificateNumber nvarchar(255) 
    , RegionId int 
    , PassportSeria nvarchar(255)
    , PassportNumber nvarchar(255) 
    , TypographicNumber nvarchar(255) 
    , Year int 
    , ParticipantID uniqueidentifier
  ) 
    
  set @commandText = @commandText +       
    ''select top 300 certificate.LastName, certificate.FirstName, certificate.PatronymicName, certificate.Id, certificate.Number,
            certificate.RegionId, isnull(certificate.PassportSeria, @internalPassportSeria), 
            isnull(certificate.PassportNumber, @passportNumber), certificate.TypographicNumber, certificate.Year,ParticipantID
    from (
		select distinct b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, 
           COALESCE(c.UseYear,b.UseYear,a.UseYear) AS Year, a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, 
           COALESCE(c.REGION,b.REGION,a.REGION) AS RegionId, b.TypographicNumber, 
           a.ParticipantID AS ParticipantID
        from rbd.Participants a with (nolock)       
			left join prn.CertificatesMarks cm with (nolock) on cm.ParticipantFK=a.ParticipantID and a.UseYear=cm.UseYear
			left join prn.Certificates b with (nolock) on b.CertificateID=cm.CertificateFK and a.UseYear=b.UseYear
			left join prn.CancelledCertificates c with (nolock) on c.CertificateFK=b.CertificateID and c.UseYear=b.UseYear
      where a.[UseYear] between @yearFrom and @yearTo '' 

  if not @lastName is null 
    set @commandText = @commandText +
      '' and a.Surname collate cyrillic_general_ci_ai = @lastName ''
  
  if not @firstName is null 
    set @commandText = @commandText +
      '' and a.Name collate cyrillic_general_ci_ai = @firstName '' 

  if not @patronymicName is null 
    set @commandText = @commandText +
      '' and a.SecondName collate cyrillic_general_ci_ai = @patronymicName '' 

  if not @internalPassportSeria is null
    begin
      if CHARINDEX(''*'', @internalPassportSeria) > 0 or CHARINDEX(''?'', @internalPassportSeria) > 0
        begin
          set @internalPassportSeria = REPLACE(@internalPassportSeria, ''*'', ''%'')
          set @internalPassportSeria = REPLACE(@internalPassportSeria, ''?'', ''_'')
            set @commandText = @commandText +
                '' and a.DocumentSeries like @internalPassportSeria and a.ParticipantID is not null ''
        end
        else begin
            set @commandText = @commandText +
                '' and a.DocumentSeries = @internalPassportSeria and a.ParticipantID is not null ''
        end
  end

  if not @passportNumber is null
    begin
      if CHARINDEX(''*'', @passportNumber) > 0 or CHARINDEX(''?'', @passportNumber) > 0
        begin
          set @passportNumber = REPLACE(@passportNumber, ''*'', ''%'')
          set @passportNumber = REPLACE(@passportNumber, ''?'', ''_'')
            set @commandText = @commandText +
                '' and a.DocumentNumber like @passportNumber and a.ParticipantID is not null ''
        end
      else begin
            if not @passportNumber is null
                set @commandText = @commandText +
                    '' and a.DocumentNumber = @passportNumber and a.ParticipantID is not null ''
        end
    end
  
  if not @typographicNumber is null
    set @commandText = @commandText +
      '' and b.TypographicNumber = @typographicNumber and a.ParticipantID is not null ''
  
  if @lastName is null and @firstName is null and @passportNumber is null
    set @commandText = @commandText +
      '' and 0 = 1 ''

  set @commandText = @commandText + '') [certificate] ''
  print @commandText 

  insert into @Search
  exec sp_executesql @commandText
    , N''@lastName nvarchar(255), @firstName nvarchar(255), @patronymicName nvarchar(255), @internalPassportSeria nvarchar(255)
      , @passportNumber nvarchar(255), @typographicNumber nvarchar(255), @yearFrom int, @yearTo int ''
    , @lastName, @firstName, @patronymicName, @internalPassportSeria, @passportNumber, @typographicNumber, @yearFrom, @YearTo                                 

  if @subjectMarks is not null
  begin 
      
/*
  Для потомков и наверное для меня самого на будущее.
  Описание задания
  
  Нужно проверять если оценки полностью совпадают с одним из свидетельств (с номером или без номера), то выдавать это свидетельство. 
  Если есть оценки и с того и другого свидетельства то возвращать пусто
  
  Кроме того тут заложена логика проверки корректности введенных баллов. Если один из баллов введен не правильно, то вовращать пусто.
  Эта проверка должна осуществляться после первого правила (см выше) 
  
  Проблема 
  Так как для некоторых проверок нет номера свидетельства то приходится связываться по участнику. 
  Но если связываться по участнику, то выдаются все свидетества для этого учатника. Проблема была решена как....
  
  Описание логики работы
  Делаем два запроса для свидетельства с номером и без него.
  Запоминаем ИД сертификата и код предмета
  Смотрим в сформированную таблицу если есть повторяющиеся баллы то удаляем те записи где есть это повторение и нет свидетельства
  
  Смотрим есть после этого в таблице есть два разных свидетельства (без номера тоже считается), то вовращаем пустую проверку
    Иначе смотрим есть свидетельство с номером, то удаляем из @Search свидетельство без номера и осуществляем проверку кооректности введеных баллов
                          иначе удаляем из @Search свидетельство с номером и осуществляем проверку кооректности введеных баллов
*/                       
    create table #tt(id int primary key identity(1,1),code int ,CertificateId uniqueidentifier) 
                                    
    insert #tt
    select distinct certificate_subject.SubjectCode,inner_search.CertificateId 
    from [prn].CertificatesMarks certificate_subject with(nolock) 
      join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
          and 1=case when inner_search.CertificateId = certificate_subject.CertificateFK then 1             
                else 0  end 
      join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark                         
    where inner_search.CertificateId is not null 
    union all
    select distinct certificate_subject.SubjectCode,inner_search.CertificateId 
    from [prn].CertificatesMarks certificate_subject with(nolock) 
      join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                 and 1= case when inner_search.ParticipantID = certificate_subject.ParticipantFK then 1 
                else 0 end 
      join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark                         
    where inner_search.CertificateId is null        
            
    if exists(select * from #tt group by code having count(code)>1)
      delete a from #tt a 
        join (select code from #tt group by code having count(code)>1) b on a.code=b.code 
      where CertificateId is null 
                
    if exists(select * from #tt having count(distinct isnull(CertificateId,''22655BE0-C368-4EB8-8835-5E0F7BA807B5''))>1)
    begin     
      delete search 
      from @Search search 
      
      delete #tt
    end     
    
    if exists(select * from #tt where CertificateId is not null)  
    begin               
      delete search 
      from @Search search 
      where CertificateId is null
      
      delete search 
      from @Search search 
      where exists(select 1 
            from [prn].CertificatesMarks certificate_subject with(nolock) 
              inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                  and search.CertificateId = inner_search.CertificateId
                  and inner_search.CertificateId = certificate_subject.CertificateFK
              right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark 
            where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)      
            
    end       
            
    if exists(select * from #tt where CertificateId is null)  
    begin               
      delete search 
      from @Search search 
      where CertificateId is not null
                    
      delete search 
      from @Search search 
      where exists(select 1 
            from [prn].CertificatesMarks certificate_subject with(nolock) 
              inner join @Search inner_search on certificate_subject.UseYear between @yearFrom and @yearTo 
                  and search.ParticipantID = inner_search.ParticipantID
                  and inner_search.ParticipantID = certificate_subject.ParticipantFK
              right join dbo.GetSubjectMarks(@subjectMarks) subject_mark on certificate_subject.SubjectCode = subject_mark.SubjectId 
                  and certificate_subject.Mark = subject_mark.Mark 
            where certificate_subject.SubjectCode is null or certificate_subject.Mark is null)            
    end
    
    drop table #tt    
  end

  set @sourceEntityIds = ''''
  
  select @sourceEntityIds = @sourceEntityIds + '','' + Convert(nvarchar(4000), search.CertificateId) 
  from @Search search 
  
  set @sourceEntityIds = substring(@sourceEntityIds, 2, len(@sourceEntityIds)) 
    
  if @sourceEntityIds = ''''
    set @sourceEntityIds = null 

  -- Выполняем подсчет уникальных проверок 
    -- Для каждого найденного сертификата вызываем хранимую процедуру подсчета проверок  

  declare @Search1 table 
  ( pkid int identity(1,1) primary key, CertificateId uniqueidentifier
  )     
  insert @Search1
    select distinct S.CertificateId 
    from @Search S   
  where CertificateId is not null

  declare @CertificateId uniqueidentifier,@pkid int
  while exists(select * from @Search1)
  begin
    select top 1 @CertificateId=CertificateId,@pkid=pkid from @Search1

      exec dbo.ExecuteChecksCount 
           @OrganizationId = @organizationId, 
           @CertificateIdGuid = @CertificateId 
                    
    delete @Search1 where pkid=@pkid
  end 
      
  select 
      isnull(cast(S.certificateId as nvarchar(255)),''Нет свидетельства'') CertificateId,
      S.CertificateNumber,
      S.LastName LastName,
      S.FirstName FirstName,
      S.PatronymicName PatronymicName,
      S.PassportSeria PassportSeria,
      S.PassportNumber PassportNumber,
      S.TypographicNumber TypographicNumber,
      region.Name RegionName, 
      case 
        when S.LastName is not null then 1 
        else 0 
      end IsExist, 
      case 
        when CD.UseYear is not null then 1 
      end IsDeny,  
      CD.Reason DenyComment, 
      null NewCertificateNumber, 
      S.Year,
      case 
        when ed.[ExpireDate] is null then ''Не найдено''
                when CD.UseYear is not null then ''Аннулировано'' 
                when getdate() <= ed.[ExpireDate] then ''Действительно'' 
                else ''Истек срок'' 
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
      CC.UniqueOtherCheck UniqueOtherCheck,
      S.ParticipantID ParticipantID
    from 
      @Search S 
            left outer join ExamCertificateUniqueChecks CC with (nolock) 
        on CC.idGUID  = S.CertificateId 
      left outer join prn.CancelledCertificates CD with (nolock) 
        on CD.[UseYear]=S.[Year] 
                and S.CertificateId = CD.CertificateFK 
      left outer join dbo.Region region with (nolock)
        on region.[Id] = S.RegionId 
      left join [ExpireDate] ed with(nolock)
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

' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetRemindAccount]    Script Date: 05/07/2015 18:13:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRemindAccount]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- exec dbo.GetRemindAccount

-- =============================================
-- Получить забытую учетную запись.
-- v.1.0: Created by Makarev Andrey
-- =============================================
CREATE procedure [dbo].[GetRemindAccount]
  @email nvarchar(255)
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
as
begin
  
  declare
    @currentYear int
    , @eventCode nvarchar(255)
    , @editorAccountId bigint
    , @login nvarchar(255) 
    , @accountId bigint
    , @accountIds nvarchar(255)

  set @currentYear = year(getdate())
  set @eventCode = N''USR_REMIND''

  select 
    @editorAccountId = account.[Id]
  from
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.[Login] = @editorLogin

  select top 1
    @login = account.[Login] 
    , @accountId = account.[Id]
  from 
    dbo.Account account with (nolock, fastfirstrow)
  where 
    account.email = @email
  order by 
    dbo.GetUserStatusOrder(dbo.GetUserStatus(account.ConfirmYear , account.Status
        , @currentYear, account.RegistrationDocument)) desc
    , account.UpdateDate desc

  select 
    @login [Login]
    , @email email

  set @accountIds = isnull(convert(nvarchar(255), @accountId), '''')

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = @email
    , @updateId = null

  return 0
end
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsAggregatedTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsAggregatedTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportCheckedCNEsAggregatedTVF](
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistrationShortTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportRegistrationShortTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportRegistrationShortTVF](
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
WHERE [ОПФ]=''Частный'' AND [Тип]=''ВУЗ''
DECLARE @NotRegistredOrgsPrivCount INT
SELECT @NotRegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Частный'' AND [Тип]=''ВУЗ''
DECLARE @RegistredOrgsStateCount INT
SELECT @RegistredOrgsStateCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Гос-ный'' AND [Тип]=''ВУЗ''
DECLARE @NotRegistredOrgsStateCount INT
SELECT @NotRegistredOrgsStateCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Гос-ный'' AND [Тип]=''ВУЗ''


DECLARE @RegistredOrgsPrivAccredCount INT
SELECT @RegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Частный'' AND [Тип]=''ВУЗ'' AND [Аккредитация по справочнику]=''Есть''
DECLARE @NotRegistredOrgsPrivAccredCount INT
SELECT @NotRegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Частный'' AND [Тип]=''ВУЗ'' AND [Аккредитация по справочнику]=''Есть''
DECLARE @RegistredOrgsStateAccredCount INT
SELECT @RegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Гос-ный'' AND [Тип]=''ВУЗ'' AND [Аккредитация по справочнику]=''Есть''
DECLARE @NotRegistredOrgsStateAccredCount INT
SELECT @NotRegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]=''Гос-ный'' AND [Тип]=''ВУЗ'' AND [Аккредитация по справочнику]=''Есть''

INSERT INTO @Report
SELECT ''Государственный'',@RegistredOrgsStateCount,@NotRegistredOrgsStateCount,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount
INSERT INTO @Report
SELECT ''Негосударственный'',@RegistredOrgsPrivCount,@NotRegistredOrgsPrivCount,@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount
INSERT INTO @Report
SELECT ''Итого''
,@RegistredOrgsStateCount+@RegistredOrgsPrivCount
,@NotRegistredOrgsStateCount+@NotRegistredOrgsPrivCount
,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount+@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount


INSERT INTO @Report
SELECT '''',null,null,null
INSERT INTO @Report
SELECT ''Аккредитованных'',null,null,null


INSERT INTO @Report
SELECT ''Государственный'',@RegistredOrgsStateAccredCount,@NotRegistredOrgsStateAccredCount,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount
INSERT INTO @Report
SELECT ''Негосударственный'',@RegistredOrgsPrivAccredCount,@NotRegistredOrgsPrivAccredCount,@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount
INSERT INTO @Report
SELECT ''Итого''
,@RegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount
,@NotRegistredOrgsStateAccredCount+@NotRegistredOrgsPrivAccredCount
,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount

RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportCheckedCNEsTVF](
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCheckedCNEsDetailedTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCheckedCNEsDetailedTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportCheckedCNEsDetailedTVF](
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
,IChecks.OrgType+''/''+CASE WHEN IChecks.OPF=1 THEN ''Негосударственный'' ELSE ''Государственный'' END AS [Тип ОУ/ОПФ ОУ]
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF_New]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusAccredTVF_New]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportUserStatusAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
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
SELECT ''Итого'',''-'',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]=''Всего''

INSERT INTO @report
SELECT 
''В том числе на ''+convert(varchar(16),@periodEnd, 120)+'' за '' 
+ case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end
+'':'' 
, ''-''
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
    case when A.[Status]=''activated'' then 1 else 0 end AS [Активирован],
    case when A.[Status]=''registration'' then 1 else 0 end AS [На регистрации],
    case when A.[Status]=''revision'' then 1 else 0 end AS [На доработке],
    case when A.[Status]=''consideration'' then 1 else 0 end AS [На согласовании],
    case when A.[Status]=''deactivated'' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
  INNER JOIN Organization2010 Org ON 
  (
    Org.Id=OReq.OrganizationId 
    AND (
      Org.IsAccredited=1 
      OR (
        Org.AccreditationSertificate != '''' 
        AND Org.AccreditationSertificate IS NOT NULL
        )
      )
  )
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status=''registration'' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  

return
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportTotalChecksTVF_New]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportTotalChecksTVF_New]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportTotalChecksTVF_New](
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
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportCommonStatisticsTVF]    Script Date: 05/07/2015 18:13:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportCommonStatisticsTVF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportCommonStatisticsTVF](
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
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]=''Пакетная''


DECLARE @UniqueChecks_UI INT

SELECT @UniqueChecks_UI=[всего уникальных проверок]
FROM ReportTotalChecksTVF(null,null) WHERE [Тип проверки]=''Интерактивная''


INSERT INTO @Report
SELECT @CNEsCount,@UsersCount,@TotalChecks,@TotalUniqueChecks,@UniqueChecks_Batch,@UniqueChecks_UI

RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportXMLSubordinateOrg]    Script Date: 05/07/2015 18:13:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportXMLSubordinateOrg]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--Функция по подведомственным учреждениям для экспорта в XML
CREATE funCTION  [dbo].[ReportXMLSubordinateOrg](
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
  A.Id [Код ОУ],
  A.FullName [Полное наименование],
  A.RegionId [Код региона] ,
  A.RegionName [Наименование региона],
  A.AccreditationSertificate [Свидетельство об аккредитации],
  A.DirectorFullName [ФИО руководителя],
  A.CountUser [Количество пользователей],
  A.UserUpdateDate [Дата активации],
  A.CountUniqueChecks [Уникальных проверок]
FROM
  dbo.ReportStatisticSubordinateOrg ( null, null, @departmentId) A
  inner join dbo.Organization2010 O on O.Id = A.Id
ORDER BY
  case when O.MainId is null then O.Id else O.MainId end, O.MainId, A.FullName
  
RETURN
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF_New]    Script Date: 05/07/2015 18:13:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportUserStatusWithAccredTVF_New]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[ReportUserStatusWithAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
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
SELECT ''Итого'',''-'',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]=''Всего'' OR [ ]=''РЦОИ'' OR [ ]=''Орган управления образованием'' OR [ ]=''Другое''


INSERT INTO @report
SELECT 
''В том числе на ''+convert(varchar(16),@periodEnd, 120)+'' за '' 
+ case @days when 1 then ''24 часа'' else cast(@days as varchar(10)) + '' дней'' end
+'':'' 
, ''-''
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
    case when A.[Status]=''activated'' then 1 else 0 end AS [Активирован],
    case when A.[Status]=''registration'' then 1 else 0 end AS [На регистрации],
    case when A.[Status]=''revision'' then 1 else 0 end AS [На доработке],
    case when A.[Status]=''consideration'' then 1 else 0 end AS [На согласовании],
    case when A.[Status]=''deactivated'' then 1 else 0 end AS [Отключен]
    
  FROM dbo.Account A 
  INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
  INNER JOIN Organization2010 Org ON Org.Id=OReq.OrganizationId 
  INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
  INNER JOIN  (SELECT DISTINCT AccountID
    FROM dbo.AccountLog 
    WHERE (IsStatusChange=1 OR (Status=''registration'' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
  ) F ON A.ID=F.AccountID
  UNION ALL
  SELECT 0,0,0,0,0,0
) T  
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
''Из них аккредитованных'',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF_New (@periodBegin ,@periodEnd)

return
END
' 
END
GO
