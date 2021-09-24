CREATE FUNCTION [dbo].[ReportCheckStatisticsTVF](@periodBegin datetime,@periodEnd datetime)
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
isnull(r.code,'') [Код региона]
,isnull(r.name,'Не указан') [Регион]

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
	AND ChLog.TypeCode= 'CNENumber'
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
	AND ChLog.TypeCode= 'Passport'
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
	AND ChLog.TypeCode= 'Typographic'
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
	AND ChLog.TypeCode= 'Marks'
	GROUP BY OReq.regionid
	) iMarks on iMarks.regionid = r.id
group by r.code, r.name
	order by max(r.id)


DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)
insert into @report
select '', 'Итого за ' + case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end 
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
	AND ChLog.TypeCode= 'CNENumber'
	
	DECLARE @PassportUnique_UI INT
	SELECT @PassportUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR, ChLog.FoundedCNEId))
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Passport'
	
	DECLARE @TypNumberUnique_UI INT
	SELECT @TypNumberUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+ CONVERT(NVARCHAR,ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Typographic'
	
	DECLARE @MarksUnique_UI INT
	SELECT @MarksUnique_UI=COUNT(DISTINCT CONVERT(NVARCHAR,OReq.OrganizationId)+CONVERT(NVARCHAR, ChLog.FoundedCNEId) )
	FROM dbo.CNEWebUICheckLog   ChLog with(nolock)
	INNER JOIN Account Acc WITH(NOLOCK) ON Acc.Id=ChLog.AccountId
	INNER JOIN GroupAccount GA WITH(NOLOCK) ON GA.AccountId = Acc.Id AND GA.GroupId = 1
	INNER JOIN OrganizationRequest2010 OReq ON Acc.OrganizationId=OReq.Id AND OReq.OrganizationId IS NOT NULL

	WHERE ChLog.FoundedCNEId IS NOT NULL
	AND ChLog.TypeCode= 'Marks'

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
select '', 'Итого за все время'
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

end