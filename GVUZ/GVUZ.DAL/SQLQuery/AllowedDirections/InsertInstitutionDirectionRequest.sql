-- ���������� ������ �� ��������� ��� ���������� ����������� �� ������ ����������� ��� ������ � ����������� ��

--declare @institutionId int
--declare @directionId int
--declare @requestType int
--declare @comment varchar(255)

insert into InstitutionDirectionRequest([InstitutionId], [DirectionId], [RequestType], [RequestComment])
values(@institutionId, @directionId, @requestType, @comment)