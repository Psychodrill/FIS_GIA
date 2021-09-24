using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Esrp.Web.Administration.SqlConstructor.Organizations
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
                inner JOIN 
                (
	                SELECT OReq.OrganizationId
	                FROM dbo.OrganizationRequest2010 OReq
	                INNER JOIN dbo.Account A ON OReq.Id=A.OrganizationId
	                WHERE OReq.OrganizationId IS NOT NULL ";

        string m_mainSqlGroupBy = @" GROUP BY OReq.OrganizationId 
                                    ) Reqs ON O.DepartmentId=Reqs.OrganizationId OR O.MainId = Reqs.OrganizationId
                left join
                (
                select Or2010.OrganizationId, count(*) UserCount
                from 
					dbo.Account A
					left join OrganizationRequest2010 Or2010 on A.OrganizationId = Or2010.Id
				group by Or2010.OrganizationId
				) T1 on T1.OrganizationId = O.Id 
                left join
                (
                select Or2010.OrganizationId, count(*) ActivatedUsers
                from 
					dbo.Account A
					left join OrganizationRequest2010 Or2010 on A.OrganizationId = Or2010.Id
                    where A.Status='activated'
				group by Or2010.OrganizationId
				) T2 on T2.OrganizationId = O.Id";

        protected override string getMainSQL()
        {
            return m_mainSqlSelect + " and OReq.Id = " + orgId.ToString()+m_mainSqlGroupBy; 
        }

        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
            base.CreateParameters(parameters, whereExpr);

            // создание специфичного параметра
            var userCount = GetVal_int("UserCount");
            if (userCount > 0)
            {
                if (userCount == 1)
                {
                    // только "не привязанные" организации
                    whereExpr.Add("UserCount=0");
                }
                else
                {
                    // только организации с привязанными пользователями
                    whereExpr.Add("UserCount>0");
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
