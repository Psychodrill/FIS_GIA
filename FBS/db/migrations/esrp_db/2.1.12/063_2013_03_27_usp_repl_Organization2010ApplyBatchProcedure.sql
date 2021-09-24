-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (63, '063_2013_03_27_usp_repl_Organization2010ApplyBatchProcedure')
go

ALTER proc [dbo].[usp_repl_Organization2010ApplyBatch]
@xml xml,@type int
as
set nocount on
if @type=1
begin
set identity_insert dbo.Organization2010 on 
 insert dbo.Organization2010(
Id,
CreateDate,
UpdateDate,
FullName,
ShortName,
RegionId,
TypeId,
KindId,
INN,
OGRN,
OwnerDepartment,
IsPrivate,
IsFilial,
DirectorPosition,
DirectorFullName,
IsAccredited,
AccreditationSertificate,
LawAddress,
FactAddress,
PhoneCityCode,
Phone,
Fax,
EMail,
Site,
WasImportedAtStart,
CNFederalBudget,
CNTargeted,
CNLocalBudget,
CNPaying,
CNFullTime,
CNEvening,
CNPostal,
RCModel,
RCDescription,
MainId,
DepartmentId,
StatusId,
NewOrgId,
Version,
DateChangeStatus,
Reason,
ReceptionOnResultsCNE,
KPP,
LetterToRescheduleName,
LetterToRescheduleContentType,
TimeConnectionToSecureNetwork,
TimeEnterInformationInFIS,
ConnectionSchemeId,
ConnectionStatusId,
DirectorPositionInGenetive,
DirectorFullNameInGenetive,
DirectorFirstName,
DirectorLastName,
DirectorPatronymicName,
OUConfirmation
)
select 	
item.ref.value('@Id', 'int') as Id,
item.ref.value('@CreateDate', 'datetime') as CreateDate,
item.ref.value('@UpdateDate', 'datetime') as UpdateDate,
item.ref.value('@FullName', 'nvarchar(1000)') as FullName,
item.ref.value('@ShortName', 'nvarchar(500)') as ShortName,
item.ref.value('@RegionId', 'int') as RegionId,
item.ref.value('@TypeId', 'int') as TypeId,
item.ref.value('@KindId', 'int') as KindId,
item.ref.value('@INN', 'nvarchar(10)') as INN,
item.ref.value('@OGRN', 'nvarchar(13)') as OGRN,
item.ref.value('@OwnerDepartment', 'nvarchar(500)') as OwnerDepartment,
item.ref.value('@IsPrivate', 'bit') as IsPrivate,
item.ref.value('@IsFilial', 'bit') as IsFilial,
item.ref.value('@DirectorPosition', 'nvarchar(255)') as DirectorPosition,
item.ref.value('@DirectorFullName', 'nvarchar(255)') as DirectorFullName,
item.ref.value('@IsAccredited', 'bit') as IsAccredited,
item.ref.value('@AccreditationSertificate', 'nvarchar(255)') as AccreditationSertificate,
item.ref.value('@LawAddress', 'nvarchar(255)') as LawAddress,
item.ref.value('@FactAddress', 'nvarchar(255)') as FactAddress,
item.ref.value('@PhoneCityCode', 'nvarchar(255)') as PhoneCityCode,
item.ref.value('@Phone', 'nvarchar(10)') as Phone,
item.ref.value('@Fax', 'nvarchar(100)') as Fax,
item.ref.value('@EMail', 'nvarchar(100)') as EMail,
item.ref.value('@Site', 'nvarchar(100)') as Site,
item.ref.value('@WasImportedAtStart', 'nvarchar(40)') as WasImportedAtStart,
item.ref.value('@CNFederalBudget', 'bit') as CNFederalBudget,
item.ref.value('@CNTargeted', 'int') as CNTargeted,
item.ref.value('@CNLocalBudget', 'int') as CNLocalBudget,
item.ref.value('@CNPaying', 'int') as CNPaying,
item.ref.value('@CNFullTime', 'int') as CNFullTime,
item.ref.value('@CNEvening', 'int') as CNEvening,
item.ref.value('@CNPostal', 'int') as CNPostal,
item.ref.value('@RCModel', 'int') as RCModel,
item.ref.value('@RCDescription', 'nvarchar(400)') as RCDescription,
item.ref.value('@MainId', 'nvarchar(255)') as MainId,
item.ref.value('@DepartmentId', 'int') as DepartmentId,
item.ref.value('@StatusId', 'int') as StatusId,
item.ref.value('@NewOrgId', 'int') as NewOrgId,
item.ref.value('@Version', 'int') as Version,
item.ref.value('@DateChangeStatus', 'datetime') as DateChangeStatus,
item.ref.value('@Reason', 'nvarchar(100)') as Reason,
item.ref.value('@ReceptionOnResultsCNE', 'bit') as ReceptionOnResultsCNE,
item.ref.value('@KPP', 'nvarchar(9)') as KPP,
item.ref.value('@LetterToRescheduleName', 'nvarchar(255)') as LetterToRescheduleName,
item.ref.value('@LetterToRescheduleContentType', 'nvarchar(255)') as LetterToRescheduleContentType,
item.ref.value('@TimeConnectionToSecureNetwork', 'datetime') as TimeConnectionToSecureNetwork,
item.ref.value('@TimeEnterInformationInFIS', 'datetime') as TimeEnterInformationInFIS,
item.ref.value('@ConnectionSchemeId', 'int') as ConnectionSchemeId,
item.ref.value('@ConnectionStatusId', 'int') as ConnectionStatusId,
item.ref.value('@DirectorPositionInGenetive', 'nvarchar(255)') as DirectorPositionInGenetive,
item.ref.value('@DirectorFullNameInGenetive', 'nvarchar(255)') as DirectorFullNameInGenetive,
item.ref.value('@DirectorFirstName', 'nvarchar(100)') as DirectorFirstName,
item.ref.value('@DirectorLastName', 'nvarchar(100)') as DirectorLastName,
item.ref.value('@DirectorPatronymicName', 'nvarchar(100)') as DirectorPatronymicName,
item.ref.value('@OUConfirmation', 'bit') as OUConfirmation
from 
(
select @xml
) feeds(feedXml)
cross apply feedXml.nodes('/rows/row') as item(ref)
set identity_insert dbo.Organization2010 off
 return
