-- =============================================
-- Получить сертификат ЕГЭ.
-- =============================================
CREATE proc [dbo].[CheckCNEByPassport]
	@lastName nvarchar(255) 
	, @firstName nvarchar(255) = null
	, @patronymicName nvarchar(255) = null
	, @passportSeria nvarchar(255) 
	, @passportNumber nvarchar(255)
as
begin
	SET NOCOUNT ON 
	DECLARE @Res TABLE
	(
		CertificateNumber NVARCHAR(255),
		LastName NVARCHAR(255),
		TypographicNumber NVARCHAR(255),
		RegionName NVARCHAR(255),
		[Year] INT,
		[Status] NVARCHAR(255),
		IsExist BIT,
		IsDeny BIT,
		DenyComment NVARCHAR(255)
	)

	DECLARE @Src TABLE
	(
		CNEId BIGINT,
		Number NVARCHAR(255),
		RegionId INT,
		TyphNumber NVARCHAR(255),
		[Year] INT,
		LastName NVARCHAR(255),
		FirstName NVARCHAR(255),
		PatrName NVARCHAR(255)
	)
	
	DECLARE @InternalPassportSeria NVARCHAR(255) 
	SET @InternalPassportSeria= dbo.GetInternalPassportSeria (@passportSeria)

	INSERT INTO @Src (CNEId,Number,RegionId,TyphNumber,[Year],LastName,FirstName,PatrName)
	SELECT CNE.Id,CNE.Number,CNE.RegionId,CNE.TypographicNumber,CNE.[Year],CNE.LastName,CNE.FirstName,CNE.PatronymicName
	FROM dbo.CommonNationalExamCertificate CNE 
	WHERE CNE.InternalPassportSeria=@InternalPassportSeria 
	AND CNE.PassportNumber=@passportNumber 
	
	DELETE Src FROM @Src  Src 
	WHERE Src.LastName <> @lastName
	OR (Src.FirstName <> @firstName AND @firstName IS NOT NULL)
	OR (Src.PatrName <> @patronymicName AND @patronymicName IS NOT NULL)

	INSERT INTO @Res (CertificateNumber,LastName,TypographicNumber,RegionName,[Year],[Status],IsExist,IsDeny,DenyComment)	
	SELECT Src.Number,
	Src.LastName,
	Src.TyphNumber,
	Reg.Name,
	Src.[Year],
	CASE WHEN ED.ExpireDate IS NULL THEN 'Не найдено' ELSE
		CASE WHEN DeniedCNE.Id IS NULL AND GETDATE() <= ED.ExpireDate THEN 'Действительно'
		ELSE 'Истек срок' END
	END,
	1,
	CASE WHEN DeniedCNE.Id IS NOT NULL
	THEN 1 
	ELSE 0
	END,
	DeniedCNE.Comment
	FROM @Src Src 
	LEFT JOIN dbo.Region Reg ON Reg.Id=Src.RegionId
	LEFT JOIN dbo.ExpireDate ED ON ED.[Year]=Src.[Year]
	LEFT JOIN dbo.CommonNationalExamCertificateDeny DeniedCNE ON Src.Number=DeniedCNE.CertificateNumber AND Src.[Year]=DeniedCNE.[Year]
	
	SELECT * FROM @Res
	SET NOCOUNT OFF
	return 0
end