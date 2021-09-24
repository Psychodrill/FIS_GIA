namespace Esrp.Web.Administration.SqlConstructor.UserAccounts
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data.SqlClient;

    /// <summary>
    /// The sql constructor_ get users.
    /// </summary>
    public class SqlConstructor_GetUsers : SqlConstructor_GetData
    {
        #region Constants and Fields

        /// <summary>
        /// The m_main sql.
        /// </summary>
        protected const string m_mainSQL =
            @"SELECT A.CreateDate, A.[Login], A.LastName, A.FirstName, A.PatronymicName, A.Email,
                OReq.FullName AS OrganizationName, A.[Status], OReq.TypeId as TypeId, 
                OReq.OrganizationId as OrgId, ISNULL(A_Op.LastName,'') OperatorName,
                CASE WHEN LEN(ISNULL(OpLog.Comments,''))=0 THEN 1 ELSE 2 END HasComments
              FROM dbo.Account A
              INNER JOIN dbo.OrganizationRequest2010 OReq ON A.OrganizationId=OReq.Id
              LEFT JOIN dbo.OperatorLog OpLog ON A.ID=OpLog.CheckedUserID
              LEFT JOIN dbo.Account A_Op ON OpLog.OperatorID=A_Op.ID
              WHERE A.OrganizationId IS NOT NULL AND
                    EXISTS (SELECT * FROM GroupAccount AGr WHERE AGr.AccountId=A.ID)";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConstructor_GetUsers"/> class.
        /// </summary>
        /// <param name="urlParameters">
        /// The url parameters.
        /// </param>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        public SqlConstructor_GetUsers(NameValueCollection urlParameters, string userLogin)
        {
            this.defaultOrderField = "CreateDate";
            this.AllowedFieldNames.AddRange(
                new[] { "login", "organizationName", "email", "status", "eitype", "lastName" });
            this.AllowedFieldNames.AddRange(new[] { "hasEtalonOrg", "operatorName", "hasComments" });
            this.m_urlParameters = urlParameters;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add param_ int_ eq.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="whereExpr">
        /// The where expr.
        /// </param>
        /// <param name="urlParamName">
        /// The url param name.
        /// </param>
        /// <param name="sqlParamName">
        /// The sql param name.
        /// </param>
        protected void AddParam_Int_Eq(
            List<SqlParameter> parameters, List<string> whereExpr, string urlParamName, string sqlParamName)
        {
            int paramVal = this.GetVal_int(urlParamName);
            if (paramVal > 0)
            {
                whereExpr.Add(string.Format("{0}=@{0}", sqlParamName));
                AddSqlParam_int(parameters, sqlParamName, paramVal);
            }
        }

        /// <summary>
        /// The add param_ str_ eq.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="whereExpr">
        /// The where expr.
        /// </param>
        /// <param name="urlParamName">
        /// The url param name.
        /// </param>
        /// <param name="sqlParamName">
        /// The sql param name.
        /// </param>
        /// <param name="paramLen">
        /// The param len.
        /// </param>
        protected void AddParam_Str_Eq(
            List<SqlParameter> parameters, 
            List<string> whereExpr, 
            string urlParamName, 
            string sqlParamName, 
            int paramLen)
        {
            string paramVal = this.GetVal_Str(urlParamName);
            if (paramVal.Length > 0)
            {
                whereExpr.Add(string.Format("{0}=@{0}", sqlParamName));
                AddSqlParam_str(parameters, sqlParamName, paramVal, paramLen);
            }
        }

        /// <summary>
        /// The add param_ str_ like.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="whereExpr">
        /// The where expr.
        /// </param>
        /// <param name="urlParamName">
        /// The url param name.
        /// </param>
        /// <param name="sqlParamName">
        /// The sql param name.
        /// </param>
        /// <param name="paramLen">
        /// The param len.
        /// </param>
        protected void AddParam_Str_Like(
            List<SqlParameter> parameters, 
            List<string> whereExpr, 
            string urlParamName, 
            string sqlParamName, 
            int paramLen)
        {
            string paramVal = this.GetVal_Str(urlParamName);
            if (paramVal.Length > 0)
            {
                whereExpr.Add(string.Format("{0} like '%'+@{0}+'%'", sqlParamName));
                AddSqlParam_str(parameters, sqlParamName, paramVal, paramLen);
            }
        }

        /// <summary>
        /// The create parameters.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="whereExpr">
        /// The where expr.
        /// </param>
        protected override void CreateParameters(List<SqlParameter> parameters, List<string> whereExpr)
        {
            this.AddParam_Str_Like(parameters, whereExpr, "login", "Login", 255);
            this.AddParam_Str_Like(parameters, whereExpr, "organizationName", "OrganizationName", 255);
            this.AddParam_Str_Like(parameters, whereExpr, "email", "Email", 255);

            var statusIds = this.GetVal_Str("status");
            if (statusIds.Length > 0)
            {
                whereExpr.Add("(Status IN (SELECT [nam] FROM ufn_ut_SplitFromString(@StatusID,',')))");
                AddSqlParam_str(parameters, "StatusID", statusIds, statusIds.Length);
            }

            var typeIds = this.GetVal_Str("eitype");
            if (typeIds.Length > 0)
            {
                whereExpr.Add("(TypeId IN (SELECT [nam] FROM ufn_ut_SplitFromString(@TypeID,',')))");
                AddSqlParam_str(parameters, "TypeID", typeIds, typeIds.Length);
            }
            
            this.AddParam_Str_Like(parameters, whereExpr, "operatorName", "OperatorName", 255);
            this.AddParam_Int_Eq(parameters, whereExpr, "hasComments", "HasComments");

            string paramVal = this.GetVal_Str("hasEtalonOrg");
            if (paramVal.Length > 0)
            {
                if (paramVal == "0")
                {
                    whereExpr.Add("OrganizationId IS NULL");
                }

                if (paramVal == "1")
                {
                    whereExpr.Add("OrganizationId IS NOT NULL");
                }
            }
        }

        /// <summary>
        /// The get main sql.
        /// </summary>
        /// <returns>
        /// The get main sql.
        /// </returns>
        protected override string getMainSQL()
        {
            return m_mainSQL;
        }

        #endregion
    }
}