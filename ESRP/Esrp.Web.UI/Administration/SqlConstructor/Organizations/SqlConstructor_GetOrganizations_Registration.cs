using System.Collections.Specialized;


namespace Esrp.Web.Administration.SqlConstructor.Organizations
{
    public class SqlConstructor_GetOrganizations_Registration : SqlConstructor_GetOrganizations
    {
        public SqlConstructor_GetOrganizations_Registration(NameValueCollection urlParameters)
            : base(urlParameters)
        {
            defaultPageSize = 10;
        }

        protected const string m_mainSQL =
            @"SELECT  O.Id OrgID, O.FullName AS FullName, O.INN AS INN, O.IsPrivate AS IsPrivate, O.StatusId,
                        ISNULL(Reg.Name,'') AS RegName, Reg.Id AS RegId,
		                ISNULL(Kind.Name,'') AS KindName, Kind.Id AS KindId,
                        ISNULL(Type.Name,'') AS TypeName, Type.Id AS TypeId
                FROM dbo.Organization2010 O
                LEFT JOIN dbo.Region Reg ON O.RegionId=Reg.Id AND Reg.InOrganizationEtalon=1
                LEFT JOIN dbo.OrganizationKind Kind ON O.KindId=Kind.Id
                LEFT JOIN dbo.OrganizationType2010 Type ON O.TypeId=Type.Id";



        protected override string getMainSQL()
        {
            return m_mainSQL;
        }

        protected override void CreateParameters(System.Collections.Generic.List<System.Data.SqlClient.SqlParameter> parameters, System.Collections.Generic.List<string> whereExpr)
        {
            base.CreateParameters(parameters, whereExpr);
            whereExpr.Add("(StatusId not in (2,3))");
        }
    }
}
