-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (39, '039__2011_11_14__CheckHistory')
-- =========================================================================
GO

-- новая роль
IF NOT EXISTS (SELECT * FROM dbo.Role WHERE Code = 'ViewCertificateCheckHistory')
BEGIN
	SET IDENTITY_INSERT dbo.Role ON
		INSERT INTO dbo.Role
		        ( Id, Code, Name )
		VALUES  (
				  41,
				  'ViewCertificateCheckHistory', -- Code - nvarchar(255)
		          'Просмотр страницы истории проверок сертификата'  -- Name - nvarchar(255)
		          )
	SET IDENTITY_INSERT dbo.Role OFF
END 
GO

-- процедура выборки проверок сертификатов

/****** Object:  StoredProcedure [dbo].[SearchCommonNationalExamCertificateCheckHistory]    Script Date: 11/25/2011 16:55:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchCommonNationalExamCertificateCheckHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchCommonNationalExamCertificateCheckHistory]
GO

-- получить все уникальные проверки сертификата вузами и их филиалами
CREATE proc [dbo].[SearchCommonNationalExamCertificateCheckHistory]
	@certificateId BIGINT,  -- id сертификата
	@startRow INT = NULL,	-- пейджинг
	@maxRow INT = NULL		-- пейджинг
AS
BEGIN

-- выбрать число организаций проверявших сертификат
IF (@startRow IS NULL)
BEGIN 
	SELECT COUNT(DISTINCT org.Id) FROM dbo.CheckCommonNationalExamCertificateLog lg INNER JOIN dbo.CommonNationalExamCertificate c ON c.Number = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.Id = @certificateId
	RETURN
END
SELECT   @certificateId AS CertificateId, * FROM (
			select org.Id AS OrganizationId,
			org.FullName AS OrganizationFullName,
			lg.Date AS [Date],
			lg.IsBatch AS CheckType,
			DENSE_RANK() OVER(ORDER BY org.FullName) AS org
			FROM dbo.CheckCommonNationalExamCertificateLog lg INNER JOIN dbo.CommonNationalExamCertificate c ON c.Number = lg.CertificateNumber
				INNER JOIN dbo.Account ac ON ac.Id = lg.AccountId 
				INNER JOIN dbo.Organization2010 org ON org.Id = ac.OrganizationId
				WHERE org.TypeId = 1 AND c.Id = @certificateId ) rowTable
			WHERE org BETWEEN @startRow + 1 AND @startRow + @maxRow
			ORDER BY org, rowTable.Date 
END
