 
-- exec dbo.CheckLastAccountIp
-- ====================================================
-- Процедура проверки последнего адресса пользователя,
-- под которым он авторизовался
-- v.1.0: Created by Sedov Anton 08.07.2008
-- v.1.1: Modified by Fomin Dmitriy 28.08.2008
-- Добавлена регистрация события авторизации.
-- ====================================================
CREATE procedure dbo.CheckLastAccountIp 
	@accountLogin nvarchar(255)
	, @ip nvarchar(255)
as
begin
	declare
		@isLastIp bit
		, @entityParams nvarchar(1000)
		, @sourceEntityIds nvarchar(255)
		, @accountId bigint

	set @isLastIp = null

	select top 1
		@isLastIp = case when auth_event_log.Ip = @ip
				then 1
			else 0
		end
		, @accountId = account.Id
	from 
		dbo.AuthenticationEventLog auth_event_log
			left join dbo.Account account
				on account.Id = auth_event_log.AccountId
	where
		account.[Login] = @accountLogin
			and auth_event_log.IsPasswordValid = 1
			and auth_event_log.IsIpValid = 1
	order by 
		auth_event_log.Date desc
		

	select
		@accountLogin AccountLogin
		, @ip Ip
		, isnull(@isLastIp, 0) IsLastIp						

	set @entityParams = @accountLogin + N'|' +
			@ip + N'||' +
			convert(nvarchar, case 
					when @isLastIp is null then 0 
					else 1 
				end)  + '|' +
			convert(nvarchar, isnull(@isLastIp, 0))

	set @sourceEntityIds = convert(nvarchar(255), @accountId)

	if isnull(@isLastIp, 0) = 1
		exec dbo.RegisterEvent 
			@accountId = @accountId
			, @ip = @ip
			, @eventCode = N'USR_VERIFY'
			, @sourceEntityIds = @sourceEntityIds
			, @eventParams = @entityParams
			, @updateId = null

	return 0
end
