-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (46, '046_2012_06_29_SyncOrgs.sql')
-- =========================================================================
GO 

alter proc [dbo].[usp_repl_Organization2010ApplyBatch]
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
ConnectionStatusId)
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
item.ref.value('@ConnectionStatusId', 'int') as ConnectionStatusId 
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
KPP=tab.KPP
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
item.ref.value('@ConnectionStatusId', 'int') as ConnectionStatusId 
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

go


alter proc [dbo].[usp_repl_Organization2010GetBatch]
@type int=3,@rowcount int=2,@xmlrange xml =null out,@xmlresult xml =null out
as
set nocount on
declare @min timestamp,@max timestamp
declare @range table(id bigint,umin varbinary(8),umax varbinary(8))
insert @range
select top (@rowcount) id,MIN(Updated) umin,MAX(Updated) umax
from dbo.Organization2010_repl
where type=@type
and done=0 
group by id
if @@rowcount=0
goto endp
if @type<>3
begin
set @xmlresult=(
select 
(
select 
b.Id as '@Id',
b.CreateDate as '@CreateDate',
b.UpdateDate as '@UpdateDate',
b.FullName as '@FullName',
b.ShortName as '@ShortName',
b.RegionId as '@RegionId',
b.TypeId as '@TypeId',
b.KindId as '@KindId',
b.INN as '@INN',
b.OGRN as '@OGRN',
b.OwnerDepartment as '@OwnerDepartment',
b.IsPrivate as '@IsPrivate',
b.IsFilial as '@IsFilial',
b.DirectorPosition as '@DirectorPosition',
b.DirectorFullName as '@DirectorFullName',
b.IsAccredited as '@IsAccredited',
b.AccreditationSertificate as '@AccreditationSertificate',
b.LawAddress as '@LawAddress',
b.FactAddress as '@FactAddress',
b.PhoneCityCode as '@PhoneCityCode',
b.Phone as '@Phone',
b.Fax as '@Fax',
b.EMail as '@EMail',
b.Site as '@Site',
b.WasImportedAtStart as '@WasImportedAtStart',
b.CNFederalBudget as '@CNFederalBudget',
b.CNTargeted as '@CNTargeted',
b.CNLocalBudget as '@CNLocalBudget',
b.CNPaying as '@CNPaying',
b.CNFullTime as '@CNFullTime',
b.CNEvening as '@CNEvening',
b.CNPostal as '@CNPostal',
b.RCModel as '@RCModel',
b.RCDescription as '@RCDescription',
b.MainId as '@MainId',
b.DepartmentId as '@DepartmentId',
b.StatusId as '@StatusId',
b.NewOrgId as '@NewOrgId',
b.Version as '@Version',
b.DateChangeStatus as '@DateChangeStatus',
b.Reason as '@Reason',
b.ReceptionOnResultsCNE as '@ReceptionOnResultsCNE',
b.KPP as '@KPP',
b.LetterToRescheduleName as '@LetterToRescheduleName',
b.LetterToRescheduleContentType as '@LetterToRescheduleContentType',
b.TimeConnectionToSecureNetwork as '@TimeConnectionToSecureNetwork',
b.TimeEnterInformationInFIS as '@TimeEnterInformationInFIS', 
b.ConnectionSchemeId as '@ConnectionSchemeId',
b.ConnectionStatusId as '@ConnectionStatusId' 
from 
@range a
join [dbo].[Organization2010] b on a.id=b.id
for xml path('row'), ELEMENTS XSINIL, type
) 
  for xml path('rows'), type
  )

 end
 else
begin
 set @xmlresult=(
select 

(
select 
isnull(a.Id,0) as '@Id'
from 
@range a
for xml path('row'), type
) 
  for xml path('rows'), type
  )
end

 select @xmlrange=
 (select 
 (select Id as '@Id',umin as '@umin',umax as '@umax'
 from @range for xml path('range'), type) 
 for xml path('ranges'), type)
endp:
-- select 	
--	item.ref.value('@Id', 'bigint') as Id	,item.ref.value('@umin', 'varbinary(8)') as umin	,item.ref.value('@umax', 'varbinary(8)') as umax	
--from 
--(
--select @xmlrange
--) feeds(feedXml)
--cross apply feedXml.nodes('/ranges/range') as item(ref)
--exec usp_repl_Organization2010ApplyBatch @xmlresult,3
--exec usp_repl_Organization2010ApplyBatch @xmlresult,2
--exec dbo.usp_repl_Organization2010ReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_Organization2010GetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/