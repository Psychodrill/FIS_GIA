using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;

namespace Esrp.Web.Administration.Dictionaries.ExpireDates
{
    public partial class List : BasePage
    {

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            Page.Validate();
            if(!Page.IsValid)
                return;

            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem item in dgExpireDates.Items)
            {
                if (sb.Length > 0)
                    sb.Append(",");
                sb.AppendFormat("{0}={1}"
                              , ((HiddenField)item.FindControl("hfExpireDateYear")).Value
                              , ((TextBox)item.FindControl("tbExpireDate")).Text); 
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateExpireDates") { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("ExpireDatesString", sb.ToString());
                cmd.Connection = connection;
                connection.Open();
                int affectedRows = cmd.ExecuteNonQuery();
            }

        }
    }
}