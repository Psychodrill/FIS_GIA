
-- =============================================
-- Лог событий входов  в систему
-- v.1.0: Created by Sedov Anton 21.05.2008
-- =============================================
CREATE view dbo.AuthenticationEventLog with schemabinding 
as
select
	event_log.Id EventId 
	, event_log.Date Date
	, event_log.Ip Ip
	, event_log.SourceEntityId AccountId
	, case 
		when event_log.EventCode = 'USR_VERIFY'  
			then Convert(bit, dbo.GetEventParam(event_log.EventParams, 4)) 
			else 1
	end IsPasswordValid
	, case 
		when event_log.EventCode = 'USR_VERIFY'
			then Convert(bit, dbo.GetEventParam(event_log.EventParams, 5)) 
			else 1
	end IsIpValid
from   
	dbo.EventLog event_log
where 
	((event_log.EventCode = 'USR_VERIFY'
			and not event_log.AccountId is null)
		or (event_log.EventCode = 'USR_REG'
			and event_log.AccountId is null))
