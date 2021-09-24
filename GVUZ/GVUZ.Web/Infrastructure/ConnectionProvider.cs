using System.Configuration;
using System.Data.SqlClient;

namespace GVUZ.Web.Infrastructure
{
    public static class ConnectionProvider
    {
        private static string _connectionString;

        private static string GetConnectionString()
        {
            return _connectionString ?? (_connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString);
        }

        public static SqlConnection CreateConnection()
        {
            SqlConnection cn = new SqlConnection(GetConnectionString());
            cn.Open();
            return cn;
        }
    }
}