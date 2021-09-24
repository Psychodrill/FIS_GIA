using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.SqlConstructor.Organizations
{
    public class SqlConstructor_GetSubordinateOrganizations : SqlConstructor_GetSubordOrgs
    {
        public SqlConstructor_GetSubordinateOrganizations(NameValueCollection urlParameters, int OrganizationId)
            : base(urlParameters)
        {
            AllowedFieldNames.Add("CountUser");
            OrgId = OrganizationId;
        }

        private int orgId;
        public int OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }

        protected override string getMainSQL()
        {
            return string.Format(@"select * 
                                   from ReportStatisticSubordinateOrg (null, null, {0})",
                                    OrgId.ToString());
        }

        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
            base.CreateParameters(parameters, whereExpr);

            { // создание специфичного параметра
                int userCount = GetVal_int("CountUser");
                if (userCount > 0)
                {
                    if (userCount == 1)
                        whereExpr.Add("CountUser=0"); //только "не привязанные" организации
                    else
                        whereExpr.Add("CountUser>0"); //только организации с привязанными пользователями
                }
            }
        }
    }
}
