-- список направлений в заявках 
-- declare @institutionId int = 587
-- declare @requestId int

delete from InstitutionDirectionRequest where InstitutionId = @institutionId and RequestId = @requestId
