
-- exec dbo.CheckSchoolLeavingCertificate
-- ============================================
-- Процедура проверки аттестатов
-- v.1.0: Created by Sedov Anton 10.07.2008
-- ============================================
CREATE procedure dbo.CheckSchoolLeavingCertificate
	@certificateNumber nvarchar(255)
	, @login nvarchar(255)
	, @ip nvarchar(255)
as
begin	
	select
		@certificateNumber CertificateNumber
		, case when school_leaving_certificate_deny.Id is null then 0
			else 1
		end IsDeny
		, school_leaving_certificate_deny.Comment DenyComment
	from 
		(select
			@certificateNumber CertificateNumber) as schoolleaving_certificate_check
			left join dbo.SchoolLeavingCertificateDeny school_leaving_certificate_deny
				on schoolleaving_certificate_check.CertificateNumber between 
					school_leaving_certificate_deny.CertificateNumberFrom
						and school_leaving_certificate_deny.CertificateNumberTo  
	
				
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

	set @eventCode = 'SLC_CHK'
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
