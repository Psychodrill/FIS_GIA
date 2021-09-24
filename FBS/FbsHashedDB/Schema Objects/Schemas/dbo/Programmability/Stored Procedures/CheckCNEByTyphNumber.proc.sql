-- =============================================
-- Проверить сертификат ЕГЭ по номеру.
-- =============================================
CREATE proc [dbo].[CheckCNEByTyphNumber]
	  @checkTypographicNumber nvarchar(255) = null
	, @checkLastName nvarchar(255) = null
	, @checkFirstName nvarchar(255) = null
	, @checkPatronymicName nvarchar(255) = null
	, @checkSubjectMarks nvarchar(max) = null
as
begin 
	SET NOCOUNT ON 
	DECLARE @Res TABLE
	(
		CertificateNumber NVARCHAR(255),
		TypographicNumber NVARCHAR(255),
		RegionName NVARCHAR(255),
		[Year] INT,
		[Status] NVARCHAR(255),
		IsExist BIT,
		IsDeny BIT,
		DenyComment NVARCHAR(255),
		SubjectName NVARCHAR(255),
		SubjectMark NVARCHAR(20),
		CheckSubjectMark NVARCHAR(20),
		SubjectMarkCorrect BIT,
		HasAppeal BIT
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

	INSERT INTO @Src (CNEId,Number,RegionId,TyphNumber,[Year],LastName,FirstName,PatrName)
	SELECT CNE.Id,CNE.Number,CNE.RegionId,CNE.TypographicNumber,CNE.[Year],CNE.LastName,CNE.FirstName,CNE.PatronymicName
	FROM dbo.CommonNationalExamCertificate CNE 
	WHERE CNE.TypographicNumber=@checkTypographicNumber 

	DELETE Src FROM @Src  Src 
	WHERE Src.LastName <> @checkLastName
	OR (Src.FirstName <> @checkFirstName AND @checkFirstName IS NOT NULL)
	OR (Src.PatrName <> @checkPatronymicName AND @checkPatronymicName IS NOT NULL)

	DECLARE @CheckSubjects TABLE
	(
		SubjectId INT,
		Mark NVARCHAR(10)
	)
	INSERT INTO @CheckSubjects(SubjectId,Mark)
	SELECT * FROM dbo.GetSubjectMarks(@checkSubjectMarks)

	INSERT INTO @Res (CertificateNumber,TypographicNumber,RegionName,[Year],[Status],IsExist,IsDeny,DenyComment,
		SubjectName,SubjectMark,CheckSubjectMark,SubjectMarkCorrect,HasAppeal)	
	SELECT Src.Number,
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
	DeniedCNE.Comment,
	Subj.Name,
	CASE WHEN CNEMark.Mark< MM.MinimalMark THEN '!' ELSE '' END + REPLACE(CAST(CNEMark.Mark AS NVARCHAR(9)),'.',','),
	CASE WHEN CNEMarkIn.Mark< MM.MinimalMark THEN '!' ELSE '' END + REPLACE(CAST(CNEMarkIn.Mark AS NVARCHAR(9)),'.',','),
	CASE WHEN CNEMark.Mark=CNEMarkIn.Mark THEN 1 ELSE 0 END,
	CNEMark.HasAppeal
	FROM @Src Src 
	LEFT JOIN dbo.Region Reg ON Reg.Id=Src.RegionId
	LEFT JOIN dbo.ExpireDate ED ON ED.[Year]=Src.[Year]
	LEFT JOIN dbo.CommonNationalExamCertificateDeny DeniedCNE ON Src.Number=DeniedCNE.CertificateNumber AND Src.[Year]=DeniedCNE.[Year]
	LEFT JOIN dbo.CommonNationalExamCertificateSubject CNEMark ON Src.CNEId=CNEMark.CertificateId
	LEFT JOIN @CheckSubjects CNEMarkIn ON CNEMark.SubjectId=CNEMarkIn.SubjectId
	LEFT JOIN dbo.Subject Subj ON Subj.Id=CNEMark.SubjectId
	LEFT JOIN MinimalMark as MM on CNEMarkIn.SubjectId = MM.SubjectId and MM.[Year] = YEAR(GETDATE())
	
	SELECT * FROM @Res
	SET NOCOUNT OFF

	return 0
end