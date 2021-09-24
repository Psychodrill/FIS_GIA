using System.Collections.Specialized;


namespace Fbs.Web.Administration.SqlConstructor.Organizations
{
    public class SqlConstructor_GetOrganizations_Registration : SqlConstructor_GetOrganizations
    {
        public SqlConstructor_GetOrganizations_Registration(NameValueCollection urlParameters)
            : base(urlParameters)
        {
            defaultPageSize = 10;
        }

        protected const string m_mainSQL =
            @"SELECT  O.Id OrgID, O.FullName AS FullName, O.INN AS INN, O.IsPrivate AS IsPrivate,
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
    }
}
