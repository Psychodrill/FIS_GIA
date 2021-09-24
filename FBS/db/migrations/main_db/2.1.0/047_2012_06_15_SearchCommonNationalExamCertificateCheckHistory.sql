-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (47, '047_2012_06_15_SearchCommonNationalExamCertificateCheckHistory.sql')
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckHistory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- получить все уникальные проверки сертификата вузами и их филиалами
CREATE proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
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
				WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0
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
				WHERE org.TypeId = 1 AND c.Id = @certificateId and org.DisableLog = 0) rowTable 
			WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow 
			ORDER BY org, rowTable.[Date] 
END

GO


