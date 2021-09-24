-- получить все уникальные проверки сертификата вузами и их филиалами
alter proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
	@certificateId uniqueidentifier,  -- id сертификата
	@startRow INT = NULL,	-- пейджинг, если null - то выбирается кол-во записей для этого сертификата всего
	@maxRow INT = NULL		-- пейджинг
AS
BEGIN
-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
    
	SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg 
				INNER JOIN prn.Certificates c with(nolock) ON c.LicenseNumber = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.CertificateID = @certificateId and org.DisableLog = 0
	RETURN
END
SELECT   @certificateId AS CertificateId, * FROM (
			select org.Id AS OrganizationId,
			org.FullName AS OrganizationFullName,
			lg.[Date] AS [Date],
			lg.IsBatch AS CheckType,
			DENSE_RANK() OVER(ORDER BY org.FullName) AS org
			FROM dbo.CheckCommonNationalExamCertificateLog lg 
			    INNER JOIN prn.Certificates c with(nolock) ON c.LicenseNumber = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.CertificateID = @certificateId and org.DisableLog = 0) rowTable 
			WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow 
			ORDER BY org, rowTable.[Date] 
END
