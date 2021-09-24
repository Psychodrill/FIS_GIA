
CREATE PROCEDURE [dbo].[CommonNationalExamCertificateSumCheckResult] @batchId BIGINT = 8774
AS 
    set nocount on
   
    select  @batchId BatchId,
            Name,
            [Sum],
           [Status],
            NameSake
            from
            (
 select    Name,[Sum],
                        case when [Status]=1 then 1 when [Status]=2 then 3 end [Status],
                        [Id],
                        NameSake
              from      CommonNationalExamCertificateSumCheck
              where     [Status] in(1,2) and
              BatchId = 
             dbo.GetInternalId(@batchId)
              --@batchId
union all              
select    Name,max([Sum]) sum,
                        2 [Status],
                        min([Id]) id,
                        NameSake
              from      CommonNationalExamCertificateSumCheck
              where     [Status]=0 and
              BatchId = 
             dbo.GetInternalId(@batchId)
            --  @batchId
               group by  Name,NameSake
               ) tt
              ORDER BY tt.Id
