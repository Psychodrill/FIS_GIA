-- exec dbo.CheckNewUserAccountEmail
-- =============================================
-- Проверка email нового пользователя 
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- =============================================
CREATE procedure dbo.CheckNewUserAccountEmail
	@email nvarchar(255)
as
begin
	declare 
		@userGroupCode nvarchar(255)
		, @isValid bit
		, @currentYear int
		, @activatedStatus  nvarchar(255)

	set @userGroupCode = 'User'
	set @activatedStatus = 'activated'
	set @currentYear = Year(GetDate())

	if exists(select 1
			from 
				dbo.Account account with (nolock)
					inner join dbo.GroupAccount group_account with (nolock) 
						inner join dbo.[Group] [group] with (nolock)
							on [group].Id = group_account.GroupId
						on group_account.AccountId = account.Id
			where
				[group].Code = @userGroupCode
				and account.Email = @email
				and dbo.GetUserStatus(account.ConfirmYear, account.Status, @currentYear
					, account.RegistrationDocument) = @activatedStatus)
		set @isValid = 0
	else 
		set @isValid = 1

	select
		@email Email
		, @isValid IsValid
return 0
end
