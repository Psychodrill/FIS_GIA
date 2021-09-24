using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Esrp.Core.Reports
{
    public class CNE_other
    {
        public static string GetLastUpdateTime()
        {
            DateTime? Result=null ;
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandTimeout = 180;
                Cmd.CommandText = @"SELECT MAX(UpdateDate) FROM dbo.CommonNationalExamCertificate";

                Result = Convert.ToDateTime(Cmd.ExecuteScalar());

                Conn.Close();
            }
            if (Result != null)
                return Result.ToString();
            return "";
        }
    }
}
