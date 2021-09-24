-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (62, '062_2013_03_27_usp_repl_OrganizationRequest2010GetBatchProcedure')
go

ALTER proc [dbo].[usp_repl_OrganizationRequest2010GetBatch]
@type int=3,@rowcount int=2,@xmlrange xml =null out,@xmlresult xml =null out
as
set nocount on
declare @min timestamp,@max timestamp
declare @range table(id bigint,umin varbinary(8),umax varbinary(8))
insert @range
select top (@rowcount) id,MIN(Updated) umin,MAX(Updated) umax
from dbo.OrganizationRequest2010_repl
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
b.OrganizationId as '@OrganizationId',
b.StatusID as '@StatusID',
--b.RegistrationDocument as '@RegistrationDocument',
b.RegistrationDocumentContentType as '@RegistrationDocumentContentType',
b.IsForActivation as '@IsForActivation',
b.RCModelID as '@RCModelID',
b.RCDescription as '@RCDescription',
b.ReceptionOnResultsCNE as '@ReceptionOnResultsCNE',
b.KPP as '@KPP',
b.DirectorPositionInGenetive as '@DirectorPositionInGenetive',
b.DirectorFullNameInGenetive as '@DirectorFullNameInGenetive',
b.DirectorFirstName as '@DirectorFirstName',
b.DirectorLastName as '@DirectorLastName',
b.DirectorPatronymicName as '@DirectorPatronymicName',
b.OUConfirmation as '@OUConfirmation'
from 
@range a
join [dbo].[OrganizationRequest2010] b on a.id=b.id
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
--exec usp_repl_OrganizationRequest2010ApplyBatch @xmlresult,3
--exec usp_repl_OrganizationRequest2010ApplyBatch @xmlresult,2
--exec dbo.usp_repl_OrganizationRequest2010ReplDone @xmlrange
 /*
declare @xmlrange xml ,@xmlresult xml 
exec usp_repl_OrganizationRequest2010GetBatch @type=2, @rowcount =2,@xmlrange =@xmlrange  out,@xmlresult=@xmlresult out
select @xmlrange,@xmlresult
*/
