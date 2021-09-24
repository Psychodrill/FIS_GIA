-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (20, '020__2011_04_13__BatchCheckFilter')
-- =========================================================================
GO



IF NOT EXISTS(select * from sys.columns where Name = N'Filter' and Object_ID = Object_ID(N'CommonNationalExamCertificateCheckBatch'))
BEGIN
	ALTER TABLE dbo.CommonNationalExamCertificateCheckBatch ADD Filter nvarchar(255) NULL
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateCommonNationalExamCertificateCheckBatch]    Script Date: 04/13/2011 11:04:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateCommonNationalExamCertificateCheckBatch

-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
ALTER proc [dbo].[UpdateCommonNationalExamCertificateCheckBatch]
	@id bigint output
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @batch ntext
	, @filter nvarchar(255)
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
			)
		select
			@currentDate
			, @currentDate
			, @accountId
			, 1
			, null
			, @batch
			, @filter

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
GO



IF NOT EXISTS(select * from sys.columns where Name = N'Filter' and Object_ID = Object_ID(N'CommonNationalExamCertificateRequestBatch'))
BEGIN
	ALTER TABLE dbo.CommonNationalExamCertificateRequestBatch ADD Filter nvarchar(255) NULL
END
GO


-- =============================================
-- Сохранить пакет проверок.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
ALTER proc [dbo].[UpdateCommonNationalExamCertificateRequestBatch]
	@id bigint output
	, @login nvarchar(255)
	, @ip nvarchar(255)
	, @batch ntext
	, @filter nvarchar(255)
	, @IsTypographicNumber bit
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

	set @eventCode = N'CNE_BCH_FND'

	begin tran insert_request_batch_tran

		insert dbo.CommonNationalExamCertificateRequestBatch
			(
			CreateDate
			, UpdateDate
			, OwnerAccountId
			, IsProcess
			, IsCorrect
			, Batch
			, [Filter]
			, IsTypographicNumber
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
			, @IsTypographicNumber
			, @year

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
