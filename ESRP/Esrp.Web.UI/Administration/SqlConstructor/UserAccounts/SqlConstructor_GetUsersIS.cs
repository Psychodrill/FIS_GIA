namespace Esrp.Web.Administration.SqlConstructor.UserAccounts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Esrp.Core;
    using Esrp.Core.Systems;

    /// <summary>
    /// The sql constructor_ get users is.
    /// </summary>
    public class SqlConstructor_GetUsersIS : SqlConstructor_GetData
    {
        #region Constants and Fields

        /// <summary>
        /// The m_main sql.
        /// </summary>
        protected const string m_mainSQL =
            @"SELECT A.CreateDate, A.[Login], A.LastName, A.FirstName, A.PatronymicName, A.Email,
                OReq.FullName AS OrganizationName, A.[Status], OReq.TypeId as TypeId, 
                OReq.OrganizationId as OrgId, ISNULL(A_Op.LastName,'') OperatorName,
                CASE WHEN LEN(ISNULL(OpLog.Comments,''))=0 THEN 1 ELSE 2 END HasComments,
				Gr.name as groupName
              FROM dbo.Account A
              INNER JOIN dbo.OrganizationRequest2010 OReq ON A.OrganizationId=OReq.Id
              INNER JOIN dbo.GroupAccount AGr ON A.ID=AGr.AccountId
              INNER JOIN dbo.[Group] Gr ON Gr.Id=AGr.GroupId
              LEFT JOIN dbo.OperatorLog OpLog ON A.ID=OpLog.CheckedUserID
              LEFT JOIN dbo.Account A_Op ON OpLog.OperatorID=A_Op.ID
              WHERE AGr.IsUserIS=1 AND ";

        private const string adminAllowedGroupsSQL = "1=1";

        private const string hotLineAllowedGroupsSQL = "AGr.GroupId in (2,5)";

        private const string missingAllowedGroupsSQL = "AGr.GroupId in (-1)";

        private readonly string userAddPartSQL = missingAllowedGroupsSQL;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConstructor_GetUsersIS"/> class.
        /// </summary>
        /// <param name="urlParameters">
        /// The url parameters.
        /// </param>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        public SqlConstructor_GetUsersIS(NameValueCollection urlParameters, string userLogin)
        {
            if (GeneralSystemManager.HasAccessToGroup(userLogin, EsrpManager.AdministratorGroupCode))
            {
                this.userAddPartSQL = adminAllowedGroupsSQL;
            }
            else
            {
                if (GeneralSystemManager.HasAccessToGroup(userLogin, EsrpManager.SupportGroupCode))
                {
                    this.userAddPartSQL = hotLineAllowedGroupsSQL;
                }
            }

            this.defaultOrderField = "CreateDate";
            this.AllowedFieldNames.AddRange(
                new[] { "login", "organizationName", "email", "status", "eitype", "lastName", "groupName" });
            this.AllowedFieldNames.AddRange(new[] { "hasEtalonOrg", "operatorName", "hasComments" });
            this.m_urlParameters = urlParameters;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The delete user groups exclude.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="excludeGroups">
        /// The exclude groups.
        /// </param>
        public static void DeleteUserGroupsExclude(string userLogin, params int[] excludeGroups)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                string postfix = string.Empty;
                if (excludeGroups.Length > 0)
                {
                    postfix = "AND GroupId NOT IN ("
                              + string.Join(",", excludeGroups.Select(x => x.ToString()).ToArray()) + ")";
                }

                Cmd.CommandText =
                    @"
DECLARE @accountId int
SELECT @accountId=Id From [Account] WHERE [login]=@loginName
DELETE FROM GroupAccount where [AccountId]=@accountId "
                    + postfix;
                Cmd.Parameters.AddWithValue("@loginName", userLogin);

                Cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The delete user groups is.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        public static void DeleteUserGroupsIS(string userLogin)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText =
                    @"
                                    DECLARE @accountId int
                                    SELECT @accountId=Id From [Account] WHERE [login]=@loginName
                                    DELETE FROM GroupAccount where [AccountId]=@accountId AND GroupId IN (SELECT Id From [Group] WHERE IsUserIS=1)";
                cmd.Parameters.AddWithValue("@loginName", userLogin);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Удаление групп у пользователя
        /// </summary>
        /// <param name="userLogin">
        /// логин пользователя
        /// </param>
        /// <param name="includeGroups">
        /// Список групп
        /// </param>
        public static void DeleteUserGroupsInclude(string userLogin, params int[] includeGroups)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                var postfix = string.Empty;
                if (includeGroups.Length > 0)
                {
                    postfix = "AND GroupId IN (" + string.Join(",", includeGroups.Select(x => x.ToString()).ToArray())
                              + ")";
                }

                cmd.CommandText =
                    @"DECLARE @accountId int
                        SELECT @accountId=Id From [Account] WHERE [login]=@loginName
                        DELETE FROM GroupAccount where [AccountId]=@accountId "
                    + postfix;
                cmd.Parameters.AddWithValue("@loginName", userLogin);

                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteUserGroup(string userLogin,string groupCode)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText =
                    "DELETE FROM GroupAccount where [AccountId]=(SELECT Id From [Account] WHERE [login]=@loginName) AND GroupId=(SELECT Id FROM [Group] WHERE Code=@groupCode)";
                cmd.Parameters.AddWithValue("@loginName", userLogin);
                cmd.Parameters.AddWithValue("@groupCode", groupCode);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The get available groups.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable GetAvailableGroups(string userLogin)
        {
            string partSql = "1=0";
            if (GeneralSystemManager.HasAccessToGroup(userLogin, EsrpManager.AdministratorGroupCode))
            {
                partSql = "1=1";
            }
            else
            {
                if (GeneralSystemManager.HasAccessToGroup(userLogin, EsrpManager.SupportGroupCode))
                {
                    partSql = "Id IN (2,5)";
                }
            }

            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = "SELECT Id, Code, Name FROM [Group] WHERE IsUserIS=1 AND [Default]=0 AND " + partSql;
                Result.Load(Cmd.ExecuteReader());
            }

            return Result;
        }

        /// <summary>
        /// The get user group.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// The get user group.
        /// </returns>
        public static int GetUserGroup(string userLogin)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =
                    @"SELECT TOP 1 ga.groupID FROM GroupAccount ga JOIN [Account] a ON ga.AccountId=a.Id
WHERE a.[login]=@loginName";
                Cmd.Parameters.AddWithValue("@loginName", userLogin);
                object res = Cmd.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return -1;
                }

                return Convert.ToInt32(res);
            }
        }

        /// <summary>
        /// The get user group name.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// The get user group name.
        /// </returns>
        public static string GetUserGroupName(string userLogin)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =
                    @"SELECT TOP 1 g.Name FROM GroupAccount ga JOIN [Account] a ON ga.AccountId=a.Id
