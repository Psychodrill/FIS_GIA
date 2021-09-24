namespace FbsServices
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Xml.Serialization;

    using FbsChecksClient;

    /// <summary>
    /// отчеты
    /// </summary>
    public class ReportingService
    {
        /// <summary>
        /// отчет по загруженным свидетельствам
        /// </summary>
        /// <returns>данные для биндинга</returns>
        public DataSet ReportCertificateLoadShortTVF()
        {
            bool isOpenFbs = (ConfigurationManager.AppSettings["EnableOpenedFbs"] ?? "false").ToLower() == "true";

            if (!isOpenFbs)
            {
                string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM dbo.ReportCertificateLoadShortTVF()";
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }

            var checkClient = new WSCheckClient();
            var dataSetXml = checkClient.ReportCertificateLoadShortTVF();
            using (TextReader tw = new StringReader(dataSetXml))
            {
                XmlSerializer ser = new XmlSerializer(typeof(DataSet));
                DataSet result = (DataSet)ser.Deserialize(tw);
                tw.Close();
                return result;
            }
        }

        /// <summary>
        /// день последнего обновления св-в
        /// </summary>
        /// <returns>
        /// дата
        /// </returns>
        public DateTime CNELastUpdateDate()
        {
            bool isOpenFbs = (ConfigurationManager.AppSettings["EnableOpenedFbs"] ?? "false").ToLower() == "true";

            DateTime? result = null;

            if (!isOpenFbs)
            {
                string connectionString =
                    ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].
                        ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT MAX(UpdateDate) FROM prn.Certificates";
                    cmd.CommandType = CommandType.Text;
                    return Convert.ToDateTime(cmd.ExecuteScalar());
                }
            }

            var checkClient = new WSCheckClient();
            return checkClient.CNELastUpdateDate();
        }
    }
}