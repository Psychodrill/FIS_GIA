namespace Esrp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Esrp.Core;

    using Esrp.Web.ViewModel.InformationSystems;

    /// <summary>
    /// Сервис для работы с ИС
    /// </summary>
    public class InformationSystemsService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Получить ИД инфорационных систем, в которых зарегистрирован пользователь
        /// </summary>
        /// <param name="userLogin">
        /// Логин пользователя
        /// </param>
        /// <returns>
        /// ИД инфорационных систем, в которых зарегистрирован пользователь
        /// </returns>
        public int[] GetUserSystems(string userLogin)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText =
                    @"SELECT g.SystemID 
                        FROM [Group] g
                        JOIN [GroupAccount] ga ON ga.GroupId=g.Id
                        JOIN [Account] a ON ga.AccountId=a.Id
                        WHERE a.[login]=@loginName";
                cmd.Parameters.AddWithValue("@loginName", userLogin);
                var l = new List<int>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    l.Add(reader.GetInt32(0));
                }

                return l.ToArray();
            }
        }

        /// <summary>
        /// Получение доступных ИС для регистрации пользователя 
        /// </summary>
        /// <returns>
        /// Доступные ИС для регистрации пользователя
        /// </returns>
        public DataTable GetAvailableSystems()
        {
            var result = new DataTable();
            using (var connection = new SqlConnection(DBSettings.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT SystemID, Name FROM [System] WHERE AvailableRegistration=1";
                result.Load(cmd.ExecuteReader());
            }

            return result;
        }

        /// <summary>
        /// Получение доступных ИС текущего пользователя для регистрации нового пользователя
        /// </summary>
        /// <param name="login">Логин текущего пользователя</param>
        /// <returns>Доступные ИС текущего пользователя для регистрации нового пользователя</returns>
        public DataTable GetAvailableSystemsByLogin(string login)
        {
            var result = new DataTable();
            using (var connection = new SqlConnection(DBSettings.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT distinct s.[SystemID], s.[Name]
                                        FROM [Account] a
                                        JOIN [GroupAccount] ga ON a.[Id]=ga.[AccountId]
                                        JOIN [Group] g ON g.[Id]=ga.[GroupId]
                                        JOIN [System] s ON s.[SystemID]=g.[SystemID]
                                        WHERE AvailableRegistration=1 AND a.[Login]=@login";
                cmd.Parameters.AddWithValue("@login", login);
                result.Load(cmd.ExecuteReader());
            }

            return result;
        }

        /// <summary>
        /// Получить текущий идентификатор для таблицы с ИС
        /// </summary>
        /// <returns>
        /// Текущий идентификатор таблицы
        /// </returns>
        public int GetCurrentSystemId()
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IDENT_CURRENT('[dbo].[System]')";
                result = Convert.ToInt32(cmd.ExecuteScalar());

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Получение информации о ИС по Id
        /// </summary>
        /// <param name="systemId">
        /// Идентификатор ИС
        /// </param>
        /// <returns>
        /// Информация о ИС
        /// </returns>
        public InformationSystemEntity SelectInformationSystemById(int systemId)
        {
            var result = new InformationSystemEntity();
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText =
                    @"SELECT Name as ShortName, SystemID, FullName, AvailableRegistration, Code
                                        FROM System WHERE SystemID=@SystemID";
                cmd.Parameters.AddWithValue("@SystemID", systemId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Code = (string)reader["Code"];
                        result.SystemId = (int)reader["SystemID"];
                        result.ShortName = (string)reader["ShortName"];
                        result.FullName = (string)reader["FullName"];
                        if ((bool)reader["AvailableRegistration"])
                        {
                            result.AvailableRegistration = true;
                        }
                        else
                        {
                            result.AvailableRegistration = false;
                        }
                    }

                    reader.Close();
                }

                conn.Close();
            }

            if (result.SystemId == 0)
            {
                result.AvailableRegistrationEnable = false;
                result.AvailableRegistration = false;
                result.IsExistDefautlGroup = true;
                result.VisibleHrefAddGroup = false;
            }
            else
            {
                int countGroup = GroupService.CountGroupForSystem(systemId);
                result.IsExistDefautlGroup = countGroup > 0;
            }

            return result;
        }

        /// <summary>
        /// Получение списка ИС
        /// </summary>
        /// <returns>
        /// Возвращает список, существующих ИС
        /// </returns>
        public List<InformationSystemsView> SelectInformationSystems()
        {
            var result = new List<InformationSystemsView>();

            using (var cmd = new Command("dbo.SelectInformationSystems"))
            {
                for (SqlDataReader reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var informationSystem = new InformationSystemsView
                        {
                            SystemId = (int)reader["SystemID"], 
                            ShortName = (string)reader["ShortName"], 
                            FullName = (string)reader["FullName"], 
                            NumberGroups = (int)reader["NumberGroups"], 
                            AvailableRegistration = (bool)reader["AvailableRegistration"] == false ? "Нет" : "Да"
                        };

                    result.Add(informationSystem);
                }
            }

            return result;
        }

        /// <summary>
        /// Получение информации о ИС для Регистрации
        /// </summary>
        /// <param name="orgId">
        /// Идентификатор организации
        /// </param>
        /// <returns>
        /// Информация о ИС для Регистрации
        /// </returns>
        public List<InformationSystemsRegistrationView> SelectInformationSystems(string orgId)
        {
            var result = new List<InformationSystemsRegistrationView>();
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"SELECT Name as ShortName, SystemID FROM [System] WHERE [AvailableRegistration] = 1";
                for (SqlDataReader reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var informationSystem = new InformationSystemsRegistrationView
                        {
                           SystemId = (int)reader["SystemID"], ShortName = (string)reader["ShortName"] 
                        };

                    result.Add(informationSystem);
                }

                conn.Close();
            }

            PrepareSystemCustomLogic(result, orgId);
            return result;
        }

        /// <summary>
        /// Этот метод возникает при Update
        /// он должен быть пустым, так как Update происходит позже
        /// </summary>
        /// <param name="informationSystemsRegistrationView">
        /// Представления для регистрации ИС
        /// </param>
        public void UpdateInformationSystems(InformationSystemsRegistrationView informationSystemsRegistrationView)
        {
        }

        /// <summary>
        /// Создает или обновляет ИС
        /// </summary>
        /// <param name="informationSystem">
        /// Информация о ИС
        /// </param>
        /// <returns>
        /// Если ИС новая, то возвращаем ее Id
        /// </returns>
        public InformationSystemEntity UpdateOrCreateInformationSystem(InformationSystemEntity informationSystem)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                int currentId = this.GetCurrentSystemId();
                if (informationSystem.SystemId > currentId)
                {
                    cmd.Parameters.AddWithValue("@Code", informationSystem.Code);
                    cmd.Parameters.AddWithValue("@ShortName", informationSystem.ShortName);
                    cmd.Parameters.AddWithValue("@FullName", informationSystem.FullName);
                    cmd.Parameters.AddWithValue("@AvailableRegistration", informationSystem.AvailableRegistration);
                    cmd.CommandText =
                        string.Format(
                            @"
						    INSERT INTO 
							    dbo.System 
							    (Code, Name, FullName, AvailableRegistration)
						    VALUES 
							    (@Code, @ShortName, @FullName, @AvailableRegistration)");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SystemID", informationSystem.SystemId);
                    cmd.Parameters.AddWithValue("@Code", informationSystem.Code);
                    cmd.Parameters.AddWithValue("@ShortName", informationSystem.ShortName);
                    cmd.Parameters.AddWithValue("@FullName", informationSystem.FullName);
                    cmd.Parameters.AddWithValue("AvailableRegistration", informationSystem.AvailableRegistration);

                    cmd.CommandText =
                        @"
						    UPDATE 
							    dbo.System 
						    SET 
							    FullName=@FullName,
                                Code=@Code,
                                Name=@ShortName,
                                AvailableRegistration=@AvailableRegistration
						    WHERE 
							    SystemID=@SystemID";
                }

                cmd.ExecuteNonQuery();

                conn.Close();
            }

            return informationSystem;
        }

        #endregion

        #region Methods

        private static void PrepareSystemCustomLogic(List<InformationSystemsRegistrationView> systems, string orgId)
        {
            foreach (InformationSystemsRegistrationView systemRegistrationView in systems)
            {
                if (string.IsNullOrEmpty(orgId))
                {
                    systemRegistrationView.PhoneEnable = false;
                    systemRegistrationView.PositionEnable = false;
                    systemRegistrationView.FullNameEnable = false;
                    systemRegistrationView.EmailEnable = false;
                    systemRegistrationView.AccessToSystemEnable = false;
                }

                if (systemRegistrationView.SystemId == 3)
                {
                    systemRegistrationView.SameDataAsFbsVisible = true;
                    systemRegistrationView.AccessToSystem = false;
                }
            }
        }

        #endregion
    }
}