JOIN [Group] g ON g.Id=ga.GroupId
WHERE a.[login]=@loginName";
                Cmd.Parameters.AddWithValue("@loginName", userLogin);
                object res = Cmd.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return string.Empty;
                }

                return Convert.ToString(res);
            }
        }

        /// <summary>
        /// The get user group names.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// The get user group names.
        /// </returns>
        public static string GetUserGroupNames(string userLogin)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =
                    @"SELECT g.Name FROM GroupAccount ga JOIN [Account] a ON ga.AccountId=a.Id
JOIN [Group] g ON g.Id=ga.GroupId
WHERE a.[login]=@loginName";
                Cmd.Parameters.AddWithValue("@loginName", userLogin);
                SqlDataReader reader = Cmd.ExecuteReader();
                var l = new List<string>();
                while (reader.Read())
                {
                    l.Add(reader.GetString(0));
                }

                return string.Join(", ", l.Distinct().ToArray());
            }
        }

        /// <summary>
        /// The get user groups.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// </returns>
        public static int[] GetUserGroups(string userLogin)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =
                    @"SELECT ga.groupID FROM GroupAccount ga JOIN [Account] a ON ga.AccountId=a.Id
WHERE a.[login]=@loginName";
                Cmd.Parameters.AddWithValue("@loginName", userLogin);
                var l = new List<int>();
                SqlDataReader reader = Cmd.ExecuteReader();
                while (reader.Read())
                {
                    l.Add(reader.GetInt32(0));
                }

                return l.ToArray();
            }
        }

        /// <summary>
        /// The get user groups.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// </returns>
        public static string[] GetUserGroupCodes(string userLogin)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =
                    @"SELECT g.Code FROM [Group] g JOIN GroupAccount ga ON g.Id=ga.GroupId JOIN [Account] a ON ga.AccountId=a.Id
WHERE a.[login]=@loginName";
                Cmd.Parameters.AddWithValue("@loginName", userLogin);
                var l = new List<string>();
                SqlDataReader reader = Cmd.ExecuteReader();
                while (reader.Read())
                {
                    l.Add(reader.GetString(0));
                }

                return l.ToArray();
            }
        }

        /// <summary>
        /// The set user group.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="groupID">
        /// The group id.
        /// </param>
        /// <param name="dropOthers">
        /// The drop others.
        /// </param>
        public static void SetUserGroup(string userLogin, int groupID, bool dropOthers)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =@" DECLARE @accountId int
                                    SELECT @accountId=Id From [Account] WHERE [login]=@loginName
                                    DELETE FROM GroupAccount where [AccountId]=@accountId AND (GroupId=@groupId OR @dropOthers = 1)
                                    IF(NOT EXISTS(SELECT * FROM GroupAccount ga WHERE ga.[AccountId]=@accountId AND ga.GroupId=@groupId))
                                    BEGIN
	                                    INSERT INTO GroupAccount(AccountId, GroupId) VALUES(@accountId, @groupId)
                                    END";
                Cmd.Parameters.AddWithValue("@loginName", userLogin);
                Cmd.Parameters.AddWithValue("@groupId", groupID);
                Cmd.Parameters.AddWithValue("@dropOthers", dropOthers);

                Cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The set user group.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="groupCode">
        /// The group code.
        /// </param>
        /// <param name="dropOthers">
        /// The drop others.
        /// </param>
        public static void SetUserGroup(string userLogin, string groupCode, bool dropOthers)
        {
            int groupId;
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = @"SELECT Id From [Group] WHERE [Code]=@groupCode";
                Cmd.Parameters.AddWithValue("@groupCode", groupCode);
                groupId = Convert.ToInt32(Cmd.ExecuteScalar());
            }

            SetUserGroup(userLogin, groupId, dropOthers);
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
            this.AddParam_Str_Eq(parameters, whereExpr, "status", "Status", 255);
            this.AddParam_Int_Eq(parameters, whereExpr, "eitype", "TypeId");

            // AddParam_Str_Like(parameters, whereExpr, "userLastName", "LastName", 255);
            // AddParam_Str_Like(parameters, whereExpr, "userFirstName", "FirstName", 255);
            // AddParam_Str_Like(parameters, whereExpr, "userPatronymicName", "PatronymicName", 255);
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
            return m_mainSQL + this.userAddPartSQL;
        }

        #endregion
    }
}