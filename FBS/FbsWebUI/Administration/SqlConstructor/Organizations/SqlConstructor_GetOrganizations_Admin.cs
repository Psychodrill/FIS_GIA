using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.SqlConstructor.Organizations
{
    public class SqlConstructor_GetOrganizations_Admin : SqlConstructor_GetOrganizations
    {
        public SqlConstructor_GetOrganizations_Admin(NameValueCollection urlParameters)
            : base(urlParameters)
        {
            AllowedFieldNames.Add("UserCount");
            AllowedFieldNames.Add("ActivatedUsers");
        }

        protected const string m_mainSQL =
            @"SELECT  O.Id AS OrgID, O.FullName AS FullName, O.INN AS OrgINN, 
                        ISNULL(Reg.Name,'') AS RegName, Reg.Id AS RegId ,
                        ISNULL(Type.Name,'') AS TypeName, Type.Id AS TypeId,
                        ISNULL(Kind.Name,'') AS KindName, Kind.Id AS KindId,
		                ISNULL(Reqs.UserCount,0) UserCount,
                        ISNULL(AUC.ActivatedUsers,0) ActivatedUsers
                FROM dbo.Organization2010 O
                LEFT JOIN dbo.Region Reg ON O.RegionId=Reg.Id AND Reg.InOrganizationEtalon=1
                LEFT JOIN dbo.OrganizationType2010 Type ON O.TypeId=Type.Id
                LEFT JOIN dbo.OrganizationKind Kind ON O.KindId=Kind.Id
                LEFT JOIN 
                (
	                SELECT O.Id, COUNT(*) UserCount
	                FROM dbo.Organization2010 O
	                INNER JOIN dbo.Account A ON O.Id=A.OrganizationId
	                GROUP BY O.Id
                ) Reqs ON O.Id=Reqs.Id 
                LEFT JOIN 
                (
	                SELECT O.Id, COUNT(*) ActivatedUsers
	                FROM dbo.Organization2010 O
	                INNER JOIN dbo.Account A ON O.Id=A.OrganizationId
	                GROUP BY O.Id
                ) AUC ON O.Id=AUC.Id 
                WHERE O.StatusId = 1"; // StatusId = 3 --  Ликвидированная организация, не показывать, 2 - реорганизованная, не показывать

        protected override string getMainSQL()
        {
            return m_mainSQL;
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
