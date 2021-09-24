using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Fbs.Web
{
    using System.Collections.Generic;
    using System.Web.Services;

    [WebService(Namespace = "http://armd.ru/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class Registation_AutoCompleteFields : WebService
    {

        [WebMethod]
        public string[] GetVUZ_List(string prefixText, int count)
        {
            List<string> orgList = new List<string>();
            if (count < 1)
                count = 10;

            try
            {
                string connectionString =ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "dbo.SearchVUZ";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("orgNamePrefix", SqlDbType.VarChar, 256));
                    cmd.Parameters["orgNamePrefix"].Value = prefixText;
                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() && (orgList.Count <= count))
                        {
                            orgList.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch
            {
                // подавление ошибок
            }
            return orgList.ToArray();
        }
    }
}
