
-- exec dbo.CheckEntrant
-- ============================================
-- Процедура проверки поступивших абитуриентов
-- v.1.0: Create by Sedov Anton 08.07.2008
-- ============================================
CREATE procedure dbo.CheckEntrant 
	@certificateNumber nvarchar(255)
	, @login nvarchar(255)
	, @ip nvarchar(255)
as
begin
	select 	
		@certificateNumber CertificateNumber
		, entrant.LastName LastName
		, entrant.FirstName FirstName
		, entrant.PatronymicName PatronymicName
		, organization.[Name] OrganizationName
		, entrant.CreateDate EntrantCreateDate
		, case when entrant.CertificateNumber is null
				then 0
			else 1
		end IsExist
	from 
		(select @certificateNumber CertificateNumber) as check_entrant
			left join dbo.Entrant entrant with(nolock, fastfirstrow)
				inner join dbo.Organization organization
					on organization.Id = entrant.OwnerOrganizationId
				on check_entrant.CertificateNumber = entrant.CertificateNumber

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId uniqueidentifier
	
	select
		@editorAccountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login 

	set @eventCode = 'ENT_CHK'
	set @updateId = NewId()			
	
	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @ip
		, @eventCode = @eventCode
		, @sourceEntityIds = null
		, @eventParams = null
		, @updateId = @updateId
			
	return 0
end
