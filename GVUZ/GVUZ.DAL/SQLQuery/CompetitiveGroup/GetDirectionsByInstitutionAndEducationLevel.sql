--declare @InstitutionID int = 587;
--declare @EducationLevelID int = 2;

select d.DirectionID 
	,d.ParentID
	,d.UGSNAME
	,d.UGSCODE
	,d.Code
	,d.NewCode
	,d.Name as DirectionName
	,ad.AdmissionItemTypeID as EducationLevelID
	,etcd.DirectionID as CreativeDirectionID
	,etpd.DirectionID as ProfileDirectionID
	,ad.AllowedDirectionStatusID
from direction d with (nolock)
inner join AllowedDirections ad with (nolock) on ad.DirectionID = d.DirectionID and ad.InstitutionID = @InstitutionID 
--and ad.AdmissionItemTypeID = @EducationLevelID -- чтобы вынести отбор направлений с сервера на клиент!
left join [EntranceTestCreativeDirection] etcd with (nolock) on etcd.DirectionID = d.DirectionID
left join [EntranceTestProfileDirection] etpd with (nolock) on etpd.DirectionID = d.DirectionID;