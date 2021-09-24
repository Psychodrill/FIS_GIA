using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
    public class ADODependencyRepository
    {


        /// <summary>
        /// CompetitiveGroupItem - получить зависимости через ApplicationCompetitiveGroupItem
        /// </summary>
        /// <param name="competitiveGroupItemId"></param>
        /// <returns></returns>
        public static List<ApplicationVocDto> GetCGIApplicationCompetitiveGroupItemDependency(int competitiveGroupItemId)
        {
            string sql = @"
select  a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where acgi.CompetitiveGroupItemId = @CompetitiveGroupItemId and acgi.Priority is not null;";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CompetitiveGroupItemId", competitiveGroupItemId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);
            
            if (ds != null && ds.Tables.Count > 0)
            {
                ApplicationVoc app = new ApplicationVoc(ds.Tables[0]);
                return app.Items;
            }
            return new List<ApplicationVocDto>();
        }

        /// <summary>
        /// CompetitiveGroupItem - получить зависимости по  Application.OrderCompetitiveGroupItemID
        /// </summary>
        /// <param name="competitiveGroupItemId"></param>
        /// <returns></returns>
        public static List<ApplicationVocDto> GetCGIOrderCompetitiveGroupDependency(int competitiveGroupItemId)
        {

            string sql = @"
select  ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application (NOLOCK)
Where OrderCompetitiveGroupItemID = @CompetitiveGroupItemId;";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CompetitiveGroupItemId", competitiveGroupItemId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            if (ds != null && ds.Tables.Count > 0)
            {
                ApplicationVoc app = new ApplicationVoc(ds.Tables[0]);
                return app.Items;
            }
            return new List<ApplicationVocDto>();
        }

        public static List<ApplicationVocDto> GetCGApplicationDependency(int competitiveGroupId)
        {
            string sql = @"
Select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where (acgi.CompetitiveGroupId = @CompetitiveGroupId and acgi.Priority is not null)
union
Select a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
Where a.OrderCompetitiveGroupID = @CompetitiveGroupID
union
Select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
left join [dbo].[RecomendedLists] rl (NOLOCK) on rl.ApplicationID = a.ApplicationID
left join [dbo].[RecomendedListsHistory] rlh (NOLOCK) on rlh.RecListID = rl.[RecListID]
Where rl.CompetitiveGroupID = @CompetitiveGroupID And rlh.DateDelete is null;
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CompetitiveGroupId", competitiveGroupId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            if (ds != null && ds.Tables.Count > 0)
            {
                ApplicationVoc app = new ApplicationVoc(ds.Tables[0]);
                return app.Items;
            }
            return new List<ApplicationVocDto>();
        }



        public static List<ApplicationVocDto> GetCGTIApplicationDependency(int competitiveGroupItemId, int competitiveGroupTargetId)
        {
            string sql = @"
Select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where (acgi.CompetitiveGroupItemId = @CompetitiveGroupItemId 
and acgi.CompetitiveGroupTargetID = @CompetitiveGroupTargetId  and acgi.Priority is not null)
union
Select a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
Where a.OrderCompetitiveGroupTargetID = @CompetitiveGroupTargetId
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CompetitiveGroupItemId", competitiveGroupItemId);
            parameters.Add("@CompetitiveGroupTargetId", competitiveGroupTargetId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            if (ds != null && ds.Tables.Count > 0)
            {
                ApplicationVoc app = new ApplicationVoc(ds.Tables[0]);
                return app.Items;
            }
            return new List<ApplicationVocDto>();
        }


        public static List<ApplicationVocDto> GetCGTApplicationDependency(int competitiveGroupTargetId)
        {
            string sql = @"
select distinct a.ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application a (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi (NOLOCK) on acgi.ApplicationId = a.ApplicationID
Where acgi.CompetitiveGroupTargetId = @CompetitiveGroupTargetId and acgi.Priority is not null
union
select  ApplicationID, UID, ApplicationNumber, RegistrationDate, StatusID
from application (NOLOCK)
Where OrderCompetitiveGroupTargetID = @CompetitiveGroupTargetId;
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CompetitiveGroupTargetId", competitiveGroupTargetId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            if (ds != null && ds.Tables.Count > 0)
            {
                ApplicationVoc app = new ApplicationVoc(ds.Tables[0]);
                return app.Items;
            }
            return new List<ApplicationVocDto>();
        }


      

    }
}
