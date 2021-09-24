using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Fbs.Web.Administration.Organizations;
using Fbs.Web.Administration.SqlConstructor.UserAccounts;

namespace Fbs.Web.Administration.Accounts.Departments
{
    public partial class List : System.Web.UI.Page
    {
        private SqlConstructor_GetDepartmentsUsers m_SqlConstructor_GetDepartmentsUsers;
        protected void Page_Load(object sender, EventArgs e)
        {
            m_SqlConstructor_GetDepartmentsUsers = new SqlConstructor_GetDepartmentsUsers(Request.QueryString);

            //rptEducationTypes.DataSource = Fbs.Core.CatalogElements.CatalogDataAcessor.GetAll(Fbs.Core.CatalogElements.CatalogDataAcessor.Catalogs.OrganizationTypes);
            //rptEducationTypes.DataBind();


            tablePager_top.ItemCount = GetUserCount();
            tablePager_bottom.ItemCount = tablePager_top.ItemCount;

            base.OnInit(e);


            dgUserList.DataSource = GetUsers();
            if (dgUserList.DataSource != null)
                dgUserList.DataBind();
        }

        private DataTable GetUsers()
        {
            DataTable table = null;

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = m_SqlConstructor_GetDepartmentsUsers.GetSQL();
                cmd.Connection = connection;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }
            return table;
        }

        private int GetUserCount()
        {
            int orgCount = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = m_SqlConstructor_GetDepartmentsUsers.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                orgCount = (int)cmd.Parameters["rowCount"].Value;
            }
            return orgCount;
        }
    }
}
