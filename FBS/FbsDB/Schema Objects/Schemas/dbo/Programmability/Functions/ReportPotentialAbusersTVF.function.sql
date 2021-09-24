--Отчет: Отчет о потенциальных пользователях, осуществляющих перебор
CREATE FUNCTION [dbo].[ReportPotentialAbusersTVF](@periodBegin datetime,@periodEnd datetime)
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
			+ isnull(cnecr.firstname,'') 
			+ isnull(cnecr.PatronymicName,'')
			+ isnull(cnecr.PassportSeria,'')
			+ isnull(cnecr.PassportNumber,'')
			+ isnull(cnecr.TypographicNumber,'')
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
			+ isnull(cnecc.firstname,'') 
			+ isnull(cnecc.PatronymicName,'')
			+ isnull(cnecc.PassportSeria,'')
			+ isnull(cnecc.PassportNumber,'')
			+ isnull(cnecc.TypographicNumber,'')
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


