using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DeliveriesCore
{
    public class Logger
    {
        const string LogFile = "C:\\TimerLog.txt";
        public static void Write(string message)
        {
            if (!System.IO.File.Exists(LogFile))
            {
                System.IO.File.Create(LogFile).Close();
            }
            System.IO.StreamWriter Wr = System.IO.File.AppendText(LogFile);
            Wr.WriteLine(DateTime.Now.ToString() + "  " + message);
            Wr.Close();
        }

        public static void WriteDeliveryLogEvent(string connectionString, string deliveryId, string EMail, string errorDescription)
        {
            bool Success = String.IsNullOrEmpty(errorDescription);

            using (SqlConnection Conn = new SqlConnection(connectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = @"INSERT INTO DeliveryLog(DeliveryId, ReciverEMail, Success, ErrorDescription)
                                  VALUES(@DeliveryId, @ReciverEMail, @Success, @ErrorDescription)";
                Cmd.Parameters.AddWithValue("DeliveryId", deliveryId);
                Cmd.Parameters.AddWithValue("ReciverEMail", EMail);
                Cmd.Parameters.AddWithValue("Success", Success);
                Cmd.Parameters.AddWithValue("ErrorDescription", errorDescription);
                Cmd.ExecuteNonQuery();
                Conn.Close();
            }
        }
    }
}
