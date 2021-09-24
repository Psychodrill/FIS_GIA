-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (10, '010__2011_01_31__ReportStatisticSubordinateOrg')
-- =========================================================================




IF object_id (N'dbo.ReportStatisticSubordinateOrg', N'TF') is not null
	DROP FUNCTION dbo.ReportStatisticSubordinateOrg;
GO

CREATE FUNCTION  [dbo].[ReportStatisticSubordinateOrg](
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

--�������� �� ������
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

--�������� �� ������������� ������
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


--�������� �� ��������
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

--�������� �������������
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


IF object_id (N'dbo.ReportXMLSubordinateOrg', N'TF') is not null
	DROP FUNCTION [dbo].[ReportXMLSubordinateOrg];
GO

--������� �� ���������������� ����������� ��� �������� � XML
CREATE FUNCTION  [dbo].[ReportXMLSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	[��� ��] int null,
	[������ ������������] nvarchar(Max) null,
	[��� �������] int null,
	[������������ �������] nvarchar(255) null,
	[������������� �� ������������] nvarchar(255) null,
	[��� ������������] nvarchar(255) null,
	[���������� �������������] int null,
	[���� ��������� ������������] datetime null,
	[���������� ���������� ��������] int null
)
AS BEGIN
INSERT INTO @Report
SELECT
	Id [��� ��],
	FullName [������ ������������],
	RegionId [��� �������] ,
	RegionName [������������ �������],
	AccreditationSertificate [������������� �� ������������],
	DirectorFullName [��� ������������],
	CountUser [���������� �������������],
	UserUpdateDate [���� ���������],
	CountUniqueChecks [���������� ��������]
FROM
	dbo.ReportStatisticSubordinateOrg ( @periodBegin, @periodEnd, @departmentId)
RETURN
END
GO


--��������� ���� ���� �����������, ����������
ALTER function [dbo].[ReportOrgsInfoByRegionTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL
	, @arg NVARCHAR(50) = NULL)
RETURNS @report TABLE 
(
[������ ������������] NVARCHAR(4000) NULL
,[������� ������������] NVARCHAR(2000) null
,[���� ��������] datetime null
,[������� �� �����������] NVARCHAR(20) null
,[������������ �������] nvarchar(255) null
,[��� �������] nvarchar(255) null
,[���] nvarchar(255) null
,[���] nvarchar(255) null
,[���] nvarchar(50) null
,[������] nvarchar(50) null
,[������������ �� �����������] NVARCHAR(20) null
,[������������� �� ������������] nvarchar(255) null
,[������������ �� �����] nvarchar(255) null
,[��� ������������] nvarchar(255) null
,[��������� ������������] nvarchar(255) null
,[������������� ��������������] nvarchar(500) null
,[��� ����������] int null
,[����������� �����] nvarchar(255) null
,[����������� �����] nvarchar(255) null
,[���] nvarchar(10) null
,[����] nvarchar(13) null
,[��� ������] nvarchar(255) null
,[�������] nvarchar(255) null
,[EMail] nvarchar(255) null
,[���������� �������������] INT NULL
,[������������� ������������] INT NULL
,[������������� �� ������������] INT NULL
,[������������� ���������] INT NULL
,[������������� �� �����������] INT NULL
,[������������� �� ���������] INT NULL
,[������ ���������������] DATETIME NULL
,[��������� ���������������] DATETIME NULL
,[���������� �������� �� ������] int null
,[���������� ���������� �������� �� ������] INT NULL
,[���������� �������� �� ���������� ������] INT NULL
,[���������� ���������� �������� �� ���������� ������] INT NULL
,[���������� �������� �� ������������� ������] INT NULL
,[���������� ���������� �������� �� ������������� ������] INT NULL
,[���������� ������������� ��������] INT NULL
,[���������� ���������� ������������� ��������] INT NULL
,[���������� ������������ ��������] INT NULL
,[������ ��������] datetime null
,[��������� ��������] datetime null
,[������ � ���] NVARCHAR(20)
,[��� ��] int null
,[��� ��������� ��] int null
)
AS 
BEGIN

--���� �� ���������� ��������� �������, �� ����������� ���������� = 1 ������
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
Org.FullName AS [������ ������������]
,ISNULL(Org.ShortName,'') AS [������� ������������]
,Org.CreateDate AS [���� ��������]
,REPLACE(REPLACE(Org.WasImportedAtStart,1,'��'),0,'���') AS [������� �� �����������]
,Reg.[Name] AS [��� �������]
,Reg.Code AS [��� �������]
,OrgType.[Name] AS [���]
,OrgKind.[Name] AS [���]
,REPLACE(REPLACE(Org.IsPrivate,1,'�������'),0,'���-���') AS [���]
,REPLACE(REPLACE(Org.IsFilial,1,'��'),0,'���') AS [������]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN '���'
	ELSE '����'
	END AS [������������ �� �����������]
,ISNULL(Org.AccreditationSertificate,'') AS [������������� �� ������������]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN '����'
	ELSE '���'
	END AS [������������ �� �����] 	
,Org.DirectorFullName AS [��� ������������]
,Org.DirectorPosition AS [��������� ������������]
,Org.OwnerDepartment AS [������������� ��������������]
,Org.DepartmentId AS [��� ����������]
,Org.FactAddress AS [����������� �����]
,Org.LawAddress AS [����������� �����]
,Org.INN AS [���]
,Org.OGRN AS [����]
,Org.PhoneCityCode AS [��� ������] 
,Org.Phone AS [�������] 
,Org.EMail AS [EMail]  

,ISNULL(UsersCnt.UsersCount,0) AS [���������� �������������]
,ISNULL(StatusesCnt.activated,0) AS [������������� ������������]
,ISNULL(StatusesCnt.consideration,0) AS [������������� �� ������������]
,ISNULL(StatusesCnt.deactivated,0) AS [������������� ���������]
,ISNULL(StatusesCnt.registration,0) AS [������������� �� �����������]
,ISNULL(StatusesCnt.revision,0) AS [������������� �� ���������]
,CreationDates.FirstCreated AS [������ ���������������]
,CreationDates.LastCreated AS [��������� ���������������]

,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [���������� ���������� �������� �� ������] 
,ISNULL(NumberChecks.TotalNumberChecks,0) AS[���������� �������� �� ������]  
,ISNULL(PassportChecks.TotalPassportChecks,0) AS [���������� �������� �� ���������� ������]  
,ISNULL(PassportChecks.UniquePassportChecks,0) AS [���������� ���������� �������� �� ���������� ������]  
,ISNULL(TNChecks.TotalTNChecks,0) AS [���������� �������� �� ������������� ������] 
,ISNULL(TNChecks.UniqueTNChecks,0) AS [���������� ���������� �������� �� ������������� ������] 
,ISNULL(UIChecks.TotalUIChecks,0) AS [���������� ������������� ��������]
,ISNULL(UIChecks.UniqueUIChecks,0) AS [���������� ���������� ������������� ��������]
,ISNULL(WrongChecks.WrongChecks,0) AS [���������� ������������ ��������]

,LimitDates.FirstCheck AS [������ ��������]  
,LimitDates.LastCheck AS [��������� ��������] 

,CASE WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
= 0 
THEN '�� ��������'
WHEN 
ISNULL(NumberChecks.UniqueNumberChecks,0)
+ISNULL(PassportChecks.UniquePassportChecks,0)
+ISNULL(TNChecks.UniqueTNChecks,0)
+ISNULL(UIChecks.UniqueUIChecks,0) 
< 10 
THEN '������ ���������'
ELSE '��������'
END
AS [������ � ���],
Org.Id [��� ��],
Org.MainId [��� ��������� ��]

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