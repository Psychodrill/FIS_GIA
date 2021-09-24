
-- =============================================
-- Триггер проверки сертификатов ЕГЭ на вставку.
-- v.1.0: Created by Makarev Andrey 04.05.2008
-- v.1.1: Modified by Makarev Andrey 05.05.2008
-- Добавление одним запросом.
-- v.1.2: Modified by Makarev Andrey 05.05.2008
-- Изменен алиас.
-- v.1.3: Modified by Valeev Denis 20.05.2009
-- Оптимизация
-- =============================================
CREATE trigger [dbo].[tgCommonNationalExamCertificateCheck]
on [dbo].[EventLog]
for insert
as
	insert into dbo.CommonNationalExamCertificateCheckLog
			(
			Date
			, AccountId
			, Ip
			, PeriodName
			, [Count]
			, IsBatch
			)
		select
			inserted_event.Date
			, inserted_event.AccountId
			, inserted_event.Ip
			, null
			, 1
			, case 
				when inserted_event.EventCode like 'CNE_BCH_%' then 1
				else 0
			end
		from 
			inserted inserted_event
		where
			inserted_event.EventCode like 'CNE_%'

