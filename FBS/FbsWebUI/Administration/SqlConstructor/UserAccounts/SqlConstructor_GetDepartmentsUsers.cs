using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.SqlConstructor.UserAccounts
{
    public class SqlConstructor_GetDepartmentsUsers : SqlConstructor_GetData
    {
        public SqlConstructor_GetDepartmentsUsers(NameValueCollection urlParameters)
            : base()
        {
            defaultOrderField = "CreateDate";
            AllowedFieldNames.AddRange(new string[] { "login", "organizationName", "email", "status", "eitype", "lastName" });
            AllowedFieldNames.AddRange(new string[] { "hasEtalonOrg", "operatorName", "hasComments" });
            m_urlParameters = urlParameters;
        }

        protected const string m_mainSQL =
            @"SELECT A.CreateDate, A.[Login], A.LastName, A.FirstName, A.PatronymicName, A.Email,
                OReq.FullName AS OrganizationName, A.[Status], OReq.TypeId as TypeId, 
                OReq.OrganizationId as OrgId, ISNULL(A_Op.LastName,'') OperatorName,
                CASE WHEN LEN(ISNULL(OpLog.Comments,''))=0 THEN 1 ELSE 2 END HasComments
              FROM dbo.Account A
              INNER JOIN dbo.OrganizationRequest2010 OReq ON A.OrganizationId=OReq.Id
              INNER JOIN dbo.GroupAccount AGr ON A.ID=AGr.AccountId
              LEFT JOIN dbo.OperatorLog OpLog ON A.ID=OpLog.CheckedUserID
              LEFT JOIN dbo.Account A_Op ON OpLog.OperatorID=A_Op.ID
              WHERE AGr.GroupId = 1 AND OReq.TypeId = 6";

        protected override string getMainSQL()
        {
            return m_mainSQL;
        }

        protected void AddParam_Str_Like(List<SqlParameter> parameters, List<string> whereExpr, string urlParamName, string sqlParamName, int paramLen)
        {
            string paramVal = GetVal_Str(urlParamName);
            if (paramVal.Length > 0)
            {
                whereExpr.Add(string.Format("{0} like '%'+@{0}+'%'", sqlParamName));
                AddSqlParam_str(parameters, sqlParamName, paramVal, paramLen);
            }
        }
        protected void AddParam_Str_Eq(List<SqlParameter> parameters, List<string> whereExpr, string urlParamName, string sqlParamName, int paramLen)
        {
            string paramVal = GetVal_Str(urlParamName);
            if (paramVal.Length > 0)
            {
                whereExpr.Add(string.Format("{0}=@{0}", sqlParamName));
                AddSqlParam_str(parameters, sqlParamName, paramVal, paramLen);
            }
        }
        protected void AddParam_Int_Eq(List<SqlParameter> parameters, List<string> whereExpr, string urlParamName, string sqlParamName)
        {
            int paramVal = GetVal_int(urlParamName);
            if (paramVal > 0)
            {
                whereExpr.Add(string.Format("{0}=@{0}", sqlParamName));
                AddSqlParam_int(parameters, sqlParamName, paramVal);
            }
        }

        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
            AddParam_Str_Like(parameters, whereExpr, "login", "Login", 255);
            AddParam_Str_Like(parameters, whereExpr, "organizationName", "OrganizationName", 255);
            AddParam_Str_Like(parameters, whereExpr, "email", "Email", 255);
            AddParam_Str_Eq(parameters, whereExpr, "status", "Status", 255);
            AddParam_Int_Eq(parameters, whereExpr, "eitype", "TypeId");

            //AddParam_Str_Like(parameters, whereExpr, "userLastName", "LastName", 255);
            //AddParam_Str_Like(parameters, whereExpr, "userFirstName", "FirstName", 255);
            //AddParam_Str_Like(parameters, whereExpr, "userPatronymicName", "PatronymicName", 255);

            AddParam_Str_Like(parameters, whereExpr, "operatorName", "OperatorName", 255);
            AddParam_Int_Eq(parameters, whereExpr, "hasComments", "HasComments");

            string paramVal = GetVal_Str("hasEtalonOrg");
            if (paramVal.Length > 0)
            {
                if (paramVal == "0")
                    whereExpr.Add("OrganizationId IS NULL");
                if (paramVal == "1")
                    whereExpr.Add("OrganizationId IS NOT NULL");
            }
        }
    }
}
