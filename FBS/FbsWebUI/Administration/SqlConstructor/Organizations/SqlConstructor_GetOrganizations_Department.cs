using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.SqlConstructor.Organizations
{
    public class SqlConstructor_GetOrganizations_Department : SqlConstructor_GetOrganizations
    {
        public SqlConstructor_GetOrganizations_Department(NameValueCollection urlParameters, int OrganizationId)
            : base(urlParameters)
        {
            AllowedFieldNames.Add("UserCount");
            AllowedFieldNames.Add("ActivatedUsers");
            OrgId = OrganizationId;
        }

        private int orgId;
        public int OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }

        string m_mainSqlSelect =
           @"SELECT  O.Id AS OrgID, O.FullName AS FullName, O.INN AS OrgINN, 
                        ISNULL(Reg.Name,'') AS RegName, Reg.Id AS RegId ,
                        ISNULL(Type.Name,'') AS TypeName, Type.Id AS TypeId,
                        ISNULL(Kind.Name,'') AS KindName, Kind.Id AS KindId,
		                ISNULL(T1.UserCount,0) UserCount,
                        ISNULL(T2.ActivatedUsers,0) ActivatedUsers
                FROM dbo.Organization2010 O
                LEFT JOIN dbo.Region Reg ON O.RegionId=Reg.Id AND Reg.InOrganizationEtalon=1
                LEFT JOIN dbo.OrganizationType2010 Type ON O.TypeId=Type.Id
                LEFT JOIN dbo.OrganizationKind Kind ON O.KindId=Kind.Id
                left join
                (
                select O2010.Id, count(*) UserCount
                from 
					dbo.Account A
					left join Organization2010 O2010 on A.OrganizationId = O2010.Id
				group by O2010.Id
				) T1 on T1.Id = O.Id
                left join
                (
                select O2010.Id, count(*) ActivatedUsers
                from 
					dbo.Account A
					left join Organization2010 O2010 on A.OrganizationId = O2010.Id
                    where A.Status='activated'
				group by O2010.Id
				) T2 on T2.Id = O.Id 
                where O.StatusId = 1 AND O.DepartmentId = "; // StatusId = 1 -- действующая организация

        protected override string getMainSQL()
        {
            return m_mainSqlSelect + orgId.ToString(); 
        }

        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
            base.CreateParameters(parameters, whereExpr);

            // создание специфических параметров
            var userCount = GetVal_int("UserCount");
            if (userCount > 0)
            {
                if (userCount == 1)
                {
                    whereExpr.Add("UserCount=0"); // только "не привязанные" организации
                }
                else
                {
                    whereExpr.Add("UserCount>0"); // только организации с привязанными пользователями
                }
            }

            var activatedUsersCount = GetVal_int("ActivatedUsers");
            if (activatedUsersCount > 0)
            {
                whereExpr.Add(activatedUsersCount == 1 ? "ActivatedUsers=0" : "ActivatedUsers>0");
            }
        }
    }
}
