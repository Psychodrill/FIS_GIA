-- добавление направления в список разрешенных

--declare @institutionId int
--declare @directionId int
--declare @year int

declare @educationLevelId smallint

select TOP 1 @educationLevelId = EducationLevelId from Direction where DirectionId = @directionId

insert into AllowedDirections([InstitutionId], [DirectionId], [Year], [AdmissionItemTypeId], [AllowedDirectionStatusID])
values(@institutionId, @directionId, @year, @educationLevelId, 3)