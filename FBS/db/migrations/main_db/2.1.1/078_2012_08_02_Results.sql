-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (78, '078_2012_08_02_Results.sql')
-- =========================================================================
GO
ALTER PROCEDURE [dbo].[CommonNationalExamCertificateSumCheckResult] @batchId BIGINT = 301336636159
AS 
    set nocount on
   
    select  @batchId BatchId,
            Name,
            [Sum],
           [Status],
            NameSake
            from
            (
 select    Name,max([Sum]) sum,
                        max(case when [Status]=1 then 1 when [Status]=2 then 3 end) [Status],
                        min([Id]) id,
                        NameSake
              from      CommonNationalExamCertificateSumCheck
              where     [Status] in(1,2) and
              BatchId = 
             dbo.GetInternalId(@batchId)
            --  @batchId
               group by  Name,NameSake
union all              
select    Name,max([Sum]) sum,
                        2 [Status],
                        min([Id]) id,
                        NameSake
              from      CommonNationalExamCertificateSumCheck a
              where     a.[Status]=0 and
              BatchId = 
               dbo.GetInternalId(@batchId)
            and not exists(select * from CommonNationalExamCertificateSumCheck where 
            BatchId = 
              dbo.GetInternalId(@batchId)
            and Name=a.Name and [Status]=1
            )
               group by  Name,NameSake
               ) tt
              ORDER BY tt.Id 