end
if @type=2
begin
 update a set
CreateDate=tab.CreateDate,
UpdateDate=tab.UpdateDate,
FullName=tab.FullName,
ShortName=tab.ShortName,
RegionId=tab.RegionId,
TypeId=tab.TypeId,
KindId=tab.KindId,
INN=tab.INN,
OGRN=tab.OGRN,
OwnerDepartment=tab.OwnerDepartment,
IsPrivate=tab.IsPrivate,
IsFilial=tab.IsFilial,
DirectorPosition=tab.DirectorPosition,
DirectorFullName=tab.DirectorFullName,
IsAccredited=tab.IsAccredited,
AccreditationSertificate=tab.AccreditationSertificate,
LawAddress=tab.LawAddress,
FactAddress=tab.FactAddress,
PhoneCityCode=tab.PhoneCityCode,
Phone=tab.Phone,
Fax=tab.Fax,
EMail=tab.EMail,
Site=tab.Site,
WasImportedAtStart=tab.WasImportedAtStart,
CNFederalBudget=tab.CNFederalBudget,
CNTargeted=tab.CNTargeted,
CNLocalBudget=tab.CNLocalBudget,
CNPaying=tab.CNPaying,
CNFullTime=tab.CNFullTime,
CNEvening=tab.CNEvening,
CNPostal=tab.CNPostal,
RCModel=tab.RCModel,
RCDescription=tab.RCDescription,
MainId=tab.MainId,
DepartmentId=tab.DepartmentId,
StatusId=tab.StatusId,
NewOrgId=tab.NewOrgId,
Version=tab.Version,
DateChangeStatus=tab.DateChangeStatus,
Reason=tab.Reason,
ReceptionOnResultsCNE=tab.ReceptionOnResultsCNE,
KPP=tab.KPP,
DirectorPositionInGenetive=tab.DirectorPositionInGenetive,
DirectorFullNameInGenetive=tab.DirectorFullNameInGenetive,
DirectorFirstName=tab.DirectorFirstName,
DirectorLastName=tab.DirectorLastName,
DirectorPatronymicName=tab.DirectorPatronymicName,
OUConfirmation=tab.OUConfirmation
from
(
 select 	
item.ref.value('@Id', 'int') as Id,
item.ref.value('@CreateDate', 'datetime') as CreateDate,
item.ref.value('@UpdateDate', 'datetime') as UpdateDate,
item.ref.value('@FullName', 'nvarchar(1000)') as FullName,
item.ref.value('@ShortName', 'nvarchar(500)') as ShortName,
item.ref.value('@RegionId', 'int') as RegionId,
item.ref.value('@TypeId', 'int') as TypeId,
item.ref.value('@KindId', 'int') as KindId,
item.ref.value('@INN', 'nvarchar(10)') as INN,
item.ref.value('@OGRN', 'nvarchar(13)') as OGRN,
item.ref.value('@OwnerDepartment', 'nvarchar(500)') as OwnerDepartment,
item.ref.value('@IsPrivate', 'bit') as IsPrivate,
item.ref.value('@IsFilial', 'bit') as IsFilial,
item.ref.value('@DirectorPosition', 'nvarchar(255)') as DirectorPosition,
item.ref.value('@DirectorFullName', 'nvarchar(255)') as DirectorFullName,
item.ref.value('@IsAccredited', 'bit') as IsAccredited,
item.ref.value('@AccreditationSertificate', 'nvarchar(255)') as AccreditationSertificate,
item.ref.value('@LawAddress', 'nvarchar(255)') as LawAddress,
item.ref.value('@FactAddress', 'nvarchar(255)') as FactAddress,
item.ref.value('@PhoneCityCode', 'nvarchar(255)') as PhoneCityCode,
item.ref.value('@Phone', 'nvarchar(10)') as Phone,
item.ref.value('@Fax', 'nvarchar(100)') as Fax,
item.ref.value('@EMail', 'nvarchar(100)') as EMail,
item.ref.value('@Site', 'nvarchar(100)') as Site,
item.ref.value('@WasImportedAtStart', 'nvarchar(40)') as WasImportedAtStart,
item.ref.value('@CNFederalBudget', 'bit') as CNFederalBudget,
item.ref.value('@CNTargeted', 'int') as CNTargeted,
item.ref.value('@CNLocalBudget', 'int') as CNLocalBudget,
item.ref.value('@CNPaying', 'int') as CNPaying,
item.ref.value('@CNFullTime', 'int') as CNFullTime,
item.ref.value('@CNEvening', 'int') as CNEvening,
item.ref.value('@CNPostal', 'int') as CNPostal,
item.ref.value('@RCModel', 'int') as RCModel,
item.ref.value('@RCDescription', 'nvarchar(400)') as RCDescription,
item.ref.value('@MainId', 'nvarchar(255)') as MainId,
item.ref.value('@DepartmentId', 'int') as DepartmentId,
item.ref.value('@StatusId', 'int') as StatusId,
item.ref.value('@NewOrgId', 'int') as NewOrgId,
item.ref.value('@Version', 'int') as Version,
item.ref.value('@DateChangeStatus', 'datetime') as DateChangeStatus,
item.ref.value('@Reason', 'nvarchar(100)') as Reason,
item.ref.value('@ReceptionOnResultsCNE', 'bit') as ReceptionOnResultsCNE,
item.ref.value('@KPP', 'nvarchar(9)') as KPP,
item.ref.value('@LetterToRescheduleName', 'nvarchar(255)') as LetterToRescheduleName,
item.ref.value('@LetterToRescheduleContentType', 'nvarchar(255)') as LetterToRescheduleContentType,
item.ref.value('@TimeConnectionToSecureNetwork', 'datetime') as TimeConnectionToSecureNetwork,
item.ref.value('@TimeEnterInformationInFIS', 'datetime') as TimeEnterInformationInFIS,
item.ref.value('@ConnectionSchemeId', 'int') as ConnectionSchemeId,
item.ref.value('@ConnectionStatusId', 'int') as ConnectionStatusId,
item.ref.value('@DirectorPositionInGenetive', 'nvarchar(255)') as DirectorPositionInGenetive,
item.ref.value('@DirectorFullNameInGenetive', 'nvarchar(255)') as DirectorFullNameInGenetive,
item.ref.value('@DirectorFirstName', 'nvarchar(100)') as DirectorFirstName,
item.ref.value('@DirectorLastName', 'nvarchar(100)') as DirectorLastName,
item.ref.value('@DirectorPatronymicName', 'nvarchar(100)') as DirectorPatronymicName,
item.ref.value('@OUConfirmation', 'bit') as OUConfirmation
 
from 
(
select @xml
) feeds(feedXml)
cross apply feedXml.nodes('/rows/row') as item(ref)
) tab
join dbo.Organization2010 a
on a.id=tab.Id


 return
end

if @type=3
begin
begin try
begin tran
declare @t table (id bigint)
insert @t(id)
select id
 from
(
 select 	
	item.ref.value('@Id', 'bigint') as Id
from 
(
select @xml
) feeds(feedXml)
cross apply feedXml.nodes('/rows/row') as item(ref)
) tab

delete a
 from
@t tab
join dbo.Organization2010 a
on a.id=tab.Id 
if @@TRANCOUNT>0
	commit
end try
begin catch
 if @@trancount>0
  rollback
  declare @msg nvarchar(4000)
  set @msg=ERROR_MESSAGE()
  raiserror(@msg,16,1)
  return -1
end catch
end