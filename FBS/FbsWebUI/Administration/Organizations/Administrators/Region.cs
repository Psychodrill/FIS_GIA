using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.Organizations
{
    public class Region
    {
        //
        public int ID;
        public string Name;

        public static DataTable GetRegions()
        {
            DataTable table = null;

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, [Name] FROM dbo.Region WHERE InOrganizationEtalon=1 ORDER BY [Name] ");
                cmd.Connection = connection;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }
            return table;
        }

        public static Region Get(int id)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT [Name] FROM dbo.Region WHERE Id=@RegionId");

            cmd.Parameters.Add(new SqlParameter("RegionId", SqlDbType.Int));
            cmd.Parameters["RegionId"].Value = id;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            Region newRegion = new Region();
            newRegion.ID = id;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd.Connection = connection;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        newRegion.Name = reader["Name"].ToString();
                    }
                }
            }
            return newRegion;
        }
    }

}