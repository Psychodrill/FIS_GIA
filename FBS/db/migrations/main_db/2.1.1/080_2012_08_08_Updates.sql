-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (80, '080_2012_08_08_Updates.sql')
-- =========================================================================
GO

create index idx_CommonNationalExamCertificate_UpdateDtate on dbo.CommonNationalExamCertificate(UpdateDate) 
go
go
create index idx_CommonNationalExamCertificate_RegionidYear on dbo.CommonNationalExamCertificate(regionid,year) 
go
