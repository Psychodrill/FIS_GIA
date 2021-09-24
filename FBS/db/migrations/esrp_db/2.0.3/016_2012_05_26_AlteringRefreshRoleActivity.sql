-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (16, '016_2012_05_26_AlteringRefreshRoleActivity')
-- =========================================================================
PRINT N'Altering [dbo].[RefreshRoleActivity]...';
GO
-- =============================================
-- Обновить активность ролей.
-- v.1.0: Created by Fomin Dmitriy 13.06.2008
-- v.1.1: Modified by Makarev Andrey 23.06.2008
-- Добавлен параметр @accountLogin.
-- =============================================
ALTER proc dbo.RefreshRoleActivity
	@accountId bigint = null
	, @accountLogin nvarchar(255) = null
as
begin
	declare
		@checkAccountId bigint
		, @checkRoleId int
		, @condition nvarchar(max)
		, @commandText nvarchar(max)

	declare @checkingAccount table
		(
		AccountId bigint
		, UpdateDate datetime
		)
	
	if @accountId is null
	begin
		if @accountLogin is null
			insert into @checkingAccount
			select
				account.Id
				, Account.UpdateDate
			from 
				dbo.Account account
			where 
				account.IsActive = 1
		else
			insert into @checkingAccount
			select
				account.Id
				, Account.UpdateDate
			from 
				dbo.Account account
			where 
				account.IsActive = 1
				and account.Login = @accountLogin
	end
	else
	begin
		if @accountLogin is null
			insert into @checkingAccount
			select
				account.Id
				, account.UpdateDate
			from dbo.Account account
			where account.IsActive = 1
				and account.Id = @accountId
		else
			insert into @checkingAccount
			select
				account.Id
				, account.UpdateDate
			from 
				dbo.Account account
			where 
				account.IsActive = 1
				and account.Id = @accountId
				and account.Login = @accountLogin				
	end

	create table #Activity 
		(
		AccountId bigint
		, RoleId int
		)

	declare activity_cursor cursor forward_only for 
	select
		account_role.AccountId
		, account_role.RoleId
		, account_role.IsActiveCondition
	from dbo.AccountRole account_role 
	where not account_role.IsActiveCondition is null
		and account_role.AccountId in (select AccountId from @checkingAccount)

	open activity_cursor
	fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
	while @@fetch_status <> -1
	begin
		set @commandText = replace(replace(replace(
			'insert into #Activity 
			select
				activity.AccountId
				, <roleId> RoleId
			from (select 
					account.Id AccountId
					, case
						when <condition> then 1
						else 0
					end IsActive
				from dbo.Account account 
				where account.Id = <accountId>) activity 
			where activity.IsActive = 1 '
			, '<accountId>', @checkAccountId)
			, '<roleId>', @checkRoleId)
			, '<condition>', @condition)

		exec (@commandText)

		fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
	end
	close activity_cursor
	deallocate activity_cursor

	if exists(select 1
			from (select
						account_activity.RoleId
						, account_activity.AccountId
					from dbo.AccountRoleActivity account_activity
					where account_activity.AccountId in (select AccountId from @checkingAccount)) account_activity
				full outer join #Activity activity
					on account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId)
	begin
		begin tran activity
			delete account_activity
			from dbo.AccountRoleActivity account_activity
			where 
				account_activity.AccountId in (select AccountId from @checkingAccount)
				and not exists(select 1
					from #Activity activity
					where account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId)

			update account_activity
			set UpdateDate = GetDate()
			from dbo.AccountRoleActivity account_activity with(rowlock)
				inner join @checkingAccount account
					on account.AccountId = account_activity.AccountId
				inner join #Activity activity
					on account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId
			where
				account.UpdateDate > account_activity.UpdateDate

			insert into dbo.AccountRoleActivity
				(
				CreateDate
				, UpdateDate
				, AccountId
				, RoleId
				)
			select
				GetDate()
				, GetDate()
				, activity.AccountId
				, activity.RoleId
			from #Activity activity
			where not exists(select 1
					from dbo.AccountRoleActivity account_activity
					where account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId)
		if @@trancount > 0 						
			commit tran activity
	end

	drop table #Activity 

	return 0
end
GO