-- отклонение заявки
-- declare @institutionId int = 587
-- declare @requestId int
-- declare @denialComment varchar

update InstitutionDirectionRequest 
set IsDenied = 1,
DenialDate = GETDATE(),
DenialComment = @denialComment
where InstitutionId = @institutionId and RequestId = @requestId
