
-- =============================================
-- Выполнить задание на загрузку сертификатов ЕГЭ.
-- v.1.0: Created by Makarev Andrey 29.05.2008
-- =============================================
CREATE proc dbo.ExecuteCommonNationalExamCertificateLoadingTask
	@id bigint
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
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
		account.[Login] = @editorLogin

	set @eventCode = N'CNE_LOAD'

	set @internalId = dbo.GetInternalId(@Id)

	update cne_certificate_loading_task
	set
		IsProcess = 0
		, UpdateDate = @currentDate
		, EditorAccountId = @accountId
		, EditorIp = @editorIp
	from
		dbo.CommonNationalExamCertificateLoadingTask cne_certificate_loading_task
	where
		cne_certificate_loading_task.Id = @internalId
		and cne_certificate_loading_task.IsActive <> 0
		and cne_certificate_loading_task.IsLoaded <> 1

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	return 0

end
