
-- exec dbo.UpdateCompetitionCertificateRequestBatch
-- =====================================================
-- Сохранение пакета в БД
-- v.1.0: Created by Sedov Anton 30.07.2008
-- v.1.1: Modified by Fomin Dmitriy 26.08.2008 
-- Переименование таблиц.
-- ======================================================
CREATE procedure dbo.UpdateCompetitionCertificateRequestBatch
	@id bigint output
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @batch ntext
as
begin
	declare 
		@currentDate datetime
		, @accountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @internalId bigint

	set @updateId = newid()
	
	set @currentDate = getdate()

	select
		@accountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	set @eventCode = N'SCC_BCH_CHK'

	begin tran insert_request_batch_tran

		insert dbo.CompetitionCertificateRequestBatch
			(
			CreateDate
			, UpdateDate
			, OwnerAccountId
			, IsProcess
			, IsCorrect
			, Batch
			)
		select
			@currentDate
			, @currentDate
			, @accountId
			, 1
			, null
			, @batch

		if (@@error <> 0)
			goto undo

		set @internalId = scope_identity()
		set @id = dbo.GetExternalId(@internalId)

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran insert_request_batch_tran

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_request_batch_tran

	return 1 
end
