CREATE function [dbo].[ReportTopCheckingOrganizationsTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[№] nvarchar(10) null,
	[Организация (за период)] nvarchar(500) null,
	[Всего проверок (за период)] int null,
	[Организация (за все время)] nvarchar(500) null,
	[Всего проверок (за все время)] int null	
)
as 
begin

DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

insert into @report
select '', 'ТОП 20 организаций за ' + case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end, null, 'ТОП 20 организаций за все время',null
insert into @report
select c.rowid [№], c.Организация, c.[Всего проверок], t.Организация, t.[Всего проверок]
from 
(
	select 
	row_number() over (order by [Всего проверок] desc) as rowid
	,*	
	from 
	(
	select top 20 
	o.FullName [Организация]
	,(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateCheckBatch] cb with(nolock)
	join [CommonNationalExamCertificateCheck] c with(nolock) on cb.id = c.batchid 
	join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
	where cb.updatedate BETWEEN @periodBegin and @periodEnd)
	+
	(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateRequestBatch] cb with(nolock)
	join [CommonNationalExamCertificateRequest] c with(nolock) on cb.id = c.batchid 
	join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
	where cb.updatedate BETWEEN @periodBegin and @periodEnd)
	[Всего проверок]
	from 
	OrganizationRequest2010 o with(nolock)
	order by [Всего проверок] desc
	) c2
) c 
full join (
select  
	row_number() over (order by [Всего проверок] desc) as rowid
	,*	
from 
	(
		select top 20	
		o.FullName [Организация]
		,(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateCheckBatch] cb with(nolock)
		join [CommonNationalExamCertificateCheck] c  with(nolock) on cb.id = c.batchid 
		join [Account] a  with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id
		)
		+
		(select count(distinct c.SourceCertificateId) from [CommonNationalExamCertificateRequestBatch] cb with(nolock)
		join [CommonNationalExamCertificateRequest] c with(nolock) on cb.id = c.batchid 
		join [Account] a with(nolock) on cb.owneraccountid = a.id and a.organizationid = o.id)
		[Всего проверок]
		from 
		OrganizationRequest2010 o with(nolock)
		order by [Всего проверок] desc
	)t2
) t on t.rowid = c.rowid
order by c.rowid asc


return
end