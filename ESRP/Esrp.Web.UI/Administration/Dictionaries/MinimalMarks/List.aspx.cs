using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;

namespace Esrp.Web.Administration.Dictionaries.MinimalMarks
{
    public partial class List : BasePage
    {

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem item in dgMinimalMarks.Items)
            {
                if (sb.Length > 0) 
                    sb.Append(",");
                sb.AppendFormat("{0}={1}"
                              , ((HiddenField) item.FindControl("hfMinimalMarkId")).Value
                              , ((TextBox) item.FindControl("tbMinimalMarkValue")).Text.Replace(",", "."));
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateMinimalMarks") {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.AddWithValue("MinimalMarksString", sb.ToString());
                cmd.Connection = connection;
                connection.Open();
                int affectedRows = cmd.ExecuteNonQuery();
            }

        }
    }
}