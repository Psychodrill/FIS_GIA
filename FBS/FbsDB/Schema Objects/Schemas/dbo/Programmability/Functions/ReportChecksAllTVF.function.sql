create function [dbo].[ReportChecksAllTVF]()
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