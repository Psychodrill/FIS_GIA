namespace Esrp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using Esrp.Core;

    using Esrp.Web.ViewModel.Group;

    /// <summary>
    /// Сервис для работы с группами
    /// </summary>
    public class GroupService
    {
        /// <summary>
        /// Метод создает группу
        /// </summary>
        /// <param name="name">Наименование группы</param>
        /// <param name="code">Обозначение группы</param>
        /// <param name="systemId">Идентификатор Информационной Системы</param>
        public static void CreateGroup(string name, string code, int systemId)
        {
            var flagDefaultGroup = IsExistDefaultGroupForSystem(systemId);
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@SystemID", systemId);
                if (flagDefaultGroup)
                {
                    cmd.CommandText =
                    string.Format(
                        @"INSERT INTO [Group] ([Code],[Name],[SystemID])
                                                   VALUES (@Code, @Name, @SystemID)");
                }
                else
                {
                    cmd.CommandText =
                    string.Format(
                        @"INSERT INTO [Group] ([Code],[Name],[SystemID],[Default])
                                                   VALUES (@Code, @Name, @SystemID,1)");
                }

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static int CountGroupForSystem(int systemId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"SELECT Count(Id) as count
                                      FROM [Group]
                                      where [SystemID]=@SystemId";
                cmd.Parameters.AddWithValue("@SystemId", systemId);

                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    conn.Close();
                    return 0;
                }

                conn.Close();
                return Convert.ToInt32(result);
            }
        }

        public static bool IsExistDefaultGroupForSystem(int systemId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"SELECT Count(Id) as count
                                      FROM [Group]
                                      where [SystemID]=@SystemId AND [Default]=1 ";
                cmd.Parameters.AddWithValue("@SystemId", systemId);

                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value || Convert.ToInt32(result) == 0)
                {
                    conn.Close();
                    return false;
                }

                conn.Close();
                return true;
            }
        }

        /// <summary>
        /// Получить список групп по ид ИС с поддержкой пейджинга
        /// </summary>
        /// <param name="systemId">Идентификатор ИС</param>
        /// <returns>
        /// список групп по ид ИС
        /// </returns>
        public List<GroupView> SelectGroupList(int systemId)
        {
            var result = new List<GroupView>();

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"SELECT [Id], [Code], [Name], [Default]
                                      FROM [Group]
                                      where [SystemID]=@SystemId";
                cmd.Parameters.AddWithValue("@SystemId", systemId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var groupView = new GroupView
                        {
                            Id = (int)reader["Id"],
                            Code = (string)reader["Code"],
                            Name = (string)reader["Name"],
                            Default = (bool)reader["Default"]
                        };
                        result.Add(groupView);
                    }

                    reader.Close();
                }

                conn.Close();
            }

            if (result.Count > 0)
            {
                var hasGroupDefault = false;
                foreach (var group in result)
                {
                    if (group.Default)
                    {
                        hasGroupDefault = true;
                    }
                }

                if (!hasGroupDefault)
                {
                    this.UpdateDefaultGroup(result[0].Id, systemId);
                    result[0].Default = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Проверяет есть ли в группе пользователи
        /// </summary>
        /// <param name="id">
        /// Идентификатор группы
        /// </param>
        /// <returns>
        /// true - пользователи есть, false - пользователей нет
        /// </returns>
        public bool IsEmptyGroupById(int id)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = @"SELECT COUNT(Id)
							            FROM [GroupAccount]
							            WHERE GroupId=@Id";
                cmd.Parameters.AddWithValue("@Id", id);

                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value || Convert.ToInt32(result) == 0)
                {
                    conn.Close();
                    return false;
                }

                conn.Close();
                return true;
            }
        }

        /// <summary>
        /// Удаляет группу по идентификатору, при условии, что в группе нет пользователей
        /// </summary>
        /// <param name="groupId">Идентификатор группы</param>
        public void DeleteGroupById(int groupId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Parameters.AddWithValue("@Id", groupId);
                cmd.CommandText = @"DELETE 
                                    FROM [Group]
                                    WHERE Id=@Id and 0 = (SELECT COUNT(Id)
							                             FROM [GroupAccount]
							                             WHERE GroupId=@Id)";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Обновляет группу по идентификатору
        /// </summary>
        /// <param name="name">Наименование группы</param>
        /// <param name="code">Обозначение группы</param>
        /// <param name="groupId">Идентификатор группы</param>
        public static void UpdateGroup(string name, string code, int groupId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Id", groupId);
                cmd.CommandText = @"UPDATE [dbo].[Group]
                                        SET 
                                        Code=@Code,
                                        Name=@Name
						            WHERE 
							            Id=@Id";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateDefaultGroup(int groupId, int? systemId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Parameters.AddWithValue("@Id", groupId);
                cmd.Parameters.AddWithValue("@SystemID", systemId);
                cmd.CommandText = @"UPDATE [dbo].[Group]
                                       SET [Default] = ( CASE
                                             WHEN (Id = @Id) THEN 1
                                             ELSE 0
                                           END
                                        )
                                     WHERE SystemID=@SystemID";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Получить идентификатор группы по умолчанию для Информационной системы
        /// </summary>
        /// <param name="systemId">Идентификатор ИС</param>
        /// <returns>Ид группы по умолчанию</returns>
        public int GetGroupIdDefaultForSystem(int systemId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT g.[Id]
                                    FROM [System] s 
                                    JOIN [Group] g ON s.[SystemID]=g.[SystemID]
                                    WHERE g.[Default]=1 AND s.[SystemID]=@systemId";
                cmd.Parameters.AddWithValue("@systemId", systemId);

                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    conn.Close();
                    return 0;
                }

                conn.Close();
                return Convert.ToInt32(result);
            }
        }
    }
}
