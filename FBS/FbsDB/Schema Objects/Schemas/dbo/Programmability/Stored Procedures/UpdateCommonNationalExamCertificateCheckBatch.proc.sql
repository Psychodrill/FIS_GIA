﻿-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch

-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE proc [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
	@id bigint output
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @batch ntext
	, @filter nvarchar(255)
	, @type int=0
	, @outerId bigint=null
	, @year nvarchar(10)
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

	set @eventCode = N'CNE_BCH_CHK'

	begin tran insert_check_batch_tran

		insert dbo.CommonNationalExamCertificateCheckBatch
			(
			CreateDate
			, UpdateDate
			, OwnerAccountId
			, IsProcess
			, IsCorrect
			, Batch
			, [Filter]
			,[Type]
			,outerId
			, [Year]
			)
		select
			@currentDate
			, @currentDate
			, @accountId
			, 1
			, null
			, @batch
			, @filter
			,@type
			,@outerId
			, @year
			
		if (@@error <> 0)
			goto undo

		set @internalId = scope_identity()
		set @id = dbo.GetExternalId(@internalId)

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran insert_check_batch_tran

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

	rollback tran insert_check_batch_tran

	return 1

end
