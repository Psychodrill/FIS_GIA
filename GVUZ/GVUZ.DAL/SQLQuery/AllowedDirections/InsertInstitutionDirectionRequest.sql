-- добавление заявки на включение или исключение направления из списка разрешенных или списка с профильными ВИ

--declare @institutionId int
--declare @directionId int
--declare @requestType int
--declare @comment varchar(255)

insert into InstitutionDirectionRequest([InstitutionId], [DirectionId], [RequestType], [RequestComment])
values(@institutionId, @directionId, @requestType, @comment)