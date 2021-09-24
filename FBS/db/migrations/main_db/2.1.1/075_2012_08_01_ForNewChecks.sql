-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (75, '075_2012_08_01_ForNewChecks.sql')
-- =========================================================================
GO
ALTER TABLE dbo.CommonNationalExamCertificate ADD FIO as  replace(REPLACE((ltrim(rtrim(LastName))+ltrim(rtrim(FirstName))+ltrim(rtrim(PatronymicName))),'ё','е'),' ','') PERSISTED
go
create index idx_CommonNationalExamCertificate_fio on dbo.CommonNationalExamCertificate(fio)
go
go

create PROCEDURE [dbo].[CommonNationalExamCertificateSumCheckResult] 
@batchId BIGINT = 8774
AS 
    set nocount on
    select  @batchId BatchId,
            Name,
            [Sum],
            case when [Status] = 0 then 2
                 when [Status] = 1 then 1
                 else 3
            end [Status],
            NameSake
    from    ( select    Name,
                        [Sum],
                        max([Status]) [Status],
                        NameSake
              from      CommonNationalExamCertificateSumCheck
              where     BatchId = dbo.GetInternalId(@batchId)
              group by  Name,
                        [sum],
                        NameSake
            ) tt