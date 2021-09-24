--declare @institutionId int = 1912
--declare @userName NVARCHAR(250) = 'vfmisis@mail.ru'

DECLARE @filialAccount bit = (
 SELECT top (1) I.IsFilial FROM UserPolicy up
 LEFT JOIN Institution I ON up.InstitutionID = I.InstitutionID
 WHERE UserName = @userName
)

DECLARE @esrpOrgId int = (
 select top (1) Institution.EsrpOrgID from Institution (NOLOCK)
 left join Institution (NOLOCK) as InstitutionFilials ON Institution.EsrpOrgID = InstitutionFilials.MainEsrpOrgId
 where (Institution.Institutionid = @institutionId AND Institution.IsFilial = 0)
 OR (InstitutionFilials.Institutionid = @institutionId AND InstitutionFilials.IsFilial = 1)
)

--Если акк принадлежит филиалу, показываем только филиал.
IF (@filialAccount = 1)
BEGIN
select 
 ins.InstitutionID,
 ins.FullName,
 ins.BriefName
from 
 Institution ins (NOLOCK)
where 
 ins.institutionId = @institutionId
order by ins.IsFilial;
END

--Если акк принадлежит головному ОО или админу, показываем головное и филиалы.
IF (@filialAccount = 0 OR @filialAccount IS NULL)
BEGIN
select 
 ins.InstitutionID,
 ins.FullName,
 ins.BriefName
from 
 Institution ins (NOLOCK)
where 
 ins.EsrpOrgID = @esrpOrgId OR ins.MainEsrpOrgId = @esrpOrgId
order by ins.IsFilial;
END