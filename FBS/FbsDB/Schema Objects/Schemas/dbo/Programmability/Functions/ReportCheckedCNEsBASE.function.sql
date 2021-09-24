







CREATE function [dbo].[ReportCheckedCNEsBASE](
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
