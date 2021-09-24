using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Esrp.Integration.Common
{
    public static class SqlConnectionChecker
    {
        public static bool CheckConnection(string connectionString, out string errorMessage)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
    }
}
