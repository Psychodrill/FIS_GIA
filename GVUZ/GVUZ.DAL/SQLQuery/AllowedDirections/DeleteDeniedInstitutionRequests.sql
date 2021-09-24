-- удаление отклоненных заявок по типам
-- declare @institutionId int = 587

delete from InstitutionDirectionRequest where InstitutionId = @institutionId and RequestType in(@@REQUESTTYPES@@) and IsDenied = 1