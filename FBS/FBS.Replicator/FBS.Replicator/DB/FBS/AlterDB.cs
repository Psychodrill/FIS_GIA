using FBS.Common;
using System;
using System.Data.SqlClient;

namespace FBS.Replicator.DB.FBS
{
    public static class FBSAlterDB
    {
        public static Tables GetCurrentTables(out bool success)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 300;
                cmd.CommandText = Queries.TablesModes_FBS.GetUsingTables;

                object resultObj = null;
                try
                {
                    resultObj = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА определения комплекта таблиц: " + ex.Message + " (" + ex.StackTrace + ")");
                    success = false;
                    return Tables.A;
                }

                success = true;
                if (resultObj == null)
                    return Tables.A;
                if (resultObj.ToString().ToUpper() == "A")
                    return Tables.A;
                if (resultObj.ToString().ToUpper() == "B")
                    return Tables.B;

                Logger.WriteLine(String.Format("ОШИБКА определения комплекта таблиц: Используется неизвестный комплект таблиц ({0})", resultObj));
                success = false;
                return Tables.A;
            }
        }

        public static void SaveCurrentTables(Tables tables)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 300;
                cmd.CommandText = Queries.TablesModes_FBS.DropUsingTables;
                cmd.ExecuteNonQuery();
                if (tables == Tables.A)
                {
                    cmd.CommandText = Queries.TablesModes_FBS.SetUsingTablesA;
                }
                else
                {
                    cmd.CommandText = Queries.TablesModes_FBS.SetUsingTablesB;
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static bool DisableIndexes(Tables tables)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 3600;
                Queries.Indexes_FBS.DropAllIndexesQuery(tables, cmd);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА отключения индексов: " + ex.Message + " (" + ex.StackTrace + ")");
                    return false;
                }
                Queries.Indexes_FBS.DropAllConstraintsQuery(tables, cmd);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА отключения первичных ключей: " + ex.Message + " (" + ex.StackTrace + ")");
                    return false;
                }
                return true;
            }
        }

        public static bool EnableIndexes(Tables tables)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 3600;
                Queries.Indexes_FBS.CreateAllConstraintsQuery(tables, cmd);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА восстановления индексов: " + ex.Message + " (" + ex.StackTrace + ")");
                    return false;
                }
                Queries.Indexes_FBS.CreateAllIndexesQuery(tables, cmd);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА восстановления первичных ключей: " + ex.Message + " (" + ex.StackTrace + ")");
                    return false;
                }
                return true;
            }
        }

        public static bool SetUsingTables(Tables tables)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 300;
                Queries.DBModes.SetSingleUserQuery(cmd);
                Logger.WriteLine("Перевод БД в однопользовательский режим");
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА перевода БД в однопользовательский режим: " + ex.Message + " (" + ex.StackTrace + ")");
                    return false;
                }

                bool multiUserSet = false;
                try
                {
                    Queries.Views_FBS.AlterParticipantsViewQuery(tables, cmd);
                    cmd.ExecuteNonQuery();
                    Queries.Views_FBS.AlterCertificatesViewQuery(tables, cmd);
                    cmd.ExecuteNonQuery();
                    Queries.Views_FBS.AlterCertificateMarksViewQuery(tables, cmd);
                    cmd.ExecuteNonQuery();
                    Queries.Views_FBS.AlterCancelledCertificatesViewQuery(tables, cmd);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА перестройки представлений: " + ex.Message + " (" + ex.StackTrace + ")");
                    return false;
                }
                finally
                {
                    Queries.DBModes.SetMultipleUserQuery(cmd);
                    Logger.WriteLine("Перевод БД в многопользовательский режим");
                    try
                    {
                        cmd.ExecuteNonQuery();
                        multiUserSet = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("ОШИБКА перевода БД в многопользовательский режим: " + ex.Message + " (" + ex.StackTrace + ")");
                        multiUserSet = false;
                    }
                }
                if (!multiUserSet)
                    return false;
                return true;
            }
        }
    }
}
