
-- ================================================
-- Тригер на добавление события CNE_CHK в EventLog
-- v.1.0: Created by Sedov Anton 22.07.2008
-- v.1.1: Modified by Fomin Dmitriy 11.09.2008
-- Ускорение.
-- v.1.2: Modified by Valeev Denis 20.05.2009
-- Оптимизация
-- ================================================
create trigger [dbo].[tgCheckCommonNationalExamCertificateLog]
on [dbo].[EventLog] 
for insert
as 
begin
	insert into dbo.CheckCommonNationalExamCertificateLog
		(
		Date
		, AccountId
		, CertificateNumber
		, IsBatch
		, IsExist
		) 
	select 
		inserted_event.Date Date
		, inserted_event.AccountId AccountId
		, dbo.GetEventParam(inserted_event.EventParams, 1) CertificateNumber
		, 0 IsBatch
		, case
			when not inserted_event.SourceEntityId is null then 1
			else 0
		end IsExist 
	from 
		Inserted inserted_event
	where 
		inserted_event.EventCode = 'CNE_CHK'
		and exists(select * from Account a join Organization2010 b on a.OrganizationId=b.id and b.DisableLog=0 and a.id=inserted_event.AccountId)
end

