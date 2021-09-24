-- exec dbo.UpdateEntrant

/*
Перед запуском ХП должна быть создана временнная таблица
	create table #Entrant 
		(
		LastName nvarchar(255)
		, FirstName nvarchar(255)
		, PatronymicName nvarchar(255)
		, CertificateNumber nvarchar(255)
		, PassportNumber nvarchar(255)
		, PassportSeria nvarchar(255)
		, GIFOCategoryName nvarchar(255)
		, DirectionCode nvarchar(255)
		, SpecialtyCode nvarchar(255)
		)
*/
-- =======================================================
-- Добавляет данные в dbo.Entrant.
-- v.1.0: Created by Makarev Andrey 26.06.2008
-- v.1.1: Modified by Fomin Dmitriy 27.06.2008
-- Поле Year - текущий год.
-- ========================================================
CREATE procedure dbo.UpdateEntrant
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
	set @eventCode = 'ENT_EDIT'
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

	declare @Entrant table
		(
		LastName nvarchar(255)
		, FirstName nvarchar(255)
		, PatronymicName nvarchar(255)
		, CertificateNumber nvarchar(255)
		, PassportNumber nvarchar(255)
		, PassportSeria nvarchar(255)
		, GIFOCategoryName nvarchar(255)
		, DirectionCode nvarchar(255)
		, SpecialtyCode nvarchar(255)
		, EntrantId bigint
		)

	insert @Entrant
	select 
		entrant.LastName
		, entrant.FirstName
		, entrant.PatronymicName
		, entrant.CertificateNumber
		, entrant.PassportNumber
		, entrant.PassportSeria
		, entrant.GIFOCategoryName
		, entrant.DirectionCode
		, entrant.SpecialtyCode
		, old_entrant.Id
	from
		(select distinct
			isnull(entrant.LastName, '') LastName
			, isnull(entrant.FirstName, '') FirstName
			, isnull(entrant.PatronymicName, '') PatronymicName
			, entrant.CertificateNumber CertificateNumber
			, entrant.PassportNumber PassportNumber
			, isnull(entrant.PassportSeria, '') PassportSeria
			, entrant.GIFOCategoryName GIFOCategoryName
			, isnull(entrant.DirectionCode, '') DirectionCode
			, isnull(entrant.SpecialtyCode, '') SpecialtyCode
		from #Entrant entrant) entrant
			left outer join dbo.Entrant old_entrant
				on old_entrant.[Year] = @year
					and old_entrant.OwnerOrganizationId = @organizationId
					and (old_entrant.CertificateNumber = entrant.CertificateNumber
						or (old_entrant.CertificateNumber is null
							and entrant.CertificateNumber is null
							and old_entrant.PassportNumber = entrant.PassportNumber
							and old_entrant.PassportSeria = entrant.PassportSeria))

	begin tran update_entrant_tran

		insert dbo.Entrant
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
			, CertificateNumber
			, PassportNumber
			, PassportSeria
			, GIFOCategoryName
			, DirectionCode
			, SpecialtyCode
			)
		select
			@currentDate
			, @currentDate
			, @updateId
			, @accountId
			, @ip
			, @organizationId
			, @year
			, entrant.LastName
			, entrant.FirstName
			, entrant.PatronymicName
			, entrant.CertificateNumber
			, entrant.PassportNumber
			, entrant.PassportSeria
			, entrant.GIFOCategoryName
			, entrant.DirectionCode
			, entrant.SpecialtyCode
		from
			@Entrant entrant
		where
			entrant.EntrantId is null

		if (@@error <> 0)
			goto undo

		update old_entrant
		set
			UpdateDate = @currentDate
			, UpdateId = @updateId
			, EditorAccountId = @accountId
			, EditorIp = @ip
			, LastName = entrant.LastName
			, FirstName = entrant.FirstName
			, PatronymicName = entrant.PatronymicName
			, PassportNumber = entrant.PassportNumber
			, PassportSeria = entrant.PassportSeria
			, GIFOCategoryName = entrant.GIFOCategoryName
			, DirectionCode = entrant.DirectionCode
			, SpecialtyCode = entrant.SpecialtyCode
		from
			dbo.Entrant old_entrant
				inner join @Entrant entrant
					on entrant.EntrantId = old_entrant.Id
						and (old_entrant.PassportNumber <> entrant.PassportNumber
								or old_entrant.PassportSeria <> entrant.PassportSeria
								or old_entrant.LastName <> entrant.LastName
								or old_entrant.FirstName <> entrant.FirstName
								or old_entrant.PatronymicName <> entrant.PatronymicName
								or isnull(old_entrant.GIFOCategoryName, '') <> isnull(entrant.GIFOCategoryName, '')
								or old_entrant.DirectionCode <> entrant.DirectionCode
								or old_entrant.SpecialtyCode <> entrant.SpecialtyCode)
		
		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran update_entrant_tran

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = @eventCode
		, @sourceEntityIds = null
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:

	rollback tran update_entrant_tran

	return 1
end
