namespace Esrp.Core.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Esrp.Core.DataAccess;

    /// <summary>
    /// The system kind.
    /// </summary>
    public enum SystemKind
    {
        /// <summary>
        /// The esrp.
        /// </summary>
        Esrp = 1, 

        /// <summary>
        /// The fbs.
        /// </summary>
        Fbs = 2, 

        /// <summary>
        /// The fbd.
        /// </summary>
        Fbd = 3
    }

    /// <summary>
    /// The user registration type.
    /// </summary>
    [Flags]
    public enum UserRegistrationType
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0, 

        /// <summary>
        /// The fbs user.
        /// </summary>
        FbsUser = 1, 

        /// <summary>
        /// The fbd user.
        /// </summary>
        FbdUser = 2
    }

    /// <summary>
    /// The general system manager.
    /// </summary>
    public class GeneralSystemManager
    {
        #region Constants and Fields

        private static readonly Dictionary<string, Type> _mGroupTypes = new Dictionary<string, Type> {
                { EsrpManager.AdministratorGroupCode, typeof(AdministratorAccount) }, 
                { FbsManager.VuzGroupCode, typeof(UserAccount) }, 
                { FbsManager.SsuzGroupCode, typeof(UserAccount) }, 
                { EsrpManager.SupportGroupCode, typeof(SupportAccount) }, 
                { EsrpManager.AuthorizedStaffGroupCode, typeof(UserAccount) }, 
                { FbsManager.AuditorGroupCode, typeof(UserAccount) }, 
                { FbsManager.AdministratorGroupCode, typeof(UserAccount) }, 
                { FbsManager.FounderGroupCode, typeof(UserAccount) }, 
                { FbsManager.InfoProcessingGroupCode, typeof(UserAccount) }, 
                { FbdManager.AuthorizedStaffGroupCode, typeof(UserAccount) }, 
                { SOManager.AdministratorGroupCode, typeof(SOAccount) }, 
                { SOManager.UserGroupCode, typeof(SOAccount) }, 
            };

        private static Dictionary<int, string> _systemDescriptions = new Dictionary<int, string>();

        #endregion

        #region Public Methods and Operators
        
        /// <summary>
        /// По логину получаем имена ИС, в которых он зарегистрирован
        /// </summary>
        /// <param name="login">
        /// Логин пользователя 
        /// </param>
        /// <returns>
        /// </returns>
        public static List<string> AccessedSystems(string login)
        {
            var systemNames = new List<string>();
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT DISTINCT g.SystemID, s.Name
						FROM Account a JOIN GroupAccount ga ON a.Id = ga.AccountId
						JOIN [Group] g ON ga.GroupId = g.Id
                        JOIN [System] s ON s.SystemID=g.SystemID
						WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        systemNames.Add(dataReader[1].ToString());
                    }
                }
            }

            return systemNames;
        }


        public static List<string> AccessedSystemsId(string login)
        {
            var systemNames = new List<string>();
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT DISTINCT g.SystemID, s.Name
						FROM Account a JOIN GroupAccount ga ON a.Id = ga.AccountId
						JOIN [Group] g ON ga.GroupId = g.Id
                        JOIN [System] s ON s.SystemID=g.SystemID
						WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        systemNames.Add(dataReader[0].ToString());
                    }
                }
            }

            return systemNames;
        }

        /// <summary>
        /// может ли пользователь видеть подчиненные организации
        /// </summary>
        /// <param name="login">
        /// логин пользователя
        /// </param>
        /// <returns>
        /// может или нет
        /// </returns>
        public static bool CanViewSubordinateOrganizations(string login)
        {
            return Account.CheckRole(login, "ViewStatisticSubordinate")
                   || Account.IsUserFromMainOrg(login);
        }

        /// <summary>
        /// может ли пользователь менять модель приемной комиссии образовательного учреждения
        /// </summary>
        /// <param name="login">
        /// логин пользователя
        /// </param>
        /// <returns>
        /// может или нет
        /// </returns>
        public static bool CanChangeRCModel(string login)
        {
            return Account.CheckRole(login, "EditOURCModel") && IsOpenSystem();
        }

        /// <summary>
        /// Получить тип пользовательского аккаунта. 
        /// </summary>
        /// <param name="groupCode">
        /// The group Code.
        /// </param>
        /// <param name="systemKind">
        /// The system Kind.
        /// </param>
        public static Type GetAccountType(string groupCode, SystemKind systemKind)
        {
            // TODO: будут вызываться на основании идентификаторов системы соответствующие менеджеры систем.
            // пока реализовано для ФБС копи пастом.
            Type type;
            return _mGroupTypes.TryGetValue(groupCode, out type) ? type : typeof(UserAccount);
        }

        /// <summary>
        /// The get account type by group code.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The get account type by group code.
        /// </returns>
        public static string GetAccountTypeByGroupCode(Type type)
        {
            return
                _mGroupTypes.Where(groupTypeCode => groupTypeCode.Value == type).Select(
                    groupTypeCode => groupTypeCode.Key).FirstOrDefault();
        }

        /// <summary>
        /// The get system name.
        /// </summary>
        /// <param name="systemCode">
        /// The system code.
        /// </param>
        /// <returns>
        /// The get system name.
        /// </returns>
        public static string GetSystemName(int systemCode)
        {
            // не страшно, если попадём на параллельность, просто два раза вытянем данные,
            // зато без локов
            if (_systemDescriptions.Count == 0)
            {
                using (var executor = new DbExecutor())
                {
                    SqlCommand sqlCommand = executor.CreateCommand();
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = @" SELECT SystemId, Name FROM [System]";

                    var d = new Dictionary<int, string>();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        d.Add(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString());
                    }

                    _systemDescriptions = d;
                }
            }

            return _systemDescriptions[systemCode];
        }

        /// <summary>
        /// The get user groups.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="systemID">
        /// The system id.
        /// </param>
        /// <returns>
        /// </returns>
        public static UserGroupData[] GetUserGroups(string login, int systemID)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT g.Id, g.Code, g.Name
						FROM Account a JOIN GroupAccount ga ON a.Id = ga.AccountId
							JOIN [Group] g ON ga.GroupId = g.Id
						WHERE a.[Login] = @login AND g.SystemID = @systemID";
                SqlParameter l_prm = new SqlParameter("@login", SqlDbType.VarChar, 255);
                l_prm.Value = login;
                sqlCommand.Parameters.Add(l_prm);
                //sqlCommand.Parameters.AddWithValue("@login", login);
                sqlCommand.Parameters.AddWithValue("@systemID", systemID);
                var l = new List<UserGroupData>();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    l.Add(
                        new UserGroupData
                            {
                                ID = Convert.ToInt32(reader.GetValue(0)), 
                                Code = Convert.ToString(reader.GetValue(1)), 
                                Name = Convert.ToString(reader.GetValue(2))
                            });
                }

                return l.ToArray();
            }
        }

        /// <summary>
        /// The get user id.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The get user id.
        /// </returns>
        public static int GetUserID(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"
						SELECT a.Id
						FROM Account a WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(res);
            }
        }

        /// <summary>
        /// The get user organization main.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The get user organization main.
        /// </returns>
        public static int GetUserOrganizationMain(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT r.OrganizationId
						FROM Account a
						JOIN OrganizationRequest2010 r ON r.Id=a.OrganizationId
						WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(res);
            }
        }

        /// <summary>
        /// The get user organization request.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The get user organization request.
        /// </returns>
        public static int GetUserOrganizationRequest(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT a.OrganizationId
						FROM Account a WHERE a.[Login] = @login";
                //sqlCommand.Parameters.AddWithValue("@login", login);
                SqlParameter l_prm = new SqlParameter("@login", SqlDbType.VarChar, 255);
                l_prm.Value = login;
                sqlCommand.Parameters.Add(l_prm);

                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(res);
            }
        }

        /// <summary>
        /// Получает дату последнего обновления данных о пользователе
        /// </summary>
        /// <param name="login">
        /// Имя учетной записи пользователя 
        /// </param>
        /// <returns>
        /// Дата последнего обновления данных о пользователе 
        /// </returns>
        public static DateTime GetUserUpdateDate(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"
						SELECT a.UpdateDate
						FROM Account a WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(res);
            }
        }

        /// <summary>
        /// The has access to group.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="groupCode">
        /// The group code.
        /// </param>
        /// <returns>
        /// The has access to group.
        /// </returns>
        public static bool HasAccessToGroup(string login, string groupCode)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT COUNT(a.id)
						FROM Account a JOIN GroupAccount ga ON a.Id = ga.AccountId
							JOIN [Group] g ON ga.GroupId = g.Id
						WHERE a.[Login] = @login AND g.Code = @groupCode";
                sqlCommand.Parameters.AddWithValue("@login", login);
                sqlCommand.Parameters.AddWithValue("@groupCode", groupCode);
                int systemGroups = Convert.ToInt32(sqlCommand.ExecuteScalar());
                return systemGroups > 0;
            }
        }

        /// <summary>
        /// The has user access.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="systemKind">
        /// The system kind.
        /// </param>
        /// <returns>
        /// The has user access.
        /// </returns>
        public static bool HasUserAccess(string login, SystemKind systemKind)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT COUNT(a.id)
						FROM Account a JOIN GroupAccount ga ON a.Id = ga.AccountId
							JOIN [Group] g ON ga.GroupId = g.Id
						WHERE a.[Login] = @login AND g.SystemID = @systemID";
                SqlParameter l_prm = new SqlParameter("@login", SqlDbType.VarChar, 255);
                l_prm.Value = login;
                sqlCommand.Parameters.Add(l_prm);
                //sqlCommand.Parameters.AddWithValue("@login", login);
                sqlCommand.Parameters.AddWithValue("@systemID", (int)systemKind);
                int systemGroups = Convert.ToInt32(sqlCommand.ExecuteScalar());
                return systemGroups > 0;
            }
        }

        /// <summary>
        /// The is user activated.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The is user activated.
        /// </returns>
        public static bool IsUserActivated(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"SELECT TOP 1 1 FROM Account WHERE Status='activated' AND login=@login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object o = sqlCommand.ExecuteScalar();
                return o != null && o != DBNull.Value;
            }
        }

        /// <summary>
        /// Обновление поля OrganizationId в таблице Account 
        /// </summary>
        /// <param name="login">
        /// Логин пользователя
        /// </param>
        /// <param name="orgID">
        /// Ид заявки
        /// </param>
        /// <returns>
        /// The set user organization request.
        /// </returns>
        public static int SetUserOrganizationRequest(string login, int orgID)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"UPDATE Account SET OrganizationId=@orgID WHERE [Login]=@login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                sqlCommand.Parameters.AddWithValue("@orgID", orgID <= 0 ? (object)DBNull.Value : orgID).SqlDbType = SqlDbType.Int;
                var res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(res);
            }
        }

        /// <summary>
        /// Открытая или закрытая система ЕСРП
        /// </summary>
        /// <returns>true - если открытая. false - если закрытая</returns>
        public static bool IsOpenSystem()
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"select count(*) from [Settings] where name='IsOpenSystem' and value = 'yes'";
                var res = sqlCommand.ExecuteScalar();
                
                if (res == null || res == DBNull.Value || Convert.ToInt32(res) == 0)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        /// <summary>
        /// The user group data.
        /// </summary>
        public class UserGroupData
        {
            #region Public Properties

            /// <summary>
            /// Gets or sets Code.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Gets or sets ID.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Gets or sets Name.
            /// </summary>
            public string Name { get; set; }

            #endregion
        }
    }
}