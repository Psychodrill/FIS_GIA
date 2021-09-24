using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
    public  class ADOCompetitiveGroupDependency
    {


        public static CGApplicationDependencyVoc GetCGTApplicationDependency(int institutionId)
        {
            string sql = @"
--declare @institutionid int = 587;

declare @campaignID identifiers;
insert into @campaignID select campaignId from Campaign Where InstitutionID = @institutionId;

declare @CompetitiveGroupID identifiers;
insert into @CompetitiveGroupID select CompetitiveGroupID from CompetitiveGroup 
--Where CampaignID = @campaignID;
Where CampaignID in (select id from  @campaignID);

--declare @CompetitiveGroupItemID identifiers;
--insert into @CompetitiveGroupItemID select CompetitiveGroupItemID from CompetitiveGroupItem Where CompetitiveGroupID in (select id from @CompetitiveGroupID);

declare @CompetitiveGroupTargetItemID identifiers;
insert into @CompetitiveGroupTargetItemID 
select distinct CompetitiveGroupTargetItemID from CompetitiveGroupTargetItem 
Where CompetitiveGroupID in (select id from @CompetitiveGroupID);

declare @CompetitiveGroupTargetID identifiers;
insert into @CompetitiveGroupTargetID 
select distinct CompetitiveGroupTargetID from CompetitiveGroupTargetItem 
Where CompetitiveGroupTargetItemID in (select id from @CompetitiveGroupTargetItemID);

SELECT distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID, 
acgi.CompetitiveGroupId, 0 as 'CompetitiveGroupTargetId'
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where (acgi.CompetitiveGroupId  in (select id from @CompetitiveGroupID) and acgi.Priority is not null)

union
Select a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID, 
a.OrderCompetitiveGroupID , 0 as 'CompetitiveGroupTargetId'
from application a (NOLOCK)
Where a.OrderCompetitiveGroupID  in (select id from @CompetitiveGroupID)

union
Select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID, 
rl.CompetitiveGroupID,  0 as 'CompetitiveGroupTargetId'
from application a (NOLOCK)
left join [dbo].[RecomendedLists] rl (NOLOCK) on rl.ApplicationID = a.ApplicationID
left join [dbo].[RecomendedListsHistory] rlh (NOLOCK) on rlh.RecListID = rl.[RecListID]
Where rl.CompetitiveGroupID in (select id from @CompetitiveGroupID) And rlh.DateDelete is null

UNION
select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID, 
acgi.CompetitiveGroupId, 0
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where acgi.CompetitiveGroupId in (select id from @CompetitiveGroupId) and acgi.Priority is not null

union
select  ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID, 
OrderCompetitiveGroupID, 0
from application (NOLOCK)
Where OrderCompetitiveGroupID in (select id from @CompetitiveGroupId)

UNION
Select a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID,  
acgi.CompetitiveGroupId, acgi.CompetitiveGroupTargetID
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where (acgi.CompetitiveGroupId in (select id from @CompetitiveGroupId)
AND acgi.CompetitiveGroupTargetID in (select id from @CompetitiveGroupTargetId)  
and acgi.Priority is not null)

UNION
select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID,  
0, acgi.CompetitiveGroupTargetId
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where acgi.CompetitiveGroupTargetId in (select id from @CompetitiveGroupTargetId) and acgi.Priority is not null

union
select  ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID, 
0, OrderCompetitiveGroupTargetID
from application (NOLOCK)
Where OrderCompetitiveGroupTargetID in (select id from @CompetitiveGroupTargetId);
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@institutionId", institutionId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            
            
            if (ds != null && ds.Tables.Count > 0)
            {
                var res = new CGApplicationDependencyVoc(ds.Tables[0]);
                return res;
            }
            return new CGApplicationDependencyVoc(null);
        }
    }


    public class CGApplicationDependencyVoc : VocabularyBase<CGApplicationDependencyDto>
    {
        public List<ApplicationVocDto> CurrentItems;
        public CGApplicationDependencyVoc(DataTable dataTable) : base(dataTable) { }

        public List<ApplicationVocDto> Get(int competitiveGroupId, int competitiveGroupTargetId)
        {
            var res = items.Where(t =>
                    (competitiveGroupId == 0 || t.CompetitiveGroupId == competitiveGroupId)
                    //&& (competitiveGroupItemId == 0 || t.CompetitiveGroupItemId == competitiveGroupItemId)
                    && (competitiveGroupTargetId == 0 || t.CompetitiveGroupTargetId == competitiveGroupTargetId)
                );

            return res.Select(t => new ApplicationVocDto { ApplicationID = t.ApplicationID, ApplicationNumber = t.ApplicationNumber, RegistrationDate = t.RegistrationDate, UID = t.UID }).ToList();
        }
    }

    public class CGApplicationDependencyDto : VocabularyBaseDto
    {
        public int ApplicationID { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int StatusID { get; set; }
        public int CompetitiveGroupId { get; set; }
        //public int CompetitiveGroupItemId { get; set; }
        public int CompetitiveGroupTargetId { get; set; }
    }
}
