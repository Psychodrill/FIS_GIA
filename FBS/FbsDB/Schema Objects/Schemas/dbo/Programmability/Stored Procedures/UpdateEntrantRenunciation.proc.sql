
/*
 перед запуском ХП должна быть создана временнная таблица
	create table #EntrantRenunciation
		(
		LastName nvarchar(255)
		, FirstName nvarchar(255)
		, PatronymicName nvarchar(255)
		, PassportNumber nvarchar(255)
		, PassportSeria nvarchar(255)
		)
*/
-- exec dbo.UpdateEntrantRenunciation
-- =======================================================
-- Добавляет данные в dbo.EntrantRenunciation.
-- v.1.0: Created by Makarev Andrey 26.06.2008
-- v.1.1: Modified by Fomin Dmitriy 27.06.2008
-- Поле Year - текущий год.
-- ========================================================
CREATE procedure dbo.UpdateEntrantRenunciation
	@login nvarchar(255)
	, @ip nvarchar(255)
as
begin
	declare 
		@currentDate datetime
		, @accountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @organizationId bigint
		, @year int

	set @updateId = NewId()
	set @currentDate = GetDate()
	set @eventCode = 'ENT_REN_EDIT'
	set @year = Year(GetDate())
	
	select
		@accountId = account.[Id]
		, @organizationId = account.OrganizationId
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	if not exists(select 1 
			from dbo.Organization organization
			where organization.Id = @organizationId)
		return 0 

	declare @EntrantRenunciation table
		(
		LastName nvarchar(255)
		, FirstName nvarchar(255)
		, PatronymicName nvarchar(255)
		, PassportNumber nvarchar(255)
		, PassportSeria nvarchar(255)
		, EntrantRenunciationId bigint
		)

	insert @EntrantRenunciation
	select 
		entrant_renunciation.LastName 
		, entrant_renunciation.FirstName 
		, entrant_renunciation.PatronymicName
		, entrant_renunciation.PassportNumber
		, entrant_renunciation.PassportSeria
		, old_entrant_renunciation.Id
	from (select distinct
			isnull(entrant_renunciation.LastName, '') LastName 
			, isnull(entrant_renunciation.FirstName, '') FirstName 
			, isnull(entrant_renunciation.PatronymicName, '') PatronymicName
			, isnull(entrant_renunciation.PassportNumber, '') PassportNumber
			, isnull(entrant_renunciation.PassportSeria, '') PassportSeria
		from #EntrantRenunciation entrant_renunciation) entrant_renunciation
		left outer join dbo.EntrantRenunciation old_entrant_renunciation
			on old_entrant_renunciation.[Year] = @year
				and old_entrant_renunciation.OwnerOrganizationId = @organizationId
				and old_entrant_renunciation.PassportNumber = entrant_renunciation.PassportNumber
				and old_entrant_renunciation.PassportSeria = entrant_renunciation.PassportSeria

	begin tran update_entrant_renunciation_tran

		insert dbo.EntrantRenunciation
			(
			CreateDate
			, UpdateDate
			, UpdateId
			, EditorAccountId
			, EditorIp
			, OwnerOrganizationId
			, [Year]
			, LastName
			, FirstName
			, PatronymicName
			, PassportNumber
			, PassportSeria
			)
		select
			@currentDate
			, @currentDate
			, @updateId
			, @accountId
			, @ip
			, @organizationId
			, @year
			, entrant_renunciation.LastName
			, entrant_renunciation.FirstName
			, entrant_renunciation.PatronymicName
			, entrant_renunciation.PassportNumber
			, entrant_renunciation.PassportSeria
		from
			@EntrantRenunciation entrant_renunciation
		where
			entrant_renunciation.EntrantRenunciationId is null

		if (@@error <> 0)
			goto undo

		update old_entrant_renunciation
		set
			UpdateDate = @currentDate
			, UpdateId = @updateId
			, EditorAccountId = @accountId
			, EditorIp = @ip
			, LastName = entrant_renunciation.LastName
			, FirstName = entrant_renunciation.FirstName
			, PatronymicName = entrant_renunciation.PatronymicName
		from
			dbo.EntrantRenunciation old_entrant_renunciation
				inner join @EntrantRenunciation entrant_renunciation
					on entrant_renunciation.EntrantRenunciationId = old_entrant_renunciation.Id
						and (old_entrant_renunciation.LastName <> entrant_renunciation.LastName
							or old_entrant_renunciation.FirstName <> entrant_renunciation.FirstName
							or old_entrant_renunciation.PatronymicName <> entrant_renunciation.PatronymicName)
		
		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran update_entrant_renunciation_tran

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = @eventCode
		, @sourceEntityIds = null
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:

	rollback tran update_entrant_renunciation_tran

	return 1
end
