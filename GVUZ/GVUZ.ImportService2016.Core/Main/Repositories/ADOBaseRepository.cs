using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
    public class ADOBaseRepository
    {
        public static readonly int TIMEOUT = 60000;
        public static readonly Object DB_LOCK_OBJECT = new Object();
        /// <summary>
        /// Общий стандартный метод выполнения sql-запросов к БД
        /// </summary>
        /// <param name="query">запрос</param>
        /// <param name="parameters">параметры</param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            var ds = new DataSet();
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted)) 
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandTimeout = TIMEOUT;

                    foreach (var p in parameters)
                        cmd.Parameters.Add(new SqlParameter(p.Key, p.Value));

                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                }
            }

            return ds;
        }

        public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            var ds = new DataSet();
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted)) 
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandTimeout = TIMEOUT;

                    foreach (var p in parameters)
                        cmd.Parameters.Add(new SqlParameter(p.Key, p.Value));

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            var ds = new DataSet();
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandTimeout = TIMEOUT;

                    foreach (var p in parameters)
                        cmd.Parameters.Add(new SqlParameter(p.Key, p.Value));

                    return cmd.ExecuteScalar();
                }
            }
        }




        public static bool ChangeCampaignStatus(int campaignID, int statusID)
        {
            string sql = @"update Campaign Set StatusID = @StatusID Where CampaignID = @CampaignID";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@CampaignID", campaignID);
            parameters.Add("@StatusID", statusID);

            ExecuteNonQuery(sql, parameters);
            return true;
        }
    }
}
