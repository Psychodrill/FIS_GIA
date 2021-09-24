using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace FbsService.FbsLoader
{
    partial class LoaderContext
    {
        static private ThreadInstanceManager<LoaderContext> mInstanceManager =
                new ThreadInstanceManager<LoaderContext>(CreateInstance);

        static private LoaderContext CreateInstance()
        {
            LoaderContext instance = new LoaderContext();
            instance.ObjectTrackingEnabled = false;
            instance.CommandTimeout = 0;
            return instance;
        }

        static internal LoaderContext Instance()
        {
            return mInstanceManager.Instance();
        }

        static internal void BeginLock()
        {
            mInstanceManager.BeginLock();
        }

        static internal void EndLock()
        {
            mInstanceManager.EndLock();
        }

        #region Обновление задач на загрузку сертификатов ЕГЭ.
        
        private const string CreateTempLoadingTaskStatement =
                "create table #CommonNationalExamCertificateLoadingTask \r\n" +
                "    ( \r\n" +
                "    SourceBatchUrl nvarchar(max) \r\n" +
                "    ) ";

        private const string DropTempLoadingTaskStatement =
                "drop table #CommonNationalExamCertificateLoadingTask ";

        private const string InsertTempLoadingTaskStatement =
                "insert into #CommonNationalExamCertificateLoadingTask \r\n" +
                "values (@sourceBatchUrl)";
            
        private const string UpdateLoadingTaskSpName =
                "dbo.UpdateCommonNationalExamCertificateLoadingTask";

        static private void BeginUpdateLoading(SqlConnection connection)
        {
            using (SqlCommand cmdCreate = connection.CreateCommand())
            {
                cmdCreate.CommandText = CreateTempLoadingTaskStatement;
                cmdCreate.ExecuteNonQuery();
            }
        }

        static private void EndUpdateLoading(SqlConnection connection)
        {
            using (SqlCommand cmdCreate = connection.CreateCommand())
            {
                cmdCreate.CommandText = DropTempLoadingTaskStatement;
                cmdCreate.ExecuteNonQuery();
            }
        }

        static private void AddLoadingTask(SqlConnection connection, string sourceBatchUrl)
        {
            using (SqlCommand cmdInsert = connection.CreateCommand())
            {
                SqlParameter[] parameters = new SqlParameter[] 
                        {
                        new SqlParameter("@sourceBatchUrl", System.Data.SqlDbType.NVarChar, 4000),
                        };

                cmdInsert.CommandText = InsertTempLoadingTaskStatement;
                cmdInsert.Parameters.AddRange(parameters);

                cmdInsert.Parameters["@sourceBatchUrl"].Value = sourceBatchUrl;
                cmdInsert.ExecuteNonQuery();
            }
        }

        static private void UpdateLoading(SqlConnection connection)
        {
            using (SqlCommand cmdCommit = connection.CreateCommand())
            {
                cmdCommit.CommandType = CommandType.StoredProcedure;
                cmdCommit.CommandText = UpdateLoadingTaskSpName;
                cmdCommit.ExecuteNonQuery();
            }
        }

        static public void RefreshLoadingTasks(string[] batchUrls)
        {
            using (SqlConnection connection = new SqlConnection(
                    global::FbsService.Properties.Settings.Default.FbsLoaderConnectionString))
            {
                connection.Open();
                try
                {
                    BeginUpdateLoading(connection);
                    try
                    {
                        foreach (string batchUrl in batchUrls)
                            AddLoadingTask(connection, batchUrl);
                        UpdateLoading(connection);
                    }
                    finally
                    {
                        EndUpdateLoading(connection);
                    }
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        #endregion

        #region Обновление задач на загрузку запрещений сертификатов ЕГЭ.

        private const string CreateTempDenyLoadingTaskStatement =
                "create table #CommonNationalExamCertificateDenyLoadingTask \r\n" +
                "    ( \r\n" +
                "    SourceBatchUrl nvarchar(max) \r\n" +
                "    ) ";

        private const string DropTempDenyLoadingTaskStatement =
                "drop table #CommonNationalExamCertificateDenyLoadingTask ";

        private const string InsertTempDenyLoadingTaskStatement =
                "insert into #CommonNationalExamCertificateDenyLoadingTask \r\n" +
                "values (@sourceBatchUrl)";

        private const string UpdateDenyLoadingTaskSpName =
                "dbo.UpdateCommonNationalExamCertificateDenyLoadingTask";

        static private void BeginUpdateDenyLoading(SqlConnection connection)
        {
            using (SqlCommand cmdCreate = connection.CreateCommand())
            {
                cmdCreate.CommandText = CreateTempDenyLoadingTaskStatement;
                cmdCreate.ExecuteNonQuery();
            }
        }

        static private void EndUpdateDenyLoading(SqlConnection connection)
        {
            using (SqlCommand cmdCreate = connection.CreateCommand())
            {
                cmdCreate.CommandText = DropTempDenyLoadingTaskStatement;
                cmdCreate.ExecuteNonQuery();
            }
        }

        static private void AddDenyLoadingTask(SqlConnection connection, string sourceBatchUrl)
        {
            using (SqlCommand cmdInsert = connection.CreateCommand())
            {
                SqlParameter[] parameters = new SqlParameter[] 
                        {
                        new SqlParameter("@sourceBatchUrl", System.Data.SqlDbType.NVarChar, 4000),
                        };

                cmdInsert.CommandText = InsertTempDenyLoadingTaskStatement;
                cmdInsert.Parameters.AddRange(parameters);

                cmdInsert.Parameters["@sourceBatchUrl"].Value = sourceBatchUrl;
                cmdInsert.ExecuteNonQuery();
            }
        }

        static private void UpdateDenyLoading(SqlConnection connection)
        {
            using (SqlCommand cmdCommit = connection.CreateCommand())
            {
                cmdCommit.CommandType = CommandType.StoredProcedure;
                cmdCommit.CommandText = UpdateDenyLoadingTaskSpName;
                cmdCommit.ExecuteNonQuery();
            }
        }

        static public void RefreshDenyLoadingTasks(string[] batchUrls)
        {
            using (SqlConnection connection = new SqlConnection(
                    global::FbsService.Properties.Settings.Default.FbsLoaderConnectionString))
            {
                connection.Open();
                try
                {
                    BeginUpdateDenyLoading(connection);
                    try
                    {
                        foreach (string batchUrl in batchUrls)
                            AddDenyLoadingTask(connection, batchUrl);
                        UpdateDenyLoading(connection);
                    }
                    finally
                    {
                        EndUpdateDenyLoading(connection);
                    }
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
        }

        #endregion
    }
}
