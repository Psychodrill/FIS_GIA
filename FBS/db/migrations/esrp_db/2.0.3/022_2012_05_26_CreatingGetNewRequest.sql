-- =========================================================================
-- «апись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (22, '022_2012_05_26_CreatingGetNewRequest')
-- =========================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNewRequest]
GO

PRINT N'Creating [dbo].[GetNewRequest]...';
GO
create PROCEDURE [dbo].[GetNewRequest]
@login NVARCHAR (255)=null, 
@organizationRegionId INT=null, 
@organizationFullName NVARCHAR (2000)=null,
@organizationShortName NVARCHAR (2000)=null, 
@organizationINN NVARCHAR (10)=null, 
@organizationOGRN NVARCHAR (13)=null, 
@organizationFounderName NVARCHAR (2000)=null, 
@organizationFactAddress NVARCHAR (500)=null,
@organizationLawAddress NVARCHAR (500)=null, 
@organizationDirName NVARCHAR (255)=null,
@organizationDirPosition NVARCHAR (500)=null, 
@organizationPhoneCode NVARCHAR (255)=null,
@organizationFax NVARCHAR (255)=null,
@organizationIsAccred bit=0,
@organizationIsPrivate bit=0,
@organizationIsFilial bit=0,
@organizationAccredSert nvarchar(255)=null,
@organizationEMail nvarchar(255)=null,
@organizationSite nvarchar(255)=null, 
@organizationPhone NVARCHAR (255)=null,
@status NVARCHAR (255)=null,
@organizationTypeId INT=null,
@organizationKindId INT=null,
@organizationRcModelId INT=null,
@orgRCDescription NVARCHAR(400)=NULL,
@ExistingOrgId INT=NULL,
@orgRequestID INT= null,
@ReceptionOnResultsCNE BIT = null,
@registrationDocument IMAGE=null,
@organizationKPP NVARCHAR (9)=null,
@error int=1 output
as
set nocount on
begin try
	set @error=1
	begin tran
		DECLARE @statusID INT, @st nvarchar(255),@currentYear int	
		
		set @currentYear = year(getdate())
		
		if @status=''
			set @status=null
			
		if @login=''
			set @login=null
			
		if @login is null
			set @st=dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		else
			set @st = (select dbo.GetUserStatus(@currentYear, isnull(@status,account.[Status]), @currentYear, @registrationDocument) from dbo.Account account with (nolock, fastfirstrow) where account.[Login] = @login)	
	
		if @orgRequestID is null		
		begin						
-- определ€ем идентификатор статуса			
			SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @st

-- за€вка подаетс€ не зависимо от того, новый аккаунт создаетс€ или обновл€етс€ старый
			insert dbo.OrganizationRequest2010
				(
					FullName, ShortName, RegionId,	TypeId,	KindId,	INN, OGRN, OwnerDepartment, IsPrivate, IsFilial, DirectorPosition, DirectorFullName,
					IsAccredited, AccreditationSertificate, LawAddress,	FactAddress, PhoneCityCode,	Phone, Fax,	EMail, Site, OrganizationId, StatusID, RCModelID,
					RCDescription, ReceptionOnResultsCNE,KPP
				)
			select @organizationFullName, @organizationShortName, @organizationRegionId, @organizationTypeId, @organizationKindId, @organizationINN,
				   @organizationOGRN, @organizationFounderName, @organizationIsPrivate, @organizationIsFilial,	@organizationDirPosition, @organizationDirName,
				   @organizationIsAccred, @organizationAccredSert,	@organizationLawAddress, @organizationFactAddress, @organizationPhoneCode,
				   @organizationPhone,	@organizationFax, @organizationEMail, @organizationSite, @ExistingOrgId, @statusID, @organizationRcModelId,
				   @orgRCDescription, @ReceptionOnResultsCNE,@organizationKPP
		 
			set @orgRequestID = scope_identity()	

			select @orgRequestID [orgRequestID], @st [Status]
		end
		else
		begin		
			if exists(select * from dbo.Account account with (nolock, fastfirstrow) where OrganizationId = @orgRequestID and account.[Status] ='registration')
				set @st = 'registration'
			
			SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @st			
					
			update OrganizationRequest2010 set registrationDocument=@registrationDocument,RegionId=@organizationRegionId,FullName=@organizationFullName,
											   ShortName=@organizationShortName,INN=@organizationINN,OGRN=@organizationOGRN,
											   OwnerDepartment=@organizationFounderName,FactAddress=@organizationFactAddress,
											   LawAddress=@organizationLawAddress,DirectorFullName=@organizationDirName,DirectorPosition=@organizationDirPosition,
											   PhoneCityCode=@organizationPhoneCode,Fax=@organizationFax,IsAccredited=@organizationIsAccred,
											   IsPrivate=@organizationIsPrivate,IsFilial=@organizationIsFilial,AccreditationSertificate=@organizationAccredSert,
											   EMail=@organizationEMail,Site=@organizationSite,Phone=@organizationPhone,
											   TypeId=@organizationTypeId,KindId=@organizationKindId,RcModelId=@organizationRcModelId,
											   RCDescription=@orgRCDescription,OrganizationId=@ExistingOrgId,ReceptionOnResultsCNE=@ReceptionOnResultsCNE,
											   StatusID=@StatusID,UpdateDate=getdate(),KPP=@organizationKPP
			where id=@orgRequestID
			
			select id [orgRequestID], Code [Status] from OrganizationRequest2010 a join AccountStatus b on a.StatusID=b.StatusID where id=@orgRequestID
		end
			
	if @@trancount > 0 
		commit tran 

end try
begin catch
	set @error=-1
	if @@trancount > 0
		rollback tran 
	declare @er nvarchar(4000)
	set @er=error_message()
	raiserror(@er,16,1) 
	return -1
end catch
GO